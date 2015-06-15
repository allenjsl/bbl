<%@ Page Title="留言板" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="MessageBoard.aspx.cs" Inherits="Web.UserCenter.Message.MessageBoard" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
 <div class="mainbody">
     <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">个人中心</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                  所在位置&gt;&gt; 个人中心 &gt;&gt; 留言板
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
    <div id="con_two_1"><form runat="server" onsubmit="return false">
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

        <div class="tablelist">
            <table cellspacing="1" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <th bgcolor="#bddcf4" align="center" width="10%">
                            标题
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            留言时间
                        </th>
                        <th bgcolor="#bddcf4" align="center">
                            留言人
                        </th>
                        <th bgcolor="#bddcf4" width="20%">组团社单位</th>
                        <th bgcolor="#bddcf4" width="12%">联系方式</th>
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
                    <asp:Repeater ID="rptList" runat="server" 
                        onitemdatabound="rptList_ItemDataBound">
                        <ItemTemplate>
                            <%# Container.ItemIndex %2==0?"<tr class=\"even\">":"<tr class=\"odd\">" %> 
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
                                <asp:Literal runat="server" ID="lt_groupName"/>
                                </td>
                                <td align="center">
                                <asp:Literal runat="server" ID="lt_Contract"/>
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
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                            <asp:Label ID="lblMsg" runat="server" Text="未找到数据!"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        </form>
    </div>
</div>
    <script type="text/javascript">
        $(function() {
            $(".btnView").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "留言信息", iframeUrl: "/UserCenter/Message/MsgInfo.aspx?id=" + id, width: "800px", height: "500px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })

            $("#btnSearch").click(function() {
                var title = $("#<%=txtTitle.ClientID %>").val();
                var para = { title: "" }
                para.title = title;
                window.location.href = "/UserCenter/Message/MessageBoard.aspx?" + $.param(para);
                return false;
            });

            $("#<%=txtTitle.ClientID %>").keydown(function(e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                    return false;
                }

            })

        })
    </script>

</asp:Content>
