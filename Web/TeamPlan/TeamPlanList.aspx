<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamPlanList.aspx.cs" Inherits="Web.TeamPlan.TeamPlanList"
    MasterPageFile="~/masterpage/Back.Master" Title="团队计划_计划列表" %>

<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">团队计划</span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt; 团队计划&gt;&gt; 团队计划
                        </td>
                    </tr>
                    <tr>
                        <td height="2px" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif">
                    </td>
                    <td>
                        <div class="searchbox" runat="server" id="searchDiv">
                            <label>
                                团号：</label>
                            <asp:TextBox ID="txtTeamNumber" runat="server" CssClass="searchinput searchinput03"
                                Width="80px"></asp:TextBox>
                            <label>
                                团队名称：</label>
                            <asp:TextBox ID="txtTeamName" runat="server" CssClass="searchinput"></asp:TextBox>
                            <label>
                                天数：</label>
                            <asp:TextBox ID="txtDayCount" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                            <label>
                                出团日期：</label>
                            <asp:TextBox ID="txtBeginDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                            至
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox><br />
                            <uc1:selectOperator ID="selectOperator1" Title="业务员" TextClass="searchinput" runat="server"
                                IsShowLabel="true" />&nbsp
                            <uc1:selectOperator ID="selectOperator2" Title="计调员" TextClass="searchinput" runat="server"
                                IsShowLabel="true" />
                            团队状态：<select id="txtTourStatus">
                                <option value="-1">请选择</option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.正在收客%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.正在收客%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.停止收客%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.停止收客%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.行程途中%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.行程途中%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.回团报账%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.回团报账%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束%></option>
                            </select>
                            <label>
                                游客姓名：</label>
                            <asp:TextBox ID="txt_Name" class="searchinput searchinput03" runat="server" Width="80px"></asp:TextBox>
                            <label>
                                <a href="javascript:void(0);" id="btnSearch">
                                    <img style="vertical-align: top;" src="../images/searchbtn.gif" border="0" /></a></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif">
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btnbox">
            <table cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlAdd" runat="server">
                                <a href="/TeamPlan/FastVersion.aspx">新 增</a>
                            </asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="pnlUpdate" runat="server">
                                <a href="javascript:void(0);" id="btnUpdate">修 改</a></asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="pnlCopy" runat="server">
                                <a href="javascript:void(0);" id="btnCopy">复 制</a></asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px;">
                            <asp:Panel ID="penDelete" runat="server">
                                <a href="javascript:void(0);" id="btnDelete">删 除</a>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table cellspacing="1" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr class="odd">
                        <th nowrap="nowrap" align="center">
                            序号
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            团号
                        </th>
                        <th nowrap="nowrap" align="center" width="10%">
                            线路名称
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            出团日期<br />
                            回团日期
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            组团社
                        </th>
                        <th nowrap="nowrap" align="center" width="7%">
                            联系人
                        </th>
                        <th nowrap="nowrap" align="center" width="9%">
                            电话
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            天数
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            人数
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            总团款
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            状态
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            计调
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr class='<%#Container.ItemIndex %2==0?"even":"odd" %>'>
                                <td align="center">
                                    <input type="checkbox" id="checkbox" name="checkbox" ref='<%#Eval("TourId") %>' reltype='<%#Eval("ReleaseType").ToString() %>'
                                        tournum="<%#Eval("TourCode") %>" state="<%#Eval("Status").ToString() %>"><%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                                </td>
                                <td align="center">
                                    <%#Eval("TourCode") %>
                                </td>
                                <td align="center">
                                    <%#GetPrintUrl(Eval("ReleaseType").ToString(),Eval("TourId").ToString())%>
                                    <%#Eval("RouteName")%>
                                    </a>
                                </td>
                                <td align="center">
                                    <%#Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd") %>
                                    <br />
                                    <%#Convert.ToDateTime(Eval("LDate")).AddDays(Convert.ToInt32(Eval("TourDays"))-1).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyerCId") %>">
                                        <%#Eval("BuyerCName")%></a>
                                </td>
                                <td align="center">
                                    <%#Eval("BuyerContacterName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("BuyerContacterTelephone")%>
                                </td>
                                <td align="center">
                                    <%#Eval("TourDays")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="AddPeople" ref="<%#Eval("OrderId") %>" rex="<%#Eval("TourId") %>" ticketType="<%# (int)Eval("TicketStatus") %>">
                                        <%#Eval("PlanPeopleNumber")%></a>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()).ToString("0.00"))%>
                                </td>
                                <td align="center">
                                    <%#GetStatusByTicket(Eval("Status").ToString(), Eval("TicketStatus").ToString())%>
                                </td>
                                <td align="center">
                                    <%if (CheckGrant(Common.Enum.TravelPermission.团队计划_团队计划_安排地接))
                                      { %>
                                    <a class="btnDiJie" href="javascript:void(0);" ref="<%#Eval("TourId") %>" ret="<%#Eval("ReleaseType").ToString()%>">
                                        <font class="fblue">安排地接</font></a><br>
                                    <%} %>
                                    <%--<%if (CheckGrant(Common.Enum.TravelPermission.团队计划_团队计划_申请机票))
                                      { %>
                                    <a class="btnJiPiao" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font
                                        class="fblue">机票申请</font></a>
                                    <%} %>--%>
                                </td>
                                <td align="center">
                                    <a class="btnDaYin" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font class="fblue">
                                        单据打印</font></a><br>
                                    <%if (CheckGrant(Common.Enum.TravelPermission.团队计划_团队计划_团队结算))
                                      { %>
                                    <a class="btnJieSuan" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font
                                        class="fblue">团队结算</font></a>
                                    <%} %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <th nowrap="nowrap" align="center" width="3%">
                            合计
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="10%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            <br />
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="7%">
                        </th>
                        <th nowrap="nowrap" align="center" width="9%">
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            <asp:Literal ID="lt_peopleNum" runat="server"></asp:Literal>
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            <asp:Literal ID="lt_paraSum" runat="server"></asp:Literal>
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                    </tr>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="13">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                            <asp:Label ID="lblMsg" runat="server" Text="未找到数据!"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //查询按钮事件
            $("#btnSearch").click(function() {
                //团号
                var teamNumber = $.trim($("#<%=txtTeamNumber.ClientID %>").val());
                //团队名称
                var teamName = $.trim($("#<%=txtTeamName.ClientID %>").val());
                //游客姓名                
                var orderName = $.trim($("#<%=txt_Name.ClientID %>").val());
                //天数
                var dayCount = $.trim($("#<%=txtDayCount.ClientID %>").val());
                //出发开始日期
                var beginDate = $.trim($("#<%=txtBeginDate.ClientID %>").val());
                //出发结束日期
                var endDate = $.trim($("#<%=txtEndDate.ClientID %>").val());
                //线路区域ID
                var areaId = '<%=Request.QueryString["xlid"]%>';
                //业务员编号
                var SellerId=<%=selectOperator1.ClientID%>.GetOperatorId();
                //业务员姓名
                var SellerName=<%=selectOperator1.ClientID%>.GetOperatorName();
                //计调员编号
                var CoordinatorId=<%=selectOperator2.ClientID%>.GetOperatorId();
                //计调员姓名
                var CoordinatorName=<%=selectOperator2.ClientID%>.GetOperatorName();
                //参数
                var para = { orderName: "", teamNumber: "", teamName: "", dayCount: "", beginDate: "", endDate: "", areaId: "", SellerId: "", SellerName: "", CoordinatorId: "", CoordinatorName: "", tourStatus: $("#txtTourStatus").val() };
                para.teamNumber = teamNumber;
                para.teamName = teamName;
                para.orderName = orderName;
                para.dayCount = dayCount;
                para.beginDate = beginDate;
                para.endDate = endDate;
                para.areaId = areaId;
                para.CoordinatorId = CoordinatorId;
                para.CoordinatorName = CoordinatorName;
                para.SellerId = SellerId;
                para.SellerName = SellerName;
                window.location.href = "/TeamPlan/TeamPlanList.aspx?" + $.param(para);
                return false;
            });
            //修改按钮事件
            $("#btnUpdate").click(function() {
                if (TeamPlanList.GetListSelectCount() == 1) {
                    var list = TeamPlanList.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        if ($(this).attr("relType") == "Normal") {
                            window.location.href = "/TeamPlan/StandardVersion.aspx?type=Update&id=" + list[0];
                        } else {
                            window.location.href = "/TeamPlan/FastVersion.aspx?type=Update&id=" + list[0];
                        }

                    }
                } else {
                    alert("请选择一行数据!");
                    return false;
                }
                return false;
            });
            //复制按钮事件
            $("#btnCopy").click(function() {
                if (TeamPlanList.GetListSelectCount() == 1) {
                    var list = TeamPlanList.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        window.location.href = "/TeamPlan/FastVersion.aspx?type=Copy&id=" + list[0];
                    }
                } else {
                    alert("请选择一行数据!");
                    return false;
                }
                return false;
            });
            //删除按钮事件
            $("#btnDelete").click(function() {
                if (TeamPlanList.GetListSelectCount() >= 1) {
                    //判断 是否有计划 提交财务或核算结束
                    var msg = "";
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        if ($(this).attr("checked")) {
                            if ($(this).attr("state") == "核算结束" || $(this).attr("state") == "财务核算") {
                                msg += "团号：" + $(this).attr("tourNum") + "已" + $(this).attr("state") + ",无法删除 \n";
                            }
                        }
                    });

                    if (msg != "") {
                        alert(msg);
                        return;
                    }

                    if (confirm("确定删除吗?")) {
                        var list = TeamPlanList.GetCheckedValueList();
                        if (list != null && list.length > 0) {
                            var idList = list;
                            $.newAjax({
                                type: "Get",
                                url: "/TeamPlan/AjaxTeamPlanList.ashx?type=Delete&idList=" + idList,
                                cache: false,
                                success: function(result) {
                                    if (result == "OK") {
                                        alert("删除成功!");
                                        window.location.href = "/TeamPlan/TeamPlanList.aspx";
                                        return false;
                                    } else {
                                        alert("删除失败");
                                        return false;
                                    }
                                },
                                error: function() {
                                    alert("删除失败! 请稍后在试!");
                                    return false;
                                }
                            });
                        }
                    }
                } else {
                    alert("请选择一行数据!");
                    return false;
                }
                return false;
            });
            //列表地接按钮事件
            $(".btnDiJie").click(function() {
                var id = $(this).attr("ref");
                var planType = $(this).attr("ret");
                Boxy.iframeDialog({ title: "地接", iframeUrl: "/TeamPlan/TeamTake.aspx?planId=" + id + "&planType=" + planType + "&type=team", width: "950px", height: "550px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            //列表机票按钮事件
            //            $(".btnJiPiao").click(function() {
            //                var id = $(this).attr("ref");
            //                window.location.href = "/sanping/SanPing_JiPiaoAdd.aspx?type=2&tourId=" + id;
            //                return false;
            //            });
            //列表打印按钮事件
            $(".btnDaYin").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "打印", iframeUrl: "/print/printlist.aspx?tourId=" + id, width: "250px", height: "300px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            //列表结算事件
            $(".btnJieSuan").click(function() {
                var id = $(this).attr("ref");
                window.location.href = "/TeamPlan/TeamSettle.aspx?type=team&id=" + id;
                return false;
            });

            //文本框回车事件
            $(".searchbox input[type='text']").keydown(function(event) {
                var e = event;
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            });

            //列表添加旅客信息
            $(".AddPeople").click(function() {
                var orderId = $(this).attr("ref");
                var tourId = $(this).attr("rex");
                var ticketType = $(this).attr("ticketType");
                if (ticketType == 0) {
                    Boxy.iframeDialog({ title: "旅客信息", iframeUrl: "/TeamPlan/TeamPlanAddMan.aspx?orderId=" + orderId + "&tourId=" + tourId, width: "850px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            });

            //打开组团信息页
            $(".openByerInfo").click(function() {
                var buyerId = $(this).attr("ref");
                Boxy.iframeDialog({ title: "组团社信息", iframeUrl: "/CRM/customerinfos/CustomerDetails.aspx?cId=" + buyerId, width: "775px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })
        })

        var TeamPlanList = {
            GetListSelectCount: function() {
                var count = 0;
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        count++;
                    }
                });
                return count;
            },
            GetCheckedValueList: function() {
                var arrayList = new Array();
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        arrayList.push($(this).attr("ref"));
                    }
                });
                return arrayList;
            }
        };
    </script>

    </form>
</asp:Content>
