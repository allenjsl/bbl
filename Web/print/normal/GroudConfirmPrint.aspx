<%@ Page Title="�ؽ�ȷ�ϵ�" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="GroudConfirmPrint.aspx.cs" Inherits="Web.print.normal.GroudConfirmPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <div style="font-size: 14px; font-weight: bold;">
        <div>
            TO��<asp:TextBox ID="txt_to_name" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                ID="txt_to_user" Style="width: 60px;" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                    ID="txt_to_tel" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;���棺<asp:TextBox
                        ID="txt_to_fax" CssClass="underlineTextBox" runat="server"></asp:TextBox>
        </div>
        <div>
            FR��<asp:TextBox ID="txt_fr_name" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                ID="txt_fr_user" Style="width: 60px;" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                    ID="txt_fr_tel" CssClass="underlineTextBox" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;���棺<asp:TextBox
                        ID="txt_fr_fax" CssClass="underlineTextBox" runat="server"></asp:TextBox>
        </div>
        <p id="p_title" runat="server">
            ��ã��ֽ�����<asp:Literal ID="lt_LDate" runat="server"></asp:Literal>�������������Դ˰��ţ�����֤������ȷ�Ϻ�����»ش���лл!!</p>
    </div>
    <div id="d_title" runat="server" style="border-top: 3px solid #000; border-bottom: 1px solid #000;
        height: 1px; overflow: hidden; _zoom: 1;">
    </div>
    <h1 style="color: Blue; text-align: center; margin: 2px;">
        <asp:Literal ID="lt_TourName" runat="server"></asp:Literal></h1>
    <div id="tb_normal" runat="server" visible="false" style="width: 759px;">
        <table width="100%" class="table_normal2">
            <tr>
                <td width="15%" height="25" align="center">
                    ����
                </td>
                <td width="15%" align="center">
                    ��ͨ
                </td>
                <td width="40%" align="center">
                    �г�����
                </td>
                <td width="18%" align="center">
                    ס��
                </td>
                <td width="18%" align="center">
                    �ò�
                </td>
            </tr>
            <asp:Repeater ID="rptTravel" runat="server">
                <ItemTemplate>
                    <tr>
                        <td height="25" align="center">
                            <%#GetDateByIndex(Container.ItemIndex)%>
                        </td>
                        <td align="center">
                            <%#Eval("Vehicle").ToString()%>
                        </td>
                        <td align="left">
                            <%#TextToHtml(Eval("Plan").ToString())%>
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
    </div>
    <div id="tb_quick" visible="false" runat="server" style="width: 759px;">
        <b>�� �г̰���</b><br />
        <table class="table_normal" width="100%">
            <tr>
                <td class="normaltd">
                    <asp:Label ID="lb_Quick" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div id="tb_single" runat="server" visible="false">
        <asp:Repeater runat="server" ID="rpt_single">
            <HeaderTemplate>
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="15%">
                            �������
                        </td>
                        <td width="15%">
                            ��Ӧ������
                        </td>
                        <td width="auto">
                            ���尲��
                        </td>
                        <td width="15%">
                            �������
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("ServiceType")%>
                    </td>
                    <td>
                        <%#Eval("SupplierName")%>
                    </td>
                    <td>
                        <%#Eval("Arrange")%>
                    </td>
                    <td>
                        <%#Eval("Amount","{0:c}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table></FooterTemplate>
        </asp:Repeater>
    </div>
    <div>
        <asp:Repeater ID="rptProject" runat="server">
            <ItemTemplate>
                <%#Container.ItemIndex+1 %>��<strong>��
                    <%#Eval("ServiceType").ToString()%>
                    ��</strong><%#Eval("Service")%>"
            </ItemTemplate>
        </asp:Repeater>
        <div id="tb_quickService" runat="server" visible="false">
            <b>�� �����׼</b><br />
            <table class="table_normal" width="100%">
                <tr>
                    <td class="normaltd">
                        <asp:Literal ID="lt_service" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="div_noproject" runat="server" visible="false">
        <strong>�� ���۲������ü�����</strong><br />
        <table class="table_normal" width="100%">
            <tr>
                <td class="normaltd">
                    <asp:Literal ID="lblNoProject" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    <div id="div_notice" runat="server" visible="false">
        <div>
            <b>�� ע������</b></div>
        <div>
            <table class="table_normal" width="100%">
                <tr>
                    <td class="normaltd">
                        <asp:Literal ID="lt_notice" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div>
        <asp:PlaceHolder runat="server" ID="phJieSuanJiaGe" Visible="false">
        <b>������ã�</b><br />
            <asp:TextBox ID="txt_Money" CssClass="underlineTextBox" runat="server" Width="100%"></asp:TextBox>
        </asp:PlaceHolder>
    </div>
</asp:Content>
