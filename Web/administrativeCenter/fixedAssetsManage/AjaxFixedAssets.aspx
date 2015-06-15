<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxFixedAssets.aspx.cs" Inherits="Web.administrativeCenter.fixedAssetsManage.AjaxFixedAssets" %>

<%@ Import Namespace="EyouSoft.Common" %>
<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<table width="100%" border="0" cellpadding="0" cellspacing="1">
  <tr>
    <th width="8%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
    <th width="9%" align="center" bgcolor="#bddcf4"><strong>编号</strong></th>
    <th width="12%" align="center" bgcolor="#bddcf4"><strong>资产名称</strong></th>
    <th width="10%" align="center" bgcolor="#bddcf4"><strong>购买时间</strong></th>
    <th width="13%" align="center" bgcolor="#bddcf4"><strong>折旧费</strong></th>
    <th width="13%" align="center" bgcolor="#bddcf4"><strong>备注</strong></th>
    <th width="9%" align="center" bgcolor="#bddcf4"><strong>操作</strong></th>
  </tr>
<cc1:CustomRepeater ID="crptFixedAssetsList" runat="server">
<ItemTemplate>
  <tr>
    <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("AssetNo")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("AssetName")%></td>
    <td align="center" bgcolor="#e3f1fc">
        <%# string.Format("{0:yyyy-MM-dd}", Eval("BuyDate")) == "1990-01-01"?"":string.Format("{0:yyyy-MM-dd}", Eval("BuyDate")) %></td>
    <td align="center" bgcolor="#e3f1fc"><%# Utils.FilterEndOfTheZeroString(string.Format("{0:C}",Eval("Cost")))%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("Remark")%></td>
    <td align="center" bgcolor="#e3f1fc">
         <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
        onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
        <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
        <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
        onclick="Default.DeleteFixedAsset('<%# Eval("Id")%>')">删除</a></td>
  </tr>
</ItemTemplate>
<AlternatingItemTemplate>
  <tr>
    <td  align="center" bgcolor="#bddcf4"><%=GetCount() %></td>
    <td align="center" bgcolor="#bddcf4"><%# Eval("AssetNo")%></td>
    <td align="center" bgcolor="#bddcf4"><%# Eval("AssetName")%></td>
    <td align="center" bgcolor="#bddcf4">
        <%# string.Format("{0:yyyy-MM-dd}", Eval("BuyDate")) == "1990-01-01"?"":string.Format("{0:yyyy-MM-dd}", Eval("BuyDate")) %></td>
    <td align="center" bgcolor="#bddcf4"><%# Utils.FilterEndOfTheZeroString(string.Format("{0:C}",Eval("Cost")))%></td>
    <td align="center" bgcolor="#bddcf4"><%# Eval("Remark")%></td>
    <td align="center" bgcolor="#bddcf4">
        <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
        onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
        <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
        <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
        onclick="Default.DeleteFixedAsset('<%# Eval("Id")%>')">删除</a></td>
  </tr>
</AlternatingItemTemplate>
</cc1:CustomRepeater>
 </table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="right" class="pageup">
     <cc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
    </td>
  </tr>
</table>

