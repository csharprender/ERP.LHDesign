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
namespace ERP.LHDesign2020.Page.EDWF.Forms.WHT
{
    public partial class WHT : System.Web.UI.Page
    {

        private const string _Eformcode = "EForm010";
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        public static string UrlAttachment = ConfigurationManager.AppSettings["UrlAttachment"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HttpContext.Current.Session.Remove("EForm010");
                ClsGrid ObjGrid = new ClsGrid();
                HttpContext.Current.Session["ObjGrid"] = ObjGrid;
            }
        }
        [WebMethod]
        public static string Selperiod(string json)
        {
            ClsEForm010 Obj = new ClsEForm010();
            Obj = ((ClsEForm010)HttpContext.Current.Session["EFORM010"]);
            foreach (ClsEForm008Period _obj in Obj.Periods)
            {
                if (_obj.Period == json)
                {
                    _obj.Selected = "x";
                }
            }
            HttpContext.Current.Session["EFORM010"] = Obj;
            return "";
        }
        [WebMethod]
        public static string Unselperiod(string json)
        {
            ClsEForm010 Obj = new ClsEForm010();
            Obj = ((ClsEForm010)HttpContext.Current.Session["EFORM010"]);
            foreach (ClsEForm008Period _obj in Obj.Periods)
            {
                if (_obj.Period == json)
                {
                    _obj.Selected = "";
                }
            }
            HttpContext.Current.Session["EFORM010"] = Obj;
            return "";
        }
        [WebMethod]
        public static List<ClsEForm008Period> Getitem()
        {
           
            ClsEForm010 Obj = new ClsEForm010();
            if (HttpContext.Current.Session["EFORM010"] != null)
            {
                Obj = (ClsEForm010)HttpContext.Current.Session["EFORM010"];
                return Obj.Periods;
            }
            else
            {
                Obj.Periods = new List<ClsEForm008Period>();
                return Obj.Periods;
            }
        }
        [WebMethod]
        public static string Deleteattachment(string json)
        {
            Boolean isfound = false;
            if (HttpContext.Current.Session["EForm010"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEForm010)HttpContext.Current.Session["EForm010"]).Attachments)
                {
                    if (obj.Attachmentid == json)
                    {
                        isfound = true;
                        if (obj.Uploaduserid == ((Clsuser)HttpContext.Current.Session["My"]).userid)
                        {
                            ((ClsEForm010)HttpContext.Current.Session["EForm010"]).Attachments.Remove(obj);
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
            if (HttpContext.Current.Session["EForm010"] != null)
            {
                foreach (ClsAttachment obj in ((ClsEForm010)HttpContext.Current.Session["EForm010"]).Attachments)
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
            ClsEForm010 Obj = new ClsEForm010();
            Obj = (ClsEForm010)HttpContext.Current.Session["EForm010"];
            return Obj.Attachments;
        }


        [WebMethod]
        public static ClsTotalWHT Getgrandtotal(string json)
        {
            ClsTotalWHT ObjTotalWHT = new ClsTotalWHT();
            ClsEForm010 Obj = new ClsEForm010();
            double Grandtotal = 0;
            if (HttpContext.Current.Session["EFORM010"] != null)
            {
                Obj = ((ClsEForm010)HttpContext.Current.Session["EFORM010"]);
                foreach (ClsEForm008Period _obj in Obj.Periods)
                {
                    if (_obj.Selected == "x")
                    {
                        Grandtotal += double.Parse(_obj.Amount);
                    }

                }
            }
            ObjTotalWHT.Date = ClsEngine.Convertdate2ddmmyyyy(DateTime.Today,"-", false);
            ObjTotalWHT.Amount = Grandtotal.ToString("N2");
            ObjTotalWHT.WHT = (Grandtotal * 0.03).ToString("N2");
            return ObjTotalWHT;
        }
        [WebMethod]
        public static ClsEForm010 GetDocumentInfo(string json)
        {

            const string Eformcode = "EForm010";
            ClsEForm010 Obj = new ClsEForm010();
            SqlConnector Cn = new SqlConnector(Connectionstring, "");
            DataTable Dt = new DataTable();
            DataTable DtPeriodEForm008 = new DataTable();
            DataTable DtAttchment = new DataTable();
            string Sqlcmd = "";
            try
            {
                if (HttpContext.Current.Session["EForm010"] != null)
                {
                    return (ClsEForm010)HttpContext.Current.Session["EForm010"];
                }
                Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
                Obj.EFormcode = Eformcode;
                Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                Obj.DocumentDate = System.DateTime.Today.ToShortDateString();
                Dt = new DataTable();
                Sqlcmd = "select * from [Sys_Edocument_EForm010] m left join [Sys_EDocument_Eform010Detail] d on m.Transbatchid = d.TransBatchId and m.IsDelete = 0 and d.IsDelete = 0 Where m.Transbatchid='" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "'";
                Dt = Cn.Select(Sqlcmd);
                Obj.Transbatchid = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
                Cn.Close();
                if (Dt.Rows.Count > 0)
                {

                    Obj.Documentno = Dt.Rows[0]["DocumentNo"].ToString(); //ถ้าเป็น Node อื่นๆ ที่ไม่ใช่ Node Begin 
                    if (Obj.Documentno.Trim() == "")
                    {
                        Obj.Documentno = ClsEngine.GenerateRunningno(_Eformcode, Connectionstring, "Sys_Edocument_EForm010", "id");
                    }
                    try
                    {
                        Obj.DocumentDate = DateTime.Parse(Dt.Rows[0]["DocumentDate"].ToString()).ToShortDateString();
                    }
                    catch
                    {

                    }
                    Obj.id = Dt.Rows[0]["id"].ToString();




                    Obj.Transbatchid = Dt.Rows[0]["Transbatchid"].ToString();
                    Obj.TransFlowDetailId = Dt.Rows[0]["TransFlowDetailId"].ToString();
                    Obj.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
                    Obj.Companyname = Dt.Rows[0]["Companyname"].ToString();
                    Obj.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
                    Obj.Companytel = Dt.Rows[0]["Companytel"].ToString();
                    Obj.Version = Dt.Rows[0]["Version"].ToString();
                   
                    Obj.Referhirecontractid = Dt.Rows[0]["Referhirecontractid"].ToString();
                    Obj.Hirecontractno = Dt.Rows[0]["Hirecontractno"].ToString();
                    Obj.Certfullname = Dt.Rows[0]["Certfullname"].ToString();
                    Obj.CertTaxno1 = Dt.Rows[0]["CertTaxno1"].ToString();
                    Obj.CertFullAddress = Dt.Rows[0]["CertFullAddress"].ToString();
                    Obj.CertTaxno2 = Dt.Rows[0]["CertTaxno2"].ToString();





                    Obj.Fullname = Dt.Rows[0]["Fullname"].ToString();
                    Obj.Taxno1 = Dt.Rows[0]["Taxno1"].ToString();
                    Obj.FullAddress = Dt.Rows[0]["FullAddress"].ToString();
                    Obj.Taxno2 = Dt.Rows[0]["Taxno2"].ToString();
                    Obj.Orderinform = Dt.Rows[0]["Orderinform"].ToString();
                    Obj.Docno = Dt.Rows[0]["Docno"].ToString();
                    Obj.Bookno = Dt.Rows[0]["Bookno"].ToString();
                    Obj.PND1A = Dt.Rows[0]["PND1A"].ToString();
                    Obj.PND1AExtra = Dt.Rows[0]["PND1AExtra"].ToString();
                    Obj.PND2 = Dt.Rows[0]["PND2"].ToString();
                    Obj.PND3 = Dt.Rows[0]["PND3"].ToString();
                    Obj.PND2A = Dt.Rows[0]["PND2A"].ToString();
                    Obj.PND3A = Dt.Rows[0]["PND3A"].ToString();
                    Obj.PND53 = Dt.Rows[0]["PND53"].ToString();










                    try
                    {
                        Obj.TotalAmount = double.Parse(Dt.Rows[0]["TotalAmount"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.TotalAmount = "";
                    }

                    try
                    {
                        Obj.TotalTax = double.Parse(Dt.Rows[0]["TotalTax"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.TotalTax = "";
                    }
                    try
                    {
                        Obj.PayforTeacherAidFund = double.Parse(Dt.Rows[0]["PayforTeacherAidFund"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayforTeacherAidFund = "";
                    }
                    try
                    {
                        Obj.PayforSocialSecurityFund = double.Parse(Dt.Rows[0]["PayforSocialSecurityFund"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayforSocialSecurityFund = "";
                    }
                    try
                    {
                        Obj.PayforProvidentFund = double.Parse(Dt.Rows[0]["PayforProvidentFund"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayforProvidentFund = "";
                    }
                    Obj.PayMethodWHT = Dt.Rows[0]["PayMethodWHT"].ToString();
                    Obj.PayMethodRecuring = Dt.Rows[0]["PayMethodRecuring"].ToString();
                    Obj.PayMethodOncetime = Dt.Rows[0]["PayMethodOncetime"].ToString();
                    Obj.PayeenameTH = Dt.Rows[0]["PayeenameTH"].ToString();
                    Obj.PayforOther = Dt.Rows[0]["PayforOther"].ToString();
                    Obj.PayforOtherRemark = Dt.Rows[0]["PayforOtherRemark"].ToString();
                    if (Dt.Rows[0]["Paymentdate"].ToString() != "")
                    {
                        Obj.Paymentdate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Paymentdate"].ToString()), "/", false);
                    }
                    else
                    {
                        Obj.Paymentdate = "";
                    }
                    if (Dt.Rows[0]["PayDate_1"].ToString() != "")
                    {
                        Obj.PayDate_1 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_1"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_1 = "";
                    }
                    try
                    {
                        Obj.PayAmount_1 = double.Parse(Dt.Rows[0]["PayAmount_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_1 = "";
                    }
                    try
                    {
                        Obj.PayWHT_1 = double.Parse(Dt.Rows[0]["PayWHT_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_1 = "";
                    }




                    if (Dt.Rows[0]["PayDate_2"].ToString() != "")
                    {
                        Obj.PayDate_2 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_2"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_2 = "";
                    }
                    try
                    {
                        Obj.PayAmount_2 = double.Parse(Dt.Rows[0]["PayAmount_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_2 = "";
                    }
                    try
                    {
                        Obj.PayWHT_2 = double.Parse(Dt.Rows[0]["PayWHT_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_2 = "";
                    }



                    if (Dt.Rows[0]["PayDate_3"].ToString() != "")
                    {
                        Obj.PayDate_3 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_3"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_3 = "";
                    }
                    try
                    {
                        Obj.PayAmount_3 = double.Parse(Dt.Rows[0]["PayAmount_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_3 = "";
                    }
                    try
                    {
                        Obj.PayWHT_3 = double.Parse(Dt.Rows[0]["PayWHT_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_3 = "";
                    }



                    if (Dt.Rows[0]["PayDate_4A"].ToString() != "")
                    {
                        Obj.PayDate_4A = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4A"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4A = "";
                    }
                    try
                    {
                        Obj.PayAmount_4A = double.Parse(Dt.Rows[0]["PayAmount_4A"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4A = "";
                    }
                    try
                    {
                        Obj.PayWHT_4A = double.Parse(Dt.Rows[0]["PayWHT_4A"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4A = "";
                    }


                    if (Dt.Rows[0]["PayDate_4B1_1"].ToString() != "")
                    {
                        Obj.PayDate_4B1_1 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B1_1"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B1_1 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B1_1 = double.Parse(Dt.Rows[0]["PayAmount_4B1_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B1_1 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B1_1 = double.Parse(Dt.Rows[0]["PayWHT_4B1_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B1_1 = "";
                    }


                    if (Dt.Rows[0]["PayDate_4B1_2"].ToString() != "")
                    {
                        Obj.PayDate_4B1_2 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B1_2"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B1_2 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B1_2 = double.Parse(Dt.Rows[0]["PayAmount_4B1_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B1_2 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B1_2 = double.Parse(Dt.Rows[0]["PayWHT_4B1_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B1_2 = "";
                    }



                    if (Dt.Rows[0]["PayDate_4B1_3"].ToString() != "")
                    {
                        Obj.PayDate_4B1_3 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B1_3"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B1_3 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B1_3 = double.Parse(Dt.Rows[0]["PayAmount_4B1_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B1_3 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B1_3 = double.Parse(Dt.Rows[0]["PayWHT_4B1_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B1_3 = "";
                    }



                    if (Dt.Rows[0]["PayDate_4B1_4"].ToString() != "")
                    {
                        Obj.PayDate_4B1_4 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B1_4"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B1_4 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B1_4 = double.Parse(Dt.Rows[0]["PayAmount_4B1_4"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B1_4 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B1_4 = double.Parse(Dt.Rows[0]["PayWHT_4B1_4"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B1_4 = "";
                    } 
                    Obj.PayRemark_4B1_4  = Dt.Rows[0]["PayRemark_4B1_4"].ToString();




                    if (Dt.Rows[0]["PayDate_4B2_1"].ToString() != "")
                    {
                        Obj.PayDate_4B2_1 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B2_1"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B2_1 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B2_1 = double.Parse(Dt.Rows[0]["PayAmount_4B2_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B2_1 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B2_1 = double.Parse(Dt.Rows[0]["PayWHT_4B2_1"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B2_1 = "";
                    }




                    if (Dt.Rows[0]["PayDate_4B2_2"].ToString() != "")
                    {
                        Obj.PayDate_4B2_2 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B2_2"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B2_2 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B2_2 = double.Parse(Dt.Rows[0]["PayAmount_4B2_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B2_2 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B2_2 = double.Parse(Dt.Rows[0]["PayWHT_4B2_2"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B2_2 = "";
                    }



                    if (Dt.Rows[0]["PayDate_4B2_3"].ToString() != "")
                    {
                        Obj.PayDate_4B2_3 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B2_3"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B2_3 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B2_3 = double.Parse(Dt.Rows[0]["PayAmount_4B2_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B2_3 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B2_3 = double.Parse(Dt.Rows[0]["PayWHT_4B2_3"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B2_3 = "";
                    }



                    if (Dt.Rows[0]["PayDate_4B2_4"].ToString() != "")
                    {
                        Obj.PayDate_4B2_4 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B2_4"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B2_4 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B2_4 = double.Parse(Dt.Rows[0]["PayAmount_4B2_4"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B2_4 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B2_4 = double.Parse(Dt.Rows[0]["PayWHT_4B2_4"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B2_4 = "";
                    }
                    Obj.PayRemark_4B2_4  = Dt.Rows[0]["PayRemark_4B2_4"].ToString();


                    if (Dt.Rows[0]["PayDate_4B2_5"].ToString() != "")
                    {
                        Obj.PayDate_4B2_5 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_4B2_5"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_4B2_5 = "";
                    }
                    try
                    {
                        Obj.PayAmount_4B2_5 = double.Parse(Dt.Rows[0]["PayAmount_4B2_5"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_4B2_5 = "";
                    }
                    try
                    {
                        Obj.PayWHT_4B2_5 = double.Parse(Dt.Rows[0]["PayWHT_4B2_5"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_4B2_5 = "";
                    }
                    Obj.PayRemark_4B2_5  = Dt.Rows[0]["PayRemark_4B2_5"].ToString();



                    if (Dt.Rows[0]["PayDate_5"].ToString() != "")
                    {
                        Obj.PayDate_5 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_5"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_5 = "";
                    }
                    try
                    {
                        Obj.PayAmount_5 = double.Parse(Dt.Rows[0]["PayAmount_5"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_5 = "";
                    }
                    try
                    {
                        Obj.PayWHT_5 = double.Parse(Dt.Rows[0]["PayWHT_5"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_5 = "";
                    }


                    if (Dt.Rows[0]["PayDate_6"].ToString() != "")
                    {
                        Obj.PayDate_6 = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["PayDate_6"].ToString()), "-", false);
                    }
                    else
                    {
                        Obj.PayDate_6 = "";
                    }
                    try
                    {
                        Obj.PayAmount_6 = double.Parse(Dt.Rows[0]["PayAmount_6"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayAmount_6 = "";
                    }
                    try
                    {
                        Obj.PayWHT_6 = double.Parse(Dt.Rows[0]["PayWHT_6"].ToString()).ToString("N2");
                    }
                    catch
                    {
                        Obj.PayWHT_6 = "";
                    }
                  
                    Obj.PayRemark_6  = Dt.Rows[0]["PayRemark_6"].ToString();


                    Obj.Periods = new List<ClsEForm008Period>();
                    List<ClsEForm008Period> Periods = new List<ClsEForm008Period>();
                    Dt = new DataTable();

                    Sqlcmd = "Select * from [Sys_EDocument_EForm010] m left join [Sys_Edocument_Eform010Period] d on m.Transbatchid = d.TransBatchId where d.TransBatchId = '" + Obj.Transbatchid + "' and d.IsDelete = 0 ";
                    Dt = Cn.Select(Sqlcmd);
                    Sqlcmd = "Select * from Sys_EDocument_Eform008 m left join Sys_EDocument_Eform008detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Referhirecontractid + "'";
                    DtPeriodEForm008 = Cn.Select(Sqlcmd);
                    ClsEForm008Period Objperiod;
                    foreach (DataRow s_dr in DtPeriodEForm008.Rows)
                    {
                        Objperiod = new ClsEForm008Period();
                        Objperiod.Period = s_dr["Period"].ToString();
                        Objperiod.Periodname = s_dr["Periodname"].ToString();
                        Objperiod.Amount = Double.Parse(s_dr["Amount"].ToString()).ToString("N2");
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
                    Obj.Referhirecontractid = "";
                    Obj.Periods = new List<ClsEForm008Period>();
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

                HttpContext.Current.Session["EForm010"] = Obj;

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
            Attachements = ((ClsEForm010)HttpContext.Current.Session["EForm010"]).Attachments;
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
            ((ClsEForm010)HttpContext.Current.Session["EForm010"]).Attachments = Attachements;
            Cn.Close();
            return "";

        }
        [WebMethod]
        public static string Print(string json)
        {
            const string Eformcode = "EForm010";
            string x = ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId;
            string StrMsg = "PrintformCode=" + Eformcode + "&Engine=CrystalReport&ID=" + x;
            StrMsg = ClsEngine.Base64Encode(StrMsg);
            return "/../../Printforms/PrintformCaller.aspx?Val=" + StrMsg;
        }
        [WebMethod]
        public static string PrintInvoice(string json)
        {
            const string Eformcode = "EForm009";
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
            DataTable Dtdetail = new DataTable();
            DataTable Dtcompany = new DataTable();
            Dicts = ClsEngine.DeSerialized(json, ':', '|');
            DataRow[] Drs;
            Clsuser ObjMy;
            DataTable Dt = new DataTable();
            ClsEForm010 ObjEform = new ClsEForm010();
            string DocumentNo = ClsEngine.GenerateRunningno("EForm010", Connectionstring, "Sys_Edocument_EForm010", "id");
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            ObjCurrentState = ((ClsInfo)HttpContext.Current.Session["Info"]);
            ObjMy = ((Clsuser)HttpContext.Current.Session["My"]);
            Dt = new DataTable();
            Dt = Cn.Select("Select * from Sys_Info_Company where isdelete =0");
  





            //======================================  Tranaction  =========================================
            ObjEform = (ClsEForm010)HttpContext.Current.Session["EForm010"];
            ObjEform.Documentno = DocumentNo;
            ObjEform.Companylogourl = Dt.Rows[0]["Companylogourl"].ToString();
            ObjEform.Companyname = Dt.Rows[0]["Companyname"].ToString();
            ObjEform.Companyaddress = Dt.Rows[0]["Companyaddress"].ToString();
            ObjEform.Companytel = Dt.Rows[0]["Companytel"].ToString();
            double Grandtotal = 0;
            foreach (ClsEForm008Period _obj in ObjEform.Periods)
            {
                Grandtotal += double.Parse(_obj.Amount);

            }
        //readonly:false | Txthirecontract: | ActionResultValue: S | ActionResultNameTH: บันทึกแบบร่าง | 
        //TxtRemark_1:| Txthirecontract:EFORM008 - 202010001 | Txtperiodname_0:1 | Txtperiodamount_0:200.00 
        //| Txtperiodname_1:2 | Txtperiodamount_1:200.00 | Txtperiodname_2:3 | Txtperiodamount_2:200.00 |
        //Txtperiodname_3:4 | Txtperiodamount_3:200.00 | Txtperiodname_4:5 | Txtperiodamount_4:400.00 |
        //Txtcerttaxno1:| Txtcertfullname:บ ครอบจักวาล จำกัด | Txttaxno2
            //Txtorderinform:| TxtPayDate_1:17 - 10 - 2020 | TxtPayAmount_1:600 | TxtPayWHT_1:18.00 | 
            //TxtPayDate_2:| TxtPayAmount_2:| TxtPayWHT_2:| TxtPayDate_3:| TxtPayAmount_3:| TxtPayWHT_3:| 
            //TxtPayDate_4A:| TxtPayAmount_4A:| TxtPayWHT_4A:| TxtPayDate_4B1_1:| TxtPayAmount_4B1_1:| 
            //TxtPayWHT_4B1_1:| TxtPayDate_4B1_2:| TxtPayAmount_4B1_2:| TxtPayWHT_4B1_2:| TxtPayDate_4B1_3:| 
            //TxtPayAmount_4B1_3:| TxtPayWHT_4B1_3:| TxtPayDate_4B1_4:| TxtPayAmount_4B1_4:| TxtPayWHT_4B1_4:| 
            //TxtPayDate_4B2_1:| TxtPayAmount_4B2_1:| TxtPayWHT_4B2_1:| TxtPayDate_4B2_2:| TxtPayAmount_4B2_2:| 
            //TxtPayWHT_4B2_2:| TxtPayDate_4B2_3:| TxtPayAmount_4B2_3:| TxtPayWHT_4B2_3:| TxtPayDate_4B2_4:| 
            //TxtPayAmount_4B2_4:| TxtPayWHT_4B2_4:| TxtRemark_4B2_5:| TxtPayDate_4B2_5:| TxtPayAmount_4B2_5:| 
            //TxtPayWHT_4B2_5:| TxtPayDate_5:| TxtPayAmount_5:| TxtPayWHT_5:| TxtRemark_6:| 
            //TxtPayDate_6:| TxtPayAmount_6:| TxtPayWHT_6:| TxtTotalAmount:600.00 | 
            //TxtTotalTax:18.00 | TxtPayforTeacherAidFund:| TxtPayforSocialSecurityFund:|
            //TxtPayforProvidentFund:| TxtPayforOtherRemark:| ChkPND1A:false | ChkPND1AExtra:false | 
            //ChkPND2:false | ChkPND3:false | ChkPND2A:false | ChkPND3A:false | ChkPND53:false | 
            //ChkPayMethodWHT:true | ChkPayMethodRecuring:true | ChkPayMethodOncetime:false | 
            //ChkPayforOther:false |
            //Txthirecontract_dat
            if (ObjCurrentState.NodeTypeId == "1") //Current Node
            {
                //เริ่มต้น
                TransBatchId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "TransBatchId");
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id");
                TransformId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Edocument_EForm010", "Id");
                string Receiptno = "RC-" + TransformId.ToString().PadLeft(6, '0');
                DataTable Dtcont = new DataTable();
               
                sqlcmd = "";
                sqlcmd += " INSERT INTO [Sys_EDocument_Eform010] ";
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
                sqlcmd += " ,[Referhirecontractid]";
                sqlcmd += " ,[Hirecontractno]";
                sqlcmd += " ,[Docno]";
                sqlcmd += " ,[Bookno]";
                sqlcmd += " ,[Certfullname]";
                sqlcmd += " ,[CertTaxno1]";
                sqlcmd += " ,[CertFullAddress]";
                sqlcmd += " ,[CertTaxno2]";
                sqlcmd += " ,[Fullname]";
                sqlcmd += " ,[Taxno1]";
                sqlcmd += " ,[FullAddress]";
                sqlcmd += " ,[Taxno2]";
                sqlcmd += " ,[Orderinform]";
                sqlcmd += " ,[PND1A]";
                sqlcmd += " ,[PND1AExtra]";
                sqlcmd += " ,[PND2]";
                sqlcmd += " ,[PND3]";
                sqlcmd += " ,[PND2A]";
                sqlcmd += " ,[PND3A] ";
                sqlcmd += " ,[PND53] ";
                sqlcmd += " ,[TotalAmount] ";
                sqlcmd += " ,[TotalTax] ";
                sqlcmd += " ,[PayforTeacherAidFund] ";
                sqlcmd += " ,[PayforSocialSecurityFund] ";
                sqlcmd += " ,[PayforProvidentFund] ";
                sqlcmd += " ,[PayMethodWHT] ";
                sqlcmd += " ,[PayMethodRecuring] ";
                sqlcmd += " ,[PayMethodOncetime] ";

                sqlcmd += " ,[Paymentdate] ";
                sqlcmd += " ,[PayforOther] ";
                sqlcmd += " ,[PayforOtherRemark] ";
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
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txthirecontract_dat") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txthirecontract") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtdocno") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtbookno") + "'";

                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtcertfullname") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtcerttaxno1") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtcertfulladdress") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtcerttaxno2") + "'";

                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtfullname") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txttaxno1") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtfulladdress") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txttaxno2") + "'";



                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "Txtorderinform") + "'";
                if (ClsEngine.FindValue(Dicts, "ChkPND1A") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND1AExtra") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND2") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND3") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND2A") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND3A") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND53") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtTotalAmount").Replace(",","") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtTotalTax").Replace(",", "") + "'";

                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayforTeacherAidFund").Replace(",", "") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayforSocialSecurityFund").Replace(",", "") + "'";
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayforProvidentFund").Replace(",", "") + "'";

                if (ClsEngine.FindValue(Dicts, "ChkPayMethodWHT") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPayMethodRecuring") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPayMethodOncetime") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }

                sqlcmd += ",'" + System.DateTime.Now + "'";
                if (ClsEngine.FindValue(Dicts, "ChkPayforOther") == "true")
                {
                    sqlcmd += ",'X'";
                }
                else
                {
                    sqlcmd += ",''";
                }
                sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayforOtherRemark") + "'";
                sqlcmd += ",'0'";
                sqlcmd += ",getdate()";
                sqlcmd += ",'" + ObjMy.userid + "'";
                sqlcmd += " ) ";
                Arrcmd.Add(sqlcmd);


                
            }
            else //Update
            {
                TransBatchId = ObjCurrentState.TransBatchId;
                TransFlowDetailId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_Trans_Flowdetail", "Id"); // Genarete
                TransformId = ObjCurrentState.TransFormId;

                sqlcmd = " Update [Sys_EDocument_Eform010] ";
                sqlcmd += " Set ";
                sqlcmd += "  [Referhirecontractid] ='" + ClsEngine.FindValue(Dicts, "Txthirecontract_dat") + "'";
                sqlcmd += " ,[Hirecontractno] ='" + ClsEngine.FindValue(Dicts, "Txthirecontract") + "'";
                sqlcmd += " ,[Docno] ='" + ClsEngine.FindValue(Dicts, "Txtdocno") + "'";
                sqlcmd += " ,[Bookno] ='" + ClsEngine.FindValue(Dicts, "Txtbookno") + "'";
               
                sqlcmd += " ,[CertFullname] ='" + ClsEngine.FindValue(Dicts, "Txtcertfullname") + "'";
                sqlcmd += " ,[CertTaxno1] ='" + ClsEngine.FindValue(Dicts, "Txtcerttaxno1") + "'";
                sqlcmd += " ,[CertFullAddress] ='" + ClsEngine.FindValue(Dicts, "Txtcertfulladdress") + "'";
                sqlcmd += " ,[CertTaxno2] ='" + ClsEngine.FindValue(Dicts, "Txtcerttaxno2") + "'";


                sqlcmd += " ,[Fullname] ='" + ClsEngine.FindValue(Dicts, "Txtfullname") + "'";
                sqlcmd += " ,[Taxno1] ='" + ClsEngine.FindValue(Dicts, "Txttaxno1") + "'";
                sqlcmd += " ,[FullAddress] ='" + ClsEngine.FindValue(Dicts, "Txtfulladdress") + "'";
                sqlcmd += " ,[Taxno2] ='" + ClsEngine.FindValue(Dicts, "Txttaxno2") + "'";



                sqlcmd += " ,[Orderinform] ='" + ClsEngine.FindValue(Dicts, "Txtorderinform") + "'";
                if (ClsEngine.FindValue(Dicts, "ChkPND1A") == "true")
                {
                    sqlcmd += " ,[PND1A]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND1A]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND1AExtra") == "true")
                {
                    sqlcmd += " ,[PND1AExtra]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND1AExtra]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND2") == "true")
                {
                    sqlcmd += " ,[PND2]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND2]  =''";
                }

                if (ClsEngine.FindValue(Dicts, "ChkPND3") == "true")
                {
                    sqlcmd += " ,[PND3]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND3]  =''";
                }

                if (ClsEngine.FindValue(Dicts, "ChkPND2A") == "true")
                {
                    sqlcmd += " ,[PND2A]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND2A]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND3A") == "true")
                {
                    sqlcmd += " ,[PND3A]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND3A]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPND53") == "true")
                {
                    sqlcmd += " ,[PND53]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PND53]  =''";
                }
                sqlcmd += " ,[TotalAmount] ='" + ClsEngine.FindValue(Dicts, "TxtTotalAmount").Replace(",", "") + "'";
                sqlcmd += " ,[TotalTax] ='" + ClsEngine.FindValue(Dicts, "TxtTotalTax").Replace(",", "") + "'";
                sqlcmd += " ,[PayforTeacherAidFund] ='" + ClsEngine.FindValue(Dicts, "TxtPayforTeacherAidFund").Replace(",", "") + "'";
                sqlcmd += " ,[PayforSocialSecurityFund] ='" + ClsEngine.FindValue(Dicts, "TxtPayforSocialSecurityFund").Replace(",", "") + "'";
                sqlcmd += " ,[PayforProvidentFund] ='" + ClsEngine.FindValue(Dicts, "TxtPayforProvidentFund").Replace(",", "") + "'";

                if (ClsEngine.FindValue(Dicts, "ChkPayMethodWHT") == "true")
                {
                    sqlcmd += " ,[PayMethodWHT]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PayMethodWHT]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPayMethodRecuring") == "true")
                {
                    sqlcmd += " ,[PayMethodRecuring]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PayMethodRecuring]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPayMethodOncetime") == "true")
                {
                    sqlcmd += " ,[PayMethodOncetime]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PayMethodOncetime]  =''";
                }
                if (ClsEngine.FindValue(Dicts, "ChkPayforOther") == "true")
                {
                    sqlcmd += " ,[PayforOther]  ='X'";
                }
                else
                {
                    sqlcmd += " ,[PayforOther]  =''";
                }
                sqlcmd += " ,[PayforOtherRemark]  ='" + ClsEngine.FindValue(Dicts, "TxtPayforOtherRemark") + "'";
                sqlcmd += " ,[Paymentdate]  ='" + System.DateTime.Now + "'";
                sqlcmd += " ,[Modifydate]  = getdate() ";
                sqlcmd += " ,[Modifyby]  ='" + ObjMy.userid + "'";
                sqlcmd += " Where Transbatchid  ='" + TransBatchId + "'";
                Arrcmd.Add(sqlcmd);

            }
            sqlcmd = " Update Sys_EDocument_Eform010Detail Set isdelete = 1,deletedate=getdate(),deleteby='" + ObjMy.userid + "' Where TransBatchId ='" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            
            string detailid = ClsEngine.GenerateRunningId(Connectionstring, "Sys_EDocument_Eform010Detail", "id");
            sqlcmd = "INSERT INTO [Sys_EDocument_Eform010Detail] ";
            sqlcmd += "([id] ";
            sqlcmd += ",[TransBatchId] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_1") != "")
            {
                sqlcmd += ",[PayDate_1] ";
            }
            sqlcmd += ",[PayAmount_1] ";
            sqlcmd += ",[PayWHT_1] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_2") != "")
            {
                sqlcmd += ",[PayDate_2] ";
            }
            sqlcmd += ",[PayAmount_2] ";
            sqlcmd += ",[PayWHT_2] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_3") != "")
            {
                sqlcmd += ",[PayDate_3] ";
            }
            sqlcmd += ",[PayAmount_3] ";
            sqlcmd += ",[PayWHT_3] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4A") != "")
            {
                sqlcmd += ",[PayDate_4A] ";
            }
            sqlcmd += ",[PayAmount_4A] ";
            sqlcmd += ",[PayWHT_4A] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_1") != "")
            {
                sqlcmd += ",[PayDate_4B1_1] ";
            }
            sqlcmd += ",[PayAmount_4B1_1] ";
            sqlcmd += ",[PayWHT_4B1_1] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_2") != "")
            {
                sqlcmd += ",[PayDate_4B1_2] ";
            }
            sqlcmd += ",[PayAmount_4B1_2] ";
            sqlcmd += ",[PayWHT_4B1_2] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_3") != "")
            {
                sqlcmd += ",[PayDate_4B1_3] ";
            }
            sqlcmd += ",[PayAmount_4B1_3] ";
            sqlcmd += ",[PayWHT_4B1_3] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_4") != "")
            {
                sqlcmd += ",[PayDate_4B1_4] ";
            }
            sqlcmd += ",[PayAmount_4B1_4] ";
            sqlcmd += ",[PayWHT_4B1_4] ";
            sqlcmd += ",[PayRemark_4B1_4] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_1") != "")
            {
                sqlcmd += ",[PayDate_4B2_1] ";
            }
            sqlcmd += ",[PayAmount_4B2_1] ";
            sqlcmd += ",[PayWHT_4B2_1] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_2") != "")
            {
                sqlcmd += ",[PayDate_4B2_2] ";
            }
            sqlcmd += ",[PayAmount_4B2_2] ";
            sqlcmd += ",[PayWHT_4B2_2] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_3") != "")
            {
                sqlcmd += ",[PayDate_4B2_3] ";
            }
            sqlcmd += ",[PayAmount_4B2_3] ";
            sqlcmd += ",[PayWHT_4B2_3] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_4") != "")
            {
                sqlcmd += ",[PayDate_4B2_4] ";
            }
            sqlcmd += ",[PayAmount_4B2_4] ";
            sqlcmd += ",[PayWHT_4B2_4] ";
            
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_5") != "")
            {
                sqlcmd += ",[PayDate_4B2_5] ";
            }
            sqlcmd += ",[PayAmount_4B2_5] ";
            sqlcmd += ",[PayWHT_4B2_5] ";
            sqlcmd += ",[PayRemark_4B2_5] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_5") != "")
            {
                sqlcmd += ",[PayDate_5] ";
            }
            sqlcmd += ",[PayAmount_5] ";
            sqlcmd += ",[PayWHT_5] ";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_6") != "")
            {
                sqlcmd += ",[PayDate_6] ";
            }
            sqlcmd += ",[PayAmount_6] ";
            sqlcmd += ",[PayWHT_6] ";
            sqlcmd += ",[PayRemark_6] ";
            sqlcmd += " ,[IsDelete] ";
            sqlcmd += " ,[CreateDate] ";
            sqlcmd += " ,[CreateBy]) ";
            sqlcmd += " VALUES (";
            sqlcmd += "'" + detailid + "'";
            sqlcmd += ",'" + TransBatchId + "'";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_1") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_1")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_1").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_1").Replace(",", "") + "'";
            if (ClsEngine.FindValue(Dicts, "TxtPayDate_2") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_2")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_2").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_2").Replace(",", "") + "'";


            if (ClsEngine.FindValue(Dicts, "TxtPayDate_3") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_3")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_3").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_3").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4A") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4A")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4A").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4A").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_1") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_1")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B1_1").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B1_1").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_2") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_2")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B1_2").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B1_2").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_3") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_3")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B1_3").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B1_3").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_4") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B1_4")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B1_4").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B1_4").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayRemark_4B1_4") + "'";



            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_1") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_1")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B2_1").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B2_1").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_2") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_2")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B2_2").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B2_2").Replace(",", "") + "'";


            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_3") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_3")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B2_3").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B2_3").Replace(",", "") + "'";


            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_4") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_4")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B2_4").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B2_4").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_5") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_4B2_5")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_4B2_5").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_4B2_5").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtRemark_4B2_5") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_5") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_5")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_5").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_5").Replace(",", "") + "'";

            if (ClsEngine.FindValue(Dicts, "TxtPayDate_6") != "")
            {
                sqlcmd += ",'" + ClsEngine.ConvertDateforSavingDatabase(ClsEngine.FindValue(Dicts, "TxtPayDate_6")) + "'";
            }
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayAmount_6").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtPayWHT_6").Replace(",", "") + "'";
            sqlcmd += ",'" + ClsEngine.FindValue(Dicts, "TxtRemark_6") + "'";
            sqlcmd += ",'0'";
            sqlcmd += ",getdate()";
            sqlcmd += ",'" + ObjMy.userid + "'";
            sqlcmd += " ) ";
            Arrcmd.Add(sqlcmd);
            sqlcmd = " Update Sys_EDocument_Eform010Period Set isdelete = 1,deletedate=getdate(),deleteby='" + ObjMy.userid + "' Where TransBatchId ='" + TransBatchId + "'";
            Arrcmd.Add(sqlcmd);
            ClsEForm010 Obj008 = new ClsEForm010();
            Obj008 = ((ClsEForm010)HttpContext.Current.Session["EFORM010"]);
            string PeriodId = ClsEngine.GenerateRunningId(Connectionstring, "Sys_EDocument_Eform010Period", "id");
            foreach (ClsEForm008Period Objperiod in Obj008.Periods)
            {
                if (Objperiod.Selected == "x")
                {
                    sqlcmd = " INSERT INTO [Sys_EDocument_Eform010Period] ";
                    sqlcmd += "([id] ";
                    sqlcmd += ",[TransBatchId] ";
                    sqlcmd += ",[Period] ";
                    sqlcmd += ",[IsDelete] ";
                    sqlcmd += ",[CreateDate] ";
                    sqlcmd += ",[CreateBy] ) ";
                    sqlcmd += " VALUES (";
                    sqlcmd += "'" + PeriodId + "'";
                    sqlcmd += ",'" + TransBatchId + "'";
                    sqlcmd += ",'" + Objperiod.Period + "'";
                    sqlcmd += ",'0'";
                    sqlcmd += ",getdate()";
                    sqlcmd += ",'" + ObjMy.userid + "'";
                    sqlcmd += " ) ";
                    Arrcmd.Add(sqlcmd);
                    PeriodId = (int.Parse(PeriodId) + 1).ToString();
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
            foreach (ClsAttachment _attch in ((ClsEForm010)(HttpContext.Current.Session["EForm010"])).Attachments)
            {
                count += 1;
                if (count < ((ClsEForm010)(HttpContext.Current.Session["EForm010"])).Attachments.Count)
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
            Attachs = ((ClsEForm010)(HttpContext.Current.Session["EForm010"])).Attachments;
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




            //======================================  FLOW =========================================
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
            //CC Section
            if (NodeTypeIdOut == "7") // NodeCC
            {
                //ตรวจหาคนที่จะ CC

                DataTable DtuserCC = new DataTable();
                sqlcmd = " Select  ";
                sqlcmd += " g.Refid  ";
                sqlcmd += " from Sys_Master_Userinflow m  ";
                sqlcmd += " inner join  ";
                sqlcmd += " Sys_Master_Customgroupdetail g on m.RefId = g.Groupid  ";
                sqlcmd += " where m.isdelete = 0 and g.isdelete = 0 and g.Reftype = 'U'  ";
                sqlcmd += " and flowid = '" + ObjCurrentState.FlowId + " ' and formid = '" + ObjCurrentState.FormId + "'";
                DtuserCC = Cn.Select(sqlcmd);
                //ตรวจหาคนที่จะ CC

                //Wtite Sys_Trans_Flowdetail เพื่อให้มี Inprocess , Completed ในกระบะของเขา
                string UserCC = "";
                int Count = 0;
                foreach (DataRow dr in DtuserCC.Rows)
                {
                    UserCC += "|" + dr["Refid"] + "|";
                    if (Count < DtuserCC.Rows.Count)
                    {
                        UserCC += ",";
                    }
                }

                foreach (DataRow dr in DtuserCC.Rows)
                {

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


                    sqlcmd += ",'" + UserCC + "'";  //User ที่ส่ง NodeNameIn


                    sqlcmd += ",'" + ObjCurrentState.NodeNamefrom + "'"; //Node ที่ส่ง
                    sqlcmd += ",'" + ObjCurrentState.NodeTypeNameTH + "'"; //Node ที่ส่ง
                    sqlcmd += ",'" + dr["Refid"] + "'";  //User ที่รับ
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
                    TransFlowDetailId = (int.Parse(TransFlowDetailId) + 1).ToString();
                }
                //เปลี่ยน State เพื่อ Divert ต่อ
                //หา Flow ที่จะ Divert ไปต่อ
                ObjCurrentState.NodeNamefrom = NodeNameOut;
                ObjCurrentState.NodeTypeNameTH = NodeTypeNameTHOut;
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
                //เปลี่ยน State เพื่อ Divert ต่อ
            }
            //CC Section
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

       

        [WebMethod]
        public static ClsEForm010 Selhirecontract(string json)
        {
            DataTable Dt = new DataTable();
            DataTable DtConfig = new DataTable();
            ClsEForm010 Obj = new ClsEForm010();
            DataTable DtPeriodEForm001 = new DataTable();
            SqlConnector cn = new SqlConnector(Connectionstring, "");
            string x = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x");
            string sqlcmd = "Select * from Sys_Edocument_Eform008 m inner join Sys_EDocument_Eform008detail d on m.Transbatchid = d.TransBatchId and m.isdelete = 0 and d.isdelete = 0  where m.id = '" + x + "'";
            Dt = cn.Select(sqlcmd);
            DtConfig = cn.Select("Select * from [Sys_Info_Company] where isdelete =0 ");
            if (HttpContext.Current.Session["EForm010"]  != null)
            {
                Obj = (ClsEForm010)HttpContext.Current.Session["EForm010"];
            }

            Obj.Referhirecontractid = Dt.Rows[0]["id"].ToString();
            Obj.Hirecontractno = Dt.Rows[0]["Documentno"].ToString();

            Obj.Certfullname = DtConfig.Rows[0]["Companyname"].ToString();
            Obj.CertFullAddress = DtConfig.Rows[0]["Companyaddress"].ToString();
            Obj.CertTaxno1 = DtConfig.Rows[0]["Companytaxno"].ToString();

            Obj.Fullname = Dt.Rows[0]["fullname"].ToString();
            Obj.FullAddress = Dt.Rows[0]["address"].ToString();
            
            double Grandtotal = 0;
           
                List<ClsEForm008Period> Periods = new List<ClsEForm008Period>();
                Dt = new DataTable();

                sqlcmd = "Select * from Sys_EDocument_EForm008detail where Transbatchid = '" + ((ClsInfo)HttpContext.Current.Session["Info"]).TransBatchId + "' and isdelete = 0 order by id ";
                Dt = cn.Select(sqlcmd);
                sqlcmd = "Select * from Sys_EDocument_Eform008 m left join Sys_EDocument_Eform008detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Referhirecontractid + "'";
                DtPeriodEForm001 = cn.Select(sqlcmd);
                ClsEForm008Period Objperiod;
                foreach (DataRow s_dr in DtPeriodEForm001.Rows)
                {
                    Objperiod = new ClsEForm008Period();
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
            HttpContext.Current.Session["EFORM010"] = Obj;
            return Obj;
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


        //[WebMethod]
        //public static ClsEForm010 Selrequisition(string json)
        //{
        //    DataTable Dt = new DataTable();
        //    ClsEForm010 Obj = new ClsEForm010();
        //    DataTable DtPeriodEForm001 = new DataTable();
        //    SqlConnector cn = new SqlConnector(Connectionstring, "");
        //    string x = ClsEngine.FindValue(ClsEngine.DeSerialized(json, ':', '|'), "x");
        //    string sqlcmd = "Select * from Sys_Edocument_Eform007 m inner join Sys_EDocument_Eform007detail d on m.Transbatchid = d.TransBatchId and m.isdelete = 0 and d.isdelete = 0  where m.id = '" + x + "'";
        //    Dt = cn.Select(sqlcmd);
        //    Obj = ((ClsEForm010)HttpContext.Current.Session["EForm010"]);
        //    Obj.id = Guid.NewGuid().ToString();
        //    Obj.Documentno = Dt.Rows[0]["Documentno"].ToString();
        //    Obj.Referrequisitionid = Dt.Rows[0]["id"].ToString();
        //    Obj.Refercontractid = Dt.Rows[0]["Refercontractid"].ToString();
        //    Obj.Contid = Dt.Rows[0]["ContId"].ToString();
        //    Obj.fullname = Dt.Rows[0]["fullname"].ToString();
        //    Obj.address = Dt.Rows[0]["address"].ToString();
        //    Obj.Tel = Dt.Rows[0]["Tel"].ToString();
        //    Obj.Cardid = Dt.Rows[0]["Cardid"].ToString();
        //    if (Dt.Rows[0]["Expirydate"].ToString() != "")
        //    {
        //        Obj.Expirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Expirydate"].ToString()), "/", false);
        //    }
        //    else
        //    {
        //        Obj.Expirydate = "";
        //    }


        //    Obj.Bankid = Dt.Rows[0]["Bankid"].ToString();
        //    Obj.Banknameth = Dt.Rows[0]["Banknameth"].ToString();
        //    Obj.Bankaccountno = Dt.Rows[0]["Bankaccountno"].ToString();
        //    Obj.Bankaccountname = Dt.Rows[0]["Bankaccountname"].ToString();
        //    Obj.Bankaccounttype = Dt.Rows[0]["Bankaccounttype"].ToString();
        //    if (Dt.Rows[0]["Contactdate"].ToString() != "")
        //    {
        //        Obj.Contactdate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactdate"].ToString()), "/", false);
        //    }
        //    else
        //    {
        //        Obj.Contactdate = "";
        //    }

        //    if (Dt.Rows[0]["Contactstart"].ToString() != "")
        //    {
        //        Obj.Contactstart = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactstart"].ToString()), "/", false);
        //    }
        //    else
        //    {
        //        Obj.Contactstart = "";
        //    }

        //    if (Dt.Rows[0]["Contactend"].ToString() != "")
        //    {
        //        Obj.Contactend = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Contactend"].ToString()), "/", false);
        //    }
        //    else
        //    {
        //        Obj.Contactend = "";
        //    }
        //    Obj.Effectdate = Dt.Rows[0]["Effectdate"].ToString();
        //    Obj.Fee = double.Parse(Dt.Rows[0]["Fee"].ToString()).ToString("N2");
        //    Obj.Sitename = Dt.Rows[0]["Sitename"].ToString();
        //    Obj.Sitefulladdress = Dt.Rows[0]["Sitefulladdress"].ToString();
        //    Obj.Jobdescription = Dt.Rows[0]["Jobdescription"].ToString();
        //    Obj.Totalamount = double.Parse(Dt.Rows[0]["Totalamount"].ToString()).ToString("N2");

        //    if (Dt.Rows[0]["Finisheddate"].ToString() != "")
        //    {
        //        Obj.Finisheddate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(Dt.Rows[0]["Finisheddate"].ToString()), "/", false);
        //    }
        //    else
        //    {
        //        Obj.Finisheddate = "";
        //    }



        //    Obj.Invoicedate = "";
        //    Obj.Paymentdate = "";
        //    double Grandtotal = 0;
        //    List<ClsEForm007Period> Periods = new List<ClsEForm007Period>();
        //    Dt = new DataTable();
        //    sqlcmd = sqlcmd = "Select * from Sys_EDocument_Eform007 m left join Sys_EDocument_Eform007detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Referrequisitionid + "'";
        //    Dt = cn.Select(sqlcmd);
        //    sqlcmd = "Select * from Sys_EDocument_Eform001 m left join Sys_EDocument_Eform001detail d on m.Transbatchid = d.TransBatchId where m.isdelete = 0 and d.isdelete = 0 and m.id = '" + Obj.Refercontractid + "'";
        //    DtPeriodEForm001 = cn.Select(sqlcmd);
        //    ClsEForm007Period Objperiod;
        //    foreach (DataRow s_dr in DtPeriodEForm001.Rows)
        //    {
        //        Objperiod = new ClsEForm007Period();
        //        Objperiod.Period = s_dr["Period"].ToString();
        //        Objperiod.Periodname = s_dr["Periodname"].ToString();
        //        Objperiod.Amount = Double.Parse(s_dr["Amount"].ToString()).ToString("N2");
        //        Grandtotal += double.Parse(Objperiod.Amount);
        //        if (Dt.Select("Period='" + s_dr["Period"].ToString() + "'").Length > 0)
        //        {
        //            Objperiod.Selected = "x";
        //        }
        //        else
        //        {
        //            Objperiod.Selected = "";
        //        }
        //        Periods.Add(Objperiod);
        //    }
        //    //Detail
        //    Obj.Periods = Periods;
        //    return Obj;
        //}

        [WebMethod]
        public static ClsGridResponse Bind(string Ctrl, long PagePerItem, long CurrentPage, string SortName, string Order, string Column, string Data, string Initial, string SelectCat, string SearchMsg, string EditButton, string DeleteButton, string Panel, string FullRowSelect, string Multiselect, string Criteria, string SearchesDat, string Searchcolumns, string WPanel, string HPanel)
        {
            string Sqlcmd = "";
            string PK = "";
            List<ClsDict> CriterialMapping = new List<ClsDict>();
            ClsGrid Objgrid = new ClsGrid();
            Clsuser Objmy = (Clsuser)HttpContext.Current.Session["My"];
            if (Ctrl == "Gvhirecontract")
            {
                PK = "id";
                Sqlcmd = "Select E.id,Documentno,Fullname,Sitename,Totalamount as Grandtotal from Sys_EDocument_Eform008 E inner join Sys_Trans_Flow F on E.Transbatchid =  F.TransBatchId Where isnull(IsComplete,0) = 1 and E.Isdelete = 0 and F.Isdelete = 0 ";
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