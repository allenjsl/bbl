<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tuipiao.aspx.cs" Inherits="Web.sales.Tuipiao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单退票操作</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <style type="text/css">
        .errmsg
        {
            color: red;
            font-size: 12px;
        }
        .style1
        {
            height: 33px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px;
            height: 650px; width: 900px;">
            <tbody>
                <tr class="odd">
                    <td height="28" align="center" width="99">
                        操作员：
                    </td>
                    <td align="left">
                        <asp:Label ID="lblOPer" runat="server" Text="" name="lblOPer"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="28" align="center">
                        <span class="errmsg">*</span>航段：
                    </td>
                    <td align="left" valign="top">
                        <table cellspacing="1" cellpadding="0" border="0" width="100%" style="margin-bottom: 0px;">
                            <tr class="odd" height="30px" align="center">
                                <th height="30px">
                                    <input type="checkbox" id="select_all" />全选
                                </th>
                                <th height="30px">
                                    日期
                                </th>
                                <th height="30px">
                                    航段
                                </th>
                                <th height="30px">
                                    航班号/时间
                                </th>
                                <th height="30px">
                                    航空公司
                                </th>
                                <th height="30px">
                                    折扣
                                </th>
                            </tr>
                            <asp:Repeater ID="rpTicket" runat="server">
                                <ItemTemplate>
                                    <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>" align="center">
                                        <td>
                                            <%# RefResult(Convert.ToString(Eval("TicketId")),Convert.ToString(Eval("Id")))%>
                                        </td>
                                        <td>
                                            <%# Eval("TicketTime")%>
                                        </td>
                                        <td>
                                            <%#Eval("FligthSegment")%>
                                        </td>
                                        <td>
                                            <%# string.Format("{0:d}" ,Eval("DepartureTime"))%>
                                        </td>
                                        <td>
                                            <%#Eval("AireLine")%>
                                        </td>
                                        <td>
                                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("Discount").ToString()).ToString("0.00"))%>%
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div width="100%" style="text-align: center; margin-top: 50px">
                            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label></div>
                    </td>
                </tr>
                <tr class="odd">
                    <td align="center" class="style1">
                        退票需知：
                    </td>
                    <td align="left" class="style1">
                        <asp:TextBox ID="txtTuiPiaoMust" runat="server" TextMode="MultiLine" class="textareastyle02"
                            MaxLength="500" Height="159px" Width="600px"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr class="even">
                    <td height="28" align="left" colspan="2">
                        <asp:CheckBox ID="chkBox" runat="server" name="chkBox" />
                        <label for="<%=this.chkBox.ClientID%>">
                            退票后重新出票</label>
                    </td>
                </tr>--%>
                <tr>
                    <td height="40" align="center" colspan="2">
                        <table cellspacing="0" cellpadding="0" border="0" width="320" id="hidtd">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lkBtnSave" runat="server" OnClientClick="return checkForm()"
                                            OnClick="lkBtnSave_Click">保存</asp:LinkButton>
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <span id="spanSubmit" style="display: none">数据提交中...</span>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="planIds" runat="server" />
    </form>

    <script type="text/javascript">
        function checkForm() {
            var count = 0;
            var ids = "";
            $(".check").each(function() {
                if ($(this).attr("checked") == true && $(this).attr("disabled") != true) {
                    ids += $(this).attr("id").split("_")[1] + "|";
                    count++;
                }
            })
            if (count == 0) {
                alert("请至少选择一个航段");
                return false;
            } else {
                $("#<%=planIds.ClientID %>").val(ids);
                $("#hidtd").attr("style", "display:none");
                $("#spanSubmit").attr("style", "display:block");
                return true;
            }
        }
        $("#select_all").click(function() {
            if ($(this).attr("checked") == true) {
                $(".check").each(function() {
                    if ($(this).attr("disabled") != true) {
                        $(this).attr("checked", true);
                    }
                })

            } else {
                $(".check").each(function() {
                    if ($(this).attr("disabled") != true) {
                        $(this).attr("checked", false);
                    }
                })


            }
        })
        
        
    </script>

</body>
</html>
