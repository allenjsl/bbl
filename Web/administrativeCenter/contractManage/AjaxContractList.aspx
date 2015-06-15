<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxContractList.aspx.cs" Inherits="Web.administrativeCenter.contractManage.AjaxContractList" %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
 
<table width="100%" border="0" cellpadding="0" cellspacing="1">
  <tr>
    <th width="7%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
    <th width="7%" align="center" bgcolor="#bddcf4"><strong>员工编号</strong></th>
    <th width="8%" align="center" bgcolor="#bddcf4"><strong>姓名</strong></th>
    <th width="12%" align="center" bgcolor="#bddcf4"><strong>签订时间</strong></th>
    <th width="12%" align="center" bgcolor="#bddcf4"><strong>到期时间</strong></th>
    <th width="12%" align="center" bgcolor="#bddcf4"><strong>状态</strong></th>
    <th width="32%" align="center" bgcolor="#bddcf4"><strong>备注</strong></th>
    <th width="10%" align="center" bgcolor="#bddcf4"><strong>操作</strong></th>
  </tr>     
<cc1:CustomRepeater ID="crptContractList" runat="server">
    <ItemTemplate>
     <tr>
        <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("StaffNo")%></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("StaffName")%></td>
        <td align="center" bgcolor="#e3f1fc"><%# string.Format("{0:yyyy-MM-dd}", Eval("BeginDate"))%></td>
        <td align="center" bgcolor="#e3f1fc"><%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("ContractStatus")%></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("Remark")%></td>
        <td align="center" bgcolor="#e3f1fc">
            <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
            onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
            <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
            <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
            onclick="Default.DeleteContract('<%# Eval("Id")%>')">删除</a></td>
      </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
      <tr>
        <td  align="center" bgcolor="#bddcf4"><%=GetCount() %></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("StaffNo")%></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("StaffName")%></td>
        <td align="center" bgcolor="#bddcf4"><%# string.Format("{0:yyyy-MM-dd}", Eval("BeginDate"))%></td>
        <td align="center" bgcolor="#bddcf4"><%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("ContractStatus")%></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("Remark")%></td>
        <td align="center" bgcolor="#bddcf4">
           <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
            onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
            <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
            <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
            onclick="Default.DeleteContract('<%# Eval("Id")%>')">删除</a></td>
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
