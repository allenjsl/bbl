<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DingDanTiXing.aspx.cs" Inherits="Web.UserCenter.WorkAwake.DingDanTiXing" MasterPageFile="~/masterpage/Back.Master" Title="订单提醒_个人中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<asp:Content ID="PageHeader1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/js/utilsUri.js" type="text/javascript"></script>
    
    <style type="text/css">
    .money{text-align:right;}
    .money span{ padding-right:10px;}
    </style>
</asp:Content>

<asp:Content ID="PageBody1" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">个人中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> <a href="#">个人中心</a>>> 事务提醒
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 30px;">
                <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                        <% =Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 254)%>
                    </tr>
                </table>
            </div>
            <div class="btnbox" style="height: 10px; margin: 0px; padding: 0px">
            </div>
            <table cellpadding="0" cellspacing="0" style="width: 100%; margin-top: 10px; margin-bottom: 10px; border: 0px;">
                <tr>
                    <td>
                        <!--search-->
                    </td>
                </tr>
            </table>
            <div class="tablelist" runat="server" id="tbShow">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr style="height: 27px; background: #BDDCF4; text-align:center; font-weight:bold">
                        <td>
                            序号
                        </td>
                        <td>
                            团号
                        </td>
                        <td>
                            线路名称
                        </td>
                        <td>
                            订单号
                        </td>
                        <td>
                            客户单位
                        </td>
                        <td>
                            人数
                        </td>
                        <td class="money">
                            <span>订单金额</span>
                        </td>
                        <td>
                            订单状态
                        </td>
                        <td>
                            查看
                        </td>
                    </tr>
                    <asp:Repeater runat="server" ID="rpt" OnItemDataBound="rtp_ItemDataBound">
                        <ItemTemplate>
                            <tr style="text-align:center;height:27px;background:<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td>
                                    <%#(PageIndex-1)*PageSize + Container.ItemIndex+1 %>
                                </td>
                                <td>
                                    <%#Eval("TourCode") %>
                                </td>
                                <td>
                                    <%#Eval("RouteName") %>
                                </td>
                                <td>
                                    <%#Eval("OrderCode") %>
                                </td>
                                <td>
                                    <%#Eval("KeHuMingCheng") %>
                                </td>
                                <td>
                                    <%#Eval("RenShu") %>
                                </td>
                                <td class="money">
                                    <span><%#Eval("JinE","{0:C2}") %></span>
                                </td>
                                <td>
                                    <asp:Literal runat="server" ID="ltrOrderStatus"></asp:Literal>
                                </td>
                                <td>
                                    <a href="/sales/order_list.aspx?orderid=<%#Eval("OrderId") %>">查看</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div style="text-align: center; margin-top: 10px; margin-bottom: 10px;" runat="server" id="divEmpty" visible="false">
                暂无订单提醒。
            </div>
            <div style="text-align: center; margin-top: 10px;" class="digg" runat="server" id="divPaging">
                <div class="diggPage">
                    <cc1:exportpageinfo id="paging" currencypagecssclass="RedFnt" linktype="4" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:content>
