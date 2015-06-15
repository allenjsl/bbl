<%@ Page Language="C#" Title="ɢ��ȷ�ϵ�" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="VisitorsConfirmPrint.aspx.cs" Inherits="Web.print.wh.VisitorsConfirmPrint" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <style>
        .underlineTextBox
        {
            text-align: left;
        }
    </style>
    <table width="675px" class="table_noneborder" align="center">
        <tr>
            <td colspan="3" align="center">
                <b><font size="4">���ڽӴ�<asp:Label ID="lblTourNum" runat="server" Text=""></asp:Label>
                    �Ŷ�ɢ�͵�ȷ�ϵ�</font> </b>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" class="table_noneborder">
                    <tr>
                        <td width="60px">
                            <b>���յ�λ</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToCompany" runat="server" CssClass="underlineTextBox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <b>����</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToName" runat="server" CssClass="underlineTextBox" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <b>����</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToFax" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                        <td>
                            <b>�绰</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToTel" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>������</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromCompany" runat="server" CssClass="underlineTextBox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <b>����</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromName" runat="server" CssClass="underlineTextBox" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <b>����</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromFax" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                        <td>
                            <b>�绰</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromTel" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;&nbsp;&nbsp;&nbsp;���ݹ���Ķ���Ҫ�󣬿��˲μ�ɢƴ�ŵ��г̼������׼�������£�����ϸ�˶������Ŀ�����ã�������������ȷ�Ϻ󣬻ش����ҹ�˾��лл����֧�֣�
            </td>
        </tr>
        <tr>
            <td height="25px" width="300px">
                <b>��·����:</b>
                <asp:TextBox ID="txtAreaName" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
            <td style="width: 187px">
            </td>
            <td width="150px">
                <b>�� ��:</b><asp:TextBox ID="txtDayCount" runat="server" CssClass="underlineTextBox"
                    Width="47px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                <strong>������ͨ:</strong>
                <asp:TextBox ID="txtLTraffic" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
            <td colspan="2">
                <strong>���̽�ͨ:</strong>
                <asp:TextBox ID="txtRTraffic" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <b>���ŷ�ʽ:</b><asp:TextBox ID="TextBox3" runat="server" CssClass="underlineTextBox"
                    Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <b>���ŷ�ʽ:</b><asp:TextBox ID="TextBox4" runat="server" CssClass="underlineTextBox"
                    Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25px" width="300px">
                <b>������:</b>
                <asp:TextBox ID="txtSendMan" runat="server" CssClass="underlineTextBox" Width="244px"></asp:TextBox>
            </td>
            <td colspan="2">
                <b>���ŵ绰:</b><asp:TextBox ID="txtSendTel" runat="server" CssClass="underlineTextBox"
                    Width="237px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <strong>��������:
                    <asp:TextBox ID="txtPeopleCount" runat="server" CssClass="underlineTextBox" Width="85px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="Placeholder20">
        &nbsp;</div>
    <table width="675px" class="table_normal2" align="center">
        <tr style="border: 1px solid #000;">
            <td width="75px" align="center" rowspan="<%=visitorRowsCount+1 %>">
                <strong>������Ϣ��</strong>
            </td>
            <asp:Repeater ID="rptCustomerList" runat="server">
                <ItemTemplate>
                    <tr style="border: 1px solid #000;">
                        <td style="width: 25px; text-align: left;">
                            <%#Container.ItemIndex+1%>��
                        </td>
                        <td style="width: 100px;">
                            <%#Eval("VisitorName")%>&nbsp;&nbsp;<%#Eval("VisitorType").ToString()%>/<%#Eval("Sex").ToString()%>
                        </td>
                        <td style="width: 280px;">
                            ֤�����룺
                            <%#Eval("CradNumber").ToString()%>
                        </td>
                        <td style="">
                            ��ϵ��ʽ��
                            <%#Eval("ContactTel")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </table>
    <table width="675px" align="center">
        <tr>
            <td height="50px" valign="middle" align="left">
                <p>
                    <strong>&nbsp;�г̰���</strong>
                </p>
            </td>
        </tr>
    </table>
    <asp:Panel ID="tblXCFast" runat="server">
        <table width="675px" align="center" class="table_normal2">
            <tr>
                <td height="50px" valign="middle">
                    <asp:Localize ID="lclXingCheng" runat="server"></asp:Localize>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="tblXingCheng" runat="server">
        <table width="675px" class="table_normal2" align="center">
            <asp:Repeater runat="server" ID="xc_list">
                <ItemTemplate>
                    <tr>
                        <td width="25%" height="25px" valign="middle">
                            <strong>��
                                <%#Container.ItemIndex+1 %>
                                ��&nbsp;&nbsp;
                                <%#getDate(Container.ItemIndex)%></strong>
                        </td>
                        <td width="40%" valign="middle">
                            <strong>
                                <%#Eval("Interval")%>-<%#Eval("Vehicle")%></strong>
                        </td>
                        <td width="10%" valign="middle">
                            <strong>ס��<%#Eval("Hotel")%></strong>
                        </td>
                        <td width="10%" valign="middle">
                            <strong>�ͣ�<%#getEat(Eval("Dinner").ToString())%></strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%#Eval("Plan")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <table width="675px" align="center">
        <tr>
            <td height="50px" valign="middle">
                <p>
                    <strong>&nbsp;�����׼��˵�� </strong>
                </p>
            </td>
        </tr>
    </table>
    <asp:Panel ID="tblFastService" runat="server">
        <table width="675px" align="center" class="table_normal2">
            <tr>
                <td height="50px" valign="middle">
                    <asp:Localize ID="lclService" runat="server"></asp:Localize>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="tblNoService" runat="server">
        <table width="675px" class="table_normal2" align="center">
            <tbody>
                <tr>
                    <td width="86" align="right">
                        ������Ŀ��
                    </td>
                    <td width="604">
                        <asp:Repeater ID="rptProject" runat="server">
                            <ItemTemplate>
                                <%#Eval("ServiceType").ToString() %>��<%#Eval("Service").ToString() %>
                                <br>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ������˵����
                    </td>
                    <td width="604">
                        <strong>����</strong>��<asp:Label ID="lblManPrice" runat="server" Text=""></asp:Label>
                        Ԫ/��*<asp:Label ID="lblManCount" runat="server" Text=""></asp:Label>
                        ��+<strong>��ͯ��</strong><asp:Label ID="lblChildPrice" runat="server" Text=""></asp:Label>
                        Ԫ/��*<asp:Label ID="lblChildCount" runat="server" Text=""></asp:Label>
                        �ˣ�<strong>�ϼƣ�</strong><asp:Label ID="lblAllPrice" runat="server" Text=""></asp:Label>
                        Ԫ
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ������Ŀ��
                    </td>
                    <td>
                        <asp:Label ID="lblNoPriject" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ͯ���ţ�
                    </td>
                    <td>
                        <asp:Label ID="lblChildren" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ���ﰲ�ţ�
                    </td>
                    <td>
                        <asp:Label ID="lblShop" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        �Է���Ŀ��
                    </td>
                    <td>
                        <asp:Label ID="lblZiFei" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ע�����
                    </td>
                    <td>
                        <asp:Label ID="lblZhuyi" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        ��ܰ���ѣ�
                    </td>
                    <td>
                        <asp:Label ID="lblTiXing" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <table width="660px" class="table_noneborder" align="center">
        <tr>
            <td height="25px">
                <strong>�����ȷ����������&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;��ǰ��Ǯ�����ҹ�˾�������ʺţ������&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;��ǰ�ҹ�˾û���յ���
                ��&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;����ÿ�ռ�      3%����Ϣ:</strong>
            </td>
        </tr>
    </table>
    <table width="675px" class="table_noneborder" align="center">
        
            
                <asp:Label ID="lblCompanyName" runat="server" Text="" visible=false></asp:Label>
            <asp:Label ID="lblBankName" runat="server" Text=""  visible=false></asp:Label>
           <asp:Label ID="lblBankUserName" runat="server" Text="" visible=false></asp:Label>
           <asp:Label ID="lblBankNum" runat="server" Text="" visible=false></asp:Label>
           <tr>
             <td width="200px">��˾ȫ�ƣ��㽭�ʵ����������޹�˾</td>
            <td width="200px">�����У���ͨ���к��ݳǱ�֧��</td>
            <td width="250px"> &nbsp;�ʺţ�331066100018010042682</td>
            </tr>
       
        <tr>
            <td>
                &nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;�����ű�
            </td>
            <td >
            �����У��������� ����</td>
            <td>
                &nbsp;�ʺţ�6227 0015 4180 0293 354
                
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;�����ű�
            </td>
            <td>
            �����У��������� ĵ����</td>
            <td>
                &nbsp;�ʺţ�622202 1202024799174
                
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;��&nbsp;&nbsp;&nbsp;&nbsp;�����ű�
            </td>
            <td>
            �����У�ũҵ���� ���뿨</td>
            <td>
                &nbsp;�ʺţ�62284 8032 09784 20414
                
            </td>
        </tr>
    </table>
    <table width="675px" class="table_noneborder" align="center">
        <tr>
            <td width="346">
                ��������£�
                <input type="text" size="30" value="" class="underlineTextBox" />
                <br />
                ��ϵ�绰��
                <input type="text" size="15" value="" class="underlineTextBox" />
                <br />
                ȷ�����ڣ�
                <input type="text" size="15" value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" class="underlineTextBox">
            </td>
            <td width="350">
                ר���̸��£�
                <asp:TextBox ID="txtCompany" runat="server" CssClass="underlineTextBox"></asp:TextBox>
                <br>
                ��ϵ�绰��
                <asp:TextBox ID="txtContactTel" runat="server" CssClass="underlineTextBox"></asp:TextBox>
                <br>
                ȷ�����ڣ�
                <input type="text" size="15" value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" class="underlineTextBox">
            </td>
        </tr>
    </table>
</asp:Content>
