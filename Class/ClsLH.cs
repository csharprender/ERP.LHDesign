using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVframework2016;
using System.Data;
namespace ERP.LHDesign2020.Class
{
    public class ClsLH
    {

        public List<ClsService> Getchildservicebyid(ref DataTable Dtservice, String Serviceid)
        {

            ClsService Obj;
            List<ClsService> Objs = new List<ClsService>();
            DataRow[] Drsservice;
            Drsservice = Dtservice.Select("Parentserviceid = '" + Serviceid + "'");
            foreach (DataRow Dr in Drsservice)
            {
                Obj = new ClsService();
                Obj.serviceid = Dr["id"].ToString();
                Obj.servicenameth = Dr["servicenameth"].ToString();
                Obj.servicenameen = Dr["servicenameen"].ToString();
                Obj.Isgroup = Dr["Isgroup"].ToString();
                Obj.Services = Getchildservicebyid(ref Dtservice, Obj.serviceid);
                Objs.Add(Obj);
            }
            return Objs;
        }
        public List<ClsService> Getservicebyid(ref SqlConnector cn, string id)
        {
            DataTable Dtservice = new DataTable();
            ClsService Obj;
            List<ClsService> Objs = new List<ClsService>();
            string Sqlcmd = "";
            Sqlcmd = "";
            Sqlcmd = "Select * from Sys_Master_Service where isdelete = 0 order by id";
            Dtservice = cn.Select(Sqlcmd);
            DataRow[] Drsservice;
            Drsservice = Dtservice.Select("id = '" + id + "'");
            Obj = new ClsService();
            Obj.serviceid = Drsservice[0]["id"].ToString();
            Obj.servicenameth = Drsservice[0]["servicenameth"].ToString();
            Obj.servicenameen = Drsservice[0]["servicenameen"].ToString();
            Obj.Isgroup = Drsservice[0]["Isgroup"].ToString();
            Obj.Services = Getchildservicebyid(ref Dtservice, Obj.serviceid);
            Objs.Add(Obj);
            return Objs;
        }
        public List<ClsService> Getchildservice(ref DataTable Dtservice,String Serviceid,string kwd)
        {

            ClsService Obj;
            List<ClsService> Objs = new List<ClsService>();

            DataRow[] Drsservice;
            Drsservice = Dtservice.Select("Parentserviceid = '" + Serviceid + "'");
            foreach (DataRow Dr in Drsservice)
            {
                Obj = new ClsService();
                Obj.serviceid = Dr["id"].ToString();
                Obj.servicenameth = Dr["servicenameth"].ToString();
                Obj.servicenameen = Dr["servicenameen"].ToString();
                Obj.Isgroup = Dr["Isgroup"].ToString();
                Obj.Services = Getchildservice(ref Dtservice, Obj.serviceid, kwd);
                if (kwd != "" && Obj.Isgroup == "False") //Search mode
                {
                    if (Obj.servicenameth.Contains(kwd) || Obj.servicenameen.Contains(kwd))
                    {
                        Objs.Add(Obj);
                    }
                }
                else
                {
                    Objs.Add(Obj);
                }
                
            }
            return Objs;
        }
        public List<ClsService> Getservice(ref SqlConnector cn,string kwd)
        {
            DataTable Dtservice = new DataTable();
            ClsService Obj;
            List<ClsService> Objs = new List<ClsService>();
            string Sqlcmd = "";
            Sqlcmd = "";
            Sqlcmd = "Select * from Sys_Master_Service where isdelete = 0 ";
            if (kwd != "")
            {
                Sqlcmd += " and servicenameth like '%" + kwd + "%'";
            }
            Sqlcmd += " order by id";
            Dtservice = cn.Select(Sqlcmd);
            DataRow[] Drsservice;
            Drsservice = Dtservice.Select("Parentserviceid = 0");
            foreach (DataRow Dr in Drsservice)
            {
                Obj = new ClsService();
                Obj.serviceid = Dr["id"].ToString();
                Obj.servicenameth = Dr["servicenameth"].ToString();
                Obj.servicenameen = Dr["servicenameen"].ToString();
                Obj.Isgroup  = Dr["Isgroup"].ToString();
                Obj.Services = Getchildservice(ref Dtservice, Obj.serviceid,kwd);
                Objs.Add(Obj);
            }
            return Objs;
        }
        public List<ClsVendor> GetVendoroutsource(ref SqlConnector cn, string id)
        {
            string sqlcmd = "select * from sys_master_Vendoroutsource where isdelete = 0  and id  = '" + id + "'";
            ClsVendor Obj = new ClsVendor();
            List<ClsVendor> Objs = new List<ClsVendor>();
            DataTable Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            foreach (DataRow dr in Dt.Rows)
            {
                Obj = new ClsVendor();
                Obj.Id = dr["Id"].ToString();
                Obj.Fullname = dr["Fullname"].ToString();
                Obj.Address = dr["Address"].ToString();
                Obj.Tel = dr["Tel"].ToString();
                Obj.CardID = dr["IDCard"].ToString();
                Obj.Exprirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(dr["CardExprirydate"].ToString()), "-", false);
                Obj.Bank = dr["Bankid"].ToString();
                Obj.Bankaccountname = dr["Bankaccountname"].ToString();
                Obj.Bankaccountno = dr["Bankaccountno"].ToString();
                Obj.Bankaccounttype = dr["Bankaccounttype"].ToString();
                Objs.Add(Obj);
            }
            return Objs;

        }
        public List<Clscontactor> GetContactor(ref SqlConnector cn, string id)
        {
            string sqlcmd = "select * from sys_master_contactor where isdelete = 0  and id  = '" + id + "'";
            Clscontactor Obj = new Clscontactor();
            List<Clscontactor> Objs = new List<Clscontactor>();
            DataTable Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            foreach (DataRow dr in Dt.Rows)
            {
                Obj = new Clscontactor();
                Obj.Id = dr["Id"].ToString();
                Obj.Fullname = dr["Fullname"].ToString();
                Obj.Address = dr["Address"].ToString();
                Obj.Tel = dr["Tel"].ToString();
                Obj.CardID = dr["IDCard"].ToString();
                Obj.Exprirydate = ClsEngine.Convertdate2ddmmyyyy(DateTime.Parse(dr["CardExprirydate"].ToString()), "-", false);
                Obj.Bank = dr["Bankid"].ToString();
                Obj.Bankaccountname = dr["Bankaccountname"].ToString();
                Obj.Bankaccountno = dr["Bankaccountno"].ToString();
                Obj.Bankaccounttype = dr["Bankaccounttype"].ToString();
                Objs.Add(Obj);
            }
            return Objs;

        }
        public List<ClsCustomer> GetCustomer(ref SqlConnector cn,string id)
        {
            string sqlcmd = "select * from sys_master_customer where isdelete = 0  and id  = '" + id +  "'";
            ClsCustomer Obj = new ClsCustomer();
            List<ClsCustomer> Objs = new List<ClsCustomer>();
            DataTable Dt = new DataTable();
            Dt = cn.Select(sqlcmd);
            foreach(DataRow dr in Dt.Rows)
            {
                Obj = new ClsCustomer();
                Obj.Id = dr["Id"].ToString();
                Obj.Code = dr["Code"].ToString();
                Obj.Fullname = dr["Fullname"].ToString();
                Obj.Address = dr["Address"].ToString();
                Obj.Tel = dr["Tel"].ToString();
                Objs.Add(Obj);
            }
            return Objs;

        }
    }
}