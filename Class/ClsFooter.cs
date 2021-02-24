using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsFooter
    {
        public string Id { get; set; }
        public string NodeTypeNameTH { get; set; }
        public string NodeMultipleId { get; set; }
        public string SignatureURL { get; set; }
        public string Remark { get; set; }
        public string ActionResultValue { get; set; }
        public string ActionResultNameTH { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionStringDate { get; set; }
        public string ActionById { get; set; }

        public string ActionByTitleCode { get; set; }
        public string ActionByTitleNameTH { get; set; }
        public string ActionByFirstNameTH { get; set; }
        public string Ismyremark { get; set; }

        public string ActionByLastNameTH { get; set; }
        public string ActionByPositionId { get; set; }
        public string ActionByPositionNameTH { get; set; }
        public string ActionByOrganizeId { get; set; }
        public string ActionByOrganizeNameTH { get; set; }

    }
}