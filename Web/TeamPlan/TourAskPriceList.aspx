<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourAskPriceList.aspx.cs"
    Inherits="Web.TeamPlan.TourAskPriceList" MasterPageFile="~/masterpage/Back.Master"
    Title="团队计划_组团社询价" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">团队计划</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 团队计划>> 组团社询价
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="../images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            团号：</label>
                        <input type="text" name="textfield18" id="txt_teamNum" class="searchinput searchinput03"
                            runat="server" />
                        <label>
                            线路名称：</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_teamName" runat="server" />
                        <label>
                            天数：</label>
                        <input type="text" name="textfield" id="txt_dayCount" class="searchinput searchinput03"
                            runat="server" />
                        <label>
                            预计出团日期：</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_beginDate" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        至
                        <input type="text" name="textfield" id="txt_endDate" class="searchinput" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        <br />
                        <label>
                            询价日期：</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_AskBegin" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        至
                        <input type="text" name="textfield" id="txt_AskEnd" class="searchinput" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        <label>
                            <a href="javascript:void(0);" id="search" onclick="searchQuote()">
                                <img src="../images/searchbtn.gif" style="vertical-align: top;" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:void(0);" onclick="delQuotes()">删 除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr class="odd">
                    <th width="4%" height="28" align="center" nowrap="nowrap">
                        <input type="checkbox" name="checkbox" id="checkAll" />
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        线路名称
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        出团日期
                    </th>
                    <th width="8%" align="center" nowrap="nowrap">
                        人数
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        询价单位
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        询价时间
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        询价人
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        联系电话
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        状态
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rpTour" runat="server">
                    <ItemTemplate>
                        <tr class="even">
                            <td height="28" align="center">
                                <input type="checkbox" name="checkbox" class="check_child" id="child_<%#Eval("ID") %>" /><%# Container.ItemIndex + 1+( this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#Eval("RouteName")%>
                            </td>
                            <td align="center">
                                <%#Convert.ToDateTime(Eval("LeaveDate")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td align="center">
                                <%#Eval("PeopleNum")%>
                            </td>
                            <td align="center">
                                <%#Eval("CustomerName")%>
                            </td>
                            <td align="center">
                                <%#Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactName")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactTel")%>
                            </td>
                            <td align="center">
                                <%#Eval("QuoteState")%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" id="creatQuote" onclick="createQuoteById('<%#Eval("Id") %>')">
                                    报价</a> | <a href="javascript:void(0);" id="delQuote" onclick="delQuoteById('<%#Eval("Id") %>')">
                                        删除</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr class="odd">
                    <th width="4%" height="28" align="center" nowrap="nowrap">
                    合计</th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="8%" align="center" nowrap="nowrap">
                        <asp:Literal ID="peopleCount" runat="server"></asp:Literal>
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                </tr>
                <tr>
                    <td height="30" colspan="9" align="right" class="pageup">
                        <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

    <script type="text/javascript" src="../js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
        //发布事件
        function createQuoteById(id) {
            var url = "/TeamPlan/TourQuotePrice.aspx?type=create&id=" + id;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "报价",
                modal: true,
                width: "850px",
                height: "580px"
            });
            return false;
        }
        //删除事件
        function delQuoteById(id) {
            if (confirm("确定删除么")) {
                $.newAjax({
                    url: "TourQuotePrice.aspx?type=del&ids=" + id + "|",
                    type: "GET",
                    cache: false,
                    success: function(result) {
                        if (result == "True") {
                            alert("删除成功");
                            window.location = location;
                        } else {
                            alert("删除失败")
                        }
                    }
                })
            }

            return false;
        }
        //批量删除事件
        function delQuotes() {
            var id = "";
            var count = 0;

            $(".check_child").each(function() {
                if ($(this).attr("checked") == true) {
                    id += $(this).attr("id").split("_")[1] + "|";
                    count++;
                }
            })

            if (count > 0) {
                if (confirm("确定要删除么")) {
                    $.newAjax({
                        url: "TourQuotePrice.aspx?type=del&ids=" + id,
                        type: "GET",
                        cache: false,
                        success: function(result) {
                            if (result == "True") {
                                alert("删除成功");
                                window.location = location;
                            } else {
                                alert("删除失败")
                            }
                        }
                    })
                }
            } else {
                alert("请至少选择一条数据");
                return false;
            }

        }
        //查询事件
        function searchQuote() {
            if (checkTime()) {
                var teamNum = $("#<%=txt_teamNum.ClientID %>").val();
                var teamName = $("#<%=txt_teamName.ClientID %>").val();
                var begTime = $("#<%=txt_beginDate.ClientID %>").val();
                var endTime = $("#<%=txt_endDate.ClientID %>").val();
                var dayCount = $("#<%=txt_dayCount.ClientID %>").val();
                var askBegin = $("#<%=txt_AskBegin.ClientID %>").val();
                var askEnd = $("#<%=txt_AskEnd.ClientID %>").val();
                var url = "TourAskPriceList.aspx?teamNum=" + teamNum + "&teamName=" + teamName + "&begTime=" + begTime + "&endTime=" + endTime + "&dayCount=" + dayCount + "&askbegin=" + askBegin + "&askend=" + askEnd;
                window.location.href = url;
            } else {
                alert("截止日期不能大于起始日期");
                return;
            }

        }

        //判断时间区间
        function checkTime() {
            var res = true;
            var begTime = $("#<%=txt_beginDate.ClientID %>").val();
            var endTime = $("#<%=txt_endDate.ClientID %>").val();

            if (endTime != "" && begTime != "") {
                if (begTime > endTime) {
                    res = false;
                }
            }
            return res;

        }
        $(function() {
            //全选按钮
            $("#checkAll").click(function() {
                if ($(this).attr("checked") == true) {
                    $(".check_child").attr("checked", true)
                } else {
                    $(".check_child").attr("checked", false)
                }
            })
        })
        
    </script>

</asp:Content>
