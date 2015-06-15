<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroundOperators.aspx.cs"
    Inherits="Web.GroupEnd.Orders.DijieInfo" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>地接社信息</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="125" height="30" bgcolor="#BDDCF4">地接社名称</th>
                <th width="125" bgcolor="#BDDCF4">许可证号</th>
                <th width="125" bgcolor="#BDDCF4">联系人</th>
                <th width="125" bgcolor="#BDDCF4">电话</th>
            </tr>
            <cc1:CustomRepeater ID="DijieList" runat="server" EmptyText="<tr><td colspan='4' align='center'>暂无数据</td></tr>"
            >
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center" bgcolor="#E3F1FC">
                            <%#Eval("Name")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%#Eval("LicenseNo")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%#Eval("Telephone")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%#Eval("ContacterName")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </cc1:CustomRepeater>
        </table>
    </div>
    </form>
</body>
</html>
