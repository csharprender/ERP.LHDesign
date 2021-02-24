using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using SVframework2016;
using System.Configuration;
using ERP.LHDesign2020.Class;
namespace ERP.LHDesign2020.Page.EDWF.Forms
{
    public partial class Control : System.Web.UI.MasterPage
    {
        public static string Connectionstring = ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ClsDict> Msgs = new List<ClsDict>();
            ClsInfo ObjInfo = new ClsInfo();
            string NodeDescfromTH = "";
            DataTable DtFlowFlowDetail = new DataTable();
            DataTable DtPending = new DataTable();
            DataTable DtFlow = new DataTable();
            string sqlcmd = "";
            SqlConnector Cn = new SqlConnector(Connectionstring,"");

            if (HttpContext.Current.Session["My"] == null)
            {
                Response.Write("<p class='form-timeout' style='text-align:center;color:red;font-size:16px;'>Session is timeout. Please close this window and Login again.</p>");
                return;
            }
            if (Request["Msg"] == null)
            {
                Response.Write("<p class='form-timeout' style='text-align:center;color:red;font-size:16px;'>Tranmission Msg is failed. Please close this window and open again.</p>");
                return;
            }
            Msgs = ClsEngine.DeSerialized(ClsEngine.Base64Decode(Request["Msg"]), ':', '|');
            if (ClsEngine.FindValue(Msgs, "TransBatchId") == "0")
            {
                SpDocumentCreateDate.InnerHtml = System.DateTime.Now.ToShortDateString();
            }
            else
            {
                SpDocumentCreateDate.InnerHtml = "-";
            }
            string TransFormId = "";
            if (ClsEngine.FindValue(Msgs, "FormId") == "1") //TransApproveAttachmentForm 
            {
                DataTable DtTransform = new DataTable();
                sqlcmd = "Select * from Sys_Edocument_Eform001 Where TransbatchId ='" + ClsEngine.FindValue(Msgs, "TransBatchId") + "'";
                DtTransform = Cn.Select(sqlcmd);
                if (DtTransform.Rows.Count == 0)
                {
                    TransFormId = "";
                }
                else
                {
                    TransFormId = DtTransform.Rows[0]["Id"].ToString();
                }
            }
            sqlcmd = "Select * from Sys_Master_FlowDetail where FormId = '" + ClsEngine.FindValue(Msgs, "FormId") + "' and isdelete = 0";
            DtFlowFlowDetail = Cn.Select(sqlcmd);


            sqlcmd = "Select tf.*,isnull(up.firstnameth,'') + ' ' + isnull(up.lastnameth,'') as fullnamecreateby  from Sys_Trans_Flow tf inner join sys_master_userprofile up on tf.Createby = up.id  Where  tf.TransbatchId ='" + ClsEngine.FindValue(Msgs, "TransBatchId") + "' and tf.isdelete = 0";
            DtFlow = Cn.Select(sqlcmd);
            if (DtFlow.Rows.Count > 0)
            {
                ObjInfo.Subject = DtFlow.Rows[0]["subject"].ToString();
                ObjInfo.CreateBy = DtFlow.Rows[0]["fullnamecreateby"].ToString();
                ObjInfo.Iscancel = DtFlow.Rows[0]["Iscancel"].ToString();
                ObjInfo.Iscomplete = DtFlow.Rows[0]["Iscomplete"].ToString();
            }
            else
            {
                ObjInfo.Subject = "สร้างเอกสารใหม่";
                ObjInfo.CreateBy = ((Clsuser)(HttpContext.Current.Session["My"])).firstnameth + ' ' + ((Clsuser)(HttpContext.Current.Session["My"])).lastnameth;
                ObjInfo.Iscancel = "0";
                ObjInfo.Iscomplete = "0";
            }
            ObjInfo.DataTableMasterFlowDetail = DtFlowFlowDetail;
            ObjInfo.TransBatchId = ClsEngine.FindValue(Msgs, "TransBatchId");
            ObjInfo.FlowId = DtFlowFlowDetail.Rows[0]["FlowId"].ToString();
            ObjInfo.FormId = ClsEngine.FindValue(Msgs, "FormId");
            ObjInfo.FormNameTH = ClsEngine.FindValue(Msgs, "FormNameTH");
            ObjInfo.NodeNamefrom = ClsEngine.FindValue(Msgs, "NodeNamefrom");
            ObjInfo.NodeTypeId = ClsEngine.FindValue(Msgs, "NodeTypeIdFrom");
            ObjInfo.NodeTypeNameTH = ClsEngine.FindValue(Msgs, "NodeTypeNameTHFrom");
            //Get for PendingBy
            sqlcmd = "select * from [Sys_Trans_Flowdetail] where isnull(IsPass,'') = '' and TransBatchId ='" + ObjInfo.TransBatchId + "'";
            DtPending = Cn.Select(sqlcmd);
            if (DtPending.Rows.Count > 0)
            {
                ObjInfo.NodeNameTo = DtPending.Rows[0]["NodeNameOut"].ToString();
                ObjInfo.NodeTypeNameToTH = DtPending.Rows[0]["NodeTypeNameOutTH"].ToString();
            }
            Cn.Close();
            ObjInfo.TransFormId = TransFormId;
            HttpContext.Current.Session["Info"] = ObjInfo;
            DtFlowFlowDetail.Dispose();


            if (ClsEngine.FindValue(Msgs, "INQ") != "1")
            {
                SpINQStatus.InnerHtml = ClsEngine.FindValue(Msgs, "Status");
                SpINQStatus.Visible = true;
                NodeDescfromTH = ObjInfo.DataTableMasterFlowDetail.Select("NodeNamefrom ='" + ObjInfo.NodeNamefrom + "'")[0]["NodeDescfromTH"].ToString();
            }
            else
            {
                NodeDescfromTH = "Inquiry Mode";
            }

            SpFormName.InnerHtml = ObjInfo.FormNameTH;
            //DivformTitle.InnerHtml = ObjInfo.FormNameTH;
            SpCurrentNode.InnerHtml = NodeDescfromTH;

            HdNodeName.Value = ObjInfo.NodeNamefrom;
            HdINQ.Value = ClsEngine.FindValue(Msgs, "INQ");
        }

    }
}