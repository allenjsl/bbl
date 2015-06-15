<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="travelList.aspx.cs" Inherits="Web.jipiao.Traffic.travelList"
    MasterPageFile="~/masterpage/Back.Master" Title="交通行程管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="head" ID="contentHead" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="contectBody" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">交通管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前用您所在位置：</b> >> 交通管理 >> 行程列表
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
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="left">
                        <a href="javascript:" id="addTravel">新增行程</a>
                    </td>
                    <td width="90" align="left">
                        <a href="javascript:" class="link2" id="updateTravel">修 改</a>
                    </td>
                    <td width="90" align="left">
                        <a href="javascript:" id="deleteTravel">删 除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                        全选
                        <input type="checkbox" id="checkAll" />
                    </th>
                    <th width="7%" align="center" bgcolor="#BDDCF4">
                        序列
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        类型
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        出发-抵达
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        航班号
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        航空公司
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        出发时间
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        抵达时间
                    </th>
                    <th width="7%" align="center" bgcolor="#bddcf4">
                        舱位
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4">
                        机型
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        经停
                    </th>
                    <th width="5%" align="center" bgcolor="#bddcf4">
                        间隔
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="repTravelList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td height="30" align="center" bgcolor="#e3f1fc">
                                <input type="checkbox" data_id="<%# Eval("TravelId") %>" class="checkItem" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc" class="pandl3">
                                第<%# Eval("SerialNum")%>段
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("TrafficType".ToString())%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("LCityName")%>-<%#Eval("RCityName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("FilghtNum")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("FlightCompany")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("LTime")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("RTime")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Space").ToString()%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("AirPlaneType")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("IsStop").ToString() == "False" ? "经停" : "直飞"%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("IntervalDays")%>天
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:" class="update" data_id="<%# Eval("TravelId") %>">修改</a> |
                                <a href="javascript:" class="delete" data_id="<%# Eval("TravelId") %>">删除</a>
                            </td>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                       <asp:Label ID="labMsg" runat="server" Visible="false" Text="暂无数据!"></asp:Label>
                        <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        var travelPage = {
            tfID: '<%=EyouSoft.Common.Utils.GetQueryStringValue("trfficID") %>',
            openDialog: function(title, url, width, height) {
                Boxy.iframeDialog({ title: title, iframeUrl: url, width: width, height: height, draggable: true, data: null, hideFade: true, modal: true });
            },
            Ajax: function(travelId) {
                $.newAjax({
                    url: '/jipiao/Traffic/travelList.aspx?type=delete&ID=' + travelId + "&tfID=" + travelPage.tfID,
                    type: 'POST',
                    cache: false,
                    success: function(ret) {
                        var obj = eval('(' + ret + ')');                        
                        alert(obj.msg);
                        window.location.reload();
                    },
                    error: function() {
                        alert("服务器繁忙，请稍后在试!");
                    }
                });
            }
        }
        $(function() {
            //全选
            $("#checkAll").click(function() {
                if ($(this).attr("checked")) {
                    $(".tablelist").find("input[type='checkbox']").attr("checked", "checked");
                }
                else {
                    $(".tablelist").find("input[type='checkbox']").attr("checked", "");
                }
            });

            //新增
            $("#addTravel").click(function() {
                var tfId = travelPage.tfID;
                travelPage.openDialog("新增", "/jipiao/Traffic/travelAdd.aspx?type=add&tfID=" + tfId, "450px", "480px");
                return false;
            });

            //菜单修改
            $("#updateTravel").click(function() {
                var checkList = $(".tablelist").find(".checkItem:checked");
                var chekLenght = checkList.length;
                if (chekLenght == 0) {
                    alert("请选择一条需要修改的记录!");
                    return;
                }
                if (chekLenght > 1) {
                    alert("只能选择一条记录进行修改!");
                    return;
                }
                var tfId = travelPage.tfID;
                var trID = checkList.eq(0).attr("data_id");
                travelPage.openDialog("修改", "/jipiao/Traffic/travelAdd.aspx?type=update&travelId=" + trID + "&tfID=" + tfId, "450px", "480px");
                return false;
            });

            //列表修改
            $(".update").click(function() {
                var tfId = travelPage.tfID;
                var trID = $(this).attr("data_id");
                travelPage.openDialog("修改", "/jipiao/Traffic/travelAdd.aspx?type=update&travelId=" + trID + "&tfID=" + tfId, "450px", "480px");
                return false;
            });

            //菜单删除
            $("#deleteTravel").click(function() {
                var checkList = $(".tablelist").find(".checkItem:checked");
                var chekLenght = checkList.length;
                if (chekLenght == 0) {
                    alert("请选择需要删除的记录!");
                    return;
                }
                var Idlist = new Array();
                $(".tablelist").find(".checkItem:checked").each(function() {
                    Idlist.push($(this).attr("data_id"));
                });

                if (Idlist.length > 0) {
                    if (confirm("确定要删除此条记录吗?")) {
                        travelPage.Ajax(Idlist);
                    }
                }
                return false;
            });

            //列表删除
            $(".delete").click(function() {
                if (confirm("确定要删除此条记录吗?")) {
                    var trID = $(this).attr("data_id");
                    if (trID != '' && trID != undefined) {
                        travelPage.Ajax(trID);
                    }
                }
                return false;
            });
        });
    </script>

</asp:Content>
