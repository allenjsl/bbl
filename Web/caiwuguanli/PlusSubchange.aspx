<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlusSubchange.aspx.cs" Inherits="Web.caiwuguanli.PlusSubchange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
<link type="text/css" rel="stylesheet" href="../css/sytle.css">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr  class="even">
                <td width="100px" align="right">
                    增加费用：</td>
                <td>
                    <asp:Literal ID="lt_add" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr  class="odd">
                <td align="right">
                    减少费用：</td>
                <td>
                    <asp:Literal ID="lt_sub" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="even">
                <td align="right">
                    备注：</td>
                <td>
                    <asp:Literal ID="lt_remark" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
