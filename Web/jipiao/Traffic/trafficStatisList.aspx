<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trafficStatisList.aspx.cs"
    Inherits="Web.jipiao.Traffic.trafficStatisList" MasterPageFile="~/masterpage/Back.Master"
    Title="交通出票统计" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Src="/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<asp:Content ContentPlaceHolderID="head" ID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="body" runat="server">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <form id="form" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">交通出票统计</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 机票管理 >> 交通出票统计
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
        <div id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                出团开始日期：</label>
                            <input name="TxtDateTime" type="text" class="searchinput" id="TxtDateTime" onfocus="WdatePicker();"
                                value="<%= Datetime %>" />
                            <label>
                                出团结束日期：</label>
                            <input name="TxtendTime" type="text" class="searchinput" id="TxtendTime" onfocus="WdatePicker();"
                                value="<%= ticketendtime %>" />
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
                            <uc3:ucprintbutton contentid="print" id="UCPrintButton1" runat="server" />
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
                        <th width="50%" align="center" bgcolor="#bddcf4">
                            交通名称
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            出票量
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            代理费
                        </th>
                    </tr>
                    <asp:Repeater ID="prtticketlist" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("TrafficName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# Eval("ChuPiaoShu")%>张
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# EyouSoft.Common.Utils.GetDecimal( Eval("AgencyPrice").ToString()).ToString("0.00")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <th width="50%" align="center" bgcolor="#bddcf4">
                            总计
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblAllTickets" runat="server" Text="0"></asp:Label>
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="labDaiLiFeiCount" runat="server" Text="0"></asp:Label>
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
        $(function() {
            $("#Searchbtn").click(function() {
                var parms = { DateTime: "", endtime: "" };
                parms.DateTime = $.trim($("#TxtDateTime").val());
                parms.endtime = $.trim($("#TxtendTime").val());
                window.location.href = "/jipiao/Traffic/trafficStatisList.aspx?" + $.param(parms);
                return false;
            });

            $("#con_two_1 input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    $("#Searchbtn").click();
                    return false;
                }
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
