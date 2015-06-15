<%@ Page Title="系统配置_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="ConfigSet.aspx.cs" Inherits="Web.systemset.ConfigSet" %>

<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">系统设置</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：系统设置>> 系统配置
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 30px;">
        </div>
        <form runat="server" id="configForm" enctype="multipart/form-data">
        <div class="tablelist">
            <input type="hidden" id="hidMethod" value="save" name="hidMethod" />
            <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                <tr>
                    <th colspan="3" align="center" bgcolor="#BDDCF4">
                        系统配置信息
                    </th>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>最长留位时间：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input id="txtRetainHour" runat="server" type="text" size="10" onblur="toMin(this)" />
                        小时
                        <input runat="server" id="txtRetainMin" type="text" size="10" onblur="toHour(this)"
                            readonly="readonly" />
                        分钟(最长留位时间按分钟计算，输入小时数后会自动折算成分钟)
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>价格组成配置：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:RadioButtonList ID="rdiPrice" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="分项价格" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="统一价格" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>列表显示控制前：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" id="txtBeforeMonth" runat="server" />月(只能输入整数)
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>列表显示控制后：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" id="txtAfterMonth" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>送团人：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <uc1:selectOperator ID="GroupSender" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>集合地：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" id="GroupSet" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>集合标志：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="text" id="SetMark" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>公司Log：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="file" name="fileLog" id="fileLog" />
                        <input type="hidden" id="hidLog" runat="server" />&nbsp;<%=companyLog%>
                        <span id="Span4" style="color: Red; display: none;">图片格式不正确</span>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>打印页眉：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="file" name="fileHeader" id="fileHeader" />
                        <input type="hidden" id="hidHeader" runat="server" />&nbsp;<%=pageHeader %>
                        <span id="hMess" style="color: Red; display: none;">图片格式不正确</span>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>打印页脚：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="file" name="fileFooter" id="fileFooter" />
                        <input type="hidden" id="hidFooter" runat="server" />&nbsp;<%=pageFooter%>
                        <span id="Span1" style="color: Red; display: none;">图片格式不正确</span>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>打印模板：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="file" name="fileModel" id="fileModel" />
                        <input type="hidden" id="hidModel" runat="server" />&nbsp;<%=pageModel%>
                        <span id="Span2" style="color: Red; display: none;">图片格式不正确</span>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>打印公章：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <input type="file" name="fileSeal" id="fileSeal" />
                        <input type="hidden" id="hidSeat" runat="server" />&nbsp;<%=departSeal%>
                        <span id="Span3" style="color: Red; display: none;">图片格式不正确</span>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                        <strong>收款提醒：</strong>
                    </td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                        <asp:RadioButtonList runat="server" ID="rbl_ReceiptRemind" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">提醒销售员</asp:ListItem>
                            <asp:ListItem Value="0">提醒全部用户</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="height: 35px; background: #e3f1fc; text-align: right;">
                        <b>弹窗提醒时间配置：</b>
                    </td>
                    <td style="background: #fafdff" colspan="2">
                        <input type="text" id="txtTanChuangTiXingInterval" runat="server" value="10" maxlength="3" />分钟
                    </td>
                </tr>
                <tr>
                    <td height="30" colspan="3" align="center">
                        <table border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="137" height="20" align="center" class="tjbtn02">
                                    <a href="javascript:;" onclick="return save();">保存</a>
                                </td>
                                <td width="135" height="20" align="center" class="tjbtn02">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>

    <script type="text/javascript">
        function del(hidId, tar) {
            if (confirm("你确定要删除该文件吗？")) {
                $("#" + hidId).val("");
                $(tar).replaceWith("");
                $("." + hidId).css("display", "none");
            }
            return false;
        }
        function save() {
            var mess = "";
            var hour1 = $("#<%=txtRetainHour.ClientID %>").val();
            var after1 = $("#<%=txtAfterMonth.ClientID %>").val();
            var before1 = $("#<%=txtBeforeMonth.ClientID %>").val();
            var grouper = $("#GroupSender.ClientID").val();
            var setaddress = $("#GroupSet.ClientID").val();
            var setmark = $("#SetMark.ClientID").val();
            if (!/(^[1-9]\d*$)|(^[1-9]+\d*.\d$)|(^0.[1-9]$)/.test(hour1)) {
                mess += "最长留位小时数格式不正确！\n";
            }
            if (!/^\d+$/.test(after1)) {
                mess += "列表显示控制后月份数必须为数字！\n";
            }
            if (!/^\d+$/.test(before1)) {
                mess += "列表显示控制前月份数必须为数字！\n";
            }
            var fl = $("#fileLog").val();
            var hV = $("#fileHeader").val();
            var fV = $("#fileFooter").val();
            var sV = $("#fileSeal").val();
            var mV = $("#fileModel").val(); //验证上传文件格式
            if (fl != "") { if (!/\.jpg|\.jpeg|\.bmp|\.gif|\.png$/i.test(fl)) { mess += "公司Logo不正确\n"; } }
            if (hV != "") { if (!/\.jpg|\.jpeg|\.bmp|\.gif|\.png$/i.test(hV)) { mess += "页眉格式不正确\n"; } }
            if (fV != "") { if (!/\.jpg|\.jpeg|\.bmp|\.gif|\.png$/i.test(fV)) { mess += "页脚格式不正确\n"; } }
            if (sV != "") { if (!/\.jpg|\.jpeg|\.bmp|\.gif|\.png$/i.test(sV)) { mess += "公章格式不正确\n"; } }
            if (mV != "") { if (!/\.dot$/i.test(mV)) { mess += "模板格式不正确\n"; } }
            if (mess != "") {
                alert(mess);
                return false;
            }
            $("#<%=configForm.ClientID %>").submit();

            return false;
        }
        function toMin(tar) {
            var value1 = tar.value;
            if ($.trim(value1) != "") {

            }
            $("#<%=txtRetainMin.ClientID %>").val(parseFloat(tar.value) * 60);

        }
        function toHour(tar) {
            $("#<%=txtRetainHour.ClientID %>").val(parseFloat(tar.value) / 60);
        }
		    
    </script>

</asp:Content>
