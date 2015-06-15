<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SightRouting.aspx.cs" Inherits="Web.GroupEnd.SightRouting" %>

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
<head id="Head1" runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("景点",this.Page) %></title>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/jquery.boxy.js" type="text/javascript"></script>
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
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
                        <span class="bg3">景点</span></h3>
                    <div class="moduel_wrap1 moduel_wrap3">
                        <div class="searcherForm">
                            <span class="searchSPn">搜索：</span> 省份：
                            <asp:DropDownList ID="ddlPro" runat="server">
                                <asp:ListItem Value="0">-请选择-</asp:ListItem>
                            </asp:DropDownList>
                            城市：
                            <asp:DropDownList ID="ddlCity" runat="server">
                                <asp:ListItem Value="0">-请选择-</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <label for="">
                                景点名称：</label>
                            <asp:TextBox ID="SpotName" runat="server"></asp:TextBox>
                            <a href="javascript:void(0);" title="" class="searchSubmit" title="提交">
                                <img src="/images/page_icon_29.gif" alt="" /></a>
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
                            <asp:Repeater ID="rptTourList" runat="server">
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
                                            <%#DisponseStar(Convert.ToInt32(Eval("Start")))%>
                                        </td>
                                        <td>
                                            <%#Eval("ProvinceName")%>_<%#Eval("CityName")%>
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

        <script src="/js/slogin.js" type="text/javascript"></script>

        <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

        <script type="text/javascript">
            var Sight = {
                OnSearch: function() {
                    var parms = { ProvinceId: "", Cityid: "", SoptName: "" };
                    parms.ProvinceId = $("#<%=ddlPro.ClientID %>").val();
                    parms.Cityid = $("#<%=ddlCity.ClientID %> ").val();
                    parms.SoptName = $("#<%=SpotName.ClientID %>").val();
                    window.location.href = "/GroupEnd/wh/SightRouting.aspx?" + $.param(parms);
                }
            }

            $(function() {
                $(".searchSubmit").click(function() {
                    Sight.OnSearch();
                    return false;
                });

                $("#<%=SpotName.ClientID %>").bind("keypress", function(e) {
                    if (e.keyCode == 13) {
                        Sight.OnSearch();
                        return false;
                    }
                });

                //省份选择事件
                $("#<%=ddlPro.ClientID %>").change(function() {
                    $("#<%=ddlCity.ClientID %>").html("<option value='0'>-请选择-</option>");
                    //公司ID后台取得
                    var id = '<%=companyId %>';
                    //参数
                    var params = '{proId:"' + $(this).val() + '",companyId:"' + id + '"}';
                    $.newAjax({
                        type: "POST",
                        //调用当前页面的BindCity方法
                        url: "SightRouting.aspx/BindCity",
                        contentType: "application/json; charset=utf-8",
                        //传递的参数
                        data: params,
                        success: function(msg) {
                            //将返回来的项添加到下拉菜单中 
                            $("#<%=ddlCity.ClientID %>").append(decodeURI(eval('(' + msg + ')').d));
                        }
                    });
                });

                //景点详细页
                $(".sightDetail").click(function() {
                    var id = $(this).next("[name='hidvalue']").val();
                    var url = "/GroupEnd/wh/SightDetail.aspx?Supplierid=" + id;
                    parent.Boxy.iframeDialog({
                        iframeUrl: url,
                        title: "景点详情",
                        modal: true,
                        width: "825px",
                        height: "530px"
                    });
                    return false;
                });
            })         
       
        </script>
    </form>
</body>
</html>
