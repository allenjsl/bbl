<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/masterpage/Back.Master" Title="销售统计分析_统计分析" CodeBehind="SoldStaList.aspx.cs" Inherits="Web.StatisticAnalysis.SoldStatistic.SoldStaList" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register src="../../UserControl/ProvinceAndCity.ascx" tagname="ProvinceAndCity" tagprefix="uc4" %>
<%@ Register Src="~/UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc6" %>
<%@ Import Namespace="EyouSoft.Common" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
</asp:Content>
<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="c1">
<form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 销售统计分析
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
        <div id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <uc5:RouteAreaList runat="server" ID="RouteAreaList1" IsComAreas="true" />
                            <label>
                                出团日期：</label>
                            <input name="txtLeaveTourStartDate" onfocus="WdatePicker()" type="text" class="searchinput" id="txtLeaveTourStartDate" value="<%=Utils.GetQueryStringValue("todate") %>" />到<input id="txtLeaveTourEndDate"  name="txtLeaveTourEndDate" onfocus="WdatePicker()" type="text" class="searchinput" value="<%=Utils.GetQueryStringValue("endate") %>" />&nbsp;<label>送团人数
                            </label><select name="liQueryOperator" id="liQueryOperator">
  <option value="0">请选择</option>
  <option value="1">等于</option>
  <option value="2">小于等于</option>
  <option value="3">大于等于</option>
