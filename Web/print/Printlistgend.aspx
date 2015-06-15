<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Printlistgend.aspx.cs" Inherits="Web.print.Printlistgend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                <asp:Repeater ID="rptUPrintUrl" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td height="25">
                                <a target="_blank" href="javascript:void(0)" link="<%#Eval("PrintUrl") %>">
                                    <%#Eval("ShowText").ToString()%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideTourId" runat="server" />

    <script type="text/javascript">
        $(function() {
            $(".printlist a").click(function() {
                var url = $(this).attr("link") + "?tourId=" + $("#<%=hideTourId.ClientID %>").val();
                window.open(url);
                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                return false;
            })
        })
    </script>
    </form>
</body>
</html>
