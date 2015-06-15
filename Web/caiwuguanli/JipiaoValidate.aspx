<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JipiaoValidate.aspx.cs"
    Inherits="Web.caiwuguanli.JipiaoValidate" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style>
        .span
        {
            margin-left: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="hd_cardType" type="hidden" runat="server" />
        <input id="hd_airCompany" type="hidden" runat="server" />
        <table width="900" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 5px;">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center">
                        航班信息：
                    </th>
                    <td align="left" colspan="3">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th height="15%" width="14%" align="center">
                                        日期
                                    </th>
                                    <th height="45%" width="14%" align="center">
                                        航班号/时间
                                    </th>
                                    <th width="20%" align="center">
                                        航段
                                    </th>
                                    <th width="10%" align="center">
                                        航空公司
                                    </th>
                                    <th width="10%" align="center">
                                        折扣
                                    </th>
                                </tr>
                                <asp:Repeater ID="RepAirList" runat="server">
                                    <ItemTemplate>
                                        <tr class="even">
                                            <td height="15%" align="center">
                                                <input type="text" style="border: 1px solid #93B7CE; font-size: 12px; height: 15px;
                                                    width: 100px" id="txtAirDate" value="<%#string.Format("{0:d}",Eval("DepartureTime"))%>"
                                                    name="txtAirTime">
                                            </td>
                                            <td height="45%" align="center">
                                                <input type="text" style="border: 1px solid #93B7CE; font-size: 12px; height: 15px;
                                                    width: 190px" id="txtTicketTime" value="<%#Eval("TicketTime")%>" name="txtAirTime">
                                            </td>
                                            <td align="center" width="20%">
                                                <input id="selAirLine" name="selAirLine" type="text" value="<%#Eval("FligthSegment")%>" />
                                            </td>
                                            <td align="center" width="10%">
                                                <select id="SelAirCompany" name="SelAirCompany">
                                                    <option>
                                                        <%#Eval("AireLine")%></option>
                                                </select>
                                            </td>
                                            <td align="center" width="10%">
                                                <input type="text" class="searchinput" id="txtZheKo" value="<%#FilterEndOfTheZeroDecimal(Eval("Discount"))%>"
                                                    name="txtZheKo">%
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <th height="30" align="center">
                        名单：
                    </th>
                    <td align="right" colspan="3">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th height="5" align="center">
                                        编号
                                    </th>
                                    <th height="25" align="center">
                                        姓名
                                    </th>
                                    <th align="center">
                                        证件类型
                                    </th>
                                    <th align="center">
                                        证件号码
                                    </th>
                                </tr>
                                <cc1:CustomRepeater ID="RepCusList" runat="server">
                                    <ItemTemplate>
                                        <tr class="even">
                                            <td align="center">
                                                <%#Container.ItemIndex+1%>
                                            </td>
                                            <td height="25" align="center">
                                                <input type="text" class="searchinput" id="txtCusName" name="txtCusName" value="<%#Eval("VisitorName")%>" />
                                            </td>
                                            <td align="center">
                                                <select id="SelCardType" name="SelCardType" style="width: 100px;">
                                                    <option>
                                                        <%#Eval("CradType")%></option>
                                                </select>
                                            </td>
                                            <td align="center">
                                                <input type="text" class="searchinput searchinput02" id="txtCardNumber" name="txtCardNumber"
                                                    value="<%#Eval("CradNumber")%>" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </cc1:CustomRepeater>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center">
                        票款：
                    </th>
                    <td height="30" align="left" colspan="3">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <td height="35" align="left">
                                        &nbsp;<b>成人：</b>票面价:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultPrice" name="txtAdultPrice"
                                            runat="server" />
                                        税/机建:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultShui" name="txtAdultShui"
                                            runat="server" />
                                        人数:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultCount" name="txtAdultCount"
                                            runat="server" />
                                        <%
                                            if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                            {
                                        %>
                                        代理费:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultProxyPrice" name="txtAdultProxyPrice"
                                            runat="server" />
                                        <%}
                                            if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                            {%>
                                        <nobr>
                                        百分比:
                                <input type="text" class="searchinput" runat="server" id="txt_Percent" name="txt_Percent">%
                                </nobr>
                                        <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" runat="server" id="txt_DaiLiFei" name="txt_DaiLiFei">
                                </nobr>
                                        <%} %>
                                        票款:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultSum" name="txtAdultSum"
                                            runat="server" />
                                        <font class="fred">
                                            <asp:Label ID="lblMsgFrist" runat="server" Text=""></asp:Label></font>
                                    </td>
                                </tr>
                                <tr class="even">
                                    <td height="35" align="left">
                                        &nbsp;<b>儿童：</b>票面价:
                                        <input type="text" class="searchinput searchinput03" id="txtChildPrice" name="txtChildPrice"
                                            runat="server" />
                                        税/机建:
                                        <input type="text" class="searchinput searchinput03" id="txtChildShui" name="txtChildShui"
                                            runat="server" />
                                        人数:
                                        <input type="text" class="searchinput searchinput03" id="txtChildCount" name="txtChildCount"
                                            runat="server" />
                                        <%
                                            if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                            {
                                        %>
                                        代理费:
                                        <input type="text" class="searchinput searchinput03" id="txtChildProxyPrice" name="txtChildProxyPrice"
                                            runat="server" />
                                        <%}
                                            if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                            {%>
                                        <nobr>
                                        百分比:
                                <input type="text" class="searchinput" runat="server" id="txt_Percent2" name="txt_Percent2">%
                                </nobr>
                                        <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" runat="server" id="txt_DaiLiFei2" name="txt_DaiLiFei2">
                                </nobr>
                                        <%} %>
                                        票款:
                                        <input type="text" class="searchinput searchinput03" id="txtChildSum" name="txtChildSum"
                                            runat="server" />
                                        <font class="fred">
                                            <asp:Label ID="lblMsgSecond" runat="server" Text=""></asp:Label></font>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <th height="30" align="center">
                        总费用：
                    </th>
                    <td height="30" align="left" colspan="3">
                        <input type="text" class="searchinput" id="txtSumMoney" name="txtSumMoney" runat="server" />
                        ￥
                    </td>
                </tr>
                <tr class="odd">
                    <td align="left" colspan="4">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th height="15%" width="20%" align="center">
                                        客户单位
                                    </th>
                                    <th height="15%" width="20%" align="center">
                                        人数
                                    </th>
                                    <th width="20%" align="center">
                                        订单金额
                                    </th>
                                    <th width="20%" align="center">
                                        已收金额
                                    </th>
                                    <th width="20%" align="center">
                                        未收金额
                                    </th>
                                </tr>
                                <asp:Repeater ID="TicketOrderlist" runat="server">
                                    <ItemTemplate>
                                        <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                            <td height="15%" width="20%" align="center">
                                                <%# Eval("CustomerName")%>
                                            </td>
                                            <td height="15%" width="20%" align="center">
                                                <%# Eval("OrderPeople")%>
                                            </td>
                                            <td width="20%" align="center">
                                                <%# Eval("OrderAmount","{0:c2}")%>
                                            </td>
                                            <td width="20%" align="center">
                                                <%# Eval("HasCheckMoney", "{0:c2}")%>
                                            </td>
                                            <td width="20%" align="center">
                                                <%# Eval("NotMoney", "{0:c2}")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <th height="30" align="center">
                        审核备注：
                    </th>
                    <td height="30" align="left" colspan="3">
                        <asp:TextBox ID="txt_remark" TextMode="MultiLine" runat="server" Height="67px" 
                            Width="394px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <table width="300" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td class="tjbtn02">
                                        <asp:Panel ID="PanClose" runat="server" CssClass="span">
                                            <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                                id="linkCancel" href="javascript:;" style="text-align: center;">关闭</a>
                                        </asp:Panel>
                                    </td>
                                    <td class="tjbtn02">
                                        <asp:Panel ID="PanShenhe" runat="server" CssClass="span">
                                            <a id="shenhebtn" href="javascript:;" style="text-align: center;">审核</a></asp:Panel>
                                    </td>
                                    <td class="tjbtn02">
                                        <asp:Panel ID="Panquxiao" runat="server" CssClass="span">
                                            <asp:LinkButton ID="lbtnquxiao" runat="server" OnClick="lbtnquxiao_Click" Style="text-align: center;">取消审核</asp:LinkButton>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideDoType" runat="server" />
    <asp:HiddenField ID="hidRefundId" runat="server" />
    </form>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#shenhebtn").click(function() {
                var RefundId = $("#<%=hidRefundId.ClientID %>").val();
                $.newAjax({
                    type: "POST",
                    url: "/ashx/JiPiaoAudit.ashx?RefundId=" + RefundId,
                    async: false,
                    dataType: "html",
                    data: { "remark": $("#txt_remark").val() },
                    success: function(data) {
                        alert(data);
                        window.parent.location.href = window.parent.location.href;
                    },
                    error: function() {
                        alert("审核失败!");
                    }
                });
            });
        })
    </script>

</body>
</html>
