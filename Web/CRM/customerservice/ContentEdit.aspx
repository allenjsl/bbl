<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentEdit.aspx.cs" Inherits="Web.CRM.customerservice.ContentEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register  Src="~/UserControl/ucCRM/CustServiceContent.ascx" TagName="ucContent" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
      <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="contentEditFrom" runat="server">
    <div>
     <input type="hidden" name="hidMethod"  value="save"/>
  <uc1:ucContent id="ucContent1" runat="server"  />
<table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="40" align="center"></td>
   <%if (hasPermit)
     { %> <td height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return ContentEdit.save();">保存</a></td><%} %>
    <td height="40" align="center" class="tjbtn02"><a href="javascript:;"  onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();return false;">关闭</a></td>
  </tr>
</table>
    </div>
    </form>
     <script src="/js/jquery.js" type="text/javascript"></script>
     <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
   
        <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
         <script type="text/javascript">
           $(document).ready(function() {
          FV_onBlur.initValid($("#<%=contentEditFrom.ClientID%>").get(0));
    });
    var ContentEdit =
       {
           save: function() {
               var formObj = $("#<%=contentEditFrom.ClientID%>").get(0);
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
