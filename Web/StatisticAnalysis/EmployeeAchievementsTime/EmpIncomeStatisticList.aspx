<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master" Title="按收入统计_员工业绩表_统计分析"
    CodeBehind="EmpIncomeStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.EmployeeAchievementsTime.EmpIncomeStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
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
                        所在位置>>统计分析 >> 员工业绩 >> 按收入统计
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
            <li><a href="/StatisticAnalysis/EmployeeAchievementsTime/EmpIncomeStatisticList.aspx"
                id="two1" class="tabtwo-on">按收入统计</a></li>
            <li><a href="/StatisticAnalysis/EmployeeAchievementsTime/EmpProfitStatisticList.aspx"
                id="two2">按利润统计</a></li>
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
                                出团日期：</label>
                            <input name="txtLeaveTourStartDate" runat="server" onfocus="WdatePicker()" type="text"
                                class="searchinput" id="txtLeaveTourStartDate" />到<input id="txtLeaveTourEndDate"
                                    runat="server" name="txtLeaveTourEndDate" onfocus="WdatePicker()" type="text"
                                    class="searchinput" />
                            &nbsp;&nbsp;<label>
                                签单日期：</label>
                            <input name="txtSignBillStartDate" type="text" runat="server" onfocus="WdatePicker()"
                                class="searchinput" id="txtSignBillStartDate" />到<input name="txtSignBillEndDate"
                                    runat="server" type="text" onfocus="WdatePicker()" class="searchinput" id="txtSignBillEndDate" />
                            <label>
                                部门：</label><uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCSelectDepartment" runat="server" />
                            <%--销售员--%>
                            <br />
                            &nbsp;<uc2:selectOperator Title="销售员：" ID="SelectSalser" runat="server" />
                            <%--计调员--%>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<uc2:selectOperator  Title="计调员：" ID="SelectAccounter" runat="server" />
                            &nbsp;<label>
                                提成比例：</label>
                            <input name="txtRoyaltyRatio" type="text" class="searchinput" runat="server" id="txtRoyaltyRatio" />
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
                        <!-- <td width="90" align="left"><a href="xsfxrent_list.html">统计表</a> </td>-->
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divEmpIncomList" runat="server">
                <table width="100%" id="tbl_EmpIncomeList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="13%" align="center" bgcolor="#BDDCF4">
                            销售员
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            部门
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            计调员
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            人数
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            收入
                        </th>
                        <th width="13%" align="center" bgcolor="#bddcf4">
                            提成
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_EmpIncomeList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetTourNum(<%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorId%>,1,0)"
                                        id="A2">
                                        <%#Eval("PeopleNum")%></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetTourNum(<%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorId%>,2,0)"
                                        id="linkTourNum">
                                       <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Income)%></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                   <font class="fred">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(GetRoyalty(Convert.ToString(Eval("Income"))))%></font>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetTourNum(<%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorId%>,1,0)"
                                        id="A1">
                                        <%#Eval("PeopleNum")%></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetTourNum(<%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorId%>,2,0)"
                                        id="linkTourNum">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Income)%></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <font class="fred">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(GetRoyalty(Convert.ToString(Eval("Income"))))%></font>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                </table>
                <table width="100%" id="tbl_ExPageEmp" runat="server" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
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
                    <th width="13%" align="center">
                        销售员
                    </th>
                    <th width="13%" align="center">
                        部门
                    </th>
                    <th width="13%" align="center">
                        计调员
                    </th>
                    <th width="13%" align="center">
                        人数
                    </th>
                    <th width="13%" align="center">
                        收入
                    </th>
                    <th width="13%" align="center">
                        提成
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                    <%#Eval("DepartName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Logistics)%>
                </td>
                <td align="center">
                        <%#Eval("PeopleNum")%>
                </td>
                <td align="center">
                        <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToString(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Income))%>
                </td>
                <td align="center">
                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(GetRoyalty(Convert.ToString(Eval("Income"))))%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                    <%#Eval("DepartName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Logistics)%>
                </td>
                <td align="center">
                        <%#Eval("PeopleNum")%>
                </td>
                <td align="center">
                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(((EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic)GetDataItem()).Income))%>
                </td>
                <td align="center">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(GetRoyalty(Convert.ToString(Eval("Income"))))%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </cc1:CustomRepeater>
    </form>

    <script type="text/javascript">
    $(function() {    
        $("#btnSearch").click(function() {//查询        
                var params={Accounter:"",AccounterId:"",LeaveTourStartDate:"",LeaveTourEndDate:"",SignBillStartDate:"",SignBillEndDate:"",Salser:"",SalserId:"",RoyaltyRatio:"",DepartName:"",DepartId:""};
                var SalerControlObj = <%=SelectSalser.ClientID %>;//获取销售员控件
                var AccounterControlObj=<%=SelectAccounter.ClientID %>;//获取计调员控件        
                var Accounter=AccounterControlObj.GetOperatorName();   
                var Salser=SalerControlObj.GetOperatorName();//销售员名字
                var LeaveTourStartDate=$.trim($("#<%=txtLeaveTourStartDate.ClientID %>").val());//出团开始时间
                var LeaveTourEndDate=$.trim($("#<%=txtLeaveTourEndDate.ClientID %>").val());//出团结束时间
                var SignBillStartDate=$.trim($("#<%=txtSignBillStartDate.ClientID %>").val());//签单开始时间 
                var SignBillEndDate=$.trim($("#<%=txtSignBillEndDate.ClientID %>").val());//签单结束时间                 
                params.DepartName=<%=UCSelectDepartment.ClientID %>.GetName();//部门名称
                params.DepartId=<%=UCSelectDepartment.ClientID %>.GetId();//部门ID
                params.Salser=Salser;//销售员名字
                params.SalserId=SalerControlObj.GetOperatorId();//销售员ID
                params.LeaveTourStartDate=LeaveTourStartDate;
                params.LeaveTourEndDate=LeaveTourEndDate;
                params.SignBillStartDate=SignBillStartDate;
                params.SignBillEndDate=SignBillEndDate; 
                params.Accounter=Accounter;//计调员   
                params.AccounterId=AccounterControlObj.GetOperatorId();//计调员Id  
                params.RoyaltyRatio=$.trim($("#<%=txtRoyaltyRatio.ClientID %>").val());    
                     
                window.location.href = "/StatisticAnalysis/EmployeeAchievementsTime/EmpIncomeStatisticList.aspx?" + $.param(params);
                return false;
        });        
        $("#LinkExport").click(function(){//导出
               if($("#tbl_EmpIncomeList").find("[id='EmptyData']").length>0)
               {
                    alert("暂无数据无法执行导出！");
                    return false;
               }
              var goToUrl = "/StatisticAnalysis/EmployeeAchievementsTime/EmpIncomeStatisticList.aspx?isExport=1";
              window.open(goToUrl,"Excel导出");
              return false;
        });  
 }); 
    </script>

    <script type="text/javascript">
