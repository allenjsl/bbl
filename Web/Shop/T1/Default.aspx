<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.Shop.T1.Default"  MasterPageFile="~/Shop/T1/Default.Master" Title="首页"%>
<%--首页--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Src="~/UserControl/wh/ImagePlayer.ascx" TagName="ImagePlayer" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>

<asp:Content runat="server" ID="PageHeader" ContentPlaceHolderID="PageHead">
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <!--[if IE]><script src="/js/bt/excanvas/excanvas-compressed.js"type="text/javascript" ></script><![endif]-->
    <script src="/js/bt/jquery.bt.min.js" type="text/javascript"></script>
    <script src="/js/utilsUri.js" type="text/javascript"></script>
    <script src="/js/tourdate.js" type="text/javascript"></script>
    <style type="text/css">
        #loginForm input, .content_right input{border: 1px solid #A2C8D2;height: 20px;line-height: 170%;margin-top: 5px;outline: medium none;padding: 0 0 0 2px;}
        .acityspan a{color:#000;}
        .acityspan a:hover{color:#ff0000;}         
    </style>
</asp:Content>

<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <uc1:imageplayer id="ImagePlayer" runat="server" />
    
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">热点线路</span></h3>
        <div class="moduel_wrap1 moduel_wrap3">
            <div class="searcherForm">
                <span class="searchSPn">搜索：</span>
                <label for=""> 线路名称：</label><input type="text" id="txtRouteName" style="width:80px" />
                <label for="">出发日期：</label><input type="text" id="txtSLDate" style="width: 80px" onfocus="WdatePicker()" />
                <label for="">至</label><input type="text" id="txtELDate" style="width: 80px" onfocus="WdatePicker()" />
                <a href="###" id="a_search" class="searchSubmit" title="查询"><img src="/images/page_icon_29.gif" style="vertical-align: middle" /></a>
            </div>
            <div class="tbtn">
                <div style="padding-top: 5px;">
                    <div style="font-size: 14px;">当前出港城市为：<span class="acityspan">
                            <a href="?cityid=0" id="acity0">全部</a>
                            <asp:Repeater runat="server" ID="rptCitys">
                                <ItemTemplate>
                                    <a href="?cityid=<%#Eval("Id") %>" id="acity<%#Eval("Id") %>"><%#Eval("CityName") %></a>
                                </ItemTemplate>
                            </asp:Repeater>
                        </span>
                    </div>
                    <div style="line-height: 26px; margin-top: 10px; width: 690px; float: left; vertical-align: middle;">出港的所有线路区域：<a class="menu-t" id="aarea0" href="?areaid=0">所有区域</a>
                        <asp:Repeater ID="rptAreas" runat="server">
                            <ItemTemplate>
                                <a class="menu-t" href="?areaid=<%#Eval("Id") %>" id="aarea<%#Eval("Id") %>"><%#Eval("AreaName")%></a>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>           
            
            <table border="0" class="item_list" bordercolor="#999999">
                <tr class="title">
                    <td>
                        序号
                    </td>
                    <td>
                        线路名称
                    </td>
                    <td>
                        出发时间&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| 剩余
                    </td>
                    <td>
                        标准
                    </td>
                    <td style="text-align:left;">
                        <span style="margin-left:10px;">价格(成人/儿童)</span>
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
                            <td style="line-height: 20px;">
                                <%#Eval("TourRouteStatus").ToString().Trim() != "无" ? Eval("TourRouteStatus").ToString().Trim() == "特价" ? "<span class=\"state1\">" + Eval("TourRouteStatus") + "</span>" : "<span class=\"state2\">" + Eval("TourRouteStatus") + "</span>" : ""%>
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
                                            <td style="text-align:center ;width:40%;height:24px;">
                                                报价标准
                                            </td>
                                            <td style="text-align: center" >
                                                价格
                                            </td>
                                        </tr>
                                        <asp:Literal runat="server" ID="ltrPriceFD"></asp:Literal>
                                    </table>
                                </div>
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
                    <cc1:exporpageinfoselect id="paging" runat="server" linktype="3" pagestyletype="NewButton" />
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

            //当前出港城市
            $("#acity" + params["cityid"]).css({ "color": "#ff0000" });
            //当前线路区域
            $("#aarea" + params["areaid"]).css({ "color": "#ff0000" });

            //查询 
            $("#a_search").bind("click", function() {
                params["routename"] = $("#txtRouteName").val();
                params["sldate"] = $("#txtSLDate").val();
                params["eldate"] = $("#txtELDate").val();

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
        });
    </script>
</asp:Content>