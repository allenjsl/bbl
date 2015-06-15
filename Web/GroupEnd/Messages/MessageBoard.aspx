<%@ Page Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
    CodeBehind="MessageBoard.aspx.cs" Inherits="Web.GroupEnd.Messages.MessageBoard"
    Title="留言板" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc11" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" onsubmit="return false">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">首页</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                       所在位置 >> 首页 >> 留言板
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div id="con_two_1">
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    标题：</label><asp:TextBox ID="txtTitle" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                                <label>
                                    <a href="javascript:void(0);" id="btnSearch">
                                        <img style="vertical-align: top;" src="/images/searchbtn.gif"></a></label></div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td align="center">
                                <a href="/TeamPlan/FastVersion.aspx" id="btnAddMsg">新 增</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                标题
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                留言时间
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                留言人
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                回复状态
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                回复人
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                操作
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <%#Container.ItemIndex % 2 != 0 ? "<tr class=\"odd\">" : "<tr class=\"even\">"%>
                                <td align="center">
                                    <%#Eval("MessageTitle") %>
                                </td>
                                <td align="center">
                                    <%#Eval("MessageTime") == null ? "" : Convert.ToDateTime(Eval("MessageTime")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("MessagePersonName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("ReplyState").ToString()%>
                                </td>
                                <td align="center">
                                    <%#Eval("MessageReplyPersonName")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="btnView" ref="<%#Eval("MessageId") %>">查看</a>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="right"  class="pageup">
                               
                             <cc11:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                                <asp:Label ID="lblMsg" runat="server" Text="未找到数据!"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function() {
            $(".btnView").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "留言信息", iframeUrl: "/GroupEnd/Messages/MsgInfo.aspx?id=" + id, width: "800px", height: "300px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })

            $("#btnSearch").click(function() {
                var title = $("#<%=txtTitle.ClientID %>").val();
                var para = { title: "" }
                para.title = title;
                window.location.href = "/GroupEnd/Messages/MessageBoard.aspx?" + $.param(para);
                return false;
            });

            $("#btnAddMsg").click(function() {
                Boxy.iframeDialog({ title: "留言", iframeUrl: "/GroupEnd/Messages/AddMessage.aspx", width: "800px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })

            $("#<%=txtTitle.ClientID %>").keydown(function(e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                    return false;
                }
               
            })
        })
    </script>

    </form>
</asp:Content>
