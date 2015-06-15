<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JingDianMingXi.aspx.cs" Inherits="Web.Shop.T1.JingDianMingXi" Title="景点明细" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>景点明细</title>
    <style type="text/css">
    .moduel_wrap1_profile table{border-collapse: collapse;width: 100%;}
    .moduel_wrap1_profile table, .moduel_wrap1_profile table tr, .moduel_wrap1_profile table tr td{border: 1px solid #ccc;padding: 5px;font-size:12px;font-weight:bold;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="moduel_wrap moduel_width1">
        <div class="moduel_wrap1 moduel_wrap1_profile">            
            <table width="550" border="1" cellpadding="1" cellspacing="1">
                <tr>
                    <td align="right">
                        景点名称
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="litSpotName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        地址
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="litAddress" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        所在城市
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="LitProc" runat="server"></asp:Literal>_<asp:Literal ID="LitCity" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        景点图片
                    </td>
                    <td colspan="2">
                        <asp:Repeater ID="rpt" runat="server">
                            <ItemTemplate>
                                <img src="<%# Eval("PicPath") %>" width="90px" height="70px" alt="" />&nbsp
                            </ItemTemplate>
                        </asp:Repeater>
                        <span style="font-weight: normal"><asp:Literal ID="litmsg" runat="server"></asp:Literal></span>
                    </td>
                </tr>
                <tr>
                    <td width="111" align="right">
                        景点简介
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="litTourGuide" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td width="111" align="right">
                        散客价
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="litTravelerPrice" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td width="111" align="right">
                        团队价
                    </td>
                    <td colspan="2" style="font-weight: normal">
                        <asp:Literal ID="litTeamprices" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
