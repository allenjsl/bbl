<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trafficpricesList.aspx.cs"
    Inherits="Web.jipiao.Traffic.trafficpricesList" MasterPageFile="~/masterpage/Back.Master"
    Title="交通价格" %>

<asp:Content ContentPlaceHolderID="head" ID="contenthead" runat="server">

    <script type="text/javascript" src="../../js/pricesdate.js"></script>

    <script type="text/javascript" src="../../js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="../../js/jquery.boxy.js"></script>

    <script type="text/javascript" src="../../js/ValiDatorForm.js"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="contentBody" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">交通管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前用您所在位置：</b> >> 交通管理 >> 价格
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
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="7%" align="center" bgcolor="#BDDCF4">
                        序列
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        类型
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        出发-抵达
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        航班号
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        航空公司
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        出发时间
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        抵达时间
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        舱位
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4">
                        机型
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        经停
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4">
                        间隔
                    </th>
                </tr>
                <asp:Repeater ID="repTravelList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc" class="pandl3">
                                第<%# Eval("SerialNum")%>段
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("TrafficType".ToString())%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("LCityName")%>-<%#Eval("RCityName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("FilghtNum")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("FlightCompany")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("LTime")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("RTime")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Space").ToString()%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("AirPlaneType")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("IsStop").ToString() == "False" ? "经停" : "直飞"%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("IntervalDays")%>天
                            </td>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="hr_10">
            </div>
            <div class="tablelist01">
                <form>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <th width="13%" height="30" align="center">
                            开始日期
                        </th>
                        <th width="13%" align="center">
                            结束日期
                        </th>
                        <th width="11%" align="center">
                            周一(价/量)
                        </th>
                        <th width="9%" align="center">
                            周二
                        </th>
                        <th width="9%" align="center">
                            周三
                        </th>
                        <th width="9%" align="center">
                            周四
                        </th>
                        <th width="9%" align="center">
                            周五
                        </th>
                        <th width="9%" align="center">
                            周六
                        </th>
                        <th width="9%" align="center">
                            周日
                        </th>
                        <th width="9%" align="center">
                            操作
                        </th>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#FFFFFF">
                            <input type="text" name="txtSDate" class="input70" size="8"
                                onfocus="WdatePicker({disabledDays:[2,3,4,5,6,0]})" valid="required" errmsg="请选择开始日期!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input type="text" name="txtEDate" class="input70" size="8" onfocus="WdatePicker({disabledDays:[1,2,3,4,5,6]})"
                                valid="required" errmsg="请选择结束日期!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input type="text" name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周一票价格!" />
                            <input type="text" name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周一票数量!" />
                            <a href="javascript:" class="Copy">
                                <img src="/images/jt-copybtn.gif" alt="复制" style="vertical-align: top;" /></a>
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周二票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周二票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周三票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周三票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周四票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周四票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周五票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周五票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周六票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周六票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <input name="txtprice" class="input30" size="8" valid="required" errmsg="请输入周日票价格!" />
                            <input name="txtnum" class="input20" size="8" valid="required" errmsg="请输入周日票数量!" />
                        </td>
                        <td align="center" bgcolor="#FFFFFF">
                            <a href="javascript:" class="baocun-btn">保 存</a>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10" align="left" bgcolor="#FFFFFF" class="pandl10">
                            如果一周价格相同,在填入周一的价格后点击方框后面的&quot;<a href="javascript:"><img src="/images/jt-copybtn.gif"
                                alt="复制" style="vertical-align: top;" /></a>&quot;即可复制价格和数量,当日价格变动请点击下面日历操作
                        </td>
                    </tr>
                </table>
                </form>
            </div>
            <div class="hr_10">
            </div>
            <div id="calendarContainer" class="table-rili">
            </div>
            <input type="hidden" id="hidChildrenPrices"  runat="server"/>
        </div>
    </div>
    <div class="clearboth">
    </div>

    <script type="text/javascript">

        function updateStatus(id, tfId) {
            Boxy.iframeDialog({ title: "状态", iframeUrl: "/jipiao/Traffic/updateStatus.aspx?Id=" + id + "&tfId=" + tfId, width: "300px", height: "105px", draggable: true, data: null, hideFade: true, modal: true });
        }
        function updatePrices(id, date, tfId) {
            Boxy.iframeDialog({ title: "修改", iframeUrl: "/jipiao/Traffic/updatePrices.aspx?type=update&Id=" + id + "&date=" + date + "&tfId=" + tfId, width: "300px", height: "160px", draggable: true, data: null, hideFade: true, modal: true });
        }

        function addPrices(date) {
            var tfId='<%=EyouSoft.Common.Utils.GetQueryStringValue("trfficID") %>';
            Boxy.iframeDialog({ title: "新增", iframeUrl: "/jipiao/Traffic/updatePrices.aspx?type=add&date=" + date + "&tfId=" + tfId, width: "300px", height: "160px", draggable: true, data: null, hideFade: true, modal: true });
        }

        $(function() {
            QGD.config.Childrens = $("#<%=hidChildrenPrices.ClientID %>").val();
            QGD.initCalendar({
                containerId: "calendarContainer", //放日历容器ID
                currentDate: <%=thisDate %>, //当前月
                firstMonthDate: <%=thisDate %>, //当前月
                nextMonthDate: <%=NextDate %>
            });


            $(".Copy").click(function() {
                var prices = $(this).parent().parent().find("[name='txtprice']").val();
                var nums = $(this).parent().parent().find("[name='txtnum']").val();
                $("form").find("input[type='text'][name='txtprice']").each(function() {
                    $(this).val(prices);
                });
                $("form").find("input[type='text'][name='txtnum']").each(function() {
                    $(this).val(nums);
                });
            });

            $(".baocun-btn").click(function() {
                var timeStart = $("input[type='text'][name='txtSDate']").val();
                var timeEnd = $("input[type='text'][name='txtEDate']").val();
                if (timeStart > timeEnd) {
                    alert("结束日期不能小于开始日期!");
                    return false;
                }
                var vResult = ValiDatorForm.validator($("form").get(0), "alert");
                if (vResult) {
                    var tfId = '<%=EyouSoft.Common.Utils.GetQueryStringValue("trfficID") %>';
                    $.newAjax({
                        url: '/jipiao/Traffic/trafficpricesList.aspx?type=save&tfId=' + tfId,
                        type: 'POST',
                        cache: false,
                        data: $("form").serialize(),
                        success: function(ret) {
                            var obj = eval('(' + ret + ')');
                            alert(obj.msg);
                            window.location.reload();
                        },
                        error: function() {
                            alert("服务器繁忙，请稍后在试!");
                        }
                    });
                }
                return false;
            });
        });
        
    </script>

</asp:Content>
