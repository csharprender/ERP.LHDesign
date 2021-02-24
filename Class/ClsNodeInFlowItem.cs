using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsNodeInFlowItem
    {
        public string No { get; set; }
        public string NodeNameTH { get; set; }
        public string NodeTypeId { get; set; }
        public string NodeTypeNameTH { get; set; }
        public string NodePicURL { get; set; }
        public List<Clsuser> Users { get; set; }
        public string FullName { get; set; }

    }
}