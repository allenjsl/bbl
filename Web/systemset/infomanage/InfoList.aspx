<%@ Page Title="信息管理_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="InfoList.aspx.cs" Inherits="Web.systemset.infomanage.InfoList" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
td
{
  text-align:center;
}
.even td{ background:#e3f1fc}
.odd td{ background:#BDDCF4}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f; text-align:right;">所在位置：系统设置>> 信息管理</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div><div class="lineCategorybox" style="height:30px;">
              </div>
              
             
            <div class="btnbox">
            	<table border="0" align="left" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="90" align="center" ><a href="/systemset/infomanage/InfoEdit.aspx">新 增</a></td>
                 
                    <td width="90" align="center"><a href="javascript:;" onclick="return InfoList.del('');">删 除</a></td>
                  </tr>
              </table>
            </div>
            <form runat="server" id="form1">
            	<div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr class="odd">
                    <th width="8%"  >全选
                    <input  id="chkAll"  type="checkbox" onclick="InfoList.selAll(this);" /></th>
                    <th width="8%">编号</th>
                    <th width="39%" >标题</th>
                    <th width="9%" >浏览数</th>
                    <th width="9%" >发布人</th>
                    <th width="17%" >发布时间</th>
                    <th width="10%" >操作</th>
                  </tr>
                   <asp:CustomRepeater ID="rptInfo" runat="server">
                  <ItemTemplate>
                  <tr class="even">
                    <td  ><input type="checkbox" value='<%# Eval("Id") %>' class="c1"/></td>
                    <td ><%# itemIndex++%></td>
                    <td  class="pandl3"><%# Eval("Title")%></td>
                    <td ><%# Eval("Clicks") %></td>
                    <td ><%# Eval("OperatorName") %></td>
                    <td ><%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm:ss")%></td>
                    <td ><a href="InfoEdit.aspx?infoId=<%# Eval("Id") %>">修改 </a>|<a href="javascript:;" onclick="return InfoList.del('<%# Eval("Id") %>')"> 删除</a></td>
                  </tr>
                  </ItemTemplate>
                  <AlternatingItemTemplate>
                  <tr class="odd">
                    <td ><input type="checkbox" value='<%# Eval("Id") %>'  class="c1" /></td>
                    <td ><%# itemIndex++%></td>
                    <td  class="pandl3"><%# Eval("Title")%></td>
                    <td ><%# Eval("Clicks") %></td>
                    <td ><%# Eval("OperatorName") %></td>
                    <td ><%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm:ss")%></td>
                    <td ><a href="InfoEdit.aspx?infoId=<%# Eval("Id") %>">修改 </a>|<a href="javascript:;" onclick="return InfoList.del('<%# Eval("Id") %>')"> 删除</a></td>
                  </tr>
                  </AlternatingItemTemplate>
                  </asp:CustomRepeater>
                 
                  <tr>
                    <td height="30" colspan="7"  style="text-align:right" class="pageup">
                      <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
                    </td>
                  </tr>
              </table>
            </div>
             </form>
			</div>
			
			<script type="text/javascript">
			    var InfoList =
			  {

			      //删除信息
			      del: function(cid) {
			          if (cid == "") {
			              var cidArr = [];
			              var chkAllObj = $("#chkAll");
			              $(".c1:checked").each(function() {
			                  cidArr.push($(this).val());
			              });
			             
			              if (cidArr.length < 1) {
			                  alert("请选择要删除的信息！");
			                  return false;
			              }
			              else {
			                  if (!confirm("你确定要删除所选信息？"))
			                      return false;
			                  cid = cidArr.toString();
			              }
			          }
			          else {
			              if (!confirm("你确定要删除该信息吗？"))
			                  return false;
			          }
			         
			          window.location = "/systemset/infomanage/InfoList.aspx?method=del&ids=" + cid

			         
			      },
			      selAll: function(tar) {
			          $(":checkbox").attr("checked", $(tar).attr("checked"));
			      }
			  }
			  
			</script>
</asp:Content>
