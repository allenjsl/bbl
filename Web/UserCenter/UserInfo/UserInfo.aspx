<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="Web.UserCenter.UserInfo.UserInfo" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<title>个人信息-个人中心</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
        <div class="mainbody">
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> 个人中心>> 个人信息</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:30px;">
                  
                </div>
           	  <div class="tablelist">
            	<table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">填写个人信息</th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>所属公司：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><% =this.SiteUserInfo.CompanyName %></label>           
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>所属部门：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><% =user.DepartName %></label>               
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>密码：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="password" value="<% =user.PassWordInfo.NoEncryptPassword %>" name="psd" />
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>姓名：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" size="30" name="name" value="<% =user.PersonInfo.ContactName %>" />
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>性别：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                          <input type="radio" name="sex" value="2" <% =user.PersonInfo.ContactSex == EyouSoft.Model.EnumType.CompanyStructure.Sex.男?"checked='checked'":"" %> />
                          男
                          <input type="radio" name="sex" value="1" <% =user.PersonInfo.ContactSex == EyouSoft.Model.EnumType.CompanyStructure.Sex.女?"checked='checked'":"" %> />
                          女
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>职位：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><% =user.PersonInfo.JobName %></label>
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>联系电话：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" size="40" name="phone" id="phone" value="<% =user.PersonInfo.ContactTel %>" valid="isPhone" errmsg="请填写正确的电话！" />
                        <span id="errMsg_phone" class="errmsg"></span>
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>传真：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input  type="text" size="40" name="fax" value="<% =user.PersonInfo.ContactFax %>" />
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>手机：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="mobile" id="mobile" type="text" size="40" value="<% =user.PersonInfo.ContactMobile %>" valid="isMobile" errmsg="请填写正确的手机！"  />
                        <span id="errMsg_mobile" class="errmsg"></span>
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>QQ：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="QQ" id="QQ" type="text" size="40" value="<% =user.PersonInfo.QQ %>" valid="isQQ" errmsg="请填写正确的QQ号码！" />
                        <span id="errMsg_QQ" class="errmsg"></span>
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>MSN：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="msn" value="<% =user.PersonInfo.MSN %>" type="text" size="40" />
                    </td>
				  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>E-mail：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="email" value="<% =user.PersonInfo.ContactEmail %>" type="text" size="40" id="email" valid="isEmail" errmsg="请填写正确的email！" />
                        <span id="errMsg_email" class="errmsg"></span>
                    </td>
				  </tr>
                  <tr>
                    <td height="30" colspan="3" align="center">
					<table border="0" align="center" cellpadding="0" cellspacing="0">
                     <tr>
                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="save">保存</a></td>
			            <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
                      </tr>
                      </table>
                    </td>
                  </tr>
              </table>
            </div>

			</div>
			<!-- InstanceEndEditable -->    
			        
		</div>
	</form>
	<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
	<script type="text/javascript">
	$(function(){
	    FV_onBlur.initValid($("a.save").closest("form").get(0));
	    
	    $("a.save").click(function(){
	        var form = $(this).closest("form").get(0);
	        if(ValiDatorForm.validator(form,"span")){
	            form.submit();
	        }
	        return false;
	    });
	});
	</script>
</asp:Content>
