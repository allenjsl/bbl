<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printlist.aspx.cs" Inherits="Web.print.printlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="200" class="printlist">
            <tbody>
                <asp:Repeater ID="rptUPrintUrl" runat="server" OnItemDataBound="rptUPrintUrlItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td height="25">
                                <asp:Literal ID="ltrLink" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideTourId" runat="server" />
    </form>
</body>
</html>
