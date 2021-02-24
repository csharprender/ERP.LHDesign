using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class CLSEForm004 : ClsEForm
    {
        public string Customerid { get; set; }

        public string Customername { get; set; }
        public string Customeraddress { get; set; }
        public string Customertel { get; set; }
        public string Offerdate { get; set; }
        public string Saleid { get; set; }
        public string Salename { get; set; }
        public string Saletel { get; set; }
        public List<Clsofferdetail> OfferDetails { get; set; }
        public string Materialtotalprice { get; set; }
        public string Manpowertotalprice { get; set; }
        public string Totalprice { get; set; }
        public string vatamount { get; set; }

        public string Grandtotalprice { get; set; }

        public List<ClsAttachment> Attachments { get; set; }


    }
    public class Clsofferdetail
    {
        public string Serviceid { get; set; }
        public string Servicenameth { get; set; }
        public List<Clsdetail> Details { get; set; }
        public string Totalprice { get; set; }
        public string Errormessage { get; set; }
    }
    public class Clsdetail
    {
        public string itemid { get; set; }
        public string itemname { get; set; }
        public string Quann { get; set; }
        public string Unitid { get; set; }
        public string Unitname { get; set; }
        public string Materialprice { get; set; }
        public string Materialtotalprice { get; set; }

        public string Manpowerprice { get; set; }
        public string Manpowertotalprice { get; set; }
        public string Totalprice { get; set; }
    }

  
}
