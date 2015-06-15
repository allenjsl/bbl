<%@ Page Title="客户等级_基础设置_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="CustomerLevel.aspx.cs" Inherits="Web.systemset.basicinfo.CustomerLevel" %>
<%@ Register Src="~/UserControl/ucSystemSet/BasicInfoHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
  .oddTr td{ background-color:#e3f1fc}
   .evenTr td{ background-color:#BDDCF4}
   .issys{ color:#777}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
  <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="4" />
   <div class="btnbox">
            	<table border="0" align="left" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="90" align="center"><a href="javascript:;" onClick="return CustLevel.openDialog('/systemset/basicinfo/CustomerLevelEdit.aspx','新增客户等级')">新 增</a></td>
                  </tr>
              </table>
            </div>
           <form runat="server" id="custForm">
            	<div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="8%" align="center" bgcolor="#BDDCF4">编号</th>
                    <th width="30%" align="center" bgcolor="#BDDCF4">等级名称</th>
                    <th width="12%" align="center" bgcolor="#bddcf4">操作</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">编号</th>
                    <th width="30%" align="center" bgcolor="#BDDCF4">等级名称</th>
                    <th width="12%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
                 <asp:CustomRepeater runat="server" ID="rptCustLevel">
                    <ItemTemplate>
                    <%# GetListTr()%>
                    <td  align="center" bgcolor="#e3f1fc"><%= itemIndex2++%></td>
                    <td align="center" bgcolor="#e3f1fc"><%# Eval("CustomStandName") %></td>
                    <td align="center" bgcolor="#e3f1fc"><%# GetExecute((bool)Eval("IsSystem"),(int)Eval("Id")) %></td>
                    </ItemTemplate>
                 </asp:CustomRepeater>
                   <%=GetLastTr() %>
                  <tr>
                    <td height="30" colspan="6" align="right" class="pageup">
                     <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
                    </td>
                  </tr>
              </table>
            </div>
            </form>
            </div>
            <script type="text/javascript">
                var CustLevel =
                {
                    //打开弹窗
                    openDialog: function(p_url, p_title) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: "120px" });
                    },
                    //修改报价
                    update: function(pId) {
                        CustLevel.openDialog("/systemset/basicinfo/CustomerLevelEdit.aspx?custId=" + pId, "修改客户等级");
                        return false;
                    },
                    delTip: function(aId) {
                    var mess = "你确认要删除该客户等级吗？";
                    var isSuccess = true;
                    $.newAjax(
                           {
                               url: "/systemset/basicinfo/CustomerLevel.aspx",
                               data: { method: "ispublish", custId: aId },
                               dataType: "json",
                               cache: false,
                               async: false,
                               type: "get",
                               success: function(result) {
                                   if (result.success == "1") {
                                       isSuccess = false;
                                       alert("该客户等级被使用，不能删除！");
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
