<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="Web.systemset.organize.EmployeeEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>部门人员编辑_组织机构_系统设置</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
       .errmsg{ color:Red;}
      </style>
      <script src="/js/jquery.js" type="text/javascript"></script>
      <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
     <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="10%" height="30" align="right"><span  style="color:red">*</span>用户名：</th>
        <td width="15%" bgcolor="#E3F1FC"> <input type="text" class="xtinput" id="txtUserName" maxlength="15"  size="15" runat="server" valid="required"  errmsg="用户名不为空"/>
        <span id="errMsg_<%=txtUserName.ClientID %>" class="errmsg" ></span>
        </td>
        <th width="10%" height="30" align="right"><span  style="color:red">*</span>密码：</th>
        <td width="15%" bgcolor="#E3F1FC"><input  type="password" class="xtinput" id="txtPass" size="15" runat="server" valid="required"  errmsg="密码不为空"/>
        <span id="errMsg_<%=txtPass.ClientID %>" class="errmsg" ></span>
        </td>
        <th width="10%" height="30" align="right" ><span  style="color:red">*</span>姓名：</th>
        <td width="15%" bgcolor="#E3F1FC"><input  type="text" runat="server" class="xtinput" id="txtName" size="15" maxlength="15" valid="required"  errmsg="姓名不为空"/>
        <span id="errMsg_<%=txtName.ClientID %>" class="errmsg"  ></span>
        </td>
        <th width="10%" height="30" align="right">性别：</th>
        <td width="15%" bgcolor="#E3F1FC">
        <asp:RadioButtonList ID="rdiSex" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem Text="男" Value="2" Selected="True"></asp:ListItem>
        <asp:ListItem Text="女" Value="1"></asp:ListItem>
        </asp:RadioButtonList>
       
      </tr>
	  <tr class="odd">
        <th width="10%" height="30" align="right"><span  style="color:red">*</span>所属部门：</th>
        <td width="15%" bgcolor="#E3F1FC"><select id="selBdepart" runat="server" valid="required"  errmsg="所属部门不为空"></select>
         <span id="errMsg_<%=selBdepart.ClientID %>" class="errmsg" style="display:block"  ></span><input type="hidden" name="selBName" id="selBName" />
        </td>
        <th width="10%" height="30" align="right">兼管部门：</th>
        <td width="15%" bgcolor="#E3F1FC"><select id="selMdepart"  runat="server" ></select>
         <input type="hidden" name="selMName" id="selMName" />
        </td>
        <th width="10%" height="30" align="right">电话：</th>
        <td width="15%" bgcolor="#E3F1FC"><input type="text" class="xtinput" runat="server" id="txtTel" size="15"  maxlength="15"/></td>
        <th width="10%" height="30" align="right">传真：</th>
        <td width="15%" bgcolor="#E3F1FC"><input  type="text" class="xtinput"  runat="server" id="txtFax" size="15"  maxlength="30"/></td>
      </tr>
	  <tr class="odd">
        <th width="10%" height="30" align="right" ><span  style="color:red">*</span>手机：</th>
        <td width="15%" bgcolor="#E3F1FC"> 
        <input type="text" class="xtinput" id="txtMoible" runat="server" size="15" valid="required|isMobile"  errmsg="手机不为空|格式不正确"/>
        <span id="errMsg_<%=txtMoible.ClientID %>" class="errmsg"  ></span>
        </td>
        <th width="10%" height="30" align="right">QQ：</th>
        <td width="15%" bgcolor="#E3F1FC"><input  type="text" runat="server" class="xtinput" id="txtQQ" size="15"  maxlength="12"/></td>
        <th width="10%" height="30" align="right">MSN：</th>
        <td height="30" bgcolor="#E3F1FC"><input  type="text" runat="server" class="xtinput" id="txtMSN" size="18"  maxlength="30"/></td>
        <th width="10%" height="30" align="right">E-mail：</th>
        <td height="30" bgcolor="#E3F1FC"><input  type="text" runat="server" class="xtinput" id="txtEmail" size="18"  maxlength="30"/></td>
	  </tr>
	    <tr class="odd">
        <th width="10%" height="30" align="right">职位：</th>
        <td style="padding:4px 0px;" height="30" colspan="7" bgcolor="#E3F1FC" >
        <input type="text" runat="server" id="txtDuty" class="xtinput" maxlength="30" size="15" /></td>
      </tr>
	  <tr class="odd">
        <th width="10%" height="30" align="right">简介：</th>
        <td style="padding:4px 0px;" height="30" colspan="7" bgcolor="#E3F1FC">
        <textarea  cols="80" rows="5" id="txtIntroduce" runat="server"  ></textarea></td>
      </tr>
	  <tr class="odd">
        <th width="10%" height="30" align="right">备注：</th>
        <td style="padding:4px 0px;" height="30" colspan="7" bgcolor="#E3F1FC">
        <textarea  cols="80" rows="5" id="txtRemark" runat="server"></textarea></td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
          <table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
              <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" id="btn_save" onclick="return Ee.save('');">保存</a></td>
            <td width="158" height="40" align="center" ><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table>        </td>
      </tr>
</table>
    </form>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%=txtPass.ClientID %>").val("<%=pass %>");
            FV_onBlur.initValid($("#btn_save").closest("form").get(0));
        });
    var Ee =
    {
        //保存表单
        save: function(method) {
            var form = $("#btn_save").closest("form").get(0);
            var vResult = ValiDatorForm.validator(form, "span");
            if (!vResult) return false;
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            $("#selBName").val($("#<%=selBdepart.ClientID %>").find("option:selected").html());
            $("#selMName").val($("#<%=selMdepart.ClientID %>").find("option:selected").html());

            $.newAjax(
                      {
                          url: "/systemset/organize/EmployeeEdit.aspx?empId=<%=empId %>&copy=<%=method2%>",
                          data: { hidMethod: "isexist", uName: $("#<%=txtUserName.ClientID %>").val() },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          async:false,
                          success: function(result) {
                              if (result.success == "1") {
                                  if (result.message == "isExist") {
                                      alert("该用户名已经存在！");
                                      vResult = false;
                                  }
                              }
                              else {
                                  alert("保存失败！");
                              }
                          },
                          error: function() {
                              alert("授权时发生未知错误！");
                          }
                      })
            if (!vResult) return false;
            form.submit();
            return false;
        }
    }
</script>
</body>
</html>
