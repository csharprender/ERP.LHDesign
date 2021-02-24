
var G_Ctrl = '';
var G_PK = '';
var G_Mode = '';
var This = document.location.pathname.match(/[^\/]+$/)[0];
function EnterSearch(event,x)
{
    if (event.keyCode == 13) {
        event.preventDefault();
        Search(x);
        return false;
     }
}
function NewGrid(ctrl,Panel,WPanel,HWPanel)
{
    //G_Ctrl = ctrl;
    //G_Mode = 'A'; 
    //ClearCtrl(Panel);
    //$('#' + Panel).dialog('option', 'title', 'บันทึกข้อมูลใหม่');
    //if (WPanel != '')
    //{
    //    $('#' + Panel).dialog("option", "width", WPanel);
    //}
    //if (HWPanel != '')
    //{
    //    $('#' + Panel).dialog("option", "height", HWPanel);
    //}
    //$('#' + Panel).dialog("open");
    try 
    {
        Custom(ctrl,Panel);
    }
    catch(ex)
    {

    }
}
//function EditGrid(ctrl,dat,div,WPanel,HPanel)
//{
//    G_Mode = "E";
//    G_Ctrl = ctrl;

//    if (div != '')
//    {
//        $('#' + div).dialog('option', 'title', 'แก้ไขข้อมูล');
//        if (WPanel != '')
//        {
//            $('#' + div).dialog("option", "width", WPanel);
//        }
//        if (WPanel != '')
//        {
//            $('#' + div).dialog("option", "height", HPanel);
//        }

//        $('#' + div).dialog("open");
//        $.ajax({
//            type: "POST",
//            url: This + "/Load2GridPanel",
//            data: "{'Ctrl' : '" +  ctrl + "','dat' : '" + dat + "'}",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            beforesend: function () {
//                setTimeout($('#divProgress').dialog("open"), 3000);
//            },
//            success: function (response) {
//                var i=0;
//                var res;
//                res = response.d;

//                if (res !=null)
//                {
//                    for(i=0;i<res.length;i++)
//                    {
//                        G_PK = dat;
//                        $("#" + div).find('input:text,input:hidden, input:password, input:file, select, textarea')
//                       .each(function() {
//                           try 
//                           {
//                               var datcolumn = '';
//                               datcolumn = $(this).attr('data-column').toLowerCase();
       
//                               if (datcolumn.toLowerCase() == "province")
//                               {
//                                   datcolumn = 'ProvinceNameTH';  
                                  
//                               }
//                               else if (datcolumn.toLowerCase() == "district")
//                               {
//                                   datcolumn = 'DistrictNameTH';   
                                  
//                               }
//                               else if (datcolumn.toLowerCase() == "subdistrict")
//                               {
//                                   datcolumn = 'SubDistrictNameTH';   
//                               }
//                               if (datcolumn.toLowerCase() == res[i].Name.toLowerCase())
//                               {
//                                   $(this).val(res[i].Val);
                                   
//                               }
//                           }
//                           catch(ex)
//                           {

//                           }
//                       });
//                        $("#" + div).find('select')
//                        .each(function() {
//                            try 
//                            {
//                                if ($(this).attr('data-column').toLowerCase() == res[i].Name.toLowerCase())
//                                {

//                                    $("#" + $(this).attr('id') + " option:contains(" + res[i].Val + ")").attr('selected', 'selected');
//                                }
//                            }
//                            catch(ex)
//                            {

//                            }
//                        });

                       
//                        $("#" + div).find('input:radio, input:checkbox').each(function() {
//                            var x;
//                            try 
//                            {
//                                if ($(this).attr('data-column').toLowerCase() == res[i].Name.toLowerCase())
//                                {

//                                    if (res[i].Val =='1' || res[i].Val == 'True')
//                                    {
//                                        x=true;
//                                    }
//                                    else
//                                    {
//                                        x=false;
//                                    }
           
//                                    $(this).prop('checked',x);
//                                }
                       
//                            }
//                            catch(ex)
//                            {
                                
//                            }
//                        });
//                        $("#" + div).find('textarea').each(function() {
//                            try 
//                            {
//                                if ($(this).attr('data-column').toLowerCase() == res[i].Name.toLowerCase())
//                                {
//                                    $(this).val(res[i].Val);
//                                }
                       
//                            }
//                            catch(ex)
//                            {

//                            }
//                        });
//                    }
//                }
//                try 
//                {
                    
//                    CustomEdit(ctrl,dat,div);
//                }
//                catch(ex)
//                {

//                }
//            },
//            async: false,
//            error: function (er) {
//                try {
//                    var x = $.parseJSON(er.responseText);
//                    show_msg(x.Message);
//                }
//                catch (ex) {
//                    console.log(ex.responseText);
//                }
//            }
//        });
        
//    }
//    else
//    {

//        try 
//        {
//            CustomEdit(ctrl,dat,div);
//        }
//        catch(ex)
//        {
          
//        }
//    }
//}
//function DeleteGrid(ctrl, dat,div) {
//    try 
//    {
//        G_Ctrl =  ctrl;
//        G_PK =  dat;
//        G_Mode = 'D';
//        $('#DivConfirmGrid').dialog("open");
//        try
//        {
//            CustomDelete(ctrl, dat,div);
//        }
//        catch(ex)
//        {

//        }
       
//    }
//    catch(ex)
//    {
//        G_Ctrl =  ctrl;
//        G_PK =  dat;
//        G_Mode = 'D';
//        $('#DivConfirmGrid').dialog("open");
//        try
//        {
//            CustomDelete(ctrl, dat,div);
//        }
//        catch(ex)
//        {

