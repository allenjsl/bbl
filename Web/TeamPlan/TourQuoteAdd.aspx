<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourQuoteAdd.aspx.cs" Inherits="Web.TeamPlan.TourQuoteAdd" %>

<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            text-align: right;            
        }
        .style2
        {
            text-align: right;
            width: 91px;
        }
        .style3
        {
            width: 91px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" style="width: 500px">
            <tbody>
                <tr class="odd">
                    <td height="25" class="style2">
                        <font class="xinghao">*</font>标题：
                    </td>
                    <td align="left" width="400px">
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right" class="style3">
                        有效期：
                    </td>
                    <td>
                        <asp:TextBox ID="txtValidityBegin" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                        到
                        <asp:TextBox ID="txtValidityEnd" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="25" class="style2">
                        上传报价单：
                    </td>
                    <td align="left">
                        <input id="fileField" type="file" name="fileField" />
                        <asp:Panel ID="pnlFile" runat="server">
                            <img src="/images/open-dx.gif" alt="" />
                            <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                            <a href="javascript:void(0);" onclick="DeleteFile()">
                                <img src="/images/fujian_x.gif" /></a>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="even">
                    <td class="style2">
                        <font class="xinghao">*</font>发布时间：
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtPeriod" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <td align="right">
                        <font class="xinghao">*</font>负责人：</td><td><uc1:selectOperator ID="selectOperator1" runat="server" />
                    </td>
                </tr>
                <tr class="odd">
                    <td class="style1" colspan="2" align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return CheckForm()" OnClick="btnSave_Click">保存</asp:LinkButton>
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                                            id="linkCancel" href="javascript:void(0);">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    
    <asp:HiddenField ID="hideFilePath" runat="server" />
    </form>
    
    <script type="text/javascript">
        function CheckForm() {
            var txtTitle = $.trim($("#<%=txtTitle.ClientID %>").val());
            var txtDate = $("#<%=txtPeriod.ClientID %>").val();
            var txtUser = $('#<%=selectOperator1.FindControl("txt_op_Name").ClientID%>').val();
            if (txtTitle == "") {
                alert("请输入标题");
                return false;
            }

            if (txtDate == "") {
                alert("请选择日期");
                return false;
            }
            if (txtUser == "") {
                alert("请选择负责人")
                return false;
            }
            if (txtUser.split(",").length > 1) {
                alert("只能选择一个负责人!");
                return false;
            }
            return true;
        }

        function DeleteFile() {
            if (confirm("确认删除?")) {
                $("#<%=pnlFile.ClientID %>").html("");
                $("#<%=hideFilePath.ClientID %>").val("");
            }
            return false;
        }
    </script>
</body>
</html>
