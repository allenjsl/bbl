<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarsList.aspx.cs" Inherits="Web.SupplierControl.CarsManager.CarsList"
    MasterPageFile="~/masterpage/Back.Master" Title="车队_供应商管理_芭比来" %>

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
                             所在位置 &gt;&gt; 供应商管理 &gt;&gt; 车队
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
                                车队名称：</label>
                            <input type="text" class="searchinput" id="carTeam_name" name="carTeam_name" value=""
                                runat="server" />
                            <label>
                                车型：</label><input type="text" class="searchinput" id="car_type" name="car_type" runat="server" /><a
                                    href="javascript:void(0);" id="searchbtn"><img src="/images/searchbtn.gif" style="vertical-align: top;" /></a>
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
                              { %><a href="javascript:;" class="loadxls">
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
                        <th width="12%" align="center" bgcolor="#BDDCF4">
                            所在地
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            车队名称
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            联系人
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            电话
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            传真
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            交易情况
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
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
                                <%#GetContactInfo(Eval("SupplierContact"))%>
                                <td align="center">
                                    共计<%#Eval("TradeNum")%>次
                                </td>
                                <td align="center">
                                    <a id="show_<%#Eval("Id") %>" href="javascript:void(0);" onclick="showCarTeam(<%#Eval("Id") %>)">
                                        查看 </a>
                                    <%if (TravelCheck("modify"))
                                      { %>
                                    <a id="modify_<%#Eval("Id") %>" href="javascript:void(0)" onclick="updateCarTeam(<%#Eval("Id") %>)">
                                        修改</a>
                                    <%} %>
                                    <%if (TravelCheck("del"))
                                      { %>
                                    <a id="del_<%#Eval("Id") %>" href="javascript:void(0)" onclick="delbyId(<%#Eval("Id") %>)">
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
                            <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"
                                runat="server"></uc2:ExportPageInfo>
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
                var url = "/SupplierControl/CarsManager/CarsLoadExcel.aspx";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "车队EXCEL",
                    modal: true,
                    width: "820px",
                    height: "400px"
                });
                return false;
            })
            $("a.toexcel").click(function() {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&act=toexcel"; } else { url += "?act=toexcel"; }
                window.location.href = url + "&carname=" + $("#<%=carTeam_name.ClientID%>").val() + "&cartype=" + $("#<%=car_type.ClientID%>").val();
            });
            $(".add").click(function() {
                var url = "/SupplierControl/CarsManager/CarsAdd.aspx?type=new";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "车队新增",
                    modal: true,
                    width: "850px",
                    height: "380px"
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
            //查询事件
            $("#searchbtn").click(function() {
                search();
                return false;
            })

            $("#<%=carTeam_name.ClientID%>,#<%=car_type.ClientID%>").keydown(function(e) {
                if (e.keyCode == 13) {
                    search();
                }
            })

        })
        //更新界面弹窗
        function updateCarTeam(obj) {
            var url = "/SupplierControl/CarsManager/CarsAdd.aspx?type=modify&cid=" + obj;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "车队更新",
                modal: true,
                width: "850px",
                height: "380px"
            });
            return false;
        }
        //查看
        function showCarTeam(obj) {
            var url = "/SupplierControl/CarsManager/CarsAdd.aspx?type=show&cid=" + obj;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "车队查看",
                modal: true,
                width: "850px",
                height: "380px"
            });
            return false;
        }
        //搜索事件
        function search() {
            var proid = $("#" + provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId).val();
            var cityid = $("#" + provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId).val();
            var url = "CarsList.aspx?cityId=" + cityid + "&proid=" + proid + "&carname=" + encodeURIComponent($("#<%=carTeam_name.ClientID%>").val()) + "&cartype=" + encodeURIComponent($("#<%=car_type.ClientID%>").val());
            window.location.href = url;
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
                        url: "CarsAdd.aspx?type=dels&ids=" + ids,
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
            } else {
                alert("请选择至少一条记录");
            }

            return false;


        }
        //单个删除
        function delbyId(obj) {
            if (confirm("确定要删除么")) {
                $.newAjax({
                    url: "CarsAdd.aspx?type=dels&ids=" + obj + "|",
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
