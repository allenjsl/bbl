<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="Web.GroupEnd.Orders.OrderList"
    MasterPageFile="~/masterpage/Front.Master" EnableViewState="true" Title="我的订单" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc11" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
    <form id="form1" runat="server">

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">线路列表</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置 >> 我的订单
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" id="LineListSearch">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            订单号：</label>
                        <input type="text" name="TxtOrderID" id="TxtOrderID" class="searchinput" runat="server" />
                        <label>
                            线路名称：</label>
                        <input type="text" name="xl_XianlName" id="xl_XianlName" class="searchinput searchinput02"
                            runat="server" />
                        <label>
                            下单时间：</label><input type="text" id="inputOrderStartDate" class="searchinput" name="textfield"
                                onfocus="WdatePicker()" runat="server">
                        至
                        <input type="text" id="inputOrderEndDate" class="searchinput" name="textfield" onfocus="WdatePicker()"
                            runat="server">
                        <label>
                            出团日期：</label>
                        <input name="BeginTime" type="text" class="searchinput" id="BeginTime" runat="server"
                            onfocus="WdatePicker();" />
                        至
                        <input type="text" name="EndTime" id="EndTime" class="searchinput" runat="server"
                            onfocus="WdatePicker();" />
                        <label>
                            <a id="Btn_Serach" href="javascript:void(0);">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        <div class="tablelist" style="border-top: 2px #000000 solid;" id="OrderList">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th height="30" bgcolor="#BDDCF4" align="center" width="4%">
                        序号
                    </th>
                    <th width="8%" align="center" bgcolor="#BDDCF4">
                        订单号
                    </th>
                    <th width="20%" align="center" bgcolor="#BDDCF4">
                        线路名称
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        下单时间
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        出团日期
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        价格
                    </th>
                    <th width="12%" align="center" bgcolor="#bddcf4">
                        联系人
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        人数
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        订单状态
                    </th>
                    <%--<th width="7%" align="center" bgcolor="#bddcf4">
                        地接社信息
                    </th>--%>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <cc1:CustomRepeater ID="rep_List" runat="server" OnItemCommand="rep_List_ItemCommand"
                    OnItemDataBound="rep_List_ItemDataBound">
                    <ItemTemplate>
                        <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td height="30" align="center">
                                <%#Container.ItemIndex+1 %>
                            </td>
                            <td align="center">
                                <%# Eval("OrderNo")%>
                            </td>
                            <td align="center">
                                <a href="javascript:();" onclick="ShowRouteName(this)" tourid="<%#Eval("TourId")%>">
                                    <%# Eval("RouteName")%></a>
                            </td>
                            <td align="center">
                                <%# Eval("IssueTime", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td align="center">
                                <%# Eval("LeaveDate", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td align="center">
                                成人价:<font class="fred">￥<%#FilterEndOfTheZeroDecimal(Eval("PersonalPrice"))%></font><br />
                                儿童价:<font class="fred">￥<%#FilterEndOfTheZeroDecimal(Eval("ChildPrice"))%></font>
                            </td>
                            <td align="center">
                                <%--<a href="Javascript:viod(0);" ref="<%#Eval("TourId")%>" result="DiJie">
                                    <%#FormatContactInfo(Eval("OperatorList"))%><br />
                                </a>--%>
                                <%#FormatContactInfo(Eval("OperatorList"))%>
                            </td>
                            <td align="center" class="talbe_btn">
                                <%#Eval("PeopleNumber")%>
                            </td>
                            <td align="center" class="talbe_btn">
                                <%# Eval("OrderState").ToString() == "不受理" ? "已取消" : Eval("OrderState")%>
                            </td>
                            <%--<td align="center">
                                <a href="Javascript:viod(0);" ref="<%#Eval("TourId")%>" result="DiJie">地接社信息</a>
                            </td>--%>
                            <td align="center">
                            <%# (int)Eval("OrderState") == 2 ? "留位到:" + Convert.ToDateTime(Eval("SaveSeatDate")).ToString("MM-dd HH:mm") + "<br/>" : ""%>
                                <%# (int)Eval("OrderState") == 3 ? "<a ref='" + Eval("ID") + "' href='Javascript:viod(0);' class='continue'>继续留位</a><br />" : ""%>
                                <a href="Javascript:viod(0);" ref="<%#Eval("ID")%>" result="Edit"><font class="fblue">
                                    查看</font></a>
                                <asp:HyperLink ID="hyMinDan" runat="server" Target="_blank">名单</asp:HyperLink>
                                <asp:LinkButton ID="lbtnCancel" runat="server" Visible="false" CommandName="Del"
                                    OnClientClick="return confirm('确认删除该订单!')">取消</asp:LinkButton>
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

    <script type="text/javascript" language="javascript">
        function ShowRouteName(thiss) {
            var tourid = $(thiss).attr("tourid");

            var url = "/GroupEnd/Orders/Journey.aspx?";
            Boxy.iframeDialog({
                iframeUrl: url + "tourid=" + tourid,
                title: "查看行程安排",
                modal: true,
                width: "650px",
                height: "300px"
            });
            return false;
        }
        var OrderList = {
            OnSearch: function() {
                //订单编号
                var OrderID = $.trim($("#<%=TxtOrderID.ClientID %>").val());
                //线路名称
                var LineName = $.trim($("#<%=xl_XianlName.ClientID %>").val());
                //出团开始时间
                var BeginTime = $.trim($("#<%=BeginTime.ClientID%>").val());
                //出团结束时间
                var EndTime = $.trim($("#<%=EndTime.ClientID%>").val());
                //下单开始时间
                var OrderStartDate = $.trim($("#<%=inputOrderStartDate.ClientID%>").val());
                //下单结束时间
                var OrderEndDate = $.trim($("#<%=inputOrderEndDate.ClientID%>").val());
                var Params = { OrderID: "", LineName: "", BeginTime: "", EndTime: "", OrderStartDate: "", OrderEndDate: "" };
                Params.OrderID = OrderID;
                Params.LineName = LineName;
                Params.BeginTime = BeginTime;
                Params.EndTime = EndTime;
                Params.OrderStartDate = OrderStartDate;
                Params.OrderEndDate = OrderEndDate;
                window.location.href = "/GroupEnd/Orders/OrderList.aspx?" + $.param(Params);
            },
            //弹出对话框
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }
        };
        $(document).ready(function() {
            //查询按钮
            $("#Btn_Serach").click(function() {
                OrderList.OnSearch();
                return false;
            });
            //回车查询
            $("#LineListSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    OrderList.OnSearch();
                    return false;
                }
            });
            $("a.continue").click(function() {
                var ID = $(this).attr("ref");
                window.location.href = "/GroupEnd/Orders/OrderList.aspx?act=updateContinue&ID=" + ID;
                return false;

            });
            $("#OrderList").find("a").click(function() {
                var Result = $(this).attr("Result");
                switch (Result) {
                    case 'DiJie':
                        {
                            var ID = $(this).attr("ref");
                            OrderList.openDialog("/GroupEnd/Orders/GroundOperators.aspx", "联系人信息", "500", "200", "TourID=" + ID);
                            return false;
                            break;
                        }
                    case 'Edit':
                        {
                            var ID = $(this).attr("ref");
                            OrderList.openDialog("/GroupEnd/Orders/OrderEdit.aspx", "订单修改", "900", "560", "OrderID=" + ID);
                            return false;
                            break;
                        }
                }
            });
        });
    </script>

    </form>
</asp:Content>
