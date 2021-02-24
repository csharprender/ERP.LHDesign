function Msgbox(Msg) {
    $("#Divalert").html('<img src="/../Image/Prevent.png" style="width:20px;height:20px;" />&nbsp;' + Msg);
    $("#Divalert").alert();
    $("#Divalert").fadeTo(2000, 500).slideUp(500, function () {
        $("#Divalert").slideUp(500);
    });
    //alert(Msg);
    //$('#DivMessage').html(Msg);
    //$('#DivMsgBox').modal('show');
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function Unloading() {
    $('#pleaseWaitDialog').modal('hide');
}
function Loading() {
    if (document.querySelector("#pleaseWaitDialog") == null) {
        var modalLoading = '<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false" role="dialog" style="z-index: 10000000 !important;">\
            <div class="modal-dialog">\
                <div class="modal-content">\
                    <div class="modal-header">\
                        <h4 class="modal-title">Please wait...</h4>\
                    </div>\
                    <div class="modal-body">\
                        <div class="progress">\
                          <div class="progress-bar progress-bar-success progress-bar-striped active" role="progressbar"\
                          aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width:100%; height: 40px">\
                          </div>\
                        </div>\
                    </div>\
                </div>\
            </div>\
          </div>';
        $(document.body).append(modalLoading);
    }

    $("#pleaseWaitDialog").modal("show");
}

function Getparamvalue(param) {
    var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < url.length; i++) {
        var urlparam = url[i].split('=');
        if (urlparam[0] == param) {
            return urlparam[1];
        }
    }
}