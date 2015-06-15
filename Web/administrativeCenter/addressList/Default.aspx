<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.addressList.Default" Title="内部通讯录_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>内部通讯录</td>
              </tr>
              <tr>
                <td colspan="2" height="2" bgcolor="#000000"></td>
              </tr>
          </table>  
        </div>
		<div class="hr_10"></div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
            <td valign="top"><div class="searchbox"  style="height:30px;">姓名：
              <input name="txt_WorkerName" type="text"  class="searchinput2" id="txt_WorkerName" size="20"/>
                部门：
                <select id="dpDepartment" name="dpDepartment" runat="server">
                </select>
                 <a href="javascript:void(0);" onclick="Default.OnSearch()">
                    <img src="/images/searchbtn.gif" style="vertical-align:top;"/></a>
            </div></td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
        <div class="btnbox">
          <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center">
                <a href="javascript:void(0);"  onclick="Default.GetPrintInfo();" >打印</a></td>
		       <td width="90" align="center">
		        <a href="javascript:void(0);" onclick="Default.GetExcelInfo();return false;">
		        <img src="/images/excel-xz.gif"/>导出Excel</a></td>
            </tr>
          </table>
        </div>
       	<div class="tablelist" id="divSearch" align="center">
       	</div>
	</div>
	<script src="/js/Loading.js" type="text/javascript"></script>
	<script type="text/javascript">
	    var Parms = { WorkerName: "", Department: "",  Method: "", Page: 1 };
	    var Default = {
	        OnSearch: function() { //查询
	            Parms.WorkerName = $.trim($("#txt_WorkerName").val());
	            Parms.Department = $.trim($("#<%=dpDepartment.ClientID %>").val());
	            Parms.Page = 1;
	            Default.GetAddressList();
	        },
	        GetAddressList: function() {//得到ajax数据
	            LoadingImg.ShowLoading("divSearch");
	            if (LoadingImg.IsLoadAddDataToDiv("divSearch")) {
	                $.newAjax({
	                    type: "GET",
	                    dateType: "html",
	                    url: "/administrativeCenter/addressList/AjaxDefault.aspx",
	                    data: Parms,
	                    cache: false,
	                    success: function(html) {
	                        $("#divSearch").html(html);
	                    }
	                });
	            }
	        },
	        GetPrintInfo: function() {      //打印
	            Parms.WorkerName = $.trim($("#txt_WorkerName").val());
	            Parms.Department = $.trim($("#<%=dpDepartment.ClientID %>").val());
	            var url = "/administrativeCenter/addressList/PrintAddressList.aspx";
	            window.open(url, "_blank");
	        },
	        GetExcelInfo: function() {      //导出
	            Parms.WorkerName = $.trim($("#txt_WorkerName").val());
	            Parms.Department = $.trim($("#<%=dpDepartment.ClientID %>").val());
	            Parms.Method = "GetExcel";
	            var url = "/administrativeCenter/addressList/AjaxDefault.aspx?" + $.param(Parms);
	            Parms.Method = "";
	            window.location.href = url;
	        },
	        LoadData: function(obj) {       //分页
	            var Page = exporpage.getgotopage(obj);
	            Parms.Page = Page;
	            Default.GetAddressList();
	        }
	    };
	    $(function() {
	        Default.OnSearch();
	    });
	</script>
</asp:Content>
