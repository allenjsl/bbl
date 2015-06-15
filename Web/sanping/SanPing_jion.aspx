<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="SanPing_jion.aspx.cs"
    Inherits="Web.sanping.SanPing_jion" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.DatePicker" TagPrefix="cc2" %>
<%@ Register Src="../UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/back.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        function isnull(v, defaultValue) {
            if (v == null || !v)
                return defaultValue;
            else
                return v;
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
        function formatFloat(src, pos) {
            return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
        }
        $(function() {
            $("#btn1").click(function() {
                parent.Boxy.iframeDialog({
                    iframeUrl: "tefu.html",
                    title: "特服",
                    modal: true
                });
                return false;
            });

            $("#link1").click(function() {
                var url = $(this).attr("href");
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "特服",
                    modal: true,
                    width: "420px",
                    height: "200px"
                });
                return false;
            });

            $(".selectTeam").click(function() {
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
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
        });
    </script>

</head>
<body>
    <form runat="server" id="myform" enctype="multipart/form-data">
    <table width="880" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
        <tr class="odd">
            <th height="25" align="center">
                <font class="xinghao">*</font>组团社：
            </th>
            <td colspan="2">
                <input type="hidden" name="hd_teamId" id="hd_teamId" value="" /><input type="text"
                    id="txt_teamName" errmsg="*请选择组团社" valid="required" name="txt_teamName" /><a href="/CRM/customerservice/SelCustomer.aspx?method=selectTeam"
                        class="selectTeam"><img src="../images/sanping_04.gif" width="28" height="18"></a>
            </td>
            <td style="text-align: center">
                <b>对方团号：</b>
            </td>
            <td>
                <input type="text" class="searchinput searchinput02" id="txtBuyerTourCode" runat="server"
                    maxlength="50" />
            </td>
        </tr>
        <tr class="even">
            <th height="25" align="center">
                线路名称：
            </th>
            <td colspan="2" align="center">
                【<asp:Literal ID="lt_teamName" runat="server"></asp:Literal>
                】
                <asp:Literal ID="lt_xianluName" runat="server"></asp:Literal>
            </td>
            <th align="center">
                出团时间：
            </th>
            <td align="center">
                <asp:Literal ID="lt_startDate" runat="server"></asp:Literal>
                <asp:HiddenField runat="server" ID="hid_StartDate" />
                &nbsp;
            </td>
        </tr>
        <tr class="odd">
            <th height="25" align="center">
                当前空位：
            </th>
            <td colspan="4" align="left">
                <font class="fbred">
                    <asp:Label runat="server" ID="lbshengyu"></asp:Label>
                    <%--<asp:Literal ID="lt_shengyu" runat="server"></asp:Literal>--%></font>
            </td>
        </tr>
        <tr class="even">
            <th height="25" align="center">
                关联交通：
            </th>
            <td colspan="4" align="left">
                <select id="selectTraffic" name="selectTraffic" data-oldprice="">
                    <%=strTraffic %>
                </select>
            </td>
        </tr>
        <tr class="odd">
            <th height="25" align="center">
                出发交通：
            </th>
            <td colspan="2" align="center">
                <asp:Literal runat="server" ID="lt_startBus"></asp:Literal>
            </td>
            <th align="center">
                返程交通：
            </th>
            <td align="center">
                <asp:Literal runat="server" ID="lt_backBus"></asp:Literal>
            </td>
        </tr>
        <tr class="even">
            <th align="center">
                销售员：
            </th>
            <td colspan="2" align="center" id="lb_seller">
                <asp:Literal ID="lt_seller" runat="server"></asp:Literal>
            </td>
            <th align="center">
                计调员：
            </th>
            <td align="center">
                <asp:Literal ID="lt_oprator" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr class="odd">
            <th height="25" align="center">
                联系人：
            </th>
            <td align="left" colspan="2">
                <asp:TextBox ID="lb_username" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
            </td>
            <th align="center">
                手机：
            </th>
            <td align="left">
                <asp:TextBox ID="lt_tel" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="even">
            <th align="center">
                电话：
            </th>
            <td colspan="2">
                <asp:TextBox CssClass="searchinput searchinput02" ID="lt_phone" runat="server"></asp:TextBox>
            </td>
            <th align="center">
                传真：
            </th>
            <td>
                <asp:TextBox runat="server" CssClass="searchinput searchinput02" ID="lt_fax"></asp:TextBox>
            </td>
        </tr>
        <tr class="odd">
            <th height="55" align="center">
                <font class="xinghao">*</font>结算价：
            </th>
            <td colspan="4" align="left" valign="middle">
                <input type="hidden" id="hd_level" name="hd_level" />
                <input type="hidden" id="hd_cuslevel" name="hd_cuslevel" errmsg="*请选择结算价" valid="required" />
                <input type="hidden" id="hd_cr_price" name="hd_cr_price" />
                <input type="hidden" id="hd_rt_price" name="hd_rt_price" />
                <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <asp:Repeater ID="rpt_price" runat="server" OnItemDataBound="rpt_price_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td width="40" valign="middle">
                                    <%#Eval("Standardname") %>：
                                </td>
                                <td height="25">
                                    <div class="divPrice" val="<%#Eval("StandardId") %>">
                                        <table>
                                            <tr>
                                                <asp:Repeater ID="rpt_list" runat="server">
                                                    <ItemTemplate>
                                                        <td>
                                                            <div style="text-align: center;">
                                                                <%#Eval("LevelName")%></div>
                                                            <input type="radio" class="rd_levelId" name="rd_levelId" id="rd_levelId" value="<%#Eval("LevelId") %>" />
                                                            成人价：<span name="sp_cr_price"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((decimal)Eval("AdultPrice")).ToString("0.00"))%></span><input
                                                                type="hidden" name="hid_cr_price" value='<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((decimal) Eval("AdultPrice")).ToString("0.00"))%>' />
                                                            儿童价：<span name="sp_et_price"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((decimal)Eval("ChildrenPrice")).ToString("0.00"))%></span><input
                                                                type="hidden" name="hid_et_price" value='<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((decimal)Eval("ChildrenPrice")).ToString("0.00"))%>' />
                                                        </td>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr class="even">
            <th height="25" align="center">
                人数：
            </th>
            <td colspan="2" align="left">
                <font class="xinghao">*</font>成人数：
                <input onchange="ChangeCustomVal()" name="txt_crNum" errmsg="*请输入成人数|*请输入正确的成人数"
                    valid="required|isNumber" type="text" id="txt_crNum" runat="server" class="searchinput searchinput03" />
                儿童数：
                <input onchange="ChangeCustomVal()" name="txt_rtNum" type="text" id="txt_rtNum" runat="server"
                    class="searchinput searchinput03" />
                返佣：<input type="hidden" id="hd_rebateType" name="hd_rebateType" /><input type="hidden"
                    id="hd_orderOprator" name="hd_orderOprator" />
                <input name="txt_Rebate" errmsg="*请输入返佣金额|*请输入正确的金额" valid="required|isMoney" type="text"
                    id="txt_Rebate" runat="server" class="searchinput searchinput03" />
            </td>
            <th>
                <font class="xinghao">*</font>对方操作员：
            </th>
            <td>
                <select name="otherOprator" id="otherOprator" errmsg="*请选择对方操作员" valid="required">
                    <option value="">请选择</option>
                </select>
            </td>
        </tr>
        <tr class="odd">
            <th>
                增加费用：
            </th>
            <td colspan="2">
                <input type="text" errmsg="*请输入金额的正确格式" valid="isMoney" class="searchinput" id="txt_addmoney"
                    name="txt_addmoney" />
            </td>
            <th>
                减少费用：
            </th>
            <td>
                <input type="text" errmsg="*请输入金额的正确格式" valid="isMoney" class="searchinput" id="txt_minusmoney"
                    name="txt_minusmoney" />
            </td>
        </tr>
        <tr class="even">
            <th>
                备注：
            </th>
            <td colspan="4">
                <textarea class="textareastyle02" rows="5" cols="45" id="txt_remark" name="txt_remark"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th height="25" align="center">
                总金额：
            </th>
            <td colspan="4" align="left">
                <input name="txt_sumMoney" errmsg="*请输入金额的正确格式" valid="isMoney" type="text" id="txt_sumMoney"
                    runat="server" class="searchinput" />
            </td>
        </tr>
        <tr class="even">
            <th height="30" rowspan="2" align="center">
                游客信息：
            </th>
            <td height="30" colspan="4" align="center">
                <table width="50%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="80%" align="center">
                            上传附件：
                            <input type="file" name="fuiLoadAttachment" id="fuiLoadAttachment" />
                        </td>
                        <td width="20%" align="left">
                            <uc1:LoadVisitors ID="LoadVisitors1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="even">
            <td colspan="4" align="center">
                <table width="95%" border="0" align="center" id="tblVisitorList" cellpadding="0"
                    cellspacing="1" bgcolor="#BDDCF4" style="margin: 10px 0;">
                    <tr>
                        <td height="5%" align="center" bgcolor="#E3F1FC">
                            编号
                        </td>
                        <td height="25" align="center" bgcolor="#E3F1FC">
                            姓名
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            类型
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件名称
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件号码
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            性别
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            联系电话
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            特服
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            操作
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <th height="42" align="center">
                特殊要求说明：
            </th>
            <td colspan="4" align="left">
                <textarea class="textareastyle02" rows="5" cols="45" runat="server" id="txt_Special"
                    name="txt_Special"></textarea>
            </td>
        </tr>
        <tr class="even">
            <th height="42" align="center">
                操作留言：
            </th>
            <td colspan="4" align="left">
                <textarea class="textareastyle02" rows="5" cols="45" runat="server" id="txt_actMsg"
                    name="txt_actMsg"></textarea>
            </td>
        </tr>
        <tr>
            <th colspan="5" align="center">
                <table width="320" border="0" cellspacing="0" cellpadding="0" style="margin: 0 auto;">
                    <tr>
                        <td height="40" align="center">
                        </td>
                        <td align="center" class="tjbtn02">
                            <input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                            <asp:LinkButton ID="LinkButton1" CommandName="submit" runat="server" OnClick="LinkButton1_Click">确认提交</asp:LinkButton>
                        </td>
                        <td height="40" align="center" class="tjbtn02">
                            <asp:LinkButton ID="LinkButton2" runat="server">同意留位</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </th>
        </tr>
    </table>
    <table style="height: 45px; position: relative; background-color: White; display: none;
        border: 1px solid gray; top: -110px; left: 380px;" id="tbl_last">
        <tr>
            <td>
                <input type="hidden" value="" runat="server" id="hd_waitTime"><asp:TextBox ID="txtEndTime"
                    name="SaveSeatDate" onfocus="WdatePicker({errDealMode:1,minDate:new Date(),dateFmt:'yyyy-MM-dd HH:mm',alwaysUseStartDate:true});"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="btnYes" runat="server" OnClick="LinkButton1_Click" CommandName="Reservations">确定</asp:LinkButton>
                &nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onclick="document.getElementById('tbl_last').style.display='none'">
                    取消</a>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">

        //根据用户输入的身份证号判断性别
        function getSex(obj) {
            var val = $(obj).val();
            var tr = $(obj).parent().parent();
            var sex = tr.children().children("select[name='ddlSex']");
            var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
            if (isIdCard.exec(val)) {
                if (15 == val.length) {// 15位身份证号码
                    if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                        sex.val(2);
                    else
                        sex.val(1);
                }

                if (18 == val.length) {// 18位身份证号码
                    if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                        sex.val(2);
                    else
                        sex.val(1);
                }
            } else {
                sex.val(0);
            }
        }
        var sum = 0;
        function querystring(uri, val) {
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }

        function selectTeam(id, name) {
            $("#hd_teamId").val(id);
            $("#txt_teamName").val(name);
            //arguments[4]为团号是否必填
            if (arguments[4] == "True") {
                $("#txtBuyerTourCode").attr("errmsg", "*请填写对方团号").attr("valid", "required")
            }
            $("#otherOprator").change(function() {
                var selectOprator = ($("#otherOprator").get(0).options[$(this).get(0).selectedIndex].text);
                if (selectOprator != "请选择") {
                    $("#hd_orderOprator").val(selectOprator);
                }
            });
            $("#txt_Rebate").keyup(CalculationPrice);
            $.newAjax({
                type: "GET",
                dataType: "json",
                url: "SanPing_jion.aspx",
                data: { act: "getSeller", comId: id, iframeId: '<%=Request.QueryString["iframeId"] %>' },
                cache: false,
                success: function(d) {
                    $("#lb_seller").html(d[0].saler);
                    $("#txt_Rebate").val(d[0].CommissionCount);
                    document.getElementById("otherOprator").options.length = 1;
                    for (var i = 0; i < d[0].cusList.length; i++) {
                        document.getElementById("otherOprator").options.add(new Option(d[0].cusList[i].Name, d[0].cusList[i].ID));
                    }
                    $("#hd_rebateType").val(d[0].CommissionType);
                    CalculationPrice();
                },
                error: function(d) {
                    alert(d);
                }
            });
        }
        function CalculationPrice() {
            var sumprice = isnull(parseFloat($("#hd_cr_price").val()), 0) * isnull(parseFloat($("#txt_crNum").val()), 0) +
                           isnull(parseFloat($("#hd_rt_price").val()) * parseFloat($("#txt_rtNum").val()), 0) + isnull(sum, 0);
            //var trafficPrice = isnull($.trim($("#selectTraffic").find("option:selected").attr("data-price")), 0);
            sumprice = FloatSub(FloatAdd(sumprice, isnull(parseFloat($("#txt_addmoney").val()), 0)), isnull(parseFloat($("#txt_minusmoney").val()), 0))
            //+(isnull(parseFloat($("#txt_crNum").val()), 0) + isnull(parseFloat($("#txt_rtNum").val()), 0)) * trafficPrice;
            if (parseInt(isnull($("#hd_rebateType").val(), 0)) == 1) {
                sumprice = FloatSub(sumprice, FloatMul(isnull(parseInt($("#txt_crNum").val()), 0) +
                            isnull(parseInt($("#txt_rtNum").val()), 0), isnull(parseFloat($("#txt_Rebate").val()), 0)));
            }
            $("#txt_sumMoney").val(sumprice);
        }
        $(function() {
            $("#<%=LinkButton1.ClientID %>").click(function() {
                var msg = "";
                var isb = true;
                var isa = ValiDatorForm.validator($("#myform").get(0), "alert");
                if (isnull(parseFloat($("#<%=txt_sumMoney.ClientID %>").val()), 0) <= 0 && isa)
                { isb = false; msg += "- *总金额必须大于0! \n"; }
                var hd_IsRequiredTraveller = $("#hd_IsRequiredTraveller").val();
                //游客验证（hd_IsRequiredTraveller是否验证根据配置false时后面的参数允许为""，txtVisitorName姓名框name，ddlCardType=证件类型name，txtContactTel=电话框name）
                var msg = visitorChecking.isChecking(hd_IsRequiredTraveller, "txtVisitorName", "ddlCardType", "txtContactTel", "txtCardNo");
                if (!msg.isYes) {
                    alert(msg.msg);
                }
                return isb && isa && msg.isYes;
            });
            $("#<%=LinkButton2.ClientID %>").click(function() {
                if (ValiDatorForm.validator($("#myform").get(0), "alert")) {
                    $("#tbl_last").show();
                }
                return false;
            });

            $("#<%=btnYes.ClientID %>").click(function() {
                if (!$("#txtEndTime").val()) { alert("请选择留位时间!"); return false; }
                var waitTime = $("#hd_waitTime").val();
                var now = new Date();
                var max = new Date(now.toString());
                max.setMinutes(max.getMinutes() + parseInt(waitTime));
                var vvv = new Date($("#txtEndTime").val());
                if (vvv > max) {
                    alert("不能超过最长留位时间!"); return false;
                }
                if (vvv <= now) {
                    alert("留位时间不能小于当前时间!"); return false;
                }
                return ValiDatorForm.validator($("#myform").get(0), "alert");
            });

            $("[name='txt_teamName']").focus(function() {
                $(".selectTeam").click();
            });
            $("[name='txt_crNum']").keyup(function() {
                CalculationPrice();
            });
            $("[name='txt_rtNum']").keyup(function() {
                CalculationPrice();
            });
            loadVisitors.init({ autoComputeToTalAmountHandle: function() {
                sum = 0;
                //alert('1');
                //            if (!/^\d+$/.test($("#txt_sumMoney").val())) {
                //                sum = 0;
                //            } else {
                //                sum = parseInt($("#txt_sumMoney").val());
                //            }
                $("[name='tefu']").each(function() {
                    if (querystring($(this).val(), "ddlOperate") == 0)
                        sum += isnull(parseInt(querystring($(this).val(), "txtCost")), 0);
                    else
                        sum += -isnull(parseInt(querystring($(this).val(), "txtCost")), 0);
                });
                //$("#txt_sumMoney").val(isnull(sum, 0));
                CalculationPrice();
                //alert(querystring($("[name='tefu']").val(), "txtCost"));
            }
            });
            $(".rd_levelId").click(function() {
                //if ($(this).attr("checked")) 
                {

                    $("#hd_cuslevel").val($(this).val());
                    $("#hd_level").val($(this).parents(".divPrice").attr("val"));
                    $("#hd_cr_price").val($(this).next("[name='sp_cr_price']").html());
                    $("#hd_rt_price").val($(this).nextAll("[name='sp_et_price']").html());
                }
                CalculationPrice();
            });
            $("#txt_addmoney").keyup(CalculationPrice);
            $("#txt_minusmoney").keyup(CalculationPrice);
            function VisitorDel(obj) {
                if (confirm("您确定删除该数据？")) {
                    $(obj).parent().parent().remove();
                    $("#txtPersonNum").val(parseInt($("#txtPersonNum").val(), 10) - 1);
                }
            }
            function AddVisitorInfo(obj, frm) {
                $("#tblVisitorList").children(0).children().each(function(i) {
                    if (i > 1)
                        $(this).remove();
                });
                for (var i = 0; i < $(obj).val(); i++) {
                    var CopyTr = $("#trClone").clone();
                    $("#tblVisitorList").append($(CopyTr).removeAttr("style"));
                }
                return true;
            }



        });
        $(document).ready(function() {
            $("#ImgbtnLoad").click(function() {//弹出导入窗口
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "从文件导入",
                    width: "853px",
                    height: "514px",
                    modal: true
                });
                return false;
            });
            $("#linkSpeServe").click(function() {//弹出特服窗口
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "特服",
                    modal: true,
                    width: "420px",
                    height: "200px"
                });
                return false;
            });

            function updatePrice(trafficePrice) {
                $("span[name='sp_cr_price']").each(function() {
                    var self = $(this);
                    self.text(FloatAdd(parseFloat(isnull(self.next("input[type='hidden'][name='hid_cr_price']").val(), 0)), parseFloat(isnull(trafficePrice, 0))));
                });
                $("span[name='sp_et_price']").each(function() {
                    var self = $(this);
                    self.text(FloatAdd(parseFloat(isnull(self.next("input[type='hidden'][name='hid_et_price']").val(), 0)), parseFloat(isnull(trafficePrice, 0))));
                });
                CalculationPrice();
            }

            $("#selectTraffic").change(function() {
                var self = $(this), price = $.trim(self.find("option:selected").attr("data-price")),
                    trafficId = self.find("option:selected").val(),
                    startDate = $("#<%=hid_StartDate.ClientID %>").val();
                if (price == "") {
                    if (trafficId) {
                        $.newAjax({
                            type: "GET",
                            dataType: "json",
                            url: "SanPing_jion.aspx",
                            async: false, //同步执行ajax 默认true为异步请求
                            data: { act: "getPrice", startDate: startDate, trafficId: trafficId, iframeId: '<%=Request.QueryString["iframeId"] %>' },
                            success: function(result) {
                                self.find("option:selected").attr("data-shengyu", result.shengyu);
                                self.find("option:selected").attr("data-price", result.result);
                                updatePrice(result.result);
                            }
                        })
                    }
                    else {
                        updatePrice(0);
                    }
                }
                else {
                    updatePrice(price);
                }
                $("#lbshengyu").text(self.find("option:selected").attr("data-shengyu"));
            })
        });
        function ChangeCustomVal() {
            var oldval = $("#tblVisitorList").find("tr[id=xx]").length;
            var cusSumNum = parseInt(!$("#txt_crNum").val() ? 0 : $("#txt_crNum").val()) + parseInt(!$("#txt_rtNum").val() ? 0 : $("#txt_rtNum").val());
            if (cusSumNum >= oldval) {
                for (var i = 0; i < cusSumNum - oldval; i++) {
                    var CopyTr = $("#tblVisitorList tr:last-child").clone(true);
                    $(CopyTr).find("td.index").html(oldval + i + 1);
                    $("#tblVisitorList").append($(CopyTr));

                }
            }
            else {
                for (var i = oldval - cusSumNum; i > 0; i--) {
                    $("#tblVisitorList tr:last-child").remove();

                }
            }
        }
    </script>

</body>
</html>
