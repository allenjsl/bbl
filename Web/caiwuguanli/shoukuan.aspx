<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shoukuan.aspx.cs" Inherits="Web.caiwuguanli.shoukuan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"><head>

<meta content="text/html; charset=utf-8" http-equiv="Content-Type">
<title>无标题文档</title>
<link type="text/css" rel="stylesheet" href="/css/sytle.css">
</head><body>
<form runat="server" id="myfrm">
<table width="800" cellspacing="1" cellpadding="0" border="0" align="center">
  <tbody><tr class="odd">
    <th width="86" height="30" bgcolor="#bddcf4" align="center">收款日期</th>
    <th width="86" bgcolor="#bddcf4" align="center">收款人</th>
    <th width="86" bgcolor="#bddcf4" align="center">收款金额</th>
    <th width="94" bgcolor="#bddcf4" align="center">收款方式</th>
    <th width="86" bgcolor="#bddcf4" align="center">开票</th>
    <th width="79" bgcolor="#bddcf4" align="center">开票金额</th>
    <th width="145" bgcolor="#bddcf4" align="center">备注</th>
    <th width="130" bgcolor="#bddcf4" align="center">操作</th>
  </tr>
  <asp:Repeater ID="rpt_list" runat="server" 
          onitemcommand="rpt_list_ItemCommand">
  <ItemTemplate>
  <tr class="even">
    <td height="30" align="center"><input type="hidden" value="<%#Eval("Id") %>" name="hd_id"/><input type="text" name="txt_date" class="searchinput" value="<%#Eval("RefundDate") %>" /></td>
    <td align="center"><input type="text" name="txt_user" class="searchinput" value="<%#Eval("StaffName") %>"/></td>
    <td align="center"><font class="fbred"><input type="hidden" name="hd_skMoney"><%#Eval("RefundMoney")%></font></td>
    <td align="center">
    <input type="hidden" value="<%#Eval("PayCompanyId") %>" name="hd_PayCompanyId"/>
    <input type="hidden" value="<%#Eval("PayCompanyName") %>" name="hd_PayCompanyName" />
              <asp:DropDownList ID="ddl_Type" runat="server">
                <asp:ListItem Value="1">财务现收</asp:ListItem>
                <asp:ListItem Value="2">财务现付</asp:ListItem>
                <asp:ListItem Value="3">导游现收</asp:ListItem>
                <asp:ListItem Value="4">银行电汇</asp:ListItem>
                <asp:ListItem Value="5">转账支票</asp:ListItem>
                <asp:ListItem Value="6">导游现付</asp:ListItem>
                <asp:ListItem Value="7">签单挂账</asp:ListItem>
                <asp:ListItem Value="8">预存款支付</asp:ListItem>
              </asp:DropDownList>
              <input type="hidden" name="IssueTime" value="<%#Eval("IssueTime") %>"/>
    </td>
    <td align="center">
              <asp:DropDownList ID="ddl_piao" runat="server">
                <asp:ListItem Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
              </asp:DropDownList>
              </td>
    <td align="center"><font class="fbred">
    <input type="hidden" name="hd_tkMoney">￥<%#Eval("BillAmount")%></font></td>
    <td align="center">    
       <textarea name="txt_mark"><%#Eval("Remark")%></textarea>
    </td>
    <td align="center"><input type="checkbox" value="<%=k %>" id="checkbox" name="checkbox">
        <asp:LinkButton ID="lnk_Audit" CommandName="update" runat="server">审核</asp:LinkButton>
        <asp:LinkButton ID="lnk_del" CommandName="delete" OnClientClick="return confirm('是否确认删除?')" runat="server">删除</asp:LinkButton>
        </td>
  </tr>
  <%k++; %>
  </ItemTemplate>
  </asp:Repeater>
  
</tbody></table>
<table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
  <tbody><tr>
    <td height="40" align="center"></td>
    <td height="40" align="center" class="tjbtn02">
            <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">保存</asp:LinkButton></td>
    <td align="center" class="tjbtn02"><a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();return false;" id="linkCancel" href="javascript:void(0);">关闭</a></td>
  </tr>
</tbody></table></form>
</body></html>
