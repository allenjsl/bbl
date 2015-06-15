<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/masterpage/Back.Master"
    CodeBehind="PerAreaStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.PersonTime.PerAreaStatisticList"
    Title="按区域统计_人次统计_统计分析" %>

<%@ Import Namespace="EyouSoft.Common" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
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
                        所在位置>>统计分析 >> 人次统计>>按区域统计
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
            <li><a href="/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx" id="two1" class="tabtwo-on">
                按区域统计</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx" id="two2">
                按部门统计</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx" id="two3">按时间统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <%-- 查询条件开始--%>
                        <div class="searchbox">
                            <label>
                                部门：</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
                            <uc2:selectOperator Title="销售员：" ID="selectSals" runat="server" />
                            <label>
                                出团时间：</label>
                            <input name="txtAreaStarTime" type="text" runat="server" class="searchinput" id="txtAreaStarTime"
                                onfocus="WdatePicker()" />
                            至
                            <input name="txtAreaEndTime" type="text" runat="server" class="searchinput" id="txtAreaEndTime"
                                onfocus="WdatePicker()" />&nbsp;
                            <label>计划类型：</label>
                            <select name="txtTourType" id="txtTourType">
                                <%=TourTypeSearchOptionHTML%>
                            </select>&nbsp;
                            <label>
                                <input id="btnSearchByArea" src="/images/searchbtn.gif" style="vertical-align: top;
                                    cursor: pointer;" type="image" value="button" alt="查询" />
                            </label>                            
                        </div>
                        <%--查询条件结束--%>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <%-- 操作开始--%>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc3:UCPrintButton ContentId="tbl_PerAreaStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a id="linkExport" href="javascript:void(0);">
                                <img src="/images/daoru.gif" />导出</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnPerStaTbl">统计表</a>
                        </td>
                        <td width="90" align="left">
                            <a href='/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx' id="linkCartogram">
                                统计图</a>
                        </td>
                    </tr>
                </table>
            </div>
            <%--操作结束--%>
            <%--列表数据显示开始--%>
            <div class="tablelist" id="divPerAreaList" runat="server">
                <table width="100%" id="tbl_PerAreaStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="20%" align="center" bgcolor="#BDDCF4">
                            线路区域
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            销售员
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            计调员
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th align="center" bgcolor="#bddcf4">团队数</th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_PerAreaStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("AreaName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#Eval("AreaName")%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                        <td colspan="3" align="right" bgcolor="#BDDCF4" style="font-weight: bold;">
                            合计
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                            <%= AllPopleNum %>
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;"><%= TuanDuiShuHeJi %></td>
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
            <%-- 列表数据结束 --%>
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
        </div>
    </div>
    <%-- 打印的Reapete容器开始，不显示--%>
    <cc1:CustomRepeater ID="crp_PrintPerAreaStaList" runat="server" Visible="false">
        <HeaderTemplate>
            <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="16%" align="center">
                        线路区域
                    </th>
                    <th width="12%" align="center">
                        销售员
                    </th>
                    <th width="12%" align="center">
                        计调员
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
                    <%#Eval("AreaName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
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
                    <%#Eval("AreaName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
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
    <%-- 打印的Reapete容器结束--%>

    <script type="text/javascript">
        function GetParams(IsCartogram) { 
            var params = {DepartId:"",DepartName:"", StartTime: "", EndTime: "", Salser: "", SalserId: "",IsCartogram:"",tourtype:"" };
            var SalerControlObj = <%=selectSals.ClientID %>;//获取销售员控件
            var Salser=SalerControlObj.GetOperatorName();//销售员名字
            var SalsID=SalerControlObj.GetOperatorId();//销售员ID
            var StartTime=$.trim($("#<%=txtAreaStarTime.ClientID%>").val());//开始时间
            var EndTime=$.trim($("#<%=txtAreaEndTime.ClientID%>").val());//结束时间  
            params.DepartId=<%=UCselectDepart.ClientID %>.GetId();//获取部门ID 
            params.DepartName=<%=UCselectDepart.ClientID %>.GetName();//获取部门名字
            params.Salser=Salser;
            params.SalserId=SalsID;
            params.StartTime=StartTime;
            params.EndTime=EndTime;
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            params.tourtype=$("#txtTourType").val();
            return params;
        }
    $(function() {    
        $("#btnSearchByArea").click(function() {//查询
                window.location.href = "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx?" + $.param(GetParams());
                return false;
        });       
        $("#linkExport").click(function(){//导出
               if($("#tbl_PerAreaStaList").find("[id='EmptyData']").length>0)
               {
                    alert("暂无数据无法执行导出！");
                    return false;
               }
              var goToUrl = "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx?isExport=1";
              window.open(goToUrl,"Excel导出");
              return false;
        });  
         $("#linkCartogram").click(function(){//统计图 
             $("#<%= divPerAreaList.ClientID %>").hide();
             $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx",
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
            $("#<%=divPerAreaList.ClientID%>").show();
            $("#divContainer").hide();
        });
    });   
    </script>

    </form>
</asp:Content>
