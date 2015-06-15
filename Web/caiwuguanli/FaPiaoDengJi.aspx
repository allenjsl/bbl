<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaPiaoDengJi.aspx.cs" Inherits="Web.caiwuguanli.FaPiaoDengJi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>财务管理-发票登记</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <style type="text/css">
    td{ text-align:center; height:30px;}
    </style>    

    <script type="text/javascript">
        $(function() {
            $("#<%=this.lbtnSubmit.ClientID%>").click(function() {
                var form = $(this).closest("form").get(0);
                return ValiDatorForm.validator(form, "alert");
            });
            FV_onBlur.initValid($("#<%=this.lbtnSubmit.ClientID%>").closest("form").get(0));
        });

        function reloadParentWindow() {
            parent.window.location.href = parent.window.location.href;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
            <tr class="odd">
                <th style="width: 15%; height: 30px;">
                    开票日期
                </th>
                <th style="width: 15%;">
                    开票金额
                </th>
                <th style="width: 15%;">
                    开票人
                </th>
                <th style="width: 15%;">
                    票号
                </th>
                <th style="width: 30%;">
                    备注
                </th>
                <th>操作</th>
            </tr>
            <tr class="even">
                <td>
                    <span style="color: Red">*</span><input type="text" id="txtKaiPiaoRiQi"
                        onfocus="WdatePicker()" class="searchinput" style="width: 100px;" valid="required"
                        errmsg="请填写开票日期" maxlength="10" runat="server" />
                </td>
                <td>
                    <span style="color: Red">*</span><input type="text" id="txtKaiPiaoJinE"
                        valid="required|isMoney" errmsg="请填写开票金额!|请填写正确的开票金额!" maxlength="10" class="searchinput"
                        style="width: 100px;" runat="server" />
                </td>
                <td>
                    <span style="color: Red">*</span><input type="text" id="txtKaiPiaoRen"
                        valid="required" errmsg="请填写开票人!" maxlength="10" class="searchinput" style="width: 100px;" runat="server" />
                </td>
                <td>
                    <input type="text" id="txtPiaoHao" maxlength="50" class="searchinput"
                        style="width: 100px;" runat="server" />
                </td>
                <td>
                    <textarea id="txtBeiZhu" cols="20" rows="2" class="searchinput"
                        style="height: auto; width: 220px;" runat="server"></textarea>
                </td>
                <td><asp:LinkButton runat="server" ID="lbtnSubmit" Text="保存" OnClick="lbtnSubmit_Click"></asp:LinkButton> </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
