<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Web.GroupEnd.SignUp" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.DatePicker" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/back.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <input type="hidden" id="hd_PriceStandId" name="hd_PriceStandId" runat="server" />
        <input type="hidden" id="hd_cr_price" name="hd_cr_price" runat="server" />
        <input type="hidden" id="hd_rt_price" name="hd_rt_price" runat="server" />
        <table width="880" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="89" height="25" align="center">
                    线路名称：
                </th>
                <td align="left">
                    【团号:<asp:Literal ID="lt_teamCode" runat="server"></asp:Literal>】
                    <asp:Literal ID="lt_xianluName" runat="server"></asp:Literal>
                </td>
                <th width="80" align="center">
                    线路区域：
                </th>
                <td width="360" align="left">
                    <asp:Literal ID="Area" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    销售员
                </th>
                <td align="left">
                    <asp:Literal ID="litseller" runat="server"></asp:Literal>
                </td>
                <th align="center">
                    计调员
                </th>
                <td align="left">
                    <input type="hidden" id="hidCoordinatorId" runat="server" />
                    <asp:Literal ID="litCoordinatorId" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="odd">
                <th height="25" align="center">
                    出团日期：
                </th>
                <td align="left">
                    <asp:Literal ID="lt_startDate" runat="server"></asp:Literal>
                </td>
                <th align="center">
                    剩余空位：
                </th>
                <td align="left">
                    <font class="fbred">
                        <asp:Literal ID="lt_shengyu" runat="server"></asp:Literal></font>
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    返佣金额：
                </th>
                <td>
                    <asp:Label ID="lblBackMoney" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hideBackMoney" runat="server" />
                    <asp:HiddenField ID="hideBackType" runat="server" />
                </td>
                <th align="center">
                    &nbsp;
                </th>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    联系人：
                </th>
                <td>
                    <asp:TextBox ID="txtContactName" runat="server" class="searchinput"></asp:TextBox>
                </td>
                <th align="center">
                    电话：
                </th>
                <td>
                    <asp:TextBox ID="txtContactPhone" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    手机：
                </th>
                <td>
                    <asp:TextBox ID="txtContactMobile" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                </td>
                <th align="center">
                    传真：
                </th>
                <td>
                    <asp:TextBox ID="txtContactFax" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr class="odd">
                <th height="55" align="center">
                    <span style="color: Red">*</span> 结算价：
                </th>
                <td align="left" valign="middle" colspan="3">
                    <table cellspacing="0" cellpadding="0" border="0" align="left">
                        <tbody>
                            <%=price%>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="25" align="center">
                    人数：
                </th>
                <td align="left" colspan="3">
                    <span style="color: Red">*</span> 成人数：
                    <asp:TextBox ID="txtDdultCount" runat="server" class="searchinput searchinput03"
                        valid="required|isNumber" errmsg="请输入成人数|请输入数字"></asp:TextBox>
                    <span id="errMsg_txtDdultCount" class="errmsg"></span>儿童数：
                    <asp:TextBox ID="txtChildCount" runat="server" class="searchinput searchinput03"></asp:TextBox>
                </td>
            </tr>
            <tr class="odd">
                <th height="25" align="center">
                    总金额：
                </th>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtTotalMoney" runat="server" class="searchinput"></asp:TextBox>
                </td>
            </tr>
            <tr class="even">
                <th rowspan="2" align="center">
                    游客信息：
                </th>
                <td height="28" colspan="3" align="center">
                    <table width="50%" border="0" align="right" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="85%" align="right">
                                上传附件：<input type="file" name="fuiLoadAttachment" id="fuiLoadAttachment" />
                            </td>
                            <td align="center">
                                <uc1:LoadVisitors ID="LoadVisitors1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <td colspan="3" align="center">
                    <table width="90%" border="0" align="center" id="tblVisitorList" cellpadding="0"
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
                <td colspan="3" align="left">
                    <textarea class="textareastyle02" rows="5" cols="45" runat="server" id="txt_Special"
                        name="txt_Special"></textarea>
                </td>
            </tr>
            <tr>
                <th colspan="4" align="center">
                    <table width="320" border="0" cellspacing="0" cellpadding="0" align="center">
                        <tr>
                            <td height="40" align="center">
                            </td>
                            <td align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton1" CommandName="submit" runat="server" OnClientClick="return checkForm()"
                                    OnClick="LinkButton1_Click">确认提交</asp:LinkButton>
                            </td>
                            <td height="40" align="center" class="tjbtn02">
                                <a href="javascript:" id="lbtnSeats" runat="server">申请留位</a>
                            </td>
                            <td height="40" align="center" class="tjbtn02">
                                <a href="javascript:void(0);" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()">
                                    关闭</a>
                            </td>
                        </tr>
                    </table>
                </th>
            </tr>
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
                    <asp:LinkButton ID="btnYes" runat="server" OnClick="btnYes_Click" OnClientClick="return checkForm()">确定</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onclick="OrderEdit.close()">取消</a>
                </td>
            </tr>
        </table>
        <input type="hidden" id="specive" value="" name="specive" />
    </div>

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
        var OrderEdit = {
            open: function(obj) {
                var pos = OrderEdit.getPosition(obj);
                $("#tbl_last").show().css({ left: Number(pos.Left) + "px", top: Number(pos.Top - 70) + "px" });
                return false;
            },

            close: function() {
                $("#tbl_last").hide();
            },

            getPosition: function(obj) {
                var objPosition = { Top: 0, Left: 0 }
                var offset = $(obj).offset();
                objPosition.Left = offset.left;
                objPosition.Top = offset.top + $(obj).height();
                return objPosition;
            }
        }

        var specMoney = 0;
        function querystring(uri, val) {
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        $(document).ready(function() {


            loadVisitors.init({ autoComputeToTalAmountHandle: function() {
                $("input[name='tefu']").each(function() {
                    specMoney += getAmountInTeFuStr(this.value);
                });
                var adult = Number($("#<%=txtDdultCount.ClientID%>").val());
                var child = Number($("#<%=txtChildCount.ClientID%>").val());
                var adultP = Number($("input[name='radio']:checked").parent().parent().find("[name='sp_cr_price']").html());
                var childP = Number($("input[name='radio']:checked").parent().parent().find("[name='sp_et_price']").html());

                if (specMoney.toString() == "NaN") {
                    specMoney = 0;
                }
                var all = parseInt((adult * adultP + child * childP + specMoney) * 100) / 100;
                if (all >= 0 || all <= 0) {
                    $("#<%=txtTotalMoney.ClientID%>").val(adult * adultP + child * childP + specMoney);
                } else {
                    $("#<%=txtTotalMoney.ClientID%>").val("0");
                }
            }
            });

            $("input[name='radio']").click(function() {
                if ($(this).attr("checked")) {
                    $("#hd_PriceStandId").val($(this).val());
                    $("#hd_cr_price").val($("input[name='radio']:checked").parent().parent().find("[name='sp_cr_price']").html());
                    $("#hd_rt_price").val($("input[name='radio']:checked").parent().parent().find("[name='sp_et_price']").html());
                    GetSumMoney();
                }
            });

            $("#<%=txtDdultCount.ClientID%>").blur(function() {
                GetSumMoney();
            });
            $("#<%=txtChildCount.ClientID %>").blur(function() {
                GetSumMoney();
            });

            //判断用户输入的金额是否正确
            function CheckIsFloat($obj) {
                var re = /^[0-9]*.?([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            }
            //检测总金额输入是否合法
            $("#<%=txtTotalMoney.ClientID%>").change(function() {
                if (CheckIsFloat($(this))) {
                    alert("总金额输入非法");
                    $(this).focus().select();
                }


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


            });
            $("#lbtnSeats").click(function() {
                OrderEdit.open(this);
                return false;
            });

            function GetSumMoney() {
                //根据选择计算标准，计算总金额
                var backType = $("<%=hideBackType.ClientID %>").val();
                var adult = Number($("#<%=txtDdultCount.ClientID%>").val());
                var child = Number($("#<%=txtChildCount.ClientID%>").val());
                var adultP = Number($("input[name='radio']:checked").parent().parent().find("[name='sp_cr_price']").html());
                var childP = Number($("input[name='radio']:checked").parent().parent().find("[name='sp_et_price']").html());
                //后返
                if (backType == "2") {
                    $("#<%=txtTotalMoney.ClientID%>").val(adult * adultP + child * childP + specMoney);
                } else {
                    //现反
                    var backMoney = Number($("#<%=lblBackMoney.ClientID %>").html());
                    $("#<%=txtTotalMoney.ClientID%>").val(adult * (adultP - backMoney) + child * (childP - backMoney) + specMoney);
                }
            }
            function VisitorDel(obj) {
                if (confirm("您确定删除该数据?")) {
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
        //判断方法
        function checkForm() {
            var form = $("#<%=LinkButton1.ClientID %>").closest("form").get(0);
            if (ValiDatorForm.validator(form, "span") && checkType() && checkNum()) {
                return true;
            } else {
                return false;
            }
        }
        //判断结算方式
        function checkType() {
            var count = 0;
            $(".radio_select").each(function() {
                if ($(this).attr("checked") == true) {
                    count++;
                }
            })

            if (count != 0) {
                return true;
            } else {
                alert("请选择一种结算方式");
                return false;
            }
        }
        //判断人数是否大于0
        function checkNum() {
            var num = $("#<%=txtDdultCount.ClientID %>").val();
            if (parseInt(num) > 0) {
                return true;
            } else {
                alert("人数必须大于0");
                return false;
            }
        }

    </script>

    </form>
</body>
</html>
