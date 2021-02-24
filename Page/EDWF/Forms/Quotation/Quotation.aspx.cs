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
namespace ERP.LHDesign2020.Page.EDWF.Forms.Quotation
{
    public partial class Quotation : System.Web.UI.Page
    {
        private const string _Eformcode = "EFORM004";
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        public static string UrlAttachment = ConfigurationManager.AppSettings["UrlAttachment"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session.Remove("EFORM004");
                HttpContext.Current.Session.Remove("Period");
                ClsGrid ObjGrid = new ClsGrid();
                HttpContext.Current.Session["ObjGrid"] = ObjGrid;
            }
        }
        [WebMethod]
        public static string Savecontactor(string json)
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
                //json += 'Txtnewcontactorname :' + $('#Txtnewcontactorname').val() + '|';
                //json += 'Txtnewcontactoraddress :' + $('#Txtnewcontactoraddress').val() + '|';
                //json += 'Txtnewcontactortel :' + $('#Txtnewcontactortel').val() + '|';
                //json += 'TxtnewcontactorcardID :' + $('#TxtnewcontactorcardID').val() + '|';
                //json += 'Txtnewcontactorexprirydate :' + $('#Txtnewcontactorexprirydate').val() + '|';
                //json += 'Cbnewcontactorbank :' + $('#Cbnewcontactorbank').val() + '|';
                //json += 'Txtnewcontactorbankaccountname :' + $('#Txtnewcontactorbankaccountname').val() + '|';
                //json += 'Txtnewcontactorbankaccountno :' + $('#Txtnewcontactorbankaccountno').val() + '|';
                //json += 'Cbnewcontactorbankaccountype :' + $('#Cbnewcontactorbankaccountype').val() + '|';
                id = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Master_contactor", "Id");
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                code = "CONT-" + id.ToString().PadLeft(7, '0');
                fullname = ClsEngine.FindValue(Dicts, "Txtnewcontactorname");
                address = ClsEngine.FindValue(Dicts, "Txtnewcontactoraddress");
                tel = ClsEngine.FindValue(Dicts, "Txtnewcontactortel");

                cardId = ClsEngine.FindValue(Dicts, "TxtnewcontactorcardID");
                exprirydate = ClsEngine.FindValue(Dicts, "Txtnewcontactorexprirydate");
                bank = ClsEngine.FindValue(Dicts, "Cbnewcontactorbank");
                accountname = ClsEngine.FindValue(Dicts, "Txtnewcontactorbankaccountname");
                accountno = ClsEngine.FindValue(Dicts, "Txtnewcontactorbankaccountno");
                accounttype = ClsEngine.FindValue(Dicts, "Cbnewcontactorbankaccounttype");


