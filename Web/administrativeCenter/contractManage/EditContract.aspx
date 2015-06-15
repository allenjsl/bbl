<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditContract.aspx.cs" Inherits="Web.administrativeCenter.contractManage.EditContract" %>

<%@ Import Namespace="EyouSoft.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
    <tr class="odd">
        <th width="13%" height="30" align="right">员工编号：</th>
        <td width="37%" bgcolor="#E3F1FC">
            <input name="txt_WorkerNO" type="text" class="xtinput" id="txt_WorkerNO" size="20" value="<%=WorkerNO %>" /></td>
        <th width="13%" height="30" align="right"><span style=" color:Red;">*</span>姓名：</th>
        <td width="37%" bgcolor="#E3F1FC">
            <input name="txt_WorkerName" type="text" class="xtinput" id="txt_WorkerName" size="20" valid="required" errmsg="请输入姓名" value="<%=WorkerName %>" /><span id="errMsg_txt_WorkerName"  style="color:Red;"></span></td>
    </tr>
    <tr class="odd">
        <th width="13%" height="30" align="right"><span style=" color:Red;">*</span>签订时间：</th>
        <td width="37%" bgcolor="#E3F1FC">
            <input name="txt_BeginDate" type="text" class="xtinput" id="txt_BeginDate" size="20" valid="required" errmsg="请输入签订时间" onfocus="WdatePicker()" value='<%= string.Format("{0:yyyy-MM-dd}",BeginDate)=="0001-01-01"?"":string.Format("{0:yyyy-MM-dd}",BeginDate) %>' /><span id="errMsg_txt_ToContract"  style="color:Red;"></span></td>
        <th width="13%" height="30" align="right"><span style=" color:Red;">*</span>到期时间：</th>
        <td width="37%" bgcolor="#E3F1FC">
            <input name="txt_EndDate" type="text" class="xtinput" id="txt_EndDate" size="20" valid="required" errmsg="请输入到期时间" onfocus="WdatePicker()"  value='<%= string.Format("{0:yyyy-MM-dd}",EndDate)=="0001-01-01"?"":string.Format("{0:yyyy-MM-dd}",EndDate) %>' /><span id="errMsg_txt_ComeDue"  style="color:Red;"></span></td>
    </tr>
    <tr class="odd">
        <th width="13%" height="30" align="right">状态：</th>
        <td bgcolor="#E3F1FC" colspan="3"><select id="dpState" name="dpState" runat="server">
            <option value="0" selected="selected">未到期</option>
            <option value="1">到期未处理</option>
            <option value="2">到期已处理</option>
            </select></td>
    </tr>
    <tr class="odd">
        <th width="13%" height="30" align="right">备注：</th>
        <td colspan="3" bgcolor="#E3F1FC">
            <textarea name="txt_Reamrk" id="txt_Reamrk" cols="60" rows="5" ><%=Reamrk %></textarea></td>
    </tr>
    <tr class="odd">
    <td height="30" colspan="8" align="center" bgcolor="#E3F1FC">
        <table border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="76" height="40" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="return EditContract.Save('save');">保存</a></td>
            <td width="158" height="40" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
        </tr>
        </table>        
    </td>
    </tr>
    </table>
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    <input id="hidWorkerID" name="hidWorkerID" type="hidden" value="" runat="server" />
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        var EditContract = {
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
