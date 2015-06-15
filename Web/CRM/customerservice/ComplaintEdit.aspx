<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplaintEdit.aspx.cs" Inherits="Web.CRM.customerservice.ComplaintEdit" %>
<%@ Register  Src="~/UserControl/ucCRM/CustServiceContent.ascx" TagName="ucContent" TagPrefix="uc1" %>
<%@ Register  Src="~/UserControl/ucCRM/CustServicePeople.ascx" TagName="ucPeople" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>新增投诉_质量管理_客户关系管理</title>
      <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      
</head>
<body>
    <form id="CompForm" runat="server">
    <div>
    <input type="hidden" name="hidMethod"  value="save"/>
    <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
   <uc2:ucPeople id="ucPeople1" runat="server"  />
   <tr class="odd">
    <th height="40" align="center">投诉结果：</th>
    <td>
    <uc1:ucContent id="ucContent1" runat="server"  />
    </td>
  </tr>
  <tr class="even">
    <th height="30" align="center">备注：</th>
    <td>
    <textarea class="textareastyle02" rows="5" cols="45"  id="txtRemarkP" runat="server"></textarea>
  
    </td>
  </tr>
</table>
<table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="40" align="center"></td>
    <td height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return ComplaitEdit.save();">保存</a></td>
    <td height="40" align="center" class="tjbtn02"><a href="javascript:;"  onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">关闭</a></td>
  </tr>
</table>

    </div>
    </form>
      <script src="/js/jquery.js" type="text/javascript"></script>
  
     <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
        <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
         $(document).ready(function() {
          FV_onBlur.initValid($("#<%=CompForm.ClientID%>").get(0));
    });
    var ComplaitEdit =
       {
           save: function() {
               var formObj = $("#<%=CompForm.ClientID%>").get(0);
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
