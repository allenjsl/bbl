<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master"
    Title="按时间统计_利润统计_统计分析" CodeBehind="ProTimeStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.ProfitStatistic.ProTimeStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc4" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc5" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    <form id="form1" runat="server">

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/utilsUri.js"></script>

    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 利润统计 >> 按时间统计
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
            <li><a href="/StatisticAnalysis/ProfitStatistic/ProAreaStatisticList.aspx" id="two1">
                按区域统计</a></li>
            <li><a href="/StatisticAnalysis/ProfitStatistic/ProDepartmentStatisticList.aspx"
                id="two2">按部门统计</a></li>
            <li><a href="/StatisticAnalysis/ProfitStatistic/ProTypeStatisticList.aspx" id="two3">
                按类型统计</a></li>
            <li><a href="/StatisticAnalysis/ProfitStatistic/ProTimeStatisticList.aspx" id="two4"
                class="tabtwo-on">按时间统计</a></li>
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
                        <div class="searchbox">
                            <uc4:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                            <uc5:RouteAreaList ID="RouteAreaList1" runat="server" />
                            <%--销售员--%>
                            <uc1:selectOperator Title="团队操作人：" ID="SelectOperator" runat="server" />
                            <label>
                                操作人部门：</label>
                            <uc6:UCSelectDepartment ID="UCSelectDepartment1" SetPicture="/images/sanping_04.gif"
                                runat="server" />
                            &nbsp;
                            <br />
                            <label>
                                出团日期：</label>
                            <input type="text" class="searchinput" onfocus="WdatePicker()" runat="server" id="txtLeaveStarDate"
                                name="txtLeaveStarDate" />
                            至<input type="text" runat="server" class="searchinput" onfocus="WdatePicker()" id="txtLeaveEndDate"
                                name="txtLeaveEndDate" />
                            <label>
                                <label>
                                    核算日期：</label>
                                <input type="text" runat="server" class="searchinput" onfocus="WdatePicker()" id="txtcheckStartDate"
                                    runat="server" name="txtcheckStartDate" />
                                至<input type="text" runat="server" class="searchinput" onfocus="WdatePicker()" id="txtcheckEndDate"
                                    runat="server" name="txtcheckEndDate" />
                                <img id="btnSearch" src="/images/searchbtn.gif" style="vertical-align: top; cursor: pointer;" /></label>
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
                            <uc3:UCPrintButton ContentId="tbl_ProTimeStaList" ID="UCPrintButton1" runat="server" />
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
            <div class="tablelist" id="divProTimeStaList">
                <table width="100%" border="0" id="tbl_ProTimeStaList" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="14%" align="center" bgcolor="#BDDCF4">
                            日期
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            团队数
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            总收入
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            总支出
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            团队毛利
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            人均毛利
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            利润分配
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            公司利润
                        </th>
                        <th width="9%" align="center" bgcolor="#bddcf4">
                            利润率
                        </th>
                    </tr>
                    <cc2:CustomRepeater ID="crp_GetProTimeList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CurrYear")%>年<%#Eval("CurrMonth") %>月
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetProTourNumList(<%#Eval("CurrYear") %>,<%#Eval("CurrMonth") %>)">
                                        <%#Eval("TourNum")%></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("TourPeopleNum")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("GrossIncome","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("GrossOut", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("TourGross","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleGross", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("TourShare","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("CompanyShare","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("LiRunLv", "{0:c2}")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CurrYear")%>年<%#Eval("CurrMonth") %>月
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetProTourNumList(<%#Eval("CurrYear") %>,<%#Eval("CurrMonth") %>)">
                                        <%#Eval("TourNum")%></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("TourPeopleNum")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("GrossIncome","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("GrossOut", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("TourGross","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("PeopleGross", "{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("TourShare","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("CompanyShare","{0:c2}")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("LiRunLv", "{0:c2}")%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc2:CustomRepeater>
                    <tr class="odd">
                        <th>
                            总计
                        </th>
                        <th>
                            <asp:Literal ID="lt_teamNum" runat="server"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="lt_peopleSum" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_InMoney" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_outMoney" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_teamgross_profit" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_pepolegross_profit" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_profitallot" runat="server"></asp:Literal>
                        </th>
                        <th>
                            ￥<asp:Literal ID="lt_comProfit" runat="server"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="lt_lirunlv" runat="server"></asp:Literal>
                        </th>
                    </tr>
                </table>
                <table width="100%" id="tbl_ExportPage" runat="server" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </table>
            </div>
            <cc2:CustomRepeater ID="crp_PrintProTimeStaList" Visible="false" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="1" id="table1" cellpadding="0" cellspacing="1">
                        <tr>
                            <th width="14%" align="center">
                                月份
                            </th>
                            <th width="8%" align="center">
                                团队数
                            </th>
                            <th width="8%" align="center">
                                人数
                            </th>
                            <th width="9%" align="center">
                                总收入
                            </th>
                            <th width="9%" align="center">
                                总支出
                            </th>
                            <th width="9%" align="center">
                                团队毛利
                            </th>
                            <th width="9%" align="center">
                                人均毛利
                            </th>
                            <th width="9%" align="center">
                                利润分配
                            </th>
                            <th width="9%" align="center">
                                公司利润
                            </th>
                            <th width="9%" align="center">
                                利润率
                            </th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%#Eval("CurrMonth") %>
                        </td>
                        <td align="center">
                            <a href="javascript:void(0);" onclick='return GetProTourNumList(<%#Eval("CurrYear") %>,<%#Eval("CurrMonth") %>)'>
                                <%#Eval("TourNum")%></a>
                        </td>
                        <td align="center">
                            <%#Eval("TourPeopleNum")%>
                        </td>
                        <td align="center">
                            <%#Eval("GrossIncome","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("GrossOut", "{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourGross","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleGross", "{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourShare","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("CompanyShare","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("LiRunLv", "{0:c2}")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td align="center">
                            <%#Eval("CurrMonth") %>
                        </td>
                        <td align="center">
                            <a href="javascript:void(0);" onclick='return GetProTourNumList(<%#Eval("CurrYear") %>,<%#Eval("CurrMonth") %>)'>
                                <%#Eval("TourNum")%></a>
                        </td>
                        <td align="center">
                            <%#Eval("TourPeopleNum")%>
                        </td>
                        <td align="center">
                            <%#Eval("GrossIncome","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("GrossOut", "{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourGross","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleGross", "{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourShare","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("CompanyShare","{0:c2}")%>
                        </td>
                        <td align="center">
                            <%#Eval("LiRunLv", "{0:c2}")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </cc2:CustomRepeater>
            <%--统计图开始--%>
            <div class="tablelist" id="divCartogram" style="display: block;">
                <p id="danwei" style="display: none; margin-left: 180px; font-size: 10px">
                    单位：万元</p>
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
    </form>

    <script type="text/javascript">
        $(function() {   
            $("#btnSearch").click(function() {
                var params = { TourType: "", 
                OperatorName: "",
                OperatorId:"", 
                leaveTourStarDate: "" ,
                leaveTourEndDate:"",
                checkStartDate:"",
                checkEndDate:"",
                RouteAreaName:""};
                params.RouteAreaName=<%=RouteAreaList1.ClientID %>.GetAreaId();
                params.TourType=<%=TourTypeList1.ClientID %>.GetTourTypeId();
                params.leaveTourStarDate=$.trim($("#<%=txtLeaveStarDate.ClientID %>").val());
                params.leaveTourEndDate=$.trim($("#<%=txtLeaveEndDate.ClientID %>").val());
                params.checkStartDate=$.trim($("#<%=txtcheckStartDate.ClientID %>").val());
                params.checkEndDate=$.trim($("#<%=txtcheckEndDate.ClientID %>").val());
                params.OperatorName=<%=SelectOperator.ClientID %>.GetOperatorName();
                params.OperatorId=<%=SelectOperator.ClientID %>.GetOperatorId();
                params.deptnames=<%=UCSelectDepartment1.ClientID %>.GetName();
                params.deptids=<%=UCSelectDepartment1.ClientID %>.GetId();
                window.location.href="/StatisticAnalysis/ProfitStatistic/ProTimeStatisticList.aspx?" + $.param(params);
                return false;
            });
             
        $("#btnExport").click(function(){
                var params = utilsUri.getUrlParams(["page"]);
	            params["recordcount"] = recordCount;
	            params["isExport"] = 1;

	            if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

	            window.open(utilsUri.createUri(null, params));
	            return false;
        });  
             $("#btnCartogram").click(function(){//统计图  
                 $("#divProTimeStaList").hide();
                 $("#divCartogram").show();
                 $("#danwei").show();
                 var getXml="<%=GetCartogramFlashXml() %>";
                 if(getXml!="")
                 {
                      var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn2D.swf", "chart1Id", "750", "300", "0", "1");	   			
			          routeAreaStaticList.setDataXML(getXml);			
			          routeAreaStaticList.render("divContainer");		
                 }
                 else
                 {
                     $("#divCartogram").css("display","block");
                     $("#divContainer").html("暂时没有数据统计！");
                 }
			     return false;
            }); 
            $("#btnPerStaTbl").click(function(){//统计表
                $("#divProTimeStaList").show();
                $("#divCartogram").hide();
            });  
        });
    </script>

    <script type="text/javascript">
        function GetProTourNumList(CuYear, CuMoth) {
            var params = {
                IsByTime: 1,
                CuYear: CuYear, //年份
                CuMoth: CuMoth, //月份
                TourType: '<%=TourType %>', //团队类型
                leaveTourStarDate: '<%=leaveTourStartDate %>', //出团开始日期
                leaveTourEndDate: '<%=leaveTourEndDate %>', //出团结束日期
                checkStartDate: '<%=checkStartDate %>', //核算开始日期
                checkEndDate: '<%=checkEndDate %>', //核算结束日期
                OperatorId: '<%=OperatorId %>',
                RouteAreaName: '<%=RouteAreaName %>',//线路区域ID
                DepartId:'<%=DepartId %>'
            };
            $("#divContainer").hide();
            //var url = "/StatisticAnalysis/ProfitStatistic/GetTourDetailList.aspx";
            var url = "<%=URL%>";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "线路明细列表",
                modal: true,
                data: params,
                width: "760px",
                height: "500px",
                afterHide: function() {
                    $("#divContainer").show();
                }
            });
            return false;
        }
    </script>

</asp:Content>
