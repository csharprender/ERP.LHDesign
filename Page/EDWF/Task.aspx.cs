using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using ERP.LHDesign2020.Class;
using System.Configuration;
using System.IO;
using SVframework2016;
namespace ERP.LHDesign2020.Page.EDWF
{
    public partial class Task : System.Web.UI.Page
    {
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            HdAction.Value = Request["Action"].ToString();
        }
        [WebMethod]
        public static List<ClsNodeInFlowItem> GetNodeInFlow(string json)
        {
            List<ClsNodeInFlowItem> Dicts = new List<ClsNodeInFlowItem>();
            string TransBatchId = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "TransBatchId");
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            int count = 0;
            DataTable Dt = new DataTable();
            DataTable DtMasNodeType = new DataTable();
            DataTable DtMasUser = new DataTable();
            ClsNodeInFlowItem ObjDict;
            string sqlcmd = "SELECT * FROM [Sys_Trans_Flowdetail] Where TransBatchId = '" + TransBatchId + "' and Isdelete = 0 Order by id";
            Dt = Cn.Select(sqlcmd);
            sqlcmd = "select * from [Sys_Master_Nodetype] where Isdelete = 0 ";
            DtMasNodeType = Cn.Select(sqlcmd);
            sqlcmd = "select * from [Sys_Master_UserProfile] where Isdelete = 0 ";
            DtMasUser = Cn.Select(sqlcmd);
            Cn.Close();
            string[] Users;
            DataRow[] Drs;
            Clsuser ObjUsr;
            List<Clsuser> Usrs;
            int c = 0;
            foreach (DataRow dr in Dt.Rows)
            {
                count += 1;
                c = 0;
                ObjDict = new ClsNodeInFlowItem();
                Usrs = new List<Clsuser>();
                ObjDict.No = count.ToString();
                ObjDict.NodeNameTH = dr["NodeNamein"].ToString();
                ObjDict.NodeTypeNameTH = dr["NodeTypeNameInTH"].ToString();
                ObjDict.NodePicURL = DtMasNodeType.Select("Nodename = '" + ObjDict.NodeTypeNameTH + "'")[0]["NodeTypeAvartar"].ToString();
                Users = dr["UserIdIn"].ToString().Split(',');
                ObjDict.FullName = "";
                foreach (string usr in Users)
                {
                    c += 1;
                    ObjUsr = new Clsuser();
                    Drs = DtMasUser.Select("Id=" + usr.Replace("|", ""));
                    ObjUsr.userid = Drs[0]["Id"].ToString();
                    ObjUsr.Titlenameth = Drs[0]["TitleNameTH"].ToString();
                    ObjUsr.firstnameth = Drs[0]["FirstNameTH"].ToString();
                    ObjUsr.lastnameth = Drs[0]["LastNameTH"].ToString();
                    ObjDict.FullName += ObjUsr.Titlenameth + "  " + ObjUsr.firstnameth + "  " + ObjUsr.lastnameth;
                    if (c != Users.Length)
                    {
                        ObjDict.FullName += ",";
                    }
                    Usrs.Add(ObjUsr);
                }
                ObjDict.Users = Usrs;
                Dicts.Add(ObjDict);
                c = 0;
                if (dr["IsFinished"].ToString() == "1")
                {
                    count += 1;
                    ObjDict = new ClsNodeInFlowItem();
                    Usrs = new List<Clsuser>();
                    ObjDict.No = count.ToString();
                    ObjDict.NodeNameTH = dr["NodeNameOut"].ToString();
                    ObjDict.NodeTypeNameTH = dr["NodeTypeNameOutTH"].ToString();
                    ObjDict.NodePicURL = DtMasNodeType.Select("Nodename = '" + ObjDict.NodeTypeNameTH + "'")[0]["NodeTypeAvartar"].ToString();
                    ObjDict.Users = Usrs;
                    ObjDict.FullName = "";
                    Dicts.Add(ObjDict);
                }
                else if (Dt.Rows.Count == count)
                {
                    count += 1;
                    ObjDict = new ClsNodeInFlowItem();
                    Usrs = new List<Clsuser>();
                    ObjDict.No = count.ToString();
                    ObjDict.NodeNameTH = dr["NodeNameOut"].ToString();
                    ObjDict.NodeTypeNameTH = dr["NodeTypeNameOutTH"].ToString();
                    ObjDict.NodePicURL = DtMasNodeType.Select("Nodename = '" + ObjDict.NodeTypeNameTH + "'")[0]["NodeTypeAvartar"].ToString();
                    Users = dr["UserIdOut"].ToString().Split(',');
                    ObjDict.FullName = "";
                    foreach (string usr in Users)
                    {
                        c += 1;
                        ObjUsr = new Clsuser();
                        Drs = DtMasUser.Select("Id=" + usr.Replace("|", ""));
                        ObjUsr.userid = Drs[0]["Id"].ToString();
                        ObjUsr.Titlenameth = Drs[0]["TitleNameTH"].ToString();
                        ObjUsr.firstnameth = Drs[0]["FirstNameTH"].ToString();
                        ObjUsr.lastnameth = Drs[0]["LastNameTH"].ToString();
                        ObjDict.FullName += ObjUsr.Titlenameth + "  " + ObjUsr.firstnameth + "  " + ObjUsr.lastnameth;
                        if (c != Users.Length)
                        {
                            ObjDict.FullName += ",";
                        }
                        Usrs.Add(ObjUsr);
                    }
                    ObjDict.Users = Usrs;
                    Dicts.Add(ObjDict);
                }
            }
            return Dicts;
        }
        [WebMethod]
        public static List<ClsPendingInfo> GetPending(string json)
        {
            List<ClsPendingInfo> Pendings = new List<ClsPendingInfo>();
            ClsPendingInfo ObjPending;
            string UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            DataTable Dt = new DataTable();
            string sqlcmd = "Select TF.TransBatchId,TF.Subject ,convert(NVARCHAR,TF.CreateDate,103) as CreateDate,isnull(up.FirstNameTH,'') + ' ' + isnull(up.LastNameTH,'') as CreateBy ,'Pending' as [Status] ,'<a onClick=FlowInfo(''' + cast(TF.TransBatchId as nvarchar) + ''')>ดูรายละเอียด</a>' as FlowInfo ,'<a onClick=Work(''' + cast(TF.TransBatchId as nvarchar) + ''')>กดเพื่อเข้าทำงาน</a>' as Work from [Sys_Trans_Flow] TF  left join Sys_Master_Userprofile up on TF.CreateBy = up.id where TF.isdelete = 0 and isnull(TF.iscomplete,0) = 0 and (TF.PendingBy like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%' or TF.PendingBy = '" + UserId + "')"; //and TF.PendingBy like (" + ClsEngine.ConvertInt2nvarchar(UserId,',') + ")";
            string inqValAll = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "inq");
            if (inqValAll != "")
            {
                sqlcmd += " and ( ";
                sqlcmd += " (TF.Subject like '%" + inqValAll + "%')";
                sqlcmd += "  Or (convert(NVARCHAR,TF.CreateDate,103) like '%" + inqValAll + "%')";
                sqlcmd += "  Or (isnull(Sys_Master_User.TitleNameTH,'') + ' '  + isnull(Sys_Master_User.FirstNameTH,'') + ' ' + isnull(Sys_Master_User.LastNameTH,'') like '%" + inqValAll + "%')";
                sqlcmd += " ) ";
            }
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            foreach (DataRow dr in Dt.Rows)
            {
                ObjPending = new ClsPendingInfo();
                ObjPending.TransBatchId = dr["TransBatchId"].ToString();
                ObjPending.Subject = dr["Subject"].ToString();
                ObjPending.Status = dr["Status"].ToString();
                ObjPending.DocumentDate = dr["CreateDate"].ToString();
                ObjPending.DocumentBy = dr["CreateBy"].ToString();
                ObjPending.FlowInfo = dr["FlowInfo"].ToString();
                ObjPending.Work = dr["Work"].ToString();
                Pendings.Add(ObjPending);
            }
            return Pendings;
        }
        [WebMethod]
        public static List<ClsInvolveInfo> GetInvolve(string json)
        {
            List<ClsInvolveInfo> Involvees = new List<ClsInvolveInfo>();
            ClsInvolveInfo ObjInvolve;
            string UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            DataTable Dt = new DataTable();
            string sqlcmd = " Select distinct TF.TransBatchId,TF.Subject ,convert(NVARCHAR,TF.CreateDate,103) as CreateDate ,isnull(up.FirstNameTH,'') + ' ' + isnull(up.LastNameTH,'') as CreateBy ,'Inprocess' as [Status] ,'<a onClick=FlowInfo(''' + cast(TF.TransBatchId as nvarchar) + ''')>ดูรายละเอียด</a>' as FlowInfo ,'<a onClick=Inq(''' + cast(TF.TransBatchId as nvarchar) + ''')>กดเพื่อดูรายละเอียด</a>' as Work  from [Sys_Trans_Flow] TF   left join Sys_Master_Userprofile as up on TF.CreateBy = up.id   left join [Sys_Trans_Flowdetail] TFD on TF.TransBatchId = TFD.TransBatchId where (UserIdin = '" + UserId + "' or  (UserIdin like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%')) and not (TF.PendingBy like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%' or TF.PendingBy = '" + UserId + "')  and isnull(TF.IsComplete,0) = 0 and isnull(TF.Iscancel,0) = 0";
            string inqValAll = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "inq");
            if (inqValAll != "")
            {
                sqlcmd += " and ( ";
                sqlcmd += " (TF.Subject like '%" + inqValAll + "%')";
                sqlcmd += "  Or (convert(NVARCHAR,TF.CreateDate,103) like '%" + inqValAll + "%')";
                sqlcmd += "  Or (isnull(Sys_Master_User.TitleNameTH,'') + ' '  + isnull(Sys_Master_User.FirstNameTH,'') + ' ' + isnull(Sys_Master_User.LastNameTH,'') like '%" + inqValAll + "%')";
                sqlcmd += " ) ";
            }
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            foreach (DataRow dr in Dt.Rows)
            {
                ObjInvolve = new ClsInvolveInfo();
                ObjInvolve.TransBatchId = dr["TransBatchId"].ToString();
                ObjInvolve.Subject = dr["Subject"].ToString();
                ObjInvolve.DocumentDate = dr["CreateDate"].ToString();
                ObjInvolve.DocumentBy = dr["CreateBy"].ToString();
                ObjInvolve.FlowInfo = dr["FlowInfo"].ToString();
                ObjInvolve.Status = dr["Status"].ToString();
                ObjInvolve.Work = dr["Work"].ToString();
                Involvees.Add(ObjInvolve);
            }
            return Involvees;
        }
        [WebMethod]
        public static List<ClsCompleteInfo> GetComplete(string json)
        {

            List<ClsCompleteInfo> Completes = new List<ClsCompleteInfo>();
            ClsCompleteInfo ObjComplete;
            string UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            DataTable Dt = new DataTable();
            string sqlcmd = " Select distinct TF.TransBatchId,TF.Subject ,convert(NVARCHAR,TF.CreateDate,103) as CreateDate ,isnull(Up.TitleNameTH,'') + ' '  + isnull(Up.FirstNameTH,'') + ' ' + isnull(Up.LastNameTH,'') as CreateBy ,'Complete' as [Status] ,'<a onClick=FlowInfo(''' + cast(TF.TransBatchId as nvarchar) + ''')>ดูรายละเอียด</a>' as FlowInfo ,'<a onClick=Inq(''' + cast(TF.TransBatchId as nvarchar) + ''')>กดเพื่อดูรายละเอียด</a>' as Work  from [Sys_Trans_Flow] TF  left join Sys_Master_Userprofile Up on TF.CreateBy = Up.id  left join [Sys_Trans_Flowdetail] TFD on TF.TransBatchId = TFD.TransBatchId where ((UserIdin = '" + UserId + "' or  (UserIdin like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%'))) and TF.Iscomplete = 1 ";
            string inqValAll = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "inq");
            if (inqValAll != "")
            {
                sqlcmd += " and ( ";
                sqlcmd += " (TF.Subject like '%" + inqValAll + "%')";
                sqlcmd += "  Or (convert(NVARCHAR,TF.CreateDate,103) like '%" + inqValAll + "%')";
                sqlcmd += "  Or (isnull(Sys_Master_User.TitleNameTH,'') + ' '  + isnull(Sys_Master_User.FirstNameTH,'') + ' ' + isnull(Sys_Master_User.LastNameTH,'') like '%" + inqValAll + "%')";
                sqlcmd += " ) ";
            }
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            foreach (DataRow dr in Dt.Rows)
            {
                ObjComplete = new ClsCompleteInfo();
                ObjComplete.TransBatchId = dr["TransBatchId"].ToString();
                ObjComplete.Subject = dr["Subject"].ToString();
                ObjComplete.DocumentDate = dr["CreateDate"].ToString();
                ObjComplete.DocumentBy = dr["CreateBy"].ToString();
                ObjComplete.FlowInfo = dr["FlowInfo"].ToString();
                ObjComplete.Status = dr["Status"].ToString();
                ObjComplete.Work = dr["Work"].ToString();
                Completes.Add(ObjComplete);
            }
            return Completes;
        }
        [WebMethod]
        public static List<ClsCancelInfo> GetCancel(string json)
        {
            List<ClsCancelInfo> Cancels = new List<ClsCancelInfo>();
            ClsCancelInfo ObjCancel;
            string UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            DataTable Dt = new DataTable();
            string sqlcmd = "select distinct TF.TransBatchId,TF.Subject ,convert(NVARCHAR,TF.CreateDate,103) as CreateDate ,isnull(up.FirstNameTH,'') + ' ' + isnull(up.LastNameTH,'') as CreateBy ,'Cancel' as [Status] ,'<a onClick=FlowInfo(''' + cast(TF.TransBatchId as nvarchar) + ''')>ดูรายละเอียด</a>' as FlowInfo ,'<a onClick=Inq(''' + cast(TF.TransBatchId as nvarchar) + ''')>กดเพื่อดูรายละเอียด</a>' as Work  from [Sys_Trans_Flow] TF   left join Sys_Master_Userprofile up  on TF.CreateBy = up.id  left join [Sys_Trans_Flowdetail] TFD on TF.TransBatchId = TFD.TransBatchId where ((UserIdin = '" + UserId + "' or  (UserIdin like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%'))) and TF.Iscancel = 1 ";
            string inqValAll = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "inq");
            if (inqValAll != "")
            {
                sqlcmd += " and ( ";
                sqlcmd += " (TF.Subject like '%" + inqValAll + "%')";
                sqlcmd += "  Or (convert(NVARCHAR,TF.CreateDate,103) like '%" + inqValAll + "%')";
                sqlcmd += "  Or (isnull(Sys_Master_User.TitleNameTH,'') + ' '  + isnull(Sys_Master_User.FirstNameTH,'') + ' ' + isnull(Sys_Master_User.LastNameTH,'') like '%" + inqValAll + "%')";
                sqlcmd += " ) ";
            }
            //sqlcmd += " where ((UserIdin = '" + UserId + "' or  (UserIdin like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%')) Or (UserIdOut = '" + UserId + "' or  (UserIdOut like '%" + ClsEngine.ConvertInt2nvarchar(UserId, ',') + "%'))) and TF.Iscancel = 1 ";
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            foreach (DataRow dr in Dt.Rows)
            {
                ObjCancel = new ClsCancelInfo();
                ObjCancel.TransBatchId = dr["TransBatchId"].ToString();
                ObjCancel.Subject = dr["Subject"].ToString();
                ObjCancel.DocumentDate = dr["CreateDate"].ToString();
                ObjCancel.DocumentBy = dr["CreateBy"].ToString();
                ObjCancel.FlowInfo = dr["FlowInfo"].ToString();
                ObjCancel.Status = dr["Status"].ToString();
                ObjCancel.Work = dr["Work"].ToString();
                Cancels.Add(ObjCancel);
            }
            return Cancels;
        }



        [WebMethod]
        public static ClsDocumentInfo Work(string json)
        {
            ClsDocumentInfo ObjDocument = new ClsDocumentInfo();
            string sqlcmd = "";
            DataTable DtSys_Trans_Flow = new DataTable();
            string TransBatchId = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "TransBatchId");
            string Dat = "";
            string NodeNamefrom = "";
            string NodeTypeIdfrom = "";
            string NodeTypeNameTHFrom = "";
            string FormId = "";
            string FormNameTH = "";
            string FormURL = "";
            string FormDescTH = "";
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            string UserId = "";
            try
            {
                UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
                sqlcmd = "select * from [Sys_Trans_Flow] TF left join Sys_Master_Form MF on TF.FormId =  MF.FormId Where TransBatchId = '" + TransBatchId + "' and TF.Isdelete =0";
                DtSys_Trans_Flow = Cn.Select(sqlcmd);
                NodeNamefrom = DtSys_Trans_Flow.Rows[0]["PendingByNodeName"].ToString();
                NodeTypeIdfrom = DtSys_Trans_Flow.Rows[0]["PendingByNodeTypeId"].ToString();
                NodeTypeNameTHFrom = DtSys_Trans_Flow.Rows[0]["PendingByNodeTypeNameTH"].ToString();
                FormId = DtSys_Trans_Flow.Rows[0]["FormId"].ToString();
                FormNameTH = DtSys_Trans_Flow.Rows[0]["FormNameTH"].ToString();
                FormURL = DtSys_Trans_Flow.Rows[0]["FormURL"].ToString();
                FormDescTH = DtSys_Trans_Flow.Rows[0]["FormDescTH"].ToString();
                Cn.Close();
                Dat = "TransBatchId:" + TransBatchId + "|FormId:" + FormId + "|FormNameTH:" + FormNameTH + "|NodeNamefrom:" + NodeNamefrom + "|NodeTypeIdfrom:" + NodeTypeIdfrom + "|NodeTypeNameTHFrom:" + NodeTypeNameTHFrom + "|";
                Dat = ClsEngine.Base64Encode(Dat);
                ObjDocument = new ClsDocumentInfo();
                ObjDocument.FormId = FormId;
                ObjDocument.FormNameTH = FormNameTH;
                ObjDocument.FormDescTH = FormDescTH;
                ObjDocument.NodeName = NodeNamefrom;
                ObjDocument.FormURL = FormURL + "?Msg=" + Dat;
                ObjDocument.Mode = "W";
                return ObjDocument;
            }
            catch
            {
                return null;
            }
            finally
            {
                DtSys_Trans_Flow.Dispose();

            }
        }
        [WebMethod]
        public static ClsDocumentInfo Inq(string json)
        {
            ClsDocumentInfo ObjDocument = new ClsDocumentInfo();
            string sqlcmd = "";
            DataTable DtSys_Trans_Flow = new DataTable();
            string TransBatchId = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', ','), "TransBatchId");
            string Dat = "";
            string NodeNamefrom = "";
            string NodeTypeIdfrom = "";
            string NodeTypeNameTHFrom = "";
            string FormId = "";
            string FormNameTH = "";
            string FormURL = "";
            string FormDescTH = "";
            string Status = "";
            SqlConnector Cn = new SqlConnector(Connectionstring,"");
            string UserId = "";
            try
            {
                UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
                sqlcmd = "select * from [Sys_Trans_Flow] TF left join Sys_Master_Form MF on TF.FormId =  MF.FormId Where TransBatchId = '" + TransBatchId + "' and TF.Isdelete =0";
                DtSys_Trans_Flow = Cn.Select(sqlcmd);
                NodeNamefrom = ""; //ไม่ต้องใช้
                NodeTypeIdfrom = ""; //ไม่ต้องใช้
                NodeTypeNameTHFrom = ""; //ไม่ต้องใช้
                FormId = DtSys_Trans_Flow.Rows[0]["FormId"].ToString();
                FormNameTH = DtSys_Trans_Flow.Rows[0]["FormNameTH"].ToString();
                FormURL = DtSys_Trans_Flow.Rows[0]["FormURL"].ToString();
                FormDescTH = DtSys_Trans_Flow.Rows[0]["FormDescTH"].ToString();
                if (DtSys_Trans_Flow.Rows[0]["IsComplete"].ToString() == "1")
                {
                    Status = "CP";
                }
                else if (DtSys_Trans_Flow.Rows[0]["IsCancel"].ToString() == "1")
                {
                    Status = "CA";
                }
                Cn.Close();
                Dat = "INQ:1|TransBatchId:" + TransBatchId + "|FormId:" + FormId + "|FormNameTH:" + FormNameTH + "|NodeNamefrom:" + NodeNamefrom + "|NodeTypeIdfrom:" + NodeTypeIdfrom + "|NodeTypeNameTHFrom:" + NodeTypeNameTHFrom + "|";
                Dat = ClsEngine.Base64Encode(Dat);
                ObjDocument = new ClsDocumentInfo();
                ObjDocument.FormId = FormId;
                ObjDocument.FormNameTH = FormNameTH;
                ObjDocument.FormDescTH = FormDescTH;
                ObjDocument.NodeName = NodeNamefrom;
                ObjDocument.FormURL = FormURL + "?Msg=" + Dat;
                ObjDocument.Mode = "I";
                return ObjDocument;
            }
            catch
            {
                return null;
            }
            finally
            {
                DtSys_Trans_Flow.Dispose();

            }
        }
    }
}