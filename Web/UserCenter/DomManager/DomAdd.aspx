<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DomAdd.aspx.cs" Inherits="Web.UserCenter.DomManager.DomAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
              <tr class="odd">
                <th width="21%" height="30" align="right">文档名称：</th>
                <td width="79%" bgcolor="#E3F1FC"> 
                    <input name="domname" id="domname" type="text" class="xtinput" size="42" valid="required" errmsg="请填写文档名"/>
                    <span id="errMsg_domname" class="errmsg"></span>
                </td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传时间：</th>
                <td width="79%" bgcolor="#E3F1FC"><% =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传人：</th>
                <td width="79%" bgcolor="#E3F1FC"> <% =this.SiteUserInfo.ContactInfo.ContactName %></td>
              </tr>
	          <tr class="odd">
                <th width="21%" height="30" align="right">上传文档：</th>
                <td width="79%" bgcolor="#E3F1FC"><input name="upfile" type="file" /></td>
              </tr>
              <tr class="odd">
                <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                  <table width="340" border="0" cellspacing="0" cellpadding="0">
                 <tr>
                    <td width="106" height="40" align="center"></td>
                    <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" class="save">保存</a></td>
                    <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide(); ">取消</a></td>
                    <%--<td width="158" height="40" align="center" class="jixusave"><a href="javascript:;" class="saveadd">保存并继续添加</a></td>--%>
                    <input type="hidden" name="continue" id="continue" value="" />
                  </tr>
                  </table>        
                </td>
              </tr>
        </table>
    
    </div>
    </form>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
    <script type="text/javascript">
       $(function(){
            FV_onBlur.initValid($("a.save").closest("form").get(0));
            $("a.save").click(function(){
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
            });
            $("a.saveadd").click(function(){
                $("#continue").val("continue");
                var form = $(this).closest("form").get(0);
                if(ValiDatorForm.validator(form,"span")){
                    form.submit();
                }
                return false;
            });
            
       });
    </script>
</body>
</html>
