<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operatorlist.aspx.cs" Inherits="Web.GroupEnd.FitHairDay.Operatorlist" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>    
    <link rel="Stylesheet" type="text/css" href="/css/sytle.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px;">
            <tr>
                <th width="19%" height="30" align="right" class="odd">
                    联系人：
                </th>
                <td width="4%">
                    &nbsp;
                </td>
                <td width="77%">
                    <asp:Label ID="Lab_Contact" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th width="19%" height="30" align="right" class="even">
                    联系电话：
                </th>
                <td>
                    &nbsp;
                </td>
                <td>
                   <asp:Label ID="Lab_Mobile" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th width="19%" height="30" align="right" class="odd">
                    传真：
                </th>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Lab_Fox" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th width="19%" height="30" align="right" class="even">
                    手机：
                </th>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Lab_Phone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th width="19%" height="30" align="right" class="odd">
                    QQ：
                </th>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Lab_QQ" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th width="19%" height="30" align="right" class="even">
                    Email：
                </th>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Lab_Email" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
