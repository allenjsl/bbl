<%@ Page Title="人事工资表" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="PersonnelSalary.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.PersonnelSalary" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <style type="text/css">
        #table_list input
        {
            width: 93%;
            margin: 0px auto;
        }
        #table_list th{}
    </style>
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">行政中心</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：行政中心>>考勤管理
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td height="50">
                    <div class="searchbox">
                        年份：
                        <select id="sel_Year" name="sel_Year" runat="server" onchange="Select()">
                        </select>
                        <input type="hidden" id="hd_Year" runat="server" />
                        月份：
                        <select id="sel_Month" name="sel_Year" runat="server" onchange="Select()">
                        </select>
                        <input type="hidden" id="hd_Month" runat="server" />
                        <input type="hidden" id="hd_Date" runat="server" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="table_list">
                <tr>
                    <th width="2%" align="center" bgcolor="#BDDCF4">
                        &nbsp;
                    </th>
                    <th width="5%" align="center" bgcolor="#BDDCF4">
                        职位
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4">
                        姓名
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        岗位工资
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        全勤奖
                    </th>
                    <th   width="4%" align="center" bgcolor="#bddcf4">
                        奖金
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        饭补
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        话补
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        油补
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        加班费
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        迟到
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        病假
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        事假
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        行政罚款
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        业务罚款
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        欠款
                    </th>
                    <th  width="6%" align="center" bgcolor="#bddcf4" style=" white-space:normal">
                        应发工资
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        五险
                    </th>
                    <th width="4%" align="center" bgcolor="#bddcf4">
                        社保
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4" style="white-space: normal">
                        实发工资
                    </th>
                    <th  align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpt_list" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc" class="pandl3">
                                <span class="index">
                                    <%#Container.ItemIndex+1 %></span>
                            </td>
                            <td align="center" bgcolor="#e3f1fc" class="pandl3">
                                <input type="text" name="txt_position" value="<%#Eval("ZhiWei") %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_name" value="<%#Eval("XingMing") %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_postWage" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("GangWeiGongZi").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_presentday" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("QuanQinJiang").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_moneyaward" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("JiangJin").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_mealreplenish" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("FanBu").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_phonereplenish" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("HuaBu").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_gasolinereplenish" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("YouBu").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_overtime" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("JiaBanFei").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_belate" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("ChiDao").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_fallill" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("BingJia").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_matter" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("ShiJia").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_administrationpunish" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("XingZhengFaKuan").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_businesspunish" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("YeWuFaKuan").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_arrearage" class="calculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("QianKuan").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_should" onchange="YFCalculate(this)" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("YingFaGongZi").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_insure" class="SFcalculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("WuXian").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_societyinsure" class="SFcalculate" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("SheBao").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="text" name="txt_reality" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("ShiFaGongZi").ToString()) %>" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <font class="fblue"><a href="javascript:void(0);" onclick="Add(this)">添加</a></font>|<font class="fblue"><a href="javascript:void(0);" onclick="Del(this)">删除</a></font>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td align="center" bgcolor="#e3f1fc" class="pandl3">
                        <span id="indexMax" class="index"></span>
                    </td>
                    <td align="center" bgcolor="#e3f1fc" class="pandl3">
                        <input type="text" class="calculate" name="txt_position" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_name" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_postWage" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_presentday" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_moneyaward" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_mealreplenish" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_phonereplenish" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_gasolinereplenish" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_overtime" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_belate" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_fallill" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_matter" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_administrationpunish" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_businesspunish" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="calculate" name="txt_arrearage" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" name="txt_should" onchange="YFCalculate(this)" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="SFcalculate" name="txt_insure" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" class="SFcalculate" name="txt_societyinsure" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="text" name="txt_reality" />
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fblue"><a href="javascript:void(0);" onclick="Add(this)">添加</a></font>|<font class="fblue"><a href="javascript:void(0);" onclick="Del(this)">删除</a></font>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="text-align: center">
            <tr>
                <td height="40" align="center">
                </td>
                <td align="center" class="tjbtn02">
                    <asp:LinkButton ID="lbtn_Save" CommandName="submit" runat="server" OnClick="lbtn_Save_Click">保存</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            //绑定时间
            BindDate();
            //绑定自动计算
            $(".calculate").change(function() {
                Calculate($(this));

            })
            $(".SFcalculate").change(function() {
                SFCalculate($(this));
            })

            $("#indexMax").append($("#table_list").find(".index").length);

        })
        //添加一行
        function Add(obj) {
            $(obj).closest("tr").after($(obj).closest("tr").clone(true));
            $(obj).closest("tr").next().find("[input[type='text']").val("");
            Count();

        }
        //删除本行
        function Del(obj) {

            if (TestDel()) {
                $(obj).closest("tr").remove();
                Count();
                return false;
            }
            else {
                alert("必须保留一行！");
                return false;
            }
        }
        //删除本行验证
        function TestDel() {
            var i = 0;
            $("#table_list").find("tr").each(function() {
                i++;
                if (i > 2) {
                    return true;
                }
            })
            return i > 2;
        }
        //计算公式
        var sign = [0, 0, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, -1, -1, -1];
        //计算
        function Calculate(obj) {
            var YF = 0;
            var SF = 0;
            $(obj).closest("tr").find("input[type='text']").each(function(i) {
                if (i <= 14 && parseFloat($(this).val()).toString() != "NaN") {
                    YF += parseFloat($(this).val()) * sign[i];
                }

                if (i > 15 && i < 18 && parseFloat($(this).val()).toString() != "NaN") {
                    SF += parseFloat($(this).val());
                }

            })
            $(obj).closest("tr").find("[name='txt_should']").val(YF)
            $(obj).closest("tr").find("[name='txt_reality']").val(YF - SF)
        }
        //绑定时间
        function BindDate() {
            var year = parseInt($("#<%=hd_Year.ClientID %>").val());
            var maxYear = parseInt($("#<%=hd_Date.ClientID %>").val());

            var str = "";
            for (var i = 0; i < 10; i++) {
                if (year == maxYear - i) {
                    str += " <option selected=\"selected\" value=\"" + (maxYear - i).toString() + "\">" + (maxYear - i).toString() + "年</option>";
                }
                else {
                    str += " <option value=\"" + (maxYear - i).toString() + "\">" + (maxYear - i).toString() + "年</option>";
                }
            }
            $("#<%=sel_Year.ClientID %>").append(str);
            str = "";
            var month = parseInt($("#<%=hd_Month.ClientID %>").val());

            for (var i = 0; i < 12; i++) {
                if (month == i + 1) {
                    str += " <option selected=\"selected\" value=\"" + (i + 1).toString() + "\">" + (i + 1).toString() + "月</option>"
                }
                else {
                    str += " <option value=\"" + (i + 1).toString() + "\">" + (i + 1).toString() + "月</option>"
                }
            }

            $("#<%=sel_Month.ClientID %>").append(str);
        }
        //改变时间回发
        function Select() {

            location.href = '/administrativeCenter/attendanceManage/PersonnelSalary.aspx?Year=' + $("#<%=sel_Year.ClientID %>").val() + '&Month=' + $("#<%=sel_Month.ClientID %>").val();
        }
        //应发工资改变计算
        function YFCalculate(thiss) {
            if (parseFloat($(thiss).val()).toString() != "NaN") {
                var obj = $(thiss).closest("tr").find("input[type='text']");
                if (parseFloat($(obj.eq(16)).val()).toString() == "NaN") {
                    $(obj.eq(16)).val("0");
                }
                if (parseFloat($(obj.eq(17)).val()).toString() == "NaN") {
                    $(obj.eq(17)).val("0");
                }
                $(obj.eq(18)).val(parseFloat($(thiss).val()) - parseFloat($(obj.eq(16)).val()) - parseFloat($(obj.eq(17)).val()))
            }
            else {
                Calculate($(thiss));
            }
        }
        //实发计算
        function SFCalculate(obj) {
            var arr = $(obj).closest("tr").find("input[type='text']");
            var yf = parseFloat($(arr).eq(15).val());
            var wx = parseFloat($(arr).eq(16).val()).toString() == "NaN" ? 0 : parseFloat($(arr).eq(16).val());
            var sb = parseFloat($(arr).eq(17).val()).toString() == "NaN" ? 0 : parseFloat($(arr).eq(17).val());
            $(arr).eq(18).val(yf - wx - sb);
        }
        function Count() {
            var i = 0;
            $("#table_list").find(".index").each(function() {
                i++;
                $(this).html(i);
            })
            //$("#indexMax").append($("#table_list").find(".index").length + 1);
        }
    </script>

</asp:Content>
