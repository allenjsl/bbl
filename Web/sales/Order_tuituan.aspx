<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order_tuituan.aspx.cs"
    Inherits="Web.sales.Order_tuituan" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单退团</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4" align="center"
            width="700" style="margin: 10px;">
            <tbody>
                <tr>
                    <td style="width: 5%" bgcolor="#e3f1fc" align="center">
                        序号
                    </td>
                    <td height="25" bgcolor="#e3f1fc" align="center">
                        姓名
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        类型
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        证件名称
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        证件号码
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        性别
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        联系电话
                    </td>
                    <td bgcolor="#e3f1fc" align="center">
                        操作功能
                    </td>
                </tr>
                <%=cusHtml %>
                <tr>
                    <td bgcolor="#e3f1fc" align="center" colspan="8">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
    
	  $("a[sign='tuituan']").click(function(){
	    var url = $(this).attr("href")+"&tourid=<%=Request.QueryString["tourid"] %>";
		parent.Boxy.iframeDialog({
		iframeUrl:url,
		title:"退团",
		modal:true,
		width:"440px",
		height:"160px"
		});
		return false;
	  });
	  
    </script>

    </form>
</body>
</html>