                Objmy = (Clsuser)HttpContext.Current.Session["My"];
                sqlcmd += " INSERT INTO [Sys_Master_Contactor] ";
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
        public static List<Clscontactor> Selcont(string json)
        {
            ClsLH Obj = new ClsLH();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            try
            {
                return Obj.GetContactor(ref cn, ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x"));
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
        public static string Deleteattachment(string json)
        {
            Boolean isfound = false;
            if (HttpContext.Current.Session["EFORM004"] != null)
            {
                foreach (ClsAttachment obj in ((CLSEForm004)HttpContext.Current.Session["EFORM004"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        isfound = true;
                        if (obj.Uploaduserid == ((Clsuser)HttpContext.Current.Session["My"]).userid)
                        {
                            ((CLSEForm004)HttpContext.Current.Session["EFORM004"]).Attachments.Remove(obj);
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
            if (HttpContext.Current.Session["EFORM004"] != null)
            {
                foreach (ClsAttachment obj in ((CLSEForm004)HttpContext.Current.Session["EFORM004"]).Attachments)
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
            CLSEForm004 Obj = new CLSEForm004();
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            return Obj.Attachments;
        }
        [WebMethod]
        public static string Savestate(string json)
        {
            CLSEForm004 Obj = new CLSEForm004();
            string[] Arr = json.Split('#');
            string itemid = "";
            string Serviceid = "";
            string itemname = "";
            string Quann = "";
            string Unitid = "";
            string Unitname = "";
            string Materialprice = "";
            string Materialtotalprice = "";
            string Manpowerprice = "";
            string Manpowertotalprice = "";
            string Totalprice = "";
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            foreach (string str in Arr)
            {
                if (str != "")
                {
                    itemid = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "id").Split('_')[1];
                    Serviceid = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "id").Split('_')[0];
                    itemname = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "name");
                    Quann = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "Quann");
                    Unitid = "1";
                    Unitname = "1";
                    Materialprice = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "matprice");
                    Materialtotalprice = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "mattotalprice");
                    Manpowerprice = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "manpowerprice");
                    Manpowertotalprice = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "manpowertotalprice");
                    Totalprice = ClsEngine.FindValue(ClsEngine.DeSerialized(str, ':', '|'), "totalprice");


                    foreach (Clsofferdetail offer in Obj.OfferDetails)
                    {
                        if (offer.Serviceid.Trim() == Serviceid.Trim())
                        {
                            foreach (Clsdetail detail in offer.Details)
                            {
                                if (itemid.Trim() == detail.itemid)
                                {
                                    detail.itemname = itemname;
                                    detail.Unitid = Unitid;
                                    detail.Unitname = Unitname;
                                    detail.Quann = Quann;
                                    detail.Materialprice = Materialprice;
                                    detail.Materialtotalprice = Materialtotalprice;
                                    detail.Manpowerprice = Manpowerprice;
                                    detail.Manpowertotalprice = Manpowertotalprice;
                                    detail.Totalprice = Totalprice;
                                }
                            }
                        }
                    }
                }
            }
            HttpContext.Current.Session["EFORM004"] = Obj;
            return "";
        }
        [WebMethod]
        public static List<ClsService> Additem(string json)
        {
            ClsLH Obj = new ClsLH();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            string id = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Serviceid");
            return Obj.Getservicebyid(ref cn, id);
        }

       
        [WebMethod]
        public static List<ClsService> Getservice(string json)
        {
            ClsLH Obj = new ClsLH();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            string kwd = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "kwd");
            return Obj.Getservice(ref cn, kwd);
        }

        [WebMethod]
        public static string Validatecustomer(string json)
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
                //json += 'Txtnewcustomername :' + $('#Txtnewcustomername').val() + '|';
                //json += 'Txtnewcustomeraddress :' + $('#Txtnewcustomeraddress').val() + '|';
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                fullname = ClsEngine.FindValue(Dicts, "Txtnewcustomername");
                address = ClsEngine.FindValue(Dicts, "Txtnewcustomeraddress");

                sqlcmd = "Select * from sys_master_customer where isdelete  = 0 and Fullname ='" + fullname + "'";
                if (cn.Select(sqlcmd).Rows.Count > 0)
                {
                    return "!E" + fullname + " is dupplicated ";
                }
                else
                {
                    sqlcmd = "Select * from sys_master_customer where isdelete  = 0 and Fullname  like '%" + fullname + "%'";
                    Dt = cn.Select(sqlcmd);
                    if (Dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Dt.Rows)
                        {
                            msg += dr["fullname"].ToString() + " ";
                        }
                        return "!W" + "Customer may be dupplicate " + msg + ". Do you want to add ?";
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
        public static string Savecustomer(string json)
        {
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            List<ClsDict> Dicts = new List<ClsDict>();

            string sqlcmd = "";
            string id = "";
            string code = "";
            string fullname = "";
            string address = "";
            string tel = "";
            Clsuser Objmy = new Clsuser();
            try
            {
                //json += 'Txtnewcustomername :' + $('#Txtnewcustomername').val() + '|';
                //json += 'Txtnewcustomeraddress :' + $('#Txtnewcustomeraddress').val() + '|';
                id = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Master_Customer", "Id");
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                code = "CUST-" + id.ToString().PadLeft(7, '0');
                fullname = ClsEngine.FindValue(Dicts, "Txtnewcustomername");
                address = ClsEngine.FindValue(Dicts, "Txtnewcustomeraddress");
                tel = ClsEngine.FindValue(Dicts, "Txtnewcustomertel");
                Objmy = (Clsuser)HttpContext.Current.Session["My"];
                sqlcmd += " INSERT INTO [Sys_Master_Customer] ";
                sqlcmd += " ([Id] ";
                sqlcmd += " ,[Code] ";
                sqlcmd += " ,[Fullname] ";
                sqlcmd += " ,[Address] ";
                sqlcmd += " ,[Tel] ";
                sqlcmd += " ,[IsDelete] ";
                sqlcmd += " ,[CreateDate] ";
                sqlcmd += " ,[CreateBy] )  ";
                sqlcmd += " VALUES  ";
                sqlcmd += " (  ";
                sqlcmd += "'" + id + "'";
                sqlcmd += ",'" + code + "'";
                sqlcmd += ",'" + fullname + "'";
                sqlcmd += ",'" + address + "'";
                sqlcmd += ",'" + tel + "'";
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
        public static Clsofferdetail Delitem(string json)
        {
            CLSEForm004 Obj = new CLSEForm004();
            Clsofferdetail Objserv = new Clsofferdetail();
            string itemid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "itemid");
            string Serviceid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Serviceid");

            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == Serviceid.Trim())
                {
                    foreach (Clsdetail detail in offer.Details)
                    {
                        if (itemid.Trim() == detail.itemid)
                        {
                            offer.Details.Remove(detail);
                            offer.Errormessage = "";
                            Objserv = offer;
                            HttpContext.Current.Session["EFORM004"] = Obj;
                            Objserv = offer;
                            return Objserv;
                        }
                    }
                }
            }

            return Objserv;
        }

