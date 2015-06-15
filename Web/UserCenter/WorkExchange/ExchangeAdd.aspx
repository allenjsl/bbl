<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="ExchangeAdd.aspx.cs" Inherits="Web.UserCenter.UserInfo.ExchangeAdd"  ValidateRequest="false"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
        <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> <a href="#">个人中心</a>>> 工作交流</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:50px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,11) %>
                    </tr>
                  </table>
                </div>
           	  <div class="tablelist">
            	<table width="880" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">新增交流专区</th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><SPAN style="COLOR: red">*</SPAN><strong>标题：</strong></td>
                    
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="title" id="title" type="text" size="50" value="<% =exModel.Title %>" valid="required" errmsg="标题不能为空"  />
                        <span id="errMsg_title" class="errmsg"></span>
                    </td>
                    
                      
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>交流内容：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <textarea id="changecon" name="description" cols="5" rows="5" ><% =exModel.Description%></textarea>
                    </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>发布人：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><label><%=exModel.OperatorName %></label>
                     <input type="checkbox" name="isAnonymous" <%= exModel.IsAnonymous?"checked='checked'":"" %> /> 
                     匿名</td>
                  </tr>
				  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>发布时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><%= exModel.CreateTime.ToString("yyyy-MM-dd")%></label>
                    </td>
                  </tr>
                  <tr>
                    <td height="30" colspan="3" align="center">
					<table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <%switch (type)
                              {
                                  case "modify": %>
                                    <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="modify">修改</a></td>
			                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
			                        <%break;
                                  default:
                                       %>
                                    <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="save">保存</a></td>
                                    <%--<td width="158" height="40" align="center" class="jixusave"><a href="javascript:;" class="saveadd">保存并继续添加</a></td>--%>
			                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>  
			                        <%break;
                              } %>
                        </tr>
                        <input type="hidden" name="tid" value="<% =tid %>" />
                        <input type="hidden" name="continue" value="" id="continue" />
                    </table>
                    </td>
                  </tr>
              </table>
            </div>

			</div>
			<!-- InstanceEndEditable -->            
	    </div>
	    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>
	    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
	    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
	    <script type="text/javascript">
	    KE.init({
            id: 'changecon', //编辑器对应文本框id
            width: '650px',
            height: '360px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        
	    $(function(){
             KE.create('changecon', 0);
             $("input.time").focus(function(){
                WdatePicker()
             });
             FV_onBlur.initValid($("#continue").closest("form").get(0));
             $("a.save").bind("click",function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
             });
             
             $("a.saveadd").bind("click",function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
             });
             
             $("a.modify").bind("click",function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
             });
        });
        </script>
    </form>
</asp:Content>
