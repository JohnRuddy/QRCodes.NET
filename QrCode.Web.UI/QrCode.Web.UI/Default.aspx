<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QrCode.Web.UI._Default" %>

<!doctype html>
<html ng-app>
<head runat="server">
    <title>Instant QR Codes</title>
    <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <label>
            QrCode: </label>
        <input type="text" ng-model="yourName" placeholder="Enter a name here">
        <hr>
        <img src="qr.code?{{yourName}}" alt="QrCode Image" runat="server" id="QrCodeImage"/><br/>

    </div>
    </form>
</body>
</html>
