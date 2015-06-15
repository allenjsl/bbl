<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleServeList.aspx.cs"
    Inherits="Web.SingleServe.SingleServeList" MasterPageFile="/masterpage/Back.Master"
    Title="单项服务" %>

<%@ Import Namespace="EyouSoft.Common" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/Loading.js"></script>

    <div class="mainbody" >
        <form id="form1" runat="server">
        <div class="lineprotitlebox" >
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">单项服务</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt; 单项服务 &gt;&gt; 单项服务 
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <div class="searchbox" id="tbl_SearchSingleServe" style=" width:100%">
            订单号：
            <input name="txtOrderNo" runat="server" type="text" class="searchinput" id="txtOrderNo" />&nbsp;团号:<input
                name="txtTourCode" runat="server" type="text" class="searchinput" id="txtTourCode" />
            &nbsp;<label>下单时间：</label>
            <input name="txtOrderStartTime" type="text" runat="server" class="searchinput" id="txtOrderStartTime"
                onfocus="WdatePicker()" />到<input name="txtOrderEndTime" type="text" runat="server"
                    class="searchinput" id="txtOrderEndTime" onfocus="WdatePicker()" />
                    <br />
            <uc1:selectOperator Title="操作员：" IsMulutSelec="false" ID="selectOperator1" runat="server" />
            
            客户单位：
            <input name="txtCustomerCompany" type="text" class="searchinput" runat="server" id="txtCustomerCompany" />
            <input id="btnSearch" type="image" src="/images/searchbtn.gif" style="vertical-align: top;"
                value="button" alt="查询" />
        </div>
        <div id="divSingleServeList">
        </div>

        <script type="text/javascript">
         var Params = {
         OrderNo: "", 
         OrderStartTime: "",
         OrderEndTime: "", 
         Operator: "",
         OperatorId:"", 
         CustomerCompany: "",
         TourCode:"",
         Page:1 };
         var SingleServeList = {
             GetSingleServeList: function() {//获取单项服务列表
                LoadingImg.ShowLoading("divSingleServeList");
                if (LoadingImg.IsLoadAddDataToDiv("divSingleServeList")) {
             
                 $.newAjax({
                     type: "GET",
                     dataType: 'html',
                     url: "/SingleServe/AjaxSingleServeList.aspx",
                     data: Params,
                     cache: false,
                     success: function(html) {
                         $("#divSingleServeList").html(html);
                     },
                     error: function(xhr, s, errorThrow) {
                         $("#divSingleServeList").html("未能成功获取响应结果")
                     }
                 });
                 }
             },
              LoadData: function(obj) {//分页
                var Page = exporpage.getgotopage(obj);
                Params.Page = Page;
                this.GetSingleServeList();
            },
             OnSearch: function() {//查询                  
                 var operatorControl=<%=selectOperator1.ClientID %>;  //获取选择用户控件                       
                 Params.OrderNo = $("#<%=txtOrderNo.ClientID %>").val();
                 Params.OrderStartTime = $("#<%=txtOrderStartTime.ClientID %>").val();
                 Params.OrderEndTime = $("#<%=txtOrderEndTime.ClientID %>").val();
                 Params.OperatorId=operatorControl.GetOperatorId();//操作人ID
                 Params.Operator =operatorControl.GetOperatorName();//操作人  
                 Params.Page=1;      
                 Params.TourCode = $("#<%=txtTourCode.ClientID %>").val();         
                 Params.CustomerCompany = $.trim($("#<%=txtCustomerCompany.ClientID %>").val());
                 SingleServeList.GetSingleServeList();
             }
         };
         //回车查询
         $(document).ready(function() {
             $("#tbl_SearchSingleServe input").bind("keypress", function(e) {
                 if (e.keyCode == 13) {
                     SingleServeList.OnSearch();
                     return false;
                 }
             });
            SingleServeList.GetSingleServeList();           
             $("#btnSearch").click(function() {
                 SingleServeList.OnSearch();
                 return false;
             });
         });
        </script>

        </form>
    </div>
</asp:Content>
