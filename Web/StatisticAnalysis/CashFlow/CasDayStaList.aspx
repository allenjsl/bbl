<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master"
    Title="����ͳ��_�ֽ�������_ͳ�Ʒ���" CodeBehind="CasDayStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.CashFlow.CasDayStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">ͳ�Ʒ���</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>>ͳ�Ʒ��� >> �ֽ����� >> ����ͳ��
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a href="/StatisticAnalysis/CashFlow/CasDayStaList.aspx" id="two1" class="tabtwo-on">
                ����ͳ��</a></li>
            <li><a href="/StatisticAnalysis/CashFlow/CasMonthStaList.aspx" id="two2">����ͳ��</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1">
            <div class="tablelist" id="divCashDayStaList">
                <div class="btnbox">
                    <table border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="90" align="left">
                                <uc1:UCPrintButton ContentId="tbl_CashDayStaList" ID="UCPrintButton1" runat="server" />
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:void(0);" id="btnExport">
                                    <img src="/images/daoru.gif" />
                                    �� �� </a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:void(0);" onclick="return AddCash();">�����ֽ�</a>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divCashDayList" runat="server">
                    <table width="100%" border="0" id="tbl_CashDayStaList" cellpadding="0" cellspacing="1">
                        <tr>
                            <th width="13%" align="center" bgcolor="#BDDCF4">
                                ����
                            </th>
                            <th width="13%" align="center" bgcolor="#bddcf4">
                                �ֽ𴢱�
                            </th>
                            <th width="13%" align="center" bgcolor="#bddcf4">
                                �ֽ�����
                            </th>
                            <th width="13%" align="center" bgcolor="#bddcf4">
                                �ֽ�֧��
                            </th>
                            <th width="13%" align="center" bgcolor="#bddcf4">
                                ��ǰ�ֽ���
                            </th>
                            <%--<th width="13%" align="center" bgcolor="#bddcf4">
                        ����
                    </th>--%>
                        </tr>
                        <cc2:CustomRepeater ID="crp_GetCashStaList" runat="server">
                            <ItemTemplate>
                                <tr cash='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("CurrCash")))%>'
                                    style="background-color: #e3f1fc">
                                    <td align="center" bgcolor="#e3f1fc">
                                        <%#Eval("IssueTime", "{0:MM��dd��}")%>
                                    </td>
                                    <td align="center" bgcolor="#e3f1fc">
                                        <%#Eval("CashReserve","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#e3f1fc">
                                        <%#Eval("CashIn","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#e3f1fc">
                                        <%#Eval("CashOut","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#e3f1fc">
                                        <%#Eval("CurrCash","{0:c2}")%>
                                    </td>
                                    <%--<td align="center" bgcolor="#e3f1fc">
                                <asp:LinkButton runat="server" ID="lkAddCash" CssClass="fblue"></asp:LinkButton>
                            </td>--%>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr cash='<%#EyouSoft.Common.Utils.GetDecimal(Convert.ToString(Eval("CurrCash")))%>'
                                    style="background-color: #e3f1fc">
                                    <td align="center" bgcolor="#bddcf4">
                                        <%#Eval("IssueTime","{0:MM��dd��}")%>
                                    </td>
                                    <td align="center" bgcolor="#bddcf4">
                                        <%#Eval("CashReserve","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#bddcf4">
                                        <%#Eval("CashIn","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#bddcf4">
                                        <%#Eval("CashOut","{0:c2}")%>
                                    </td>
                                    <td align="center" bgcolor="#bddcf4">
                                        <%#Eval("CurrCash","{0:c2}")%>
                                    </td>
                                    <%--<td align="center" bgcolor="#bddcf4">
                                <asp:LinkButton runat="server" ID="lkAddCash"></asp:LinkButton>
                            </td>--%>
                                </tr>
                            </AlternatingItemTemplate>
                        </cc2:CustomRepeater>
                    </table>
                    <%-- <table width="100%" id="tbl_ExportPage" runat="server" border="0" cellspacing="0"
                cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" />
                    </td>
                </tr>
            </table>--%>
                </div>
            </div>
        </div>
        <cc2:CustomRepeater Visible="false" ID="crp_PrintGetCashStaList" runat="server">
            <HeaderTemplate>
                <table width="100%" border="1" id="Table1" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="13%" align="center">
                            ����
                        </th>
                        <th width="13%" align="center">
                            �ֽ𴢱�
                        </th>
                        <th width="13%" align="center">
                            �ֽ�����
                        </th>
                        <th width="13%" align="center">
                            �ֽ�֧��
                        </th>
                        <th width="13%" align="center">
                            ��ǰ�ֽ���
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("IssueTime", "{0:MM��dd��}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashReserve","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashIn","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashOut","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CurrCash","{0:c2}")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("IssueTime","{0:MM��dd��}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashReserve","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashIn","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CashOut","{0:c2}")%>
                    </td>
                    <td align="center">
                        <%#Eval("CurrCash","{0:c2}")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </cc2:CustomRepeater>
    </div>
        <script type="text/javascript">
        $("#tbl_CashDayStaList tr").each(function() {
            if (parseInt($(this).attr("cash"), 10) <= 0)
                $(this).css("color", "red");
        });
        $(document).ready(function() {
            $("#btnExport").click(function() {//����
                if ($("#tbl_CashDayStaList").find("[id='EmptyData']").length >0) {
                    alert("��������,�޷�ִ�е�����");
                    return false;
                }
                var goToUrl = "/StatisticAnalysis/CashFlow/CasDayStaList.aspx?isExport=1";
                window.open(goToUrl, "Excel����");
                return false;
            });
        });
        function AddCash() {
            var url = "/StatisticAnalysis/CashFlow/AddCash.aspx";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "�����ֽ�",
                modal: true,
                width: "326",
                height: "162px"
            });
            return false;
        }
        </script>
    </form>
</asp:Content>
