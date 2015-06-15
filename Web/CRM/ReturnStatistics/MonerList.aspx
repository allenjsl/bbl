<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonerList.aspx.cs" Inherits="Web.CRM.ReturnStatistics.MonerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <%@ register assembly="ControlLibrary" namespace="Adpost.Common.ExporPage" tagprefix="cc1" %>

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="hr_10">
    </div>
    <%if (!isnow)
      { %>
    <div class="btnbox">
        <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="90" align="center">
                    <a href="javascript:;" id="pay">支 付</a>
                </td>
            </tr>
        </table>
    </div>
    <%} %>
    <div>
        <table id="print" width="97%" border="0" align="center" cellpadding="0" cellspacing="1"
            style="margin: 10px;">
            <tr bgcolor="#BDDCF4">
                <%if (!isnow)
                  { %>
                <th width="5%" align="center">
                    <input type="checkbox" id="chkAll" />
                </th>
                <%} %>
                <th width="16%" align="center">
                    线路名称
                </th>
                <th width="10%" align="center">
                    团号
                </th>
                <th width="13%" align="center">
                    订单号
                </th>
                <th width="9%" align="center">
                    人数
                </th>
                <th width="7%" align="center">
                    订单金额
                </th>
                <th width="10%" align="center">
                    我社操作员
                </th>
                <th width="10%" align="center">
                    返佣金额
                </th>
            </tr>
            <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                    <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <%if (!isnow)
                          { %>
                        <td align="center">
                            <input type="checkbox" class="chk" getcont="<%#Eval("TourId")%>,<%#Eval("CommissionAmount")%>,<%#Eval("OrderId")%>,<%=customerid %>,<%=contactid %>,<%#Eval("OrderNo")%>"
                                ispaid="<%#Eval("IsPaid")%>" />
                        </td>
                        <%} %>
                        <td align="center">
                            <%#Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%#Eval("OrderNo")%>
                        </td>
                        <td>
                            成人：<%#Eval("AdultNumber")%>人<br />
                            儿童：<%#Eval("ChildrenNumber")%>人
                        </td>
                        <td align="center">
                            <%#Eval("OrderAmount","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("OperatorName")%>
                        </td>
                        <td align="center">
                            <%#Eval("CommissionAmount", "{0:c2}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="30" colspan="7" align="right" class="pageup">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {
            $(".chk[ispaid='True']").each(function() {
                $(this).attr("disabled", true);
            })
            //全选
            $("#chkAll").click(function() {
                chkAll();
            })
            //支付
            $("#pay").click(function() {
                ok();
            })
        })
        function chkAll() {
            $(".chk[ispaid='False']").each(function() {
                $(this).attr("checked", $("#chkAll").attr("checked"));
            })
        }
        function ok() {
            if (confirm("确定支付选择的订单？")) {
                var url = window.location.pathname;
                var cont = "";
                $(".chk:checked").each(function() {
                    cont += $(this).attr("getcont") + ";";
                })
                $("#GetCont").val(cont.substring(0, cont.length - 1));
                if ($("#GetCont").val() != "") {
                    $("#getfrom").submit();
                }
                else {

                    alert("请选择数据！");
                }
            }
        }
    </script>

    </form>
    <form id="getfrom" method="post" action="MonerList.aspx">
    <input type="hidden" id="GetCont" runat="server" name="GetCont" />
    <input type="hidden" id="iframeId" runat="server" name="iframeId" />
    </form>
</body>
</html>