</select><input type="text" id="TradeNum" name="TradeNum" class="searchinput" maxlength="4" style="width:50px" /><label><br/>客户单位：</label><input type="hidden" name="hd_teamId" id="hd_teamId" value="" /><input type="text" id="txt_teamName" name="txt_teamName" class="searchinput2" /><a href="javascript:" onclick="return Cust.SelectCustomer();" class="selectTeam"><img src="../../images/sanping_04.gif" width="28" height="18"></a>
                            <label>
                                部门：</label>
                            <uc6:UCSelectDepartment ID="UCSelectDepartment1" SetPicture="/images/sanping_04.gif" runat="server" />
                            <%--销售员--%>
                            &nbsp;<uc2:selectOperator Title="销售员：" ID="SelectSalser" runat="server" /><label><br/>地区：
                            </label><uc4:ProvinceAndCity  ID="ProvinceAndCity1" runat="server" />
                            <script type="text/javascript">
                                window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                            </script>     
                            <label>
                                <input id="btnSearch" type="image" src="/images/searchbtn.gif" style="vertical-align: top;cursor:pointer;"
                                    value="button" />
                            </label>
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
                            <uc3:UCPrintButton ContentId="tbl_EmpIncomeList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="LinkExport">
                                <img src="/images/daoru.gif" />
                                导 出 </a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divEmpIncomList" runat="server">
                <table width="100%" id="tbl_EmpIncomeList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="40" rowspan="2" align="center" bgcolor="#BDDCF4">
                            编号                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            所属地区                        </th>
                        <th rowspan="2" align="center" bgcolor="#bddcf4">
                            组团社名称                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            联系人                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            联系电话                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            责任销售                        </th>
                        <th width="150" colspan="3" align="center" bgcolor="#bddcf4">人数</th>
                    </tr>
                    <tr>
                      <th align="center" width="80" bgcolor="#bddcf4"><a href="javascript:void(0)" style="color: " onclick="return order(5)">↑</a>团队<a href="javascript:void(0)" style="color: " onclick="return order(4)">↓</a></th>
                      <th align="center" width="80" bgcolor="#bddcf4"><a href="javascript:void(0)" style="color: " onclick="return order(3)">↑</a>散客<a href="javascript:void(0)" style="color: " onclick="return order(2)">↓</a></th>
                      <th align="center" width="80" bgcolor="#bddcf4"><a href="javascript:void(0)" style="color: " onclick="return order(1)">↑</a>合计<a href="javascript:void(0)" style="color: " onclick="return order(0)">↓</a></th>
                    </tr>
                    <cc1:CustomRepeater ID="rptCustomer" runat="server">
                        <ItemTemplate>
                        <tr <%#(Container.ItemIndex + 1)%2==0?"style='background:#bddcf4'":"style='background:#e3f1fc'" %>>
                      <th align="center"><%#PageSize*(PageIndex-1)+Container.ItemIndex+1 %></th>
                      <th align="center"><%# Eval("ProvinceName") %>&nbsp;<%# Eval("CityName")%></th>
                      <th align="left"><a href="javascript:;" onclick="return Cust.show('<%# Eval("Id") %>')"><%# Eval("Name") %></a></th>
                      <th align="center"><%# Eval("ContactName") %></th>
                      <th align="center"><%# Eval("Phone") %></th>
                      <th align="center"><%# Eval("Saler")%></th>
                      <th align="center"><a href="javascript:;" onclick="return Cust.getTrade('<%# Eval("Id") %>','1');"><%# Convert.ToInt32(Eval("TradeNum").ToString()) - Convert.ToInt32(Eval("TradeTourNum").ToString())%></a></th>
                      <th align="center"><a href="javascript:;" onclick="return Cust.getTrade('<%# Eval("Id") %>','0');"><%# Eval("TradeTourNum")%></a></th>
                      <th align="center"><a href="javascript:;" onclick="return Cust.getTrade('<%# Eval("Id") %>','0,1');"><%# Eval("TradeNum")%></a></th>
                    </tr>
                        </ItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                      <th align="center" bgcolor="#BDDCF4"></th>
                      <th align="center" bgcolor="#BDDCF4"></th>
                      <th align="left" bgcolor="#BDDCF4"></th>
                      <th align="center" bgcolor="#BDDCF4"></th>
                      <th align="center" bgcolor="#BDDCF4"></th>
                      <th align="center" bgcolor="#BDDCF4">合计:</th>
                      <th align="center" bgcolor="#BDDCF4">
                          <asp:Literal ID="ltrSumVistorNumber" runat="server"></asp:Literal>
                        </th>
                      <th align="center" bgcolor="#BDDCF4">
                          <asp:Literal ID="ltrSumTourNumber" runat="server"></asp:Literal>
                        </th>
                      <th align="center" bgcolor="#BDDCF4">
                          <asp:Literal ID="ltrSumTradeNumber" runat="server"></asp:Literal>
                        </th>
                    </tr>
                </table>
                <table width="100%" id="tbl_ExPageEmp" runat="server" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4" PageStyleType="NewButton"
                    CurrencyPageCssClass="RedFnt"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <cc1:CustomRepeater ID="crp_PrintEmpIncomeList" Visible="false" runat="server">
        <HeaderTemplate>
            <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                <tr>
                        <th width="40" rowspan="2" align="center" bgcolor="#BDDCF4">
                            编号                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            所属地区                        </th>
                        <th rowspan="2" align="center" bgcolor="#bddcf4">
                            组团社名称                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            联系人                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            联系电话                        </th>
                        <th width="100" rowspan="2" align="center" bgcolor="#bddcf4">
                            责任销售                        </th>
                        <th width="150" colspan="3" align="center" bgcolor="#bddcf4">人数</th>
                    </tr>
                    <tr>
                      <th align="center" width="80" bgcolor="#bddcf4">团队</th>
                      <th align="center" width="80" bgcolor="#bddcf4">散客</th>
                      <th align="center" width="80" bgcolor="#bddcf4">合计</th>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </cc1:CustomRepeater>
    </form>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
 <script type="text/javascript">
     var Cust = {//全选
         changeAll: function(obj) {
             $("input:checkbox").not("[name='checkAll']").attr("checked", $(obj).attr("checked"));
         }, //打开弹窗
         openDialog: function(p_url, p_title, p_width, p_height) {
             Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
         },
         show: function(cId) {
             Cust.openDialog("/CRM/customerinfos/CustomerEdit.aspx?type=show&custId=" + cId, "查看详细信息", "950px", "550px");
             return false;
         },
         CheckSeller: function() {
             return true;
         },
         //查看交易情况
         getTrade: function(cid, TourType) {
             var searchParams = utilsUri.getUrlParams([]);
             var params = {};
             params["lsdate"] = searchParams["todate"] || "";
             params["ledate"] = searchParams["endate"] || "";
             params["TourType"] = TourType;
             params["cid"] = cid;
             params["areaid"] = searchParams["areaid"] || "";

             var url = utilsUri.createUri("/CRM/customerinfos/CustomerTrade.aspx", params);

             Boxy.iframeDialog({ title: "交易情况", iframeUrl: url, width: "635px", height: "400px" });
             return false;
         },
         //选择组团社
         SelectCustomer: function() {
             var iframeId = '<%=Request.QueryString["iframeId"] %>';
             var url = '/CRM/customerservice/SelCustomer.aspx?method=selectTeam';
             Boxy.iframeDialog({
                 iframeUrl: url,
                 title: "选用组团社",
                 modal: true,
                 width: "820px",
                 height: "520px",
                 data: {
                     desid: iframeId,
                     backfun: "selectTeam"
                 }
             });
             return false;
         }
     };
     $(function() {
         $("#btnSearch").click(function() {
             search();
             return false;
         });
         $("[name='txt_teamName']").focus(function() {
             Cust.SelectCustomer();
         });
         $("#LinkExport").click(function() {
             //search("toexcel");
             //return false;

             var params = utilsUri.getUrlParams([]);
             params["recordcount"] = recordCount;
             params["istoxls"] = 1;

             if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

             window.location.href = utilsUri.createUri(null, params);
             return false;
         });

         initSearch();
     });
     function selectTeam(id, name) {
         $("#hd_teamId").val(id);
         $("#txt_teamName").val(name);
     }
     function search() {         
         var todate = encodeURIComponent($("#txtLeaveTourStartDate").val());
         var endate = $("#txtLeaveTourEndDate").val();
         var qo = $("#liQueryOperator").val();
         var tn = encodeURIComponent($("#TradeNum").val());
         var customerId = $("#hd_teamId").val();
         var customerName = encodeURIComponent($("#txt_teamName").val());
         var xsyId = window["<%= SelectSalser.ClientID%>"].GetOperatorId();
         var xsyName = encodeURIComponent(window["<%= SelectSalser.ClientID%>"].GetOperatorName());
         var province = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().id;
         var cityId = window["<%=ProvinceAndCity1.ClientID %>"].getCity().id;

         var provinceName = encodeURIComponent(window["<%=ProvinceAndCity1.ClientID %>"].getProvince().name);
         var cityName = encodeURIComponent(window["<%=ProvinceAndCity1.ClientID %>"].getCity().name);

         var areaid = window["<%=RouteAreaList1.ClientID %>"].GetAreaId();

         var wuc_dept = window['<%=UCSelectDepartment1.ClientID %>'];
         var deptids = wuc_dept.GetId();
         var deptnames= wuc_dept.GetName();

         location.href = "SoldStaList.aspx?todate=" + todate + "&endate=" +
            endate + "&qo=" + qo + "&tn=" + tn + "&customerId=" + customerId + "&customerName=" + customerName +
            "&xsyId=" + xsyId + "&cityId=" + cityId + "&province=" + province + "&provinceName=" + provinceName + "&cityName=" + cityName
            + "&xsyName=" + xsyName + "&areaid=" + areaid + "&deptids=" + deptids + "&deptnames=" + deptnames;
     }

     //排序
     function order(_v) {
         var params = utilsUri.getUrlParams(["page"]);

         if (params["orderbytype"] == _v) { alert("当前数据已经按此规则排序"); return false };

         params["orderbytype"] = _v;

         window.location.href = utilsUri.createUri(null, params);
         return false;
     }

     //init search
     function initSearch() {
         var _params = utilsUri.getUrlParams([]);

         $("#liQueryOperator").val(_params["qo"] || "");
         $("#TradeNum").val(_params["tn"] || "");
         $("#hd_teamId").val(_params["customerId"] || "");
         $("#txt_teamName").val(decodeURIComponent(_params["customerName"] || ""));
         window["<%= SelectSalser.ClientID%>"].setV((_params["xsyId"] || ""), decodeURIComponent(_params["xsyName"] || ""));
         window["<%=ProvinceAndCity1.ClientID %>"].setProvince((_params["province"] || ""), decodeURIComponent(_params["provinceName"] || ""));
         window["<%=ProvinceAndCity1.ClientID %>"].setCity((_params["cityId"] || ""), decodeURIComponent(_params["cityName"] || ""));

     }
 </script>
</asp:Content>