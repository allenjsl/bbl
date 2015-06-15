<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SightAdd.aspx.cs" Inherits="Web.SupplierControl.SightManager.SightAdd" %>

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
        .style2
        {
            width: 88px;
            height: 27px;
        }
        .style3
        {
            height: 27px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <input type="hidden" id="hid_linkmanCount" value="<%=linkman_row %>" />
    <input type="hidden" name="hid_img" />
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
                    <span style="color: Red">&nbsp; *</span>城市：
                </th>
                <td align="left" width="330">
                    <uc2:ucCity ID="ucCity1" runat="server" />
                    &nbsp;
                </td>
            </tr>
            <tr class="even">
                <th align="center" class="style2">
                    <span style="color: Red">*</span>景点名称：
                </th>
                <td align="left" class="style3">
                    <input type="text" id="companyName" class="searchinput" name="companyName" style="width: 150px"
                        valid="required" errmsg="请输入景点名称" runat="server" />
                    <span id="errMsg_companyName" class="errmsg"></span>
                </td>
                <th align="center" class="style2">
                    星级：
                </th>
                <td align="left" width="330" class="style3">
                    <asp:DropDownList ID="sel_star" runat="server">
                    </asp:DropDownList>
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
            <tr class="even">
                <th height="30" align="center" class="style1">
                    合作协议：
                </th>
                <td align="left" colspan="3">
                    <input type="file" name="file_green" id="file_green" style="float: left" /><asp:HiddenField
                        ID="green_file" runat="server" />
                    <%if (hz_img_state == true)
                      { %><div id="div_hz" style="float: left">
                          <img src="../../images/sanping_03.gif" /><a target="_blank" href="<%=GetPathByType("green")%>"><asp:Label
                              ID="hz_img_name" runat="server"></asp:Label></a><a id="del_img_hz" href="javascript:void(0):;"
                                  onclick="del_img_hz()"><img src="../../images/fujian_x.gif" /></a></div>
                    <%} %>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center" class="style1">
                    景点图片：
                </th>
                <td align="left" colspan="3">
                    <asp:HiddenField ID="landspace_file" runat="server" />
                    <asp:HiddenField ID="landspace_name" runat="server" />
                    <%if (jd_img_state == true)
                      { %><div id="div_jd" style="float: left">
                          <asp:HiddenField ID="sight_hidden" runat="server" />
                          <asp:HiddenField ID="hidPhotos" runat="server" />
                          <asp:HiddenField ID="hidImgPhoto" runat="server" />
                      </div>
                    <%} %>
                    <table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" width="100%"
                        id="tbl_photo">
                        <tr class="odd">
                            <td align="center" width="30%">
                                选择图片
                            </td>
                            <td align="center" width="40%">
                                图片预览(点击图片查看原图)
                            </td>
                            <td align="center" width="30%">
                                操作
                            </td>
                        </tr>
                        <%if (type == "add" || photo_row == 0)
                          { %>
                        <tr class="even">
                            <td align="center">
                                <input type="file" class="file_landspace" name="file_landspace" onchange="changePhoto(this)" />
                            </td>
                            <td align="center">
                                -
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" class="add_photo" id="a1">
                                    <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />添加</a>
                                <a href='javascript:void(0);' onclick="del_photo_first(this)">
                                    <img height='16' width='15' src='../../images/delicon01.gif' alt='' />清空本行</a>
                            </td>
                        </tr>
                        <%}
                          else
                          { %>
                        <asp:Repeater ID="repPhotos" runat="server">
                            <ItemTemplate>
                                <tr class="even">
                                    <td align="center">
                                        <input type="file" class="file_landspace" name="file_landspace" onchange="changePhoto(this)" />
                                    </td>
                                    <td align="center">
                                        <a href="<%#Eval("PicPath") %>" target="_blank" class="viewPhoto">
                                            <img name="view_photo" class="view_photo" str="<%#Eval("PicPath") %>♂<%#Eval("PicName") %>♀"
                                                width="80px;" height="60px;" title="点击查看大图"   src="<%#Eval("PicPath") %>" /></a>
                                    </td>
                                    <td align="center">
                                        <%if (photo_row == 1)
                                          { %>
                                        <a href="javascript:void(0);" class="add_photo">
                                            <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />添加</a>
                                        <a href='javascript:void(0);' onclick='del_photo_first(this)'>
                                            <img height='16' width='15' src='../../images/delicon01.gif' alt='' />清空本行</a>
                                        <%}
                                          else
                                          { %><a href='javascript:void(0);' onclick='del_photo(this)'>
                                              <img height='16' width='15' src='../../images/delicon01.gif' alt='' />删除</a>
                                        <%} %>
                                    </td>
                                </tr>
                                <%photo_row++; %>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%} %>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center" class="style1">
                    导游词：
                </th>
                <td align="left" colspan="3">
                    <textarea name="guideworld" style="width: 400px; height: 200px" id="guideworld" runat="server"></textarea>
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
                                    姓名
                                </th>
                                <th align="center" width="14%">
                                    职务
                                </th>
                                <th align="center" width="13%">
                                    电话
                                </th>
                                <th align="center" width="13%">
                                    手机
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
                                        <%} %>
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
                                    <%if (!show)
                                      { %><a href="javascript:void(0);" class="add_linkman" id="add_linkman_1">
                                          <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />
                                          添加</a><%} %>
                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center" class="style1">
                    价格：
                </th>
                <td align="left" colspan="3">
                    散客价:<input type="text" id="single_price" valid="isNumber" errmsg="请输入数字" class="searchinput"
                        name="single_price" runat="server" />
                    <span id="errMsg_single_price" class="errmsg"></span>团队价:<input type="text" id="rl_price"
                        class="searchinput" name="rl_price" runat="server" valid="isNumber" errmsg="请输入数字" />
                    <span id="errMsg_rl_price" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center" class="style1">
                    政策：
                </th>
                <td align="left" colspan="3">
                    <textarea class="textareastyle" rows="5" cols="45" name="txt_zc" id="txt_zc" runat="server"></textarea>
                </td>
            </tr>
            <tr class="even">
                <th height="60" align="center" class="style1">
                    备注：
                </th>
                <td colspan="3">
                    <textarea class="textareastyle" rows="5" cols="45" id="txt_remark" name="txt_remark"
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
                    <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return checkForm()" OnClick="btnSave_Click"
                        Text="保存" />
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
    <asp:HiddenField ID="greend_hidden" runat="server" />
    <asp:HiddenField ID="pro_id" runat="server" />
    <asp:HiddenField ID="city_id" runat="server" />
    </form>
