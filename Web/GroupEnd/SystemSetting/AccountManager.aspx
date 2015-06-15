<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManager.aspx.cs"
    Inherits="Web.GroupEnd.SystemSetting.AccountManager" MasterPageFile="~/masterpage/Front.Master" %>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <form id="form1" runat="server" enctype="multipart/form-data">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">系统设置</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 系统设置
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a class="tabtwo-on">帐号管理</a></li>
            <li><a href="CompanyInfo.aspx" class="">公司信息</a></li>
            <li><a href="DeployManager.aspx" class="">配置管理</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox">
            <input type="hidden" id="hidMethod" value="save" name="hidMethod" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_1">
                <tr>
                    <td width="10%" height="40" align="right">
                        用户名：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td width="86%">
                        <asp:Label ID="Txt_UserName" runat="server"></asp:Label><input type="hidden" id="txtUserName"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao">*</font>密码：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="Txt_PassWord" runat="server" valid="required" errmsg="请填写密码!" CssClass="searchinput searchinput02"></asp:TextBox>
                        <span id="errMsg_<%=Txt_PassWord.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao">*</font>姓名：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Name" id="Txt_Name" class="searchinput searchinput02"
                            runat="server" valid="required" errmsg="请填写姓名!" />
                        <span id="errMsg_<%=Txt_Name.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        性别：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSex" runat="server">
                            <asp:ListItem Text="请选择性别" Value="0"></asp:ListItem>
                            <asp:ListItem Text="女" Value="1"></asp:ListItem>
                            <asp:ListItem Text="男" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        职位：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Position" id="Txt_Position" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        电话：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Phone" id="Txt_Phone" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        传真：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Fox" id="Txt_Fox" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao">*</font>手机：
                    </td>
                    <td width="2%">
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Moblie" id="Txt_Moblie" class="searchinput searchinput02"
                            runat="server" valid="required" errmsg="请填写手机号码!" />
                        <span id="errMsg_<%=Txt_Moblie.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        QQ：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_QQNumber" id="Txt_QQNumber" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        E-mail：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Email" id="Txt_Email" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr align="left">
                    <td height="35" align="right">
                        <font class="xinghao"></font>备注：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <textarea name="Txt_Remiks" id="Txt_Remiks" cols="45" rows="5" class="textareastyle"
                            runat="server"></textarea>
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="Javascript:void(0);" id="BtnIsSave" onclick="return Save();">保存</a></li>
                <li><a href="Javascript:void(0);" id="BtnIsHistory">返回</a> </li>
                <div class="clearboth">
                </div>
            </ul>
        </div>
    </div>
    </form>

    <script type="text/javascript" language="javascript">
        function Save() {
            var form = FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));
            //验证初始化
            var form = $("#<%=form1.ClientID %>").get(0);
            var validatorshow = ValiDatorForm.validator(form, "span"); //获取提示信息
            if (validatorshow) {
                $("#<%=form1.ClientID %>").submit();
            }
        }
    </script>

</asp:Content>
