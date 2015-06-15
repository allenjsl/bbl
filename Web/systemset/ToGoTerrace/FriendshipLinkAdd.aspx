<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FriendshipLinkAdd.aspx.cs"
    Inherits="Web.systemset.ToGoTerrace.FriendshipLinkAdd" %>

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
            <table width="400" border="0" align="center" cellpadding="2" cellspacing="1" bgcolor="#BDDCF4">
                <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">
                        ��������
                    </th>
                </tr>
                <tr>
                    <td width="30%" height="35" align="right" bgcolor="#e3f1fc">
                        <span style="color: red">*</span><strong>�����������֣�</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:TextBox ID="txt_LinkName" runat="server"></asp:TextBox>
                        <asp:Literal ID="lit_LinkName" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right" bgcolor="#e3f1fc">
                        <strong>�������ӵ�ַ��</strong>
                    </td>
                    <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:TextBox ID="txt_linkURL" runat="server" Text="http://"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td height="30" colspan="3" align="center">
                        <table width="200" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="137" height="40" align="center" class="tjbtn02">
                                    <asp:LinkButton ID="linkbtnSave" runat="server" OnClick="linkbtnSave_Click">����</asp:LinkButton>
                                </td>
                                <td width="135" height="40" align="center" class="tjbtn02">
                                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                                        �ر�</a>
                                </td>
                            </tr>  <input type="hidden" name="tid" value="<%=id %>" />
                        </table>
                    </td>
                </tr>
            </table>
          
        </div>
    </div>
    </form>

    <script type="text/javascript">
        function checking() {
        var linkname = $("#<%=txt_LinkName.ClientID %>").val();
        
            if (linkname == "") {
                alert("�����������ֲ���Ϊ�գ�");
                return false;
            }

        }
        $(function() {
            $("#<%=linkbtnSave.ClientID %>").click(function() {                
                return checking();
            });
            //ɾ��ԭ��loge
            $("img.close").click(function() {

                var that = $(this);

                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //�������Э��Ŀؼ�
                return false;
            });

        })
    </script>

</body>
</html>
