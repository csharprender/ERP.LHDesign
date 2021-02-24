using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsEForm007 :ClsEForm
    {
        public string id { get; set; }
        public string Refercontractid { get; set; } //EFORM001
        public string Contid { get; set; }
        public string fullname { get; set; }
        public string address { get; set; }
        public string Tel { get; set; }
        public string Cardid { get; set; }
        public string Expirydate { get; set; }
        public string Bankaccountno { get; set; }
        public string Bankaccounttype { get; set; }
        public string Bankaccountname { get; set; }
        public string Bankid { get; set; }
        public string Banknameth { get; set; }
        public string Contactdate { get; set; }
        public string Contactstart { get; set; }
        public string Contactend { get; set; }
        public string Effectdate { get; set; }
        public string Sitename { get; set; }
        public string Sitefulladdress { get; set; }
        public string Jobdescription { get; set; }
        public string Totalamount { get; set; }
        public string Finisheddate { get; set; }
        public string Fee { get; set; }
        public string Grandtotal { get; set; }
        public string Invoicedate { get; set; }
        public string Paymentdate { get; set; }

        public List<ClsAttachment> Attachments { get; set; }
        public List<ClsEForm007Period> Periods { get; set; }
    }

    public class ClsEForm007Period
    {
        public string Period { get; set; }
        public string Periodname { get; set; }
        public string Amount { get; set; }
        public string Selected { get; set; }
    }
}