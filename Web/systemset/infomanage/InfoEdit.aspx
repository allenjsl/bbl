<%@ Page Title="信息编辑_信息管理_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="InfoEdit.aspx.cs" Inherits="Web.systemset.infomanage.InfoEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.errmsg{ color:Red;}
</style>
<script type="text/javascript" src="/js/kindeditor/kindeditor.js" cache="true" ></script>
 <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
<script type="text/javascript">
    //初始化编辑器

    KE.init({
    id: '<%=txtInfoContent.ClientID %>', //编辑器对应文本框id
        width: '680px',
        height: '360px',
        skinsPath: '/js/kindeditor/skins/',
        pluginsPath: '/js/kindeditor/plugins/',
        scriptPath: '/js/kindeditor/skins/',
        resizeMode: 0, //宽高不可变
        items: keSimple //功能模式(keMore:多功能,keSimple:简易)
    });
</script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> <a href="#">系统设置</a>>>信息管理</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div><div class="lineCategorybox" style="height:30px;">
                	
              </div>
              <div class="tablelist">
              <form runat="server" id="InfoFrom"   enctype="multipart/form-data">
              <input type="hidden" value="save" name="hidMethod" id="hidMethod"/>
              <input type="hidden" value="" name="hidDel" id="hidDel"/>
            	<table width="800" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                  <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4"><%=infoId==0?"添加信息":"修改信息" %></th>
                  </tr>
                  <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><span style="color:Red">*</span><strong>标题：</strong></td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input id="txtInfoTitle" runat="server" type="text"  size="50"   valid="required"  errmsg="标题不为空" />
                    <span id="errMsg_<%=txtInfoTitle.ClientID %>" class="errmsg"></span>
                    </td>
                  </tr>

                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><span style="color:Red">*</span><strong>发布对象：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
					<input  type="checkbox" name="chkPublishTo" value="cInner" <%=innerChecked %> /> 
					公司内部
                    <input  type="checkbox" name="chkPublishTo" value="sDepart"  <%=departChecked %>/> 
                    <a href="javascript:;" onClick="return InfoEdit.selDeaprt();">指定部门<img src="/images/icon_select.jpg" alt="" /></a>
                    <input type="hidden" id="txtDeparts" runat="server" />
                    <input  type="checkbox" name="chkPublishTo" value="cTour" <%=tourChecked %>/> 组团社
                    <span style="color:Red; display:none; margin-left:5px;" id="toMess">发布对象不为空</span>
                    </td>
                  </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>内容：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <textarea id="txtInfoContent"  name="txtInfoContent" style="width:680px;height:360px;" runat="server"></textarea>
                   </td>
                  </tr>
                   <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>添加附件：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input type="file" name="fileAttach" />
                   <%=GetAttchHtml()%>
                    </td>
                   </tr>
                  <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc"><strong>发布人：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtAuthor" runat="server" type="text"  readonly="readonly"  size="25" /></td>
                  </tr>
                   <tr>
                    <td height="35"  align="right" bgcolor="#e3f1fc"><strong>发布时间：</strong></td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3"><input id="txtPublishDate" runat="server" type="text"  onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" size="25" /></td>
                  </tr>
				  

                  <tr>
                    <td height="30" colspan="3" align="center">
					<table border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
		    
            <td width="137" height="20" align="center" class="tjbtn02"><a href="javascript:;" onclick="return InfoEdit.save('');">保存</a></td>
          
            <td width="137" height="20" align="center" class="tjbtn02"><a href="/systemset/infomanage/InfoList.aspx">返回</a></td>
          </tr>
          </table></td>
                  </tr>
              </table>
              </form>
            </div>

			</div>
			<script  type="text/javascript">
			    $(document).ready(function() {
			        setTimeout(function() {
			        KE.create('<%=txtInfoContent.ClientID %>', 0); //创建编辑器
			    }, 100); 
			});
			  $(document).ready(function() {
			    FV_onBlur.initValid($("#<%=InfoFrom.ClientID %>").get(0));
			  });
			var InfoEdit =
			    {
			        //打开弹窗
			        openDialog: function(p_url, p_title) {
			            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "750px", height: "250px" });
			        },
			        //指定部门
			        selDeaprt: function() {
			            InfoEdit.openDialog("/systemset/infomanage/SelectDepart.aspx?infoId=<%=infoId %>&dpids="+$("#<%=txtDeparts.ClientID %>").val(), '选择部门');
			            return false;
			        },
			        //删除附件
			        delAttach: function() {
			            $("#hidDel").val("delAttach"); //设置隐藏域为删除附件
			        },
			        //保存
			        save: function(method) {
			            var isValid = true;
			            var form = $("#<%=InfoFrom.ClientID %>").get(0);
			            isValid = ValiDatorForm.validator(form, "span");
			            if ($(":checked").length < 1) {

			                $("#toMess").css("display", ""); isValid = false;
			            }
			            else {
			                $("#toMess").css("display", "none");
			            }
			            if (!isValid) { return false; }
			            if (method == "continue") {
			                $("#hidMethod").val("continue");
			            }
			            $("#<%=InfoFrom.ClientID %>").submit();
			            return false;
			        },
			        //选择部门后回调
			        selDepartBack: function(dIds) {
			            $("#<%=txtDeparts.ClientID %>").val(dIds);
			        }
			    }
			</script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
