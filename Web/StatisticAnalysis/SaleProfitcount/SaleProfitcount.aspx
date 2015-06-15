<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="SaleProfitcount.aspx.cs" Inherits="Web.StatisticAnalysis.SaleProfitAccount.SaleProfitcount"
    Title="销售利润统计" %>

<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/ProvinceAndCity.ascx" TagName="ProvinceAndCity"
    TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/utilsUri.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 统计分析 >> 销售利润统计
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <form id="form1" method="post" runat="server">
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <%--出团时间、部门、销售员、线路区域、区域查询(客户单位所在省份城市)。--%>
                    <div class="searchbox">
                        &nbsp;出团日期
                        <input name="txtLeaveStartDate" type="text" onfocus="WdatePicker()" class="searchinput"
                            id="txtLeaveStartDate" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("leavesdate") %>' />至<input
                                name="txtLeaveEndDate" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("leaveedate") %>'
                                type="text" onfocus="WdatePicker()" class="searchinput" id="txtLeaveEndDate" />
                        <label>
                            部门：</label>
                        <span id="department">
                            <uc2:UCSelectDepartment ID="UCSelectDepartment1" SetPicture="/images/sanping_04.gif"
                                runat="server" />
                        </span>&nbsp;
                        <%--销售员--%>
                        <span id="seller">
                            <uc1:selectOperator Title="销售员：" ID="SelectOperator" runat="server" />
                        </span>
                        <br />
                        &nbsp;<span id="routearea">
                            <uc3:RouteAreaList ID="RouteAreaList1" runat="server" />
                        </span>&nbsp;<span id="PandC"><uc4:ProvinceAndCity ID="ProvinceAndCity1" runat="server" />
                        </span>

                        <script type="text/javascript">
                            window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                        </script>

                        &nbsp;
                        <img id="btnSearch" src="/images/searchbtn.gif" alt="" style="vertical-align: top;
                            cursor: pointer;" />
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        </form>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="left">
                        <uc3:UCPrintButton ContentId="tbl_ProAreaStaList" ID="UCPrintButton1" runat="server" />
                    </td>
                    <td width="90" align="left">
                        <a href="javascript:void(0);" id="daochu">
                            <img src="/images/daoru.gif" alt="" />
                            导 出 </a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist" id="divProAreaStaList">
            <table width="100%" border="0" id="tbl_ProAreaStaList" cellpadding="0" cellspacing="1">
                <asp:Repeater ID="rptlist" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th width="8%" align="center" bgcolor="#bddcf4">
                                销售员
                            </th>
                            <th width="14%" align="center" bgcolor="#BDDCF4">
                                团队数
                            </th>
                            <th width="9%" align="center" bgcolor="#bddcf4">
                                人数
                            </th>
                            <th width="9%" align="center" bgcolor="#bddcf4">
                                收入
                            </th>
                            <th width="9%" align="center" bgcolor="#bddcf4">
                                利润
                            </th>
                            <th width="9%" align="center" bgcolor="#bddcf4">
                                人均利润
                            </th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="even">
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("XiaoShouYuanName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("TuanDuiShu")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("RenShu") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("ShouRuJinE", "{0:c2}")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("LiRun", "{0:c2}")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("RenJunLiRun", "{0:c2}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="odd">
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("XiaoShouYuanName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("TuanDuiShu")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("RenShu") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("ShouRuJinE", "{0:c2}")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("LiRun", "{0:c2}")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("RenJunLiRun", "{0:c2}")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                <tr class="even" runat="server" id="TrTotal">
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">合计:</font>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">
                            <label id="lbTeamNum" runat="server">
                            </label>
                        </font>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">
                            <label id="lbPeopleNum" runat="server">
                            </label>
                        </font>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">
                            <label id="lbShouRu" runat="server">
                            </label>
                        </font>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">
                            <label id="lbProfit" runat="server">
                            </label>
                        </font>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <font class="fred" style="font-weight: bold">
                            <label id="lbAvgProfit" runat="server">
                            </label>
                        </font>
                    </td>
                </tr>
                <asp:Literal ID="lbemptydata" runat="server" Visible="false"></asp:Literal>
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
    </div>

    <script type="text/javascript" language="javascript">
        $(function(){
            ProfitPage.PageInit();
            $("#daochu").click(function(){
                    var params = utilsUri.getUrlParams(["page"]);
	                params["recordcount"] = <%=recordCount %>;
	                params["isExport"] = 1;
	                if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

	                window.open(utilsUri.createUri(null, params));
	                return false;
                });  
        })
        var ProfitPage={
            PageInit:function(){
                $("#btnSearch").click(function(){
                    ProfitPage.search();
                })
                $("input").keydown(function(event) {
                if (event.keyCode == 13) {
                    ProfitPage.search();
                }
                });
                ProfitPage.initSearch();
            },
            search: function() {
                var params={leavesdate:"",leaveedate:"",department:"",departid:"",seller:"",sellerid:"",routearea:"",province:"",city:"",provincename:"",cityname:""};
                //执行查询
                params.leavesdate = $("#txtLeaveStartDate").val(); //出团开始时间
                
                params.leaveedate=$("#txtLeaveEndDate").val();//出团结束时间
                params.department=<%=UCSelectDepartment1.ClientID %>.GetName();//部门名称
                params.departid=<%=UCSelectDepartment1.ClientID %>.GetId();//部门ID
                var sellObj=<%=SelectOperator.ClientID %>;
                params.seller=sellObj.GetOperatorName();
                params.sellerid=sellObj.GetOperatorId();
                var routeObj= <%=RouteAreaList1.ClientID %>;
                params.routearea=routeObj.GetAreaId();
                params.province = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().id;
                params.provincename = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().name;
                params.cityname=window["<%=ProvinceAndCity1.ClientID %>"].getCity().name;
                params.city = window["<%=ProvinceAndCity1.ClientID %>"].getCity().id;
                var theUrl = "/StatisticAnalysis/SaleProfitcount/SaleProfitcount.aspx?"+ $.param(params);
                window.location.href = theUrl;
                return false;
            },
            initSearch:function(){
                var _params = utilsUri.getUrlParams([]);
                 window["<%=ProvinceAndCity1.ClientID %>"].setProvince(decodeURIComponent(_params["province"] || ""), decodeURIComponent(_params["provincename"] || ""));
         window["<%=ProvinceAndCity1.ClientID %>"].setCity(decodeURIComponent(_params["city"] || ""), decodeURIComponent(_params["cityname"] || ""));
            }
        
        }
    </script>

</asp:Content>
