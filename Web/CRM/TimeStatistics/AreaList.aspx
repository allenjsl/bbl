<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaList.aspx.cs" Inherits="Web.CRM.TimeStatistics.AreaList"
    MasterPageFile="~/masterpage/Back.Master" Title="帐龄分析表_销售分析_客户关系管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SOperator" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="SDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
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
                            所在位置>>客户关系管理 >> 销售分析 >> 帐龄分析表
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
                        <% =Web.Common.AwakeTab.createSale(SiteUserInfo, 129) %>
                    </tr>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <ul class="fbTab">
                <li><a href="/CRM/TimeStatistics/AreaList.aspx"  class="tabtwo-on">按区域统计</a></li>
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
                                <uc4:RouteAreaList ID="RouteAreaList1" runat="server" />
                                <label>
                                    责任销售：
                                </label>
                                <uc1:SOperator runat="server" ID="saleman"/>
                                <label>出团时间：</label>
                                <input name="txtStar" type="text" class="searchinput time" id="timeStar" readonly="readonly" value="<% =qaaModel.LeaveDateStart == null?"": Convert.ToDateTime(qaaModel.LeaveDateStart).ToString("yyyy-MM-dd") %>" />
                                至 
                                <input name="txtEnd" type="text" class="searchinput time" readonly="readonly" id="timeEnd" value="<% =qaaModel.LeaveDateEnd == null?"": Convert.ToDateTime(qaaModel.LeaveDateEnd).ToString("yyyy-MM-dd") %>" />
                                <label>
                                    <a href="javascript:void(0);" id="searchbtn"><img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
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
                                <a href="javascript:;" class="toexcel" >
                                    <img src="/images/daoru.gif">
                                    导 出 </a>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="tablelist">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1" id="printtable">
                        <tr>
                          <th width="19%" align="center" bgcolor="#BDDCF4">销售员</th>
                          <th width="19%" align="center" bgcolor="#bddcf4">拖欠款总额</th>
                          <th width="19%" align="center" bgcolor="#bddcf4">最长拖欠时间（天）</th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptList">
                            <ItemTemplate>
                            <tr tid="<%#((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorId%>"> 
                              <td align="center" bgcolor="#e3f1fc"><%#((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%></td>
                              <td align="center" bgcolor="#e3f1fc"><a href="javascript:;" class="show"><%#Eval("ArrearageSum","{0:c2}")%></a></td>
                              <td align="center" bgcolor="#e3f1fc"><%# (DateTime.Now - Convert.ToDateTime(Eval("MaxArrearageTime"))).Days%></td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (len == 0)
                            { %>
                            <tr align="center"><td colspan="3" id="EmptyData">没有相关数据</td></tr>
                          <%} %>
                    </table>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="3">
                                <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3"  PageStyleType="NewButton" CurrencyPageCssClass="RedFnt" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <script type="text/javascript">
    $(function(){
         $("input.time").bind("click",function(){
            WdatePicker();
        });
        $("a.print").click(function(){  //打印
                if (window.print != null) {
                    window.print();
                } else {
                    alert('没有安装打印机');
                }
                return false;
        });
        $("#searchbtn").bind("click",function(){
            var teamtype = <%=TourTypeList1.ClientID %>.GetTourTypeId();
            var saleman = <%=saleman.ClientID %>.GetOperatorId(); 
            var salemanname = <%=saleman.ClientID %>.GetOperatorName();
            var stime = $("#timeStar").val();
            var etime = $("#timeEnd").val();
            var areaid=<%=RouteAreaList1.ClientID %>.GetAreaId();//线路区域
            //参数
            var para = {type:teamtype,saleman:saleman,salemanname:salemanname,stime:stime,etime:etime,areaid:areaid};

            window.location.href = "/CRM/TimeStatistics/AreaList.aspx?" + $.param(para);
            return false;
        });
        $("a.show").bind("click",function(){
            var that = $(this);
	        var url = "/CRM/TimeStatistics/MoneyShow.aspx";
	        
	        var tid = that.parent().parent().attr("tid");
	        var teamtype = <%=TourTypeList1.ClientID %>.GetTourTypeId();
            var saleman = <%=saleman.ClientID %>.GetOperatorId(); 
            var stime = $("#timeStar").val();
            var etime = $("#timeEnd").val();
            var areaid=<%=RouteAreaList1.ClientID %>.GetAreaId();//线路区域
            //参数
            var para = {type:teamtype,stime:stime,etime:etime,areaid:areaid,tid:tid};
	        
	        Boxy.iframeDialog({
		        iframeUrl:url,
		        title:"拖欠款",
		        modal:true,
		        width:"720",
		        height:"340px",
		        data:para
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
