<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="waitkuanPass.aspx.cs" Inherits="Web.caiwuguanli.waitkuanPass" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="/js/jquery.1.5.2.min.js"></script>
    <script type="text/javascript">
        //取消审批或支付 
        //_type=1取消审批
        //_type=2取消支付 
        //_v取消审批或支付的登记编号
        function cancel(_type, _v) {
            var _typeName = "";
            if (_type == 1) _typeName = "取消审批";
            if (_type == 2) _typeName = "取消支付";
            if (_type == 3) _typeName = "确认审批";
            if (_type == 4) _typeName = "确认支付";
            if (_typeName.length < 1) return;
            if (!confirm("你确定要" + _typeName + "吗？")) return;

            var postData = {
                "requestType": "ajax_cancel"
                , "registerId": _v
                , "planId": '<%=EyouSoft.Common.Utils.GetQueryStringValue("receiveid") %>'
                , "tourId": '<%=EyouSoft.Common.Utils.GetQueryStringValue("tourid") %>'
                , "requestTypeId": _type
            };

            $.ajax({
                url: window.location.pathname,
                cache: false,
                data: postData,
                async: false,
                type: "POST",
                success: function(response) {
                    if (response == "0") {
                        alert(_typeName + "成功。");
                        window.location.href = window.location.href;
                    } else {
                        alert(_typeName + "失败，请重试。")
                    }
                }
            }); 
        }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
