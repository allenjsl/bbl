<%@ Page Language="C#"  MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.cahierManage.Default" Title="会议记录管理_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
    <div class="lineprotitlebox">
       <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
            <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>会议记录管理</td>
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
        <td height="50">
          <div class="searchbox">会议编号：
          <input name="txt_MeetingNum" type="text"  class="searchinput2" id="txt_MeetingNum" size="13"  />
          会议主题：
          <input name="txt_MeetingTheme" type="text" class="searchinput2" id="txt_MeetingTheme" size="15"  />
          会议时间：
          <input name="txt_MeetingDateBegin" type="text"  class="searchinput2" id="txt_MeetingDateBegin" size="12" onfocus="WdatePicker()"  />
          至
          <input name="txt_MeetingDateEnd" type="text" class="searchinput2" id="txt_MeetingDateEnd" size="12" onfocus="WdatePicker()" />              
          <a href="javascript:void(0);" onclick="Default.OnSearch();"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a>
          </div>
        </td>
        <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
      </tr>
    </table>
    <div class="btnbox">
      <table id="table_Insert" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
          <td width="90" align="center"><a id="a_Insert" href="javascript:void(0);">新增</a></td>
        </tr>
      </table>
    </div>
   	<div class="tablelist"  id="divSearch" align="center">
    </div>
    
</div>

<script src="/js/Loading.js" type="text/javascript"></script>
<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    var Parms = { MeetingNum: "", MeetingTheme: "", MeetingDateBegin: "", MeetingDateEnd: "", Page: 1 };
    var Default = {
        OnSearch: function() { //查询
            Parms.MeetingNum = $.trim($("#txt_MeetingNum").val());
            Parms.MeetingTheme = $.trim($("#txt_MeetingTheme").val());
            Parms.MeetingDateBegin = $.trim($("#txt_MeetingDateBegin").val());
            Parms.MeetingDateEnd = $.trim($("#txt_MeetingDateEnd").val());
            Parms.Page = 1;
            Default.GetCahierList();
        },
        GetCahierList: function() {//得到ajax数据
            LoadingImg.ShowLoading("divSearch");
            if (LoadingImg.IsLoadAddDataToDiv("divSearch")) {
                $.newAjax({
                    type: "GET",
                    dateType: "html",
                    url: "AjaxCahierList.aspx",
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
        OpenPage: function(method, MeetingID) {
            if (method == "add") {
                Default.OpenForm("/administrativeCenter/cahierManage/EditCahier.aspx", "添加会议记录",
	                 "690px", "350px", "");
            }
            else {
                Default.OpenForm("/administrativeCenter/cahierManage/EditCahier.aspx", "修改会议记录",
	                 "690px", "350px", "MeetingID=" + MeetingID);
            }
        },
        DeleteMeeting: function(ID) {
            if (ID > 0) {
                if (window.confirm("您确认要删除此会议记录信息吗?\n此操作不可恢复!")) {
                    $.newAjax({
                        type: "GET",
                        dateType: "html",
                        url: "/administrativeCenter/cahierManage/AjaxCahierList.aspx",
                        data: {
                            MeetingID: ID,
                            Method: 'DeleteMeeting'
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
            Default.GetCahierList();
        }
    };
    $(document).ready(function() {
        $("#txt_MeetingNum,#txt_MeetingTheme,#txt_MeetingDateBegin,#txt_MeetingDateEnd").keydown(function() {
            if (event.keyCode == 13) {
                Default.OnSearch();
            }
        });
    }); 
    $(function() {
        if ("<%=InsertFlag %>" == "True") {
            $("#table_Insert").show();
            $("#a_Insert").click(function() {
                Default.OpenPage('add');
                return false;
            });
        } else {
            $("#table_Insert").hide();
        }
        Default.OnSearch();
    });
</script>
</asp:Content>