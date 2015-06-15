<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveEdit.aspx.cs" Inherits="Web.CRM.customerservice.ActiveEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
  
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
      .searchinput{ width:100px;}
    </style>
</head>
<body>
    <form id="activeFrom" runat="server">
    <div>
    <input type="hidden" name="hidMethod" value="save" />
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
  <tr class="odd">
    <th width="130" height="30" align="center">活动时间：</th>
    <td><input type="text" id="txtActiveDate" runat="server" class="searchinput" onfocus="WdatePicker()"/></td>
  </tr>
  <tr class="even">
    <th height="30" align="center"><span style="color:Red;">*</span>活动主题：</th>
    <td>
    <input  type="text" id="txtActive" runat="server" class="searchinput" valid="required"  errmsg="活动主题不为空"   />
    <span id="errMsg_<%=txtActive.ClientID %>" class="errmsg" ></span>
    </td>
  </tr>
  <tr class="odd">
    <th height="40" align="center">活动内容：</th>
    <td><textarea class="textareastyle02" rows="5" cols="45" id="txtActiveContant" runat="server"></textarea></td>
  </tr>
  <tr class="even">
    <th height="30" align="center">参加单位(人)：</th>
    <td><input  type="text" id="txtMeeter" runat="server" class="searchinput"/></td>
  </tr>
  <tr class="odd">
    <th height="40" align="center">活动效果：</th>
    <td><textarea class="textareastyle02" rows="5" cols="45" id="txtActiveResult" runat="server"></textarea></td>
  </tr>
  <tr class="even">
    <th height="30" align="center">主办人：</th>
    <td><input type="text" id="txtActiveUser" runat="server" class="searchinput"/></td>
  </tr>
  <tr class="odd">
    <th height="30" align="center">活动状态：</th>
    <td><select id="selActiveState" runat="server">
      <option value="0">请选择</option>
      <option value="1">准备阶段</option>
      <option value="2">进行中</option>
      <option value="3">活动结束</option>
    </select>
    </td>
  </tr>
</table>
<table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="40" align="center"></td>
    <td height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return ActiveEdit.save();">保存</a></td>
    <td height="40" align="center" class="tjbtn02"><a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">关闭</a></td>
  </tr>
</table>
    </div>
    </form>
      <script src="/js/jquery.js" type="text/javascript"></script>
     <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
        <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
       $(document).ready(function() {
        FV_onBlur.initValid($("#<%=activeFrom.ClientID%>").get(0));
    });
        var ActiveEdit =
       {  
           save: function() {
               var formObj = $("#<%=activeFrom.ClientID%>").get(0);
               var vResult = ValiDatorForm.validator(formObj, "span");
               if (vResult) {
                   formObj.submit();
               }
               return false;
           }
          
          
       }
       
    </script>
</body>
</html>