<table width="800" border="0" align="center" cellpadding="0" cellspacing="1">
              <tr class="odd">
                <th width="13%" height="31" align="center">团号： </th>
                <td width="22%">
                    <asp:Literal ID="lt_teamNum" runat="server"></asp:Literal></td>
                <th width="11%" align="center">线路名称：</th>
                <td colspan="3">
                    <asp:Literal ID="lt_xianluName" runat="server"></asp:Literal></td>
            </tr>
            <tr class="even">
              <th height="30" align="center">天数：</th>
                <td width="22%">
                    <asp:Literal ID="lt_days" runat="server"></asp:Literal></td>
                <th align="center">人数：</th>
              <td width="25%">
                  <asp:Literal ID="lt_pepoleNum" runat="server"></asp:Literal></td>
              <th width="10%">出团时间：</th>
              <td width="19%">
                  <asp:Literal ID="lt_LevelDate" runat="server"></asp:Literal></td>
            </tr>
            <tr class="odd">
              <th height="30" align="center">销售员<strong>：</strong></th>
                <td width="22%"><asp:Literal ID="lt_seller" runat="server"></asp:Literal></td>
                <th align="center">计调员</th>
              <td width="25%"><asp:Literal ID="lt_oprator" runat="server"></asp:Literal></td>
              <td width="10%">&nbsp;</td>
              <td width="19%">&nbsp;</td>
            </tr>
            <tr>
              <th height="30" colspan="6" align="left"><span class="fukuanT">团款收入</span></th>
            </tr>
            <tr>
              <td height="30" colspan="6" align="left"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr class="odd">
                  <th width="9%" height="25" align="center">订单号</th>
                  <th width="20%" align="center">客源单位</th>
                  <th width="12%" align="center">收入金额</th>
                  <th width="14%" align="center">增加费用</th>
                  <th width="11%" align="center">减少费用</th>
                  <th width="11%" align="center">小计</th>
                  <th width="23%" align="center">备注</th>
                </tr>
                <%i = 0; %>
                <cc2:CustomRepeater runat="server" ID="rpt_list1" >
                <ItemTemplate>
                <tr class="<%=i%2==0 ?"even":"odd" %>">
                  <td height="25" align="center"><%#Eval("OrderNo")%></td>
                  <td align="center"><%#Eval("CompanyName")%></td>
                  <td align="center"><font class="fred"><%#Eval("SumPrice","{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("FinanceAddExpense", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("FinanceRedExpense", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("FinanceSum", "{0:c2}")%></font></td>
                  <td align="center"><%#Eval("FinanceRemark")%></td>
                </tr>
                <% i++; %>
                </ItemTemplate>
                </cc2:CustomRepeater>

              </table></td>
            </tr>
             <tr>
              <th height="30" colspan="6" align="left"><span class="fukuanT">其它收入</span></th>
            </tr>
            <tr>
              <td height="30" colspan="6" align="left"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                
                <tr class="odd">
                  <th width="16%" height="25" align="center">收入项目</th>
                  <th width="16%" align="center">收款金额</th>
                  <th width="15%" align="center">增加费用</th>
                  <th width="15%" align="center">减少费用</th>
                  <th width="15%" align="center">小计</th>
                  <th width="23%" align="center">备注</th>
                </tr>
                <%i = 0; %>
                <cc2:CustomRepeater runat="server" ID="rpt_list2" >
                <ItemTemplate>
                <tr class="<%=i%2==0 ?"even":"odd" %>">
                  <td height="25" align="center"><%#Eval("Item")%></td>
                  <td align="center"><font class="fred"><%#Eval("Amount","{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("AddAmount", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("ReduceAmount", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("TotalAmount", "{0:c2}")%></font></td>
                  <td align="center"><%#Eval("Remark")%></td>
               
                <% i++; %>
                </ItemTemplate>
                </cc2:CustomRepeater>
              </table></td>
            </tr>
            <tr>
              <th height="30" colspan="6" align="left"><span class="fukuanT">其它支出</span></th>
            </tr>
            <tr>
              <td height="30" colspan="6" align="left"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr class="odd">
                  <th width="16%" height="25" align="center">支出项目</th>
                  <th width="16%" align="center">支出金额</th>
                  <th width="15%" align="center">增加费用</th>
                  <th width="15%" align="center">减少费用</th>
                  <th width="15%" align="center">小计</th>
                  <th width="23%" align="center">备注</th>
                </tr>
                <%i = 0; %>
                <cc2:CustomRepeater runat="server" ID="rpt_list3" >
                <ItemTemplate>
                <tr class="<%=i%2==0 ?"even":"odd" %>">
                  <td height="25" align="center"><%#Eval("Item")%></td>
                  <td align="center"><font class="fred"><%#Eval("Amount", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("AddAmount", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("ReduceAmount", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("TotalAmount", "{0:c2}")%></font></td>
                  <td align="center"><%#Eval("Remark")%></td>
                </tr>
                <% i++; %>
                </ItemTemplate>
                </cc2:CustomRepeater>
              </table></td>
            </tr>
            <tr>
              <th height="30" colspan="6" align="left"><span class="fukuanT">利润分配</span></th>
            </tr>
            <tr>
              <td height="30" colspan="6" align="left"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr class="odd">
                  <th width="19%" height="25" align="center">分配项目</th>
                  <th width="20%" align="center">分配金额</th>
                  <th width="19%" align="center">部门</th>
                  <th width="19%" align="center">人员</th>
                  <th width="23%" align="center">备注</th>
                </tr>
                <%i = 0; %>
                <cc2:CustomRepeater runat="server" ID="rpt_list4" >
                <ItemTemplate>
                <tr class="<%=i%2==0 ?"even":"odd" %>">
                  <td height="25" align="center"><%#Eval("ShareItem")%></td>
                  <td align="center"><font class="fred"><%#Eval("ShareCost", "{0:c2}")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("DepartmentName")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("Saler")%></font></td>
                  <td align="center"><%#Eval("Remark")%></td>
                </tr>
                <% i++; %>
                </ItemTemplate>
                </cc2:CustomRepeater>
              </table></td>
            </tr>
             
            
             <tr>
              <th height="30" colspan="6" align="left"><span class="fukuanT">当前付款申请</span></th>
            </tr>
            <tr>
              <td height="30" colspan="6" align="left"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
                <tr class="odd">
                  <th width="15%" height="25" align="center">收款单位</th>
                  <th width="15%" align="center">付款申请人</th>
                  <th width="10%" align="center">申请时间</th>
                  <th width="15%" align="center">付款金额</th>
                  <th width="28%" align="center">备注</th>
                  <th width="17%" align="center">操作</th>
                </tr>
                <%i = 0; %>
                <cc2:CustomRepeater runat="server" ID="rpt_list5" >
                <ItemTemplate>
                <tr class="<%=i%2==0 ?"even":"odd" %>">
                  <td height="25" align="center"><%#Eval("ReceiveCompanyName")%></td>
                  <td align="center"><font class="fred"><%#Eval("StaffName")%></font></td>
                  <td align="center"><font class="fred"><%#Eval("PaymentDate", "{0:d}")%></font></td>
                  <td align="center"><font class="fred"> <%#Eval("PaymentAmount", "{0:c2}") %></font></td>
                  <td align="center"><%#Eval("Remark")%></td>
                  <td align="center"><%#getUrl(bool.Parse(Eval("IsChecked").ToString()), bool.Parse(Eval("IsPay").ToString()), Eval("RegisterId").ToString(), Eval("ItemId").ToString(),Eval("ReceiveId").ToString())%></td>
                </tr>
                <% i++; %>
                </ItemTemplate>
                </cc2:CustomRepeater>
              </table></td>
            </tr>
</table>
<table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td align="center" class="tjbtn02"><a href="javascript:void(0);" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();window.parent.location.href=window.parent.location.href;return false;">关闭</a></td>
  </tr>
</table>

    </div>
    </form>
</body>
</html>
