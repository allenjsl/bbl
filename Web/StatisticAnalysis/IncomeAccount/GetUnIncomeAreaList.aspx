<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetUnIncomeAreaList.aspx.cs"
    Inherits="Web.StatisticAnalysis.IncomeAccount.GetUnIncomeAreaList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jquery.js" type="text/javascript"></script>

    <script src="../../js/jquery.boxy.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="btnbox">
        <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
                <td width="90" align="left">
                    <a href="javascript:void(0);" style="line-height: 16px;" id="btnPrint">打 印 </a>
                </td>
                <td width="90" align="left">
                    <a href="javascript:void(0);" id="btnExport">
                        <img src="/images/daoru.gif" />
                        导 出 </a>
                </td>
            </tr>
        </table>
    </div>
    <div id="divIncAreaList">
        <table width="100%" id="tbl_UnIncomeAreaStaList" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <th width="10%" align="center" bgcolor="#BDDCF4">
                    线路名称
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    团号
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    出团日期
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    客户单位
                </th>
                <th align="center" bgcolor="#bddcf4">
                    联系电话
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    人数
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    责任销售
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    总收入
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    已收
                </th>
                <th width="10%" align="center" bgcolor="#bddcf4">
                    未收
                </th>
            </tr>
            <cc1:CustomRepeater ID="crp_UnIncomeAreaList" runat="server">
                <ItemTemplate>
                    <tr class="list">
                        <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("RouteName")%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <%#GetRouteName("1", Convert.ToString(Eval("RouteName")), Convert.ToString(Eval("tourclassid")), Convert.ToString(Eval("LeaveDate", "{0:yyyy-MM-dd}")))%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("BuyCompanyName")%>
                        </td>
                         <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("ContactTel")%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("PeopleNumber")%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <%#Eval("SalerName")%>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <font class="fred">￥
                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("HasCheckMoney")))%></font>
                        </td>
                        <td align="center" bgcolor="#e3f1fc">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("RouteName")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("TourNo")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#GetRouteName("1",Convert.ToString(Eval("RouteName")),Convert.ToString(Eval("OrderType")),Convert.ToString(Eval("LeaveDate","{0:yyyy-MM-dd}")))%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("BuyCompanyName")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                            <%#Eval("ContactTel")%>
                        </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("PeopleNumber")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("SalerName")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <font class="fred">￥
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("HasCheckMoney")))%></font>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font>
                    </td>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <tr>
                <td rowspan="2" bgcolor="#BDDCF4" align="center">
                    <b>总计:</b>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center">
                    <b>总收入</b>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center">
                    <b>已收</b>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center">
                    <b>未收</b>
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoneny)%>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(hasGetMoney) %>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoneny-hasGetMoney) %>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" id="tbl_ExportPage" runat="server" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="right">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="TourType" runat="server" />
    <asp:HiddenField ID="RouteArea" runat="server" />
    <asp:HiddenField ID="SalserId" runat="server" />
    <asp:HiddenField ID="LeaveTourStartDate" runat="server" />
    <asp:HiddenField ID="LeaveTourEndDate" runat="server" />
    <asp:HiddenField ID="Company" runat="server" />
    </form>

    <script type="text/javascript">
        $("#btnPrint").click(function() {
            if ($(".list").length < 0) {
                alert("当前无数据,无法打印");
            } else {
                var data = { RouteArea: $("#<%=RouteArea.ClientID %>").val(), TourType: $("#<%=TourType.ClientID %>").val(), SalserId: $("#<%=SalserId.ClientID %>").val(), LeaveTourStartDate: $("#<%=LeaveTourStartDate.ClientID %>").val(), LeaveTourEndDate: $("#<%=LeaveTourEndDate.ClientID %>").val(), Company: $("#<%=Company.ClientID %>").val() }; //参数传递
                var url = "/StatisticAnalysis/IncomeAccount/IncomeAreaPrint.aspx";
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "打印",
                    modal: true,
                    width: "750px",
                    height: "550px",
                    data: data

                });
            }

        })
        $("#btnExport").click(function() {
            if ($(".list").length < 0) {
                alert("当前无数据,无法导出");
            } else {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&act=toexcel"; } else { url += "?act=toexcel"; }
                window.location.href = url;
                return false;
            }
        })
        

    </script>

</body>
</html>
