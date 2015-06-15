<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jipiaoUpdate.ascx.cs"
    Inherits="Web.UserControl.jipiaoUpdate" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

<script type="text/javascript" src="/js/ValiDatorForm.js"></script>

<table class="tb_jipiao" width="98%" cellspacing="1" cellpadding="0" border="0" align="center"
    style="margin: 10px;">
    <tbody>
        <tr>
            <td width="12%" height="25" align="right">
                <font class="xinghao">*</font>航班信息：
            </td>
            <td width="88%" colspan="4">
                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                    <tbody>
                        <tr class="odd">
                            <th align="center" height="25" width="14%">
                                日期
                            </th>
                            <th align="center" width="10%">
                                航段
                            </th>
                            <th align="center" width="22%">
                                航班号/时间
                            </th>
                            <th align="center" width="16%">
                                航空公司
                            </th>
                            <th align="center" width="16%">
                                折扣
                            </th>
                            <th align="center" width="16%">
                                操作
                            </th>
                        </tr>
                        <%if (modelinfo != null)
                          {  %>
                        <asp:Repeater runat="server" ID="rpt_hangbang">
                            <ItemTemplate>
                                <tr class="even">
                                    <td align="center" height="25">
                                        <input name="txt_date" onfocus="WdatePicker()" id="txt_date" class="searchinput"
                                            type="text" errmsg="*请输入日期！" valid="required" value="<%#Convert.ToDateTime(Eval("DepartureTime")).ToString("yyyy-MM-dd")%>" />
                                    </td>
                                    <td align="center">
                                        <input type="text" name="sel_Flight" value="<%#Eval("FligthSegment") %>" />
                                    </td>
                                    <td align="center">
                                        <input name="txt_time" id="txt_time" class="searchinput searchinput01" type="text"
                                          style="width:150px;"   value="<%#Eval("TicketTime") %>">
                                    </td>
                                    <td align="center">
                                        <select name="sel_com" style="margin: 0px auto;" id="sel_com" class="selectArea"
                                            errmsg="*请选择航空公司！" valid="required">
                                            <option value="<%#(int)Eval("AireLine") %>"></option>
                                        </select>
                                    </td>
                                    <td align="center">
                                        <input name="txt_Discount" id="txt_Discount" class="searchinput" type="text" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("Discount").ToString()) %> ">
                                        %
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0)" class="addList">
                                            <img alt="" src="../images/tianjiaicon01.gif" height="16" width="15">
                                            添加</a> <a href="javascript:void(0)" class="delList">
                                                <img alt="删除" src="../images/delicon.gif">
                                                删除</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%}
                          else
                          { %>
                        <tr class="even">
                            <td align="center" height="25">
                                <input name="txt_date" onfocus="WdatePicker()" id="txt_date" class="searchinput"
                                    type="text" errmsg="*请输入日期！" valid="required" />
                            </td>
                            <td align="center">
                                <input type="text" name="sel_Flight" id="sel_Flight" class="searchinput searchinput01" />
                            </td>
                            <td align="center">
                                <input name="txt_time" id="txt_time" class="searchinput searchinput01" type="text" style="width:150px;min-width:150px;">
                            </td>
                            <td align="left">
                                <select name="sel_com" id="sel_com" class="selectArea" errmsg="*请选择航空公司！" valid="required">
                                </select>
                            </td>
                            <td align="center">
                                <input name="txt_Discount" id="txt_Discount" class="searchinput" type="text">
                                %
                            </td>
                            <td align="center">
                                <a href="javascript:void(0)" class="addList">
                                    <img alt="" src="../images/tianjiaicon01.gif" height="16" width="15">
                                    添加</a> <a href="javascript:void(0)" class="delList">
                                        <img alt="删除" src="../images/delicon.gif">
                                        删除</a>
                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td height="25" align="right">
                <font class="xinghao">*</font>票款：
            </td>
            <td align="left" colspan="4">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr class="odd">
                            <td height="35" align="left">
                                &nbsp;<b>成人：</b>票面价:
                                <input type="text" class="searchinput" value="<%=piaomianjia %>" id="txt_piaomianjia"
                                    name="txt_piaomianjia" errmsg="*请输入成人价！">
                                税/机建:
                                <input type="text" class="searchinput" value="<%=shui %>" id="txt_shui" name="txt_shui">
                                人数:
                                <input type="text" class="searchinput" value="<%=pepoleNum %>" id="txt_pepoleNum"
                                    name="txt_pepoleNum">
                                    <%
                                        if (!config_Agency.HasValue|| config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                        {
                                         %>
                                代理费:
                                <input type="text" class="searchinput" value="<%=DaiLiFei %>" id="txt_DaiLiFei" name="txt_DaiLiFei">
                                <%}
                                        if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                        {%>
                                <nobr>
                                        百分比:
                                <input type="text" class="searchinput" value="<%=Percent %>" id="txt_Percent" name="txt_Percent">%
                                </nobr>
                                <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" value="<%=OtherMoney %>" id="txt_DaiLiFei" name="txt_DaiLiFei">
                                </nobr>
                                    <%} %>
                                <nobr>
                                票款:
                                <input type="text" class="searchinput" value="<%=piaokuan %>" id="txt_piaokuan" name="txt_piaokuan">
                                </nobr>
                                <font class="fred">
                                    <asp:Label ID="lblMsgFrist" runat="server" Text=""></asp:Label>
                                </font>
                            </td>
                        </tr>
                        <tr class="even">
                            <td height="35" align="left">
                                &nbsp;<b>儿童：</b>票面价:
                                <input type="text" class="searchinput" value="<%=piaomianjia2 %>" id="txt_piaomianjia2"
                                    name="txt_piaomianjia2">
                                税/机建:
                                <input type="text" class="searchinput" value="<%=shui2 %>" id="txt_shui2" name="txt_shui2">
                                人数:
                                <input type="text" class="searchinput" value="<%=pepoleNum2 %>" id="txt_pepoleNum2"
                                    name="txt_pepoleNum2">
                                     <%
                                        if (!config_Agency.HasValue|| config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 || config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二)
                                        {
                                         %>
                                代理费:
                                <input type="text" class="searchinput" value="<%=DaiLiFei2 %>" id="txt_DaiLiFei2" name="txt_DaiLiFei2">
                                <%}
                                        if (config_Agency.HasValue && config_Agency == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                        {%>
                                <nobr>
                                        百分比:
                                <input type="text" class="searchinput" value="<%=Percent2 %>" id="txt_Percent2" name="txt_Percent2">%
                                </nobr>
                                <nobr>
                                        其它费用:
                                <input type="text" class="searchinput" value="<%=OtherMoney2 %>" id="txt_DaiLiFei2" name="txt_DaiLiFei2">
                                </nobr>
                                    <%} %>
                                 <nobr>票款:
                                <input type="text" class="searchinput" value="<%=piaokuan2 %>" id="txt_piaokuan2"
                                    name="txt_piaokuan2"/>
                                    </nobr>
                                <font class="fred">
                                    <asp:Label ID="lblMsgSecond" runat="server" Text=""></asp:Label></font>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <font class="xinghao">*</font>名单：
            </td>
            <td align="left" colspan="4">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr class="odd">
                        <th align="center">序号</th>
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
                                操作<input type="checkbox" id="chkAll" />
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_list">
                            <ItemTemplate>
                                <tr class="even">
                                <td align="center"><%#Container.ItemIndex+1 %></td>
                                    <td height="25" align="center">
                                        <input type="hidden" value="<%#Eval("id") %>" name="hd_uid" />
                                        <input type="text" value="<%#Eval("VisitorName") %>" id="txt_name" class="searchinput"
                                            name="txt_name" errmsg="*请输入名单姓名！" valid="required">
                                    </td>
                                    <td align="center">
                                        <select id="sel_cred" name="sel_cred" errmsg="*请选择证件类型！" valid="required">
                                        <option value="0">请选择证件</option>
                                            <option value="1">身份证</option>
                                            <option value="2">护照</option>
                                            <option value="3">军官证</option>
                                            <option value="4">台胞证</option>
                                            <option value="5">港澳通行证</option>
                                            <option value="6">户口本</option>
                                        </select>
                                        <input type="hidden" name="codetype" value="<%#(int)(EyouSoft.Model.EnumType.TourStructure.CradType)Eval("CradType") %>" />
                                    </td>
                                    <td align="center">
                                        <input type="text" value="<%#Eval("CradNumber") %>" id="txt_code" class="searchinput searchinput02"
                                            name="txt_code">
                                    </td>
                                    <td align="center">
                                        <%--   <%if (CustomerList != null)
                                      {
                                          if (CustomerList.Contains(int.Parse(Eval("id").ToString())))
                                          { 
                                            
                                          }
                                      }
                                           %>--%>
                                        <input type="checkbox" name="chk_md" id="chk_md" <%#isHave(Eval("id").ToString(),(List<int>)Eval("ApplyFlights"),(List<int>)Eval("RefundFlights")) %>
                                            value="<%#Eval("id") %>" />
                                    </td>
                                </tr>
                                <%iii++; %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td height="5" align="right" colspan="5">
            </td>
        </tr>
        <tr>
            <td height="35" align="right">
                PNR：
            </td>
            <td align="left" colspan="4">
                <textarea rows="3" cols="45" id="txt_Pnr" name="txt_Pnr"><%=Png%></textarea>
            </td>
        </tr>
        <tr>
            <td height="25" align="right">
                <font class="xinghao">*</font>总费用：
            </td>
            <td align="left" colspan="4">
                <input type="text" value="<%=Total %>" id="txt_SumPrice" class="searchinput" name="txt_SumPrice">
                ￥
            </td>
        </tr>
        <tr>
            <td height="30" align="right">
                支付方式：
            </td>
            <td align="left" colspan="4">
                <select id="sel_paytype" name="sel_paytype">
                    <option value="1">财务现收</option>
                    <option value="2">财务现付</option>
                    <option value="3">导游现收</option>
                    <option value="4">银行电汇</option>
                    <option value="5">转账支票</option>
                    <option value="6">导游现付</option>
                    <option value="7">签单挂账</option>
                    <option value="8">预存款支付</option>
                </select>
                <input type="hidden" value="<%=paytype %>" name="hd_paytype" />
            </td>
        </tr>
        <tr style="display:<%=toft==EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime.Apply?"":"none"%>;">
            <td height="30" align="right">
                售票处：
            </td>
            <td height="30" align="left">
                <input id="hd_PiaoWuSuppId" type="hidden" runat="server" />
                <input type="text" class="searchinput searchinput02" id="txtSalePlace" name="txtSalePlace"
                    runat="server" readonly="readonly" />
                <img id="salePlace" src="/images/sanping_04.gif" />
            </td>
        </tr>
        <tr>
            <td align="right">
                订票需知：
            </td>
            <td align="left" colspan="4">
                <textarea name="txt_Order" id="txt_Order" class="textareastyle"> <%=Notice%></textarea>
            </td>
        </tr>
        <tr>
            <td height="5" align="right" colspan="5">
            </td>
        </tr>
        <tr>
            <td height="30" align="right">
                备注：
            </td>
            <td align="left" colspan="4">
                <textarea name="textRemark" id="textRemark" class="textareastyle"><%=Remark%></textarea>
            </td>
        </tr>
    </tbody>
</table>
<table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
    <tbody>
        <tr>
            <td height="40" align="center">
            </td>
            <td align="center" class="tjbtn02">
                <asp:LinkButton ID="lbtn_submit" runat="server" OnClick="lbtn_submit_Click">保存</asp:LinkButton>
            </td>
            <td align="center" class="tjbtn02">
                <a id="linkCancel" href="javascript:history.back()">返回</a>
            </td>
        </tr>
    </tbody>
</table>
<asp:HiddenField ID="hideAreaList" runat="server" />
<asp:HiddenField ID="hideId" runat="server" />
<asp:HiddenField ID="hideTourId" runat="server" />
<input type="hidden" runat=server id="hideDoType" name="hideDoType" class="hideDoType" />

<script type="text/javascript">
    $("#salePlace").click(function() {
        parent.Boxy.iframeDialog({ iframeUrl: "/TeamPlan/DiJieList.aspx", title: "票务供应商", modal: true, width: "700px", height: "300px", data: 'text=<%=txtSalePlace.ClientID%>&value=<%=hd_PiaoWuSuppId.ClientID%>&sType=<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通 %>' });
    })
    function formatFloat(src, pos) {
        return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
    }
    $(function() {
        function sum() {
            $("#txt_SumPrice").val(FloatAdd(isnull(parseFloat($("#txt_piaokuan").val()), 0), isnull(parseFloat($("#txt_piaokuan2").val()), 0)));
        }
        $("#txt_piaokuan,#txt_piaokuan2").keyup(sum);
        //航空公司下拉框data
        var areaList = $("#<%=hideAreaList.ClientID %>").val();
        $("[name='codetype']").each(function(d) {
            $(this).prev("select").val($(this).val());
        })
        $("[name='hd_paytype']").prev("select").val($("[name='hd_paytype']").val());
        $("#<%=lbtn_submit.ClientID %>").click(function() {
            var chk = false;
            $("[name=chk_md]").each(function() {
                if ($(this).attr("checked")) {
                    chk = true;
                }
            })
            if (!chk) {
                alert("请先选择游客名单!");
                return false;
            }
            FV_onBlur.initValid($("#<%=lbtn_submit.ClientID %>").closest("form").get(0));
            var form = $(this).closest("form").get(0);

            if (isnull(parseInt($("#txt_pepoleNum").val()), 0) + isnull(parseInt($("#txt_pepoleNum2").val()), 0) <= 0) {
                alert("成人数与儿童数之和应大于0"); return false;
            }

            //点击按纽触发执行的验证函数
            return ValiDatorForm.validator(form, "alert");
        });
        //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
        FV_onBlur.initValid($("#<%=lbtn_submit.ClientID %>").closest("form").get(0));

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
        $(".addList").click(function() {
            $(this).closest(".even").clone(true).insertAfter($(this).closest(".even")).find("input").val("");
        });
        $(".delList").click(function() {
            if ($(this).closest(".even").parent().find(".even").length > 1)
                $(this).closest(".even").remove();
        });
        $("[name='txt_piaomianjia'],[name='txt_shui'],[name='txt_pepoleNum'],[name='txt_DaiLiFei'],[name='txt_Percent']").keyup(
        function() {
            //txt_piaokuan=(票面价+税/机建)*人数+代理费)
            var piao = isnull(parseFloat($(this).closest("tr").find("[name='txt_piaomianjia']").val()), 0);
            var shui = isnull(parseFloat($(this).closest("tr").find("[name='txt_shui']").val()), 0);
            var pepoleNum = isnull(parseFloat($(this).closest("tr").find("[name='txt_pepoleNum']").val()), 0);
            var DaiLiFei = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei']").val()), 0);
            var txt_piaokuan = $(this).closest("tr").find("[name='txt_piaokuan']");
            var otherMoney = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei']").val()), 0);
            var percent = isnull(FloatDiv(parseFloat($(this).closest("tr").find("[name='txt_Percent']").val()), 100), 1);

            txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
            switch ($(".hideDoType").val()) {
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
        }
        );

        $("[name='txt_piaomianjia2'],[name='txt_shui2'],[name='txt_pepoleNum2'],[name='txt_DaiLiFei2'],[name='txt_Percent2']").keyup(
        function() {

            var piao = isnull(parseFloat($(this).closest("tr").find("[name='txt_piaomianjia2']").val()), 0);
            var shui = isnull(parseFloat($(this).closest("tr").find("[name='txt_shui2']").val()), 0);
            var pepoleNum = isnull(parseFloat($(this).closest("tr").find("[name='txt_pepoleNum2']").val()), 0);
            var DaiLiFei = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei2']").val()), 0);
            var txt_piaokuan = $(this).closest("tr").find("[name='txt_piaokuan2']");
            var otherMoney = isnull(parseFloat($(this).closest("tr").find("[name='txt_DaiLiFei2']").val()), 0);
            var percent = isnull(FloatDiv(parseFloat($(this).closest("tr").find("[name='txt_Percent2']").val()), 100), 1);
            txt_piaokuan.val(FloatAdd((piao + shui) * pepoleNum, DaiLiFei));
            switch ($(".hideDoType").val()) {
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
        function isnull(v, defaultValue) {
            if (v == null || !v)
                return defaultValue;
            else
                return v;
        }

        //页面初始化设置航空公司下拉框
        $(".selectArea").each(function() {
            var selectIndex = $(this).val();
            proArray = areaList.split("|");
            $(this).html("");
            for (var i = 0; i < proArray.length; i++) {
                var obj = eval('(' + proArray[i] + ')');
                $(this).append("<option value=\"" + obj.value + "\" " + (selectIndex == obj.value ? "selected=\"selected\"" : "") + ">" + obj.text + "</option>");
            }
        })
        $("#chkAll").click(function() {
            if ($(this).attr("checked") == true) {
                $("[name='chk_md']:not([disabled='disabled'])").attr("checked", true);
            }
            else {
                $("[name='chk_md']:not([disabled='disabled'])").attr("checked", false);
            }
        });
        $("[name='chk_md']").each(function() {
            $(this).click(function() {
                if ($(this).attr("checked") == false) {
                    $("#chkAll").attr("checked", false);
                }
            })
        })
    });
  </script>

