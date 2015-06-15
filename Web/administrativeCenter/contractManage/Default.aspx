<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.contractManage.Default" Title="劳动合同管理_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
	<div class="mainbody">
        <div class="lineprotitlebox">
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>劳动合同管理</td>
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
            <td height="50"><div class="searchbox">员工编号：<input name="txt_WorkerNo" type="text"  class="searchinput2" id="txt_WorkerNo" size="8"/>
              姓名：<input name="txt_WorkerName" type="text"  class="searchinput2" id="txt_WorkerName" size="10"/>
              签订时间：<input name="txt_TimeOfSigningStart" type="text" class="searchinput2" id="txt_TimeOfSigningStart" size="12" onfocus="WdatePicker()" />-<input name="txt_TimeOfSigningEnd" type="text" class="searchinput2" id="txt_TimeOfSigningEnd" size="12" onfocus="WdatePicker()" />
到期时间：<input name="txt_ComeDueStart" type="text" class="searchinput2" id="txt_ComeDueStart" size="12" onfocus="WdatePicker()" />-<input name="txt_ComeDueEnd" type="text" class="searchinput2" id="txt_ComeDueEnd" size="12" onfocus="WdatePicker()" />
<a href="javascript:void(0);" onclick="Default.OnSearch();"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a>
              </div>
            </td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
        <div class="btnbox">
          <table id="table_Insert" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center">
                <a id="a_Insert" href="javascript:void(0);">新增</a></td>
            </tr>
          </table>
        </div>
        <div class="tablelist" id="divSearch" align="center">
    	
   	    </div>
	</div>
	<script src="/js/Loading.js" type="text/javascript"></script>
	<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
	<script type="text/javascript">
	    var Parms = { WorkerNo: "", WorkerName: "", TimeOfSigningStart: "", TimeOfSigningEnd: "",
	        ComeDueStart: "", ComeDueEnd: "", Page: 1
	    };
	    var Default = {
	        OnSearch: function() { //查询
	            Parms.WorkerNo = $.trim($("#txt_WorkerNo").val());
	            Parms.WorkerName = $.trim($("#txt_WorkerName").val());
	            Parms.TimeOfSigningStart = $.trim($("#txt_TimeOfSigningStart").val());
	            Parms.TimeOfSigningEnd = $.trim($("#txt_TimeOfSigningEnd").val());
	            Parms.ComeDueStart = $.trim($("#txt_ComeDueStart").val());
	            Parms.ComeDueEnd = $.trim($("#txt_ComeDueEnd").val());
	            Parms.Page = 1;
	            Default.GetContractList();
	        },
	        GetContractList: function() {//得到ajax数据
	            LoadingImg.ShowLoading("divSearch");
	            if (LoadingImg.IsLoadAddDataToDiv("divSearch")) {
	                $.newAjax({
	                    type: "GET",
	                    dateType: "html",
	                    url: "/administrativeCenter/contractManage/AjaxContractList.aspx",
	                    data: Parms,
	                    cache: false,
	                    success: function(html) {
	                        $("#divSearch").html(html);
	                    }
	                });
	            }
	        },
	        OpenForm: function(strurl, strtitle, strwidth, strheight, strdate) {
	            Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight,
	                draggable: true, data: strdate
	            });
	        },
	        OpenPage: function(method, workerid) {
	            if (method == "add") {
	                Default.OpenForm("/administrativeCenter/contractManage/EditContract.aspx", "添加劳动合同",
	                 "640px", "290px", "true");
	            }
	            if (method == "update") {
	                Default.OpenForm("/administrativeCenter/contractManage/EditContract.aspx", "修改劳动合同",
	                 "640px", "290px", "WorkerID=" + workerid);
	            }
	        },
	        DeleteContract: function(ID) {
	            if (ID > 0) {
	                if (window.confirm("您确认要删除此劳动合同信息吗?\n此操作不可恢复!")) {
	                    $.newAjax({
	                        type: "GET",
	                        dateType: "html",
	                        url: "/administrativeCenter/contractManage/AjaxContractList.aspx",
	                        data: {
	                            WorkerID: ID,
	                            Method: 'DeleteContract'
	                        },
	                        cache: false,
	                        async: false,
	                        success: function(result) {
	                            if (result == "True") {
	                                alert("删除成功！");
	                                Default.OnSearch();
	                            } else if (result == "NoPermission") {
	                                alert("没有删除权限！");
	                            } else {
	                                alert("删除失败！");
	                            }
	                        }
	                    });
	                }
	            } else {
	                alert("没有该记录。");
	            }
	        },
	        LoadData: function(obj) {       //分页
	            var Page = exporpage.getgotopage(obj);
	            Parms.Page = Page;
	            Default.GetContractList();
	        }
	    };
	    $(document).ready(function() {
	        $("#txt_WorkerNo,#txt_WorkerName,#txt_TimeOfSigningStart,#txt_TimeOfSigningEnd,#txt_ComeDueStart,#txt_ComeDueEnd").keydown(function() {
	            if (event.keyCode == 13) {
	                Default.OnSearch();
	            }
	        });
	    }); 
	    $(function() {
	        if ("<%=InsertFlag %>" == "True") {
	            $("#table_Insert").show();
	            $("#a_Insert").click(function() {
	                Default.OpenPage('add', '');
	                return false;
	            });
	        } else {
	            $("#table_Insert").hide();
	        }
	        Default.OnSearch();
	    });
	</script>
</asp:Content>