<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sale_tuikuan.aspx.cs" Inherits="Web.sales.Sale_tuikuan" %>

<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>销售退款页面</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <style type="text/css">
        .errmsg
        {
            color: red;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <table cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 0px;">
            <tbody>
                <tr class="odd">
                    <th width="86" height="30" bgcolor="#bddcf4" align="center">
                        退款日期
                    </th>
                    <th width="86" bgcolor="#bddcf4" align="center">
                        退款人
                    </th>
                    <th width="86" bgcolor="#bddcf4" align="center">
                        退款金额
                    </th>
                    <th width="94" bgcolor="#bddcf4" align="center">
                        退款方式
                    </th>
                    <th width="86" bgcolor="#bddcf4" align="center">
                        开票
                    </th>
                    <th width="79" bgcolor="#bddcf4" align="center">
                        开票金额
                    </th>
                    <th width="145" bgcolor="#bddcf4" align="center">
                        备注
                    </th>
                    <th width="130" bgcolor="#bddcf4" align="center">
                        操作
                    </th>
                </tr>
                <%=tuikuaiList%>
                <tr class="even">
                    <td height="30" align="center">
                        <span style="color: Red">*</span><asp:TextBox CssClass="searchinput" ID="txtTuiKuaiDate"
                            runat="server" onfocus="WdatePicker()" valid="required" errmsg="请写正确的日期!"></asp:TextBox>
                    </td>
                    <td align="center">
                        <span style="color: Red">*</span><asp:TextBox ID="txtTuiKuaiPerson" CssClass="searchinput"
                            runat="server" valid="required" errmsg="请写退款人!"></asp:TextBox>
                    </td>
                    <td align="center">
                        <span style="color: Red">*</span><asp:TextBox ID="txtTuiKuaiMoney" CssClass="searchinput"
                            MaxLength="10" runat="server" valid="required|isMoney" errmsg="请写退款金额!|请填写正确的退款金额!"></asp:TextBox>
                    </td>
                    <td align="center" width="110px">
                        <span style="color: Red">*</span><asp:DropDownList Width="85px" name="ddlTuiKuaiStyle" ID="ddlTuiKuaiStyle"
                            title="请选择" runat="server" valid="required" errmsg="请选择退款方式!">
                        </asp:DropDownList>
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddlKaiPiaoStyle" runat="server">
                            <asp:ListItem Value="-1">请选择</asp:ListItem>
                            <asp:ListItem Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtKaiPiaoMoney" CssClass="searchinput" MaxLength="10" runat="server" valid="isMoney"
                            errmsg="请填写正确的开票金额!"></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtMemo" TextMode="MultiLine" MaxLength="500" runat="server" ></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lbtnSave" CommandName="update" runat="server" OnClick="lbtnSave_Click">保存</asp:LinkButton>
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="320">
            <tbody>
                <tr align="center">
                    <td height="40" class="tjbtn02">
                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();window.parent.location.reload();"
                            id="linkCancel" href="javascript:;">关闭</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            $("#<%=this.lbtnSave.ClientID%>").click(function() {
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=this.lbtnSave.ClientID%>").closest("form").get(0));
        })

        function editTuiKuai(RID, OrderID, $obj) {
            var data = {
                RID: RID,
                OrderID: OrderID,
                TuiKuaiDate: $obj.parent().parent().find("td input[name='date']").val(),
                TuiKuaiPerson: $obj.parent().parent().find("td input[name='tuikuaiPerson']").val(),
                TuiKuaiMoney: $obj.parent().parent().find("td input[name='tuikuaiMoney']").val(),
                TuiKuaiStyle: $obj.parent().parent().find("td select[name='ddlShouStyle']").val(),
                KaiPiaoStyle: $obj.parent().parent().find("td select[name='ddlkaiStyle']").val(),
                KaiPiaoMoney: $obj.parent().parent().find("td input[name='kaipiaoMoney']").val(),
                Memo: $obj.parent().parent().find("td textarea[name='memo']").val(),
                State: "EDIT",
                act: '<%=EyouSoft.Common.Utils.GetQueryStringValue("act") %>'
            };
            var postUrl = "/sales/Sale_tuikuan.aspx?" + $.param(data);
            $.ajax({
                type: "Get",
                url: postUrl,
                cache: false,
                dataType: "html",
                success: function(data) {
                    if (data == "-1") {
                        alert("操作失败：没有权限。");
                        window.location.href = window.location.href;
                    } else {
                        alert(data);
                        location.href = location.href;
                    }
                }
            });
        }
        function passTuiKuai(RID, OrderID, obj) {
          
            var data = {
                RID: RID,
                OrderID: OrderID,
                TuiKuaiDate: $(obj).parent().parent().find("td input[name='date']").val(),
                TuiKuaiPerson: $(obj).parent().parent().find("td input[name='tuikuaiPerson']").val(),
                TuiKuaiMoney: $(obj).parent().parent().find("td input[name='tuikuaiMoney']").val(),
                TuiKuaiStyle: $(obj).parent().parent().find("td select[name='ddlShouStyle']").val(),
                KaiPiaoStyle: $(obj).parent().parent().find("td select[name='ddlkaiStyle']").val(),
                KaiPiaoMoney: $(obj).parent().parent().find("td input[name='kaipiaoMoney']").val(),
                Memo: $(obj).parent().parent().find("td textarea[name='memo']").val(),
                State: "PASS",
                act: '<%=EyouSoft.Common.Utils.GetQueryStringValue("act") %>'
            };
            

            var postUrl = "/sales/Sale_tuikuan.aspx?" + $.param(data);
            $.ajax({
                type: "Get",
                url: postUrl,
                cache: false,
                dataType: "html",
                success: function(data) {
                    if (data == "-1") {
                        alert("操作失败：没有权限。");
                        window.location.href = window.location.href;
                    } else {
                        alert(data);
                        location.href = location.href;
                    }
                },
                error: function() {
                    alert("审核出错!");
                    location.href = location.href;
                }
            });
        }
        function delTuiKuai(RID, OrderID) {
            if (!confirm("你确定要删除该退款登记吗？删除后数据不可恢复。")) {
                return false;
            }
        
            var data = {
                RID: RID,
                OrderID: OrderID,
                State: "DEL",
                act: '<%=EyouSoft.Common.Utils.GetQueryStringValue("act") %>'
            };
            var postUrl = "/sales/Sale_tuikuan.aspx?" + $.param(data);
            $.ajax({
                type: "Get",
                url: postUrl,
                cache: false,
                dataType: "html",
                success: function(data) {
                    if (data == "-1") {
                        alert("操作失败：没有权限。");
                        window.location.href = window.location.href;
                    } else {
                        alert(data);
                        location.href = location.href;
                    }
                },
                error: function() {
                    alert("删除出错!");
                    location.href = location.href;
                }
            });
        }

        //取消退款审核
        function cancelChecked(_v) {
            if (!confirm("你确定要取消退款审核吗?")) return;
            var postData = { "cancelId": _v, "orderId": '<%=EyouSoft.Common.Utils.GetQueryStringValue("OrderId") %>' };
            $.ajax({
                url: "/sales/sale_tuikuan.aspx?requestType=ajax_cancel",
                cache: false,
                data: postData,
                async: false,
                type: "POST",
                success: function(response) {
                    if (response == "0") {
                        alert("取消审核成功。");
                        window.location.href = window.location.href;
                    } else {
                        alert("取消审核失败，请重试。")
                    }
                }
            });
        }
    </script>

</body>
</html>
