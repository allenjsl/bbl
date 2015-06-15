<%@ Page Title="地接确认单" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="GroundConfirmPrint.aspx.cs" Inherits="Web.print.wh.GroundConfirmPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal" width="750px">
        <tbody>
            <tr>
                <td class="normaltd" style="padding: 0px; margin: 0px" height="30" align="center"
                    valign="top">
                    <table class="table_noneborder" align="center" width="100%">
                        <tbody>
                            <tr>
                                <td class="td_r_b_border" align="center" valign="top">
                                    接待社
                                </td>
                                <td class="td_r_b_border" align="center" valign="top">
                                    联系人
                                </td>
                                <td class="td_r_b_border" align="center" valign="top">
                                    电话
                                </td>
                                <td class="td_b_border" align="center" valign="top">
                                    传真
                                </td>
                            </tr>
                            <asp:Repeater ID="rpt_area" runat="server" OnItemDataBound="rpt_area_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td class="td_r_b_border" align="center" valign="top">
                                            <%#Eval("LocalTravelAgency")%>
                                        </td>
                                        <td class="td_r_b_border" align="center" valign="top">
                                            <asp:Literal ID="lt_cName" runat="server"></asp:Literal>
                                        </td>
                                        <td class="td_r_b_border" align="center" valign="top">
                                            <asp:Literal ID="lt_cTel" runat="server"></asp:Literal>
                                        </td>
                                        <td class="td_b_border" align="center" valign="top">
                                            <asp:Literal ID="lt_cFax" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td class="td_r_border" align="center" valign="top">
                                    团号：
                                    <asp:Label ID="lblTourCode" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="td_r_border" align="center" valign="top">
                                    <nobr> 人数：<asp:TextBox ID="txtDate1" runat="server" CssClass="nonelineTextBox"></asp:TextBox></nobr>
                                </td>
                                <td align="center" valign="top">
                                    <nobr>接待时间：<asp:TextBox ID="txtDate0" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                                    </nobr>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="normaltd" style="padding: 0px; margin: 0px" height="30" align="center"
                    valign="top">
                    <table  class="table_noneborder" align="center" width="100%">
                        <tbody>
                            <tr runat="server" id="tr_Treval">
                                <td class="td_r_b_border" align="center" width="10%">
                                    日期
                                </td>
                                <td class="td_r_b_border" align="center" width="82%">
                                    行程
                                </td>
                                <td class="td_b_border" align="center" width="8%">
                                    住宿
                                </td>
                            </tr>
                            <tr runat="server" visible="false" id="trXc">
                                <td colspan="3" class="td_b_border">
                                    <asp:Literal ID="lt_xc" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptTravel" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="td_r_b_border" height="25" align="center">
                                            <%#GetDateByIndex(Container.ItemIndex)%>
                                        </td>
                                        <td class="td_r_b_border" align="left">
                                            <%#Eval("Plan")%>
                                        </td>
                                        <td class="td_b_border" align="center">
                                            <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="3" class="td_b_border" align="left">
                                    &nbsp;<asp:Label ID="lblMsg" runat="server" Text="标准及接待要求："></asp:Label>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptProject" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="td_b_border" colspan="3" height="25" align="left">
                                            <%#Container.ItemIndex+1 %>.
                                            <%#Eval("ServiceType")%>:
                                            <%#Eval("Service")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="3" class="td_b_border" align="left">
                                    &nbsp;<strong>★此团比较重要，请千万保证团队质量</strong>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"  class="td_b_border" align="left">
                                    &nbsp;游客联系方式及要求：
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="500px" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="td_b_border" align="left">
                                    &nbsp;<strong>请告知住宿酒店：</strong>
                                    <asp:TextBox ID="TextBox3" runat="server" Width="500px" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="td_b_border" align="left">
                                    &nbsp;<strong>请告知导游：</strong>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="500px" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding: 0px; margin: 0px" align="center">
                                    <table class="table_noneborder" align="center" width="100%" id="tblMoney">
                                        <tbody>
                                            <tr>
                                                <td >
                                                    <strong>
                                                        <asp:Label ID="lblMoney" runat="server" Text="费用结算："></asp:Label><span id="span_Money"><asp:TextBox
                                                            ID="txt_priceSum" runat="server" Width="500px" CssClass="nonelineTextBox"></asp:TextBox></span></strong>
                                                </td>
                                            </tr>
                                            <asp:Literal ID="litList" runat="server"></asp:Literal>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="padding:0px;margin:0px"  align="left" >
                                    <table align="center" width="100%">
                                        <tbody>
                                            <tr>
                                                <td class="td_t_r_border" align="left" width="50%" >
                                                    &nbsp;发件人：张奔13396585565
                                                </td>
                                                <td class="td_t_border" align="left" width="50%">
                                                    &nbsp;发件日期：<asp:TextBox ID="txtDate" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_t_r_border" align="left">
                                                    &nbsp;电话：0571-87981506
                                                </td>
                                                <td class="td_t_border" align="left">
                                                    &nbsp;传真：0571-87981576
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
