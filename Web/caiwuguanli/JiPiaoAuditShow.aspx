<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiPiaoAuditShow.aspx.cs"
    Inherits="Web.JiPiaoAuditShow" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="hdOrderID" type="hidden" runat="server" />
        <input id="hdTourID" type="hidden" runat="server" />
        <input id="hd_cardType" type="hidden" runat="server" />
        <input id="hd_airCompany" type="hidden" runat="server" />
        <table width="900" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 5px;">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center">
                        航班信息：
                    </th>
                    <td align="left">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th height="25" width="14%" align="center">
                                        日期
                                    </th>
                                    <th height="25" width="14%" align="center">
                                        航班号/时间
                                    </th>
                                    <th width="10%" align="center">
                                        航段
                                    </th>
                                    <th width="30%" align="center">
                                        航空公司
                                    </th>
                                    <th width="30%" align="center">
                                        折扣
                                    </th>
                                </tr>
                                <asp:Repeater ID="RepAirList" runat="server">
                                    <ItemTemplate>
                                        <tr class="even">
                                            <td height="25" align="center">
                                                <input type="text" class="searchinput" id="txtAirDate" value="<%#string.Format("{0:d}",Eval("DepartureTime"))%>"
                                                    name="txtAirTime">
                                            </td>
                                            <td height="25" align="center">
                                                <input type="text" class="searchinput" id="Text1" value="<%#Eval("TicketTime")%>"
                                                    name="txtAirTime">
                                            </td>
                                            <td align="center">
                                                <input id="selAirLine" name="selAirLine" type="text" value="<%#Eval("FligthSegment")%>" />
                                            </td>
                                            <td align="center">
                                                <select id="SelAirCompany" name="SelAirCompany">
                                                    <option>
                                                        <%#Eval("AireLine")%></option>
                                                </select>
                                            </td>
                                            <td align="center">
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
                                    <th align="center">
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
                    <td height="30" align="left">
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
                    <td height="30" align="left">
                        <input type="text" class="searchinput" id="txtSumMoney" name="txtSumMoney" runat="server" />
                        ￥
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center">
                        支付方式：
                    </th>
                    <td height="30" align="left">
                        <asp:DropDownList ID="ddlPayType" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center">
                        订票需知：
                    </th>
                    <td align="left" colspan="3">
                        <textarea name="txtOrderPiaoMust" id="txtOrderPiaoMust" class="textareastyle" runat="server" /></textarea>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center">
                        PNR
                    </th>
                    <td align="left" colspan="3">
                        <textarea rows="3" cols="45" id="txtPNR" name="txtPNR" runat="server"></textarea>
                    </td>
                </tr>
                <tr class="even">
                    <th height="30" align="center">
                        售票处：
                    </th>
                    <td height="30" align="left">
                        <input type="text" class="searchinput searchinput02" id="txtSalePlace" name="txtSalePlace"
                            valid="required" errmsg="*请填写售票处!" runat="server" />
                        <span id="errMsg_txtSalePlace" class="errmsg"></span>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center">
                        票号：
                    </th>
                    <td height="30" align="left">
                        <textarea rows="3" cols="45" id="txtPiaoHao" name="txtPiaoHao" valid="required" errmsg="*请填写票号!"
                            runat="server"></textarea>
                        <span id="errMsg_txtPiaoHao" class="errmsg"></span>
                    </td>
                </tr>
                <tr class="even">
                    <th height="80" align="center">
                        备注：
                    </th>
                    <td align="left" colspan="3">
                        <textarea name="txtMemo" id="txtMemo" class="textareastyle" runat="server"></textarea>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="4">
                        <table width="320" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lbtnCancelTicket" runat="server" Visible="false" OnClick="lbtnCancelTicket_Click" OnClientClick="return cancelTicketConfirm()">取消出票</asp:LinkButton>
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideDoType" runat="server" />
    </form>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function() {

            //证件类型初始化
            $("select[name=SelCardType]").each(function() {
                var selectIndex = $(this).val();
                proArray = $("#<%=hd_cardType.ClientID %>").val().split("|");
                $(this).html("");
                for (var i = 0; i < proArray.length; i++) {
                    var obj = eval('(' + proArray[i] + ')');
                    $(this).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }
                if (selectIndex != null && selectIndex != "") {
                    $(this).val(selectIndex);
                }
            });

            //航空公司初始化
            $("select[name=SelAirCompany]").each(function() {
                var selectIndex = $(this).val();
                proArray = $("#<%=hd_airCompany.ClientID %>").val().split("|");
                $(this).html("");
                for (var i = 0; i < proArray.length; i++) {
                    var obj = eval('(' + proArray[i] + ')');
                    $(this).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }
                if (selectIndex != null && selectIndex != "") {
                    $(this).val(selectIndex);
                }
            });

        });

        //取消出票按钮事件
        function cancelTicketConfirm() {
            if (confirm("取消出票将会删除该出票信息的所有付款明细、相关退票信息，删除后数据不可恢复，你确定要取消出票吗？")) {
                return true;
            }
            return false;
        }

    </script>

</body>
</html>
