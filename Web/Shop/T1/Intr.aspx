<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Intr.aspx.cs" Inherits="Web.Shop.T1.Intr" MasterPageFile="~/Shop/T1/Default.Master" Title="公司介绍" %>
<%--公司介绍相关--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Src="~/UserControl/wh/ImagePlayer.ascx" TagName="ImagePlayer" TagPrefix="uc1" %>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">
                <asp:Literal runat="server" ID="ltrTitle"></asp:Literal>
            </span>
        </h3>
        <div class="moduel_wrap1 moduel_wrap1_profile">
            <div class="moduel_wrap1 moduel_wrap2">
                <asp:Literal runat="server" ID="ltrContent"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
