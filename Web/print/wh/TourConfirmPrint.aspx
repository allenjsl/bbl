<%@ Page Title="�Ŷ�ȷ�ϵ�" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="TourConfirmPrint.aspx.cs" Inherits="Web.print.wh.TourConfirmPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="759px">
        <tr>
            <td align="center" height="30" valign="top">
                <table align="center" class="table_normal" width="100%">
                    <tr>
                        <td align="center" class="normaltd" colspan="5" height="50" style="font-size: 20px;
                            font-weight: bold;" valign="middle">
                            <strong>����
                                <asp:Label ID="lblTid" runat="server" Text=""></asp:Label>
                                �Ŷ�ɢ�� ��ȷ�ϵ�</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>�����ˣ�</strong><asp:Label ID="lblSetMan" runat="server"></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="22%">
                            <strong>������</strong><asp:Label ID="lblSetName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="21%">
                            <strong>���棺</strong><asp:Label ID="lblSetFAX" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="21%">
                            <strong>�绰��</strong><asp:Label ID="lblSetPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>�����ˣ�</strong><asp:Label ID="lblGetMan" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>������</strong><asp:Label ID="lblGetName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>���棺</strong><asp:Label ID="lblGetFAX" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>�绰��</strong><asp:Label ID="lblGetPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>���ݹ���Ķ���Ҫ�󣬿��˲μ�ɢƴ�ŵ��г̼������׼�������£�����ϸ�˶������Ŀ�����ã�������������ȷ�Ϻ󣬻ش����ҹ�˾��лл����֧�֣�</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="3" style="padding-left: 10px;" valign="top">
                            <strong>��·���ƣ�</strong><asp:Label ID="lblLineName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>������</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label ID="lblDaySum"
                                runat="server" Text=""></asp:Label>��
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="3" style="padding-left: 10px;" valign="top">
                            <strong>������ͨ��</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblGoTraffic" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>���̽�ͨ��</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblEndTraffic" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            ���Ϻ���źͺ���ʱ������ο�������б䶯�������ǰ��֪�����ޱ䶯������֪ͨ��
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>���ŷ�ʽ��</strong>����ʱ��<asp:Label ID="lblGatheringTime" runat="server" Text=""></asp:Label>���ϵص�<asp:Label
                                ID="lblGatheringPlace" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>��������</strong>��<asp:Label ID="lblManSum" runat="server" Text=""></asp:Label><strong>(����)</strong>
                        </td>
                    </tr>
                </table>
                <div class="Placeholder20">
                    &nbsp;</div>
                <table width="100%" class="table_normal2">
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
                                    <strong>
                                        <%#Eval("Vehicle")%></strong>
                                </td>
                                <td align="left" style="padding-left: 10px;" width="19%">
                                    <strong>ס��<%#Eval("Hotel")%></strong>
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
            <td align="center" height="30" valign="top">
                <div id="tabProject20" class="Placeholder20" runat="server">
                    &nbsp;</div>
                <table id="tabProject" runat="server" class="table_normal" width="100%">
                    <tr>
                        <td align="left" height="30" class="normaltd" colspan="2">
                            <strong>�����׼��˵����</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="30" valign="top">
                            ������Ŀ��
                        </td>
                        <td align="left" class="normaltd" height="25" style="padding-left: 10px;" width="89%">
                            <table width="100%" style="padding: 0px; margin: 0px">
                                <asp:Repeater ID="rpt_sList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <% li++; %>
                                            <td align="left" class='<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex(li, 1, sListRowsCount )%>'
                                                height="25" style="padding-left: 10px;">
                                                <%# (Container.ItemIndex+1)+"��"+Eval("ServiceType").ToString()+"��"+Eval("Service").ToString()%>
                                                &nbsp;&nbsp;���ۣ���&nbsp;<%#Convert.ToDecimal(Eval("SelfUnitPrice")).ToString("0.00")%>&nbsp;��������&nbsp;<%#Eval("SelfPeopleNumber")%>
                                                &nbsp; �ܼƣ���&nbsp;
                                                <%#Convert.ToDecimal( Eval("SelfPrice")).ToString("0.00")%>&nbsp;Ԫ
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ����ۣ�
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ������Ŀ��
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblBuHanXiangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            �Է���Ŀ��
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblZiFeiXIangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ��ͯ���ţ�
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblErTongAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ���ﰲ�ţ�
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblGouWuAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ������Ŀ��
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:TextBox ID="txtPresent" runat="server" Width="90%" BorderStyle="None"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            ע�����
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblZhuYiShiXiang" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="tabxcquick" class="table_normal2" width="100%" runat="server">
                    <tr>
                        <td align="left">
                            <strong>�г̰��ţ�</strong>
                            <br />
                            &nbsp;&nbsp;<asp:Label ID="lblQuickPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>�����׼��˵����</strong><br />
                            &nbsp;&nbsp;<asp:Label ID="lblKs" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="table_normal2" width="100%" style="margin-top: 10px;" cellpadding="0"
                    cellspacing="0">
                    <tr style="border: 1px solid #000">
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="middle" width="11%"
                            rowspan="<%=visitorRowsCount+1 %>">
                            <strong>������Ϣ��</strong>
                        </td>
                        <asp:Repeater ID="rptVisitorList" runat="server">
                            <ItemTemplate>
                                <tr style="border: 1px solid #000">
                                    <td align="left" class="normaltd" valign="top" width="15px">
                                        <% mi++; %>
                                        <%#Container.ItemIndex+1%>��
                                    </td>
                                    <td width="70px" align="center">
                                        <%#Eval("VisitorName")%>
                                    </td>
                                    <td width="40px" align="center">
                                        <%#Eval("VisitorType").ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("CradNumber")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </table>
            </td>
            <%-- <div class="Placeholder20">
                &nbsp;</div>--%>
        </tr>
    </table>
    <div class="Placeholder20">
        &nbsp;</div>
    <table class="table_normal2" width="100%">
        <tr>
            <td align="left" class="normaltd" colspan="3" height="25">
                <strong>�����ȷ������,�뽫������ҹ�˾�����ʺ�:</strong>
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                ��˾ȫ�ƣ��㽭�ʵ����������޹�˾
            </td>
            <td align="left" height="25" style="padding-left: 10px;" width="33%">
                �����У���ͨ���к��ݳǱ�֧��
            </td>
            <td align="left" style="padding-left: 10px;" width="30%">
                �ʺţ�331066100018010042682
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                �������ű�
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                �����У��������� ����
            </td>
            <td align="left" style="padding-left: 10px;">
                �ʺţ�6227 0015 4180 0293 354
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                �������ű�
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                �����У��������� ĵ����
            </td>
            <td align="left" style="padding-left: 10px;">
                �ʺţ�622202 1202024799174
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                �������ű�
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                �����У�ũҵ���� ���뿨
            </td>
            <td align="left" style="padding-left: 10px;">
                �ʺţ�62284 8032 09784 20414
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                �������ű�
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                �����У��������� ��������
            </td>
            <td align="left" style="padding-left: 10px;">
                �ʺţ�6227 0015 9550 0281 598
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0 0 0;">
        &nbsp;&nbsp;</div>
    <table class="table_normal2" width="100%">
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                �����絥λ���£�
                <asp:Label ID="lblSetMan2" runat="server" Text=""></asp:Label><br />
                ��ϵ�绰��
                <asp:Label ID="lblSetPhone2" runat="server" Text=""></asp:Label><span class="Apple-converted-space">&nbsp;</span><br />
                ȷ�����ڣ�
                <%=DateTime.Now.ToString("yyyy-MM-dd") %>
            </td>
            <td align="left" height="25" style="padding-left: 10px;" width="33%">
                ר���̵�λ���£�
                <asp:Label ID="lblGetMan2" runat="server" Text=""></asp:Label><br />
                ��ϵ�绰��
                <asp:Label ID="lblGetPhone2" runat="server" Text=""></asp:Label><span class="Apple-converted-space">&nbsp;</span><br />
                ȷ�����ڣ�
                <%=DateTime.Now.ToString("yyyy-MM-dd") %>
            </td>
        </tr>
    </table>
    </td> </tr> </table>

    <script type="text/javascript">
        $(function() {
            $(".tbl").each(function() {
                var height = $(this).parent("td").parent("tr");
                $(this).height(height.height() + 10);
            })
            var priceAll = 0;
            $("#tblProject").find("input[name='txtPrice']").each(function() {
                var price = parseFloat($(this).val());
                if (price.toString() != "NaN" && price > 0) {
                    priceAll = priceAll + price;
                }
            });
            priceAll = parseInt(priceAll * 100) / 100;
            $("#txt_allPrice").val(priceAll);
        })
         
    </script>

</asp:Content>
