using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsEForm006 : ClsEForm
    {
        public string id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Grandtotalamount { get; set; }
        public List<ClsPrettycashItem> PrettycashItems { get; set; }
        public List<ClsAttachment> Attachments { get; set; }
    }
    public class ClsPrettycashItem
    {
        public string itemid { get; set; }
        public string itemdesc { get; set; }
        public string itemamount { get; set; }
    }
}