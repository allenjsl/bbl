<%@ Page Title="望海送团通知书" Language="C#" MasterPageFile="~/masterpage/Print.Master"
    AutoEventWireup="true" CodeBehind="GoNotice.aspx.cs" Inherits="Web.print.wh.GoNotice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_noneborder" align="center" width="759">
        <tr>
            <td align="center" height="50">
                <asp:Label ID="lblRouteName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="border: none">
                <table class="table_normal2" align="center" width="100%">
                    <tr>
                        <td align="left" colspan="3" height="25">
                            <strong>出发交通：</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblGoTraffic" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <strong>返程交通：</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblEndTraffic" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlProject" runat="server" Width="100%">
                        <asp:Repeater ID="xc_list" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="left" height="25" style="padding-left: 10px;" width="33%">
                                        <strong>第
                                            <%#Container.ItemIndex+1%>天</strong>&nbsp;&nbsp;&nbsp;
                                        <%#((DateTime)xcTime).AddDays((int)Container.ItemIndex).ToString("yyyy-MM-dd")%>
                                        <%# EyouSoft.Common.Utils.ConvertWeekDayToChinese(((DateTime)xcTime).AddDays((int)Container.ItemIndex))%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="16%">
                                        <strong>
                                            <%#Eval("Interval")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="15%">
                                        <strong>交通:<%#Eval("Vehicle")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="19%">
                                        <strong>住：<%#Eval("Vehicle")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="17%">
                                        <strong>餐：<%#getEat(Eval("Dinner").ToString())%></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="5" height="25" style="padding-left: 10px;">
                                        &nbsp;&nbsp;<%#Eval("Plan")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div class="Placeholder20">
                    &nbsp;</div>
                <table class="table_normal" align="center" width="100%">
                    <tr>
                        <td class="normaltd" align="left" height="30" colspan="2">
                            <strong>服务标准及说明：</strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="30" valign="top">
                            包含项目：
                        </td>
                        <td class="normaltd" style="padding: 0px; margin: 0px" align="left" height="25" width="89%">
                            <table width="100%" class="table_noneborder">
                                <asp:Repeater ID="rpt_sList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left" height="25" style="padding-left: 10px;">
                                                <%# (Container.ItemIndex+1)+"、"+Eval("ServiceType").ToString()+"："+Eval("Service").ToString()%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            结算价：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:TextBox ID="txtTotalAmount" runat="server" class="underlineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            不含项目：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblBuHanXiangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            自费项目：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblZiFeiXIangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            儿童安排：
                        </td>
                        <td class="normaltd" align="left" style="padding-left: 10px;">
                            <asp:Label ID="lblErTongAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            购物安排：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblGouWuAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            赠送项目：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:TextBox ID="txtPresent" runat="server" class="underlineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            注意事项：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblZhuYiShiXiang" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <asp:Panel ID="xcquick" runat="server">
                    <table class="table_normal2" align="center" width="100%">
                        <tr>
                            <td align="left">
                                行程安排：<br />
                                &nbsp;<asp:Label ID="lblQuickPlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                服务标准及说明：<br />
                                &nbsp;<asp:Label ID="lblKs" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <div class="Placeholder20">
        &nbsp;</div>
    <table align="center" class="table_normal" width="759">
        <tr>
            <td align="left" class="normaltd" style="font-size: 14px; font-weight: bold;">
                参考去程航班时间：<asp:Label ID="lblLDate" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblSentPeoples" runat="server" Text=""></asp:Label><br />
                <%--联系人--%>
                集合时间：<asp:Label ID="lblGatheringTime" runat="server" Text=""></asp:Label><br />
                集合地点：<asp:Label ID="lblGatheringPlace" runat="server" Text=""></asp:Label><br />
                集合标志：<asp:Label ID="lblGatheringSign" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <asp:Repeater ID="rptlist" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="left" class="table_r_border" style="font-size: 14px; font-weight: bold;">
                        接待处：<%#Eval("Name") %><br />
                        导游：<asp:TextBox ID="TextBox1" runat="server" class="underlineTextBox"></asp:TextBox><br />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td align="left"  class="normaltd" style=" border-top:1px solid #000; font-size: 14px; font-weight: bold;" colspan="2">
                参考回程航班时间：<asp:Label ID="lblRTraffic" runat="server" Text=""></asp:Label><br />
                <u>紧急联系方式：张先生13396585565、宋小姐18868700650</u>
            </td>
        </tr>
    </table>
</asp:Content>
