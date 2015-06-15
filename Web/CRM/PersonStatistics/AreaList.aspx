<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaList.aspx.cs" Inherits="Web.CRM.PersonStatistics.AreaList"
    MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SOperator" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="SDepartment" TagPrefix="uc1" %>
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
                                <%= type == "xianlu" ? "��·��Ʒ��" : "�ͻ���ϵ����"%></span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            ����λ��>>
                            <%= type == "xianlu" ? "��·��Ʒ�� >> Ӫ������" : "�ͻ���ϵ���� >> ���۷��� >> �˴�ͳ��"%>
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
                <li><a href="/CRM/PersonStatistics/AreaList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>"
                    class="tabtwo-on">������ͳ��</a></li>
                <li><a href="/CRM/PersonStatistics/DepartmentalList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>">
                    ������ͳ��</a></li>
                <li><a href="/CRM/PersonStatistics/TimeList.aspx<%= type=="xianlu"?"?type=xianlu":"" %>">
                    ��ʱ��ͳ��</a></li>
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
                                <label>
                                    <b>���ţ�</b>
                                </label>
                                <uc1:SDepartment runat="server" ID="SDepartment1" />
                                <label>
                                    ����Ա��
                                </label>
                                <uc1:SOperator runat="server" ID="SOperator1" />
                                <label>
                                    ����ʱ�䣺</label>
                                <input name="txtStar" type="text" class="searchinput time" id="timeStar" readonly="readonly"
                                    value="<%=QueryModel.LeaveDateStart==null?"":Convert.ToDateTime(QueryModel.LeaveDateStart).ToString("yyyy-MM-dd") %>" />
                                ��
                                <input name="txtEnd" type="text" class="searchinput time" readonly="readonly" id="timeEnd"
                                    value="<%= QueryModel.LeaveDateEnd==null?"":Convert.ToDateTime(QueryModel.LeaveDateEnd).ToString("yyyy-MM-dd") %>" />
                                <label>
                                    <a href="javascript:void(0);" id="searchbtn" width="125">
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
                                <a href="javascript:;" class="toprint">�� ӡ </a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="toexcel">
                                    <img src="/images/daoru.gif">�� �� </a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="table">ͳ�Ʊ�</a>
                            </td>
                            <td width="90" align="left">
                                <a href="javascript:;" class="chart">ͳ��ͼ</a>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="tablelist">
                    <table width="100%" id="printtable" border="0" cellpadding="0" cellspacing="1">
                        <tr bgcolor="#BDDCF4">
                            <th width="16%" align="center">
                                ��·����
                            </th>
                            <th width="12%" align="center">
                                ����Ա
                            </th>
                            <th width="12%" align="center">
                                �Ƶ�Ա
                            </th>
                            <th width="10%" align="center">
                                ����
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptList">
                            <ItemTemplate>
                                <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
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
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <% if (type == "xianlu")
                           {%>
                        <tr class="even">
                            <td height="30" align="center"><span style="color: #021F43; font-size: 12px; font-weight: bold;">�ܼ�</span></td>
                            <td height="30" align="right" colspan="2">
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
                                û���������!
                            </td>
                        </tr>
                        <%} %>
                    </table>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="4">
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
        $("input.time").bind("click",function(){
            WdatePicker();
        });
        $("#searchbtn").bind("click",function(){
            var type = "<%=type%>";
            var depid = <%=SDepartment1.ClientID %>.GetId();
            var depname = <%=SDepartment1.ClientID %>.GetName();
            var operid = <%=SOperator1.ClientID %>.GetOperatorId();
            var opername = <%=SOperator1.ClientID %>.GetOperatorName();
            var stime = $("#timeStar").val();
            var etime = $("#timeEnd").val();
            //����
            var para = {type:type,depid:depid,depname:depname, operid:operid, opername:opername, stime: stime, etime: etime};
            window.location.href = "/CRM/PersonStatistics/AreaList.aspx?" + $.param(para);
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
