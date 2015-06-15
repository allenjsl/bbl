<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerLevelEdit.aspx.cs" Inherits="Web.systemset.basicinfo.CustomerLevelEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>客户等级编辑_基础设置_系统设置</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="custForm" name="custForm"  method="post" runat="server"><input type="text" style="display:none;"/>
     <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="21%" height="30" align="right"><span style="color:Red">*</span>等级名称：</th>
        <td width="79%" bgcolor="#E3F1FC"> <input name="txtCustLevel" type="text" class="xtinput" id="txtCustLevel" size="40" value="<%=custLevel %>" valid="required"  errmsg="客户等级不为空"/>
           <span id="errMsg_txtCustLevel" class="errmsg" ></span>
        </td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
          <table width="340" border="0" cellspacing="0" cellpadding="0">
         <tr>
            <td width="106" height="40" align="center"></td>
           <%if (!IsSystem)
             { %> <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return save('');">保存</a><%} %></td>
            <td width="158" height="40" align="center" ><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table>
          </td>
      </tr>
</table>
    </form>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function() {
         FV_onBlur.initValid($("#<%=custForm.ClientID %>").get(0));
         });
       //保存报价信息
        function save(method) {
            var isSuccess = ValiDatorForm.validator($("#<%=custForm.ClientID %>").get(0), "span");
            if (!isSuccess) return false;
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            $("#<%=custForm.ClientID %>").get(0).submit();
            return false;
            
        }
    </script>
</body>
</html>
