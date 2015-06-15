<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketStatisticslist.aspx.cs"
    Inherits="Web.jipiao.TicketStatistics.TicketStatisticslist" MasterPageFile="~/masterpage/Back.Master"
    Title="按售票处统计_出票统计" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Src="/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" ID="Content1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="Content2" runat="server">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <form id="form" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">出票统计</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 机票管理 >> 出票统计
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
            <li><a href="/jipiao/TicketStatistics/TicketStatisticslist.aspx" class="tabtwo-on">按售票处统计</a></li>
            <li><a href="/jipiao/TicketStatistics/AirwaysStat.aspx">按航空公司统计</a></li>
            <li><a href="/jipiao/TicketStatistics/TicketDepartList.aspx">按部门统计</a></li>
            <li><a href="/jipiao/TicketStatistics/DateStats.aspx">按日期统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                航空公司：</label>
                            <asp:DropDownList ID="Airlineslist" runat="server">
                            </asp:DropDownList>
                            <label>
                                部门：</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
                            <label>
                                售票开始日期：</label>
                            <input name="TxtDateTime" type="text" class="searchinput" id="TxtDateTime" onfocus="WdatePicker();"
                                value="<%=Datetime %>" />
                            <label>
                                售票结束日期：</label>
                            <input name="TxtendTime" type="text" class="searchinput" id="TxtendTime" onfocus="WdatePicker();"
                                value="<%=ticketendtime %>" />
                            <label>
                                出团日期：</label>
                            <asp:TextBox ID="txtLeaveDateStart" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                            </asp:TextBox>
                            <label>
                                至</label>
                            <asp:TextBox ID="txtLeaveDateEnd" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                            </asp:TextBox>
                            <label>
                                <a href="javascript:void(0);" id="Searchbtn">
                                    <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="查询" /></a></label></div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc3:UCPrintButton ContentId="print" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" class="toexcel">
                                <img src="/images/daoru.gif" />导 出</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1" id="print">
                    <tr>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            售票处
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            出票量
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            应付机票款
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            已付机票款
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            未付机票款
                        </th>
                    </tr>
                    <asp:Repeater ID="prtticketlist" runat="server">
                        <ItemTemplate>
                            <tr id="<%# Eval("OfficeId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("OfficeName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" class="TicketOutNum">
                                        <%# Eval("TicketOutNum")%>张</a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("TotalAmount", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("PayAmount", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("UnPaidAmount", "{0:c2}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            总计
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblAllTickets" runat="server" Text="0"></asp:Label>
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblNeedMoney" runat="server" Text="0"></asp:Label>
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblOverMoney" runat="server" Text="0"></asp:Label>
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblNoMoney" runat="server" Text="0"></asp:Label>
                        </th>
                    </tr>
                </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblMsg" runat="server" Text="暂无数据!"></asp:Label>
                            <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        var TicketStatistics = {
            Onsearch: function() {
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { AirlinesValue: "", DepartMents: "", DepartIds: "", DateTime: "", endtime: "", leaDateS: "", leaDateE: "" };
                parms.AirlinesValue = $("#<%=Airlineslist.ClientID %>").val();
                parms.DepartMents = UCselectDepart.GetName();
                parms.DepartIds = UCselectDepart.GetId();
                parms.DateTime = $.trim($("#TxtDateTime").val());
                parms.endtime = $.trim($("#TxtendTime").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                window.location.href = "/jipiao/TicketStatistics/TicketStatisticslist.aspx?" + $.param(parms);
            }
        };
        $(document).ready(function() {
            $("#Searchbtn").click(function() {
                TicketStatistics.Onsearch();
                return false;
            });
            $("#con_two_1 input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    TicketStatistics.Onsearch();
                    return false;
                }
            });
            $(".tablelist a.TicketOutNum").click(function() {
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var that = $(this);
                var tid = that.parent().parent().attr("id");
                var parms = { id: "", areaId: "", beginDate: "", endDate: "", leaDateS: "", leaDateE: "", departId: ""};
                parms.areaId = $("#<%=Airlineslist.ClientID %>").val();
                parms.departId = UCselectDepart.GetId();
                parms.beginDate = $.trim($("#TxtDateTime").val());
                parms.endDate = $.trim($("#TxtendTime").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.id = tid
                var url = "/jipiao/TicketStatistics/TicketCount.aspx?" + $.param(parms);
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "出票量",
                    modal: true,
                    width: "930px",
                    height: "380px"
                });
                return false;
            });
            //导出excel
            $("a.toexcel").click(function() {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&action=toexcel"; } else { url += "?action=toexcel"; }
                window.location.href = url;
                return false;
            });
        });
    </script>

</asp:Content>
