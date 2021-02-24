<%@ Page Title="" Language="C#" MasterPageFile="~/Page/EDWF/Forms/Control.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Forms.Contact.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../../../../js/Numeric.js"></script>
    <script>
        var This = "Contact.aspx";
        function Newdocument() {
            $('#Txtattachmentlabel').val('');
            $('#Divupload').modal('show');
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
                    window.open(response.d, '_blank','toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=800,height=800');
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
                        html += ' <div style="font-size: 0.8em;">' + response.d[i]['Uploaddate'] +'</div>';
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
        function Print() {
            var json = '';
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
        function ValidateforAction(nodename) {
            var json = '';
            json += 'nodename :' + nodename;
            var out = '';
            $.ajax({
                type: "POST",
                url: This + "/ValidateforAction",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    $('#CbAction').prop("disabled", false);
                    $("#CmdAction").html('Action');
                    $('#CmdActionfg').prop("disabled", false);
                    if (out != '') {

                        $("#CbAction option").each(function (i) {
                            if ($(this).val() != out && $(this).val() != '') {

                                $(this).remove();
                            }
                        });
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
        function SaveMultiple(nodename, id) {
            var val = '';
            var text = '';
            var remark = '';

            text = $("#CbRoute" + nodename + "_" + id + " option:selected").text();
            val = $('#CbRoute' + nodename + '_' + id).val();
            remark = $('#TxtRemark_' + nodename + '_' + id).val();

            if (val == '') {
                Msgbox('Please select action');
                return;
            }
            var json = 'DirectionNameTH :' + text + ",";
            json += 'DirectionValue :' + val + ",";
            json += 'remark :' + remark + ",";
            json += 'userid :' + id + ",";
            json += 'nodename :' + nodename;
            $.ajax({
                type: "POST",
                url: This + "/SaveMultiple",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    GetRoute();
                    Msgbox('Save Completed')
                    ValidateforAction(nodename);
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
        function GetNodeMultipleItem(nodename, item, readonly) {
            var html = '';
            var btn = '';
            var id = item['UserId'];
            var i = 0;
            var TxtRemark = 'TxtRemark_' + nodename + '_' + id;
            var Cbroute = '<select id=\"CbRoute' + nodename + '_' + id + '\" style=\"width:100%;\" class="form-control">';
            Cbroute += '<option value="">-- Please Select --</option>'
            btn += '<button type="button" class="btn btn-danger" style="width: 50px;" id="CmdOK_' + nodename + '_' + id + '\" onclick="SaveMultiple(\'' + nodename + "','" + id + '\');">';
            btn += 'OK';
            btn += '</button>';
            for (i = 0; i < item.Directions.length; i++) {

                if (item.Directions[i]["Extend1"] == 'selected') {
                    Cbroute += '<option value=\'' + item.Directions[i]["Val"] + '\' selected >' + item.Directions[i]["Name"] + '</option>'
                }
                else {
                    Cbroute += '<option value=\'' + item.Directions[i]["Val"] + '\' >' + item.Directions[i]["Name"] + '</option>'
                }
            }

            Cbroute += '</select>';
            html += '<div class="panel panel-default" id="Div_' + nodename + '_' + id + '"  style="display: block;width:98%;">';
            html += '<div class="panel-heading" style="text-align:left;">';
            html += '<span id="SP' + nodename + "_" + id + '">' + item['FullName'] + '</span> ';
            html += '</div>';
            html += '<div  class="panel-collapse collapse in">';
            html += '<div class="panel-body">';
            html += '<table style="width:100%;border-spacing: 10px;border-collapse: separate;" >';
            html += '<tr>';
            html += '<td><span>เส้นทาง</span></td><td>' + Cbroute + '</td><td style="text-align:right;">' + btn + '</td>';
            html += '</tr>';
            html += '<tr>';
            html += '<td style="vertical-align:top;"><span>หมายเหตุ</span></td><td colspan="2"><textarea style="width:100%;height:80px;" id=' + TxtRemark + '>' + item['Remark'] + '</textarea></td>';
            html += '</tr>';
            html += '</table>';
            html += '</div>';
            html += '</div>';
            html += '</div>';
            return html;
        }
        function GetNodeMultipleDetail(NodeMultipleName, DivOutput, IsInquiry) {
            var json = 'NodeMultipleName:' + NodeMultipleName;
            var r = 0;
            var out;
            var i = 0;
            var c = 0;
            var rowlength = 0;
            var collength = 3;
            var html = '';
            $.ajax({
                type: "POST",
                url: This + "/GetNodeMultipleDetail",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    html += '<table style="width:100%; border="1">';
                    rowlength = out.length / collength;
                    if (out.length % collength != 0) {
                        rowlength += 1;
                    }
                    for (i = 0; i < out.length; i++) {
                        if (c == 0) {
                            html += '<tr>';
                        }
                        html += '<td>' + GetNodeMultipleItem(NodeMultipleName, out[i], IsInquiry) + '</td>';
                        if (c == collength) {
                            c = 0;
                            html += '</tr>';
                        }
                        c += 1;
                    }
                    for (i = c; i <= collength; i++) {
                        html += '<td>&nbsp;</td>';
                    }
                    c = 0;
                    html += '</tr>';
                    html += '</table>';
                    $('#' + DivOutput).html(html);
                    for (i = 0; i < out.length; i++) {

                        $('#CmdOK_' + NodeMultipleName + '_' + out[i]["UserId"]).prop('disabled', true);


                        $('#CbRoute' + NodeMultipleName + '_' + out[i]["UserId"]).prop('disabled', true);

                        $('#TxtRemark_' + NodeMultipleName + '_' + out[i]["UserId"]).attr('readonly', 'readonly');
                        $('#TxtRemark_' + NodeMultipleName + '_' + out[i]["UserId"]).css('background-color', 'lightgrey');
                    }



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
            if (!IsInquiry) {
                $('#CbAction').prop("disabled", true);
                $("#CmdAction").html('Wait for Submit Approve');
                $('#CmdAction').prop("disabled", true);
                ValidateforAction(NodeMultipleName);
            }

        }
        function Progress() {
            $('#DivProgress').modal({

            });
        }
        function ProgressHide() {
            $('#DivProgress').modal('toggle');
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
                    if (response.d["Documentno"] == '') {
                        $('#SpDocumentNo').html('<span style="color:red;">สร้างเมื่อมีการเดินเอกสาร</span>');
                    }
                    else {
                        $('#SpDocumentNo').html(response.d["Documentno"]);
                    }
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
                        $('#Txtcontactorname').attr('data-value', response.d['ContId']);
                        $('#Txtcontactorname').val(response.d['fullname']);
                        $('#Txtcontactoraddress').val(response.d['address']);
                        $('#Txtcontactortel').val(response.d['Tel']);
                        $('#Txtcontactdate').val(response.d['Contactdate']);
                        $('#TxtcontactorcardID').val(response.d['Cardid']);
                        $('#Txtcontactorexprirydate').val(response.d['Expirydate']);
                        $("#Cbcontactorbank").val(response.d['Bankid']);
                        $('#Txtcontactorbankaccountno').val(response.d['Bankaccountno']);
                        $('#Txtcontactorbankaccountname').val(response.d['Bankaccountname']);
                        $("#Cbcontactorbankaccounttype").val(response.d['Bankaccounttype']);
                        $('#Txtsitename').val(response.d['Sitename']);
                        $('#Txtsiteaddress').val(response.d['Sitefulladdress']);
                        $('#Txtjobdesc').val(response.d['Jobdescription']);
                        $('#Txtfee').val(response.d['Fee']);
                        $('#Txtfinisheddate').val(response.d['Finisheddate']);
                        $('#Txttotalamount').val(response.d['Totalamount']);
                        $('#Txtcontactstart').val(response.d['Contactstart']);
                        $('#Txtcontactend').val(response.d['Contactend']);
                        try {
                            if (response.d['Periods'].length != null) {
                                $('#Txtperiod').val(response.d['Periods'].length);
                            }
                        }
                        catch (ex) {

                        }
                        Renderperiod();
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
                        $('#Txtcontactorname').attr('data-value', response.d['ContId']);
                        $('#Txtcontactorname').val(response.d['fullname']);
                        $('#Txtcontactoraddress').val(response.d['address']);
                        $('#Txtcontactortel').val(response.d['Tel']);
                        $('#Txtcontactdate').val(response.d['Contactdate']);
                        $('#TxtcontactorcardID').val(response.d['Cardid']);
                        $('#Txtcontactorexprirydate').val(response.d['Expirydate']);
                        $("#Cbcontactorbank").val(response.d['Bankid']);
                        $('#Txtcontactorbankaccountno').val(response.d['Bankaccountno']);
                        $('#Txtcontactorbankaccountname').val(response.d['Bankaccountname']);
                        $("#Cbcontactorbankaccounttype").val(response.d['Bankaccounttype']);
                        $('#Txtsitename').val(response.d['Sitename']);
                        $('#Txtsiteaddress').val(response.d['Sitefulladdress']);
                        $('#Txtjobdesc').val(response.d['Jobdescription']);
                        $('#Txtfee').val(response.d['Fee']);
                        $('#Txtfinisheddate').val(response.d['Finisheddate']);
                        $('#Txttotalamount').val(response.d['Totalamount']);
                        $('#Txtcontactstart').val(response.d['Contactstart']);
                        $('#Txtcontactend').val(response.d['Contactend']);
                        try {
                            if (response.d['Periods'].length != null) {
                                $('#Txtperiod').val(response.d['Periods'].length);
                            }
                        }
                        catch (ex) {

                        }
                        Renderperiod();
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
                        //$('#CbAction').append($('<option/>', {
                        //    value: val["Val"],
                        //    text: val["Name"]
                        //}));
                        $('#CbAction').append('<option id="' + val["Val"] + '" value="' + val["Val"] + '">' + val["Name"] + '</option>');
                    }
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
        //function DownloadAttachment(Id, NodeName, DivOutput) {
        //    var json = 'AttachmentId:' + Id + ',';
        //    json += 'NodeName:' + NodeName + ',';
        //    $.ajax({
        //        type: "POST",
        //        url: This + "/DownloadAttachment",
        //        data: "{'json' :'" + json + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            out = response.d;
        //            window.open(out, '_blank', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=5, height=5');
        //        },
        //        async: false,
        //        error: function (er) {
        //            Msgbox(er.status);
        //        }
        //    });

        //}
        //function DeleteAttachment(Id, NodeName, DivOutput) {
        //    var json = 'AttachmentId:' + Id + ',';
        //    json += 'NodeName:' + NodeName + ',';
        //    $.ajax({
        //        type: "POST",
        //        url: This + "/DeleteAttachment",
        //        data: "{'json' :'" + json + "'}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            out = response.d;
        //            GetAttachment(NodeName, DivOutput);
        //        },
        //        async: true,
        //        error: function (er) {
        //            Msgbox(er.status);
        //        }
        //    });
        //}
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
      
        function Savestate() {
            var json = '';
            var id = '';
            var quann = '';
            var name = '';
            $("Textarea[id*='Txtperiodname_']").each(function (i, el) {

                id = $(el).attr('id').replace('Txtperiodname_', '');
                name = $(el).val();
                amount = $('#Txtperiodamount_' + id).val();
                json += 'id:' + id + '|';
                json += 'name:' + name + '|';
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

        function Renderperiod() {
            var html = '';
            var i = 0
            $.ajax({
                type: "POST",
                url: This + "/Renderperiod",
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
                            html += '<div class="row" style="margin-top:10px;">'
                            html += '<div class="col-1">'
                            html += '<span>' + (i + 1) + '</span>';
                            html += '</div>';
                            html += '<div class="col-8">'
                            html += '<Textarea style="height:100px;" class="form-control" id="Txtperiodname_' + i.toString() + '">' + response.d[i]['Periodname'] + '</textarea>';
                            html += '</div>';
                            html += '<div class="col-3">'
                            html += '<input type="text" class="form-control" style="color:red;text-align:right;" onblur="FormatWithRound(this,2)" onfocus="UnFormat(this)" onkeypress="OnlyNumeric(event,this)"  id="Txtperiodamount_' + i.toString() + '" ' + '" value="' + response.d[i]['Amount'] + '"   onkeypress="return isNumberKey(event)" />';
                            html += '</div>';
                            html += '</div>';
                        }
                        html += '</div>';
                    }
                    $("#Divperiod").html(html);
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
        function Createperiod() {
           
          
            var x = $('#Txtperiod').val();
            $.ajax({
                type: "POST",
                url: This + "/Createperiod",
                data: "{'x' : '" + x + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    res = response.d;
                    if (res != '') {
                        Msgbox(res);
                        return;
                    }
                    Renderperiod();
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
        function Searchcontactor() {

            ClearResource('Gvcontactor');
            var Columns = ["ชื่อลูกค้า!C", "ที่อยู่!L"];
            var Data = ["fullname", "Address"];
            var Searchcolumns = ["ชื่อลูกค้า", "ที่อยู่"];
            var SearchesDat = ["fullname", "Address"];
            var Width = ["40%", "60%"];
            var grdcontactor = new Grid(Columns, SearchesDat, Searchcolumns, 'Gvcontactor', 30, Width, Data, "", '', '2', 'สร้างลูกค้าใหม่', '', '', '', '', '', 'id', 'id', '', 'id', '', '');
            $('#Divcontactorcont').html(grdcontactor._Tables());
            grdcontactor._Bind();

        }
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
                        $('#Txtcontactortel').val(response.d[0]['Tel']);
                        $('#TxtcontactorcardID').val(response.d[0]['CardID']);
                        $('#Txtcontactorexprirydate').val(response.d[0]['Exprirydate']);
                        $('#Cbcontactorbank').val(response.d[0]['Bank']);
                        $('#Txtcontactorbankaccountname').val(response.d[0]['Bankaccountname']);
                        $('#Txtcontactorbankaccountno').val(response.d[0]['Bankaccountno']);

                        $('#Cbcontactorbankaccounttype').val(response.d[0]['Bankaccounttype']);
                        $('#Divcontactor').modal('hide');
                    },
                    async: true,
                    error: function (er) {

                    }
                });
            }
        }
       
        $(function () {
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
            $("#Txtfinisheddate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());
            $("#Txtcontactorexprirydate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());
            $("#Txtcontactdate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#Txtnewcontactorexprirydate").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            $("#Txtcontactstart").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());

            $("#Txtcontactend").datepicker({
                format: 'dd-mm-yyyy',
                autoclose: true,
                todayHighlight: true
            }).datepicker('update', new Date());


            
            GetRoute();
            Getbank();
            GetInfo();
        });
        function Save() {



            var html = '';
            var i = 0;
            var json = 'readonly:false|';
            var Div = 'DivSignnatures';
            
            json += 'contactorid :' + $('#Txtcontactorname').attr('data-value') + '|';
            json += 'contactorname :' + $('#Txtcontactorname').val() + '|';
            json += 'contactoraddress :' + $('#Txtcontactoraddress').val() + '|';
            json += 'contactortel :' + $('#Txtcontactortel').val() + '|';
            json += 'contactdate :' + $('#Txtcontactdate').val() + '|';
            json += 'contactorcardID :' + $('#TxtcontactorcardID').val() + '|';
            json += 'contactorexprirydate :' + $('#Txtcontactorexprirydate').val() + '|';
            json += 'contactorbank :' + $("#Cbcontactorbank").val() + '|';
            json += 'contactorbanktext :' + $("#Cbcontactorbank option:selected").text() + '|';
            json += 'contactorbankaccountno :' + $('#Txtcontactorbankaccountno').val() + '|';
            json += 'contactorbankaccountname :' + $('#Txtcontactorbankaccountname').val() + '|';
            json += 'contactorbankaccountype :' + $("#Cbcontactorbankaccounttype").val() + '|';
            json += 'contactorbankaccountypetext :' + $("#Cbcontactorbankaccounttype option:selected").text() + '|';
            json += 'sitename :' + $('#Txtsitename').val() + '|';
            json += 'siteaddress :' + $('#Txtsiteaddress').val() + '|';
            json += 'jobdesc :' + $('#Txtjobdesc').val() + '|';

            json += 'fee :' + $('#Txtfee').val() + '|';
            json += 'finisheddate :' + $('#Txtfinisheddate').val() + '|';
            json += 'totalamount :' + $('#Txttotalamount').val() + '|';
            json += 'contactstart :' + $('#Txtcontactstart').val() + '|';
            json += 'contactend :' + $('#Txtcontactend').val() + '|';
            json += 'period :' + $('#Txtperiod').val() + '|';
            json += 'DocumentNo: ' + $('#SpDocumentNo').html() + '|';
            json += 'ActionResultValue: ' + $('#CbAction').val() + '|';
            json += 'ActionResultNameTH: ' + $('#CbAction option:selected').text() + '|';
            //if ($('#HdNodeName').val() == 'NodeBegin1') {
            //    Div = 'Div_NodeBegin1';
            //}
            $("#" + Div).find('textarea').each(function () {
                if ($(this).attr('disabled') != 'disabled') {

                    json += $(this).attr('id') + ':' + $(this).val() + "|";
                }
            });
            //$("#" + Div).find('input:text|input:hidden| input:password| input:file')
            //    .each(function () {
            //        json += $(this).attr('data-column') + ':' + $(this).val() + '|';
            //    });
           
            //$("#" + Div).find('img')
            //    .each(function () {

            //        json += $(this).attr('data-column') + ':' + $(this).attr('data-value') + "|";

            //    });
            //$("#" + Div).find('select')
            //    .each(function () {
            //        json += $(this).attr('data-column') + ':' + $(this).val() + "|";
            //        json += $(this).attr('data-column') + '_ext:' + $('#' + $(this).prop('id') + ' option:selected').text() + '|';
            //    });
            //$("#" + Div).find('input:radio| input:checkbox').each(function () {
            //    if ($(this).prop('checked')) {
            //        flg = 1;
            //    }
            //    else {
            //        flg = 0;
            //    }
            //    json += $(this).attr('data-column') + ':' + flg + "|";
            //});
            
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
                    if (response.d != '') {
                        Msgbox(response.d);
                        return;
                    }
                    
                    window.opener.CallBackRefresh(out);
                    window.close();
                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
      
    </script>
    <style>
        .sp-require
        {
            color:red;
        }
        .row { margin-top:10px; }
        body {
            font-family: Kanit;
        }
        .button
        {
             color:gray;
            font-size:14px;
            font-family:Kanit;
        }
        .form-control
        {
            border-radius:1px;
             color:#313131;
            font-size:14px;
            font-family:Kanit;
        }
        .modal-body
        {
            color:gray;
            font-size:14px;
            font-family:Kanit;
        }
    </style>
    <div class="container-fluid">

        <div class="row" style="margin-top: 20px;">
            <div class="col-2" style="text-align: right;">
                <span>ผู้ว่าจ้าง /ลูกค้า</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5">
                <div class="input-group mb-3">
                    <input type="text" id="Txtcontactorname" data-value="" class="form-control" placeholder="กดค้นหาผู้ว่าจ้าง" style="font-size: 14px; border-radius: 1px;" readonly="readonly">
                    <div class="input-group-append">
                        <button class="btn btn-outline-info" style="font-size: 14px; border-radius: 1px;" type="button" id="CmdSearch"><i class="fa fa-search" aria-hidden="true"></i></button>
                    </div>
                </div>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>สัญญามีผลบังคับใช้</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class='input-group date' id='Dtpcontactdate'>
                    <input id="Txtcontactdate" readonly="readonly" class="form-control"  style="cursor:pointer;"  type="text" />
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
                <input type="text" id="Txtcontactortel" onkeypress="return isNumberKey(event)" class="form-control" />
            </div>
        </div>
        <div class="row">
            <div class="col-2" style="text-align: right;">
                <span>บัตรประชาชน</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5">
                <div class="input-group mb-3">
                    <input type="text" id="TxtcontactorcardID" onkeypress="return isNumberKey(event)" class="form-control" />

                </div>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>บัตรหมดอายุ</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class='input-group date' id='Dtpcontactorexprirydate'>
                    <input id="Txtcontactorexprirydate" readonly="readonly" class="form-control"  style="cursor:pointer;"  type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>

            </div>
        </div>
        <div class="row">
            <div class="col-2" style="text-align: right;">
                <span>ธนาคาร</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5">
                <select id="Cbcontactorbank" class="form-control" style="margin-bottom: 10px;">
                </select>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>เลขที่บัญชี</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class="input-group mb-3">
                    <input type="text" id="Txtcontactorbankaccountno" onkeypress="return isNumberKey(event)" class="form-control" />
                </div>
            </div>

        </div>
        <div class="row" style="margin-bottom: 20px;">
            <div class="col-2" style="text-align: right;">
                <span>ชื่อบัญชี</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5">
                <input type="text" id="Txtcontactorbankaccountname" class="form-control" />
            </div>
            <div class="col-2" style="text-align: right;">
                <span>ประเภท</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <select id="Cbcontactorbankaccounttype" class="form-control">
                    <option value="SV" selected>ออมทรัพย์</option>
                    <option value="CA">กระแสรายวัน</option>
                </select>

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
                <input type="text" id="Txtsitename" data-value="" class="form-control" style="font-size: 14px; border-radius: 1px;">
            </div>
            <div class="col-2" style="text-align: right;">
                <span>สัญญาเริ่มต้น</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class='input-group date' id='Dtpcontactstart'>
                    <input id="Txtcontactstart" readonly="readonly" class="form-control"  style="cursor:pointer;"  type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>

            </div>
        </div>
        <div class="row" style="margin-top: 20px;">
            <div class="col-2" style="text-align: right;">
                <span>ที่อยู่ Site งาน</span> &nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5" style="text-align: left;">
                <input type="text" id="Txtsiteaddress" data-value="" class="form-control" style="font-size: 14px; border-radius: 1px;">
            </div>
            <div class="col-2" style="text-align: right;">
                <span>สัญญาสิ้นสุด</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <div class='input-group date' id='Dtpcontactend'>
                    <input id="Txtcontactend" readonly="readonly" class="form-control"  style="cursor:pointer;"  type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>

            </div>
        </div>
        <div class="row" style="margin-top: 20px;">
            <div class="col-2" style="text-align: right;">
                <span>ค่าปรับ/วัน (ถ้ามี)</span>
            </div>
            <div class="col-1" style="text-align: left;">
                <input type="text" id="Txtfee" data-value="" style="color:red;text-align:right;" onblur="FormatWithRound(this,2)" onfocus="UnFormat(this)" onkeypress="OnlyNumeric(event,this)" class="form-control">
            </div>
            <div class="col-2" style="text-align: right;">
                <span>กำหนดเสร็จ</span> &nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-2" style="text-align: left;">
                <div class='input-group date' id='Dtpfinisheddate'>
                    <input id="Txtfinisheddate" style="cursor:pointer;" readonly="readonly" class="form-control" type="text" />
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                </div>
            </div>
            <div class="col-2" style="text-align: right;">
                <span>ราคาจ้างเหมารวมทั้งสิ้น</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-3" style="text-align: right;">
                <input type="text" id="Txttotalamount" style="color:red;text-align:right;" onblur="FormatWithRound(this,2)" onfocus="UnFormat(this)" onkeypress="OnlyNumeric(event,this)" class="form-control" />

            </div>
        </div>
        <div class="row" style="margin-top: 20px;">
            <div class="col-2" style="text-align: right;">
                <span>รายละเอียดงาน</span> &nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-5" style="text-align: left;">
                <input type="text" id="Txtjobdesc" data-value="" class="form-control" style="font-size: 14px; border-radius: 1px;">
            </div>

            <div class="col-2" style="text-align: right;">
                <span>จำนวนงวด</span>&nbsp<span class="sp-require">*</span>
            </div>
            <div class="col-1" style="text-align: right;">
                <input type="number" min="1" max="99" id="Txtperiod" value="1" class="form-control" />
            </div>
            <div class="col-2" style="text-align: right;">
                <button class="btn btn-danger" id="Cmdperiod" onclick="Createperiod();" style="width: 100%;">สร้างงวดงาน</button>
            </div>
        </div>
        <div class="row">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div class="card card-body bg-light">งวดงาน</div>
            </div>
        </div>
        <div class="row">
            <div class="col-12" style="text-align: center; margin-top: 50px;">
                <div id="Divperiod" style="text-align: center;">
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
    <div class="modal" id="Divcontactor" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" id="Divprofilecontent">
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
                    <button type="button" class="btn btn-primary" style="font-size: 14px; border-radius: 0;" onclick="Doupload();" >แนบเอกสาร</button>
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
