<%@ Page Title="��������֪ͨ��" Language="C#" MasterPageFile="~/masterpage/Print.Master"
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
                            <strong>������ͨ��</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblGoTraffic" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" colspan="2">
                            <strong>���̽�ͨ��</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblEndTraffic" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel ID="pnlProject" runat="server" Width="100%">
                        <asp:Repeater ID="xc_list" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td align="left" height="25" style="padding-left: 10px;" width="33%">
                                        <strong>��
                                            <%#Container.ItemIndex+1%>��</strong>&nbsp;&nbsp;&nbsp;
                                        <%#((DateTime)xcTime).AddDays((int)Container.ItemIndex).ToString("yyyy-MM-dd")%>
                                        <%# EyouSoft.Common.Utils.ConvertWeekDayToChinese(((DateTime)xcTime).AddDays((int)Container.ItemIndex))%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="16%">
                                        <strong>
                                            <%#Eval("Interval")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="15%">
                                        <strong>��ͨ:<%#Eval("Vehicle")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="19%">
                                        <strong>ס��<%#Eval("Vehicle")%></strong>
                                    </td>
                                    <td align="left" style="padding-left: 10px;" width="17%">
                                        <strong>�ͣ�<%#getEat(Eval("Dinner").ToString())%></strong>
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
                            <strong>�����׼��˵����</strong>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="30" valign="top">
                            ������Ŀ��
                        </td>
                        <td class="normaltd" style="padding: 0px; margin: 0px" align="left" height="25" width="89%">
                            <table width="100%" class="table_noneborder">
                                <asp:Repeater ID="rpt_sList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left" height="25" style="padding-left: 10px;">
                                                <%# (Container.ItemIndex+1)+"��"+Eval("ServiceType").ToString()+"��"+Eval("Service").ToString()%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ����ۣ�
                        </td>
                        <td class="normaltd" align="left">
                            <asp:TextBox ID="txtTotalAmount" runat="server" class="underlineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ������Ŀ��
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblBuHanXiangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            �Է���Ŀ��
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblZiFeiXIangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ��ͯ���ţ�
                        </td>
                        <td class="normaltd" align="left" style="padding-left: 10px;">
                            <asp:Label ID="lblErTongAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ���ﰲ�ţ�
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblGouWuAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ������Ŀ��
                        </td>
                        <td class="normaltd" align="left">
                            <asp:TextBox ID="txtPresent" runat="server" class="underlineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="normaltd" align="center" height="25">
                            ע�����
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
                                �г̰��ţ�<br />
                                &nbsp;<asp:Label ID="lblQuickPlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                �����׼��˵����<br />
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
                �ο�ȥ�̺���ʱ�䣺<asp:Label ID="lblLDate" runat="server" Text=""></asp:Label><br />
                <asp:Label ID="lblSentPeoples" runat="server" Text=""></asp:Label><br />
                <%--��ϵ��--%>
                ����ʱ�䣺<asp:Label ID="lblGatheringTime" runat="server" Text=""></asp:Label><br />
                ���ϵص㣺<asp:Label ID="lblGatheringPlace" runat="server" Text=""></asp:Label><br />
                ���ϱ�־��<asp:Label ID="lblGatheringSign" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <asp:Repeater ID="rptlist" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="left" class="table_r_border" style="font-size: 14px; font-weight: bold;">
                        �Ӵ�����<%#Eval("Name") %><br />
                        ���Σ�<asp:TextBox ID="TextBox1" runat="server" class="underlineTextBox"></asp:TextBox><br />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td align="left"  class="normaltd" style=" border-top:1px solid #000; font-size: 14px; font-weight: bold;" colspan="2">
                �ο��س̺���ʱ�䣺<asp:Label ID="lblRTraffic" runat="server" Text=""></asp:Label><br />
                <u>������ϵ��ʽ��������13396585565����С��18868700650</u>
            </td>
        </tr>
    </table>
</asp:Content>
