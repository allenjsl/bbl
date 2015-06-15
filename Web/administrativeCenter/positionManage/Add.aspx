<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Web.administrativeCenter.positionManage.Add" %>

<%@ Import Namespace="EyouSoft.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>职务管理编辑</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
	  <tr class="odd">
        <th width="21%" height="30" align="right"><span style=" color:Red;">*</span>职务名称：</th>
        <td width="79%" bgcolor="#E3F1FC">
            <input name="txt_JobName" type="text" class="xtinput" id="txt_JobName" size="40" valid="required" errmsg="请输入职务名称" value="<%=JobName %>" />
            <span id="errMsg_txt_JobName"  style="color:Red;"></span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="21%" height="30" align="right">职务说明：</th>
        <td width="79%" bgcolor="#E3F1FC">
            <textarea name="txt_JobDes" id="txt_JobDes"  cols="50" rows="4"><%=JobDes %></textarea>
        </td>
      </tr>
	  <tr class="odd">
        <th width="21%" height="30" align="right">职务具体要求：</th>
        <td width="79%" bgcolor="#E3F1FC">
            <textarea name="txt_JobRequire" id="txt_JobRequire"  cols="50" rows="4"><%=JobRequire %></textarea>
        </td>
      </tr>
	  <tr class="odd">
        <th width="21%" height="30" align="right">备    注：</th>
        <td width="79%" bgcolor="#E3F1FC">
            <textarea name="txt_Remarks" id="txt_Remarks"  cols="50" rows="4"><%=Remarks %></textarea>
        </td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
            <div id="submitAdd" style="display:none;">
                <table width="340" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="106" height="40" align="center" ></td> 
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return PostionAdd.Save('save');">保存</a></td>
                        <td width="158" height="40" align="center" class="tjbtn02">
                        <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
                    </tr>
                 </table>
            </div>
            <div id="submitUpdate"  style="display:none;" >
                 <table width="340" border="0" cellspacing="0" cellpadding="0">
                     <tr>
                        <td width="106" height="40" align="center"></td>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return PostionAdd.Save('update');">修改</a></td>
                        <td width="158" height="40" align="center" class="tjbtn02">
                        <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
                      </tr>
                </table>
            </div>     
        </td>
      </tr>
    </table>
    <input id="hiddenID" name="hiddenID" runat="server" type="hidden" />
    <input id="hiddenMethod"  name="hiddenMethod" type="hidden" />
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
        
    <script language="javascript">
        var PostionAdd = {
            Save: function(Method) {
                var isValidator = false;
                if (Method == "saveandadd") {//保存并继续添加
                    document.getElementById("hiddenMethod").value = "saveandadd";
                }
                else if (Method == "update") {//修改
                    document.getElementById("hiddenMethod").value = "update";
                }
                else if (Method == "save") {
                    document.getElementById("hiddenMethod").value = "save";
                }
                isValidator = ValiDatorForm.validator($("#form1").get(0), "span");
                if (!isValidator) {
                    return false;
                }

                document.forms["form1"].submit();
                return false;
            }
        };
        $(function() {
            var ID = $("#<%=hiddenID.ClientID %>").val();
            if (ID == "") {//初始化
                document.getElementById("submitUpdate").style.display = "none";
                document.getElementById("submitAdd").style.display = "";
            } else {
                document.getElementById("submitAdd").style.display = "none";
                document.getElementById("submitUpdate").style.display = "";
            }
            FV_onBlur.initValid($("#form1").get(0));
        });
    </script>
    </form>
</body>
</html>
