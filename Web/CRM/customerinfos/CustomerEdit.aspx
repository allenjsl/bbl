<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="Web.CRM.customerinfos.CustomerEdit" %>

<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="cEditForm" runat="server">
    <div>
        <input type="hidden" name="hidMethod" value="save" />
        <input type="hidden" name="delIds" value="" id="delIds" />
        <table width="900" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="100" height="30" align="center">
                    省份：
                </th>
                <td width="310" align="left">
                    <uc1:ucProvince ID="ucProvince1" runat="server" />
                    <input type="hidden" id="pName" name="pName" />
                </td>
                <th width="100" align="center">
                    城市：
                </th>
                <td width="320" align="left">
                    <uc2:ucCity ID="ucCity1" runat="server" />
                    <input type="hidden" id="cName" name="cName" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    <span style="color: Red">*</span>单位名称：
                </th>
                <td align="left">
                    <input type="text" id="txtCompany" runat="server" class="searchinput searchinput02"
                        valid="required" errmsg="单位不为空" />
                    <span id="errMsg_<%=txtCompany.ClientID %>" class="errmsg"></span>
                </td>
                <th align="center">
                    许可证号：
                </th>
                <td align="left">
                    <input id="txtLicense" runat="server" type="text" class="searchinput" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    地址：
                </th>
                <td align="left">
                    <input type="text" id="txtAddress" runat="server" class="searchinput searchinput02" />
                </td>
                <th align="center">
                    邮编：
                </th>
                <td align="left">
                    <input type="text" class="searchinput" id="txtPostalCode" runat="server" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    银行账号：
                </th>
                <td align="left">
                    <input type="text" id="txtBankCode" runat="server" class="searchinput searchinput02" />
                </td>
                <th align="center">
                    预存款：
                </th>
                <td align="left">
                    <input type="text" class="searchinput" id="txtBeforeMoney" runat="server" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    最高欠款额度：
                </th>
                <td align="left">
                    <input type="text" class="searchinput" id="txtArrears" runat="server" />
                </td>
                <th align="center">
                    返佣类型：
                </th>
                <td align="left">
                    <asp:RadioButtonList ID="rdiBackType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="现返" Value="1" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="后返" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    返佣金额：
                </th>
                <td align="left">
                    <input type="text" class="searchinput" id="txtBackMoney" runat="server" />
                </td>
                <th align="center">
                    <span style="color: Red">*</span>责任销售：
                </th>
                <td align="left">
                    <select id="selDutySaler" runat="server" valid="required" errmsg="责任销售不为空">
                    </select><input type="hidden" id="hidSaler" name="hidSaler" />
                    <span id="errMsg_<%=selDutySaler.ClientID %>" class="errmsg"></span>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    <span style="color: Red">*</span>客户等级：
                </th>
                <td align="left">
                    <select id="selCustLevel" runat="server" valid="required" errmsg="客户等级不为空">
                    </select>
                    <span id="errMsg_<%=selCustLevel.ClientID %>" class="errmsg"></span>
                </td>
                <th height="30" align="center">
                    品牌名称：
                </th>
                <td align="left">
                    <select id="selBrand" runat="server">
                    </select>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    结算日期：
                </th>
                <td align="left" colspan="3">
                    <input type="radio" name="AccountTypeRadio" value="1" runat="server" checked id="AccountTypeRadio1" />
                    <label for="AccountTypeRadio1" id="label1" style="cursor: pointer;">
                        按月</label>
                    &nbsp<asp:DropDownList ID="AccountDaylist" runat="server">
                    </asp:DropDownList>
                    &nbsp
                    <input type="radio" name="AccountTypeRadio" value="2" runat="server" id="AccountTypeRadio2" />
                    <label for="AccountTypeRadio2" id="label2" style="cursor: pointer;">
                        按周</label>
                    &nbsp &nbsp<input type="radio" name="checkDay" value="1" runat="server" checked id="checkDay1" />
                    <label for="checkDay1" id="labelweek1" style="cursor: pointer;">
                        星期一</label>
                    &nbsp<input type="radio" name="checkDay" value="2" runat="server" id="checkDay2" />
                    <label for="checkDay2" id="labelweek2" style="cursor: pointer;">
                        星期二</label>
                    &nbsp<input type="radio" name="checkDay" value="3" runat="server" id="checkDay3" />
                    <label for="checkDay3" id="labelweek3" style="cursor: pointer;">
                        星期三</label>
                    &nbsp<input type="radio" name="checkDay" value="4" runat="server" id="checkDay4" />
                    <label for="checkDay4" id="labelweek4" style="cursor: pointer;">
                        星期四</label>
                    &nbsp<input type="radio" name="checkDay" value="5" runat="server" id="checkDay5" />
                    <label for="checkDay5" id="labelweek5" style="cursor: pointer;">
                        星期五</label>
                    &nbsp<input type="radio" name="checkDay" value="6" runat="server" id="checkDay6" />
                    <label for="checkDay6" id="labelweek6" style="cursor: pointer;">
                        星期六</label>
                    &nbsp<input type="radio" name="checkDay" value="7" runat="server" id="checkDay7" />
                    <label for="checkDay7" id="labelweek7" style="cursor: pointer;">
                        星期日</label>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    结算方式：
                </th>
                <td align="left" colspan="3">
                    <input type="text" class="searchinput" id="AccountType" runat="server" style="width: 120px;" />
                </td>
            </tr>
            <tr class="odd">
                <th width="100" height="30" align="center">
                    结算方式：
                </th>
                <td width="310" align="left">
                    <asp:DropDownList ID="ddl_checkType" runat="server">
                        <asp:ListItem Value="0">请选择</asp:ListItem>
                        <asp:ListItem Value="1">单团结算</asp:ListItem>
                        <asp:ListItem Value="2">月结</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <th width="100" align="center">
                    对方团号必填：
                </th>
                <td width="320" align="left">
                    <asp:RadioButtonList runat="server" ID="rad_IsShowGroupNo" RepeatDirection="Horizontal">
                        <asp:ListItem Value="true">是</asp:ListItem>
                        <asp:ListItem Value="false">否</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    主要联系人：
                </th>
                <td colspan="3" align="left">
                    <input type="text" class="searchinput" id="txtMainContact" runat="server" />
                    <b>电话：</b>
                    <input type="text" class="searchinput" id="txtMainTel" runat="server" />
                    <b>手机：</b>
                    <input id="txtMainMobile" runat="server" type="text" class="searchinput" />
                    <b>传真：</b>
                    <input type="text" class="searchinput" id="txtMainFax" runat="server" />
                </td>
            </tr>
            <tr class="even">
                <th align="center">
                    联系人信息：
                </th>
                <td colspan="3" align="left" id="tdContact">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF"
                        id="tableC">
                        <tr class="odd">
                            <th width="10%" height="30" align="center">
                                姓名
                            </th>
                            <th width="6%" align="center">
                                性别
                            </th>
                            <th width="11%" align="center">
                                生日
                            </th>
                            <th width="11%" align="center">
                                部门/职务
                            </th>
                            <th width="14%" align="center">
                                手机
                            </th>
                            <th width="14%" align="center">
                                电话
                            </th>
                            <th width="13%" align="center">
                                传真
                            </th>
                            <th width="13%" align="center">
                                QQ
                            </th>
                            <td width="19%" align="center">
                                操作<br />
                                <nobr><a href="javascript:;" onclick="CustEdit.add();return false;">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
                                    添加</a></nobr>
                            </td>
                        </tr>
                        <%if (IsStartState)
                          { %>
                        <tr class="even" style="display: none;" id="tr1">
                            <th height="30" align="center">
                                <input name="txtContact" type="text" class="searchinput" /><input type='hidden' name='contactId' />
                            </th>
                            <th align="center">
                                <select name="selSex">
                                    <option value="0">男</option>
                                    <option value="1">女</option>
                                </select>
                            </th>
                            <th align="center">
                                <input name="txtBirth" type="text" class="searchinput" onfocus="WdatePicker()" />
                            </th>
                            <td align="center">
                                部门:
                                <input name="txtDepart" type="text" class="jiesuantext" />
                                <br />
                                职务:
                                <input name="txtDuty" type="text" class="jiesuantext" />
                            </td>
                            <th align="center">
                                <input name="txtMobile" type="text" class="searchinput" style="width: 95%" />
                            </th>
                            <th align="center">
                                <input name="txtTel" type="text" class="searchinput" style="width: 95%" />
                            </th>
                            <th align="center">
                                <input name="txtFax" type="text" class="searchinput" />
                            </th>
                            <th align="center">
                                <input name="txtQQ" type="text" class="searchinput" />
                            </th>
                            <td align="center">
                                <a onclick="return CustEdit.del(this);return false;" href="javascript:;">
                                    <img width="14" height="14" src="/images/delicon01.gif">
                                    删除</a><%if (hasAccountPermit)
                                            { %><br />
                                <a href="javascript:;" onclick="return CustEdit.setCode('',this)">分配账号</a><%} %><input
                                    type="hidden" name="hidCodeInfo" value="" />
                            </td>
                        </tr>
                        <%} %>
                        <tr class="even" style="display: none" name='cinfo'>
                            <th height="40" align="center" colspan="9">
                                用户名：<input type="text" name="txtUserName" style="width: 80px" /><input type='hidden'
                                    value='0' name='userId' />密码：<input type="password" name="txtPwd" style="width: 90px" />
                                确认密码：<input type="password" name="txtPwdSure" style="width: 90px" />线路区域：<input type="text"
                                    name="txtRouteArea" style="width: 120px" readonly="readonly" /><input type="hidden"
                                        name="custHidArea" value="|" />
                                <a href="javascript:;" onclick="return CustEdit.selArea('',this);">
                                    <img src="/images/sanping_04.gif" width="28" height="18" /></a>
                            </th>
                        </tr>
                        <%=strContactHtml%>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th height="60" align="center">
                    备注：
                </th>
                <td colspan="3">
                    <textarea id="txtRemark" cols="45" rows="5" runat="server" class="textareastyle"></textarea>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <%if (!isshow)
                  { %>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" onclick="return CustEdit.save();">保存</a>
                </td>
                <%} %>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script type="text/javascript">
    
       var trId=<%=contactNum %>;//联系人数
        $(document).ready(function() {
           FV_onBlur.initValid($("#<%=cEditForm.ClientID%>").get(0));
           var isShow = <%=isshow?"1":"0" %>;
           if(isShow == "1")
            {
                $("input").each(function(){
                    $(this).attr("disabled","disabled").css("color","#000");
                });
                $("select").each(function(){
                    $(this).attr("disabled","disabled").css("color","#000");
                });
                $("textarea").each(function(){
                    $(this).attr("disabled","disabled").css("color","#000");
                });
                $("#tableC tr>td:last-child").remove();
            }
        });
        var CustEdit =
       {
           openDialog: function(p_url, p_title, p_width, p_height) {
               window.parent.Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height, data: { desid: '<%=Request.QueryString["iframeId"] %>'} });
           },
           //添加联系人信息
           add: function() {//创建新联系人
               trId++;
               var cHtml = "<tr class=\"even\" id=\"tr" + trId + "\"><th height=\"30\" align=\"center\"><input name=\"txtContact\" type=\"text\"  class=\"searchinput\" /><input type='hidden'  name='contactId'/></th><th align=\"center\"><select name=\"selSex\"><option value=\"0\">男</option><option value=\"1\">女</option></select></th><th align=\"center\"><input name=\"txtBirth\" type=\"text\"  class=\"searchinput\" onfocus=\"WdatePicker();\"/></th><td align=\"center\">部门:<input name=\"txtDepart\" type=\"text\"  class=\"jiesuantext\" /><br />职务:<input name=\"txtDuty\" type=\"text\"  class=\"jiesuantext\" /></td><th align=\"center\"><input name=\"txtMobile\" type=\"text\"  class=\"searchinput\" /></th><th align=\"center\"><input name=\"txtTel\" type=\"text\"  class=\"searchinput\" /></th><th align=\"center\"><input name=\"txtFax\" type=\"text\"  class=\"searchinput\" /></th><th align=\"center\"><input name=\"txtQQ\" type=\"text\"  class=\"searchinput\" /></th><td align=\"center\"><a href=\"javascript:;\" onclick=\"return CustEdit.del(this);return false;\"><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a>&nbsp;<br/><a href=\"javascript:;\" onclick=\"return CustEdit.setCode('trId',this);\">分配账号</a><input type=\"hidden\" name=\"hidCodeInfo\" value=\"\" /></td></tr><tr class=\"even\" style=\"display:none\" name='cinfo'><th height=\"40\" align=\"center\" colspan=\"9\" >用户名：<input type=\"text\" name=\"txtUserName\" style=\"width:80px\" /><input type='hidden' value='0' name='userId'/>密码：<input  type=\"password\" name=\"txtPwd\" style=\"width:90px\"/>确认密码：<input type=\"password\" name=\"txtPwdSure\" style=\"width:90px\"/>线路区域：<input type=\"text\" name=\"txtRouteArea\" style=\"width:120px\" readonly='readonly'/><input type=\"hidden\" name=\"custHidArea\" value=\"|\" />&nbsp;<a href=\"javascript:;\" onclick=\"return CustEdit.selArea('',this);\"><img src=\"/images/sanping_04.gif\" width=\"28\" height=\"18\" /></a></th></tr>";
               $("#tableC").append(cHtml);
               $("#tdContact").attr("colspan", parseInt($("#tdContact").attr("colspan")) + 2);
           },

           //删除联系人信息
           del: function(tar) {
               var closeTr = $(tar).closest("tr");
               if (closeTr.siblings("tr:visible").not("[name='cinfo']").length <= 1) {
                   alert("请至少保留一行！");
                   return false;
               }
               $("#delIds").val($("#delIds").val() + "," + closeTr.find("input[name='contactId']").val());
               closeTr.next("tr").remove();
               closeTr.remove();
           }, //弹出区域选择框
           selArea: function(custid, tar) {
               CustEdit.openDialog("/CRM/customerinfos/SelRouteArea.aspx?custid=" + custid, "选择线路区域", "500px", "300px");
               CustEdit.hidArea = $(tar).prev("input");
           }, //现则区域后回调
           selAreaBack: function(areaids, areaNames) {
               CustEdit.hidArea.val(areaids + "|");
               CustEdit.hidArea.prev().val(areaNames);

           },
           hidArea: {},
           //分配账号(联系人ID)
           setCode: function(cId, tar) {
               $(tar).closest("tr").next("tr").show();
               $(tar).closest("td").attr("rowspan", "2");
           }, //保存客户
           save: function() {
               var formObj = $("#<%=cEditForm.ClientID%>").get(0);
               var vResult = ValiDatorForm.validator(formObj, "span");
               
               if (vResult) {                                
                   var mobiles = [];
                   $("input[name='txtMobile']").each(function() {
                       var m = $(this).val();
                       if (m != "" && !/^(13|15|18|14)\d{9}$/.test(m)) {
                           mobiles.push(m);
                       }

                   });
                   var pass = [];
                   var trIndex = 1;
                   $("tr[name='cinfo']").each(function() {
                       var tr1 = $(this);
                       if (tr1.find("input[name='txtPwd']").val() != tr1.find("input[name='txtPwdSure']").val())
                       { pass.push(trIndex); }
                       trIndex++;
                   });
                   var mess = "";
                   if (mobiles.length != 0) {
                       mess += mobiles.toString() + " 手机格式不正确！\n";
                   }
                   if (pass.length != 0) {
                       mess += "第" + pass.toString() + " 行的密码与确认密码不一致！\n";
                   }
                   var userNameArr = [];
                   var rowNum = 0;
                   var userContact = true; //姓名是否验证通过
                   $("input[name='txtContact']").each(function() {
                       if (rowNum != 0) {
                           if ($.trim($(this).val()) == "") { userContact = false; }
                       }
                       else {
                           if ($.trim($("input[name='txtUserName']:first").val()) != "" && $.trim($(this).val()) == "")
                           { userContact = false; }
                       }
                       rowNum++;
                   });
                   if (!userContact) {
                       mess += "请填写完整姓名！\n";
                   }
                   $("input[name='txtUserName']").each(function() {
                       if ($(this).val() != "") {

                           userNameArr.push($(this).val());
                       }
                   });
                   var b = [];
                   var repeatM = [];
                   for (var i = 0, len = userNameArr.length; i < len; i++) {
                       if (b[userNameArr[i]] == null) {
                           b[userNameArr[i]] = i + 1;
                       }
                       else {
                           repeatM.push(userNameArr[i]); //重复号码压入重复数组
                           userNameArr[i] = null; //设置当前为null
                       }
                   }
                   if (repeatM.length != 0) {
                       mess += repeatM.toString() + "用户名重复！";
                   }
                   if (mess != "") {
                       alert(mess);
                       return false;
                   }

                   $("#pName").val($("#uc_provinceDynamic").find("option:selected").html());
                   $("#cName").val($("#uc_cityDynamic").find("option:selected").html());
                   $("input[name='txtRouteArea']").each(function() {
                       $(this).val($(this).val() + "|");
                   });
                   $("#hidSaler").val($("#<%=selDutySaler.ClientID%>").find("option:selected").html());
                   formObj.submit();
                   return false;
               }
           }
       }
        $(function() {
            if ($("#tdContact").find(".even:visible").length == 0) {
                $("#tr1").show();
            };
            $("input[type='radio'][name='AccountTypeRadio']").each(function() {
                if ($("input[type='radio'][name='AccountTypeRadio'][checked]").val() == "1") {
                    $("input[type='radio'][name='checkDay']").attr("disabled", "disabled");
                }
                $(this).click(function() {
                    if ($(this).val() == "1") {
                        $("input[type='radio'][name='checkDay']").attr("disabled", "disabled");
                        $("#<%=AccountDaylist.ClientID %>").attr("disabled", "");
                    }
                    else {
                        $("#<%=AccountDaylist.ClientID %>").attr("disabled", "disabled");
                        $("input[type='radio'][name='checkDay']").attr("disabled", "");
                    }
                });
            });

        });
    </script>

</body>
</html>
