<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradeSituation.aspx.cs"
    Inherits="Web.SupplierControl.AreaConnect.TradeSituation" %>

<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center" style="text-align: center">
            <tbody>
                <tr>
                    <td>
                        <div style="background: url('../images/yuancenter.gif') repeat-x scroll 0 0 transparent;
                            line-height: 15px; min-height: 30px; padding-bottom: 5px; padding-top: 10px;
                            width: 104%;">
                            <label>
                                出团开始时间：</label>
                            <input type="text" onfocus="WdatePicker()" id="txt_Date" class="searchinput" runat="server"
                                name="txt_Date" />
                            <label>
                                出团结束时间：</label>
                            <input type="text" onfocus="WdatePicker()" id="txt_Date1" class="searchinput" runat="server"
                                name="txt_Date" />
                            <label>
                                付款状态：</label>
                            <select id="SeleState" runat="server">
                                <option value="-1">请选择</option>
                                <option value="1">已结清</option>
                                <option value="2">未结清</option>
                            </select>
                            <label>
                                <a href="javascript:void(0);" id="a_search">
                                    <img border="0" src="/images/searchbtn.gif" alt="" valign="middel" /></a>
                            </label>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <table id="print" width="700" border="0" align="center" cellpadding="0" cellspacing="1"
            style="margin: 10px;">
            <tr class="odd">
                <td width="20%" height="30" align="center">
                    团号
                </td>
                <td width="20%" align="center">
                    线路名称
                </td>
                <td width="15%" align="center">
                    出团日期
                </td>
                <td width="5%" align="center">
                    人数
                </td>
                <td width="10%" align="center">
                    计调员
                </td>
                <td width="10%" align="center">
                    返利
                </td>
                <td width="10%" align="center">
                    未付金额
                </td>
                <td width="10%" align="center">
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
                            <%# Eval("RealityPeopleNumber")%>
                        </td>
                        <td align="center">
                            <%# Eval("PlanNames")%>
                        </td>
                        <td align="center">
                            <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("CommissionAmount", "{0:c2}").ToString())%>
                        </td>
                        <td align="center">
                            <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("NotPayAmount", "{0:c2}").ToString())%>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("SettlementAmount", "{0:c2}").ToString())%></font>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="odd">
                <td width="20%" height="30" align="center">
                    <b>总计</b>
                </td>
                <td width="20%" align="center">
                </td>
                <td width="15%" align="center">
                </td>
                <td width="5%" align="center">
                    <asp:Label ID="lblPeopleNum" runat="server" Text=""></asp:Label>
                </td>
                <td width="10%" align="center">
                </td>
                <td width="10%" align="center">
                    <b>
                        <asp:Label ID="lblCommissionAmount" runat="server" Text=""></asp:Label></b>
                </td>
                <td width="10%" align="center">
                    <b>
                        <asp:Label ID="lblNotPayAmount" runat="server" Text=""></asp:Label></b>
                </td>
                <td width="10%" align="center">
                    <b>
                        <asp:Label ID="lblSettlementAmount" runat="server" Text=""></asp:Label></b>
                </td>
            </tr>
            <%if (len == 0)
              { %>
            <tr align="center">
                <td colspan="7">
                    没有相关数据
                </td>
            </tr>
            <%} %>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
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
                    <uc3:UCPrintButton ContentId="print" ID="UCPrintButton1" runat="server" />
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="TradeSituation.aspx?act=toexcel&tid=<%=Request.QueryString["tid"]%>" target="_blank">
                        导出</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {
            $("#a_search").click(function() {
                var para = { date: "", LDate: "", status: "", iframeid: "" };
                para.date = $("#<%=this.txt_Date.ClientID %>").val();
                para.LDate = $("#<%=this.txt_Date1.ClientID %>").val();
                para.status = $("#<%=this.SeleState.ClientID %>").val();
                para.iframeid = '<%=Request.QueryString["iframeid"]%>';

                window.location.href = '/SupplierControl/AreaConnect/TradeSituation.aspx?tid=<%=Request.QueryString["tid"]%>&' + $.param(para);
                return false;
            })

        })
    </script>

    </form>
</body>
</html>
