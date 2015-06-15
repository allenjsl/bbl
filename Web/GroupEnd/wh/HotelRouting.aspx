<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelRouting.aspx.cs" Inherits="Web.GroupEnd.HotelRouting" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc3" %>
<%@ Register Src="/UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl"
    TagPrefix="uc1" %>
<%@ Register Src="/UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc2" %>
<%@ Register Src="/UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc3" %>
<%@ Register Src="/UserControl/wh/WhLineControl.ascx" TagName="WhLineControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<head runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("酒店",this.Page) %></title>
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/tipsy.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
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
                <uc5:TravelTool ID="TravelTool1" runat="server" />
                <!--infoListStart-->
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">酒店</span></h3>
                    <div class="moduel_wrap1 moduel_wrap3">
                        <div class="searcherForm">
                            <span class="searchSPn">搜索：</span> 省份：<asp:DropDownList ID="ddlPro" runat="server">
                                <asp:ListItem Value="0">-请选择-</asp:ListItem>
                            </asp:DropDownList>
                            城市：<asp:DropDownList ID="ddlCity" runat="server">
                                <asp:ListItem Value="0">-请选择-</asp:ListItem>
                            </asp:DropDownList>
                            星级
                            <asp:DropDownList ID="sel_star" runat="server">
                            </asp:DropDownList>
                            <label for="">
                                酒店名称</label><input type="text" id="hotelName" name="hotelName" runat="server" />
                            <a href="javascript:void(0);" id="a_submit" title="" class="searchSubmit" title="提交">
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
                            <asp:Repeater ID="rptTourList" runat="server">
                                <ItemTemplate>
                                    <tr align="center">
                                        <td>
                                            <a href="HotelInfo.aspx?hotelId=<%#Eval("Id") %>">
                                                <%#Eval("UnitName")%></a>
                                        </td>
                                        <td>
                                            <%#ReturnStar(Convert.ToString(Eval("Star"))) %>
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

    <script src="/js/jquery.tipsy.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#a_submit").click(function() {
                var para = { ProId: "", CityId: "", Star: "", HotelName: "" };
                para.ProId = $("#<%=ddlPro.ClientID %>").val();
                para.CityId = $("#<%=ddlCity.ClientID %>").val();
                para.Star = $("#<%=sel_star.ClientID %>").val();
                para.HotelName = encodeURIComponent($("#<%=hotelName.ClientID %>").val());
                window.location.href = "/GroupEnd/wh/HotelRouting.aspx?" + $.param(para);
                return false;
            })

            $(".info").each(function() {
                $(this).tipsy({ fade: true, gravity: 'w' });
            })

        })
        //省份选择事件
        $("#<%=ddlPro.ClientID %>").change(function() {
            $("#<%=ddlCity.ClientID %>").html("<option value='0'>-请选择-</option>");
            //公司ID后台取得
            var id = '<%=companyId %>';
            //参数
            var params = '{proId:"' + $(this).val() + '",companyId:"' + id + '"}';
            $.ajax({
                type: "POST",
                //调用当前页面的BindCity方法
                url: "HotelRouting.aspx/BindCity",
                contentType: "application/json; charset=utf-8",
                //传递的参数
                data: params,
                success: function(msg) {
                    //将返回来的项添加到下拉菜单中 
                    $("#<%=ddlCity.ClientID %>").append(decodeURI(eval('(' + msg + ')').d));
                }
            });
        })
      
            
       
    </script>

    </form>
</body>
</html>
