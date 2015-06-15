<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPermit.aspx.cs" Inherits="Web.systemset.organize.SetPermit" %>
<%@ Register Src="~/UserControl/ucSystemSet/PermitList.ascx" TagName="perList" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>授权_组织机构_系统设置</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
       .errmsg{ color:Red;}
      </style>
      <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="40" align="center" style="font-size:14px"><strong>请选择角色:</strong> 
      <select id="selRole"  runat="server" >
     </select></td>
  </tr>
</table>
 <uc1:perList id="cu_perList" runat="server" />

            	
<table width="304" border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
            <td width="152" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return SepPermit.setPer();return false;">授权</a></td>
            <td width="152" height="40" align="center" class="tjbtn02"><span class="tjbtn02"><a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
  </tr>
          </table>
    </div>
    </form>
    <script type="text/javascript">

        var SepPermit =
      {
          //提交设置的权限
          setPer: function() {
              var perArr = [];
              $("input[name='perItem']:checked").each(function() {
                  perArr.push($(this).val());
              })//获取选中权限
              var permitIds = perArr.toString();
              $.newAjax(
                      {
                          url: "/systemset/organize/SetPermit.aspx?empId=<%=empId %>&method=setPermit",
                          data: { perIds: permitIds, roleId: $("#<%=selRole.ClientID %>").val() },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          async: false,
                          success: function(result) {
                              if (result.success == "1") {
                                  alert("授权成功！"); 
                                  window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                              }
                              else {
                                  alert("授权失败！");
                              }
                          },
                          error: function() {
                              alert("授权时发生未知错误！");
                          }
                      })
              return false;
          },

          changeRole: function(tar) {
              var roleId = $(tar).val();
              window.location = '/systemset/organize/SetPermit.aspx?iframeId=<%=Request.QueryString["iframeId"] %>&method=getPermit&empId=<%=empId %>&roleId=' + roleId;
          },
          chkA1: function(tar) {
              var chkObj = $(tar);
              var sonClass = "p" + chkObj.attr("class").subString(3);
          },
          chkA2: function(tar) {
          }
      }
        
    </script>
</body>
</html>
