<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OffGroupPrint.aspx.cs" Title="���ŵ�" Inherits="Web.print.normal.OffGroupPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="760px" border="0" align="center" class="table_normal">
        <tr>
            <td height="30" align="center" class="normaltd">
                <span style="font-size: 20px; font-weight: bold;">
                    �� �� �� �� ��</span>
            </td>
        </tr>
        <tr>
            <td height="30" align="center" class="normaltd" style="padding: 0px; margin: 0px">
                <table width="100%" border="0" class="table_noneborder">
                    <tr>
                        <td width="15%" height="25" align="center" class="normaltd">
                            ��������
                        </td>
                        <td colspan="2" align="center" class="td_r_b_border">
                            <asp:TextBox ID="txtOutDate" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td width="15%" align="center" class="td_r_b_border">
                            ����ʱ��
                        </td>
                        <td colspan="2" align="center" class="td_b_border">
                            <asp:TextBox ID="txtGather" runat="server" CssClass="nonelineTextBox" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%" height="25" align="center" class="td_r_b_border">
                            ȥ�̺���/ʱ��
                        </td>
                        <td colspan="5" align="left" class="td_b_border">
                            <asp:Label ID="lblBenginDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%" height="25" align="center" class="td_r_b_border">
                            �س̺���/ʱ��
                        </td>
                        <td colspan="5" align="left" class="td_b_border">
                            <asp:Label ID="lblBackDate" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%" height="25" align="center" class="td_r_b_border">
                            ������·
                        </td>
                        <td width="14%" align="center" class="td_r_b_border">
                            <asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="12%" align="center" class="td_r_b_border">
                            ����
                        </td>
                        <td width="15%" align="center" class="td_r_b_border">
                            <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="10%" align="center" class="td_r_b_border">
                            ����/ɢƴ
                        </td>
                        <td width="36%" align="center" class="td_b_border">
                            <asp:Label ID="lblTourType" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="21%" height="25" align="center" colspan="4" style="width: 35%" class="td_r_b_border">
                            <b>�ؽ���</b>
                        </td>
                        <td width="18%" align="center"  class="td_r_b_border">
                            <b>��ϵ��</b>
                        </td>
                        <td width="14%" align="center" style="width: 30%" class="td_b_border">
                            <b>��ϵ�绰</b>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptDjList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td width="21%" height="25" class="td_r_b_border" align="center" colspan="4">
                                    <%#Eval("Name")%>����<%#Container.ItemIndex+1 %>�Σ�
                                </td>
                                <td width="17%" align="center" class="td_r_b_border">
                                    <%#Eval("ContacterName")%>
                                </td>
                                <td width="14%" align="center" class="td_b_border">
                                    <%#Eval("Telephone")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td width="21%" height="25" rowspan="3" align="center" class="td_r_b_border">
                            ���ε绰(�Ա�)
                        </td>
                        <td colspan="5" align="left" class="td_b_border">
                            <input type="text" runat="server" id="txtNum1" class="nonelineTextBox" value="���Σ���һ�Σ�"
                                style="width: 90%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="left" class="td_b_border">
                            <input type="text" runat="server" id="txtNum2" class="nonelineTextBox" value="���Σ��ڶ��Σ�"
                                style="width: 90%" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="left" class="td_b_border">
                            <input type="text" runat="server" id="txtNum3" class="nonelineTextBox" value="���Σ������Σ�"
                                style="width: 90%" />
                        </td>
                    </tr>
                    <tr>
                        <td width="21%" height="25" align="center" class="td_r_b_border">
                            ��������
                        </td>
                        <td width="14%" align="center" class="td_r_b_border">
                            ���֤����
                        </td>
                        <td width="12%" align="center" class="td_r_b_border">
                            ��ϵ�绰
                        </td>
                        <td width="12%" align="center" class="td_r_b_border">
                            ����������
                        </td>
                        <td width="14%" align="center" class="td_r_b_border">
                            ��ϵ��
                        </td>
                        <td width="20%" align="center" class="td_b_border">
                            �ֻ�
                        </td>
                    </tr>
                    <asp:Repeater ID="rptCustomer" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" class="td_r_b_border">
                                    <%#Eval("VisitorName")%>
                                </td>
                                <td align="center" class="td_r_b_border">
                                    <%#Eval("CradNumber")%>
                                </td>
                                <td align="center" class="td_r_b_border">
                                    <%#Eval("ContactTel")%>
                                </td>
                                <%#GetOtherInfo(Eval("orderId"))%>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td height="30" align="center" class="td_r_b_border">
                            ���ϱ�־
                        </td>
                        <td colspan="5" class="td_b_border" align="left" valign="middle">
                            <input runat="server" id="txtNum4" class="nonelineTextBox" value="" style="width: 100%;
                                text-align: left;" />
                        </td>
                    </tr>
                    <tr>
                        <td height="70" colspan="6" class="td_noneborder" align="left" valign="top">
                            &nbsp;<b>�ڲ���Ϣ��</b>
                            <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div class="PrintPreview" style="text-align: center; z-index: 2" ref="noprint">
        <a id="a_show" href="javascript:void(0);" style="text-decoration: none;">��</a>
        <div style="height: 40px; display: none" id="showdiv">
            <table width="100%">
                <tr>
                    <td align="center">
                        <a href="javascript:void(0);" id="btnSave" style="display: block; font-size: 14px;
                            color: #000000; font-weight: bold; background: url(/images/tjbtnbg02.gif) no-repeat 0 0;
                            width: 76px; height: 31px; line-height: 31px;">����</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hideTourID" runat="server" />

    <script type="text/javascript">
        $(function() {
            $("#btnSave").click(function() {

                $.ajax({
                    type: "POST",
                    url: "/print/normal/OffGroupPrint.aspx?type=save",
                    data: $($("#btnSave").closest("form").get(0)).serializeArray(),
                    cache: false,
                    success: function(state) {
                        if (state == "Ok") {
                            alert("����ɹ�!");
                            $("#btnSave").remove();
                            window.location.reload();
                        }
                    }
                });
                return false;
            })

            $(".PrintPreview").click(function() {
                if ($("#showdiv").css("display") == "none") {
                    $("#showdiv").slideDown("slow");
                    $("#a_show").html("��")
                } else {
                    $("#showdiv").slideUp("slow");
                    $("#a_show").html("��")
                }
            })
        })

    </script>

</asp:Content>
