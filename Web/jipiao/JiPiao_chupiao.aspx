<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiPiao_chupiao.aspx.cs"
    Inherits="Web.jipiao.JiPiao_chupiao" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>机票出票</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <style type="text/css">
        .errmsg
        {
            color: red;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="hdOrderID" type="hidden" runat="server" />
        <input id="hdTourID" type="hidden" runat="server" />
        <input id="hd_cardType" type="hidden" runat="server" />
        <input id="hd_airCompany" type="hidden" runat="server" />
        <input id="hd_LineName" type="hidden" runat="server" />
        <table width="890" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 5px;">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center">
                        航班信息：
                    </th>
                    <td align="left">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr class="odd">
                                    <th height="15%" width="14%" align="center">
                                        日期
                                    </th>
                                    <th height="35%" width="14%" align="center">
                                        航班号/时间
                                    </th>
                                    <th width="20%" align="center">
                                        航段
                                    </th>
                                    <th width="10%" align="center">
                                        航空公司
                                    </th>
                                    <th width="7%" align="center">
                                        折扣
                                    </th>
                                    <th align="center" width="13%">
                                        操作
                                    </th>
                                </tr>
                                <asp:Repeater ID="RepAirList" runat="server">
                                    <ItemTemplate>
                                        <tr class="even">
                                            <td height="25" align="center" height="15%">
                                                <input type="text" style="border: 1px solid #93B7CE; font-size: 12px; height: 15px;
                                                    width: 100px" id="txtAirDate" value="<%# string.Format("{0:d}", Eval("DepartureTime"))%>""
                                                    name="txtAirTime" onfocus="WdatePicker()" valid="required" errmsg="请输入日期!" />
                                            </td>
                                            <td align="center" height="35%">
                                                <input type="text" style="border: 1px solid #93B7CE; font-size: 12px; height: 15px;
                                                    width: 190px" name="txt_hbh_date" valid="required" errmsg="请输入航班号/时间!" id="txt_hbh_date"
                                                    value="<%#Eval("TicketTime") %>" />
                                            </td>
                                            <td align="center" width="20%">
                                                <input id="selAirLine" name="selAirLine" type="text" value="<%#Eval("FligthSegment")%>"
                                                    valid="required" errmsg="请输入航段!" />
                                            </td>
                                            <td align="center" width="10%">
                                                <select id="SelAirCompany" name="SelAirCompany" title="请选择" valid="required" errmsg="请选择航空公司!">
                                                    <option value="<%#(int)Eval("AireLine")%>">
                                                        <%#Eval("AireLine")%></option>
                                                </select>
                                            </td>
                                            <td align="center" width="7%">
                                                <input type="text" style="border: 1px solid #93B7CE; font-size: 12px; height: 15px;
                                                    width: 50px" id="txtZheKo" value="<%#FilterEndOfTheZeroDecimal((Eval("Discount")))%>"
                                                    name="txtZheKo" valid="isNumber" errmsg="请输入有效的折扣!">%
                                            </td>
                                            <td align="center" width="13%">
                                                <a href="javascript:" class="addList">
                                                    <img src="/images/tianjiaicon01.gif" alt="">
                                                    添加</a> <a href="javascript:void(0)" class="delList">
                                                        <img alt="删除" src="/images/delicon.gif">
                                                        删除</a>
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
                        <table width="100%" cellspacing="1" cellpadding="0" border="0" id="tbl_treavel">
                            <tbody>
                                <tr class="odd">
                                    <th height="30" bgcolor="#bddcf4" align="center" width="5%">
                                        序号
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
                                    <th align="center">
                                        操作
                                    </th>
                                </tr>
                                <cc1:CustomRepeater ID="RepCusList" runat="server">
                                    <ItemTemplate>
                                        <tr class="even">
                                            <td height="30" align="center">
                                                <%# Container.ItemIndex+1 %>
                                            </td>
                                            <td height="25" align="center">
                                                <input type="text" class="searchinput" id="txtCusName" name="txtCusName" value="<%#Eval("VisitorName")%>" />
                                            </td>
                                            <td align="center">
                                                <select id="SelCardType" name="SelCardType">
                                                    <option>
                                                        <%#Eval("CradType")%></option>
                                                </select>
                                            </td>
                                            <td align="center">
                                                <input type="text" class="searchinput searchinput02" id="txtCardNumber" name="txtCardNumber"
                                                    value="<%#Eval("CradNumber")%>" />
                                            </td>
                                            <td align="center">
                                                <input type="checkbox" name="chkOper" value="<%#Eval("id")%>" <%#isHave(Eval("id").ToString(),(System.Collections.Generic.List<int>)Eval("ApplyFlights"),(System.Collections.Generic.List<int>)Eval("RefundFlights")) %> />
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
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效票面价!" />
                                        税/机建:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultShui" name="txtAdultShui"
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效税/机建!" />
                                        人数:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultCount" name="txtAdultCount"
                                            runat="server" valid="isNumber" maxlength="5" errmsg="请填写有效人数!" />
                                        <%
                                            if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                            {
                                        %>
                                        代理费:
                                        <input type="text" class="searchinput" id="txtAdultProxyPrice" runat="server" name="txtAdultProxyPrice">
                                        <%}
                                            if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                            {%>
                                        <nobr>
                                        百分比:
                                <input type="text" class="searchinput" runat="server" id="txt_Percent" name="txt_Percent">%
                                </nobr>
                                        <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" runat="server" id="txt_OtherMoney" name="txt_OtherMoney">
                                </nobr>
                                        <%} %>
                                        票款:
                                        <input type="text" class="searchinput searchinput03" id="txtAdultSum" name="txtAdultSum"
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效票款!" />
                                        <font class="fred">
                                            <asp:Label ID="lblMsgFrist" runat="server" Text=""></asp:Label></font>
                                    </td>
                                </tr>
                                <tr class="even">
                                    <td height="35" align="left">
                                        &nbsp;<b>儿童：</b>票面价:
                                        <input type="text" class="searchinput searchinput03" id="txtChildPrice" name="txtChildPrice"
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效票面价!" />
                                        税/机建:
                                        <input type="text" class="searchinput searchinput03" id="txtChildShui" name="txtChildShui"
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效税/机建!" />
                                        人数:
                                        <input type="text" class="searchinput searchinput03" id="txtChildCount" name="txtChildCount"
                                            runat="server" valid="isNumber" maxlength="5" errmsg="请填写有效人数!" />
                                        <%
                                            if (!config_Agency.HasValue || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                            {
                                        %>
                                        代理费:
                                        <input type="text" class="searchinput" runat="server" id="txtChildProxyPrice" name="txtChildProxyPrice">
                                        <%}
                                            if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                            {%>
                                        <nobr>
                                        百分比:
                                <input type="text" class="searchinput" runat="server"  id="txt_PercentChild" name="txt_PercentChild">%
                                </nobr>
                                        <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" runat="server" id="txt_OtherMoneyChild" name="txt_OtherMoneyChild">
                                </nobr>
                                        <%} %>
                                        票款:
                                        <input type="text" class="searchinput searchinput03" id="txtChildSum" name="txtChildSum"
                                            runat="server" valid="isMoney" maxlength="10" errmsg="请填写有效票款!" />
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
                        <span class="errmsg">*</span>总费用：
                    </th>
                    <td height="30" align="left">
                        <input type="text" class="searchinput" id="txtSumMoney" name="txtSumMoney" valid="required|isMoney"
                            errmsg="请输入总费用!|总费用填写有误!" runat="server" maxlength="10" />
                        ￥<span id="errMsg_txtSumMoney" class="errmsg"></span>
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
                        <span id="span_shoupiao" class="errmsg"></span>售票处：
                    </th>
                    <td height="30" align="left">
                        <input id="hd_PiaoWuSuppId" type="hidden" runat="server" />
                        <input type="text" class="searchinput searchinput02" id="txtSalePlace" name="txtSalePlace"
                            runat="server" readonly="readonly" />
                        <img id="salePlace" src="/images/sanping_04.gif" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center">
                        <span id="span_piaohao" class="errmsg"></span>票号：
                    </th>
                    <td height="30" align="left">
                        <textarea rows="3" cols="45" id="txtPiaoHao" name="txtPiaoHao" runat="server"></textarea>
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
                <tr class="odd">
                    <th height="80" align="center">
                        完成出票：
                    </th>
                    <td align="left" colspan="3">
                        <asp:CheckBox ID="IsOk" runat="server" Text="是" />
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="4">
                        <table width="320" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lbtnSave" runat="server" OnClientClick="return CheckIsSelect()"
                                            OnClick="lbtnSave_Click1">保存</asp:LinkButton>
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
    <asp:HiddenField ID="hideTourId" runat="server" />
    </form>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">

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
        $("#salePlace").click(function() {
            parent.Boxy.iframeDialog({ iframeUrl: "/TeamPlan/DiJieList.aspx", title: "票务供应商", modal: true, width: "700px", height: "300px", data: 'text=<%=txtSalePlace.ClientID%>&value=<%=hd_PiaoWuSuppId.ClientID%>&sType=<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通 %>&ifid=<%=Request.QueryString["iframeId"] %>' });
        })
        $(function() {
            $("#<%=this.lbtnSave.ClientID%>").click(function() {
                //成人数与儿童数相加必须大于0
                var chupiaoNum = parseInt($("#<%=this.txtAdultCount.ClientID%>").val()) + parseInt($("#<%=this.txtChildCount.ClientID%>").val());
                if (parseInt(chupiaoNum) <= 0) {
                    alert("成人数与儿童数相加必须大于0!");
                    return false;
                }
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=this.lbtnSave.ClientID%>").closest("form").get(0));

            var proArrayList = $("#<%=hd_airCompany.ClientID %>").val().split("|");
            //航空公司初始化
            $("select[name=SelAirCompany]").each(function() {
                var selectIndex = $(this).val();

                $(this).html("");
                for (var i = 0; i < proArrayList.length; i++) {
                    var obj = eval('(' + proArrayList[i] + ')');
                    if (selectIndex != null && selectIndex != "" && selectIndex == obj.value) {
                        $(this).append("<option value=\"" + obj.value + "\" selected=\"selected\">" + obj.text + "</option>");
                    } else {
                        $(this).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                    }

                }

            });
        })




        $(function() {
            $("#<%=txtAdultPrice.ClientID %>,#<%=txtAdultShui.ClientID %>,#<%=txtAdultCount.ClientID %>,#<%=txtAdultProxyPrice.ClientID %>,#<%=txt_Percent.ClientID %>,#<%=txt_OtherMoney.ClientID %>").blur(function() {
                //票款=(票面价+税/机建)*人数+代理费)
                var piao = parseFloat($("#<%=txtAdultPrice.ClientID %>").val());
                var shui = parseFloat($("#<%=txtAdultShui.ClientID %>").val());
                var pepoleNum = parseFloat($("#<%=txtAdultCount.ClientID %>").val());
                var DaiLiFei = parseFloat($("#<%=txtAdultProxyPrice.ClientID %>").val(), 0);
                var txtAdultSum = $("#<%=txtAdultSum.ClientID %>");
                var otherMoney = isnull(parseFloat($("#<%=txt_OtherMoney.ClientID %>").val()), 0);
                var percent = isnull(FloatDiv(parseFloat($("#<%=txt_Percent.ClientID %>").val()), 100), 1);
                switch ($("#<%=hideDoType.ClientID %>").val()) {
                    case "1": //txt_piaokuan=(票面价+税/机建)*人数+代理费)
                        {
                            txtAdultSum.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "2": //txt_piaokuan=(票面价+税/机建)*人数-代理费)
                        {
                            txtAdultSum.val(FloatSub((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "3": //（票面价*百分比+机建燃油）*人数+其它费用
                        {
                            txtAdultSum.val(FloatAdd((FloatMul(piao, percent) + shui) * pepoleNum, otherMoney));
                        } break;
                }
                $("#<%=txtSumMoney.ClientID %>").val(FloatAdd(txtAdultSum.val(), $("#<%=txtChildSum.ClientID %>").val()));
            });


            $("#<%=txtChildPrice.ClientID %>,#<%=txtChildShui.ClientID %>,#<%=txtChildCount.ClientID %>,#<%=txtChildProxyPrice.ClientID %>,#<%=txt_PercentChild.ClientID %>,#<%=txt_OtherMoneyChild.ClientID %>").blur(function() {
                //票款=(票面价+税/机建)*人数+代理费)
                var piao = isnull(parseFloat($("#<%=txtChildPrice.ClientID %>").val()));
                var shui = isnull(parseFloat($("#<%=txtChildShui.ClientID %>").val()));
                var pepoleNum = isnull($("#<%=txtChildCount.ClientID %>").val());
                var DaiLiFei = isnull($("#<%=txtChildProxyPrice.ClientID %>").val(), 0);
                var txtChildSum = isnull($("#<%=txtChildSum.ClientID %>"));
                var otherMoney = isnull($("#<%=txt_OtherMoneyChild.ClientID %>").val(), 0);
                var percent = isnull(FloatDiv(parseFloat($("#<%=txt_PercentChild.ClientID %>").val()), 100), 1);

                // txtChildSum.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));

                switch ($("#<%=hideDoType.ClientID %>").val()) {
                    case "1": //txt_piaokuan=(票面价+税/机建)*人数+代理费)
                        {
                            txtChildSum.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "2": //txt_piaokuan=(票面价+税/机建)*人数-代理费)
                        {
                            txtChildSum.val(FloatSub((piao + shui) * pepoleNum, DaiLiFei));
                        } break;
                    case "3": //（票面价*百分比+机建燃油）*人数+其它费用
                        {
                            txtChildSum.val(FloatAdd((FloatMul(piao, percent) + shui) * pepoleNum, otherMoney));
                        } break;
                }
                $("#<%=txtSumMoney.ClientID %>").val(FloatAdd(txtChildSum.val(), $("#<%=txtAdultSum.ClientID %>").val()));
            });

        })


        $(".addList").click(function() {
            $(this).closest(".even").clone(true).insertAfter($(this).closest(".even")).find("input").val("");
        });

        $(".delList").click(function() {
            if ($(this).closest(".even").parent().find(".even").length > 1)
                $(this).closest(".even").remove();
        });

        //计算总票款
        $("#<%=txtAdultSum.ClientID %>,<%=txtChildSum.ClientID %>").change(
        function() {
            $("#<%=txtSumMoney.ClientID %>").val(FloatAdd($("#<%=txtAdultSum.ClientID %>").val(), $("#<%=txtChildSum.ClientID %>").val()));
        });

        //证件类型初始化
        $("select[name=SelCardType]").each(function() {
            var selectIndex = $(this).val();
            var proArray = $("#<%=hd_cardType.ClientID %>").val().split("|");
            $(this).html("");
            for (var i = 0; i < proArray.length; i++) {
                var obj = eval('(' + proArray[i] + ')');
                $(this).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
            }
            if (selectIndex != null && selectIndex != "") {
                $(this).val(selectIndex);
            }
        });






        function FloatAdd(arg1, arg2) {
            var r1, r2, m, result;
            try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
            try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
            m = Math.pow(10, Math.max(r1, r2))
            return (arg1 * m + arg2 * m) / m;
        }

        //检查是否勾选出票
        function CheckIsSelect() {

            //勾选  必须选择售票处 和输入票号
            if ($("#<%=IsOk.ClientID %>").attr("checked")) {




                if ($("#<%=txtSalePlace.ClientID %>").val() == "") {
                    $("#span_shoupiao").html("*");
                    $("#<%=txtSalePlace.ClientID %>").focus();
                    alert("请选择售票处!");
                    return false;
                }
                if ($.trim($("#<%=txtPiaoHao.ClientID %>").val()) == "") {
                    $("#span_piaohao").html("*");
                    $("#<%=txtPiaoHao.ClientID %>").focus();
                    alert("请填写票号!");
                    return false;
                }

                var count = 0;
                $("#tbl_treavel").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        count++;
                    }
                })
                if (count <= 0) {
                    alert("请选择旅客!");
                    return false;
                }

                return true;
            } else {
                return true;
            }
        }
    </script>

</body>
</html>
