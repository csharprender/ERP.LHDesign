using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Services;
using SVframework2016;
using System.Data;
using ERP.LHDesign2020.Class;
namespace ERP.LHDesign2020.Page.EDWF.Forms.HireContact
{
    public partial class HireContact : System.Web.UI.Page
    {
        private const string _Eformcode = "EFORM008";
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        public static string UrlAttachment = ConfigurationManager.AppSettings["UrlAttachment"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session.Remove("EFORM008");
                HttpContext.Current.Session.Remove("Period");
                ClsGrid ObjGrid = new ClsGrid();
                HttpContext.Current.Session["ObjGrid"] = ObjGrid;
            }
        }
        [WebMethod]
        public static string Deleteattachment(string json)
        {
            Boolean isfound = false;
            if (HttpContext.Current.Session["EFORM008"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEform008)HttpContext.Current.Session["EFORM008"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        isfound = true;
                        if (obj.Uploaduserid == ((Clsuser)HttpContext.Current.Session["My"]).userid)
                        {
                            ((ClsEform008)HttpContext.Current.Session["EFORM008"]).Attachments.Remove(obj);
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
            if (HttpContext.Current.Session["EFORM008"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEform008)HttpContext.Current.Session["EFORM008"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        return obj.URL;
                    }
                }
            }
            //not found ,vendoract admin
            return "";
        }
        [WebMethod]
        public static List<ClsAttachment> Getupload(string json)
        {
            ClsEform008 Obj = new ClsEform008();
            Obj = (ClsEform008)HttpContext.Current.Session["EFORM008"];
            return Obj.Attachments;
        }
        [WebMethod]
        public static string Savestate(string json)
        {
            List<ClsEForm008Period> Objs = new List<ClsEForm008Period>();
            ClsEForm008Period Obj;
            string[] Arr = json.Split('#');
            string id = "";
            string name = "";
            string amount = "";

            foreach (string str in Arr)
            {
                if (str != "")
                {
                    id = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "id");
                    name = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "name");
                    amount = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "Amount");
                    Obj = new ClsEForm008Period();
                    Obj.Period = (int.Parse(id) + 1).ToString();


                    Obj.Periodname = name;

                    if (Obj.Amount != "")
                    {
                        Obj.Amount = amount;
                    }
                    else
                    {
                        Obj.Amount = "0.00";
                    }


                    Objs.Add(Obj);
                }

            }
            HttpContext.Current.Session["Period"] = Objs;
            return "";
        }
        [WebMethod]
        public static List<ClsEForm008Period> Renderperiod()
        {
            if (HttpContext.Current.Session["Period"] != null)
            {
                return (List<ClsEForm008Period>)HttpContext.Current.Session["Period"];
            }
            else
            {
                List<ClsEForm008Period> Periods = new List<ClsEForm008Period>();
                HttpContext.Current.Session["Period"] = Periods;
                return Periods;
            }
        }
        [WebMethod]
        public static string Createperiod(string x)
        {
            List<ClsEForm008Period> Periods = new List<ClsEForm008Period>();
            ClsEForm008Period Obj = new ClsEForm008Period();
            try
            {
                for (int i = 1; i <= int.Parse(x); i++)
                {
                    Obj = new ClsEForm008Period();
                    Obj.Period = i.ToString();
                    Obj.Periodname = "";
                    Obj.Amount = "";
                    Periods.Add(Obj);

                }
                HttpContext.Current.Session["Period"] = Periods;
                return "";
            }
            catch
            {
                return "โปรดระบุงวดงานก่อน";
            }

        }

        [WebMethod]
        public static ClsEform008 GetDocumentInfo(string json)
        {
            const string Eformcode = "EFORM008";
            ClsEform008 Obj = new ClsEform008();
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            DataTable Dt = new DataTable();
            DataTable DtAttchment = new DataTable();
            string Sqlcmd = "";
            try
            {
                if (HttpContext.Current.Session["EFORM008"] != null)
                {
                    return (ClsEform008)HttpContext.Current.Session["EFORM008"];
                }

                Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
                Obj.EFormcode = Eformcode;
                Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                Obj.DocumentDate = System.DateTime.Today.ToShortDateString();
                Dt = new DataTable();
                Sqlcmd = "select * from [Sys_Edocument_EFORM008] Where Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                Dt = Cn.Select(Sqlcmd);
                Obj.Transbatchid = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
                Cn.Close();
                if (Dt.Rows.Count > 0)
                {
                    Obj.Documentno = Dt.Rows[0]["DocumentNo"].ToString(); //ถ้าเป็น Node อื่นๆ ที่ไม่ใช่ Node Begin 
                    if (Obj.Documentno.Trim() == "")
                    {
                        Obj.Documentno = ClsEngine.GenerateRunningno(_Eformcode, Connectionstring, "Sys_Edocument_EFORM008", "id");
                    }
                    try
                    {
                        Obj.DocumentDate = DateTime.Parse(Dt.Rows[0]["DocumentDate"].ToString()).ToShortDateString();
                    }
                    catch
                    {

                    }

                    HttpContext.Current.Session["Period"] = null;
                    Sqlcmd = "Select e001.*,convert(nvarchar,e001.CreateDate,103) as opendate,e001.isdelete from  Sys_EDocument_EFORM008 e001  where e001.isdelete = 0 and e001.Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                    Dt = new DataTable();
                    Dt = Cn.Select(Sqlcmd);
                    Obj.Documentno = Dt.Rows[0]["Documentno"].ToString();

                    ////Transaction
                    Obj.VendorId = Dt.Rows[0]["VendorId"].ToString();
                    Obj.fullname = Dt.Rows[0]["fullname"].ToString();
                    Obj.address = Dt.Rows[0]["address"].ToString();
                    Obj.Tel = Dt.Rows[0]["Tel"].ToString();
                    Obj.Cardid = Dt.Rows[0]["Cardid"].ToString();
                    if (Dt.Rows[0]["Expirydate"].ToString() != "")
                    {
                        Obj.Expirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Expirydate"].ToString()), "-", false);
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
                        Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactdate"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(System.DateTime.Today, "-", false);
                    }

                    if (Dt.Rows[0]["Contactstart"].ToString() != "")
                    {
                        Obj.Contactstart = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactstart"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.Contactstart = "";
                    }

                    if (Dt.Rows[0]["Contactend"].ToString() != "")
                    {
                        Obj.Contactend = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactend"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.Contactend = "";
                    }



                    Obj.Effectdate = Dt.Rows[0]["Effectdate"].ToString(); //Total
                    Obj.Sitename = Dt.Rows[0]["Sitename"].ToString();
                    Obj.Sitefulladdress = Dt.Rows[0]["Sitefulladdress"].ToString();
                    Obj.Jobdescription = Dt.Rows[0]["Jobdescription"].ToString();
                    Obj.Totalamount = double.Parse(Dt.Rows[0]["Totalamount"].ToString()).ToString("N2");

                    if (Dt.Rows[0]["Finisheddate"].ToString() != "")
                    {
                        Obj.Finisheddate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Finisheddate"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.Finisheddate = "";
                    }
                    Obj.Fee = double.Parse(Dt.Rows[0]["Fee"].ToString()).ToString("N2");
                    List<ClsEForm008Period> Periods = new List<ClsEForm008Period>();
                    Dt = new DataTable();
                    Sqlcmd = "Select * from Sys_EDocument_EFORM008detail where Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and isdelete = 0 order by id ";
                    Dt = Cn.Select(Sqlcmd);
                    ClsEForm008Period Objperiod;
                    foreach (DataRow s_dr in Dt.Rows)
                    {
                        Objperiod = new ClsEForm008Period();
                        Objperiod.Period = s_dr["Period"].ToString();
                        Objperiod.Periodname = s_dr["Periodname"].ToString();
                        Objperiod.Amount = double.Parse(s_dr["Amount"].ToString()).ToString("N2");
                        Periods.Add(Objperiod);
                    }
                    //Detail
                    Obj.Periods = Periods;
                    HttpContext.Current.Session["Period"] = Periods;
                }
                else
                {
                    Obj.Documentno = "";
                    Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(System.DateTime.Today, "-", false);
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
                HttpContext.Current.Session["EFORM008"] = Obj;
                //  
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
            Attachements = ((ClsEform008)HttpContext.Current.Session["EFORM008"]).Attachments;
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
            ((ClsEform008)HttpContext.Current.Session["EFORM008"]).Attachments = Attachements;
            Cn.Close();
            return "";

        }
        [WebMethod]
        public static string Print(string json)
        {
            const string Eformcode = "EFORM008";
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
            catch
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
            //ClsAttachment ObjAttachment;
            //if (HttpContext.Current.Session["Attachment_" + NodeName] == null)
            //{
            //    string sqlcmd = "Select TA.Id as AttachmentId,FileName,Extension,[Path],F.AttachmentId  as TransAttachmentId,UploadDate,UploadBy,U.TitleNameTH,U.firstnameth,U.lastnameth from [Sys_Trans_Flowdetail] as T left join [TransApproveAttachmentForm] TF on T.Id =  TF.TransFlowDetailId left join [TransApproveAttachmentFile] F on TF.Id = F.TransAttachmentForm left join [TranAttachment] TA on F.AttachmentId = TA.Id left join Sys_Master_User U on F.UploadBy = U.UserId  Where F.NodeName  = '" + NodeName + "' and TA.Isdelete = 0 and T.TransBatchId = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
            //    Dt = Cn.Select(sqlcmd);
            //    Cn.Close();
            //    foreach (DataRow dr in Dt.Rows)
            //    {
            //        ObjAttachment = new ClsAttachment();
            //        ObjAttachment.AttachmentId = Guid.NewGuid().ToString();
            //        ObjAttachment.FileName = dr["FileName"].ToString();
            //        ObjAttachment.Extension = dr["Extension"].ToString();
            //        ObjAttachment.Path = dr["Path"].ToString();
            //        ObjAttachment.RefAttachmentId = dr["AttachmentId"].ToString();
            //        ObjAttachment.TransAttachmentId = dr["TransAttachmentId"].ToString();
            //        ObjAttachment.UploadDate = dr["UploadDate"].ToString();
            //        ObjAttachment.UploadBy = (dr["TitleNameTH"].ToString() + " " + dr["firstnameth"].ToString() + " " + dr["lastnameth"].ToString()).Trim();
            //        Attachments.Add(ObjAttachment);
            //    }
            //    HttpContext.Current.Session["Attachment_" + NodeName] = Attachments;
            //}
            //else
            //{
            //    Attachments = (List<ClsAttachment>)HttpContext.Current.Session["Attachment_" + NodeName];
            //}
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
                    sqlcmd = "select * from [TransNodeMultipleDetail] where TransFlowDetailId = '" + TransflowDetail + "' and TransbatchId = '" + ObjInfo.TransBatchId + "' and NodeMultipleName = '" + ObjInfo.NodeNamefrom + "' and Isdelete = 0";
                    DtMultiple = Cn.Select(sqlcmd);
                    foreach (DataRow m_dr in DtMultiple.Rows)
                    {
                        ObjFooter = new ClsFooter();
                        ObjFooter.Id = (Footers.Count + 1).ToString();
                        ObjFooter.NodeMultipleId = m_dr["Id"].ToString();
                        ObjFooter.NodeTypeNameTH = ObjInfo.NodeTypeNameTH;
                        ObjFooter.SignatureURL = m_dr["SignatureURL"].ToString();
                        ObjFooter.Remark = m_dr["RemarkFooter"].ToString();
                        ObjFooter.ActionResultValue = "";
                        ObjFooter.ActionResultNameTH = "<รอการอนุมัติ>";
                        ObjFooter.ActionById = m_dr["ActionById"].ToString();
                        ObjFooter.ActionByTitleCode = m_dr["ActionByTitleCode"].ToString();
                        ObjFooter.ActionByTitleNameTH = m_dr["ActionByTitleNameTH"].ToString();
                        ObjFooter.ActionByFirstNameTH = m_dr["ActionByfirstnameth"].ToString();
                        ObjFooter.ActionByLastNameTH = m_dr["ActionBylastnameth"].ToString();
                        ObjFooter.ActionDate = DateTime.Parse(m_dr["ActionDate"].ToString());
                        ObjFooter.ActionStringDate = "<รอการอนุมัติ>";
                        ObjFooter.ActionByPositionId = m_dr["ActionByPositionId"].ToString();
                        ObjFooter.ActionByPositionNameTH = m_dr["ActionByPositionNameTH"].ToString();
                        ObjFooter.ActionByOrganizeId = m_dr["ActionByOrganizeId"].ToString();
                        ObjFooter.ActionByOrganizeNameTH = m_dr["ActionByOrganizeNameTH"].ToString();
                        Footers.Add(ObjFooter);
                    }
                }
            }
            return Footers;
            //}
            //catch
            //{
            //    return null;
            //}
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
            DataRow[] Drs;
            Clsuser ObjMy;
            DataTable Dt = new DataTable();
            ClsEform008 ObjEform = new ClsEform008();
            string DocumentNo = ClsEngine.GenerateRunningno("EFORM008", Connectionstring, "Sys_Edocument_EFORM008", "id");
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            ObjCurrentState = ((ClsInfo)HttpContext.Current.Session["Info"]);
            ObjMy = ((Clsuser)HttpContext.Current.Session["My"]);
            Dt = new DataTable();
            Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");

            //======================================  Tranaction  =========================================
            ObjEform.Periods = new List<ClsEForm008Period>();
            ObjEform.Periods = (List<ClsEForm008Period>)HttpContext.Current.Session["Period"];
            ObjEform.Documentno = DocumentNo;
            ObjEform.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
            ObjEform.Companyname = Dt.Rows[0]["Companyname"].ToString();
            ObjEform.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
            ObjEform.Companytel = Dt.Rows[0]["Companytel"].ToString();
            ObjEform.VendorId = ClsEngine.FindValue(Dicts, "VendorId");
            ObjEform.fullname = ClsEngine.FindValue(Dicts, "Vendorname");
            ObjEform.address = ClsEngine.FindValue(Dicts, "Vendoraddress");
            ObjEform.Tel = ClsEngine.FindValue(Dicts, "Vendortel");
            ObjEform.Contactdate = ClsEngine.FindValue(Dicts, "Contactdate");
            ObjEform.Cardid = ClsEngine.FindValue(Dicts, "VendorcardID");
            ObjEform.Expirydate = ClsEngine.FindValue(Dicts, "Vendorexprirydate");
            ObjEform.Bankid = ClsEngine.FindValue(Dicts, "Vendorbank");
            ObjEform.Banknameth = ClsEngine.FindValue(Dicts, "Vendorbanktext");
            ObjEform.Bankaccountno = ClsEngine.FindValue(Dicts, "Vendorbankaccountno");
            ObjEform.Bankaccountname = ClsEngine.FindValue(Dicts, "Vendorbankaccountname");
            ObjEform.Bankaccounttype = ClsEngine.FindValue(Dicts, "Vendorbankaccountype");
            ObjEform.Sitename = ClsEngine.FindValue(Dicts, "Sitename");
            ObjEform.Sitefulladdress = ClsEngine.FindValue(Dicts, "siteaddress");
            ObjEform.Fee = ClsEngine.FindValue(Dicts, "fee");
            if (ObjEform.Fee == "")
            {
                ObjEform.Fee = "0";
            }
            ObjEform.Finisheddate = ClsEngine.FindValue(Dicts, "finisheddate");
            ObjEform.Totalamount = ClsEngine.FindValue(Dicts, "totalamount");
            if (ObjEform.Totalamount == "")
            {
                ObjEform.Totalamount = "0";
            }
            ObjEform.Contactstart = ClsEngine.FindValue(Dicts, "Contactstart");
            ObjEform.Contactend = ClsEngine.FindValue(Dicts, "Contactend");
            ObjEform.Jobdescription = ClsEngine.FindValue(Dicts, "jobdesc");

            DateTime D1 = new System.DateTime();
            DateTime D2 = new System.DateTime();
            try
            {
                D1 = (DateTime)ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactstart);
                D2 = (DateTime)ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactend);
                TimeSpan Ts = D2.Subtract(D1);
                ObjEform.Effectdate = Ts.Days.ToString();
            }
            catch
            {
                ObjEform.Effectdate = "0";
            }


            if (ObjEform.Contactstart != "" && ObjEform.Contactend != "")
            {
                D1 = (DateTime)ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactstart);
                D2 = (DateTime)ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactend);
                if (D1 > D2)
                {
                    return "วันที่เริ่มต้นต้องน้อยกว่าวันที่สิ้นสุด";
                }
            }

            if (ObjCurrentState.NodeTypeId == "1") //Current Node
            {
                //เริ่มต้น
                TransBatchId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "TransBatchId");
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id");
                TransformId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Edocument_EFORM008", "Id");


                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_Edocument_EFORM008] ";
                sqlcmd += " ([id] ";
                sqlcmd += " ,[TransFlowDetailId] ";
                sqlcmd += " ,[Transbatchid] ";
                sqlcmd += " ,[Companylogourl] ";
                sqlcmd += " ,[Companyname] ";
                sqlcmd += " ,[Companyaddress] ";
                sqlcmd += " ,[Companytel] ";
                sqlcmd += " ,[Version] ";
                sqlcmd += " ,[Documentdate] ";
                sqlcmd += " ,[Documentno] ";
                sqlcmd += " ,[VendorId] ";
                sqlcmd += " ,[fullname] ";
                sqlcmd += " ,[address] ";
                sqlcmd += " ,[Tel] ";
                sqlcmd += " ,[Cardid] ";
                sqlcmd += " ,[Expirydate] ";
                sqlcmd += " ,[Bankaccountno] ";
                sqlcmd += " ,[Bankaccounttype] ";
                sqlcmd += " ,[Bankaccountname] ";
                sqlcmd += " ,[Bankid] ";
                sqlcmd += " ,[Banknameth] ";
                if (ObjEform.Contactdate != "")
                {
                    sqlcmd += " ,[Contactdate] ";
                }
                if (ObjEform.Contactstart != "")
                {
                    sqlcmd += " ,[Contactstart] ";
                }
                if (ObjEform.Contactend != "")
                {
                    sqlcmd += " ,[Contactend] ";
                }
                sqlcmd += " ,[Effectdate] ";
                sqlcmd += " ,[Sitename] ";
                sqlcmd += " ,[Sitefulladdress] ";
                sqlcmd += " ,[Jobdescription] ";
                sqlcmd += " ,[Totalamount] ";
                if (ObjEform.Finisheddate != "")
                {
                    sqlcmd += " ,[Finisheddate] ";
                }
                sqlcmd += " ,[Fee] ";
                sqlcmd += " ,[IsDelete] ";
                sqlcmd += " ,[CreateDate] ";
                sqlcmd += " ,[CreateBy]) ";
                sqlcmd += " VALUES (";
                sqlcmd += "'" + TransformId + "'";
                sqlcmd += ",'" + TransFlowDetailId + "'";
                sqlcmd += ",'" + TransBatchId + "'";
                sqlcmd += ",'" + ObjEform.Companylogourl + "'";
                sqlcmd += ",'" + ObjEform.Companyname + "'";
                sqlcmd += ",'" + ObjEform.Companyaddress + "'";
                sqlcmd += ",'" + ObjEform.Companytel + "'";
                sqlcmd += ",'" + ObjEform.Version + "'";
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.DocumentDate) + "'";
                sqlcmd += ",'" + ObjEform.Documentno + "'";
                sqlcmd += ",'" + ObjEform.VendorId + "'";
                sqlcmd += ",'" + ObjEform.fullname + "'";
                sqlcmd += ",'" + ObjEform.address + "'";
                sqlcmd += ",'" + ObjEform.Tel + "'";
                sqlcmd += ",'" + ObjEform.Cardid + "'";
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Expirydate) + "'";
                sqlcmd += ",'" + ObjEform.Bankaccountno + "'";
                sqlcmd += ",'" + ObjEform.Bankaccounttype + "'";
                sqlcmd += ",'" + ObjEform.Bankaccountname + "'";
                sqlcmd += ",'" + ObjEform.Bankid + "'";
                sqlcmd += ",'" + ObjEform.Banknameth + "'";
                if (ObjEform.Contactdate != "")
                {

                    sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactdate) + "'";
                }
                if (ObjEform.Contactstart != "")
                {

                    sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactstart) + "'";
                }
                if (ObjEform.Contactend != "")
                {

                    sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactend) + "'";
                }

                sqlcmd += ",'" + ObjEform.Effectdate + "'";
                sqlcmd += ",'" + ObjEform.Sitename + "'";
                sqlcmd += ",'" + ObjEform.Sitefulladdress + "'";
                sqlcmd += ",'" + ObjEform.Jobdescription + "'";
                sqlcmd += ",'" + ObjEform.Totalamount.Replace(",", "") + "'";
                if (ObjEform.Finisheddate != "")
                {
                    sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Finisheddate) + "'";
                }
                sqlcmd += ",'" + ObjEform.Fee.Replace(",", "") + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",getdate()";
                sqlcmd += ",'" + ObjMy.userid + "'";
                sqlcmd += " ) ";
                Arrcmd.Add(sqlcmd);
                //Remark
                //Remark
            }
            else //ถ้ามีอยู่แล้ว
            {
                TransBatchId = ObjCurrentState.TransBatchId;
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id"); // Genarete
                TransformId = ObjCurrentState.TransFormId;
                if (ObjCurrentState.NodeNamefrom.ToLower() == "nodesingle1")
                {
                    sqlcmd = " Update Sys_EDocument_EFORM008 set ";
                    sqlcmd += " [fullname] = '" + ObjEform.fullname + "'";
                    sqlcmd += ",[address] = '" + ObjEform.address + "'";
                    sqlcmd += ",[Tel] = '" + ObjEform.Tel + "'";
                    sqlcmd += ",[Cardid] = '" + ObjEform.Cardid + "'";
                    if (ObjEform.Expirydate != "")
                    {
                        sqlcmd += ",[Expirydate] = '" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Expirydate) + "'";
                    }
                    sqlcmd += ",[Bankaccountno] = '" + ObjEform.Bankaccountno + "'";
                    sqlcmd += ",[Bankaccounttype]= '" + ObjEform.Bankaccounttype + "'";
                    sqlcmd += ",[Bankaccountname]= '" + ObjEform.Bankaccountname + "'";
                    sqlcmd += ",[Bankid] = '" + ObjEform.Bankid + "'";
                    sqlcmd += ",[Banknameth] = '" + ObjEform.Banknameth + "'";
                    if (ObjEform.Contactdate != "")
                    {
                        sqlcmd += ",[Contactdate] = '" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactdate) + "'";
                    }
                    if (ObjEform.Contactstart != "")
                    {
                        sqlcmd += ",[Contactstart] = '" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactstart) + "'";
                    }
                    if (ObjEform.Contactend != "")
                    {
                        sqlcmd += ",[Contactend] = '" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactend) + "'";
                    }
                    sqlcmd += ",[Effectdate]= '" + ObjEform.Effectdate + "'";
                    sqlcmd += ",[Sitename] = '" + ObjEform.Sitename + "'";
                    sqlcmd += ",[Sitefulladdress]= '" + ObjEform.Sitefulladdress + "'";
                    sqlcmd += ",[Jobdescription]= '" + ObjEform.Jobdescription + "'";
                    sqlcmd += ",[Totalamount] = '" + ObjEform.Totalamount.Replace(",", "") + "'";
                    if (ObjEform.Finisheddate != "")
                    {
                        sqlcmd += ",[Finisheddate] = '" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Finisheddate) + "'";
                    }
                    sqlcmd += ",[Fee] = '" + ObjEform.Fee.Replace(",", "") + "'";
                    sqlcmd += " Where id = '" + TransformId + "'";
                    Arrcmd.Add(sqlcmd);
                }
            }
            sqlcmd = "Update Sys_EDocument_EFORM008detail set isdelete =1,deletedate=getdate(),deleteby='" + ObjMy.userid + "'  Where TransBatchId ='" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            string _detailid = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_EDocument_EFORM008detail", "Id");
            foreach (ClsEForm008Period period in ObjEform.Periods)
            {
                if (period.Amount == "")
                {
                    period.Amount = "0.00";
                }
                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_EDocument_EFORM008detail] ";
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
                sqlcmd += ",'" + period.Amount.Replace(",", "") + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",getdate()";
                sqlcmd += ",'" + ObjMy.userid + "'";
                sqlcmd += " ) ";
                Arrcmd.Add(sqlcmd);
                _detailid = (int.Parse(_detailid) + 1).ToString();
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
            foreach (ClsAttachment _attch in ((ClsEform008)(HttpContext.Current.Session["EFORM008"])).Attachments)
            {
                count += 1;
                if (count < ((ClsEform008)(HttpContext.Current.Session["EFORM008"])).Attachments.Count)
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
            Attachs = ((ClsEform008)(HttpContext.Current.Session["EFORM008"])).Attachments;

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
            //Validate before final save
            if (NodeTypeIdOut == "2")
            {
                double totalamount = 0;
                if (ObjEform.fullname == "")
                {
                    return "โปรดระบุชื่อลูกค้า";
                }
                if (ObjEform.address == "")
                {
                    return "โปรดระบุที่อยู่ลูกค้า";
                }
                if (ObjEform.Tel == "")
                {
                    return "โปรดระบุเบอร์โทรศัพท์ลูกค้า";
                }
                if (ObjEform.Cardid == "")
                {
                    return "โปรดระบุเลขที่บัตรประชาชน / เลขที่นิติบุคคลลูกค้า";
                }
                if (ObjEform.Expirydate == "")
                {
                    return "โปรดระบุวันที่หมดอายุบัตรประชาชน / วันหมดอายุเลขที่นิติบุคคลลูกค้า";
                }
                if (ObjEform.Bankaccountno == "")
                {
                    return "โปรดระบุเลขที่บัญชีลูกค้า";
                }
                if (ObjEform.Bankaccounttype == "")
                {
                    return "โปรดระบุประเภทบัญชีลูกค้า";
                }
                if (ObjEform.Bankaccountname == "")
                {
                    return "โปรดระบุชื่อบัญชีลูกค้า";
                }
                if (ObjEform.Banknameth == "")
                {
                    return "โปรดระบุธนาคารเจ้าของบัญชีลูกค้า";
                }
                if (ObjEform.Contactdate == "")
                {
                    return "โปรดระบุวันที่ในสัญญา";
                }
                if (ObjEform.Contactstart == "")
                {
                    return "โปรดระบุวันที่เริ่มสัญญา";
                }
                if (ObjEform.Contactend == "")
                {
                    return "โปรดระบุวันที่สิ้นสุดสัญญา";
                }
                if (ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactstart) > ClsEngine.ConvertDateforSavingDatabase(ObjEform.Contactend))
                {
                    return "วันที่สิ้นสุดสัญญาต้องมากกว่าวันที่เริ่มสัญญา";
                }
                if (ObjEform.Sitename == "")
                {
                    return "โปรดระบุชื่อไซต์งาน";
                }
                if (ObjEform.Sitefulladdress == "")
                {
                    return "โปรดระบุที่อยู่ไซต์งาน";
                }
                if (ObjEform.Jobdescription == "")
                {
                    return "โปรดระบุรายละเอียดของงานที่ต้องทำในไซต์งาน";
                }
                if (ObjEform.Totalamount == "")
                {
                    return "โปรดระบุจำนวนเงิน";
                }
                try
                {
                    totalamount = double.Parse(ObjEform.Totalamount);
                    if (totalamount <= 0)
                    {
                        return "จำนวนเงินต้องมากกว่า 9";
                    }
                }

                catch
                {
                    return "จำนวนเงินต้องเป็นตัวเลข";
                }
                if (ObjEform.Finisheddate == "")
                {
                    return "โปรดระบุวันที่แล้วเสร็จ";
                }
                double periodtotalamount = 0;
                double _periodamount = 0;
                foreach (ClsEForm008Period period in ObjEform.Periods)
                {
                    if (period.Periodname == "")
                    {
                        return "ตรวจพบงวดงานเป็นค่าว่าง โปรดตรวจสอบ";
                    }
                    if (period.Amount == "")
                    {
                        return "โปรดระบุจำนวนเงิน";
                    }
                    try
                    {
                        _periodamount = double.Parse(period.Amount);
                        if (_periodamount < 0) //เป็น 0 ได้เผื่อเป็นส่วนลด
                        {
                            return "จำนวนเงินต้องมากกว่า 0";
                        }
                        periodtotalamount += _periodamount;
                    }
                    catch
                    {
                        return "ตรวจพบจำนวนเงินในงวดงานเป็นค่าว่างหรือไม่ใช่ตัวเลข โปรดตรวจสอบ";
                    }
                }
                if (periodtotalamount != totalamount)
                {
                    return "จำนวนเงินรวมของงวดงานกับจำนวนเงินในโครงการไม่ตรงกัน โปรดตรวจสอบ";
                }
                //ตรวจ Detail

            }
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
            ////Cancel
            //sqlcmd = "Update Sys_Trans_Flow Set ";
            //sqlcmd += " [PendingDate] ='" + System.DateTime.Now + "'";
            //sqlcmd += ",[PendingBy] =''";
            //sqlcmd += ",[PendingByNodeName] ='" + NodeNameOut + "'";
            //sqlcmd += ",[PendingByNodeTypeId] ='" + NodeTypeIdOut + "'";
            //sqlcmd += ",[PendingByNodeTypeNameTH]='" + NodeTypeNameTHOut + "'";
            //sqlcmd += ",[PendingByType] ='U'";
            //sqlcmd += ",[CompleteDate]='" + System.DateTime.Now + "'";
            //sqlcmd += ",[IsComplete] = 1";
            //sqlcmd += ",[CompleteBy] = '" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
            //sqlcmd += ",[CompleteByType] = 'U'";
            //sqlcmd += " Where TransBatchId = '" + TransBatchId + "'";
            //Arrcmd.Add(sqlcmd);
            ////Cancel
            //======================================  FLOW =========================================
            Cn.Execute(Arrcmd, null);
            Cn.Close();
            return "";
            //return ObjCurrentState.NodeTypeId;
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
        public static ClsGridResponse Bind(string Ctrl, long PagePerItem, long CurrentPage, string SortName, string Order, string Column, string Data, string Initial, string SelectCat, string SearchMsg, string EditButton, string DeleteButton, string Panel, string FullRowSelect, string Multiselect, string Criteria, string SearchesDat, string Searchcolumns, string WPanel, string HPanel)
        {
            string Sqlcmd = "";
            string PK = "";
            List<ClsDict> CriterialMapping = new List<ClsDict>();
            ClsGrid Objgrid = new ClsGrid();
            Clsuser Objmy = (Clsuser)HttpContext.Current.Session["My"];
            if (Ctrl == "GvVendor")
            {
                PK = "id";
                Sqlcmd = "Select id,fullname,address from Sys_Master_VendorOutsource where isdelete = 0";
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
        public static string ValidateVendor(string json)
        {
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            List<ClsDict> Dicts = new List<ClsDict>();
            DataTable Dt = new DataTable();
            string sqlcmd = "";
            string msg = "";
            string fullname = "";
            string address = "";
            Clsuser Objmy = new Clsuser();
            try
            {
                //json += 'TxtnewVendorname :' + $('#TxtnewVendorname').val() + '|';
                //json += 'TxtnewVendoraddress :' + $('#TxtnewVendoraddress').val() + '|';
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                fullname = ClsEngine.FindValue(Dicts, "TxtnewVendorname");
                address = ClsEngine.FindValue(Dicts, "TxtnewVendoraddress");

                sqlcmd = "Select * from Sys_Master_VendorOutsource where isdelete  = 0 and Fullname ='" + fullname + "'";
                if (cn.Select(sqlcmd).Rows.Count > 0)
                {
                    return "!E" + fullname + " is dupplicated ";
                }
                else
                {
                    sqlcmd = "Select * from Sys_Master_VendorOutsource where isdelete  = 0 and Fullname  like '%" + fullname + "%'";
                    Dt = cn.Select(sqlcmd);
                    if (Dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Dt.Rows)
                        {
                            msg += dr["fullname"].ToString() + " ";
                        }
                        return "!W" + "Vendor may be dupplicate " + msg + ". Do you want to add ?";
                    }
                    else
                    {
                        return "";
                    }
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                cn.Close();
            }
        }
        [WebMethod]
        public static List<ClsVendor> Selvendor(string json)
        {
            ClsLH Obj = new ClsLH();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            try
            {
                return Obj.GetVendoroutsource(ref cn, ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x"));
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
        public static string SaveVendor(string json)
        {
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            List<ClsDict> Dicts = new List<ClsDict>();

            string sqlcmd = "";
            string id = "";
            string code = "";
            string fullname = "";
            string address = "";
            string tel = "";
            string cardId = "";
            string exprirydate = "";
            string bank = "";
            string accountname = "";
            string accountno = "";
            string accounttype = "";
            Clsuser Objmy = new Clsuser();
            try
            {
                //json += 'TxtnewVendorname :' + $('#TxtnewVendorname').val() + '|';
                //json += 'TxtnewVendoraddress :' + $('#TxtnewVendoraddress').val() + '|';
                //json += 'TxtnewVendortel :' + $('#TxtnewVendortel').val() + '|';
                //json += 'TxtnewVendorcardID :' + $('#TxtnewVendorcardID').val() + '|';
                //json += 'TxtnewVendorexprirydate :' + $('#TxtnewVendorexprirydate').val() + '|';
                //json += 'CbnewVendorbank :' + $('#CbnewVendorbank').val() + '|';
                //json += 'TxtnewVendorbankaccountname :' + $('#TxtnewVendorbankaccountname').val() + '|';
                //json += 'TxtnewVendorbankaccountno :' + $('#TxtnewVendorbankaccountno').val() + '|';
                //json += 'CbnewVendorbankaccountype :' + $('#CbnewVendorbankaccountype').val() + '|';
                id = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Master_VendorOutsource", "Id");
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                code = "vendor-" + id.ToString().PadLeft(7, '0');
                fullname = ClsEngine.FindValue(Dicts, "TxtnewVendorname");
                address = ClsEngine.FindValue(Dicts, "TxtnewVendoraddress");
                tel = ClsEngine.FindValue(Dicts, "TxtnewVendortel");

                cardId = ClsEngine.FindValue(Dicts, "TxtnewVendorcardID");
                exprirydate = ClsEngine.FindValue(Dicts, "TxtnewVendorexprirydate");
                bank = ClsEngine.FindValue(Dicts, "CbnewVendorbank");
                accountname = ClsEngine.FindValue(Dicts, "TxtnewVendorbankaccountname");
                accountno = ClsEngine.FindValue(Dicts, "TxtnewVendorbankaccountno");
                accounttype = ClsEngine.FindValue(Dicts, "CbnewVendorbankaccounttype");


                Objmy = (Clsuser)HttpContext.Current.Session["My"];
                sqlcmd += " INSERT INTO [Sys_Master_VendorOutsource] ";
                sqlcmd += "([Id] ";
                sqlcmd += ",[Fullname] ";
                sqlcmd += ",[Address] ";
                sqlcmd += ",[Tel] ";
                sqlcmd += ",[Bankid] ";
                sqlcmd += ",[Bankaccountno] ";
                sqlcmd += ",[Bankaccountname] ";
                sqlcmd += ",[Bankaccounttype] ";
                sqlcmd += ",[IDCard] ";
                sqlcmd += ",[CardExprirydate] ";
                sqlcmd += ",[IsDelete] ";
                sqlcmd += ",[CreateDate] ";
                sqlcmd += ",[CreateBy] )  ";
                sqlcmd += " VALUES  ";
                sqlcmd += " (  ";
                sqlcmd += "'" + id + "'";

                sqlcmd += ",'" + fullname + "'";
                sqlcmd += ",'" + address + "'";
                sqlcmd += ",'" + tel + "'";
                sqlcmd += ",'" + bank + "'";
                sqlcmd += ",'" + accountno + "'";
                sqlcmd += ",'" + accountname + "'";
                sqlcmd += ",'" + accounttype + "'";
                sqlcmd += ",'" + cardId + "'";
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(exprirydate) + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",'" + System.DateTime.Now + "'";
                sqlcmd += ",'" + Objmy.userid + "'";
                sqlcmd += ")";
                cn.Execute(sqlcmd, null);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                cn.Close();
            }
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