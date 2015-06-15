<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddRecord.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.AddRecord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px;">
        <tr>
            <td height="26" align="left" bgcolor="#bddcf4" class="pandl10">
                <span style="color: Red;">*</span>考勤时间：
                <input name="txt_AttStartDate" id="txt_AttStartDate" type="text" class="searchinput2"
                    size="15" valid="required" errmsg="请输入考勤开始时间" onclick="WdatePicker()" />
                <span id="spanMany" runat="server">至
                    <input name="txt_AttEndDate" id="txt_AttEndDate" type="text" class="searchinput2"
                        size="15" valid="required" errmsg="&nbsp;&nbsp;&nbsp;&nbsp;请输入考勤结束时间" onfocus="WdatePicker()" />
                    <span style="color: Red;">*</span> 考勤人员：<input name="txt_Name" id="txt_Name" readonly="readonly"
                        style="background-color: #CBCBCB;" type="text" class="searchinput2" size="25"
                        valid="required" errmsg="&nbsp;&nbsp;&nbsp;&nbsp;请选择考勤人员" />
                    <a href="javascript:void(0);" onclick="AttAddRecord.SelectWorker();return false;">
                        <img src="/images/sanping_04.gif" border="0" style="vertical-align: top;" /></a>
                    <input id="hiddenID" type="hidden" name="hiddenID" />
                </span>
            </td>
        </tr>
        <tr>
            <td align="left" bgcolor="#bddcf4" class="pandl10">
                <span id="errMsg_txt_AttStartDate" style="display: none; color: Red;"></span><span
                    id="errMsg_txt_AttEndDate" style="display: none; color: Red;"></span><span id="errMsg_txt_Name"
                        style="display: none; color: Red;"></span>
            </td>
        </tr>
        <tr>
            <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                <input type="radio" name="radio1" id="rd_OnTime" value="0" checked="checked" onclick="return AttAddRecord.HidLeave();" />
                <label for="rd_OnTime">
                    准点</label>
                <span id="spanState1" runat="server">
                    <input type="radio" name="radio1" id="rd_ToLate" value="1" onclick="return AttAddRecord.HidLeave();" />
                    <label for="rd_ToLate">
                        迟到</label>
                    <input type="radio" name="radio1" id="rd_Absenteeism" value="3" onclick="return AttAddRecord.HidLeave();" />
                    <label for="rd_Absenteeism">
                        旷工</label>
                    <input type="radio" name="radio1" id="rd_DayOff" value="4" onclick="return AttAddRecord.HidLeave();" />
                    <label for="rd_DayOff">
                        休假</label>
                    <input type="radio" name="radio1" id="rd_LeaveFor" value="7" onclick="return AttAddRecord.HidLeave();" />
                    <label for="rd_LeaveFor">
                        请假</label>
                </span>
            </td>
        </tr>
        <span id="spanState2" runat="server">
            <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                    <input type="checkbox" name="checkbox1" id="chb_WorkOverTime" value="8" onclick="return AttAddRecord.HidWorkOverTime();" />
                    <label for="chb_WorkOverTime">
                        加班</label>
                    <input type="checkbox" name="checkbox1" id="chk_LeaveEarly" value="2" />
                    <label for="chk_LeaveEarly">
                        早退</label>
                    <input type="checkbox" name="checkbox1" id="chk_GoOut" value="5" />
                    <label for="chk_GoOut">
                        外出</label>
                    <input type="checkbox" name="checkbox1" id="chk_GoGroup" value="6" />
                    <label for="chk_GoGroup">
                        出团</label>
                </td>
            </tr>
            <tr>
                <td bgcolor="#e3f1fc" class="pandl10">
                    <div id="spanLeave" style="width: 100%; display: none;">
                        <table>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    &nbsp;请假原因：
                                    <input name="txt_LeaveWhy" id="txt_LeaveWhy" type="text" class="searchinput2" size="60" />
                                </td>
                            </tr>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    <span style="color: Red;">*</span>请假时间：
                                    <input name="txt_LeaveForStartDate" id="txt_LeaveForStartDate" type="text" class="searchinput2"
                                        onfocus="WdatePicker()" size="12" />
                                    至
                                    <input name="txt_LeaveForEndDate" id="txt_LeaveForEndDate" type="text" class="searchinput2"
                                        onfocus="WdatePicker()" size="12" />
                                    <span id="errMsg_txt_LeaveForStartDate" style="display: none; color: Red;"></span>
                                    &nbsp;&nbsp; <span id="errMsg_txt_LeaveForEndDate" style="display: none; color: Red;">
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    &nbsp;请假天数：
                                    <input name="txt_LeaveDayNum" id="txt_LeaveDayNum" type="text" class="searchinput2"
                                        size="15" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td bgcolor="#e3f1fc" class="pandl10">
                    <div id="spanWorkOverTime" style="width: 100%; display: none;">
                        <table>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    &nbsp;加班内容：
                                    <input name="txt_WorkOverTimeContent" id="txt_WorkOverTimeContent" type="text" class="searchinput2"
                                        size="60" />
                                </td>
                            </tr>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    <span style="color: Red;">*</span>加班时间：
                                    <input name="txt_WorkOverTimeDateStart" id="txt_WorkOverTimeDateStart" type="text"
                                        class="searchinput2" onfocus="WdatePicker()" size="12" />
                                    至
                                    <input name="txt_WorkOverTimeDateEnd" id="txt_WorkOverTimeDateEnd" type="text" class="searchinput2"
                                        onfocus="WdatePicker()" size="12" />
                                    <span id="errMsg_txt_WorkOverTimeDateStart" style="display: none; color: Red;"></span>
                                    &nbsp;&nbsp; <span id="errMsg_txt_WorkOverTimeDateEnd" style="display: none; color: Red;">
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">
                                    &nbsp;加班时数：
                                    <input name="txt_WorkOverTimeNum" id="txt_WorkOverTimeNum" type="text" class="searchinput2"
                                        size="15" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </span>
        <tr>
            <td height="40" align="center" bgcolor="#bddcf4">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="86" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return AttAddRecord.Save('Save');">确认</a>
                        </td>
                        <td width="86" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="top.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">
                                取消</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidWorkerID" type="hidden" runat="server" value="" />
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        var AttAddRecord = {
            HidLeave: function() {//请假
                if ($("#rd_LeaveFor").attr("checked")) {
                    $("#spanLeave").css("display", "");
                    $("#chb_WorkOverTime").attr("checked", "");
                    AttAddRecord.HidWorkOverTime();
                } else {
                    $("#spanLeave").css("display", "none");
                }
            },
            HidWorkOverTime: function() {//加班
                if ($("#chb_WorkOverTime").attr("checked")) {
                    $("#spanWorkOverTime").css("display", "");
                    $("#rd_LeaveFor").attr("checked", "");
                    AttAddRecord.HidLeave();
                } else {
                    $("#spanWorkOverTime").css("display", "none");
                }
            },
            Save: function(method) {//保存
                var Result = false;
                Result = ValiDatorForm.validator($("#form1").get(0), "alert");
                if ($("#chb_WorkOverTime").attr("checked")) {
                    if ($("#txt_WorkOverTimeDateStart").val() == "") {
                        $("#errMsg_txt_WorkOverTimeDateStart").show().html("请输入加班开始时间");
                        Result = false;
                    } else {
                        $("#errMsg_txt_WorkOverTimeDateStart").css("display", "none");
                    }

                    if ($("#txt_WorkOverTimeDateEnd").val() == "") {
                        $("#errMsg_txt_WorkOverTimeDateEnd").show().html("请输入加班结束时间");
                        Result = false;
                    } else {
                        $("#errMsg_txt_WorkOverTimeDateEnd").css("display", "none");
                    }
                }
                if ($("#rd_LeaveFor").attr("checked")) {
                    if ($("#txt_LeaveForStartDate").val() == "") {
                        $("#errMsg_txt_LeaveForStartDate").show().html("请输入请假开始时间");
                        Result = false;
                    } else {
                        $("#errMsg_txt_LeaveForStartDate").css("display", "none");
                    }

                    if ($("#txt_LeaveForEndDate").val() == "") {
                        $("#errMsg_txt_LeaveForEndDate").show().html("请输入请假结束时间");
                        Result = false;
                    } else {
                        $("#errMsg_txt_LeaveForEndDate").css("display", "none");
                    }
                }
                if (!Result) {
                    return false;
                }

                $("#hidMethod").attr("value", method);
                $("#<%=form1.ClientID %>").get(0).submit();
                return false;
            },
            QueryString: function(val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            },
            SelectWorker: function() {
                var iframeId = AttAddRecord.QueryString("iframeid");
                parent.Boxy.iframeDialog({ title: "选择考勤人员",
                    iframeUrl: "/administrativeCenter/attendanceManage/SelectWorker.aspx",
                    width: "640px", height: "460px",
                    model: true, data: {
                        desid: iframeId
                    }
                });
            }
        };
        $(function() {
            FV_onBlur.initValid($("#form1").get(0));
        });
    </script>

    </form>
</body>
</html>
