<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SanFightVisitorsListPrint.aspx.cs" Inherits="Web.print.normal.groupend.SanFightVisitorsListPrint" MasterPageFile="~/masterpage/Print.Master" Title="游客名单" %>
 
 <%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="759px" class="table_normal" align="center">
        <tr height="30">
            <td  class="normaltd" align="center" width="50px;">
            团号：
            </td>
            <td class="normaltd" width="140px;">
            &nbsp;<asp:Literal ID="ltrTourNumber" runat="server"></asp:Literal>
            </td>
            <td class="normaltd" align="center" width="90px;">
            线路名称：
            </td>
            <td class="normaltd" width="240px">
            &nbsp;<asp:Literal ID="ltrRouteName" runat="server"></asp:Literal>
            </td>
            <td  class="normaltd" align="center" width="50px;">
            天数：
            </td>
            <td class="normaltd" width="70px">
            &nbsp;<asp:Literal ID="ltrDays" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    
    <table width="759px" class="table_normal"  align="center" style="margin-top:20px;">
        <tr height="30">
            <td class="normaltd"  align="center" width="120px;">
            姓名
            </td>
            <td  class="normaltd"  align="center" width="80px;">
            性别
            </td>
            <td  class="normaltd" align="center" width="130px;">
            证件类型
            </td>
            <td class="normaltd"  align="center" width="200px">
            证件号码
            </td>
            <td class="normaltd"  align="center" width="110px;">
            联系电话
            </td>
        </tr>
        <cc1:CustomRepeater ID="rptVisitorList" runat="server" EmptyText="<tr height='30'><td colspan='5' class='normaltd'>无游客名单</td></tr>">
            <ItemTemplate>
                <tr height="30">
                    <td align="center" width="120px;">
                        &nbsp;<%#Eval("VisitorName")%>
                    </td>
                    <td align="center" width="80px;">
                        &nbsp;<%#Eval("Sex")%>
                    </td>
                    <td align="center" width="130px;">
                        &nbsp;<%#Eval("CradType")%>
                    </td>
                    <td align="center" width="200px">
                        &nbsp;<%#Eval("CradNumber")%>
                    </td>
                    <td align="center" width="110px;">
                        &nbsp;<%#Eval("ContactTel")%>
                    </td>
                </tr>
            </ItemTemplate>
        </cc1:CustomRepeater>
        
    </table>
    
</asp:Content>