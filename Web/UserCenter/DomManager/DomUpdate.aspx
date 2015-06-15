<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DomUpdate.aspx.cs" Inherits="Web.UserCenter.DomManager.DomUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <title>文档修改</title>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <% if (pdModel != null)
       { %>
    <div>
         <input type="hidden" value="<%=tid %>" name="domId" />
         <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
              <tr class="odd">
                <th width="21%" height="30" align="right">文档名称：</th>
                <td width="79%" bgcolor="#E3F1FC"> 
                    <input name="domname" id="domname" type="text" class="xtinput"  value="<%= pdModel.DocumentName %>" size="42" valid="required" errmsg="请填写用户名"/>
                    <span id="errMsg_domname" class="errmsg"></span>
                </td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传时间：</th>
                <td width="79%" bgcolor="#E3F1FC"> <%= pdModel.CreateTime.ToString("yyyy-MM-dd HH:mm:ss") %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传人：</th>
                <td width="79%" bgcolor="#E3F1FC"> <%=pdModel.OperatorName %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传文档：</th>
                <td width="79%" bgcolor="#E3F1FC" class="updom">
                <%if (string.IsNullOrEmpty(pdModel.FilePath))
                  { %>
                  <input name="upfile" type="file" />
                  <%}
                  else
                  { %>
                    <a href="<%= pdModel.FilePath%>" target="_blank">查看文档</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" />
                    <%} %>
                </td>
              </tr>
              <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                  <table width="194" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                    <td width="103" height="40" align="center"></td>
                    <td width="106" height="40" align="center" class="tjbtn02">
                         <a href="javascript:;" class="save">修改</a>
                    </td>
                    <td width="76" height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide(); ">取消</a>
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
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
    <script type="text/javascript">
        $(function(){
             
            $("a.save").click(function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
            });
            
            $("img.close").click(function(){
                var that = $(this);
                $("td.updom").html("<input name=\"upfile\" type=\"file\" />");
            });
        });
    </script>
    
</body>
</html>
