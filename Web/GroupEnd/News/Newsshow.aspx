<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Newsshow.aspx.cs" Inherits="Web.GroupEnd.News.Newsshow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
<link href="/css/boxy.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
  
            <div class="tablelist">
                <table width="880" border="0" align="center" cellpadding="1" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            最新动态查看
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>标题：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <% =nModel.Title %>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#BDDCF4">
                            <strong>内容：</strong>
                        </td>
                        <td height="100" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <% =nModel.Content %>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>附件：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (!string.IsNullOrEmpty(nModel.UploadFiles))
                              { %>
                            <a href="<% =nModel.UploadFiles%>" target="_blank">点击下载</a>
                            <%}
                              else
                              { %>
                            无
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#BDDCF4">
                            <strong>发布人：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <% =nModel.OperatorName%>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>发布时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <% =nModel.IssueTime.ToString("yyyy-MM-dd HH:mm:ss") %>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="3" align="center">
                            <table border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="86" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                                            关闭</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    </form>
</body>
</html>
