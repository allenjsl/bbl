<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiuDian.aspx.cs" Inherits="Web.Shop.T1.JiuDian" MasterPageFile="~/Shop/T1/Default.Master" Title="酒店" %>
<%--酒店--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <!--itemListStart-->
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">酒店</span></h3>
        <div class="moduel_wrap1 moduel_wrap3">
            <div class="searcherForm">
                <span class="searchSPn">搜索：</span> 省份：<asp:DropDownList ID="txtProvince" runat="server">
                    <asp:ListItem Value="0">-请选择-</asp:ListItem>
                </asp:DropDownList>
                城市：<select id="txtCity">
                    <option value="0">-请选择-</option>
                </select>
                星级
                <asp:DropDownList ID="txtStar" runat="server">
                </asp:DropDownList>
                <label>
                    酒店名称</label><input type="text" id="txtName" name="txtName" />
                <a href="javascript:void(0);" id="a_submit" title="" class="searchSubmit" title="查询">
                    <img src="/images/page_icon_29.gif" /></a>
            </div>
            <table border="0" class="item_list" bordercolor="#999999" width="100%">
                <tr class="title">
                    <td>
                        酒店名称
                    </td>
                    <td width="15%">
                        星级
                    </td>
                    <td width="25%">
                        地址
                    </td>
                    <td width="15%">
                        所在城市
                    </td>
                </tr>
                <asp:Repeater ID="rpt" runat="server">
                    <ItemTemplate>
                        <tr align="center">
                            <td>
                                <a href="jiudianmingxi.aspx?hotelid=<%#Eval("Id") %>">
                                    <%#Eval("UnitName")%></a>
                            </td>
                            <td>
                                <%#GetStarString((int)Eval("Star"))%>
                            </td>
                            <td>
                                <%#Eval("UnitAddress") %>
                            </td>
                            <td>
                                <%# Eval("ProvinceName")%>_<%# Eval("CityName")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div style="text-align: center; margin-top: 10px; margin-bottom:10px;" runat="server" id="divEmpty" visible="false">
                未找到相关数据。
            </div>
            <div style="text-align: center; margin-top: 10px;" class="digg" runat="server" id="divPaging">
                <div class="diggPage">
                    <cc1:ExporPageInfoSelect ID="paging" runat="server" LinkType="3" PageStyleType="NewButton" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
    <script type="text/javascript">
        //查询
        function search() {
            var params = { provinceid: $("#<%=txtProvince.ClientID %>").val(), cityid: $("#txtCity").val(), star: $("#<%=txtStar.ClientID %>").val(), name: $("#txtName").val() };

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
        //省份变更
        function changeProvince() {
            $("#txtCity").html("<option value='0'>-请选择-</option>");
            var params = { provinceid: $("#<%=txtProvince.ClientID %>").val() };
            $.ajax({
                type: "POST",
                url: "default.aspx?doType=getCity",
                data: params,
                success: function(response) {
                    $("#txtCity").append(response);
                    $("#txtCity").val(searchCityId);
                }
            });
        }

        var searchCityId = "0";

        $(document).ready(function() {
            $("#<%=txtProvince.ClientID %>").bind("change", function() { changeProvince(); });
            $("#a_submit").bind("click", function() { return search(); });

            //初始化查询
            var params = utilsUri.getUrlParams([]);
            params["name"] = params["name"] || "";
            searchCityId = params["cityid"];
            $("#<%=txtProvince.ClientID %>").val(params["provinceid"]);
            if (params["provinceid"] != "0") changeProvince();
            $("#txtName").val(decodeURI(params["name"]));
            $("#<%=txtStar.ClientID %>").val(params["star"])
        });
    </script>
</asp:Content>
