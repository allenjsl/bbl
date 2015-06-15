<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxAttendanceList.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.AjaxAttendanceList" %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Import Namespace="EyouSoft.Common" %>
<table width="100%" border="0" cellpadding="0" cellspacing="1">
  <tr>
    <th width="6%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
    <th width="8%" align="center" bgcolor="#BDDCF4"><strong>员工编号</strong></th>
    <th width="8%" align="center" bgcolor="#bddcf4"><strong>姓名</strong></th>
    <th width="12%" align="center" bgcolor="#bddcf4"><strong>部门</strong></th>
    <th width="48%" align="center" bgcolor="#bddcf4"><strong>当月考勤概况</strong></th>
    <th width="8%" align="center" bgcolor="#bddcf4"><strong>考勤明细</strong></th>
    <th width="8%" align="center" bgcolor="#bddcf4"><strong>考勤登记</strong></th>
  </tr>
    <cc1:CustomRepeater ID="crptAttendanceList" runat="server">
        <ItemTemplate>
            <tr>
                <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
                <td align="center" bgcolor="#e3f1fc" class="pandl3"><%# Eval("ArchiveNo")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("StaffName") %></td>
                <td align="center" bgcolor="#e3f1fc"><%# GetDepartmentString(Eval("DepartmentList"))%></td>
                <td align="center" bgcolor="#e3f1fc">
                    准点<%# Eval("Punctuality")%>天，迟到<%# Eval("Late")%>天，早退<%# Eval("LeaveEarly")%>天，
                    旷工<%# Eval("Absenteeism")%>天，
                    请假<%# decimal.Round(Utils.GetDecimal(Eval("AskLeave").ToString()),1)%>天，
                    加班<%# decimal.Round(Utils.GetDecimal( Eval("OverTime").ToString()),1) %>小时</td>
                <td align="center" bgcolor="#e3f1fc">
                    <a href="javascript:void(0);" onclick="return AttDefault.OpenPage('look',<%# Eval("StaffNo") %>);return false;">查看</a></td>
                <td align="center" bgcolor="#e3f1fc">
                    <a <%# EditFlag?"":"style='display:none'" %> href="javascript:void(0);" 
                    onclick="return AttDefault.OpenPage('add',<%# Eval("StaffNo") %>);return false;">登记</a></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td  align="center" bgcolor="#BDDCF4"><%=GetCount() %></td>
                <td align="center" bgcolor="#BDDCF4" class="pandl3"><%# Eval("ArchiveNo")%></td>
                <td align="center" bgcolor="#BDDCF4"><%# Eval("StaffName") %></td>
                <td align="center" bgcolor="#BDDCF4"><%# GetDepartmentString(Eval("DepartmentList"))%></td>
                <td align="center" bgcolor="#BDDCF4">
                    准点<%# Eval("Punctuality")%>天，迟到<%# Eval("Late")%>天，早退<%# Eval("LeaveEarly")%>天，
                    旷工<%# Eval("Absenteeism")%>天，
                    请假<%# decimal.Round(Utils.GetDecimal(Eval("AskLeave").ToString()),1)%>天，
                    加班<%# decimal.Round(Utils.GetDecimal( Eval("OverTime").ToString()),1) %>小时</td>
                <td align="center" bgcolor="#BDDCF4">
                    <a href="javascript:void(0);" onclick="return AttDefault.OpenPage('look',<%# Eval("StaffNo") %>);return false;">查看</a></td>
                <td align="center" bgcolor="#BDDCF4">
                    <a <%# EditFlag?"":"style='display:none'" %> href="javascript:void(0);" 
                    onclick="return AttDefault.OpenPage('add',<%# Eval("StaffNo") %>);return false;">登记</a></td>
            </tr>
        </AlternatingItemTemplate>
    </cc1:CustomRepeater>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="right" class="pageup"><cc2:ExporPageInfoSelect  ID="ExporPageInfoSelect1" runat="server" /></td>
  </tr>
</table>