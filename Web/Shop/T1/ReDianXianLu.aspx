<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReDianXianLu.aspx.cs" Inherits="Web.Shop.T1.ReDianXianLu" MasterPageFile="~/Shop/T1/Default.Master" Title="热点线路" %>

<%--热点线路--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Src="~/UserControl/wh/ImagePlayer.ascx" TagName="ImagePlayer" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content runat="server" ID="PageHeader" ContentPlaceHolderID="PageHead">
    <style type="text/css">
    #loginForm input, .content_right input{border: 1px solid #A2C8D2;height: 20px;line-height: 170%;margin-top: 5px;outline: medium none;padding: 0 0 0 2px;}
    </style>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <!--[if IE]><script src="/js/bt/excanvas/excanvas-compressed.js"type="text/javascript" ></script><![endif]-->
    <script src="/js/bt/jquery.bt.min.js" type="text/javascript"></script>
    <script src="/js/utilsUri.js" type="text/javascript"></script>
    <script src="/js/tourdate.js" type="text/javascript"></script>
</asp:content>

<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">热点线路</span></h3>
        <div class="moduel_wrap1 moduel_wrap3">
            <div class="searcherForm">
                <span class="searchSPn">搜索：</span>
                <label for="">
                    线路名称：</label><input type="text" id="txtRouteName" style="width: 65px" />
                <label for="">出发日期：</label><input type="text" id="txtSLDate" onfocus="WdatePicker()" style="width: 65px" />
                <label for="">至</label><input type="text" id="txtELDate" onfocus="WdatePicker()" style="width: 65px" />
                &nbsp;
                <asp:DropDownList ID="txtAreas" runat="server" Width="105px">
                </asp:DropDownList>
                <asp:DropDownList ID="txtCitys" runat="server" Width="105px">
                    <asp:ListItem Value="0" Text="--请选择--"></asp:ListItem>
                </asp:DropDownList>
                <a href="javascript:void(0);" id="a_search" title="" class="searchSubmit" title="提交">
                    <img src="/images/page_icon_29.gif" style="vertical-align: middle;" /></a>
            </div>
            <table border="0" class="item_list" bordercolor="#999999">
                <tr class="title">
                    <td>
                        序号
                    </td>
                    <td style="width: 269px;">
                        线路名称
                    </td>
                    <td>
                        出发时间&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| 剩余
                    </td>
                    <td>
                        标准
                    </td>
                    <td style="text-align:left">
                        <span style="margin-left:10px;">价格(成人/儿童)</span>
                    </td>
                    <td>
                        交易操作
                    </td>
                    <td>
                        在线交流
                    </td>
                </tr>
                <asp:Repeater ID="rptTours" runat="server" OnItemDataBound="rtpTours_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span>
                                    <%#(PageIndex-1)*PageSize + Container.ItemIndex+1 %>
                                </span>
                            </td>
                            <td>
                                <h3>
                                    <asp:Literal runat="server" ID="ltrRouteName"></asp:Literal>
                                </h3>
                            </td>
                            <td>
                                    <asp:Literal runat="server" ID="ltrLDate"></asp:Literal>
                            </td>
                            <td>
                                <span>标准</span>
                            </td>
                            <td>
                                <div style="float: left; margin-left: 10px; color: Red;" class="show">
                                    <asp:Literal runat="server" ID="ltrPrice"></asp:Literal>
                                </div>
                                <div style="display: none; float: left;">
                                    <table width="200" border="0" align="center" cellpadding="5" cellspacing="1">
                                        <tr bgcolor="#d0eafb">
                                            <td style="text-align: center; width: 40%; height: 24px;">
                                                报价标准
                                            </td>
                                            <td style="text-align: center">
                                                价格
                                            </td>
                                        </tr>
                                        <asp:Literal runat="server" ID="ltrPriceFD"></asp:Literal>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <a href="javascript:void(0);" data_tourid="<%#Eval("tourId") %>" class="Destine">
                                    <img src="/images/preBuyIcon.gif" alt="" /></a>
                            </td>
                            <td>
                                <%#EyouSoft.Common.Utils.GetQQ(Eval("ContacterQQ").ToString())%>
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
    
    <script type="text/javascript">
        $(document).ready(function() {
            //查询初始化
            var params = utilsUri.getUrlParams(["Page"]);
            params["routename"] = params["routename"] || "";
            params["sldate"] = params["sldate"] || "";
            params["eldate"] = params["eldate"] || "";
            params["areaid"] = params["areaid"] || "0";
            params["cityid"] = params["cityid"] || "0";

            $("#txtRouteName").val(params["routename"]);
            $("#txtSLDate").val(params["sldate"]);
            $("#txtELDate").val(params["eldate"]);
            $("#<%=txtAreas.ClientID %>").val(params["areaid"]);
            $("#<%=txtCitys.ClientID %>").val(params["cityid"]);

            //查询 
            $("#a_search").bind("click", function() {
                params["routename"] = $("#txtRouteName").val();
                params["sldate"] = $("#txtSLDate").val();
                params["eldate"] = $("#txtELDate").val();
                params["areaid"] = $("#<%=txtAreas.ClientID %>").val();
                params["cityid"] = $("#<%=txtCitys.ClientID %>").val();
                
                window.location.href = utilsUri.createUri(null, params);
                return false;
            });

            //浮动价格
            $('.show').bt({
                contentSelector: function() {
                    return $(this).next("div").html();
                },
                positions: ['right', 'left'],
                fill: '#FFFFFF',
                strokeStyle: '#666666',
                spikeLength: 20,
                spikeGirth: 10,
                width: 200,
                overlap: 0,
                centerPointY: 1,
                cornerRadius: 0,
                cssStyles: {
                    fontFamily: '"Lucida Grande",Helvetica,Arial,Verdana,sans-serif',
                    fontSize: '12px',
                    padding: '10px 14px'
                },
                shadow: true,
                shadowColor: 'rgba(0,0,0,.5)',
                shadowBlur: 8,
                shadowOffsetX: 4,
                shadowOffsetY: 4,
                closeWhenOthersOpen: true
            });

            tourdate.init();

            //预定操作
            $(".Destine").click(function() {
                var tourid = $(this).attr("data_tourid");
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/shop/t1/default.aspx?doType=isLogin",
                    cache: false,
                    success: function(response) {
                        if (response == "0") {
                            window.location.href = "/groupend/linelist/lineproductslist.aspx?isdetails=1&tourid=" + tourid;
                        } else {
                            alert("您尚未登录,请先登录系统!");
                        }
                    }
                });
                return false;
            });
        });
    </script>
</asp:content>
