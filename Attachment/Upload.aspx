<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ERP.LHDesign2020.Attachment.Upload" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE = edge" />
    <meta name="viewport" />
    <title>ERP-Luxury Home Design</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js"></script>
    <link href="https://fonts.googleapis.com/css?family=Merienda+One" rel="stylesheet" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <script lang="ja">
        function CallBack(Ctrl, RunningNo) {
            window.opener.CallBackUpload(Ctrl, RunningNo);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">


        <div>
            <div class="panel panel-default">
                <div class="panel-heading"><small>Upload Files</small></div>
                <div class="panel-body">

                    <!-- Standar Form -->
                    <h4>Select files</h4>
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:FileUpload ID="FileUpd" runat="server" />
                        </div>
                        <asp:Button ID="CmdFileUpload" runat="server" class="btn btn-sm btn-primary" OnClick="CmdFileUpload_Click" Text="Upload" />
                        <asp:HiddenField ID="HdKey" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <!-- /container -->
        <br />
    </form>
</body>
</html>