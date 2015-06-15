<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditBylaw.aspx.cs" Inherits="Web.administrativeCenter.bylaw.EditBylaw"
    ValidateRequest="false" %>

<%@ Import Namespace="EyouSoft.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js" cache="true"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <table width="650" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
        <tr class="odd">
            <th width="14%" height="30" align="right">
                编号：
            </th>
            <td width="86%" bgcolor="#E3F1FC">
                <input name="txt_Number" type="text" class="xtinput" id="txt_Number" size="40" value="<%=Number %>" />
            </td>
        </tr>
        <tr class="odd">
            <th width="14%" height="30" align="right">
                <span style="color: Red;">*</span>制度标题：
            </th>
            <td width="86%" bgcolor="#E3F1FC">
                <input name="txt_RegentTitle" type="text" class="xtinput" id="txt_RegentTitle" size="40"
                    valid="required" errmsg="请输入制度标题" value="<%=RegentTitle %>" />
                <span id="errMsg_txt_RegentTitle" style="color: Red;"></span>
            </td>
        </tr>
        <tr class="odd">
            <th width="14%" height="30" align="right">
                制度内容：
            </th>
            <td width="86%" bgcolor="#E3F1FC">
                <textarea name="txt_RegentContent" class="xtinput" id="txt_RegentContent" runat="server"
                    style="width: 550px; height: 180px;"></textarea>
            </td>
        </tr>
        <tr class="odd">
            <th width="14%" height="30" align="right">
                附件上传：
            </th>
            <td width="86%" bgcolor="#E3F1FC">
                <input style="float:left" type="file" name="file_Bylaw" id="file_Bylaw" />&nbsp;&nbsp;&nbsp;&nbsp;<div runat="server" style="float:left" id="file"><a
                    href="<%=FileHref %>" id="view_file" target="_blank">查看图片</a>
                <img src="../../images/closebox2.gif" style="cursor:pointer" id="del_file" /></div>
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="8" align="center" bgcolor="#E3F1FC">
                <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return EditBylaw.Save('save');">保存</a>
                        </td>
                        <td width="158" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">
                                关闭</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    <input id="hidDutyID" name="hidWorkerID" type="hidden" value="" runat="server" />
    <input id="hidFileValue" name="hidFileValue" type="hidden" runat="server" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script type="text/javascript">
        //初始化编辑器
        KE.init({
            id: '<%=txt_RegentContent.ClientID %>', //编辑器对应文本框id
            width: '550px',
            height: '180px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keSimple //功能模式(keMore:多功能,keSimple:简易)
        });
        var EditBylaw = {
            Save: function(method) {//保存
                var Result = false;
                Result = ValiDatorForm.validator($("#form1").get(0), "span");
                if (!Result) {
                    return false;
                }

                $("#hidMethod").attr("value", method);
                $("#<%=form1.ClientID %>").get(0).submit();
                return false;
            }
        };
        $(function() {
            setTimeout(function() {
                KE.create('<%=txt_RegentContent.ClientID %>', 0); //创建编辑器
            }, 100);
            FV_onBlur.initValid($("#form1").get(0));
        });
        $("#del_file").click(function() {
            if (confirm("确定要删除么?")) {
                $("#<%=file.ClientID %>").css("display", "none");
                $("#<%=hidFileValue.ClientID %>").val("");
            }
        })
    </script>

    </form>
</body>
</html>
