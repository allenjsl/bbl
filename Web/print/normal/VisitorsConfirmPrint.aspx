<%@ Page Language="C#" Title="散客确认单" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="VisitorsConfirmPrint.aspx.cs" Inherits="Web.print.normal.VisitorsConfirmPrint" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ID="c1" ContentPlaceHolderID="PrintC1" runat="server">
    <style>
        #divContent input
        {
            border: none;
            border-bottom: 1px solid #000;
        }
        p.MsoNormal
        {
            margin-bottom: .0001pt;
            text-align: justify;
            text-justify: inter-ideograph;
            font-size: 10.5pt;
            font-family: "Times New Roman";
            margin-left: 0cm;
            margin-right: 0cm;
            margin-top: 0cm;
        }
        .style1
        {
            border-collapse: collapse;
            font-size: 10.0pt;
            font-family: "Times New Roman";
            border-style: none;
            border-color: inherit;
            border-width: medium;
        }
    </style>
    <table width="760px" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30" colspan="2" align="center">
                <span style="font-size: 20px; font-weight: bold;">                    
                    散 客 确 认 单</span>
            </td>
        </tr>
        <tr>
            <th width="450px" height="30" align="left" valign="middle">
                甲方（组团社）：<input type="text" name="txt_TeamName" id="txt_TeamName" runat="server" />
            </th>
            <th width="310px" align="left">
                乙 方：<input type="text" />
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                联 系 人：
                <input type="text" name="txt_lxr" id="txt_lxr" runat="server" class="underlineTextBox" />
            </th>
            <th align="left">
                联 系 人：
                <input type="text" name="txt_lxr2" id="txt_lxr2" runat="server" />
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                联系电话：
                <input type="text" name="txt_tel" id="txt_tel" runat="server" />
            </th>
            <th align="left">
                联系电话：
                <input type="text" name="txt_tel2" id="txt_tel2" runat="server" />
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                传 &nbsp;&nbsp; 真：
                <input type="text" name="txt_fax" id="txt_fax" runat="server" />
            </th>
            <th align="left">
                传 &nbsp;&nbsp; 真：
                <input name="txt_fax2" type="text" id="txt_fax2" runat="server" />
            </th>
        </tr>
        <tr>
            <th height="30" colspan="2" align="left" valign="middle" style="text-indent: 24px;">
                您好，首先感谢您对我公司工作的支持与信任, 我们将以“优良的信誉、精心的安排、丰富的内容、合理的价格、周到的服务”，为新老客户提供高品质服务！
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                出团日期：<input type="text" name="txt_Ldate" id="txt_Ldate" runat="server" />
            </th>
            <th align="left">
                人数：
                <input type="text" name="txt_PepoleNum" id="txt_PepoleNum" runat="server" />
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                线路名称：
                <input type="text" name="txt_RouteName" id="txt_RouteName" runat="server" style="width:370px" />
            </th>
            <th align="left">
                （具体行程及接待标准请参见出团通知）
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                结算价格：
                <input type="text" name="txt_Price" id="txt_Price" runat="server" />
            </th>
            <th align="left">
                大写：
                <input type="text" name="txt_large" id="txt_large" runat="server" />
            </th>
        </tr>
        <tr>
            <th height="30" colspan="2" align="left" valign="middle" style="text-indent: 24px;">
                游客名单/身份证号码：（请仔细核对参团客人名单，身份证是否过期（16周岁以上含16周岁必需拿身份证登机），确认无误后回传，以便我社出票，如因名字错误产生损失后果自负！）
            </th>
        </tr>
        <tr>
            <asp:Repeater runat="server" ID="cus_list">
                <ItemTemplate>
                    <th height="30" align="left" valign="middle">
                        <%=ci %>、
                        <input type="text" name="textfield8" id="textfield8" value="<%#Eval("VisitorName")%> <%#Eval("CradNumber")%>" />
                    </th>
                    <%if (ci % 2 == 0)
                      { %>
                    </tr><tr>
                        <%} %>
                        <%ci++; %>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
        <tr>
            <th height="35" align="left" valign="middle">
                游客联系方式：
                <input type="text" name="txt_first" id="txt_first" runat="server" />
            </th>
            <th align="left">
                &nbsp;
            </th>
        </tr>
        <tr>
            <th height="30" colspan="2" align="left" valign="middle">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="2%" rowspan="4" align="center">
                            出团时间
                        </td>
                        <td width="49%" height="30">
                            &nbsp;集合时间：
                            <input type="text" name="txt_jhDate" id="txt_jhDate" runat="server" />
                        </td>
                        <td width="49%">
                            集合地点：
                            <input name="txt_jhdd" runat="server" type="text" id="txt_jhdd" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30">
                            &nbsp;送 团 人：
                            <input type="text" name="txt_str" id="txt_str" runat="server" />
                        </td>
                        <td height="30">
                            集合标志：
                            <input type="text" name="txt_jhBZ" id="txt_jhBZ" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30">
                            &nbsp;去程航班：
                            <input type="text" name="txt_QCHB" id="txt_QCHB" runat="server" />
                        </td>
                        <td height="30">
                            回程航班：
                            <input type="text" name="txt_HCHB" id="txt_HCHB" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="2" align="center">
                            航班仅供参考，以具体出票时间为准！如有变动，我社会提前通知！
                        </td>
                    </tr>
                </table>
            </th>
        </tr>
        <tr>
            <th height="35" colspan="2" align="left" valign="middle">
                结 款 方 式：
                <input type="text" name="textfield8" id="textfield8" />
                （如汇款请将底单传真我社!谢谢合作!）
            </th>
        </tr>        
        <tr>
            <td height="30" colspan="2" align="left" valign="middle">               
                <table class="table_t_border" width="100%" style="margin-top: 5px;">
                    <tr>
                        <td>
                            <b>
                                <asp:Literal ID="lt_routName" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div runat="server" id="xc_quick" visible="false">
                                <table class="table_normal2" width="100%">
                                    <tbody>
                                        <tr>
                                            <td height="20" bgcolor="#ffffff" align="left">
                                                <b>· 行程安排</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td bgcolor="#ffffff" align="left">
                                                <asp:Literal ID="litTravel" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div runat="server" id="xc_Normarl" visible="false">
                                <table width="100%" class="table_normal2">
                                    <tr>
                                        <td width="10%" height="25" align="center">
                                            日期
                                        </td>
                                        <td width="76%" align="center">
                                            行程
                                        </td>
                                        <td width="6%" align="center">
                                            住宿
                                        </td>
                                        <td width="8%" align="center">
                                            用餐
                                        </td>
                                    </tr>
                                    <asp:Repeater ID="rptTravel" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td height="25" align="center">
                                                    <%#GetDateByIndex(Container.ItemIndex)%>
                                                </td>
                                                <td align="left">
                                                    <%#Eval("Plan")%>
                                                </td>
                                                <td align="center">
                                                    <%#Eval("Hotel")%>
                                                </td>
                                                <td align="center">
                                                    <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <b><span>报价</span></b> <span>：成人</span><span><input type="text" name="textfield8"
                    id="text1" />
                </span><span>元，儿童</span> <u><span>
                    <input type="text" name="textfield8" id="text2" />
                </span></u><span>元</span><span>/人</span><span>（儿童价格指12周岁以下，不占床,不含景点门票）</span>
                <p class="MsoNormal">
                    <b><span>报价已含费用及服务标准</span></b><span>：</span></p>
                <br />
                <table width="100%" id="tb_service_quick" visible="false" runat="server" class="table_normal2">
                    <tr>
                        <td height="30" style="border: 1px solid rgb(0, 0, 0);">
                            <asp:Literal ID="litService" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table width="100%" id="tb_service_normal" visible="false" runat="server" class="table_normal">
                    <tr>
                        <td width="15%" height="25" class="normaltd" align="center">
                            包含项目：
                        </td>
                        <td class="normaltd" style="margin: 0; padding: 0">
                            <table width="100%" class="table_noneborder">
                                <tr>
                                    <td align="left">
                                        <b>接待标准</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptProject" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left">
                                                <%#Eval("Service")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" class="normaltd" align="center">
                            不含项目：
                        </td>
                        <td class="normaltd">
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd">
                            购物安排：
                        </td>
                        <td class="normaltd">
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd">
                            自费项目：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" class="normaltd" align="center">
                            儿童安排：
                        </td>
                        <td class="normaltd">
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" class="normaltd" align="center">
                            注意事项：
                        </td>
                        <td class="normaltd" align="left">
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <p class="MsoNormal" style="line-height: 15.0pt;">
                    <b><span>特别说明</span></b><span>：</span><span></span></p>
                <p class="MsoNormal">
                    <asp:Label ID="lblTips" runat="server" Text=""></asp:Label>
                </p>
            </td>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                甲方（组团社）：<input type="text" name="txt_jiafan" id="txt_jiafan" runat="server" />
            </th>
            <th height="30" align="left" valign="middle">
                乙方：<input type="text" />
            </th>
        </tr>
        <tr class="PrintText">
            <th height="30" align="left" valign="middle">
                经办人：<input type="text" id="txtJbr" runat="server" class="underlineTextBox" />
            </th>
            <th height="30" align="left" valign="middle">
                经办人：<input type="text" id="txtyfJbr" runat="server" class="underlineTextBox" />
            </th>
        </tr>
        <tr class="PrintText">
            <th height="30" align="left" valign="middle">
                日期：<input type="text" name="textfield8" />
            </th>
            <th height="30" align="left" valign="middle">
                日期：<input type="text" name="textfield8" />
            </th>
        </tr>
        <tr>
            <th height="30" align="left" valign="middle">
                盖章：
            </th>
            <th height="30" align="left" valign="middle">
                盖章：
            </th>
        </tr>
        <tr>
            <th height="20" colspan="2" align="left" valign="middle">
                &nbsp;
            </th>
        </tr>
    </table>
</asp:Content>
