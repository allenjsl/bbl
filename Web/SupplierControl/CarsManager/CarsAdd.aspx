<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarsAdd.aspx.cs" Inherits="Web.SupplierControl.CarsManager.CarsAdd" %>

<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <style type="text/css">
        .perview
        {
            width: 713px;
            background: #fff;
            font-size: 12px;
            border-collapse: collapse;
        }
        .tjbtn02 input
        {
            display: block;
            font-size: 14px;
            color: #000000;
            font-weight: bold;
            background: url(../../images/tjbtnbg02.gif) no-repeat 0 0;
            width: 76px;
            height: 31px;
            line-height: 31px;
        }
        .tjbtn02 input:hover
        {
            background: url(../../images/tjbtnbg.gif) no-repeat 0 0;
            text-decoration: none;
        }
        .style1
        {
            width: 88px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <input type="hidden" id="hid_type" value="<%=type %>" />
    <table cellspacing="1" cellpadding="0" border="0" align="center" width="800" style="margin-top: 10px;">
        <tbody>
            <tr class="odd">
                <th height="30" align="center" class="style1">
                    <span style="color: Red">*</span>省份：
                </th>
                <td align="left" width="330">
                    <uc1:ucProvince ID="ucProvince1" runat="server" />
                </td>
                <th align="center" width="70">
                    <span style="color: Red">*</span>城市：
                </th>
                <td align="left" width="330">
                    <uc2:ucCity ID="ucCity1" runat="server" />
                    &nbsp;
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center" class="style1">
                    <span style="color: Red">*</span>单位名称：
                </th>
                <td align="left" colspan="3">
                    <input type="text" id="companyName" class="searchinput" name="companyName" style="width: 150px"
                        valid="required" errmsg="请输入单位名称" runat="server" />
                    <span id="errMsg_companyName" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center" class="style1">
                    地址：
                </th>
                <td align="left" colspan="3">
                    <input type="text" id="companyAddress" name="companyAddress" style="width: 150px"
                        class="searchinput" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th align="center" class="style1">
                    联系人：
                </th>
                <td align="left" colspan="3">
                    <table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" width="100%"
                        id="tbl_linkman">
                        <tbody>
                            <tr class="odd">
                                <th height="30" align="center" width="14%">
                                    <span style="color: Red">*</span>姓名
                                </th>
                                <th align="center" width="14%">
                                    职务
                                </th>
                                <th align="center" width="13%">
                                    电话
                                </th>
                                <th align="center" width="13%">
                                    <span style="color: Red">*</span>手机
                                </th>
                                <th align="center" width="13%">
                                    QQ
                                </th>
                                <th align="center" width="13%">
                                    E-mail
                                </th>
                                <th align="center" width="12%">
                                    传真
                                </th>
                                <th align="center" width="15%">
                                    操作
                                </th>
                            </tr>
                            <asp:Repeater ID="rep_linkman" runat="server">
                                <ItemTemplate>
                                    <tr class="even" id='tr_linkman_<%# Container.ItemIndex %>' name="linkman_tr">
                                        <th height="30" align="center">
                                            <input type="text" id='linkman_name_<%# Container.ItemIndex %>' class="searchinput"
                                                name="linkman_name" value="<%#Eval("ContactName") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_bussiness_<%# Container.ItemIndex %>" class="searchinput"
                                                name="linkman_bussiness" value="<%#Eval("JobTitle") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_tel_<%# Container.ItemIndex %>" class="searchinput"
                                                name="linkman_tel" value="<%#Eval("ContactTel") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_phone_<%# Container.ItemIndex %>" class="searchinput"
                                                name="linkman_phone" value="<%#Eval("ContactMobile") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_qq_<%# Container.ItemIndex %>" class="searchinput"
                                                name="linkman_qq" value="<%#Eval("QQ") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_email_<%# Container.ItemIndex %>" class="searchinput"
                                                name="linkman_email" value="<%#Eval("Email") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="linkman_fax" class="searchinput" name="linkman_fax" value="<%#Eval("ContactFax") %>" />
                                        </th>
                                        <%if (linkman_row == 1)
                                          { %>
                                        <td align="center">
                                            <%if (!show)
                                              { %><a href="javascript:void(0);" class="add_linkman" id="add_linkman_<%# Container.ItemIndex %>">
                                                  <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />
                                                  添加</a><%} %>
                                        </td>
                                        <%}
                                          else
                                          { %>
                                        <td align="center">
                                            <%if (!show)
                                              { %><a href="javascript:void(0);" class="del_linkman" id="A2" onclick="del_linkman(this)">
                                                  <img height="16" width="15" src="../../images/delicon01.gif" alt="" />
                                                  删除</a><%} %>
                                        </td>
                                        <%}
                                        %>
                                        <%linkman_row++; %>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%if (this.hideContactCount.Value == "0")
                              { %>
                            <tr class="even" id='tr_linkman_1' name="linkman_tr">
                                <th height="30" align="center">
                                    <input type="text" id='linkman_name_1' class="searchinput" name="linkman_name" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_bussiness_1" class="searchinput" name="linkman_bussiness" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_tel_1" class="searchinput" name="linkman_tel" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_phone_1" class="searchinput" name="linkman_phone" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_qq_1" class="searchinput" name="linkman_qq" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_email_1" class="searchinput" name="linkman_email" />
                                </th>
                                <th align="center">
                                    <input type="text" id="linkman_fax_1" class="searchinput" name="linkman_fax" />
                                </th>
                                <td align="center">
                                    <a href="javascript:void(0);" class="add_linkman" id="add_linkman_1">
                                        <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />
                                        添加</a>
                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center" class="style1">
                    车辆信息：
                </th>
                <td align="left" colspan="3">
                    <table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" width="100%"
                        id="tbl_car">
                        <tbody>
                            <tr class="odd">
                                <th height="30" align="center" width="10%">
                                    <span style="color: Red">*</span>车型
                                </th>
                                <th align="center" width="8%">
                                    车牌号
                                </th>
                                <th align="center" width="40%">
                                    图片
                                </th>
                                <th align="center" width="8%">
                                    价格
                                </th>
                                <th align="center" width="10%">
                                    司机
                                </th>
                                <th align="center" width="10%">
                                    司机电话
                                </th>
                                <th align="center" widdth="14%">
                                    导游词
                                </th>
                                <th align="center" width="12%">
                                    操作
                                </th>
                            </tr>
                            <%if (this.hideCarCount.Value == "0")
                              { %>
                            <tr class="even" id="tr_car_1" name="car_tr">
                                <th height="30" align="center">
                                    <input type="text" id="car_type_1" class="searchinput" name="car_type" />
                                </th>
                                <th align="center">
                                    <input type="text" id="car_num_1" class="searchinput" name="car_num" />
                                </th>
                                <th align="center">
                                    <%if (!show)
                                      { %>
                                    <input type="file" name="car_img" />
                                    <input type="hidden" name="hid_img" /><%} %>
                                </th>
                                <th align="center">
                                    <input type="text" id="car_price_1" class="jiesuantext" name="car_price" vaild="isNumber"
                                        errmsg="请输入数字" /><span id="errMsg_car_price_1" class="errmsg"></span>
                                </th>
                                <th align="center">
                                    <input type="text" id="car_driver_1" class="searchinput" name="car_driver" />
                                </th>
                                <th align="center">
                                    <input type="text" id="car_driverPhone_1" class="searchinput" name="car_driverPhone" />
                                </th>
                                <th align="center">
                                    <input type="text" id="car_world_1" class="searchinput" name="car_world" />
                                </th>
                                <td align="center">
                                    <a href="javascript:void(0);" id="add_car_1" class="add_car">
                                        <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />
                                        添加</a>
                                </td>
                            </tr>
                            <%} %>
                            <asp:Repeater ID="rep_car" runat="server">
                                <ItemTemplate>
                                    <tr class="even" id="tr_car_<%#Container.ItemIndex %>" name="car_tr">
                                        <th height="30" align="center">
                                            <input type="text" id="car_type_<%#Container.ItemIndex %>" class="searchinput" name="car_type"
                                                value="<%#Eval("CarType") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="car_num_<%#Container.ItemIndex %>" class="searchinput" name="car_num"
                                                value="<%#Eval("CarNumber") %>" />
                                        </th>
                                        <input type="hidden" name="hid_img" value="<%#Eval("Image") %>" />
                                        <th>
                                            <div style="float: left;">
                                                <input type="file" name="car_img" /></div>
                                            <%#ImageHave(Convert.ToString(Eval("Image"))) %>
                                        </th>
                                        <th align="center">
                                            <input type="text" id="car_price_<%#Container.ItemIndex %>" class="jiesuantext" name="car_price"
                                                value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("Price").ToString()).ToString("0.00"))%>"
                                                vaild="isNumber" errmsg="请输入数字" /><span id="errMsg_car_price_<%#Container.ItemIndex %>"
                                                    class="errmsg"></span>
                                        </th>
                                        <th align="center">
                                            <input type="text" id="car_driver_<%#Container.ItemIndex %>" class="searchinput"
                                                name="car_driver" value="<%#Eval("DriverName") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="car_driverPhone_<%#Container.ItemIndex %>" class="searchinput"
                                                name="car_driverPhone" value="<%#Eval("DriverPhone") %>" />
                                        </th>
                                        <th align="center">
                                            <input type="text" id="car_world_<%#Container.ItemIndex %>" class="searchinput" name="car_world"
                                                value="<%#Eval("GuideWord") %>" />
                                        </th>
                                        <%if (car_row == 1)
                                          { %>
                                        <td align="center">
                                            <%if (!show)
                                              { %><a href="javascript:void(0);" id="add_car_1" class="add_car">
                                                   <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />
                                                   添加</a>
                                        </td>
                                        <%}
                                          }
                                          else
                                          { %>
                                        <td align="center">
                                            </th><%if (!show)
                                                   { %><a href="javascript:void(0);" id="del_car_<%#Container.ItemIndex %>" onclick="del_car(this)">
                                                   <img height="16" width="15" src="../../images/delicon01.gif" alt="" />
                                                   删除</a>
                                        </td>
                                        <%}
                                          }%>
                                        <%car_row++; %>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th height="60" align="center" class="style1">
                    备注：
                </th>
                <td colspan="3">
                    <textarea class="textareastyle" rows="5" cols="45" id="companyRemark" name="companyRemark"
                        runat="server"></textarea>
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="0" border="0" align="center" width="320">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <%if (!show)
                      { %>
                    <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return checkForm()" OnClick="btnSave_Click1"
                        Text="保存" /><%} %>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="livemargins_control" style="position: absolute; display: none; z-index: 9999;">
        <img height="5" width="77" style="position: absolute; left: -77px; top: -5px;" src="chrome://livemargins/skin/monitor-background-horizontal.png" />
        <img style="position: absolute; left: 0pt; top: -5px;" src="chrome://livemargins/skin/monitor-background-vertical.png" />
        <img style="position: absolute; left: 1px; top: 0pt; opacity: 0.5; cursor: pointer;"
            onmouseout="this.style.opacity=0.5" onmouseover="this.style.opacity=1" src="chrome://livemargins/skin/monitor-play-button.png"
            id="monitor-play-button" />
    </div>
    <asp:HiddenField ID="hideContactCount" runat="server" Value="0" />
    <asp:HiddenField ID="hideCarCount" runat="server" Value="0" />
    <asp:HiddenField ID="pro_id" runat="server" Value="0" />
    <asp:HiddenField ID="city_id" runat="server" Value="0" />
    </form>
