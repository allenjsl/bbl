<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order_edit.aspx.cs" Inherits="Web.sales.Order_edit" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.DatePicker" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SOperator" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>订单编辑页面</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <input type="hidden" id="hd_TeamOrSanPing" name="hd_TeamOrSanPing" runat="server" />
        <input id="hd_lineID" type="hidden" runat="server" />
        <input type="hidden" id="hd_PriceStandId" name="hd_PriceStandId" runat="server" />
        <input type="hidden" id="hd_BuyCompanyId" name="hd_BuyCompanyId" runat="server" />
        <input type="hidden" id="hd_cr_price" name="hd_cr_price" runat="server" />
        <input type="hidden" id="hd_rt_price" name="hd_rt_price" runat="server" />
        <input type="hidden" id="hd_LevelID" name="hd_LevelID" runat="server" />
        <input type="hidden" id="TourID" name="TourID" runat="server" />
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="880" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <th>
                        组团社
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtGroupsName" runat="server" ReadOnly="true" BackColor="#dadada"></asp:TextBox>
                        <a href="javascript:void(0);" id="a_GetGroups" class="selectTeam">
                            <img src="../images/sanping_04.gif" width="28px" height="18px" border="0"></a>
                    </td>
                    <td style="text-align: center">
                        <b>对方团号：</b>
                    </td>
                    <td>
                        <input id="IsRequiredTourCode" type="hidden" value="<%=IsRequiredTourCode %>" />
                        <input type="text" class="searchinput searchinput02" id="txtBuyerTourCode" runat="server"
                            maxlength="50" errmsg="*请填写对方团号" />
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center" width="100px">
                        线路名称：
                    </th>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblLineName" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        出团时间：
                    </th>
                    <td align="center">
                        <asp:Label ID="lblChuTuanDate" runat="server" Text=""></asp:Label>
                        <asp:HiddenField runat="server" ID="hid_ChuTuanDate" />
                    </td>
                </tr>
                <tr class="odd">
                    <th height="25" align="center">
                        当前空位：
                    </th>
                    <td align="left" colspan="4">
                        <font class="fbred">
                            <asp:Label ID="lblCurFreePosi" runat="server" Text=""></asp:Label></font>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        关联交通：
                    </th>
                    <td colspan="4" align="left">
                        <select id="selectTraffic" name="selectTraffic">
                            <%=strTraffic %>
                        </select>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center">
                        出发交通：
                    </th>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblChuFanTra" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        返程交通：
                    </th>
                    <td align="center">
                        <asp:Label ID="lblBackTra" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center">
                        联系人：
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtContactName" runat="server" class="searchinput"></asp:TextBox>
                    </td>
                    <th align="center">
                        电话：
                    </th>
                    <td>
                        <asp:TextBox ID="txtContactPhone" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center">
                        手机：
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtContactMobile" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                    <th align="center">
                        传真：
                    </th>
                    <td>
                        <asp:TextBox ID="txtContactFax" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center">
                        销售员：
                    </th>
                    <td colspan="2">
                        <input type="hidden" id="hidSalerID" name="hidSalerID" runat="server" />
                        <input type="hidden" id="hidSalerName" name="hidSalerName" runat="server" />
                        <asp:Label ID="lblXSY" runat="server" Text=""></asp:Label>
                    </td>
                    <%if (isDXFW)
                      { %>
                    <th lign="center">
                        计调员：
                    </th>
                    <td>
                        <asp:Label ID="lblJDY" runat="server" Text=""></asp:Label>
                    </td>
                    <%} %>
                </tr>
                <tr class="even">
                    <th height="55" align="center">
                        结算价：
                    </th>
                    <td align="left" valign="middle" colspan="4">
                        <table cellspacing="0" cellpadding="0" border="0" align="left">
                            <tbody>
                                <%=price%>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="25" align="center">
                        人数：
                    </th>
                    <td align="left" colspan="2">
                        <div id="SanPingPersonNum" runat="server" style="float: left;">
                            成人数：
                            <asp:TextBox ID="txtDdultCount" runat="server" class="searchinput searchinput03"
                                MaxLength="6" valid="isNumber" errmsg="请输入有效成人数"></asp:TextBox>
                            儿童数：
                            <asp:TextBox ID="txtChildCount" runat="server" class="searchinput searchinput03"
                                MaxLength="6" valid="isNumber" errmsg="请输入有效儿童数"></asp:TextBox>
                        </div>
                        <div id="TeamPersonNumMaax" runat="server" style="float: left;" visible="false">
                            <div id="TeamPersonNumCount" runat="server" style="float: left;">
                                成人数：
                                <asp:TextBox ID="txtNumberCr" runat="server" class="searchinput searchinput03" MaxLength="6"
                                    valid="isNumber" errmsg="请输入有效成人数" onchange="groupSum()"></asp:TextBox>
                                儿童数：
                                <asp:TextBox ID="txtNumberEt" runat="server" class="searchinput searchinput03" MaxLength="6"
                                    valid="isNumber" errmsg="请输入有效儿童数" onchange="groupSum()"></asp:TextBox>
                                全陪数：
                                <asp:TextBox ID="txtNumberQp" runat="server" class="searchinput searchinput03" MaxLength="6"
                                    valid="isNumber" errmsg="请输入有效全陪数" onchange="groupSum()"></asp:TextBox>
                            </div>
                            <div id="TeamPersonNumMin" runat="server" style="float: left;">
                                总人数：
                                <asp:TextBox ID="lblTeamPersonNum" runat="server" class="searchinput searchinput03"
                                    MaxLength="6" valid="isNumber" errmsg="请输入有效人数"></asp:TextBox></div>
                        </div>
                        返佣：<input type="hidden" id="hd_rebateType" name="hd_rebateType" value="<%=CommissionType %>" />
                        <input name="txt_Rebate" errmsg="*请输入返佣金额|*请输入正确的金额" valid="required|isMoney" type="text"
                            id="txt_Rebate" class="searchinput searchinput03" value="<%=CommissionPrice.ToString("0.00") %>" />
                        <input id="hd_manNum" type="hidden" value="" runat="server" />
                        <input id="hd_childNum" type="hidden" value="" runat="server" />
                        <input id="hd_tuituan" type="hidden" value="" runat="server" />
                        <input id="hd_xianfan" type="hidden" value="" runat="server" />
                        <input id="hd_tourtype" type="hidden" value="" runat="server" />
                    </td>
                    <th>
                        对方操作员：
                    </th>
                    <td>
                        <input type="hidden" id="hd_BuyerContactId" name="hd_BuyerContactId" value="<%=BuyerContactId %>" /><input
                            type="hidden" id="hd_orderOprator" name="hd_orderOprator" value="<%=BuyerContactName %>" />
                        <select name="otherOprator" id="otherOprator" errmsg="请选择对方操作员" valid="required">
                            <option value="">请选择</option>
                        </select>
                    </td>
                </tr>
                <tr class="odd">
                    <th>
                        增加费用：
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="addmoney" errmsg="请输入合法的数字" valid="isNumber" runat="server" CssClass="searchinput"></asp:TextBox>
                    </td>
                    <th>
                        减少费用：
                    </th>
                    <td>
                        <asp:TextBox ID="delmoney" errmsg="请输入合法的数字" valid="isNumber" runat="server" CssClass="searchinput"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th>
                        备注：
                    </th>
                    <td colspan="4">
                        <textarea id="remark" style="width: 400px; height: 150px;" runat="server"></textarea>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        总金额：
                    </th>
                    <td align="left" colspan="4">
                        <asp:TextBox ID="txtTotalMoney" runat="server" class="searchinput" MaxLength="10"
                            valid="required|isMoney" errmsg="请输入金额|请输入有效金额"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center" rowspan="2">
                        游客信息：
                    </th>
                    <td height="28" align="center" colspan="4">
                        <table cellspacing="0" cellpadding="0" border="0" align="right" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">游客附件</asp:HyperLink>
                                    </td>
                                    <td align="right">
                                        上传附件：
                                        <input type="file" name="fileField" id="fileField" />
                                    </td>
                                    <td align="center">
                                        <a href="/Common/LoadVisitors.aspx?topID=<%=Request.QueryString["iframeId"]%>" id="import">
                                            <img src="/images/sanping_03.gif">
                                            导入</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <td align="center" colspan="4">
                        <div style="width: 100px; color: Red;" id="div_msg">
                        </div>
                        <table id="cusListTable" cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4"
                            align="center" width="95%" style="margin: 10px 0pt;">
                            <tbody>
                                <tr>
                                    <td style="width: 5%" bgcolor="#e3f1fc" align="center">
                                        序号
                                    </td>
                                    <td height="25" bgcolor="#e3f1fc" align="center">
                                        姓名
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        类型
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        证件名称
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        证件号码
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        性别
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        联系电话
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        特服
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        操作 <a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>
                                    </td>
                                </tr>
                                <%=cusHtml%>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center">
                        特殊要求说明：
                    </th>
                    <td align="left" colspan="4">
                        <asp:TextBox ID="txtSpecialRe" runat="server" TextMode="MultiLine" class="textareastyle02"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th height="42" align="center">
                        操作留言：
                    </th>
                    <td align="left" colspan="4">
                        <asp:TextBox ID="txtOperMes" runat="server" TextMode="MultiLine" class="textareastyle02"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="5">
                        <table cellspacing="0" cellpadding="0" border="0" width="320" id="tableroot" runat="server">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                                        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click">确认成交</asp:LinkButton>
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <a href="javascript:" id="lbtnSeats" runat="server">同意留位</a>
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" OnClick="LinkButton1_Click"
                                            OnClientClick="return confirm('您确定要取消该订单吗？')">取消订单</asp:LinkButton>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </th>
                </tr>
            </tbody>
        </table>
        <table style="height: 45px; position: absolute; background-color: White; display: none;
            border: 1px solid gray;" id="tbl_last">
            <tr>
                <td>
                    <cc2:DatePicker ID="txtEndTime" name="SaveSeatDate" DisplayTime="true" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnYes" runat="server" OnClick="btnYes_Click">确定</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onclick="OrderEdit.close()">取消</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        function isnull(v, defaultValue) {
            if (v == null || !v)
                return defaultValue;
            else
                return v;
        }
        function updatePrice(trafficePrice) {
            $("span[name='sp_cr_price']").each(function() {
                var self = $(this);
                self.text(FloatAdd(parseFloat(isnull(self.next("input[type='hidden'][name='hid_cr_price']").val(), 0)), parseFloat(isnull(trafficePrice, 0))));
            });
            $("span[name='sp_et_price']").each(function() {
                var self = $(this);
                self.text(FloatAdd(parseFloat(isnull(self.next("input[type='hidden'][name='hid_et_price']").val(), 0)), parseFloat(isnull(trafficePrice, 0))));
            });
            OrderEdit.GetSumMoney();
        }
        $(function() {
            $(".divPrice[val='<%=PriceStandId%>']").parent().parent().find("input[type='radio']").each(function() {
                if ($(this).val() == "<%=CustomerLevId%>") {
                    $(this).attr("checked", "checked");
                }
            });
            //加载价格
            $.newAjax({
                type: "GET",
                dataType: "json",
                url: "../sanping/SanPing_jion.aspx",
                async: false,
                data: {
                    act: "getPrice",
                    startDate: $("#<%=hid_ChuTuanDate.ClientID %>").val(),
                    trafficId: $("#selectTraffic").find("option:selected").val(),
                    iframeId: '<%=Request.QueryString["iframeId"] %>'
                },
                success: function(result) {
                    updatePrice(result.result);
                    $("#selectTraffic").find("option:selected").attr("data-shengyu", result.shengyu);
                    $("#selectTraffic").find("option:selected").attr("data-price", result.result);
                    $("#lblCurFreePosi").text(result.shengyu);
                }
            })

            //关联交通改变事件
            $("#selectTraffic").change(function() {
                var self = $(this), price = $.trim(self.find("option:selected").attr("data-price")),
                    trafficId = self.find("option:selected").val(),
                    startDate = $("#<%=hid_ChuTuanDate.ClientID %>").val();
                if (price == "") {
                    if (trafficId) {
                        $.newAjax({
                            type: "GET",
                            dataType: "json",
                            url: "../sanping/SanPing_jion.aspx",
                            async: false, //同步执行ajax 默认true为异步请求
                            data: { act: "getPrice", startDate: startDate, trafficId: trafficId, iframeId: '<%=Request.QueryString["iframeId"] %>' },
                            success: function(result) {
                                updatePrice(result.result);
                                self.find("option:selected").attr("data-shengyu", result.shengyu);
                                self.find("option:selected").attr("data-price", result.result);
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
                $("#lblCurFreePosi").text(self.find("option:selected").attr("data-shengyu"));
            })



            $("#otherOprator").change(function() {
                var selectOprator = ($("#otherOprator").get(0).options[$(this).get(0).selectedIndex].text);
                if (selectOprator != "请选择") {
                    $("#hd_orderOprator").val(selectOprator);
                } else {
                    $("#hd_orderOprator").val("");
                }
            });
            $("#txt_Rebate").keyup($("#hd_tourtype").val() == "0" ? OrderEdit.GetSumMoney : "");
            $.newAjax({
                type: "GET",
                dataType: "json",
                url: "../sanping/SanPing_jion.aspx",
                data: { act: "getSeller", comId: $("#hd_BuyCompanyId").val(), iframeId: '<%=Request.QueryString["iframeId"] %>' },
                cache: false,
                success: function(d) {
                    document.getElementById("otherOprator").options.length = 1;
                    for (var i = 0; i < d[0].cusList.length; i++) {
                        document.getElementById("otherOprator").options.add(new Option(d[0].cusList[i].Name, d[0].cusList[i].ID));
                    }
                    $("#otherOprator").val($("#hd_BuyerContactId").val());
                },
                error: function(d) {
                    alert(d);
                }
            });
            //打开导入窗口
            $("a[id='import']").click(function() {
                parent.Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    width: "853px",
                    height: "514px",
                    async: false,
                    title: "导入",
                    modal: true
                });
                return false;
            });

            $("#<%=txtChildCount.ClientID%>").change(function() {
                if (OrderEdit.CheckIsInt($(this))) {
                    alert("输入的儿童数应为整数");
                    $(this).focus().select();
                }
                else {
                    OrderEdit.GetSumMoney();
                }
            });

            $("#<%=txtDdultCount.ClientID%>").change(function() {
                if (OrderEdit.CheckIsInt($(this))) {
                    alert("输入的成人数应为整数");
                    $(this).focus();
                }
                else {
                    OrderEdit.GetSumMoney();
                }
            });

            //提交表单时去除游客类型,成人数，儿童数，总金额的disabled
            $("#<%=lbtnSubmit.ClientID%>").click(function() {
                var isb = true;
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                var isb = ValiDatorForm.validator(form, "alert");

                if (!isb) {
                    return false;
                }
                var hd_IsRequiredTraveller = $("#hd_IsRequiredTraveller").val();
                //游客验证（hd_IsRequiredTraveller是否验证根据配置false时后面的参数允许为""，txtVisitorName姓名框name，ddlCardType=证件类型name，txtContactTel=电话框name）
                var msg = visitorChecking.isChecking(hd_IsRequiredTraveller, "cusName", "cusCardType", "cusPhone", "cusCardNo");

                if (!msg.isYes) {

                    alert(msg.msg);

                    return false;
                }
                else {

                    OrderEdit.RemoveDisabled();
                }
            });

            //检测总金额输入是否合法
            $("#<%=txtTotalMoney.ClientID%>").change(function() {
                if (OrderEdit.CheckIsFloat($(this))) {
                    alert("总金额输入非法");
                    $(this).focus().select();
                }
            });


            $("[name='radio']").click(function() {
                if ($(this).attr("checked")) {
                    $("#hd_PriceStandId").val($(this).parents().find(".divPrice").attr("val"));
                    $("#hd_cr_price").val($(this).next("[name='sp_cr_price']").html());
                    $("#hd_rt_price").val($(this).nextAll("[name='sp_et_price']").html());
                    $("#hd_LevelID").val($(this).val());
                    OrderEdit.GetSumMoney();
                }
            });

            $("#<%=btnYes.ClientID %>").click(function() {
                var leaveTime = $("#txtEndTime_dateTextBox").val();
                if (leaveTime == "") {
                    alert("留位时间不能为空！");
                    return false;
                }
                if (Date.parse(leaveTime.replace(/-/g, "/") + ":00") < new Date()) {
                    alert("留位时间必须大于当前时间！");
                    return false;
                }
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                var msg = ValiDatorForm.validator(form, "alert");
                if (!msg) {
                    return false;
                }
                OrderEdit.RemoveDisabled();
            });

            $("#lbtnSeats").click(function() {
                OrderEdit.open(this);
                return false;
            });

            $("#<%=addmoney.ClientID %>").blur(function() {
                //                var addMoney = Number( $.trim( $("#<%=addmoney.ClientID %>").val())); 
                //               
                //                if(addMoney>0)
                //                {
                //                     var allPrice = Number($("#<%=txtTotalMoney.ClientID %>").val()) + addMoney;
                //                     if(allPrice>0)
                //                     {
                //                        $("#<%=txtTotalMoney.ClientID %>").val( allPrice);
                //                     }
                //                }

                OrderEdit.GetSumMoney();
            })
            $("#<%=delmoney.ClientID %>").blur(function() {
                //                var delMoney =Number( $.trim($("#<%=delmoney.ClientID %>").val()));
                //                if(delMoney>0)
                //                {
                //                       var allPrice = Number($("#<%=txtTotalMoney.ClientID %>").val()) - delMoney;
                //                     if(allPrice>0)
                //                     {
                //                        $("#<%=txtTotalMoney.ClientID %>").val( allPrice);
                //                     }
                //                }

                OrderEdit.GetSumMoney();
            })

        });

        function CreateCusList(array) {            
            $("#div_msg").html("正在加载名单...");
            var trCount = Number($("#cusListTable tr").length) - 1;
            var url = "/ashx/CreateCurList.ashx?type=order&trCount=" + trCount;
            $.newAjax({
                type: "Post",
                url: url,
                cache: false,
                data: { "postArray": array },
                dataType: "html",
                success: function(data) {
                    $("#div_msg").html("");
                    $("#cusListTable tr:last").after(data);
                    OrderEdit.RemoveDisabled();
                }
            });
        }

        //根据用户输入的身份证号判断性别
        function getSex(obj) {
            var val = $(obj).val();
            var tr = $(obj).parent().parent();
            var sex = tr.children().children("select[class='ddlSex']");
            var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
            if (isIdCard.exec(val)) {
                if (15 == val.length) {// 15位身份证号码
                    if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                        sex.val(1);
                    else
                        sex.val(2);
                }

                if (18 == val.length) {// 18位身份证号码
                    if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                        sex.val(1);
                    else
                        sex.val(2);
                }
            } else {
                sex.val(0);
            }
        }
        //浮点数乘法运算
        function FloatMul(arg1, arg2) {
            var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
            try { m += s1.split(".")[1].length } catch (e) { }
            try { m += s2.split(".")[1].length } catch (e) { }
            return formatFloat(Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m), 2);
        } function FloatAdd(arg1, arg2) {
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

        function formatFloat(src, pos) {
            return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
        }
        var OrderEdit = {

            AddCus: function() {
                OrderEdit.CreateTR();
            },

            DelCus: function($obj) {
                if ($obj.prev("input[name='cusState']").val() == "ADD") {
                    $obj.parent().parent().remove();
                }
                else {
                    $obj.parent().parent().css("display", "none");
                    $obj.prev("input[name='cusState']").val("DEL");
                }
                var i = 1;
                $("td[index]").each(function() {
                    if ($(this).parent().css("display") != "none") {
                        $(this).html(i);
                        i++;
                    }
                })
            },

            //数据提交时，却除某些需要提交控件的disabled属性.
            RemoveDisabled: function() {
                $("select[name='cusType']").each(function() {
                    $(this).removeAttr("disabled");
                });
                $("#<%=lblTeamPersonNum.ClientID%>").removeAttr("disabled");
                $("#<%=txtDdultCount.ClientID%>").removeAttr("disabled");
                $("#<%=txtChildCount.ClientID%>").removeAttr("disabled");
                $("#<%=txtTotalMoney.ClientID%>").removeAttr("disabled");
            },


            //检测输入是否是大于等于0整数
            CheckIsInt: function($obj) {
                var re = /^[0-9]([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            },

            //检测总金额输入是否正确
            CheckIsFloat: function($obj) {
                var re = /^[0-9]*.?([0-9])*$/;
                if (re.test($obj.val())) {

                    return false;
                }
                else {
                    return true;
                }
            },

            //根据选择计算标准，计算总金额
            GetSumMoney: function() {
                if ($("#hd_tourtype").val() == "0") {
                    var specMoney = 0; var sumprice = 0;
                    var adult = Number($("#<%=txtDdultCount.ClientID%>").val());
                    var hdcr = Number($("#hd_manNum").val());
                    var child = Number($("#<%=txtChildCount.ClientID%>").val());
                    var hdet = Number($("#hd_childNum").val());
                    var adultP = Number($("input[name='radio']:checked").next("span[name='sp_cr_price']").text());
                    var childP = Number($("input[name='radio']:checked").nextAll("span[name='sp_et_price']").text());
                    var addMoney = Number($.trim($("#<%=addmoney.ClientID %>").val()));
                    var delMoney = Number($.trim($("#<%=delmoney.ClientID %>").val()));
                    var tuituansunshi = Number($("#hd_tuituan").val());
                    $("input[name='specive']").each(function() {
                        specMoney += getAmountInTeFuStr($(this).val());
                    });
                    var trafficPrice = Number($.trim($("#selectTraffic").find("option:selected").attr("data-price")));

                    sumprice = FloatMul((adult - hdcr), adultP) + FloatMul((child - hdet), childP) + specMoney; //+ FloatMul((adult + child - hdcr - hdet), trafficPrice);

                    if (parseInt(isnull($("#hd_rebateType").val(), 0)) == 1) {

                        sumprice = FloatSub(sumprice, FloatMul(isnull(adult + child), isnull(parseFloat($("#txt_Rebate").val()), 0)));
                    }
                    sumprice = sumprice + addMoney - delMoney + tuituansunshi;
                    $("#<%=txtTotalMoney.ClientID%>").val(sumprice);
                }
                else {
                    groupSum();
                }
            },

            open: function(obj) {
                var pos = OrderEdit.getPosition(obj);
                $("#tbl_last").show().css({ left: Number(pos.Left) + "px", top: Number(pos.Top - 70) + "px" });
                return false;
            },

            close: function() {
                $("#tbl_last").hide()
            },

            getPosition: function(obj) {
                var objPosition = { Top: 0, Left: 0 }
                var offset = $(obj).offset();
                objPosition.Left = offset.left;
                objPosition.Top = offset.top + $(obj).height();
                return objPosition;
            },

            //打开特服窗口
            OpenSpecive: function(tefuID) {
                parent.Boxy.iframeDialog({
                    iframeUrl: '/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid=' + tefuID,
                    title: "特服",
                    width: "420px",
                    height: "200px",
                    modal: true,
                    afterHide: function() {
                        OrderEdit.GetSumMoney();
                    }
                });
                return false;
            },

            CreateTR: function() {
                var i = 1;
                $("td[index]").each(function() {
                    if ($(this).parent().css("display") != "none") {
                        i++;
                    }
                })
                var d = new Date();
                var tefuID = d.getTime();
                var TR = '<tr>'
            + '<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index="0" align=\"center\">' + i + '</td><td height="25" bgcolor="#e3f1fc" align="center">'
            + '    <input type="text" class="searchinput" id="Text1" MaxLength="50"  name="cusName" />'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select1" title="请选择"  name="cusType">'
                    + '<option value=""  selected="selected">请选择</option>'
                    + '<option value="1">成人</option>'
                    + '<option value="2">儿童</option>'
                + '</select>'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select2" name="cusCardType">'
                    + '<option value="0" selected="selected">请选择证件</option>'
                    + '<option value="1">身份证</option>'
                    + '<option value="2">护照</option>'
                    + '<option value="3">军官证</option>'
                    + '<option value="4">台胞证</option>'
                    + '<option value="5">港澳通行证</option>'
                    + '<option value="6">户口本</option>'
                + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                + ' <input type="text" class="searchinput searchinput02" id="text2" onblur="getSex(this)" MaxLength="150" name="cusCardNo">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" name="ddlSex" align="center">'
                 + '<select id="Select3" class="ddlSex" name="cusSex">'
                     + '<option value="0" selected="selected">请选择</option>'
                     + '<option value="1">男</option>'
                     + '<option value="2">女</option>'
                 + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                 + '<input type="text" class="searchinput" id="Text3" MaxLength="50" name="cusPhone">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="6%">'
              + '<input type="hidden" name="txtItem" value="" />'
              + '<input type="hidden" name="txtServiceContent" value="" />'
              + '<input type="hidden" name="ddlOperate" value="" />'
              + '<input type="hidden" name="txtCost" value="" />'
               + '<input id="' + tefuID + '" type="hidden" name="specive" value=""/>'
                 + '<a sign="speService" href="javascript:void(0)" onclick="OrderEdit.OpenSpecive(' + tefuID + ')">特服</a>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="12%">'
              + '<input type="hidden" name="cusID" value="" />'
             + '<a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>&nbsp;'
             + '<input type="hidden" name="cusState" value="ADD" />'
             + '<a sign="del" href="javascript:void(0)" onclick="OrderEdit.DelCus($(this))">删除</a>'
             + '</td>'
         + '</tr>';
                $("#cusListTable tr:last").after(TR);
            }

        }

        OrderEdit.RemoveDisabled();

        //根据组团社获得相应的责任计调
        function GetSeller(id, name) {
            //arguments[4]为团号是否必填
            if (arguments[4] == "True") {
                $("#txtBuyerTourCode").attr("valid", "required");
            }
            else {
                $("#txtBuyerTourCode").attr("valid", "");
            }
            $.newAjax({
                type: "GET",
                dataType: "json",
                url: "Order_edit.aspx",
                data: { act: "getSeller", comId: id },
                cache: false,
                success: function(d) {
                    $("#<%=hidSalerID.ClientID%>").val(d[0].salerid);
                    $("#<%=hidSalerName.ClientID%>").val(d[0].saler);
                    $("#<%=lblXSY.ClientID%>").html(d[0].saler);
                    $("#txt_Rebate").val(d[0].CommissionCount);
                    $("#hd_rebateType").val(d[0].CommissionType);
                    $("#hd_BuyCompanyId").val(id);
                    $("#<%=txtGroupsName.ClientID%>").val(name);
                    document.getElementById("otherOprator").options.length = 1;
                    for (var i = 0; i < d[0].cusList.length; i++) {
                        document.getElementById("otherOprator").options.add(new Option(d[0].cusList[i].Name, d[0].cusList[i].ID));
                    }
                    OrderEdit.GetSumMoney();
                },
                error: function(d) {
                    alert(d);
                }
            });
        }

        //选择组团社
        $("#a_GetGroups").click(function() {
            var iframeId = '<%=Request.QueryString["iframeId"] %>';
            var url = "/CRM/customerservice/SelCustomer.aspx?method=SetGroupsVal";
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "选用组团社",
                modal: true,
                width: "820px",
                height: "520px",
                data: {
                    desid: iframeId,
                    backfun: "GetSeller"
                }
            });
            return false;
        });
        $("#IsRequiredTourCode").val() == "True" ? $("#txtBuyerTourCode").attr("valid", "required") : "";
        //组团合计
        function groupSum() {
            var specMoney = 0; var sumprice = 0;
            var crsingle = Number($("input[name=UnitAmountCr]").val());
            var etsingle = Number($("input[name=UnitAmountEt]").val());
            var qpsingle = Number($("input[name=UnitAmountQp]").val());
            var crnum = Number($("#<%=txtNumberCr.ClientID%>").val());
            var etnum = Number($("#<%=txtNumberEt.ClientID%>").val());
            var qpnum = Number($("#<%=txtNumberQp.ClientID%>").val());
            var addMoney = Number($("#<%=addmoney.ClientID%>").val());
            var delMoney = Number($("#<%=delmoney.ClientID%>").val());
            var fymoney = Number($("#txt_Rebate").val());
            var hdcr = Number($("#hd_manNum").val());
            var hdet = Number($("#hd_childNum").val());
            var hdtt = Number($("#hd_tuituan").val())

            $("input[name='specive']").each(function() {
                specMoney += getAmountInTeFuStr($(this).val());
            });
            var trafficPrice = Number($.trim($("#selectTraffic").find("option:selected").attr("data-price")));
            //alert(trafficPrice);
            if ($("#hd_xianfan").val() && $("#hd_xianfan").val() == "1") {
                sumprice = (crsingle * (crnum - hdcr)) + (etsingle * (etnum - hdet)) + (qpsingle * qpnum) - (fymoney * (crnum + etnum)) + addMoney - delMoney + hdtt + specMoney + ((crnum - hdcr + etnum - hdet) * trafficPrice);
            }
            else {
                sumprice = (crsingle * (crnum - hdcr)) + (etsingle * (etnum - hdet)) + (qpsingle * qpnum) + addMoney - delMoney + hdtt + specMoney + ((crnum - hdcr + etnum - hdet) * trafficPrice);
            }
            $("#<%=txtTotalMoney.ClientID%>").val(sumprice);

        }
    </script>

    </form>
</body>
</html>
