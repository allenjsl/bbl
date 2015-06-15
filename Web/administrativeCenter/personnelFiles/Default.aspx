<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.personnelFiles.Default" Title="人事档案_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server" style=" margin:0; padding:0;">
    <div class="mainbody">
		<div class="mainbody">
            <div class="lineprotitlebox">
               <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>人事档案</td>
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
                <div class="searchbox">
                    档案编号：<input name="txt_FileNo" id="txt_FileNo"  type="text"  class="searchinput2"  size="14"/>
                
                    姓名：<input name="txt_Name" type="text" class="searchinput2" id="txt_Name"  size="12" />
                    性别：
                    <asp:DropDownList ID="ddlSex" runat="server">
                        <asp:ListItem Selected="True" Value="0">请选择</asp:ListItem>
                        <asp:ListItem Value="1">女</asp:ListItem>
                        <asp:ListItem Value="2">男</asp:ListItem>
                    </asp:DropDownList>
                    出生日期：<input name="txt_BirthdayStart"  type="text"  class="searchinput2" 
                        id="txt_BirthdayStart"  size="9" onfocus="WdatePicker()"  /> -
                    <input name="txt_BirthdayEnd" type="text"  class="searchinput2" 
                        id="txt_BirthdayEnd"  size="9" onfocus="WdatePicker()" />工龄：<input name="txt_WorkYearFrom" type="text"  class="searchinput2" id="txt_WorkYearFrom"  size="3" />-<input name="txt_WorkYearTo" type="text"  class="searchinput2" id="txt_WorkYearTo"  size="3" />
                    职务：
                    <asp:DropDownList ID="ddlJobPostion" runat="server">
                        <asp:ListItem Selected="True" Value="0">请选择</asp:ListItem>
                        <asp:ListItem Value="1">财务总监</asp:ListItem>
                        <asp:ListItem Value="2">行政总监</asp:ListItem>
                        <asp:ListItem Value="3">出境中心执行总监</asp:ListItem>
                        <asp:ListItem Value="4">入境中心执行总监</asp:ListItem>
                        <asp:ListItem Value="5">地接中心行政总监</asp:ListItem>
                        <asp:ListItem Value="6">客服总监</asp:ListItem>
                        <asp:ListItem Value="7">策划总监</asp:ListItem>
                        <asp:ListItem Value="8">业务一部</asp:ListItem>
                        <asp:ListItem Value="9">总经理</asp:ListItem>
                        <asp:ListItem Value="10">销售一部</asp:ListItem>
                        <asp:ListItem Value="11">计调</asp:ListItem>
                    </asp:DropDownList>
                    类型：
                    <asp:DropDownList ID="ddlWorkerType" runat="server">
                        <asp:ListItem Selected="True" Value="-1">请选择</asp:ListItem>
                        <asp:ListItem Value="0">正式员工</asp:ListItem>
                        <asp:ListItem Value="1">试用期</asp:ListItem>
                        <asp:ListItem Value="2">学徒期</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    员工状态：
                    <asp:DropDownList ID="ddlWorkerState" runat="server">
                        <asp:ListItem Selected="True" Value="null">-请选择-</asp:ListItem>
                        <asp:ListItem Value="false">在职</asp:ListItem>
                        <asp:ListItem Value="true">离职</asp:ListItem>
                    </asp:DropDownList>
                    婚姻状况：
                    <asp:DropDownList ID="ddlMarriageState" runat="server">
                        <asp:ListItem Selected="True" Value="null">请选择</asp:ListItem>
                        <asp:ListItem Value="false">未婚</asp:ListItem>
                        <asp:ListItem Value="true">已婚</asp:ListItem>
                    </asp:DropDownList>
                    <a href="javascript:void(0);" onclick="PDefault.OnSearch();return false;"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a>
                             </div>
                </td>
                <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
              </tr>
        </table>
        <div class="btnbox">
          <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center"><a id="a_Insert" href="javascript:void(0);">新 增</a></td>
	           <td width="90" align="center"><a href="javascript:void(0);" onclick="PDefault.GetExcelInfo();return false;"><img src="/images/excel-xz.gif" style="vertical-align:top;"/>导出excel</a></td>
                <td width="90" align="center"><a id="aPrint" href="javascript:void(0);" target="_blank" onclick="PDefault.GetPrintInfo();return false;">打印</a></td>
            </tr>
          </table>
        </div>
        <div class="tablelist">
            <div id="DivPersonnelFileList" align="center">
            </div>
      </div>
	</div>     
  </div>
  <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
  <script src="/js/Loading.js" type="text/javascript"></script>
  <script type="text/javascript">
      var Parms = { FileNo: "", Name: "", Sex: "", BirthdayStart: "", BirthdayEnd: "", WorkYearFrom: "", WorkYearTo: "",
          JobPostion: "", WorkerType: "", WorkerState: "", MarriageState: "", personnelID: "", method: "", Page: 1
      };
      var PDefault = {
          OnSearch: function() {
              PDefault.GetSearchParms();
              PDefault.GetPersonnelInfoList();
          },
          GetPersonnelInfoList: function() {
              LoadingImg.ShowLoading("DivPersonnelFileList");
              if (LoadingImg.IsLoadAddDataToDiv("DivPersonnelFileList")) {
                  $.newAjax({
                      type: "GET",
                      dateType: "html",
                      url: "/administrativeCenter/personnelFiles/AjaxDefaultInfo.aspx",
                      data: Parms,
                      cache: false,
                      success: function(html) {
                          $("#DivPersonnelFileList").html(html);
                      }
                  });
              }
          },
          GetSearchParms: function() {
              Parms.FileNo = $.trim($("#txt_FileNo").val());
              Parms.Name = $.trim($("#txt_Name").val());
              Parms.Sex = $("#<%=ddlSex.ClientID %>").val();
              Parms.BirthdayStart = $.trim($("#txt_BirthdayStart").val());
              Parms.BirthdayEnd = $.trim($("#txt_BirthdayEnd").val());
              Parms.WorkYearFrom = $.trim($("#txt_WorkYearFrom").val());
              Parms.WorkYearTo = $.trim($("#txt_WorkYearTo").val());
              Parms.JobPostion = $("#<%=ddlJobPostion.ClientID %>").val();
              Parms.WorkerType = $("#<%=ddlWorkerType.ClientID %>").val();
              Parms.WorkerState = $("#<%=ddlWorkerState.ClientID %>").val();
              Parms.MarriageState = $("#<%=ddlMarriageState.ClientID %>").val();
              Parms.Page = 1;
          },
          GetPrintInfo: function() {
              PDefault.GetSearchParms();
              var url = "/administrativeCenter/personnelFiles/Print.aspx?" + $.param(Parms);
              window.open(url, "_blank");
          },
          GetDetailInfo: function(ID) {
              var url = "/administrativeCenter/personnelFiles/Detail.aspx?personnelID=" + ID;
              window.open(url, "_blank");
          },
          GetExcelInfo: function() {
              PDefault.GetSearchParms();
              Parms.method = "GetExcel";
              var url = "/administrativeCenter/personnelFiles/AjaxDefaultInfo.aspx?" + $.param(Parms);
              Parms.method = "";
              window.location.href = url;
          },
          DeletePersonnelInfo: function(ID) {
              if (ID > 0) {
                  if (window.confirm("您确认要删除此员工档案信息吗?\n此操作不可回复!")) {
                      $.newAjax({
                          url: "/administrativeCenter/personnelFiles/AjaxDefaultInfo.aspx",
                          data: {
                              personnelID: ID,
                              method: 'deletePersonnelInfo'
                          },
                          dataType: "html",
                          cache: false,
                          type: "GET",
                          success: function(result) {
                              if (result == "True") {
                                  alert("删除成功！");
                                  PDefault.GetSearchParms();
                                  PDefault.GetPersonnelInfoList();
                              } else {
                                  alert("删除失败！");
                              }
                          }
                      });
                  }
              } else {
                  alert("没有该职位信息，请确认！");
              }
          },
          LoadData: function(obj) {       //分页
              var Page = exporpage.getgotopage(obj);
              Parms.Page = Page;
              PDefault.GetPersonnelInfoList();
          }
      };
      $(function() {
          PDefault.OnSearch();
          if ("<%=InsertFlag %>" == "True") {
              $("#a_Insert").show();
              $("#a_Insert").click(function() {
                  window.location.href = "/administrativeCenter/personnelFiles/Edit.aspx";
                  return false;
              });
          } else {
            $("#a_Insert").hide();
          }
      });
  </script>
  </form>
</asp:Content>