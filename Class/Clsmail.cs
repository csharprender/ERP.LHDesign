using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net;
using System.Net.Mail;
using SVframework2016;
namespace ERP.LHDesign2020.Class
{
    public class ClsMail
    {
        private SVframework2016.SqlConnector _Cn;
        private string _Connectionstring = "";
        public ClsMail(string Connectionstring)
        {
            _Connectionstring = Connectionstring;
            _Cn = new SVframework2016.SqlConnector(_Connectionstring, null);
        }
        public string SendMail(string Mailto, string TemplatedId, string RefId, string Module, string SendBy, List<ClsDictExtend> Dats, MailAddressCollection CC)
        {
            DataTable DtMailServer = new DataTable();
            DataTable DtTemplated = new DataTable();
            DataTable DtTemplatedParameter = new DataTable();
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            string sqlcmd = "";
            string Id = "";
            sqlcmd = "Select * from [Sys_Conf_MailConfig] Where isdelete = 0 ";
            DtMailServer = _Cn.Select(sqlcmd);
            sqlcmd = "Select * from [Sys_Conf_MailTemplated] Where Id ='" + TemplatedId + "' and Isdelete= 0";
            DtTemplated = _Cn.Select(sqlcmd);
            sqlcmd = "Select * from [Sys_Conf_MailTemplatedParameter] Where TemplatedId ='" + TemplatedId + "' and Isdelete= 0";
            DtTemplatedParameter = _Cn.Select(sqlcmd);
            string MailSubject = "";
            string MailFooter = "";
            string MailMessage = "";
            string MailFrom = "";
            
           
            SmtpClient client = new SmtpClient(DtMailServer.Rows[0]["MailServer"].ToString(), int.Parse(DtMailServer.Rows[0]["MailPort"].ToString()));
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (DtMailServer.Rows[0]["MailPassword"].ToString() == "")
            {

            }
            else
            {
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential(DtMailServer.Rows[0]["MailUser"].ToString(), DtMailServer.Rows[0]["MailPassword"].ToString());
            }
            if (DtMailServer.Rows[0]["EnableSSL"].ToString().ToLower() == "false".ToLower())
            {
                client.EnableSsl = false;
            }
            else
            {
                client.EnableSsl = true;
            }
            string Message = "";
            Message = DtTemplated.Rows[0]["Message"].ToString().Trim();
            Id = ClsEngine.GenerateRunningId(_Connectionstring, "Sys_Trans_Email", "Id");
            if (DtTemplated.Rows.Count > 0)
            {
                MailSubject = DtTemplated.Rows[0]["Subject"].ToString().Trim();
                MailFrom = DtMailServer.Rows[0]["MailUser"].ToString().Trim();
                MailMessage = DtTemplated.Rows[0]["Message"].ToString().Trim();
                MailFooter = DtTemplated.Rows[0]["Footer"].ToString().Trim();
            }
            foreach (DataRow dr in DtTemplatedParameter.Rows)
            {
                foreach (ClsDictExtend dat in Dats)
                {
                    if (dat.Name.ToLower() == dr["parametername"].ToString().ToLower())
                    {
                        MailSubject = MailSubject.Replace("{" + dr["parametername"].ToString() + "}", dat.Val);
                        MailMessage = MailMessage.Replace("{" + dr["parametername"].ToString() + "}", dat.Val);
                        MailFooter = MailFooter.Replace("{" + dr["parametername"].ToString() + "}", dat.Val);
                    }
                }
            }
            MailMessage mail = new MailMessage(DtMailServer.Rows[0]["MailUser"].ToString(), Mailto);
            foreach (MailAddress MailCC in CC)
            {
                mail.CC.Add(MailCC);
            }
            mail.Subject = MailSubject;
            mail.Body = MailMessage + MailFooter;
            mail.IsBodyHtml = true;
            try
            {
                client.Send(mail);
         
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                sqlcmd = " INSERT INTO [Sys_Trans_Email] ";
                sqlcmd += " ([Id] ";
                sqlcmd += " ,[Module] ";
                sqlcmd += " ,[RefId] ";
                sqlcmd += " ,[MailSubject] ";
                sqlcmd += " ,[MailMessage] ";
                sqlcmd += " ,[MailFooter] ";
                sqlcmd += " ,[MailFrom] ";
                sqlcmd += " ,[MailTo] ";
                sqlcmd += " ,[SendDate] ";
                sqlcmd += " ,[SendBy]) ";
                sqlcmd += " Values('" + Id + "','" + Module + "','" + RefId + "','" + MailSubject + "','" + MailMessage + "','" + MailFooter + "','" + MailFrom + "','" + Mailto + "','" + System.DateTime.Now + "','" + SendBy + "')";
                Arrcmd.Add(sqlcmd);
                _Cn.Execute(Arrcmd, null);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _Cn.Close();
            }
        }


    }
}