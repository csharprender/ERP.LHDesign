<%@ Page Title="" Language="C#" MasterPageFile="~/Page/EDWF/Forms/Control.Master" AutoEventWireup="true" CodeBehind="WHT.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Forms.WHT.WHT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../../../js/Numeric.js"></script>
    <script>
        var This = "WHT.aspx";

        function Unselperiod(x) {
            var json = x;
            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/Unselperiod",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Renderitem();
                },
                async: false,
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
        function Selperiod(x) {
            var json = x;

            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/Selperiod",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Renderitem();
                },
                async: false,
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

        function Renderitem() {
            var html = '';
            var i = 0
            $.ajax({
                type: "POST",
                url: This + "/Getitem",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                  
                    if (response.d.length == 0) {
                        html = '<div style="color:red;padding:50px;" >ไม่พบงวดงาน</div>';

                    }
                    else {
                        html = '<div class="container">';
                        html += '<div class="row" style="margin-top:10px;">'
                        html += '<div class="col-1">'
                        html += '<span>งวดที่</span>';
                        html += '</div>';
                        html += '<div class="col-8">'
                        html += 'รายละเอียดงวด';
                        html += '</div>';
                        html += '<div class="col-3">'
                        html += 'จำนวนเงิน';
                        html += '</div>';
                        html += '</div>';
                        html += '<div class="row" style="margin-top:10px;">'
                        html += '<div class="col-12">'
                        html += '<hr>';
                        html += '</div>';
                        html += '</div>';
                        for (i = 0; i < response.d.length; i++) {
     
                            if (response.d[i]['Selected'] == 'x') {
                               
                                html += '<div class="row" style="border-radius:4px; padding:30px;border:1px solid gray;margin-top:10px;background-color:lightgray;color:black;cursor:pointer;" onclick="Unselperiod(' + response.d[i]['Period'] + ');">';
                                html += '<div class="col-1">'
                                html += '<span>' + (i + 1) + '</span>';
                                html += '</div>';
                                html += '<div class="col-8">'
                                html += '<input type="text" class="form-control" Readonly="readonly" id="Txtperiodname_' + i.toString() + '" value="' + response.d[i]['Periodname'] + '" />';
                                html += '</div>';
                                html += '<div class="col-3">'
                                html += '<input type="text" class="form-control" Readonly="readonly" style="text-align:right;" id="Txtperiodamount_' + i.toString() + '" ' + '" value="' + response.d[i]['Amount'] + '"/>';
                                html += '</div>';
                                html += '</div>';
                            }
                            else {

                                html += '<div class="row" style="border-radius:4px;padding:30px;border:1px solid gray;margin-top:10px;background-color:white;color:black;cursor:pointer;" onclick="Selperiod(' + response.d[i]['Period'] + ');">';
                                html += '<div class="col-1">'
                                html += '<span>' + (i + 1) + '</span>';
                                html += '</div>';
                                html += '<div class="col-8">'
                                html += '<input type="text" class="form-control" Readonly="readonly" id="Txtperiodname_' + i.toString() + '" value="' + response.d[i]['Periodname'] + '" />';
                                html += '</div>';
                                html += '<div class="col-3">'
                                html += '<input type="text" class="form-control" Readonly="readonly" style="text-align:right;" id="Txtperiodamount_' + i.toString() + '" ' + '" value="' + response.d[i]['Amount'] + '"    />';
                                html += '</div>';
                                html += '</div>';
                            }
                        }
                        html += '</div>';
                    }
                    $("#Divitem").html(html);
                    Getgrandtotal();
                    Calc();
                    
                },
                async: false,
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
        function Getgrandtotal() {
            var json = '';
            $.ajax({
                type: "POST",
                url: This + "/Getgrandtotal",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    $('#TxtPayDate_6').val(response.d["Date"]);
                    $('#TxtPayAmount_6').val(response.d["Amount"]);
                    $('#TxtPayWHT_6').val(response.d["WHT"]);
                 
                },
                async: false,
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
        function Calc() {
            var totalwht = 0;
            var totalamount = 0;
            $("input[id*='TxtPayAmount_']").each(function (i, el) {
                if ($(el).val() != '') {
                    totalamount += Number($(el).val().replace(',', ''));
                }
            });
            $("input[id*='TxtPayWHT_']").each(function (i, el) {
                if ($(el).val() != '') {
                    totalwht += Number($(el).val().replace(',', ''));
                }
            });
           
            $('#TxtTotalAmount').val(FormatNumber(parseFloat(totalamount)));
            $('#TxtTotalTax').val(FormatNumber(parseFloat(totalwht)));
          
        }
        function Newdocument() {
            $('#Txtattachmentlabel').val('');
            $('#Divupload').modal('show');
        }
        function CallBackUpload(Label, Running) {
            $.ajax({
                type: "POST",
                url: This + "/CallBackUpload",
                data: "{'Label' : '" + Label + "','Running' :'" + Running + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Renderupload();
                    $('#Divupload').modal('hide');
                },
                async: false,
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
        function Doupload() {
            var key = $('#Txtattachmentlabel').val();
            w = 600;
            h = 400;
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            window.open('/Attachment/Upload.aspx?key=' + key, '_blank', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        }
        function Deleteattachment(x) {
            var json = '';
            json = x;
            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/Deleteattachment",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d != "") {
                        Msgbox(response.d);
                        return;
                    }
                    Renderupload();
                },
                async: false,
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
        function Downloadattachment(x) {
            var json = '';
            json = x;
            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/Downloadattachment",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "") {
                        Msgbox('ไม่สามารถค้นหาไฟล์ได้ โปรดติดต่อผู้ดูแลระบบ');
                        return;
                    }
                    w = 600;
                    h = 400;
                    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
                    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
                    var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
                    var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
                    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
                    var top = ((height / 2) - (h / 2)) + dualScreenTop;
                    window.open(response.d, '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=800,height=800');
                },
                async: false,
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
        function Renderupload() {
            var json = '';
            var html = '';
            $.ajax({
                type: "POST",
                url: This + "/Getupload",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '  <div class="col-12" style="text-align: left; margin-top: 20px;"> ';
                    html += ' <div style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;" > ';
                    html += ' <div class="row"> ';
                    html += ' <div class="col-lg-2 col-md-4 col-sm-6 col-xs-12"> ';
                    html += ' <div class="div-document" style="cursor:pointer;" onclick="Newdocument(\'\');">';
                    html += ' <div class="container" style="padding: 10px;"> ';
                    html += ' <div class="row"> ';
                    html += ' <div class="col-12" style="text-align: center;"> ';
                    html += ' <img src="/Img/EDOC/newdoc.png" style="width: 60px;"> ';
                    html += ' </div> ';
                    html += ' <div class="col-12" style="text-align: center;"> ';
                    html += ' <div>แนบไฟล์เพิ่ม</div> ';
                    html += ' </div> ';
                    html += ' <div class="col-12" style="text-align: center; margin-top: 30px;"> ';
                    html += ' <div style="font-size: 0.8em; color: red;">ขนาดไฟล์ไม่เกิน 25 MB</div> ';
                    html += ' </div> ';
                    html += ' </div> ';
                    html += ' </div> ';
                    html += ' </div> ';
                    html += ' </div> ';
                    for (i = 0; i < response.d.length; i++) {
                        html += ' <div class="col-lg-2 col-md-4 col-sm-6 col-xs-12">';
                        html += ' <div class="div-document" id="divattchment_' + response.d[i]['Attachmentid'] + '">';
                        html += ' <div class="container" style="padding: 10px;">';
                        html += ' <div class="row">';
                        html += ' <div class="col-12" style="text-align: center;">';
                        html += ' <div onclick="Deleteattachment(\'' + response.d[i]['Attachmentid'] + '\');" style="position:absolute;right:20px;top:-30px;font-size:1.5em;color:red;cursor:pointer;"><i class="fa fa-trash" aria-hidden="true"></i></div>';
                        html += ' <img src="/Img/EDOC/1.png" style="width: 60px;cursor:pointer;"  onclick="Downloadattachment(\'' + response.d[i]['Attachmentid'] + '\');">';
                        html += ' </div>';
                        html += ' <div class="col-12" style="text-align: center;">';
                        html += ' <div>' + response.d[i]['Label'] + '</div>';
                        html += ' </div>';
                        html += ' <div class="col-12" style="text-align: center; margin-top: 10px;">';
                        html += ' <div style="font-size: 0.8em;">' + response.d[i]['Uploaddate'] + '</div>';
                        html += ' </div>';
                        html += ' <div class="col-12" style="text-align: center;">';
                        html += ' <div style="font-size: 0.8em;">โดย ' + response.d[i]['Uploadbynameth'] + '</div>';
                        html += ' </div>';
                        html += ' </div>';
                        html += ' </div>';
                        html += ' </div>';
                        html += ' </div>';

                    }
                    html += ' </div>';
                    html += '  </div> ';
                    html += ' </div> ';
                    $('#Divuploads').html(html);

                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function GetFooter(readonly) {
            var html = '';
            var i = 0;
            var Ismultiple = false;
            var json = 'readonly:' + readonly + ',';
            $.ajax({
                type: "POST",
                url: This + "/GetFooter",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    html += '<table style="width:100%" border="0">';
                    for (i = 0; i < out.length; i++) {
                        if (out[i]["NodeMultipleId"] == null) {
                            if (Ismultiple == true) {
                                html += '</table>';
                                html += '</div>';
                                html += '</div>';
                                html += '</td>';
                                html += '</tr>';
                            }

                            html += '<tr>';
                            html += '<td style="vertical-align:top;width:20%;">';
                            html += '<div class = "panel panel-info">';
                            html += '<div class = "panel-heading">';
                            html += '<h3 class = "panel-title">สถานะการอนุมัติ</h3>';
                            html += '</div>';
                            html += '<div class = "panel-body">';
                            html += '<table style="width:100%;">';
                            html += '<tr>';
                            html += '<td style="width:40%;"><span>ผลการอนุมัติ</span></td><td><span style="font-weight:bold;color:green;">' + out[i]["ActionResultNameTH"] + '</span></td>';
                            html += '</tr>';
                            html += '<tr>';
                            html += '<td><span>อนุมัติเมื่อ</span></td><td><span>' + out[i]["ActionStringDate"] + '</span></td>';
                            html += '</tr>';
                            html += '</table>';
                            html += '</div>';
                            html += '</div>';
                            html += '</td>';
                            html += '<td style="width:10px;">&nbsp;</td>'
                            html += '<td style="vertical-align:top;text-align:center;width:50%;">';
                            if (i == (out.length - 1)) {
                                if (readonly) {
                                    html += '<textarea class="form-control" disabled="disabled"  id="TxtRemark_' + out[i]["Id"] + '" style="width:100%;height:110px;backgound-color:lightgrey">' + out[i]["Remark"] + '</textarea>';
                                }
                                else {
                                    html += '<textarea class="form-control"  id="TxtRemark_' + out[i]["Id"] + '" style="width:100%;height:100px;">' + out[i]["Remark"] + '</textarea>';
                                }

                            }
                            else {
                                html += '<textarea class="form-control" disabled="disabled"  id="TxtRemark_' + out[i]["Id"] + '" style="width:100%;height:110px;backgound-color:lightgrey">' + out[i]["Remark"] + '</textarea>';
                            }
                            html += '</td>';
                            html += '<td style="width:10px;"><table style="width:100%"><tr><td colspan="2" style="text-align:center;"><img class="img-signature" id="ImgSignature_' + out[i]["Id"] + '" src="' + out[i]["SignatureURL"] + '"></td></tr><tr><td>&nbsp;</td><td style="text-align:center;"><span id="SpActionBy_' + out[i]["Id"] + '">' + out[i]["ActionByTitleNameTH"] + ' ' + out[i]["ActionByFirstNameTH"] + ' ' + out[i]["ActionByLastNameTH"] + '<br/>' + out[i]["ActionByPositionNameTH"] + '<br/>' + out[i]["ActionByOrganizeNameTH"] + '</span></td></tr></table></td>';
                            html += '</tr>';
                        }
                        else {
                            if (!Ismultiple) {
                                Ismultiple = true;
                                html += '<tr>';
                                html += '<td colspan="4">';
                                html += '<div class = "panel panel-warning">';
                                html += '<div class = "panel-heading">';
                                html += '<h3 class = "panel-title">กลุ่มการอนุมัติ</h3>';
                                html += '</div>';
                                html += '<div class = "panel-body">';
                                html += '<table style="width:100%">';
                            }
                            html += '<tr>';
                            html += '<td style="vertical-align:top;width:20%;">';
                            html += '<div class = "panel panel-info">';
                            html += '<div class = "panel-heading">';
                            html += '<h3 class = "panel-title">สถานะการอนุมัติ</h3>';
                            html += '</div>';
                            html += '<div class = "panel-body">';
                            html += '<table style="width:100%;" border="0">';
                            html += '<tr>';
                            html += '<td style="width:40%;"><span>ผลการอนุมัติ</span></td><td><span style="font-weight:bold;color:green;">' + out[i]["ActionResultNameTH"] + '</span></td>';
                            html += '</tr>';
                            html += '<tr>';
                            html += '<td><span>อนุมัติเมื่อ</span></td><td><span>' + out[i]["ActionStringDate"] + '</span></td>';
                            html += '</tr>';
                            html += '</table>';
                            html += '</div>';
                            html += '</div>';
                            html += '</td>';
                            html += '<td style="width:10px;">&nbsp;</td>'
                            html += '<td style="vertical-align:top;text-align:center;width:50%;">';


                            if (out[i]["Ismyremark"] == "x") {
                                html += '<textarea class="form-control"  id="TxtRemark_' + out[i]["Id"] + '" style="width:100%;height:100px;">' + out[i]["Remark"] + '</textarea>';
                            }
                            else {
                                html += '<textarea class="form-control" disabled="disabled"  id="TxtRemark_' + out[i]["Id"] + '" style="width:100%;height:110px;backgound-color:lightgrey">' + out[i]["Remark"] + '</textarea>';
                            }
                            html += '</td>';
                            html += '<td style="width:10px;"><table style="width:100%"><tr><td colspan="2" style="text-align:center;"><img class="img-signature" id="ImgSignature_' + out[i]["Id"] + '" src="' + out[i]["SignatureURL"] + '"></td></tr><tr><td>&nbsp;</td><td style="text-align:center;"><span id="SpActionBy_' + out[i]["Id"] + '">' + out[i]["ActionByTitleNameTH"] + ' ' + out[i]["ActionByFirstNameTH"] + ' ' + out[i]["ActionByLastNameTH"] + '<br/>' + out[i]["ActionByPositionNameTH"] + '<br/>' + out[i]["ActionByOrganizeNameTH"] + '</span></td></tr></table></td>';
                        }
                    }
                    html += '</table>';
                    $('#DivSignnatures').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function Print() {

            var json = $('#Cmddocumentprint').attr('data-value');
            $.ajax({
                type: "POST",
                url: This + "/Print",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d != '') {
                        w = 600;
                        h = 400;
                        var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
                        var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
                        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
                        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
                        var left = ((width / 2) - (w / 2)) + dualScreenLeft;
                        var top = ((height / 2) - (h / 2)) + dualScreenTop;
                        window.open(response.d, '');

                       

                    }
                },
                async: false,
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
        function Selhirecontract(json) {
            $.ajax({
                type: "POST",
                url: This + "/Selhirecontract",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == null) {
                        Msgbox('System error , Please hirecontract administrator');
                        return;
                    }

                    $('#Divhirecontract').modal('hide');
                    $('#Txthirecontract').val(response.d['Hirecontractno']);
                    $('#Txthirecontract').attr('data-value', response.d['Referhirecontractid']);
                    $('#Txtcertfullname').val(response.d['Certfullname']);
                    $('#Txtcertfulladdress').val(response.d['CertFullAddress']);
                    $('#Txtcerttaxno1').val(response.d['CertTaxno1']);
                    


                    $('#Txtfullname').val(response.d['Fullname']);
                    $('#Txtfulladdress').val(response.d['FullAddress']);

                    Renderitem();
                   
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function RowSelect(ctrl, x) {
            var json = '';
            json = 'x :' + x + '|';

            if (ctrl == 'Gvhirecontract') {
                Selhirecontract(json);
            }
        }

        function GetRoute() {
            var json = '';
            $.ajax({
                type: "POST",
                url: This + "/GetRoute",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    $('#CbAction').find('option').remove().end();
                    for (i = 0; i < out.length; i++) {
                        val = out[i];

                        $('#CbAction').append('<option id="' + val["Val"] + '" value="' + val["Val"] + '">' + val["Name"] + '</option>');
                    }
                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function GetDocumentInfo(out) {
            var json = '';
            var out;
            $.ajax({
                type: "POST",
                url: This + "/GetDocumentInfo",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {

                },
                success: function (response) {
                    if (response.d['Err'] != '') {
                        Msgbox(response.d['Err']);
                        return;
                    }
                    $('#SpDocumentCreateBy').html(out["CreateBy"]);
                    $('#SpISODocument').html(response.d["EFormcode"]);
                    $('#SpDocumentNo').html(response.d["Documentno"]);
                    $('#SpDocumentCreateDate').html(response.d["DocumentDate"]);

                    $('#Imgcompanylogo').attr('src', response.d['Companylogourl']);
                    $('#Spcompanyname').html(response.d['Companyname']);
                    $('#Spcompanyaddress').html(response.d['Companyaddress']);
                    $('#Spcompanytel').html('Tel. ' + response.d['Companytel']);
                    $('#Divformheader').html(response.d['Formheader']);
                    if ($('#HdINQ').val() == "1") { //Inquiry
                        GetFooter(true);
                        $('#SpActionFlow').hide();
                        $('#CmdAction').hide();
                        $('#CbAction').hide();
                        $('#Divdocumentdate').html(response.d['Documentdate']);
                        $('#Spdocumentno').html(response.d['Documentno']);
                        $('#Hdflowconfigid').val(response.d['Flowconfigid']);
                        $('#Hdcurrentnode').val(response.d['Nodenamefrom']);
                        $('#Imgcompanylogo').attr('src', response.d['Companylogourl']);
                        $('#Diveformname').html(response.d['Eformname']);
                        $('#Spcompanyname').html(response.d['Companyname']);
                        $('#Spcompanyaddress').html(response.d['Companyaddress']);
                        $('#Spcompanytel').html('Tel. ' + response.d['Companytel']);
                        $('#Divdocumentcode').html(response.d['EFormcode']);
                        $('#Divdocumentdate').html(response.d['Documentdate']);
                        $('#Divdocumentversion').html(response.d['Version']);
                        $('#Spdocumentstatus').html(response.d['Statustext']);
                        $('#Spdocumentno').html(response.d['Documentno']);

                        $('#Txthirecontract').val(response.d['Hirecontractno']);
                        $('#Txthirecontract').attr('data-value', response.d['Referhirecontractid']);


                        $('#Txtcertfullname').val(response.d['Certfullname']);
                        $('#Txtcerttaxno1').val(response.d['CertTaxno1']);
                        $('#Txtcertfulladdress').val(response.d['CertFullAddress']);
                        $('#Txtcerttaxno2').val(response.d['CertTaxno2']);

                        $('#Txtfullname').val(response.d['Fullname']);
                        $('#Txttaxno1').val(response.d['Taxno1']);
                        $('#Txtfulladdress').val(response.d['FullAddress']);
                        $('#Txttaxno2').val(response.d['Taxno2']);




                        $('#Txtorderinform').val(response.d['Orderinform']);
                        $('#Txtdocno').val(response.d['Docno']);
                        $('#Txtbookno').val(response.d['Bookno']);

      
   
   
                        if (response.d['PND1A'] == 'X') {
                            
                            $('#ChkPND1A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND1A').prop('checked', false);
                        }

                        if (response.d['PND1AExtra'] == 'X') {
                            $('#ChkPND1AExtra').prop('checked', true);
                        }
                        else {
                            $('#ChkPND1AExtra').prop('checked', false);
                        }
                        if (response.d['PND2'] == 'X') {
                            
                            $('#ChkPND2').prop('checked', true);
                        }
                        else {
                            $('#ChkPND2').prop('checked', false);
                        }

                        if (response.d['PND3'] == 'X') {
                            
                            $('#ChkPND3').prop('checked', true);
                        }
                        else {
                            $('#ChkPND3').prop('checked', false);
                        }

                        if (response.d['PND2A'] == 'X') {
                            $('#ChkPND2A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND2A').prop('checked', false);
                        }

                        if (response.d['PND3A'] == 'X') {
                            $('#ChkPND3A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND3A').prop('checked', false);
                        }

                        if (response.d['PND53'] == 'X') {
                          
                            $('#ChkPND53').prop('checked', true);
                        }
                        else {
                            $('#ChkPND53').prop('checked', false);
                        }

                        $('#TxtPayDate_1').val(response.d['PayDate_1']);
                        $('#TxtPayAmount_1').val(response.d['PayAmount_1']);
                        $('#TxtPayWHT_1').val(response.d['PayWHT_1']);
                        $('#TxtPayDate_2').val(response.d['PayDate_2']);
                        $('#TxtPayAmount_2').val(response.d['PayAmount_2']);
                        $('#TxtPayWHT_2').val(response.d['PayWHT_2']);

                        $('#TxtPayDate_3').val(response.d['PayDate_3']);
                        $('#TxtPayAmount_3').val(response.d['PayAmount_3']);
                        $('#TxtPayWHT_3').val(response.d['PayWHT_3']);
                        $('#TxtPayDate_4A').val(response.d['PayDate_4A']);
                        $('#TxtPayAmount_4A').val(response.d['PayAmount_4A']);
                        $('#TxtPayWHT_4A').val(response.d['PayWHT_4A']);


                        $('#TxtPayDate_4B1_1').val(response.d['PayDate_4B1_1']);
                        $('#TxtPayAmount_4B1_1').val(response.d['PayAmount_4B1_1']);
                        $('#TxtPayWHT_4B1_1').val(response.d['PayWHT_4B1_1']);
                        $('#TxtPayDate_4B1_2').val(response.d['PayDate_4B1_2']);



                        $('#TxtPayAmount_4B1_2').val(response.d['PayAmount_4B1_2']);
                        $('#TxtPayWHT_4B1_2').val(response.d['PayWHT_4B1_2']);
                        $('#TxtPayDate_4B1_3').val(response.d['PayDate_4B1_3']);
                        $('#TxtPayAmount_4B1_3').val(response.d['PayAmount_4B1_3']);
                        $('#TxtPayWHT_4B1_3').val(response.d['PayWHT_4B1_3']);


                        $('#TxtPayRemark_4B1_4').val(response.d['PayRemark_4B1_4']);
                        $('#TxtPayDate_4B1_4').val(response.d['PayDate_4B1_4']);
                        $('#TxtPayAmount_4B1_4').val(response.d['PayAmount_4B1_4']);
                        $('#TxtPayWHT_4B1_4').val(response.d['PayWHT_4B1_4']);
                        $('#TxtPayDate_4B2_1').val(response.d['PayDate_4B2_1']);
                        $('#TxtPayAmount_4B2_1').val(response.d['PayAmount_4B2_1']);
                        $('#TxtPayWHT_4B2_1').val(response.d['PayWHT_4B2_1']);
                        $('#TxtPayDate_4B2_2').val(response.d['PayDate_4B2_2']);
                        $('#TxtPayAmount_4B2_2').val(response.d['PayAmount_4B2_2']);
                        $('#TxtPayWHT_4B2_2').val(response.d['PayWHT_4B2_2']);
                        $('#TxtPayDate_4B2_3').val(response.d['PayDate_4B2_3']);
                        $('#TxtPayAmount_4B2_3').val(response.d['PayAmount_4B2_3']);


                        $('#TxtPayWHT_4B2_3').val(response.d['PayWHT_4B2_3']);
                        $('#TxtPayDate_4B2_4').val(response.d['PayDate_4B2_4']);
                        $('#TxtPayAmount_4B2_4').val(response.d['PayAmount_4B2_4']);
                        $('#TxtPayWHT_4B2_4').val(response.d['PayWHT_4B2_4']);


                        $('#TxtRemark_4B2_5').val(response.d['PayRemark_4B2_5']);
                        $('#TxtPayDate_4B2_5').val(response.d['PayDate_4B2_5']);
                        $('#TxtPayAmount_4B2_5').val(response.d['PayAmount_4B2_5']);

                        $('#TxtPayWHT_4B2_5').val(response.d['PayWHT_4B2_5']);
                        $('#TxtPayDate_5').val(response.d['PayDate_5']);
                        $('#TxtPayAmount_5').val(response.d['PayAmount_5']);

                        $('#TxtPayWHT_5').val(response.d['PayWHT_5']);
                        $('#TxtRemark_6').val(response.d['PayRemark_6']);
                        $('#TxtPayDate_6').val(response.d['PayDate_6']);
                        $('#TxtPayAmount_6').val(response.d['PayAmount_6']);
                        $('#TxtPayWHT_6').val(response.d['PayWHT_6']);



                        $('#TxtTotalAmount').val(response.d['TotalAmount']);
                        $('#TxtTotalTax').val(response.d['TotalTax']);
                        $('#TxtPayforTeacherAidFund').val(response.d['PayforTeacherAidFund']);
                        $('#TxtPayforSocialSecurityFund').val(response.d['PayforSocialSecurityFund']);

                        $('#TxtPayforProvidentFund').val(response.d['PayforProvidentFund']);
                        $('#TxtPayforOtherRemark').val(response.d['PayforOtherRemark']);






                        if (response.d['PayMethodWHT'] == 'X') {
                            $('#ChkPayMethodWHT').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodWHT').prop('checked', false);
                        }


                        if (response.d['PayMethodRecuring'] == 'X') {
                            $('#ChkPayMethodRecuring').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodRecuring').prop('checked', false);
                        }



                        if (response.d['PayMethodOncetime'] == 'X') {
                            $('#ChkPayMethodOncetime').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodOncetime').prop('checked', false);
                        }

                        if (response.d['PayforOther'] == 'X') {
                            $('#ChkPayforOther').prop('checked', true);
                        }
                        else {
                            $('#ChkPayforOther').prop('checked', false);
                        }
                       
                        Calc();
                        Renderupload();
                        if (out["NodeNameTo"].toLowerCase() == "") {
                            //Completed
                            $("#DivformBody").find('input:text, input:password, input:file, select, textarea')
                                .each(function () {
                                    $(this).attr('readonly', 'readonly');
                                });
                            $("#DivformBody").find('button')
                                .each(function () {
                                    //$(this).attr('disabled', 'disabled');
                                    $(this).hide();
                                });
                            $('#Txtperiod').attr('readonly', 'readonly');

                            if (out["Iscomplete"] == "1") {
                                $('#Cmdprint').show();
                                $('#Spbadgecancel').hide();
                                $('#Spbadgecomplete').show();
                            }
                            else {
                                $('#Cmdprint').hide();
                                $('#Spbadgecancel').show();
                                $('#Spbadgecomplete').hide();
                            }
                        }
                        else if (out["NodeNameTo"].toLowerCase() == "nodebegin1") {

                        }
                        else if (out["NodeNameTo"].toLowerCase() == "nodesingle1") {

                        }
                    }
                    else {
                        GetFooter(false);
                        $('#Divdocumentdate').html(response.d['Documentdate']);
                        $('#Spdocumentno').html(response.d['Documentno']);
                        $('#Hdflowconfigid').val(response.d['Flowconfigid']);
                        $('#Hdcurrentnode').val(response.d['Nodenamefrom']);
                        $('#Imgcompanylogo').attr('src', response.d['Companylogourl']);
                        $('#Diveformname').html(response.d['Eformname']);
                        $('#Spcompanyname').html(response.d['Companyname']);
                        $('#Spcompanyaddress').html(response.d['Companyaddress']);
                        $('#Spcompanytel').html('Tel. ' + response.d['Companytel']);
                        $('#Divdocumentcode').html(response.d['EFormcode']);
                        $('#Divdocumentdate').html(response.d['Documentdate']);
                        $('#Divdocumentversion').html(response.d['Version']);
                        $('#Spdocumentstatus').html(response.d['Statustext']);
                        $('#Spdocumentno').html(response.d['Documentno']);

                      
                        $('#Txthirecontract').val(response.d['Hirecontractno']);
                        $('#Txthirecontract').attr('data-value', response.d['Referhirecontractid']);

                        $('#Txtcertfullname').val(response.d['Certfullname']);
                        $('#Txtcerttaxno1').val(response.d['CertTaxno1']);
                        $('#Txtcertfulladdress').val(response.d['CertFullAddress']);
                        $('#Txtcerttaxno2').val(response.d['CertTaxno2']);

                        $('#Txtfullname').val(response.d['Fullname']);
                        $('#Txttaxno1').val(response.d['Taxno1']);
                        $('#Txtfulladdress').val(response.d['FullAddress']);
                        $('#Txttaxno2').val(response.d['Taxno2']);

                        $('#Txtorderinform').val(response.d['Orderinform']);
                        $('#Txtdocno').val(response.d['Docno']);
                        $('#Txtbookno').val(response.d['Bookno']);


                        if (response.d['PND1A'] == 'X') {
                            
                            $('#ChkPND1A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND1A').prop('checked', false);
                        }

                        if (response.d['PND1AExtra'] == 'X') {
                            $('#ChkPND1AExtra').prop('checked', true);
                        }
                        else {
                            $('#ChkPND1AExtra').prop('checked', false);
                        }
                        if (response.d['PND2'] == 'X') {
                            
                            $('#ChkPND2').prop('checked', true);
                        }
                        else {
                            $('#ChkPND2').prop('checked', false);
                        }

                        if (response.d['PND3'] == 'X') {

                            $('#ChkPND3').prop('checked', true);
                        }
                        else {
                            $('#ChkPND3').prop('checked', false);
                        }

                        if (response.d['PND2A'] == 'X') {
                            $('#ChkPND2A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND2A').prop('checked', false);
                        }

                        if (response.d['PND3A'] == 'X') {
                            $('#ChkPND3A').prop('checked', true);
                        }
                        else {
                            $('#ChkPND3A').prop('checked', false);
                        }

                        if (response.d['PND53'] == 'X') {
                          
                            $('#ChkPND53').prop('checked', true);
                        }
                        else {
                            $('#ChkPND53').prop('checked', false);
                        }
                
                        $('#TxtPayDate_1').val(response.d['PayDate_1']);
         
                        $('#TxtPayAmount_1').val(response.d['PayAmount_1']);
                        $('#TxtPayWHT_1').val(response.d['PayWHT_1']);
                        $('#TxtPayDate_2').val(response.d['PayDate_2']);
                        $('#TxtPayAmount_2').val(response.d['PayAmount_2']);
                        $('#TxtPayWHT_2').val(response.d['PayWHT_2']);

                        $('#TxtPayDate_3').val(response.d['PayDate_3']);
                        $('#TxtPayAmount_3').val(response.d['PayAmount_3']);
                        $('#TxtPayWHT_3').val(response.d['PayWHT_3']);
                        $('#TxtPayDate_4A').val(response.d['PayDate_4A']);
                        $('#TxtPayAmount_4A').val(response.d['PayAmount_4A']);
                        $('#TxtPayWHT_4A').val(response.d['PayWHT_4A']);


                        $('#TxtPayDate_4B1_1').val(response.d['PayDate_4B1_1']);
                        $('#TxtPayAmount_4B1_1').val(response.d['PayAmount_4B1_1']);
                        $('#TxtPayWHT_4B1_1').val(response.d['PayWHT_4B1_1']);
                        $('#TxtPayDate_4B1_2').val(response.d['PayDate_4B1_2']);



                        $('#TxtPayAmount_4B1_2').val(response.d['PayAmount_4B1_2']);
                        $('#TxtPayWHT_4B1_2').val(response.d['PayWHT_4B1_2']);
                        $('#TxtPayDate_4B1_3').val(response.d['PayDate_4B1_3']);
                        $('#TxtPayAmount_4B1_3').val(response.d['PayAmount_4B1_3']);
                        $('#TxtPayWHT_4B1_3').val(response.d['PayWHT_4B1_3']);


                        $('#TxtPayRemark_4B1_4').val(response.d['PayRemark_4B1_4']);
                        $('#TxtPayDate_4B1_4').val(response.d['PayDate_4B1_4']);
                        $('#TxtPayAmount_4B1_4').val(response.d['PayAmount_4B1_4']);
                        $('#TxtPayWHT_4B1_4').val(response.d['PayWHT_4B1_4']);
                        $('#TxtPayDate_4B2_1').val(response.d['PayDate_4B2_1']);
                        $('#TxtPayAmount_4B2_1').val(response.d['PayAmount_4B2_1']);
                        $('#TxtPayWHT_4B2_1').val(response.d['PayWHT_4B2_1']);
                        $('#TxtPayDate_4B2_2').val(response.d['PayDate_4B2_2']);
                        $('#TxtPayAmount_4B2_2').val(response.d['PayAmount_4B2_2']);
                        $('#TxtPayWHT_4B2_2').val(response.d['PayWHT_4B2_2']);
                        $('#TxtPayDate_4B2_3').val(response.d['PayDate_4B2_3']);
                        $('#TxtPayAmount_4B2_3').val(response.d['PayAmount_4B2_3']);


                        $('#TxtPayWHT_4B2_3').val(response.d['PayWHT_4B2_3']);
                        $('#TxtPayDate_4B2_4').val(response.d['PayDate_4B2_4']);
                        $('#TxtPayAmount_4B2_4').val(response.d['PayAmount_4B2_4']);
                        $('#TxtPayWHT_4B2_4').val(response.d['PayWHT_4B2_4']);


                        $('#TxtRemark_4B2_5').val(response.d['PayRemark_4B2_5']);
                        $('#TxtPayDate_4B2_5').val(response.d['PayDate_4B2_5']);
                        $('#TxtPayAmount_4B2_5').val(response.d['PayAmount_4B2_5']);

                        $('#TxtPayWHT_4B2_5').val(response.d['PayWHT_4B2_5']);
                        $('#TxtPayDate_5').val(response.d['PayDate_5']);
                        $('#TxtPayAmount_5').val(response.d['PayAmount_5']);

                        $('#TxtPayWHT_5').val(response.d['PayWHT_5']);
                        $('#TxtRemark_6').val(response.d['PayRemark_6']);
                        $('#TxtPayDate_6').val(response.d['PayDate_6']);
                        $('#TxtPayAmount_6').val(response.d['PayAmount_6']);
                        $('#TxtPayWHT_6').val(response.d['PayWHT_6']);



                        $('#TxtTotalAmount').val(response.d['TotalAmount']);
                        $('#TxtTotalTax').val(response.d['TotalTax']);
                        $('#TxtPayforTeacherAidFund').val(response.d['PayforTeacherAidFund']);
                        $('#TxtPayforSocialSecurityFund').val(response.d['PayforSocialSecurityFund']);

                        $('#TxtPayforProvidentFund').val(response.d['PayforProvidentFund']);
                        $('#TxtPayforOtherRemark').val(response.d['PayforOtherRemark']);






                        if (response.d['PayMethodWHT'] == 'X') {
                            $('#ChkPayMethodWHT').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodWHT').prop('checked', false);
                        }


                        if (response.d['PayMethodRecuring'] == 'X') {
                            $('#ChkPayMethodRecuring').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodRecuring').prop('checked', false);
                        }



                        if (response.d['PayMethodOncetime'] == 'X') {
                            $('#ChkPayMethodOncetime').prop('checked', true);
                        }
                        else {
                            $('#ChkPayMethodOncetime').prop('checked', false);
                        }

                        if (response.d['PayforOther'] == 'X') {
                            $('#ChkPayforOther').prop('checked', true);
                        }
                        else {
                            $('#ChkPayforOther').prop('checked', false);
                        }
                        Renderitem();
                        Calc();
                        Renderupload();
                        if (out["NodeNameTo"].toLowerCase() == "") {

                        }
                        else if (out["NodeNamefrom"].toLowerCase() == 'nodebegin1'.toLowerCase()) {


                        }
                        else if (out["NodeNamefrom"].toLowerCase() == 'nodesingle1'.toLowerCase()) {



                        }
                    }
                },
                async: false,
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
            //Node Attachment
        }
        function GetInfo() {
            var json = '';
            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/GetInfo",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    GetDocumentInfo(out);

                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function Save() {
            var html = '';
            var i = 0;
            var json = 'readonly:false|';
            var Div = 'DivSignnatures';
            json += 'Txthirecontract: ' + $('#Txthirecontract').val() + '|';
            json += 'ActionResultValue: ' + $('#CbAction').val() + '|';
            json += 'ActionResultNameTH: ' + $('#CbAction option:selected').text() + '|';
            $("#" + Div).find('textarea').each(function () {
                if ($(this).attr('disabled') != 'disabled') {
                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                }
            });
            $("#Divscreen").find('textarea').each(function () {

                json += $(this).attr('id') + ':' + $(this).val() + "|";

            });
            $("#Divscreen").find('input[type = text]').each(function () {
            
                 json += $(this).attr('id') + ':' + $(this).val() + "|";
                
            });
            $("#Divscreen").find('input[type = checkbox]').each(function () {
            
                json += $(this).attr('id') + ':' + $(this).prop('checked') + "|";
                
            });
            $("[id*='Dtpinvoicedate']").each(function () {

                json += $(this).attr('id') + ':' + $(this).val() + "|";

            });
            json += 'Txthirecontract_dat:'  + $('#Txthirecontract').attr('data-value');
            //$("[id*='Dtppaymentdate']").each(function () {

            //    json += $(this).attr('id') + ':' + $(this).val() + "|";

            //});
            $.ajax({
                type: "POST",
                url: This + "/Save",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {

                },
                success: function (response) {
                    out = response.d;
                    window.opener.CallBackRefresh(out);
                    window.close();
                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        $(function () {
            var i = 0;
            var json = '';
            var html = '';

            $("#TxtPayDate_1").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_2").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_3").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_4A").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_4B1_1").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_4B1_2").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_4B1_3").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_4B1_4").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_4B2_1").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#TxtPayDate_4B2_2").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_4B2_3").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_4B2_4").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_4B2_5").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_5").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#TxtPayDate_6").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $('#CmdAction').on('click', function () {
                if ($('#CbAction').val() == "") {
                    Msgbox('โปรดระบุเส้นทางการส่งเอกสาร');
                    return;
                }
                Save();
            });
            GetRoute();
            GetInfo();
            $(this).attr('title', 'ใบหักภาษี ณ ที่จ่าย');


            //$('#TxtPayDate_1').val('');
            //$('#TxtPayDate_2').val('');
            //$('#TxtPayDate_3').val('');
            //$('#TxtPayDate_4A').val('');
            //$('#TxtPayDate_4B1_1').val('');
            //$('#TxtPayDate_4B1_2').val('');
            //$('#TxtPayDate_4B1_3').val('');
            //$('#TxtPayDate_4B1_4').val('');

            //$('#TxtPayDate_4B2_1').val('');
            //$('#TxtPayDate_4B2_2').val('');
            //$('#TxtPayDate_4B2_3').val('');
            //$('#TxtPayDate_4B2_4').val('');
            //$('#TxtPayDate_4B2_5').val('');
            //$('#TxtPayDate_5').val('');
            //$('#TxtPayDate_6').val('');

        });
        function Searchhirecontract() {

            ClearResource('Gvhirecontract');
            var Columns = ["เลขที่เอกสารสัญญา!L","Outsource!L","Site งาน!L", "จำนวนเงิน!R"];
            var Data = ["Documentno","Fullname","Sitename","Grandtotal"];
            var Searchcolumns = ["เลขที่เอกสารสัญญา", "Outsource"];
            var SearchesDat = ["Documentno", "Fullname"];
            var Width = ["15%", "25%", "30%", "30%"];
            var grdhirecontract = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvhirecontract', 30, Width, Data, "", '', '1', '', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
            $('#Divhirecontractcont').html(grdhirecontract._Tables());
            grdhirecontract._Bind();

        }
        function Newhirecontract() {
            $('#Divhirecontract').modal('show');
            Searchhirecontract();
        }
    </script>
    <div class="container-fluid" id="Divscreen">
        <div class="row mt-3">
            <div class="col-8" style="color:red;text-align:right;">
                <span>เอกสารสัญญาจ้างงาน</span>
            </div>
            <div class="col-4">
                <div class="input-group mb-3">
                    <input type="text" class="form-control" readonly="readonly" id="Txthirecontract" data-value="" />
                    <button class="btn btn-info" onclick="Newhirecontract();"><i class="fa fa-search"></i></button>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <div id="Divitem" style="min-height: 50px;text-align:center; border:solid 1px lightgray;padding:50px;">
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <div class="container-fluid" style="background-color: #ffffff; border: solid 1px lightgray; min-height: 200px; padding: 20px;">

                    <div class="row mt-3">
                        <div class="col-5" style="color:red;text-align:right;">
                            &nbsp;
                        </div>
                        <div class="col-4" style="text-align:right;">
                            <span>เลขที่ประจำตัวผู้เสียภาษีอากร (13 หลัก )</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                            <input id="Txtcerttaxno1" onkeypress="return isNumberKey(event)" class="form-control" type="text" />
                        </div>
                    </div>
                    <div class="row mt-3">

                        <div class="col-2" style="text-align:right;">
                            <span>ผู้หักภาษี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <input id="Txtcertfullname" class="form-control" type="text" />
                        </div>
                        <div class="col-2" style="text-align:right;">
                            <span>เลขผู้เสียภาษี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                            <input type="text" id="Txtcerttaxno2" onkeypress="return isNumberKey(event)" class="form-control" />
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-2" style="text-align:right;">
                            <span>ที่อยู่</span>
                        </div>
                        <div class="col-5">
                            <div class="input-group mb-3">
                                <textarea class="form-control" id="Txtcertfulladdress" data-value="" style="height: 50px;"></textarea>
                            </div>
                        </div>
                        <div class="col-2" style="text-align:right;">
                           
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                          
                        </div>
                    </div>

                      <div class="row mt-3">
                        <div class="col-12" style="color:red;text-align:right;">
                            <hr />
                        </div>
                       
                    </div>

                    <div class="row mt-3">
                        <div class="col-5" style="color:red;text-align:right;">
                            &nbsp;
                        </div>
                        <div class="col-4" style="text-align:right;">
                            <span>เลขที่ประจำตัวผู้เสียภาษีอากร (13 หลัก )</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                            <input id="Txttaxno1" onkeypress="return isNumberKey(event)" class="form-control" type="text" />
                        </div>
                    </div>
                    <div class="row mt-3">

                        <div class="col-2" style="text-align:right;">
                            <span>ผู้ถูกหักภาษี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <input id="Txtfullname" class="form-control" type="text" />
                        </div>
                        <div class="col-2" style="text-align:right;">
                            <span>เลขผู้เสียภาษี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                            <input type="text" id="Txttaxno2" onkeypress="return isNumberKey(event)" class="form-control" />
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-2" style="text-align:right;">
                            <span>ที่อยู่</span>
                        </div>
                        <div class="col-5">
                            <div class="input-group mb-3">
                                <textarea class="form-control" id="Txtfulladdress" data-value="" style="height: 50px;"></textarea>
                            </div>
                        </div>
                        <div class="col-2" style="text-align:right;">
                            <span>ลำดับที่ (ในใบแนบ)</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="color:red;text-align:right;">
                            <input type="text" id="Txtorderinform" class="form-control" />
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-2">
                            &nbsp;
                        </div>
                        <div class="col-5">
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND1A" value="PND1A">
                                <label class="form-check-label" for="ChkPND1A">(1) ภ.ง.ด.1ก</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND1AExtra" value="PND1AExtra">
                                <label class="form-check-label" for="ChkPND1AExtra">(2) ภ.ง.ด.1ก พิเศษ</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND2" value="PND2">
                                <label class="form-check-label" for="ChkPND2">(3) ภ.ง.ด.2</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND3" value="PND3">
                                <label class="form-check-label" for="ChkPND3">(4) ภ.ง.ด.3</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND2A" value="PND2A">
                                <label class="form-check-label" for="ChkPND2A">(5) ภ.ง.ด.2ก</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND3A" value="PND3A">
                                <label class="form-check-label" for="ChkPND3A">(6) ภ.ง.ด.3ก</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPND53" value="PND53">
                                <label class="form-check-label" for="ChkPND53">(7) ภ.ง.ด.53</label>
                            </div>
                        </div>
                        <div class="col-2" style="text-align:right;">
                           เลขที่ /เล่มที่
                        </div>
                        <div class="col-2">
                          <div class="input-group">
                              <input type="text" class="form-control" id="Txtdocno" />
                              &nbsp;
                              <input type="text" class="form-control" id="Txtbookno" />
                          </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid" style="font-size:11px!important;">
        <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">รายละเอียดใบหัก ณ ที่จ่าย</div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 50px;">ประเภทเงินได้พึงประเมินที่จ่าย</span>
            </div>
            <div class="col-2">
                <span>วัน เดือน หรือปีภาษีที่จ่าย</span>
            </div>
            <div class="col-2">
                <span>จำนวนเงินที่จ่าย</span>
            </div>
            <div class="col-2">
                <span>ภาษีที่หัก และนำส่งไว้</span>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <hr style="width: 98%;" />
            </div>
        </div>
        <div class="row mt-3">

            <div class="col-6">
                <span style="margin-left: 50px;">1.เงินเดือน ค่าจ้าง โบนัส ตามมาตรา 40  (1)</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_1'>
                    <input id="TxtPayDate_1" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 50px;">2.ค่าธรรมเนียม ค่านายหน้า ตามมาตรา 40 (2)</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_2'>
                    <input id="TxtPayDate_2" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 50px;">3.ค่าแห่งลิขสิทธิ์ ตามมาตรา 40 (3)</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_3'>
                    <input id="TxtPayDate_3" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 50px;">4.(ก) ค่าดอกเบี้ย ตามมาตรา ตามมาตรา 40 (4) (ก)</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4A'>
                    <input id="TxtPayDate_4A" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4A" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4A" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>


        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 80px;">(ข) เงินปันผล เงินส่วนแบ่งกำไร ตามมาตรา 40 (4) (ข) ที่จ่ายจาก</span>
            </div>
            <div class="col-2">
                &nbsp;
            </div>
            <div class="col-2">
                &nbsp;
           
            </div>
            <div class="col-2">
                &nbsp;
             
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 80px;">(1) กิจการที่ต้องเสียเงินได้ นิติบุคคลในอัตราดังนี้</span>
            </div>
            <div class="col-2">
                &nbsp;
            </div>
            <div class="col-2">
                &nbsp;
           
            </div>
            <div class="col-2">
                &nbsp;
             
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(1.1) อัตราร้อยละ 30  ของกำไรสุทธิ</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B1_1'>
                    <input id="TxtPayDate_4B1_1" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B1_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B1_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(1.2) อัตราร้อยละ 25 ของกำไรสุทธิ</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B1_2'>
                    <input id="TxtPayDate_4B1_2" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B1_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B1_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>


        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(1.3) อัตราร้อยละ 20 ของกำไรสุทธิ</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B1_3'>
                    <input id="TxtPayDate_4B1_3" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B1_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B1_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>


        <div class="row mt-3">
            <div class="col-6">
                <div class="input-group mb-3">
                    <span style="margin-left: 100px;">(1.4) อัตรา</span><input type="text" id="TxtPayRemark_4B1_4" class="form-control" style="width: 50px; margin: 5px;" /><span>ของกำไรสุทธิ</span><span>&nbsp;</span>
                </div>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B1_4'>
                    <input id="TxtPayDate_4B1_4" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B1_4" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B1_4" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>

        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 80px;">(2) กิจการที่ได้รับการยกเว้นภาษีเงินได้นิติบุคคลซึ่งผู้รับเงินปันผลไมได้รับเครดิตภาษี</span>
            </div>
            <div class="col-2">
                &nbsp;
            </div>
            <div class="col-2">
                &nbsp;
           
            </div>
            <div class="col-2">
                &nbsp;
             
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(2.1) กำไรสุทธิของกิจการที่ได้รับการยกเว้นภาษีเงินได้นิติบุคคล</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B2_1'>
                    <input id="TxtPayDate_4B2_1" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B2_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B2_1" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>



        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(2.2) เงินปันผลหรือส่วนแบ่งของกำไรที่ได้รับการยกเว้นไม่ต้องนำมารวมคำนวณเป็นรายได้เพื่อเสียภาษีเงินได้นิติบุคคล</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B2_2'>
                    <input id="TxtPayDate_4B2_2" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B2_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B2_2" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>



        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(2.3) กำไรสุทธิส่วนที่ได้หักผลขาดทุนสุทธิยกมาไม่เกิน 5 ปี ก่อนรอบระยะเวลาบัญชีปีปัจจุบัน</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B2_3'>
                    <input id="TxtPayDate_4B2_3" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B2_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B2_3" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>


        <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 100px;">(2.4) กำไรที่รับรู้ทางบัญชีโดยวิธีส่วนได้เสีย (equity method)</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B2_4'>
                    <input id="TxtPayDate_4B2_4" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B2_4" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B2_4" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>

        

         <div class="row mt-3">
            <div class="col-6">
                <div class="input-group mb-3">
                    <span style="margin-left: 100px;">(2.5) อื่นๆ ระบุ</span>&nbsp;
                    <input id="TxtRemark_4B2_5" class="form-control" type="text" />
                </div>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_4B2_5'>
                    <input id="TxtPayDate_4B2_5" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_4B2_5" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_4B2_5" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
       
         <div class="row mt-3">
            <div class="col-6">
                <span style="margin-left: 50px;">5.การจ่ายเงินได้ที่ต้องหักภาษี ณ ที่จ่าย ตามคำสั่งกรมสรรพากรที่ออกตามมาตรา 3 เตรส เช่น ค่าซื้อพืชผลทางการเกษตร (ยางพารา มันสำปะหลัง ข้าว) รางวัลในการประกวด การแข่งขัน การชิงโชค ร้องเพลง ค่าบริการ ค่าแสดงภาพยนตร์ ดนตรี มหรสพ ค่าจ้างทำของ ส่วนลดหรือประโยชน์ใดๆ เนื่องจากการส่งเสริมการขาย ค่าโฆษณา ค่าเช่า ค่าเบี้ยประกัน วินาศภัย ค่าจ้างขนส่ง เงินสะสมจ่ายเข้ากองทุนสำรองเบี้ยเลี้ยงชีพ</span>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_5'>
                    <input id="TxtPayDate_5" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_5" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_5" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>


                  <div class="row mt-3">
            <div class="col-6">
                <div class="input-group mb-3">
                <span style="margin-left: 50px;">6.อื่นๆ (ระบุ)</span>
                    &nbsp;
                    <input type="text" class="form-control" id="TxtRemark_6" />
                </div>
            </div>
            <div class="col-2">
                <div class='input-group date' id='DtpPayDate_6'>
                    <input id="TxtPayDate_6" class="form-control" readonly="readonly" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayAmount_6" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
            <div class="col-2">
                <input type="text" onkeyup="Calc();" id="TxtPayWHT_6" onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" />

            </div>
        </div>
          </div>  

        <div class="row mt-3">
            <div class="col-12">
                <hr />
            </div>
        </div>

        <div class="row mt-3">
           <div class="col-8" style="text-align:right;">
                <span style="margin-left: 50px;">รวมเงินที่จ่ายและภาษีที่หักนำส่ง</span>
            </div>
           
            <div class="col-2">
                <input type="text" id="TxtTotalAmount"  onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;" readonly="readonly" />

            </div>
            <div class="col-2">
                <input type="text" id="TxtTotalTax"  onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;"  readonly="readonly"  />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <hr />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-4" style="text-align:right;">
                <span>เงินที่จ่ายเข้า กบข./กสจ./กองทุนสงเคราะห์ครูโรงเรียนเอกชน</span>
            </div>
             <div class="col-1">
                <input type="text" id="TxtPayforTeacherAidFund"  onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;"    />
            </div>
             <div class="col-2"  style="text-align:right;">
                <span>กองทุนประกันสังคม</span>
            </div>
             <div class="col-1">
                <input type="text" id="TxtPayforSocialSecurityFund"  onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;"   />
            </div>
             <div class="col-2"  style="text-align:right;">
                <span>กองทุนสำรองเลี้ยงชีพ</span>
            </div>
             <div class="col-1">
                <input type="text" id="TxtPayforProvidentFund"  onkeypress="return isNumberKey(event)" class="form-control" style="color:red;text-align:right;"   />
            </div>
        </div>
          <div class="row mt-3">
            <div class="col-12">
                <hr />
            </div>
        </div>
           <div class="row mt-3">
                        <div class="col-4" style="text-align:right;">
                          <span>ผู้จ่ายเงิน</span>
                        </div>
                        <div class="col-8">
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPayMethodWHT" value="PayMethodWHT">
                                <label class="form-check-label" for="ChkPayMethodWHT">หัก ณ ที่จ่าย</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPayMethodRecuring" value="PayMethodRecuring">
                                <label class="form-check-label" for="ChkPayMethodRecuring">ออกให้ตลอดไป</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPayMethodOncetime" value="PayMethodOncetime">
                                <label class="form-check-label" for="ChkPayMethodOncetime">ออกให้ครั้งเดียว</label>
                            </div>
                             <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="ChkPayforOther" value="PayforOther">
                                <label class="form-check-label" for="ChkPayforOther">อื่นๆ</label>&nbsp;
                                <input type="text" id="TxtPayforOtherRemark" class="form-control" />
                            </div>
                            
                        </div>
                        <div class="col-5">
                            &nbsp;
                        </div>
                    </div>
          <div class="row mt-3">
            <div class="col-12">
                <hr />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 20px;">
                <div class="card card-body bg-light">ไฟล์แนบ</div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12" id="Divuploads" style="text-align: left; margin-top: 20px;">
                <div style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;">
                    <div class="row">
                        <div class="col-2">
                            <div class="div-document" onclick="Newdocument('EFORM001');">
                                <div class="container" style="padding: 10px;">
                                    <div class="row">

                                        <div class="col-12" style="text-align: center;">

                                            <img src="../../Img/EDOC/newdoc.png" style="width: 60px;" />
                                        </div>
                                        <div class="col-12" style="text-align: center;">
                                            <div>แนบไฟล์เพิ่ม</div>
                                        </div>
                                        <div class="col-12" style="text-align: center; margin-top: 30px;">
                                            <div style="font-size: 0.8em; color: red;">ขนาดไฟล์ไม่เกิน 25 MB</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="div-document">
                                <div class="container" style="padding: 10px;">
                                    <div class="row">
                                        <div class="col-12" style="text-align: center;">
                                            <img src="../../Img/EDOC/1.png" style="width: 60px;" />
                                        </div>

                                        <div class="col-12" style="text-align: center;">
                                            <div>บัตรประชาชนลูกค้า</div>
                                        </div>
                                        <div class="col-12" style="text-align: center; margin-top: 30px;">
                                            <div style="font-size: 0.8em;">แนบเมื่อ 01/05/2020 11:45</div>
                                        </div>
                                        <div class="col-12" style="text-align: center;">
                                            <div style="font-size: 0.8em;">โดย มนัสวิน เอมะศิริ</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-2">
                            <div class="div-document">
                                <div class="container" style="padding: 10px;">
                                    <div class="row">
                                        <div class="col-12" style="text-align: center;">
                                            <img src="../../Img/EDOC/1.png" style="width: 60px;" />
                                        </div>

                                        <div class="col-12" style="text-align: center;">
                                            <div>สำเนาทะเบียนบ้าน</div>
                                        </div>
                                        <div class="col-12" style="text-align: center; margin-top: 30px;">
                                            <div style="font-size: 0.8em;">แนบเมื่อ 01/05/2020 11:45</div>
                                        </div>
                                        <div class="col-12" style="text-align: center;">
                                            <div style="font-size: 0.8em;">โดย มนัสวิน เอมะศิริ</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>



                </div>
            </div>

        </div>
        <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">สายการอนุมัติ</div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12" style="text-align: left; margin-top: 20px;">
                <div id="DivSignnatures" style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;">
                </div>
            </div>
        </div>
    </div>


    <div class="modal" id="Divhirecontract" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <span>เอกสารงวดงาน</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divhirecontractcont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" style="font-size: 0.9em; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>

    <div class="modal" id="Divupload" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-md" id="Divuploadcont">
            <div class="modal-content">
                <div class="modal-header">
                    <span>แนบเอกสาร</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container">
                        <div class="row">
                            <div class="col-2">
                                <span>เอกสาร</span>&nbsp;<span style="color: red">*</span>
                            </div>
                            <div class="col-10">
                                <input type="text" class="form-control" id="Txtattachmentlabel" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" style="font-size: 1.0em; border-radius: 0;" onclick="Doupload();">แนบเอกสาร</button>
                    <button type="button" class="btn btn-danger" style="font-size: 1.0em; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
