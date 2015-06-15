<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateRecord.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.UpdateRecord" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px;">
      <tr>
        <td width="83" height="26" align="center" bgcolor="#bddcf4"><strong>考勤时间</strong></td>
        <td width="62" height="26" align="center" bgcolor="#bddcf4"><strong>结果</strong></td>
        <td width="379" height="26" align="center" bgcolor="#bddcf4"><strong>备注</strong></td>
      </tr>
      <cc1:CustomRepeater ID="crptAttUpdate" runat="server">
        <ItemTemplate>
          <tr>
            <td height="26" align="center" bgcolor="#e3f1fc"><%# string.Format("{0:yyy-MM-dd}",Eval("AddDate")) %> </td>
            <td height="26" align="center" bgcolor="#e3f1fc"><%# Eval("WorkStatus")%></td>
            <td height="26" align="center" bgcolor="#e3f1fc">
                <%# GetReason(Eval("WorkStatus"), Eval("Reason").ToString())%></td>
          </tr>
        </ItemTemplate>
      </cc1:CustomRepeater>
      <tr>
            <td height="26" align="left" bgcolor="#bddcf4" class="pandl10" colspan="3"><span style=" color:Red;">*</span>考勤时间：
            <input name="txt_AttStartDate" id="txt_AttStartDate" type="text" class="searchinput2" size="15" valid="required" errmsg="请输入考勤时间" onfocus="WdatePicker()" value="<%= string.Format("{0:yyyy-MM-dd}", AttStartDate )%>" /> 
               <span id="errMsg_txt_AttStartDate"  style="color:Red;"></span>
            </td>
          </tr>
          <tr>
            <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10" colspan="3">
                <%--<asp:RadioButtonList ID="rbtlist" runat="server" RepeatColumns="5">
                    <asp:ListItem Value="0" Text="&nbsp;准点"></asp:ListItem>
                    <asp:ListItem Value="1" Text="&nbsp;迟到"></asp:ListItem>
                    <asp:ListItem Value="3" Text="&nbsp;旷工"></asp:ListItem>
                    <asp:ListItem Value="4" Text="&nbsp;休假"></asp:ListItem>
                    <asp:ListItem Value="7" Text="&nbsp;请假"></asp:ListItem>
                </asp:RadioButtonList>--%>
                <input type="radio" name="radio1"  id="rd_OnTime" value="0" <%=radioValue==0?"checked":"" %>
                    onclick="return UpdateRecord.HidLeave();" />
                <label for="rd_OnTime">准点</label>   
                <input type="radio" name="radio1" id="rd_ToLate" value="1" <%=radioValue==1?"checked":"" %>
                    onclick="return UpdateRecord.HidLeave();" />
                <label for="rd_ToLate">迟到</label>     
                <input type="radio" name="radio1" id="rd_Absenteeism" value="3" <%=radioValue==3?"checked":"" %>
                     onclick="return UpdateRecord.HidLeave();" />
                <label for="rd_Absenteeism">旷工</label> 
                <input type="radio" name="radio1" id="rd_DayOff" value="4" <%=radioValue==4?"checked":"" %>
                    onclick="return UpdateRecord.HidLeave();" />
                <label for="rd_DayOff">休假</label> 
                <input type="radio" name="radio1" id="rd_LeaveFor" value="7" <%=radioValue==7?"checked":"" %>
                    onclick="return UpdateRecord.HidLeave();" />
                <label for="rd_LeaveFor">请假</label>
            </td>
          </tr>
           <tr>
            <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10" colspan="3">
                <%--<asp:CheckBoxList ID="chblist" runat="server" RepeatColumns="4">
                    <asp:ListItem Value="8" Text="&nbsp;加班"></asp:ListItem>
                    <asp:ListItem Value="2" Text="&nbsp;早退"></asp:ListItem>
                    <asp:ListItem Value="5" Text="&nbsp;外出"></asp:ListItem>
                    <asp:ListItem Value="6" Text="&nbsp;出团"></asp:ListItem>
                </asp:CheckBoxList>--%>
             <input type="checkbox" name="checkbox1"  id="chb_WorkOverTime" value="8" 
                 <%=GetCheckBoxSelect("8")?"checked":"" %>  onclick="return UpdateRecord.HidWorkOverTime();" />
                <label for="chb_WorkOverTime">加班</label>
                <input type="checkbox" name="checkbox1"  id="chk_LeaveEarly" value="2" 
                 <%=GetCheckBoxSelect("2")?"checked":"" %> />
                <label for="chk_LeaveEarly">早退</label> 
                <input type="checkbox" name="checkbox1" id="chk_GoOut" value="5"
                 <%=GetCheckBoxSelect("5")?"checked":"" %> />
                <label for="chk_GoOut">外出</label>    
                <input type="checkbox" name="checkbox1" id="chk_GoGroup" value="6" 
                 <%=GetCheckBoxSelect("6")?"checked":"" %> />
                <label for="chk_GoGroup">出团</label>  
            </td>
          </tr>
          <tr>
            <td bgcolor="#e3f1fc" class="pandl10" colspan="3">
              <div id="spanLeave" style=" width:100%;display:none;">
              <table>
              <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">&nbsp;请假原因：
                <input name="txt_LeaveWhy" id="txt_LeaveWhy" type="text" class="searchinput2" size="60"
                  value="<%=LeaveWhy %>"  /></td>
              </tr>
              <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10"><span style=" color:Red;">*</span>请假时间：
                <input name="txt_LeaveForStartDate" id="txt_LeaveForStartDate" type="text" class="searchinput2"
                  value="<%= string.Format("{0:yyyy-MM-dd}", LeaveForStartDate )%>"   onfocus="WdatePicker()" size="12" /> 
                至 
                <input name="txt_LeaveForEndDate" id="txt_LeaveForEndDate" type="text" class="searchinput2"
                  value="<%= string.Format("{0:yyyy-MM-dd}", LeaveForEndDate )%>"   onfocus="WdatePicker()" size="12" />
                  <span id="span_txt_LeaveForStartDate" style=" display:none; color:Red;">请输入请假开始时间</span>&nbsp;&nbsp;
                  <span id="span_txt_LeaveForEndDate" style=" display:none; color:Red;">请输入请假结束时间</span>
                  </td>
              </tr>
              <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">&nbsp;请假天数：
                <input name="txt_LeaveDayNum" id="txt_LeaveDayNum" type="text" class="searchinput2" size="15"
                  value="<%=LeaveDayNum %>"   /></td>
              </tr>
              </table>
              </div>
            </td>
          </tr>
          <tr>
            <td bgcolor="#e3f1fc" class="pandl10" colspan="3">
              <div id="spanWorkOverTime" style=" width:100%; display:none;">
              <table>
              <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">&nbsp;加班内容：
                <input name="txt_WorkOverTimeContent" id="txt_WorkOverTimeContent" type="text"
                  value="<%=WorkOverTimeContent %>" class="searchinput2" size="60" /></td>
              </tr>
                <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10"><span style="color:Red;">*</span>加班时间：
                  <input name="txt_WorkOverTimeDateStart" id="txt_WorkOverTimeDateStart" type="text" class="searchinput2" 
                   value="<%= string.Format("{0:yyyy-MM-dd}", WorkOverTimeDateStart )%>" onfocus="WdatePicker()" size="12" />
                  至
                  <input name="txt_WorkOverTimeDateEnd" id="txt_WorkOverTimeDateEnd" type="text" class="searchinput2"
                   value="<%= string.Format("{0:yyyy-MM-dd}", WorkOverTimeDateEnd )%>" onfocus="WdatePicker()" size="12" />
                   <span id="span_txt_WorkOverTimeDateStart" style=" display:none; color:Red;">请输入加班开始时间</span>&nbsp;&nbsp;
                  <span id="span_txt_WorkOverTimeDateEnd" style=" display:none; color:Red;">请输入加班结束时间</span>
                   </td>
              </tr>
              <tr>
                <td height="26" align="left" bgcolor="#e3f1fc" class="pandl10">&nbsp;加班时数：
                <input name="txt_WorkOverTimeNum" id="txt_WorkOverTimeNum" type="text"
                 value="<%=WorkOverTimeNum %>" class="searchinput2" size="15" /></td>
              </tr>
              </table>
              </div>
            </td>
          </tr>
      <tr>
        <td height="40" colspan="3" align="center">
	    <table border="0" cellspacing="0" cellpadding="0">
             <tr>
                <td width="86" height="40" align="center" class="tjbtn02">
                    <a href="javascript:void(0);" onclick="return UpdateRecord.Save('Save');return false;">确认</a></td>
			    <td width="86" height="40" align="center" class="tjbtn02">
			    <a href="javascript:void(0);" onclick="top.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">取消</a></td>
            </tr>
        </table></td>
      </tr>
    </table>
    <input id="hidWorkerID" type="hidden" runat="server" value="" />
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    
    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script type="text/javascript">
        var UpdateRecord = {
            HidLeave: function() {//请假
                if ($("#rd_LeaveFor").attr("checked")) {
                    $("#spanLeave").css("display", "");
                    $("#chb_WorkOverTime").attr("checked", "");
                    UpdateRecord.HidWorkOverTime();
                } else {
                    $("#spanLeave").css("display", "none");
                }
            },
           HidWorkOverTime: function() {//加班
               if ($("#chb_WorkOverTime").attr("checked")) {
                   $("#spanWorkOverTime").css("display", "");
                    $("#rd_LeaveFor").attr("checked", "");
                    UpdateRecord.HidLeave();
                } else {
                   $("#spanWorkOverTime").css("display", "none");
                }
            },
            Save: function(method) {//保存
                var Result = false;
                Result = ValiDatorForm.validator($("#form1").get(0), "span");
                if ($("#chb_WorkOverTime").attr("checked")) {
                    if ($("#txt_WorkOverTimeDateStart").val() == "") {
                        $("#span_txt_WorkOverTimeDateStart").show();
                        Result = false;
                    } else {
                        $("#span_txt_WorkOverTimeDateStart").css("display", "none");
                    }

                    if ($("#txt_WorkOverTimeDateEnd").val() == "") {
                        $("#span_txt_WorkOverTimeDateEnd").show();
                        Result = false;
                    } else {
                        $("#span_txt_WorkOverTimeDateEnd").css("display", "none");
                    }
                }
                if ($("#rd_LeaveFor").attr("checked")) {
                    if ($("#txt_LeaveForStartDate").val() == "") {
                        $("#span_txt_LeaveForStartDate").show();
                        Result = false;
                    } else {
                        $("#span_txt_LeaveForStartDate").css("display", "none");
                    }

                    if ($("#txt_LeaveForEndDate").val() == "") {
                        $("#span_txt_LeaveForEndDate").show();
                        Result = false;
                    } else {
                        $("#span_txt_LeaveForEndDate").css("display", "none");
                    }
                }
                
                
                if (!Result) {
                    return false;
                }

                $("#hidMethod").attr("value", method);
                $("#<%=form1.ClientID %>").get(0).submit();
                return false;
            }
        };
        $(function() {
            UpdateRecord.HidLeave();
            UpdateRecord.HidWorkOverTime();
            FV_onBlur.initValid($("#form1").get(0));
        });
    </script>
    </form>
</body>
</html>
