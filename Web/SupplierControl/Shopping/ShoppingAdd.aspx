<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingAdd.aspx.cs" Inherits="Web.SupplierControl.Shopping.ShoppingAdd" %>

<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="80" align="center">
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
                    <input type="text" id="unionname" name="unionname" value="<%=ssModel.UnitName %>"
                        class="searchinput searchinput02" valid="required" errmsg="单位不能为空" />
                    <span id="errMsg_unionname" class="errmsg"></span>
                </td>
                <th align="center">
                    <span style="color: red">*</span>销售商品：
                </th>
                <td align="left">
                    <input type="text" id="goodsName" name="goodsName" value="<%=ssModel.SaleProduct %>"
                        class="searchinput searchinput02" valid="required" errmsg="销售商品不能为空" />
                    <span id="errMsg_goodsName" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    地址：
                </th>
                <td colspan="3" align="left">
                    <input type="text" id="txtAddress" name="txtAddress" value="<%=ssModel.UnitAddress %>"
                        class="searchinput searchinput02" />
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    合作协议：
                </th>
                <td colspan="3" align="left" class="updom">
                    <%if (!string.IsNullOrEmpty(ssModel.AgreementFile))
                      { %>
                    <a target="_blank" href="<%=ssModel.AgreementFile %>">查看合作协议</a><img style="cursor: pointer"
                        src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                    <%}
                      else
                      { %>
                    <input type="file" name="workAgree" />
                    <%} %><asp:Literal ID="lstMsg" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    导游词：
                </th>
                <td colspan="3" align="left">
                    <textarea name="tourGuids" id="tourGuids" cols="45" rows="5" class="textareastyle"><%=ssModel.GuideWord%></textarea>
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    联系人：
                </th>
                <td colspan="3" align="left">
                    <table id="userlist" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF">
                        <tr class="odd">
                            <th width="12%" height="30" align="center">
                                <span style="color: red">*</span>姓名
                            </th>
                            <th width="12%" align="center">
                                职务
                            </th>
                            <th width="12%" align="center">
                                电话
                            </th>
                            <th width="12%" align="center">
                                <span style="color: red">*</span>手机
                            </th>
                            <th width="12%" align="center">
                                QQ
                            </th>
                            <th width="12%" align="center">
                                E-mail
                            </th>
                            <th width="12%" align="center">
                                传真
                            </th>
                            <th width="15%" align="center">
                                操作
                            </th>
                        </tr>
                        <%if (ssModel.SupplierContact != null && ssModel.SupplierContact.Count > 0)
                          {
                              int temp = 0;
                              foreach (EyouSoft.Model.CompanyStructure.SupplierContact sc in ssModel.SupplierContact)
                              {
                        %>
                        <tr class="<% =temp%2==0?"even":"odd" %>">
                            <th height="30" align="center">
                                <input type="text" name="inname" class="searchinput inname" value="<%=sc.ContactName %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" value="<%=sc.JobTitle %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="searchinput inphone" value="<%=sc.ContactTel %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="searchinput inmobile" value="<%=sc.ContactMobile %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" value="<%=sc.QQ %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inemail" class="searchinput inemail" value="<%=sc.Email %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="infax" class="searchinput infax" value="<%=sc.ContactFax %>" />
                            </th>
                            <td align="center">
                                 <%if (!show)
                                   { %><a href="javascript:;" class="add">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
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
                                <input type="text" name="inname" class="searchinput inname" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="searchinput inphone" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="searchinput inmobile" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" />
                            </th>
                            <th align="center">
                                <input type="text" name="inemail" class="searchinput inemail" />
                            </th>
                            <th align="center">
                                <input type="text" name="infax" class="searchinput infax" />
                            </th>
                            <td align="center">
                                <%if (!show)
                                  { %> <a href="javascript:;" class="add">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
                                    添加</a> <a href="javascript:;" class="del">
                                        <img src="/images/delicon01.gif" width="14" height="14" />
                                        删除</a><%} %>
                            </td>
                        </tr>
                        <%} %>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th height="60" align="center">
                    备注：
                </th>
                <td colspan="3">
                    <textarea name="remark" id="remark" cols="45" rows="5" class="textareastyle"><%=ssModel.Remark %></textarea>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <%if (!show)
                      { %><a href="javascript:;" class="save">保存</a><%} %>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
        <input type="hidden" name="tid" value="<% =tid %>" />
    </div>

    <script type="text/javascript">
        $(function() {
            $("img.close").click(function() {//删除原有协议添加新的协议
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //生成添加协议的控件
            });

            $("a.save").click(function() {

                var proid = provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId; //获取省份ID
                var cityid = provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId; //获取城市ID
                if ($("#" + proid).val() == '0') {//判断获取的省份ID是否为0
                    alert("请选择省份");
                    return false;
                }
                if ($("#" + cityid).val() == '0') {//判断获取的城市ID是否为0
                    alert("请选择城市");
                    return false;
                }
                $("#proname").val($("#" + proid + ">option:selected").eq(0).text()); //获取省份名称
                $("#cityname").val($("#" + cityid + ">option:selected").eq(0).text()); //获取城市名称

                var form = $(this).closest("form").get(0); //查找form
                //点击按纽触发执行的验证函数
                if (ValiDatorForm.validator(form, "span") && checkname() && checkmobile()) {
                    form.submit();
                } else {
                    return false;
                }

                return false;
            });

            function checkname() {//验证姓名是否为空
                var res = true;
                $("#userlist").find("input.inname").each(function() {//对所有input标签中name为inname的进行遍历
                    if (!$.trim($(this).val())) {//判断是否为空//知道作用没看懂~
                        alert("请填写联系人姓名");
                        $(this).focus();
                        res = false;
                        return false;
                    }
                });
                return res;
            }

            function checkmobile() {//验证手机号是否为空
                var res = true;
                $("#userlist").find("input.inmobile").each(function() {//对所有input标签中name为inmobile的进行遍历
                    if (!$.trim($(this).val())) {//同上
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
                                "<th height=\"30\" align=\"center\"><input  type=\"text\" name=\"inname\"  class=\"searchinput inname\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"indate\" class=\"searchinput indate\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inphone\" class=\"searchinput inphone\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inmobile\" class=\"searchinput inmobile\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inqq\" class=\"searchinput inqq\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inemail\" class=\"searchinput inemail\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"infax\" class=\"searchinput infax\" /></th>" +
                                "<td align=\"center\"><a href=\"javascript:;\" class=\"add\" ><img height=\"16\" width=\"15\" alt=\"\" src=\"/images/tianjiaicon01.gif\" /> 添加</a> <a href=\"javascript:;\" class=\"del\" ><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a></td>" +
                                "</tr>";
                return html;
            }
            function add() {//联系人添加按钮
                var html = getuserinput(); //生成html语句
                $("#userlist").append(html); //对table进行追加
                $("a.add").unbind().bind("click", function() {//删除a.add的绑定，进行重新绑定
                    add();
                    return false;
                });
                $("a.del").unbind().bind("click", function() {//删除a.del的绑定，进行重新绑定
                    var that = $(this); //获取本身
                    del(that); //删除本身
                    return false;
                });
            }


            function del(that) {
                var trlist = $("#userlist").find("tr"); //获取联系人人数
                if (trlist.length > 2) {//联系人人数大于2人
                    that.parent().parent().remove(); //删除本身
                } else {
                    that.parent().parent().find("input").val("");
                }
            }
            $("a.add").bind("click", function() {//绑定add
                add();
                return false;
            });
            $("a.del").bind("click", function() {//绑定del
                var that = $(this);
                del(that);
                return false;
            });

        });
    </script>

    </form>
</body>
</html>
