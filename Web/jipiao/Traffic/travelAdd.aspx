<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="travelAdd.aspx.cs" Inherits="Web.jipiao.travelAdd" %>

<%@ Register Src="~/UserControl/Provinces.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Citys.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/../js/GetCityList.js"></script>

    <script type="text/javascript" src="../../js/jquery.js"></script>

    <script type="text/javascript" src="../../js/ValiDatorForm.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="450" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0 auto;">
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span> 序列：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                第<input type="text" class="xtinput" size="6" id="txtSerialNum" runat="server" valid="required"
                    errmsg="请输入序列段!" />段 <span id="errMsg_txtSerialNum" class="errmsg"></span>
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>交通类型：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <asp:DropDownList ID="seleTfrricType" runat="server">
                    <asp:ListItem Value="0">飞机</asp:ListItem>
                    <asp:ListItem Value="1">火车</asp:ListItem>
                    <asp:ListItem Value="2">其他</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>出发地：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <uc1:ucProvince ID="ucLProvince" runat="server" />
                <uc2:ucCity ID="ucLCity" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>抵达地：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <uc1:ucProvince ID="ucRprovince" runat="server" />
                <uc2:ucCity ID="ucRcity" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>航班号：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" size="15" id="txtFlightNum" runat="server" valid="required"
                    errmsg="请输入航班号!" />
                <span id="errMsg_txtFlightNum" class="errmsg"></span>例：CZ6758
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>航空公司：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <asp:DropDownList ID="seleLineCompanyNamev" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>出发时间：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" size="15" id="txtLTime" runat="server" valid="required"
                    errmsg="请输入出发时间!" />
                <span id="errMsg_txtLTime" class="errmsg"></span>例：11:00
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>抵达时间：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" size="15" id="txtRTime" runat="server" valid="required"
                    errmsg="请输入抵达时间!" />
                <span id="errMsg_txtRTime" class="errmsg"></span>例：15:00
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                直飞经停：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="radio" name="IsStop" value="1"  <%=IsStop=="1"?"checked='checked'":""%> />
                经停
                <input type="radio" name="IsStop" value="0" <%=IsStop=="1"?"":"checked='checked'"%>/>
                直飞
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>舱位：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <asp:DropDownList ID="ddlSpace" runat="server">
                    <asp:ListItem Value="0">公务舱</asp:ListItem>
                    <asp:ListItem Value="1">头等舱</asp:ListItem>
                    <asp:ListItem Value="2">经济舱</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                机型：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" size="15" id="txtAirPlaneType" runat="server" />
                例：737
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>间隔天数：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" size="6" id="txtIntervalDays" runat="server" valid="required"
                    errmsg="请输入行程间隔天数!" />
                <span id="errMsg_txtIntervalDays" class="errmsg"></span>天 请选择与上段行程间隔天数
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="100" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" id="save">保存</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">

        $(function() {

            //获取表单验证
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));

            //保存
            $("#save").click(function() {
                var form = $("#<%=form1.ClientID %>").get(0);
                var type = '<%=EyouSoft.Common.Utils.GetQueryStringValue("type") %>';
                var tfID = '<%=EyouSoft.Common.Utils.GetQueryStringValue("tfID") %>';
                var vResult = ValiDatorForm.validator(form, "span");
                if (!vResult) return false;
                var vret = true;

                //省份城市验证
                if ($("#uc_provinceDynamic1").val() == "0") {
                    alert("请选择出发省份!");
                    $("#uc_provinceDynamic1").focus();
                    vret = false;
                    return false;
                }
                if ($("#uc_cityDynamic1").val() == "0") {
                    alert("请选择出发城市!");
                    $("#uc_cityDynamic1").focus();
                    vret = false;
                    return false;
                }
                if ($("#uc_provinceDynamic2").val() == "0") {
                    alert("请选择抵达省份!");
                    $("#uc_provinceDynamic2").focus();
                    vret = false;
                    return false;
                }
                if ($("#uc_cityDynamic2").val() == "0") {
                    alert("请选择抵达城市!");
                    $("#uc_cityDynamic2").focus();
                    vret = false;
                    return false;
                }
                if (vResult && vret) {
                    var trID = '<%=EyouSoft.Common.Utils.GetQueryStringValue("travelId") %>';
                    $.ajax({
                        url: '/jipiao/Traffic/travelAdd.aspx?action=save&type=' + type + "&tfID=" + tfID + "&trID=" + trID,
                        type: 'POST',
                        cache: false,
                        data: $("#<%=form1.ClientID %>").serialize(),
                        success: function(ret) {
                            var obj = eval('(' + ret + ')');
                            alert(obj.msg);
                            parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                            parent.window.location.reload();
                        },
                        error: function() {
                            alert("服务器繁忙，请稍后在试!");
                        }
                    });
                }
                return false;
            });
        })
    </script>

</body>
</html>
