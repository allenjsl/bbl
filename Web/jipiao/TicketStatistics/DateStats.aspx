<%@ Page Title="按日期统计_出票统计" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="DateStats.aspx.cs" Inherits="Web.jipiao.TicketStatistics.DateStats" %>

<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
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
            <li><a href="/jipiao/TicketStatistics/TicketDepartList.aspx" id="two3">按部门统计</a></li>
            <li><a href="javascript:void(0)" id="two4" class="tabtwo-on">按日期统计</a></li>
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
                                部门：</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
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
                            <uc3:UCPrintButton ID="UCPrintButton1" ContentId="printList" runat="server" />
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
                <table width="100%" border="0" cellpadding="0" cellspacing="1" id="printList">
                    <tr>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            月份
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
                            <tr year="<%#Eval("CurrYear")%>" month="<%#Eval("CurrMonth")%>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center">
                                    <span id="year">
                                        <%#Eval("CurrYear")%></a>年 <span id="Month">
                                            <%#Eval("CurrMonth")%></a>月
                                </td>
                                <td align="center">
                                    <a class="count" href="javascript:void(0);">
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
                            <asp:Label ID="lblMsg" runat="server" Text="没有相关数据" Visible="false"></asp:Label>
                            <cc2:ExportPageInfo ID="ExportPageInfo1" LinkType="4" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {
            $(".count").click(function() {
                var that = $(this);
                var year = that.parent().parent().attr("Year");
                var month = that.parent().parent().attr("Month");
                var startDay = new Date(year, month - 1, 1).getDate();
                var endDay = new Date(year, month - 1, getLastDay(year, month)).getDate();
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { officeName: "", id: "", areaId: "", beginDate: "", endDate: "", leaDateS: "", leaDateE: "", departId: ""};
                parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
                parms.departId = UCselectDepart.GetId();
                parms.beginDate = year + "-" + month + "-" + startDay;
                parms.endDate = year + "-" + month + "-" + endDay;
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.officeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                var url = "/jipiao/TicketStatistics/TicketCount.aspx?" + $.param(parms);
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "出票量",
                    modal: true,
                    width: "950px",
                    height: "450px"
                });
                return false;
            });


            $("#toexcel").click(function() {
                if ($("#len").val() == 0) {
                    alert("暂无数据无法执行导出！");
                    return false;
                }
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var parms = { OfficeName: "", DepartMents: "", DepartIds: "", leaDateS: "", leaDateE: "", areaId: "" };
                parms.DepartMents = UCselectDepart.GetName();
                parms.DepartIds = UCselectDepart.GetId();
                parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
                parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
                parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
                parms.cat = "toexcel";
                location.href = "/jipiao/TicketStatistics/DateStats.aspx?" + $.param(parms);
            })
        })
        function getLastDay(year, month) {
            var new_year = year;    //取当前的年份   
            var new_month = month++; //取下一个月的第一天，方便计算（最后一天不固定）   
            if (month > 12)            //如果当前大于12月，则年份转到下一年   
            {
                new_month -= 12;        //月份减   
                new_year++;            //年份增   
            }
            var new_date = new Date(new_year, new_month, 1);                //取当年当月中的第一天   
            return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate(); //获取当月最后一天日期   
        };
        function search() {
            var UCselectDepart = <%= UCselectDepart.ClientID %>;
            var parms = { OfficeName: "", DepartMents: "", DepartIds: "", leaDateS: "", leaDateE: "", areaId: "" };
            parms.DepartMents = UCselectDepart.GetName();
            parms.DepartIds = UCselectDepart.GetId();
            parms.leaDateS = $("#<%=txtLeaveDateStart.ClientID %>").val();
            parms.leaDateE = $("#<%=txtLeaveDateEnd.ClientID %>").val();
            parms.OfficeName = $.trim($("#<%= txt_spq.ClientID %>").val());
            parms.areaId = $("#<%=ddlAirLineIds.ClientID %>").val();
            window.location.href = "/jipiao/TicketStatistics/DateStats.aspx?" + $.param(parms);
        };

    </script>

    </form>
</asp:Content>
