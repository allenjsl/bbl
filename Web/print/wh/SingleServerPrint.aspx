<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleServerPrint.aspx.cs"
    Inherits="Web.print.wh.SingleServerPrint" MasterPageFile="~/masterpage/Print.Master"
    Title="单项服务确认单" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="Content1" runat="server">
    <style>
        input.short
        {
            width: 60px;
        }
        input.middle
        {
            width: 260px;
        }
    </style>
    <table width="760px" class="table_normal2">
        <tr>
            <td colspan="2">
                <strong>TO:
                    <input type="text" id="txtTo" runat="server" class="underlineTextBox" style="width: 80%" /></strong>
            </td>
            <td colspan="2">
                <strong>FR:
                    <input type="text" id="txtFr" runat="server" class="underlineTextBox" /></strong>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>TEL:
                    <input type="text" runat="server" id="txtTel" class="underlineTextBox" style="width: 80%" /></strong>
            </td>
            <td colspan="2">
                <strong>TEL:
                    <input type="text" runat="server" id="txtTelS" class="underlineTextBox" /></strong>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>FAX:
                    <input type="text" runat="server" id="txtFax" class="underlineTextBox" style="width: 80%" /></strong>
            </td>
            <td colspan="2">
                <strong>FAX:
                    <input type="text" runat="server" id="txtFaxS" class="underlineTextBox" /></strong>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <strong>人数:
                    <input type="text" class="short underlineTextBox" id="PelpeoNumber" runat="server" />人</strong>
            </td>
            <td colspan="2">
                <strong>确认时间：
                    <input type="text" class="underlineTextBox" id="ConfirmDate" runat="server" /></strong>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="left" valign="middle" style="padding: 0; margin: 0; border-bottom: solid 0px">
                <p>
                    <strong>供应商安排：</strong></p>
                <table width="100%" style="border: solid 0px;">
                    <asp:Repeater ID="Repeaterlist" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" valign="middle" style="border-top: solid 1px black; border-right: solid 0px #fff">
                                    <strong>服务类别：</strong><%#Eval("ServiceType").ToString()%><br />
                                    <strong>供应商：</strong><%#Eval("SupplierName")%><br />
                                    <strong>具体安排：</strong><%#Eval("Arrange")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Panel ID="pnlIsShow" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="border-top: solid 1px black; border-right: solid 0px #fff">
                                <asp:Label ID="lblServiceType" runat="server" Text=""></asp:Label><br />
                                <asp:Label ID="lblSupplierName" runat="server" Text=""></asp:Label><br />
                                <asp:Label ID="lblArrange" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="left" valign="middle">
                <p>
                    <strong>游客名单：</strong></p>
                <div style="width: 100%">
                    <asp:Repeater ID="repCustomer" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td style="width: 33%; border: solid 0px #fff; text-align: left;">
                                        姓 名
                                    </td>
                                    <td style="width: 33%; margin-left: 30px; border: solid 0px #fff; text-align: left;">
                                        类 型
                                    </td>
                                    <td style="width: 33%; margin-left: 30px; border: solid 0px #fff; text-align: left;">
                                        证件号码
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="width: 33%; border: solid 0px #fff; text-align: left;">
                                    <%# Eval("VisitorName")%>
                                </td>
                                <td style="width: 33%; margin-left: 30px; border: solid 0px #fff; text-align: left;">
                                    <%# Eval("VisitorType").ToString()%>
                                </td>
                                <td style="width: 33%; margin-left: 30px; border: solid 0px #fff; text-align: left;">
                                    <%# Eval("CradNumber")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table></FooterTemplate>
                    </asp:Repeater>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="middle" style="width: 30%">
                <strong>帐号：</strong>
            </td>
            <td align="left" valign="middle" style="width: 70%" colspan="3">
                户名：张奔<br />
                帐号：<br />
                建行6227 0015 4180 0293 354<br />
                工行622202 1202024799174<br />
                农行62284 8032 09784 20414
            </td>
        </tr>
    </table>
</asp:Content>
