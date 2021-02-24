using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsService
    {
        public string parentserviceid { get; set; }
        public string serviceid { get; set; }
        public string servicenameth { get; set; }
        public string servicenameen { get; set; }
        public string Isgroup { get; set; }
        public List<ClsService> Services { get; set; }
    }
}