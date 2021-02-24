using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class CLSEForm001 : ClsEForm
    {
        public string Subject { get; set; }
        
        public string ContId { get; set; }
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
        
        public List<ClsEForm001Period> Periods { get; set; }
        public List<ClsAttachment> Attachments { get; set; }

    }
}