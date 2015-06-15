<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master" Title="账龄分析_统计分析"
    CodeBehind="AgeDepartmentStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.Ageanalysis.AgeDepartmentStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <form id="form1" runat="server">
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
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
       <%-- <ul class="fbTab">
            <li><a href="/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx" id="two2"
                class="tabtwo-on">按部门统计</a></li>
            <div class="clearboth">
            </div>
        </ul>--%>
        <ul class="fbTab">
            <li><a href="javascript:void(0)" id="two1" class="tabtwo-on">
                按销售员统计</a></li>
            <li><a href="ZhangLingAnKeHuDanWei.aspx" id="two2" class="aw130">
                按客户单位统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div>
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <uc3:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                            <uc4:RouteAreaList ID="RouteAreaList1" runat="server" />
                            <%--销售员--%>
                            <uc1:selectOperator ID="SelectSalers" runat="server" />
                            <br />
                            <label>
                                出团日期：</label>
                            <input name="txtStartDate" runat="server" type="text" onfocus="WdatePicker()" class="searchinput"
                                id="txtStartDate" />至<input name="txtEndDate" onfocus="WdatePicker()" type="text"
                                    runat="server" class="searchinput" id="txtEndDate" />
                            <label>
                                <img id="btnSearch" src="/images/searchbtn.gif" style="vertical-align: top;cursor:pointer;" /></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc2:UCPrintButton ContentId="tbl_GetAgeDepartStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                        <!-- <td width="90" align="left"><a href="xsfxrent_list.html">统计表</a> </td>-->
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divAgeDepartStaList" runat="server">
                <table width="100%" id="tbl_GetAgeDepartStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="19%" align="center" bgcolor="#BDDCF4">
                            销售员
                        </th>
                        <th width="19%" align="center" bgcolor="#bddcf4">
                            拖欠款总额
                        </th>
                        <th width="19%" align="center" bgcolor="#bddcf4">
                            最长拖欠时间
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_GetAgeDepartStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetTotalArrea(<%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorId%>)"
                                        id="link1">
                                        <%#Eval("ArrearageSum","{0:c2}")%></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetTotalArrea(<%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorId%>)"
                                        id="link1">
                                        <%#Eval("ArrearageSum","{0:c2}")%></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                </table>
                <table id="tbl_ExportPage" runat="server" width="100%" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <cc1:CustomRepeater ID="crp_PrintGetAgeDepartStaList" Visible="false" runat="server">
        <HeaderTemplate>
            <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="19%" align="center">
                        销售员
                    </th>
                    <th width="19%" align="center">
                        拖欠款总额
                    </th>
                    <th width="19%" align="center">
                        最长拖欠时间
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                        <%#Eval("ArrearageSum","{0:c2}")%>
                </td>
                <td align="center">
                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                        <%#Eval("ArrearageSum","{0:c2}")%>
                </td>
                <td align="center">
                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </cc1:CustomRepeater>
    </form>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnSearch").click(function() {
                var params = { tourType: "",routeAreaName:"", Salser:"",SalserId: "" ,startDate:"",endDate:""};
                var SalserControlObj = <%=SelectSalers.ClientID %>; //获取销售控件对象
                params.tourType = <%=TourTypeList1.ClientID %>.GetTourTypeId(); //团队类型
                params.Salser = SalserControlObj.GetOperatorName(); //责任销售名字
                params.SalserId = SalserControlObj.GetOperatorId(); //责任销售ID
                params.routeAreaName=<%=RouteAreaList1.ClientID %>.GetAreaId();//线路区域
                params.startDate=$.trim($("#<%=txtStartDate.ClientID %>").val());//出团开始日期
                params.endDate=$.trim($("#<%=txtEndDate.ClientID %>").val());//出团结束日期
                window.location.href = "/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx?" + $.param(params);
            });           
            $("#btnExport").click(function() {//导出
            if ($("#tbl_GetAgeDepartStaList").find("[id='EmptyData']").length>0) {
                    alert("暂无数据无法执行导出！");
                    return false;
                }
                var goToUrl = "/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx?isExport=1";
                window.open(goToUrl, "Excel导出");
                return false;
            });             
        });
    </script>

    <script type="text/javascript">
             ///拖欠款总金额
             function GetTotalArrea(SalerId){
             var data={tourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(),routeAreaName:<%=RouteAreaList1.ClientID %>.GetAreaId(),startDate:$.trim($("#<%=txtStartDate.ClientID %>").val()),endDate:$.trim($("#<%=txtEndDate.ClientID %>").val()),SalerId:SalerId};
					var url = "/StatisticAnalysis/Ageanalysis/GetArrearageList.aspx";
					Boxy.iframeDialog({
						iframeUrl:url,
						title:"拖欠款总额详情",
						modal:true,
						data:data,
						width:"750px",
						height:"740px"
					});
					return false;
		    }
    </script>

</asp:Content>
