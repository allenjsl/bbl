<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderVisitorsListPrint.aspx.cs" Inherits="Web.print.normal.OrderVisitorsListPrint" MasterPageFile="~/masterpage/Print.Master" Title="�ο�����" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal2" width="759" cellspacing="0" cellpadding="0" border="none"
        align="center">
        <tr height="30">
            <td align="center" width="50px;">
                �źţ�
            </td>
            <td width="140px;">
                &nbsp;<asp:Literal ID="ltrTourNumber" runat="server"></asp:Literal>
            </td>
            <td align="center" width="90px;">
                ��·���ƣ�
            </td>
            <td width="240px">
                &nbsp;<asp:Literal ID="ltrRouteName" runat="server"></asp:Literal>
            </td>
            <td align="center" width="50px;">
                ������
            </td>
            <td width="70px">
                &nbsp;<asp:Literal ID="ltrDays" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <div class="Placeholder10">
        &nbsp;</div>
    <table class="table_normal2" width="759" cellspacing="0" cellpadding="0" border="none"
        align="center">
        <tr height="30">
            <td style="width: 5%" align="center">
                ���
            </td>
            <td align="center" width="120px;">
                ����
            </td>
            <td align="center" width="80px;">
                �Ա�
            </td>
            <td align="center" width="130px;">
                ֤������
            </td>
            <td align="center" width="200px">
                ֤������
            </td>
            <td align="center" width="110px;">
                ��ϵ�绰
            </td>
        </tr>
        <cc1:CustomRepeater ID="rptVisitorList" runat="server" EmptyText="<tr height='30'><td colspan='6'>���ο�����</td></tr>">
            <ItemTemplate>
                <tr height="30">
                    <td align="center">
                        <%#Container.ItemIndex+1%>
                    </td>
                    <td align="center" width="120px;">
                        &nbsp;<%#Eval("VisitorName")%>
                    </td>
                    <td align="center" width="80px;">
                        &nbsp;<%#Eval("Sex")%>
                    </td>
                    <td align="center" width="130px;">
                        &nbsp;<%#Eval("CradType")%>
                    </td>
                    <td align="center" width="200px">
                        &nbsp;<%#Eval("CradNumber")%>
                    </td>
                    <td align="center" width="110px;">
                        &nbsp;<%#Eval("ContactTel")%>
                    </td>
                </tr>
            </ItemTemplate>
        </cc1:CustomRepeater>
    </table>
</asp:Content>
