<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketDepartList.aspx.cs"
    Inherits="Web.jipiao.TicketStatistics.TicketDepartList" MasterPageFile="~/masterpage/Back.Master"
    Title="按部门统计_出票统计" %>

<%@ Register Src="/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">出票统计</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>机票管理 >> 出票统计
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
            <li><a href="/jipiao/TicketStatistics/TicketStatisticslist.aspx" id="two1">按售票处统计</a></li>
            <li><a href="/jipiao/TicketStatistics/AirwaysStat.aspx" id="two2">按航空公司统计</a></li>
            <li><a href="javascript:void(0)" id="two3" class="tabtwo-on">按部门统计</a></li>
            <li><a href="/jipiao/TicketStatistics/DateStats.aspx" id="two4">按日期统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_4">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                售票处：</label>
                            <input type="text" id="txt_spq" class="searchinput" name="txt_spq" runat="server" />
                            <label>
                                航空公司：</label>
                            <asp:DropDownList ID="ddlAirLineIds" runat="server" AppendDataBoundItems="True" DataTextField="Text"
                                DataValueField="value">
                            </asp:DropDownList>
                            <label>
                                出票开始日期：</label>
                            <input onfocus="WdatePicker()" type="text" id="txt_date" class="searchinput" name="txt_date"
                                runat="server" />
                            <label>
                                出票结束日期：</label>
                            <input onfocus="WdatePicker()" type="text" id="txt_endDate" class="searchinput" name="txt_endDate"
                                runat="server" />
                            <label>
                                出团日期：</label>
                            <asp:TextBox ID="txtLeaveDateStart" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                            </asp:TextBox>
                            <label>
                                至</label>
                            <asp:TextBox ID="txtLeaveDateEnd" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                            </asp:TextBox>
                            <label>
                                <img style="vertical-align: top; cursor: pointer;" onclick="search()" alt="查询" src="/images/searchbtn.gif" /></label>
                        </div>
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
                            <uc1:UCPrintButton ID="UCPrintButton1" ContentId="printList" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a id="toexcel" href="javascript:void(0)">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table id="printList" width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            部门
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
                    <asp:Repeater ID="retList" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" id="count" class="num" departid="<%#Eval("departId") %>">
                                        <%#Eval("TicketOutNum")%>(张)</a>
                                </td>
                                <td align="center">
                                    <%#Eval("TotalAmount","{0:c2}")%>
                                </td>
                                <td align="center">
                                    <%#Eval("PayAmount", "{0:c2}")%>
                                </td>
                                <td align="center">
                                    <%#Eval("UnPaidAmount", "{0:c2}")%>
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
                            <cc2:ExportPageInfo ID="ExportPageInfo1" LinkType="4" PageStyleType="NewButton" CurrencyPageCssClass="RedFnt"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {
            $(".num").click(function() {
                var departId = $(this).attr("departId");
                var parms = { officeName: "", id: "", areaId: "", beginDate: "", endDate: "", leaDateS: "", leaDateE: "", departId: "" };
                parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
                parms.departId = departId;
                parms.beginDate = $.trim($("#<%= txt_date.ClientID %>").val());
                parms.endDate = $.trim($("#<%= txt_endDate.ClientID %>").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.officeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                var url = "/jipiao/TicketStatistics/TicketCount.aspx?" + $.param(parms);
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "出票-部门统计",
                    modal: true,
                    width: "960px",
                    height: "500px"
                });
                return false;
            });

            $("#toexcel").click(function() {
                var parms = { OfficeName: "", areaId: "", DateTime: "", endtime: "", leaDateS: "", leaDateE: "" };
                parms.DateTime = $.trim($("#<%= txt_date.ClientID %>").val());
                parms.endtime = $.trim($("#<%= txt_endDate.ClientID %>").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
                parms.cat = "toexcel";
                var url = "/jipiao/TicketStatistics/TicketDepartList.aspx?" + $.param(parms);
                location.href = url;
            })
        })
        function search() {
            var parms = { OfficeName: "", areaId: "", DateTime: "", endtime: "", leaDateS: "", leaDateE: "" };
            parms.DateTime = $.trim($("#<%= txt_date.ClientID %>").val());
            parms.endtime = $.trim($("#<%= txt_endDate.ClientID %>").val());
            parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
            parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
            parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
            parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
            var url = "TicketDepartList.aspx?" + $.param(parms);
            location.href = url;
        };

    </script>

    </form>
</asp:Content>
