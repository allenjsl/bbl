<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRouteArea.aspx.cs" Inherits="Web.systemset.basicinfo.EditRouteArea" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="SelOperator" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>线路区域编辑_基础设置_系统设置</title>
    <style type="text/css">
    .pandl3 input
    {
    height:18px; border:1px #93b7ce solid; font-size:12px;
    }
    </style>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
   
    
</head>
<body>
    <form id="eraForm" runat="server" name="eraForm">
    
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="520" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <td  height="30" align="right"><span style="color:Red">*</span>区域名称：</td>
        <td colspan="3" bgcolor="#E3F1FC" class="pandl3"><input name="txtAreaName" type="text" class="xtinput" id="txtAreaName" value="<%=areaName %>"  maxlength="50" size="40" valid="required"  errmsg="区域不为空"/>
       <span id="errMsg_txtAreaName" class="errmsg" ></span>
        </td>
      </tr>
	  <tr align="center" class="odd">
	   <td  height="30" align="right"><span style="color:Red">*</span>责任计调：</td>
	    <td  bgcolor="#E3F1FC" class="pandl3" colspan="3"  align="left">
	    <span style="float:left">
         <uc1:SelOperator  runat="server" ID="selOperator" IsShowLabel="false"></uc1:SelOperator></span>
	     <span class="talbe_btn" style="float:left; margin-left:5px;"> <a href="javascript:;" onclick="return EditRArea.clearOperator();" style="text-align:center; text-decoration:none;">清空计调</a></span>
          <span id="operMess" style="color:Red; display:none; margin-left:5px;">计调人不为空</span>
        </td>
  </tr>
        <tr class="odd">
            <td style="text-align:right">排序编号：</td>
            <td><input name="txtSortId" id="txtSortId" value="<%=AreaSortId %>" /><span style="color:#666; padding-left:8px;">序号越小,排序越前。当前最小序号<%=MinSortId %>，最大序号<%=MaxSortId %>。</span></td>
        </tr>
      <tr class="odd">
        <td height="30" colspan="4" align="left" bgcolor="#E3F1FC">
          <table width="324" border="0" cellspacing="0" cellpadding="0">
         <tr>
            <td width="83" height="40" align="center"></td>
            <td width="83" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return EditRArea.save('');">保存</a></td>
            <td width="158" height="40" align="center" ><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table>        </td>
      </tr>
</table>
    </form>
     <script src="/js/jquery.js" type="text/javascript"></script>
  
     <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
        FV_onBlur.initValid($("#<%=eraForm.ClientID %>").get(0));
        });
        var EditRArea =
        {
            //清空计调
            clearOperator: function() {
            
                $("#<%=txtOperatorName %>").val("");
                 $("#<%=txtHidOperatorId %>").val("");
                 return false;
            },
            //保存表单
            save: function(method) {
             
                var isSuccess = ValiDatorForm.validator($("#<%=eraForm.ClientID %>").get(0), "span");
                var operIds =<%=selOperator.ClientID %>.GetOperatorId();
            
                
                if (operIds =="") {
                   document.getElementById("operMess").style.display = "";
                     isSuccess = false;
                }
                if(!isSuccess){return false;}
                if (method == "continue") {
                    document.getElementById("hidMethod").value = "continue";
                }
                //提交表单
                $("#hidMethod").closest("form").submit();
                return false;
            }
        }
    </script>
</body>
</html>
