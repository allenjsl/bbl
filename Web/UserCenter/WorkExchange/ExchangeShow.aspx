<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="ExchangeShow.aspx.cs" Inherits="Web.UserCenter.WorkExchange.ExchangeShow" Title="工作交流查看_工作交流_个人中心" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
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
                <th colspan="3" align="center" bgcolor="#BDDCF4">交流专区查看</th>
              </tr>
              <tr>
                <td width="16%" height="35"  align="right" bgcolor="#e3f1fc"><strong>标题：</strong></td>
                
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <% =exModel.Title%>
                </td>
              </tr>
              <tr>
                <td height="35"  align="right" bgcolor="#e3f1fc"><strong>交流内容：</strong></td>
                <td height="100" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                     <% =exModel.Description%>
                </td>
              </tr>
              <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>发布人：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%=exModel.OperatorName%></label>
                </td>
              </tr>
			  <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>发布时间：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= exModel.CreateTime.ToString("yyyy-MM-dd")%></label>
                </td>
              </tr>
              <%--回复内容--%>
              <%if (exModel.ReplyList != null)
                {
                    foreach (EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply reply in exModel.ReplyList)
                    {  %>
               <tr>
                   <td height="10"></td>
               </tr>         
              
              <tr>
                <td height="35"  align="right" bgcolor="#e3f1fc"><strong>回复：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><% =reply.Description%></label>
                </td>
              </tr>
			  <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>回复人：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= reply.OperatorName%></label>
                </td>
              </tr>
              <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>回复时间：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= reply.ReplyTime.ToString("yyyy-MM-dd")%></label>
                 </td>
              </tr>
                    <%}
                }%>
              <%--新的回复--%>
              <tr>
                <td height="35"  align="right" bgcolor="#e3f1fc"><strong>回复：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                <textarea id="repcon" cols="80" rows="8"></textarea></td>
              </tr>
			  <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>回复人：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                <label><%= this.SiteUserInfo.ContactInfo.ContactName%></label>
                <input type="checkbox" name="repIsAnonymous" id="isanony"/>匿名</td>
              </tr>
			  <tr>
                <td height="35" align="right" bgcolor="#e3f1fc"><strong>回复时间：</strong></td>
                <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label><%= DateTime.Now.ToString("yyyy-MM-dd")%></label>
                </td>
              </tr>
              
              <tr>
                <td height="30" colspan="3" align="center">
				<table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="repeat">回复</a></td>
		                <td width="86" height="40" align="center" class="tjbtn02"><a href="javascript:history.go(-1);">返回</a></td>
                    </tr>
                    <input type="hidden" id="tid" value="<% =tid %>" />
                    <input type="hidden" name="continue" value="" id="continue" />
                </table>
                </td>
              </tr>
          </table>
        </div>
		</div>
		<!-- InstanceEndEditable -->            
    </div>
    <script type="text/javascript">
    $(function(){
         $("a.repeat").bind("click",function(){
            var tid = $("#tid").val();
            var isannoy = $("#isanony").attr("checked");
            var connent = $("#repcon").val();
            $.newAjax({
                type: "POST",
                dataType: "json",
                url: "/UserCenter/WorkExchange/ExchangeShow.aspx?type=repeat",
                data: {"tid":tid,"isannoy":isannoy?1:0,"connent":connent},
                cache: false,
                success: function(d) {
                    switch(d.res){
                        case 1:
                            alert("回复成功!");
                            location.reload();
                            break;
                        case -1:
                            alert("回复失败!");
                            break;
                    }
                }
            });
         });
    });
    </script>
</asp:Content>
