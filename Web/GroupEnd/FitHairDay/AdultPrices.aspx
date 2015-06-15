<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdultPrices.aspx.cs" Inherits="Web.GroupEnd.FitHairDay.AdultPrices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title><link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top:10px;">
          <tr class="odd">
            <th align="center">门市价</th>
            <th align="center">同行价</th>
          </tr>
          <tr class="even">
            <td align="center">成人/儿童</td>
            <td align="center">成人/儿童</td>
          </tr>
          <tr class="odd">
            <td align="center">￥<asp:Label ID="MsCrprices" runat="server"></asp:Label>/￥<asp:Label ID="MsetPrices" runat="server"></asp:Label></td>
            <td align="center">￥<asp:Label ID="ThCrprices" runat="server"></asp:Label>/￥<asp:Label ID="Therprices" runat="server"></asp:Label></td>
          </tr>
      </table>
    </div>
    </form>
</body>
</html>
