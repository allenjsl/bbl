<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chutuantongzhiD.aspx.cs"
    Inherits="Web.print.hkmj.chutuantongzhiD" MasterPageFile="~/masterpage/Print.Master" Title="出团通知单" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="print1" runat="server">
    <table width="760" border="0" align="center" cellpadding="0" cellspacing="0" class="table_noneborder">
        <tr>
            <td >
                <table width="99.9%" border="0" align="center" cellpadding="0" cellspacing="1" class="table_normal2">
                    <tr>
                        <th height="25" colspan="4" align="center" >
                            出团通知：<asp:Literal ID="litRouteName" runat="server"></asp:Literal>
                        </th>
                    </tr>
                    <tr>
                        <td width="20%" align="right">
                            客户名称：
                        </td>
                        <td width="41%" height="25" align="left" >
                            <asp:Literal ID="litBuyCompanyName" runat="server"></asp:Literal>
                        </td>
                        <td width="12%" align="right" >
                            联系人：
                        </td>
                        <td width="27%" align="left" >
                            <asp:Literal ID="litCustomName" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            电话：
                        </td>
                        <td height="25" align="left" >
                            <asp:Literal ID="litTel" runat="server"></asp:Literal>
                        </td>
                        <td align="right" bgcolor="#FFFFFF">
                            传真：
                        </td>
                        <td align="left" >
                            <asp:Literal ID="litFax" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="right" >
                            客人名单（请仔细核对）：
                        </td>
                        <td height="25" colspan="3" align="left" bgcolor="#FFFFFF">
                            <asp:Literal ID="litVisitorsList" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            机场送团：
                        </td>
                        <td height="25" align="left" >
                            <asp:Literal ID="litSongTuanName" runat="server"></asp:Literal>
                        </td>
                        <td align="right" >
                            手机：
                        </td>
                        <td align="left" >
                            <asp:Literal ID="litSongTuanPhone" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            去程航班/时间：
                        </td>
                        <td height="25" align="left" bgcolor="#FFFFFF" colspan="3">
                            <asp:Literal ID="litLFilght" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            返程航班/时间：
                        </td>
                        <td height="25" align="left" bgcolor="#FFFFFF" colspan="3">
                            <asp:Literal ID="litRFilght" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="right">
                            集合地点：
                        </td>
                        <td height="25" colspan="3" align="left" >
                            <asp:Literal ID="litJihePlace" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            抵达后：
                        </td>
                        <td height="25" colspan="3" align="left" bgcolor="#FFFFFF">
                            <p>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left">
                •行程安排
            </th>
        </tr>
        <tr>
            <td height="30" id="tabView" runat="server">
            <asp:PlaceHolder ID="pnetab" runat="server">
                <table  width="100%" border="0" align="center" cellpadding="0"
                    cellspacing="1" class="table_normal2">
                    <tr>
                        <td width="11%" height="25" align="center" bgcolor="#FFFFFF">
                            日期
                        </td>
                        <td width="59%" align="center" bgcolor="#FFFFFF">
                            行程&nbsp;主要含景点
                        </td>
                        <td width="11%" align="center" bgcolor="#FFFFFF">
                            用餐
                        </td>
                        <td width="19%" align="center" bgcolor="#FFFFFF">
                            住宿
                        </td>
                    </tr>
                   <%=TravelplanStr %>
                </table>
                </asp:PlaceHolder>
                <asp:Literal ID="litQplan" runat="server"></asp:Literal>
            </td>
        </tr>
        <asp:PlaceHolder ID="pneStand" runat="server">
            <tr>
                <th height="30" align="left">
                    • 注意事项
                </th>
            </tr>
            <tr>
                <td style="border: 1px #000000 solid;">
                    <asp:Literal ID="litZhuYiShiXiang" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <th height="30" align="left">
                    • 温馨提醒
                </th>
            </tr>
            <tr>
                <td style="border: 1px #000000 solid;">
                    <asp:Literal ID="litWenXinTiXing" runat="server"></asp:Literal>
                </td>
            </tr>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pnequick" runat="server">
            <tr>
                <th height="30" align="left">
                    • 服务标准
                </th>
            </tr>
            <tr>
                <td style="border: 1px #000000 solid;">
                    <asp:Literal ID="litServerStand" runat="server"></asp:Literal>
                </td>
            </tr>
        </asp:PlaceHolder>
    </table>
</asp:Content>
