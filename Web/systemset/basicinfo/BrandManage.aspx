<%@ Page Title="品牌管理_基础设置_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="BrandManage.aspx.cs" Inherits="Web.systemset.basicinfo.BrandManage" %>
<%@ Register Src="~/UserControl/ucSystemSet/BasicInfoHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
     <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="5" />
            <div class="btnbox">
            	<table border="0" align="left" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="90" align="center"><a href="javascript:;" onClick="return BrandM.openDialog('/systemset/basicinfo/BrandEdit.aspx','新增品牌')">新 增</a></td>
                  </tr>
              </table>
            </div>
            <form runat="server" id="form1">
            	<div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="8%" align="center" bgcolor="#BDDCF4">编号</th>
                    <th width="30%" align="center" bgcolor="#BDDCF4">品牌名称</th>
                    <th width="25%" align="center" bgcolor="#bddcf4">内logo</th>
                    
                    <th width="12%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
                  <asp:CustomRepeater runat="server" ID="rptBrand">
                  <ItemTemplate>
                  <tr>
                    <td height="35"  align="center" bgcolor="#e3f1fc"><%# itemIndex++%></td>
                    <td height="35" align="center" bgcolor="#e3f1fc"><%# Eval("BrandName") %></td>
                    <td height="35" align="center" bgcolor="#e3f1fc"><img src='<%# Eval("Logo1") %>' alt="" width="150" height="30" /></td>
                  
                    <td height="35" align="center" bgcolor="#e3f1fc"><a href="javascript:;" onclick="return BrandM.update('<%# Eval("Id") %>');">修改 </a>|<a href="/systemset/basicinfo/BrandManage.aspx?brandId=<%# Eval("Id") %>" onclick="return BrandM.delTip();"> 删除</a></td>
                  </tr>
                  </ItemTemplate>
                  <AlternatingItemTemplate>
                  <tr>
                    <td height="35"  align="center" bgcolor="#BDDCF4"><%# itemIndex++%></td>
                    <td height="35" align="center" bgcolor="#BDDCF4"><%# Eval("BrandName") %></td>
                    <td height="35" align="center" bgcolor="#BDDCF4"><img src='<%# Eval("Logo1") %>' alt="" width="150" height="30" /></td>
                   
                    <td height="35" align="center" bgcolor="#BDDCF4"><a href="javascript:;" onclick="return BrandM.update('<%# Eval("Id") %>');">修改 </a>|<a href="/systemset/basicinfo/BrandManage.aspx?brandId=<%# Eval("Id") %>" onclick="return BrandM.delTip();"> 删除</a></td>
                  </tr>
                  </AlternatingItemTemplate>
               </asp:CustomRepeater>
                  <tr>
                    <td height="30" colspan="4" align="right" class="pageup">
                        <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
                    
                    </td>
                  </tr>
              </table>
            </div>
            </form>
            </div>
             <script type="text/javascript">
                var BrandM =
                {
                    //打开弹窗
                    openDialog: function(p_url, p_title) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: "180px" });
                    },
                    //修改报价
                    update:function(pId){
                    BrandM.openDialog("/systemset/basicinfo/BrandEdit.aspx?brandId=" + pId, "修改品牌");
                       return false;
                    },
                    delTip: function() {
                        return confirm("你确认要删除该品牌吗？");
                    }
                }
            </script>
</asp:Content>
