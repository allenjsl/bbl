<%@ Page Language="C#" Title="�ſ�֧��" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="TeamExpenditure.aspx.cs" Inherits="Web.caiwuguanli.TeamExpenditure" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
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
            <li><a class="tabtwo-on" id="two1" href="TeamExpenditure.aspx">Ӧ���˿�</a></li>
            <li><a id="two2" href="waitkuan.aspx" class="">��������</a></li>
            <li><a id="two3" href="teamPayClear.aspx" class="">�ѽ����˿�</a></li>
            <li><a id="two4" href="jidiaoleixing.aspx">�����Ǽ�</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1" style="display: block;">
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �Ŷ����ͣ�</label>
                                <select id="select" name="select" runat="server">
                                    <option value="-1">��ѡ��</option>
                                    <option value="0">ɢƴ</option>
                                    <option value="1">�Ŷ�</option>
                                    <option value="2">�������</option>
                                </select>
                                <label>
                                    �źţ�</label>
                                <input type="text" id="txt_teamNum" class="searchinput" name="txt_teamNum" runat="server">
                                <label>
                                    ��λ���ƣ�</label>
                                <input type="text" class="searchinput searchinput02" id="txt_com" name="txt_com"
                                    runat="server">
                                <label>
                                    ��λ���</label>
                                <asp:DropDownList ID="ddl_comType" runat="server">
                                    <asp:ListItem Value="-1">��ѡ��</asp:ListItem>
                                    <asp:ListItem Value="2">Ʊ��</asp:ListItem>
                                    <asp:ListItem Value="1">�ؽ�</asp:ListItem>
                                </asp:DropDownList>
                                <br>
                                <label>
                                    ����ʱ�䣺</label>
                                <input type="text" id="txt_godate" onfocus="WdatePicker({dateFmt:'yyyy/MM/dd'});"
                                    class="searchinput" name="txt_godate" runat="server">
                                ��
                                <input type="text" id="txtRDate" onfocus="WdatePicker({dateFmt:'yyyy/MM/dd'});" class="searchinput" name="txtRDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("rdate") %>" />
                                <label>
                                    ����ʱ�䣺</label>
                                <input type="text" id="txt_payDate" onfocus="WdatePicker({dateFmt:'yyyy/MM/dd'});"
                                    class="searchinput" name="txt_payDate" runat="server" />
                                    ��
                                <input type="text" id="txtPayEndTime" onfocus="WdatePicker({dateFmt:'yyyy/MM/dd'});" class="searchinput" name="txtPayEndTime" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("payendtime") %>" />
                                <label>
                                    <asp:ImageButton ID="ImageButton1" Style="vertical-align: middle;" runat="server"
                                        ImageUrl="../images/searchbtn.gif" OnClick="ImageButton1_Click" />
                                </label>
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
            <div class="btnbox" style="display:none">
                <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td td width="90" align="left">
                            <a href="javascript:();" onclick="piliangdengji()">�����Ǽ�</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                        <th bgcolor="#bddcf4" align="center"  style=" width :5%">ȫѡ<input type="checkbox" id="allchk" /></th>
                            <th bgcolor="#bddcf4" align="center">
                                �ź�
                            </th>
                            <th bgcolor="#bddcf4" align="center">
                                ��·����
                            </th>
                            <th bgcolor="#bddcf4" align="center">
                                ��������
                            </th>
                            <th bgcolor="#bddcf4" align="center">
                                ֧����Ŀ
                            </th>
                            <th bgcolor="#bddcf4" align="center">
                                ����
                            </th>
                        </tr>
                        <cc2:CustomRepeater runat="server" ID="rpt_list1" OnItemDataBound="rpt_list1_ItemDataBound">
                            <ItemTemplate>
                                <tr class="<%=i%2==0 ?"even":"odd" %>">
                                <td align="center" valign="middle"><input type="checkbox" class="chk" tourid="<%#Eval("TourId") %>"  /></td>
                                    <td align="center">
                                        <input type="hidden" id="hd_tourType" name="hd_tourType" value="<%#Eval("TourType") %>" />
                                        <%#((EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType")) == EyouSoft.Model.EnumType.TourStructure.TourType.������� ? Eval("TourCode") : Eval("TourCode")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("RouteName")%>
                                    </td>
                                    <td align="center">
                                        <%#((EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType"))==EyouSoft.Model.EnumType.TourStructure.TourType.�������?"": DateTime.Parse(Eval("LDate").ToString()).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="left">
                                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rpt_project">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td height="10" align="left">
                                                                <b>
                                                                    <%#Eval("SupplierType").ToString()%></b>(֧��:<font class="fred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%></font>,δ��:<font
                                                                        class="fred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalAmount").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("PayedAmount").ToString()))%></font>)
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                        <asp:Literal ID="lt_Amount" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <a href="<%#getFukuanUrl((int)(Eval("TourType"))) %>?tourId=<%#Eval("tourId") %>"><font
                                            class="fblue">����</font></a> 
                                            <% if (CheckGrant(global::Common.Enum.TravelPermission.�������_�ſ�֧��_�ɱ�ȷ��)) {%>
                                            <a style="display: <%#(bool)Eval("IsCostConfirm")==true?"none":"block" %>"
                                                href="TeamExpenditure.aspx?tourId=<%#Eval("tourId") %>&act=cbsubmit"><font class="fblue">
                                                    �ɱ�ȷ��</font></a>
                                                    <%} %>
                                                     <span style="display: <%#(bool)Eval("IsCostConfirm")==true?"block":"none" %>">
                                                        ��ȷ��</span>
                                    </td>
                                </tr>
                                <%i++; %>
                            </ItemTemplate>
                        </cc2:CustomRepeater>
                        <tr>
                            <th bgcolor="#bddcf4" colspan="4" align="left">
                                �ϼƣ�
                            </th>
                            <td bgcolor="#bddcf4" align="left">
                                <asp:PlaceHolder runat="server" ID="phDanXiang">
                                <b>����</b>(֧����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(singleAmount)%></font>,
                                δ����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(singleAmount-singleHasAmount)%></font>)<br />
                                </asp:PlaceHolder>
                                <asp:Panel ID="pnlTicket" runat="server">
                                    <b>Ʊ��</b>(֧����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(ticketAmount)%></font>,
                                    δ����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(ticketAmount-ticketHasAmount)%></font>)
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlGround" runat="server">
                                    <b>�ؽ�</b>(֧����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(travelAgencyAmount)%></font>,
                                    δ����<font class="fred"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(travelAgencyAmount-travelAgencyHasAmount)%></font>)
                                </asp:Panel>
                            </td>
                            <th bgcolor="#bddcf4" align="center">
                            </th>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="5">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            $("#<%=ImageButton1.ClientID %>").click(function() {

                var param = "tourCode=" + $("#<%=txt_teamNum.ClientID%>").val();
                param += "&tourtype=" + $("#<%=select.ClientID %>").val();
                param += "&companyName=" + encodeURIComponent($("#<%=txt_com.ClientID %>").val());
                param += "&comType=" + $("#<%=ddl_comType.ClientID %>").val();
                param += "&beginDate=" + $("#<%=txt_godate.ClientID %>").val();
                param += "&payDate=" + $("#<%=txt_payDate.ClientID %>").val();
                param += "&rdate=" + $("#txtRDate").val();
                param += "&payendtime=" + $("#txtPayEndTime").val();
                location.href = "TeamExpenditure.aspx?" + param;
                return false;
            });
            $("#allchk").click(function() {

                $(".chk").each(function() {
                    $(this).attr("checked", $("#allchk").attr("checked"));
                })
            })
        });
        function piliangdengji() {
            var tourids = "";
            $(".chk:checked").each(function() {
                tourids += ($(this).attr("tourid") + ",");
            })
            if (tourids.length > 0) {
                var params = utilsUri.getUrlParams([]);
                var comType = $("#<%=ddl_comType.ClientID %>").val();
                if (params["companyName"] == 'undefined' || $.trim(params["companyName"]).length == 0) params["companyName"] = "";

                var iframeRequestParams = { comType: comType, gysName: params["companyName"], tourids: tourids.substring(0, tourids.length - 1) };
                var iframeUrl = utilsUri.createUri("/caiwuguanli/piliangdengji.aspx", iframeRequestParams);
                Boxy.iframeDialog({
                    iframeUrl: iframeUrl,
                    title: "�����Ǽ�",
                    data: {},
                    modal: true,
                    width: "920px",
                    height: "200px"
                });
            }
            else {
                alert("��ѡ��Ǽ���Ŀ��");
            }
            return false;
        }
    </script>

</asp:Content>
