<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCahier.aspx.cs" Inherits="Web.administrativeCenter.cahierManage.EditCahier" %>

<%@ Import Namespace="EyouSoft.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="650" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="14%" height="30" align="right">会议编号：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <input name="txt_MeetingNum" type="text" class="xtinput" id="txt_MeetingNum" size="20" value="<%=MeetingNum %>" />
        </td>
      </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right"><span style=" color:Red;">*</span>会议主题：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <input name="txt_MeetingSubject" type="text" class="xtinput" id="txt_MeetingSubject" size="60" valid="required" errmsg="请输入会议主题" value="<%=MeetingSubject %>" />
             <span id="errMsg_txt_MeetingSubject"  style="color:Red;"></span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">参会人员：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <input name="txt_JoinPersonnel" type="text" class="xtinput" id="txt_JoinPersonnel" size="90" value="<%=JoinPersonnel %>" />
        </td>
      </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">会议时间：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <input name="txt_MeetingTimeBegin" type="text" class="xtinput" id="txt_MeetingTimeBegin" size="20"
                 value='<%= string.Format("{0:yyyy-MM-dd}", MeetingTimeBegin)=="1900-01-01"?"":string.Format("{0:yyyy-MM-dd}", MeetingTimeBegin) %>' onfocus="WdatePicker()"  /> 
          至 
          <input name="txt_MeetingTimeEnd" type="text" class="xtinput" id="txt_MeetingTimeEnd" size="20" value='<%= string.Format("{0:yyyy-MM-dd}", MeetingTimeEnd)=="1900-01-01"?"":string.Format("{0:yyyy-MM-dd}", MeetingTimeEnd) %>' onfocus="WdatePicker()" /></td>
	  </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">会议地点：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <input name="txt_MeetingLocale" type="text" class="xtinput" id="txt_MeetingLocale" size="90" value="<%=MeetingLocale %>" /></td>
      </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">备注：</th>
        <td width="86%" bgcolor="#E3F1FC">
            <textarea name="txt_Remark" id="txt_Remark" cols="60" rows="5"><%=Remark %></textarea></td>
	  </tr>
      <tr class="odd">
        <td height="30" colspan="8" align="center" bgcolor="#E3F1FC">
         <table border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td width="76" height="40" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="return EditCahier.Save('save');">保存</a></td>
            <td width="158" height="40" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
          </tr>
        </table>        
        </td>
      </tr>
</table>
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    <input id="hidMeetingID" name="hidMeetingID" type="hidden" value="" runat="server" />
<script type="text/javascript" src="/js/jquery.js"></script>
<script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
<script type="text/javascript">
    var EditCahier = {
        Save: function(method) {//保存
            var Result = false;
            Result = ValiDatorForm.validator($("#form1").get(0), "span");
            if (!Result) {
                return false;
            }

            $("#hidMethod").attr("value", method);
            $("#<%=form1.ClientID %>").get(0).submit();
            return false;
        }
    };
    $(function() {
        FV_onBlur.initValid($("#form1").get(0));
    });
</script>
    </form>
</body>
</html>
