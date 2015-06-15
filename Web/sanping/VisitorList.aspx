<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitorList.aspx.cs" Inherits="Web.sanping.VisitorList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>机票申请游客名单</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="1" cellpadding="0" border="0">
            <tbody>
                <tr class="odd">
                    <th align="center">
                        序号
                    </th>
                    <th height="25" align="center">
                        姓名
                    </th>
                    <th align="center">
                        证件类型
                    </th>
                    <th align="center">
                        证件号码
                    </th>
                </tr>
                <cc2:CustomRepeater runat="server" ID="rpt_list">
                    <ItemTemplate>
                        <tr class="even">
                            <td align="center">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td height="25" align="center">
                                <%#Eval("VisitorName") %>
                            </td>
                            <td align="center">
                                <%#Eval("CradType").ToString() %>
                            </td>
                            <td align="center">
                                <%#Eval("CradNumber") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </cc2:CustomRepeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