//        }
//    }
   
   
//}
function UnselectAllByClick(x)
{
    $.ajax({
        type: "POST",
        url: This + "/UnSelectAll",
        data: "{'Ctrl' :'" + x + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#Chk' + x + '_SelectAll').prop('checked',false);
            Rebind(x);
        },
        async: false,
        error: function (er) {
          
        }
    });
}
function UnselectAll(x)
{
    $.ajax({
        type: "POST",
        url: This + "/UnSelectAll",
        data: "{'Ctrl' :'" + x + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            return;     
        },
        async: false,
        error: function (er) {
       
        }
    });
}
function Search(x)
{
    
    var _Ctrl;
    var result;
    var i,j,p;
    var val;
    var SelectCat = '';
    var SearchMsg = '';
    try 
    {
        SelectCat = $('#SelectCat' + x).val();
        SearchMsg = $('#TxtSearch' + x).val();
        var results = GetResource(x);
        this.gridname = results.Ctrl;
        this.PagePerItem = results.PagePerItem;
        this.CurrentPage = "1";
        this.columns = results.Column.split(',');
        this.Data = results.Data.split(',');
        this.SortName = this.Data[0];
        this.Order = results.Order;//'desc';
        this.Initial = true;
        this.EditButton = results.EditButton;
        this.DeleteButton = results.DeleteButton; 
        this.Panel =  results.Panel; 
        this.FullRowSelect = results.FullRowSelect;
        this.Multiselect = results.Multiselect;
        this.Criteria =  results.Criteria;
        this.SearchesDat =  results.SearchesDat;
        this.Searchcolumns =  results.Searchcolumns;
        this.WPanel = results.WPanel;
        this.HPanel = results.HPanel;
        this.Panel = results.Panel;
    }
    catch(ex)
    {

    }
    var strhtml = '';
    var PagePerItem = this.PagePerItem;
    var ColSpan = this.columns.length;
    var TotalRecord = 0;
    if (this.EditButton != '' || this.DeleteButton !='')
    {
        ColSpan+=1;
    }

    $.ajax({
        type: "POST",
        url: This + "/Bind",
        data: "{'Ctrl' :'" + this.gridname + "','PagePerItem' : '" + this.PagePerItem + "','CurrentPage' : '" + this.CurrentPage + "','SortName' : '" + this.SortName + "','Order' : '" + this.Order + "','Column' : '" + this.columns.join() + "','Data' : '" + this.Data.join() + "','Initial' :'" + this.Initial + "','SelectCat' :'" + SelectCat + "','SearchMsg' :'" + SearchMsg + "','EditButton' :'" + this.EditButton + "','DeleteButton' :'" + DeleteButton + "','Panel' :'" + this.Panel + "','FullRowSelect' :'" + this.FullRowSelect + "','Multiselect' :'" + this.Multiselect +  "','Criteria' : '" + this.Criteria +  "','SearchesDat' : '" + this.SearchesDat +  "','Searchcolumns' : '" + this.Searchcolumns + "','WPanel' :'" + this.WPanel  + "','HPanel' :'" + this.HPanel + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            var bfhtml;
            bfhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'><img src='../../Img/ajax-loader.gif' alt='loading...' /></div></td></tr>";
            $('#' + this.gridname).children('tbody').html(bfhtml);
        },
        success: function (response) {
            result = response.d;
            _Ctrl = result.Ctrl;
            this.gridname = result.Ctrl;
            this.PagePerItem = result.PagePerItem;
            this.CurrentPage = result.CurrentPage;
            this.SortName = result.SortName;
            this.Order = result.Order;
            this.Column = result.Column.split(',');
            this.Data = result.Data.split(',');
            this.Initial =  result.Initial;
            this.EditButton = result.EditButton;
            this.DeleteButton = result.DeleteButton; 
            this.Panel = result.Panel; 
            this.FullRowSelect = result.FullRowSelect;
            this.Multiselect = result.Multiselect;
            this.Criteria =  result.Criteria;
            this.SearchesDat =  result.SearchesDat;
            this.Searchcolumns =  result.Searchcolumns;
            this.WPanel = result.WPanel;
            this.HPanel = result.HPanel;
            this.Panel = result.Panel;
            $('#' + this.gridname).children('tbody').html('');
            if (result.ResData.length > 0) {
                for (i = 0; i < result.ResData.length; i++) {
                    val = result.ResData[i]; 
                        
                    if (this.FullRowSelect !='')
                    {
                        strhtml += "<tr id='GR" + _Ctrl + "_" + result.FullRowSelectEvent[i].Val + "'  Onclick=\"RowSelect('" + _Ctrl  + "','" + result.FullRowSelectEvent[i].Val +  "');\" style='Cursor:pointer'>";
                    }
                    else
                    {
                        strhtml += "<tr>";
                    }
                    if(this.Multiselect != "")
                    {
                        var curitem = 0;
                        if (Number(this.CurrentPage) == 1)
                        {
                            curitem  = i;
                        }
                        else
                        {
                            curitem = ((Number(this.PagePerItem) * Number(this.CurrentPage-1)) + i);
                        }
                   
                        if ($.inArray(result.FullRowSelectEvent[curitem].Val, result.Selection) != -1)
                        {
       
                            strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Checked Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                        }
                        else 
                        {
                            strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                        }
                        ColSpan +=1;
                    }
                    for (j = 0; j < val.length; j++) {
                        try 
                        {
                            if (this.Column[j].split('!').length == 1)
                            {
                                strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                            }
                            else
                            {
                                if (this.Column[j].split('!')[1] == "H")
                                {
  
                                }
                                else if (this.Column[j].split('!')[1] == "R")
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:Right;'>" + val[j].Val + "</td>";
                                }
                                else if (this.Column[j].split('!')[1] == "C")
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:Center;'>" + val[j].Val + "</td>";
                                }
                                else
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                                }
                            }
                        }
                        catch(ex)
                        {
                            strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                        }
                    }
                    strhtml += "</tr>";
                }
            }
            else {
                if(this.Multiselect != "")
                {
                    ColSpan +=1;
                }
                strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>ไม่พบข้อมูล</div></td></tr>";
            }
            $.ajax({
                type: "POST",
                url: This + "/GetTotalRecord",
                data: "{'Ctrl' :'" + this.gridname + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    TotalRecord = response.d;
                },
                async: false,
                error: function (er) {
                    try {
                        var x = $.parseJSON(er.responseText);
                        show_msg(x.Message);
                    }
                    catch (ex) {
                        alert(ex.responseText);
                    }
                }
            });
            $('#' + this.gridname).children('tbody').html(strhtml);
            if(this.Multiselect != "")
            {
                try{
                    var res_selected;
                    var i_selected;
                    $.ajax({
                        type: "POST",
                        url: This + "/GetResource",
                        data: "{'Ctrl' :'" + this.gridname + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            res_selected = response.d.Selection;
                            for(i_selected =0;i_selected < res_selected.length;i_selected++)
                            {            
                                $('#Chk' + _Ctrl + '_' + res_selected[i_selected]).prop('checked',true);
                            }
           
                       
                        },
                        async: false,
                        error: function (er) {
                            try {
                                var x = $.parseJSON(er.responseText);
                                show_msg(x.Message);
                            }
                            catch (ex) {
                                alert(ex.responseText);
                            }
                        }
                    });
                }
                catch(ex)
                {

                }
            }
 
            var SumRes = '';
            if (this.Criteria.indexOf('#SUM') > -1 )
            {
                var x = 0;
                    
                var Arr = this.Criteria.toString().replace('!','').split('|');
                for(x=0;x<Arr.length;x++)
                {
                    if (Arr[x].toString().indexOf('#SUM') > -1)
                    {
                        $.ajax({
                            type: "POST",
                            url: This + "/GetSummary",
                            data: "{'dat':'" +  Arr[x] + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                SumRes = response.d;
                                SumRes = '<span style="font-weight:0.1;color:blue;font-size:11px;">' + SumRes + '</span>';
                                    
                            },
                            async: false,
                            error: function (er) {
                                try {
                                    var x = $.parseJSON(er.responseText);
                                    show_msg(x.Message);
                                }
                                catch (ex) {
                                    alert(ex.responseText);
                                }
                            }
                        });
                          
                    }
                }
                  
            }
            else
            {
                SumRes = '';
            }
            strhtml = "<tr style='background-color:#1466a4;border-radius:4px;color:white;'>";
            strhtml += "<td id=\"DivFooter" + this.gridname + "\" colspan=\"" + ColSpan + "\">";
            strhtml += "<div id=\"mdiv" + this.gridname + "\" class=\"container-fluid\">";
            strhtml += "<div class=\"row\"  >";
            strhtml += "<div class=\"col-9\" style='text-align:right;'>";
            strhtml += "<button class='btn btn-info' style='background-color:#1466a4;'  title=\"First page\" id=\"btnFirstSpan" + this.gridname + "\"><i class=\"fa fa-fast-backward\" aria-hidden=\"true\"></i></button><button class='btn btn-info'  title=\"Previous page\" id=\"btnPrevSpan" + this.gridname + "\"><i class=\"fa fa-backward\" aria-hidden=\"true\"></i></button><span id=\"pgbeforespan" + this.gridname + "\" class=\"nbpg\">Page&nbsp;<select  id=\"slcPages" + this.gridname + "\" class=\"pgSlc\" style=\"font-weight: bold;\">";
            strhtml += "</select>";
            strhtml += " &nbsp;of&nbsp;<span id=\"pgspan" + this.gridname + "\">1</span>&nbsp;</span><button class='btn btn-info'  title=\"Next page\" id=\"btnNextSpan" + this.gridname + "\"><i class=\"fa fa-forward\" aria-hidden=\"true\"></i></button><button class='btn btn-info'  title=\"Last page\" id=\"btnLastSpan" + this.gridname + "\"><i class=\"fa fa-fast-forward\" aria-hidden=\"true\"></i></button>";
            strhtml += "</div>";

            strhtml += "<div class=\"col-3\" style='background-color:#363b41;border-radius:4px;padding:10px;'>";
            strhtml += "<div class=\"pgItem\"><span id=\"pgItem" + this.gridname + "\"></span>&nbsp;<span id=\"pgfooter" + this.gridname + "\"></span></div>";
            strhtml += "<div class=\"clearfloat\"></div>";
            strhtml += "</div>";
            strhtml += "</div>";
            strhtml += " </td>";
            strhtml += "</tr>";
            $('#' + this.gridname).children('tfoot').html(strhtml);
            $('#pgItem' + this.gridname).html('Total Record : ' + TotalRecord + '&nbsp;<br>' + SumRes);
            var TotalPage = Math.ceil(TotalRecord / PagePerItem);
            if (TotalPage == 0) TotalPage = 1;
            $('#slcPages' + this.gridname).empty();
                
            for (p = 1; p <= TotalPage; p++) {
                $('#slcPages' + this.gridname).append($('<option/>', {
                    value: p,
                    text: p
                }));
            }
            $("#slcPages" + _Ctrl).val(this.CurrentPage);
            $('#pgspan' + _Ctrl).html(TotalPage);
            $("#slcPages" + _Ctrl).change(function () {
                this.CurrentPage = $("#slcPages" + _Ctrl).val();
                UpdCurrentPage(_Ctrl, this.CurrentPage);
                Rebind(_Ctrl);
            });
            $('#btnFirstSpan' + _Ctrl).click(
                function () {
                    this.CurrentPage = 1;
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnPrevSpan' + this.gridname).click(
                function () {
                    this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                    this.CurrentPage -= 1;
                    if (this.CurrentPage <= 0) {
                        this.CurrentPage = 1;
                    }
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnNextSpan' + _Ctrl).click(
                function () {
                    this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                        
                    this.CurrentPage += 1;
                    if (this.CurrentPage >= TotalPage) {
                        this.CurrentPage = TotalPage;
                    }
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnLastSpan' + this.gridname).click(
                function () {
                    this.CurrentPage = TotalPage;
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
               
            if (this.Initial) {
                for (i = 0; i < this.Data.length; i++) {
                    $('#DivSort' + this.gridname + '_' + this.Data[i]).click(
                       function () {
                           var ctrl = $(this).attr('id').split('_')[0].replace('DivSort', '');
                           var colname = $(this).attr('id').split('_')[1];
                           _Sort(ctrl, colname);
                       }
                   );
                }
                this.Initial = false;
                UpdInitial(this.gridname,this.Initial);
            }
        },
        async: true,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                //var ErrMsg = 'ไม่สามารถดึงข้อมูลได้';
                //$("#dialog").dialog({
                //    modal: true,
                //    buttons: {
                //        Ok: function () {
                //            $(this).dialog("close");
                //        }
                //    }
                //});
                //$("#dialogmsg").html('<center>' + ErrMsg + '</center>');
                //strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>&nbsp;</div></td></tr>";
                //$('#' + this.gridname).children('tbody').html(strhtml);
            }
        }
    });
}
function ClearCtrl(div)
{
    $("#" + div).find('input:text, input:password, input:file, select, textarea')
    .each(function() {
        $(this).val('');
    });
    $("#" + div).find('input:radio, input:checkbox').each(function() {
        $(this).prop('checked',false);
    });
}
function UpdCurrentPage(Ctrl,CurrentPage)
{
    var res;
    $.ajax({
        type: "POST",
        url: This + "/UpdCurrentPage",
        data: "{'Ctrl' :'" + Ctrl + "','CurrentPage' :'" + CurrentPage + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            res = response.d;

        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
    return res;
}
function UpdInitial(Ctrl,Initial)
{
    var res;
    $.ajax({
        type: "POST",
        url: This + "/UpdInitial",
        data: "{'Ctrl' :'" + Ctrl + "','Initial' :'" + Initial + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            res = response.d;

        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
    return res;
}
function GetResource(Ctrl) {
    var res;
    $.ajax({
        type: "POST",
        url: This + "/GetResource",
        data: "{'Ctrl' :'" + Ctrl + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            res = response.d;
           
        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
    return res;
}
function ClearResource(Ctrl)
{
    $.ajax({
        type: "POST",
        url: This + "/ClearResource",
        data: "{'Ctrl' : '" + Ctrl + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        
        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
}
function DatSelect(Ctrl,PK,SelName)
{
    $.ajax({
        type: "POST",   
        url: This + "/DatSelect",
        data: "{'Ctrl' : '" + Ctrl + "','PK' : '" +  PK + "','SelName' : '" +  SelName +  "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            Callback(Ctrl,response.d.Name,response.d.Val,response.d.Extend1);
            window.close();
        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
          
            }
            catch (ex) {
           
            }
        }
    });
}
function Save(Panel)
{
    var dat = "";

    var flg = 0;
    $("#" + Panel).find('input:text,input:hidden, input:password, input:file')
    .each(function() {
        dat += $(this).attr('data-column') + ':' + $(this).val() + "|"; 
        
        if($(this).attr('data-value') != undefined)
        {
            dat += $(this).attr('data-column') + '_value' + ':' + $(this).attr('data-value') + "|";
        }
        dat += "|";
    });
    $("#" + Panel).find('textarea')
   .each(function() {
       if ($(this).html().trim().toLowerCase() == 'ckeditor')
       {
           //var editorText = CKEDITOR.instances.TxtContent.getData();
           dat += $(this).attr('data-column') + ':' + CKEDITOR.instances.TxtContent.getData() + "|";
       }
       else
       {
           dat += $(this).attr('data-column') + ':' + $(this).val() + "|";
       }
   });


    $("#" + Panel).find('img')
  .each(function() {
     
      dat += $(this).attr('data-column') + ':' + $(this).attr('data-value') + "|";
      
  });
    $("#" + Panel).find('select')
   .each(function() {
       dat += $(this).attr('data-column') + ':' + $(this).val() + "|";
       dat += $(this).attr('data-column') + '_ext:' + $('#' + $(this).prop('id') + ' option:selected').text() + '|';
   });
    $("#" + Panel).find('input:radio, input:checkbox').each(function() {
        if ($(this).prop('checked'))
        {
            flg = '1';
        }
        else
        {
            flg  ='0';
        }
        dat += $(this).attr('data-column') + ':' + flg + "|";
    });
    $.ajax({
        type: "POST",
        url: This + "/ExecuteGrid",
        data: "{'Ctrl' : '" +  G_Ctrl + "','Dat' :'" +  dat  + "','PK' :'" +  G_PK + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforesend: function () {
            setTimeout($('#divProgress').dialog("open"), 3000);
        },
        success: function (response) {
            Rebind(G_Ctrl);
            G_Mode = '';
            G_PK ='';
            G_Ctrl =  '';
            $('#divProgress').dialog("close");
        },
        async: true,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                console.log(ex.responseText);
            }
        }
    });
}
class Grid
{


    constructor(HeaderColumns,SearchesDat,Searchcolumns, gridname, PagePerItem,Width,Data,Label,Fx,Search,NewButton,EditButton,DeleteButton,Panel,WPanel,HPanel,SortFieldName,FullRowSelect,Multiselect,PK,Criteria,SelName)
    {

        if (SortFieldName == '')
        {
            this.SortName = Data[0];
            this.Order = "desc"; // not found
        }
        else 
        {
        
            this.SortName = SortFieldName;
            if (this.SortName.toString().indexOf('#') == -1)
            {
                this.Order = "desc"; // not found
            }
            else
            {
                //found
                this.Order = this.SortName.toString().split('#')[0];
                this.SortName = this.SortName.toString().split('#')[1];
               
            }
        }
      
        this.PK  = PK;
        this.Data = Data;
        this.CurrentPage = 1;
        this.gridname = gridname;
        this.Width = Width;
        this.columns = HeaderColumns;
        this.PagePerItem = PagePerItem;
        this.Fx = Fx;
        this.Search = Search;
        this.Label = Label;
        this.SearchCAT = '';
        this.NewButton = NewButton;
        this.EditButton = EditButton;
        this.DeleteButton = DeleteButton; 
        this.Panel  = Panel;
        this.FullRowSelect = FullRowSelect;
        this.Multiselect = Multiselect;
        this.SelName = SelName;
        Grid.prototype.pro = gridname;
        var _Ctrl = gridname;
        this.Initial = true;
        this.Criteria = Criteria;
        var _Columns = HeaderColumns;
        var _Data = Data;
        this.Searchcolumns = Searchcolumns;
        this.SearchesDat = SearchesDat;
        this.WPanel = WPanel;
        this.HPanel = HPanel;

        if (Panel != '')
        {
            $("#" + Panel).dialog({
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "บันทึก": function () {
                        var v = '';
                        try
                        {
                            BeforeSave(Panel);
                        }
                        catch(ex)
                        {

                        }
                        v = ValidateCtrl(Panel);

                        if (v == undefined )
                        {
                            v = '';
                        }
                        if (v != '') {
                            Msgbox('โปรดระบุ' + v);
                            return;
                        }


                        try
                        {
                            v=Validate(Panel,G_Mode,G_PK);
                        }
                        catch(ex)
                        {
                        }
                        if (v == 'undefined')
                        {
                            v = '';
                        }
                        if ($.trim(v) !='')
                        {
                            Msgbox(v);
                            return;
                        }
                        Save(Panel);
                        try
                        {
                            Saved(Panel,G_Mode,G_PK);
                        }
                        catch(ex)
                        {
                        }
                        $(this).dialog("close");
                    }
                    ,
                    "ยกเลิก": function () {
                        G_Mode = '';
                        G_PK ='';
                        G_Ctrl =  '';
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    $(this).dialog("close");
                }
            })
        }
        //$("#DivConfirmGrid").dialog({
        //    autoOpen: false,
        //    height: 180,
        //    width: 400,
        //    modal: true,
        //    buttons: {
        //        "ยืนยัน": function () {
        //            $.ajax({
        //                type: "POST",
        //                url: This + "/ExecuteDeleteGrid",
        //                data: "{'Ctrl' : '" +  G_Ctrl  + "','PK' : '" + G_PK + "'}",
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                beforesend: function () {
        //                    setTimeout($('#divProgress').dialog("open"), 3000);
        //                },
        //                success: function (response) {
        //                    Rebind(G_Ctrl);
        //                    G_Ctrl =  '';
        //                    G_PK='';
        //                    try 
        //                    {
        //                        PostDelete(G_Ctrl,G_PK);
        //                    }
        //                    catch(ex)
        //                    {
                                
        //                    }
        //                    $('#divProgress').dialog("close");
        //                },
        //                async: true,
        //                error: function (er) {
        //                    try {
        //                        var x = $.parseJSON(er.responseText);
        //                        show_msg(x.Message);
        //                    }
        //                    catch (ex) {
        //                        console.log(ex.responseText);
        //                    }
        //                }
        //            });
        //            $(this).dialog("close");
        //        }
        //        ,
        //        "ยกเลิก": function () {
        //            G_Mode = '';
        //            $(this).dialog("close");
        //        }
        //    },
        //    close: function () {
        //        $(this).dialog("close");
        //    }
        //})
    }
     _Tables() 
    {
        var span = 0;
        var html = '';
        var i = 0;
        if (this.Label != "")
        {
            html += "<div id='DivLabel' class='grid-label'>" + this.Label  +"</div>";
        }

         html += "<div class='container-fluid'>";
         html += "<div class='row'>";
         html += "<div class='col-12'>";
        html += "<table  id='DivCtrl" + this.gridname + "' style='width:100%;border:none;border-bottom:none;background-color:white;'>";
         html += "<tr>";

        if (this.NewButton != "")
        {   
            html += "<td id='TdNewButton_" + this.gridname + "' style='width:200px;text-align:left;'>";
            html += " <div  id='divNewButton" + this.gridname + "'><button class='btn btn-info'  style='background-color:#1466a4; font-size:1.0em;border-radius:1px;' id='lnknew_" + this.gridname + "' onClick=NewGrid('" + this.gridname + "','" + this.Panel + "','" + this.WPanel + "','" + this.HPanel + "'); ><span><i class='fa fa-plus' aria-hidden='true'></i></span>&nbsp;" + this.NewButton + "</button></div>";
            //html += " <div  id='divNewButton" + this.gridname + "'><img src='../../Pictures/Grid/Add.png' alt='add-logo' class='add-logo' />&nbsp;<a id='lnknew_" +  this.gridname + "' class='lnk-download' onClick=NewGrid('" + this.gridname + "','" + this.Panel + "','" + this.WPanel + "','" + this.HPanel + "');>" + this.NewButton + "</a></div>";
            html += "</td>";
        }
        if (this.Multiselect != "")
        {   
            html += "<td style='width:30%;text-align:left;padding-top:5px;' colspan='2'>";
            html += " <div  id='divSelectButton" + this.gridname + "'><a  id='lnkselect_" + this.gridname + "' class='lnk-download' onClick=\"DatSelect('" + this.gridname + "','" + this.PK + "','" + this.SelName + "');\">เลือกข้อมูล</a>&nbsp;&nbsp;<a  id='lnkUnselect_" + this.gridname + "' class='lnk-download' onClick=\"UnselectAllByClick('" + this.gridname +  "');\">ยกเลิกการเลือก</a></div>";
            html += "</td>";
        }
        if (this.Fx == "E")
        {   
            html += "<td style='text-align:left;'>";
            html += " <div  id='divExport" + this.gridname + "'><a  class='lnk-download' onClick=Export('" + this.gridname + "');>Export Excel</a></div><div id='divDownloadPanel" + this.gridname +"' style='display:none'><a id='lnkdownloadfile" + this.gridname + "' href='#' class='lnk-download'>Download file</a><img src='/../Pictures/Initial/Hide-Download/cancel.png' alt='hide download' id='CmdHideDownloadSection" + this.gridname + "' class='img-hide-download' /></div>";
            html += "</td>";
        }
        if (this.Search != "")
        {

            html += "<td style='width:50%;'>";
            html += "<div id='divSearch" + this.gridname + "' style='font-size:1.0em;'></div>";
            html += "</td>";
        }
         html += "</tr>";
         html += "</table>";
         html += "</div>";
         html += "</div>";
         html += "</div>";
         html += "<div class='container-fluid'> ";
         html += "<table border='1' class='table table-hover' cellpadding='0' cellspacing='0' border='0' style='width:100%' id='" + this.gridname + "'>";
         html += "<thead  style='background-color:#1466a4;border-radius:4px;'>";
        html += "<tr style='height:40px;'>";
        if (this.Multiselect != '')
        {
            html += "<th id='" + this.gridname + "_Header' style='cursor:pointer;margin:5px;text-decoration: none; width: 7%;'>";
            html += "<div class='sorting'></div>";
            html += "<div id='DivSort" + this.gridname + "_Header'><input type='checkbox' id='Chk" + this.gridname + "_SelectAll' Onclick=\"ChkSelectAll('Chk" + this.gridname + "_SelectAll','" + this.gridname + "','" + this.PK + "');\" /></div>";
            html += "<div class='sorting'></div>";
            html += "</th>";
        }
        for (i == 0; i < this.columns.length; i++) {
           
            var c = '';
            var command =  '';
            if (this.columns[i].split('!').length == 1)
            {
                c = this.columns[i];
                command = '';
            }
            else
            {
                c = this.columns[i].split('!')[0];
                command = this.columns[i].split('!')[1];
            }
            if (command != 'H')
            {
                html += "<th id='" + this.gridname + "_" + c + "' style='cursor:pointer;color:white;font-size:kanit;font-weight:100;margin:5px;text-decoration: underline; width: " + this.Width[i] + ";'>";
                html += "<div></div>";
                html += "<div id='DivSort" + this.gridname + "_" + this.Data[i] + "');>" + c + "</div>";
                html += "<div></div>";
                html += "</th>";
            }

        }
        if (this.EditButton != '' || this.DeleteButton !='')
        {
           
            html += "<th id='Ctrl" + this.gridname + "' style='cursor:pointer;margin:5px;text-decoration: underline; width: 80px;'>";
            html += "<div class='sorting'></div>";
            html += "<div id='DivSortCtrl" + this.gridname +  "'></div>";
            html += "<div class='sorting'></div>";
            html += "</th>";
        }
      
        html += "</tr>";
        html += "</thead>";
        html += "<tbody>";
        if (this.EditButton != '' || this.DeleteButton !='')
        {
            span =  this.columns.length+1;
        }
        else  if (this.Multiselect != '')
        {
            span =  this.columns.length +1;
        }
        else
        {
            span = this.columns.length;
        }
        html += "<tr><td colspan='" + span + "'><div style='height: 80px;margin-top: 40px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'><img src='../../pictures/ajax-loader.gif' alt='loading...' /></div></td></tr>";
        html += "</tbody>";
        html += "<tfoot>";
        html += "</tfoot>";
        html += "</table></div>";
        html += "<span id='" + this.gridname + "' style='display: none;'></span>";
        return html;
    }
   
    _Bind()
    {
        
        var _Ctrl;
        var result;
        var i,j,p;
        var val;
        try 
        {
            if (!this.Initial)
            {
              
                var results = GetResource(this.gridname);
                this.gridname = results.Ctrl;
                this.PagePerItem = results.PagePerItem;
                this.CurrentPage = results.CurrentPage;
                this.SortName = results.SortName;
                this.Order = results.Order;
                this.columns = results.Column.split(',');
                this.Data = results.Data.split(',');
                this.Initial = results.Order;
                this.EditButton = results.EditButton;
                this.DeleteButton = results.DeleteButton; 
                this.FullRowSelect = results.FullRowSelect;
                this.Multiselect = results.Multiselect;
                this.Criteria = results.Criteria;
                this.SearchesDat =  results.SearchesDat;
                this.Searchcolumns =  results.Searchcolumns;
                this.Panel = Panel;
                this.WPanel = results.WPanel;
                this.HPanel = results.HPanel;
            }
        }
        catch(ex)
        {

        }
        var ASync;
        if(this.Fx == 'S')
        {
            ASync = false;
        }
        else
        {
            ASync = true;
        }
        var strhtml = '';
        var PagePerItem = this.PagePerItem;
        var ColSpan = this.columns.length;
        var TotalRecord = 0;
        if (this.EditButton != '' || this.DeleteButton !='')
        {
            ColSpan+=1;
        }

        $.ajax({
            type: "POST",
            url: This + "/Bind",
            data: "{'Ctrl' :'" + this.gridname + "','PagePerItem' : '" + this.PagePerItem + "','CurrentPage' : '" + this.CurrentPage + "','SortName' : '" + this.SortName + "','Order' : '" + this.Order + "','Column' : '" + this.columns.join() + "','Data' : '" + this.Data.join() + "','Initial' :'" + this.Initial + "','SelectCat' :'','SearchMsg' :'','EditButton' :'" + this.EditButton + "','DeleteButton' :'" + this.DeleteButton + "','Panel' :'" + this.Panel + "','FullRowSelect' :'" + this.FullRowSelect + "','Multiselect' :'" + this.Multiselect  +  "','Criteria' : '" + this.Criteria +  "','SearchesDat' : '" + this.SearchesDat +  "','Searchcolumns' : '" + this.Searchcolumns + "','WPanel' :'" + this.WPanel  + "','HPanel' :'" + this.HPanel + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                var bfhtml;
                bfhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'><img src='../../pictures/ajax-loader.gif' alt='loading...' /></div></td></tr>";
                $('#' + this.gridname).children('tbody').html(bfhtml);
            },
            success: function (response) {
                result = response.d;
              
                _Ctrl = result.Ctrl;
                this.gridname = result.Ctrl;
                this.PagePerItem = result.PagePerItem;
                this.CurrentPage = result.CurrentPage;
                this.SortName = result.SortName;
                this.Order = result.Order;
                try 
                {
                    this.Column = result.Column.split(',');
                }
                catch(ex)
                {
                    this.Column = result.Column;
                }
                try 
                {
                    this.Data = result.Data.split(',');
                }
                catch(ex)
                {
                    this.Data = result.Data;
                }
                this.Initial =  result.Initial;
                this.EditButton = result.EditButton;
                this.DeleteButton = result.DeleteButton; 
                this.FullRowSelect = result.FullRowSelect;
                this.Multiselect = result.Multiselect;
                this.Criteria  = result.Criteria;
                this.SearchesDat =  result.SearchesDat;
                this.Searchcolumns =  result.Searchcolumns;
                this.Panel = result.Panel;
                this.WPanel = result.WPanel;
                this.HPanel = result.HPanel;
                $('#' + this.gridname).children('tbody').html('');
                
                if (result.ResData.length > 0) {
                    for (i = 0; i < result.ResData.length; i++) {
                        val = result.ResData[i]; 
                        
                        if (this.FullRowSelect !='')
                        {
                            try 
                            {
                                strhtml += "<tr id='GR" + _Ctrl + "_" + result.FullRowSelectEvent[i].Val + "'  Onclick=\"RowSelect('" + _Ctrl + "','" + result.FullRowSelectEvent[i].Val +  "');\" style='Cursor:pointer'>";
                            }
                            catch(ex)
                            {
                                strhtml += "<tr Onclick=\"RowSelect('#');\" style='Cursor:pointer'>";
                            }

                        }
                        else
                        {
                            strhtml += "<tr>";
                        }
                        if(this.Multiselect != "")
                        {
                            var curitem = 0;
                            if (Number(this.CurrentPage) == 1)
                            {
                                curitem  = i;
                            }
                            else
                            {
                                curitem = ((Number(this.PagePerItem) * Number(this.CurrentPage-1)) + i);
                            }
                            try 
                            {
                                if ($.inArray(result.FullRowSelectEvent[curitem].Val, result.Selection) != -1)
                                {
       
                                    strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Checked Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                                }
                                else 
                                {
                                    strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                                }
                            }
                            catch(ex)
                            {

                            }
                            ColSpan +=1;
                        }
                        for (j = 0; j < val.length; j++) {
                            try{
                                if (this.Column[j].split('!').length == 1)
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:left;' id='Item" +  val[j].Name  +"'>" + val[j].Val + "</td>";
                                }
                                else
                                {
                                    if (this.Column[j].split('!')[1] == "H")
                                    {

                                    }
                                    else if (this.Column[j].split('!')[1] == "R")
                                    {
                                        strhtml += "<td class=\"grid-item\" style='text-align:Right;' id='Item_" + i + "_" +  val[j].Name  +"'>" + val[j].Val + "</td>";
                                    }
                                    else if (this.Column[j].split('!')[1] == "C")
                                    {
                                        strhtml += "<td class=\"grid-item\" style='text-align:Center;' id='Item_" + i + "_" +  val[j].Name  +"'>" + val[j].Val + "</td>";
                                    }
                                    else
                                    {
                                        strhtml += "<td class=\"grid-item\" style='text-align:left;' id='Item_" + i + "_" +  val[j].Name  +"'>" + val[j].Val + "</td>";
                                    }
                                }
                            }
                            catch(ex)
                            {
                                strhtml += "<td class=\"grid-item\" style='text-align:left;' id='Item_" + i + "_" +  val[j].Name  +"'>" + val[j].Val + "</td>";
                            }


                            
                        }
                        strhtml += "</tr>";
                    }
                }
                else {
                    if(this.Multiselect != "")
                    {
 
                        ColSpan +=1;
                    }
                    strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>ไม่พบข้อมูล</div></td></tr>";
                }
              
                $.ajax({
                    type: "POST",
                    url: This + "/GetTotalRecord",
                    data: "{'Ctrl' :'" + this.gridname + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        TotalRecord = response.d;
                    },
                    async: false,
                    error: function (er) {
                        try {
                            var x = $.parseJSON(er.responseText);
                            show_msg(x.Message);
                        }
                        catch (ex) {
                            alert(ex.responseText);
                        }
                    }
                });
             
                $('#' + this.gridname).children('tbody').html(strhtml);
                if(this.Multiselect != "")
                {
                    try{
                        var res_selected;
                        var i_selected;
                        $.ajax({
                            type: "POST",
                            url: This + "/GetResource",
                            data: "{'Ctrl' :'" + this.gridname + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                res_selected = response.d.Selection;
                                for(i_selected =0;i_selected < res_selected.length;i_selected++)
                                {            
                                    $('#Chk' + _Ctrl + '_' + res_selected[i_selected]).prop('checked',true);
                                }
           
                       
                            },
                            async: false,
                            error: function (er) {
                                try {
                                    var x = $.parseJSON(er.responseText);
                                    show_msg(x.Message);
                                }
                                catch (ex) {
                                    alert(ex.responseText);
                                }
                            }
                        });
                    }
                    catch(ex)
                    {

                    }
                }

                strhtml = "<tr>";
                strhtml += "<td id=\"DivFooter" + this.gridname + "\" colspan=\"" + ColSpan + "\">";
                var SumRes = '';
                if (this.Criteria.indexOf('#SUM') > -1 )
                {
                    var x = 0;
                    
                    var Arr = this.Criteria.toString().replace('!','').split('|');
                    for(x=0;x<Arr.length;x++)
                    {
                        if (Arr[x].toString().indexOf('#SUM') > -1)
                        {
                            $.ajax({
                                type: "POST",
                                url: This + "/GetSummary",
                                data: "{'dat':'" +  Arr[x] + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    SumRes = response.d;
                                    SumRes = '<span style="font-weight:0.1;color:blue;font-size:11px;">' + SumRes + '</span>';
                                    
                                },
                                async: false,
                                error: function (er) {
                                    try {
                                        var x = $.parseJSON(er.responseText);
                                        show_msg(x.Message);
                                    }
                                    catch (ex) {
                                        alert(ex.responseText);
                                    }
                                }
                            });
                          
                        }
                    }
                  
                }
                else
                {
                    SumRes = '';
                }
                strhtml = "<tr style='background-color:#1466a4;border-radius:4px;color:white;'>";
                    strhtml += "<td id=\"DivFooter" + this.gridname + "\" colspan=\"" + ColSpan + "\">";
                        strhtml += "<div id=\"mdiv" + this.gridname + "\" class=\"container-fluid\">";
                strhtml += "<div class=\"row\"  >";
                                strhtml += "<div class=\"col-9\" style='text-align:right;'>";
                strhtml += "<button class='btn btn-info' style='background-color:darkgray;margin-right:5px;font-size:1.0em;' title=\"First page\" id=\"btnFirstSpan" + this.gridname + "\"><i class=\"fa fa-fast-backward\"  aria-hidden=\"true\"></i></button><button class='btn btn-info'  style='font-size:1.0em;background-color:darkgray;margin-right:5px;'  title=\"Previous page\" id=\"btnPrevSpan" + this.gridname + "\"><i class=\"fa fa-backward\" aria-hidden=\"true\"></i></button><span id=\"pgbeforespan" + this.gridname + "\" class=\"nbpg\">Page&nbsp;<select  id=\"slcPages" + this.gridname + "\" class=\"pgSlc\" style=\"font-weight: bold;padding:5px;\">";
                                        strhtml += "</select>";
                strhtml += " &nbsp;of&nbsp;<span id=\"pgspan" + this.gridname + "\">1</span>&nbsp;</span><button   style='background-color:darkgray;margin-right:5px;' class='btn btn-info'  title=\"Next page\" id=\"btnNextSpan" + this.gridname + "\"><i class=\"fa fa-forward\" aria-hidden=\"true\"></i></button><button  style='background-color:darkgray;margin-right:5px;' class='btn btn-info'  title=\"Last page\" id=\"btnLastSpan" + this.gridname + "\"><i class=\"fa fa-fast-forward\" aria-hidden=\"true\"></i></button>";
                                strhtml += "</div>";
                                
                                        strhtml += "<div class=\"col-3\" style='background-color:#363b41;border-radius:4px;padding:10px;'>";
                                        strhtml += "<div class=\"pgItem\"><span id=\"pgItem" + this.gridname + "\"></span>&nbsp;<span id=\"pgfooter" + this.gridname + "\"></span></div>";
                                        strhtml += "<div class=\"clearfloat\"></div>";
                                strhtml += "</div>";
                                strhtml += "</div>";
                    strhtml += " </td>";
                strhtml += "</tr>";
                $('#' + this.gridname).children('tfoot').html(strhtml);
                $('#pgItem' + this.gridname).html('Total Record : ' + TotalRecord + '&nbsp;<br>' + SumRes);
                var TotalPage = Math.ceil(TotalRecord / PagePerItem);
                if (TotalPage == 0) TotalPage = 1;
                $('#slcPages' + this.gridname).empty();
                
                for (p = 1; p <= TotalPage; p++) {
                    $('#slcPages' + this.gridname).append($('<option/>', {
                        value: p,
                        text: p
                    }));
                }
                $("#slcPages" + _Ctrl).val(this.CurrentPage);
                $('#pgspan' + _Ctrl).html(TotalPage);
               

                $("#slcPages" + _Ctrl).change(function () {
                    this.CurrentPage = $("#slcPages" + _Ctrl).val();
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                });
                $('#btnFirstSpan' + _Ctrl).click(
                    function () {
                        this.CurrentPage = 1;
                        UpdCurrentPage(_Ctrl, this.CurrentPage);
                        Rebind(_Ctrl);
                    }
                );
                $('#btnPrevSpan' + this.gridname).click(
                    function () {
                        this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                        this.CurrentPage -= 1;
                        if (this.CurrentPage <= 0) {
                            this.CurrentPage = 1;
                        }
                        UpdCurrentPage(_Ctrl, this.CurrentPage);
                        Rebind(_Ctrl);
                    }
                );
                $('#btnNextSpan' + _Ctrl).click(
                    function () {
                        this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                        
                        this.CurrentPage += 1;
                        if (this.CurrentPage >= TotalPage) {
                            this.CurrentPage = TotalPage;
                        }
                        UpdCurrentPage(_Ctrl, this.CurrentPage);
                        Rebind(_Ctrl);
                    }
                );
                $('#btnLastSpan' + this.gridname).click(
                    function () {
                        this.CurrentPage = TotalPage;
                        UpdCurrentPage(_Ctrl, this.CurrentPage);
                        Rebind(_Ctrl);
                    }
                );
                
                if (this.Initial) {

                    for (i = 0; i < this.Data.length; i++) {
                        $('#DivSort' + this.gridname + '_' + this.Data[i]).click(
                           function () {
                               var ctrl = $(this).attr('id').split('_')[0].replace('DivSort', '');
                               var colname = $(this).attr('id').split('_')[1];
                               _Sort(ctrl, colname);
                           }
                       );
                    }
                    this.Initial = false;
                    UpdInitial(this.gridname,this.Initial);
                }
                try 
                {
                    ActivateRole();
                }
                catch(ex)
                {
                    console.log(ex.Message);
                }
            },

            async: ASync ,
            error: function (er) {
                try {
                    var x = $.parseJSON(er.responseText);
                    show_msg(x.Message);
                }
                catch (ex) {
                    var ErrMsg = 'ไม่สามารถดึงข้อมูลได้';
                    $("#dialog").dialog({
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $("#dialogmsg").html('<center>' + ErrMsg + '</center>');
                    strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>&nbsp;</div></td></tr>";
                    $('#' + this.gridname).children('tbody').html(strhtml);
                }
            }
        });
       
        if(this.Search !='')
        {
            i=0;
            var Searchhtml = '';
            var Display = '';
            Searchhtml += '<div style="float:right;margin:2px;">';
            Searchhtml += '<div class="input-group mb-3">';
            if(this.Search == '2')
            {
                Searchhtml += '<div><select id="SelectCat' + this.gridname + '" class="form-control" style="width: 150px;font-size:1em;">';
                Searchhtml += '<option selected value="">All</option>';
                for (i = 0; i < this.SearchesDat.length; i++)
                {
                    Searchhtml += '<option   value=' + this.SearchesDat[i] + '>' + this.Searchcolumns[i] + '</option>';
                }

                Searchhtml += '</Select></div>&nbsp;';
            }
            
            Searchhtml += '<input type="text" class="form-control" style="font-size:1em;"; id="TxtSearch' + this.gridname + '" placeholder="ข้อมูลที่ต้องการค้นหา" style="font-size: 1.0em; border-radius: 1px;" onkeypress=EnterSearch(event,\"' + this.gridname + '\");>';
            Searchhtml += '<div class="input-group-append">';
            Searchhtml += '<button class="btn btn-outline-info" id="btSearch' + this.gridname + '" onClick=Search("' + this.gridname + '\") style="font-size: 1.0em; border-radius: 1px;" type="button"><i class="fa fa-search" aria-hidden="true"></i></button>';
            Searchhtml += '</div>';
            Searchhtml += '</div>';


            $('#divSearch'+ this.gridname).html(Searchhtml);
            //$('#SelectCat' + this.gridname).pqSelect({
            //    multiplePlaceholder: 'ประเภทการค้นหา',
            //    deselect: true,
            //    checkbox: true,
            //    search: true
            //}).on("change", function (evt) {
            //    var val = $(this).val();
            //});
        }
    }
}
function ChkSelectAll(Chk,Ctrl,PK)
{
    if($('#' + Chk).prop('checked'))
    {
        $.ajax({
            type: "POST",
            url: This + "/SelectAll",
            data: "{'Ctrl':'" + Ctrl + "','PK' : '" + PK + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#' + Ctrl + ' tr').each(function (i, row) {

                    var $row = $(row),
                        $check = $row.find('input[type=checkbox]');

                    $check.each(function (i, checkbox) {
                        $(checkbox).prop('checked', $('#' + Chk).prop('checked'));
                    });
                });
            },
            async: false,
            error: function (er) {
                try {
                    var x = $.parseJSON(er.responseText);
                    show_msg(x.Message);
                }
                catch (ex) {
                    alert(ex.responseText);
                }
            }
        });
    }
    else
    {
        $.ajax({
            type: "POST",
            url: This + "/UnSelectAll",
            data: "{'Ctrl' :'" + Ctrl + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('#' + Ctrl + ' tr').each(function (i, row) {

                    var $row = $(row),
                        $check = $row.find('input[type=checkbox]');

                    $check.each(function (i, checkbox) {
                        $(checkbox).prop('checked', $('#' + Chk).prop('checked'));
                    });
                }); 
            },
            async: false,
            error: function (er) {
                try {
                    var x = $.parseJSON(er.responseText);
                    show_msg(x.Message);
                }
                catch (ex) {
                    alert(ex.responseText);
                }
            }
        });
    }
}
function ChkSelect(Chk,Ctrl,ProjectId)
{
    if($('#' + Chk).prop('checked'))
    {
        $.ajax({
            type: "POST",
            url: This + "/Selected",
            data: "{'Ctrl':'" + Ctrl + "','ProjectId' : '" + ProjectId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try 
                {
                    Selected(Chk,Ctrl,ProjectId);
                }
                catch(ex)
                {

                }
            },
            async: false,
            error: function (er) {
                try {
                    var x = $.parseJSON(er.responseText);
                    show_msg(x.Message);
                }
                catch (ex) {
                    alert(ex.responseText);
                }
            }
        });
    }
    else
    {
        $.ajax({
            type: "POST",
            url: This + "/UnSelected",
            data: "{'Ctrl' :'" + Ctrl + "','ProjectId' : '" + ProjectId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                try 
                {
                    UnSelected(Chk,Ctrl,ProjectId);
                }
                catch(ex)
                {

                }
            },
            async: false,
            error: function (er) {
                try {
                    var x = $.parseJSON(er.responseText);
                    show_msg(x.Message);
                }
                catch (ex) {
                    alert(ex.responseText);
                }
            }
        });
    }
}
function Export(Ctrl)
{
    $.ajax({
        type: "POST",
        url: This + "/Export",
        data: "{'Ctrl':'" + Ctrl + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var url;
            url = response.d;
            $(location).attr("href",url);
            
          
        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
}
function _Sort(Ctrl, ColName) {
   
    $.ajax({
        type: "POST",
        url: This + "/Sort",
        data: "{'Ctrl' :'" + Ctrl + "','ColName' :'" + ColName + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            res = response.d;
            Rebind(Ctrl);
        },
        async: false,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                alert(ex.responseText);
            }
        }
    });
   
}
function Rebind(x)
{
    var _Ctrl;
    var result;
    var i,j,p;
    var val;
    var SelectCat = '';
    var SearchMsg = '';
    try 
    {
        SelectCat = $('#SelectCat' + x).val();
        SearchMsg = $('#TxtSearch' + x).val();
        var results = GetResource(x);
        this.gridname = results.Ctrl;
        this.PagePerItem = results.PagePerItem;
        this.CurrentPage = results.CurrentPage;
        this.SortName = results.SortName;
        this.Order = results.Order;
        this.columns = results.Column.split(',');
        this.Data = results.Data.split(',');
        this.Initial = results.Initial;
        this.EditButton = results.EditButton;
        this.DeleteButton = results.DeleteButton; 
        this.Panel =  results.Panel; 
        this.FullRowSelect = results.FullRowSelect;
        this.Multiselect = results.Multiselect;
        this.Criteria = results.Criteria;
        this.SearchesDat =  results.SearchesDat;
        this.Searchcolumns =  results.Searchcolumns;
        this.WPanel = results.WPanel;
        this.HPanel = results.HPanel;
        this.Panel = results.Panel;
    }
    catch(ex)
    {
    }

    var strhtml = '';
    var PagePerItem = this.PagePerItem;
    var ColSpan = this.columns.length;
    var TotalRecord = 0;
    if (this.EditButton != '' || this.DeleteButton !='')
    {
        ColSpan+=1;
    }

    $.ajax({
        type: "POST",
        url: This + "/Bind",
        data: "{'Ctrl' :'" + this.gridname + "','PagePerItem' : '" + this.PagePerItem + "','CurrentPage' : '" + this.CurrentPage + "','SortName' : '" + this.SortName + "','Order' : '" + this.Order + "','Column' : '" + this.columns.join() + "','Data' : '" + this.Data.join() + "','Initial' :'" + this.Initial + "','SelectCat' :'" + SelectCat + "','SearchMsg' :'" + SearchMsg +  "','EditButton' :'" + this.EditButton + "','DeleteButton' :'" + this.DeleteButton + "','Panel' :'" + this.Panel + "','FullRowSelect' :'" + this.FullRowSelect + "','Multiselect' :'" + this.Multiselect +  "','Criteria' : '" + this.Criteria +  "','SearchesDat' : '" + this.SearchesDat +  "','Searchcolumns' : '" + this.Searchcolumns + "','WPanel' :'" + this.WPanel  + "','HPanel' :'" + this.HPanel + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            var bfhtml;
            bfhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'><img src='../../pictures/ajax-loader.gif' alt='loading...' /></div></td></tr>";
            $('#' + this.gridname).children('tbody').html(bfhtml);
        },
        success: function (response) {
            result = response.d;
            _Ctrl = result.Ctrl;
            this.gridname = result.Ctrl;
            this.PagePerItem = result.PagePerItem;
            this.CurrentPage = result.CurrentPage;
            this.SortName = result.SortName;
            this.Order = result.Order;
            this.Column = result.Column.split(',');
            this.Data = result.Data.split(',');
            this.EditButton = result.EditButton;
            this.DeleteButton = result.DeleteButton; 
            this.Panel =  result.Panel; 
            this.FullRowSelect = result.FullRowSelect;
            this.Multiselect = result.Multiselect;
            this.Criteria = result.Criteria;
            this.SearchesDat =  result.SearchesDat;
            this.Searchcolumns =  result.Searchcolumns;
            this.WPanel = result.WPanel;
            this.HPanel = result.HPanel;
            this.Panel = result.Panel;
            $('#' + this.gridname).children('tbody').html('');
            
            if (result.ResData.length > 0) {
                var curitem = 0;

                for (i = 0; i < result.ResData.length; i++) {
                    val = result.ResData[i];    
                    if (this.FullRowSelect !='')
                    {
                        strhtml += "<tr id='GR" + _Ctrl + "_" + result.FullRowSelectEvent[i].Val + "' Onclick=\"RowSelect('" + _Ctrl + "','" + result.FullRowSelectEvent[i].Val +  "');\" style='Cursor:pointer'>";
                        //strhtml += "<tr Onclick=\"RowSelect('" + _Ctrl + "','" + result.FullRowSelectEvent[Number(this.CurrentPage) * i].Val +  "');\" style='Cursor:pointer'>";
                    }
                    else
                    {
                        strhtml += "<tr>";
                    }
                    if(this.Multiselect != "")
                    {
                       
                        //if (Number(this.CurrentPage) == 1)
                        //{
                        //    curitem  = i;
                        //}
                        //else
                        //{
                        //    curitem = ((Number(this.PagePerItem) * Number(this.CurrentPage-1)) + i);
                        //}
                        
                            //alert('CurItem = ' + curitem);
                            //alert('FullRowSelectEvent = ' + result.FullRowSelectEvent.length);
                            //alert('ResData = ' +  result.ResData.length);
                            if ($.inArray(result.FullRowSelectEvent[curitem].Val, result.Selection) != -1)
                            {
      
                                strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Checked Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                            }
                            else 
                            {
                                strhtml += "<td class=\"grid-item\"><input type='checkbox' id='Chk" +  _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val  + "' Onclick=\"ChkSelect('Chk" + _Ctrl + '_' + result.FullRowSelectEvent[curitem].Val + "','" + _Ctrl + "','" +  result.FullRowSelectEvent[curitem].Val + "');\" /></td>";
                            }
                        curitem +=1;
                        ColSpan +=1;
                    }
                    for (j = 0; j < val.length; j++) {
                        try 
                        {
                            if (this.Column[j].split('!').length == 1)
                            {
                                strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                            }
                            else
                            {
                               
                                if (this.Column[j].split('!')[1] == "H")
                                {

                                }
                                else if (this.Column[j].split('!')[1] == "R")
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:Right;'>" + val[j].Val + "</td>";
                                }
                                else if (this.Column[j].split('!')[1] == "C")
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:Center;'>" + val[j].Val + "</td>";
                                }
                                else
                                {
                                    strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                                }
                            }
                        }
                        catch(ex)
                        {
                            strhtml += "<td class=\"grid-item\" style='text-align:left;'>" + val[j].Val + "</td>";
                        }

                    }
                    strhtml += "</tr>";
                }
            }
            else {
                if(this.Multiselect != "")
                {
                    ColSpan +=1;
                }
                strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>ไม่พบข้อมูล</div></td></tr>";
               
            }
            $.ajax({
                type: "POST",
                url: This + "/GetTotalRecord",
                data: "{'Ctrl' :'" + this.gridname + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    TotalRecord = response.d;
                },
                async: false,
                error: function (er) {
                    try {
                        var x = $.parseJSON(er.responseText);
                        show_msg(x.Message);
                    }
                    catch (ex) {
                        alert(ex.responseText);
                    }
                }
            });
            $('#' + this.gridname).children('tbody').html(strhtml);
            if(this.Multiselect != "")
            {
                try{
                    var res_selected;
                    var i_selected;
                    $.ajax({
                        type: "POST",
                        url: This + "/GetResource",
                        data: "{'Ctrl' :'" + this.gridname + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            res_selected = response.d.Selection;
                            for(i_selected =0;i_selected < res_selected.length;i_selected++)
                            {            
                                $('#Chk' + _Ctrl + '_' + res_selected[i_selected]).prop('checked',true);
                            }
           
                       
                        },
                        async: false,
                        error: function (er) {
                            try {
                                var x = $.parseJSON(er.responseText);
                                show_msg(x.Message);
                            }
                            catch (ex) {
                                alert(ex.responseText);
                            }
                        }
                    });
                }
                catch(ex)
                {

                }
            }
            //strhtml = "<tr>";
            //strhtml += "<td id=\"DivFooter" + this.gridname + "\" colspan=\"" + ColSpan + "\">";
            //strhtml += "<div id=\"mdiv" + this.gridname + "\" class=\"mdiv\">";
            //strhtml += "<div class=\"internal\">";
            //strhtml += "<input type=\"button\" class=\"pgInp NavFirst\" title=\"First page\" id=\"btnFirstSpan" + this.gridname + "\"><input type=\"button\" class=\"pgInp NavPrevious\" title=\"Previous page\" id=\"btnPrevSpan" + this.gridname + "\"><span id=\"pgbeforespan" + this.gridname + "\" class=\"nbpg\">Page&nbsp;<select id=\"slcPages" + this.gridname + "\" class=\"pgSlc\" style=\"font-weight: bold;\">";
            //strhtml += "</select>";
            //strhtml += " &nbsp;of&nbsp;<span id=\"pgspan" + this.gridname + "\">1</span>&nbsp;</span><input type=\"button\" class=\"pgInp NavNext\" title=\"Next page\" id=\"btnNextSpan" + this.gridname + "\"><input type=\"button\" class=\"pgInp NavLast\" title=\"Last page\" id=\"btnLastSpan" + this.gridname + "\">";
            //strhtml += "</div>";
            //strhtml += "</div>";
            //strhtml += "<div class=\"pgItem\"><span id=\"pgfooter" + this.gridname + "\"></span><span id=\"pgItem" + this.gridname + "\"></span></div>";
            //strhtml += "<div class=\"clearfloat\"></div>";
            //strhtml += " </td>";
            //strhtml += "</tr>";
            //$('#' + this.gridname).children('tfoot').html(strhtml);
            //$('#pgItem' + this.gridname).html('Total Record : ' + TotalRecord);

                var SumRes = '';
                if (this.Criteria.indexOf('#SUM') > -1 )
                {
                    var x = 0;
                    
                    var Arr = this.Criteria.toString().replace('!','').split('|');
                    for(x=0;x<Arr.length;x++)
                    {
                        if (Arr[x].toString().indexOf('#SUM') > -1)
                        {
                            $.ajax({
                                type: "POST",
                                url: This + "/GetSummary",
                                data: "{'dat':'" +  Arr[x] + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (response) {
                                    SumRes = response.d;
                                    SumRes = '<span style="font-weight:0.1;color:blue;font-size:11px;">' + SumRes + '</span>';
                                    
                                },
                                async: false,
                                error: function (er) {
                                    try {
                                        var x = $.parseJSON(er.responseText);
                                        show_msg(x.Message);
                                    }
                                    catch (ex) {
                                        alert(ex.responseText);
                                    }
                                }
                            });
                          
                        }
                    }
                  
                }
                else
                {
                    SumRes = '';
                }
            strhtml = "<tr style='background-color:#1466a4;border-radius:4px;color:white;'>";
            strhtml += "<td id=\"DivFooter" + this.gridname + "\" colspan=\"" + ColSpan + "\">";
            strhtml += "<div id=\"mdiv" + this.gridname + "\" class=\"container-fluid\">";
            strhtml += "<div class=\"row\"  >";
            strhtml += "<div class=\"col-9\" style='text-align:right;'>";
            strhtml += "<button class='btn btn-info' title=\"First page\" id=\"btnFirstSpan" + this.gridname + "\"><i class=\"fa fa-fast-backward\" aria-hidden=\"true\"></i></button><button class='btn btn-info'  title=\"Previous page\" id=\"btnPrevSpan" + this.gridname + "\"><i class=\"fa fa-backward\" aria-hidden=\"true\"></i></button><span id=\"pgbeforespan" + this.gridname + "\" class=\"nbpg\">Page&nbsp;<select  id=\"slcPages" + this.gridname + "\" class=\"pgSlc\" style=\"font-weight: bold;\">";
            strhtml += "</select>";
            strhtml += " &nbsp;of&nbsp;<span id=\"pgspan" + this.gridname + "\">1</span>&nbsp;</span><button class='btn btn-info'  title=\"Next page\" id=\"btnNextSpan" + this.gridname + "\"><i class=\"fa fa-forward\" aria-hidden=\"true\"></i></button><button class='btn btn-info'  title=\"Last page\" id=\"btnLastSpan" + this.gridname + "\"><i class=\"fa fa-fast-forward\" aria-hidden=\"true\"></i></button>";
            strhtml += "</div>";

            strhtml += "<div class=\"col-3\" style='background-color:#363b41;border-radius:4px;padding:10px;'>";
            strhtml += "<div class=\"pgItem\"><span id=\"pgItem" + this.gridname + "\"></span>&nbsp;<span id=\"pgfooter" + this.gridname + "\"></span></div>";
            strhtml += "<div class=\"clearfloat\"></div>";
            strhtml += "</div>";
            strhtml += "</div>";
            strhtml += " </td>";
            strhtml += "</tr>";
                $('#' + this.gridname).children('tfoot').html(strhtml);
                $('#pgItem' + this.gridname).html('Total Record : ' + TotalRecord + '&nbsp;<br>' + SumRes);


            var TotalPage = Math.ceil(TotalRecord / PagePerItem);
            if (TotalPage == 0) TotalPage = 1;
            $('#slcPages' + this.gridname).empty();
                
            for (p = 1; p <= TotalPage; p++) {
                $('#slcPages' + this.gridname).append($('<option/>', {
                    value: p,
                    text: p
                }));
            }
            $("#slcPages" + _Ctrl).val(this.CurrentPage);
            $('#pgspan' + _Ctrl).html(TotalPage);


            $("#slcPages" + _Ctrl).change(function () {
                this.CurrentPage = $("#slcPages" + _Ctrl).val();
                UpdCurrentPage(_Ctrl, this.CurrentPage);
                Rebind(_Ctrl);
            });
            $('#btnFirstSpan' + _Ctrl).click(
                function () {
                    this.CurrentPage = 1;
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnPrevSpan' + this.gridname).click(
                function () {
                    this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                    this.CurrentPage -= 1;
                    if (this.CurrentPage <= 0) {
                        this.CurrentPage = 1;
                    }
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnNextSpan' + _Ctrl).click(
                function () {
                    this.CurrentPage = Number($("#slcPages" + _Ctrl).val());
                        
                    this.CurrentPage += 1;
                    if (this.CurrentPage >= TotalPage) {
                        this.CurrentPage = TotalPage;
                    }
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            $('#btnLastSpan' + this.gridname).click(
                function () {
                    this.CurrentPage = TotalPage;
                    UpdCurrentPage(_Ctrl, this.CurrentPage);
                    Rebind(_Ctrl);
                }
            );
            if (this.Initial) {

                for (i = 0; i < this.Data.length; i++) {
                    $('#DivSort' + this.gridname + '_' + this.Data[i]).click(
                       function () {
                           var ctrl = $(this).attr('id').split('_')[0].replace('DivSort', '');
                           var colname = $(this).attr('id').split('_')[1];
                           _Sort(ctrl, colname);
                       }
                   );
                }
                UpdInitial(this.gridname,this.Initial);
            }
            try 
            {
                ActivateRole();
            }
            catch(ex)
            {
                console.log(ex.Message);
            }
        },
        async: true,
        error: function (er) {
            try {
                var x = $.parseJSON(er.responseText);
                show_msg(x.Message);
            }
            catch (ex) {
                var ErrMsg = 'ไม่สามารถดึงข้อมูลได้';
                $("#dialog").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
                $("#dialogmsg").html('<center>' + ErrMsg + '</center>');
                strhtml = "<tr><td colspan='" + ColSpan + "'><div style='height: 50px;margin-top: 25px;color: red; font-size:15px;vertical-align:bottom;text-align : center;'>&nbsp;</div></td></tr>";
                $('#' + this.gridname).children('tbody').html(strhtml);
            }
        }
    });
}

