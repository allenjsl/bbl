<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncomeDetails.aspx.cs"
    Inherits="Web.caiwuguanli.IncomeDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="670" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <th height="30" bgcolor="#bddcf4" align="center" width="86">
                        <asp:Label ID="lblDateTime" runat="server" Text="收款日期"></asp:Label>
                    </th>
                    <th bgcolor="#bddcf4" align="center" width="86">
                        <asp:Label ID="lblPro" runat="server" Text="收入项目"></asp:Label>
                    </th>
                    <th bgcolor="#bddcf4" align="center" width="86">
                        单位名称
                    </th>
                    <th bgcolor="#bddcf4" align="center" width="94">
                        <asp:Label ID="lblPeople" runat="server" Text="收款人"></asp:Label>
                    </th>
                    <th bgcolor="#bddcf4" align="center" width="79">
                        金额
                    </th>
                    <th bgcolor="#bddcf4" align="center" width="145">
                        备注
                    </th>
                </tr>
                <tr class="even" id="tbl_tr">
                    <td height="30" align="center">
                        <asp:TextBox ID="txtDateTime" CssClass="searchinput" runat="server"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtProName" CssClass="searchinput" runat="server"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtCompany" CssClass="searchinput" runat="server"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtPeople" CssClass="searchinput" runat="server"></asp:TextBox>
                    </td>
                    <td align="center">
                        <font class="fbred">
                            <asp:TextBox ID="txtPrice" CssClass="searchinput" runat="server"></asp:TextBox>
                        </font>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtRemarks" CssClass="searchinput" runat="server" TextMode="MultiLine"
                            Width="165px" Height="50px"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="320">
            <tbody>
                <tr>
                    <td height="40" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="" Width="1" Height="1" BorderColor="Transparent"
                            BackColor="Transparent" OnClick="btnSave_Click" />
                    </td>
                    <td height="40" align="center" class="tjbtn02">
                        <a href="javascript:void(0);" id="a_btnSave">保存</a>
                    </td>
                    <td align="center" class="tjbtn02">
                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                            id="linkCancel" href="javascript:;">关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script type="text/javascript">
        $(function() {
            $("#a_btnSave").click(function() {
                var b = false;
                $("#tbl_tr").find("input[type='text']").each(function() {
                    if ($.trim($(this).val()) != "") {
                        b = true;
                    }
                });

                if ($.trim($("#<%=txtRemarks.ClientID %>").val()) != "") {
                    b = true;
                }
                //如果有一个输入，那么允许提交
                if (b) {
                    $("#<%=btnSave.ClientID %>").click();
                } else {
                    alert("请输入值!");
                }
            })
        })
    </script>

    </form>
</body>
</html>
