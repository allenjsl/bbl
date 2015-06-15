<%@ Page Language="C#" Title="出纳登账" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="chunadz_list.aspx.cs" Inherits="Web.caiwuguanli.chunadz_list" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">财务管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt;财务管理 &gt;&gt; 出纳登帐
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
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="../images/yuanleft.gif">
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                客户单位：</label>
                            <input type="text" class="searchinput searchinput02" id="txt_com" name="txt_com"
                                runat="server"><input type="hidden" id="hd_comId" name="hd_comId" runat="server" /><a
                                    href="/CRM/customerservice/SelCustomer.aspx?backfun=selectTeam" class="selectTeam"><img
                                        width="28" height="18" src="../images/sanping_04.gif"></a>
                            <label>
                                到款银行：</label>
                            <input type="text" class="searchinput searchinput02" id="txt_bank" name="txt_bank"
                                runat="server" />
                            到款时间：</label>
                            <input type="text" id="txt_date" onfocus="WdatePicker()" class="searchinput" name="txt_date"
                                runat="server" />
                            <label>
                                <asp:ImageButton ID="ImageButton1" Style="vertical-align: middle;" runat="server"
                                    ImageUrl="../images/searchbtn.gif" OnClick="ImageButton1_Click" /></label>
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
            <table width="45%" cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td width="90" align="left">
                            <a id="link1" class="dengji" href="Cashier_Register.aspx">登 记</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
            <table width="100%" cellspacing="1" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <%-- <th width="3%" height="30" bgcolor="#bddcf4" align="center">&nbsp;</th>--%>
                        <th width="15%" bgcolor="#bddcf4" align="center">
                            到款时间
                        </th>
                        <th width="13%" bgcolor="#bddcf4" align="center">
                            到款金额
                        </th>
                        <th width="14%" bgcolor="#bddcf4" align="center">
                            到款银行
                        </th>
                        <th width="13%" bgcolor="#bddcf4" align="center">
                            客户单位
                        </th>
                        <th width="13%" bgcolor="#bddcf4" align="center">
                            联系人
                        </th>
                        <th width="13%" bgcolor="#bddcf4" align="center">
                            电话
                        </th>
                        <th width="10%" bgcolor="#bddcf4" align="center">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rpt_list">
                        <ItemTemplate>
                            <tr class="<%=i%2==0 ?"even":"odd" %>">
                                <%-- <td height="30" bgcolor="#bddcf4" align="center"><input type="checkbox" id="checkbox2" name="checkbox2"></td>--%>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Eval("PaymentTime").ToString()).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("PaymentCount").ToString())%><br />
                                        已销账:(￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%>)</font>
                                </td>
                                <td align="center">
                                    <%#Eval("PaymentBank")%>
                                </td>
                                <td align="center">
                                    <%#Eval("CustomerName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Contacter")%>
                                </td>
                                <td align="center">
                                    <%#Eval("ContactTel")%>
                                </td>
                                <td align="center">
                                    <a href="chunaxiaozhang.aspx?id=<%#Eval("RegisterId") %>&money=<%#EyouSoft.Common.Utils.GetDecimal( Eval("PaymentCount").ToString())-EyouSoft.Common.Utils.GetDecimal( Eval("TotalAmount").ToString()) %>"
                                        class="Writeoffs"><font class="fblue">销账</font></a>
                                </td>
                            </tr>
                            <%i++; %>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <th bgcolor="#bddcf4" align="left">
                            合计：
                        </th>
                        <td width="13%" bgcolor="#bddcf4" align="center">
                            <font class="fred">总到款：(￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(paymentCount)%>)<br />
                                总销账：(￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(totalAmount)%>)</font>
                        </td>
                        <th bgcolor="#bddcf4" align="center" colspan="5">
                        </th>
                    </tr>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="7">
                            <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </form>

    <script>
        $(function() {
            $("#txt_com").click(function() {
                $(".selectTeam").click();
            });
            $(".selectTeam").click(function() {
                var iframeId = '';
                var url = $(this).attr("href");
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选用组团社",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: iframeId,
                        backfun: "selectTeam"
                    }
                });
                return false;

                ///根据选择的组团社绑定报价等级
            });
            $(".dengji").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "520px",
                    height: "340px"
                });
                return false;
            });
            $(".Writeoffs").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "销账",
                    modal: true,
                    width: "750px",
                    height: "400px"
                });
                return false;
            });
        });
        function selectTeam(id, name) {
            $("#<%=hd_comId.ClientID %>").val(id);
            $("#<%=txt_com.ClientID %>").val(name);
        }
    </script>

</asp:Content>
