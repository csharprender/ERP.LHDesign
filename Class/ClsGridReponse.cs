using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVframework2016;
using System.Collections;
using System.Data;
namespace ERP.LHDesign2020.Class
{
    public class ClsGridResponse
    {
        public string Ctrl { get; set; }
        public string PagePerItem { get; set; }
        public string CurrentPage { get; set; }
        public string SortName { get; set; }
        public string Order { get; set; }
        public string Column { get; set; }
        public string Data { get; set; }
        public string Initial { get; set; }
        public string EditButton { get; set; }
        public string DeleteButton { get; set; }
        public string FullRowSelect { get; set; }
        public string Multiselect { get; set; }
        public DataTable RawData { get; set; }
        public string Criteria { get; set; }
        public List<ClsDict> FullRowSelectEvent { get; set; }
        public List<List<ClsDict>> ResData { get; set; }
        public ArrayList Selection { get; set; } // ใช้เก็บ Collections จาก Checkbox
        public string SearchesDat { get; set; }
        public string Searchcolumns { get; set; }
        public string WPanel { get; set; }
        public string HPanel { get; set; }
        public string Panel { get; set; }
        public ArrayList PKValues { get; set; }


    }
}