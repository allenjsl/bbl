<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpecialService.aspx.cs"
    Inherits="Web.Common.SpecialService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type"  />
    <title>�ط�</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.js"></script>
    <script type="text/javascript">
    var query = function(key,str){
        var uri = str;
        var val = key;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    };
    function isnull(v, defaultValue) {
        if (v == null || !v)
            return defaultValue;
        else
            return v;
    }
    $(function(){
        
        var desiframeid = "<%=Request.QueryString["desiframeId"] %>";
        var tefuid = "<%=Request.QueryString["tefuid"] %>";
        
        var win =desiframeid? parent.Boxy.getIframeDocument(desiframeid):window.parent.document;
        var tefuStr = win.getElementById(tefuid).value;
       
        var txtItem = query("txtItem",tefuStr);
        var txtServiceContent = query("txtServiceContent",tefuStr);
        var ddlOperate = query("ddlOperate",tefuStr);
        var txtCost = query("txtCost",tefuStr);
        
        if(txtItem!=null){
          txtItem=decodeURI(txtItem);      
        }else{
            txtItem="";
        }
         
        if(txtServiceContent!=null){
          txtServiceContent=decodeURIComponent(txtServiceContent);
        }else{
            txtServiceContent = "";
        }
        if(txtCost!=null){
          txtCost=decodeURIComponent(txtCost);
        }else{
            txtCost="";
        }
        
        $("#txtItem").val(txtItem);
        $("#txtServiceContent").val(txtServiceContent);
        $("#txtCost").val(txtCost);
        if(ddlOperate=="0"||ddlOperate=="1"){
            $("#ddlOperate").val(ddlOperate);
        }
         
        $("#linkSave").click(function(){
            var desiframeid = "<%=Request.QueryString["desiframeId"] %>";
            var tefuid = "<%=Request.QueryString["tefuid"] %>";
            
            var win =desiframeid? parent.Boxy.getIframeDocument(desiframeid):window.parent.document;
            win.getElementById(tefuid).value = $("#table1").find("*").serialize();
            
            
            parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();
            return false;
        });
    });
    </script>
</head>
<body>
<form id="form1" runat="server">
    <table id="table1" width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top: 10px;">
        <tr class="odd">
            <th width="130" height="30" align="right">
                ��Ŀ��
            </th>
            <td>
                <input type="text" id="txtItem" name="txtItem" class="searchinput searchinput02" />
            </td>
        </tr>
        <tr class="even">
            <th width="130" height="30" align="right">
                �������ݣ�
            </th>
            <td>
                <textarea class="textareastyle02" rows="5" cols="45" id="txtServiceContent" name="txtServiceContent"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th width="130" height="30" align="right">
                ��/����
            </th>
            <td>
                <select id="ddlOperate" name="ddlOperate">
	                <option value="0">��</option>
	                <option value="1">��</option>
                </select>
            </td>
        </tr>
        <tr class="even">
            <th width="130" height="30" align="right">
                ���ã�
            </th>
            <td>
                <input type="text" id="txtCost" name="txtCost" class="searchinput searchinput03" />
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="center">
                <table width="200" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="40" align="center">
                        </td>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="linkSave">
                                ����</a>
                        </td>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();return false;">
                                ȡ��</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
