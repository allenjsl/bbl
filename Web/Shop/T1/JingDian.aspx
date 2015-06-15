<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JingDian.aspx.cs" Inherits="Web.Shop.T1.JingDian" MasterPageFile="~/Shop/T1/Default.Master" Title="景点" %>
<%--景点--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">景点</span></h3>
        <div class="moduel_wrap1 moduel_wrap3">
            <div class="searcherForm">
                <span class="searchSPn">搜索：</span> 省份：<asp:DropDownList ID="txtProvince" runat="server">
                    <asp:ListItem Value="0">-请选择-</asp:ListItem>
                </asp:DropDownList>
                城市：<select id="txtCity">
                    <option value="0">-请选择-</option>
                </select>
                <label>
                    景点名称</label><input type="text" id="txtName" name="txtName" />
                <a href="javascript:void(0);" id="a_submit" title="" class="searchSubmit" title="查询">
                    <img src="/images/page_icon_29.gif" /></a>
            </div>
            <table border="0" class="item_list" bordercolor="#999999" width="100%">
                <tr class="title">
                    <td width="10%">
                        序号
                    </td>
                    <td width="40%">
                        景点名称
                    </td>
                    <td width="25%">
                        星级
                    </td>
                    <td width="25%">
                        所在城市
                    </td>
                </tr>
                <asp:Repeater ID="rpt" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span>
                                    <%#(pageIndex-1)*pageSize + Container.ItemIndex+1 %>
                                </span>
                            </td>
                            <td>
                                <h3 class="sightDetail">
                                    <a style="cursor: pointer" target="_blank" title="<%#Eval("UnitName") %>">
                                        <%#EyouSoft.Common.Utils.GetText(Eval("UnitName").ToString(), 20, true)%></a>
                                </h3>
                                <input type="hidden" name="hidvalue" value="<%# Eval("id") %>" />
                            </td>
                            <td>
                                <%#Eval("Start").ToString().Replace("_","")%>
                            </td>
                            <td>
                                <%#Eval("ProvinceName")%>_<%#Eval("CityName")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div style="text-align: center; margin-top: 10px; margin-bottom: 10px;" runat="server" id="divEmpty" visible="false">
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
            var params = { provinceid: $("#<%=txtProvince.ClientID %>").val(), cityid: $("#txtCity").val(), name: $("#txtName").val() };

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

        //查看景点明细
        function openDetail(obj) {
            var id = $(obj).next("[name='hidvalue']").val();
            var url = "jingdianmingxi.aspx?id=" + id;
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "景点详情",
                modal: true,
                width: "825px",
                height: "530px"
            });
            
            return false;
        }

        var searchCityId = "0";

        $(document).ready(function() {
            $("#<%=txtProvince.ClientID %>").bind("change", function() { changeProvince(); });
            $("#a_submit").bind("click", function() { return search(); });
            $(".sightDetail").bind("click", function() { openDetail(this); });

            //初始化查询
            var params = utilsUri.getUrlParams([]);
            params["name"] = params["name"] || "";
            $("#<%=txtProvince.ClientID %>").val(params["provinceid"]);
            searchCityId = params["cityid"];
            if (params["provinceid"] != "0") changeProvince();
            $("#txtName").val(decodeURI(params["name"]));
        });
    </script>
</asp:Content>
