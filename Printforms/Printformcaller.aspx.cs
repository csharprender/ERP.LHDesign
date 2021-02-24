using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;
using SVframework2016;
using ERP.LHDesign2020.Class;
namespace IAdvertise
{
    public partial class Printform : System.Web.UI.Page
    {
        private static string _connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        private static string _PF_PDFPath = System.Configuration.ConfigurationManager.AppSettings["PF_PDFPath"];
        private static string PF_URLPDF = System.Configuration.ConfigurationManager.AppSettings["PF_URLPDF"];
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnector Cn = null;
            try
            {

                if (Request["Val"] != null)
                {
                    string PrintformCode = "";
                    string Engine = "";
                    string sqlcmd = "";
                    string Cmd = "";
                    Cn = new SqlConnector(_connectionstring, "");
                    DataTable DtPrintform = new DataTable();
                    DataTable DtPrintformParameter = new DataTable();
                    List<ClsDict> Values = new List<ClsDict>();
                    List<ClsDict> ObjResults = new List<ClsDict>();
                    List<ClsDict> Parameters = new List<ClsDict>();
                    Values = ClsEngine.DeSerialized(ClsEngine.Base64Decode(Request["Val"].ToString()), '=', '&');
                    foreach (ClsDict ObjVal in Values)
                    {
                        if (ObjVal.Name == "PrintformCode")
                        {
                            PrintformCode = ObjVal.Val;
                        }

                        else if (ObjVal.Name == "Engine")
                        {
                            Engine = ObjVal.Val;
                        }

                    }
                    sqlcmd = "Select * from Sys_Conf_Printform Where PrintformId = '" + PrintformCode + "' and isdelete = 0 ";
                    DtPrintform = Cn.Select(sqlcmd);
                    if (DtPrintform.Rows.Count > 0)
                    {
                        sqlcmd = "Select * from Sys_Conf_PrintformParameter Where PrintformId = '" + PrintformCode + "' ";
                        DtPrintformParameter = Cn.Select(sqlcmd);
                        foreach (DataRow dr in DtPrintformParameter.Rows)
                        {
                            foreach (ClsDict ObjVal in Values)
                            {
                                if (dr["UrlParameter"].ToString().ToLower() == ObjVal.Name.ToLower())
                                {
                                    ClsDict ObjParameter = new ClsDict();
                                    ObjParameter.Name = dr["ParameterName"].ToString();
                                    ObjParameter.Val = ObjVal.Val;
                                    Parameters.Add(ObjParameter);
                                }
                            }
                        }
                        //Parameters ready ! and Print form have a only one record
                        if (Engine == "CrystalReport")
                        {
                            CrystalReportPF(DtPrintform, Parameters, Cmd);
                        }
                    }

                }
        }
            catch (Exception ex)
            {
                Response.Write("<font style='color:red;font-size:16px;'>" + ex.Message + "</font>");
            }
            finally
            {

            }
        }
        private void CrystalReportPF(DataTable Dtprintform, List<ClsDict> Parameters, string Cmd)
        {
            SqlConnector Cn = new SqlConnector(_connectionstring, "");
            string path = Dtprintform.Rows[0]["PrintformPath"].ToString().Trim();
            string StoreProc = Dtprintform.Rows[0]["StoreProc"].ToString().Trim();
            string PrintformId = Dtprintform.Rows[0]["PrintformId"].ToString().Trim();
            string StoreProcCommand = "";


            string sqlcmd = "Exec " + StoreProc;
            string password = "";
            System.Collections.ArrayList ArrCmd = new System.Collections.ArrayList();
            DataTable Dt = new DataTable();
            DataTable DtSubReport = new DataTable();
            DataTable DtSubReportItem = new DataTable();
            int i = 0;
            foreach (ClsDict ObjParameter in Parameters)
            {
                if (i > 0)
                {
                    sqlcmd += ",";
                }
                sqlcmd += "'" + ObjParameter.Val + "'";
                i++;
            }
            Dt = Cn.Select(sqlcmd);
            StoreProcCommand = sqlcmd;
            ReportDocument cryRpt = new ReportDocument();
            cryRpt.Load(path);
            ExportOptions CrExportOptions;

            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            Dt.TableName = "Test";
            cryRpt.SetDataSource(Dt);
            //Sub Report Section
            //Validate Sub Report 
            //User Parameter from Main Report 
            string[] SupReportParametrers = { };
            sqlcmd = "Select * from sys_conf_subprintform where isdelete = 0 and PrintformId = '" + PrintformId + "'";
            DtSubReport = Cn.Select(sqlcmd);
            foreach (DataRow s_dr in DtSubReport.Rows)
            {
                i = 0;
                SupReportParametrers = s_dr["Parameters"].ToString().Split(',');
                sqlcmd = "Exec " + s_dr["StoreProc"].ToString() + " ";
                DtSubReportItem = new DataTable();
                foreach (ClsDict ObjParameter in Parameters)
                {
                    foreach (string strsubparam in SupReportParametrers)
                    {
                        if (ObjParameter.Name == strsubparam)
                        {
                            if (i > 0)
                            {
                                sqlcmd += ",";
                            }
                            sqlcmd += "'" + ObjParameter.Val + "'";
                            i++;
                        }
                    }
                }
                DtSubReportItem = Cn.Select(sqlcmd);
                cryRpt.Subreports[s_dr["SubPrintformName"].ToString()].Database.Tables[0].SetDataSource(DtSubReportItem);
            }
            string _fname = Guid.NewGuid().ToString(); ;
            string filename = _fname + ".pdf";
            string filepath = _PF_PDFPath + filename;
            CrDiskFileDestinationOptions.DiskFileName = filepath;
            CrExportOptions = cryRpt.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            cryRpt.Export();


            HttpResponse response = HttpContext.Current.Response;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(filepath);
            System.IO.MemoryStream PDFStream = new MemoryStream(FileBuffer);
            response.Clear();
            response.ClearHeaders();
            response.AddHeader("Content-Disposition", "inline;filename=" + "EForm " + _fname + ".pdf");
            response.AddHeader("Content-Length", PDFStream.Length.ToString());
            response.ContentType = "application/pdf";
            response.Flush();
            response.BinaryWrite(PDFStream.ToArray());
            response.End();

            //WebClient User = new WebClient();
            //Byte[] FileBuffer = User.DownloadData(filepath);
            //if (FileBuffer != null)
            //{
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
            //    Response.BinaryWrite(FileBuffer);
            //}
            string RunningNo;
            RunningNo = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_Trans_Printform", "Id");
            sqlcmd = "INSERT INTO [Sys_Trans_Printform] ([Id],[SessionId],[FileName],[Password],[PrinformId],[StoreProcCommand],[Path],[Createdate],[CreateBy]) ";
            sqlcmd += "VALUES('" + RunningNo + "','" + HttpContext.Current.Session.SessionID + "','" + new FileInfo(filepath).Name + "','" + password + "','" + new FileInfo(path).Name + "','" + StoreProcCommand.Replace("'", "|") + "','" + filepath + "','" + System.DateTime.Now + "','" + ((Clsuser)HttpContext.Current.Session["My"]).username + "')";
            Cn.Execute(sqlcmd, null);
            CrystalReportViewer1.ReportSource = cryRpt;
            Cn.Close();
            Cn.Dispose();
            cryRpt.Close();
            cryRpt.Dispose();
            GC.Collect();
            //Response.Redirect(PF_URLPDF + filename);
        }
    }
}