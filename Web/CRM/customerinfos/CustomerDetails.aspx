<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs"
    Inherits="Web.CRM.customerinfos.CustomerDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="cName" name="cName" />
    <div>
        <table width="750px" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="20%" height="30" align="center">
                    单位名称：
                </th>
                <td width="80%" align="left">
                    &nbsp;&nbsp;
                    <asp:Label ID="lblCompanyName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" align="center" colspan="2">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF"
                        id="tableC">
                        <tr class="odd">
                            <th width="5%" align="center">
                                编号
                            </th>
                            <th width="10%" height="30" align="center">
                                姓名
                            </th>
                            <th width="6%" align="center">
                                性别
                            </th>
                            <th width="11%" align="center">
                                手机
                            </th>
                            <th width="11%" align="center">
                                电话
                            </th>
                            <th width="11%" align="center">
                                传真
                            </th>
                            <th width="11%" align="center">
                                QQ
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <tr class="even">
                                    <td align="center">
                                        <%#Container.ItemIndex+1 %>
                                    </td>
                                    <td height="30" align="center">
                                        <%#Eval("Name") %>
                                        </d>
                                        <td align="center">
                                            <%#Eval("Sex") %>
                                        </td>
                                        <td align="center">
                                            <%#Eval("Mobile") %>
                                        </td>
                                        <td align="center">
                                            <%#Eval("Tel") %>
                                        </td>
                                        <td align="center">
                                            <%#Eval("Fax") %>
                                        </td>
                                        <td align="center">
                                            <%#Eval("qq") %>
                                        </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="320px">
            <tr>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
