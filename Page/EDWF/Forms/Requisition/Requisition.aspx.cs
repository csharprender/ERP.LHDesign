using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using SVframework2016;
using System.Data;
using ERP.LHDesign2020.Class;
namespace ERP.LHDesign2020.Page.EDWF.Forms.Requisition
{
    public partial class Requisition : System.Web.UI.Page
    {
        private const string _Eformcode = "EFORM007";
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        public static string UrlAttachment = ConfigurationManager.AppSettings["UrlAttachment"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session.Remove("EFORM007");
                ClsGrid ObjGrid = new ClsGrid();
                HttpContext.Current.Session["ObjGrid"] = ObjGrid;
            }
        }
        [WebMethod]
        public static string Deleteattachment(string json)
        {
            Boolean isfound = false;
            if (HttpContext.Current.Session["EFORM007"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        isfound = true;
                        if (obj.Uploaduserid == ((Clsuser)HttpContext.Current.Session["My"]).userid)
                        {
                            ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Attachments.Remove(obj);
                            return "";
                        }
                        else
                        {
                            return "เจ้าของไฟล์จึงจะมีสิทธิ์ลบเท่านั้น";
                        }
                    }
                }
            }
            if (!isfound)
            {
                return "ไม่พบเอกสารที่ต้องการลบ โปรดติดต่อผู้ดูแลระบบ";
            }
            return "";
        }

        [WebMethod]
        public static string Downloadattachment(string json)
        {
            if (HttpContext.Current.Session["EFORM007"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        return obj.URL;
                    }
                }
            }
            //not found ,contact admin
            return "";
        }
        [WebMethod]
        public static List<ClsAttachment> Getupload(string json)
        {
            ClsEForm007 Obj = new ClsEForm007();
            Obj = (ClsEForm007)HttpContext.Current.Session["EFORM007"];
            return Obj.Attachments;
        }

        
        [WebMethod]
        public static string Getgrandtotal(string json)
        {
            ClsEForm007 Obj = new ClsEForm007();
            double Grandtotal = 0;
            Obj = ((ClsEForm007)HttpContext.Current.Session["EFORM007"]);
            foreach (ClsEForm007Period _obj in Obj.Periods)
            {
                if (_obj.Selected == "x")
                {
                    Grandtotal += double.Parse(_obj.Amount);
                }
            }
            return Grandtotal.ToString("N2");
        }
        [WebMethod]
        public static string Selperiod(string json)
        {
            ClsEForm007 Obj = new ClsEForm007();
            Obj = ((ClsEForm007)HttpContext.Current.Session["EFORM007"]);
            foreach(ClsEForm007Period _obj in Obj.Periods)
            {
                if (_obj.Period == json)
                {
                    _obj.Selected = "x";
                }
            }
            HttpContext.Current.Session["EFORM007"] = Obj;
            return "";
        }
        [WebMethod]
        public static string Unselperiod(string json)
        {
            ClsEForm007 Obj = new ClsEForm007();
            Obj = ((ClsEForm007)HttpContext.Current.Session["EFORM007"]);
            foreach (ClsEForm007Period _obj in Obj.Periods)
            {
                if (_obj.Period == json)
                {
                    _obj.Selected = "";
                }
            }
            HttpContext.Current.Session["EFORM007"] = Obj;
            return "";
        }
        [WebMethod]
        public static List<ClsEForm007Period> Getitem()
        {
            return ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Periods;
        }
        [WebMethod]
        public static ClsEForm007 GetDocumentInfo(string json)
        {

            const string Eformcode = "EFORM007";
            ClsEForm007 Obj = new ClsEForm007();
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            DataTable Dt = new DataTable();
            DataTable DtPeriodEForm001 = new DataTable();
            DataTable DtAttchment = new DataTable();
            string Sqlcmd = "";
            try
            {
                if (HttpContext.Current.Session["EFORM007"] != null)
                {
                    return (ClsEForm007)HttpContext.Current.Session["EFORM007"];
                }
                Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
                Obj.EFormcode = Eformcode;
                Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                Obj.DocumentDate = System.DateTime.Today.ToShortDateString();
                Dt = new DataTable();
                Sqlcmd = "select * from [Sys_Edocument_EFORM007] Where Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                Dt = Cn.Select(Sqlcmd);
                Obj.Transbatchid = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
                Cn.Close();
                if (Dt.Rows.Count > 0)
                {
                    Obj.Documentno = Dt.Rows[0]["DocumentNo"].ToString(); //ถ้าเป็น Node อื่นๆ ที่ไม่ใช่ Node Begin 
                    if (Obj.Documentno.Trim() == "")
                    {
                        Obj.Documentno = ClsEngine.GenerateRunningno(_Eformcode, Connectionstring, "Sys_Edocument_EFORM007", "id");
                    }
                    try
                    {
                        Obj.DocumentDate = DateTime.Parse(Dt.Rows[0]["DocumentDate"].ToString()).ToShortDateString();
                    }
                    catch
                    {

                    }
                    Sqlcmd = "Select  * from  Sys_EDocument_EFORM007  where isdelete = 0 and Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                    Dt = new DataTable();
                    Dt = Cn.Select(Sqlcmd);
                    Obj.id = Dt.Rows[0]["id"].ToString();
                    Obj.Refercontractid = Dt.Rows[0]["Refercontractid"].ToString();
                    Obj.Contid = Dt.Rows[0]["Contid"].ToString();
                    Obj.fullname = Dt.Rows[0]["fullname"].ToString();
                    Obj.address = Dt.Rows[0]["address"].ToString();
                    Obj.Tel = Dt.Rows[0]["Tel"].ToString();
                    Obj.Cardid = Dt.Rows[0]["Cardid"].ToString();
                    if (Dt.Rows[0]["Expirydate"].ToString() != "")
                    {
                        Obj.Expirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Expirydate"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Expirydate = "";
                    }
                    Obj.Bankaccountno = Dt.Rows[0]["Bankaccountno"].ToString();
                    Obj.Bankaccounttype = Dt.Rows[0]["Bankaccounttype"].ToString();
                    Obj.Bankaccountname = Dt.Rows[0]["Bankaccountname"].ToString();
                    Obj.Bankid = Dt.Rows[0]["Bankid"].ToString();
                    Obj.Banknameth = Dt.Rows[0]["Banknameth"].ToString();
                    if (Dt.Rows[0]["Contactdate"].ToString() != "")
                    {
                        Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactdate"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Contactdate = "";
                    }
                    if (Dt.Rows[0]["Contactstart"].ToString() != "")
                    {
                        Obj.Contactstart = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactstart"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Contactstart = "";
                    }
                    if (Dt.Rows[0]["Contactend"].ToString() != "")
                    {
                        Obj.Contactend = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactend"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Contactend = "";
                    }
                    Obj.Effectdate = Dt.Rows[0]["Effectdate"].ToString();
                  
                   
                    
                    Obj.Sitename = Dt.Rows[0]["Sitename"].ToString();
                    Obj.Sitefulladdress = Dt.Rows[0]["Sitefulladdress"].ToString();
                    Obj.Jobdescription = Dt.Rows[0]["Jobdescription"].ToString();
                    Obj.Totalamount = Dt.Rows[0]["Totalamount"].ToString();
                    if (Dt.Rows[0]["Finisheddate"].ToString() != "")
                    {
                        Obj.Finisheddate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Finisheddate"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Finisheddate = "";
                    }
                    Obj.Fee = Dt.Rows[0]["Fee"].ToString();
                    if (Dt.Rows[0]["Invoicedate"].ToString() != "")
                    {
                        Obj.Invoicedate = Dt.Rows[0]["Invoicedate"].ToString();
                    }
                    else
                    {
                        Obj.Invoicedate = "";
                    }
                    if (Dt.Rows[0]["Paymentdate"].ToString() != "")
                    {
                        Obj.Paymentdate = Dt.Rows[0]["Paymentdate"].ToString();
                    }
                    else
                    {
                        Obj.Paymentdate = "";
                    }
                    Obj.Grandtotal = Dt.Rows[0]["Grandtotal"].ToString();
                   
                    List<ClsEForm007Period> Periods = new List<ClsEForm007Period>();
                    Dt = new DataTable();
                   
                    Sqlcmd = "Select * from Sys_EDocument_EForm007detail where Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and isdelete = 0 order by id ";
                    Dt = Cn.Select(Sqlcmd);
                    Sqlcmd = "Select * from Sys_EDocument_Eform001 m left join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Refercontractid + "'";
                    DtPeriodEForm001 = Cn.Select(Sqlcmd);
                    ClsEForm007Period Objperiod;
                    foreach (DataRow s_dr in DtPeriodEForm001.Rows)
                    {
                        Objperiod = new ClsEForm007Period();
                        Objperiod.Period = s_dr["Period"].ToString();
                        Objperiod.Periodname = s_dr["Periodname"].ToString();
                        Objperiod.Amount = double.Parse(s_dr["Amount"].ToString()).ToString("N2");
                        if (Dt.Select("Period='" + s_dr["Period"].ToString() + "'").Length > 0)
                        {
                            Objperiod.Selected = "x";
                        }
                        else
                        {
                            Objperiod.Selected = "";
                        }
                        Periods.Add(Objperiod);
                    }
                    //Detail
                    Obj.Periods = Periods;
                }
                else
                {
                    ////Transaction
                    Obj.Refercontractid = "";
                    Obj.Periods = new List<ClsEForm007Period>();
                }
                Dt = new DataTable();
                Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
                Obj.EFormcode = Eformcode;
                Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                Sqlcmd = "Select * from sys_master_form where isdelete = 0 and formcode = '" + Eformcode + "'";
                try
                {
                    Obj.Formheader = Cn.Select(Sqlcmd).Rows[0]["Formnameth"].ToString();
                }
                catch (Exception ex)
                {
                    Obj.Formheader = ex.Message;
                }

                Sqlcmd = "Select a.path, ta.Attachmentid,Label,Uploaddate,Uploadbynameth,a.CreateBy as Uploaduserid,A.Filename,Extension,Sizebyte from [Sys_EDocument_Transflowattachment] ta inner join Sys_trans_Attachment a on ta.Attachmentid = a.id where ta.isdelete = 0 and a.isdelete = 0 and ta.Transbatchid ='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' order by a.id asc";
                List<ClsAttachment> Attachments = new List<ClsAttachment>();
                ClsAttachment Objattchment;
                string UrlPath = ClsEngine.Getconfigurationvalue(ref Cn, "AttachmentURL", "Attachment");
                DtAttchment = Cn.Select(Sqlcmd);
                foreach (DataRow dr in DtAttchment.Rows)
                {
                    Objattchment = new ClsAttachment();
                    Objattchment.Attachmentid = dr["Attachmentid"].ToString();
                    Objattchment.Extension = dr["Extension"].ToString();
                    Objattchment.Label = dr["Label"].ToString();
                    Objattchment.Sizebyte = dr["Sizebyte"].ToString();
                    Objattchment.Uploadbynameth = dr["Uploadbynameth"].ToString();
                    Objattchment.Uploaddate = dr["Uploaddate"].ToString();
                    Objattchment.Uploaduserid = dr["Uploaduserid"].ToString();
                    Objattchment.URL = UrlPath + System.IO.Path.GetFileName(dr["Path"].ToString());
                    Attachments.Add(Objattchment);
                }
                Obj.Attachments = Attachments;
                Obj.Err = "";

                HttpContext.Current.Session["EFORM007"] = Obj;

                return Obj;
            }
            catch (Exception ex)
            {
                Obj.Err = ex.Message;
                return Obj;
            }
            finally
            {
                Cn.Close();
            }
        }

        [WebMethod]
        public static string ValidateforAction(string json)
        {
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string TransBatchId = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
            List<ClsDict> jsons = ClsEngine.DeSerialized(json, ':', ',');
            string res = "";
            string nodename = ClsEngine.FindValue(jsons, "nodename");
            DataTable Dt = new DataTable();
            string sqlcmd = "";
            string MultipleRule = "";
            sqlcmd = "Select * from TransNodeMultipleDetail Where Isaction = 1 and TransbatchId ='" + TransBatchId + "' and NodeMultipleName = '" + nodename + "'";
            Dt = Cn.Select(sqlcmd);
            if (Dt.Rows.Count > 0)
            {
                MultipleRule = Dt.Rows[0]["MultipleRule"].ToString();
            }
            else
            {
                MultipleRule = "";
            }
            Cn.Close();
            foreach (DataRow dr in Dt.Rows)
            {

                if (MultipleRule.ToLower() == "Differ2E".ToLower()) //ถ้าเหมือนกันหมดให้เอาตัวนั้น // ถ้าไม่เหมือนให้ตกตัว E 
                {
                    if (res == "")
                    {
                        res = dr["DirectionValue"].ToString();
                    }
                    else
                    {
                        if (res != dr["DirectionValue"].ToString())
                        {
                            return "E";
                        }
                    }
                }
            }
            return res;
        }
        [WebMethod]
        public static string SaveMultiple(string json)
        {
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string TransBatchId = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
            List<ClsDict> jsons = ClsEngine.DeSerialized(json, ':', ',');
            string DirectionValue = ClsEngine.FindValue(jsons, "DirectionValue");
            string DirectionNameTH = ClsEngine.FindValue(jsons, "DirectionNameTH");
            string UserId = ClsEngine.FindValue(jsons, "UserId");
            string nodename = ClsEngine.FindValue(jsons, "nodename");
            string Remark = ClsEngine.FindValue(jsons, "Remark");
            Clsuser OBjMy = (Clsuser)HttpContext.Current.Session["My"];
            string sqlcmd = "Update TransNodeMultipleDetail  set ";
            sqlcmd += " Isaction = 1 ";
            sqlcmd += " ,ActionDate='" + System.DateTime.Now + "'";
            sqlcmd += " ,ActionBy='" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
            sqlcmd += " ,DirectionValue='" + DirectionValue + "'";
            sqlcmd += " ,DirectionNameTH = '" + DirectionNameTH + "'";
            sqlcmd += " ,Remark ='" + Remark + "'";
            sqlcmd += " ,[ActionByTitleCode] = '" + OBjMy.Titlecode + "'";
            sqlcmd += " ,[ActionByTitleNameTH]  = '" + OBjMy.Titlenameth + "'";
            sqlcmd += " ,[ActionByfirstnameth] = '" + OBjMy.firstnameth + "'";
            sqlcmd += " ,[ActionBylastnameth]  = '" + OBjMy.lastnameth + "'";
            sqlcmd += " ,[ActionByPositionId]  = '" + OBjMy.positionid + "'";
            sqlcmd += " ,[ActionByPositionNameTH]  = '" + OBjMy.positionnameth + "'";
            sqlcmd += " ,[ActionByOrganizeId] = '" + OBjMy.organizeId + "'";
            sqlcmd += " ,[ActionByOrganizeNameTH] = '" + OBjMy.organizenameth + "'";
            sqlcmd += " ,[SignatureURL]  = '" + OBjMy.sigurl + "'";
            sqlcmd += " Where TransbatchId ='" + TransBatchId + "' and  UserId ='" + UserId + "' and NodeMultipleName = '" + nodename + "'";
            Cn.Execute(sqlcmd, null);
            Cn.Close();
            return "";
        }
        [WebMethod]
        public static List<ClsNodeMulipleDetailInfo> GetNodeMultipleDetail(string json)
        {
            List<ClsNodeMulipleDetailInfo> Objs = new List<ClsNodeMulipleDetailInfo>();
            List<ClsDict> jsons = ClsEngine.DeSerialized(json, ':', ',');
            string NodeMultipleName = ClsEngine.FindValue(jsons, "NodeMultipleName");
            ClsNodeMulipleDetailInfo Obj;
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string TransBatchId = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
            List<ClsDict> Directions;
            ClsDictExtend ObjDirection;
            DataRow[] Drs = ((ClsInfo)HttpContext.Current.Session["Info"]).DataTableMasterFlowDetail.Select("NodeNamefrom ='" + NodeMultipleName + "'");
            DataTable Dt = new DataTable();
            //Should be one;
            string TransflowdetailId = "";
            if (Cn.Select("select * from Sys_Trans_Flowdetail Where TransBatchId = '" + TransBatchId + "' and NodeNameOut = '" + NodeMultipleName + "'").Rows.Count > 0)
            {
                TransflowdetailId = Cn.Select("select * from Sys_Trans_Flowdetail Where TransBatchId = '" + TransBatchId + "' and NodeNameOut = '" + NodeMultipleName + "'").Rows[0]["Id"].ToString();
                string sqlcmd = "Select * from [TransNodeMultipleDetail] Where isdelete = 0 and NodeMultipleName = '" + NodeMultipleName + "' and TransbatchId = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and TransflowdetailId ='" + TransflowdetailId + "'";
                Dt = Cn.Select(sqlcmd);
                foreach (DataRow dr in Dt.Rows)
                {
                    Obj = new ClsNodeMulipleDetailInfo();
                    Obj.NodeMultipleDetailId = dr["Id"].ToString();
                    Obj.MultipleRule = dr["MultipleRule"].ToString();
                    Obj.UserId = dr["UserId"].ToString();
                    Obj.UserName = dr["UserId"].ToString();
                    Obj.Remark = dr["Remark"].ToString();
                    Obj.FullName = (dr["ActionByTitleNameTH"] + " " + dr["ActionByfirstnameth"] + " " + dr["ActionBylastnameth"]).Trim();
                    Directions = new List<ClsDict>();
                    foreach (DataRow m_dr in Drs)
                    {
                        ObjDirection = new ClsDictExtend();
                        ObjDirection.Name = m_dr["DirectionNameTH"].ToString().Trim();
                        ObjDirection.Val = m_dr["DirectionValue"].ToString().Trim();
                        if (m_dr["DirectionValue"].ToString().Trim() == dr["DirectionValue"].ToString().Trim())
                        {
                            ObjDirection.Extend1 = "selected";
                        }
                        Directions.Add(ObjDirection);
                    }
                    Obj.Directions = Directions;
                    Objs.Add(Obj);
                }
                Cn.Close();
            }
            return Objs;
        }
        [WebMethod]
        public static string CallBackUpload(string Label, string Running)
        {

            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string UrlPath = ClsEngine.Getconfigurationvalue(ref Cn, "AttachmentURL", "Attachment");
            List<ClsAttachment> Attachements = new List<ClsAttachment>();
            Attachements = ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Attachments;
            string sqlcmd = "Select * from Sys_Trans_Attachment where id = '" + Running + "'";
            DataTable Dtattachment = new DataTable();
            Dtattachment = Cn.Select(sqlcmd);
            ClsAttachment Objattach = new ClsAttachment();
            Objattach.Attachmentid = Dtattachment.Rows[0]["id"].ToString();
            Objattach.Extension = Dtattachment.Rows[0]["Extension"].ToString();
            Objattach.Label = Label;
            Objattach.Sizebyte = Dtattachment.Rows[0]["Sizebyte"].ToString();
            Objattach.Uploadbynameth = ((Clsuser)HttpContext.Current.Session["My"]).firstnameth + " " + ((Clsuser)HttpContext.Current.Session["My"]).lastnameth;
            Objattach.Uploaddate = System.DateTime.Now.ToShortDateString();
            Objattach.Uploaduserid = ((Clsuser)HttpContext.Current.Session["My"]).userid;
            Objattach.URL = UrlPath + System.IO.Path.GetFileName(Dtattachment.Rows[0]["Path"].ToString());
            Attachements.Add(Objattach);
            ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Attachments = Attachements;
            Cn.Close();
            return "";

        }
        [WebMethod]
        public static string Print(string json)
        {
            const string Eformcode = "EFORM007";
            string x = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
            string StrMsg = "PrintformCode=" + Eformcode + "&Engine=CrystalReport&ID=" + x;
            StrMsg = ClsEngine.Base64Encode(StrMsg);
            return "/../../Printforms/PrintformCaller.aspx?Val=" + StrMsg;
        }



        [WebMethod]
        public static ClsNodeInfo GetInfo(string json)
        {
            try
            {
                ClsNodeInfo ObjNodeInfo = new ClsNodeInfo();
                ObjNodeInfo.Subject = ((ClsInfo)HttpContext.Current.Session["Info"]).Subject;
                ObjNodeInfo.CreateBy = ((ClsInfo)HttpContext.Current.Session["Info"]).CreateBy;
                ObjNodeInfo.TransBatchId = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
                ObjNodeInfo.FormId = ((ClsInfo)HttpContext.Current.Session["Info"]).FormId;
                ObjNodeInfo.FormNameTH = ((ClsInfo)HttpContext.Current.Session["Info"]).FormNameTH;
                ObjNodeInfo.NodeNamefrom = ((ClsInfo)HttpContext.Current.Session["Info"]).NodeNamefrom;
                try
                {
                    ObjNodeInfo.NodeNameTo = ((ClsInfo)HttpContext.Current.Session["Info"]).NodeNameTo.ToString();
                }
                catch
                {
                    ObjNodeInfo.NodeNameTo = ""; //Node End
                }
                try
                {
                    ObjNodeInfo.NodeTypeNameToTH = ((ClsInfo)HttpContext.Current.Session["Info"]).NodeTypeNameToTH.ToString();
                }
                catch
                {
                    ObjNodeInfo.NodeTypeNameToTH = "";
                }
                ObjNodeInfo.Iscancel = ((ClsInfo)HttpContext.Current.Session["Info"]).Iscancel;
                ObjNodeInfo.Iscomplete = ((ClsInfo)HttpContext.Current.Session["Info"]).Iscomplete;
                return ObjNodeInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [WebMethod]
        public static List<ClsAttachment> GetAttachment(string json)
        {
            List<ClsDict> jsons = new List<ClsDict>();
            DataTable Dt = new DataTable();
            jsons = ClsEngine.DeSerialized(json, ':', ',');
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string NodeName = ClsEngine.FindValue(jsons, "NodeName"); // Node ที่ต้องการให้แสดง FIle
            List<ClsAttachment> Attachments = new List<ClsAttachment>();
            return Attachments;
        }
        [WebMethod]
        public static List<ClsFooter> GetFooter(string json)
        {
            List<ClsFooter> Footers = new List<ClsFooter>();
            ClsFooter ObjFooter;
            ClsInfo ObjInfo = new ClsInfo();
            DataTable Dt = new DataTable();
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            List<ClsDict> Jsons = new List<ClsDict>();
            Clsuser OBjMy = new Clsuser();
            //try
            //{
            Jsons = ClsEngine.DeSerialized(json, ':', ',');
            ObjInfo = (ClsInfo)HttpContext.Current.Session["Info"];
            string sqlcmd = "Select * from Sys_Trans_Flowdetail where Isdelete=0 and TransBatchId='" + ObjInfo.TransBatchId + "' Order by Id";
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            OBjMy = (Clsuser)HttpContext.Current.Session["My"];
            foreach (DataRow dr in Dt.Rows)
            {
                if (dr["NodeTypeNameInTH"].ToString().ToLower() == "nodebegin" || dr["NodeTypeNameInTH"].ToString().ToLower() == "nodesingle")
                {
                    ObjFooter = new ClsFooter();
                    ObjFooter.Id = dr["Id"].ToString();
                    ObjFooter.SignatureURL = dr["SignatureURL"].ToString();
                    ObjFooter.Remark = dr["Remark"].ToString();
                    ObjFooter.ActionResultValue = dr["ActionResultValue"].ToString();
                    ObjFooter.ActionResultNameTH = dr["ActionResultNameTH"].ToString();
                    ObjFooter.ActionById = dr["ActionById"].ToString();
                    ObjFooter.ActionByTitleCode = dr["ActionByTitleCode"].ToString();
                    ObjFooter.ActionByTitleNameTH = dr["ActionByTitleNameTH"].ToString();
                    ObjFooter.ActionByFirstNameTH = dr["ActionByfirstnameth"].ToString();
                    ObjFooter.ActionByLastNameTH = dr["ActionBylastnameth"].ToString();
                    ObjFooter.ActionDate = DateTime.Parse(dr["ActionDate"].ToString());
                    ObjFooter.ActionStringDate = DateTime.Parse(dr["ActionDate"].ToString()).ToShortDateString() + " : " + DateTime.Parse(dr["ActionDate"].ToString()).ToShortTimeString();
                    ObjFooter.ActionByPositionId = dr["ActionByPositionId"].ToString();
                    ObjFooter.ActionByPositionNameTH = dr["ActionByPositionNameTH"].ToString();
                    ObjFooter.ActionByOrganizeId = dr["ActionByOrganizeId"].ToString();
                    ObjFooter.ActionByOrganizeNameTH = dr["ActionByOrganizeNameTH"].ToString();
                    Footers.Add(ObjFooter);
                }
                else if (dr["NodeTypeNameInTH"].ToString().ToLower() == "nodemultiple")
                {
                    //Get multiple team
                    DataTable DtMultiple = new DataTable();

                    sqlcmd = "select * from [TransNodeMultipleDetail] where TransFlowDetailOutId = '" + dr["Id"].ToString().ToLower() + "' and TransbatchId = '" + dr["TransbatchId"].ToString().ToLower() + "' and NodeMultipleName = '" + dr["NodenameIn"].ToString() + "' and Isdelete = 0";
                    DtMultiple = Cn.Select(sqlcmd);
                    foreach (DataRow m_dr in DtMultiple.Rows)
                    {
                        ObjFooter = new ClsFooter();
                        ObjFooter.Id = m_dr["Id"].ToString();
                        ObjFooter.NodeMultipleId = dr["Id"].ToString();
                        ObjFooter.NodeTypeNameTH = dr["NodenameIn"].ToString();
                        ObjFooter.SignatureURL = m_dr["SignatureURL"].ToString();
                        ObjFooter.Remark = m_dr["RemarkFooter"].ToString();
                        ObjFooter.ActionResultValue = m_dr["DirectionValue"].ToString();
                        ObjFooter.ActionResultNameTH = m_dr["DirectionNameTH"].ToString();
                        ObjFooter.ActionById = m_dr["ActionById"].ToString();
                        ObjFooter.ActionByTitleCode = m_dr["ActionByTitleCode"].ToString();
                        ObjFooter.ActionByTitleNameTH = m_dr["ActionByTitleNameTH"].ToString();
                        ObjFooter.ActionByFirstNameTH = OBjMy.firstnameth;
                        ObjFooter.ActionByLastNameTH = OBjMy.lastnameth;
                        ObjFooter.ActionDate = DateTime.Parse(dr["ActionDate"].ToString());
                        ObjFooter.ActionStringDate = DateTime.Parse(dr["ActionDate"].ToString()).ToShortDateString() + " : " + DateTime.Parse(dr["ActionDate"].ToString()).ToShortTimeString();
                        ObjFooter.ActionByPositionId = m_dr["ActionByPositionId"].ToString();
                        ObjFooter.ActionByPositionNameTH = m_dr["ActionByPositionNameTH"].ToString();
                        ObjFooter.ActionByOrganizeId = m_dr["ActionByOrganizeId"].ToString();
                        ObjFooter.ActionByOrganizeNameTH = m_dr["ActionByOrganizeNameTH"].ToString();
                        Footers.Add(ObjFooter);
                    }

                }
            }
            if (ClsEngine.FindValue(Jsons, "readonly").ToString().Trim() == "false")  // Node ที่เพิ่มเพื่อที่จะให้ Sign
            {
                if (ObjInfo.NodeTypeNameTH.ToLower() == "nodebegin" || ObjInfo.NodeTypeNameTH.ToLower() == "nodesingle")
                {
                    ObjFooter = new ClsFooter();
                    ObjFooter.Id = (Dt.Rows.Count + 1).ToString();
                    ObjFooter.SignatureURL = OBjMy.sigurl;
                    ObjFooter.Remark = ""; //Ready to remark
                    ObjFooter.ActionResultValue = "";
                    ObjFooter.ActionResultNameTH = "<รอการอนุมัติ>";
                    ObjFooter.ActionById = OBjMy.userid;
                    ObjFooter.ActionByTitleCode = OBjMy.Titlecode;
                    ObjFooter.ActionByTitleNameTH = OBjMy.Titlenameth;
                    ObjFooter.ActionByFirstNameTH = OBjMy.firstnameth;
                    ObjFooter.ActionByLastNameTH = OBjMy.lastnameth;
                    ObjFooter.ActionDate = DateTime.Now;
                    ObjFooter.ActionStringDate = "<รอการอนุมัติ>";

                    ObjFooter.ActionByPositionId = OBjMy.positionid;
                    ObjFooter.ActionByPositionNameTH = OBjMy.positionnameth;
                    ObjFooter.ActionByOrganizeId = OBjMy.organizeId;
                    ObjFooter.ActionByOrganizeNameTH = OBjMy.organizenameth;
                    Footers.Add(ObjFooter);
                }
                else if (ObjInfo.NodeTypeNameTH.ToLower() == "nodemultiple")
                {
                    string TransflowDetail = Dt.Select("TransbatchId = '" + ObjInfo.TransBatchId + "' and NodenameOut ='" + ObjInfo.NodeNamefrom + "' and isdelete = 0 and isnull(ispass,0) = 0 ")[0]["Id"].ToString();
                    //Get multiple team
                    DataTable DtMultiple = new DataTable();
                    sqlcmd = " Select md.*,up.firstnameth,up.lastnameth,up.Sigurl,p.PositionnameTH from[TransNodeMultipleDetail] md ";
                    sqlcmd += " inner join Sys_Master_Userprofile up on md.UserId = up.id ";
                    sqlcmd += " inner join Sys_Master_UserinPosition uip on up.id = uip.userid ";
                    sqlcmd += " inner join Sys_Master_Position p on uip.positionid = p.id and Priority = '1' ";
                    sqlcmd += " where TransFlowDetailId = '" + TransflowDetail + "' and TransbatchId = '" + ObjInfo.TransBatchId + "' and NodeMultipleName = '" + ObjInfo.NodeNamefrom + "' and up.Isdelete = 0 and uip.isdelete = 0 and md.isdelete = 0";
                    //sqlcmd = "select * from [TransNodeMultipleDetail] where TransFlowDetailId = '" + TransflowDetail + "' and TransbatchId = '" + ObjInfo.TransBatchId + "' and NodeMultipleName = '" + ObjInfo.NodeNamefrom + "' and Isdelete = 0";
                    DtMultiple = Cn.Select(sqlcmd);
                    foreach (DataRow m_dr in DtMultiple.Rows)
                    {
                        ObjFooter = new ClsFooter();

                        if (m_dr["userid"].ToString() == ((Clsuser)HttpContext.Current.Session["My"]).userid)
                        {
                            ObjFooter.Ismyremark = "x";
                        }
                        else
                        {
                            ObjFooter.Ismyremark = "";
                        }

                        ObjFooter.Id = (Footers.Count + 1).ToString();
                        ObjFooter.NodeMultipleId = m_dr["Id"].ToString();
                        ObjFooter.NodeTypeNameTH = ObjInfo.NodeTypeNameTH;
                        ObjFooter.SignatureURL = m_dr["Sigurl"].ToString();
                        ObjFooter.Remark = m_dr["RemarkFooter"].ToString();
                        ObjFooter.ActionResultValue = "";
                        ObjFooter.ActionResultNameTH = "<รอการอนุมัติ>";
                        ObjFooter.ActionById = m_dr["ActionById"].ToString();
                        ObjFooter.ActionByTitleCode = m_dr["ActionByTitleCode"].ToString();
                        ObjFooter.ActionByTitleNameTH = m_dr["ActionByTitleNameTH"].ToString();
                        ObjFooter.ActionByFirstNameTH = m_dr["firstnameth"].ToString();
                        ObjFooter.ActionByLastNameTH = m_dr["lastnameth"].ToString();
                        try
                        {
                            ObjFooter.ActionDate = DateTime.Parse(m_dr["ActionDate"].ToString());
                        }
                        catch
                        {
                            ObjFooter.ActionDate = System.DateTime.Today;
                        }
                        ObjFooter.ActionStringDate = "<รอการอนุมัติ>";
                        ObjFooter.ActionByPositionId = m_dr["ActionByPositionId"].ToString();
                        ObjFooter.ActionByPositionNameTH = m_dr["PositionnameTH"].ToString();
                        ObjFooter.ActionByOrganizeId = m_dr["ActionByOrganizeId"].ToString();
                        ObjFooter.ActionByOrganizeNameTH = m_dr["ActionByOrganizeNameTH"].ToString();
                        Footers.Add(ObjFooter);
                    }
                }
            }
            return Footers;
     
        }
        [WebMethod]
        public static List<ClsDictExtend> GetRoute(string json)
        {
            List<ClsDictExtend> Dicts = new List<ClsDictExtend>();
            ClsDictExtend ObjDict = new ClsDictExtend();
            ClsInfo ObjInfo = new ClsInfo();
            DataRow[] Drs;
            ObjInfo = (ClsInfo)HttpContext.Current.Session["Info"];
            try
            {
                Drs = ObjInfo.DataTableMasterFlowDetail.Select("NodeNamefrom ='" + ObjInfo.NodeNamefrom + "'");

                ObjDict = new ClsDictExtend();
                ObjDict.Name = "Select Route";
                ObjDict.Val = "";
                Dicts.Add(ObjDict);
                foreach (DataRow dr in Drs)
                {
                    ObjDict = new ClsDictExtend();
                    ObjDict.Name = dr["DirectionNameTH"].ToString();
                    ObjDict.Val = dr["DirectionValue"].ToString();
                    Dicts.Add(ObjDict);
                }
                return Dicts;
            }
            catch
            {
                return null;
            }
        }
        [WebMethod]
        public static string Save(string json)
        {
            List<ClsDict> Dicts = ClsEngine.DeSerialized(json, ':', '|');
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            ClsNodeInfo ObjNodeInfo = new ClsNodeInfo();
            ClsInfo ObjCurrentState = new ClsInfo();
            DataTable DtUserInFlow = new DataTable();
            string TransBatchId = ""; //Transaction Id
            string TransFlowDetailId = "";  //Unique ของ FlowDetail
            string TransformId = ""; //Transaction Document ที่คุมเอกสาร
            string remark = "";
            string FlowDetailId = "";
            string sqlcmd = "";
            string UserIdOut = "";
            string NodeTypeIdOut = "";
            string NodeTypeNameTHOut = "";
            string NodeNameOut = "";
            string MultipleRule = "";
            double Grandtotal = 0;
            DataTable Dtdetail = new DataTable();
            string Invoicedate = "";
            string Paymentdate = "";
            string Refercontractid = ClsEngine.FindValue(Dicts, "Txtcontract");
            DataRow[] Drs;
            Clsuser ObjMy;
            DataTable Dt = new DataTable();
            ClsEForm007 ObjEform = new ClsEForm007();
            string DocumentNo = ClsEngine.GenerateRunningno("EFORM007", Connectionstring, "Sys_Edocument_EFORM007", "id");
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            ObjCurrentState = ((ClsInfo)HttpContext.Current.Session["Info"]);
            ObjMy = ((Clsuser)HttpContext.Current.Session["My"]);
            Dt = new DataTable();
            Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
            DataTable Dtcont = new DataTable();
            //validate
            Boolean Isfound = false;
            if (Refercontractid == "")
            {
                return "โปรดเลือกใบสัญญาจ้างงานก่อน";
            }
            foreach (ClsEForm007Period period in ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Periods)
            {
                if (period.Selected == "x")
                {
                    Isfound = true;
                }
            }
            if (!Isfound)
            {
                return "โปรดระบุงวดงานที่ต้องการเบิกอย่างน้อย 1 งวดงาน";
            }
            //validate


            foreach (ClsEForm007Period Obj in ((ClsEForm007)HttpContext.Current.Session["EFORM007"]).Periods)
            {
                if (Obj.Selected == "x")
                {
                    Grandtotal += double.Parse(Obj.Amount);
                }
            }

            try
            {
                Invoicedate = ClsEngine.FindValue(Dicts, "Txtinvoicedate");
            }
            catch
            {
                Invoicedate = "";
            }
            try
            {
                Paymentdate = ClsEngine.FindValue(Dicts, "Txtpaymentdate");
            }
            catch
            {
                Paymentdate = "";
            }

            if (Invoicedate != "" && Paymentdate != "")
            {
                if (ClsEngine.ConvertDateforSavingDatabase(Invoicedate) > ClsEngine.ConvertDateforSavingDatabase(Paymentdate))
                {
                    return "วันที่แจ้งหนี้ห้ามน้อยกว่าวันที่ครบกำหนดชำระเงิน";
                }
            }



            //======================================  Tranaction  =========================================
            ObjEform = (ClsEForm007)HttpContext.Current.Session["EFORM007"];
            ObjEform.Documentno = DocumentNo;
            ObjEform.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
            ObjEform.Companyname = Dt.Rows[0]["Companyname"].ToString();
            ObjEform.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
            ObjEform.Companytel = Dt.Rows[0]["Companytel"].ToString();

            

            if (ObjCurrentState.NodeTypeId == "1") //Current Node
            {
                //เริ่มต้น
                TransBatchId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "TransBatchId");
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id");
                TransformId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Edocument_EFORM007", "Id");
               
                sqlcmd = "Select * from [Sys_Edocument_Eform001] m inner join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId and m.isdelete = 0 and d.isdelete = 0  where m.id = '" + Refercontractid + "'";
                Dtcont = Cn.Select(sqlcmd);
                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_EDocument_EForm007]";
                sqlcmd += " ([id]";
                sqlcmd += " ,[Transbatchid]";
                sqlcmd += " ,[TransFlowDetailId]";
                sqlcmd += " ,[Companylogourl]";
                sqlcmd += " ,[Companyname]";
                sqlcmd += " ,[Companyaddress]";
                sqlcmd += " ,[Companytel]";
                sqlcmd += " ,[Version]";
                sqlcmd += " ,[Documentdate]";
                sqlcmd += " ,[Documentno]";
                sqlcmd += " ,[Refercontractid]";
                sqlcmd += " ,[ContId]";
                sqlcmd += " ,[fullname]";
                sqlcmd += " ,[address]";
                sqlcmd += " ,[Tel]";
                sqlcmd += " ,[Cardid]";
                sqlcmd += " ,[Expirydate]";
                sqlcmd += " ,[Bankid]";
                sqlcmd += " ,[Banknameth]";
                sqlcmd += " ,[Bankaccountno]";
                sqlcmd += " ,[Bankaccountname]";
                sqlcmd += " ,[Bankaccounttype]";
                sqlcmd += " ,[Contactdate]";
                sqlcmd += " ,[Contactstart]";
                sqlcmd += " ,[Contactend]";
                sqlcmd += " ,[Effectdate]";
                sqlcmd += " ,[Sitename]";
                sqlcmd += " ,[Sitefulladdress]";
                sqlcmd += " ,[Jobdescription]";
                sqlcmd += " ,[Totalamount]";
                sqlcmd += " ,[Finisheddate]";
                sqlcmd += " ,[Fee]";
                if (Invoicedate != "")
                {
                    sqlcmd += " ,[Invoicedate]";
                }
                if (Paymentdate != "")
                {
                    sqlcmd += " ,[Paymentdate]";
                }
                sqlcmd += " ,[Grandtotal]";
                sqlcmd += " ,[IsDelete] ";
                sqlcmd += " ,[CreateDate] ";
                sqlcmd += " ,[CreateBy]) ";
                sqlcmd += " VALUES (";
                sqlcmd += "'" + TransformId + "'";
                sqlcmd += ",'" + TransBatchId + "'";
                sqlcmd += ",'" + TransFlowDetailId + "'";
                sqlcmd += ",'" + ObjEform.Companylogourl + "'";
                sqlcmd += ",'" + ObjEform.Companyname + "'";
                sqlcmd += ",'" + ObjEform.Companyaddress + "'";
                sqlcmd += ",'" + ObjEform.Companytel + "'";
                sqlcmd += ",'" + ObjEform.Version + "'";
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.DocumentDate) + "'";
                sqlcmd += ",'" + ObjEform.Documentno + "'";
                sqlcmd += ",'" + Refercontractid + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["ContId"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["fullname"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["address"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Tel"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Cardid"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Expirydate"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Bankid"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Banknameth"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Bankaccountno"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Bankaccountname"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Bankaccounttype"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Contactdate"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Contactstart"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Contactend"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Effectdate"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Sitename"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Sitefulladdress"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Jobdescription"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Totalamount"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Finisheddate"].ToString() + "'";
                sqlcmd += ",'" + Dtcont.Rows[0]["Fee"].ToString() + "'";
                if (Invoicedate != "")
                {

                    sqlcmd += ",getdate() ";

                  //sqlcmd += ",'" + Dtcont.Rows[0]["Invoicedate"].ToString() + "'";
                }
                if (Paymentdate != "")
                {
                    sqlcmd += ",getdate() ";
                    // sqlcmd += ",'" + Dtcont.Rows[0]["Paymentdate"].ToString() + "'";
                }
                sqlcmd += ",'" + Grandtotal.ToString().Replace(",","") + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",getdate()";
                sqlcmd += ",'" + ObjMy.userid + "'";
                sqlcmd += " ) ";
                Arrcmd.Add(sqlcmd);
            }
            else //ถ้ามีอยู่แล้ว
            {
                TransBatchId = ObjCurrentState.TransBatchId;
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id"); // Genarete
                TransformId = ObjCurrentState.TransFormId;
                if (ObjCurrentState.NodeNamefrom.ToLower() == "nodesingle1" || ObjCurrentState.NodeNamefrom.ToLower() == "nodesingle3")
                {
                        sqlcmd = "Select * from [Sys_Edocument_Eform001] m inner join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId and m.isdelete = 0 and d.isdelete = 0  where m.id = '" + Refercontractid + "'";
                        Dtcont = Cn.Select(sqlcmd);
                        sqlcmd = " Update Sys_EDocument_EForm007 set modifydate =getdate(),modifyby='" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                        sqlcmd += " ,[Refercontractid] = '" + Refercontractid + "'";
                        sqlcmd += " ,[ContId] = '" + Dtcont.Rows[0]["ContId"].ToString() + "'";
                        sqlcmd += " ,[fullname]  = '" + Dtcont.Rows[0]["fullname"].ToString() + "'";
                        sqlcmd += " ,[address]  = '" + Dtcont.Rows[0]["address"].ToString() + "'";
                        sqlcmd += " ,[Tel] = '" + Dtcont.Rows[0]["Tel"].ToString() + "'";
                        sqlcmd += " ,[Cardid]  = '" + Dtcont.Rows[0]["Cardid"].ToString() + "'";
                        if (Dtcont.Rows[0]["Expirydate"].ToString() != "")
                        {
                            sqlcmd += " ,[Expirydate] = '" + Dtcont.Rows[0]["Expirydate"].ToString() + "'";
                        }
                    sqlcmd += " ,[Bankid] = '" + Dtcont.Rows[0]["Bankid"].ToString() + "'";
                    sqlcmd += " ,[Banknameth] = '" + Dtcont.Rows[0]["Banknameth"].ToString() + "'";
                    sqlcmd += " ,[Bankaccountno] = '" + Dtcont.Rows[0]["Bankaccountno"].ToString() + "'";
                    sqlcmd += " ,[Bankaccountname] = '" + Dtcont.Rows[0]["Bankaccountname"].ToString() + "'";
                    sqlcmd += " ,[Bankaccounttype] = '" + Dtcont.Rows[0]["Bankaccounttype"].ToString() + "'";
                    if (Dtcont.Rows[0]["Contactdate"].ToString() != "")
                    {
                        sqlcmd += " ,[Contactdate] = '" + Dtcont.Rows[0]["Contactdate"].ToString() + "'";
                    }
                    if (Dtcont.Rows[0]["Contactstart"].ToString() != "")
                    {
                        sqlcmd += " ,[Contactstart] = '" + Dtcont.Rows[0]["Contactstart"].ToString() + "'";
                    }
                    if (Dtcont.Rows[0]["Contactend"].ToString() != "")
                    {
                        sqlcmd += " ,[Contactend] = '" + Dtcont.Rows[0]["Contactend"].ToString() + "'";
                    }
                    if (Dtcont.Rows[0]["Effectdate"].ToString() != "")
                    {
                        sqlcmd += " ,[Effectdate] = '" + Dtcont.Rows[0]["Effectdate"].ToString() + "'";
                    }
                  
                    sqlcmd += " ,[Sitename] = '" + Dtcont.Rows[0]["Sitename"].ToString() + "'";
                    sqlcmd += " ,[Sitefulladdress] = '" + Dtcont.Rows[0]["Sitefulladdress"].ToString() + "'";
                    sqlcmd += " ,[Jobdescription] = '" + Dtcont.Rows[0]["Jobdescription"].ToString() + "'";
                    sqlcmd += " ,[Totalamount] = '" + Dtcont.Rows[0]["Totalamount"].ToString() + "'";
                    if (Dtcont.Rows[0]["Finisheddate"].ToString() != "")
                    {
                        sqlcmd += " ,[Finisheddate] = '" + Dtcont.Rows[0]["Finisheddate"].ToString() + "'";
                    }
                    sqlcmd += " ,[Fee] = '" + Dtcont.Rows[0]["Fee"].ToString() + "'";
                   
                        

                        if (Invoicedate != "")
                        {
                            sqlcmd += ",[Invoicedate] = '" + ClsEngine.ConvertDateforSavingDatabase(Invoicedate) + "'";
                        }
                        if (Paymentdate != "")
                        {
                            sqlcmd += ",[Paymentdate] = '" + ClsEngine.ConvertDateforSavingDatabase(Paymentdate) + "'";
                        }

                    sqlcmd += ",Grandtotal ='" + Grandtotal.ToString().Replace(",", "") + "'";

                    sqlcmd += " Where TransBatchId = '" + TransBatchId  + "'";
                        Arrcmd.Add(sqlcmd);
                    
                }
            }

            sqlcmd = "Update Sys_EDocument_Eform007detail set isdelete =1,deletedate=getdate(),deleteby='" + ObjMy.userid + "'  Where TransBatchId ='" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            string _detailid = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_EDocument_Eform007detail", "Id");
            foreach (ClsEForm007Period period in ObjEform.Periods)
            {
                if (period.Selected == "x")
                {
                    sqlcmd = "";
                    sqlcmd += " INSERT INTO [Sys_EDocument_Eform007detail] ";
                    sqlcmd += " ([id]";
                    sqlcmd += " ,[TransBatchId]";
                    sqlcmd += " ,[Period]";
                    sqlcmd += " ,[PeriodName]";
                    sqlcmd += " ,[Amount]";
                    sqlcmd += " ,[IsDelete]";
                    sqlcmd += " ,[CreateDate]";
                    sqlcmd += " ,[CreateBy] ) ";
                    sqlcmd += " VALUES ";
                    sqlcmd += "( ";
                    sqlcmd += "'" + _detailid + "'";
                    sqlcmd += ",'" + TransBatchId + "'";
                    sqlcmd += ",'" + period.Period + "'";
                    sqlcmd += ",'" + period.Periodname + "'";
                    sqlcmd += ",'" + period.Amount.ToString().Replace(",", "") + "'";
                    sqlcmd += ",'0'";
                    sqlcmd += ",getdate()";
                    sqlcmd += ",'" + ObjMy.userid + "'";
                    sqlcmd += " ) ";
                    Arrcmd.Add(sqlcmd);
                    _detailid = (int.Parse(_detailid) + 1).ToString();
                }
            }
            foreach (ClsDict objdict in Dicts)
            {
                if (objdict.Name.Contains("TxtRemark"))
                {
                    remark = objdict.Val;
                }
            }
            string remarkid = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Master_FlowRemark", "id");
            sqlcmd = "INSERT INTO [Sys_Master_FlowRemark] ";
            sqlcmd += "([Id]";
            sqlcmd += ",[Transbatchid]";
            sqlcmd += ",[TransformId]";
            sqlcmd += ",[Nodename]";
            sqlcmd += ",[Remark]";
            sqlcmd += " ,[IsDelete]";
            sqlcmd += " ,[CreateDate]";
            sqlcmd += " ,[CreateBy] ) ";
            sqlcmd += " VALUES ";
            sqlcmd += "( ";
            sqlcmd += " '" + remarkid + "'";
            sqlcmd += ",'" + TransBatchId + "'";
            sqlcmd += ",'" + TransformId + "'";
            sqlcmd += ",'" + ObjCurrentState.NodeNamefrom.ToLower() + "'";
            sqlcmd += ",'" + remark + "'";
            sqlcmd += ",0";
            sqlcmd += ",getdate()";
            sqlcmd += ",'" + ObjMy.userid + "'";
            sqlcmd += " ) ";
            Arrcmd.Add(sqlcmd);
            string attachmentid = "";
            int count = 0;
            foreach (ClsAttachment _attch in ((ClsEForm007)(HttpContext.Current.Session["EFORM007"])).Attachments)
            {
                count += 1;
                if (count < ((ClsEForm007)(HttpContext.Current.Session["EFORM007"])).Attachments.Count)
                {
                    attachmentid += _attch.Attachmentid + ",";
                }
                else
                {
                    attachmentid += _attch.Attachmentid;
                }

            }
            //ลบตัวที่ไม่ได้อยู่ใน Session ทิ้ง
            if (attachmentid != "")
            {
                sqlcmd = "Update Sys_EDocument_Transflowattachment set isdelete = 1,deletedate=getdate(),deleteby ='" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "' Where uploaduserid = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "' and Transbatchid = '" + TransBatchId + "' and attachmentid not in (" + attachmentid + ")";
                Arrcmd.Add(sqlcmd);
            }
            //เพิ่มตัวที่อยู่ใน Session
            List<ClsAttachment> Attachs = new List<ClsAttachment>();
            Attachs = ((ClsEForm007)(HttpContext.Current.Session["EFORM007"])).Attachments;
            DataTable Dtattachment = new DataTable();
            string _transattachment = ClsEngine.GenerateRunningId(Connectionstring, "Sys_EDocument_Transflowattachment", "id");
            sqlcmd = "Select * from [Sys_EDocument_Transflowattachment]  where isdelete = 0 and uploaduserid = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "' and Transbatchid = '" + TransBatchId + "'";
            Dtattachment = Cn.Select(sqlcmd);
            foreach (DataRow dr in Dtattachment.Rows)//ลบตัวที่มีออกจาก Object
            {
                Attachs.RemoveAll(attch => attch.Attachmentid == dr["Attachmentid"].ToString());
            }
            foreach (ClsAttachment _attch in Attachs) //เหลือตัวที่ไม่ซ้ำ
            {
                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_EDocument_Transflowattachment] ";
                sqlcmd += " ([Id] ";
                sqlcmd += " ,[Transbatchid] ";
                sqlcmd += " ,[Attachmentid] ";
                sqlcmd += " ,[Label] ";
                sqlcmd += " ,[Uploaduserid] ";
                sqlcmd += " ,[Uploaddate] ";
                sqlcmd += " ,[Uploadbynameth] ";
                sqlcmd += " ,[isdelete] ";
                sqlcmd += " ,[CreateDate] ";
                sqlcmd += " ,[CreateBy] ) ";
                sqlcmd += "  VALUES ( ";
                sqlcmd += " '" + _transattachment + "'";
                sqlcmd += " ,'" + TransBatchId + "'";
                sqlcmd += " ,'" + _attch.Attachmentid + "'";
                sqlcmd += " ,'" + _attch.Label + "'";
                sqlcmd += " ,'" + _attch.Uploaduserid + "'";
                sqlcmd += " ,'" + _attch.Uploaddate + "'";
                sqlcmd += " ,'" + _attch.Uploadbynameth + "'";
                sqlcmd += " ,'0'";
                sqlcmd += " ,Getdate()";
                sqlcmd += " ,'" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "')";
                Arrcmd.Add(sqlcmd);
                _transattachment = (int.Parse(_transattachment) + 1).ToString();
            }
            sqlcmd = "Select * from [Sys_Master_Userinflow] Where FlowId ='" + ObjCurrentState.FlowId + "'";
            DtUserInFlow = Cn.Select(sqlcmd);
            //ดูว่าตอนนี้ User ที่กดอยู่ Node ไหนและเลือกเส้นทางใด
            //if (ObjCurrentState.NodeTypeId == "1" || ObjCurrentState.NodeTypeId == "3") //Node Begin ,Node Single
            //{
            Drs = ((DataTable)ObjCurrentState.DataTableMasterFlowDetail).Select("NodeNamefrom='" + ObjCurrentState.NodeNamefrom + "' and DirectionValue ='" + ClsEngine.FindValue(Dicts, "ActionResultValue") + "'");
            FlowDetailId = Drs[0]["Id"].ToString();
            NodeNameOut = Drs[0]["NodeNameTo"].ToString();
            NodeTypeIdOut = Drs[0]["NodeTypeIdTo"].ToString();
            NodeTypeNameTHOut = Drs[0]["NodeTypeNameTHTo"].ToString();
            if (NodeTypeIdOut == "1" || NodeTypeIdOut == "3")
            {
                if (DtUserInFlow.Select("NodeName='" + NodeNameOut + "' and RefType='S'").Length != 0)
                {
                    UserIdOut = ((Clsuser)HttpContext.Current.Session["My"]).userid;
                }
                else
                {
                    UserIdOut = DtUserInFlow.Select("NodeName='" + NodeNameOut + "' and RefType='U'")[0]["RefId"].ToString();
                }
            }
            //}
            //======================================  FLOW =========================================
            if (ObjCurrentState.NodeTypeId == "1") // NodeBegin เป็น Nodefrom 
            {
                sqlcmd = "INSERT INTO [Sys_Trans_Flow] ";
                sqlcmd += "([TransBatchId] ";
                sqlcmd += ",[FlowId] ";
                sqlcmd += ",[FormId] ";
                sqlcmd += ",[Subject] ";
                sqlcmd += ",[CreateDate] ";
                sqlcmd += ",[CreateBy] ";
                sqlcmd += ",[PendingDate] ";
                sqlcmd += ",[PendingBy] ";
                sqlcmd += ",[PendingByNodeName] ";
                sqlcmd += ",[PendingByNodeTypeId] ";
                sqlcmd += ",[PendingByNodeTypeNameTH] ";
                sqlcmd += ",[PendingByType] ";
                sqlcmd += ",[Isdelete])";
                sqlcmd += "VALUES ( ";
                sqlcmd += "'" + TransBatchId + "'";
                sqlcmd += ",'" + ObjCurrentState.FlowId + "'";
                sqlcmd += ",'" + ObjCurrentState.FormId + "'";
                sqlcmd += ",'" + ObjCurrentState.FormNameTH + "เลขที่ " + DocumentNo + "'";
                sqlcmd += ",'" + System.DateTime.Now + "'";
                sqlcmd += ",'" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                sqlcmd += ",'" + System.DateTime.Now + "'";
                sqlcmd += ",'" + UserIdOut + "'";
                sqlcmd += ",'" + NodeNameOut + "'";
                sqlcmd += ",'" + NodeTypeIdOut + "'";
                sqlcmd += ",'" + NodeTypeNameTHOut + "'";
                sqlcmd += ",'u'";
                sqlcmd += ",0)";
                Arrcmd.Add(sqlcmd);
            }


            if (NodeTypeIdOut == "3") //NodeSingle
            {
                sqlcmd = "Update Sys_Trans_Flow Set ";
                sqlcmd += " [PendingDate] ='" + System.DateTime.Now + "'";
                sqlcmd += ",[PendingBy] ='" + UserIdOut + "'";
                sqlcmd += ",[PendingByNodeName] ='" + NodeNameOut + "'";
                sqlcmd += ",[PendingByNodeTypeId] ='" + NodeTypeIdOut + "'";
                sqlcmd += ",[PendingByNodeTypeNameTH]='" + NodeTypeNameTHOut + "'";
                sqlcmd += ",[PendingByType] ='U'";
                sqlcmd += "  Where TransBatchId = '" + TransBatchId + "'";
                Arrcmd.Add(sqlcmd);
            }
            else if (NodeTypeIdOut == "4") //NodeMultiple
            {
                //*************** ตารางของ Transactionflow กับ Multiple มีการเปลี่ยนแปลง
                UserIdOut = "";
                MultipleRule = ((DataTable)ObjCurrentState.DataTableMasterFlowDetail).Select("NodeNamefrom='" + NodeNameOut + "'")[0]["MultiPleRule"].ToString();
                string MultipleDetail = ClsEngine.GenerateRunningId(Connectionstring, "TransNodeMultipleDetail", "Id");
                foreach (DataRow dr in DtUserInFlow.Select("NodeName='" + NodeNameOut + "' and RefType='U'"))
                {
                    if (UserIdOut != "")
                    {
                        UserIdOut += ",|" + dr["RefId"].ToString() + "|";
                    }
                    else
                    {
                        UserIdOut += "|" + dr["RefId"].ToString() + "|";
                    }
                }
                sqlcmd = "Update Sys_Trans_Flow Set ";
                sqlcmd += " [PendingDate] ='" + System.DateTime.Now + "'";
                sqlcmd += ",[PendingBy] ='" + UserIdOut + "'";
                sqlcmd += ",[PendingByNodeName] ='" + NodeNameOut + "'";
                sqlcmd += ",[PendingByNodeTypeId] ='" + NodeTypeIdOut + "'";
                sqlcmd += ",[PendingByNodeTypeNameTH]='" + NodeTypeNameTHOut + "'";
                sqlcmd += ",[PendingByType] ='U'";
                sqlcmd += "  Where TransBatchId = '" + TransBatchId + "'";
                Arrcmd.Add(sqlcmd);
                foreach (DataRow dr in DtUserInFlow.Select("NodeName='" + NodeNameOut + "' and RefType='U'"))
                {
                    sqlcmd = " INSERT INTO [TransNodeMultipleDetail] ";
                    sqlcmd += " ([Id] ";
                    sqlcmd += " ,[TransbatchId] ";
                    sqlcmd += " ,[NodeMultipleName] ";
                    sqlcmd += " ,[TransFlowDetailId] ";
                    sqlcmd += " ,[MultipleRule] ";
                    sqlcmd += " ,[UserId] ";
                    sqlcmd += " ,[Isdelete] ";
                    sqlcmd += " ,[IsAction]) ";
                    sqlcmd += "  VALUES ( ";
                    sqlcmd += "'" + MultipleDetail + "'";
                    sqlcmd += ",'" + TransBatchId + "'";
                    sqlcmd += ",'" + NodeNameOut + "'";
                    sqlcmd += ",'" + TransFlowDetailId + "'";
                    sqlcmd += ",'" + MultipleRule + "'";
                    sqlcmd += ",'" + dr["RefId"].ToString() + "'";
                    sqlcmd += ",0";
                    sqlcmd += ",0";
                    sqlcmd += " ) ";
                    Arrcmd.Add(sqlcmd);
                    MultipleDetail = (int.Parse(MultipleDetail) + 1).ToString();
                }
            }
            else if (NodeTypeIdOut == "2") //NodeEnd
            {
                if (ClsEngine.FindValue(Dicts, "ActionResultValue") == "R") //Cancel
                {
                    sqlcmd = "Update Sys_Trans_Flow Set ";
                    sqlcmd += " [PendingDate] ='" + System.DateTime.Now + "'";
                    sqlcmd += ",[PendingBy] =''";
                    sqlcmd += ",[PendingByNodeName] ='" + NodeNameOut + "'";
                    sqlcmd += ",[PendingByNodeTypeId] ='" + NodeTypeIdOut + "'";
                    sqlcmd += ",[PendingByNodeTypeNameTH]='" + NodeTypeNameTHOut + "'";
                    sqlcmd += ",[PendingByType] ='U'";
                    sqlcmd += ",[CancelDate]='" + System.DateTime.Now + "'";
                    sqlcmd += ",[Iscancel] = 1";
                    sqlcmd += ",[CancelBy] = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                    sqlcmd += ",[CancelByType] = 'U'";
                    sqlcmd += " Where TransBatchId = '" + TransBatchId + "'";
                    Arrcmd.Add(sqlcmd);
                }
                else
                {
                    sqlcmd = "Update Sys_Trans_Flow Set ";
                    sqlcmd += " [PendingDate] ='" + System.DateTime.Now + "'";
                    sqlcmd += ",[PendingBy] =''";
                    sqlcmd += ",[PendingByNodeName] ='" + NodeNameOut + "'";
                    sqlcmd += ",[PendingByNodeTypeId] ='" + NodeTypeIdOut + "'";
                    sqlcmd += ",[PendingByNodeTypeNameTH]='" + NodeTypeNameTHOut + "'";
                    sqlcmd += ",[PendingByType] ='U'";
                    sqlcmd += ",[CompleteDate]='" + System.DateTime.Now + "'";
                    sqlcmd += ",[IsComplete] = 1";
                    sqlcmd += ",[CompleteBy] = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                    sqlcmd += ",[CompleteByType] = 'U'";
                    sqlcmd += " Where TransBatchId = '" + TransBatchId + "'";
                    Arrcmd.Add(sqlcmd);
                }
            }

            //Node Begin Write แค่ 1 Record"
            sqlcmd = "Update Sys_Trans_Flowdetail Set Ispass='1' Where TransBatchId = '" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            sqlcmd = "INSERT INTO [Sys_Trans_Flowdetail] ";
            sqlcmd += "([Id] ";
            sqlcmd += ",[TransBatchId] ";
            sqlcmd += ",[FlowId] ";
            sqlcmd += ",[FormId] ";
            sqlcmd += ",[FlowDetailId] ";
            sqlcmd += ",[UserIdin] ";
            sqlcmd += ",[NodeNameIn] ";
            sqlcmd += ",[NodeTypeNameInTH]";
            sqlcmd += ",[UserIdOut] ";
            sqlcmd += ",[NodeNameOut] ";
            sqlcmd += ",[NodeTypeNameOutTH]";
            sqlcmd += ",[Remark] ";
            sqlcmd += ",[ActionDate] ";
            sqlcmd += ",[ActionResultValue] ";
            sqlcmd += ",[ActionResultNameTH] ";
            sqlcmd += ",[ActionById] ";
            sqlcmd += ",[ActionByTitleCode] ";
            sqlcmd += ",[ActionByTitleNameTH] ";
            sqlcmd += ",[ActionByfirstnameth] ";
            sqlcmd += ",[ActionBylastnameth] ";
            sqlcmd += ",[ActionByPositionId] ";
            sqlcmd += ",[ActionByPositionNameTH] ";
            sqlcmd += ",[ActionByOrganizeId] ";
            sqlcmd += ",[ActionByOrganizeNameTH] ";
            sqlcmd += ",[SignatureURL] ";
            sqlcmd += ",[Isfinished] ";
            sqlcmd += ",[isdelete]) ";
            sqlcmd += " VALUES (";
            sqlcmd += "'" + TransFlowDetailId + "'"; //Running ของตารางนีี้ว่าส่งหาใครบ้างในรายคน
            sqlcmd += ",'" + TransBatchId + "'"; //เลขที่ Transaction
            sqlcmd += ",'" + ObjCurrentState.FlowId + "'";
            sqlcmd += ",'" + ObjCurrentState.FormId + "'";
            sqlcmd += ",'" + FlowDetailId + "'"; //เลขที่อ้างอิงหาร Sys_Master_FlowDetail (ตัวกำหนด Flow)
            sqlcmd += ",'" + ObjMy.userid + "'";  //User ที่ส่ง NodeNameIn
            sqlcmd += ",'" + ObjCurrentState.NodeNamefrom + "'"; //Node ที่ส่ง
            sqlcmd += ",'" + ObjCurrentState.NodeTypeNameTH + "'"; //Node ที่ส่ง
            sqlcmd += ",'" + UserIdOut + "'";  //User ที่รับ
            sqlcmd += ",'" + NodeNameOut + "'"; //Node ที่รับ
            sqlcmd += ",'" + NodeTypeNameTHOut + "'"; //Node ที่รับ
            sqlcmd += ",'" + remark + "'"; //signature
            sqlcmd += ",'" + DateTime.Now + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "ActionResultValue") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "ActionResultNameTH") + "'";
            sqlcmd += ",'" + ObjMy.userid + "'";
            sqlcmd += ",'" + ObjMy.Titlecode + "'";
            sqlcmd += ",'" + ObjMy.Titlenameth + "'";
            sqlcmd += ",'" + ObjMy.firstnameth + "'";
            sqlcmd += ",'" + ObjMy.lastnameth + "'";
            sqlcmd += ",'" + ObjMy.positionid + "'";
            sqlcmd += ",'" + ObjMy.positionnameth + "'";
            sqlcmd += ",'" + ObjMy.organizeId + "'";
            sqlcmd += ",'" + ObjMy.organizenameth + "'";
            sqlcmd += ",'" + ObjMy.sigurl + "'";
            sqlcmd += ",'0'";
            sqlcmd += ",0)";
            Arrcmd.Add(sqlcmd);
            if (ObjCurrentState.NodeTypeId == "4")
            {
                //Update Remark Footer
                Dt = new DataTable();
                string RemarkFooter = "";
                string TransflowdetailId = Cn.Select("select * from Sys_Trans_Flowdetail Where TransBatchId = '" + TransBatchId + "' and NodeNameOut = '" + ObjCurrentState.NodeNamefrom + "'").Rows[0]["Id"].ToString();
                sqlcmd = "Select * from [TransNodeMultipleDetail] Where isdelete = 0 and NodeMultipleName = '" + ObjCurrentState.NodeNamefrom + "' and TransbatchId = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and TransflowdetailId ='" + TransflowdetailId + "'";
                Dt = Cn.Select(sqlcmd);
                foreach (DataRow dr in Dt.Rows)
                {
                    sqlcmd = "Update TransNodeMultipleDetail Set TransFlowDetailOutId = '" + TransFlowDetailId + "',[RemarkFooter] ='" + RemarkFooter + "' Where isdelete = 0 and NodeMultipleName = '" + ObjCurrentState.NodeNamefrom + "' and TransbatchId = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and TransflowdetailId ='" + TransflowdetailId + "' and UserId = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                    Arrcmd.Add(sqlcmd);
                }
            }
            if (NodeTypeIdOut == "2") //NodeEnd
            {
                //Node Begin Write แค่ 1 Record"
                sqlcmd = "Update Sys_Trans_Flowdetail Set Ispass='1' Where TransBatchId = '" + TransBatchId + "'";
                Arrcmd.Add(sqlcmd);
                sqlcmd = "Update Sys_Trans_Flowdetail Set Isfinished='1' Where TransBatchId = '" + TransBatchId + "' and Id = '" + TransFlowDetailId + "'";
                Arrcmd.Add(sqlcmd);
            }
            //======================================  FLOW =========================================
            Cn.Execute(Arrcmd, null);
            Cn.Close();
            return "";

        }
        #region "Grid"
        [WebMethod]
        public static string ExecuteDeleteGrid(string Ctrl, string PK)
        {
            return "";
        }
        [WebMethod]
        public static string ExecuteGrid(string Mode, string Ctrl, string Dat, string PK)
        {
            return "";
        }
        [WebMethod]
        public static string GetTotalRecord(string Ctrl)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.GetTotalRecord(Ctrl);
        }
        [WebMethod]
        public static string GetCriteriaValue()
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.GetCriteriaValue();
        }
        [WebMethod]
        public static void ClearResource(string Ctrl)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            ObjGrid.ClearResource(Ctrl);
        }
        [WebMethod]
        public static void Selected(string Ctrl, string ProjectId) //Finished
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            ObjGrid.Selected(Ctrl, ProjectId);

        }
        [WebMethod]
        public static ClsDictExtend DatSelect(string Ctrl, string PK, string SelName)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.DatSelect(Ctrl, PK, SelName);
        }
        [WebMethod]
        public static void UnSelected(string Ctrl, string ProjectId) //Finished
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            ObjGrid.UnSelected(Ctrl, ProjectId);
        }
        [WebMethod]
        public static void SelectAll(string Ctrl, string PK)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            ObjGrid.SelectAll(Ctrl, PK);
        }


        [WebMethod] //Finished
        public static string UnSelectAll(string Ctrl)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.UnSelectAll(Ctrl);
        }

        [WebMethod]
        public static List<ClsCustomer> Selcust(string json)
        {
            ClsLH Obj = new ClsLH();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            try
            {
                return Obj.GetCustomer(ref cn, ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x"));
            }
            catch
            {
                return null;
            }
            finally
            {
                cn.Close();
            }
        }

        [WebMethod]
        public static List<ClsDictExtend> Getbank()
        {
            SqlConnector cn = new SqlConnector(Connectionstring, "");
            string sqlcmd = "Select bankid, BankNameTH as bankname from Sys_Master_Bank where isdelete = 0";
            DataTable Dt = new DataTable();
            List<ClsDictExtend> Dicts = new List<ClsDictExtend>();
            ClsDictExtend Objdict;
            Dt = cn.Select(sqlcmd);
            Objdict = new ClsDictExtend();
            Objdict.Name = "-- Please select  --";
            Objdict.Val = "";
            Dicts.Add(Objdict);
            foreach (DataRow dr in Dt.Rows)
            {
                Objdict = new ClsDictExtend();
                Objdict.Name = dr["bankname"].ToString();
                Objdict.Val = dr["bankid"].ToString();
                Dicts.Add(Objdict);
            }
            return Dicts;
        }
        [WebMethod]
        public static ClsEForm007 Selcontract(string json)
        {
            DataTable Dt = new DataTable();
            ClsEForm007 Obj = new ClsEForm007();
            DataTable DtPeriodEForm001 = new DataTable();
            SqlConnector cn = new SqlConnector(Connectionstring, "");
            string x = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x");
            string sqlcmd = "Select * from Sys_Edocument_Eform001 m inner join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId and m.isdelete = 0 and d.isdelete = 0  where m.id = '" + x + "'";
            Dt = cn.Select(sqlcmd);
            Obj = ((ClsEForm007)HttpContext.Current.Session["EFORM007"]);
            Obj.id = Guid.NewGuid().ToString();
            Obj.Documentno = Dt.Rows[0]["Documentno"].ToString();
            Obj.Refercontractid = Dt.Rows[0]["id"].ToString();
            Obj.Contid = Dt.Rows[0]["ContId"].ToString();
            Obj.fullname = Dt.Rows[0]["fullname"].ToString();
            Obj.address = Dt.Rows[0]["address"].ToString();
            Obj.Tel = Dt.Rows[0]["Tel"].ToString();
            Obj.Cardid = Dt.Rows[0]["Cardid"].ToString();
            if (Dt.Rows[0]["Expirydate"].ToString() != "")
            {
                Obj.Expirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Expirydate"].ToString()), "/", false);
            }
            else
            {
                Obj.Expirydate = "";
            }

           
            Obj.Bankid = Dt.Rows[0]["Bankid"].ToString();
            Obj.Banknameth = Dt.Rows[0]["Banknameth"].ToString();
            Obj.Bankaccountno = Dt.Rows[0]["Bankaccountno"].ToString();
            Obj.Bankaccountname = Dt.Rows[0]["Bankaccountname"].ToString();
            Obj.Bankaccounttype = Dt.Rows[0]["Bankaccounttype"].ToString();
            if (Dt.Rows[0]["Contactdate"].ToString() != "")
            {
                Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactdate"].ToString()), "/", false);
            }
            else
            {
                Obj.Contactdate = "";
            }

            if (Dt.Rows[0]["Contactstart"].ToString() != "")
            {
                Obj.Contactstart = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactstart"].ToString()), "/", false);
            }
            else
            {
                Obj.Contactstart = "";
            }

            if (Dt.Rows[0]["Contactend"].ToString() != "")
            {
                Obj.Contactend = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactend"].ToString()), "/", false);
            }
            else
            {
                Obj.Contactend = "";
            }
            Obj.Effectdate = Dt.Rows[0]["Effectdate"].ToString();
            Obj.Fee = Dt.Rows[0]["Fee"].ToString();
            Obj.Sitename = Dt.Rows[0]["Sitename"].ToString();
            Obj.Sitefulladdress = Dt.Rows[0]["Sitefulladdress"].ToString();
            Obj.Jobdescription = Dt.Rows[0]["Jobdescription"].ToString();
            Obj.Totalamount = Dt.Rows[0]["Totalamount"].ToString();

            if (Dt.Rows[0]["Finisheddate"].ToString() != "")
            {
                Obj.Finisheddate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Finisheddate"].ToString()), "/", false);
            }
            else
            {
                Obj.Finisheddate = "";
            }


          
            Obj.Invoicedate = "";
            Obj.Paymentdate = "";
            
            double Grandtotal = 0;


            if (((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId == "0")
            {

                List<ClsEForm007Period> Periods = new List<ClsEForm007Period>();
                ClsEForm007Period Objperiod;
                foreach (DataRow s_dr in Dt.Rows)
                {
                    Objperiod = new ClsEForm007Period();
                    Objperiod.Period = s_dr["Period"].ToString();
                    Objperiod.Periodname = s_dr["Periodname"].ToString();
                    Objperiod.Amount = double.Parse(s_dr["Amount"].ToString()).ToString("N2");
                    Objperiod.Selected = "";
                    Grandtotal += double.Parse(Objperiod.Amount);
                    Periods.Add(Objperiod);
                }
                Obj.Periods = Periods;
                Obj.Grandtotal = Grandtotal.ToString();
                HttpContext.Current.Session["EFORM007"] = Obj;
            }
            else
            {
                List<ClsEForm007Period> Periods = new List<ClsEForm007Period>();
                Dt = new DataTable();

                sqlcmd = "Select * from Sys_EDocument_EForm007detail where Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and isdelete = 0 order by id ";
                Dt = cn.Select(sqlcmd);
                sqlcmd = "Select * from Sys_EDocument_Eform001 m left join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Refercontractid + "'";
                DtPeriodEForm001 = cn.Select(sqlcmd);
                ClsEForm007Period Objperiod;
                foreach (DataRow s_dr in DtPeriodEForm001.Rows)
                {
                    Objperiod = new ClsEForm007Period();
                    Objperiod.Period = s_dr["Period"].ToString();
                    Objperiod.Periodname = s_dr["Periodname"].ToString();
                    Objperiod.Amount = s_dr["Amount"].ToString();
                    Grandtotal += double.Parse(Objperiod.Amount);
                    if (Dt.Select("Period='" + s_dr["Period"].ToString() + "'").Length > 0)
                    {
                        Objperiod.Selected = "x";
                    }
                    else
                    {
                        Objperiod.Selected = "";
                    }
                    Periods.Add(Objperiod);
                }
                //Detail
                Obj.Periods = Periods;
            }
            return Obj;
        }

        [WebMethod]
        public static ClsGridResponse Bind(string Ctrl, long PagePerItem, long CurrentPage, string SortName, string Order, string Column, string Data, string Initial, string SelectCat, string SearchMsg, string EditButton, string DeleteButton, string Panel, string FullRowSelect, string Multiselect, string Criteria, string SearchesDat, string Searchcolumns, string WPanel, string HPanel)
        {
            string Sqlcmd = "";
            string PK = "";
            List<ClsDict> CriterialMapping = new List<ClsDict>();
            ClsGrid Objgrid = new ClsGrid();
            Clsuser Objmy = (Clsuser)HttpContext.Current.Session["My"];
            if (Ctrl == "Gvcontract")
            {
                PK = "id";
                Sqlcmd = "Select E.id,Documentno,Fullname,Sitename, Convert(nvarchar,isnull(Contactstart,''),103) + ' -  ' + Convert(nvarchar,isnull(Contactend,''),103) as Contractdate from Sys_EDocument_Eform001 E inner join Sys_Trans_Flow F on E.Transbatchid =  F.TransBatchId Where isnull(IsComplete,0) = 1 and E.Isdelete = 0 and F.Isdelete = 0 ";
                SqlConnector Cn = new SqlConnector(Connectionstring, null);
                try
                {
                    ClsGridResponse ObjGridResponse = Objgrid.Bind(ref Cn, Ctrl, PagePerItem, CurrentPage, SortName, Order, Column, Data, Initial, SelectCat, SearchMsg, EditButton, DeleteButton, Panel, FullRowSelect, Multiselect, Criteria, PK, Sqlcmd, CriterialMapping, SearchesDat, Searchcolumns, WPanel, HPanel);
                    return ObjGridResponse;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    Cn.Close();
                }
            }

            return null;
        }
        
        [WebMethod]
        public static string Edit(string ProjectId)
        {
            return "";
        }
        [WebMethod]
        public static List<ClsDict> Load2GridPanel(string Ctrl, string dat)
        {
            return null;
        }
        [WebMethod]
        public static string Sort(string Ctrl, string ColName)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.Sort(Ctrl, ColName);
        }
        [WebMethod]
        public static ClsGridResponse GetResource(string Ctrl)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.GetResource(Ctrl);
        }
        [WebMethod]
        public static string UpdCurrentPage(string Ctrl, string CurrentPage)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.UpdCurrentPage(Ctrl, CurrentPage);
        }
        [WebMethod]
        public static string UpdInitial(string Ctrl, string Initial)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.UpdInitial(Ctrl, Initial);
        }
        [WebMethod]
        public static string Export(string Ctrl)
        {
            ClsGrid ObjGrid;
            ObjGrid = (ClsGrid)HttpContext.Current.Session["ObjGrid"];
            return ObjGrid.Export(Ctrl);
        }

        #endregion
    }
}