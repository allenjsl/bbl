<%@ Page Language="C#" Title="�ſ�֧��" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="waitkuan.aspx.cs" Inherits="Web.caiwuguanli.waitkuan" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">�������</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            ����λ��&gt;&gt;������� &gt;&gt; �ſ�֧��
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
        <ul class="fbTab">
            <li><a id="two1" href="TeamExpenditure.aspx">Ӧ���˿�</a></li>
            <li><a class="tabtwo-on" id="two2" href="waitkuan.aspx" class="">��������</a></li>
            <li><a id="two3" href="teamPayClear.aspx" class="">�ѽ����˿�</a></li>
            <li><a id="two4" href="jidiaoleixing.aspx">�����Ǽ�</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div style="display: block;" id="con_two_2">
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �źţ�</label>
                                <input type="text" id="txt_teamNum" class="searchinput" runat="server" name="txt_teamNum">
                                <label>
                                    �տλ��</label>
                                <input type="text" class="searchinput searchinput02" runat="server" id="txt_com"
                                    name="txt_com">
                                <label>
                                    �����ˣ�</label>
                                <input type="hidden" id="hd_operateId" />
                                <select id="selOperator" runat="server">
                                </select>
                                <label>״̬��</label>
                                <select id="seleState" runat="server">
                                  <option value="-1">��ѡ��</option>
                                  <option value="1">δ����</option>
                                  <option value="2">δ֧��</option>
                                </select>
                                <label>
                                    �������ڣ�</label>
                                <input type="text" onfocus="WdatePicker()" id="txt_Date" class="searchinput" runat="server"
                                    name="txt_Date" />-<input type="text" onfocus="WdatePicker()" id="txtRegEDate" class="searchinput" 
                                    name="txtRegEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("regedate") %>" />
                                <label>
                                    <asp:ImageButton ID="ImageButton1" Style="vertical-align: middle;" ImageUrl="../images/searchbtn.gif"
                                        runat="server" OnClick="ImageButton1_Click" /></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="../images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox" style="display: none">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td width="90" align="center">
                                <asp:LinkButton ID="lnkCheck" Visible="false" CssClass="del lnkCheck" runat="server"
                                    OnClick="lnkCheck_Click">�� ��</asp:LinkButton>
                            </td>
                            <td width="90" align="center">
                                <asp:LinkButton ID="lnkPay" Visible="false" CssClass="del lnkPay" runat="server"
                                    OnClick="lnkPay_Click">֧ ��</asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <a href="javascript:();" onclick="All(this)">�������</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:();" onclick="All(this)">����֧��</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <th width="3%" bgcolor="#bddcf4" align="center">
                                ȫѡ<input type="checkbox" id="allchk" />
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                �ź�
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                �Ŷ�����
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                �տλ
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                ������
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                ��������
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                ������
                            </th>
                            <th width="20%" bgcolor="#bddcf4" align="center">
                                ��ע
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                ����
                            </th>
                        </tr>
                        <cc2:CustomRepeater runat="server" ID="rpt_list" OnItemCommand="rpt_list_ItemCommand">
                            <ItemTemplate>
                                <tr class="<%=i%2==0 ?"even":"odd" %>">
                                    <td align="center">
                                        <input type="checkbox" class="chk" name="chk_sel" registerid="<%#Eval("RegisterId")%>"
                                            ischecked="<%#Eval("IsChecked")%>" ispay="<%#Eval("IsPay")%>">
                                    </td>
                                    <td align="center">
                                        <%#((EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Eval("ReceiveType")) == EyouSoft.Model.EnumType.FinanceStructure.OutPlanType.�������Ӧ�̰��� ? Eval("tourCode") : Eval("tourCode")%>
                                    </td>
                                    <td align="center">
                                        <%#((EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Eval("ReceiveType")) == EyouSoft.Model.EnumType.FinanceStructure.OutPlanType.�������Ӧ�̰��� ? "�������Ӧ�̰���" : Eval("RouteName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ReceiveCompanyName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("StaffName")%>
                                    </td>
                                    <td align="center">
                                        <%#EyouSoft.Common.Utils.GetDateTimeNullable(Eval("PaymentDate").ToString()).Value.ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center">
                                        <font class="fred">��<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("PaymentAmount").ToString())%></font>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Remark")%>
                                    </td>
                                    <td align="center">
                                        <%#getUrl(bool.Parse(Eval("IsChecked").ToString()), bool.Parse(Eval("IsPay").ToString()), Eval("RegisterId").ToString(), Eval("ItemId").ToString(),Eval("ReceiveId").ToString())%>
                                    </td>
                                </tr>
                                <%i++; %>
                            </ItemTemplate>
                        </cc2:CustomRepeater>
                        <tr>
                            <th width="12%" bgcolor="#bddcf4" align="left" colspan="6">
                                �ϼƣ�
                            </th>
                            <td width="10%" bgcolor="#bddcf4" align="center">
                                <font class="fred">��<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(paymentAmount)%></font>
                            </td>
                            <th width="20%" bgcolor="#bddcf4" align="center" colspan="2">
                            </th>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="8">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script>
        $(function() {
            $(".lnkPay,.lnkCheck").click(function() {
                var chkval = "";
                $("[name='chk_sel']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                    }
                });
                if (chkval == "") {
                    alert("��ѡ��һ����¼!");
                    return false;
                }
            });
            $(".shenpi").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "����",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });

            $(".pass").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "����",
                    modal: true,
                    width: "820px",
                    height: "620px"
                });
                return false;
            });
            $("#allchk").click(function() {
                $(".chk[ispay='False']").each(function() {
                    $(this).attr("checked", $("#allchk").attr("checked"));
                })
            });
            $(".chk").each(function() {

                if ($(this).attr("ispay") == "True") {
                    $(this).attr("disabled", true);
                }
            });
        });
        //��������
        function All(thiss) {
            var msg = "";
            //�жϲ���
            if ($(thiss).html() == "�������") {
                msg = "���";
            }
            else {
                msg = "֧��";
            }
            var ids = "";
            var url = window.location.pathname;
            var val = 0;
            //���ݲ�����ȡIds
            if (msg == "���") {
                $(".chk[ischecked='True']:checked").each(function() {
                    val++;

                })
                if (val <= 0) {
                    $(".chk[ischecked='False']:checked").each(function() {
                        ids += ($(this).attr("registerid") + ",");

                    })
                }
                url += "?act=Allpass";
            }
            else {
                $(".chk[ischecked='False'][ispay='False']:checked").each(function() {
                    val++;

                })
                if (val <= 0) {
                    $(".chk[ischecked='True'][ispay='False']:checked").each(function() {
                        ids += ($(this).attr("registerid") + ",");

                    })
                }
                url += "?act=Allpay";
            }
            if (ids.length > 0) {
                $.newAjax({
                    url: url,
                    type: "POST",
                    data: { "ids": ids.substring(0, ids.length - 1) },
                    dataType: 'json',
                    success: function(d) {
                        if (d.res) {
                            switch (d.res) {
                                case 1:
                                    alert("����" + msg + "�ɹ�");
                                    $(':checkbox').attr("checked", "");
                                    location.reload();
                                    break;
                                case -1:
                                    alert("����" + msg + "ʧ��");
                                    break;
                            }
                        }
                    },
                    error: function() {
                        alert("��������æ!");
                    }
                });
            }
            else {
                if (msg == "���") {
                    alert("����ȷѡ��" + msg + "�ļ�¼,Ŀǰ���ڣ�" + val + "��������˵ļ�¼");
                }
                else {
                    alert("����ȷѡ��" + msg + "����Ŀ,Ŀǰ���ڣ�" + val + "������֧���ļ�¼");
                }

                return false;
            }
        }    
    </script>

</asp:Content>
