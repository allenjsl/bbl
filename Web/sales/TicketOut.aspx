<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketOut.aspx.cs" Inherits="Web.sales.TicketOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="920" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0 auto;">
        <tr class="odd">
            <th width="78" height="30" align="right">
                成人票款：
            </th>
            <td bgcolor="#E3F1FC">
                票面价:
                <input type="text" class="xtinput" value="<%=piaomianjia %>" id="txt_piaomianjia"
                    size="6" name="txt_piaomianjia" errmsg="*请输入成人票面价！|*请输入有效的票面价！" valid="required|isMoney">
                税/机建:
                <input type="text" class="xtinput" value="<%=shui %>" id="txt_shui" name="txt_shui"
                    size="6">
                人数:
                <input type="text" class="xtinput" value="<%=pepoleNum %>" id="txt_pepoleNum" name="txt_pepoleNum"
                    readonly="readonly" size="4">
                <%
                    if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                    {
                %>
                代理费:
                <input type="text" class="xtinput" value="<%=DaiLiFei %>" id="txt_DaiLiFei" name="txt_DaiLiFei"
                    size="6">
                <%} if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                    {%>
                <nobr>
                    百分比:
                    <input type="text" class="xtinput" value="<%=Percent %>" id="txt_Percent" name="txt_Percent" size="6">%
                    </nobr>
                <nobr>
                    其它费用:
                    <input type="text" class="xtinput" value="<%=OtherMoney %>" id="txt_DaiLiFei" name="txt_DaiLiFei" size="6">
                    </nobr>
                <%} %>
                <nobr>
                票款:
                <input type="text" class="xtinput" value="<%=piaokuan %>" id="txt_piaokuan" name="txt_piaokuan" size="6">
                </nobr>
                <span class="fred">
                    <asp:Label ID="lblMsgFrist" runat="server" Text=""></asp:Label></span>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                儿童票款：
            </th>
            <td bgcolor="#E3F1FC">
                票面价:
                <input type="text" class="xtinput" value="<%=piaomianjia2 %>" id="txt_piaomianjia2"
                    size="6" size="6" name="txt_piaomianjia2">
                税/机建:
                <input type="text" class="xtinput" value="<%=shui2 %>" id="txt_shui2" name="txt_shui2"
                    size="6">
                人数:
                <input type="text" class="xtinput" value="<%=pepoleNum2 %>" id="txt_pepoleNum2" name="txt_pepoleNum2"
                    readonly="readonly" size="4">
                <%if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 ||
                      config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                  {
                %>
                代理费:
                <input type="text" class="xtinput" value="<%=DaiLiFei2 %>" id="txt_DaiLiFei2" name="txt_DaiLiFei2"
                    size="6">
                <%} if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                  {%>
                <nobr>
                    百分比:
                    <input type="text" class="xtinput" value="<%=Percent2 %>" id="txt_Percent2" name="txt_Percent2" size="6">%
                    </nobr>
                <nobr>
                    其它费用:
                    <input type="text" class="xtinput" value="<%=OtherMoney2 %>" id="txt_DaiLiFei2" name="txt_DaiLiFei2" size="6">
                    </nobr>
                <%} %>
                <nobr>票款:
                    <input type="text" class="xtinput" value="<%=piaokuan2 %>" id="txt_piaokuan2" size="6"
                    name="txt_piaokuan2"/>
                    </nobr>
                <span class="fred">
                    <asp:Label ID="lblMsgSecond" runat="server" Text=""></asp:Label></span>
            </td>
        </tr>
        <tr class="odd">
            <td height="25" align="right">
                <font class="xinghao">*</font>总费用：
            </td>
            <td align="left" colspan="4">
                <input type="text" value="<%=Total %>" id="txt_SumPrice" class="searchinput" name="txt_SumPrice"
                    errmsg="*请输入总费用！|*请输入有效的总费用！" valid="required|isMoney">
                ￥
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                <span class="fred">*</span> PNR：
            </th>
            <td width="779" bgcolor="#E3F1FC">
                <textarea name="txt_PNR" id="txt_PNR" cols="70" rows="3" errmsg="*请输入PNR！" valid="required"><%=PNR %></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                支付方式：
            </th>
            <td width="779" bgcolor="#E3F1FC">
                <asp:DropDownList runat="server" ID="ddl_PayType">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                订单须知：
            </th>
            <td width="779" bgcolor="#E3F1FC">
                <textarea name="txt_Order" id="txt_Order" cols="70" rows="3"> <%=Notice%></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="right">
                备注：
            </th>
            <td width="779" bgcolor="#E3F1FC">
                <textarea name="txt_Remark" id="txt_Remark" cols="70" rows="3"><%=Remark %></textarea>
            </td>
        </tr>
    </table>
    <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="40" align="center">
            </td>
            <td height="40" align="center" class="tjbtn02">
                <asp:LinkButton ID="lbtn_submit" runat="server" OnClick="lbtn_submit_Click">保存</asp:LinkButton>
            </td>
            <td height="40" align="center" class="tjbtn02">
                <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                    关闭</a>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hideId" runat="server" />
    <input type="hidden" runat="server" id="hideDoType" name="hideDoType" class="hideDoType" />
    <input type="hidden" runat="server" id="hidIsExtsisTicket" name="hidIsExtsisTicket" />
    <input type="hidden" runat="server" id="hidRouteName" name="hidRouteName" />
    <input type="hidden" runat="server" id="hidOrderId" name="hidOrderId" />
    </form>

    <script type="text/javascript">
        $(function() {
            function isnull(v, defaultValue) {
                if (v == null || !v)
                    return defaultValue;
                else
                    return v;
            }
            function formatFloat(src, pos) {
                return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
            }
            function FloatAdd(arg1, arg2) {
                var r1, r2, m;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2))
                return formatFloat((arg1 * m + arg2 * m) / m, 2)
            }
            function FloatSub(arg1, arg2) {
                var r1, r2, m, n;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2));
                //动态控制精度长度  
                n = (r1 >= r2) ? r1 : r2;
                return formatFloat(((arg1 * m - arg2 * m) / m).toFixed(n), 2);
            }
            //浮点数除法运算  
            function FloatDiv(arg1, arg2) {
                var t1 = 0, t2 = 0, r1, r2;
                try { t1 = arg1.toString().split(".")[1].length } catch (e) { }
                try { t2 = arg2.toString().split(".")[1].length } catch (e) { }
                with (Math) {
                    r1 = Number(arg1.toString().replace(".", ""))
                    r2 = Number(arg2.toString().replace(".", ""))
                    return formatFloat((r1 / r2) * pow(10, t2 - t1), 2);
                }
            }
            //浮点数乘法运算  
            function FloatMul(arg1, arg2) {
                var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
                try { m += s1.split(".")[1].length } catch (e) { }
                try { m += s2.split(".")[1].length } catch (e) { }
                return formatFloat(Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m), 2);
            }

            function sum() {
                $("#txt_SumPrice").val(FloatAdd(isnull(parseFloat($("#txt_piaokuan").val()), 0), isnull(parseFloat($("#txt_piaokuan2").val()), 0)));
            }
            $("[name='txt_piaomianjia'],[name='txt_shui'],[name='txt_pepoleNum'],[name='txt_DaiLiFei'],[name='txt_Percent']").keyup(function() {
                //txt_piaokuan=(票面价+税/机建)*人数+代理费)
                var piao = isnull(parseFloat($(this).closest("tr").find("[name='txt_piaomianjia']").val()), 0);
                var shui = isnull(parseFloat($(this).closest("tr").find("[name='txt_shui']").val()), 0);
                var pepoleNum = isnull(parseFloat($(this).closest("tr").find("[name='txt_pepoleNum']").val()), 0);
                var DaiLiFei = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei']").val()), 0);
                var txt_piaokuan = $(this).closest("tr").find("[name='txt_piaokuan']");
                var otherMoney = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei']").val()), 0);
                var percent = isnull(FloatDiv(parseFloat($(this).closest("tr").find("[name='txt_Percent']").val()), 100), 1);

                txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                switch ($("#<%=hideDoType.ClientID %>").val()) {
                    case "公式一": //txt_piaokuan=(票面价+税/机建)*人数+代理费)
                        {
                            txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "公式二": //txt_piaokuan=(票面价+税/机建)*人数-代理费)
                        {
                            txt_piaokuan.val(FloatSub((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "公式三": //（票面价*百分比+机建燃油）*人数+其它费用
                        {
                            txt_piaokuan.val(FloatAdd((FloatMul(piao, percent) + shui) * pepoleNum, otherMoney));
                        } break;
                }
                sum();
            });

            $("[name='txt_piaomianjia2'],[name='txt_shui2'],[name='txt_pepoleNum2'],[name='txt_DaiLiFei2'],[name='txt_Percent2']").keyup(function() {
                var piao = isnull(parseFloat($(this).closest("tr").find("[name='txt_piaomianjia2']").val()), 0);
                var shui = isnull(parseFloat($(this).closest("tr").find("[name='txt_shui2']").val()), 0);
                var pepoleNum = isnull(parseFloat($(this).closest("tr").find("[name='txt_pepoleNum2']").val()), 0);
                var DaiLiFei = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei2']").val()), 0);
                var txt_piaokuan = $(this).closest("tr").find("[name='txt_piaokuan2']");
                var otherMoney = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei2']").val()), 0);
                var percent = isnull(FloatDiv(parseFloat($(this).closest("tr").find("[name='txt_Percent2']").val()), 100), 1);
                txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                switch ($("#<%=hideDoType.ClientID %>").val()) {
                    case "公式一": //txt_piaokuan=(票面价+税/机建)*人数+代理费)
                        {
                            txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "公式二": //txt_piaokuan=(票面价+税/机建)*人数-代理费)
                        {
                            txt_piaokuan.val(FloatSub((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "公式三": //（票面价*百分比+机建燃油）*人数+其它费用
                        {
                            txt_piaokuan.val(FloatAdd((FloatMul(piao, percent) + shui) * pepoleNum, otherMoney));
                        } break;
                }
                sum();
            });

            $("#<%=lbtn_submit.ClientID %>").click(function() {
                var form = $(this).closest("form").get(0);
                if (!ValiDatorForm.validator(form, "alert")) {
                    return false;
                }
            })
        })
    </script>

</body>
</html>
