<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AwakeShow.aspx.cs" Inherits="Web.UserCenter.WorkAwake.AwakeShow" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
<head runat="server">
<script type="text/javascript" src="/js/jquery.1.5.2.min.js"></script>
<script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
<script src="/js/utilsUri.js" type="text/javascript"></script>
<script type="text/javascript">
    //查询按钮事件
    function search() {
        var _departObj = eval("<%=txtQueryDepart.ClientID %>");
        var _operatorObj = eval("<%=txtQueryOperator.ClientID %>");

        var params = utilsUri.getUrlParams([]);
        params["lsdate"]=$("#txtLeaveSDate").val();
        params["ledate"]=$("#txtLeaveEDate").val();
        params["departids"]=_departObj.GetId();
        params["operatorids"]=_operatorObj.GetOperatorId();

        window.location.href = window.location.pathname + "?" + $.param(params);
        return false;
    }

    //导出
    function toXls() {
        var params = utilsUri.getUrlParams([]);
        params["recordcount"] = recordCount;
        params["istoxls"] = 1;

        if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

        window.location.href = utilsUri.createUri(null, params);
        return false;
    }

    $(document).ready(function() {
        $("#btnSearch").bind("click", function() { search(); return false; });
    });
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width:98%; margin:0px auto;">
        <table cellpadding="0" cellspacing="0" style="width:100%; margin-top:10px; margin-bottom:10px; border:0px;">
            <tr>
                <td>
                    <label>出团时间：</label>
                    <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput" name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                    
                    <label>部门：</label>
                    <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="txtQueryDepart" runat="server" Style="width:100px;" />
                    <uc2:selectOperator Title="操作员：" ID="txtQueryOperator" runat="server" Style="width:100px;" />
                    <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a>
                    <a onclick="return toXls();return false;" href="javascript:void(0);"><img width="68" height="27" style="margin-top:0px;border:0px" alt="" src="/images/export.xls.png"></a>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0px auto;">
            <tr class="odd">
                <td width="100" height="30" align="center">
                    团号
                </td>
                <td width="100" align="center">
                    线路名称
                </td>
                <td width="100" align="center">
                    出团日期
                </td>
                <td width="100" align="right">
                    总金额&nbsp;&nbsp;
                </td>
                <td width="100" align="right">
                    <%= type == "Appect" ? "待收金额" : "未付金额"%>&nbsp;&nbsp;
                </td>
            </tr>
            <asp:Repeater ID="repList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <font class="fbred">
                                <%# Eval(type == "Appect" ? "TourNo" : "TourCode")%></font>
                        </td>
                        <td align="center">
                            <%#Eval(type == "Appect" ?"TourClassId" :"TourType").ToString() == "单项服务" ? "单项服务" : Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%#Eval(type == "Appect" ? "TourClassId" : "TourType").ToString() == "单项服务" ? "" : Convert.ToDateTime(Eval("LeaveDate")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td style="text-align:right">
                            <%# Eval(type == "Appect" ? "FinanceSum" : "TotalAmount", "{0:c2}")%>&nbsp;&nbsp;
                        </td>
                        <td align="center" style="text-align:right;">
                            <%# Eval(type == "Appect" ? "NotReceived" : "Arrear", "{0:c2}")%>&nbsp;&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            
            <asp:PlaceHolder runat="server" ID="phDaiShouKuanHeJi" Visible="false">
              <tr bgcolor="#BDDCF4">
                <td colspan="4" style="text-align:right;"><b>合计：</b></td>
                <td style="text-align:right"><asp:Literal runat="server" ID="ltrDaiShouKuanHeJi"></asp:Literal>&nbsp;&nbsp;</td>
              </tr>
            </asp:PlaceHolder>
            
            <%if (len == 0)
              { %>
            <tr align="center">
                <td colspan="5">
                    没有相关数据
                </td>
            </tr>
            <%} %>
            <tr>
                <td height="30" colspan="5" align="right" class="pageup">
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
    </form>
</body>
</html>
