<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZhangLingAnKeHuDanWei.aspx.cs" Inherits="Web.StatisticAnalysis.Ageanalysis.ZhangLingAnKeHuDanWei" MasterPageFile="~/masterpage/Back.Master" Title="按客户单位统计_账龄分析_统计分析" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>

<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
    <script src=" /js/utilsUri.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
        //search
        function search() {
            var params = {"kehumingcheng": $.trim($("#txtKeHuMingCheng").val())
                , "sorttype": 0
                , "lsdate": $("#txtLeaveSDate").val()
                , "ledate": $("#txtLeaveEDate").val()
            };

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }

        //order 
        function order(_v) {
            var params = utilsUri.getUrlParams(["page"]);

            if (params["sorttype"] == _v) {alert("当前数据已经按此规则排序");  return false };
            
            params["sorttype"] = _v;

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
        
        //to xls
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
</asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    

    <form runat="server" id="form1">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 帐龄分析
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000"></td>
                </tr>
            </table>
        </div>
        <div class="hr_10"></div>
        <ul class="fbTab">
            <li><a href="AgeDepartmentStatisticList.aspx" id="two1">按销售员统计</a></li>
            <li><a href="javascript:void(0)" id="two2" class="tabtwo-onw130">按客户单位统计</a></li>
            <li style="clear:both"></li>
        </ul>
        <div class="hr_10"></div>
        <div>
            <div class="btnbox" style="height: 10px; margin: 0px; padding: 0px"></div>
            <table cellpadding="0" cellspacing="0" style="width: 100%; margin-top: 10px; margin-bottom: 10px; border: 0px;">
                <tr>
                    <td>
                        <label>客户单位：</label>
                        <input type="text" name="txtKeHuMingCheng" id="txtKeHuMingCheng" class="searchinput searchinput02" style="width: 150px" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kehumingcheng") %>" />
                        <label>出团时间：</label>
                        <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput" name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" style="width:100px" />-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" style="width: 100px" />
                        <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a>
                        <a onclick="toXls();return false;" href="javascript:void(0);"><img width="68" height="27" style="margin-top: 0px; border: 0px" alt="" src="/images/export.xls.png"></a>
                    </td>
                </tr>
            </table>
            <div class="tablelist">          
                <asp:PlaceHolder runat="server" ID="ph" Visible="false">    
                    <table cellpadding="0" cellspacing="1" style="border:0px;width:100%">
                        <tr style="background: #BDDCF4">
                            <td style="width:40%;text-align:center; height:30px">
                                <b>客户单位</b>
                            </td>
                            <td style="width: 30%; text-align: right;">
                                <b><a onclick="return order(1)" style="color: " href="javascript:void(0)">↑</a>拖欠款总额<a onclick="return order(0)" style="color: " href="javascript:void(0)"> ↓</a>&nbsp;&nbsp;</b>
                            </td>
                            <td style="width: 30%; text-align: center;">
                                <b><a onclick="return order(3)" style="color: " href="javascript:void(0)">↑</a>最长拖欠时间<a onclick="return order(2)" style="color: " href="javascript:void(0)"> ↓</a></b>
                            </td>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt">
                            <ItemTemplate>
                                <tr <%#(Container.ItemIndex + 1)%2==0?"style='background:#bddcf4'":"style='background:#e3f1fc'" %>>
                                <td style="height:30px; text-align:center;"><%#Eval("KeHuName") %></td>
                                <td style="text-align:right;"><%#Eval("WeiShouKuan","{0:C2}")%>&nbsp;&nbsp;</td>
                                <td style="text-align:center;"><%#Eval("EarlyTime","{0:yyy-MM-dd}")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td style="height:30px;text-align:right;"></td>
                            <td style="text-align: right;">合计：<asp:Literal runat="server" ID="ltrWeiShouHeJi"></asp:Literal>&nbsp;&nbsp;</td>
                            <td></td>
                        </tr>
                    </table>
                    <table id="tbl_ExportPage" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="right">
                                <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                
                <asp:PlaceHolder runat="server" ID="phEmpty" Visible="false">
                <div>未统计到任何数据！</div>
                </asp:PlaceHolder>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
