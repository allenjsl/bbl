<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" Title="按月统计_现金流量表_统计分析"
    CodeBehind="CasMonthStaList.aspx.cs" Inherits="Web.StatisticAnalysis.CashFlow.CasMonthStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 现金流量 >> 按月统计
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a href="/StatisticAnalysis/CashFlow/CasDayStaList.aspx" id="two1" >按天统计</a></li>
            <li><a href="/StatisticAnalysis/CashFlow/CasMonthStaList.aspx"  class="tabtwo-on" id="two2">
                按月统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
         <div id="con_two_2">
        <div class="tablelist">
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc1:UCPrintButton ContentId="tbl_GetCashMothSList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a id="btnExport" href="javascript:void(0);">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" onclick="return AddCash();">补充现金</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divCashMothStaList" runat="server">
                <table width="100%" id="tbl_GetCashMothSList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th align="center" bgcolor="#BDDCF4">
                            月份
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            现金储备
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            现金收入
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            现金支出
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            当前现金量
                        </th>
                        <%--   <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>--%>
                    </tr>
                    <cc2:CustomRepeater ID="crp_GetCashMonthStaList" runat="server">
                        <ItemTemplate>
                            <tr cash='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("CurrCash")))%>'
                                style="background-color: #e3f1fc">
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("IssueTime", "{0:MM月}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CashReserve","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CashIn","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CashOut","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CurrCash","{0:c2}")%>
                                </td>
                                <%-- <td align="center" bgcolor="#e3f1fc">
                                <asp:LinkButton runat="server" ID="lkAddCash" CssClass="fblue"></asp:LinkButton>
                            </td>--%>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr cash='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("CurrCash")))%>'
                                style="background-color: #BDDCF4">
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("IssueTime", "{0:MM月}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CashReserve","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CashIn","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CashOut","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CurrCash","{0:c2}")%>
                                </td>
                                <%--<td align="center" bgcolor="#bddcf4">
                                <asp:LinkButton runat="server" ID="lkAddCash" CssClass="fblue"></asp:LinkButton>
                            </td>--%>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc2:CustomRepeater>
                </table>
            </div>
            <%--  <table width="100%" id="tbl_ExportPage" runat="server" border="0" cellspacing="0"
                cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>--%>
        </div>
        </div>
        <cc2:CustomRepeater ID="crp_PrintGetCashMonthStaList" Visible="false" runat="server">
            <HeaderTemplate>
                <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                    <tr>
                        <th align="center">
                            月份
                        </th>
                        <th align="center">
                            现金储备
                        </th>
                        <th align="center">
                            现金收入
                        </th>
                        <th align="center">
                            现金支出
                        </th>
                        <th align="center">
                            当前现金量
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("IssueTime", "{0:MM月}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashReserve","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashIn","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashOut","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CurrCash","{0:c2}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("IssueTime", "{0:MM月}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashReserve","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashIn","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashOut","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CurrCash","{0:c2}")%>
                    </td>

                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </cc2:CustomRepeater>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#tbl_GetCashMothSList tr").each(function() {
                if (parseInt($(this).attr("cash"), 10) <= 0)
                    $(this).css("color", "red");
            });
            $("#btnExport").click(function() {//导出
            if ($("#tbl_GetCashMothSList").find("[id='EmptyData']").length >0) {
                    alert("暂无数据,无法执行导出！");
                    return false;
                }
                var goToUrl = "/StatisticAnalysis/CashFlow/CasMonthStaList.aspx?isExport=1";
                window.open(goToUrl, "Excel导出");
                return false;
            });
        });
        function AddCash() {
            var url = "/StatisticAnalysis/CashFlow/AddCash.aspx";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "补充现金",
                modal: true,
                width: "320px",
                height: "135px"
            });
            return false;
        }
    </script>

    </form>
</asp:Content>
