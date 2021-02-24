﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace ERP.LHDesign2020.Class
{
    public class ClsInfo
    {
        public string TransBatchId { get; set; }
        public string TransFormId { get; set; }
        public string FlowId { get; set; }
        public string FormId { get; set; }
        public string FormNameTH { get; set; }
        public string NodeNamefrom { get; set; }
        public string NodeTypeNameTH { get; set; }
        public string NodeTypeId { get; set; }

        public string NodeNameTo { get; set; }
        public string NodeTypeNameToTH { get; set; }
        public DataTable DataTableMasterFlowDetail { get; set; }
        public string Iscancel { get; set; }
        public string Iscomplete { get; set; }
        public string CreateBy { get; set; }
        public string Subject { get; set; }
    }
}