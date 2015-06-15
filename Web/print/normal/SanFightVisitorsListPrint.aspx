<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SanFightVisitorsListPrint.aspx.cs" Inherits="Web.print.normal.SanFightVisitorsListPrint" MasterPageFile="~/masterpage/Print.Master" Title="游客名单" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal" width="750" align="center">
        <tr height="30">
            <td align="center" width="50px;" class="normaltd">
                团号：
            </td>
            <td width="140px;" class="normaltd">
                &nbsp;<asp:Literal ID="ltrTourNumber" runat="server"></asp:Literal>
            </td>
            <td align="center" width="90px;" class="normaltd">
                线路名称：
            </td>
            <td width="240px" class="normaltd">
                &nbsp;<asp:Literal ID="ltrRouteName" runat="server"></asp:Literal>
            </td>
            <td align="center" width="50px;" class="normaltd">
                天数：
            </td>
            <td width="70px" class="normaltd">
                &nbsp;<asp:Literal ID="ltrDays" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    <div class="Placeholder10">
        &nbsp;</div>
    <table class="table_normal2" align="center" width="750">
        <tr height="30">
            <td style="width: 5%" class="normaltd" align="center">
                序号
            </td>
            <td align="center" class="normaltd" width="120px;">
                姓名
            </td>
            <td align="center" class="normaltd" width="80px;">
                性别
            </td>
            <td align="center" class="normaltd" width="130px;">
                证件类型
            </td>
            <td align="center" class="normaltd" width="200px">
                证件号码
            </td>
            <td align="center" class="normaltd" width="110px;">
                联系电话
            </td>
        </tr>
        <cc1:CustomRepeater ID="rptVisitorList" runat="server" EmptyText="<tr height='30'><td colspan='5'>无游客名单</td></tr>">
            <ItemTemplate>
                <tr height="30">
                    <td>
                        <%#Container.ItemIndex+1%>
                    </td>
                    <td align="center" width="120px;" class="normaltd">
                        &nbsp;<%#Eval("VisitorName")%>
                    </td>
                    <td align="center" width="80px;" class="normaltd">
                        &nbsp;<%#Eval("Sex")%>
                    </td>
                    <td align="center" width="130px;" class="normaltd">
                        &nbsp;<%#Eval("CradType")%>
                    </td>
                    <td align="center" width="200px" class="normaltd">
                        &nbsp;<%#Eval("CradNumber")%>
                    </td>
                    <td align="center" width="110px;" class="normaltd">
                        &nbsp;<%#Eval("ContactTel")%>
                    </td>
                </tr>
            </ItemTemplate>
        </cc1:CustomRepeater>
    </table>
</asp:Content>
