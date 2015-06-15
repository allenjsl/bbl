<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxCahierList.aspx.cs" Inherits="Web.administrativeCenter.cahierManage.AjaxCahierList" %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<table width="100%" border="0" cellpadding="0" cellspacing="1">
  <tr>
    <th width="7%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
    <th width="7%" align="center" bgcolor="#bddcf4"><strong>会议编号</strong></th>
    <th width="14%" align="center" bgcolor="#bddcf4"><strong>会议主题</strong></th>
    <th width="23%" align="center" bgcolor="#bddcf4"><strong>参会人员</strong></th>
    <th width="18%" align="center" bgcolor="#bddcf4"><strong>会议时间</strong></th>
    <th width="7%" align="center" bgcolor="#bddcf4"><strong>会议地点</strong></th>
    <th width="15%" align="center" bgcolor="#bddcf4"><strong>备注</strong></th>
    <th width="9%" align="center" bgcolor="#bddcf4"><strong>操作</strong></th>
  </tr>
  <cc1:CustomRepeater ID="crptCahierList" runat="server">
    <ItemTemplate>
      <tr>
        <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("MetttingNo")%></td>
        <td align="left" bgcolor="#e3f1fc" class="pandl3"><%# Eval("Title")%></td>
        <td align="left" bgcolor="#e3f1fc" class="pandl3"><%# Eval("Personal")%></td>
        <td align="center" bgcolor="#e3f1fc">
        <%# GetFormartDate( string.Format("{0:yyyy-MM-dd}", Eval("BeginDate")),string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))) %></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("Location")%></td>
        <td align="center" bgcolor="#e3f1fc"><%# Eval("Remark")%></td>
        <td align="center" bgcolor="#e3f1fc">
            <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
             onclick="Default.OpenPage('','<%# Eval("Id")%>');">修改</a>
            <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
            <a href="javascript:void(0);"   <%# DeleteFlag?"":"style='display:none;'" %>
             onclick="Default.DeleteMeeting('<%# Eval("Id")%>');">删除</a></td>
      </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
      <tr>
        <td  align="center" bgcolor="#bddcf4"><%=GetCount() %></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("MetttingNo")%></td>
        <td align="left" bgcolor="#bddcf4" class="pandl3"><%# Eval("Title")%></td>
        <td align="left" bgcolor="#bddcf4" class="pandl3"><%# Eval("Personal")%></td>
        <td align="center" bgcolor="#bddcf4">
         <%# GetFormartDate( string.Format("{0:yyyy-MM-dd}", Eval("BeginDate")),string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))) %></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("Location")%></td>
        <td align="center" bgcolor="#bddcf4"><%# Eval("Remark")%></td>
        <td align="center" bgcolor="#bddcf4">
            <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
             onclick="Default.OpenPage('','<%# Eval("Id")%>');">修改</a>
            <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
            <a href="javascript:void(0);"   <%# DeleteFlag?"":"style='display:none;'" %>
             onclick="Default.DeleteMeeting('<%# Eval("Id")%>');">删除</a></td>
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
