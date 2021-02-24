using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using ERP.LHDesign2020.Class;
using SVframework2016;
namespace ERP.LHDesign2020.Page.EDWF
{
    public partial class Eforms : System.Web.UI.Page
    {
        private static string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["Primary"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static DocumentInfoCollection LoadDocuments(string json)
        {
            DocumentInfoCollection Documentcollection = new DocumentInfoCollection();
            List<ClsDocumentInfo> Documents = new List<ClsDocumentInfo>();
            ClsDocumentInfo ObjDocument = new ClsDocumentInfo();
            string sqlcmd = "";
            DataTable DtNodeInfo = new DataTable();
            DataTable DtSys_Master_FlowDetail = new DataTable();
            string Dat = "";
            string NodeNamefrom = "";
            string NodeTypeIdfrom = "";
            string NodeTypeNameTHFrom = "";
            SqlConnector Cn = new SqlConnector(connectionstring, null);
            string NodeBegintypeId = "1"; //เปิดเอกสาร
            string UserId = "";
            try
            {
                UserId = ((Clsuser)HttpContext.Current.Session["My"]).userid;
                sqlcmd = "select * from [Sys_Master_Userinflow] MF left join Sys_Master_Form M on MF.FormId = M.FormId where MF.NodeTypeId = '" + NodeBegintypeId + "' ";
                sqlcmd += " and  ";
                sqlcmd += " (  ";
                sqlcmd += "    ( RefId = '" + UserId + "' and RefType = 'U' and MF.isdelete = 0) ";
                sqlcmd += " OR (RefId in (Select Orgid from Sys_Master_Organizeuser ou where ou.Userid = '" + UserId + "' and ou.isdelete = 0) and RefType = 'O' and MF.isdelete = 0) ";
                sqlcmd += " OR (RefId in (Select Groupid from Sys_Master_Customgroupdetail where RefId = '" + UserId + "' and RefType = 'U' and isdelete = 0) and RefType = 'G' and MF.isdelete = 0)  ";
                sqlcmd += " OR (RefId in (Select Groupid from Sys_Master_Customgroupdetail where RefId = (select Orgid from Sys_Master_Organizeuser ou where userid = '" + UserId + "' and ou.isdelete = 0) and RefType = 'O' and isdelete = 0) and RefType = 'G' and MF.isdelete = 0))";
                DtNodeInfo = Cn.Select(sqlcmd);
                sqlcmd = "Select * from Sys_Master_FlowDetail Where Isdelete = 0";
                DtSys_Master_FlowDetail = Cn.Select(sqlcmd);
                Cn.Close();
                foreach (DataRow dr in DtNodeInfo.Rows)
                {
                    NodeNamefrom = DtSys_Master_FlowDetail.Select("FormId='" + dr["FormId"].ToString() + "' and NodeTypeIdfrom ='" + NodeBegintypeId + "'")[0]["NodeNamefrom"].ToString();
                    NodeTypeIdfrom = DtSys_Master_FlowDetail.Select("FormId='" + dr["FormId"].ToString() + "' and NodeTypeIdfrom ='" + NodeBegintypeId + "'")[0]["NodeTypeIdfrom"].ToString();
                    NodeTypeNameTHFrom = DtSys_Master_FlowDetail.Select("FormId='" + dr["FormId"].ToString() + "' and NodeTypeIdfrom ='" + NodeTypeIdfrom + "'")[0]["NodeTypeNameTHFrom"].ToString();
                    Dat = "TransBatchId:0|FormId:" + dr["FormId"].ToString() + "|FormNameTH:" + dr["FormNameTH"].ToString() + "|NodeNamefrom:" + NodeNamefrom + "|NodeTypeIdfrom:" + NodeTypeIdfrom + "|NodeTypeNameTHFrom:" + NodeTypeNameTHFrom + "|";
                    Dat = ClsEngine.Base64Encode(Dat);
                    ObjDocument = new ClsDocumentInfo();
                    ObjDocument.FormId = dr["FormId"].ToString();
                    ObjDocument.FormNameTH = dr["FormNameTH"].ToString();
                    ObjDocument.FormDescTH = dr["FormDescTH"].ToString();
                    ObjDocument.FormURL = dr["FormURL"].ToString() + "?Msg=" + Dat;
                    ObjDocument.NodeName = NodeNamefrom;
                    ObjDocument.Err = "";
                    Documents.Add(ObjDocument);
                   
                   
                }
                Documentcollection.Err = "";
                Documentcollection.Documents = Documents;
                return Documentcollection;

            }
            catch(Exception ex)
            {
                Documentcollection.Err = ex.ToString();
                return Documentcollection;
            }
            finally
            {
                DtNodeInfo.Dispose();
                DtSys_Master_FlowDetail.Dispose();
                Cn.Close();
            }

        }
    }
}