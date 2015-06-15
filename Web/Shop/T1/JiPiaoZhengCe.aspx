<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiPiaoZhengCe.aspx.cs" Inherits="Web.Shop.T1.JiPiaoZhengCe" MasterPageFile="~/Shop/T1/Default.Master" Title="机票政策" %>
<%--机票政策--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">机票政策</span></h3>
        <div class="moduel_wrap1 moduel_wrap3">
            <table bordercolor="#999999" border="0" class="item_list">
                <tbody>
                    <tr class="title">
                        <td width="40">
                            序号
                        </td>
                        <td width="654">
                            标题
                        </td>
                    </tr>
                    <asp:Repeater ID="rpt" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="30" align="center">
                                    <%#(pageIndex - 1) * pageSize + Container.ItemIndex + 1%>
                                </td>
                                <td align="center">
                                    <a href="jipiaozhengcemingxi.aspx?id=<%# Eval("Id")%>"">
                                        <%# EyouSoft.Common.Utils.GetText( Eval("Title").ToString(),42,true)%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div style="text-align: center; margin-top: 10px; margin-bottom: 10px;" runat="server" id="divEmpty" visible="false">
                未找到相关数据。
            </div>
            <div style="text-align: center; margin-top: 10px;" class="digg" runat="server" id="divPaging">
                <div class="diggPage">
                    <cc1:exporpageinfoselect id="paging" runat="server" linktype="3" pagestyletype="NewButton" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
