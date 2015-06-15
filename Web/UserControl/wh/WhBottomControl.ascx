<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WhBottomControl.ascx.cs"
    Inherits="Web.UserControl.wh.WhBottomControl" %>
<!--linksStart-->
<div class="links">
    <div class="moduel_wrap1 moduel_wrap1_links">
        <p class="linksTitle">
            友情连接</p>
        <p class="linkList">
            <asp:Repeater ID="rptLinkList" runat="server">
                <ItemTemplate>
                    <a href="<%#Eval("LinkUrl") %>" title="<%# Eval("LinkName") %>"  target="_blank" >
                        <%# EyouSoft.Common.Utils.GetText( Eval("LinkName").ToString(),10,false)%></a>
                </ItemTemplate>
            </asp:Repeater>
        </p>
    </div>
</div>
<!--linksEnd-->
<div class="footer">
    <asp:Localize ID="lclFooter" runat="server"></asp:Localize>
</div>
