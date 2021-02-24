<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgotpassword.aspx.cs" Inherits="ERP.LHDesign2020.Forgotpassword" %>

<!DOCTYPE html>
<html lang="en">
<head>
     <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>ERP - Luxury Home Design</title>
    <!-- External CSS -->
    <link type="text/css" rel="stylesheet" href="http://uiwebsoft.com/justlog/assets/css/bootstrap.min.css">
    <link type="text/css" rel="stylesheet" href="../assets/fonts/font-awesome/css/font-awesome.min.css">

    <!-- Favicon icon -->
    <link rel="icon" type="image/png" sizes="32x32" href="images/fev.png">

    <!-- Custom Stylesheet -->
    <link type="text/css" rel="stylesheet" href="http://uiwebsoft.com/justlog/login-one/css/login-one.css">




   
    <script src="js/engine.js"></script>
    <link href="font-awesome-4.7.0/css/font-awesome.css" rel="stylesheet" />
    
    <!-- External JS libraries -->
    <script src="http://uiwebsoft.com/justlog/assets/js/jquery-2.2.0.min.js"></script>
    <script src="http://uiwebsoft.com/justlog/assets/js/popper.min.js"></script>
    <script src="http://uiwebsoft.com/justlog/assets/js/bootstrap.min.js"></script>
    <!-- Custom JS Script -->
    <script type="text/javascript">
        $(window).load(function () {
            $(".loader").fadeOut("slow");
        });
    </script>
    <script>
        $(function () {
            var json = '';
            $.ajax({
                type: "POST",
                url: "Forgotpassword.aspx/ValidateToken",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    res = response.d;
                    if (res != '') {
                        window.location.href = res;
                        return;
                    }
                    json = '';
                    $.ajax({
                        type: "POST",
                        url: "Forgotpassword.aspx/GetResetPasswordInfo",
                        data: "{'json' :'" + json + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            res = response.d;
                            $('#Txtforgotpasswordfullname').val(res);
                        },
                        async: true,
                        error: function (er) {

                        }
                    });

                },
                async: true,
                error: function (er) {

                }
            });
        });
        function doForgotpassword() {
            var json = '';
            $('#Diverror').hide();
            $('#Txtnewpassword').removeClass('form-control is-invalid');
            $('#Txtnewpassword').addClass('form-control');
            $('#Txtconfirmpassword').removeClass('form-control is-invalid');
            $('#Txtconfirmpassword').addClass('form-control');
            if ($('#Txtnewpassword').val() == '') {
                $('#Txtnewpassword').addClass('form-control is-invalid');
                return;
            }
            if ($('#Txtconfirmpassword').val() == '') {
                $('#Txtconfirmpassword').addClass('form-control is-invalid');
                return;
            }
            if ($('#Txtnewpassword').val() != $('#Txtconfirmpassword').val()) {
                $('#Diverror').show();
                $('#Diverror').html('รหัสผ่านและรหัสยืนยันไม่่ตรงกัน');
                return;
            }
            json = '';
            json += 'Txtnewpassword : ' + $('#Txtnewpassword').val() + '|';
            json += 'Txtconfirmpassword : ' + $('#Txtconfirmpassword').val() + '|';
            $.ajax({
                type: "POST",
                url: "Forgotpassword.aspx/doForgotpassword",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    Loading();
                },
                success: function (response) {
                    res = response.d;
                    if (res != '') {
                        $('#Diverror').hide();
                        $('#Diverror').html(res);
                    }
                    $("#Divsigninpanel").show();
                    $("#Divchangepasswordpanel").hide();
                    Unloading();
                },
                async: true,
                error: function (er) {

                }
            });
        }
     
    </script>

    <style>
        .modal-dialog { position: absolute; left: 0; right: 0; top: 0; bottom: 0; margin: auto; width: 500px; height: 300px; }
        .modal-ku {
            width: 550px;
            margin: auto;
        }

        .login_right {
            background-color: #1466a4 !important;
        }

        .login-inner-form .btn-theme {
            background-color: #1466a4 !important;
        }
    </style>
   
</head>

<body>

    <!-- Loader -->
    <div class="loader">
        <div class="loader_div"></div>
    </div>

    <!-- Login page -->
    <div class="login_wrapper">
        <div class="container">
            <div class="col-md-12 pad-0">
                <div class="row login-box-12">
                    <div class="col-lg-5 col-md-12 col-sm-12 px-0">
                        <div class="login_right">
                            <a href="#" class="logo_text">
                                <span>ERP</span>
                            </a>
                            <p>Enterprise Resource Planning</p>
                            <div style="text-align: center">
                                <img src="http://www.lhdesign.co.th/img/Logo.png" style="width: 120px;" /></div>
                            <ul class="social-list clearfix">
                                <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                                <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                                <li><a href="#"><i class="fa fa-google-plus"></i></a></li>
                                <li><a href="#"><i class="fa fa-linkedin"></i></a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-lg-7 col-sm-12">
                        <div class="login-inner-form" id="Divsigninpanel" style="display:none;">
                            <div style="color:green;text-align:center;">คุณได้เปลี่ยนรหัสเรียบร้อยแล้ว</div>
                            <div style="color:green;text-align:center;"><a href="Page/index.aspx">เข้าสู่ระบบได้เลย</a></div>
                        </div>
                        <div class="login-inner-form" id="Divchangepasswordpanel">
                            <div class="details">
                                <h3>Change <span>your Password</span></h3>
                                <div class="form-group" style="text-align: left;">
                                    <input type="text" id="Txtforgotpasswordfullname" autocomplete="off" class="form-control" readonly="readonly">
                                    
                                </div>
                                <div class="form-group" style="text-align: left;">
                                    <input type="text" id="Txtnewpassword" autocomplete="off" class="form-control" placeholder="New Password">
                                    <div class="invalid-feedback">
                                        โปรดระบุ New Password
                                    </div>
                                </div>
                                <div class="form-group" style="text-align: left;">
                                    <input type="text" id="Txtconfirmpassword" autocomplete="off" class="input-text" placeholder="Confirm Password">
                                    <div class="invalid-feedback">
                                        โปรดระบุ Confirm Password
                                    </div>
                                </div>
                               

                                <div style="text-align: left; color: red; display: none; margin-top: 5px; margin-bottom: 5px" id="Diverror">
                                </div>

                                <div class="form-group">

                                    <button type="button" onclick="doForgotpassword();" class="btn-md btn-theme btn-block">เปลี่ยนรหัสผ่าน</button>
                                </div>


                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>
    <!-- /. Login page -->
    <!-- Custom Theme JavaScript -->
    <div class="modal" tabindex="-1" role="dialog" id="DivMsgbox">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" style="text-align: left!important;">
                    <h5 class="modal-title">Electronic document workflow</h5>

                </div>
                <div class="modal-body" id="DivMsgboxmessage" style="text-align: center; min-height: 60px; margin-top: 20px; font-family: Kanit; font-size: 1em;">
                </div>
                <div class="modal-footer">

                    <button type="button" class="btn btn-success" style="font-size: 0.9em;" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
   

</body>
</html>