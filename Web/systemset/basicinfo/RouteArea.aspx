<%@ Page Title="线路区域_基础设置_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="RouteArea.aspx.cs" Inherits="Web.systemset.basicinfo.RouteArea" %>
<%@ Register Src="~/UserControl/ucSystemSet/BasicInfoHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
  <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="2" />
 <div class="btnbox">
            	<table border="0" align="left" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="90" align="center"><a href="javascript:;" onClick="return RouteArea.openDialog('EditRouteArea.aspx','新增线路区域');">新 增</a></td>
                  </tr>
              </table>
            </div>
            <form runat="server" id="form1">
 <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="14%" align="center" bgcolor="#BDDCF4">编号</th>
                    <th width="30%" align="center" bgcolor="#BDDCF4">区域名称</th>
                    <th width="19%" align="center" bgcolor="#bddcf4">计调员</th>
                      <th width="10%" align="center" bgcolor="#bddcf4">
                          排序编号
                      </th>
                    <th width="17%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
                  <asp:CustomRepeater ID="rptRouteArea" runat="server">
                 <ItemTemplate>
                  <tr>
                    <td  align="center" bgcolor="#e3f1fc"><%# itemIndex++%></td>
                    <td align="center" bgcolor="#e3f1fc"><%# Eval("AreaName") %></td>
                    <td align="center" bgcolor="#e3f1fc"><%# GetUsers(Eval("AreaUserList"))%></td>
                    <td style="text-align:center; background:#e3f1fc;"><%#Eval("SortId") %></td>
                    <td align="center" bgcolor="#e3f1fc"> <a href="javascript:;" onclick="return RouteArea.openDialog('EditRouteArea.aspx?areaId=<%# Eval("Id") %>','修改修路区域');">修改 </a>|<a href='/systemset/basicinfo/RouteArea.aspx?areaId=<%# Eval("Id") %>' onclick="return RouteArea.delTip('<%# Eval("Id") %>');"> 删除</a></td>
                  </tr>
                  </ItemTemplate>
                
                  <AlternatingItemTemplate>
                  <tr>
                    <td  align="center" bgcolor="#BDDCF4"><%# itemIndex++%></td>
                    <td align="center" bgcolor="#BDDCF4"><%# Eval("AreaName") %></td>
                    <td align="center" bgcolor="#BDDCF4"><%# GetUsers(Eval("AreaUserList"))%></td>
                    <td style="text-align: center; background: #bddcf4;"><%#Eval("SortId") %></td>
                    <td align="center" bgcolor="#BDDCF4"><a href="javascript:;" onclick="return RouteArea.openDialog('EditRouteArea.aspx?areaId=<%# Eval("Id") %>','修改修路区域');">修改 </a>|<a href='/systemset/basicinfo/RouteArea.aspx?areaId=<%# Eval("Id") %>' onclick="return RouteArea.delTip('<%# Eval("Id") %>');"> 删除</a></td>
                  </tr>
                  </AlternatingItemTemplate>
               </asp:CustomRepeater>
                     <tr>
                    <td height="30" colspan="4" align="right" class="pageup">
                     <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo></td>
                  </tr>
              </table>
              </form>
            </div>
           
            <script type="text/javascript">
                var RouteArea =
                {
                    //打开弹窗
                    openDialog: function(p_url, p_title) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: "450px" });
                        return false;
                    },
                    delTip: function(aId) {
                        var mess = "你确认要删除该线路区域吗？";
                        var isSuccess = true;
                        $.newAjax(
                           {
                               url: "/systemset/basicinfo/RouteArea.aspx",
                               data: { method: "ispublish", areaId: aId },
                               dataType: "json",
                               cache: false,
                               async: false,
                               type: "get",
                               success: function(result) {
                                   if (result.success == "1") {
                                       isSuccess = false;
                                       alert("该线路区域下有线路发布，不能删除！");
                                   }

                               },
                               error: function() {
                                   isSuccess = false;
                                   alert("删除失败！");
                               }
                           })
                        if (isSuccess) {
                            return confirm(mess);
                        }
                        return false;
                    }
                }
            </script>
</asp:Content>
