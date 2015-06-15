<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineInfo.aspx.cs" Inherits="Web.GroupEnd.Orders.LineInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>线路信息</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="200" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="96" height="30" bgcolor="#BDDCF4">
                    负责人：
                </th>
                <td width="154" align="left" bgcolor="#BDDCF4">
                    <asp:Label ID="Lab_Auothor" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center" bgcolor="#E3F1FC">
                    电话：
                </th>
                <td align="left" bgcolor="#E3F1FC">
                    <asp:Label ID="Lab_Phone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    QQ：
                </th>
                <td align="left">
                    <asp:Label ID="Lab_QQ" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