        [WebMethod]
        public static Clsofferdetail Selitem(string json)
        {
            CLSEForm004 Obj = new CLSEForm004();
            Clsdetail Objdetail;
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            DataTable Dt = new DataTable();
            Clsofferdetail Objserv = new Clsofferdetail();
            string servicegroupid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "servicegroupid");
            string Serviceid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Serviceid");
            string sqlcmd = "Select * from sys_master_service where isdelete =0 and id= '" + Serviceid + "'";
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            Dt = cn.Select(sqlcmd);
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];

            //validate dupplicate
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == servicegroupid.Trim())
                {
                    foreach (Clsdetail detail in offer.Details)
                    {
                        if (detail.itemid == Serviceid)
                        {
                            Objserv.Errormessage = "Dupplicate item";
                            return Objserv;
                        }
                    }
                }

            }
            //validate dupplicate

            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == servicegroupid.Trim())
                {
                    if (offer.Details == null || offer.Details.Count == 0)
                    {
                        offer.Details = new List<Clsdetail>();
                    }
                    Objdetail = new Clsdetail();
                    Objdetail.itemid = Dt.Rows[0]["id"].ToString();
                    Objdetail.itemname = Dt.Rows[0]["Servicenameth"].ToString();
                    offer.Details.Add(Objdetail);
                    offer.Errormessage = "";
                    HttpContext.Current.Session["EFORM004"] = Obj;
                    Objserv = offer;
                    return Objserv;
                }
            }

            return Objserv;
        }
        [WebMethod]
        public static Clsofferdetail Selservice(string json)
        {
            CLSEForm004 Obj = new CLSEForm004();
            SqlConnector cn = new SqlConnector(Connectionstring, null);
            DataTable Dt = new DataTable();
            Clsofferdetail Objserv = new Clsofferdetail();
            string Serviceid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Serviceid");
            string sqlcmd = "Select * from sys_master_service where isdelete =0 and id= '" + Serviceid + "'";
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            Dt = cn.Select(sqlcmd);
            //validate dupplicate
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == Serviceid.Trim())
                {
                    Objserv.Errormessage = "Dupplicate service group";
                    return Objserv;
                }
            }
            //validate dupplicate
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == "0")
                {
                    offer.Serviceid = Serviceid;
                    offer.Servicenameth = Dt.Rows[0]["Servicenameth"].ToString();
                    offer.Errormessage = "";
                    Objserv = offer;
                    HttpContext.Current.Session["EFORM004"] = Obj;
                    return Objserv;
                }
            }
            return Objserv;
        }

        [WebMethod]
        public static string Delservice(string json)
        {
            CLSEForm004 Obj = new CLSEForm004();
            string Serviceid = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "Serviceid");
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid.Trim() == Serviceid.Trim())
                {
                    Obj.OfferDetails.Remove(offer);
                    HttpContext.Current.Session["EFORM004"] = Obj;
                    return "";
                }
            }
            return "";
        }
        [WebMethod]
        public static string Addservice()
        {
            CLSEForm004 Obj = new CLSEForm004();
            Clsofferdetail Objoffer = new Clsofferdetail();
            Obj = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            foreach (Clsofferdetail offer in Obj.OfferDetails)
            {
                if (offer.Serviceid == "0")
                {
                    return "Please Select service before";
                }
            }
            Objoffer.Serviceid = "0";
            Objoffer.Details = new List<Clsdetail>();
            Obj.OfferDetails.Add(Objoffer);
            HttpContext.Current.Session["EFORM004"] = Obj;
            return "";
        }
        [WebMethod]
        public static CLSEForm004 Getdesc()
        {
            return (CLSEForm004)HttpContext.Current.Session["EFORM004"];
        }

        [WebMethod]
        public static CLSEForm004 GetDocumentInfo(string json)
        {

            const string Eformcode = "EFORM004";
            CLSEForm004 Obj = new CLSEForm004();
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            string sqlcmd = "";
            DataTable Dt = new DataTable();
            DataTable DtAttchment = new DataTable();
            string Sqlcmd = "";
            try
            {
                if (HttpContext.Current.Session["EFORM004"] != null)
                {
                    return (CLSEForm004)HttpContext.Current.Session["EFORM004"];
                }

                Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
                Obj.EFormcode = Eformcode;
                Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                Obj.DocumentDate = System.DateTime.Today.ToShortDateString();
                Dt = new DataTable();
                Sqlcmd = "select * from [Sys_Edocument_EFORM004] Where Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                Dt = Cn.Select(Sqlcmd);
                Cn.Close();
                Obj.Transbatchid = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
                List<Clsofferdetail> Offerdetails = new List<Clsofferdetail>();
                if (Dt.Rows.Count > 0)
                {
                    //Transaction
                    
                    Obj.Customerid = Dt.Rows[0]["Customerid"].ToString();
                    Obj.Customername = Dt.Rows[0]["Customername"].ToString();
                    Obj.Customeraddress = Dt.Rows[0]["Customeraddress"].ToString();
                    Obj.Saleid = Dt.Rows[0]["Saleid"].ToString();
                    Obj.Salename = Dt.Rows[0]["Salename"].ToString();
                    Obj.Totalprice = Dt.Rows[0]["Totalprice"].ToString();
                    Obj.vatamount = Dt.Rows[0]["vatamount"].ToString();

                    Obj.Grandtotalprice = Dt.Rows[0]["Grandtotalprice"].ToString();
                    //Transaction
                    //Detail
                    
                    Clsofferdetail Objofferdetail;
                    List<Clsdetail> Details = new List<Clsdetail>();
                    Clsdetail Objdetail;
                    double Totalprice = 0;
                    DataRow[] Services;
                    Dt = new DataTable();
                    DataTable Dtservice = new DataTable();
                    sqlcmd = "Select Distinct S.id as Serviceid,S.Servicenameth from [Sys_EDocument_Efrom004detail] efd inner join sys_master_service s on efd.serviceid = s.id  Where efd.Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and efd.isdelete = 0 ";
                    Dtservice = Cn.Select(sqlcmd);
                    sqlcmd = "Select efd.*,s.*,m.Servicenameth as itemname from [Sys_EDocument_Efrom004detail] efd inner join sys_master_service s on efd.serviceid = s.id inner join sys_master_service m on m.id = Materialid where  efd.Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and efd.isdelete = 0 ";
                    Dt = Cn.Select(sqlcmd);
                    foreach (DataRow s_dr in Dtservice.Rows)
                    {
                        Totalprice = 0;
                        Details = new List<Clsdetail>();
                        Objofferdetail = new Clsofferdetail();
                        Services = Dt.Select("Serviceid =" + s_dr["Serviceid"]);
                        Objofferdetail.Serviceid = s_dr["Serviceid"].ToString();
                        Objofferdetail.Servicenameth = s_dr["Servicenameth"].ToString();
                        foreach (DataRow dr in Services)
                        {
                            Objdetail = new Clsdetail();
                            Objdetail.itemid = dr["Materialid"].ToString();
                            Objdetail.itemname = dr["itemname"].ToString();
                            Objdetail.Quann = dr["Quann"].ToString();
                            Objdetail.Unitid = dr["Unitid"].ToString();
                            Objdetail.Unitname = dr["Unitname"].ToString();
                            Objdetail.Materialprice = dr["Materialprice"].ToString();
                            Objdetail.Materialtotalprice = dr["Materialtotalprice"].ToString();
                            Objdetail.Manpowerprice = dr["Manpowerprice"].ToString();
                            Objdetail.Manpowertotalprice = dr["Manpowertotalprice"].ToString();
                            Objdetail.Totalprice = dr["Totalprice"].ToString();
                            Totalprice += double.Parse(Objdetail.Totalprice);
                            Details.Add(Objdetail);
                        }
                        Objofferdetail.Details = Details;
                        Objofferdetail.Totalprice = Totalprice.ToString();
                        Offerdetails.Add(Objofferdetail);
                    }
                    //Detail
                }
                Obj.OfferDetails = Offerdetails;
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
                Obj.Attachments = new List<ClsAttachment>(); 
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
                HttpContext.Current.Session["EFORM004"] = Obj;
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
            Attachements = ((CLSEForm004)HttpContext.Current.Session["EFORM004"]).Attachments;
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
            ((CLSEForm004)HttpContext.Current.Session["EFORM004"]).Attachments = Attachements;
            Cn.Close();
            return "";

        }
        [WebMethod]
        public static string Print(string json)
        {
            const string Eformcode = "EFORM004";
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
            catch(Exception ex)
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
            CLSEForm004 ObjEform = new CLSEForm004();
            string DocumentNo = ClsEngine.GenerateRunningno("EFORM004", Connectionstring, "Sys_Edocument_EFORM004", "id");
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            ObjCurrentState = ((ClsInfo)HttpContext.Current.Session["Info"]);
            ObjMy = ((Clsuser)HttpContext.Current.Session["My"]);
            Dt = new DataTable();
            Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
            ObjEform = (CLSEForm004)HttpContext.Current.Session["EFORM004"];
            ObjEform.Customerid = ClsEngine.FindValue(Dicts, "Customerid");
            ObjEform.Customername = ClsEngine.FindValue(Dicts, "Customername");
            ObjEform.Customeraddress = ClsEngine.FindValue(Dicts, "Customeraddress");
            ObjEform.Customertel = ClsEngine.FindValue(Dicts, "Customertel");
            ObjEform.Saleid = ClsEngine.FindValue(Dicts, "Saleid");
            ObjEform.Salename = ClsEngine.FindValue(Dicts, "Salename");
            ObjEform.Saletel = ClsEngine.FindValue(Dicts, "Saletel");
            ObjEform.Offerdate = ClsEngine.FindValue(Dicts, "Offerdate");
            ObjEform.Totalprice = ClsEngine.FindValue(Dicts, "Totalprice");
            ObjEform.vatamount = ClsEngine.FindValue(Dicts, "vatamount");
            ObjEform.Grandtotalprice = ClsEngine.FindValue(Dicts, "Grandtotalprice");
            //======================================  Tranaction  =========================================
            if (ObjCurrentState.NodeTypeId == "1") //Current Node
            {
                //เริ่มต้น
                TransBatchId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "TransBatchId");
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id");
                string _id = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_EDocument_EFORM004", "id");
                ObjEform.Documentno = "EFORM004-" + DateTime.Today.Year.ToString().PadLeft(4, '0') + DateTime.Today.Month.ToString().PadLeft(2, '0') + _id.ToString().PadLeft(4, '0');
                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_EDocument_Eform004] ";
                sqlcmd += " ([id] ";
                sqlcmd += ",[Transbatchid] ";
                sqlcmd += ",[TransFlowDetailId] ";
                sqlcmd += ",[Companylogourl] ";
                sqlcmd += ",[Companyname] ";
                sqlcmd += ",[Companyaddress] ";
                sqlcmd += ",[Companytel] ";
                sqlcmd += ",[Version] ";
                sqlcmd += ",[Documentdate] ";
                sqlcmd += ",[Documentno] ";
                sqlcmd += ",[Customerid] ";
                sqlcmd += ",[Customername] ";
                sqlcmd += ",[Customeraddress] ";

                sqlcmd += ",[Customertel] ";
                sqlcmd += ",[Offerdate] ";
                sqlcmd += ",[Saleid] ";
                sqlcmd += ",[Salename] ";
                sqlcmd += ",[SaleTel] ";
                sqlcmd += ",[Totalprice] ";
                sqlcmd += ",[vatamount] ";
                sqlcmd += ",[Grandtotalprice] ";
                sqlcmd += ",[IsDelete] ";
                sqlcmd += ",[CreateDate] ";
                sqlcmd += ",[CreateBy] ) ";
                sqlcmd += "VALUES ";
                sqlcmd += "( ";
                sqlcmd += "'" + _id + "'";
                sqlcmd += ",'" + TransBatchId + "'";
                sqlcmd += ",'" + TransFlowDetailId + "'";
                sqlcmd += ",'" + ObjEform.Companylogourl + "'";
                sqlcmd += ",'" + ObjEform.Companyname + "'";
                sqlcmd += ",'" + ObjEform.Companyaddress + "'";

                sqlcmd += ",'" + ObjEform.Companytel + "'";
                sqlcmd += ",'" + ObjEform.Version + "'";
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.DocumentDate) + "'";
                sqlcmd += ",'" + ObjEform.Documentno + "'";
                sqlcmd += ",'" + ObjEform.Customerid + "'";
                sqlcmd += ",'" + ObjEform.Customername + "'";
                sqlcmd += ",'" + ObjEform.Customeraddress + "'";
                sqlcmd += ",'" + ObjEform.Customertel + "'";

                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Offerdate) + "'";
                sqlcmd += ",'" + ObjEform.Saleid + "'";
                sqlcmd += ",'" + ObjEform.Salename + "'";
                sqlcmd += ",'" + ObjEform.Saletel + "'";
                sqlcmd += ",'" + ObjEform.Totalprice + "'";
                sqlcmd += ",'" + ObjEform.vatamount + "'";
                sqlcmd += ",'" + ObjEform.Grandtotalprice + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",getdate()";
                sqlcmd += ",'" + ((Clsuser)HttpContext.Current.Session["My"]).userid + "'";
                sqlcmd += " ) ";
                Arrcmd.Add(sqlcmd);
                //Remark
            }
            else //ถ้ามีอยู่แล้ว
            {
                TransBatchId = ObjCurrentState.TransBatchId;
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id"); // Genarete
                TransformId = ObjCurrentState.TransFormId;
                sqlcmd = " Update Sys_EDocument_Eform004 set ";
                sqlcmd += " [Customerid] ='" + ObjEform.Customerid + "'";
                sqlcmd += ",[Customername] ='" + ObjEform.Customername + "'";
                sqlcmd += ",[Customeraddress] ='" + ObjEform.Customeraddress + "'";
                sqlcmd += ",[Customertel] ='" + ObjEform.Customertel + "'";
                sqlcmd += ",[Offerdate] ='" + ClsEngine.ConvertDateforSavingDatabase(ObjEform.Offerdate) + "'";
                sqlcmd += ",[Saleid] ='" + ObjEform.Saleid + "'";
                sqlcmd += ",[Salename] ='" + ObjEform.Salename + "'";
                sqlcmd += ",[Saletel] ='" + ObjEform.Saletel + "'";
                sqlcmd += ",[Totalprice]='" + ObjEform.Totalprice + "'";
                sqlcmd += ",[vatamount]='" + ObjEform.vatamount + "'";
                sqlcmd += ",[Grandtotalprice]='" + ObjEform.Grandtotalprice + "'";
                sqlcmd += " Where TransBatchId = '" + TransBatchId + "'";
                Arrcmd.Add(sqlcmd);
            }

            sqlcmd = "update Sys_EDocument_Efrom004detail set isdelete =1,deletedate=getdate(),deleteby='" + ObjMy.userid + "'  Where TransBatchId ='" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            string _detailid = ClsEngine.GenerateRunningId(Cn.Connectionstring, "Sys_EDocument_Efrom004detail", "Id");
            foreach (Clsofferdetail offer in ObjEform.OfferDetails)
            {
                foreach (Clsdetail detail in offer.Details)
                {

                    sqlcmd = "";
                    sqlcmd += "INSERT INTO [Sys_EDocument_Efrom004detail] ";
                    sqlcmd += "([id]";
                    sqlcmd += ",[Transbatchid]";
                    sqlcmd += ",[Serviceid]";
                    sqlcmd += ",[Servicenameth]";
                    sqlcmd += ",[Materialid]";
                    sqlcmd += ",[Materialnameth]";
                    sqlcmd += ",[Quann]";
                    sqlcmd += ",[Unitid]";
                    sqlcmd += ",[Unitname]";
                    sqlcmd += ",[Materialprice]";
                    sqlcmd += ",[Materialtotalprice]";
                    sqlcmd += ",[Manpowerprice]";
                    sqlcmd += ",[Manpowertotalprice]";
                    sqlcmd += ",[Totalprice]";
                    sqlcmd += ",[IsDelete]";
                    sqlcmd += ",[CreateDate]";
                    sqlcmd += ",[CreateBy] ) ";
                    sqlcmd += " VALUES ";
                    sqlcmd += "( ";
                    sqlcmd += "'" + _detailid + "'";
                    sqlcmd += ",'" + TransBatchId + "'";
                    sqlcmd += ",'" + offer.Serviceid + "'";
                    sqlcmd += ",'" + offer.Servicenameth + "'";
                    sqlcmd += ",'" + detail.itemid + "'";
                    sqlcmd += ",'" + detail.itemname + "'";
                    sqlcmd += ",'" + int.Parse(detail.Quann) + "'";
                    sqlcmd += ",'" + detail.Unitid + "'";
                    sqlcmd += ",'" + detail.Unitname + "'";
                    sqlcmd += ",'" + detail.Materialprice + "'";
                    sqlcmd += ",'" + detail.Materialtotalprice + "'";
                    sqlcmd += ",'" + detail.Manpowerprice + "'";
                    sqlcmd += ",'" + detail.Manpowertotalprice + "'";
                    sqlcmd += ",'" + detail.Totalprice + "'";
                    sqlcmd += ",'0'";
                    sqlcmd += ",getdate()";
                    sqlcmd += ",'" + ObjMy.userid + "'";
                    sqlcmd += " ) ";
                    Arrcmd.Add(sqlcmd);
                    _detailid = (int.Parse(_detailid) + 1).ToString();
                }
            }
            //EOFORM004
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
            foreach (ClsAttachment _attch in ((CLSEForm004)(HttpContext.Current.Session["EFORM004"])).Attachments)
            {
                count += 1;
                if (count < ((CLSEForm004)(HttpContext.Current.Session["EFORM004"])).Attachments.Count)
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
            Attachs = ((CLSEForm004)(HttpContext.Current.Session["EFORM004"])).Attachments;

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
            //
            //==================== Transaction =============================================================
            
            //======================================  Tranaction  =========================================

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
            return ObjCurrentState.NodeTypeId;
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
            if (Ctrl == "Gvcustomer")
            {
                PK = "id";
                Sqlcmd = "Select id,fullname,address from Sys_Master_Customer where isdelete = 0";
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
            else if (Ctrl == "Gvcontactor")
            {
                PK = "id";
                Sqlcmd = "Select id,fullname,address from Sys_Master_Contactor where isdelete = 0";
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
        public static string Validatecontactor(string json)
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
                //json += 'Txtnewcontactorname :' + $('#Txtnewcontactorname').val() + '|';
                //json += 'Txtnewcontactoraddress :' + $('#Txtnewcontactoraddress').val() + '|';
                Dicts = ClsEngine.DeSerialized(json, ':', '|');
                fullname = ClsEngine.FindValue(Dicts, "Txtnewcontactorname");
                address = ClsEngine.FindValue(Dicts, "Txtnewcontactoraddress");

                sqlcmd = "Select * from sys_master_contactor where isdelete  = 0 and Fullname ='" + fullname + "'";
                if (cn.Select(sqlcmd).Rows.Count > 0)
                {
                    return "!E" + fullname + " is dupplicated ";
                }
                else
                {
                    sqlcmd = "Select * from sys_master_contactor where isdelete  = 0 and Fullname  like '%" + fullname + "%'";
                    Dt = cn.Select(sqlcmd);
                    if (Dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Dt.Rows)
                        {
                            msg += dr["fullname"].ToString() + " ";
                        }
                        return "!W" + "contactor may be dupplicate " + msg + ". Do you want to add ?";
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