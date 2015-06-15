<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.bylaw.Default" Title="规章制度_行政中心" %>



<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>规章制度</td>
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
            <td height="50"><div class="searchbox">编号：
              <input name="txt_Number" type="text"  class="searchinput2" id="txt_Number" size="20"/>
              制度标题：
              <input name="txt_RegentTitle" type="text" class="searchinput2" id="txt_RegentTitle" size="25" />
              <a href="javascript:void(0);" onclick="Default.OnSearch();"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a>
              </div></td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
        <div class="btnbox">
          <table id="table_Insert" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center">
                <a id="a_Insert" href="javascript:void(0);">新增</a>
              </td>
            </tr>
          </table>
        </div>
        <div class="tablelist"  id="divSearch" align="center">
        </div>
	</div>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
	<script src="/js/Loading.js" type="text/javascript"></script>
	<script type="text/javascript">
	    var Parms = { Number: "", RegentTitle: "", Page: 1 };
	    var Default = {
	        OnSearch: function() { //查询
	            Parms.Number = $.trim($("#txt_Number").val());
	            Parms.RegentTitle = $.trim($("#txt_RegentTitle").val());
	            Parms.Page = 1;
	            Default.GetBylawList();
	        },
	        GetBylawList: function() {//得到ajax数据
	            LoadingImg.ShowLoading("divSearch");
	            if (LoadingImg.IsLoadAddDataToDiv("divSearch")) {
	                $.newAjax({
	                    type: "GET",
	                    dateType: "html",
	                    url: "/administrativeCenter/bylaw/AjaxBylaw.aspx",
	                    data: Parms,
	                    cache: false,
	                    success: function(html) {
	                        $("#divSearch").html(html);
	                    }
	                });
	            }
	        },
	        LoadData: function(obj) {       //分页
	            var Page = exporpage.getgotopage(obj);
	            Parms.Page = Page;
	            Default.GetBylawList();
	        },
	        OpenForm: function(strurl, strtitle, strwidth, strheight, strdate) {
	            Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight,
	                draggable: true, data: strdate
	            });
	        },
	        OpenPage: function(ID, type) {
	            var Number = $("#txt_Number");
	            if (type == "add") {
	                Default.OpenForm("/administrativeCenter/bylaw/EditBylaw.aspx", "添加规章制度",
	                    "690px", "400px", "");
	            } else {
	                Default.OpenForm("/administrativeCenter/bylaw/EditBylaw.aspx", "修改规章制度",
	                    "690px", "400px", "DutyID=" + ID);
	            }
	        },
	        DeleteDuty: function(ID) {
	            if (ID > 0) {
	                if (window.confirm("您确认要删除此规章制度信息吗?\n此操作不可恢复!")) {
	                    $.newAjax({
	                        type: "GET",
	                        dateType: "html",
	                        url: "/administrativeCenter/bylaw/AjaxBylaw.aspx",
	                        data: {
	                            DutyID: ID,
	                            Method: 'DeleteDuty'
	                        },
	                        cache: false,
	                        async: false,
	                        success: function(result) {
	                            if (result == "True") {
	                                alert("删除成功！");
	                                Default.OnSearch();
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
	            Default.GetBylawList();
	        }
	    };
	    $(document).ready(function() {
	        $("#txt_Number,#txt_RegentTitle").keydown(function() {
	            if (event.keyCode == 13) {
	                Default.OnSearch();
	            }
	        });
	    }); 
	    $(function() {
	        if ("<%=InsertFlag %>" == "True") {
	            $("#table_Insert").show();
	            $("#a_Insert").click(function() {
	                Default.OpenPage('', 'add');
	                return false;
	            });
	        } else {
	            $("#table_Insert").hide();
	        }
	        Default.OnSearch();
	    });
	</script>
    </form>
</asp:Content>
