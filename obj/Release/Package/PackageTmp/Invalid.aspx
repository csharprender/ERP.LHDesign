<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invalid.aspx.cs" Inherits="ERP.LHDesign2020.Invalid" %>


<!DOCTYPE html>
<html lang="en">
<head>
    <script src="js/engine.js"></script>
    <link href="font-awesome-4.7.0/css/font-awesome.css" rel="stylesheet" />
    

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
    <link type="text/css" rel="stylesheet" href="http://uiwebsoft.com/justlog/assets/css/bootstrap.min.css">
    <link type="text/css" rel="stylesheet" href="../assets/fonts/font-awesome/css/font-awesome.min.css">

    <!-- Favicon icon -->
    <link rel="icon" type="image/png" sizes="32x32" href="images/fev.png">

    <!-- Custom Stylesheet -->
    <link type="text/css" rel="stylesheet" href="http://uiwebsoft.com/justlog/login-one/css/login-one.css">




    <script type="text/javascript" src="http://gc.kis.v2.scr.kaspersky-labs.com/FD126C42-EBFA-4E12-B309-BB3FDD723AC1/main.js?attr=i0JMrbGqw4YTKWbZHAR4XFkfkOmBfGpt3_fXODrx2oSSzMFuul2Af31izLrFq8C8ICXlKqaBWXPIBGotyCaOWr0FU2b7SjIRrgEWmpvzwCY" charset="UTF-8"></script>
</head>

<body>

    <!-- Loader -->
    <div class="loader">
        <div class="loader_div"></div>
    </div>

    <!-- Login page -->
    <div class="login_wrapper">
        <div class="container" style="background-color:white;border-radius:8px;padding:100px;box-shadow:inset;">
            <div class="col-md-12 pad-0">
                <div style="text-align:center;color:red;font-size:1em;">Invalid path ,Please contact administrator</div>
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
    <script src="http://uiwebsoft.com/justlog/assets/js/jquery-2.2.0.min.js"></script>
    <script src="http://uiwebsoft.com/justlog/assets/js/popper.min.js"></script>
    <script src="http://uiwebsoft.com/justlog/assets/js/bootstrap.min.js"></script>
    <!-- Custom JS Script -->
    <script type="text/javascript">
        $(window).load(function () {
            $(".loader").fadeOut("slow");
        });
    </script>

</body>
</html>
