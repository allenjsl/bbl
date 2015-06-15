<%@ Page Language="C#" Title="散客确认单" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="VisitorsConfirmPrint.aspx.cs" Inherits="Web.print.wh.VisitorsConfirmPrint" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <style>
        .underlineTextBox
        {
            text-align: left;
        }
    </style>
    <table width="675px" class="table_noneborder" align="center">
        <tr>
            <td colspan="3" align="center">
                <b><font size="4">关于接待<asp:Label ID="lblTourNum" runat="server" Text=""></asp:Label>
                    团队散客的确认单</font> </b>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" class="table_noneborder">
                    <tr>
                        <td width="60px">
                            <b>接收单位</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToCompany" runat="server" CssClass="underlineTextBox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <b>姓名</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToName" runat="server" CssClass="underlineTextBox" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <b>传真</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToFax" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                        <td>
                            <b>电话</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToTel" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>发送人</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromCompany" runat="server" CssClass="underlineTextBox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <b>姓名</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromName" runat="server" CssClass="underlineTextBox" Width="60px"></asp:TextBox>
                        </td>
                        <td>
                            <b>传真</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromFax" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                        <td>
                            <b>电话</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromTel" runat="server" CssClass="underlineTextBox" Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;&nbsp;&nbsp;&nbsp;根据贵社的订购要求，客人参加散拼团的行程及服务标准整理如下，请仔细核对相关项目及费用，如果无误，请盖章确认后，回传至我公司，谢谢您的支持！
            </td>
        </tr>
        <tr>
            <td height="25px" width="300px">
                <b>线路名称:</b>
                <asp:TextBox ID="txtAreaName" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
            <td style="width: 187px">
            </td>
            <td width="150px">
                <b>天 数:</b><asp:TextBox ID="txtDayCount" runat="server" CssClass="underlineTextBox"
                    Width="47px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left">
                <strong>出发交通:</strong>
                <asp:TextBox ID="txtLTraffic" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
            <td colspan="2">
                <strong>返程交通:</strong>
                <asp:TextBox ID="txtRTraffic" runat="server" CssClass="underlineTextBox" Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="left">
                <b>送团方式:</b><asp:TextBox ID="TextBox3" runat="server" CssClass="underlineTextBox"
                    Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <b>接团方式:</b><asp:TextBox ID="TextBox4" runat="server" CssClass="underlineTextBox"
                    Width="235px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td height="25px" width="300px">
                <b>送团人:</b>
                <asp:TextBox ID="txtSendMan" runat="server" CssClass="underlineTextBox" Width="244px"></asp:TextBox>
            </td>
            <td colspan="2">
                <b>送团电话:</b><asp:TextBox ID="txtSendTel" runat="server" CssClass="underlineTextBox"
                    Width="237px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <strong>订购人数:
                    <asp:TextBox ID="txtPeopleCount" runat="server" CssClass="underlineTextBox" Width="85px"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="Placeholder20">
        &nbsp;</div>
    <table width="675px" class="table_normal2" align="center">
        <tr style="border: 1px solid #000;">
            <td width="75px" align="center" rowspan="<%=visitorRowsCount+1 %>">
                <strong>客人信息：</strong>
            </td>
            <asp:Repeater ID="rptCustomerList" runat="server">
                <ItemTemplate>
                    <tr style="border: 1px solid #000;">
                        <td style="width: 25px; text-align: left;">
                            <%#Container.ItemIndex+1%>：
                        </td>
                        <td style="width: 100px;">
                            <%#Eval("VisitorName")%>&nbsp;&nbsp;<%#Eval("VisitorType").ToString()%>/<%#Eval("Sex").ToString()%>
                        </td>
                        <td style="width: 280px;">
                            证件号码：
                            <%#Eval("CradNumber").ToString()%>
                        </td>
                        <td style="">
                            联系方式：
                            <%#Eval("ContactTel")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </table>
    <table width="675px" align="center">
        <tr>
            <td height="50px" valign="middle" align="left">
                <p>
                    <strong>&nbsp;行程安排</strong>
                </p>
            </td>
        </tr>
    </table>
    <asp:Panel ID="tblXCFast" runat="server">
        <table width="675px" align="center" class="table_normal2">
            <tr>
                <td height="50px" valign="middle">
                    <asp:Localize ID="lclXingCheng" runat="server"></asp:Localize>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="tblXingCheng" runat="server">
        <table width="675px" class="table_normal2" align="center">
            <asp:Repeater runat="server" ID="xc_list">
                <ItemTemplate>
                    <tr>
                        <td width="25%" height="25px" valign="middle">
                            <strong>第
                                <%#Container.ItemIndex+1 %>
                                天&nbsp;&nbsp;
                                <%#getDate(Container.ItemIndex)%></strong>
                        </td>
                        <td width="40%" valign="middle">
                            <strong>
                                <%#Eval("Interval")%>-<%#Eval("Vehicle")%></strong>
                        </td>
                        <td width="10%" valign="middle">
                            <strong>住：<%#Eval("Hotel")%></strong>
                        </td>
                        <td width="10%" valign="middle">
                            <strong>餐：<%#getEat(Eval("Dinner").ToString())%></strong>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <%#Eval("Plan")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </asp:Panel>
    <table width="675px" align="center">
        <tr>
            <td height="50px" valign="middle">
                <p>
                    <strong>&nbsp;服务标准及说明 </strong>
                </p>
            </td>
        </tr>
    </table>
    <asp:Panel ID="tblFastService" runat="server">
        <table width="675px" align="center" class="table_normal2">
            <tr>
                <td height="50px" valign="middle">
                    <asp:Localize ID="lclService" runat="server"></asp:Localize>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="tblNoService" runat="server">
        <table width="675px" class="table_normal2" align="center">
            <tbody>
                <tr>
                    <td width="86" align="right">
                        包含项目：
                    </td>
                    <td width="604">
                        <asp:Repeater ID="rptProject" runat="server">
                            <ItemTemplate>
                                <%#Eval("ServiceType").ToString() %>：<%#Eval("Service").ToString() %>
                                <br>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        报价总说明：
                    </td>
                    <td width="604">
                        <strong>成人</strong>：<asp:Label ID="lblManPrice" runat="server" Text=""></asp:Label>
                        元/人*<asp:Label ID="lblManCount" runat="server" Text=""></asp:Label>
                        人+<strong>儿童：</strong><asp:Label ID="lblChildPrice" runat="server" Text=""></asp:Label>
                        元/人*<asp:Label ID="lblChildCount" runat="server" Text=""></asp:Label>
                        人；<strong>合计：</strong><asp:Label ID="lblAllPrice" runat="server" Text=""></asp:Label>
                        元
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        不含项目：
                    </td>
                    <td>
                        <asp:Label ID="lblNoPriject" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        儿童安排：
                    </td>
                    <td>
                        <asp:Label ID="lblChildren" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        购物安排：
                    </td>
                    <td>
                        <asp:Label ID="lblShop" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        自费项目：
                    </td>
                    <td>
                        <asp:Label ID="lblZiFei" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        注意事项：
                    </td>
                    <td>
                        <asp:Label ID="lblZhuyi" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        温馨提醒：
                    </td>
                    <td>
                        <asp:Label ID="lblTiXing" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <table width="660px" class="table_noneborder" align="center">
        <tr>
            <td height="25px">
                <strong>如果您确认无误，请于&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;月&nbsp;&nbsp;&nbsp;&nbsp;日前将钱打入我公司的以下帐号，如果在&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;月&nbsp;&nbsp;&nbsp;&nbsp;日前我公司没有收到汇款，
                从&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;年&nbsp;&nbsp;&nbsp;&nbsp;月&nbsp;&nbsp;&nbsp;&nbsp;日起将每日加      3%的利息:</strong>
            </td>
        </tr>
    </table>
    <table width="675px" class="table_noneborder" align="center">
        
            
                <asp:Label ID="lblCompanyName" runat="server" Text="" visible=false></asp:Label>
            <asp:Label ID="lblBankName" runat="server" Text=""  visible=false></asp:Label>
           <asp:Label ID="lblBankUserName" runat="server" Text="" visible=false></asp:Label>
           <asp:Label ID="lblBankNum" runat="server" Text="" visible=false></asp:Label>
           <tr>
             <td width="200px">公司全称：浙江邮电旅行社有限公司</td>
            <td width="200px">开户行：交通银行杭州城北支行</td>
            <td width="250px"> &nbsp;帐号：331066100018010042682</td>
            </tr>
       
        <tr>
            <td>
                &nbsp;户&nbsp;&nbsp;&nbsp;&nbsp;名：张奔
            </td>
            <td >
            开户行：建设银行 龙卡</td>
            <td>
                &nbsp;帐号：6227 0015 4180 0293 354
                
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;户&nbsp;&nbsp;&nbsp;&nbsp;名：张奔
            </td>
            <td>
            开户行：工商银行 牡丹卡</td>
            <td>
                &nbsp;帐号：622202 1202024799174
                
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;户&nbsp;&nbsp;&nbsp;&nbsp;名：张奔
            </td>
            <td>
            开户行：农业银行 金穗卡</td>
            <td>
                &nbsp;帐号：62284 8032 09784 20414
                
            </td>
        </tr>
    </table>
    <table width="675px" class="table_noneborder" align="center">
        <tr>
            <td width="346">
                组团社盖章：
                <input type="text" size="30" value="" class="underlineTextBox" />
                <br />
                联系电话：
                <input type="text" size="15" value="" class="underlineTextBox" />
                <br />
                确认日期：
                <input type="text" size="15" value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" class="underlineTextBox">
            </td>
            <td width="350">
                专线商盖章：
                <asp:TextBox ID="txtCompany" runat="server" CssClass="underlineTextBox"></asp:TextBox>
                <br>
                联系电话：
                <asp:TextBox ID="txtContactTel" runat="server" CssClass="underlineTextBox"></asp:TextBox>
                <br>
                确认日期：
                <input type="text" size="15" value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" class="underlineTextBox">
            </td>
        </tr>
    </table>
</asp:Content>
