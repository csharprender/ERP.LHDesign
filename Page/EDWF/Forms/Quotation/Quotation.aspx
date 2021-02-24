<%@ Page Title="" Language="C#" MasterPageFile="~/Page/EDWF/Forms/Control.Master" AutoEventWireup="true" CodeBehind="Quotation.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Forms.Quotation.Quotation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../../../js/Numeric.js"></script>
    <script>
        var This = "Quotation.aspx";
        function RowSelect(ctrl, x) {
            var json = '';
            json = 'x :' + x + '|';
            if (ctrl == 'Gvcontactor') {
                $.ajax({
                    type: "POST",
                    url: This + "/Selcont",
                    data: "{'json' :'" + json + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response == null) {
                            Msgbox('System error , Please contract administrator');
                            return;
                        }
                        $('#Txtcontactorname').attr('data-value', response.d[0]['Id']);
                        $('#Txtcontactorname').val(response.d[0]['Fullname']);
                        $('#Txtcontactoraddress').val(response.d[0]['Address']);
                        $('#Divcontactor').modal('hide');
                    },
                    async: true,
                    error: function (er) {

                    }
                });
            }
        }
        function Getbank() {
            $.ajax({
                type: "POST",
                url: This + "/Getbank",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    res = response.d;
                    $('#Cbnewcontactorbank').find('option').remove().end();
                    for (i = 0; i < response.d.length; i++) {
                        val = response.d[i];
                        $('#Cbnewcontactorbank').append($('<option/>', {
                            value: val["Val"],
                            text: val["Name"]
                        }));
                    }
                    $('#Cbcontactorbank').find('option').remove().end();
                    for (i = 0; i < response.d.length; i++) {
                        val = response.d[i];
                        $('#Cbcontactorbank').append($('<option/>', {
                            value: val["Val"],
                            text: val["Name"]
                        }));
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
        function Custom(ctrl, Panel) {

            if (ctrl == 'Gvcontactor') {
                Getbank();
                $('#Divcontactor').modal('hide');
                $('#Divnewcontactor').modal('show');
            }
        }
        function doSavecontactor(json) {
            $.ajax({
                type: "POST",
                url: This + "/Savecontactor",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d != '') {
                        Msgbox(response.d);
                        $('#Divnewcontactor').modal('show');
                        return;
                    }
                    $('#Divcontactor').modal('show');
                    $('#Divnewcontactor').modal('hide');
                    Searchcontactor();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Savecontactor() {
            var json = '';
            if ($('#Txtnewcontactorname').val() == '') {
                Msgbox('โปรดระบุชื่อลูกค้า');
                return;
            }
            if ($('#Txtnewcontactortel').val() == '') {
                Msgbox('โปรดระบุเบอร์โทรศัพท์ลูกค้า');
                return;
            }
            if ($('#Txtnewcontactoraddress').val() == '') {
                Msgbox('โปรดระบุที่อยู่ลูกค้า');
                return;
            }
            if ($('#TxtnewcontactorcardID').val() == '') {
                Msgbox('โปรดระบุบัตรประชาชนลูกค้า');
                return;
            }

            json += 'Txtnewcontactorname :' + $('#Txtnewcontactorname').val() + '|';
            json += 'Txtnewcontactoraddress :' + $('#Txtnewcontactoraddress').val() + '|';
            json += 'Txtnewcontactortel :' + $('#Txtnewcontactortel').val() + '|';
            json += 'TxtnewcontactorcardID :' + $('#TxtnewcontactorcardID').val() + '|';
            json += 'Txtnewcontactorexprirydate :' + $('#Txtnewcontactorexprirydate').val() + '|';
            json += 'Cbnewcontactorbank :' + $('#Cbnewcontactorbank').val() + '|';
            json += 'Txtnewcontactorbankaccountname :' + $('#Txtnewcontactorbankaccountname').val() + '|';
            json += 'Txtnewcontactorbankaccountno :' + $('#Txtnewcontactorbankaccountno').val() + '|';
            json += 'Cbnewcontactorbankaccounttype :' + $('#Cbnewcontactorbankaccounttype').val() + '|';

            $.ajax({
                type: "POST",
                url: This + "/Validatecontactor",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d.includes("!E")) {
                        Msgbox(response.d.replace('!E', ''));
                        $('#Divnewcontactor').modal('show');
                        return;
                    }
                    else if (response.d.includes("!W")) {
                        $('#DivConfirmMsg').html(response.d.replace('!W', ''))
                        $("#DivConfirm").modal({
                            backdrop: 'static',
                            keyboard: false,
                            show: true
                        });
                        $('#CmdConfirm').on('click', function () {
                            $("#DivConfirm").modal('hide');
                            doSavecontactor(json);
                        });

                    }
                    else {
                        doSavecontactor(json);
                    }


                },
                async: true,
                error: function (er) {

                }
            });


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



        function Newdocument() {
            $('#Txtattachmentlabel').val('');
            $('#Divupload').modal('show');
        }

        //function Newdocument() {
        //    w = 600;
        //    h = 400;
        //    var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
        //    var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
        //    var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        //    var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
        //    var left = ((width / 2) - (w / 2)) + dualScreenLeft;
        //    var top = ((height / 2) - (h / 2)) + dualScreenTop;
        //    window.open('../Upload.aspx', '_blank', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
        //}
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
                            if (i == (out.length - 1)) {
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
        //function Getattachment() {
        //    var i = 0;
        //    var html = '';
        //    $.ajax({
        //        type: "POST",
        //        url: This + "/Getattachment",
        //        data: "{}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            html = '';
        //            html += ' <div class="row">';
        //            html += ' <div class="col-2">';
        //            html += ' <div class="div-document" onclick="Newdocument(\'EFORM004\');">';
        //            html += ' <div class="container" style="padding: 10px;">';
        //            html += ' <div class="row">';
        //            html += ' <div class="col-12" style="text-align: center;">';
        //            html += ' <img src="../../Img/EDOC/newdoc.png" style="width: 60px;" />';
        //            html += ' </div>';
        //            html += ' <div class="col-12" style="text-align: center;">';
        //            html += ' <div>แนบไฟล์เพิ่ม</div>';
        //            html += ' </div>';
        //            html += ' <div class="col-12" style="text-align: center; margin-top: 30px;">';
        //            html += ' <div style="font-size: 0.8em; color: red;">ขนาดไฟล์ไม่เกิน 25 MB</div>';
        //            html += ' </div>';
        //            html += ' </div>';
        //            html += ' </div>';
        //            html += ' </div>';
        //            html += ' </div>';
        //            for (i = 0; i < response.d.length; i++) {
        //                html += ' <div class="col-2">';
        //                html += ' <div class="div-document">';
        //                html += ' <div class="container" style="padding: 10px;">';
        //                html += ' <div class="row">';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <img src="../../Img/EDOC/1.png" style="width: 60px;" />';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <div>' + response.d[i]['label'] + '</div>';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center; margin-top: 30px;">';
        //                html += ' <div style="font-size: 0.8em;">แนบเมื่อ ' + response.d[i]['Uploaddate'] + '</div>';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <div style="font-size: 0.8em;">โดย ' + response.d[i]['Uploadbynameth'] + '</div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //            }
        //            html += '</div>';
        //            $('#Divattachment').html(html);
        //        },
        //        async: false,
        //        error: function (er) {
        //            try {
        //                var x = $.parseJSON(er.responseText);
        //                show_msg(x.Message);
        //            }
        //            catch (ex) {
        //                console.log(ex.responseText);
        //            }
        //        }
        //    });
        //}
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
        function Calc() {
            var id = '';
            var quann = '';
            var matprice = '';
            var mattotalprice = '';
            var manpowertotalprice = '';
            var manpowerprice = '';
            var totalprice = '';
            var beforevat = 0;
            var vat = 0;
            var grandtotal = 0;
            $("input[id*='Txtitemname_']").each(function (i, el) {
                id = $(el).attr('id').replace('Txtitemname_', '');

                quann = $('#Txtitemquann_' + id).val();
                matprice = $('#Txtitemmatprice_' + id).val();
                manpowerprice = $('#Txtitemmanpowerprice_' + id).val();

                if (quann == '') {
                    quann = 0;
                }
                if (matprice == '') {
                    matprice = 0;
                }
                if (manpowerprice == '') {
                    manpowerprice = 0;
                }
                mattotalprice = Number(matprice) * Number(quann);
                manpowertotalprice = Number(manpowerprice) * Number(quann);
                totalprice = mattotalprice + manpowertotalprice;

                $('#Txtitemmattotal_' + id).val(mattotalprice);

                $('#Txtitemmanpowertotal_' + id).val(manpowertotalprice);

                $('#Txtitemtotalprice_' + id).val(totalprice);

                beforevat += totalprice;

            });
            vat = beforevat * 0.07;
            grandtotal = beforevat + vat;
            $('#Txtbeforetotal').val(beforevat.toFixed(2));
            $('#Txtvat').val(vat.toFixed(2));
            $('#Txtgrandtotal').val(grandtotal.toFixed(2));
        }
        function Savestate() {
            var json = '';
            var id = '';
            var quann = '';
            var matprice = '';
            var manpowerprice = '';
            var name = '';
            $("input[id*='Txtitemname_']").each(function (i, el) {
                id = $(el).attr('id').replace('Txtitemname_', '');
                name = $(el).val();
                quann = $('#Txtitemquann_' + id).val();
                matprice = $('#Txtitemmatprice_' + id).val();
                manpowerprice = $('#Txtitemmanpowerprice_' + id).val();
                if (quann == '') {
                    quann = 0;
                }
                if (matprice == '') {
                    matprice = 0;
                }
                if (manpowerprice == '') {
                    manpowerprice = 0;
                }
                mattotalprice = Number(matprice) * Number(quann);
                manpowertotalprice = Number(manpowerprice) * Number(quann);
                totalprice = mattotalprice + manpowertotalprice;
                json += 'id:' + id + '|';
                json += 'name:' + name + '|';
                json += 'quann:' + quann + '|';
                json += 'matprice:' + matprice + '|';
                json += 'manpowerprice:' + manpowerprice + '|';
                json += 'mattotalprice:' + mattotalprice + '|';
                json += 'manpowertotalprice:' + manpowertotalprice + '|';
                json += 'totalprice:' + totalprice + '|';
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
        function getchildservice(res, serviceid) {
            var html = '';
            var j = 0;
            var isitem = false;
            for (j = 0; j < res.length; j++) {

                if (res[j]['Isgroup'] == "True" && res[j]['Services'].length > 0) {

                    html += '<div class="card">';
                    html += '<div class="card-header">';
                    html += '<a class="card-link" data-toggle="collapse" href="#S_' + res[j]['serviceid'] + '">';
                    html += res[j]['servicenameth'];
                    html += '</a>';
                    html += '</div>';
                    html += '<div id="S_' + res[j]['serviceid'] + '" class="collapse show" data-parent="#accordion">';
                    html += '<div class="card-body">';
                    html += getchildservice(res[j]['Services'], res[j]['serviceid']);
                    html += '</div > ';
                    html += '</div>';
                    html += '</div>';

                }
                else if (res[j]['Isgroup'] == "False") {
                    isitem = true;
                    html += "<span style='color:gray;font-size:12px;'>";
                    html += res[j]['servicenameth'] + " , "
                    //html += '<button class="btn btn-secondary"  style="margin:5px;color:white;height:60px;border-radius:1px;">' + res[j]['servicenameth'] + '</button>';
                }

            }
            if (isitem) {
                html = html.trim();
                html = html.substring(0, html.length - 2);
                html += "</span>";
                html += '<div class="container">';
                html += '<div class="row">';
                html += '<div class="col-12" style="margin-top:20px; text-align:center;"><a style="color:blue;" onclick="Selservice(' + serviceid + ');">เลือกหมวดนี้</a></div>';
                //html += '<button class="btn btn-secondary"  onclick="Selservice(' + serviceid + ');"  style="width:100%;margin-top:15px; margin-bottom:2px;color:white;height:50px;border-radius:1px !important;">เลือกหมวดนี้</button>';
                html += '</div>';
                html += '</div>';
            }
            return html;
        }
        function Searchservice() {

            var html = '';
            var i = 0;
            var json = 'kwd:' + $('#Txtsearchservice').val() + '|';
            $('#Divservice').modal('show');

            $.ajax({
                type: "POST",
                url: This + "/Getservice",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    for (i = 0; i < response.d.length; i++) {

                        html += '<div class="card" style="margin-top:5px;padding:10px;">';
                        html += '<div class="card-header">';
                        html += '<a class="card-link" data-toggle="collapse" href="#S_' + response.d[i]['serviceid'] + '">';
                        html += response.d[i]['servicenameth'];
                        html += '</a>';
                        html += '</div>';
                        html += '<div id="S_' + response.d[i]['serviceid'] + '" class="collapse show" data-parent="#accordion">';
                        html += '<div class="card-body">';
                        html += getchildservice(response.d[i]['Services'], response.d[i]['serviceid']);
                        html += '</div > ';
                        html += '</div>';
                        html += '</div>';

                    }
                    $('#Divservicecont').html(html);
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Getchilditem(parentserviceid, res) {
            var html = '';
            var j = 0;
            for (j = 0; j < res.length; j++) {
                if (res[j]['Isgroup'] == "False") {
                    html += '<div class="card card-body bg-light"  onclick = "Selitem(' + parentserviceid + ',' + res[j]['serviceid'] + ');" style="cursor:pointer; margin:5px;color:black;height:60px;border-radius:1px;">' + res[j]['servicenameth'] + '</div>';
                }
                //if (res[j]['Isgroup'] == "True" && res[j]['Services'].length > 0) {

                //    html += '<div class="card">';
                //    html += '<div class="card-header">';
                //    html += '<a class="card-link" data-toggle="collapse" href="#S_' + res[j]['serviceid'] + '">';
                //    html += res[j]['servicenameth'];
                //    html += '</a>';
                //    html += '</div>';
                //    html += '<div id="S_' + res[j]['serviceid'] + '" class="collapse show" data-parent="#accordion">';
                //    html += '<div class="card-body">';
                //    html += Getchilditem(res[j]['serviceid'], res[j]['Services']);
                //    html += '</div > ';
                //    html += '</div>';
                //    html += '</div>';
                //}
                //else if (res[j]['Isgroup'] == "False") {
                //    html += '<button class="btn btn-info"  onclick = "Selitem(' + parentserviceid + ',' + res[j]['serviceid'] + ');" style="margin:5px;color:white;height:60px;border-radius:1px;">' + res[j]['servicenameth'] + '</button>';
                //}

            }
            return html;
        }
        function Additem(Serviceid) {
            var html = '';
            var i = 0;
            var json = 'Serviceid:' + Serviceid + '|';
            Savestate();
            $('#Divservice').modal('show');
            $.ajax({
                type: "POST",
                url: This + "/Additem",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    for (i = 0; i < response.d.length; i++) {

                        html += '<div class="card" style="margin-top:5px;padding:10px;">';
                        html += '<div class="card-header">';
                        html += '<a class="card-link" data-toggle="collapse" href="#S_' + response.d[i]['serviceid'] + '">';
                        html += response.d[i]['servicenameth'];
                        html += '</a>';
                        html += '</div>';
                        html += '<div id="S_' + response.d[i]['serviceid'] + '" class="collapse show" data-parent="#accordion">';
                        html += '<div class="card-body">';
                        html += Getchilditem(response.d[i]['serviceid'], response.d[i]['Services']);
                        html += '</div > ';
                        html += '</div>';
                        html += '</div>';

                    }
                    $('#Divservicecont').html(html);
                    Calc();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Delitem(serviceid, itemid) {
            var html = '';
            var i = 0;
            var json = 'Serviceid:' + serviceid + '|';
            json += 'itemid:' + itemid + '|';
            Savestate();
            $.ajax({
                type: "POST",
                url: This + "/Delitem",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d["Errormessage"] != "") {
                        Msgbox(response.d["Errormessage"]);
                        return;
                    }
                    Getdesc();
                    Calc();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Getdesc() {
            var html = '';
            var i = 0;
            var j = 0;
            var serviceid = '';
            $.ajax({
                type: "POST",
                url: This + "/Getdesc",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '';
                    if (response.d['OfferDetails'] == null || response.d['OfferDetails'].length == 0) {
                        html += '<div class="col-12" style="text-align: center;height:100px;padding-top:30px; margin-top: 20px; border:solid 1px lightgray;">';
                        html += '<span style="color:red;">Not found</span>'
                        html += '</div>';
                    }
                    else
                    {
                        for (i = 0; i < response.d['OfferDetails'].length; i++) {
                            serviceid = response.d['OfferDetails'][i]['Serviceid'];

                            html += '<div class="col-12" style="text-align: left; margin-top: 20px;">';
                            html += '<div style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;">';
                            html += '<div class="row">';
                            html += '<div class="col-4">';
                            html += '<div class="input-group mb-3">';

                            if (serviceid == '0') {
                                html += '<input type="text" id="Txtservice_' + serviceid + '" data-value="' + serviceid + '"  class="form-control" placeholder="หมวดรายการ" style="font-size: 0.9em; border-radius: 1px;">';
                                html += '<div class="input-group-append">';
                                html += '<button class="btn btn-outline-info" id="Cmdsearchservice_' + serviceid + '" style="font-size: 0.9em; border-radius: 1px;" type="button" onclick="Searchservice(' + serviceid + ')"><i class="fa fa-search" aria-hidden="true"></i></button>';
                                html += '</div>';
                            }
                            else {
                                html += '<input type="text" id="Txtservice_' + serviceid + '" readonly=readonly data-value="' + serviceid + '"  class="form-control" placeholder="หมวดรายการ" style="font-size: 0.9em; border-radius: 1px;" value=' + response.d['OfferDetails'][i]['Servicenameth'] + ' />';
                            }
                            html += '</div>';
                            html += '</div>';
                            html += '<div class="col-8" style="text-align: right">';
                            html += '<button class="btn btn-outline-danger" onclick="Delservice(' + serviceid + ')"><i class="fa fa-trash" aria-hidden="true"></i>&nbsp;ลบหมวดรายการ</button>';
                            html += '</div>';
                            html += '</div>';
                            html += '<div class="row">';
                            html += '<div class="col-12">';

                            html += '<table border="1" class="table table-info">';
                            html += '<thead style="text-align: center; font-weight: 500">';
                            html += '<tr>';
                            html += '<th rowspan="2" style="width: 5%;">ลำดับ</th>';
                            html += '<th rowspan="2" style="width: 28%;">รายการ</th>';
                            html += '<th rowspan="2" style="width: 8%;">จำนวน</th>';
                            html += '<th rowspan="2" style="width: 10%;">หน่วย</th>';
                            html += '<th colspan="2" style="width: 15%;">ราคาของ</th>';
                            html += '<th colspan="2" style="width: 15%;">ราคาค่าแรง</th>';
                            html += '<th rowspan="2" style="width: 15%;">รวม</th>';
                            html += '<th rowspan="2" style="width: 5%;"></th>';
                            html += '</tr>';
                            html += '<tr>';
                            html += '<th style="width: 10%;">ราคา/หน่วย</th>';
                            html += '<th style="width: 10%;">รวมบาท</th>';
                            html += '<th style="width: 10%;">ราคา/หน่วย</th>';
                            html += '<th style="width: 10%;">รวมบาท</th>';
                            html += '</tr>';
                            html += '<tr>';
                            html += '<th colspan="10" style="background-color: white; text-align: left;">';
                            html += '<button class="btn btn-info" onclick="Additem(' + serviceid + ');"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp;เพิ่มรายการ</button>';
                            html += '</th>';
                            html += '</tr>';
                            html += '</thead>';
                            html += '<tbody style="background-color: white;">';

                            if (response.d['OfferDetails'][i]['Details'] == null || response.d['OfferDetails'][i]['Details'].length == 0) {
                                html += '<tr>';
                                html += '<td colspan=10 style="text-align:center;"><div class="col-12" style="text-align: center;height:100px;padding-top:30px; margin-top: 20px; border:solid 1px lightgray;"><span style="color:red;">Not found</span></div></td>';
                                html += '</tr>';
                            }
                            else {
                                for (j = 0; j < response.d['OfferDetails'][i]['Details'].length; j++) {
                                    html += '<tr>';
                                    html += '<td style="text-align: center;">' + (j + 1) + '</td>';
                                    html += '<td>';
                                    html += '<div class="input-group mb-3">';
                                    html += '<input type="text" id="Txtitemname_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" class="form-control" readonly=readonly data-value=' + response.d['OfferDetails']['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + ' style="font-size: 0.9em; border-radius: 1px;" value=' + response.d['OfferDetails'][i]['Details'][j]['itemname'] + '>';
                                    html += '</div>';
                                    html += '</td>';
                                    html += '<td>';
                                    html += '<input type="number"  onkeypress="OnlyNumeric(event,this)"  class="form-control" onkeyup="Calc();" id="Txtitemquann_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value= "' + response.d['OfferDetails'][i]['Details'][j]['Quann'] + '"/> ';
                                    html += '</td>';
                                    html += '<td>';
                                    html += '<select class="form-control" id="Cbitemunit_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" >';
                                    html += '<option>หลุม</option>';
                                    html += '<option>ต้น</option>';
                                    html += '<option>L/S</option>';
                                    html += '</select></td>';
                                    html += '<td>';
                                    html += '<input type="number" onkeypress="OnlyNumeric(event,this)"  class="form-control" onkeyup="Calc();" id="Txtitemmatprice_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value = "' + response.d['OfferDetails'][i]['Details'][j]['Materialprice'] + '" /></td>';
                                    html += '<td>';
                                    html += '<input type="number"  onkeypress="OnlyNumeric(event,this)"  class="form-control" readonly=readonly id="Txtitemmattotal_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value = "' + response.d['OfferDetails'][i]['Details'][j]['Materialtotalprice'] + '" /></td>';
                                    html += '<td>';
                                    html += '<input type="number"  onkeypress="OnlyNumeric(event,this)"  class="form-control" onkeyup="Calc();"  id="Txtitemmanpowerprice_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value = "' + response.d['OfferDetails'][i]['Details'][j]['Manpowerprice'] + '" /></td>';
                                    html += '<td>';
                                    html += '<input type="number" onkeypress="OnlyNumeric(event,this)"  class="form-control" readonly=readonly  id="Txtitemmanpowertotal_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value = "' + response.d['OfferDetails'][i]['Details'][j]['Manpowertotalprice'] + '" ></td>';
                                    html += '<td>';
                                    html += '<input type="number"  onkeypress="OnlyNumeric(event,this)"  class="form-control"  readonly=readonly id="Txtitemtotalprice_' + response.d['OfferDetails'][i]['Serviceid'] + '_' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + '" value = "' + response.d['OfferDetails'][i]['Details'][j]['Totalprice'] + '" ></td>';
                                    html += '<td>';
                                    html += '<button class="btn btn-danger" onclick="Delitem(' + response.d['OfferDetails'][i]['Serviceid'] + ',' + response.d['OfferDetails'][i]['Details'][j]['itemid'] + ');" ><i class="fa fa-trash" aria-hidden="true"></i></button>';
                                    html += '</td>';
                                    html += '</tr>';
                                }
                            }
                            html += '</tbody>';
                            html += '</table>';
                            html += '</div>';
                            html += '</div>';
                            html += '</div>';
                        }
                    }
                    $("#Divdesc").html(html);
                },
                async: false,
                error: function (er) {

                }
            });






            //item_html += '<table border="1" class="table table-info">';
            //item_html += '<thead style="text-align: center; font-weight: 500">';
            //item_html += '<tr>';
            //item_html += '<th rowspan="2" style="width: 5%;">ลำดับ</th>';
            //item_html += '<th rowspan="2" style="width: 28%;">รายการ</th>';
            //item_html += '<th rowspan="2" style="width: 8%;">จำนวน</th>';
            //item_html += '<th rowspan="2" style="width: 10%;">หน่วย</th>';
            //item_html += '<th colspan="2" style="width: 15%;">ราคาของ</th>';
            //item_html += '<th colspan="2" style="width: 15%;">ราคาค่าแรง</th>';
            //item_html += '<th rowspan="2" style="width: 15%;">รวม</th>';
            //item_html += '<th rowspan="2" style="width: 5%;"></th>';
            //item_html += '</tr>';
            //item_html += '<tr>';
            //item_html += '<th style="width: 10%;">ราคา/หน่วย</th>';
            //item_html += '<th style="width: 10%;">รวมบาท</th>';
            //item_html += '<th style="width: 10%;">ราคา/หน่วย</th>';
            //item_html += '<th style="width: 10%;">รวมบาท</th>';
            //item_html += '</tr>';
            //item_html += '<tr>';
            //item_html += '<th colspan="10" style="background-color: white; text-align: left;">';
            //item_html += '<button class="btn btn-info"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp;เพิ่มรายการ</button>
            //item_html += '</th>';
            //item_html += '</tr>';
            //item_html += '</thead>';
            //item_html += '<tbody style="background-color: white;">';
            //item_html += '<tr>';
            //item_html += '<td style="text-align: center;">1</td>';
            //item_html += '<td>';
            //item_html += '<div class="input-group mb-3">';
            //item_html += '<input type="text" class="form-control" placeholder="รายการ" style="font-size: 0.9em; border-radius: 1px;">';
            //item_html += '<div class="input-group-append">';
            //item_html += '<button class="btn btn-outline-info" style="font-size: 0.9em; border-radius: 1px;" type="button"><i class="fa fa-search" aria-hidden="true"></i></button>';
            //item_html += '</div>';
            //item_html += '</div>';
            //item_html += '</td>';
            //item_html += '<td>';
            //item_html += '<input type="number" class="form-control" /></td>';
            //item_html += '<td>';
            //item_html += '<select class="form-control">';
            //item_html += '<option>หลุม</option>';
            //item_html += '<option>ต้น</option>';
            //item_html += '<option>L/S</option>';
            //item_html += '</select></td>';
            //item_html += '<td>';
            //item_html += '<input type="text" class="form-control" readonly="readonly" /></td>';
            //item_html += '<td>';
            //item_html += '<input type="text" class="form-control" readonly="readonly" /></td>';
            //item_html += '<td>';
            //item_html += '<input type="text" class="form-control" readonly="readonly" /></td>';
            //item_html += '<td>';
            //item_html += '<input type="text" class="form-control" readonly="readonly" /></td>';
            //item_html += '<td>';
            //item_html += '<input type="text" class="form-control" readonly="readonly" /></td>
            //item_html += '<td>';
            //item_html += '<button class="btn btn-danger"><i class="fa fa-trash" aria-hidden="true"></i></button>';
            //item_html += '</td>';
            //item_html += '</tr>';
            //item_html += '</tbody>';
            //item_html += '</table>';
        }
        //function RowSelect(ctrl, x) {
        //    var json = '';
        //    json = 'x :' + x + '|';
        //    if (ctrl == 'Gvcustomer') {
        //        $.ajax({
        //            type: "POST",
        //            url: This + "/Selcust",
        //            data: "{'json' :'" + json + "'}",
        //            contentType: "application/json; charset=utf-8",
        //            dataType: "json",
        //            success: function (response) {
        //                if (response == null) {
        //                    Msgbox('System error , Please contract administrator');
        //                    return;
        //                }
        //                $('#Txtcontactorname').attr('data-value', response.d[0]['Id']);
        //                $('#Txtcontactorname').val(response.d[0]['Fullname']);
        //                $('#Txtcontactoraddress').val(response.d[0]['Address']);
        //                $('#Txtcontactoraddress').attr('data-value', response.d[0]['Tel']);
        //                $('#Divcustomer').modal('hide');
        //            },
        //            async: true,
        //            error: function (er) {

        //            }
        //        });
        //    }
        //}
        //function Custom(ctrl, Panel) {

        //    if (ctrl == 'Gvcustomer') {
        //        $('#Divcustomer').modal('hide');
        //        $('#Divnewcustomer').modal('show');
        //    }
        //}
        //function doSavecustomer(json) {

        //    $.ajax({
        //        type: "POST",
        //        url: This + "/Savecustomer",
        //        data: "{'json' :'" + json + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            if (response.d != '') {
        //                Msgbox(response.d);
        //                $('#Divnewcustomer').modal('show');
        //                return;
        //            }
        //            $('#Divcustomer').modal('show');
        //            $('#Divnewcustomer').modal('hide');
        //            Searchcustomer();
        //        },
        //        async: true,
        //        error: function (er) {

        //        }
        //    });
        //}
        //function Savecustomer() {
        //    var json = '';
        //    if ($('#Txtnewcustomername').val() == '') {
        //        Msgbox('Please fill customer name');
        //        return;
        //    }
        //    if ($('#Txtnewcustomeraddress').val() == '') {
        //        Msgbox('Please fill customer address');
        //        return;
        //    }
        //    if ($('#Txtnewcustomertel').val() == '') {
        //        Msgbox('Please fill customer tel');
        //        return;
        //    }
        //    json += 'Txtnewcustomername :' + $('#Txtnewcustomername').val() + '|';
        //    json += 'Txtnewcustomeraddress :' + $('#Txtnewcustomeraddress').val() + '|';
        //    json += 'Txtnewcustomertel :' + $('#Txtnewcustomertel').val() + '|';
        //    $.ajax({
        //        type: "POST",
        //        url: This + "/Validatecustomer",
        //        data: "{'json' :'" + json + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            if (response.d.includes("!E")) {
        //                Msgbox(response.d.replace('!E', ''));
        //                $('#Divnewcustomer').modal('show');
        //                return;
        //            }
        //            else if (response.d.includes("!W")) {
        //                $('#DivConfirmMsg').html(response.d.replace('!W', ''))
        //                $("#DivConfirm").modal({
        //                    backdrop: 'static',
        //                    keyboard: false,
        //                    show: true
        //                });
        //                $('#CmdConfirm').on('click', function () {
        //                    $("#DivConfirm").modal('hide');
        //                    doSavecustomer(json);
        //                });

        //            }
        //            else {
        //                doSavecustomer(json);
        //            }


        //        },
        //        async: true,
        //        error: function (er) {

        //        }
        //    });


        //}
        //function Searchcustomer() {

        //    ClearResource('Gvcustomer');
        //    var Columns = ["Customer name!C", "Address!L"];
        //    var Data = ["fullname", "Address"];
        //    var Searchcolumns = ["Customer name", "Address"];
        //    var SearchesDat = ["fullname", "Address"];
        //    var Width = ["40%", "60%"];
        //    var grdcustomer = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvcustomer', 30, Width, Data, "", '', '2', 'New Customer', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
        //    $('#Divcustomercont').html(grdcustomer._Tables());
        //    grdcustomer._Bind();

        //}

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
                success: function (response) {
                    if (response.d['Err'] != '') {
                        Msgbox(response.d['Err']);
                        return;
                    }



                    $('#SpDocumentCreateBy').html(out["CreateBy"]);
                    $('#SpISODocument').html(response.d["EFormcode"]);
                    $('#SpDocumentNo').html(response.d["Documentno"]);
                    $('#SpDocumentCreateDate').html(response.d["DocumentDate"]);
                    //$('#TxtSubject').val(response.d["Subject"]);
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
                        $('#Txtcontactorname').val(response.d['Customername']);
                        $('#Txtcontactorname').attr('data-value', response.d['Customerid']);
                        $('#Txtcontactoraddress').val(response.d['Customeraddress']);
                        Getdesc();
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
                        
                        $('#Txtcontactorname').val(response.d['Customername']);
                        $('#Txtcontactorname').attr('data-value', response.d['Customerid']);
                        $('#Txtcontactoraddress').val(response.d['Customeraddress']);
                        Getdesc();
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
            json += 'Customerid :' + $('#Txtcontactorname').attr('data-value') + '|';
            json += 'Customername :' + $('#Txtcontactorname').val() + '|';
            json += 'Customeraddress :' + $('#Txtcontactoraddress').val() + '|';
            json += 'Customertel :' + $('#Txtcontactoraddress').attr('data-value') + '|';
            json += 'Offerdate :' + $('#Txtofferdate').val() + '|';
            json += 'Saleid :' + $('#Cbsale').val() + '|';
            json += 'Saletel :' + $("#Cbsale option:selected").attr('data-value') + '|';
            json += 'Salename :' + $("#Cbsale option:selected").text() + '|';
            json += 'Totalprice :' + $('#Txtbeforetotal').val() + '|';
            json += 'vatamount :' + $('#Txtvat').val() + '|';
            json += 'Grandtotalprice :' + $('#Txtgrandtotal').val() + '|';
            json += 'ActionResultValue: ' + $('#CbAction').val() + '|';
            json += 'ActionResultNameTH: ' + $('#CbAction option:selected').text() + '|';
            $("#" + Div).find('textarea').each(function () {
                if ($(this).attr('disabled') != 'disabled') {

                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                }
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
        function Searchcontactor() {

            ClearResource('Gvcontactor');
            var Columns = ["ชื่อลูกค้า!L", "ที่อยู่!L"];
            var Data = ["fullname", "Address"];
            var Searchcolumns = ["ชื่อลูกค้า", "ที่อยู่"];
            var SearchesDat = ["fullname", "Address"];
            var Width = ["40%", "60%"];
            var grdcontactor = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvcontactor', 30, Width, Data, "", '', '2', 'สร้างลูกค้าใหม่', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
            $('#Divcontactorcont').html(grdcontactor._Tables());
            grdcontactor._Bind();

        }
        $(function () {
            var i = 0;
            var json = '';
            var html = '';
            $("#Txtnewcontactorexprirydate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());
            $('#CmdSearch').on('click', function () {
                $('#Divcontactor').modal('show');
                Searchcontactor();
            })
            $('#CmdAction').on('click', function () {
                if ($('#CbAction').val() == "") {
                    Msgbox('โปรดระบุเส้นทางการส่งเอกสาร');
                    return;
                }
                Save();
            });

            $("#Dtpofferdate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            GetRoute();
            GetInfo();
            //$.ajax({
            //    type: "POST",
            //    url: This + "/GetInfo",
            //    data: "{'json' :'" + json + "'}",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (response) {
            //        res = response.d;
            //        if (res == "") {
            //            Msgbox('ไม่พบระบบที่จะเข้าหรือคุณไม่มีสิทธิ์ใช้งาน');
            //            return;
            //        }

            //        $('#Hdflowconfigid').val(response.d['Flowconfigid']);
            //        $('#Hdcurrentnode').val(response.d['Nodenamefrom']);
            //        $('#Imgcompanylogo').attr('src', response.d['Companylogourl']);
            //        $('#Diveformname').html(response.d['Eformname']);
            //        $('#Spcompanyname').html(response.d['Companyname']);
            //        $('#Spcompanyaddress').html(response.d['Companyaddress']);
            //        $('#Spcompanytel').html('Tel. ' + response.d['Companytel']);
            //        $('#Divdocumentcode').html(response.d['EFormcode']);
            //        $('#Divdocumentdate').html(response.d['Documentdate']);
            //        $('#Divdocumentversion').html(response.d['Version']);
            //        $('#Spdocumentstatus').html(response.d['Statustext']);
            //        $('#Spdocumentno').html(response.d['Documentno']);
            //        $('#Txtcontactorname').val(response.d['Customername']);
            //        $('#Txtcontactorname').attr('data-value', response.d['Customerid']);
            //        $('#Txtcontactoraddress').val(response.d['Customeraddress']);

            //        //if (response.d['Status'] != 'S') {
            //        //    for (i = 0; i < response.d['Routes'].length; i++) {
            //        //        html += '<option value=' + response.d['Routes'][i]['value'] + '>' + response.d['Routes'][i]['text'] + '</option>';

            //        //    }
            //        //    $('#Cbroute').html(html);
            //        //    $('#Cmddocumentprint').attr('data-value', '');
            //        //    $('#Cmddocumentprint').hide();
            //        //}
            //        //else {
            //        //    $('#Sproutename').hide();
            //        //    $('#Cbroute').hide();
            //        //    $('#Cmdroute').hide();
            //        //    $('#Cmddocumentprint').attr('data-value', response.d['Flowid']);
            //        //    $('#Cmddocumentprint').show();
            //        //}

            //        html = '';
            //        //if (response.d['Status'] != 'S') {
            //        //    //Approver
            //        //    for (i = 0; i < res['Approves'].length; i++) {

            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-3">';
            //        //        html += ' <div class="container">';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">สถานะเอกสาร</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">';
            //        //        html += ' <input type="text" class="form-control" readonly=readonly value="' + res['Approves'][i]['Approvestatustext'] + '" />';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="col-6">';
            //        //        html += ' <div class="container">';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">ความเห็นเพิ่มเติม</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">';

            //        //        if (res['Isreadonly'] == "" && i == (res['Approves'].length - 1)) {
            //        //            html += ' <textarea style="height:100px;width:100%;" id="Txtcurrentremark"  class="form-control"></textarea>';
            //        //        }
            //        //        else {
            //        //            html += ' <textarea style="height:100px;width:100%;" readonly=readonly  class="form-control">' + res['Approves'][i]['Remark'] + '</textarea>';
            //        //        }
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="col-3">';
            //        //        html += ' <div class="container" style="text-align:center;">';
            //        //        html += ' <div class="row">';

            //        //        html += ' <div class="col-12"> <img src="' + res['Approves'][i]['Sigurl'] + '" style="width: 150px;" /></div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['Approvename'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['nodedesc'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['Approvedate'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <hr/>';
            //        //    }
            //        //    //Approver
            //        //    $('#Divapprover').html(html);
            //        //}
            //        //else {
            //        //    //Approver
            //        //    for (i = 0; i < res['Approves'].length; i++) {

            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-3">';
            //        //        html += ' <div class="container">';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">สถานะเอกสาร</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">';
            //        //        html += ' <input type="text" class="form-control" readonly=readonly value="' + res['Approves'][i]['Approvestatustext'] + '" />';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="col-6">';
            //        //        html += ' <div class="container">';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">ความเห็นเพิ่มเติม</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">';

            //        //        html += ' <textarea style="height:100px;width:100%;" readonly=readonly  class="form-control">' + res['Approves'][i]['Remark'] + '</textarea>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="col-3">';
            //        //        html += ' <div class="container" style="text-align:center;">';
            //        //        html += ' <div class="row">';

            //        //        html += ' <div class="col-12"> <img src="' + res['Approves'][i]['Sigurl'] + '" style="width: 150px;" /></div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['Approvename'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['nodedesc'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' <div class="row">';
            //        //        html += ' <div class="col-12">' + res['Approves'][i]['Approvedate'] + '</div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' </div>';
            //        //        html += ' <hr/>';
            //        //    }
            //        //    //Approver
            //        //    $('#Divapprover').html(html);
            //        //}
            //        //Desc 
            //        Getdesc(response.d['Flowid'])
            //        Calc();
            //        //Desc
            //        Getattachment()
            //        $('#Cmdroute').on('click', function () {
            //            Send($('#Cbroute').val());
            //        });
            //        if (response.d['Errormessage'] != '') {

            //            Msgbox(response.d['Errormessage']);
            //            return;
            //        }
            //    },
            //    async: true,
            //    error: function (er) {

            //    }
            //});
            $(this).attr('title', 'ใบเสนอราคา');
        });
        function Send(route) {
            var json = '';


            json += 'Routevalue :' + route + '|';
            json += 'Nodenamefrom :' + $('#Hdcurrentnode').val() + '|';
            json += 'Flowid :' + $('#Hdflowid').val() + '|';
            json += 'Flowconfigid :' + $('#Hdflowconfigid').val() + '|';
            json += 'Topic:ใบเสนอราคา - นาย มนัสวิน เอมะศิริ|';
            json += 'Customerid :' + $('#Txtcontactorname').attr('data-value') + '|';
            json += 'Customername :' + $('#Txtcontactorname').val() + '|';
            json += 'Customeraddress :' + $('#Txtcontactoraddress').val() + '|';
            json += 'Customertel :' + $('#Txtcontactoraddress').attr('data-value') + '|';
            json += 'Offerdate :' + $('#Txtofferdate').val() + '|';
            json += 'Saleid :' + $('#Cbsale').val() + '|';
            json += 'Saletel :' + $("#Cbsale option:selected").attr('data-value') + '|';
            json += 'Salename :' + $("#Cbsale option:selected").text() + '|';
            json += 'Totalprice :' + $('#Txtbeforetotal').val() + '|';
            json += 'vatamount :' + $('#Txtvat').val() + '|';
            json += 'Grandtotalprice :' + $('#Txtgrandtotal').val() + '|';
            json += 'Currentremark :' + $('#Txtcurrentremark').val() + '|';


            $.ajax({
                type: "POST",
                url: This + "/Save",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    Savestate();
                },
                success: function (response) {
                    if (response.d != '') {
                        Msgbox(response.d);
                        return;
                    }
                    window.opener.Callback('');
                    window.close();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Delservice(Serviceid) {

            var json = 'Serviceid : ' + Serviceid + '|';
            Savestate();
            $.ajax({
                type: "POST",
                url: This + "/Delservice",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    Getdesc();
                    Calc();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Addservice() {
            Savestate();
            $.ajax({
                type: "POST",
                url: This + "/Addservice",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d != '') {
                        Msgbox(response.d);
                        return;
                    }
                    Getdesc();
                    Calc();
                },
                async: true,
                error: function (er) {

                }
            });
        }

        function Selitem(servicegroupid, serviceid) {
            var json = 'servicegroupid :' + servicegroupid + '|';
            json += 'serviceid :' + serviceid + '|';
            $.ajax({
                type: "POST",
                url: This + "/Selitem",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d["Errormessage"] != "") {
                        Msgbox(response.d["Errormessage"]);
                        $('#Divservice').modal('hide');
                        return;
                    }
                    Getdesc();

                    $('#Divservice').modal('hide');
                },
                async: true,
                error: function (er) {

                }
            });
        }
        //function Renderupload() {
        //    var json = '';
        //    var html = '';
        //    $.ajax({
        //        type: "POST",
        //        url: This + "/Getupload",
        //        data: "{'json' :'" + json + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            html = '  <div class="col-12" style="text-align: left; margin-top: 20px;"> ';
        //            html += ' <div style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;" > ';
        //            html += ' <div class="row"> ';
        //            html += ' <div class="col-lg-2 col-md-4 col-sm-6 col-xs-12"> ';
        //            html += ' <div class="div-document" style="cursor:pointer;" onclick="Newdocument(\'\');">';
        //            html += ' <div class="container" style="padding: 10px;"> ';
        //            html += ' <div class="row"> ';
        //            html += ' <div class="col-12" style="text-align: center;"> ';
        //            html += ' <img src="/Img/EDOC/newdoc.png" style="width: 60px;"> ';
        //            html += ' </div> ';
        //            html += ' <div class="col-12" style="text-align: center;"> ';
        //            html += ' <div>แนบไฟล์เพิ่ม</div> ';
        //            html += ' </div> ';
        //            html += ' <div class="col-12" style="text-align: center; margin-top: 30px;"> ';
        //            html += ' <div style="font-size: 0.8em; color: red;">ขนาดไฟล์ไม่เกิน 25 MB</div> ';
        //            html += ' </div> ';
        //            html += ' </div> ';
        //            html += ' </div> ';
        //            html += ' </div> ';
        //            html += ' </div> ';
        //            for (i = 0; i < response.d.length; i++) {
        //                html += ' <div class="col-lg-2 col-md-4 col-sm-6 col-xs-12">';
        //                html += ' <div class="div-document" id="divattchment_' + response.d[i]['Attachmentid'] + '">';
        //                html += ' <div class="container" style="padding: 10px;">';
        //                html += ' <div class="row">';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <div onclick="Deleteattachment(\'' + response.d[i]['Attachmentid'] + '\');" style="position:absolute;right:20px;top:-30px;font-size:1.5em;color:red;cursor:pointer;"><i class="fa fa-trash" aria-hidden="true"></i></div>';
        //                html += ' <img src="/Img/EDOC/1.png" style="width: 60px;cursor:pointer;"  onclick="Downloadattachment(\'' + response.d[i]['Attachmentid'] + '\');">';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <div>' + response.d[i]['Label'] + '</div>';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center; margin-top: 10px;">';
        //                html += ' <div style="font-size: 0.8em;">' + response.d[i]['Uploaddate'] + '</div>';
        //                html += ' </div>';
        //                html += ' <div class="col-12" style="text-align: center;">';
        //                html += ' <div style="font-size: 0.8em;">โดย ' + response.d[i]['Uploadbynameth'] + '</div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';
        //                html += ' </div>';

        //            }
        //            html += ' </div>';
        //            html += '  </div> ';
        //            html += ' </div> ';
        //            $('#Divuploads').html(html);

        //        },
        //        async: false,
        //        error: function (er) {
        //            Msgbox(er.status);
        //        }
        //    });
        //}
        function Selservice(serviceid) {
            var json = 'serviceid :' + serviceid + '|';
            $.ajax({
                type: "POST",
                url: This + "/Selservice",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d["Errormessage"] != "") {
                        Msgbox(response.d["Errormessage"]);
                        $('#Divservice').modal('hide');
                        return;
                    }
                    Getdesc();
                    $('#Txtservice_' + response.d["Serviceid"]).val(response.d["Servicenameth"]);
                    $('#Txtservice_' + response.d["Serviceid"]).attr('data-value', response.d["Serviceid"]);
                    $('#Txtservice_' + response.d["Serviceid"]).attr('readonly', true);
                    $('#Cmdsearchservice_' + response.d["Serviceid"]).hide()
                    $('#Divservice').modal('hide');
                },
                async: true,
                error: function (er) {

                }
            });
        }
    </script>
    <div class="container-fluid">
        <div class="row" style="margin-top: 20px;">
              <div class="col-1" style="text-align: right;">
                <span>ลูกค้า</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-6">
                <div class="input-group mb-3">
                    <input type="text" id="Txtcontactorname" data-value="" class="form-control" placeholder="กดค้นหาลูกค้า" style="font-size: 14px; border-radius: 1px;" readonly="readonly">
                    <div class="input-group-append">
                        <button class="btn btn-outline-info" style="font-size: 14px; border-radius: 1px;" type="button" id="CmdSearch"><i class="fa fa-search" aria-hidden="true"></i></button>
                    </div>
                </div>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>วันที่เสนอราคา</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class='input-group date' id='Dtpofferdate'>
                        <input id="Txtofferdate" readonly="readonly" style="cursor:pointer;" class="form-control" type="text" />
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>

                </div>

            </div>
        </div>
        <div class="row">
            <div class="col-1" style="text-align: right;">
                <span>ที่อยู่</span>
            </div>
            <div class="col-6">
                <div class="input-group mb-3">
                    <textarea class="form-control" id="Txtcontactoraddress" data-value="" readonly="readonly" style="height: 50px;"></textarea>
                </div>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>ผู้ขาย</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <select class="form-control" id="Cbsale">
                    <option value="1" data-value="0855043969">สถาพร สีหะวงษ์</option>
                </select>
            </div>
        </div>
        <div class="row">
            <div class="col-12" style="text-align: center;">
                <div class="card card-body bg-light">รายละเอียดรายการ</div>
            </div>
        </div>
        <div class="row">
            <div class="col-12" style="text-align: left; margin-top: 20px;">
                <button class="btn btn-info" style="border-radius: 1px;" onclick="Addservice();">เพิ่มหมวดรายการ</button>
            </div>
        </div>
        <div class="row">
            <div class="col-12" style="text-align: left; margin-top: 20px;">
                <hr />
            </div>
        </div>
        <div class="row" id="Divdesc">
        </div>
        <div class="row">
             <div class="col-12">
                 <hr />
             </div>
          </div>
        <div class="row">
          <div class="col-12">
        <div class="container-fluid" style="background-color:cadetblue;color:white;padding:10px;padding-bottom:20px;">

       
         
         <div class="row">
             <div class="col-9" style="text-align:right;margin-top:10px;" >ราคารวมก่อนภาษีมูลค่าเพิ่ม</div>
              <div class="col-3" style="margin-top:10px;"><input class="form-control" style="text-align:right;" id="Txtbeforetotal" readonly="readonly" /></div>
        </div>
        <div class="row">
             <div class="col-9" style="text-align:right;margin-top:10px;">ภาษีมูลค่าเพิ่ม (7%)</div>
              <div class="col-3" style="margin-top:10px;" ><input class="form-control"  style="text-align:right;" id="Txtvat" readonly="readonly" /></div>
        </div>
         <div class="row">
             <div class="col-9" style="text-align:right;margin-top:10px;">ราคารวมทั้งสิ้น</div>
              <div class="col-3" style="margin-top:10px;"><input class="form-control"  style="text-align:right;" id="Txtgrandtotal" readonly="readonly"  /></div>
        </div>
        </div>
              </div>
       </div>
        <div class="row">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">ไฟล์แนบ</div>
            </div>
        </div>
        <div class="row">
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


        <div class="row">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">สายการอนุมัติ</div>
            </div>
        </div>
        <div class="row">
           <div class="col-12" style="text-align: left; margin-top: 20px;">
                <div id="DivSignnatures" style="border: solid 0.5px lightblue; border-radius: 4px; padding: 50px; margin-left: 20px; margin-right: 20px; min-height: 200px; margin-top: 10px;">
                </div>
            </div>
        </div>
    </div>
    <div class="modal" id="Divservice" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <span>รายการบริการ</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container">
                        <div class="row">
                            <div class="col-8">
                            </div>
                            <div class="col-4">
                                <div class="input-group mb-3">
                                    <input type="text" class="form-control" id="Txtsearchservice" placeholder="Search here" style="font-size: 0.9em; border-radius: 1px;">
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-info"  onclick="Searchservice();" style="font-size: 0.9em; border-radius: 1px;" type="button"><i class="fa fa-search" aria-hidden="true"></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container" id="Divservicecont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
                    </div>

                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" style="font-size: 0.9em; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>
    <div class="modal" id="Divcustomer" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" id="Divprofilecontent">
            <div class="modal-content">
                <div class="modal-header">
                    <span>Customers</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divcustomercont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" style="font-size: 0.9em; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>
    <div class="modal" id="Divnewcustomer" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <span>New Customer</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divnewcustomercont" style="margin-right: 10px; min-height: 320px; padding: 8px;">
                        <div class="row">
                            <div class="col-12">
                                <span>Customer name</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <input type="text" class="form-control" id="Txtnewcustomername" style="border-radius: 1px;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <span>Customer tel</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <input type="text" onkeypress="return isNumberKey(event)" class="form-control" id="Txtnewcustomertel" style="border-radius: 1px;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <span>Address</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <textarea class="form-control" id="Txtnewcustomeraddress" style="border-radius: 1px; height: 100px;"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" style="font-size: 0.9em; border-radius: 0;" data-dismiss="modal" onclick="Savecustomer();">Save</button>
                    <button type="button" class="btn btn-danger" style="font-size: 0.9em; border-radius: 0;" data-dismiss="modal">Close</button>
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
    <div class="modal" id="Divcontactor" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" >
            <div class="modal-content">
                <div class="modal-header">
                    <span>ลูกค้า</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divcontactorcont" style="border: solid 1px lightgray; margin-right: 10px; min-height: 320px; padding: 8px;">
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" style="font-size: 14px; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่าง</button>
                </div>

            </div>
        </div>
    </div>
    <div class="modal" id="Divnewcontactor" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <span>ลูกค้าใหม่</span>
                </div>
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="container" id="Divnewcontactorcont" style="margin-right: 10px; min-height: 320px; padding: 8px;">
                        <div class="row">
                            <div class="col-6">
                                <span>ชื่อลูกค้า</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                            <div class="col-6">
                                <span>เบอร์โทร</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                        
                          
                         </div>
                        <div class="row">
                            <div class="col-6">
                                <input type="text" class="form-control" id="Txtnewcontactorname" style="border-radius: 1px;" />
                            </div>
                       
                            <div class="col-6">
                                <input type="text" onkeypress="return isNumberKey(event)" class="form-control" id="Txtnewcontactortel" style="border-radius: 1px;" />
                            </div>
                        
                         </div>
                        <div class="row">
                            <div class="col-6">
                                <span>เลขที่บัตรประชาชน</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                      
                         <div class="col-6">
                                <span>วันที่หมดอายุบัตร</span>
                            </div>
                           
                        </div>
                        <div class="row">
                           
                         <div class="col-6">
                                <input type="text" maxlength="13" onkeypress="return isNumberKey(event)" class="form-control" id="TxtnewcontactorcardID" style="border-radius: 1px;" />
                            </div>
                      
                            <div class="col-6">
                                <div class='input-group date' id='Dtpnewcontactorexprirydate'>
                                    <input id="Txtnewcontactorexprirydate" class="form-control" type="text" />
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                </div>

                            </div>
                        </div>

                        <div class="row">
                            <div class="col-6">
                                <span>ธนาคาร</span>
                            </div>
                            <div class="col-6">
                                <span>ประเภทบัญชีธนาคาร</span>
                            </div>
                          
                        </div>
                        <div class="row">
                           
                            <div class="col-6">
                                <select class="form-control" id="Cbnewcontactorbank">
                                </select>
                            </div>
                            <div class="col-6">
                                <select class="form-control" id="Cbnewcontactorbankaccounttype">
                                    <option value="SV" selected>ออมทรัพย์</option>
                                    <option value="CA">กระแสรายวัน</option>
                                </select>
                            </div>
                           
                          
                        </div>
                        <div class="row">
                              <div class="col-6">
                                <span>เลขที่บัญชีธนาคาร</span>
                            </div>
                          
                       
                            <div class="col-6">
                                <span>ชื่อบัญชีธนาคาร</span>
                            </div>
                      
                        </div>
                        <div class="row">
                              <div class="col-6">
                                <input type="text" class="form-control" onkeypress="return isNumberKey(event)" id="Txtnewcontactorbankaccountno" style="border-radius: 1px;" />
                            </div>
                            <div class="col-6">
                                <input type="text" class="form-control" id="Txtnewcontactorbankaccountname" style="border-radius: 1px;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <span>ที่อยู่</span>&nbsp;<span style="color: red;">*</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <textarea class="form-control" id="Txtnewcontactoraddress" style="border-radius: 1px; height: 100px;"></textarea>
                            </div>
                        </div>

                    </div>
                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <button type="button" class="btn btn-standard" style="background-color:#313131;color:white; font-size: 14px; border-radius: 0;"  onclick="Savecontactor();">บันทึกลูกค้าใหม่</button>
                        <button type="button" class="btn btn-danger" style="font-size: 14px; border-radius: 0;" data-dismiss="modal">ปิดหน้าต่างนี้</button>
                    </div>

                </div>
            </div>
        </div>
    </div>

  
</asp:Content>
