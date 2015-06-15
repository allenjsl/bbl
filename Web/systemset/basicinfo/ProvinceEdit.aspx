<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvinceEdit.aspx.cs" Inherits="Web.systemset.basicinfo.ProvinceEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>省份编辑_基础设置_系统设置</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
  
     </style>
</head>
<body>
    <form id="peForm"  runat="server"><input type="text" style="display:none;"/>
      <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
     <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="18%" height="30" align="right"><span style="color:Red">*</span>省份名称：</th>
        <td bgcolor="#E3F1FC"> <input name="txtProvinceName" id="txtProvinceName" type="text" maxlength="20" class="xtinput" size="40" value="<%=provinceName %>"  valid="required"  errmsg="省份不为空"/>
        <span id="errMsg_txtProvinceName" class="errmsg" ></span>
        </td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="2" align="left" bgcolor="#E3F1FC">
          <table width="324" border="0" cellspacing="0" cellpadding="0">
         <tr>
            <td width="90" height="40" align="center"></td>
            <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return save('');">保存</a></td>
            <td width="158" height="40" align="center"><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table>
       </td>
      </tr>
    </table>
    </form>
    <script src="/js/jquery.js" type="text/javascript"></script>
     <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
             FV_onBlur.initValid($("#<%=peForm.ClientID %>").get(0));
        });
       //保存省份信息
        function save(method) {
            var isSuccess = ValiDatorForm.validator($("#<%=peForm.ClientID %>").get(0), "span");
            if (!isSuccess) return false;
            $.newAjax(
            { url: "/systemset/basicinfo/ProvinceEdit.aspx",
                data: { pId: "<%=pId %>", pName: $("#txtProvinceName").val(), isExist: "isExist" },
                dataType: "json",
                cache: false,
                async: false,
                type: "post",
                success: function(result) {
                    if (result.success == "1") {
                        alert("该省份已经存在！");
                        isSuccess = false;
                    }

                },
                error: function() {
                    alert("数据保存失败！");
                }
            })
            if (!isSuccess) { return false; }
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            $("#<%=peForm.ClientID %>").get(0).submit();
            return false;
            
        }
    </script>
</body>
</html>
