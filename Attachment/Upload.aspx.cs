using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using ERP.LHDesign2020.Class;
using SVframework2016;
using System.Data;
namespace ERP.LHDesign2020.Attachment
{
    public partial class Upload : System.Web.UI.Page
    {
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Key"] != null)
            {
                HdKey.Value = Request["Key"];
            }
        }
        protected void CmdFileUpload_Click(object sender, EventArgs e)
        {
            SqlConnector _Cn = new SqlConnector(Connectionstring,null);
            string AttachmentPath = "";
            string Destfilename = "";
            string Extension;
            SVFrameWork.Datetime Svdatetime = new SVFrameWork.Datetime();
            string SessionId = HttpContext.Current.Session.SessionID;
            DataTable Dt = new DataTable();
            string Filename = FileUpd.PostedFile.FileName;
            string Extenstion = FileUpd.PostedFile.ContentType;
            string sqlcmd = "";
            string RunningNo = "";
            if (FileUpd.PostedFile.ContentLength > 0)
            {
                Extension = new FileInfo(FileUpd.PostedFile.FileName).Extension;

                AttachmentPath = ClsEngine.Getconfigurationvalue(ref _Cn,  "AttachmentPath", "Attachment");
                if (AttachmentPath.ToString().Trim().Substring(AttachmentPath.Length - 1, 1) != "/")
                {
                    AttachmentPath += "/";
                }
                else
                {
                    AttachmentPath = AttachmentPath.Trim();
                }
                _Cn.ObjectCommand = new System.Data.SqlClient.SqlCommand();
                Destfilename = SessionId + Svdatetime.Getcurrentdateyyyymmdd() + Svdatetime.GetCurrentHHMMSS() + Extension;
                FileUpd.PostedFile.SaveAs(AttachmentPath + Destfilename);
                RunningNo = ClsEngine.GenerateRunningId(_Cn.Connectionstring, "Sys_trans_attachment", "Id");
                sqlcmd = "INSERT INTO [Sys_Trans_Attachment] ([Id],[FileName],[Extension],Sizebyte,[Path],Isdelete,Createdate,Createby) VALUES('" + RunningNo + "','" + FileUpd.PostedFile.FileName + "','" + Extension + "','" + FileUpd.PostedFile.ContentLength + "','" + AttachmentPath + Destfilename + "','0',getdate(),'" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "')";
                _Cn.Execute(sqlcmd,null);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "CallBack('" + HdKey.Value + "','" + RunningNo + "')", true);
            }
        }
    }
}