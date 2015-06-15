<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.Default"
    Title="考勤管理_行政中心" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">行政中心</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：行政中心>>考勤管理
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
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td height="50">
                    <div class="searchbox">
                        员工编号：
                        <input name="txt_WorkerNum" id="txt_WorkerNum" type="text" class="searchinput2" size="20" />
                        姓名：<input name="txt_Name" id="txt_Name" type="text" class="searchinput2" size="12" />
                        部门：
                        <select id="dpDepartment" name="dpDepartment" runat="server">
                        </select>
                        <a href="javascript:void(0);" onclick="AttDefault.OnSearch();return false;">
                            <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a id="a_Insert" href="javascript:void(0);">批量考勤</a>
                    </td>
                    <td width="90" align="center">
                        <a href="/administrativeCenter/attendanceManage/PersonalList.aspx" target="_blank">个人考勤表</a>
                    </td>
                    <td width="90" align="center">
                        <a href="/administrativeCenter/attendanceManage/CollectAllList.aspx" target="_blank">
                            考勤汇总表</a>
                    </td>
                    <%if (CheckGrant(Common.Enum.TravelPermission.行政中心_考勤管理_工资表))
                      {%>
                    <td width="90" align="center">
                        <a href="/administrativeCenter/attendanceManage/PersonnelSalary.aspx">
                            人事工资表</a>
                    </td>
                    <%} %>
                </tr>
            </table>
        </div>
        <div class="tablelist" id="divSearchContent" align="center">
        </div>
    </div>

    <script src="/js/Loading.js" type="text/javascript"></script>

    <script type="text/javascript">
        var Parms = { WorkerNum: "", Name: "", Department: "", Page: 1 };
        var AttDefault = {
            OnSearch: function() { //查询
                Parms.WorkerNum = $.trim($("#txt_WorkerNum").val());
                Parms.Name = $.trim($("#txt_Name").val());
                Parms.Department = $("#<%=dpDepartment.ClientID %>").val();
                Parms.Page = 1;
                AttDefault.GetAttendanceList();
            },
            GetAttendanceList: function() {//得到ajax数据
                LoadingImg.ShowLoading("divSearchContent");
                if (LoadingImg.IsLoadAddDataToDiv("divSearchContent")) {
                    $.newAjax({
                        type: "GET",
                        dateType: "html",
                        url: "/administrativeCenter/attendanceManage/AjaxAttendanceList.aspx",
                        data: Parms,
                        cache: false,
                        success: function(html) {
                            $("#divSearchContent").html(html);
                        }
                    });
                }
            },
            OpenForm: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            },
            OpenPage: function(Type, ID) {
                if (Type == "lot") {
                    AttDefault.OpenForm("/administrativeCenter/attendanceManage/AddRecord.aspx", "批量考勤登记", "640px", "290px", "");
                }
                else if (Type == "look") {
                    Boxy.iframeDialog({ title: "员工本月考勤明细",
                        iframeUrl: "/administrativeCenter/attendanceManage/LookDetail.aspx",
                        width: "740px", height: "420px",
                        model: true, data: {
                            WorkerID: ID
                        },
                        afterHide: function() {         //点击关闭 刷新
                            window.parent.document.location.reload();
                        }
                    });
                }
                else if (Type == "add") {
                    AttDefault.OpenForm("/administrativeCenter/attendanceManage/AddRecord.aspx", "考勤登记", "640px", "290px", "WorkerID=" + ID);
                }
            },
            LoadData: function(obj) {       //分页
                var Page = exporpage.getgotopage(obj);
                Parms.Page = Page;
                AttDefault.GetAttendanceList();
            }
        };
        $(function() {
            AttDefault.OnSearch();
            if ("<%=InsertFlag %>" == "True") {
                $("#a_Insert").show();
                $("#a_Insert").click(function() {
                    AttDefault.OpenPage('lot', '');
                    return false;
                });
            } else {
                $("#a_Insert").hide();
            }
        });
    </script>

</asp:Content>
