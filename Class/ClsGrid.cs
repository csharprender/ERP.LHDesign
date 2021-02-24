using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SVframework2016;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HPSF;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.XSSF.Util;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Model;
namespace ERP.LHDesign2020.Class
{
    public class ClsGrid
    {
        public string GetTotalRecord(string Ctrl)
        {
            try
            {
                return ((DataTable)HttpContext.Current.Session["RAW_" + Ctrl]).Rows.Count.ToString();
            }
            catch
            {
                return "0";
            }
        }
        public List<ClsDict> Load2GridPanel(ref SqlConnector Cn, string Ctrl, string dat, string sqlcmd)
        {
            //dat ค่าที่ต้องการใช้
            List<ClsDict> Dicts = new List<ClsDict>();
            ClsDict ObjDict;
            DataTable Dt = new DataTable();
            List<ClsDict> Criterias = new List<ClsDict>();
            System.Collections.ArrayList Arrcmd = new System.Collections.ArrayList();
            foreach (ClsDict ObjCriteria in Criterias)
            {
                sqlcmd += " and " + ObjCriteria.Name + " = '" + ObjCriteria.Val + "'";
            }
            Dt = Cn.Select(sqlcmd);
            if (Dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in Dt.Columns)
                {
                    ObjDict = new ClsDict();
                    ObjDict.Name = dc.ColumnName;
                    ObjDict.Val = Dt.Rows[0][dc.ColumnName].ToString().Trim();
                    Dicts.Add(ObjDict);
                }
            }
            return Dicts;
        }
        public static DataTable GetData(ref SqlConnector Cn, ref List<ClsDict> FullRowSelects, string Ctrl, long PagePerItem, long CurrentPage, string SortName, string Order, string SearchCat, string SearchMsg, string Data, string EditButton, string DeleteButton, string Panel, string FullRowSelect, string Multiselect, string Criteria, string PK, string SqlCommand, List<ClsDict> CriteriaValueMapping, string SearchesDat, string Searchcolumns, string WPanel, string HPanel)
        {
            string sqlcmd = "";

            string[] ArrSearchCat = { };
            int i = 0;
            string[] ArrData = Data.Split(',');
            if (SearchCat != "" && SearchCat != "undefined")
            {
                ArrSearchCat = SearchCat.Split(',');
            }
            if (SearchMsg == "undefined")
            {
                SearchMsg = "";
            }
            DataTable DtResult = new DataTable();
            DataTable Dt = new DataTable();
            foreach (string col in Data.Split(','))
            {
                DtResult.Columns.Add(col);
                DtResult.Columns[col].MaxLength = 2000;
            }
            List<ClsDict> CriteriaValues = new List<ClsDict>();
            //And Criteria
            if (Criteria != "")
            {
                if (Criteria.Substring(0, 1) != "!")
                {
                    CriteriaValues = ClsEngine.DeSerialized(Criteria, ':', '|');
                }
            }
            if (SqlCommand.Contains("Exec")) //Store Procedure
            {
                sqlcmd = SqlCommand;//"Select ProjectId,ProjectCode,ProjectNameTH from Sys_Master_Project Where Isdelete = 0 ";
                Dt = Cn.Select(sqlcmd);
            }
            else if (!SqlCommand.Contains("Session!"))
            {
                //And Criteria

                sqlcmd = SqlCommand;//"Select ProjectId,ProjectCode,ProjectNameTH from Sys_Master_Project Where Isdelete = 0 ";
                foreach (ClsDict CriVal in CriteriaValues)
                {

                    foreach (ClsDict ObjMap in CriteriaValueMapping)
                    {
                        if (CriVal.Name.ToLower() == ObjMap.Name.ToLower())
                        {
                            CriVal.Name = ObjMap.Val;
                        }
                    }
                    if (CriVal.Val.Trim() != "")
                    {
                        sqlcmd += " and (" + CriVal.Name + " in ('" + CriVal.Val.Replace(",", "','") + "') )";
                    }
                }
                if (SearchCat != "" && SearchCat != "null" && SearchCat != "undefined")
                {
                    sqlcmd += " And (";
                    if (ArrSearchCat.Length > 1) // *****  Array 1 member cannot use foreach loop ***** 
                    {
                        foreach (string str in ArrSearchCat)
                        {

                            if (str != "" && str != "null")
                            {
                                if (i != 0)
                                {
                                    sqlcmd += " Or ";
                                }
                                sqlcmd += " (" + str + " like '%" + SearchMsg + "%') ";
                                i += 1;
                            }

                        }
                    }
                    else
                    {
                        sqlcmd += " (" + SearchCat + " like '%" + SearchMsg + "%') ";
                    }
                    sqlcmd += " )";

                }
                else
                {
                    if (SearchesDat != "")
                    {
                        sqlcmd += " And (";
                        foreach (string str in SearchesDat.Split(','))
                        {

                            if (str != "" && str != "null")
                            {
                                if (i != 0)
                                {
                                    sqlcmd += " Or ";
                                }
                                sqlcmd += " (" + str + " like '%" + SearchMsg + "%') ";
                                i += 1;
                            }

                        }
                        sqlcmd += " )";
                    }
                }
                if (SortName != "")
                {
                    if (SortName.Contains("N!"))
                    {
                        sqlcmd += " Order by Cast(" + SortName.Replace("N!", "") + " as int ) " + Order;
                    }
                    else
                    {
                        sqlcmd += " Order by " + SortName + " " + Order;
                    }
                }
                Dt = Cn.Select(sqlcmd);
            }

            else //Session
            {
                i = 0;
                if (SearchesDat != "" && SearchesDat != "undefined")
                {
                    ArrSearchCat = SearchesDat.Split(',');
                }
                string _sqlcmd = " 1=1 ";
                foreach (ClsDict CriVal in CriteriaValues)
                {
                    if (CriVal.Val.Trim() != "")
                    {
                        _sqlcmd += " and (" + CriVal.Name + " in (" + CriVal.Val + ") )";
                    }
                }
                if (SearchesDat != "" && Data != "null" && SearchesDat != "undefined")
                {
                    _sqlcmd += " And (";
                    if (ArrSearchCat.Length > 1) // *****  Array 1 member cannot use foreach loop ***** 
                    {
                        foreach (string str in ArrSearchCat)
                        {

                            if (str != "" && str != "null")
                            {
                                if (i != 0)
                                {
                                    _sqlcmd += " Or ";
                                }
                                _sqlcmd += " (" + str + " like '%" + SearchMsg + "%') ";
                            }
                            i += 1;
                        }
                    }
                    else
                    {
                        _sqlcmd += " (" + SearchesDat + " like '%" + SearchMsg + "%') ";
                    }
                    _sqlcmd += " )";

                }
                else
                {
                    if (SearchesDat != "")
                    {
                        _sqlcmd += " And (";
                        foreach (string str in SearchesDat.Split(','))
                        {

                            if (str != "" && str != "null")
                            {
                                if (i != 0)
                                {
                                    _sqlcmd += " Or ";
                                }
                                _sqlcmd += " (" + str + " like '%" + SearchMsg + "%') ";
                            }
                            i += 1;
                        }
                        _sqlcmd += " )";
                    }
                }
                try
                {
                    Dt = ((DataTable)HttpContext.Current.Session[SqlCommand]).Select(_sqlcmd).CopyToDataTable();
                    if (SortName != "")
                    {

                        DataView dv = Dt.DefaultView;
                        if (SortName.Contains("N!"))
                        {
                            dv.Sort = SortName.Replace("N!", "") + " " + Order;

                        }
                        else
                        {
                            dv.Sort = SortName + " " + Order;
                        }


                        Dt = dv.ToTable();
                    }
                }
                catch
                {
                    Dt = (DataTable)HttpContext.Current.Session[SqlCommand];
                    Dt.Rows.Clear();
                    HttpContext.Current.Session[SqlCommand] = Dt;
                }
            }

            HttpContext.Current.Session["RAW_" + Ctrl] = Dt;
            DtResult = Dt.Clone();
            int j = 0;
            int Count = 0;
            int Start = ((int.Parse(PagePerItem.ToString()) * (int.Parse(CurrentPage.ToString()) - 1)) + 1);
            int End = int.Parse(PagePerItem.ToString()) * int.Parse(CurrentPage.ToString());
            if (End > Dt.Rows.Count)
            {
                End = Dt.Rows.Count;
            }
            Count = (Start - j);
            for (j = (Start - j); j <= End; j++)
            {
                DtResult.ImportRow(Dt.Rows[Count - 1]);
                Count++;
            }
            if (DtResult.Rows.Count != 0)
            {
                ArrayList ArrPK = new ArrayList();
                foreach (DataRow dr in DtResult.Rows) //Keep PK
                {
                    foreach (DataRow m_dr in DtResult.Rows)
                    {
                        ArrPK.Add(PK + ":" + m_dr[PK].ToString().Trim());
                    }
                }
                HttpContext.Current.Session["PK"] = ArrPK;
                if (FullRowSelect != "" || Multiselect != "")
                {
                    foreach (DataRow dr in DtResult.Rows)
                    {
                        ClsDict ObjfullSelect = new ClsDict();
                        ObjfullSelect.Name = PK;
                        ObjfullSelect.Val = dr[PK].ToString();
                        FullRowSelects.Add(ObjfullSelect);
                    }
                }
                if (EditButton.Trim() != "" || DeleteButton.Trim() != "")
                {
                    string CtrlButton = "";
                    string Criterias = "";
                    DtResult.Columns.Add("Ctrl");
                    DtResult.Columns["Ctrl"].MaxLength = 2000;
                    foreach (DataRow dr in DtResult.Rows)
                    {
                        Criterias = PK + ":" + dr[PK].ToString().Trim() + "|";
                        CtrlButton = "";
                        if (EditButton.Trim() != "")
                        {
                            CtrlButton += "<image onClick=\"EditGrid('" + Ctrl + "','" + Criterias + "','" + Panel + "','" + WPanel + "','" + HPanel + "');\" id='GvEdit" + Ctrl + '_' + dr[PK].ToString() + "' src='../..//Pictures/Grid/Edit.png'/>";
                        }
                        if (DeleteButton.Trim() != "")
                        {

                            CtrlButton += "&nbsp;<image onClick=\"DeleteGrid('" + Ctrl + "','" + Criterias + "','" + Panel + "');\" id='GvDel" + Ctrl + '_' + dr[PK].ToString() + "' src='../..//Pictures/Grid/Del.png'/>";
                        }
                        dr["Ctrl"] = CtrlButton;
                        //dr["Ctrl"] = "<div id='DivCtrlPanel_" + Ctrl + "'>" + CtrlButton + "</div>";
                    }
                }
            }
            Boolean found = false;
            string Unuseval = "";
            System.Collections.ArrayList ArrUnuse = new System.Collections.ArrayList();
            foreach (DataColumn dc in DtResult.Columns)
            {
                Unuseval = "";
                foreach (string col in Data.Split(','))
                {
                    if (col.Split('.').Length > 1)
                    {
                        Unuseval = col.Split('.')[1];
                    }
                    else
                    {
                        Unuseval = col;
                    }
                    if (Unuseval.ToLower() == dc.ColumnName.ToLower())
                    {
                        found = true;
                        break;
                    }
                    if (dc.ColumnName.ToLower() == "ctrl")
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ArrUnuse.Add(dc.ColumnName);
                }
                found = false;
            }
            foreach (string col in ArrUnuse)
            {
                DtResult.Columns.Remove(col);
            }
            DtResult.AcceptChanges();

            //Ordinal
            int o = 0;
            foreach (string col in Data.Split(','))
            {
                try
                {
                    DtResult.Columns[col].SetOrdinal(o);
                }
                catch
                {

                }
                o++;
            }
            DtResult.AcceptChanges();
            //Ordinal
            Cn.Close();
            return DtResult;
        }
        public string GetCriteriaValue()
        {
            string Result = "";
            if (HttpContext.Current.Session["CriValue"] != null)
            {
                Result = HttpContext.Current.Session["CriValue"].ToString();
                HttpContext.Current.Session.Remove("CriValue");
                return Result;
            }
            else
            {
                return "";
            }
        }
        public void ClearResource(string Ctrl)
        {
            HttpContext.Current.Session.Remove("Select_" + Ctrl);
            HttpContext.Current.Session.Remove("Raw_" + Ctrl);
            HttpContext.Current.Session.Remove("Res_" + Ctrl);
            try
            {
                HttpContext.Current.Session.Remove("PK");
            }
            catch
            {

            }
        }
        public void Selected(string Ctrl, string ProjectId) //Finished
        {
            if (((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]) == null)
            {
                ArrayList ArrSelect = new ArrayList();
                HttpContext.Current.Session["Select_" + Ctrl] = ArrSelect;
            }
            ((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]).Add(ProjectId);
        }
        public ClsDictExtend DatSelect(string Ctrl, string PK, string SelName)
        {
            string res = "";
            string SelectName = "";
            int count = 0;
            ClsDictExtend ObjRes = new ClsDictExtend();
            //try
            //{
            if (((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]) != null)
            {
                foreach (string Sel in ((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]))
                {
                    if (count != 0)
                    {
                        res += "," + Sel;
                        try
                        {
                            SelectName += "," + ((DataTable)HttpContext.Current.Session["RAW_" + Ctrl]).Select(PK + " = '" + Sel + "'")[0][SelName].ToString();
                        }
                        catch
                        {
                            SelectName += "," + Sel;
                        }
                    }
                    else
                    {
                        res += Sel;
                        try
                        {
                            SelectName += ((DataTable)HttpContext.Current.Session["RAW_" + Ctrl]).Select(PK + " = '" + Sel + "'")[0][SelName].ToString();
                        }
                        catch
                        {
                            SelectName += "," + Sel;
                        }

                    }

                    count += 1;
                }
                try
                {
                    ObjRes.Name = HttpContext.Current.Session["Opener"].ToString();
                    HttpContext.Current.Session.Remove("Opener");
                }
                catch
                {

                }
                ObjRes.Val = res;
                ObjRes.Extend1 = SelectName;
            }
            else
            {
                if (HttpContext.Current.Session["Opener"] != null)
                {
                    ObjRes.Name = HttpContext.Current.Session["Opener"].ToString();
                    ObjRes.Val = "";
                    ObjRes.Extend1 = "";
                }
            }
            //}
            //catch
            //{

            //}
            return ObjRes;
        }
        public void UnSelected(string Ctrl, string ProjectId) //Finished
        {
            try
            {
                ((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]).Remove(ProjectId);
            }
            catch
            {

            }

        }
        public void SelectAll(string Ctrl, string PK)
        {
            DataTable Dt = new DataTable();

            Dt = (DataTable)HttpContext.Current.Session["RAW_" + Ctrl];
            if (((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]) == null)
            {
                ArrayList ArrSelect = new ArrayList();
                HttpContext.Current.Session["Select_" + Ctrl] = ArrSelect;
            }
            foreach (DataRow dr in Dt.Rows)
            {
                ((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]).Add(dr[PK].ToString());
            }
        }
        public string GetSummary(string dat) // #SUM:ผลรวมของใบแจ้งหนี้ {x} บาท@Amount Summary Parameter Data
        {
            string[] Arr = dat.Replace("#SUM:", "").Split('@');
            double Sum = 0.00;
            DataTable Dt = new DataTable();
            Dt = (DataTable)HttpContext.Current.Session["RAW_" + Arr[2]];
            foreach (DataRow dr in Dt.Rows)
            {
                Sum += double.Parse(dr[Arr[1]].ToString());
            }
            return Arr[0].Replace("{x}", Sum.ToString("N2")).ToString();
        }
        public string UnSelectAll(string Ctrl)
        {
            if (((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]) != null)
            {
                ((ArrayList)HttpContext.Current.Session["Select_" + Ctrl]).Clear();
            }
            return "";
        }
        public ClsGridResponse Bind(ref SqlConnector Cn, string Ctrl, long PagePerItem, long CurrentPage, string SortName, string Order, string Column, string Data, string Initial, string SelectCat, string SearchMsg, string EditButton, string DeleteButton, string Panel, string FullRowSelect, string Multiselect, string Criteria, string PK, string Sqlcommand, List<ClsDict> CriteriaMapping, string SearchesDat, string Searchcolumns, string WPanel, string HPanel)
        {
            List<List<ClsDict>> Gvrs = new List<List<ClsDict>>();
            ClsGridResponse ObjRes = new ClsGridResponse();
            List<ClsDict> Gvr = new List<ClsDict>();
            List<ClsDict> FullRowSelects = new List<ClsDict>();
            DataTable DtRawData = new DataTable();
            ClsDict Obj = new ClsDict();
            DataTable Dt = new DataTable();
            long i = 0;
            long Start = 1;
            long End = 1;
            HttpContext.Current.Session[Ctrl] = GetData(ref Cn,ref FullRowSelects, Ctrl, PagePerItem, CurrentPage, SortName, Order, SelectCat, SearchMsg, Data, EditButton, DeleteButton, Panel, FullRowSelect, Multiselect, Criteria, PK, Sqlcommand, CriteriaMapping, SearchesDat, Searchcolumns, WPanel, HPanel);
            Dt = (DataTable)HttpContext.Current.Session[Ctrl];
            if (Dt.Rows.Count != 0)
            {
        
                Start = 1;
                End = Dt.Rows.Count;
                for (i = Start - 1; i < End; i++)
                {
                    Gvr = new List<ClsDict>();
                    foreach (DataColumn dc in Dt.Columns)
                    {
                        Obj = new ClsDict();
                        Obj.Name = dc.ColumnName;
                        Obj.Val = Dt.Rows[int.Parse(i.ToString())][Obj.Name.ToString()].ToString();
                        Gvr.Add(Obj);
                    }
                    Gvrs.Add(Gvr);
                    Gvr = null;
                    Obj = null;
                }
            }
            GC.Collect();
            ObjRes.Ctrl = Ctrl;
            ObjRes.Column = Column;
            ObjRes.CurrentPage = CurrentPage.ToString();
            ObjRes.Order = Order;
            ObjRes.PagePerItem = PagePerItem.ToString();
            ObjRes.SortName = SortName;
            //ObjRes.RawData = (DataTable)HttpContext.Current.Session["RAW_" + Ctrl];
            ObjRes.ResData = Gvrs;
            ObjRes.Initial = Initial;
            ObjRes.EditButton = EditButton;
            ObjRes.DeleteButton = DeleteButton;
            ObjRes.FullRowSelectEvent = FullRowSelects;
            ObjRes.FullRowSelect = FullRowSelect;
            ObjRes.Criteria = Criteria;
            ObjRes.SearchesDat = SearchesDat;
            ObjRes.Searchcolumns = Searchcolumns;
            ObjRes.WPanel = WPanel;
            ObjRes.HPanel = HPanel;
            ObjRes.Panel = Panel;
            ObjRes.Data = Data;
            ObjRes.PKValues = (ArrayList)HttpContext.Current.Session["PK"];
            try
            {
                HttpContext.Current.Session.Remove("PK");
            }
            catch
            {

            }
            if (HttpContext.Current.Session["Select_" + Ctrl] != null)
            {
                ObjRes.Selection = (ArrayList)HttpContext.Current.Session["Select_" + Ctrl];
            }
            else
            {
                ObjRes.Selection = null;
            }
            ObjRes.Multiselect = Multiselect;
            HttpContext.Current.Session["Res_" + Ctrl] = ObjRes;
            return ObjRes;
        }
        public string Sort(string Ctrl, string ColName)
        {
            const string DefaultOrderBy = "ASC";
            string result = "";
            string CurrentSort = "";
            string CurOrederBy = "";
            CurrentSort = ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).SortName;
            CurOrederBy = ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).Order;
            if (ColName == CurrentSort)
            {
                if (CurOrederBy == DefaultOrderBy)
                {
                    CurOrederBy = "DESC";
                }
                else
                {
                    CurOrederBy = DefaultOrderBy; //ASC
                }
            }
            else
            {
                CurrentSort = ColName;
                CurOrederBy = DefaultOrderBy; //ASC
            }
            ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).SortName = CurrentSort;
            ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).Order = CurOrederBy;
            return result;
        }
        public ClsGridResponse GetResource(string Ctrl)
        {
            //if (HttpContext.Current.Session["Select_" + Ctrl] != null)
            //{
            //    ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).Selection = (ArrayList)HttpContext.Current.Session["Select_" + Ctrl];
            //}
            return (ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl];
        }
        public string UpdCurrentPage(string Ctrl, string CurrentPage)
        {
            ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).CurrentPage = CurrentPage;
            return "";
        }
        public string UpdInitial(string Ctrl, string Initial)
        {
            try
            {
                ((ClsGridResponse)HttpContext.Current.Session["Res_" + Ctrl]).Initial = Initial;
            }
            catch
            {

            }
            return "";
        }
        public string Export(string Ctrl)
        {
            string Filename = Guid.NewGuid().ToString();
            string Path = System.Configuration.ConfigurationManager.AppSettings["Initial_Templated"].ToString();
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite);
            Path = System.Configuration.ConfigurationManager.AppSettings["Initial_OutputFolder"].ToString() + @"\" + Filename + ".xls";
            FileStream fsout = new FileStream(Path, FileMode.Create);
            HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);
            HSSFSheet sheet = (HSSFSheet)templateWorkbook.GetSheet("Sheet1");
            HSSFRow row1 = null;
            HSSFCell dataCell;
            int r = 0;
            int c = 0;
            DataTable Dt = new DataTable();
            Dt = ((DataTable)HttpContext.Current.Session[Ctrl]);
            row1 = (HSSFRow)sheet.CreateRow(r);
            foreach (DataColumn DC in Dt.Columns)
            {
                dataCell = (HSSFCell)row1.CreateCell(c);
                dataCell.SetCellValue(DC.ColumnName);
                dataCell.CellStyle.IsLocked = true;
                c++;
            }
            r++;
            c = 0;
            foreach (DataRow e_dr in Dt.Rows)
            {
                row1 = (HSSFRow)sheet.CreateRow(r);
                foreach (DataColumn DC in Dt.Columns)
                {
                    dataCell = (HSSFCell)row1.CreateCell(c);
                    dataCell.SetCellValue(e_dr[DC.ColumnName].ToString().Trim());
                    dataCell.CellStyle.IsLocked = true;
                    c++;
                }
                c = 0;
                r++;
            }
            sheet.ForceFormulaRecalculation = true;
            templateWorkbook.Write(fsout);
            fsout.Flush();
            fsout.Close();
            fsout.Dispose();
            return System.Configuration.ConfigurationManager.AppSettings["Initial_OutputUrl"].ToString() + @"/" + Filename + ".xls";
        }
    }
}