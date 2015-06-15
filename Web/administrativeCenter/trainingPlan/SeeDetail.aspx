<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeeDetail.aspx.cs" Inherits="Web.administrativeCenter.trainingPlan.SeeDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top:20px;">
      <tr class="odd">
        <th width="17%" height="30" align="right">计划标题：</th>
        <td height="30" colspan="3" bgcolor="#E3F1FC" class="pandl3">
            <asp:Label ID="lbl_PlanTitle" runat="server" Text=""></asp:Label></td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right" valign="middle">计划内容：</th>
        <td colspan="3" bgcolor="#E3F1FC" class="pandl4">
            <asp:Label ID="lbl_PlanContent" runat="server" Text=""></asp:Label></td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right">发送对象 ：</th>
        <td height="30" colspan="3" bgcolor="#E3F1FC" class="pandl4">
            <asp:Label ID="lbl_AcceptPersonnel" runat="server" Text=""></asp:Label></td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right">发布人：</th>
        <td width="32%" bgcolor="#E3F1FC" class="pandl3">
            <asp:Label ID="lbl_OperatorName" runat="server" Text=""></asp:Label></td>
        <th width="17%" height="30" align="right">发布时间：</th>
        <td width="34%" bgcolor="#E3F1FC" class="pandl3">
            <asp:Label ID="lbl_IssueTime" runat="server" Text=""></asp:Label></td>
	  </tr>
</table>
    </form>
</body>
</html>
