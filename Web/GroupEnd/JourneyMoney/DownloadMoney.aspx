<%@ Page Title="下载报价及行程-组团-杭州望海" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
    CodeBehind="DownloadMoney.aspx.cs" Inherits="Web.GroupEnd.JourneyMoney.DownloadMoney" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td>
                                <span class="lineprotitle">行程报价</span>
                            </td>
                            <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right">
                                所在位置&gt;&gt; <a href="#">行程报价</a>&gt;&gt; 下载报价及行程
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#000000" height="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table class="xtnav" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td align="center">
                                <a href="SelectMoney.aspx">我要询价</a>
                            </td>
                            <td class="xtnav-on" align="center">
                                <a href="#">下载报价及行程</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="btnbox">
            </div>
            <table width="99%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <td width="28%" align="center" bgcolor="#BDDCF4">
                        <strong>标题</strong>
                    </td>
                    <td width="16%" align="center" bgcolor="#BDDCF4">
                        <strong>有效期</strong>
                    </td>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        发布时间
                    </th>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        负责人
                    </th>
                    <td width="10%" align="center" bgcolor="#BDDCF4">
                        <strong>操作</strong>
                    </td>
                </tr>
                <asp:Repeater ID="retList" runat="server">
                    <ItemTemplate>
                        <tr operatorid="<%# Eval("OperatorId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td width="28%" align="center">
                                <%#Eval("FileName") %>
                            </td>
                            <td width="19%" align="center">
                                从<%#Eval("ValidityStart") == null ? "" : Convert.ToDateTime(Eval("ValidityStart")).ToString("yyyy-MM-dd")%>至<%#Eval("ValidityEnd") == null ? "" : Convert.ToDateTime(Eval("ValidityEnd")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td width="10%" align="center">
                                <%#Eval("AddTime") == null ? "" : Convert.ToDateTime(Eval("AddTime")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td width="10%" align="center">
                                <a href="javascript:;" class="principal">
                                    <%#Eval("OperatorName")%></a>
                            </td>
                            <td width="10%" align="center">
                            <%# Eval("FilePath") != "" ? "<a href='" + Eval("FilePath") + "' target='_blank'>下载行程</a>" : "<span>无附件</span>"%>
                       
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td height="30" colspan="5" align="right" valign="middle" bgcolor="#FFFFFF" class="pageup">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>

        <script type="text/javascript">
            function showOperatorName(that) {
                var OperatorID = that.parent().parent().attr("OperatorID");
                var url = "/GroupEnd/JourneyMoney/PrincipalShow.aspx?";

                Boxy.iframeDialog({
                    iframeUrl: url + "OperatorID=" + OperatorID,
                    title: "负责人",
                    modal: true,
                    width: "600",
                    height: "100px"
                });
                return false;
            };
            $(function() {
                $(".principal").click(function() {
                    showOperatorName($(this));
                    return false;
                });
            })
        </script>

    </div>
</asp:Content>
