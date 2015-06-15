<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrincipalShow.aspx.cs"
    Inherits="Web.GroupEnd.JourneyMoney.PrincipalShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="100" height="30" bgcolor="#BDDCF4">
                    负责人
                </th>
                <th width="100" bgcolor="#BDDCF4">
                    电话
                </th>
                <th width="100" bgcolor="#BDDCF4">
                    传真
                </th>
                <th width="100" bgcolor="#BDDCF4">
                    手机
                </th>
                <th width="100" bgcolor="#BDDCF4">
                    QQ
                </th>
                <th width="100" bgcolor="#BDDCF4">
                    E-mail
                </th>
            </tr>
            <tr class="even">
                <td height="30" align="center" bgcolor="#E3F1FC">
                   <%=csModel.ContactName %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%=csModel.ContactTel %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%=csModel.ContactFax %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%=csModel.ContactMobile %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%=csModel.QQ %>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <%=csModel.ContactEmail %>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
