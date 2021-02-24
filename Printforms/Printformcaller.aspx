<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Printformcaller.aspx.cs" Inherits="IAdvertise.Printform" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Page/Css/FMSPRO.css" rel="stylesheet" />
    <title></title>
</head>
<body style="background-color:#373535">
    <form id="form1" runat="server">
        <center>
       <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"></CR:CrystalReportViewer>
        </center>
        
    </form>
    </body>
</html>