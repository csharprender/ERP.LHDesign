using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsEForm005 : ClsEForm
    {
        public string id { get; set; }
        public string Referquotationid { get; set; } //EFORM004
        
        public string Customerid { get; set; }
        public string Customername { get; set; }
        public string Customeraddress { get; set; }
        public string Customertel { get; set; }
        public string Offerdate { get; set; }
        public string Saleid { get; set; }
        public string Salename { get; set; }
        public string Saletel { get; set; }

        public string Grandtotal { get; set; }
        public List<ClsAttachment> Attachments { get; set; }
        public List<ClsEForm005Invoice> Invoices { get; set; }

    }

    public class  ClsEForm005Invoice
    {
        public string id { get; set; }
        public string Invoiceno { get; set; }
        public string Vendorid { get; set; }
        public string Vendorcode { get; set; }
        public string Vendorname { get; set; }
        public string Taxno { get; set; }
        public string Orderdate { get; set; }
        public string Receivedate { get; set; }
        public string Vendorfulladdress { get; set; }
        public List<ClsEForm005invoiceitem> Invoiceitems { get; set; }

    }
    public class ClsEForm005invoiceitem
    {
        public string id { get; set; }
        public string Documentno { get; set; }
        public string Referquotationitemid { get; set; }
        public string Orderdate { get; set; }
        public string Receivedate { get; set; }
        public string Serviceid { get; set; }
        public string Materialid { get; set; }
        public string Materialnameth { get; set; }
        public string Servicenameth { get; set; }
        public string Quann { get; set; }
        public string Unitid { get; set; }
        public string Unitname { get; set; }
        public string Materialprice { get; set; }
        public string Materialtotalprice { get; set; }
        public string Manpowerprice { get; set; }
        public string Manpowertotalprice { get; set; }
        public string Totalprice { get; set; }
    
    }
}