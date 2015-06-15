<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutDepartmentStatisticList.aspx.cs"
    Title="支出对帐单_统计分析" MasterPageFile="~/masterpage/Back.Master" Inherits="Web.StatisticAnalysis.OutlayAccount.OutDepartmentStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="printButton" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc3" %>
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
                        所在位置>>统计分析 >> 支出对帐单
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
        <%--  <ul class="fbTab">
            <li><a href="/StatisticAnalysis/OutlayAccount/OutDepartmentStatisticList.aspx" class="tabtwo-on"
                id="two2">按部门统计</a></li>
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
                            <uc2:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                            &nbsp;
                            <uc3:RouteAreaList ID="RouteAreaList1" runat="server" />
                            &nbsp;<uc1:selectOperator ID="SelectOperator" runat="server" />
                            <br />
                            <label>
                                出团日期：</label>
                            <input type="text" id="txtStartDate" runat="server" name="txtStartDate" onfocus="WdatePicker()"
                                class="searchinput" />至<input type="text" id="txtEndDate" runat="server" name="txtEndDate"
                                    onfocus="WdatePicker()" class="searchinput" />
                            <label>
                                供应商单位：</label><input name="txt_com" runat="server" type="text" class="searchinput"
                                    id="txt_com" />
                            <label>
                                是否结清：<asp:DropDownList runat="server" ID="drpIsAccount">
                                    <asp:ListItem Value="-1"> --请选择--</asp:ListItem>
                                    <asp:ListItem Value="0">已结清</asp:ListItem>
                                    <asp:ListItem Value="1">未结清</asp:ListItem>
                                </asp:DropDownList>
                                <img id="btnSearch" alt="查询" src="/images/searchbtn.gif" style="vertical-align: top;
                                    cursor: pointer;" /></label>
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
                            <uc1:printButton ContentId="tbl_OutDepartStaList" runat="server" ID="printButton1">
                            </uc1:printButton>
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
            <div class="tablelist" id="divOutDeaprtStaList" runat="server">
                <table width="100%" id="tbl_OutDepartStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="13%" align="center" bgcolor="#BDDCF4">
                            操作员
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            总支出
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            已付
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            未付
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_OutDepartStatiList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("OperatorName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0)" onclick="return GetSumList(<%#Eval("Operatorid") %>)"><font
                                        class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("TotalAmount")))%></font></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0)" onclick="return GetPayedList(<%#Eval("Operatorid") %>)">
                                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("PaidAmount")))%></font></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return  GetUnpayList(<%#Eval("OperatorId") %>)"
                                        id="link3">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("NotPaidAmount")))%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("OperatorName")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0)" onclick="return GetSumList(<%#Eval("Operatorid") %>)"><font
                                        class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("TotalAmount")))%></font></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0)" onclick="return GetPayedList(<%#Eval("Operatorid") %>)">
                                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("PaidAmount")))%></font></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return  GetUnpayList(<%#Eval("OperatorId") %>)"
                                        id="link3">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(Eval("NotPaidAmount")))%></a>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr style="background-color: #bddcf4" align="center">
                        <th>
                            金额总计
                        </th>
                        <th>
                            <%if (res == true)
                              { %>
                            ￥<asp:Label ID="sum_total_account" runat="server"></asp:Label>
                            <%}
                              else
                              {  %>
                            <span style="color: Red">￥<asp:Label ID="sum_total_noaccount" runat="server"></asp:Label></span>
                            <%} %>
                        </th>
                        <th>
                            <%if (res == true)
                              { %>
                            ￥<asp:Label ID="sum_ys_account" runat="server"></asp:Label>
                            <%}
                              else
                              {  %>
                            <span style="color: Red">￥<asp:Label ID="sum_ys_noaccount" runat="server"></asp:Label></span>
                            <%} %>
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
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4"></cc2:ExportPageInfo>
                        </td>
                    </tr>
                </table>
                <cc1:CustomRepeater ID="crp_PrintOutDepartList" Visible="false" runat="server">
                    <HeaderTemplate>
                        <table width="100%" id="tbl_PrintOutDepart" border="1" cellpadding="0" cellspacing="1">
                            <tr>
                                <th width="13%" align="center">
                                    操作员
                                </th>
                                <th width="13%" align="center">
                                    总支出
                                </th>
                                <th width="13%" align="center">
                                    已付
                                </th>
                                <th width="13%" align="center">
                                    未付
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <%#Eval("OperatorName")%>
                            </td>
                            <td align="center">
                                <%#Eval("TotalAmount","{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("PaidAmount","{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("NotPaidAmount","{0:c2}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center">
                                <%#Eval("OperatorName")%>
                            </td>
                            <td align="center">
                                <%#Eval("TotalAmount","{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("PaidAmount","{0:c2}")%>
                            </td>
                            <td align="center">
                                <%#Eval("NotPaidAmount","{0:c2}")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table></FooterTemplate>
                </cc1:CustomRepeater>
                </table>
            </div>
            <%--统计图开始--%>
            <div class="tablelist">
                <table id="tbl_Cartogram" width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center">
                            <div id="divContainer" align="center">
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
        function GetParams(IsCartogram) { 
            var params = { OperatorName: "", OperatorId: "",TourType:"",StartDate:"",EndDate:"",RouteArea:"",IsCartogram:"" ,CompanyName:"",isAccount:""};
            var OperatorControlObj =<%=SelectOperator.ClientID %>;//获取操作员控件
            var OperatorName=OperatorControlObj.GetOperatorName();//操作员
            var OperatorId=OperatorControlObj.GetOperatorId();//操作员ID   
            params.TourType=<%=TourTypeList1.ClientID %>.GetTourTypeId();
            params.OperatorName=OperatorName;
            params.OperatorId=OperatorId;
            params.RouteArea=<%=RouteAreaList1.ClientID %>.GetAreaId();
            params.StartDate=$.trim($("#<%=txtStartDate.ClientID %>").val());//开始日期
            params.EndDate=$.trim($("#<%=txtEndDate.ClientID %>").val());//结束日期
            params.CompanyName = $.trim($("#<%=txt_com.ClientID %>").val());//单位
            params.isAccount=$("#<%=drpIsAccount.ClientID %>").val();
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            return params;
        }
    $(function() {    
        $("#btnSearch").click(function() {//查询
                    window.location.href = "/StatisticAnalysis/OutlayAccount/OutDepartmentStatisticList.aspx?" + $.param(GetParams());
                    return false;
        });        
        $("#btnExport").click(function(){//导出
               if($("#tbl_OutDepartStaList").find("[id='EmptyData']").length>0)
               {
                    alert("暂无数据无法执行导出！");
                    return false;
               }
              var goToUrl = "/StatisticAnalysis/OutlayAccount/OutDepartmentStatisticList.aspx?isExport=1&isAll=1";
              window.open(goToUrl,"Excel导出");
              return false;
        });  
         $("#btnCartogram").click(function(){//统计图   
            $("#<%= divOutDeaprtStaList.ClientID %>").hide();
            $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/OutlayAccount/OutDepartmentStatisticList.aspx",
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
                $("#<%=divOutDeaprtStaList.ClientID %>").show();
                $("#divContainer").hide()
        });         
    });      
    </script>

    <script type="text/javascript">
        //////////统计表未付////////////////
        function GetUnpayList(OperatorId) {
            var data = { OperatorId: OperatorId, TourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(), StartDate: $.trim($("#<%=txtStartDate.ClientID%>").val()), EndDate: $.trim($("#<%=txtEndDate.ClientID%>").val()), RouteArea: <%=RouteAreaList1.ClientID %>.GetAreaId(),CompanyName:$("#<%=txt_com.ClientID %>").val() };
            var url = "/StatisticAnalysis/OutlayAccount/GetDepartuncollectedList.aspx?searchType=3";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "未付",
                modal: true,
                width: "780px",
                height: "550px",
                data: data
            });
        }
        function GetSumList(OperatorId) {
            var data = { OperatorId: OperatorId, TourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(), StartDate: $.trim($("#<%=txtStartDate.ClientID%>").val()), EndDate: $.trim($("#<%=txtEndDate.ClientID%>").val()), RouteArea: <%=RouteAreaList1.ClientID %>.GetAreaId(),CompanyName:$("#<%=txt_com.ClientID %>").val() };
            var url = "/StatisticAnalysis/OutlayAccount/GetDepartuncollectedList.aspx?searchType=1";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "总支出",
                modal: true,
                width: "780px",
                height: "550px",
                data: data
            });
        }
        function GetPayedList(OperatorId) {
            var data = { OperatorId: OperatorId, TourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(), StartDate: $.trim($("#<%=txtStartDate.ClientID%>").val()), EndDate: $.trim($("#<%=txtEndDate.ClientID%>").val()), RouteArea: <%=RouteAreaList1.ClientID %>.GetAreaId(),CompanyName:$("#<%=txt_com.ClientID %>").val() };
            var url = "/StatisticAnalysis/OutlayAccount/GetDepartuncollectedList.aspx?searchType=2";
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "已付",
                modal: true,
                width: "780px",
                height: "550px",
                data: data
            });
        }
//        //////////////统计图未付//////////////////////
//        function GetCartogramUnpaidList(OperatorId) {
//            var data = { OperatorId: OperatorId, TourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(), StartDate: $.trim($("#txtStartDate").val()), EndDate: $.trim($("#txtEndDate").val()), RouteArea:<%=RouteAreaList1.ClientID %>.GetAreaId() };
//            var url = "/StatisticAnalysis/OutlayAccount/GetCartogramUnpaidList.aspx";
//            Boxy.iframeDialog({
//                iframeUrl: url,
//                title: "未付",
//                modal: true,
//                width: "820",
//                data: data,
//                height: "150px"
//            });
//            return false;
//        }
    </script>

</asp:Content>
