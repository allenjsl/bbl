<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamPlanAddMan.aspx.cs"
    Inherits="Web.TeamPlan.TeamPlanAddMan" %>

<%@ Register Src="../UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>

    <script src="../js/loadVisitors.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="850px">
        <tr class="even">
            <th height="30" rowspan="2" align="center">
                游客信息：
            </th>
            <td height="30" colspan="5" align="center">
                <table width="50%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="80%" align="center">
                            上传附件：
                            <input type="file" name="fuiLoadAttachment" id="fuiLoadAttachment" />
                        </td>
                        <td width="20%" align="left">
                            <uc1:LoadVisitors ID="LoadVisitors1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="even">
            <td colspan="5" align="center">
                <table width="95%" border="0" align="center" id="tblVisitorList" cellpadding="0"
                    cellspacing="1" bgcolor="#BDDCF4" style="margin: 10px 0;">
                    <tr>
                        <td height="5%" align="center" bgcolor="#E3F1FC">
                            编号
                        </td>
                        <td height="25" align="center" bgcolor="#E3F1FC">
                            姓名
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            类型
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件名称
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件号码
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            性别
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            联系电话
                        </td>
                        <td align="center" bgcolor="#E3F1FC" width="50">
                            特服
                        </td>
                        <td align="center" bgcolor="#E3F1FC" width="100">
                            操作
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="320" border="0" cellspacing="0" cellpadding="0" align="center">
        <tr>
            <td height="40" align="center">
            </td>
            <td align="center" class="tjbtn02">
                <input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                <asp:LinkButton ID="LinkButton1" CommandName="submit" runat="server" OnClick="LinkButton1_Click"
                    OnClientClick="return CheckingSave()">确认提交</asp:LinkButton>
            </td>
            <td height="40" align="center" class="tjbtn02">
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hideOrderId" runat="server" />

    <script type="text/javascript">
        function querystring(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        var VisitorData = null; //游客信息数组
        $(function() {


            VisitorData = "<%= VisitorArr.ToString() %>";

            VisitorData = eval(VisitorData);

            if (VisitorData.length == 0) { VisitorData = null; }

            //新增时初始化游客列表，添加一行空行游客
            loadVisitors.init({ data: VisitorData, autoComputeToTalAmountHandle: function() {
                //$("#txtPersonNum").val($("#tblVisitorList").find("tr:gt(0)").length);
            }
            });

            $("#ImgbtnLoad").click(function() {//弹出导入窗口
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "从文件导入",
                    width: "853px",
                    height: "514px",
                    modal: true
                });
                return false;
            });
            $("#linkSpeServe").click(function() {//弹出特服窗口
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "特服",
                    modal: true,
                    width: "420px",
                    height: "200px"
                });
                return false;
            });
        });

        //根据用户输入的身份证号判断性别
        function getSex(obj) {
            var val = $(obj).val();
            var tr = $(obj).parent().parent();
            var sex = tr.children().children("select[name='ddlSex']");
            var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
            if (isIdCard.exec(val)) {
                if (15 == val.length) {// 15位身份证号码
                    if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                        sex.val(2);
                    else
                        sex.val(1);
                }

                if (18 == val.length) {// 18位身份证号码
                    if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                        sex.val(2);
                    else
                        sex.val(1);
                }
            } else {
                sex.val(0);
            }
        }
        function CheckingSave() {
            var hd_IsRequiredTraveller = $("#hd_IsRequiredTraveller").val();
            //游客验证（hd_IsRequiredTraveller是否验证根据配置false时后面的参数允许为""，txtVisitorName姓名框name，ddlCardType=证件类型name，txtContactTel=电话框name）
            var msg = visitorChecking.isChecking(hd_IsRequiredTraveller, "txtVisitorName", "ddlCardType", "txtContactTel", "txtCardNo");
            if (!msg.isYes) {
                alert(msg.msg);
            }

            return msg.isYes;
        }

    </script>

    </form>
</body>
</html>
