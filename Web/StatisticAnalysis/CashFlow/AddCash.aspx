<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCash.aspx.cs" Inherits="Web.StatisticAnalysis.CashFlow.AddCash"  Title="补充现金_现金流量表_统计分析" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>补充现金</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.js"></script>
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="300" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px">
            <tr class="odd">
                <th width="100" height="30" align="center">
                    补充时间：
                </th>
                <td>
                    &nbsp;<b><asp:Label ID="lblAddTime" runat="server" Text=""></asp:Label></b>
                   <%-- <input name="txtAddDate" runat="server" type="text" class="searchinput" id="txtAddDate"
                        onfocus="WdatePicker()"  errmsg="*补充时间不能为空！" valid="required"  />
                <span id="errMsg_txtAddDate" class="errmsg"></span>--%>
                </td>
            </tr>
            <tr class="even">
                <th width="100" height="30" align="center">
                    补充金额：
                </th>
                <td>
                    <input name="txtAddCash" type="text" runat="server" class="searchinput" id="txtAddCash"
                    errmsg="*补充金额不能为空！|*请输入合法的金额！" valid="required|isMoney" />
                    <span id="errMsg_txtAddCash" class="errmsg"></span>
                </td>
            </tr>
        </table>
        <table width="280" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <asp:LinkButton ID="lbtnSave" Text="保存" runat="server" OnClick="lbtnSave_Click"></asp:LinkButton>
                </td>
                <td align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function() {
        $("#lbtnSave").click(function() {
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "span");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#lbtnSave").closest("form").get(0));
        })
    </script>
    </form>
</body>
</html>
