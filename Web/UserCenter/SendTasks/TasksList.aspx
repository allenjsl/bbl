<%@ Page Title="送团任务表" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="TasksList.aspx.cs" Inherits="Web.UserCenter.SendTasks.TasksList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">个人中心 </span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>个人中心 >> 送团任务表
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
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
                                出团日期：</label><input name="LDate" type="text" class="searchinput" id="LDate" onfocus="WdatePicker()" value="<%=LDate %>" /> 至 <input name="EndDate" type="text" class="searchinput" id="EndDate" onfocus="WdatePicker()" value="<%=LEDate %>" />
                            <label>
                                线路名称：</label><input name="RouteName" type="text" class="searchinput" id="RouteName" value="<%=RouteName %>" />
                            <a href="javascript:void(0);" id="select">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a></div>
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
                            <a href="javascript:void(0)" class="toexcel" onclick="toXls();return false;">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table  width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <%--<th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                            &nbsp;
                        </th>--%>
                        <th width="10%" height="30" align="center" bgcolor="#bddcf4">
                            出团日期
                        </th>
                        <th width="11%" align="center" bgcolor="#bddcf4">
                            集合时间
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            去程航班/时间
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            回程航班/时间
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            线路名称
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            计调
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            手机号码
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="retList" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <%--<td height="30" align="center">
                                    <input type="checkbox" name="checkbox" id="checkbox1" />
                                </td>--%>
                                <td height="30" align="center">
                                    <%#Eval("LDate") == null ? "" : Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("GatheringTime")%>
                                </td>
                                <td align="center">
                                    <%#Eval("LTraffic")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RTraffic")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("PlanPeopleNumber")%>
                                </td>
                                <td align="center">
                                    <%#GetJiDiaoName(Eval("TourCoordinatorInfo"))%>
                                </td>
                                <td align="center">
                                    <%#GetJiDiaoMobile(Eval("TourCoordinatorInfo"))%>
                                </td>
                                <td align="center">
                                    <a href="<%= printUrl %>?tourId=<%#Eval("TourId") %>" target="_blank">查看</a>
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
                </table>
                <div class="tablelist" style="display:none">
                  <table id="print" width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <%--<th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                            &nbsp;
                        </th>--%>
                        <th width="10%" height="30" align="center" bgcolor="#bddcf4">
                            出团日期
                        </th>
                        <th width="11%" align="center" bgcolor="#bddcf4">
                            集合时间
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            去程航班/时间
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            回程航班/时间
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            线路名称
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            计调
                        </th>
                    </tr>
                    <asp:Repeater ID="Repeater1" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <%--<td height="30" align="center">
                                    <input type="checkbox" name="checkbox" id="checkbox1" />
                                </td>--%>
                                <td height="30" align="center">
                                    <%#Eval("LDate") == null ? "" : Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("GatheringTime")%>
                                </td>
                                <td align="center">
                                    <%#Eval("LTraffic")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RTraffic")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("PlanPeopleNumber")%>
                                </td>
                                <td align="center">
                                    <%#GetJiDiaoName(Eval("TourCoordinatorInfo"))%>
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
                </table>
                </div>
                
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script src="/js/utilsUri.js" type="text/javascript"></script>
    <script type="text/javascript">
        function select() {
            
            var LDate = $.trim($("#LDate").val());
            var LEdate = $.trim($("#EndDate").val());
            var RouteName = $.trim($("#RouteName").val());
            //参数集
            var para = { LDate: "",LEDate:"", RouteName: ""
            };
            para.LDate = LDate;
            para.LEDate = LEdate;
            para.RouteName = RouteName
            window.location.href = "/UserCenter/SendTasks/TasksList.aspx?" + $.param(para);
            return false;
        };
        $(function() {
            $("#select").click(function() {
                select(); //查询
                return false;
            });
            $("#RouteName").keypress(function(e) {
                if (e.keyCode == 13) {
                    select(); //查询
                    return false;
                }
            });
        })

        //导出
        function toXls() {
            var params = utilsUri.getUrlParams([]);
            params["recordcount"] = recordCount;
            params["istoxls"] = 1;

            if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
    </script>

    </form>
</asp:Content>
