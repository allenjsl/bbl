<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardRouteInfoPrint.aspx.cs"
    Title="行程单" Inherits="Web.print.bbl.StandardRouteInfoPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="Content1" runat="server">
    <table width="759" border="0" align="center"  class="table_noneborder">
        <tr>
            <td height="30" style=" padding:0; margin:0;" >
                <table width="100%" class="table_normal2">
                    <tr>
                        <th height="25" align="center" colspan="3" >
                            线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td height="25" align="left" >
                            旅游天数：<asp:Label ID="lblDay" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" colspan="2" >
                            发布人：
                            <asp:Label ID="lblAuthor" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="left"  >
                            上团数：<asp:Label ID="lblTourCount" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" colspan="2" >
                            收客数 ：<asp:Label ID="lblVisitorCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left">
                • 行程安排
            </th>
        </tr>
        <tr>
            <td height="30" style=" padding:0; margin:0;">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="10%" height="25" align="center" >
                            天数
                        </td>
                        <td align="center" width="70%" >
                            行程描述
                        </td>
                        <td width="10%" align="center">
                            住宿
                        </td>
                        <td width="10%" align="center" >
                            用餐
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTravel" runat="server">
                        <ItemTemplate>
                            <tr style="width: 100%">
                                <td height="25" align="center" style="width: 10%;" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-3,4,rptTravelTDCount) %>">
                                    第<%#Container.ItemIndex+1%>天
                                </td>
                                <td align="left" style="width: 70%;" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-2,4,rptTravelTDCount) %>">
                                    <%# TextTohtml(Eval("Plan").ToString())%>
                                </td>
                                <td align="center" style="width: 10%;" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-1,4,rptTravelTDCount) %>">
                                    <%#Eval("Hotel")%>
                                </td>
                                <td align="center" style="width: 10%;" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4,4,rptTravelTDCount) %>">
                                    <%# GetDinnerValue(Eval("Dinner").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left" >
                • 服务标准
            </th>
        </tr>
        <tr>
            <td height="30" style=" margin:0; padding:0;border:solid 0px;">
                <table width="100%" border="0" class="table_normal2" >
                    <tr>
                        <td width="15%" height="25" align="center">
                            包含项目：
                        </td>
                        <td style=" padding:0;  margin:0;">
                            <table width="100%" class="table_noneborder">
                                <tr>
                                    <td width="82%" align="center" style="border-right:solid 0px;">
                                        <b>接待标准</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="82%" align="left" style=" border-right:solid 0px; border-bottom:solid 0px">
                                        <asp:Repeater ID="rptProject" runat="server">
                                            <ItemTemplate>
                                                <%#Container.ItemIndex+1 %>. &nbsp;
                                                <%# TextTohtml(Eval("Service").ToString())%><br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            不含项目：
                        </td>
                        <td>
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            购物安排：
                        </td>
                        <td>
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            自费项目：
                        </td>
                        <td align="left">
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            儿童安排：
                        </td>
                        <td>
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            注意事项：
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
                • 温馨提醒
            </th>
        </tr>
        <tr>
            <td height="30">
            <table class="table_normal2" width="100%">
            <tr>
                <td>
                 <asp:Label ID="lblTips" runat="server" Text=""></asp:Label>
                </td>
            </tr>
               
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
