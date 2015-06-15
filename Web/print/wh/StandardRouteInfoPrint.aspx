<%@ Page Title="行程单" Language="C#" AutoEventWireup="true" CodeBehind="StandardRouteInfoPrint.aspx.cs"
    Inherits="Web.print.wh.StandardRouteInfoPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="Content1" runat="server">
 <style type="text/css">
     
    </style>
    <table width="759" border="0" align="center" >
        <tr>
            <td height="30">
                <table width="100%" class="table_normal2">
                    <tr>
                        <th height="25"  align="center" colspan="3" >
                            线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td height="25"  align="left" >
                            旅游天数：<asp:Label ID="lblDay" runat="server" Text=""></asp:Label>
                        </td>
                        <td  align="left" colspan="2" > 
                            发布人：
                            <asp:Label ID="lblAuthor" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25"  align="left" >
                            上团数：<asp:Label ID="lblTourCount" runat="server" Text=""></asp:Label>
                        </td>
                        <td  align="left" colspan="2" >
                            收客数 ：<asp:Label ID="lblVisitorCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left" >
                ·行程安排
            </th>
        </tr>
        <tr>
            <td height="30">
                <table width="100%" class="table_normal2" >
                    <tr>
                        <td width="10%" height="25" align="center"  >
                            天数
                        </td>
                        <td width="70%" align="center" >
                            行程描述
                        </td>
                        <td width="10%" align="center" >
                            住宿
                        </td>
                        <td width="10%" align="center" >
                            用餐
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTravel" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" >
                                    第<%#Container.ItemIndex+1%>天 
                                </td>
                                <td align="left" >
                                    <%#Eval("Plan")%>
                                </td>
                                <td align="center"  >
                                    <%#Eval("Hotel")%>
                                </td>
                                <td align="center"  >
                                    <%# GetDinnerValue(Eval("Dinner").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
               ·服务标准
            </th>
        </tr>
        <tr>
            <td height="30" class="normaltd" style="padding:0px;margin:0px">
                <table width="100%" class="table_normal" >
                    <tr>
                        <td width="15%" height="25" align="center" class="normaltd"  >
                            包含项目：
                        </td>
                        <td  style="padding:0px;margin:0px" class="normaltd" >
                            <table class="table_noneborder" style=" text-align: center; width: 100%; margin: -1px -2px -1px -0px;" >
                                <tr>
                                    <td width="8%" height="25" align="left" >
                                        <b>项目</b>
                                    </td>
                                    <td width="82%" align="left" >
                                        <b>接待标准</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptProject" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td width="8%" height="25" align="left" >
                                                <%#Eval("ServiceType")%>
                                            </td>
                                            <td width="82%"  align="left">
                                                <%#Eval("Service")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd">
                            不含项目：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd">
                            购物安排：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd">
                            儿童安排：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd">
                            自费项目：
                        </td>
                        <td align="left" class="normaltd">
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd">
                            注意事项：
                        </td>
                        <td align="left"class="normaltd" >
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left" >
                ·温馨提醒
            </th>
        </tr>
        <tr>
            <td height="30" style="border: 1px #000000 solid;" align="left" >
                <asp:Label ID="lblTips" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