</body>

<script type="text/javascript">
    $(function() {
        var link_count = $("#<%=hideContactCount.ClientID %>").val();
        var car_count = $("#<%=hideCarCount.ClientID %>").val();
        //判断是添加还是更新
        var type = $("#hid_type").val();
        //添加联系人输入框事件
        $(".add_linkman").click(function() {
            link_count++;
            $("#tbl_linkman").append("<tr class=\"even\" id=\"tr_linkman_" + link_count + "\" name=\"linkman_tr\"><th height=\"30\" align=\"center\"><input name=\"linkman_name\" type=\"text\" id=\"linkman_name_" + link_count + "\" class=\"searchinput\"   /><th align=\"center\"><input name=\"linkman_bussiness\" type=\"text\" id=\"linkman_bussiness_" + link_count + "\" class=\"searchinput\"   /><th align=\"center\"><input name=\"linkman_tel\" type=\"text\" id=\"linkman_tel_" + link_count + "\" class=\"searchinput\" /><th align=\"center\"><input name=\"linkman_phone\" type=\"text\" id=\"linkman_phone_" + link_count + "\" class=\"searchinput\"  /><th align=\"center\"><input name=\"linkman_qq\" type=\"text\" id=\"linkman_qq_" + link_count + "\" class=\"searchinput\" /><th align=\"center\"><input name=\"linkman_email\" type=\"text\" id=\"linkman_email_" + link_count + "\" class=\"searchinput\"  /><th align=\"center\"><input type=\"text\" id=\"linkman_fax_" + link_count + "\"  class=\"searchinput\" name=\"linkman_fax\"/><td align=\"center\" id=\"del_linkman_" + link_count + "\"><a href=\"javascript:void(0)\" onclick='del_linkman(this)'><img height=\"16\" width=\"15\" a src=\"../../images/delicon01.gif\" />删除</a>");

            $("#<%=hideContactCount.ClientID %>").val(link_count);

            return false;

        })
        //添加汽车信息输入框事件
        $(".add_car").click(function() {
            car_count++;
            $("#tbl_car").append("<tr class=\"even\" id=\"tr_car_" + car_count + "\" name=\"car_tr\"><th height=\"30\" align=\"center\" ><input type=\"text\" name=\"car_type\" class=\"searchinput\" id=\"car_type_" + car_count + "\" /><th align=\"center\"><input type=\"text\" id=\"car_num_" + car_count + "\" class=\"searchinput\" name=\"car_num\"/><th align=\"center\"><input type=\"file\" id=\"car_img_" + car_count + "\" name=\"car_img\"/><input type=\"hidden\" name=\"hid_img\" /><th align=\"center\"><input type=\"text\" id=\"car_price_" + car_count + "\" class=\"jiesuantext\" name=\"car_price\"/></th><th align=\"center\"><input type=\"text\" id=\"car_driver_" + car_count + "\" class=\"searchinput\" name=\"car_driver\"/><th align=\"center\"><input type=\"text\" id=\"car_driverPhone_" + car_count + "\" class=\"searchinput\" name=\"car_driverPhone\"/><th align=\"center\"><input type=\"text\" id=\"car_world_" + car_count + "\" class=\"searchinput\" name=\"car_world\" /><td align=\"center\"><a href=\"javascript:void(0)\" id=\"del_car_" + car_count + "\" onclick='del_car(this)'><img height=\"16\" width=\"15\" src=\"../../images/delicon01.gif\"/>删除</a>");

            $("#<%=hideCarCount.ClientID %>").val(car_count);

            return false;
        })

    })
    //验证事件
    function checkForm() {
        var proid = provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId;
        var cityid = provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId;
        $("#pro_id").val($("#" + proid).val());
        $("#city_id").val($("#" + cityid).val());
        if ($("#" + proid).val() == '0') {
            alert("请选择省份");
            return false;
        }
        if ($("#" + cityid).val() == '0') {
            alert("请选择城市");
            return false;
        }
        var form = $("#btnSave").closest("form").get(0);
        if (ValiDatorForm.validator(form, "span") && checkName() && checkPhone() && checkCarType() && checkPhoto()) {
            return true;
        } else {
            return false;
        }
    }
    //验证联系人姓名是否空
    function checkName() {
        var res = true;
        $("#tbl_linkman").find("input[name='linkman_name']").each(function() {
            if (!$.trim($(this).val())) {
                alert("请填写联系人姓名");
                $(this).focus();
                res = false;
                return false;
            }
        });
        return res;
    }
    //验证联系人手机号是否空
    function checkPhone() {
        var res = true;
        $("#tbl_linkman").find("input[name='linkman_phone']").each(function() {
            if (!$.trim($(this).val())) {
                alert("请填写联系手机号");
                $(this).focus();
                res = false;
                return false;
            }
        });

        return res;
    }
    //验证车辆型号是否为空
    function checkCarType() {
        var res = true;
        $("#tbl_car").find("input[name='car_type']").each(function() {
            if (!$.trim($(this).val())) {
                alert("请输入车辆型号");
                $(this).focus();
                res = false;
                return false;
            }
        });
        return res;
    }
    //验证上传附件的后缀是否为图片
    function checkPhoto() {
        var res = true;
        $("#tbl_car").find("input[name='car_img']").each(function() {
            var img_type;
            if ($(this).val() != "") {
                img_type = $(this).val().substring($(this).val().lastIndexOf('.'), $(this).val().length);
                if (img_type.toLowerCase() != ".jpg" && img_type.toLowerCase() != ".jpeg" && img_type.toLowerCase() != ".png" && img_type.toLowerCase() != ".gif" && img_type.toLowerCase() != ".xls" && img_type.toLowerCase() != ".doc" && img_type.toLowerCase() != ".docx" && img_type.toLowerCase() != ".rar" && img_type.toLowerCase() != ".txt") {
                    alert("请选择正确格式的文件");
                    $(this).focus();
                    res = false;
                    return false;
                }
            }

        });
        return res;
    }

    function del_linkman(obj) {
        $(obj).parent().parent().remove();
        return false;
    }
    function del_car(obj) {
        $(obj).parent().parent().remove();
        return false;
    }
</script>

</html>
