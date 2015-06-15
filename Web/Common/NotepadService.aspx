<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotepadService.aspx.cs" Inherits="Web.Common.NotepadService" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="812" cellspacing="1" cellpadding="0" border="0" align="center">
            <tbody>
                <tr class="odd">
                    <th width="86" height="30" bgcolor="#bddcf4" align="center">
                        选择
                    </th>
                    <th width="86" bgcolor="#bddcf4" align="center">
                        内容
                    </th>
                    <th width="130" bgcolor="#bddcf4" align="center">
                        操作
                    </th>
                </tr>  
    <asp:Repeater runat="server" ID="rptList">
    <ItemTemplate>
     <tr class="even">
    <td><input type=checkbox /></td>
    <td><%# Eval("text") %></td>
                </tr>
    </ItemTemplate>
    </asp:Repeater>
     </table>
    <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
    <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="40" align="center"></td>
            <td height="40" align="center" class="tjbtn02"><a href="#" id="selectxl">选用线路</a></td>
          </tr>
      </table>
    </div>
    </form>
</body>
</html>
