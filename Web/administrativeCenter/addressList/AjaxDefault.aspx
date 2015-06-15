<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxDefault.aspx.cs" Inherits="Web.administrativeCenter.addressList.AjaxDefault" %>

<%@ Register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<table width="100%" border="0" cellpadding="0" cellspacing="1">
      <tr>
        <th width="7%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
        <th width="7%" align="center" bgcolor="#BDDCF4"><strong>姓名</strong></th>
        <th width="12%" align="center" bgcolor="#bddcf4"><strong>部门</strong></th>
        <th width="11%" align="center" bgcolor="#bddcf4"><strong>电话</strong></th>
        <th width="10%" align="center" bgcolor="#bddcf4"><strong>手机</strong></th>
        <th width="13%" align="center" bgcolor="#bddcf4"><strong>E-mail</strong></th>
        <th width="8%" align="center" bgcolor="#bddcf4">QQ</th>
        <th width="14%" align="center" bgcolor="#bddcf4">MSN</th>
      </tr>
      <cc1:CustomRepeater ID="crptAddressList" runat="server">
        <ItemTemplate>
            <tr>
                <td  align="center" bgcolor="#e3f1fc"><%=GetCount()%></td>
                <td align="center" bgcolor="#e3f1fc" class="pandl3"><%# Eval("UserName")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# GetDepartmentString( Eval("DepartmentList")) %></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("ContactTel")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("ContactMobile")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("Email")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("QQ")%></td>
                <td align="center" bgcolor="#e3f1fc"><%# Eval("MSN")%></td>
             </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td  align="center" bgcolor="#bddcf4"><%=GetCount()%></td>
                <td align="center" bgcolor="#bddcf4" class="pandl3"><%# Eval("UserName")%></td>
                <td align="center" bgcolor="#bddcf4"><%# GetDepartmentString( Eval("DepartmentList")) %></td>
                <td align="center" bgcolor="#bddcf4"><%# Eval("ContactTel")%></td>
                <td align="center" bgcolor="#bddcf4"><%# Eval("ContactMobile")%></td>
                <td align="center" bgcolor="#bddcf4"><%# Eval("Email")%></td>
                <td align="center" bgcolor="#bddcf4"><%# Eval("QQ")%></td>
                <td align="center" bgcolor="#bddcf4"><%# Eval("MSN")%></td>
            </tr>
        </AlternatingItemTemplate>
      </cc1:CustomRepeater>
  </table>
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td align="right" class="pageup">
        <td align="right" class="pageup"><cc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" /></td>
      </tr>
  </table>
