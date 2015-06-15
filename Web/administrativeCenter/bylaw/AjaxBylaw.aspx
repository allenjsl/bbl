<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxBylaw.aspx.cs" Inherits="Web.administrativeCenter.bylaw.AjaxBylaw" %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<table width="100%" border="0" cellpadding="0" cellspacing="1">
      <tr>
        <th width="8%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
        <th width="15%" align="center" bgcolor="#BDDCF4"><strong>编号</strong></th>
        <th width="62%" align="center" bgcolor="#bddcf4"><strong>标题</strong></th>
        <th width="15%" align="center" bgcolor="#bddcf4"><strong>操作</strong></th>
      </tr>
      <cc1:CustomRepeater ID="crptBylawList" runat="server">
        <ItemTemplate>
            <tr>
                <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
                <td  align="center" bgcolor="#e3f1fc"><%# Eval("RoleNo")%></td>
                <td align="left" bgcolor="#e3f1fc" class="pandl10">
                    <a href="/administrativeCenter/bylaw/PrintDetail.aspx?DutyID=<%# Eval("Id") %> " target="_blank">
                        <%# Eval("Title")%></a></td>
                <td align="center" bgcolor="#e3f1fc">
                    <a href="javascript:void(0);" <%# EditFlag?"":"style='display:none;'" %>
                    onclick="Default.OpenPage(<%# Eval("Id") %>,'update');">修改</a>
                    <span <%# DelateFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
                    <a href="javascript:void(0);" <%# DelateFlag?"":"style='display:none;'" %>
                    onclick="Default.DeleteDuty(<%# Eval("Id") %>);">删除</a></td>
              </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td  align="center" bgcolor="#BDDCF4"><%=GetCount() %></td>
                <td  align="center" bgcolor="#BDDCF4"><%# Eval("RoleNo")%></td>
                <td align="left" bgcolor="#BDDCF4" class="pandl10">
                    <a href="/administrativeCenter/bylaw/PrintDetail.aspx?DutyID=<%# Eval("Id") %> " target="_blank">
                        <%# Eval("Title")%></a></td>
                <td align="center" bgcolor="#BDDCF4">
                    <a href="javascript:void(0);" <%# EditFlag?"":"style='display:none;'" %>
                    onclick="Default.OpenPage(<%# Eval("Id") %>,'update');">修改</a>
                    <span <%# DelateFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
                    <a href="javascript:void(0);" <%# DelateFlag?"":"style='display:none;'" %>
                    onclick="Default.DeleteDuty(<%# Eval("Id") %>);">删除</a></td>
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