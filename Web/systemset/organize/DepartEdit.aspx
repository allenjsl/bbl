<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartEdit.aspx.cs"  EnableViewState="false" Inherits="Web.systemset.organize.DepartEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>部门设置_组织机构_系统设置</title>
      <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
       .errmsg{ color:Red;}
      </style>
      <script src="/js/jquery.js" type="text/javascript"></script>
      <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
     <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="13%" height="30" align="right">部门名称：</th>
        <td width="36%" bgcolor="#E3F1FC"> 
        <input  type="text" class="xtinput" id="txtDepName" size="20" runat="server" valid="required"  maxlength="15"  errmsg="部门不为空" />
        <span id="errMsg_<%=txtDepName.ClientID %>" class="errmsg" ></span>
        </td>
        <th width="15%" height="30" align="right">部门主管：</th>
        <td width="36%" bgcolor="#E3F1FC">
        <select id="selDepEmp" runat="server">
         </select><span id="errMsg_<%=selDepEmp.ClientID %>" class="errmsg" ></span>
         </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">上级部门：</th>
        <td colspan="3" bgcolor="#E3F1FC"> 
        <select id="selParentDE" runat="server"  valid="required"  errmsg="上级部门不为空" >
         </select><input type="hidden" id="hidParentDE" name="hidParentDE"  value="<%=parentD %>"/>
         <span id="errMsg_<%=selParentDE.ClientID %>" class="errmsg" ></span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">联系电话：</th>
        <td width="36%" bgcolor="#E3F1FC"> 
        <input type="text" class="xtinput" runat="server" id="txtTel" size="20" valid="isPhone"  errmsg="格式不正确"/>
         <span id="errMsg_<%=txtTel.ClientID %>" class="errmsg" ></span>
        </td>
        <th width="15%" height="30" align="right">传真：</th>
        <td width="36%" bgcolor="#E3F1FC"><input  type="text"  runat="server"  class="xtinput" id="txtFax" size="25"/></td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">打印页眉：</th>
        <td colspan="3" bgcolor="#E3F1FC"><input type="file" name="fileHeader" id="fileHeader" />
        <input type="hidden" id="hidHeader"  runat="server"/>&nbsp;<%=pageHeader %>
         <span id="hMess" style="color:Red; display:none;">图片格式不正确</span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">打印页脚：</th>
        <td colspan="3" bgcolor="#E3F1FC"><input type="file" name="fileFooter" id="fileFooter"/>
        <input type="hidden" id="hidFooter"  runat="server"/>&nbsp;<%=pageFooter %>
         <span id="fMess" style="color:Red; display:none;">图片格式不正确</span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">打印模板：</th>
        <td colspan="3" bgcolor="#E3F1FC"><input type="file" name="fileModel" id="fileModel"/>
        <input type="hidden" id="hidModel"  runat="server"/>&nbsp;<%=pageModel %>
         <span id="mMess" style="color:Red; display:none;">文档格式必须Word</span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">部门公章：</th>
        <td colspan="3" bgcolor="#E3F1FC"><input type="file" name="fileSeal" id="fileSeal"/>
        <input type="hidden" id="hidSeat"  runat="server"/>&nbsp;<%=departSeal %>
          <font color="#FF0000">请上传透明背景图片</font>
           <span id="sMess" style="color:Red; display:none;">图片格式不正确</span>
          </td>
      </tr>
	  <tr class="odd">
        <th width="13%" height="30" align="right">备注：</th>
        <td colspan="3" bgcolor="#E3F1FC">
        <textarea name="txtRemark" id="txtRemark" cols="55" rows="5" runat="server"></textarea></td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="4" align="center" bgcolor="#E3F1FC">
          <table border="0" align="center" cellpadding="0" cellspacing="0">
         <tr>
            
             <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" id="btn_save" onclick="return De.save('');">保存</a></td>
            <td width="158" height="40" align="center"><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table>        </td>
      </tr>
</table>
    </form>
<script type="text/javascript">
    $(document).ready(function() {
        FV_onBlur.initValid($("#btn_save").closest("form").get(0));
    });
    var De =
    {
        //保存表单
        save: function(method) {
            var form = $("#btn_save").closest("form").get(0);
            var vResult = ValiDatorForm.validator(form, "span");
            var hV = $("#fileHeader").val();
            var fV = $("#fileFooter").val();
            var sV = $("#fileSeal").val();
            var mV = $("#fileModel").val(); //验证上传文件格式
            if (hV != "") { if (!/.jpg|.jpeg|.bmp|.gif|.png$/i.test(hV)) { $("#hMess").css("display", ""); vResult = false; } else { $("#hMess").css("display", "none"); } }
            if (fV != "") { if (!/.jpg|.jpeg|.bmp|.gif|.png$/i.test(fV)) { $("#fMess").css("display", ""); vResult = false; } else { $("#fMess").css("display", "none"); } }
            if (sV != "") { if (!/.jpg|.jpeg|.bmp|.gif|.png$/i.test(sV)) { $("#sMess").css("display", ""); vResult = false; } else { $("#sMess").css("display", "none"); } }
            if (mV != "") { if (!/.dot|.doc|.docx|.gif|.png$/i.test(mV)) { $("#mMess").css("display", ""); vResult = false; } else { $("#mMess").css("display", "none"); } }
            if (!vResult) return false;
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            form.submit();
            return false;
        }
        , del: function(hidId, tar) {
            if (confirm("你确定要删除该文件吗？")) {
                $("#" + hidId).val("");
                $(tar).replaceWith("");
            }
            return false;
        }
    }
</script>
</body>

</html>
