<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaAdd.aspx.cs" Inherits="Web.SupplierControl.AreaConnect.AreaAdd" %>

<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="900" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="80" height="30" align="center">
                    <span style="color: red">*</span>省份：
                </th>
                <td width="330" align="left">
                    <uc1:ucProvince ID="ucProvince1" runat="server" />
                    <input type="hidden" name="proname" id="proname" />
                </td>
                <th width="80" align="center">
                    <span style="color: red">*</span>城市：
                </th>
                <td width="330" align="left">
                    <uc2:ucCity ID="ucCity1" runat="server" />
                    <input type="hidden" name="cityname" id="cityname" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    <span style="color: red">*</span>单位名称：
                </th>
                <td align="left">
                    <input type="text" id="unionname" name="unionname" value="<% =csModel.UnitName %>"
                        class="searchinput searchinput02" valid="required" errmsg="单位不能为空" />
                    <span id="errMsg_unionname" class="errmsg"></span>
                </td>
                <th align="center">
                    地址：
                </th>
                <td align="left">
                    <input type="text" id="txtAddress" name="txtAddress" value="<% =csModel.UnitAddress %>"
                        class="searchinput searchinput02" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    合作协议：
                </th>
                <td align="left" class="updom">
                    <%if (!string.IsNullOrEmpty(csModel.AgreementFile))
                      { %>
                    <a target="_blank" href="<%=csModel.AgreementFile %>">查看合作协议</a><img style="cursor: pointer"
                        src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                    <%}
                      else
                      { %>
                    <input type="file" name="workAgree" />
                    <%} %>
                </td>
                <th align="center">
                    许可证:
                </th>
                <td align="left">
                    <input type="text" id="txtLicense" name="txtLicense" value="<% =csModel.LicenseKey %>"
                        class="searchinput searchinput02" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    返佣设置：
                </th>
                <td colspan="3" align="left">
                    <input type="text" id="txtReturnSet" name="txtReturnSet" value="<% =csModel.Commission.ToString( "0.## ") %>"
                        class="searchinput" valid="IsDecimalTwo" errmsg="返佣为两位小数" />
                    <span id="errMsg_txtReturnSet" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    联系人：
                </th>
                <td colspan="3" align="left">
                    <table id="userlist" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF">
                        <tr class="odd">
                            <th width="5%" height="30" align="center">
                                <span style="color: red">*</span>姓名
                            </th>
                            <th width="5%" align="center">
                                职务
                            </th>
                            <th width="12%" align="center">
                                电话
                            </th>
                            <th width="12%" align="center">
                                <span style="color: red">*</span>手机
                            </th>
                            <th width="8%" align="center">
                                QQ
                            </th>
                            <th width="8%" align="center">
                                传真
                            </th>
                            <th width="8%" align="center">
                                用户名
                            </th>
                            <th width="8%" align="center">
                                密码
                            </th>
                            <th width="10%" align="center">
                                操作
                            </th>
                        </tr>
                        <%if (csModel.SupplierContact != null && csModel.SupplierContact.Count > 0)
                          {
                              int temp = 0;
                              foreach (EyouSoft.Model.CompanyStructure.SupplierContact sc in csModel.SupplierContact)
                              {
                        %>
                        <tr class="<% =temp%2==0?"even":"odd" %>">
                            <th height="30" align="center">
                                <input type="hidden" name="hidcontactid" value="<%=sc.Id %>" /><input type="text"
                                    name="inname" class="searchinput inname" value="<%=sc.ContactName %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" value="<%=sc.JobTitle %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="inphone" style="border: 1px solid #93B7CE;
                                    font-size: 12px; height: 15px; width: 100px" value="<%=sc.ContactTel %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="inmobile" style="border: 1px solid #93B7CE;
                                    font-size: 12px; height: 15px; width: 100px" value="<%=sc.ContactMobile %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" value="<%=sc.QQ %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="infax" class="searchinput infax" value="<%=sc.ContactFax %>" />
                            </th>
                            <th align="center">
                                <input type="hidden" name="inusernameid" value="<%= sc.UserAccount!=null?sc.UserAccount.ID.ToString():""%>" /><input
                                    <%= sc.UserAccount!=null?"readonly='readonly'":"" %> type="text" name="inusername"
                                    class="searchinput" value="<%=sc.UserAccount!=null?sc.UserAccount.UserName:"" %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inpwd" class="searchinput" value="<%=sc.UserAccount!=null?sc.UserAccount.PassWordInfo.NoEncryptPassword:"" %>" />
                            </th>
                            <td align="center">
                                <%if (!show)
                                  { %><a href="javascript:;" class="add"><img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
                                          添加</a> <a href="javascript:;" class="del">
                                              <img src="/images/delicon01.gif" width="14" height="14" />
                                              删除</a><%} %>
                            </td>
                        </tr>
                        <% temp++;
                    }
                }
                          else
                          { %>
                        <tr class="even">
                            <th height="30" align="center">
                                <input type="hidden" name="hidcontactid" /><input type="text" name="inname" class="searchinput inname" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="inphone" style="border: 1px solid #93B7CE;
                                    font-size: 12px; height: 15px; width: 100px" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="inmobile" style="border: 1px solid #93B7CE;
                                    font-size: 12px; height: 15px; width: 100px" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" />
                            </th>
                            <th align="center">
                                <input type="text" name="infax" class="searchinput infax" />
                            </th>
                            <th align="center">
                                <input type="hidden" name="inusernameid" /><input type="text" name="inusername" class="searchinput" />
                            </th>
                            <th align="center">
                                <input type="text" name="inpwd" class="searchinput" />
                            </th>
                            <td align="center">
                                <%if (!show)
                                  { %><a href="javascript:;" class="add"><img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
                                     添加</a> <a href="javascript:;" class="del">
                                         <img src="/images/delicon01.gif" width="14" height="14" />
                                         删除</a><%} %>
                            </td>
                        </tr>
                        <%} %>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="60" align="center">
                    备注：
                </th>
                <td colspan="3">
                    <textarea name="remark" id="remark" cols="45" rows="5" class="textareastyle"><%= csModel.Remark %></textarea>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" style="display: <%=!show?"block":"none"%>" class="save">保存</a>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
                <input type="hidden" name="tid" value="<%=tid %>" />
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {
            $("img.close").click(function() {
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />");
            });

            $("a.save").click(function() {

                var proid = provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId;
                var cityid = provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId;
                if ($("#" + proid).val() == '0') {
                    alert("请选择省份");
                    return false;
                }
                if ($("#" + cityid).val() == '0') {
                    alert("请选择城市");
                    return false;
                }
                //如果填写了用户名，则对应的密码不能为空
                var isUserPwdValid = true;
                $("input[name='inusername']").each(function() {
                    var $u = $(this);
                    var $p = $u.closest("tr").find("input[name='inpwd']");

                    if ($.trim($u.val()) != "" && $p.length > 0) {
                        if ($.trim($p.val()) == "") {
                            isUserPwdValid = false;
                            return false;
                        }
                    }
                });
                if (isUserPwdValid == false) {
                    alert("如果填写了联系人中的【用户名】，则对应的【密码】不能为空");
                    return false;
                }

                $("#proname").val($("#" + proid + ">option:selected").eq(0).text());
                $("#cityname").val($("#" + cityid + ">option:selected").eq(0).text());

                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                if (ValiDatorForm.validator(form, "span") && checkname() && checkmobile()) {
                    form.submit();
                } else {
                    return false;
                }

                return false;
            });

            function checkname() {
                var res = true;
                $("#userlist").find("input.inname").each(function() {
                    if (!$.trim($(this).val())) {
                        alert("请填写联系人姓名");
                        $(this).focus();
                        res = false;
                        return false;
                    }
                });
                return res;
            }

            function checkmobile() {
                var res = true;
                $("#userlist").find("input.inmobile").each(function() {
                    if (!$.trim($(this).val())) {
                        alert("请填写联系人手机号码");
                        $(this).focus();
                        res = false;
                        return false;
                    }
                });
                return res;
            }

            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("a.save").closest("form").get(0));


            function getuserinput() {
                var cls = $("#userlist").find("tr").length % 2 ? "even" : "odd";
                var html = "<tr class=\"" + cls + "\">" +
                                "<th height=\"30\" align=\"center\"><input type=\"hidden\" name=\"hidcontactid\" /><input  type=\"text\" name=\"inname\"  class=\"searchinput inname\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"indate\" class=\"searchinput indate\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inphone\" class=\"inphone\" style=\"border:1px solid #93B7CE;font-size:12px;height:15px;width:100px\"/></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inmobile\" class=\"inmobile\" style=\"border:1px solid #93B7CE;font-size:12px;height:15px;width:100px\"/></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inqq\" class=\"searchinput inqq\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"infax\" class=\"searchinput infax\" /></th>" +
                                "<th align=\"center\"><input type=\"hidden\" name=\"inusernameid\" /><input type=\"text\" name=\"inusername\" class=\"searchinput\" /></th>" +
                                "<th align=\"center\"><input type=\"text\" name=\"inpwd\" class=\"searchinput\" /></th>" +
                                "<td align=\"center\"><a href=\"javascript:;\" class=\"add\" ><img height=\"16\" width=\"15\" alt=\"\" src=\"/images/tianjiaicon01.gif\" /> 添加</a> <a href=\"javascript:;\" class=\"del\" ><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a></td>" +
                                "</tr>";
                return html;
            }
            function add() {
                var html = getuserinput();
                $("#userlist").append(html);
                $("a.add").unbind().bind("click", function() {
                    add();
                    return false;
                });
                $("a.del").unbind().bind("click", function() {
                    var that = $(this);
                    del(that);
                    return false;
                });
            }


            function del(that) {
                var trlist = $("#userlist").find("tr");
                if (trlist.length > 2) {
                    that.parent().parent().remove();
                } else {
                    that.parent().parent().find("input").val("");
                }
            }
            $("a.add").bind("click", function() {
                add();
                return false;
            });
            $("a.del").bind("click", function() {
                var that = $(this);
                del(that);
                return false;
            });

        });
    </script>

    </form>
</body>
</html>
