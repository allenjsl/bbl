<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DomShow.aspx.cs" Inherits="Web.UserCenter.DomManager.DomShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <title>文档查看</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <% if (pdModel != null)
       { %>
    <div>
         <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
              <tr class="odd">
                <th width="21%" height="30" align="right">文档名称：</th>
                <td width="79%" bgcolor="#E3F1FC"> <%= pdModel.DocumentName %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传时间：</th>
                <td width="79%" bgcolor="#E3F1FC"> <%= pdModel.CreateTime.ToString("yyyy-MM-dd") %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传人：</th>
                <td width="79%" bgcolor="#E3F1FC"> <%=pdModel.OperatorName %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传文档：</th>
                <td width="79%" bgcolor="#E3F1FC">
                    <%if (string.IsNullOrEmpty(pdModel.FilePath))
                      { %>无
                    <%}
                      else
                      { %>
                    <a href="<%= pdModel.FilePath%>" target="_blank">查看文档</a>
                    <%} %>
                </td>
              </tr>
              <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                  <table width="194" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td width="103" height="40" align="center"></td>
                    <td width="106" height="40" align="center" class="tjbtn02">
                         <a href="javascript:;" class="close">关闭</a>
                    </td>
                    </tr>
                  </table>        
                </td>
              </tr>
        </table>
    </div>
    <%}
       else
       {
           %>
           没有相关文档！
           <%} %>
    </form>
    <script type="text/javascript">
        $(function(){
             
            $("a.close").click(function(){
                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                return false;
            });
        });
    </script>
    
</body>
</html>
