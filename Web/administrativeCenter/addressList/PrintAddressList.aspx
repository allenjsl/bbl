<%@ Page Language="C#" MasterPageFile="/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="PrintAddressList.aspx.cs" Inherits="Web.administrativeCenter.addressList.PrintAddressList" ValidateRequest="false"  Title="内部通讯录_行政中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
<table width="695" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse; line-height:16px; margin:10px auto;">
   <tr>
    <th width="7%" align="center"><strong>序号</strong></th>
    <th width="7%" align="center" ><strong>姓名</strong></th>
    <th width="12%" align="center" ><strong>部门</strong></th>
    <th width="11%" align="center" ><strong>电话</strong></th>
    <th width="10%" align="center" ><strong>手机</strong></th>
    <th width="13%" align="center" ><strong>E-mail</strong></th>
    <th width="8%" align="center" ><strong>QQ</strong></th>
    <th width="14%" align="center" ><strong>MSN</strong></th>
  </tr>
  <cc1:CustomRepeater ID="crptPrintAddressList" runat="server">
    <ItemTemplate>
          <tr>
            <td  align="center"><%=++i%></td>
            <td align="center" ><%# Eval("UserName")%></td>
            <td align="center" ><%# GetDepartmentString( Eval("DepartmentList")) %></td>
            <td align="center"><%# Eval("ContactTel")%></td>
            <td align="center" ><%# Eval("ContactMobile")%></td>
            <td align="center" ><%# Eval("Email")%></td>
            <td align="center" ><%# Eval("QQ")%></td>
            <td align="center" ><%# Eval("MSN")%></td>
          </tr>
    </ItemTemplate>
  </cc1:CustomRepeater>
</table>
</asp:Content>