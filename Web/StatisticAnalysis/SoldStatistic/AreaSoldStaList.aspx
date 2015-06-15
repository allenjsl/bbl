<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master"
    Title="区域销售统计分析_统计分析" CodeBehind="AreaSoldStaList.aspx.cs" Inherits="Web.StatisticAnalysis.SoldStatistic.AreaSoldStaList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/ProvinceAndCity.ascx" TagName="ProvinceAndCity"
    TagPrefix="uc4" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc5" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script src="../../js/utilsUri.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="c1">
    <form id="form1" runat="server">
    <input type="hidden" id="hidContent" name="hidContent" />
    <div style="width: auto; height: 100%; overflow: auto;">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 区域销售统计分析
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
                            <label>
                                出团日期：</label>
                            <input type="text" id="txtStartDate" runat="server" name="txtStartDate" onfocus="WdatePicker()"
                                class="searchinput" />至<input type="text" id="txtEndDate" runat="server" name="txtEndDate"
                                    onfocus="WdatePicker()" class="searchinput" />
                            &nbsp;
                            <uc5:TourTypeList runat="server" ID="ddlTourType" />
                            &nbsp;<%--销售员--%>
                            &nbsp;<uc2:selectOperator Title="销售员：" ID="SelectSalser" runat="server" />
                            <label>
                                &nbsp;地区：
                            </label>
                            <uc4:ProvinceAndCity ID="ProvinceAndCity1" runat="server" />

                            <script type="text/javascript">
                                window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                            </script>

                            <label>
                                <input id="btnSearch" type="image" src="/images/searchbtn.gif" style="vertical-align: top;
                                    cursor: pointer;" value="button" />
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
                            <uc3:UCPrintButton ContentId="tbl_EmpIncomeList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <%--<a href="javascript:void(0);" id="LinkExport" title="导出">
                                <asp:ImageButton ID="imgBtnExport" runat="server" BorderStyle="None" ImageUrl="/images/daoru.gif" OnClick="ExportExcel" OnClientClick="return toXls();" />--%>
                            <a onclick="toXls();return false;" href="###">
                                <img src="/images/daoru.gif">导&nbsp;出</a>
                            <input type="hidden" name="istoxls" id="istoxls" value="1" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divEmpIncomList" runat="server">
                <table id="tbl_EmpIncomeList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="80" align="center" bgcolor="#bddcf4">
                            地域
                        </th>
                        <th width="80" align="center" bgcolor="#bddcf4">
                            销售员
                        </th>
                        <!--    部门    -->
                        <asp:Literal ID="ltrDepartList" runat="server"></asp:Literal>
                    </tr>
                    <cc1:CustomRepeater ID="crp_AreaSoldList" runat="server" OnItemDataBound="InitDepartStat">
                        <ItemTemplate>
                            <tr <%#(Container.ItemIndex + 1)%2==0?"style='background:#bddcf4'":"style='background:#e3f1fc'" %>>
                                <th align="center">
                                    <%#Eval("CityName")%>
                                </th>
                                <th align="center">
                                    <%#Eval("Saler")%>
                                </th>
                                <asp:Literal ID="ltrDepartStatInfo" runat="server"></asp:Literal>
                            </tr>
                        </ItemTemplate>
                    </cc1:CustomRepeater>
                    <tr id="trHeJi">
                        <td align="center" bgcolor="#bddcf4">
                            &nbsp;
                        </td>
                        <td align="center" bgcolor="#bddcf4">
                            <b>合计</b>
                        </td>
                        <asp:Literal ID="ltrHeJi" runat="server"></asp:Literal>
                    </tr>
                </table>
            </div>
            <table width="100%" id="tbl_ExPageEmp" runat="server" border="0" cellspacing="0"
                cellpadding="0">
                <tr>
                    <td align="right">
                        <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

    <script type="text/javascript">
     $(function() {
         $("#btnSearch").click(function() {
                search("");
                return false;
            });
            initSearch();
     });
     function search(type) {
        
            var cat = "";
            var st = $("#<%= txtStartDate.ClientID %>").val();
            var et = $("#<%= txtEndDate.ClientID %>").val();
            var SalerId = <%= SelectSalser.ClientID%>.GetOperatorId();  
            var SalerName=<%= SelectSalser.ClientID%>.GetOperatorName();  
            var province = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().id;
            var provincename=window["<%=ProvinceAndCity1.ClientID %>"].getProvince().name;
            var cityname=window["<%=ProvinceAndCity1.ClientID %>"].getCity().name;
            var cityId = window["<%=ProvinceAndCity1.ClientID %>"].getCity().id;    
            var tourType = <%=ddlTourType.ClientID %>.GetTourTypeId();      
            location.href = "AreaSoldStaList.aspx?" + cat + "st=" + st + "&et=" +
            et + "&SalerId=" + SalerId + "&cityId="+ cityId + "&province=" + province + "&tourType=" + tourType+"&salername="+SalerName+"&provincename="+provincename+"&cityname="+cityname;  
        }
    function toXls() {
        var s='<tr id="trHeJi">'+$("#trHeJi").html()+'</tr>';
        $("#trHeJi").remove();
        var content = $("#<%=divEmpIncomList.ClientID %>").html();
        $("#hidContent").val(content);
        $("#tbl_EmpIncomeList").append(s);
        
        $("#<%=form1.ClientID %>").submit(); 
        return false;
    };
    function initSearch(){
                var _params = utilsUri.getUrlParams([]);
                 window["<%=ProvinceAndCity1.ClientID %>"].setProvince(decodeURIComponent(_params["province"] || ""), decodeURIComponent(_params["provincename"] || ""));
         window["<%=ProvinceAndCity1.ClientID %>"].setCity(decodeURIComponent(_params["cityId"] || ""), decodeURIComponent(_params["cityname"] || ""));
            }
    </script>

</asp:Content>
