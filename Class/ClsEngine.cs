using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using SVframework2016;
using System.Text;
namespace ERP.LHDesign2020.Class
{
    public class ClsEngine
    {
        public static string GetMonthThai(int MonthCode)
        {
            switch (MonthCode)
            {
                case 1: return "มกราคม";
                case 2: return "กุมภาพันธ์";
                case 3: return "มีนาคม";
                case 4: return "เมษายน";
                case 5: return "พฤษภาคม";
                case 6: return "มิถุนายน";
                case 7: return "กรกฏาคม";
                case 8: return "สิงหาคม";
                case 9: return "กันยายน";
                case 10: return "ตุลาคม";
                case 11: return "พฤศจิกายน";
                case 12: return "ธันวาคม";
                default: return "";
            }

        }
        public static string ConvertInt2nvarchar(string Str, char Delimiter)
        {
            string res = "";
            string[] Arr = Str.Split(Delimiter);
            foreach (string s in Arr)
            {
                if (res != "")
                {
                    res += ",|" + s + "|";
                }
                else
                {
                    res += "|" + s + "|";
                }
            }
            return res;
        }
        public static string Getconfigurationvalue(ref SqlConnector cn,string key,string group)
        {
            string sqlcmd = "Select * from sys_conf_configuration where isdelete = 0 and Configurationgroup = '" + group + "' and configurationname  = '" + key + "'";
            try
            {
                return cn.Select(sqlcmd).Rows[0]["configurationvalue"].ToString();
            }
            catch
            {
                return "";
            }
        }
        public static Nullable<System.DateTime> ConvertDateforSavingDatabase(string dd_mm_yyyyy_bud)
        {
            try
            {
                string d = dd_mm_yyyyy_bud.Split('-')[0];
                string m = dd_mm_yyyyy_bud.Split('-')[1];
                string y = dd_mm_yyyyy_bud.Split('-')[2];

                if (int.Parse(y) > 2500)
                {
                    y = (int.Parse(y) - 543).ToString();
                }
                System.DateTime dat = new DateTime(int.Parse(y), int.Parse(m), int.Parse(d));
                return dat;
            }
            catch
            {
                try
                {
                    System.DateTime dat = System.DateTime.Parse(dd_mm_yyyyy_bud);
                    return dat;
                }
                catch
                {
                    return null;
                }

            }

        }
        public string ValidateDateBud(System.DateTime ObjDate)
        {
            if (ObjDate.Year == 1900) //DC
            {
                return "";
            }
            else if (ObjDate.Year == 2443) //BC
            {
                return "";
            }
            if (ObjDate.Year < 2500)
            {
                return ObjDate.Day.ToString().PadLeft(2, '0') + "/" + ObjDate.Month.ToString().PadLeft(2, '0') + "/" + ObjDate.AddYears(543).Year;
            }
            else
            {
                return ObjDate.Day.ToString().PadLeft(2, '0') + "/" + ObjDate.Month.ToString().PadLeft(2, '0') + "/" + ObjDate.Year;
            }
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData.Replace(' ','+'));
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        private static byte[] key = {
        };
        private static byte[] IV = {
            0x12,
            0x34,
            0x56,
            0x78,
            0x90,
            0xab,
            0xcd,
            0xef
        };
        public static string Convertdate2ddmmyyyy(System.DateTime _datetime, string delimeter, Boolean convert2bud)
        {
            int year = _datetime.Year;
            if (convert2bud)
            {
                if (year < 2500)
                {
                    year += 543;
                }
            }
            else
            {
                if (year > 2500)
                {
                    year -= 543;
                }
            }
            return _datetime.Day.ToString().PadLeft(2, '0') + delimeter + _datetime.Month.ToString().PadLeft(2, '0') + delimeter + year.ToString().PadLeft(4, '0');
        }
        public static string Serialize(System.Collections.ArrayList arr)
        {
            int count = 0;
            string res = "";
            if (arr.Count == 0)
            {
                return "-99";
            }
            foreach (string str in arr)
            {
                count += 1;
                if (count >= arr.Count)
                {
                    res += str;
                }
                else
                {
                    res += str + ",";
                }
            }
            return res;
        }

