<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrandEdit.aspx.cs" Inherits="Web.systemset.basicinfo.BrandEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>品牌管理_基础设置_系统设置</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="beForm" runat="server" method="post" enctype="multipart/form-data">
    <input type="text" style="display: none;" />
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 20px auto;">
        <tr class="odd">
            <th width="21%" height="30" align="right">
                <span style="color: Red">*</span>品牌名称：
            </th>
            <td width="79%" bgcolor="#E3F1FC">
                <input name="txtBrand" type="text" value="<%=brandName %>" class="xtinput" id="txtBrand"
                    size="40" maxlength="20" valid="required" errmsg="品牌名称不为空" />
                <span id="errMsg_txtBrand" class="errmsg"></span>
            </td>
        </tr>
        <tr class="odd">
            <th width="21%" height="30" align="right">
                内LOGO上传：
            </th>
            <td width="79%" bgcolor="#E3F1FC">
                <input type="file" id="inLogo" style="float: left" name="inLogo" /><span style="color: red;">(图片大小1003*97)</span>&nbsp;<%=logInUrl%>
                <span id="inMess" style="color: Red; display: none;">图片格式不正确</span>
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="2" align="left" bgcolor="#E3F1FC">
                <table width="340" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="106" height="40" align="center">
                            <asp:HiddenField ID="hid_file" runat="server" />
                        </td>
                        <td width="76" height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" onclick="return save('');">保存</a>
                        </td>
                        <td width="158" height="40" align="center" class="tjbtn02">
                            <span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">
                                关闭</a></span>
                        </td>
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
            FV_onBlur.initValid($("#<%=beForm.ClientID %>").get(0));
        });
        //保存报价信息
        function save(method) {
            var isSuccess = true;
            var isSuccess = ValiDatorForm.validator($("#<%=beForm.ClientID %>").get(0), "span");

            var inValue = document.getElementById("inLogo").value.replace(/^\s+/, "");

            if (inValue != "") {
                if (!/.jpg|.jpeg|.bmp|.gif|.png$/i.test(inValue)) {
                    document.getElementById("inMess").style.display = "";
                    isSuccess = false;
                }
                else {
                    document.getElementById("inMess").style.display = "none";
                }
            }

            if (!isSuccess) {
                return false;
            }
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            $("#<%=beForm.ClientID %>").get(0).submit();
            return false;

        }
        $("#del_file").click(function() {
            if (confirm("确定要删除么?")) {
                $("#<%=hid_file.ClientID %>").val("");
                $("#file").css("display", "none");
            }
        })
    </script>

</body>
</html>
