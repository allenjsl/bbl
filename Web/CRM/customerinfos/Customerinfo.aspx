<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customerinfo.aspx.cs" Inherits="Web.CRM.customerinfos.Customerinfo" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>联系人信息</title>
    <link rel="Stylesheet" type="text/css" href="/css/sytle.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="14%" bgcolor="#BDDCF4">
                    联系人姓名
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    职位
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    性别
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    电话
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    手机
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    传真
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    QQ
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    生日
                </th>
            </tr>
            <cc1:CustomRepeater ID="CloseNumberList" runat="server" EmptyText="<tr><td colspan='7' align='center'>暂无数据</td></tr>">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center" bgcolor="#E3F1FC">
                            <%# Eval("Name")%>
                        </td>
                        <td bgcolor="#E3F1FC">
                            <%# Eval("Job")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("Sex")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%#Eval("Tel")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("Mobile")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("Fax")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("qq")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("BirthDay", "{0:yyyy-MM-dd}")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td height="30" align="center" bgcolor="#BDDCF4">
                            <%# Eval("Name")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("Job")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("Sex")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%#Eval("Tel")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("Mobile")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("Fax")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("qq")%>
                        </td>
                        <td align="center" bgcolor="#BDDCF4">
                            <%# Eval("BirthDay","{0:yyyy-MM-dd}")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <tr style="display: none">
                <td height="30" colspan="8" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="Close_ExportPageInfo1" runat="server" CurrencyPageCssClass="RedFnt"
                        LinkType="4" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
