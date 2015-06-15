<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeopleEdit.aspx.cs" Inherits="Web.CRM.customerservice.PeopleEdit" %>
<%@ Register  Src="~/UserControl/ucCRM/CustServicePeople.ascx" TagName="ucPeople" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style>
       .searchinput{ width:150px;}
    </style>
</head>
<body>
    <form id="peopleEditForm" runat="server">
    <div>
     <input type="hidden" name="hidMethod"  value="save"/>
     <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
      <uc1:ucPeople id="ucPeople1" runat="server"  />
      <tr class="odd">
    <th height="30" align="center">备注：</th>
    <td>
    <textarea class="textareastyle02" rows="5" cols="45"  id="txtRemarkP" runat="server"></textarea>
  
    </td>
  </tr>
     </table>
     <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="40" align="center"></td>
    <td height="40" align="center" class="tjbtn02"><%if (HasPermit)
                                                     { %><a href="javascript:;" onclick="return ComplaitEdit.save();">保存</a><%} %></td>
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
         FV_onBlur.initValid($("#<%=peopleEditForm.ClientID%>").get(0));
    });
    var ComplaitEdit =
       {
           save: function() {
               var formObj = $("#<%=peopleEditForm.ClientID%>").get(0);
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
