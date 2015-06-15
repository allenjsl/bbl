<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/masterpage/Back.Master"
    Title="按时间统计_人次统计_统计分析" CodeBehind="PerTimeStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.PersonTime.PerTimeStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 人次统计>>按时间统计
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
            <li><a href="/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx" id="two2">
                按部门统计</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx" id="two3" class="tabtwo-on">
                按时间统计</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <!--按时间统计-->
        <div id="con_two_2">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <uc4:RouteAreaList ID="RouteAreaList1" runat="server" />
                            <label>
                                部门：</label><uc3:UCSelectDepartment ID="UCSelectDepartment1" runat="server" SetPicture="/images/sanping_04.gif" />
                            <uc1:selectOperator Title="销售员：" ID="SelectSalser" runat="server" />&nbsp;
                            <label>计划类型：</label>
                            <select name="txtTourType" id="txtTourType">
                                <%=TourTypeSearchOptionHTML%>
                            </select>&nbsp;
                            <label>
                                <input id="ImgbtnSearchByTime" src="/images/searchbtn.gif" style="vertical-align: top;
                                    cursor: pointer;" type="image" value="查询" />
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
                            <uc2:UCPrintButton ContentId="tbl_PerTimeStaList" ID="UCPrintButton1" runat="server" />
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
                            <a href="javascript:void(0);" id="linkCartogram">统计图</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divPerTimeList" runat="server">
                <table width="100%" id="tbl_PerTimeStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th align="center" bgcolor="#BDDCF4">
                            月份
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th align="center" bgcolor="#bddcf4">
                            团队数
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_PerTimeStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CurrYear")%>年<%#Eval("CurrMonth")%>月
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc"><%#Eval("TuanDuiShu") %></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CurrYear")%>年<%#Eval("CurrMonth")%>月
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4"><%#Eval("TuanDuiShu") %></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                        <td align="right" bgcolor="#BDDCF4" style="font-weight: bold;">
                            合计
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                            <%= AllPopleNum %>
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;"><%=TuanDuiShuHeJi %></td>
                    </tr>
                </table>
                <table id="tblExporPageSelect" runat="server" width="100%" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc3:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--统计图开始--%>
        <div class="tablelist" id="divCartogram" style="display: block;">
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
        <cc1:CustomRepeater ID="crp_PrintPerTimeStaList" runat="server" Visible="false">
            <HeaderTemplate>
                <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                    <tr>
                        <th align="center">
                            月份
                        </th>
                        <th align="center">
                            人数
                        </th>
                        <td align="center">团队数</td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("CurrYear")%>年<%#Eval("CurrMonth")%>月
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center"><%#Eval("TuanDuiShu") %></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("CurrYear")%>年<%#Eval("CurrMonth")%>月
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center"><%#Eval("TuanDuiShu") %></td>
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
           var params = {DepartName:"",DepartId:"",RouteAreaId:"",Salser:"",SalserId:"",IsCartogram:"",tourtype:""};
           var SalerControlObj = <%=SelectSalser.ClientID %>;//获取销售员控件
           var Salser=SalerControlObj.GetOperatorName();//销售员名字          
           params.RouteAreaId=<%=RouteAreaList1.ClientID%>.GetAreaId();      //线路区域
           params.Salser=Salser;          
           params.DepartName=<%=UCSelectDepartment1.ClientID %>.GetName();//获取部门名字
           params.DepartId=<%=UCSelectDepartment1.ClientID %>.GetId();//获取部门ID ;
           params.SalserId=SalerControlObj.GetOperatorId(); //销售员ID  
           if(IsCartogram!=null)
           {
                params.IsCartogram=IsCartogram;
           }
           params.tourtype=$("#txtTourType").val();
            return params; 
        }
        $(function() { 
            $("#ImgbtnSearchByTime").click(function() {//查询
                       window.location.href = "/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx?" + $.param(GetParams());
                       return false;
            });
            $("#btnExport").click(function() {//导出
                if ($("#tbl_PerTimeStaList").find("[id='EmptyData']").length > 0) {
                    alert("暂无数据无法执行导出！");
                    return false;
                }
                var goToUrl = "/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx?isExport=1";
                window.open(goToUrl, "Excel导出");
                return false;
            });
            $("#linkCartogram").click(function() {//统计图  
                 $("#divCartogram").css("display", "block");
                 $("#<%= divPerTimeList.ClientID %>").hide();
                 $("#divContainer").show();
                 $.newAjax({
                         type: "GET",
                         dataType: 'text',
                         url: "/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx",
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
            $("#btnPerStaTbl").click(function() {//统计表
                $("#<%= divPerTimeList.ClientID %>").css("display", "block");
                $("#divCartogram").css("display", "none");
            });

        }); 
    </script>

</asp:Content>
