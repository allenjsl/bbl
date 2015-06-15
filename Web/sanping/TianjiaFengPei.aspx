<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TianjiaFengPei.aspx.cs" Inherits="Web.sanping.TianjiaFengPei" %>


<%@ Register src="../UserControl/UCSelectDepartment.ascx" tagname="UCSelectDepartment" tagprefix="uc1" %>
<%@ Register src="../UserControl/selectOperator.ascx" tagname="selectOperator" tagprefix="uc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
<script language="javascript" src="/js/jquery.js"></script>
<script type="text/javascript" src="/js/ValiDatorForm.js"></script>
</head>
<body>
    <form id="form1" runat="server">
<table width="500" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top:10px;">
  <tr class="odd">
    <th width="117" height="35" align="right"><font class="xinghao">*</font>分配类别：</th>
    <td width="283">
        <asp:TextBox ID="txt_Income" class="searchinput searchinput02"  errmsg="*请输入分配类别！" valid="required" runat="server"></asp:TextBox>
      </td>
  </tr>
  <tr class="even">
    <th width="117" height="35" align="right"><font class="xinghao">*</font>分配金额：</th>
    <td>
    <asp:TextBox ID="txt_IncomeMoney" class="searchinput" errmsg="*请输入分配金额！|请输入正确的格式!" valid="required|isMoney" runat="server"></asp:TextBox>
      </td>
  </tr>
  <tr class="odd">
    <th width="117" height="35" align="right">部门：</th>
    <td><input type="hidden" id ="hd_deptId" name="hd_deptId" value="" />
        <uc1:UCSelectDepartment ID="UCSelectDepartment1" runat="server" 
            SetPicture="/images/sanping_04.gif" />
      </td>
  </tr>
  <tr class="even">
    <th width="117" height="35" align="right">人员：</th>
    <td>
      <input type="hidden"  id="hd_userid" name="hd_userid"/>
        <uc2:selectOperator ID="selectOperator1" runat="server" />
      </td>
  </tr>
  <tr class="odd">
    <th width="117" height="40" align="right">备注：</th>
    <td>
    <textarea class="textareastyle02" id="txt_Remarks" name="txt_Remarks" runat="server"></textarea></td>
  </tr>
  <tr>
    <td height="30" colspan="2" align="center"><table width="200" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td height="40" align="center"></td>
        <td height="40" align="center" class="tjbtn02">
            <asp:LinkButton ID="lbtn_Submit" runat="server" onclick="lbtn_Submit_Click">保存</asp:LinkButton></td>
        <td height="40" align="center" class="tjbtn02"><a href="javascript:void(0);" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();return false;">
            取消</a></td>
      </tr>
    </table></td>
  </tr>
</table>
    </form>
    <script>
        //计算长度
        String.prototype.len = function() { return this.replace(/[^\x00-\xff]/g, "**").length; }

        $(function() {

            function FloatAdd(arg1, arg2) {
                var r1, r2, m;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2))
                return (arg1 * m + arg2 * m) / m
            }

            $("#<%=lbtn_Submit.ClientID %>").click(function() {
                var form = $(this).closest("form").get(0);
                if ($("#txt_Remarks").val().len() > 500) {
                    alert("备注长度不得超过500个字节!");
                    return false;
                }
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");

            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=lbtn_Submit.ClientID %>").closest("form").get(0));
        });
    </script>
</body>
</html>
