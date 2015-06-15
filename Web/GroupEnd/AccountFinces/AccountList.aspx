<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="Web.GroupEnd.AccountFinces.AccountList"
    MasterPageFile="~/masterpage/Front.Master" Title="财务管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc11" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">财务管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                          所在位置>> 财务管理
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" id="TabAccountSearch">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            团号：</label>
                        <input name="Txt_TourCode" type="text" class="searchinput" id="Txt_TourCode" runat="server" />
                        <label>
                            线路名称：</label>
                        <input type="text" name="Txt_LineName" id="Txt_LineName" class="searchinput searchinput02"
                            runat="server" />
                        <label>
                            操作员：</label>
                        <input name="Txt_operator" type="text" class="searchinput" id="Txt_operator" runat="server" />
                        <label>
                            对方销售员：</label>
                        <input type="text" name="Txt_salesperson" id="Txt_salesperson" class="searchinput"
                            runat="server" /><br />
                        <label>
                            状态：</label>
                        <asp:DropDownList ID="Ddl_State" runat="server">
                            <asp:ListItem Text="--请选择状态--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="已结清" Value="1"></asp:ListItem>
                            <asp:ListItem Text="未结清" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <label>
                            <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="查询" id="Btn_Search" /></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        <div class="tablelist" style="border-top: 2px #000000 solid;" id="AccountList">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th height="30" bgcolor="#BDDCF4" align="center" width="5%">
                        序号
                    </th>
                    <th width="7%" align="center" bgcolor="#BDDCF4">
                        团号
                    </th>
                    <th width="14%" align="center" bgcolor="#BDDCF4">
                        线路名称
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        订单号
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        对方计调
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        对方销售员
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        人数
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        总金额
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        已付
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        未付
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        操 作
                    </th>
                </tr>
                <cc1:CustomRepeater ID="ctp_AccountList" runat="server">
                    <ItemTemplate>
                        <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td height="30" align="center">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td align="center">
                                <%# Eval("TourNo")%>
                            </td>
                            <td align="center">
                                <%# Eval("RouteName")%>
                            </td>
                            <td align="center">
                                <%# Eval("OrderNo")%>
                            </td>
                            <td align="center">
                                <%#GetOperatorName(Eval("OperatorList"))%>
                            </td>
                            <td align="center">
                                <%# Eval("SalerName")%>
                            </td>
                            <td align="center" class="talbe_btn">
                                <%# Eval("PeopleNumber")%>
                            </td>
                            <td align="center">
                                <font class="fred">￥<%#FilterEndOfTheZeroDecimal(Eval("SumPrice"))%></font>
                            </td>
                            <td align="center">
                                <font class="fred">￥<%#FilterEndOfTheZeroDecimal(Eval("HasCheckMoney"))%></font>
                            </td>
                            <td align="center">
                                <font class="fred">￥<%#FilterEndOfTheZeroDecimal(Eval("NotReceived"))%></font>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" ref="<%#Eval("ID")%>" result="Show"><font class="fblue">
                                    查看</font></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </cc1:CustomRepeater>
                <tr>
                    <td height="30" colspan="11" align="right" class="pageup">
                             <cc11:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var Account = {
            OnSearch: function() {
                var TourCode = $.trim($("#<%=Txt_TourCode.ClientID %>").val()); //团号
                var LineName = $.trim($("#<%=Txt_LineName.ClientID %>").val()); //线路名称
                var Operator = $.trim($("#<%=Txt_operator.ClientID %>").val()); //操作员
                var Salesperson = $.trim($("#<%=Txt_salesperson.ClientID %>").val()); //对方销售员
                var State = $.trim($("#<%=Ddl_State.ClientID %>").val()); //状态                  
                //参数
                var param = { TourCode: "", LineName: "", Operator: "", Salesperson: "", State: "" };
                param.TourCode = TourCode;
                param.LineName = LineName;
                param.Operator = Operator;
                param.Salesperson = Salesperson;
                param.State = State;
                window.location.href = "/GroupEnd/AccountFinces/AccountList.aspx?" + $.param(param);
            },

            //弹出对话框
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }
        };
        $(document).ready(function() {
            $("#Btn_Search").click(function() {
                Account.OnSearch();
                return false;
            });

            //回车查询
            $("#TabAccountSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    Account.OnSearch();
                    return false;
                }
            });

            $("a[result='Show']").click(function() {
                var Result = $(this).attr("Result");
                var ID = $(this).attr("ref");
                Account.openDialog("/GroupEnd/Orders/OrderEdit.aspx", "订单修改", "900", "560", "OrderID=" + ID);
                return false;
            });
        });
    </script>

    </form>
</asp:Content>
