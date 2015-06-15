<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="WorkReportAdd.aspx.cs" Inherits="Web.UserCenter.UserInfo.WorkReportAdd" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
    <form id="form1" enctype="multipart/form-data" runat="server">
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
                      <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,9) %>
                    </tr>
                  </table>
                </div>
           	  <div class="tablelist">
            	<table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4"><% = type=="modify"?"修改":"新增" %>工作汇报</th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><SPAN style="COLOR: red">*</SPAN><strong>标题：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input name="title" type="text" id="intitle" size="50" value="<% =wrModel.Title %>" valid="required" errmsg="请填写标题" />
                        <span id="errMsg_intitle" class="errmsg"></span>
                    </td>
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>汇报内容：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <textarea id="reportcontent" style="width:650px; height:370px" rows="5" cols="5" name="description" ><% =wrModel.Description%></textarea>
                    </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>附件上传</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3 updom">
                     <%if (string.IsNullOrEmpty(wrModel.FilePath))
                      { %>
                        <input type="file" name="upfile" />
                    <%}
                      else
                      {
                     %>
                     <a href="<%=wrModel.FilePath %>" target="_blank">查看附件</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                     <%} %>
                    </td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>汇报部门：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                       <label><% =wrModel.DepartmentName %></label>
                    </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>汇报人</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><% =wrModel.OperatorName %></label>
                    </td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>汇报时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <label><% =wrModel.ReportingTime.ToString("yyyy-MM-dd") %></label>
                    </td>
                  </tr>
                  <%if (type=="modify"&&wrModel.OperatorId != SiteUserInfo.ID)
                    { %>
                   <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>状态：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                     <input type="radio" name="status" value="0" <% = Convert.ToInt32(wrModel.Status)==0?"checked='checked'":"" %>  />
                     未审核
                     <input type="radio" name="status" value="1" <% = Convert.ToInt32(wrModel.Status)==1?"checked='checked'":"" %> />
                     已审核</td>
                  </tr>
                  <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>评语：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <textarea name="comment" cols="80" rows="8" id="comment"><% =wrModel.Comment%></textarea>
                    </td>
                  </tr>
                  <%}%>
                  <tr>
                    <td height="30" colspan="3" align="center">
					<table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                        <%switch (type)
                          {
                              case "modify": %>
                                <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="modify">修改</a></td>
                                <%if(wrModel.OperatorId!=SiteUserInfo.ID){ %><td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="check">审核</a></td><%} %>
		                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
		                        <%break;
                              default:
                                   %>
                                <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="save">保存</a></td>
                                <%--<td width="158" height="40" align="center" class="jixusave"><a href="javascript:;" class="saveadd">保存并继续添加</a></td>--%>
		                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>  
		                        <%break;
                          } %>
			            <input type="hidden" value="<% =tid %>" name="tid" id="tid" />
			            <input type="hidden" name="continue" value="" id="continue" />
                        </tr>
                    </table>
                    </td>
                  </tr>
              </table>
            </div>

			</div>
			<!-- InstanceEndEditable -->   
			</form>         
	</div>
	<script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>
	<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
	<script type="text/javascript" src="/js/ValiDatorForm.js"></script>
	
	<script type="text/javascript">
       $(function(){
             KE.init({
                id: 'reportcontent', //编辑器对应文本框id
                width: '650px',
                height: '360px',
                skinsPath: '/js/kindeditor/skins/',
                pluginsPath: '/js/kindeditor/plugins/',
                scriptPath: '/js/kindeditor/skins/',
                resizeMode: 0, //宽高不可变
                items: keMore //功能模式(keMore:多功能,keSimple:简易)
            });
            KE.create('reportcontent', 0); //创建编辑器
             
             FV_onBlur.initValid($("#tid").closest("form").get(0));
             $("#reporttime").focus(function(){
                WdatePicker()
             });
             
             $("a.check").bind("click",function(){
                var tid = $("#tid").val();
                var sta = $("input[name='status']:checked").val();
                var comment = $("#comment").val();
                $.newAjax({
	                type: "POST",
	                dataType: "json",
	                url: "/UserCenter/WorkExchange/WorkReportAdd.aspx?type=check",
	                data: {"tid":tid,"status":sta,"comment":comment},
	                cache: false,
	                success: function(d) {
	                    switch(d.res){
	                        case 1:
	                            alert("审核成功!");
	                            location.href = "/UserCenter/WorkExchange/WorkReport.aspx";
	                            break;
	                        case -1:
	                            alert("审核失败!");
	                            break;
	                    }
	                }
	            });
                
                return false;
             });
             
             $("a.save").bind("click",function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
             });
             
             $("a.saveadd").bind("click",function(){
                $("#continue").val("continue");
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
