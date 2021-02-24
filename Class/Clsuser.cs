using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{

    public class Clsuser
    {
        public string Titlecode { get; set; }
        public string Titlenameth { get; set; }
        public string username { get; set; }
        public string userid { get; set; }
        public string firstnameth { get; set; }
        public string lastnameth { get; set; }
        public string firstnameEN { get; set; }
        public string lastnameEN { get; set; }

        public string tel { get; set; }
        public string email { get; set; }
        public string avartarurl { get; set; }
        public string sigurl { get; set; }
        public string logintype { get; set; }

        public string addressTH { get; set; }
        public string addressEN { get; set; }
        public List<ClsSystem> Systems { get; set; }
        public string positionid { get; set; }
        public string positionnameth { get; set; }
        public string organizeId { get; set; }
        public string organizenameth { get; set; }
        //public List<Clsuserposition> userpositions { get; set; }
        public List<Clsuserrole> userroles { get; set; }
        public string Err { get; set; }
    }
    public class Clsuserrole
    {
        public string roleid { get; set; }
        public string rolenameTH { get; set; }
        public string rolenameEN { get; set; }
        public string Selected { get; set; }
        public List<Clsfeature> features { get; set; }
    }
    public class ClsSystem
    {
        public string Systemid { get; set; }
        public string Systemcode { get; set; }

        public string SystemnameTH { get; set; }
        public string SystemnameEN { get; set; }
        public string SystemnameDescTH { get; set; }
        public string SystemnameDescEN { get; set; }
    }

    public class Clsfeature
    {
        public string functionid { get; set; }
        public string systemid { get; set; }
        public string systemcode { get; set; }
        public string systemnameTH { get; set; }
        public string systemnameEN { get; set; }
        public string functioncode { get; set; }
        public string functionnameTH { get; set; }
        public string functionnameEN { get; set; }
        public string ctrl { get; set; }
        public string isedit { get; set; }
        public string isview { get; set; }
    }
    public class Clsuserposition
    {
        public string positionid { get; set; }
        public string orgid { get; set; }
        public string positionnameTH { get; set; }
        public string positionnameEN { get; set; }
        public string organizenameTH { get; set; }
        public string organizenameEN { get; set; }
        public string priority { get; set; }
    }
}