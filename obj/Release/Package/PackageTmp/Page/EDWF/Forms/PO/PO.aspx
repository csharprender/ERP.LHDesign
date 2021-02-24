<%@ Page Title="" Language="C#" MasterPageFile="~/Page/EDWF/Forms/Control.Master" AutoEventWireup="true" CodeBehind="PO.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Forms.PO.PO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        var This = "PO.aspx";
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
            var i = 0;
            var j = 0;
            $.ajax({
                type: "POST",
                url: This + "/Getitem",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                   
                    if (response.d.length == 0) {
                        html = '<div style="color:red;padding:50px;text-align:center;" >ไม่พบรายการ</div>';

                    }
                    else {
                        html = '<div class="container-fluid" style="padding:20px;">';
                        for (i = 0; i < response.d.length; i++) {
                            html += '<div style="border: solid 0.5px lightblue; border-radius: 4px !important; padding: 50px; margin-left: 2px; margin-right: 2px; min-height: 200px; margin-top: 10px;">';
                            html += '<div class="row mt-3" style="margin-top:10px;">';
                            html += '<div class="col-8">';
                            html += '<span style="font-size:18px;font-weight:bold;font-family:kanit;">' + response.d[i]['Vendorname'] + '</span>';
                            html += '</div>';
                            html += '<div class="col-1" style="text-align:right;">';
                            html += '<span>วันที่สั่งของ</span>';
                            html += '</div>';
                            html += '<div class="col-3">';
                            html += '<div class="input-group date">';
                            html += '<input id="Dtporderdate_' + response.d[i]['Vendorid'] + '"  class="form-control" type="text" />';
                            html += '<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>';
                            html += '</div>';
                            html += '</div>';
                            html += '</div>';



                            html += '<div class="row mt-3" style="margin-top:10px;">';
                            html += '<div class="col-8">';
                            html += '<span style="font-size:14px;margin-left:30px;">' + response.d[i]['Vendorfulladdress'] + '</span>';
                            html += '</div>';
                            html += '<div class="col-1" style="text-align:right;">';
                            html += '<span>วันที่รับของ</span>';
                            html += '</div>';
                            html += '<div class="col-3">';
                            html += '<div class="input-group date">';
                            html += '<input id="Dtpreceivedate_' + response.d[i]['Vendorid'] + '"  class="form-control" type="text" />';
                            html += '<span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>';
                            html += '</div>';
                            html += '</div>';
                            html += '</div>';

                            //Detail
                            html += '<div class="container-fluid" style="border: solid 0.5px lightblue; border-radius: 4px; padding: 20px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;">';
                            html += '<div class="row">';
                            html += '<div class="col-4">';
                            html += '<div class="input-group mb-3">';
                            html += '<input type="text"  readonly="readonly"  class="form-control" placeholder="หมวดรายการ" style="font-size: 0.9em; border-radius: 1px;" value="' + response.d[i]['Invoiceitems'][0]["Servicenameth"] + '" />';
                            html += '</div>';
                            html += '</div>';
                            html += '<div class="col-8" style="text-align: right">';
                            html += '</div>';
                            html += '</div>';
                            html += '<div class="row">';
                            html += '<div class="col-12">';
                            html += '<table border="1" class="table table-info">';
                            html += '<thead style="text-align: center; font-weight: 500">';
                            html += '<tr>';
                            html += '<th rowspan="2" style="width: 5%;">ลำดับ</th>';
                            html += '<th rowspan="2" style="width: 28%;">รายการ</th>';
                            html += ' <th rowspan="2" style="width: 8%;">จำนวน</th>';
                            html += '<th rowspan="2" style="width: 8%;">หน่วย</th>';
                            html += '<th colspan="2" style="width: 15%;">ราคาของ</th>';
                            html += '<th colspan="2" style="width: 15%;">ราคาค่าแรง</th>';
                            html += '<th rowspan="2" style="width: 18%;">รวม</th>';
                          
                            html += '</tr>';
                            html += '<tr>';
                            html += '<th style="width: 10%;">ราคา/หน่วย</th>';
                            html += '<th style="width: 10%;">รวมบาท</th>';
                            html += '<th style="width: 10%;">ราคา/หน่วย</th>';
                            html += '<th style="width: 10%;">รวมบาท</th>';
                            html += '</tr>';
                            html += '<tr>';
                            html += '<th colspan="9" style="background-color: white; text-align: left;"></th>';
                            html += '</tr>';

                            html += '</thead>';
                            html += '<tbody style="background-color: white;">';


                            for (j = 0; j < response.d[i]['Invoiceitems'].length; j++)
                            {
                                html += '<tr>';
                                html += '<td style="text-align: center;">1</td>';
                                html += '<td>';
                                html += '<div class="input-group mb-3">';
                                html += '<input type="text"  class="form-control" readonly="readonly"   style="font-size: 0.9em; border-radius: 1px;" value="' + response.d[i]['Invoiceitems'][j]['Materialnameth'] + '">';
                                html += '</div>';
                                html += '</td>';
                                html += '<td>';
                                html += '<input type="number" readonly="readonly" class="form-control"  value="'  + response.d[i]['Invoiceitems'][j]['Quann'] +  '">';
                                html += '</td>';
                                html += '<td>';
                                html += '<input type="number" readonly="readonly" class="form-control"  value="' + response.d[i]['Invoiceitems'][j]['Unitname'] +  '"></td>';
                                html += '<td>';
                                html += '<input type="number" class="form-control"  readonly="readonly"  value="' + response.d[i]['Invoiceitems'][j]['Materialprice'] + '"></td>';
                                html += '<td>';
                                html += '<input type="number" class="form-control" readonly="readonly"  value="' + response.d[i]['Invoiceitems'][j]['Materialtotalprice'] + '"></td>';
                                html += '<td>';
                                html += '<input type="number" readonly="readonly" class="form-control"  value="' + response.d[i]['Invoiceitems'][j]['Manpowerprice'] + '"></td>';
                                html += '<td>';
                                html += '<input type="number" class="form-control" readonly="readonly" value="' + response.d[i]['Invoiceitems'][j]['Manpowertotalprice'] + '"></td>';
                                html += '<td>';
                                html += '<input type="number" class="form-control" readonly="readonly" value="' + response.d[i]['Invoiceitems'][j]['Totalprice'] + '"></td>';
                                
                                html += '</tr>';
                            }


                            html += '</tbody>';
                            html += '</table>';
                            html += '</div>';
                            html += '</div>';
                            html += '</div>';
                            html += '</div>';
                        }


                        
                        
                        html += '</div>';
                    }
                    $("#Divitem").html(html);

                    for (i = 0; i < response.d.length; i++) {
                        $("#Dtporderdate_" + response.d[i]['Vendorid']).datepicker({
                            format: 'dd-mm-yyyy',
                            autoclose: true,
                            todayHighlight: true
                        }).datepicker('update', new Date());
                        $("#Dtpreceivedate_" + response.d[i]['Vendorid']).datepicker({
                            format: 'dd-mm-yyyy',
                            autoclose: true,
                            todayHighlight: true
                        }).datepicker('update', new Date());
               
                        if (response.d[i]['Invoiceitems'][0]['Orderdate'] != "") {
                            $("#Dtporderdate_" + response.d[i]['Vendorid']).val(response.d[i]['Invoiceitems'][0]['Orderdate']);
                        }
                        else {
                            $("#Dtporderdate_" + response.d[i]['Vendorid']).val('');
                        }

                        if (response.d[i]['Invoiceitems'][0]['Receivedate'] != "") {
                            $("#Dtpreceivedate_" + response.d[i]['Vendorid']).val(response.d[i]['Invoiceitems'][0]['Receivedate']);
                        }
                        else {
                            $("#Dtpreceivedate_" + response.d[i]['Vendorid']).val('');
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
        }
        function Savestate() {
            var json = '';
            var id = '';
            var quann = '';
            var matprice = '';
            var manpowerprice = '';
            var name = '';
            $("[id*='Txtitem_']").each(function (i, el) {
                id = $(el).attr('id').replace('Txtitem_', '');
                desc = $(el).val();
                amount = $('#Txtitemamount_' + id).val();
                json += 'id:' + id + '|';
                json += 'desc:' + desc + '|';
                json += 'amount:' + amount + '|';
                json += '#';
            });
            
            $.ajax({
                type: "POST",
                url: This + "/Savestate",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                },
                async: false,
                error: function (er) {

                }
            });

        }
        function Selquotation(json) {
            $.ajax({
                type: "POST",
                url: This + "/Selquotation",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == null) {
                        Msgbox('System error , Please contract administrator');
                        return;
                    }

                    $('#Divquotation').modal('hide');
                    $('#Txtquotation').val(response.d['Documentno']);
                    $('#Txtquotation').attr('data-value', response.d['Referquotationid']);
                    $('#Txtcustomer').val(response.d['Customername']);
                    $('#Txtcustomeraddress').val(response.d['Customeraddress']);
                    $('#Txtofferdate').val(response.d['Offerdate']);
                    $('#Txtsalename').val(response.d['Salename']);
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
          
            if (ctrl == 'Gvquotation') {
                Selquotation(json);
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
                        Selquotation('x :' + response.d['Referquotationid'] + '|');
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
                    
                        if (response.d['Referquotationid'] != '') {
                            Selquotation('x :' + response.d['Referquotationid'] + '|');
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
            json += 'Txtquotation: ' + $('#Txtquotation').attr('data-value') + '|';
            json += 'ActionResultValue: ' + $('#CbAction').val() + '|';
            json += 'ActionResultNameTH: ' + $('#CbAction option:selected').text() + '|';
            $("#" + Div).find('textarea').each(function () {
                if ($(this).attr('disabled') != 'disabled') {
                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                }
            });
            $("[id*='Dtporderdate_']").each(function () {
                    
                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                
            });
            $("[id*='Dtpreceivedate_']").each(function () {
               
                json += $(this).attr('id') + ':' + $(this).val() + "|";

            });
            $.ajax({
                type: "POST",
                url: This + "/Save",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function (xhr) {
                    Savestate();
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
        function Searchquotation() {

            ClearResource('Gvquotation');
            var Columns = ["เลขที่ใบเสนอราคา!L","วันที่!L", "ลูกค้า!L"];
            var Data = ["Documentno", "Offerdate", "Customername"];
            var Searchcolumns = ["เลขที่ใบเสนอราคา", "ลูกค้า"];
            var SearchesDat = ["Documentno", "Customername"];
            var Width = ["30%", "10%","60%"];
            var grdquotation = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvquotation', 30, Width, Data, "", '', '1', '', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
            $('#Divquotationcont').html(grdquotation._Tables());
            grdquotation._Bind();

        }
        function Newquotation() {
            $('#Divquotation').modal('show');
            Searchquotation();
        }
    </script>
    <div class="container-fluid">
        <div class="row mt-3">
            <div class="col-8" style="text-align: right;">
                <span>ใบเสนอราคา</span>
            </div>
            <div class="col-4">
                <div class="input-group mb-3">
                   <input type="text" class="form-control" readonly="readonly" id="Txtquotation" data-value="" />
                   <button class="btn btn-info" onclick="Newquotation();"><i class="fa fa-search"></i></button>
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <div class="container-fluid" style="background-color: #ffffff; border: solid 1px lightgray; min-height: 200px;padding:20px;">
                    <div class="row mt-3">
                        <div class="col-1">
                            <span>ลูกค้า</span>
                        </div>
                        <div class="col-5">
                            <input class="form-control" id="Txtcustomer" readonly="readonly" />
                        </div>
                          <div class="col-3" style="text-align:right;">
                            <span>วันที่เสนอราคา</span>
                        </div>
                        <div class="col-3">
                            <input class="form-control" id="Txtofferdate" readonly="readonly" />
                        </div>
                    </div>
                     <div class="row mt-3">
                        <div class="col-1">
                            <span>ที่อยู่</span>
                        </div>
                        <div class="col-5">
                            <textarea class="form-control" id="Txtcustomeraddress" style="height:100px;" readonly="readonly" ></textarea>
                        </div>
                          <div class="col-3" style="text-align:right;">
                            <span>ผู้ขาย</span>
                        </div>
                        <div class="col-3">
                            <input class="form-control" id="Txtsalename" readonly="readonly" />
                        </div>
                    </div>

                    
                </div>
            </div>
        </div>
         <div class="row mt-3">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">รายละเอียด</div>
            </div>
        </div>
         <div class="row mt-3">
            <div class="col-12">
                <div class="container-fluid" id="Divitem" style="background-color: #ffffff; border: solid 1px lightgray; min-height: 200px;padding:20px;">
                    
                </div>
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
     <%--       <div class="col-12" style="text-align: left; margin-top: 20px;">
                <div style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;" id="Divattachment">
                    <div class="row mt-3">
                        <div class="col-2">
                            <div class="div-document" onclick="Newdocument('EFORM004');">
                                <div class="container" style="padding: 10px;">
                                    <div class="row mt-3">

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
                                    <div class="row mt-3">
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
                                    <div class="row mt-3">
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
            </div>--%>
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


     <div class="modal" id="Divquotation" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" id="Divprofilecontent">
            <div class="modal-content">
                <div class="modal-header">
                    <span>ใบเสนอราคา</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divquotationcont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
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
                               <span>เอกสาร</span>&nbsp;<span style="color:red">*</span>
                           </div>
                           <div class="col-10">
                               <input type="text" class="form-control" id="Txtattachmentlabel" />
                           </div>
                       </div>
                   </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" style="font-size: 1.0em; border-radius: 0;" onclick="Doupload();" >แนบเอกสาร</button>
                    <button type="button" class="btn btn-danger" style="font-size: 1.0em; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>