function GetTourNum(SalserId,typeId,StaticType)
{
    var title="人数";
    if(typeId==1)
    {
       title="人数";
    }
    else if(typeId==2)
    {
       title="收入";
    }
   var AccounterControlObj=<%=SelectAccounter.ClientID %>;//获取计调员控件        
   var data={
       Type:StaticType,
       Accounter:AccounterControlObj.GetOperatorName(),
       AccounterId:AccounterControlObj.GetOperatorId(),
       LeaveTourStartDate:$.trim($("#<%=txtLeaveTourStartDate.ClientID %>").val()),
       LeaveTourEndDate:$.trim($("#<%=txtLeaveTourEndDate.ClientID %>").val()),
       SignBillStartDate:$.trim($("#<%=txtSignBillStartDate.ClientID %>").val()),
       SignBillEndDate:$.trim($("#<%=txtSignBillEndDate.ClientID %>").val()),
       SalserId:SalserId,
       RoyaltyRatio:$.trim($("#<%=txtRoyaltyRatio.ClientID %>").val())
           };
    var url="/StatisticAnalysis/EmployeeAchievementsTime/GetEmpTourList.aspx";
    Boxy.iframeDialog({
                        iframeUrl: url,
                        title: title,
                        modal: true,
                        width: "790px",
                        height: "300px",
                        data:data
                    });
                    return false;
}
    </script>

</asp:Content>
