<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeList.aspx.cs" Inherits="Web.CRM.PersonStatistics.TimeList"
    MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SOperator" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="SDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="c1" runat="server" ID="Content1">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">
                                <%= type == "xianlu" ? "线路产品库" : "客户关系管理"%></span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>><%= type == "xianlu" ? "线路产品库 >> 营销分析" : "客户关系管理 >> 销售分析 >> 人次统计"%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <%if (type != "xianlu")
              { %>
            <div class="lineCategorybox" style="height: 40px;">
                <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                    <tr>
                        <% =Web.Common.AwakeTab.createSale(SiteUserInfo, 127)%>
                    </tr>
                </table>
            </div>
            <%} %>
            <div class="hr_10">
            </div>
            <ul class="fbTab">
                <li><a href="/CRM/PersonStatistics/AreaList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>">
                    按区域统计</a></li>
                <li><a href="/CRM/PersonStatistics/DepartmentalList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>">
                    按部门统计</a></li>
                <li><a href="/CRM/PersonStatistics/TimeList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>"
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
                                <uc1:RouteAreaList ID="RouteAreaList1" runat="server" />
                                <label>
                                    <b>部门：</b>
                                </label>
                                <uc1:SDepartment runat="server" ID="SDepartment1" />
                                <label>
                                    销售员：
                                </label>
                                <uc1:SOperator runat="server" ID="SOperator1" />
                                <label>
                                    <a href="javascript:void(0);" id="searchbtn">
                                        <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                                </label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif" alt="" />
                        </td>
                    </tr>
                </table>
                <div class="btnbox">
                    <table border="0" align="left" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="90" align="left">
                                <a href="javascript:;" class="toprint">打 印 </a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="toexcel">
                                    <img src="/images/daoru.gif">导 出 </a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="table">统计表</a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="chart">统计图</a>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="tablelist">
                    <table width="100%" id="printtable" border="0" cellpadding="0" cellspacing="1">
                        <tr bgcolor="#BDDCF4">
                            <th width="12%" align="center">
                                月份
                            </th>
                            <th width="10%" align="center">
                                人数
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptList">
                            <ItemTemplate>
                                <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <td align="center">
                                        <%#Eval("CurrYear")%>年<%#Eval("CurrMonth")%>月
                                    </td>
                                    <td align="center">
                                        <%#Eval("PeopleCount")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (type == "xianlu")
                          {%>
                        <tr class="even">
                            <td height="30" align="center"><span style="color: #021F43; font-size: 12px; font-weight: bold;">总计</span>
                            </td>
                            <td align="center">
                                <span style="color: #021F43; font-size: 12px; font-weight: bold;">
                                <asp:Literal ID="litPeopleCount" runat="server"></asp:Literal></span>
                            </td>
                        </tr>
                        <%} %>
                        <%if (len == 0)
                          { %>
                        <tr align="center">
                            <td colspan="4" id="EmptyData">
                                没有相关数据
                            </td>
                        </tr>
                        <%} %>
                    </table>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="13">
                                <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                    CurrencyPageCssClass="RedFnt" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="chartshow" align="center" style="display: none;">
                </div>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <style type="text/css">
        .text
        {
            width: 50px;
        }
    </style>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript">
    $(function(){
        $("#searchbtn").bind("click",function(){
            var type = "<%=type%>";
            var depid = <%=SDepartment1.ClientID %>.GetId();
            var depname = <%=SDepartment1.ClientID %>.GetName();
            var operid = <%=SOperator1.ClientID %>.GetOperatorId();
            var opername = <%=SOperator1.ClientID %>.GetOperatorName();
            var roadid = <%=RouteAreaList1.ClientID %>.GetAreaId();
            var para = {type:type,depid:depid,depname:depname, operid:operid, opername:opername, roadid:roadid};
            window.location.href = "/CRM/PersonStatistics/TimeList.aspx?" + $.param(para);
            return false;
        });
        $("a.chart").bind("click",function(){
            $("div.tablelist").hide();
            $("#chartshow").show();
            var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn2D.swf", "chart1Id", "750", "300", "0", "1");		   			
		    routeAreaStaticList.setDataXML("<%=GetCartogramFlashXml() %>");			
		    routeAreaStaticList.render("chartshow");			
		    return false;
        });
        $("a.table").bind("click",function(){
            $("div.tablelist").show();
            $("#chartshow").html("").hide();
            return false;
        });
        $("a.toexcel").bind("click",function(){
            url = location.pathname+location.search;
            if(url.indexOf("?") >= 0){ url += "&act=toexcel"; }else{url += "?act=toexcel";}
            window.location.href = url;
            return false;
        });
        $("a.toprint").bind("click",function(){
            var url = "/Common/SalePrint.aspx";
            var purl = location.pathname+location.search;
            if(purl.indexOf("?") >= 0){ purl += "&act=toprint"; }else{purl += "?act=toprint";}
            window.open(url+"?url="+purl, "_blank");
            return false;
        });
    });	    
    </script>

    </form>
</asp:Content>
