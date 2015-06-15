<%@ Page Language="C#" MasterPageFile="/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="PrintDetail.aspx.cs" Inherits="Web.administrativeCenter.bylaw.PrintDetail" Title="规章制度_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="695" border="0" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse; line-height:16px; margin:10px auto;">
      <tr>
        <td height="24" align="left"> 编 号： <%=DutyNo %></td>
      </tr>
      <tr>
        <td align="center" valign="middle" class="pandl4"><strong><%=DutyTitle %></strong></td>
      </tr>
      <tr>
        <td align="left" valign="middle" class="pandl4"><p style=" text-indent:28px; line-height:24px;"><%=Content %></p></td>
      </tr>
    </table>
    <div id="noData" runat="server" style="width:695; border-color:#000000; border-width:1px; text-align:center;" visible="false">
            没有该信息！
    </div>
</asp:Content>
