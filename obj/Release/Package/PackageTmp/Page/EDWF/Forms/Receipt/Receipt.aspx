<%@ Page Title="" Language="C#" MasterPageFile="~/Page/EDWF/Forms/Control.Master" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Forms.Receipt.Receipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        var This = "Receipt.aspx";
     
        function Getgrandtotal() {
            var json = '';
            $.ajax({
                type: "POST",
                url: This + "/Getgrandtotal",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#Txtgrandtotal').val(response.d);
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
        function Print2() {
            var json = '';
            $.ajax({
                type: "POST",
                url: This + "/PrintInvoice",
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
                                
                                html += '<div class="row" style="border-radius:4px; padding:30px;border:1px solid gray;margin-top:10px;background-color:lightgray;color:black;">';
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

                                html += '<div class="row" style="border-radius:4px;padding:30px;border:1px solid gray;margin-top:10px;background-color:white;color:black;">';
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
        function Selrequisition(json) {
            $.ajax({
                type: "POST",
                url: This + "/Selrequisition",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == null) {
                        Msgbox('System error , Please requisition administrator');
                        return;
                    }

                    $('#Divrequisition').modal('hide');
                    $('#Txtrequisition').val(response.d['Documentno']);
                    $('#Txtrequisition').attr('data-value', response.d['Referrequisitionid']);
                    $('#Txtcontactorname').val(response.d['fullname']);
                    $('#Txtcontactoraddress').val(response.d['address']);
                    $('#Txtcontactortel').val(response.d['Tel']);
                    $('#Txtcontactdate').val(response.d['Contactdate']);
                    $('#TxtcontactorcardID').val(response.d['Cardid']);
                    $('#Txtcontactorexprirydate').val(response.d['Expirydate']);
                    $("#Txtbank").val(response.d['Banknameth']);
                    $('#Txtcontactorbankaccountno').val(response.d['Bankaccountno']);
                    $('#Txtcontactorbankaccountname').val(response.d['Bankaccountname']);
                    $("#Txtbankaccounttype").val(response.d['Bankaccounttype']);
                    $('#Txtsitename').val(response.d['Sitename']);
                    $('#Txtsiteaddress').val(response.d['Sitefulladdress']);
                    $('#Txtjobdesc').val(response.d['Jobdescription']);
                    $('#Txtfee').val(response.d['Fee']);
                    $('#Txtfinisheddate').val(response.d['Finisheddate']);
                    $('#Txttotalamount').val(response.d['Totalamount']);
                    $('#Txtcontactstart').val(response.d['Contactstart']);
                    $('#Txtcontactend').val(response.d['Contactend']);
                    Renderitem();

                    //Adjust

                },
                async: true,
                error: function (er) {

                }
            });
        }
        function RowSelect(ctrl, x) {
            var json = '';
            json = 'x :' + x + '|';
           
            if (ctrl == 'Gvrequisition') {
                Selrequisition(json);
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
                        Selrequisition('x :' + response.d['Referrequisitionid'] + '|');
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
                                $('#Cmdprint2').show();
                                $('#Spbadgecancel').hide();
                                $('#Spbadgecomplete').show();
                            }
                            else {
                                $('#Cmdprint').hide();
                                $('#Cmdprint2').hide();
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

                        if (response.d['Referrequisitionid'] != '') {
                           
                            Selrequisition('x :' + response.d['Referrequisitionid'] + '|');
                        }
                        else {
                            Renderitem();
                        }
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
            json += 'Txtrequisition: ' + $('#Txtrequisition').attr('data-value') + '|';
            json += 'ActionResultValue: ' + $('#CbAction').val() + '|';
            json += 'ActionResultNameTH: ' + $('#CbAction option:selected').text() + '|';
            $("#" + Div).find('textarea').each(function () {
                if ($(this).attr('disabled') != 'disabled') {
                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                }
            });
            $("[id*='Dtpinvoicedate']").each(function () {

                json += $(this).attr('id') + ':' + $(this).val() + "|";

            });
            $("[id*='Dtppaymentdate']").each(function () {

                json += $(this).attr('id') + ':' + $(this).val() + "|";

            });
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
            $('#CmdAction').on('click', function () {
                if ($('#CbAction').val() == "") {
                    Msgbox('โปรดระบุเส้นทางการส่งเอกสาร');
                    return;
                }
                Save();
            });
            GetRoute();
            GetInfo();
            $(this).attr('title', 'ใบสั่งของ');
        });
        function Searchrequisition() {

            ClearResource('Gvrequisition');
            var Columns = ["เลขที่ใบงวดงาน!L", "วันที่แจ้งหนี้!!C","วันกำหนดชำระ!C", "ลูกค้า!L", "จำนวนเงิน!R"];
            var Data = ["Documentno", "Invoicedate", "Paymentdate","Fullname", "Grandtotal"];
            var Searchcolumns = ["เลขที่ใบงวดงาน", "ลูกค้า"];
            var SearchesDat = ["Documentno", "Fullname"];
            var Width = ["20%", "15%", "15%","30%", "20%"];
            var grdrequisition = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvrequisition', 30, Width, Data, "", '', '1', '', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
            $('#Divrequisitioncont').html(grdrequisition._Tables());
            grdrequisition._Bind();

        }
        function Newrequisition() {
            $('#Divrequisition').modal('show');
            Searchrequisition();
        }
    </script>
    <div class="container-fluid">
        <div class="row mt-3">
            <div class="col-8" style="text-align: right;">
                <span>เอกสารงวดงาน</span>
            </div>
            <div class="col-4">
                <div class="input-group mb-3">
                    <input type="text" class="form-control" readonly="readonly" id="Txtrequisition" data-value="" />
                    <button class="btn btn-info" onclick="Newrequisition();"><i class="fa fa-search"></i></button>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <div class="container-fluid" style="background-color: #ffffff; border: solid 1px lightgray; min-height: 200px; padding: 20px;">
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-2" style="text-align: right;">
                            <span>ผู้รับจ้าง</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <div class="input-group mb-3">
                                <input type="text" id="Txtcontactorname" data-value="" readonly="readonly" class="form-control" placeholder="กดค้นหาผู้รับจ้าง" style="font-size: 1.0em; border-radius: 1px;" readonly="readonly">
                            </div>
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>สัญญามีผลบังคับใช้</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <div class='input-group date' id='Dtpcontactdate'>
                                <input id="Txtcontactdate" class="form-control" type="text" readonly="readonly" />
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>

                            </div>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2" style="text-align: right;">
                            <span>ที่อยู่</span>
                        </div>
                        <div class="col-5">
                            <div class="input-group mb-3">
                                <textarea class="form-control" id="Txtcontactoraddress" data-value="" readonly="readonly" style="height: 50px;"></textarea>
                            </div>
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>เบอร์โทรศัพท์</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <input type="text" id="Txtcontactortel" readonly="readonly" onkeypress="return isNumberKey(event)" class="form-control" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2" style="text-align: right;">
                            <span>บัตรประชาชน</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <div class="input-group mb-3">
                                <input type="text" id="TxtcontactorcardID" readonly="readonly" onkeypress="return isNumberKey(event)" class="form-control" />

                            </div>
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>บัตรหมดอายุ</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <div class='input-group date' id='Dtpcontactorexprirydate'>
                                <input id="Txtcontactorexprirydate" class="form-control" readonly="readonly" type="text" />
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-2" style="text-align: right;">
                            <span>ธนาคาร</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <input type="text" id="Txtbank" readonly="readonly" class="form-control" />
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>เลขที่บัญชี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <div class="input-group mb-3">
                                <input type="text" id="Txtcontactorbankaccountno" readonly="readonly" class="form-control" />
                            </div>
                        </div>

                    </div>
                    <div class="row" style="margin-bottom: 20px;">
                        <div class="col-2" style="text-align: right;">
                            <span>ชื่อบัญชี</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5">
                            <input type="text" id="Txtcontactorbankaccountname" readonly="readonly" class="form-control" />
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>ประเภท</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <input type="text" id="Txtbankaccounttype" readonly="readonly" class="form-control" />

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12" style="text-align: center;">
                            <div class="card card-body bg-light">รายละเอียดการว่าจ้าง</div>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px;">

                        <div class="col-2" style="text-align: right;">
                            <span>Site งาน</span> &nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5" style="text-align: left;">
                            <input type="text" id="Txtsitename" data-value="" readonly="readonly" class="form-control" style="font-size: 1.0em; border-radius: 1px;">
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>สัญญาเริ่มต้น</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <div class='input-group date' id='Dtpcontactstart'>
                                <input id="Txtcontactstart" class="form-control" type="text" readonly="readonly" />
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-2" style="text-align: right;">
                            <span>ที่อยู่ Site งาน</span> &nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5" style="text-align: left;">
                            <input type="text" id="Txtsiteaddress" readonly="readonly" data-value="" class="form-control" style="font-size: 1.0em; border-radius: 1px;">
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>สัญญาสิ้นสุด</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <div class='input-group date' id='Dtpcontactend'>
                                <input id="Txtcontactend" readonly="readonly" class="form-control" type="text" />
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-2" style="text-align: right;">
                            <span>ค่าปรับ</span> &nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-1" style="text-align: left;">
                            <input type="text" id="Txtfee" data-value="" readonly="readonly" onkeypress="return isNumberKey(event)" class="form-control" style="font-size: 1.0em; border-radius: 1px;">
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>กำหนดเสร็จ</span> &nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-2" style="text-align: left;">
                            <div class='input-group date' id='Dtpfinisheddate'>
                                <input id="Txtfinisheddate" class="form-control" type="text" readonly="readonly" />
                                <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                            </div>
                        </div>
                        <div class="col-2" style="text-align: right;">
                            <span>ราคาจ้างเหมารวมทั้งสิ้น</span>&nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-3" style="text-align: right;">
                            <input type="text" id="Txttotalamount" readonly="readonly" onkeypress="return isNumberKey(event)" class="form-control" />

                        </div>
                    </div>
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-2" style="text-align: right;">
                            <span>รายละเอียดงาน</span> &nbsp<span class="sp-require">*</span>
                        </div>
                        <div class="col-5" style="text-align: left;">
                            <input type="text" id="Txtjobdesc" readonly="readonly" data-value="" class="form-control" style="font-size: 1.0em; border-radius: 1px;">
                        </div>

                        <div class="col-5" style="text-align: right;">
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">งวดงาน</div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <div class="container-fluid" id="Divitem" style="background-color: #ffffff; border: solid 1px lightgray; min-height: 200px; padding: 20px; text-align: center;">
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-9" style="text-align: Right;">
                <span>จำนวนเงินที่เบิก</span>
            </div>
            <div class="col-3">
                <input type="text" id="Txtgrandtotal" style="text-align: right;" readonly="readonly" onkeypress="return isNumberKey(event)" class="form-control" />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
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


    <div class="modal" id="Divrequisition" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <span>เอกสารงวดงาน</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divrequisitioncont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
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
