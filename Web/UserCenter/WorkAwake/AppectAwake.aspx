<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="AppectAwake.aspx.cs" Inherits="Web.UserCenter.WorkAwake.AppectAwake" Title="收款提醒_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
<script src="/js/utilsUri.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">

    <form runat="server">
    
    <% if (!string.IsNullOrEmpty(Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 5)))
       { %>
    <div class="mainbody">

			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> <a href="#">个人中心</a>>> 信息管理</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:30px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <% =Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 1)%>
                    </tr>
                  </table>
                </div>
                <div class="btnbox" style="height:10px; margin:0px;padding:0px"></div>              
                <table cellpadding="0" cellspacing="0" style="width:100%; margin-top:10px; margin-bottom:10px; border:0px;">
                    <tr>
                        <td>客源单位：
                            <input type="text" name="txtSN" id="txtSN" class="searchinput searchinput02" value="<%=S_QianKuanDanWei %>" style="width:80px" />
                            <label>出团时间：</label>
                            <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput" name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                            
                            <label>部门：</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="txtQueryDepart" runat="server" Style="width:80px;" />
                            <uc2:selectOperator Title="操作员：" ID="txtQueryOperator" runat="server" Style="width:80px;" />
                            <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a>
                            <a onclick="toXls();return false;" href="javascript:void(0);"><img width="68" height="27" style="margin-top:0px;border:0px" alt="" src="/images/export.xls.png"></a>
                        </td>
                    </tr>
                </table>
              
           	  <div class="tablelist" runat="server" id="tbShow">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr bgcolor="#BDDCF4">
                    
                    <th width="25%" align="center" >客源单位</th>
                    <th width="13%" align="center" >联系人</th>
                    <th width="18%" align="center" >电话</th>
                    <th width="13%" align="right" >待收金额&nbsp;&nbsp;</th>
                    <th width="13%" align="center" >责任销售</th>
                    <th width="10%" align="center" >查看明细</th>
                  </tr>
                  
                  <asp:Repeater ID="rptlist" runat="server">
                    <ItemTemplate>
                      <tr tid="<%# Eval("CustomerId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>" sellerId="<%#Eval("SellerId") %>">
                        
                        <td align="center" class="pandl3"><%# Eval("CustomerName")%></td>
                        <td align="center"><%# Eval("ContactName") %></td>
                        <td align="center"><%# Eval("ContactTel")%></td>
                        <td align="right"><%#Eval("ArrearCash", "{0:c2}")%>&nbsp;&nbsp;</td>
                        <td align="center"><%# Eval("SalerName")%></td>
                        <td align="center"><a href="javascript:;" class="show">查看</a></td>
                      </tr>
                     </ItemTemplate>
                  </asp:Repeater>
                  <asp:PlaceHolder runat="server" ID="phDaiShouKuanHeJi" Visible="false">
                  <tr bgcolor="#BDDCF4">
                    <td colspan="3" style="text-align:right;"><b>合计：</b></td>
                    <td style="text-align:right"><asp:Literal runat="server" ID="ltrDaiShouKuanHeJi"></asp:Literal>&nbsp;&nbsp;</td>
                    <td colspan="2"></td>
                  </tr>
                  </asp:PlaceHolder>
                  <%if (len == 0)
                    { %>
                    <tr align="center"><td colspan="5">没有相关数据</td></tr>
                  <%} %>
              </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td height="30" align="right" class="pageup" colspan="13">
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3"  PageStyleType="NewButton" CurrencyPageCssClass="RedFnt" />
                    </td>
                  </tr>
                </table>
           	  </div>
                <div id="tbinfo" runat=server></div>
			</div>
			<!-- InstanceEndEditable -->
	    </div>
	<%}
       else
       {%>
       <div style="height:500px"></div>
	<%} %>
	    
	    <script type="text/javascript">
	        $(function() {
	            $("a.show").click(function() {
	                var that = $(this);
	                var tid = that.parent().parent().attr("tid");
	                var sellerId = that.parent().parent().attr("sellerId")
	                var url = "/UserCenter/WorkAwake/AwakeShow.aspx?";

	                var _departObj = eval("<%=txtQueryDepart.ClientID %>");
	                var _operatorObj = eval("<%=txtQueryOperator.ClientID %>");

	                var params = utilsUri.getUrlParams(["sn", "page"]);
	                params["tid"] = tid;
	                params["sellerid"] = sellerId;
	                params["type"] = "Appect";

	                Boxy.iframeDialog({
	                    iframeUrl: utilsUri.createUri(url, params),
	                    title: "收款提醒查看",
	                    modal: true,
	                    width: "800px",
	                    height: "500px"
	                });
	                return false;
	            });
	            $("#btnSearch").bind("click", function() { search(); return false; }); ;
	        });

            //查询按钮事件
	        function search() {
	            var _departObj =eval("<%=txtQueryDepart.ClientID %>");
	            var _operatorObj = eval("<%=txtQueryOperator.ClientID %>");

	            var params = { "sn": $.trim($("#txtSN").val())
                    , "lsdate": $("#txtLeaveSDate").val()
                    , "ledate": $("#txtLeaveEDate").val()
                    , "departids": _departObj.GetId()
                    , "operatorids": _operatorObj.GetOperatorId()                    
	            };

	            window.location.href = "AppectAwake.aspx?" + $.param(params);
	            return false;
	        }

            //导出
	        function toXls() {
	            var params = utilsUri.getUrlParams([]);
	            params["recordcount"] = recordCount;
	            params["istoxls"] = 1;
	            
	            if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

	            window.location.href = utilsUri.createUri(null, params);
	            return false;
	        }
        </script>
      
      </form>

</asp:Content>
