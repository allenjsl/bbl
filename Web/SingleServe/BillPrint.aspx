<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillPrint.aspx.cs" Inherits="Web.SingleServe.BillPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>单据打印</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="200" border="0" align="center" cellpadding="0" cellspacing="0" class="printlist">
  <tr>
    <td height="25"><a href="#" class="listIn">订房确认单</a></td>
  </tr>
  <tr>
    <td height="25"><a href="#">订票确认单</a></td>
  </tr>
  <tr>
    <td height="25"><a href="#">门票确认单</a></td>
  </tr>
  <tr>
    <td height="25"><a href="#">保险确认单</a></td>
  </tr>
  <tr>
    <td height="25"><a href="#">签证确认单</a></td>
  </tr>
  <tr>
    <td height="25"><a href="#">租车确认单</a></td>
  </tr>
</table>
    </div>
    </form>
</body>
</html>