        public static string FindMessage(DataTable DtMessage, string MsgCode, string ColumnLang)
        {
            try
            {
                return DtMessage.Select("MsgCode = '" + MsgCode + "'")[0][ColumnLang].ToString();

            }
            catch
            {
                return "";
            }
        }
        public static string GetIPAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
        public static string FindValue(List<ClsDict> Dats, string FieldName)
        {
            foreach (ClsDict Objdat in Dats)
            {
                if (Objdat.Name.ToLower().Trim() == FieldName.ToLower().Trim())
                {
                    return Objdat.Val.Trim();
                }
            }
            return "";
        }
        public static string GenerateRunningno(string Eformcode, string Connectionstring, string Table, string FieldsRunningId)
        {
            SqlConnector Cn = new SqlConnector(Connectionstring, null);
            DataTable Dt = new DataTable();
            string sqlcmd = "Select (isnull(max(cast(" + FieldsRunningId + " as int)),0)) + 1 as id from [" + Table + "] ";
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            Cn.Dispose();
            return Eformcode + "-" + DateTime.Today.Year.ToString().PadLeft(2, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + Dt.Rows[0][0].ToString().PadLeft(3, '0');
        }
        public static string GenerateRunningId(string Connectionstring, string Table, string FieldsRunningId)
        {
            SqlConnector Cn = new SqlConnector(Connectionstring, null);
            DataTable Dt = new DataTable();
            string sqlcmd = "Select (isnull(max(cast(" + FieldsRunningId + " as int)),0)) + 1 as id from [" + Table + "] ";
            Dt = Cn.Select(sqlcmd);
            Cn.Close();
            Cn.Dispose();
            return Dt.Rows[0][0].ToString();
        }
        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            if (SEncryptionKey.Length > 8)
            {
                SEncryptionKey = SEncryptionKey.Substring(0, 8);
            }
            else
            {
                SEncryptionKey = SEncryptionKey.PadRight(8, '!');
            }
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static List<ClsDict> DeSerialized(string dat, char delitmiter, char endline)
        {
            try
            {
                string[] lines = dat.Split(endline);
                List<ClsDict> Dicts = new List<ClsDict>();

                foreach (string str in lines)
                {
                    if (str.Trim() != "")
                    {
                        ClsDict Objdict = new ClsDict();
                        string[] strval = str.Split(delitmiter);
                        foreach (string _str in strval)
                        {
                            if (Objdict.Name == null)
                            {
                                Objdict.Name = _str;
                            }
                            else
                            {
                                Objdict.Val += _str;
                            }
                        }

                        Dicts.Add(Objdict);
                    }
                }
                return Dicts;
            }
            catch
            {
                return new List<ClsDict>();
            }
        }
        public static Clsuser Loadprofile(ref SqlConnector cn, string username)
        {
            Clsuser Objmy = new Clsuser();
            DataTable Dt = new DataTable();
            string sqlcmd = "select * from sys_master_user u inner join sys_master_userprofile up on u.id = up.id where u.isdelete = 0 and u.username  = '" + username + "'";
            Dt = cn.Select(sqlcmd);
            Objmy.userid = Dt.Rows[0]["id"].ToString();
            Objmy.Titlecode = Dt.Rows[0]["Titlecode"].ToString();
            Objmy.Titlenameth = Dt.Rows[0]["Titlenameth"].ToString();
            Objmy.username = Dt.Rows[0]["username"].ToString();
            Objmy.firstnameth = Dt.Rows[0]["firstnameth"].ToString();
            Objmy.lastnameth = Dt.Rows[0]["lastnameth"].ToString();
            Objmy.tel = Dt.Rows[0]["mobile"].ToString();
            Objmy.email = Dt.Rows[0]["firstnameth"].ToString();
            Objmy.addressEN = Dt.Rows[0]["addressEN"].ToString();
            Objmy.addressTH = Dt.Rows[0]["addressTH"].ToString();
            Objmy.avartarurl = Dt.Rows[0]["avartarurl"].ToString();
            Objmy.sigurl = Dt.Rows[0]["sigurl"].ToString();
            Objmy.logintype = Dt.Rows[0]["logintype"].ToString();

            ////Orgazie 
            //List<Clsuserposition> Userpositions = new List<Clsuserposition>();
            //Clsuserposition Objuserposition;
            sqlcmd = "select positionid,Orgid, PositionnameTH,PositionnameEN,OrganizenameTH,OrganizenameEN,Priority from Sys_Master_UserinPosition up inner join Sys_Master_Position p on up.positionid = p.id  and up.isdelete= 0 and p.isdelete = 0 inner join sys_master_organize o on p.Orgid= o.id and o.IsDelete = 0 and p.isdelete = 0 where userid = '" + Objmy.userid + "'";
            Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            if (Dt.Rows.Count > 0)
            {
                Objmy.positionid = Dt.Rows[0]["positionid"].ToString();
                Objmy.positionnameth = Dt.Rows[0]["PositionnameTH"].ToString();
                Objmy.organizeId = Dt.Rows[0]["Orgid"].ToString();
                Objmy.organizenameth = Dt.Rows[0]["OrganizenameTH"].ToString();
            }
            //foreach (DataRow dr in Dt.Rows)
            //{
            //    Objuserposition = new Clsuserposition();
            //    Objuserposition.positionnameEN = dr["positionnameEN"].ToString();
            //    Objuserposition.positionnameTH = dr["positionnameTH"].ToString();
            //    Objuserposition.positionid = dr["positionid"].ToString();
            //    Objuserposition.organizenameTH = dr["organizenameTH"].ToString();
            //    Objuserposition.organizenameEN = dr["organizenameEN"].ToString();
            //    Objuserposition.orgid = dr["orgid"].ToString();
            //    Objuserposition.priority = dr["priority"].ToString();
            //    Userpositions.Add(Objuserposition);
            //}
            //Objmy.userpositions = Userpositions;
            //Organize

            //Role 
            System.Collections.ArrayList ArrSystem = new System.Collections.ArrayList();
            List<Clsuserrole> Userroles = new List<Clsuserrole>();
            Clsuserrole Objuserrole;
            sqlcmd = "Select r.id,r.rolename,ru.selected from Sys_Master_Roleuser ru left join Sys_Master_Role r on ru.IsDelete = 0 and r.IsDelete = 0 where userid = '" + Objmy.userid + "'";
            Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            foreach (DataRow dr in Dt.Rows)
            {
                Objuserrole = new Clsuserrole();
                Objuserrole.roleid = dr["id"].ToString();
                Objuserrole.rolenameTH = dr["rolename"].ToString();
                Objuserrole.features = GetFeatures(ref cn, Objuserrole.roleid);
                Objuserrole.Selected = dr["Selected"].ToString();

                foreach (Clsfeature _f in Objuserrole.features)
                {
                    ArrSystem.Add(_f.systemid);
                }
                Userroles.Add(Objuserrole);
            }
            Objmy.userroles = Userroles;
            sqlcmd = "Select id,systemcode,SystemnameTH,SystemnameEN,SystemnameDescTH,SystemnameDescEN from Sys_Master_System s  where isdelete = 0 and id in (" + ClsEngine.Serialize(ArrSystem) + ")";
            Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            ClsSystem Objsys;
            List<ClsSystem> Systems = new List<ClsSystem>();
            foreach (DataRow dr in Dt.Rows)
            {
                Objsys = new ClsSystem();
                Objsys.Systemid = dr["id"].ToString();
                Objsys.Systemcode = dr["Systemcode"].ToString();
                Objsys.SystemnameTH = dr["SystemnameTH"].ToString();
                Objsys.SystemnameEN = dr["SystemnameEN"].ToString();
                Objsys.SystemnameDescTH = dr["SystemnameDescTH"].ToString();
                Objsys.SystemnameDescEN = dr["SystemnameDescEN"].ToString();
                Systems.Add(Objsys);
            }
            Objmy.Systems = Systems;
            //Role
            return Objmy;
        }
        private static List<Clsfeature> GetFeatures(ref SqlConnector cn, string roleid)
        {
            List<Clsfeature> Objs = new List<Clsfeature>();
            Clsfeature Obj;
            string sqlcmd = "";
            DataTable Dt = new DataTable();
            sqlcmd = "select functioncode,functionid,FunctionnameTH,FunctionnameEN,isedit,isview,Systemid,systemcode,SystemnameTH,SystemnameEN,ctrl from Sys_Master_Rolefunction rf left join sys_master_function f on rf.functionid = f.id and rf.isdelete = 0 and f.isdelete = 0 left join Sys_Master_System s on f.Systemid = s.id where rf.isdelete = 0 and rf.roleid = '" + roleid + "'";
            Dt = cn.Select(sqlcmd);
            foreach (DataRow dr in Dt.Rows)
            {
                Obj = new Clsfeature();
                Obj.ctrl = dr["ctrl"].ToString();
                Obj.isedit = dr["isedit"].ToString();
                Obj.isview = dr["isview"].ToString();
                Obj.systemid = dr["systemid"].ToString();
                Obj.systemcode = dr["systemcode"].ToString();
                Obj.systemnameEN = dr["systemnameEN"].ToString();
                Obj.systemnameTH = dr["systemnameTH"].ToString();
                Obj.functionid = dr["functionid"].ToString();
                Obj.functioncode = dr["functioncode"].ToString();
                Obj.functionnameTH = dr["functionnameTH"].ToString();
                Obj.functionnameEN = dr["functionnameEN"].ToString();
                Objs.Add(Obj);
            }
            return Objs;
        }
        public string Post(string url, object json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";

            var js = new System.Web.Script.Serialization.JavaScriptSerializer();

            // turn our request string into a byte stream
            byte[] postBytes = Encoding.UTF8.GetBytes(js.Serialize(json));

            // this is important - make sure you specify type this way
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result;
            using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
            {
                result = rdr.ReadToEnd();
            }

            return result;
        }
    }
}