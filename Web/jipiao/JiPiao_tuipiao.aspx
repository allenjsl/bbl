<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiPiao_tuipiao.aspx.cs"
    Inherits="Web.jipiao.JiPiao_tuipiao" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <title>机票出票</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="hd_cardType" type="hidden" runat="server" />
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="880" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center" width="94">
                        线路名称：
                    </th>
                    <td align="center" width="259">
                        <asp:Label ID="lblLineName" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center" width="156">
                        团号：
                    </th>
                    <td align="center" width="69">
                        <asp:Label ID="lblTuanHao" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center" width="146">
                        人数：
                    </th>
                    <td align="center" width="269">
                        <asp:Label ID="lblPersonCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        航段：
                    </th>
                    <td align="left" colspan="5">
                        <table width="100%">
                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" colspan="5">
                                            日期：
                                            <input type="text" class="searchinput" style="width:150px;"  value="<%#Convert.ToDateTime(Eval("DepartureTime")).ToString("yyyy-MM-dd")%>"/>
                                            航段：
                                            <input type="text" class="searchinput" value="<%#Eval("FligthSegment") %>" />
                                            航班号/时间：
                                            <input type="text" class="searchinput"  value="<%#Eval("TicketTime") %>" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="30" align="center" rowspan="2">
                        退票名单：
                    </th>
                </tr>
                <tr class="odd">
                    <td align="left" colspan="5">
                        <table width="100%" cellspacing="1" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <th height="25" align="center">
                                        姓名
                                    </th>
                                    <th align="center">
                                        证件类型
                                    </th>
                                    <th align="center">
                                        证件号码
                                    </th>
                                </tr>
                                <tr class="even">
                                    <td height="25" align="center">
                                        <input type="text" class="searchinput" id="txtCusName" name="txtCusName" runat="server" />
                                    </td>
                                    <td align="center">
                                        <select id="SelCardType" name="SelCardType">
                                            <option>
                                                <%=CardType%></option>
                                        </select>
                                    </td>
                                    <td align="center">
                                        <input type="text" class="searchinput searchinput02" id="txtCardNumber" name="txtCardNumber"
                                            runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <th height="68" align="center">
                        退票需知：
                    </th>
                    <td align="left" colspan="5">
                        <asp:TextBox ID="txtTuiPiaoMust" runat="server" class="textareastyle" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="6">
                        <table cellspacing="1" cellpadding="0" border="0" width="100%">
                            <tbody>
                                <tr class="odd">
                                    <th height="30" align="center" width="9%">
                                        <span style="color: red">*</span>状态：
                                    </th>
                                    <td align="center" width="16%">
                                        <asp:DropDownList ID="ddlJiPiaoState" title="请选择" valid="required" errmsg="请选择退票状态!"
                                            runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <th align="center" width="12%">
                                        <span style="color: red">*</span>退回金额：
                                    </th>
                                    <td align="left" width="63%">
                                        <asp:TextBox ID="txtTuiMoney" runat="server" class="searchinput" valid="required|isMoney"
                                            MaxLength="8" errmsg="请输入退票金额|请输入有效退票金额！"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="6">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click">保存</asp:LinkButton>
                                    </td>
                                    <td style="width:50px">&nbsp;</td>
                                    <td class="tjbtn02" style="text-align:center;">
                                        <asp:LinkButton ID="lbtnQuXiaoTuiPiao" runat="server" OnClick="lbtnQuXiaoTuiPiao_Click" OnClientClick="if(!confirm('取消退票将会清除退票以及与之关联的退票款等信息，具有不可逆性。你确定要取消退票吗？')) {return false;}  return true;">取消退票</asp:LinkButton>
                                    </td>
                                    <td style="width:50px">&nbsp;</td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#<%=this.lbtnSave.ClientID%>").click(function() {
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=this.lbtnSave.ClientID%>").closest("form").get(0));

            //证件类型初始化
            $("#SelCardType").each(function() {
                var selectIndex = $(this).val();
                proArray = $("#<%=hd_cardType.ClientID %>").val().split("|");
                $(this).html("");
                for (var i = 0; i < proArray.length; i++) {
                    var obj = eval('(' + proArray[i] + ')');
                    $(this).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }
                if (selectIndex != null && selectIndex != "") {
                    $(this).val(selectIndex);
                }
            })
        })
        
    </script>

</body>
</html>
