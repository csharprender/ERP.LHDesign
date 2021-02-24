using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERP.LHDesign2020.Class
{
    public class ClsEForm010 : ClsEForm
    {
        public string id { get; set; }

        public string TransFlowDetailId { get; set; }
        public string Referhirecontractid { get; set; }
        public string Hirecontractno { get; set; }
        public string Documentdate { get; set; }
        
        public string Certfullname { get; set; }
        public string CertTaxno1 { get; set; }
        public string CertFullAddress { get; set; }
        public string CertTaxno2 { get; set; }
        public string Fullname { get; set; }
        public string Taxno1 { get; set; }
        public string FullAddress { get; set; }
        public string Taxno2 { get; set; }
        public string Orderinform { get; set; }

        public string Docno { get; set; }

        public string Bookno { get; set; }
        public string PND1A { get; set; }
        public string PND1AExtra { get; set; }
    public string PND2 { get; set; }
    public string PND3 { get; set; }
    public string PND2A { get; set; }
    public string PND3A { get; set; }
    public string PND53 { get; set; }

        public string TotalAmount { get; set; }
        public string TotalTax { get; set; }
    public string PayforTeacherAidFund { get; set; }
    public string PayforSocialSecurityFund { get; set; }
    public string PayforProvidentFund { get; set; }
    public string PayMethodWHT { get; set; }
    public string PayMethodRecuring { get; set; }
    public string PayMethodOncetime { get; set; }
    public string PayeenameTH { get; set; }
    public string Paymentdate { get; set; }
        public string PayforOther { get; set; }
        public string PayforOtherRemark { get; set; }

        public string PayDate_1 { get; set; }
    public string PayAmount_1 { get; set; }
    public string PayWHT_1 { get; set; }
    public string PayDate_2 { get; set; }
    public string PayAmount_2 { get; set; }
    public string PayWHT_2 { get; set; }
    public string PayDate_3 { get; set; }
    public string PayAmount_3 { get; set; }
    public string PayWHT_3 { get; set; }
    public string PayDate_4A { get; set; }
    public string PayAmount_4A { get; set; }
    public string PayWHT_4A { get; set; }
    public string PayDate_4B1_1 { get; set; }
    public string PayAmount_4B1_1 { get; set; }
    public string PayWHT_4B1_1 { get; set; }
    public string PayDate_4B1_2 { get; set; }
    public string PayAmount_4B1_2 { get; set; }
    public string PayWHT_4B1_2 { get; set; }
    public string PayDate_4B1_3 { get; set; }
    public string PayAmount_4B1_3 { get; set; }
    public string PayWHT_4B1_3 { get; set; }
    public string PayDate_4B1_4 { get; set; }
    public string PayAmount_4B1_4 { get; set; }
    public string PayWHT_4B1_4 { get; set; }
    public string PayRemark_4B1_4 { get; set; }
    public string PayDate_4B2_1 { get; set; }
    public string PayAmount_4B2_1 { get; set; }
    public string PayWHT_4B2_1 { get; set; }
    public string PayDate_4B2_2 { get; set; }
    public string PayAmount_4B2_2 { get; set; }
    public string PayWHT_4B2_2 { get; set; }
    public string PayDate_4B2_3 { get; set; }
    public string PayAmount_4B2_3 { get; set; }
    public string PayWHT_4B2_3 { get; set; }
    public string PayDate_4B2_4 { get; set; }
    public string PayAmount_4B2_4 { get; set; }
    public string PayWHT_4B2_4 { get; set; }
    public string PayRemark_4B2_4 { get; set; }

        public string PayDate_4B2_5 { get; set; }
    public string PayAmount_4B2_5 { get; set; }
    public string PayWHT_4B2_5 { get; set; }
    public string PayRemark_4B2_5 { get; set; }
    public string PayDate_5 { get; set; }
    public string PayAmount_5 { get; set; }
    public string PayWHT_5 { get; set; }
    public string PayDate_6 { get; set; }
    public string PayAmount_6 { get; set; }
    public string PayWHT_6 { get; set; }
    public string PayRemark_6 { get; set; }

       



        public List<ClsEForm008Period> Periods { get; set; }
        public List<ClsAttachment> Attachments { get; set; }

    }
}