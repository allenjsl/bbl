<%@ Page Title="系统日志_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="LogList.aspx.cs" Inherits="Web.systemset.systemlog.LogList" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：系统设置>> 系统日志</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div><div class="lineCategorybox" style="height:50px;"><table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="10" valign="top">&nbsp;</td>
                <td style="padding:5px 0px;">
                <div>
                	<label>部门：</label> 
                	<select class="select" runat="server" id="selDeaprt">
                    </select>
                     <label>操作员：
                      
                    </label>
                     <select id="selOperator" runat="server" >
                  </select>
                    <label></label><label>操作时间：</label>
                    <input  type="text"  class="searchinput" id="txtStartDate" onfocus="WdatePicker()" runat="server"/> 
                  至 <input type="text"  id="txtEndDate" class="searchinput" onfocus="WdatePicker()"  runat="server"/>
                    <label></label>
                    <label><a href="javascript:;" onclick="return Log.search();"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a></label>
              </div></td>
                <td width="10" valign="top">&nbsp;</td>
              </tr>
            </table></div>
				<div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="6%" align="center" bgcolor="#BDDCF4">编号                    </th>
                    <th width="12%" align="center" bgcolor="#BDDCF4">操作员</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">部门</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">操作模块</th>
                    <th width="26%" align="center" bgcolor="#bddcf4">操作内容</th>
                    <th width="15%" align="center" bgcolor="#bddcf4">操作时间</th>
                    <th width="13%" align="center" bgcolor="#bddcf4">IP</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
                  <asp:CustomRepeater ID="rptLog" runat="server">
                  <ItemTemplate>
                    <tr>
                    <td height="35"  align="center" bgcolor="#e3f1fc"><%# itemIndex++%></td>
                    <td height="35" align="center" bgcolor="#e3f1fc"><%# Eval("OperatorName")%></td>
                    <td height="35" align="center" bgcolor="#e3f1fc"><%# Eval("DepartName")%></td>
                    <td height="35" align="center" bgcolor="#e3f1fc"><%# Eval("ModuleId") %></td>
                    <td height="35" align="left" bgcolor="#e3f1fc" class="pandl3"><%# Eval("EventMessage") %></td>
                    <td align="center" bgcolor="#e3f1fc"><%# Convert.ToDateTime(Eval("EventTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                    <td align="center" bgcolor="#e3f1fc"><%# Eval("EventIp") %> </td>
                    <td align="center" bgcolor="#e3f1fc"><a href="javascript:;" onclick="return Log.lookLog('<%# Eval("Id") %>')">查看</a> </td>
                  </tr>
                  </ItemTemplate>
                  <AlternatingItemTemplate>
                   <tr>
                    <td height="35"  align="center" bgcolor="#BDDCF4"><%# itemIndex++%></td>
                    <td height="35" align="center" bgcolor="#BDDCF4"><%# Eval("OperatorName")%></td>
                    <td height="35" align="center" bgcolor="#BDDCF4"><%# Eval("DepartName")%></td>
                    <td height="35" align="center" bgcolor="#BDDCF4"><%# Eval("ModuleId") %></td>
                    <td height="35" align="left" bgcolor="#BDDCF4" class="pandl3"><%# Eval("EventMessage") %></td>
                    <td align="center" bgcolor="#BDDCF4"><%# Convert.ToDateTime(Eval("EventTime")).ToString("yyyy-MM-dd HH:mm") %></td>
                    <td align="center" bgcolor="#BDDCF4"><%# Eval("EventIp") %> </td>
                    <td align="center" bgcolor="#BDDCF4"><a href="javascript:;" onclick="return Log.lookLog('<%# Eval("Id") %>')">查看</a>  </td>
                  </tr>
                  </AlternatingItemTemplate>
                  </asp:CustomRepeater>
                
                 
                  
                  <tr>
                    <td height="30" colspan="8" align="right" class="pageup">
                      <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
                    </td>
                  </tr>
              </table>
            </div>

			</div>
			  <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
			  <script type="text/javascript">
			      var Log = {
			          openDialog: function(p_url, p_title) {
			              Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: "250px" });
			          },
			          //修改部门
			          lookLog: function(logid) {
			              Log.openDialog("/systemset/systemlog/LogDetail.aspx?logId=" + logid, "查看日志");
			              return false;
			          },
			          //查询
			          search: function() {
			              var departId = $("#<%=selDeaprt.ClientID %>").val(); //部门ID
			              var operator = $("#<%=selOperator.ClientID %>").val(); //操作员
			              var startDate = $("#<%=txtStartDate.ClientID %>").val(); //开始日期
			              var endDate = $("#<%=txtEndDate.ClientID %>").val(); //结束日期
			              //执行查询
			              window.location = "/systemset/systemlog/LogList.aspx?startDate=" + startDate + "&endDate=" + endDate + "&departId=" + departId + "&operator=" + operator;
			              return false;
			          }
			      }
			      $(document).ready(function() {
			      $("#<%=txtStartDate.ClientID %>,#<%=txtEndDate.ClientID %>").keydown(function(event) {
			              if (event.keyCode == 13) {
			                  Log.search();
			              }
			          });
			      });
			  </script>
</asp:Content>
