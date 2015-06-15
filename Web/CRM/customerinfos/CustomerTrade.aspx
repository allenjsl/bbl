<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerTrade.aspx.cs"
    Inherits="Web.CRM.customerinfos.CustomerTrade" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/utilsUri.js"></script>

    <style type="text/css">
        table td
        {
            height: 30px;
            width: 12%;
            text-align: center;
        }
        table tr.head
        {
            background: #BDDCF4;
            font-weight: bold;
        }
        table td.money
        {
            text-align: right;
        }
        table td.money span
        {
            padding-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 98%; margin: 0px auto; text-align: left; margin-top: 5px; margin-bottom: 5px;">
        <label>
            出团开始时间：</label>
        <input type="text" onfocus="WdatePicker()" id="txtLSDate" class="searchinput" name="txtLSDate"
            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />
        <label>
            出团结束时间：</label>
        <input type="text" onfocus="WdatePicker()" id="txtLEDate" class="searchinput" name="txtLEDate"
            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
        欠款金额：
        <select name="txtQueryAmountOperator" id="txtQueryAmountOperator">
            <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None %>">
                请选择</option>
            <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于 %>">
                大于等于</option>
            <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于 %>">
                等于</option>
            <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于 %>">
                小于等于</option>
        </select>
        <input type="text" id="txtQueryAmount" name="txtQueryAmount" class="searchinput"
            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("queryAmount") %>" />
        <label>
            <a href="javascript:void(0);" id="a_search">
                <img border="0" src="/images/searchbtn.gif" alt="" valign="middel" /></a>
        </label>
        <a onclick="toXls();return false;" href="###">
            <img width="68" height="27" style="margin-top: 0px; border: 0px" alt="导出" src="/images/export.xls.png"></a>
    </div>
    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0px auto;">
        <tr class="odd head">
            <td>
                序号
            </td>
            <td>
                团号
            </td>
            <td style="width: 16%">
                线路名称
            </td>
            <td>
                订单号
            </td>
            <td>
                对方操作员
            </td>
            <td>
                对方团号
            </td>
            <td>
                报团人次
            </td>
            <td class="money">
                <span>交易金额</span>
            </td>
            <td class="money">
                <span>已收金额</span>
            </td>
            <td class="money">
                <span>欠款金额</span>
            </td>
        </tr>
        <asp:CustomRepeater ID="rptTrade" runat="server">
            <ItemTemplate>
                <tr <%#(Container.ItemIndex + 1)%2==0?"class='even'":"class='odd'" %>>
                    <td>
                        <%#(PageSize*(PageIndex-1))+Container.ItemIndex+1 %>
                    </td>
                    <td>
                        <%# Eval("TourCode")%>
                    </td>
                    <td>
                        <%# Eval("RouteName")%>
                    </td>
                    <td>
                        <%# Eval("OrderCode")%>
                    </td>
                    <td>
                        <%#Eval("BuyerContactName")%>
                    </td>
                    <td>
                        <%#Eval("BuyerTourCode")%>
                    </td>
                    <td>
                        <%# Eval("RenCi") %>
                    </td>
                    <td class="money">
                        <span>
                            <%#Eval("JiaoYiJinE","{0:C2}")%></span>
                    </td>
                    <td class="money">
                        <span>
                            <%#Eval("YiShouJinE", "{0:C2}")%></span>
                    </td>
                    <td class="money">
                        <span>
                            <%#Eval("WeiShouJinE", "{0:C2}")%></span>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:CustomRepeater>
        <asp:PlaceHolder runat="server" ID="phHeJi" Visible="false">
            <tr class="odd head">
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                 <td>
                    &nbsp;
                </td>
                <td>
                    合计：
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltrRenCiHeJi"></asp:Literal>
                </td>
                <td class="money">
                    <span>
                        <asp:Literal runat="server" ID="ltrJiaoYiJinEHeJi"></asp:Literal></span>
                </td>
                <td class="money">
                    <span>
                        <asp:Literal runat="server" ID="ltrYiShouJinEHeJi"></asp:Literal></span>
                </td>
                <td class="money">
                    <span>
                        <asp:Literal runat="server" ID="ltrWeiShouJinEHeJi"></asp:Literal></span>
                </td>
            </tr>
        </asp:PlaceHolder>
        <tr class="even">
            <td height="30" colspan="10" align="right" class="pageup">
                <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"
                    runat="server"></uc2:ExportPageInfo>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function() {
            $("#a_search").click(function() {
                var params = utilsUri.getUrlParams(["page"]);
                params["lsdate"] = $("#txtLSDate").val();
                params["ledate"] = $("#txtLEDate").val();
                params["queryAmountOperator"] = $("#txtQueryAmountOperator").val();
                params["queryAmount"] = $("#txtQueryAmount").val();
                window.location.href = utilsUri.createUri(null, params);
                return false;
            })
        })

        //to xls
        function toXls() {
            var params = utilsUri.getUrlParams(["page"]);
            params["recordcount"] = recordCount;
            params["istoxls"] = 1;

            if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
    </script>

    </form>
</body>
</html>
