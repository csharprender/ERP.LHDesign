using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVframework2016;
namespace ERP.LHDesign2020.Class
{
    public class ClsNodeMulipleDetailInfo
    {
        public string NodeMultipleDetailId { get; set; }
        public string MultipleRule { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<ClsDict> Directions { get; set; }
        public string Remark { get; set; }
    }
}