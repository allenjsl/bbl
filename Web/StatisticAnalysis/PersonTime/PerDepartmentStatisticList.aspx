<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/masterpage/Back.Master" Title="按部门统计_人次统计_统计分析"
    CodeBehind="PerDepartmentStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.PersonTime.PerDepartmentStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

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
                        所在位置>>统计分析 >> 人次统计>>按部门统计
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
        <ul class="fbTab">
            <li><a href="/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx" id="two1">按区域统计</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx" id="two2"
                class="tabtwo-on">按部门统计</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx" id="two3">按时间统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <!-- 按部门统计-->
        <div id="con_two_3" style="display: block;">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">                           
                           <uc4:RouteAreaList  ID="RouteAreaList1" runat="server" />
                            &nbsp; <%-- 销售员--%>
                            <uc2:selectOperator Title="销售员：" ID="SelectSalser" runat="server" />
                            <label>
                                时间段：</label>
                            <input name="txtOrderStarTime" type="text" runat="server" class="searchinput" id="txtOrderStarTime"
                                onfocus="WdatePicker()" />
                            至
                            <input name="txtOrderEndTime" type="text" runat="server" class="searchinput" id="txtOrderEndTime"
                                onfocus="WdatePicker()" />&nbsp;
                            <label>计划类型：</label>
                            <select name="txtTourType" id="txtTourType">
                                <%=TourTypeSearchOptionHTML%>
                            </select>&nbsp;
                            <label>
                                <input id="ImgbtnSearchByDepartment" src="/images/searchbtn.gif" style="vertical-align: top;cursor:pointer;"
                                    type="image" value="button" alt="查询"/>
                            </label>
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
                            <uc3:UCPrintButton ContentId="tbl_PerDepartStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnPerStaTbl">统计表</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnCartogram">统计图</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divPerDepartList" runat="server">
                <table width="100%" id="tbl_PerDepartStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="25%" align="center" bgcolor="#BDDCF4">
                            部门
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            销售员
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4"> 团队数</th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_PerDepartStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                        <td colspan="2" align="right" bgcolor="#BDDCF4" style="font-weight: bold;">
                            合计
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                            <%= AllPopleNum %>
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;"><%=TuanDuiShuHeJi %></td>
                    </tr>
                </table>
                <table width="100%" id="tbl_PageInfo" runat="server" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc3:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--统计图开始--%>
        <div class="tablelist">
            <table id="tbl_Cartogram" width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <div id="divContainer">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%-- 统计图结束--%>
        <cc1:CustomRepeater ID="crp_PrintPerDepartStaList" Visible="false" runat="server">
            <HeaderTemplate>
                <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="10%" align="center">
                            部门
                        </th>
                        <th width="12%" align="center">
                            销售员
                        </th>
                        <th width="10%" align="center">
                            人数
                        </th>
                        <th width="12%" align="center">
                            团队数
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("DepartName")%>
                    </td>
                    <td align="center">
                        <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center">
                        <%#Eval("TuanDuiShu")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("DepartName")%>
                    </td>
                    <td align="center">
                        <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center">
                        <%#Eval("TuanDuiShu")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </cc1:CustomRepeater>
    </div>
    </form>

    <script type="text/javascript">
        function GetParams(IsCartogram) { 
            var params = {RouteAreaId:"", Salser: "", SalserId: "" ,OrderStartTime:"",OrderEndTime:"",IsCartogram:"",tourtype:""};
            var SalerControlObj = <%=SelectSalser.ClientID %>;//获取销售员控件
            var Salser=SalerControlObj.GetOperatorName();//销售员名字
            var SalserId=SalerControlObj.GetOperatorId();//销售员ID
            var RouteAreaId=<%=RouteAreaList1.ClientID %>.GetAreaId();//线路区域
            params.Salser=Salser;
            params.SalserId=SalserId;
            params.RouteAreaId=RouteAreaId;  
            params.OrderStartTime=$("#<%=txtOrderStarTime.ClientID %>").val();    
            params.OrderEndTime=$("#<%=txtOrderEndTime.ClientID %>").val();
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            params.tourtype=$("#txtTourType").val();
            return params;
        }
    $(function() {    
        $("#ImgbtnSearchByDepartment").click(function() {//查询
                    window.location.href = "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx?" + $.param(GetParams());
                    return false;
        });       
        $("#btnExport").click(function(){//导出
                   if($("#tbl_PerDepartStaList").find("[id='EmptyData']").length>0)
                   {
                        alert("暂无数据无法执行导出！");
                        return false;
                   }
                  var goToUrl = "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx?isExport=1";
                  window.open(goToUrl,"Excel导出");
                  return false;
        });  
         $("#btnCartogram").click(function(){//统计图  
             $("#<%= divPerDepartList.ClientID %>").hide();
             $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx",
                     data: GetParams(1),
                     cache: false,
                     success: function(CartogramXml) {
                        if(CartogramXml!="")
                        {
                                var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn2D.swf", "chart1Id", "750", "300", "0", "1");	   			
			                    routeAreaStaticList.setDataXML(CartogramXml);			
			                    routeAreaStaticList.render("divContainer");	
                        }
                        else 
                        {
                            $("#<%= divPerDepartList.ClientID %>").css("display","none");
                            $("#divContainer").html("暂时没有数据统计！");
                        }             	
                     },
                     error: function(xhr, s, errorThrow) {
                         $("#divContainer").html("未能成功获取响应结果")
                     }
                 });
			    return false;
        });  
          $("#btnPerStaTbl").click(function(){//统计表
                $("#divContainer").css("display","none");
                $("#<%= divPerDepartList.ClientID %>").css("display","block");
                $("#divCartogram").css("display","none");
        });        

    });    
    </script>

</asp:Content>
