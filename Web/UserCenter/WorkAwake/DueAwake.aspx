<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="DueAwake.aspx.cs" Inherits="Web.UserCenter.WorkAwake.DueAwake" Title="劳动合同到期提醒_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <% if (!string.IsNullOrEmpty(Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 1)))
           
       { %>
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> <a href="#">个人中心</a>>> 信息管理</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:30px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <% =Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 5)%>
                    </tr>
                  </table>
                </div>
              <div class="btnbox">
                <%if (setday)
                  { %>
                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr>
                    <td align="left">设置提前 
                      <input name="textfield" type="text" size="10" class="duetime" value="<% =day %>" />
                      天提醒
                        <input type="button" value="保存" class="save" /></td>
                  </tr>
                </table>
                <%} %>
              </div>
           	  <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr bgcolor="#BDDCF4">
                    <th width="13%" align="center" >员工编号</th>
                    <th width="21%" align="center" >姓名</th>
                    <th width="22%" align="center" >签订日期</th>
                    <th width="22%" align="center" >到期日期</th>
                    <th width="22%" align="center" >提醒</th>
                  </tr>
                  <asp:Repeater ID="rptList" runat="server">
                  <ItemTemplate>
                  <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                    <td  align="center" ><%# Eval("StaffNo") %></td>
                    <td align="center" class="pandl3"><%# Eval("StaffName")%></td>
                    <td align="center" ><span class="pandl3"><%# Convert.ToDateTime(Eval("SignDate")).ToString("yyyy-MM-dd") %></span></td>
                    <td align="center" ><span class="pandl3"><%# Convert.ToDateTime(Eval("ExpireDate")).ToString("yyyy-MM-dd")%></span></td>
                    <td align="center" ><font color="#FF0000">还有<%# (Convert.ToDateTime(Eval("ExpireDate"))-DateTime.Now).Days.ToString() %>天到期</font></td>
                  </tr>
                  </ItemTemplate>
                  </asp:Repeater>
                  <%if (len == 0)
                    { %>
                    <tr align="center"><td colspan="5">没有相关数据</td></tr>
                  <%} %>
              </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td height="30" align="right" class="pageup" colspan="13">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3"  PageStyleType="NewButton" CurrencyPageCssClass="RedFnt" />
                    </td>
                  </tr>
                </table>
           	  </div>

			</div>
			<!-- InstanceEndEditable -->
			</div>
    <%}
       else
       {%>
       <div style="height:500px"></div>
	<%} %>
			
			<script type="text/javascript">
			    $(function(){
			        $("input.save").click(function(){
			            var r=/^\+?[1-9][0-9]*$/;
			            var duetime = $("input.duetime").val();
			            if(r.test(duetime)){
			                $.newAjax({
			                    url:"/UserCenter/WorkAwake/DueAwake.aspx?type=duedayset",
			                    type:"POST",
			                    data:{"num":duetime},
			                    dataType: 'json',
			                    cache: false,
			                    success:function(d){
			                        switch(d.ret){
			                            case 1:
			                                alert("设置成功");
			                                location.reload();
			                                break;
			                            case -1:
			                                alert("服务器繁忙");
			                                break;
			                            case -2:
			                                alert("没有设置权限");
			                                break; 
			                        }
			                    }
			                });
			            }else{
			                alert("请正确设定日期");
			            }
			        });
			        
			        
			        
			    });
			</script>
</asp:Content>
