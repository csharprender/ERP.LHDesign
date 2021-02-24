<%@ Page Title="" Language="C#" MasterPageFile="~/Page/ERP.Master" AutoEventWireup="true" CodeBehind="Task.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Task" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        span {
            font-size: 12px;
        }

        table {
            font-size: 12px;
        }
    </style>

    <script src="../../Bootstrap/jquery.min.js"></script>
    <script>
        var This = document.location.pathname.match(/[^\/]+$/)[0];
        function FlowInfo(TransbatchId) {
            var json = 'TransbatchId:' + TransbatchId;
            var r = 0;
            var out;
            var i = 0;
            var c = 1;
            var rows = 0;
            var cols = 0;
            var itemlength = 4;
            var Totalitems = 0;
            var html = '';

            $.ajax({
                type: "POST",
                url: This + "/GetNodeInFlow",
                data: "{'json' : '" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    out = response.d;
                    //Draw Layout
                    html += '<table style="width:100%;" border="0">';
                    rows = out.length / itemlength;
                    rows = rows * 2;
                    cols = (itemlength * 2) - 1;
                    if (out.length % itemlength != 0) {
                        rows += 1; //ถ้ามี Row เกินให้เพิ่มขึ้นมา 1 บรรทัด
                    }
                    Totalitems = rows * cols;
                    for (i = 1; i <= Totalitems; i++) {
                        if (c == 1) {
                            html += '<tr>';
                            r += 1;
                        }
                        html += '<td style="width:10%"><div id="div_' + r + '_' + c + '">&nbsp;</div></td>';
                        if (c == cols) {
                            c = 0;

                            html += '</tr>';
                        }
                        c += 1;
                    }
                    for (i = c; i <= cols; i++) {
                        html += '<td>&nbsp;</td>';
                    }
                    c = 0;
                    html += '</tr>';
                    html += '</table>';
                    $('#DivNodeinFlow').html(html);
                    r = 1;
                    c = 1;
                    var inrow = 0;
                    var item = '';
                    var L2R = true;
                    var IsArrow = false;
                    var ArrowRight = '<img src="../../Image/ArrowR.png" style="width:60px;" />';
                    var ArrowLeft = '<img src="../../Image/ArrowLeft.png" style="width:60px;" />';
                    var ArrowDown = '<img src="../../Image/ArrowDown.png" style="padding:20px;margin-left:10px;height:100px;" />';
                    for (i = 0; i < out.length; i++) {

                        item = '<div><table style="width:100%"><tr><td><img src="' + out[i]['NodePicURL'] + '" style="width:100px;" /></td></tr><tr><td ><span style="margin-top:5px;margin-left:20px;">' + out[i]['NodeTypeNameTH'] + '</span></td></tr><tr><td ><span style="margin-top:5px;margin-left:25px;">' + out[i]['FullName'] + '</span></td></tr></table></div>';
                        if (L2R) {
                            if (inrow < cols) {
                                inrow += 1
                            }
                            else {
                                r += 1;
                                L2R = false;
                            }
                            $('#div_' + r + '_' + inrow).html(item);
                            inrow += 1;
                            if (out[i]['NodeTypeNameTH'] != "NodeEnd") {
                                if (i < (out.length - 1)) {
                                    $('#div_' + r + '_' + inrow).html(ArrowRight);
                                }
                            }
                        }

                        if (!L2R) {

                            if ((r % 2) == 0) {

                                inrow = 7;

                                $('#div_' + r + '_' + inrow).html(ArrowDown);
                                r += 1;

                            }
                            else {
                                $('#div_' + r + '_' + inrow).html(item);
                                inrow -= 1;

                                if (out[i]['NodeTypeNameTH'] != "NodeEnd") {
                                    if (i < (out.length - 1)) {
                                        $('#div_' + r + '_' + inrow).html(ArrowLeft);
                                    }
                                }
                                inrow -= 1;
                            }
                            //Write ตัวท้ายสุดแล้วถอยไปตัวหน้าสุด
                        }

                    }
                    $('#DivFlowInfo').modal({

                    });
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
        function CallBackRefresh(Module) {
            if (Module == "2") //End
            {
                GetComplete();
            }
            else {
                GetPending();
            }
            Msgbox('Save is successful')
        }
        function Progress() {
            $('#DivProgress').modal({

            });
        }
        function ProgressHide() {
            $('#DivProgress').modal('toggle');
        }
        function Msgbox(Msg) {
            $("#Divalert").html('<img src="Image/Prevent.png" style="width:20px;height:20px;" />&nbsp;' + Msg);
            $("#Divalert").alert();
            $("#Divalert").fadeTo(2000, 500).slideUp(500, function () {
                $("#Divalert").slideUp(500);
            });
        }
        function Inq(TransBatchId) {
            w = 800;
            h = 600;
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var json = 'TransBatchId:' + TransBatchId + ',';
            var i = 0;
            var out;
            var html = '';
            $.ajax({
                type: "POST",
                url: This + "/Inq",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    popup = window.open(response.d['FormURL'], 'popup', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });

        }
        function Work(TransBatchId) {
            w = 800;
            h = 600;
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            var json = 'TransBatchId:' + TransBatchId + ',';
            var i = 0;
            var out;
            var html = '';
            $.ajax({
                type: "POST",
                url: This + "/Work",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    popup = window.open(response.d.FormURL, 'popup', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
                },
                async: false,
                error: function (er) {
                    Msgbox(er.status);
                }
            });

        }
        function GetPending() {
            $('#Divinvolve').hide();
            $('#Divcomplete').hide();
            $('#Divcancel').hide();
            $('#Divpending').show();

            var json = '';
            var i = 0;
            var html = '';
            var Subject
            var DocumentDate = '';
            var DocumentBy = '';
            var Status = '';
            var FlowInfo = '';
            var Work = '';
            json = 'inq:' + $('#TxtInqPending').val() + ",";
            $.ajax({
                type: "POST",
                url: This + "/GetPending",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '';

                    if (response.d.length > 0) {
                        for (i = 0; i < response.d.length; i++) {
                            Subject = response.d[i]["Subject"];
                            DocumentDate = response.d[i]["DocumentDate"];
                            DocumentBy = response.d[i]["DocumentBy"];
                            Status = response.d[i]["Status"];
                            FlowInfo = response.d[i]["FlowInfo"];
                            Work = response.d[i]["Work"];
                            html += "<tr>";
                            html += "<td>" + Subject + "</td>";
                            html += "<td>" + DocumentBy + "</td>";
                            html += "<td>" + DocumentDate + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td>" + FlowInfo + "</td>";
                            html += "<td>" + Work + "</td>";
                            html += "</tr>";
                        }
                    }
                    else {
                        html += "<tr>";
                        html += "<td colspan='6' style='text-align:center'><span style='Color:red;'>ไม่พบข้อมูล</span></td>";
                        html += "</tr>";
                    }
                    $('#DivPendingBody').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function GetInvolve() {
            $('#Divinvolve').show();
            $('#Divcomplete').hide();
            $('#Divcancel').hide();
            $('#Divpending').hide();

            var json = '';
            var i = 0;
            var html = '';
            var Subject
            var DocumentDate = '';
            var DocumentBy = '';
            var Status = '';
            var FlowInfo = '';
            var Work = '';
            json = 'inq:' + $('#TxtInqInvolve').val() + ",";
            $.ajax({
                type: "POST",
                url: This + "/GetInvolve",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '';
                    if (response.d.length > 0) {
                        for (i = 0; i < response.d.length; i++) {
                            Subject = response.d[i]["Subject"];
                            DocumentDate = response.d[i]["DocumentDate"];
                            DocumentBy = response.d[i]["DocumentBy"];
                            Status = response.d[i]["Status"];
                            FlowInfo = response.d[i]["FlowInfo"];
                            Work = response.d[i]["Work"];
                            html += "<tr>";
                            html += "<td>" + Subject + "</td>";
                            html += "<td>" + DocumentBy + "</td>";
                            html += "<td>" + DocumentDate + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td style='text-align:center;color:red;'>" + FlowInfo + "</td>";
                            html += "<td  style='text-align:center;color:blue;'>" + Work + "</td>";
                            html += "</tr>";
                        }
                    }
                    else {
                        html += "<tr>";
                        html += "<td colspan='6' style='text-align:center;'><div style='Color:red;min-height:300px;border:solid 1px lightgray;padding:20px;'><span style='margin-top:100px;font-size:1.2em;'>ไม่พบข้อมูล</span></div></td>";
                        html += "</tr>";
                    }
                    $('#DivInvolveBody').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function GetComplete() {
            $('#Divinvolve').hide();
            $('#Divcomplete').show();
            $('#Divcancel').hide();
            $('#Divpending').hide();

            var json = '';
            var i = 0;
            var html = '';
            var Subject
            var DocumentDate = '';
            var DocumentBy = '';
            var Status = '';
            var FlowInfo = '';
            var Work = '';
            json = 'inq:' + $('#TxtInqComplete').val() + ",";
            $.ajax({
                type: "POST",
                url: This + "/GetComplete",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '';
                    if (response.d.length > 0) {
                        for (i = 0; i < response.d.length; i++) {
                            Subject = response.d[i]["Subject"];
                            DocumentDate = response.d[i]["DocumentDate"];
                            DocumentBy = response.d[i]["DocumentBy"];
                            Status = response.d[i]["Status"];
                            FlowInfo = response.d[i]["FlowInfo"];
                            Work = response.d[i]["Work"];
                            html += "<tr>";
                            html += "<td>" + Subject + "</td>";
                            html += "<td>" + DocumentBy + "</td>";
                            html += "<td>" + DocumentDate + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td>" + FlowInfo + "</td>";
                            html += "<td>" + Work + "</td>";
                            html += "</tr>";
                        }
                    }
                    else {
                        html += "<tr>";
                        html += "<td colspan='6' style='text-align:center'><span style='Color:red;'>ไม่พบข้อมูล</span></td>";
                        html += "</tr>";
                    }
                    $('#DivCompleteBody').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        function GetCancel() {
            $('#Divinvolve').hide();
            $('#Divcomplete').hide();
            $('#Divcancel').show();
            $('#Divpending').hide();
            var json = '';
            var i = 0;
            var html = '';
            var Subject
            var DocumentDate = '';
            var DocumentBy = '';
            var Status = '';
            var FlowInfo = '';
            var Work = '';
            json = 'inq:' + $('#TxtInqCancel').val() + ",";
            $.ajax({
                type: "POST",
                url: This + "/GetCancel",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    html = '';
                    if (response.d.length > 0) {
                        for (i = 0; i < response.d.length; i++) {
                            Subject = response.d[i]["Subject"];
                            DocumentDate = response.d[i]["DocumentDate"];
                            DocumentBy = response.d[i]["DocumentBy"];
                            Status = response.d[i]["Status"];
                            FlowInfo = response.d[i]["FlowInfo"];
                            Work = response.d[i]["Work"];
                            html += "<tr>";
                            html += "<td>" + Subject + "</td>";
                            html += "<td>" + DocumentBy + "</td>";
                            html += "<td>" + DocumentDate + "</td>";
                            html += "<td>" + Status + "</td>";
                            html += "<td>" + FlowInfo + "</td>";
                            html += "<td>" + Work + "</td>";
                            html += "</tr>";
                        }
                    }
                    else {
                        html += "<tr>";
                        html += "<td colspan='6' style='text-align:center'><span style='Color:red;'>ไม่พบข้อมูล</span></td>";
                        html += "</tr>";
                    }
                    $('#DivCancelBody').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        $(function () {
            if ($('#ContentPlaceHolder1_HdAction').val() == 'pending') {
                GetPending();
            }
            else if ($('#ContentPlaceHolder1_HdAction').val() == 'involve') {
                GetInvolve();
            }
            else if ($('#ContentPlaceHolder1_HdAction').val() == 'complete') {
                GetComplete();
            }
            else if ($('#ContentPlaceHolder1_HdAction').val() == 'cancel') {
                GetCancel();
            }
            $('#CmdPending').on('click', function () {
                GetPending();
            });
            $('#CmdInvolve').on('click', function () {
                GetInvolve();
            });
            $('#CmdComplete').on('click', function () {
                GetComplete();
            });
            $('#CmdCancel').on('click', function () {
                GetCancel();
            });
        });
    </script>
    <input type="hidden" id="HdAction" runat="server" value="" />
    <div class="container-fluid" style="background-color: black;">
        <div class="row">

            <div class="card" id="Divpending" style="display: none; width: 100%;">
                <div class="card-header">
                    <div class="row">
                        <div class="col-8">
                            <h3 class="panel-title" style="font-family: Kanit;">Pending E-Form</h3>
                        </div>
                        <div class="col-4" style="text-align: right;">
                            <div class="input-group mb-3">
                                <input id="TxtInqPending" type="text" class="form-control" placeholder="Search..." style="border-radius: 1px; font-size: 0.8em;">
                                <div class="input-group-append">
                                    <button id="CmdPending" class="btn btn-info" type="button">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </div>
                            </div>


                        </div>
                        <div class="card-body">
                            <table class="table table-hover">
                                <thead class="bg-info" style="color: white; height: 100px;">
                                    <tr>
                                        <th>หัวเรื่อง</th>
                                        <th>สร้างโดย</th>
                                        <th>วันที่สร้าง (dd/mmm/yyyy)</th>
                                        <th>สถานะเอกสาร</th>
                                        <th>รายละเอียดดำเนินการ</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody id="DivPendingBody" style="font-size: 1em;">
                                </tbody>

                            </table>
                        </div>
                    </div>





                </div>
                <div class="alert alert-danger" id="Divalert" style="display: none; width: 400px; height: 50px; position: fixed; bottom: 0; right: 20px; padding-right: -20px;"></div>
                <!-- Modal -->
                <div class="modal fade" id="DivProgress" tabindex="-1" role="dialog"
                    aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-body" style="text-align: center; padding-top: 50px; padding-bottom: 50px;">
                                <img src="Image/ajax-loader.gif" />
                            </div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->

                </div>
                <!-- /.modal -->
            </div>
            <div class="card" id="Divinvolve" style="display: none; width: 100%;">
                <div class="card-header">
                    <div class="row">
                        <div class="col-8">
                            <h3 class="panel-title" style="font-family: Kanit;">Inprocess E-Form</h3>
                        </div>
                        <div class="col-4" style="text-align: right;">
                            <div class="input-group mb-3">
                                <input id="TxtInqInvolve" type="text" class="form-control" placeholder="Search..." style="border-radius: 1px; font-size: 0.8em;">
                                <div class="input-group-append">
                                    <button id="CmdInvolve" class="btn btn-info" type="button">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </div>
                            </div>


                        </div>
                        <div class="card-body">
                            <table class="table table-hover">
                                <thead class="bg-info" style="color: white; height: 100px;">
                                    <tr>
                                        <th>หัวเรื่อง</th>
                                        <th>สร้างโดย</th>
                                        <th>วันที่สร้าง (dd/mmm/yyyy)</th>
                                        <th>สถานะเอกสาร</th>
                                        <th>รายละเอียดดำเนินการ</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody id="DivInvolveBody" style="font-size: 1em;">
                                </tbody>

                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card" id="Divcomplete" style="display: none; width: 100%;">
                <div class="card-header">
                    <div class="row">
                        <div class="col-8">
                            <h3 class="panel-title" style="font-family: Kanit;">Complete E-Form</h3>
                        </div>
                        <div class="col-4" style="text-align: right;">
                            <div class="input-group mb-3">
                                <input id="TxtInqComplete" type="text" class="form-control" placeholder="Search..." style="border-radius: 1px; font-size: 0.8em;">
                                <div class="input-group-append">
                                    <button id="CmdComplete" class="btn btn-info" type="button">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </div>
                            </div>


                        </div>
                        <div class="card-body">
                            <table class="table table-hover">
                                <thead class="bg-info" style="color: white; height: 100px;">
                                    <tr>
                                        <th>หัวเรื่อง</th>
                                        <th>สร้างโดย</th>
                                        <th>วันที่สร้าง (dd/mmm/yyyy)</th>
                                        <th>สถานะเอกสาร</th>
                                        <th>รายละเอียดดำเนินการ</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody id="DivCompleteBody" style="font-size: 1em;">
                                </tbody>

                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card" id="Divcancel" style="display: none; width: 100%;">
                <div class="card-header">
                    <div class="row">
                        <div class="col-8">
                            <h3 class="panel-title" style="font-family: Kanit;">Cancel E-Form</h3>
                        </div>
                        <div class="col-4" style="text-align: right;">
                            <div class="input-group mb-3">
                                <input id="TxtInqCancel" type="text" class="form-control" placeholder="Search..." style="border-radius: 1px; font-size: 0.8em;">
                                <div class="input-group-append">
                                    <button id="CmdCancel" class="btn btn-info" type="button">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </div>
                            </div>


                        </div>
                        <div class="card-body">
                            <table class="table table-hover">
                                <thead class="bg-info" style="color: white; height: 100px;">
                                    <tr>
                                        <th>หัวเรื่อง</th>
                                        <th>สร้างโดย</th>
                                        <th>วันที่สร้าง (dd/mmm/yyyy)</th>
                                        <th>สถานะเอกสาร</th>
                                        <th>รายละเอียดดำเนินการ</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </thead>

                                <tbody id="DivCancelBody" style="font-size: 1em;">
                                </tbody>

                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="DivFlowInfo" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" >
            <div class="modal-content">
                <div class="modal-body" style="min-height: 500px;">
                    <div id="DivNodeinFlow"></div>
                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close
                    </button>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>
