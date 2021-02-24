<%@ Page Title="" Language="C#" MasterPageFile="~/Page/ERP.Master" AutoEventWireup="true" CodeBehind="Eforms.aspx.cs" Inherits="ERP.LHDesign2020.Page.EDWF.Eforms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function CallBackRefresh(x) {
            Msgbox('ส่งเอกสารเรียบร้อยแล้ว')
        }
        function OpenDoc(FormURL, NodeName) {
            
         
            w = 1200;
            h = 768;
            var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
            var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
            var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
            var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
            var left = ((width / 2) - (w / 2)) + dualScreenLeft;
            var top = ((height / 2) - (h / 2)) + dualScreenTop;
            popup = window.open(FormURL, 'popup', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            //$('#ImgFlow').attr('src', '../../Image/Form/1.jpg')
            //$('#CmdNewDocument').on('click', function () {
            //    var json = 'NodeName:' + NodeName + ',';
            //    var i = 0;
            //    var out;
            //    var html = '';
            //    $.ajax({
            //        type: "POST",
            //        url: This + "/ClearSession",
            //        data: "{'json' :'" + json + "'}",
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        success: function (response) {
            //            popup = window.open(FormURL, 'popup', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            //        },
            //        async: false,
            //        error: function (er) {
            //            Msgbox(er.status);
            //        }
            //    });



            //});
        }
        function Loaddocuments() {
            var json = '';
            var i = 0;
            var res;
            var html = '';
            $.ajax({
                type: "POST",
                url: "EForms.aspx/Loaddocuments",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d['Err'] != '') {
                        html = response.d["Err"];
                    }
                    else {
                        res = response.d['Documents'];
                        for (i = 0; i < res.length; i++) {
                            html += '<div class=\"col-lg-2 col-md-2  col-sm-2 col-sm-4\" style="cursor:pointer;"  onClick=OpenDoc(\'' + res[i]['FormURL'] + '\',\'' + res[i]['NodeName'] + '\');> ';
                            html += '<div style="text-align:center;"><img style=\"text-align:center;width:120px;\" src=\"../../Image/Document.png\"  /></div>';
                            html += '<br />';
                            html += '<div style="margin-top:10px;text-align:center;" ><span class=\"document-label\">' + res[i]['FormNameTH'] + '</span></div>';
                            html += '</div>';
                        }
                        if (html == '') {
                            html += '<div style="color:red;">ไม่พบเอกสาร E-Form </div>';
                        }
                    }
                    $('#DivDocuments').html(html);
                },
                async: true,
                error: function (er) {
                    Msgbox(er.status);
                }
            });
        }
        $(function () {
            Loaddocuments();
        })
    </script>
      <div>
        <div class="row">
            <div class="card bg-light mb-3" style="width: 100%;margin-top:10px;margin-left:10px; margin-right: 10px;">
                <div class="card-header">
                    <h3 class="panel-title" style="font-size:1em;">สร้างเอกสารใหม่</h3>
                </div>
                <div class="card-body">
                    <div class="row" style="padding-left: 20px; padding-top: 10px; padding-bottom: 10px; min-height: 150px;" id="DivDocuments">
                    </div>
                </div>
            </div>
        
        </div>
          <div class="row">
            <div class="col-12" style="width: 100%; margin-right: 5px;">
                
               <img id="ImgFlow" src="../../Image/Form/Default.jpg" style="width: 100%; height: 100%;" />
            </div>
        
        </div>
     
     
    </div>
      <div class="alert alert-danger" id="Divalert" style="display: none; width: 400px; height: 50px; position: fixed; bottom: 0; right: 20px; padding-right: -20px;"></div>
</asp:Content>
