<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TravelAgencyList.aspx.cs"
    Inherits="Web.UserCenter.DjArrangeMengs.ArrangeMengsList" MasterPageFile="~/masterpage/AreaConnect.Master" Title="地接安排_个人中心"%>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="head" ID="Head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="body" runat="server">
   <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">个人中心</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                       所在位置>> <a href="javascript:void(0);">个人中心</a>>> 地接安排
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000"></td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table id="ArrangeMengsSearch" width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top"><img src="/images/yuanleft.gif"  alt=""/></td>
                <td>
                    <div class="searchbox"> 
                        <label>线路名称：</label>
                        <input type="text" name="TxtRouteName" id="TxtRouteName" class="searchinput searchinput02"  value="<%=RouteName %>"/>
                        <label>出团日期：</label>
                        <input style="width: 60px;" name="TxtStartTime" type="text" class="searchinput" id="TxtStartTime"  onfocus="WdatePicker();" value="<%=Starttime %>"/>
                        <label>天数：</label>
                        <input type="text" name="TourDays" id="TourDays" class="searchinput searchinput03" value="<%=TourDays %>"/>
                        <label><a href="javascript:void(0);" class="search"><img src="/images/searchbtn.gif" style="vertical-align: top;"  alt="" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top"><img src="/images/yuanright.gif" alt=""/></td>
            </tr>
        </table>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="18%" align="center" bgcolor="#BDDCF4">线路名称</th>
                    <th width="9%" align="center" bgcolor="#bddcf4">出团日期</th>
                    <th width="7%" align="center" bgcolor="#bddcf4">天数</th>
                    <th width="7%" align="center" bgcolor="#bddcf4">人数</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">去程航班/时间</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">回程程航班/时间</th>
                    <th width="9%" align="center" bgcolor="#bddcf4">对方计调</th>
                    <th width="9%" align="center" bgcolor="#bddcf4">联系电话</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">QQ</th>
                    <th width="6%" align="center" bgcolor="#bddcf4">操作</th>
                </tr>
                <asp:Repeater ID="Replist" runat="server">
                   <ItemTemplate>
                         <tr tid="<%# Eval("TourId") %>"  bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td align="center" bgcolor="#e3f1fc" class="pandl3"><a href="<%# GetPrintUrl(Eval("TourId").ToString()) %>?tourId=<%# Eval("TourId") %>" target="_blank"><%# Eval("RouteName")%></a></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("LDate","{0:yyyy-MM-dd}")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("TourDays")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("PlanPeopleNumber") %></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("LTrafic")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("RTrafic")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("JDYContacterName")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("JDYTelephone")%></td>
                            <td align="center" bgcolor="#e3f1fc"><%# Eval("JDYQQ")%></td>
                            <td align="center" bgcolor="#e3f1fc"><a href="javascript:viod(0);" class="show" ref="<%#Eval("ReleaseType").ToString()%>">查看</a></td>
                        </tr>
                   </ItemTemplate>
                </asp:Repeater>
                <% if (len == 0) { %> <tr align="center"><td colspan="11">没有相关数据!</td></tr><%} %>               
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right" class="pageup">
                       <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
     <script type="text/javascript" language="javascript">
         var ArrangeMengs = {
             SetAllCheckValue: function(obj) {
                 $(".tablelist").find("input.selectedAll").each(function() {
                     $(this).attr("checked", obj);
                 });
             },
             OnSearch: function() {
                 var _RouteName = $.trim($("#TxtRouteName").val());
                 var _Stratime = $.trim($("#TxtStartTime").val());
                 var _Tourdays = $.trim($("#TourDays").val());
                 var para = { _RouteName: "", _Stratime: "", _Tourdays: "" };
                 para._RouteName = _RouteName;
                 para._Stratime = _Stratime;
                 para._Tourdays = _Tourdays;
                 window.location.href = "/UserCenter/DjArrangeMengs/TravelAgencyList.aspx?" + $.param(para);
             }
         };
         $(document).ready(function() {
             //查看
             $(".show").click(function() {
                 var url = "/UserCenter/DjArrangeMengs/TeamTake.aspx?";
                 var tid = $(this).parent().parent().attr("tid");
                 var planType = $(this).attr("ref");
                 Boxy.iframeDialog({
                     iframeUrl: url + "Tourid=" + tid + "&action=first" + "&planType=" + planType,
                     title: "查看",
                     modal: true,
                     width: "950",
                     height: "450px"
                 });
                 return false;
             });
             //查询
             $("a.search").click(function() {
                 ArrangeMengs.OnSearch();
                 return false;
             });
             //回车查询
             $("#ArrangeMengsSearch input").bind("keypress", function(e) {
                 if (e.keyCode == 13) {
                     ArrangeMengs.OnSearch();
                     return false;
                 }
             });
         })
     </script>
    </form>
</asp:Content>
