<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogDetail.aspx.cs" Inherits="Web.systemset.systemlog.LogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="16%" height="30" align="right">操作员：</th>
        <td height="30" colspan="4" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litOperator"></asp:Literal></td>
      </tr>
	  <tr class="odd">
        <th width="16%" height="30" align="right">部门：</th>
        <td colspan="4" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litDepart"></asp:Literal></td>
      </tr>
	  <tr class="odd">
        <th width="16%" height="30" align="right">操作模块：</th>
        <td colspan="4" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litModule"></asp:Literal></td>
      </tr>
	  <tr class="odd">
        <th width="16%" height="30" align="right">操作内容：</th>
        <td colspan="4" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litContent"></asp:Literal></td>
      </tr>
	  <tr class="odd">
        <th width="16%" height="30" align="right">操作时间：</th>
        <td width="80%" height="30" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litTime"></asp:Literal></td>
        <td width="4%" colspan="3" bgcolor="#E3F1FC" class="pandl3">&nbsp;</td>
      </tr>
	  <tr class="odd">
        <th width="16%" height="30" align="right">IP：</th>
        <td colspan="4" bgcolor="#E3F1FC" class="pandl3"><asp:Literal  runat="server" ID="litIP"></asp:Literal> </td>
      </tr>
</table>
    </div>
    </form>
</body>
</html>
