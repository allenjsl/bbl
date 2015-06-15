<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotRouting.aspx.cs" Inherits="Web.GroupEnd.HotRouting" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc3" %>
<%@ Register Src="/UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc2" %>
<%@ Register Src="/UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl" TagPrefix="uc3" %>
<%@ Register Src="/UserControl/wh/WhLineControl.ascx" TagName="WhLineControl" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("热点线路",this.Page) %></title>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <!--[if IE]><script src="/js/bt/excanvas/excanvas-compressed.js"type="text/javascript" ></script><![endif]-->

    <script src="/js/bt/jquery.bt.min.js" type="text/javascript" charset="utf-8"></script>

</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server">
    <div id="page">
        <uc1:WhHeadControl ID="WhHeadControl1" runat="server" />
        <!--mainContentStart-->
        <div class="content">
            <div class="content_left">
                <!--loginFromStart-->
                <uc3:WhLoginControl ID="WhLoginControl1" runat="server" />
                <uc5:TravelTool ID="TravelTool1" runat="server" />
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">热点线路</span></h3>
                    <div class="moduel_wrap1 moduel_wrap3">
                        <div class="searcherForm">
                            <span class="searchSPn">搜索：</span>
                            <label for="">
                                线路名称：</label><asp:TextBox ID="txtAreaName" runat="server" Width="55px"></asp:TextBox>
                            <label for="">
                                出发日期：</label><asp:TextBox ID="txtBeginDate" runat="server" Width="65px" onfocus="WdatePicker()"></asp:TextBox>
                            <label for="">
                                至</label>
                            <asp:TextBox ID="txtEndDate" runat="server" Width="70px" onfocus="WdatePicker()"></asp:TextBox>
                            &nbsp;
                            <asp:DropDownList ID="ddlArea" runat="server" Width="105px">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlOutCity" runat="server" Width="105px">
                                <asp:ListItem Value="0" Text="--请选择--"></asp:ListItem>
                            </asp:DropDownList>
                            <a href="javascript:void(0);" id="a_submit" title="" class="searchSubmit" title="提交">
                                <img src="/images/page_icon_29.gif" style="vertical-align: middle;" /></a>
                        </div>
                        <table border="0" class="item_list" bordercolor="#999999">
                            <tr class="title">
                                <td>
                                    序号
                                </td>
                                <td style="width:269px;">
                                    线路名称
                                </td>
                                <td>
                                    出发时间 | 剩余
                                </td>
                                <td>
                                    标准
                                </td>
                                <td>
                                    价格(成人/儿童)
                                </td>
                                <td>
                                    交易操作
                                </td>
                                <td>
                                    在线交流
                                </td>
                            </tr>
                            <asp:Repeater ID="rptTourList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <span>
                                                <%#(pageIndex-1)*pageSize + Container.ItemIndex+1 %>
                                            </span>
                                        </td>
                                        <td>
                                            <h3>
                                                <a target="_blank" title="<%#Eval("RouteName") %>" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType"))+"?tourId="+Eval("tourId").ToString() %>">
                                                    <%#EyouSoft.Common.Utils.GetText(Eval("RouteName").ToString(),21,true)%></a>
                                            </h3>
                                        </td>
                                        <td>
                                            <span>
                                                <%#Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd")%></span> | <span>
                                                    <%#GetDays(Eval("LDate"))%>天</span>
                                        </td>
                                        <td>
                                            <span>标准</span>
                                        </td>
                                        <td>
                                            <div style="float: left; margin-left: 20px; cursor: pointer; color: Red;" class="show">
                                                ￥<%#getPeoPlePrice(Eval("tourId").ToString())%></div>
                                            <div style="display: none; float: left;">
                                                <table width="200" border="0" align="center" cellpadding="5" cellspacing="1">
                                                    <%#customlevtypeprice(Convert.ToString(Eval("tourId"))) %>
                                                </table>
                                            </div>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" ref="/GroupEnd/LineList/LineProductsList.aspx?tourid=<%#Eval("tourId") %>"
                                                class="Destine">
                                                <img src="/images/preBuyIcon.gif" alt="" /></a>
                                        </td>
                                        <td>
                                            <%#EyouSoft.Common.Utils.GetQQ(Eval("ContacterQQ").ToString())%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <div style="text-align: center; margin-top: 10px;" class="digg">
                            <div class="diggPage">
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                <cc3:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton" />
                            </div>
                        </div>
                    </div>
                </div>
                <!--itemListEnd-->
                <!--picListStart-->
                <!--picListEnd-->
            </div>
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <uc2:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
    <div id="trMsg" style="color: Red; display: none;">
        <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px;">
            <tr>
                <td height="60">
                    <span style="font-size: 14px; font-weight: bold; color: #FF0000; text-indent: 24px;">
                        尊敬的客户： 您已经登录成功 若系统后台没有自动弹出，请点这里进入系统后台！ <a id="linkManage" href="#" style="text-decoration: underline"
                            target="_self">进入系统后台</a></span>
                </td>
            </tr>
        </table>
    </div>

    <script src="../../js/slogin.js" type="text/javascript"></script>

    <script src="../../js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#a_submit").click(function() {
                var para = { AreaName: "", BeginDate: "", EndDate: "", AreaId: "", CityID: "" };

                para.AreaName = $.trim($("#<%=txtAreaName.ClientID %>").val());
                para.BeginDate = $.trim($("#<%=txtBeginDate.ClientID %>").val());
                para.EndDate = $.trim($("#<%=txtEndDate.ClientID %>").val());
                para.AreaId = $("#<%=ddlArea.ClientID %>").val();
                para.CityID = $("#<%=ddlOutCity.ClientID %>").val();

                window.location.href = "/GroupEnd/wh/HotRouting.aspx?" + $.param(para);
                return false;
            })

            $(".Destine").click(function() {
                var url = $(this).attr("ref");
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/GroupEnd/whlogin.aspx?type=isLogin",
                    cache: false,
                    success: function(result) {
                        if (result == "True") {
                            window.location.href = url;
                        } else {
                            alert("您尚未登录,请先登录系统!");
                        }
                    }
                });
                return false;
            })
        })

        $(function() {
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
        });
       
    </script>

    </form>
</body>
</html>
