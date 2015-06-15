<%@ Page Title="���ر��ۼ��г�-����-��������" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
    CodeBehind="DownloadMoney.aspx.cs" Inherits="Web.GroupEnd.JourneyMoney.DownloadMoney" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td>
                                <span class="lineprotitle">�г̱���</span>
                            </td>
                            <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right">
                                ����λ��&gt;&gt; <a href="#">�г̱���</a>&gt;&gt; ���ر��ۼ��г�
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#000000" height="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table class="xtnav" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td align="center">
                                <a href="SelectMoney.aspx">��Ҫѯ��</a>
                            </td>
                            <td class="xtnav-on" align="center">
                                <a href="#">���ر��ۼ��г�</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="btnbox">
            </div>
            <table width="99%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td width="28%" align="center" bgcolor="#BDDCF4">
                        <strong>����</strong>
                    </td>
                    <td width="16%" align="center" bgcolor="#BDDCF4">
                        <strong>��Ч��</strong>
                    </td>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        ����ʱ��
                    </th>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        ������
                    </th>
                    <td width="10%" align="center" bgcolor="#BDDCF4">
                        <strong>����</strong>
                    </td>
                </tr>
                <asp:Repeater ID="retList" runat="server">
                    <ItemTemplate>
                        <tr operatorid="<%# Eval("OperatorId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td width="28%" align="center">
                                <%#Eval("FileName") %>
                            </td>
                            <td width="19%" align="center">
                                ��<%#Eval("ValidityStart") == null ? "" : Convert.ToDateTime(Eval("ValidityStart")).ToString("yyyy-MM-dd")%>��<%#Eval("ValidityEnd") == null ? "" : Convert.ToDateTime(Eval("ValidityEnd")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td width="10%" align="center">
                                <%#Eval("AddTime") == null ? "" : Convert.ToDateTime(Eval("AddTime")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td width="10%" align="center">
                                <a href="javascript:;" class="principal">
                                    <%#Eval("OperatorName")%></a>
                            </td>
                            <td width="10%" align="center">
                            <%# Eval("FilePath") != "" ? "<a href='" + Eval("FilePath") + "' target='_blank'>�����г�</a>" : "<span>�޸���</span>"%>
                       
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td height="30" colspan="5" align="right" valign="middle" bgcolor="#FFFFFF" class="pageup">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>

        <script type="text/javascript">
            function showOperatorName(that) {
                var OperatorID = that.parent().parent().attr("OperatorID");
                var url = "/GroupEnd/JourneyMoney/PrincipalShow.aspx?";

                Boxy.iframeDialog({
                    iframeUrl: url + "OperatorID=" + OperatorID,
                    title: "������",
                    modal: true,
                    width: "600",
                    height: "100px"
                });
                return false;
            };
            $(function() {
                $(".principal").click(function() {
                    showOperatorName($(this));
                    return false;
                });
            })
        </script>

    </div>
</asp:Content>
