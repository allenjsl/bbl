<%@ Page Title="公司信息_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="Web.systemset.CompanyInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
<style>
.errmsg{ color:Red;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;"> 所在位置：系统设置>> 公司信息</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div><div class="lineCategorybox" style="height:30px;">
                	
              </div>
              
             
          <form runat="server" id="companyFrom">
            	<div class="tablelist">
                <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
            	<table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">填写公司信息</th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><span style="color:Red;font-size:12px;">*</span><strong>公司名称：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                     <input id="txtCompanyName" runat="server" type="text" size="50" valid="required"  errmsg="公司名称不为空" />
                     <span id="errMsg_<%=txtCompanyName.ClientID %>" class="errmsg" ></span>
                    </td>
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>旅行社类别：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtType" runat="server" type="text" size="50" /></td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>公司英文名称：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtEngName" runat="server" type="text" size="50" /></td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>许可证号：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtLicence" runat="server" type="text" size="50" /></td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>公司负责人：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtAdmin" runat="server" type="text" size="50" /></td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>电话：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtTel" runat="server" type="text" size="50" valid="isPhone"  errmsg="格式不正确" />
                      <span id="errMsg_<%=txtTel.ClientID %>" class="errmsg" ></span>
                    </td>
                  </tr>
                   <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>手机：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtMoible" runat="server" type="text" size="50" valid="isMobile"  errmsg="格式不正确" />
                    <span id="errMsg_<%=txtMoible.ClientID %>" class="errmsg" ></span>
                    </td>
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>传真：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtFax" runat="server" type="text" size="50" /></td>
                  </tr>
                   <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>地址：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtAddress" runat="server" type="text" size="90" /></td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>邮编：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtEmail" runat="server" type="text" size="50" /></td>
                  </tr>
				  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>公司网站：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtWeb" runat="server" type="text" size="50" /></td>
                  </tr>
				  <tr>
                    <td height="35" rowspan="3" align="right" bgcolor="#e3f1fc"><strong>公司帐号：</strong></td>
                    <td width="9%" height="17" align="right" bgcolor="#FAFDFF" class="pandl3">开户行：                    </td>
                    <td width="75%" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtBank" type="text" size="50"  runat="server"/></td>
				  </tr>
				  <tr>
				    <td height="17" align="right" bgcolor="#FAFDFF" class="pandl3">户名：                      </td>
			        <td height="17" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtUserName" runat="server" type="text" size="50" /></td>
				  </tr>
				  <tr>
				    <td height="35" align="right" bgcolor="#FAFDFF" class="pandl3">账号：                      </td>
			        <td height="35" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtUserNo" runat="server" type="text" size="50" /></td>
				  </tr>
				 
				  

                  <tr>
                    <td height="30" colspan="3" align="center">
					<table  border="0" align="left" cellpadding="0" cellspacing="0">
         <tr>
		    <td width="114" height="40" align="center"></td>
            <td width="84" height="40" align="center" class="tjbtn02"><a href="javascript:;" id="btn_save" onclick="return save();">保存</a></td>
            <td width="186" height="40" align="center" class="tjbtn02"></td>
          </tr>
          </table></td>
                  </tr>
              </table>
            </div>
            </form>

			</div>
	    <script type="text/javascript">
	        $(document).ready(function() {
	            FV_onBlur.initValid($("#btn_save").closest("form").get(0));
	        });
	
        //保存表单
        function save(method) {
            var form = $("#btn_save").closest("form").get(0);
            var vResult = ValiDatorForm.validator(form, "span");
            if (!vResult) return false;
            form.submit();
            return false;
        }
   
	    </script>
</asp:Content>
