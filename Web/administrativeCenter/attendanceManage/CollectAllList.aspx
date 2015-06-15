<%@ Page Language="C#" MasterPageFile="/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="CollectAllList.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.CollectAllList" ValidateRequest="false" Title="考勤汇总表_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
<table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="35" align="center"><h2><strong>考勤汇总表</strong></h2></td>
  </tr>
</table>
<div ref="noprint">
<table width="800" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="35" align="left" >
    年份：<select id="dpYear" name="dpYear" style=" width:65px;" runat="server">
          <option value="0" selected="selected">请选择</option>
          <option value="1">2010</option>
    </select> 月份：<select id="dpMonth" name="dpMonth" style=" width:65px;" runat="server">
        <option value="1">1月</option>
        <option value="2">2月</option>
        <option value="3">3月</option>
        <option value="4">4月</option>
        <option value="5">5月</option>
        <option value="6">6月</option>
        <option value="7">7月</option>
        <option value="8">8月</option>
        <option value="9">9月</option>
        <option value="10">10月</option>
        <option value="11">11月</option>
        <option value="12">12月</option>
    </select>部门：<select id="dpDepartment" name="dpDepartment" style=" width:100px;" runat="server">
    </select>
    员工编号：<input name="txt_WorkerNum" id="txt_WorkerNum" type="text" class="searchinput2" size="15" />
    姓名：<input name="txt_Name" id="txt_Name" type="text" class="searchinput2" size="10" />
    <input type="button" name="txt_Submit" id="txt_Submit" value="查询" onclick="return CollectAllList.OnSearch();" />
    </td>
  </tr>
</table>
</div>
<div id="divSearch" align="center"></div>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/Loading.js" type="text/javascript"></script>
    <script type="text/javascript">
        var Parms = { Year: "", Month: "", DepartmentID: "", WorkerNum: "", Name: "", method: "" };
        var CollectAllList = {
            OnSearch: function() { //查询
                Parms.Year = $("#<%=dpYear.ClientID %>").val();
                Parms.Month = $.trim($("#<%=dpMonth.ClientID%>").val());
                Parms.DepartmentID = $("#<%=dpDepartment.ClientID%>").val();
                Parms.WorkerNum = $.trim($("#txt_WorkerNum").val());
                Parms.Name = $.trim($("#txt_Name").val());
                Parms.method = "CollectAllList";
                CollectAllList.GetCollectAllList();
            },
            GetCollectAllList: function() {//得到ajax数据
                LoadingImg.ShowLoading("divSearch");
                if (LoadingImg.IsLoadAddDataToDiv("divSearch")) {
                    $.newAjax({
                        type: "GET",
                        dateType: "html",
                        url: "/administrativeCenter/attendanceManage/AjaxList.aspx",
                        data: Parms,
                        cache: false,
                        success: function(html) {
                            $("#divSearch").html(html);
                        }
                    });
                }
            }
        };
        $(function() {
            CollectAllList.OnSearch();
        });
    </script>
</asp:Content>
