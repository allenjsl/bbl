<%@ Page Title="�г̵�" Language="C#" AutoEventWireup="true" CodeBehind="StandardSanFightPrint.aspx.cs"
    Inherits="Web.print.wh.StandardSanFightPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="759" align="center"  class="table_noneborder">
        <tr>
            <td height="30">
                <table width="100%"  class="table_normal2">
                    <tr>
                        <th height="25" colspan="3" align="center" >
                            ��·���ƣ�<asp:Label ID="lblAreaName" runat="server" Text=""> </asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td width="40%" height="25" align="left" >
                                �� �ţ�<asp:Label ID="lblTeamNum" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="txttourcode" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td width="30%" align="left" >
                            ����������<asp:Label ID="lblDayCount" runat="server" Text=""></asp:Label>��
                        </td>
                        <td width="30%" align="left" >
                                �� ����<asp:Label ID="lblAdult" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="TXTADULT" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="left" >
                                ������ͨ��
                                <asp:Label ID="lblBegin" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="TXtBEGIN" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td colspan="2" align="left" >
                                ���̽�ͨ ��
                                <asp:Label ID="lblEnd" runat="server" Text=""></asp:Label>
                                <asp:TextBox ID="TXTEnd" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left">
                ���г̰���
            </th>
        </tr>
        <tr>
            <td height="30">
                <table width="100%"  class="table_normal2">
                    <tr>
                        <td width="10%" height="25" align="center" bgcolor="#FFFFFF">
                            ����
                        </td>
                        <td width="60%" align="center" bgcolor="#FFFFFF">
                            �г�����
                        </td>
                        <td width="10%" align="center" bgcolor="#FFFFFF">
                            ס��
                        </td> 
                        <td width="10%" align="center" bgcolor="#FFFFFF">
                            �ò�
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTravel" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" bgcolor="#FFFFFF">
                                    <%#GetDateByIndex(Container.ItemIndex)%>
                                </td>
                                <td align="left" bgcolor="#FFFFFF">
                                    <%#Eval("Plan")%>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    <%#Eval("Hotel")%>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
                �������׼
            </th>
        </tr>
        <tr>
            <td height="30">
                <table width="100%"  class="table_normal">
                    <tr>
                        <td width="9%" height="25" align="center"  class="normaltd">
                            ������Ŀ��
                        </td>
                        <td style="padding:0px;margin:0px"  class="normaltd">
                            <table width="100%" class="table_noneborder">
                                <tr>
                                    <td width="8%" height="25" align="left">
                                        <b>��Ŀ</b>
                                    </td>
                                    <td width="82%" align="left" >
                                        <b>�Ӵ���׼</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptProject" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td width="8%" height="25" align="left" >
                                                <%#Eval("ServiceType")%>
                                            </td>
                                            <td width="82%" align="left" >
                                                <%#Eval("Service")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            ������Ŀ��
                        </td>
                        <td   class="normaltd" align="left">
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd">
                            ���ﰲ�ţ�
                        </td>
                        <td  class="normaltd" align="left">
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"   class="normaltd">
                            ��ͯ���ţ�
                        </td>
                        <td  class="normaltd" align="left">
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            �Է���Ŀ��
                        </td>
                        <td align="left"  class="normaltd">
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            ע�����
                        </td>
                        <td align="left"  class="normaltd">
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
                ����ܰ����
            </th>
        </tr>
        <tr>
            <td height="30" style="border: 1px #000000 solid;" align="left">
                <asp:Label ID="lblTips" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <asp:Panel ID="Panel1" runat="server">
            <tr>
                <th height="30" align="left">
                    ���ؽ�����Ϣ
                </th>
            </tr>
            <tr>
                <td height="30">
                    <table  class="table_normal2" align="center"
                        width="100%">
                        <tbody>
                            <tr>
                                <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                    <b>��������</b>
                                </td>
                                <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                    <b>���֤��</b>
                                </td>
                                <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                    <b>�绰</b>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptDjInfo" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                            <%#Eval("Name") %>
                                        </td>
                                        <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                            <%#Eval("LicenseNo") %>
                                        </td>
                                        <td height="20" bgcolor="#ffffff" align="center" width="33%">
                                            <%#Eval("Telephone") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
        </asp:Panel>
    </table>
</asp:Content>
