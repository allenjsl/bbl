<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxDefaultInfo.aspx.cs" Inherits="Web.administrativeCenter.personnelFiles.AjaxDefaultInfo"  %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>

<table width="100%" border="0" cellpadding="0" cellspacing="1">
  <tr>
    <th width="5%" align="center" bgcolor="#BDDCF4">序号</th>
    <th width="7%" align="center" bgcolor="#BDDCF4"><strong>档案编号</strong></th>
    <th width="7%" align="center" bgcolor="#bddcf4"><strong>姓名</strong></th>
    <th width="4%" align="center" bgcolor="#bddcf4"><strong>性别</strong></th>
    <th width="8%" align="center" bgcolor="#bddcf4"><strong>出生日期</strong></th>
    <th width="9%" align="center" bgcolor="#bddcf4"><strong>所属部门</strong></th>
    <th width="7%" align="center" bgcolor="#bddcf4"><strong>职务</strong></th>
    <th width="4%" align="center" bgcolor="#bddcf4"><strong>工龄</strong></th>
    <th width="11%" align="center" bgcolor="#bddcf4"><strong>联系电话</strong></th>
    <th width="10%" align="center" bgcolor="#bddcf4"><strong>手机</strong></th>
    <th width="13%" align="center" bgcolor="#bddcf4"><strong>E-mail</strong></th>
    <th width="15%" align="center" bgcolor="#bddcf4">操作</th>
  </tr>
  <cc2:CustomRepeater ID="rptData" runat="server" >
    <ItemTemplate>
  <tr>
    <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
    <td align="center" bgcolor="#e3f1fc" class="pandl3"><%# Eval("ArchiveNo")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("UserName")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# (EyouSoft.Model.EnumType.CompanyStructure.Sex)Eval("ContactSex") %></td>
    <td align="center" bgcolor="#e3f1fc"><%# string.Format("{0:yyyy-MM-dd}", Eval("BirthDate"))%></td>
    <td align="center" bgcolor="#e3f1fc"><%# GetDepartmentByList(Eval("DepartmentList"))%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("DutyName")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# GetWorkYear((DateTime?)Eval("EntryDate"))%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("ContactTel")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("ContactMobile")%></td>
    <td align="center" bgcolor="#e3f1fc"><%# Eval("Email")%></td>
    <td align="center" bgcolor="#e3f1fc">
        <a <%# EditFlag?"":"style='display:none'" %> 
            href="/administrativeCenter/personnelFiles/Edit.aspx?PersonnelID=<%# Eval("Id") %>">修改</a>
        <span <%# EditFlag&&DeleteFlag?"":"style='display:none'" %>>|</span>
        <a <%# DeleteFlag?"":"style='display:none'" %>
            href="javascript:void(0);" onclick="PDefault.DeletePersonnelInfo('<%# Eval("Id") %>');return false;">删除</a>
        <span <%# EditFlag||DeleteFlag?"":"style='display:none'" %>>|</span>
        <a href="javascript:void(0);" onclick="PDefault.GetDetailInfo('<%# Eval("Id") %>');return false;">查看</a></td>
  </tr>
  </ItemTemplate>
  <AlternatingItemTemplate>
  <tr>
    <td  align="center" bgcolor="#BDDCF4"><%=GetCount() %></td>
    <td align="center" bgcolor="#BDDCF4" class="pandl3"><%# Eval("ArchiveNo")%></td>
    <td align="center" bgcolor="#BDDCF4"><%# Eval("UserName")%></td>
    <td align="center" bgcolor="#BDDCF4"><%# (EyouSoft.Model.EnumType.CompanyStructure.Sex)Eval("ContactSex") %></td>
    <td align="center" bgcolor="#BDDCF4"><%# string.Format("{0:yyyy-MM-dd}", Eval("BirthDate"))%></td>
    <td align="center" bgcolor="#BDDCF4"><%# GetDepartmentByList(Eval("DepartmentList"))%></td>
    <td align="center" bgcolor="#BDDCF4"><%# Eval("DutyName")%></td>
    <td align="center" bgcolor="#BDDCF4"><%# GetWorkYear((DateTime?)Eval("EntryDate"))%></td>
    <td align="center" bgcolor="#BDDCF4"><%# Eval("ContactTel")%></td>
    <td align="center" bgcolor="#BDDCF4"><%# Eval("ContactMobile")%></td>
    <td align="center" bgcolor="#BDDCF4"><%# Eval("Email")%></td>
    <td align="center" bgcolor="#BDDCF4">
       <a <%# EditFlag?"":"style='display:none'" %> 
            href="/administrativeCenter/personnelFiles/Edit.aspx?PersonnelID=<%# Eval("Id") %>">修改</a>
        <span <%# EditFlag&&DeleteFlag?"":"style='display:none'" %>>|</span>
        <a <%# DeleteFlag?"":"style='display:none'" %>
            href="javascript:void(0);" onclick="PDefault.DeletePersonnelInfo('<%# Eval("Id") %>');return false;">删除</a>
        <span <%# EditFlag||DeleteFlag?"":"style='display:none'" %>>|</span>
        <a href="javascript:void(0);" onclick="PDefault.GetDetailInfo('<%# Eval("Id") %>');return false;">查看</a></td>
  </tr>
  </AlternatingItemTemplate>
  </cc2:CustomRepeater>
</table>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="right" class="pageup">
        <cc1:ExporPageInfoSelect  ID="ExporPageInfoSelect1" runat="server" />
    </td>
  </tr>
</table>