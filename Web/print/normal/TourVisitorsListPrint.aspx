<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourVisitorsListPrint.aspx.cs" Inherits="Web.print.normal.TourVisitorsListPrint" Title="游客名单" MasterPageFile="~/masterpage/Print.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal2" width="750px" align="center">
        <tr height="30">
            <td align="center" width="68px;">
                团号：
            </td>
            <td width="158px;">
                &nbsp;<asp:Literal ID="ltrTourNumber" runat="server"></asp:Literal>
            </td>
            <td align="center" width="108px;">
                线路名称：
            </td>
            <td width="260px">
                &nbsp;<asp:Literal ID="ltrRouteName" runat="server"></asp:Literal>
            </td>
            <td align="center" width="68px;">
                天数：
            </td>
            <td width="88px">
                &nbsp;<asp:Literal ID="ltrDays" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0 0 0;">
        &nbsp;&nbsp;</div>
    <table class="table_normal2" width="750px" align="center">
        <tr height="30">
            <td style="width: 5%" align="center">
                序号
            </td>
            <td align="center" width="142px;">
                姓名
            </td>
            <td align="center" width="102px;">
                性别
            </td>
            <td align="center" width="152px;">
                证件类型
            </td>
            <td align="center" width="222px">
                证件号码
            </td>
            <td align="center" width="132px;">
                联系电话
            </td>
        </tr>
        <cc1:CustomRepeater ID="rptVisitorList" runat="server" EmptyText="<tr height='30'><td colspan='5'>无游客名单</td></tr>">
            <ItemTemplate>
                <tr height="30">
                    <td align="center">
                        <%#Container.ItemIndex+1%>
                    </td>
                    <td align="center" width="142px;">
                        &nbsp;<%#Eval("VisitorName")%>
                    </td>
                    <td align="center" width="102px;">
                        &nbsp;<%#Eval("Sex")%>
                    </td>
                    <td align="center" width="152px;">
                        &nbsp;<%#Eval("CradType")%>
                    </td>
                    <td align="center" width="222px">
                        &nbsp;<%#Eval("CradNumber")%>
                    </td>
                    <td align="center" width="132px;">
                        &nbsp;<%#Eval("ContactTel")%>
                    </td>
                </tr>
            </ItemTemplate>
        </cc1:CustomRepeater>
    </table>
</asp:Content>

