<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signin.aspx.cs" Inherits="ERP.LHDesign2020.Signin" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <script src="js/engine.js"></script>

    <link href="font-awesome-4.7.0/css/font-awesome.css" rel="stylesheet" />
    <link href="font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />
    <script>

        function Doforgotpasswordsendpass() {
            var json = '';
            json += 'Hdforgotpassworduserid : ' + $('#Hdforgotpassworduserid').val() + '|';
            $.ajax({
                type: "POST",
                url: "Signin.aspx/Doforgotpasswordsendpass",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    $('#Divforgotpassword').modal('hide');
                    Loading();
                },
                success: function (response) {
                    res = response.d;
                    if (res['Err'] != null) {
                        $('#Divforgotpassworderror').show();
                        $("#Divforgotpassworderror").html(res['Err']);
                        $('#Cmdforgotpasswordsendlink').attr('disabled', true);
                        Unloading();
                        $('#Divforgotpassword').modal('show');
                        return;
                    }
                    Msgbox('ระบบได้ส่ง Link เปลี่ยนรหัสผ่านไปที่ E-Mail เรียบร้อยแล้ว');
                    Unloading();
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Validateforgotpassword() {
           
     
            var json = '';
            $('#Txtforgotpasswordemail').removeClass('form-control is-invalid');
            $('#Txtforgotpasswordemail').addClass('form-control');
            if ($('#Txtforgotpasswordemail').val() == '') {
                $('#Txtforgotpasswordemail').addClass('form-control is-invalid');
                return;
            }
            json += 'Txtforgotpasswordemail :' + $('#Txtforgotpasswordemail').val() + '|'
            $.ajax({
                type: "POST",
                url: "Signin.aspx/Validateforgotpassword",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    res = response.d;
                    if (res['Err'] != null) {
                        $('#Divforgotprofile').hide();
                        $('#Divforgotpassworderror').show();
                        $("#Divforgotpassworderror").html(res['Err']);
                        $('#Cmdforgotpasswordsendlink').attr('disabled', true);
                        return;
                    }
                    $('#Divforgotprofile').show();
                    $('#Cmdforgotpasswordsendlink').attr('disabled', false);
                    $('#Hdforgotpassworduserid').val(res['userid']);
                    $('#Imgforgotpasswordavartar').attr('src', res['avartarurl']);
                    $('#Spforgotpasswordtel').html('Tel : ' + res['tel']);
                    $('#Spforgotpasswordfullname').html(res['firstnameth'] + ' ' + res['lastnameth']);
                },
                async: true,
                error: function (er) {

                }
            });



        }
        function Signin() {
            var json = '';
            $('#Diverror').hide();

            $('#Txtusername').removeClass('form-control is-invalid');
            $('#Txtpassword').removeClass('form-control is-invalid');
            $('#Txtusername').addClass('form-control');
            $('#Txtpassword').addClass('form-control');
            if ($('#Txtusername').val() == '') {
                $('#Txtusername').addClass('form-control is-invalid');
                return;
            }
            if ($('#Txtpassword').val() == '') {
                $('#Txtpassword').addClass('form-control is-invalid');
                return;
            }
            json += 'Txtusername :' + $('#Txtusername').val() + '|'
            json += 'Txtpassword :' + $('#Txtpassword').val() + '|'
            $.ajax({
                type: "POST",
                url: "Signin.aspx/doSignin",
                data: "{'json' :'" + json + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    res = response.d;
                    if (res != "") {
                        $('#Diverror').show();
                        $("#Diverror").html(res);

                        return;
                    }
                    window.location.href = 'Page/index.aspx';
                },
                async: true,
                error: function (er) {

                }
            });
        }
        function Forgotpassword() {
            $('#Divforgotprofile').hide();
            $('#Divforgotpassword').modal('show');
            $('#Cmdforgotpasswordsendlink').attr('disabled', true);
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
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>ERP - Luxury Home Design</title>
    <!-- External CSS -->


    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet" />

    <!-- Favicon icon -->
    <link rel="icon" type="image/png" sizes="32x32" href="Img/Logo.png">

    <!-- Custom Stylesheet -->
    <link href="css/login-one.css" rel="stylesheet" />

</head>

<body>

    <!-- Loader -->
    <div class="loader">
        <div class="loader_div"></div>
    </div>

    <!-- Login page -->

    <div class="login_wrapper" style="background-image:url('Img/login-bg2.jpg')">
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
                            
                                <img src="Img/logo.png" style="width: 120px;" /></div>
                            <ul class="social-list clearfix">
                                <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                                <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                                <li><a href="#"><i class="fa fa-google-plus"></i></a></li>
                                <li><a href="#"><i class="fa fa-linkedin"></i></a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="col-lg-7 col-sm-12">
                        <div class="login-inner-form">
                            <div class="details">
                                <h3>Login to <span>your account</span></h3>

                                <div class="form-group" style="text-align: left;">
                                    <input type="text" id="Txtusername" autocomplete="off" class="form-control" placeholder="Username">
                                    <div class="invalid-feedback">
                                        โปรดระบุ Username
                                    </div>
                                </div>
                                <div class="form-group" style="text-align: left;">
                                    <input type="password" id="Txtpassword" autocomplete="off" class="input-text" placeholder="Password">
                                    <div class="invalid-feedback">
                                        โปรดระบุ Password
                                    </div>
                                </div>
                                <div class="checkbox clearfix">
                                    <div class="form-check checkbox-theme">
                                    </div>
                                    <a onclick="Forgotpassword();" style="color: deepskyblue; text-decoration: underline; cursor: pointer;">Forgot Password</a>
                                </div>

                                <div style="text-align: left; color: red; display: none; margin-top: 5px; margin-bottom: 5px" id="Diverror">
                                </div>

                                <div class="form-group">

                                    <button type="button" onclick="Signin();" class="btn-md btn-theme btn-block">Login</button>
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
    <div class="modal fade bd-example-modal-lg" id="Divforgotpassword" tabindex="-1" role="dialog" style="z-index: 99999;" aria-labelledby="myModalLabel">
        <div class="modal-dialog modal-ku" role="document">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #82c2f3;">
                    <span style="color: white; font-size: 14px;">ERP - Luxury Home Design </span>
                </div>
                <div class="modal-body">
                    <div class="container" style="font-size: 0.9em; margin-top: 20px;">
                        <div class="row">
                            <div class="col-3" style="text-align: right;">
                                E-mail &nbsp;<span style="color: red;">*</span>
                            </div>
                            <div class="col-9">

                                <div class="input-group mb-3">
                                    <input style="font-size: 0.9em;" type="email" id="Txtforgotpasswordemail" autocomplete="off" class="form-control" placeholder="E-mail">

                                    <div class="input-group-append">
                                        <button type="button" onclick="Validateforgotpassword();" class="btn btn-info" style="border-radius: 2px; font-size: 0.9em; width: 100%;"><i class="fa fa-search" aria-hidden="true"></i></button>
                                    </div>
                                </div>
                                <div class="invalid-feedback">
                                    โปรดระบุ E-Mail
                                </div>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                                <div style="text-align: left; color: red; display: none; margin-top: 5px; margin-bottom: 5px" id="Divforgotpassworderror">
                                </div>
                            </div>
                        </div>
                        <div class="row" id="Divforgotprofile" style="display: none;">
                            <div class="col-12" style="text-align: center; margin-top: 10px;">
                                <img src="Img/profile.png" style="width: 120px;" id="Imgforgotpasswordavartar" class="img-thumbnail" />
                            </div>
                            <div class="col-12" style="text-align: center; margin-top: 10px;">
                                <input type="hidden" id="Hdforgotpassworduserid"/>
                                <span id="Spforgotpasswordfullname"></span>
                            </div>
                            <div class="col-12" style="text-align: center; margin-top: 10px;">
                                <span id="Spforgotpasswordtel"></span>
                            </div>
                        </div>

                    </div>
                </div>
                <div style="min-height: 50px; margin-top: 10%; font-size: 14px;"></div>
                <div class="modal-footer">
                    <button type="button" onclick="Doforgotpasswordsendpass();" class="btn btn-primary" style="border-radius: 2px; font-size: 0.8em;"  id="Cmdforgotpasswordsendlink">ส่ง Link เปลี่ยนรหัสผ่าน</button>
                    <button type="button" class="btn btn-secondary" style="border-radius: 2px; font-size: 0.8em;" data-dismiss="modal">ปิดหน้าต่างนี้</button>
                </div>
            </div>
        </div>
    </div>
    <!-- External JS libraries -->

    <script src="Js/jquery-2.2.0.min.js"></script>
    <script src="Js/popper.min.js"></script>
    <script src="Js/bootstrap.min.js"></script>
  
    <!-- Custom JS Script -->
    <script type="text/javascript">
        $(window).load(function () {
            $(".loader").fadeOut("slow");
            $(document).on('focus', ':input', function () {
                $(this).attr('autocomplete', 'off');
            });
        });
    </script>
     
</body>
</html>
