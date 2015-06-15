<%@ Page Title="按航空公司统计_出票统计" MasterPageFile="~/masterpage/Back.Master" Language="C#"
    AutoEventWireup="true" CodeBehind="AirwaysStat.aspx.cs" Inherits="Web.jipiao.TicketStatistics.AirwaysStat" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="c2" %>
<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="printButton" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server" id="form1">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">出票统计</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置>>机票管理 &gt;&gt; 出票统计
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a class="" href="/jipiao/TicketStatistics/TicketStatisticslist.aspx">按售票处统计</a></li>
            <li><a id="two2" href="javascript:void(0)" class="tabtwo-on">按航空公司统计</a></li>
            <li><a id="two3" href="/jipiao/TicketStatistics/TicketDepartList.aspx" class="">按部门统计</a></li>
            <li><a id="two4" href="/jipiao/TicketStatistics/DateStats.aspx" class="">按日期统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <!--按部门统计-->
        <div style="display: block;" id="con_two_2">
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    售票处：</label>
                                <input type="text" id="txt_spq" class="searchinput" name="txt_spq" runat="server" />
                                <label>
                                    部门：</label>
                                <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
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
                                    <a href="javascript:void(0);" id="Searchbtn">
                                        <img style="vertical-align: top; cursor: pointer;" alt="查询" src="/images/searchbtn.gif" /></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td width="90" align="left">
                                <uc1:printButton ContentId="tbl_OutDepartStaList" runat="server" ID="printButton1">
                                </uc1:printButton>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:void(0)" id="btnExport">
                                    <img src="/images/daoru.gif" style="cursor: pointer;">
                                    导 出 </a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <c2:CustomRepeater runat="server" ID="rpt_list">
                    <HeaderTemplate>
                        <table width="100%" id="tbl_OutDepartStaList" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th width="13%" align="center">
                                        航空公司
                                    </th>
                                    <th width="13%" align="center">
                                        出票量
                                    </th>
                                    <th width="13%" align="center">
                                        应付机票款
                                    </th>
                                    <th width="13%" align="center">
                                        已付机票款
                                    </th>
                                    <th width="13%" align="center">
                                        未付机票款
                                    </th>
                                </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <td align="center">
                                <%#Eval("AirLineName")%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" airline="<%#Eval("AirLineId") %>" class="num">
                                    <%#Eval("TicketOutNum")%>(张)</a>
                            </td>
                            <td align="center">
                                <%#Eval("TotalAmount", "{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("PayAmount", "{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("UnPaidAmount", "{0:c2}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr class="odd">
                            <th width="13%" align="center">
                                总计
                            </th>
                            <th width="13%" align="center">
                                <%=lblAllTickets%>
                            </th>
                            <th width="13%" align="center">
                                <%=lblNeedMoney%>
                            </th>
                            <th width="13%" align="center">
                                <%=lblOverMoney %>
                            </th>
                            <th width="13%" align="center">
                                <%=lblNoMoney%>
                            </th>
                        </tr>
                        </table></FooterTemplate>
                </c2:CustomRepeater>
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td align="right">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" pagestyletype="NewButton"
                                    CurrencyPageCssClass="RedFnt" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        var AirwaysStat = {
            Onsearch: function() {
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { OfficeName:"", DepartMents: "", DepartIds: "", DateTime: "", endtime: "", leaDateS: "", leaDateE: "" };
                parms.DepartMents = UCselectDepart.GetName();
                parms.DepartIds = UCselectDepart.GetId();
                parms.DateTime = $.trim($("#<%= txt_date.ClientID %>").val());
                parms.endtime = $.trim($("#<%= txt_endDate.ClientID %>").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                window.location.href = "/jipiao/TicketStatistics/AirwaysStat.aspx?" + $.param(parms);
            }
        };
        $(function() {
            $("#Searchbtn").click(function() {
                AirwaysStat.Onsearch();
                return false;
            });
            $("#btnExport").click(function() {//导出
                if ($("#tbl_OutDepartStaList").find("[id='EmptyData']").length > 0) {
                    alert("暂无数据无法执行导出！");
                    return false;
                }
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { OfficeName:"", DepartMents: "", DepartIds: "", DateTime: "", endtime: "", leaDateS: "", leaDateE: "" };
                parms.DepartMents = UCselectDepart.GetName();
                parms.DepartIds = UCselectDepart.GetId();
                parms.DateTime = $.trim($("#<%= txt_date.ClientID %>").val());
                parms.endtime = $.trim($("#<%= txt_endDate.ClientID %>").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                parms.isExport = 1;
                parms.isAll = 1;
                var goToUrl = "AirwaysStat.aspx?" + $.param(parms);
                window.open(goToUrl, "Excel导出");
                return false;
            });
            $(".num").click(function() {
                var airLineId = $(this).attr("airLine");
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { officeName: "", id: "", areaId: "", beginDate: "", endDate: "", leaDateS: "", leaDateE: "", departId: ""};
                parms.areaId = airLineId;
                parms.departId = UCselectDepart.GetId();
                parms.beginDate = $.trim($("#<%= txt_date.ClientID %>").val());
                parms.endDate = $.trim($("#<%= txt_endDate.ClientID %>").val());
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.officeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                var url = "/jipiao/TicketStatistics/TicketCount.aspx?" + $.param(parms);
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "出票量",
                    modal: true,
                    width: "960px",
                    height: "500px"
                });
                return false;
            });
        });
    </script>

</asp:Content>
