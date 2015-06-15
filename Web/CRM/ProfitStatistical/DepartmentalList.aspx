<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentalList.aspx.cs"
    Inherits="Web.CRM.ProfitStatistical.DepartmentalList" MasterPageFile="~/masterpage/Back.Master"
    Title="利润统计_销售分析_客户关系管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SOperator" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="SDepartment" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<asp:Content ContentPlaceHolderID="c1" runat="server" ID="Content1">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">客户关系管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>>客户关系管理 >> 销售分析 >> 利润统计
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 40px;">
                <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                    <tr>
                        <% =Web.Common.AwakeTab.createSale(SiteUserInfo, 128) %>
                    </tr>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <ul class="fbTab">
                <li><a href="/CRM/ProfitStatistical/AreaList.aspx">按区域统计</a></li>
                <li><a href="/CRM/ProfitStatistical/DepartmentalList.aspx" class="tabtwo-on">按部门统计</a></li>
                <li><a href="/CRM/ProfitStatistical/TypeList.aspx">按类型统计</a></li>
                <li><a href="/CRM/ProfitStatistical/TimeList.aspx">按时间统计</a></li>
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
                                <uc1:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                                <label>
                                    出团时间：</label>
                                <input type="text" class="searchinput time" id="lstime" readonly="readonly" value="<%=qesModel.LeaveDateStart==null?"":Convert.ToDateTime(qesModel.LeaveDateStart).ToString("yyyy-MM-dd")  %>" />
                                至
                                <input type="text" class="searchinput time" readonly="readonly" id="letime" value="<%=qesModel.LeaveDateEnd==null?"":Convert.ToDateTime(qesModel.LeaveDateEnd).ToString("yyyy-MM-dd")  %>" />
                                <label>
                                    核算时间：</label>
                                <input type="text" class="searchinput time" id="cstime" readonly="readonly" value="<%=qesModel.CheckDateStart==null?"":Convert.ToDateTime(qesModel.CheckDateStart).ToString("yyyy-MM-dd")  %>" />
                                至
                                <input type="text" class="searchinput time" readonly="readonly" id="cetime" value="<%=qesModel.CheckDateEnd==null?"":Convert.ToDateTime(qesModel.CheckDateEnd).ToString("yyyy-MM-dd") %>" />
                                <br />
                                <label>
                                    操作人部门：
                                </label>
                                <uc1:SDepartment runat="server" ID="department" />
                                &nbsp;&nbsp;&nbsp;
                                <label>
                                    团队操作人：
                                </label>
                                <uc1:SOperator runat="server" ID="saleman" />
                                <label>
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
                                <uc3:UCPrintButton ContentId="printtable" ID="UCPrintButton1" runat="server" />
                            </td>
<%--                            <td width="90" align="left">
                                <a href="javascript:;" class="toprint">打 印 </a>
                            </td>--%>
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
                    <table width="100%" border="0" id="printtable" cellpadding="0" cellspacing="1">
                        <tr bgcolor="#BDDCF4">
                            <th width="14%" align="center">
                                部门
                            </th>
                            <%--<th width="8%" align="center">销售员</th>--%>
                            <th width="8%" align="center">
                                团队数
                            </th>
                            <th width="8%" align="center" bgcolor="#bddcf4">
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
                        <asp:Repeater runat="server" ID="rptList">
                            <ItemTemplate>
                                <tr bgcolor="#e3f1fc" tid="<%#Eval("DepartId") %>">
                                    <td align="center">
                                        <%#Eval("DepartName")%>
                                    </td>
                                    <%--<td align="center" ><%#GetProfSalsers(((EyouSoft.Model.StatisticStructure.EarningsDepartStatistic)GetDataItem()).SalesClerk)%></td>--%>
                                    <td align="center">
                                        <a href="javascript:;" class="showteam">
                                            <%#Eval("TourNum") %></a>
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
                                        <%#Eval("TourGross", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("PeopleGross", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("TourShare", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("CompanyShare", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("LiRunLv", "{0:c2}")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="odd">
                            <th>
                                总计
                            </th>
                            <%--<th>&nbsp;
                        </th>--%>
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
                        <%if (len == 0)
                          { %>
                        <tr align="center">
                            <td colspan="9">
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
                <p id="danwei" style="display: none; margin-left: 200px; font-size: 10px">
                    单位：万元</p>
                <div id="chartshow" align="center" style="display: none;">
                </div>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript">
    $(function(){
        $(".time").bind("click",function(){
            WdatePicker();
            return false;
        });
        $("a.print").click(function(){  //打印
                if (window.print != null) {
                    window.print();
                } else {
                    alert('没有安装打印机');
                }
                return false;
        });
        
        function getpara(){
            var teamtype = <%=TourTypeList1.ClientID %>.GetTourTypeId();
                        var depname = <%=department.ClientID %>.GetName();
            var depid = <%=department.ClientID %>.GetId();
            var saleman = <%=saleman.ClientID %>.GetOperatorId();
            var salemanname = <%=saleman.ClientID %>.GetOperatorName();
            var lstime = $("#lstime").val();
            var letime = $("#letime").val();
            var cstime = $("#cstime").val();
            var cetime = $("#cetime").val();
            var para = { depname:depname,depid:depid,saleman:saleman,salemanname:salemanname,type: teamtype, lstime: lstime, letime: letime,cstime:cstime,cetime:cetime};
            return para;
        }
        $("#searchbtn").bind("click",function(){
            window.location.href = "/CRM/ProfitStatistical/DepartmentalList.aspx?" + $.param(getpara());
            return false;
        });
        $("a.chart").bind("click",function(){
            $("div.tablelist").hide();
            $("#chartshow").show();
            $("#danwei").show();
            var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn2D.swf", "chart1Id", "750", "300", "0", "1");		   			
		    routeAreaStaticList.setDataXML("<%=GetCartogramFlashXml() %>");			
		    routeAreaStaticList.render("chartshow");			
		    return false;
        });
        $("a.table").bind("click",function(){
            $("div.tablelist").show();
            $("#chartshow").html("").hide();
            $("#danwei").hide();
            return false;
        });
         $("a.showteam").bind("click",function(){
            var that = $(this);
	        var url = "/CRM/ProfitStatistical/TeamShow.aspx";
	        var tid = that.parent().parent().attr("tid");
	        Boxy.iframeDialog({
		        iframeUrl:url + "?depid=" + tid,
		        title:"团队数",
		        modal:true,
		        width:"720",
		        height:"340px",
		        data:getpara()
	        });
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
