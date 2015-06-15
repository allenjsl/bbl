<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systems.aspx.cs" Inherits="Web.Webmaster.systems"  MasterPageFile="~/Webmaster/mpage.Master"%>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master"%>

<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
    .tblLB{border-top:1px solid #999;border-left:1px solid #999;width: 100%;margin-bottom: 10px;}
    .tblLB thead{text-align:center;background: #efefef; height:28px;}
    .tblLB tfoot{text-align: center; height: 26px; background: #efefef; height:26px;}
    .tblLB td{border-right:1px solid #999;border-bottom:1px solid #999; line-height:24px; text-align:center;}
    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    子系统管理
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <asp:Repeater runat="server" ID="rptSys" onitemdatabound="rptSys_ItemDataBound">
        <HeaderTemplate>
            <table class="tblLB" cellpadding="0" cellspacing="0">
                <thead>
                    <td>
                        系统编号
                    </td>
                    <td>
                        系统名称
                    </td>
                    <td>
                        系统域名
                    </td>
                    <td>
                        联系人
                    </td>
                    <td>
                        联系电话
                    </td>
                    <td>
                        管理员账号
                    </td>
                    <td>
                        管理员密码
                    </td>
                    <td style="">
                        操作
                    </td>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr onmouseover="changeTrBgColor(this,'#c8dae6')" onmouseout="changeTrBgColor(this,'#ffffff')">
                <td>
                    <%#Eval("SysId") %>
                </td>
                <td>
                    <%#Eval("SysName") %>
                </td>
                <td style="text-align:left;">
                    <asp:Literal runat="server" ID="ltrDomain"></asp:Literal>
                </td>
                <td>
                    <%#Eval("Realname") %>
                </td>
                <td>
                    <%#Eval("Telephone") %>
                </td>
                <td>
                    <%#Eval("AdminUsername") %>
                </td>
                <td>
                    <%#Eval("AdminPassword") %>
                </td>                
                <td>
                    <a href="systemedit.aspx?sysid=<%#Eval("SysId") %>">修改</a>
                </td>
            </tr></ItemTemplate><FooterTemplate>
            <tfoot>
                <td colspan="8">
                    &nbsp;
                </td>
            </tfoot>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <asp:PlaceHolder runat="server" ID="phNotFound" Visible="false">
        <div style="line-height:24px;">
            未找到任何子系统信息.
        </div>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">

</asp:Content>
