<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FitHairDayList.aspx.cs" Inherits="Web.GroupEnd.FitHairDay.FitHairDayList"  Title="散客天天发" MasterPageFile="~/masterpage/Front.Master"%>

<%@ Register Src="~/UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc11" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
  <form id="Form1" runat="server">
       <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap"><span class="lineprotitle">散客天天发</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 散客天天发
                    </td>
                </tr>
                <tr><td colspan="2" height="2" bgcolor="#000000"></td></tr>
            </table>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" id="TourEveryDay">
            <tr>
                <td width="10" valign="top"><img src="/images/yuanleft.gif" alt="" /></td>
                <td>
                    <div class="searchbox">      
                        <label>线路名称：</label>
                          <input type="text"  id="RouteName" name="RouteName" runat="server" class="searchinput searchinput02"/>                        
                        <label><img src="/images/searchbtn.gif" style="vertical-align: top; cursor: pointer;" id="Btn_Serach"
                                alt="查询" /></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
         <div class="tablelist">
          <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr class="odd">
              <th width="18%" align="center" nowrap="nowrap">线路名称</th>
              <th width="18%" align="center" nowrap="nowrap">线路区域</th>
              <th width="13%" align="center" nowrap="nowrap">天数</th>
              <th width="13%" align="center" nowrap="nowrap">门市价格</th>
              <th width="10%" align="center" nowrap="nowrap">负责人</th>
              <th width="12%" align="center" nowrap="nowrap">操作</th>
            </tr>
             <asp:Repeater ID="repeaterlist" runat="server">
                <ItemTemplate>
                    <tr id="<%# Eval("TourId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                      <td align="center"><a target="_blank" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType"))+"?tourId="+Eval("tourId").ToString()+"&action=tourevery" %>"><%#Eval("RouteName")%></a></td>
                      <td align="center"><%# Eval("AreaName")%></td>
                      <td align="center"><%# Eval("TourDays")%></td>
                      <td align="center">成人:<a href="javascript:void(0);" class="crprices" THAdultPrice="<%# Eval("THAdultPrice") %>" THChildrenPrice="<%# Eval("THChildrenPrice") %>" MSAdultPrice="<%# Eval("MSAdultPrice") %>" MSChildrePricen="<%# Eval("MSChildrePricen") %>"><font class="fred"><%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("MSAdultPrice").ToString())%></font></a><br />
                      儿童:<a href="javascript:void(0);" class="etprices" THAdultPrice="<%# Eval("THAdultPrice") %>" THChildrenPrice="<%# Eval("THChildrenPrice") %>" MSAdultPrice="<%# Eval("MSAdultPrice") %>" MSChildrePricen="<%# Eval("MSChildrePricen") %>"><font class="fred"><%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("MSChildrePricen").ToString())%></font></a></td>
                      <td  align="center"><a href="javascript:void(0);" ref="<%# Eval("TourId") %>" ContacterName="<%# Eval("ContacterName") %>" ContacterTelephone="<%# Eval("ContacterTelephone") %>"  ContacterFax="<%# Eval("ContacterFax") %>" ContacterMobile="<%# Eval("ContacterMobile") %>" ContacterQQ="<%# Eval("ContacterQQ") %>" ContacterEmail="<%# Eval("ContacterEmail") %>" class="Operator"><%# Eval("ContacterName")%></a></td>
                      <td align="center"><a href="javascript:void(0);" ref="<%# Eval("TourId") %>" class="Application">提交申请</a></td>
                    </tr>             
                </ItemTemplate>
            </asp:Repeater>
               <%if(lenght==0){ %><tr align="center"><td colspan="5">暂无数据!</td></tr><%} %>           
          </table>          
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>              
              <td align="right" class="pageup">
                             <cc11:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
              </td>
            </tr>
          </table>                    
        </div>               
     </div>
     
       <script type="text/javascript">
           var FitHairDay = {
               Search: function() {
                   var RouteName = $("#<%=RouteName.ClientID %>").val();
                   var Params = { RouteName: "" };
                   Params.RouteName = RouteName;
                   window.location.href = "/GroupEnd/FitHairDay/FitHairDayList.aspx?" + $.param(Params);
               }
               
           };
           $(document).ready(function() {
               //查询
               $("#Btn_Serach").click(function() {
                   FitHairDay.Search();
                   return false;
               });

               //回车查询
               $("#TourEveryDay input").bind("keypress", function(e) {
                   if (e.keyCode == 13) {
                       FitHairDay.Search();
                       return false;
                   }
               });

               //负责人信息
               $("a.Operator").click(function() {
                   var url = "/GroupEnd/FitHairDay/Operatorlist.aspx";
                   var ContacterName = $(this).attr("ContacterName");
                   var ContacterTelephone = $(this).attr("ContacterTelephone");
                   var ContacterFax = $(this).attr("ContacterFax");
                   var ContacterMobile = $(this).attr("ContacterMobile");
                   var ContacterQQ = $(this).attr("ContacterQQ");
                   var ContacterEmail = $(this).attr("ContacterEmail");
                   Boxy.iframeDialog({
                       iframeUrl: url + "?ContacterName=" + escape(ContacterName) + "&ContacterTelephone=" + ContacterTelephone + "&ContacterFax=" + ContacterFax + "&ContacterMobile=" + ContacterMobile + "&ContacterQQ=" + ContacterQQ + "&ContacterEmail=" + ContacterEmail,
                       title: "负责人",
                       modal: true,
                       width: "400px",
                       height: "250px"
                   });
                   return false;
               });

               //操作
               $("a.Application").click(function() {
                   var url = "/GroupEnd/FitHairDay/SingUp.aspx";
                   var that = $(this).attr("ref");
                   Boxy.iframeDialog({
                       iframeUrl: url + "?tourid=" + that,
                       title: "申请",
                       modal: true,
                       width: "905px",
                       height: "410px"
                   });
                   return false;
               });

               //成人价详细
               $("a.crprices").click(function() {
                   var url = "/GroupEnd/FitHairDay/AdultPrices.aspx";
                   var MSAdultPrice = $(this).attr("MSAdultPrice");
                   var MSChildrePricen = $(this).attr("MSChildrePricen");
                   var THAdultPrice = $(this).attr("THAdultPrice");
                   var THChildrenPrice = $(this).attr("THChildrenPrice");
                   Boxy.iframeDialog({
                       iframeUrl: url + "?MSAdultPrice=" + MSAdultPrice + "&MSChildrePricen=" + MSChildrePricen + "&THAdultPrice=" + THAdultPrice + "&THChildrenPrice=" + THChildrenPrice,
                       title: "申请",
                       modal: true,
                       width: "430px",
                       height: "160px"
                   });
                   return false;
               });

               //儿童价详细
               $("a.etprices").click(function() {
                   var url = "/GroupEnd/FitHairDay/Childrenprice.aspx";
                   var MSAdultPrice = $(this).attr("MSAdultPrice");
                   var MSChildrePricen = $(this).attr("MSChildrePricen");
                   var THAdultPrice = $(this).attr("THAdultPrice");
                   var THChildrenPrice = $(this).attr("THChildrenPrice");
                   Boxy.iframeDialog({
                       iframeUrl: url + "?MSAdultPrice=" + MSAdultPrice + "&MSChildrePricen=" + MSChildrePricen + "&THAdultPrice=" + THAdultPrice + "&THChildrenPrice=" + THChildrenPrice,
                       title: "申请",
                       modal: true,
                       width: "430px",
                       height: "160px"
                   });
                   return false;
               });
           });
       </script>
  </form>
</asp:Content>

