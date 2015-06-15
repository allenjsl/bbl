<%@ Page Language="C#"  MasterPageFile="/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="Web.administrativeCenter.personnelFiles.Print" ValidateRequest="false" Title="行政中心_人事档案_打印" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <cc2:CustomRepeater ID="rptDataList" runat="server">
        <HeaderTemplate>
            <table width="695" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse; line-height:16px; margin:10px auto;">
          <tr>
            <td width="30" height="20" align="center"><strong>序号</strong></td>
            <td width="60" align="center"><strong>档案编号</strong></td>
            <td width="45" align="center"><strong>姓名</strong></td>
            <td width="36" align="center"><strong>性别</strong></td>
            <td width="72" align="center"><strong>出生日期</strong></td>
            <td width="67" align="center"><strong>所属部门</strong></td>
            <td width="62" align="center"><strong>职务</strong></td>
            <td width="33" align="center"><strong>工龄</strong></td>
            <td width="85" align="center"><strong>联系电话</strong></td>
            <td width="85" align="center"><strong>手机</strong></td>
            <td width="96" align="center"><strong>E-mail</strong></td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
          <tr>
            <td align="center" valign="middle" class="pandl4"><%=GetCount() %></td>
            <td align="center" class="pandl4"><%# Eval("ArchiveNo")%></td>
            <td align="center" class="pandl4"><%# Eval("UserName")%></td>
            <td align="center" class="pandl4"><%# Eval("ContactSex") %></td>
            <td align="center" class="pandl4"><%# string.Format("{0:yyyy-MM-dd}", Eval("BirthDate"))%></td>
            <td align="center" class="pandl4"><%# GetDepartmentString((Eval("DepartmentList")))%></td>
            <td align="center" class="pandl4"><%# Eval("DutyName")%></td>
            <td align="center" class="pandl4"><%# Eval("WorkYear")%></td>
            <td align="center" class="pandl4"><%# Eval("ContactTel")%></td>
            <td align="center" class="pandl4"><%# Eval("ContactMobile")%></td>
            <td align="center" class="pandl4"><%# Eval("Email")%></td>
          </tr>
        </ItemTemplate>
        <FooterTemplate>
          </table>
        </FooterTemplate>
    </cc2:CustomRepeater>
</asp:Content>

