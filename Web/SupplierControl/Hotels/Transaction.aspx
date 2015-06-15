<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transaction.aspx.cs" Inherits="Web.SupplierControl.Hotels.Transaction" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <script src="/js/jquery.js" type="text/javascript"></script>
      <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style=" margin:10px;">
          <tr class="odd">
            <td width="100" height="30" align="center">团号</td>
            <td width="100" align="center">线路名称</td>
            <td width="100" align="center">出团日期</td>
            <td width="100" align="center">人数</td>
            <td width="100" align="center">计调员</td>
            <td width="100" align="center">返利</td>
            <td width="100" align="center">结算费用</td>
          </tr>
          <asp:Repeater ID="replist" runat="server">
            <ItemTemplate>
                 <tr class="even">
                    <td height="30" align="center"><font class="fbred"></font></td>
                    <td align="center"></td>
                    <td align="center"></td>
                    <td align="center"></td>
                    <td align="center"></td>
                    <td align="center">%</td>
                    <td align="center"><font class="fbred">￥</font></td>
                </tr>
            </ItemTemplate>
          </asp:Repeater>     
          <%if(len==0){ %> <tr align="center"><td colspan="7">没有相关数据</td></tr><%} %>   
          <tr>
            <td height="30" colspan="7" align="right" class="pageup">
               <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="4" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
            </td>
          </tr>
    </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="40" align="center" class="tjbtn02"><a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">关闭</a></td>
          </tr>
        </table>
    </div>
    </form>
</body>
</html>
