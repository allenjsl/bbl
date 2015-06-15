<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourRouting.aspx.cs" Inherits="Web.GroupEnd.TourRouting" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc3" %>
<%@ Register Src="/UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc2" %>
<%@ Register Src="/UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl" TagPrefix="uc3" %>
<%@ Register Src="/UserControl/wh/WhLineControl.ascx" TagName="WhLineControl" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<head runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("旅游线路",this.Page) %></title>

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
                <!--loginFromEnd-->
                <uc5:traveltool id="TravelTool1" runat="server" />
                <!--infoListStart-->
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">旅游线路</span></h3>
                    <div class="moduel_wrap1 moduel_wrap3">
                        <table border="0" class="item_list" bordercolor="#999999" width="100%">
                            <tr class="title">
                                <td colspan="4" width="70%">
                                    线路名称
                                </td>
                                <td width="15%">
                                    附件下载
                                </td>
                                <td width="15%">
                                    操作
                                </td>
                            </tr>
                            <asp:Repeater ID="rptTourList" runat="server">
                                <ItemTemplate>
                                    <tr align="center">
                                        <td colspan="4">
                                            <a target="_blank" title="<%#Eval("RouteName") %>" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType"))+"?tourId="+Eval("tourId").ToString()+"&action=tourevery" %>">
                                                <%#Eval("RouteName")%></a>
                                        </td>
                                        <td>
                                            <%#GetAttachs(Eval("attachs"))%>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0)" target="_blank" ref="/GroupEnd/FitHairDay/FitHairDayList.aspx?tourid=<%#Eval("tourId") %>"
                                                class="apply">
                                                <img alt="提交申请" style="width: 66px; height: 18px" src="/images/tjsq.gif" /></a>
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
    <div id="box_view" style="display: none">
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="200" style="margin: 10px;
            border: 1px solid rgb(230, 251, 254);">
            <tbody>
                <tr>
                    <th bgcolor="#f5f4f4" width="69">
                        负责人：
                    </th>
                    <td bgcolor="#f5f4f4" align="left" width="129">
                        &nbsp;<span id="conName"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        电 话：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conTel"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        手 机：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conPhone"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        传 真：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conFax"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        QQ：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conQQ"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        MSN：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conMsn"></span>
                    </td>
                </tr>
                <tr>
                    <th bgcolor="#f5f4f4" align="center">
                        E-mail：
                    </th>
                    <td bgcolor="#f5f4f4" align="left">
                        &nbsp;<span id="conEmail"></span>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <script src="/js/slogin.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        var box;
        $(function() {
            //显示联系人的详细信息
            $(".contacter").click(function() {
                box = new Boxy($("#box_view"), { closeText: "X", title: "联系人信息", modal: "true", cenete: "true", fixed: "false" });
                var info = $(this).attr("contacter");
                var list = new Array();
                list = info.split("Ξ");
                $("#conName").html(list[0]);
                $("#conTel").html(list[1]);
                $("#conPhone").html(list[2]);
                $("#conFax").html(list[3]);
                $("#conQQ").html(list[4]);
                $("#conMsn").html(list[5]);
                $("#conEmail").html(list[6]);
                return false;
            })
            //预订
            $(".apply").click(function() {
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
        })
    </script>

    </form>
</body>
</html>
