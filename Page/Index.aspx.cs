using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using ERP.LHDesign2020.Class;
using SVframework2016;
namespace ERP.LHDesign2020.Page
{
    public partial class Index : System.Web.UI.Page
    {


        
        [WebMethod]
        public static string Logout(string json)
        {
            HttpContext.Current.Session.Clear();
            return "";
        }
        [WebMethod]
        public static Clsuser Loadprofile(string json)
        {
            if (HttpContext.Current.Session["My"] == null)
            {
                Clsuser Objuser = new Clsuser();
                Objuser.Err = "Session is expired,Please re-login ahain";
                return Objuser;
            }
            ((Clsuser)HttpContext.Current.Session["My"]).Err = "";
            return (Clsuser)HttpContext.Current.Session["My"];
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}