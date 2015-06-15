<%@ Page Title="团队报价单" Language="C#" AutoEventWireup="true" CodeBehind="TourQuotePrint.aspx.cs"
    Inherits="Web.print.bbl.TourQuotePrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="760px" class="table_noneborder">
        <tr>
            <td>
                <table width="100%" class="table_normal2">
                    <tr>
                        <td colspan="6" align="center" style="border-bottom: none;">
                            <span style="font-size: 20px; font-weight: bold;">
                                <%=SiteUserInfo.SysId == 3 ? "诚 信 假 期 ":" 我 爱 中 华 "%>
                                团 队 报 价 单</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" colspan="6" align="right">
                            报团人：
                            <input type="text" name="textfield" id="textfield" />
                            报团日期：
                            <input type="text" name="textfield" id="textfield" />
                        </td>
                    </tr>
                    <tr>
                        <td height="35" width="10%" align="center">
                            组 团 社
                        </td>
                        <td width="22%" align="center">
                            <asp:TextBox ID="txtGroups" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td width="8%" align="center">
                            联系人
                        </td>
                        <td  align="center">
                            <asp:TextBox ID="txtContact" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td width="8%" align="center">
                            电 话
                        </td>
                        <td width="23%" align="center">
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            预计出团日期
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtOutDate" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td align="center" >
                            人 数
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtCount" runat="server" CssClass="nonelineTextBox" Width="100%"></asp:TextBox>
                        </td>
                        <td align="center">
                            传 真
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtFax" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            线路名称
                        </td>
                        <td align="left" colspan="5">
                            <asp:TextBox ID="txtAreaName" runat="server" CssClass="nonelineTextBox" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlSpecific" runat="server" Height="20px">
                    <b>·具体要求</b>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlContent" runat="server">
                    <table width="100%" class="table_normal2" style="margin: 0; padding: 0;">
                        <tr>
                            <th width="13%" align="center" style="border-left: solid 1px #000">
                                <strong>项目</strong>
                            </th>
                            <th height="25" align="center">
                                <strong>具体要求</strong>
                            </th>
                            <th align="center">
                                <strong>项目</strong>
                            </th>
                            <th align="center">
                                <strong>具体要求</strong>
                            </th>
                        </tr>
                        <tr>
                            <asp:Repeater ID="rptRequire" runat="server">
                                <ItemTemplate>
                                    <td height="25" align="center" style="border-left: solid 1px #000">
                                        <%#Eval("ServiceType").ToString()%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Service")%>
                                    </td>
                                    <%#IsOutTrOrTd(Container.ItemIndex,this.RecordSum,2)%>
                                </ItemTemplate>
                            </asp:Repeater>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="30">
                <b>·行程安排</b>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlFrist" runat="server">
                    <table width="100%" class="table_normal2">
                        <tr>
                            <td width="15%" height="25" align="center">
                                日期
                            </td>
                            <td width="49%" align="center">
                                行程描述
                            </td>
                            <td width="18%" align="center">
                                住宿
                            </td>
                            <td width="18%" align="center">
                                用餐
                            </td>
                        </tr>
                        <asp:Repeater ID="rptTravel" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td height="25" align="center">
                                        <%#Convert.ToDateTime(this.LeaveTime.AddDays(Container.ItemIndex)).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="left">
                                        <%#Eval("Plan")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Hotel")%>
                                    </td>
                                    <td align="center">
                                        <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlSecond" runat="server">
                    <table width="100%" class="table_normal2">
                        <tr>
                            <td>
                                <asp:Label ID="lblSecond" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <b>·地接报价</b>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" class="table_normal2">
                    <tr>
                        <th width="25%" height="25" align="center">
                            项目
                        </th>
                        <th width="25%" align="center">
                            具体报价
                        </th>
                        <th width="25%" align="center">
                            项目
                        </th>
                        <th width="25%" align="center">
                            具体报价
                        </th>
                    </tr>
                    <tr>
                        <asp:Repeater ID="rptDjList" runat="server">
                            <ItemTemplate>
                                <td align="center" style="height: 24px">
                                    <%#Eval("ServiceType").ToString() %>
                                </td>
                                <td align="center" style="height: 24px">
                                    <%#Convert.ToDecimal(Eval("SelfPrice")).ToString("0.00")%>
                                </td>
                                <%#IsOutTrOrTd(Container.ItemIndex,this.RecordSumS,2)%>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <th height="25" align="center">
                                合计：
                            </th>
                            <td colspan="3" align="center">
                                <asp:Label ID="lblAllPrice" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="30">
                <b>·机票折扣</b>
            </td>
        </tr>
        <tr>
            <td height="30" style="border: solid 1px #000;">
                <asp:TextBox ID="txtTicket" runat="server" CssClass="underlineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="30">
                <b>·报 价</b>
            </td>
        </tr>
        <tr>
            <td style="border: solid 1px #000;">
                <asp:TextBox ID="txtBaoJia" runat="server" CssClass="underlineTextBox" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="30">
                <b>·回访情况（已确认或取消原因）</b>
            </td>
        </tr>
        <tr>
            <td style="border: solid 1px #000; text-align: left;">
                <textarea name="textarea" id="textarea" style="width: 99%; overflow: auto;"></textarea>
            </td>
        </tr>
    </table>
</asp:Content>
