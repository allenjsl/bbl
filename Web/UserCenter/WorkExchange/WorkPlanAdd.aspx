<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="WorkPlanAdd.aspx.cs" Inherits="Web.UserCenter.UserInfo.WorkPlanAdd"  ValidateRequest="false" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagPrefix="cc1" TagName="SDepartment" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagPrefix="cc1" TagName="SOperator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<form runat="server" id="form1" enctype="multipart/form-data">
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> 个人中心>> 工作交流</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:50px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,10) %>
                    </tr>
                  </table>
                </div>
           	  <div class="tablelist">
            	<table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">新增工作计划</th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>编号：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="num" type="text" size="50" value="<%= wpModel.PlanNO %>" />
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>提交人：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><% =wpModel.OperatorName%></label>
                    </td>
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>接收人：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <cc1:SOperator runat="server" ID="SOperator1" Title="指定人员" IsShowText="false" IsShowTextLabel="true" /> 
                    </td>   
                  </tr>
				  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><SPAN style="COLOR: red">*</SPAN><strong>计划标题：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="title" id="title" type="text" size="50" value="<% =wpModel.Title %>" valid="required" errmsg="标题不能为空"  />
                        <span id="errMsg_title" class="errmsg"></span>
                    </td>
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>计划内容：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <textarea rows="5" cols="5" id="txtplancon" name="description" >
                        <% =wpModel.Description%>
                    </textarea>
                    </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>附件上传</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3 updom">
                    <%if (string.IsNullOrEmpty(wpModel.FilePath))
                      { %>
                        <input type="file" name="upfile" />
                    <%}
                      else
                      {
                     %>
                     <a href="<%=wpModel.FilePath%>" target="_blank">查看附件</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                     <%} %>
                     </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>计划说明：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <textarea name="remark" cols="80" rows="8"><% =wpModel.Remark%></textarea>
                    </td>
                   </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>预计完成时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="expectedDate" class="time" id="plantime"  readonly="readonly" type="text" size="30" value="<%= wpModel.ExpectedDate.ToString("yyyy-MM-dd") %>" />
                    </td>
                  </tr>
				  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>实际完成时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="actualDate" class="time" id="realtime" readonly="readonly" type="text" size="30" value="<%= wpModel.ActualDate.ToString("yyyy-MM-dd") %>" />
                    </td>
                  </tr>
                   <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>状态：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input class="rad" type="radio" name="status" value="1" <%= wpModel.Status==EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.进行中?"checked='checked'":"" %> />
                     进行中
                     <input class="rad" type="radio" name="status" value="2" <%= wpModel.Status==EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.取消?"checked='checked'":"" %> />
                     取消
                     <input class="rad" type="radio" name="status" value="3" <%= wpModel.Status==EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.未完成?"checked='checked'":"" %> />
                     未完成
                    <input class="rad" type="radio" name="status" value="4" <%= wpModel.Status==EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.效果不理想完成?"checked='checked'":"" %> />
                     效果不理想完成
                    <input class="rad" type="radio" name="status" value="5" <%= wpModel.Status==EyouSoft.Model.EnumType.PersonalCenterStructure.PlanCheckState.效果理想完成?"checked='checked'":"" %> />
                     效果理想完成
                    </td>
                  </tr>
                  <%if (isDirector)
                    { %>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>上级部门评语：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <textarea name="departmentComment" cols="80" rows="8"><% =wpModel.DepartmentComment%></textarea>
                    </td>
                  </tr>
                  <%} if (isManager)
                    { %>
				  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>总经理评语：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <textarea name="managerComment" cols="80" rows="8"><% =wpModel.ManagerComment%></textarea>
                    </td>
                  </tr>
                  <%} %>
				  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>填写时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= wpModel.CreateTime.ToString("yyyy-MM-dd")%></label> 
                    </td>
                  </tr>
				  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>最后修改时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= wpModel.LastTime == DateTime.MaxValue ? "" : wpModel.LastTime.ToString("yyyy-MM-dd")%></label>
                    </td>
                  </tr>

                  <tr>
                    <td height="30" colspan="3" align="center">
					<table border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
            <%if (type != "modify")
              { %>
            <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="save">保存</a></td>
            <%--<td width="158" height="40" align="center" class="jixusave"><a href="javascript:;" class="saveadd">保存并继续添加</a></td>--%>
			<td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
			<%}
              else
              { %>
			<td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="modify">修改</a></td>
			<td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
			
			<%} %>
			<input type="hidden" value="<%=tid %>" name="tid" />
			<input type="hidden" name="continue" value="" id="continue" />
          </tr>
          </table></td>
                  </tr>
              </table>
            </div>

			</div>
			<!-- InstanceEndEditable -->            
		</div>
	</form>
		<script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>
	    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
	    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
	    <script type="text/javascript">
	    KE.init({
            id: 'txtplancon', //编辑器对应文本框id
            width: '650px',
            height: '360px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        
        $(function(){
             KE.create('txtplancon', 0); //创建编辑器
             FV_onBlur.initValid($("#continue").closest("form").get(0));
             $("input.time").focus(function(){
                WdatePicker()
             }); 
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
             $("img.close").click(function(){
                var that = $(this);
                $("td.updom").html("<input name=\"upfile\" type=\"file\" />");
            });
        });
	    </script>
</asp:Content>
