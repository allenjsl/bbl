<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Expenditure.aspx.cs" Inherits="Web.caiwuguanli.Expenditure" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@Import Namespace="EyouSoft.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>�ӷ�֧��_�������</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="Form1" runat="server" name="form1">
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">�������</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt;������� &gt;&gt; �ӷ�֧��
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    ֧����Ŀ��</label>
                                <asp:TextBox ID="txtIncomePro" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                                <label>
                                    �����ˣ�</label>
                                <asp:TextBox ID="txtIncomeMan" runat="server" CssClass="searchinput"></asp:TextBox>
                                <label>
                                    ����ʱ�䣺</label>
                                <asp:TextBox ID="txtIncomeTime" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                                <label>�źţ�</label>
                                <input type="text" class="searchinput" id="txtTourCode" value="<%= Utils.GetQueryStringValue("tcode")%>" />
                                <label>����ʱ�䣺</label>
                                <input type="text" onfocus="WdatePicker()" id="txtLSDate" class="searchinput" name="txtLSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input type="text" onfocus="WdatePicker()" id="txtLEDate" class="searchinput" name="txtLEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                                <label>
                                    <a href="javascript:void(0);" id="btnSearch">
                                        <img style="vertical-align: top;" src="../images/searchbtn.gif"></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="../images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="hr_10">
            </div>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left" width="45%">
                    <tbody>
                        <tr>
                            <td align="left" width="90">
                                <asp:Panel ID="pnlAdd" runat="server">
                                    <a id="btnAdd" href="javascript:void(0);">�� ��</a></asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th bgcolor="#bddcf4" align="center" width="15%">
                                ֧����Ŀ
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                ����ʱ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="14%">
                                ��λ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                ������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                ���
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                ״̬
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                ��ע
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="15%">
                                ����
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <%#Container.ItemIndex % 2 == 0 ? "<tr bgcolor=\"#e3f1fc\">" : "<tr bgcolor=\"#bddcf4\">"%>
                                <td align="center">
                                    <%#Eval("Item")%>
                                </td>
                                <td align="center">
                                    <%#Convert.ToDateTime(Eval("PayTime")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("CustromCName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Payee")%>
                                </td>
                                <td align="center">
                                    <font class="fred">��<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00"))%></font>
                                </td>
                                <td align="center">
                                    <%#(bool)Eval("Status") ? "�Ѹ�" : "δ��"%>
                                </td>
                                <td align="center">
                                    <font class="fred">
                                        <%#Eval("Remark") %></font>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" onclick="OpenDetails('<%#Eval("OutId") %>','expend')">
                                        <font class="fblue">�޸�</font></a> <a href="javascript:void(0);" onclick="DeleteDetails('<%#Eval("OutId") %>','<%#Eval("tourId") %>');return false;">
                                            <font class="fblue">ɾ��</font></a>
                                    <%if (CheckGrant(Common.Enum.TravelPermission.�������_�ӷ�֧��_֧�����)) {%>
                                        <a href="javascript:void(0);" onclick="SetOtherCostStatus(this, '<%#Eval("Status")%>','<%#Eval("OutId")%>')"><font class="fblue"><%#(bool)Eval("Status") ? "ȡ������" : "ȷ�ϸ���"%></font></a>
                                    <%} %>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <th bgcolor="#bddcf4" align="left" colspan="4">
                                �ϼƣ�
                            </th>
                            <td bgcolor="#bddcf4" align="center" width="13%">
                                <font class="fred">��<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(totalAmount)%></font>
                            </td>
                            <th bgcolor="#bddcf4" align="center" colspan="3">
                            </th>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="7">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" pagestyletype="NewButton"
                                    CurrencyPageCssClass="RedFnt" />
                                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script src="../js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#btnSearch").click(function() {
                //para.tcode:tourcode
                var para = { incomePro: "", incomeMan: "", incomeTime: "","tcode":"",lsdate:"",ledate:"" };
                //֧����Ŀ����
                var incomePro = $("#<%=txtIncomePro.ClientID %>").val();
                //������
                var incomeMan = $("#<%=txtIncomeMan.ClientID %>").val(); "";
                //����ʱ��
                var incomeTime = $("#<%=txtIncomeTime.ClientID %>").val(); "";

                para.incomePro = incomePro;
                para.incomeMan = incomeMan;
                para.incomeTime = incomeTime;
                para.tcode=$.trim($("#txtTourCode").val());
                para.lsdate=$("#txtLSDate").val();
                para.ledate=$("#txtLEDate").val();

                window.location.href = "/caiwuguanli/Expenditure.aspx?" + $.param(para);
                return false;
            });

            $("#btnAdd").click(function() {
                OpenDetails("", "expend");
            });

            $(".searchbox input[type='text']").keydown(function(event) {
                var e = event;
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            });

        });

        function OpenDetails(id, type) {
            Boxy.iframeDialog({ iframeUrl: "/sanping/TianjiaZhichu.aspx?id=" + id + "&type=" + type, title: "�ӷ�֧��", modal: true, width: "510px", height: "350px" });
        }

        function DeleteDetails(id, tourId) {
            if (confirm("ȷ��ɾ����?")) {
                $.newAjax({
                    type: "POST",
                    url: "/caiwuguanli/AjaxIncome.ashx?id=" + id + "&tourId=" + tourId + "&type=expend" + "&v=" + Math.random(),
                    cache: false,
                    success: function(state) {
                        if (state == "OK") {
                            alert("ɾ���ɹ�!");
                            window.location.reload();
                        }
                    }
                });
            }
        }
        
        <%if (CheckGrant(Common.Enum.TravelPermission.�������_�ӷ�֧��_֧�����)) {%>
        function SetOtherCostStatus(thiss, status, OutId) {
            if (confirm("ȷ��" + $.trim($(thiss).find(".fblue").html()) + "��?")) {
                $.newAjax({
                    type: "POST",
                    url: "/caiwuguanli/AjaxIncome.ashx?cat=status&IncomeId=" + OutId + "&status=" + status + "&verifyType=out",
                    cache: false,
                    success: function(state) {
                        if (state == "OK") {
                            alert($.trim($(thiss).find(".fblue").html()) + "�ɹ�!");
                            window.location.reload();
                        } else {
                            alert($.trim($(thiss).find(".fblue").html()) + "ʧ��!");
                            return false;
                        }
                    }

                });
            }
        }
        <%} %>
    </script>

    </form>
</asp:Content>