</body>

<script type="text/javascript">
    $(function() {
        $("#hz_name").val("123");
        var link_count = $("#<%=hideContactCount.ClientID %>").val();
        //判断是添加还是更新
        var type = $("#hid_type").val();
        //添加联系人输入框事件
        $(".add_linkman").click(function() {
            link_count++;
            $("#tbl_linkman").append("<tr class=\"even\" id=\"tr_linkman_" + link_count + "\" name=\"linkman_tr\"><th height=\"30\" align=\"center\"><input name=\"linkman_name\" type=\"text\" id=\"linkman_name_" + link_count + "\" class=\"searchinput\"   /><th align=\"center\"><input name=\"linkman_bussiness\" type=\"text\" id=\"linkman_bussiness_" + link_count + "\" class=\"searchinput\"   /><th align=\"center\"><input name=\"linkman_tel\" type=\"text\" id=\"linkman_tel_" + link_count + "\" class=\"searchinput\" /><th align=\"center\"><input name=\"linkman_phone\" type=\"text\" id=\"linkman_phone_" + link_count + "\" class=\"searchinput\"  /><th align=\"center\"><input name=\"linkman_qq\" type=\"text\" id=\"linkman_qq_" + link_count + "\" class=\"searchinput\" /><th align=\"center\"><input name=\"linkman_email\" type=\"text\" id=\"linkman_email_" + link_count + "\" class=\"searchinput\"  /><th align=\"center\"><input type=\"text\" id=\"linkman_fax_" + link_count + "\"  class=\"searchinput\" name=\"linkman_fax\"/><td align=\"center\" id=\"del_linkman_" + link_count + "\"><a href=\"javascript:void(0)\" onclick='del_linkman(this)'><img height=\"16\" width=\"15\" a src=\"../../images/delicon01.gif\" />删除</a>");

            $("#<%=hideContactCount.ClientID %>").val(link_count);

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
        if (ValiDatorForm.validator(form, "span") && checkGreend() && checkPhoto() ) {
            getImgStr();
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
            }
        });
        return res;
    }
    //验证图片上传格式
    function checkPhoto() {
        var img_type;
        var res = true;
        $(".file_landspace").each(function() {
            if ($(this).val() != "") {
                img_type = $(this).val().substring($(this).val().lastIndexOf('.'), $(this).val().length);
                if (img_type.toLowerCase() != ".jpg" && img_type.toLowerCase() != ".jpeg" && img_type.toLowerCase() != ".png" && img_type.toLowerCase() != ".gif") {
                    alert("请选择正确格式的图片文件");
                    $(this).focus();
                    res = false;
                    return false;
                }
            }
        })
        return res;
    }
    //验证上传协议格式
    function checkGreend() {
        var img_type;
        var res = true;
        if ($("#file_green").val() != "") {
            img_type = $("#file_green").val().substring($("#file_green").val().lastIndexOf('.'), $("#file_green").val().length);
            if (img_type.toLowerCase() != ".jpg" && img_type.toLowerCase() != ".jpeg" && img_type.toLowerCase() != ".png" && img_type.toLowerCase() != ".gif" && img_type.toLowerCase() != ".xls" && img_type.toLowerCase() != ".doc" && img_type.toLowerCase() != ".docx" && img_type.toLowerCase() != ".rar" && img_type.toLowerCase() != ".txt") {
                alert("请选择正确格式的协议文件");
                $("#file_green").focus();
                res = false;
                return false;
            }

        }
        return res;
    }
    function del_linkman(obj) {
        $(obj).parent().parent().remove();
        return false;
    }
    function del_img_hz() {
        if (confirm("确定要删除么")) {
            $("#div_hz").css("display", "none");
            $("#greend_hidden").val("null");
        }
        return false;
    }
    function del_img_jd() {
        if (confirm("确定要删除么")) {
            $("#div_jd").css("display", "none");
            $("#sight_hidden").val("null");
        }
        return false;
    }
    //首行图片删除处理,不删除本行,删除图片预览
    function del_photo_first(obj) {
        var tr = $(obj).parent().parent();
        var td = tr.children().children("a[class='viewPhoto']").children("img").parent().parent();
        var td_first = tr.children().children("input").parent();
        tr.children().children("a[class='viewPhoto']").children("img").remove();
        td.html("-");
        var input = tr.children().children("input");
        tr.children().children("input").remove();
        td_first.html("<input type='file' name='file_landspace' onchange='changePhoto(this)' />")

    }
    //删除图片
    function del_photo(obj) {
        $(obj).parent().parent().remove();
        return false;
    }
    //添加图片行
    $(".add_photo").click(function() {
        //            $("#tbl_photo").append(" <tr  class=\"even\"><td align=\"center\"><input type=\"file\" name=\"file_landspace\"  style=\"float: left\" /></td><td align=\"center\">-</td><td align=\"center\"><a href=\"javascript:void(0);\" class=\"add_photo\" ><img height=\"16\" width=\"15\" src=\"../../images/tianjiaicon01.gif\"  />添加</a>    <a href=\"javascript:void(0);\" onclick=\"del_photo(this)\" ><img height=\"16\" width=\"15\" src=\"../../images/delicon01.gif\"  />删除</a></td></tr>");
        $("#tbl_photo").append("<tr height='22px' class='even'><td align='center'><input class=\"file_landspace\" type=\"file\" name=\"file_landspace\" /></td><td align='center'>-</td><td align='center'><a href=\"javascript:void(0);\" onclick=\"del_photo(this)\" ><img height=\"16\" width=\"15\" src=\"../../images/delicon01.gif\"  />删除</a></td></tr>");
        return false;
    })
    //更新时操作.如果选择图片,则清空那行的图片.
    function changePhoto(obj) {
        var tr = $(obj).parent().parent();
        var td = tr.children().children("a[class='viewPhoto']").children("img").parent().parent();
        var img = tr.children().children("a[class='viewPhoto']").children("img");
        tr.children().children("a[class='viewPhoto']").children("img").remove();
        td.html("-");
    }
    //保存时将所有图片的值保存到一个hid控件中
    function getImgStr() {
        var str = "";
        $(".view_photo").each(function() {
            str += $(this).attr("str");
        })
        $("#<%=hidImgPhoto.ClientID %>").val(str);
    }
    
</script>

</html>
