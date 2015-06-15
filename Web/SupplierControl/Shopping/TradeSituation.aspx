<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="TradeSituation.aspx.cs" Inherits="Web.SupplierControl.Shopping.TradeSituation" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <div class="tablelist">
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <td width="100" height="30" align="center">
                    团号
                </td>
                <td width="100" align="center">
                    线路名称
                </td>
                <td width="100" align="center">
                    出团日期
                </td>
                <td width="100" align="center">
                    人数
                </td>
                <td width="100" align="center">
                    计调员
                </td>
                <td width="100" align="center">
                    返利
                </td>
                <td width="100" align="center">
                    结算费用
                </td>
            </tr>
            <asp:Repeater ID="repList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <font class="fbred">
                                <%# Eval("TourCode")%></font>
                        </td>
                        <td align="center">
                            <%# Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%# Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <%# Eval("PlanPeopleNumber")%>
                        </td>
                        <td align="center">
                            <%# Eval("PlanNames")%>
                        </td>
                        <td align="center">
                            <%# Eval("CommissionAmount", "{0:c2}")%>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# Eval("SettlementAmount", "{0:c2}")%></font>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <%if (len == 0)
              { %>
            <tr align="center">
                <td colspan="7">
                    没有相关数据
                </td>
            </tr>
            <%} %>
            <tr>
                <td height="30" colspan="7" align="right" class="pageup">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
