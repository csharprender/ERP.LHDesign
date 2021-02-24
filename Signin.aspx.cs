using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SVframework2016;
using System.Web.Services;
using ERP.LHDesign2020.Class;
using System.Data;
namespace ERP.LHDesign2020
{
    public partial class Signin : System.Web.UI.Page
    {
        private static string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static string Doforgotpasswordsendpass(string json)
        {
            const string TemplatedForgotpassId = "1";
            string Email = "";
            string LinkToken = "";
            string Id = "";
            string IPAddress = "";
            string Userid  = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Hdforgotpassworduserid");
            System.Net.Mail.MailAddressCollection CC = new System.Net.Mail.MailAddressCollection();
            DataTable Dt = new DataTable();
            SVFrameWork.Datetime SvDatetime = new SVFrameWork.Datetime();
          
            List<ClsDictExtend> Dicts = new List<ClsDictExtend>();
            DataTable DtConfiguration = new DataTable();
            ClsMail ObjMail = new ClsMail(connectionstring);
            ClsDictExtend Objdict = new ClsDictExtend();
            string Sqlcmd = "select * from Sys_Master_Userprofile where id = '" + Userid + "' and isdelete = 0 ";
            SqlConnector Cn = new SqlConnector(connectionstring, null);
            string Token = ClsEngine.Base64Encode(Userid + SvDatetime.Getcurrentdateyyyymmdd() + SvDatetime.GetCurrentHHMMSS());
            try
            {
                Dt = Cn.Select(Sqlcmd);
                if (Dt.Rows.Count > 0)
                {
                    Id = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_Audit_ResetPassword", "Id");
                    Email = Dt.Rows[0]["Email"].ToString();
                    Sqlcmd = "select * from sys_conf_configuration where ConfigurationName = 'LinkToken' and IsDelete = 0";
                    DtConfiguration = Cn.Select(Sqlcmd);
                    LinkToken = DtConfiguration.Rows[0]["ConfigurationValue"].ToString();
                    LinkToken += "?Key=" + Token;
                    if (DtConfiguration.Rows.Count > 0)
                    {
                        Objdict = new ClsDictExtend();
                        Objdict.Name = "FirstName";
                        Objdict.Val = Dt.Rows[0]["Firstnameth"].ToString();
                        Dicts.Add(Objdict);
                        Objdict = new ClsDictExtend();
                        Objdict.Name = "Lastname";
                        Objdict.Val = Dt.Rows[0]["Lastnameth"].ToString();
                        Dicts.Add(Objdict);
                        Objdict = new ClsDictExtend();
                        Objdict.Name = "LinkToken";
                        Objdict.Val = LinkToken;
                        Dicts.Add(Objdict);
                        IPAddress = ClsEngine.GetIPAddress();
                        Sqlcmd = " INSERT INTO [Sys_Audit_ResetPassword] ";
                        Sqlcmd += " ([Id] ";
                        Sqlcmd += " ,[Username] ";
                        Sqlcmd += " ,[Token] ";
                        Sqlcmd += " ,[Email] ";
                        Sqlcmd += " ,[RequestDate] ";
                        Sqlcmd += " ,[RequestIP] ";
                        Sqlcmd += " ,[IsResponse] ";
                        Sqlcmd += " ,[IsDelete] ";
                        Sqlcmd += " ,[CreateDate] ";
                        Sqlcmd += " ,[CreateBy] ) ";
                        Sqlcmd += " VALUES ('" + Id + "','" + Userid + "','" + Token + "','" + Email + "','" + System.DateTime.Now + "','" + IPAddress + "',0,0,'" + System.DateTime.Now + "','" + Userid + "')";
                        Cn.Execute(Sqlcmd, null);
                        return ObjMail.SendMail(Email, TemplatedForgotpassId, "", "Forgotpassword", Userid, Dicts, CC);

                    }
                    else
                    {
                        return "Email กับ Username ไม่ตรงกันโปรดเปลี่ยนรหัสผ่านกับผู้ดูแลระบบ";
                    }

                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                Cn.Close();
                Cn.Dispose();
            }
        }
        [WebMethod]
        public static Clsuser Validateforgotpassword(string json)
        {
            //json += 'Txtforgotpasswordemail :' + $('#Txtforgotpasswordemail').val() + '|'
            SqlConnector cn = new SqlConnector(connectionstring, null);
            Clsuser Objuser = new Clsuser();
            string email = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Txtforgotpasswordemail");
            string sqlcmd = " Select * from Sys_Master_Userprofile up inner join sys_master_user u on up.id=  u.id where u.isdelete = 0 and up.isdelete = 0 and Email = '" + email + "'";
            DataTable Dt = new DataTable();
            try
            {
                Dt = cn.Select(sqlcmd);
                if (Dt.Rows.Count == 0)
                {
                    Objuser.Err =  "E-mail ไม่ถูกต้อง โปรดตรวจสอบ";
                    return Objuser;
                }
                Objuser =  ClsEngine.Loadprofile(ref cn, Dt.Rows[0]["username"].ToString());
                return Objuser;
            }
            catch (Exception ex)
            {
                Objuser.Err =  ex.Message;
                return Objuser;
            }
            finally
            {
                cn.Close();
            }
        }
        [WebMethod]
        public static string doSignin(string json)
        {
            SqlConnector cn = new SqlConnector(connectionstring, null);
            string username = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Txtusername");
            string password = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Txtpassword");
            string sqlcmd = "";
            
            sqlcmd = " Select ConfigurationValue from Sys_Conf_Configuration where isdelete = 0 and ConfigurationName = 'PrivateKey'";
            string PrivateKey = cn.Select(sqlcmd).Rows[0][0].ToString();
            sqlcmd = " select * from sys_master_user where username  = '" + username + "' and password = '" + ClsEngine.Encrypt(password, PrivateKey) + "' and logintype='LC'";
            try
            {
                if (cn.Select(sqlcmd).Rows.Count == 0)
                {
                    return "Username หรือ Password ไม่ถูกต้อง";
                }
                HttpContext.Current.Session["My"] = ClsEngine.Loadprofile(ref cn, username);
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

 
    

    }
}