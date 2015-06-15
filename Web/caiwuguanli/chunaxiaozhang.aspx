<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chunaxiaozhang.aspx.cs"
    Inherits="Web.caiwuguanli.chunaxiaozhang" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <title>出纳登帐-销账弹出框</title>
    <link type="text/css" rel="stylesheet" href="../css/sytle.css">

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form runat="server" id="myform">
    <table width="720px" cellspacing="1" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td>
                    <div class="searchbox" style="width: 100%;">
                        <label>
                            客户单位：</label>
                        <input type="text" class="searchinput searchinput02" id="txt_com" name="txt_com"
                            runat="server"><input type="hidden" id="hd_comId" name="hd_comId" runat="server" /><a
                                href="/CRM/customerservice/SelCustomer.aspx?backfun=selectTeam" class="selectTeam"><img
                                    width="28" height="18" src="../images/sanping_04.gif"></a>
                        <label>
                            <asp:ImageButton ID="ImageButton1" runat="server"  style=" vertical-align:middle;"
                            ImageUrl="../images/searchbtn.gif" onclick="ImageButton1_Click" /></label>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <table width="720px" cellspacing="1" cellpadding="0" border="0" align="center">
        <tbody>
            <tr class="odd">
                <td width="70px" height="30" bgcolor="#bddcf4" align="center">
                    <input type="checkbox" id="chkAll" name="chkAll" />
                    全选
                </td>
                <th width="70px">
                    销账金额
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    团号
                </th>
                <th width="100px" bgcolor="#bddcf4">
                    线路名称
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    出团时间
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    订单号
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    客户单位
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    应收金额
                </th>
                <th width="80px" bgcolor="#bddcf4">
                    未收金额
                </th>
            </tr>
            <asp:Repeater ID="rpt_list" runat="server">
                <ItemTemplate>
                    <tr class="<%=i%2==0?"even":"odd" %>">
                        <td width="70px" height="30" align="center">
                            <input type="hidden" name="hd_orderId" value="<%#Eval("ID") %>" /><input type="hidden"
                                name="hd_money" value="<%#Eval("NotReceived") %>" /><input type="hidden" value="<%#Eval("OrderNo") %>"
                                    name="hd_orderNo" /><input type="hidden" value="<%#Eval("BuyCompanyID") %>" name="hd_comId"><input
                                        type="checkbox" id="chk" class="chk" name="chk" value="<%=i %>">
                        </td>
                        <td width="70px" align="center">
                            <input type="text" class="searchinput txt_money" name="txt_money" id="txt_money"
                                value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotReceived").ToString()) %>" />
                        </td>
                        <td width="100px" align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td width="80px" align="center">
                            <%#Eval("RouteName")%>
                        </td>
                        <td width="80px" align="center">
                            <%#DateTime.Parse( Eval("LeaveDate").ToString()).ToString("yyyy-MM-dd")%>
                        </td>
                        <td width="80px" align="center">
                            <%#Eval("OrderNo")%>
                        </td>
                        <td width="80px" align="center">
                            <%#Eval("BuyCompanyName")%>
                        </td>
                        <td width="80px" align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("SumPrice").ToString())%></font>
                        </td>
                        <td width="80px" align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotReceived").ToString())%></font>
                        </td>
                    </tr>
                    <%i++; %>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="even">
                <td height="30" align="right" class="pageup" colspan="9">
                    <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                </td>
            </tr>
        </tbody>
    </table>
    <table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">销账</asp:LinkButton>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
    </table>
    </form>

    <script>
    function selectTeam(id, name) {
           $("#<%=hd_comId.ClientID %>").val(id);
           $("#<%=txt_com.ClientID %>").val(name);
       }
    $(function() {
     $("#txt_com").click(function() {
               $(".selectTeam").click();
           });
            $(".selectTeam").click(function() {
               var iframeId = '<%=Request.QueryString["iframeId"] %>';
               var url = $(this).attr("href");
               parent.Boxy.iframeDialog({
                   iframeUrl: url,
                   title: "选用组团社",
                   modal: true,
                   width: "820px",
                   height: "520px",
                   data: {
                       desid: iframeId,
                       backfun: "selectTeam"
                   }
               });
               return false;

               ///根据选择的组团社绑定报价等级
           });
        var money = parseFloat( "<%=Request.QueryString["money"] %>");
        $("#chkAll").click(function() {
            //$("#chkAll").attr("checked", true);
            $(".chk").attr("checked", !$(".chk").attr("checked"));
            return true;
        });
        $("#<%=LinkButton1.ClientID%>").click(function() {
            var isb = false;
            var summoney = 0 ;
            $(".chk").each(function() {
                if ($(this).attr("checked")){
                    var curMoney= parseFloat($(".txt_money").eq($(this).val()).val());
                    summoney += curMoney;
                    if(curMoney<=0){alert("销账金额必须大于零!");return false;}
                    isb = true;
                    }
            });
            if (!isb) {
                alert("请选择要销账的记录!"); return false;
            }else
            {
                if(summoney >money)
                {
                    alert("销账总额不能大于到账金额!");
                    return false;
                }
            }
        });
        $(".txt_money").change(function() {
           if(parseFloat($(this).val())<=0)
           {
              alert("销账金额必须大于零!");
           }
           if(parseFloat($(this).val())>money)
           {
                alert("销账金额不能大于到账金额!");
           }
        });
    });
</script>

</body>
</html>
