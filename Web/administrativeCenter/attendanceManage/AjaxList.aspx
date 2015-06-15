<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.AjaxList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Import Namespace="EyouSoft.Common" %>
<cc1:CustomRepeater ID="crptPersonalList" runat="server">
    <ItemTemplate>
        <table width="800" border="0" cellpadding="0" cellspacing="0" align="center">
          <tr>
            <td   align="left">员工编号：<%# Eval("ArchiveNo")%>   姓名：<%# Eval("UserName")%>    部门：<%# GetDepartmentByList(Eval("DepartmentList"))%>
            </td>
          </tr>
        </table>
        <%# GetAttendanceDetails(Eval("AttendanceList"),Utils.GetInt(Eval("Id").ToString()))%>
        <table width="800" border="0" cellpadding="0" cellspacing="0" align="center">
          <tr>
            <td bgcolor="#eeeeee" class="pandl10" align="left"><%# GetMonthAttendance(Utils.GetInt(Eval("Id").ToString())) %></td>
            
          </tr>
        </table>
    </ItemTemplate>
</cc1:CustomRepeater>

<cc1:CustomRepeater ID="crptCollectAllList" runat="server">
  <HeaderTemplate>
        <table width="800" border="1"  align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse; line-height:16px;">
        <%=GetAttendanceAllDepartTitle() %>
  </HeaderTemplate>
  <ItemTemplate>
    <%# GetAttendanceAllDepart(Eval("PersonList"), Eval("DepartmentName").ToString())%>
  </ItemTemplate>
  <FooterTemplate>
        </table>
  </FooterTemplate>
</cc1:CustomRepeater>
