<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditFixedAssets.aspx.cs" Inherits="Web.administrativeCenter.fixedAssetsManage.EditFixedAssets" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="EyouSoft.Common" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" 
        style="margin:7px auto 20px auto;">
      <tr class="odd">
        <th width="14%" height="30" align="right">编号：</th>
        <td width="28%" bgcolor="#E3F1FC">
            <input name="txt_Number" type="text" class="xtinput" id="txt_Number" size="20" value="<%=Number %>" /></td>
        <th width="14%" height="30" align="right"><span style=" color:Red;">*</span>资产名称：</th>
        <td width="44%" bgcolor="#E3F1FC">
            <input name="txt_AssetName" type="text" class="xtinput" id="txt_AssetName" size="28"  valid="required" errmsg="请输入资产名称" value="<%=AssetName %>" /><span id="errMsg_txt_AssetName"  style="color:Red;"></span></td>
      </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">折 旧 费：</th>
        <td width="28%" bgcolor="#E3F1FC">
            <input name="txt_Cost" type="text" class="xtinput" id="Text1" size="20" value="<%=Cost==0.0M?"":Cost.ToString() %>" /></td>
        <th width="14%" height="30" align="right">购买时间：</th>
        <td width="44%" bgcolor="#E3F1FC">
            <input name="txt_BuyTime" type="text" class="xtinput" id="txt_BuyTime" size="20" value="<%= string.Format("{0:yyyy-MM-dd}", BuyTime) == "1990-01-01" ? "" : string.Format("{0:yyyy-MM-dd}", BuyTime)%>" onfocus="WdatePicker()" /></td>
	  </tr>
	  <tr class="odd">
        <th width="14%" height="30" align="right">备注：</th>
        <td colspan="3" bgcolor="#E3F1FC">
            <textarea name="txt_Reamrk" id="txt_Remark" cols="60" rows="5"><%=Reamrk %></textarea></td>
	  </tr>
      <tr class="odd">
        <td height="30" colspan="10" align="center" bgcolor="#E3F1FC">
          <table border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
            <td width="76" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="return EditFixedAssets.Save('save');">保存</a></td>
            <td width="158" align="center" class="tjbtn02">
                <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
          </tr>
        </table>        </td>
      </tr>
</table>
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    <input id="hidFixedID" name="hidFixedID" type="hidden" value="" runat="server" />
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var EditFixedAssets = {
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
