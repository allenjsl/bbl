<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SightList.aspx.cs" Inherits="Web.SupplierControl.SightManager.SightList"
    MasterPageFile="~/masterpage/Back.Master" Title="景点_供应商管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">供应商管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置 &gt;&gt; 供应商管理 &gt;&gt; 景点
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
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" alt="" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                省份：</label>
                            <uc1:ucProvince runat="server" ID="ucProvince1" />
                            <label>
                                城市：</label>
                            <uc1:ucCity runat="server" ID="ucCity1" />
                            <label>
                                景点名称：</label>
                            <input type="text" class="searchinput" id="sight_name" name="sight_name" runat="server"
                                value="" />
                            <a href="javascript:void(0);" id="searchbtn">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" alt="" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0" style="width: 360px">
                    <tr>
                        <td width="90" align="center">
                            <%if (TravelCheck("add"))
                              { %>
                            <a href="javascript:;" class="add">新 增</a><%} %>
                        </td>
                        <td width="90" align="center">
                            <%if (TravelCheck("del"))
                              { %>
                            <a href="javascript:;" class="delall" onclick="del_carTeams()">批量删除</a><%} %>
                        </td>
                        <td width="90" align="center">
                            <%if (TravelCheck("load"))
                              { %>
                            <a href="javascript:;" class="loadxls">
                                <img src="/images/daoru.gif" />
                                导 入</a><%} %>
                        </td>
                        <td width="90" align="center">
                            <%if (TravelCheck("toExcel"))
                              { %>
                            <a href="javascript:;" class="toexcel">
                                <img src="/images/daoru.gif" alt="" width="16" height="16" />
                                导 出</a><%} %>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="5%" height="30" align="center" bgcolor="#BDDCF4">
                            全选<input type="checkbox" name="checkbox_all" class="selall" />
                        </th>
                        <th width="14%" align="center" bgcolor="#BDDCF4">
                            所在地
                        </th>
                        <th width="10%" align="center" bgcolor="#bddcf4">
                            景点名称
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            星级
                        </th>
                        <th width="8%" align="center" bgcolor="#bddcf4">
                            散客价
                        </th>
                        <th width="7%" align="center" bgcolor="#bddcf4">
                            团队价
                        </th>
                        <th width="7%" align="center" bgcolor="#bddcf4">
                            联系人
                        </th>
                        <th width="7%" align="center" bgcolor="#bddcf4">
                            电话
                        </th>
                        <th width="7%" align="center" bgcolor="#bddcf4">
                            传真
                        </th>
                        <th width="10%" align="center" bgcolor="#bddcf4">
                            政策
                        </th>
                        <th width="7%" align="center" bgcolor="#bddcf4">
                            交易次数
                        </th>
                        <th width="14%" align="center" bgcolor="#bddcf4">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                                <td height="30" align="center">
                                    <input type="checkbox" name="checkbox" id="checkbox_<%#Eval("Id") %>" class="checkbox_child" />
                                </td>
                                <td align="center">
                                    <%#Eval("ProvinceName")%>_<%#Eval("CityName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("UnitName")%>
                                </td>
                                <td align="center">
                                    <%#DisponseStar(Convert.ToString(Eval("Start")))%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("TravelerPrice").ToString()).ToString("0.00"))%>
                                </td>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("TeamPrice").ToString()).ToString("0.00"))%>
                                </td>
                                <%#GetContactInfo(Eval("SupplierContact"))%>
                                <td align="center">
                                    <%#Eval("UnitPolicy")%>
                                </td>
                                <td align="center">
                                    共计<%#Eval("TradeNum")%>次
                                </td>
                                <td align="center">
                                    <a id="show_<%#Eval("Id") %>" href="javascript:void(0);" onclick="showSight(<%#Eval("Id") %>)">查看</a>
                                    <%if (TravelCheck("modify"))
                                      { %>
                                    <a id="modify_<%#Eval("Id") %>" href="javascript:void(0)" onclick="updateSight(<%#Eval("Id") %>)">
                                        修改</a><%} %>
                                    <%if (TravelCheck("del"))
                                      { %><a id="del_<%#Eval("Id") %>" href="javascript:void(0)" onclick="delbyId(<%#Eval("Id") %>)">
                                          删除</a><%} %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%--            <%if (len == 0)
                    { %>
                    <tr align="center"><td colspan="8">没有相关数据</td></tr>
                  <%} %>--%>
                </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="13">
                            <uc2:ExportPageInfo ID="ExportPageInfo1" LinkType="4" PageStyleType="NewButton" CurrencyPageCssClass="RedFnt"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript">
        $(function() {
            $("a.loadxls").click(function() {
                var url = "/SupplierControl/SightManager/SightLoadExcel.aspx";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "景点EXCEL",
                    modal: true,
                    width: "820px",
                    height: "400px"
                });
                return false;
            })
            $("a.toexcel").click(function() {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&act=toexcel"; } else { url += "?act=toexcel"; }
                window.location.href = url;
                return false;
            });
            $(".add").click(function() {
                var url = "/SupplierControl/SightManager/SightAdd.aspx?type=add";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "景点新增",
                    modal: true,
                    width: "850px",
                    height: "680px"
                });
                return false;
            })
            //全选事件
            $(".selall").click(function() {
                if ($(".selall").attr("checked") == true) {
                    $(".checkbox_child").attr("checked", true);
                } else {
                    $(".checkbox_child").attr("checked", false);
                }
            })

            $("#searchbtn").click(function() {
                var proid = $("#" + provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId).val();
                var cityid = $("#" + provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId).val();
                var url = "SightList.aspx?cityId=" + cityid + "&proid=" + proid + "&sight_name=" + encodeURIComponent($("#<%=sight_name.ClientID %>").val());
 
                window.location.href = url;
                return false;
            })
            $("#<%=sight_name.ClientID %>").keydown(function(e) {
                if (e.keyCode == 13) {
                    $("#searchbtn").click();
                }
                return false;
            })

        })
        //更新界面弹窗
        function updateSight(obj) {
            var url = "/SupplierControl/SightManager/SightAdd.aspx?type=modify&sid=" + obj;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "景点更新",
                modal: true,
                width: "850px",
                height: "680px"
            });
            return false;
        }
        function showSight(obj) {
            var url = "/SupplierControl/SightManager/SightAdd.aspx?type=show&sid=" + obj;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "景点查看",
                modal: true,
                width: "850px",
                height: "680px"
            });
            return false;
        }

        //批量删除事件
        function del_carTeams() {
            var ids = "";
            var count = 0;
            $(".checkbox_child").each(function() {
                if ($(this).attr("checked") == true) {
                    count++;
                }
            })
            if (count != 0) {
                if (confirm("确定要删除么")) {
                    $(".checkbox_child").each(function() {
                        if ($(this).attr("checked") == true) {
                            ids += $(this).attr("id").split('_')[1] + "|";
                        }
                    })
                    $.newAjax({
                        url: "SightAdd.aspx?type=dels&ids=" + ids,
                        type: "GET",
                        cache: false,
                        success: function(result) {
                            alert(result);
                            if (result == "True") {
                                alert("删除成功");
                                window.location = location;
                            } else {
                                alert("删除失败")
                            }
                        }
                    });
                }
            } else {
                alert("请选择至少一条记录");
            }
            return false;
        }
        //单个删除
        function delbyId(obj) {
            if (confirm("确定要删除么")) {
                $.newAjax({
                url: "SightAdd.aspx?type=dels&ids=" + obj + "|",
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
                });
            }
            return false;
        }
    
    </script>

    </form>
</asp:Content>
