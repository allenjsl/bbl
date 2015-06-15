<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateLineProducts.aspx.cs"
    Inherits="Web.xianlu.UpdateLineProducts" MasterPageFile="~/masterpage/Back.Master"
    Title="标准版发布_线路管理" %>

<%@ Register Src="~/UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ConProjectControl.ascx" TagName="ProjectControl"
    TagPrefix="uc4" %>
<asp:Content ContentPlaceHolderID="c1" ID="Content1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <input type="hidden" value="1" name="issave" />

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">线路产品库</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 线路产品库
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
            <li><a href="javascript:viod(0);" class="tabtwo-on">标准版发布</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox" id="UpdateLineBox">
            <asp:HiddenField ID="hideType" runat="server" />
            <asp:HiddenField ID="hidRouteid" runat="server" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_1">
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>线路区域：
                    </td>
                    <td width="86%">
                        <select id="ddlLineType" runat="server" valid="required" errmsg="请选择线路区域!">
                        </select><span id="errMsg_<%=ddlLineType.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>线路名称：
                    </td>
                    <td>
                        <input type="text" name="txt_LineName" id="txt_LineName" class="searchinput searchinput02"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao"></font>线路描述：
                    </td>
                    <td>
                        <input name="Txt_XianlDescript" type="text" class="searchinput searchinput02" id="Txt_XianlDescript"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>旅游天数：
                    </td>
                    <td>
                        <input name="Txt_Days" type="text" class="searchinput searchinput03" id="Txt_Days"
                            runat="server" valid="required" errmsg="请输入旅游天数!" />天 <span id="errMsg_<%=Txt_Days.ClientID %>"
                                class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc1:xingcheng ID="xingcheng1" runat="server" daysControlName="txt_Days" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="xuexian">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td height="40" background="/images/bg003.gif" align="right">
                        添加附件：
                    </td>
                    <td background="/images/bg003.gif">
                        <div style="float:left; margin-right:10px;"><input type="file" id="fileUpLoad" name="fileUpLoad"/></div>
                      <asp:Panel ID="pnlFile" runat="server">
                            <img src="/images/open-dx.gif" />
                           <asp:HyperLink ID="hypFilePath" runat="server"  Target="_blank">查看附件</asp:HyperLink>
                            <a href="javascript:void(0);" id="DeleFile">
                                <img src="/images/fujian_x.gif" /></a>
                        </asp:Panel>
                        <input  runat="server" id="hidOldFilePath" type="hidden"/>
                        <input  runat="server" id="hidOldName" type="hidden"/>
                    </td>
                </tr>
                <tr>
                    <td height="10" colspan="2" align="right">
                        <table width="100%" cellpadding="0" cellspacing="1" border="0">
                            <tr class="odd">
                                <th height="25" align="center" width="13%">
                                    &nbsp
                                </th>
                                <th align="center" width="12%">
                                    项目
                                </th>
                                <th align="center" width="59%">
                                    接待标准
                                </th>
                                <th align="center" width="16%">
                                    操作
                                </th>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" border="0" cellspacing="1" cellpadding="0">
                            <tr>
                                <th height="60" align="center" bgcolor="#E3F1FC">
                                    包含项目：
                                </th>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <uc4:ProjectControl ID="ProjectControl1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th height="60" align="center" bgcolor="#BDDCF4">
                                    不含项目：
                                </th>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="Txt_XianlProjectNo" id="Txt_XianlProjectNo" class="textareastyle"
                                        runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th height="60" align="center" bgcolor="#E3F1FC">
                                    购物安排：
                                </th>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="Txt_ShoppPlan" id="Txt_ShoppPlan" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>                           
                            <tr>
                                <th height="60" align="center" bgcolor="#BDDCF4">
                                    自费项目：
                                </th>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="Txt_expensce" id="Txt_expensce" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                             <tr>
                                <th height="60" align="center" bgcolor="#E3F1FC">
                                    儿童安排：
                                </th>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="Txt_ChildPlan" id="Txt_ChildPlan" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th height="60" align="center" bgcolor="#BDDCF4">
                                    注意事项：
                                </th>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="Txt_Notes" id="Txt_Notes" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <th height="60" align="center" bgcolor="#E3F1FC">
                                    温鑫提醒：
                                </th>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="Txt_Remind" id="Txt_Remind" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="4" align="center">
                                </td>
                            </tr>
                            <tr bgcolor="#BDDCF4">
                                <th height="30" colspan="4" align="center" style="border: 1px #93B5D7 solid;">
                                    内部信息（内部注意事项，内部资料等）
                                </th>
                            </tr>
                            <tr>
                                <td height="10" colspan="4" align="center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <textarea name="Txt_Reception" id="Txt_Reception" class="textareastyle" runat="server"
                                        cols="20" rows="1"></textarea>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="JavaScript:void(0);" id="OnSave">保存</a></li>
                <li><a href="Javascript:Void(0);" id="ISHistory">返回</a> </li>
                <div class="clearboth">
                </div>
            </ul>
        </div>
    </div>
    <asp:Literal ID="litMsg" runat="server"></asp:Literal>

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#DeleFile").click(function() {
                if (confirm("是否确认删除图片?")) {
                    $("#<%=pnlFile.ClientID %>").html("");
                }
            });
            //获取表单验证
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));

            $("#OnSave").click(function() {
                var form = $("#<%=form1.ClientID %>").get(0);
                var vResult = ValiDatorForm.validator(form, "span");
                if (vResult) {
                    $("#<%=form1.ClientID %>").submit();
                    return false;
                }
            });
            $("#ISHistory").click(function() {
                window.location.href = "/xianlu/LineProducts.aspx";
                return false;
            });
        });
    
    </script>

    </form>
</asp:Content>
