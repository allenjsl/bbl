<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketPolicyAdd.aspx.cs"
    Inherits="Web.systemset.ToGoTerrace.TicketPolicyAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <div class="tablelist">
            <table width="800" border="0" align="center" cellpadding="2" cellspacing="1" bgcolor="#BDDCF4">
                <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">
                        机票政策
                    </th>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <span style="color: red">*</span><strong>标题：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:TextBox ID="txt_Title" runat="server"></asp:TextBox>
                        <asp:Literal ID="lit_Title" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>添加附件：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="updom">
                        <%if (!string.IsNullOrEmpty(filePath))
                          { %>
                        <a target="_blank" href="<%= filePath %>">查看附件</a><img style="cursor: pointer" src="/images/fujian_x.gif"
                            width="14" height="13" class="close" alt="" />
                        <%}
                          else
                          { %>
                        <input type="file" name="workAgree" />
                        <%} %>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>内容：</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:TextBox ID="txt_Contert" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td height="30" colspan="3" align="center">
                        <table width="459" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="137" height="40" align="center" class="tjbtn02">
                                    <asp:LinkButton ID="linkbtnSave" runat="server" OnClick="linkbtnSave_Click">保存</asp:LinkButton>
                                </td>
                                <td width="135" height="40" align="center" class="tjbtn02">
                                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                                        关闭</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <input type="hidden" name="tid" value="<%=id %>" />
        </div>
    </div>
    </form>

    <script type="text/javascript">
        KE.init({
            id: '<%=txt_Contert.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        function checking() {
            var linkname = $("#<%=txt_Title.ClientID %>").val();

            if (linkname == "" || linkname == null) {
                alert("标题不能为空！");
                return false;
            }

            return true;
        }

        $(function() {
            KE.create('<%=txt_Contert.ClientID %>', 0); //创建编辑器


            $("#<%=linkbtnSave.ClientID %>").click(function() {
                return checking();
            });
            //删除原有loge
            $("img.close").click(function() {

                var that = $(this);

                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //生成添加协议的控件
                return false;
            });

        })
    </script>

</body>
</html>
