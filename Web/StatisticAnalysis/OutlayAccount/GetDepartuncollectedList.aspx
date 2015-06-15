<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetDepartuncollectedList.aspx.cs"
    Inherits="Web.StatisticAnalysis.OutlayAccount.GetDepartuncollectedList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="printButton" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    </head>
<body>
    <form id="form1" runat="server">
    <div class="btnbox">
    <table>
    <tr><td>
                            <uc1:printButton ContentId="tbl_OutDepartStaList" runat="server" ID="printButton1">
                            </uc1:printButton></td><td>
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                导 出 </a></td></tr>
    </table></div>
    <div>  
            <cc1:CustomRepeater ID="crp_GetDepartUncollList" runat="server">
            <HeaderTemplate>
        <table width="780" border="0" id="tbl_OutDepartStaList" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
          
            <tr class="odd">
                <th height="30" align="center">
                    线路名称
                </th>
                <th align="center">
                    团号
                </th>
                <th align="center">
                    出团日期
                </th>
                <th align="center">
                    供应商
                </th>
                <th align="center">
                    操作员
                </th>
                <th align="center">
                    总支出
                </th>
                <th align="center">
                    已付
                </th>
                <th align="center">
                    未付
                </th>
            </tr>
            </HeaderTemplate>
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "单项服务" : Eval("RouteName")%>
                        </td>
                        <td align="center" style="vnd.ms-excel.numberformat:@">
                            <%#Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType")==EyouSoft.Model.EnumType.TourStructure.TourType.单项服务?"":Eval("LeaveDate", "{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                            <%#Eval("Supplier")%>
                        </td>
                        <td align="center">
                            <%#Eval("Operator")%>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( Convert.ToDecimal(Eval("TotalAmount")))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">
                               ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( Convert.ToDecimal(Eval("TotalAmount"))-Convert.ToDecimal(Eval("Arrear")))%>
                            </font>
                        </td>
                        <td align="center">
                         <font class="fred">
                                 ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("Arrear")))%></font>
                           </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td height="30" align="center">
                            <%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "单项服务" : Eval("RouteName")%>
                        </td>
                        <td align="center" style="vnd.ms-excel.numberformat:@">
                            <%#Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "" : Eval("LeaveDate", "{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                            <%#Eval("Supplier")%>
                        </td>
                        <td align="center">
                            <%#Eval("Operator")%>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( Convert.ToDecimal(Eval("TotalAmount")))%></font>
                        </td>
                       <td align="center">
                            <font class="fred">
                               ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( Convert.ToDecimal(Eval("TotalAmount"))-Convert.ToDecimal(Eval("Arrear")))%>
                            </font>
                        </td>
                        <td align="center">
                         <font class="fred">
                                 ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("Arrear")))%></font>
                           </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                            <tr>
                <td rowspan="2" bgcolor="#BDDCF4" align="center">
                    <b>总计:</b>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center">
                    <b>总支出</b>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center">
                    <b>已付</b>
                </td>
                <td colspan="2" bgcolor="#BDDCF4" align="center">
                    <b>未付</b>
                </td>
            </tr>
            <tr>
                <td colspan="3" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoney)%>
                </td>
                <td colspan="3" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoney-arrear)%>
                </td>
                <td colspan="2" bgcolor="#BDDCF4" align="center" style="color: Red">
                    ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(arrear) %>
                </td>
            </tr>
        </table>
                
                </FooterTemplate>
            </cc1:CustomRepeater>
        <table width="100%" border="0" id="tbl_ExportPage" runat="server" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="right">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script type="text/javascript">
        $(function() {

            $("#btnExport").click(function() {//导出
                if ($("#tbl_OutDepartStaList").find("[id='EmptyData']").length > 0) {
                    alert("暂无数据无法执行导出！");
                    return false;
                }
                var goToUrl = "<%=Request.Url.ToString() %>&isExport=1&isAll=1";
                window.open(goToUrl, "Excel导出");
                return false;
            });
        });
    </script>
</body>
</html>
