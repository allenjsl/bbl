<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="PayAwake.aspx.cs" Inherits="Web.UserCenter.WorkAwake.PayAwake" Title="付款提醒_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
<script src="/js/utilsUri.js" type="text/javascript"></script>

<script type="text/javascript">
    function search() {
        var _departObj = eval("<%=txtQueryDepart.ClientID %>");
        var _operatorObj = eval("<%=txtQueryOperator.ClientID %>");
        var params = { "scname": $("#txtShouKuanDanWei").val()
            , "lsdate": $("#txtLeaveSDate").val()
            , "ledate": $("#txtLeaveEDate").val()
            , "departids": _departObj.GetId()
            , "operatorids": _operatorObj.GetOperatorId()
        };
        window.location.href = utilsUri.createUri(null, params);
    }

    $(document).ready(function() { $("#btnSearch").bind("click", function() { search(); return false; }); });

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="Form1" runat="server">
    
    <% if (!string.IsNullOrEmpty(Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 2)))
       { %>
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
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
                      <% =Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 2)%>
                    </tr>
                  </table>
                </div>
              <div class="btnbox" style="height:10px; margin:0px;padding:0px"></div>              
              <table cellpadding="0" cellspacing="0" style="width:100%; margin-top:10px; margin-bottom:10px; border:0px;">
                    <tr>
                        <td>收款单位：
                            <input type="text" name="txtShouKuanDanWei" id="txtShouKuanDanWei" class="searchinput searchinput02" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("scname") %>" style="width:80px" />
                            <label>出团时间：</label>
                            <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput" name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                            
                            <label>部门：</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="txtQueryDepart" runat="server" Style="width:80px;" />
                            <uc2:selectOperator Title="操作员：" ID="txtQueryOperator" runat="server" Style="width:80px;" />
                            <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a>
                            <a onclick="return toXls();return false;" href="javascript:void(0);"><img width="68" height="27" style="margin-top:0px;border:0px" alt="" src="/images/export.xls.png"></a>
                        </td>
                    </tr>
                </table>
              
           	  <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr bgcolor="#BDDCF4">
                    <%--<th width="8%" align="center" >编号</th>--%>
                    <th width="25%" align="center">收款单位</th>
                    <th width="13%" align="center">联系人</th>
                    <th width="18%" align="center" >电话</th>
                    <th width="13%" align="right" >欠款金额&nbsp;&nbsp;</th>
                    <th width="13%" align="center" >责任计调</th>
                    <th width="10%" align="center" >查看明细</th>
                  </tr>
                  
                  <asp:Repeater ID="rptlist" runat="server">
                    <ItemTemplate>
                      <tr tid="<%# Eval("SupplierId")%>_<%# (int)Eval("SupplierType")%>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <%--<td  align="center" ><%# Eval("CustomerId") %></td>--%>
                        <td align="center" class="pandl3"><%# Eval("SupplierName")%></td>
                        <td align="center"><%# Eval("ContactName") %></td>
                        <td align="center"><%# Eval("ContactTel")%></td>
                        <td align="right"><%#Eval("PayCash", "{0:c2}")%>&nbsp;&nbsp;</td>
                        <td align="center"><%# Eval("JobName")%></td>
                        <td align="center"><a href="javascript:;" class="show">查看</a></td>
                      </tr>
                     </ItemTemplate>
                  </asp:Repeater>
                  
                  <asp:PlaceHolder runat="server" ID="phWeiShouHeJi" Visible="false">
                  <tr bgcolor="#BDDCF4">
                    <td colspan="3" style="text-align:right;"><b>合计：</b></td>
                    <td style="text-align:right"><asp:Literal runat="server" ID="ltrWeiShouHeJi"></asp:Literal>&nbsp;&nbsp;</td>
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
	                var params = utilsUri.getUrlParams(["scname"]);
	                params["tid"] = tid;
	                params["type"] = "Pay";
	                Boxy.iframeDialog({
	                    iframeUrl: utilsUri.createUri("/UserCenter/WorkAwake/AwakeShow.aspx", params),
	                    title: "付款提醒查看",
	                    modal: true,
	                    width: "800px",
	                    height: "500px"
	                });
	                return false;
	            });
	        });
	        
		</script>

    </form>
</asp:Content>
