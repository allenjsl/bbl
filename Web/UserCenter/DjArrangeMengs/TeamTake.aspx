<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamTake.aspx.cs" Inherits="Web.UserCenter.DjArrangeMengs.TeamTake" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <style type="text/css">
        .tbl td
        {
            border: solid 1px #fff;
        }
        .tbl
        {
            border-collapse: collapse;
            border: 0;
            width: 100%;
            text-align: center;
        }
        .style1
        {
            width: 124px;
        }
        .style2
        {
            width: 234px;
        }
        .style3
        {
            width: 111px;
        }
        .style4
        {
            width: 132px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 910px; margin: 0 auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0">
            <tbody>
                <tr class="odd">
                    <th nowrap="nowrap" width="10%" align="center">
                        地接社
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        接团时间
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        送团时间
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        团款
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        返利
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        结算费用
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        支付方式
                    </th>
                    <th nowrap="nowrap" width="20%" align="center">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <%#Container.ItemIndex%2==0?"<tr class=\"even\">":"<tr>" %>
                        <td align="center">
                            <%#Eval("LocalTravelAgency")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDateTime(Eval("ReceiveTime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDateTime(Eval("DeliverTime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Fee")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Commission")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Settlement")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Eval("PayType")%>
                        </td>
                        <td align="center">
                            <a href="/UserCenter/DjArrangeMengs/TeamTake.aspx?id=<%#Eval("ID").ToString().Trim() %>&planId=<%=hidePlanId.Value %>&planType=<%=hidePlanType.Value %>&iframeid=<%=Request.QueryString["iframeid"] %>">
                                查看</a>
                        </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td height="30" align="right" class="pageup" colspan="10">
                        <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="900">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center" class="style1">
                        地接社名称：
                    </th>
                    <td align="center" class="style2">
                        <asp:Literal ID="LitDjName" runat="server"></asp:Literal>
                    </td>
                    <th align="center" class="style3"> 
                        联系人：
                    </th>
                    <td align="center" class="style4">
                        <asp:Literal ID="LitName" runat="server"></asp:Literal>
                    </td>
                    <th align="center" width="132">
                        联系电话：
                    </th>
                    <td align="center" width="247">
                        <asp:Literal ID="LitPhone" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center" class="style1">
                        接团时间：
                    </th>
                    <td align="left" colspan="5">
                        <table cellspacing="0" cellpadding="0" border="0" width="70%">
                            <tbody>
                                <tr>
                                    <td align="center" width="46%">
                                        <asp:Literal ID="LitBeginTime" runat="server"></asp:Literal>
                                    </td>
                                    <th align="center" width="27%">
                                        送团时间：
                                    </th>
                                    <td align="left" width="27%">
                                        <asp:Literal ID="LitEndTime" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center" colspan="6">
                        <asp:Panel ID="pnlJieDai" runat="server">
                            <table style="width: 100%;" id="tblJieDai">
                                <tr>
                                    <td width="85">
                                        接待行程：
                                    </td>
                                    <td align="left">
                                        <asp:Repeater ID="rptJieDai" runat="server">
                                            <ItemTemplate>
                                                <div style="width: 800px;">
                                                    <input type="checkbox" disabled="disabled" />
                                                    <label for="cbxTravel_<%#Container.ItemIndex %>">
                                                        <%#Eval("TraveDate")%></label>
                                                    &nbsp;&nbsp;&nbsp;<%#Eval("PlanContent") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr class="even">
                    <th align="center" class="style1">
                        价格组成
                    </th>
                    <td align="center" colspan="5">
                        <table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" align="center"
                            width="95%" style="margin: 10px 0pt;">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e3f1fc" align="center">
                                        <asp:Panel ID="pnlProjectB" runat="server">
                                            <table class="tbl" id="tblProject">
                                                <tr>
                                                    <td height="30" bgcolor="#e3f1fc" align="center" width="110px">
                                                        项目
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center" width="520px">
                                                        具体要求
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center" width="80px">
                                                        价格项目
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center">
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptProjectB" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="107px">
                                                                <%#Eval("PriceTpyeId")%>
                                                            </td>
                                                            <td width="524px" align="center">
                                                                <%#Eval("Remark")%>
                                                            </td>
                                                            <td width="77px">
                                                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Price")).ToString("0.00")) %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" bgcolor="#e3f1fc" align="center">
                                        <asp:Panel ID="pnlProjectA" runat="server">
                                            <table style="width: 100%;" class="tbl" id="tblPricePro">
                                                <tr>
                                                    <th align="center" bgcolor="#e3f1fc" height="25">
                                                        价格项目
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc" width="405px">
                                                        备注
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc">
                                                        单人价格
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc">
                                                        人数
                                                    </th>
                                                </tr>
                                                <asp:Repeater ID="rptProjectA" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="107px">
                                                                <%#Eval("Title") %>
                                                            </td>
                                                            <td width="403px">
                                                                <%#Eval("Remark")%>
                                                            </td>
                                                            <td width="117px">
                                                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Price")).ToString("0.00")) %>
                                                            </td>
                                                            <td width="78px">
                                                                <%#Eval("PeopleCount")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd" id="tr_Price">
                    <th height="25" align="center" class="style1">
                        &nbsp;
                    </th>
                    <th align="right" class="style2">
                        团款：
                    </th>
                    <td align="left" colspan="2">
                        <asp:Literal ID="litTeamPrice" runat="server"></asp:Literal>元
                    </td>
                    <th align="right" width="132">
                        返款：
                    </th>
                    <td align="left" width="247">
                        <asp:Literal ID="litBackPrice" runat="server"></asp:Literal>元
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center" class="style1">
                        &nbsp;
                    </th>
                    <th align="right" class="style2">
                        结算费用：
                    </th>
                    <td align="left" colspan="2">
                        <asp:Literal ID="litSettlePrice" runat="server"></asp:Literal>元
                    </td>
                    <th align="right" width="132">
                        支付方式：
                    </th>
                    <td align="left" width="247">
                        <asp:Literal ID="litPayType" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="60" align="center" class="style1">
                        地接社需知：
                    </th>
                    <td align="left" colspan="5">
                        <asp:Literal ID="litNotes" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr class="even">
                    <th height="60" align="center" class="style1">
                        备注：
                    </th>
                    <td align="left" colspan="5">
                        <asp:Literal ID="litRemarks" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="6">
                        <asp:Panel ID="pnlEdit" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" width="320" text-align="center"
                                style="margin-left: 200px;">
                                <tbody>
                                    <tr>
                                        <td class="tjbtn02" align="center">
                                            <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();"
                                                href="javascript:void(0);">关闭</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideId" runat="server" />
    <asp:HiddenField ID="hidePlanId" runat="server" />
    <asp:HiddenField ID="hideDiJieID" runat="server" />
    <asp:HiddenField ID="hidePlanType" runat="server" Value="" />
    <asp:HiddenField ID="hideCommission" runat="server" Value="" />
    <asp:HiddenField ID="hideXingCheng" runat="server" Value="" />
    </form>

    <script type="text/javascript">
        $(function() {
            if ("<%=SiteUserInfo.LocalAgencyCompanyInfo.CompanyId %>" == "<%=this.hideDiJieID.Value %>") {
                var days = $("#<%=hideXingCheng.ClientID %>").val();
                var dayArray = days.split(",");
                if (dayArray.length > 0) {
                    for (var i = 0; i < dayArray.length; i++) {
                        if (dayArray[i] != "") {
                            if ($("#<%=pnlJieDai.ClientID %>").find("input[type='checkbox']").eq(parseInt(dayArray[i]) - 1).attr("type") != null) {
                                $("#<%=pnlJieDai.ClientID %>").find("input[type='checkbox']").eq(parseInt(dayArray[i]) - 1).attr("checked", "checked");
                            }
                        }
                    }
                }
            } else {
                $("#<%=pnlJieDai.ClientID %>").find("input[type='checkbox']").css("display", "none");
            }
        })
    </script>

</body>
</html>
