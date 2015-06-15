<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master"
    Title="收入对帐单_统计分析" CodeBehind="IncDepartmentStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.IncomeAccount.IncDepartmentStatisticList" %>

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
                        所在位置>>统计分析 >> 收入对帐单
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
            <li><a href="/StatisticAnalysis/IncomeAccount/IncDepartmentStatisticList.aspx" id="two2"
                class="tabtwo-on">按部门统计</a></li>
            <div class="clearboth">
            </div>
        </ul>--%>
        <div class="hr_10">
        </div>
        <div class="tablelist" id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <uc3:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                            &nbsp;<uc4:RouteAreaList ID="RouteAreaList1" runat="server" />
                            <uc1:selectOperator Title="销售员：" ID="SelectSalser" runat="server" />
                            <br />
                            <label>
                                出团日期：</label>
                            <input name="txtLeaveTourStarTime" runat="server" type="text" class="searchinput"
                                id="txtLeaveTourStarTime" onfocus="WdatePicker()" />
                            至
                            <input name="txtLeaveTourEndTime" runat="server" type="text" class="searchinput"
                                id="txtLeaveTourEndTime" onfocus="WdatePicker()" />
                            <label>
                                客户单位：</label><input name="txt_com" runat="server" type="text" class="searchinput"
                                    id="txt_com" />
                            <label>
                                是否结清：<asp:DropDownList runat="server" ID="drpIsAccount">
                                    <asp:ListItem Value="-1"> --请选择--</asp:ListItem>
                                    <asp:ListItem Value="0">已结清</asp:ListItem>
                                    <asp:ListItem Value="1">未结清</asp:ListItem>
                                </asp:DropDownList>
                                <img id="btnSearch" src="/images/searchbtn.gif" style="vertical-align: top; cursor: pointer;" /></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <div class="hr_10">
            </div>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc2:UCPrintButton ContentId="tbl_IncDepartmentAreaStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="linkStatiTbl">统计表</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="linkCartogram">统计图</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divIncDepartList" runat="server">
                <table id="tbl_IncDepartmentAreaStaList" width="100%" border="0" cellpadding="0"
                    cellspacing="1">
                    <tr>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            销售员
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            总收入
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            已收
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            未收
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_IncDepartStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("OperatorName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("TotalAmount")))%></font>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("AccountAmount")))%></font>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetUnCollectIncomeList(<%#Eval("OperatorId") %>)"
                                        id="link3">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotAccountAmount")))%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("OperatorName")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("TotalAmount")))%></font>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("AccountAmount")))%></font>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetUnCollectIncomeList(<%#Eval("OperatorId") %>)"
                                        id="link3">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotAccountAmount")))%></a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr style="background-color: #bddcf4" align="center">
                        <th>
                            金额总计
                        </th>
                        <th>
                            <asp:Label ID="sum_total" runat="server"></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="sum_ys" runat="server"></asp:Label>
                        </th>
                        <th>
                            <%if (res == true)
                              { %>
                            ￥<asp:Label ID="sum_ws_account" runat="server"></asp:Label>
                            <%}
                              else
                              {  %>
                            <span style="color: Red">￥<asp:Label ID="sum_ws_noaccount" runat="server"></asp:Label></span>
                            <%} %>
                        </th>
                    </tr>
                </table>
                <table width="100%" id="tbl_ExportPage" runat="server" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </table>
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
        </div>
    </div>
    <cc1:CustomRepeater ID="crp_PrintIncDepartStaList" Visible="false" runat="server">
        <HeaderTemplate>
            <table id="Table1" width="100%" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="13%" align="center">
                        销售员
                    </th>
                    <th width="13%" align="center">
                        总收入
                    </th>
                    <th width="13%" align="center">
                        已收
                    </th>
                    <th width="13%" align="center">
                        未收
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <%#Eval("OperatorName")%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("TotalAmount")))%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("AccountAmount")))%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotAccountAmount")))%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td align="center">
                    <%#Eval("OperatorName")%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("TotalAmount")))%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("AccountAmount")))%>
                </td>
                <td align="center">
                    ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotAccountAmount")))%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </cc1:CustomRepeater>
    </form>

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript">
        function GetParams(IsCartogram) { 
            var params = { RouteArea: "",TourType:"",Salser: "", SalserId: "",LeaveTourStartDate:"",LeaveTourEndDate:"",IsCartogram:"",CompanyName:"" ,IsAccount:""};
            var SalerControlObj =<%=SelectSalser.ClientID %>;//获取销售员控件
            var Salser=SalerControlObj.GetOperatorName();//销售员名字
            var SalsID=SalerControlObj.GetOperatorId();//销售员ID
            params.TourType=<%=TourTypeList1.ClientID %>.GetTourTypeId();//类型
            params.RouteArea=<%=RouteAreaList1.ClientID %>.GetAreaId();//线路区域
            params.Salser=Salser;
            params.SalserId=SalsID;
            params.LeaveTourStartDate=$.trim($("#<%=txtLeaveTourStarTime.ClientID %>").val());//出团开始日期
            params.LeaveTourEndDate=$.trim($("#<%=txtLeaveTourEndTime.ClientID %>").val());//出团结束日期
            params.CompanyName = $.trim($("#<%=txt_com.ClientID %>").val());//单位
            params.IsAccount=$("#<%=drpIsAccount.ClientID %>").val(); //是否结算
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            return params;
        }
    $(function() {    
         //查询
        $("#btnSearch").click(function() {
        window.location.href = "/StatisticAnalysis/IncomeAccount/IncDepartmentStatisticList.aspx?" + $.param(GetParams());
        return false;
        });
        ///统计表
        $("#linkStatiTbl").click(function()
        {
              $("#divContainer").hide();//统计图隐藏
              $("#<%= divIncDepartList.ClientID %>").show();//统计表显示
        });
        //导出
        $("#btnExport").click(function(){
               if($("#tbl_IncDepartmentAreaStaList").find("[id='EmptyData']").length>0)
               {
                    alert("暂无数据无法执行导出！");
                    return false;
               }
              var goToUrl = "/StatisticAnalysis/IncomeAccount/IncDepartmentStatisticList.aspx?isExport=1";
              window.open(goToUrl,"Excel导出");
              return false;
        });
        //统计图  
        $("#linkCartogram").click(function(){ 
            $("#<%= divIncDepartList.ClientID %>").hide();
            $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/IncomeAccount/IncDepartmentStatisticList.aspx",
                     data: GetParams(1),
                     cache: false,
                     success: function(CartogramXml) {
                        if(CartogramXml!="")
                        {
                                var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn3D.swf", "chart1Id", "400", "300", "0", "1");		   			
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
    });      
    </script>

    <script type="text/javascript">
        /////////////未收入明细列表////
        function GetUnCollectIncomeList(SalserId) {
            var data = { RouteArea: <%=RouteAreaList1.ClientID %>.GetAreaId(), TourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(), SalserId: SalserId, LeaveTourStartDate: $.trim($("#txtLeaveTourStarTime").val()), LeaveTourEndDate: $.trim($("#txtLeaveTourEndTime").val()),Company:$.trim($("#<%=txt_com.ClientID%> ").val()) }; //参数传递
            $("#divContainer").hide();
            var url = "/StatisticAnalysis/IncomeAccount/GetUnIncomeAreaList.aspx";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "未收",
                modal: true,
                width: "800px",
                height: "550px",
                data: data,
                afterHide: function() {
                    $("#divContainer").show();
                }
            });
            //return false;
        }
    </script>

</asp:Content>
