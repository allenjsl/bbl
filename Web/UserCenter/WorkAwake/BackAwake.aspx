<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="BackAwake.aspx.cs" Inherits="Web.UserCenter.WorkAwake.BackAwake"
    Title="回团提醒_个人中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .FlightTD
        {
            border-top: solid 1px #fff;
            border-right: solid 1px #fff;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <% if (!string.IsNullOrEmpty(Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 2)))
       { %>
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">个人中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> <a href="#">个人中心</a>>> 信息管理
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 30px;">
                <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                        <% =Web.Common.AwakeTab.createAwakeTab(this.SiteUserInfo, 4)%>
                    </tr>
                </table>
            </div>
            <div class="btnbox">
                <%if (setday)
                  { %>
                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            设置提前
                            <input name="textfield" type="text" size="10" class="duetime" value="<% =day %>" />
                            天提醒
                            <input type="button" value="保存" class="save" />
                        </td>
                    </tr>
                </table>
                <%} %>
            </div>
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr bgcolor="#BDDCF4">
                        <th width="9%" align="center">
                            团号
                        </th>
                        <th width="12%" align="center">
                            线路名称
                        </th>
                        <th width="14%" align="center">
                            组团社
                        </th>
                        <th width="8%" align="center">
                            联系人
                        </th>
                        <th width="8%" align="center">
                            联系电话
                        </th>
                        <th width="12%" align="center">
                            回团日期
                        </th>
                        <th width="8%" align="center">
                            人数
                        </th>
                        <th width="15%" align="center">
                            责任计调
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                        <ItemTemplate>
                            <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="center">
                                    <%# Eval("TourCode")%>
                                </td>
                                <td align="left" class="pandl3">
                                    <%# Eval("RouteName")%>
                                </td>
                                <td align="center" colspan="3" style="padding: 0px; margin: 0px">
                                    <table width="100%" border="0" cellpadding="0" cellspacing="1" style="border: none;border-collapse: collapse;" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                        <asp:Repeater ID="rptAgencyInfo" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 42%;  border-right: 1px solid #fff;"
                                                        align="center">
                                                        <%#Eval("AgencyName")%>
                                                    </td>
                                                    <td style="width: 24%;  border-right: 1px solid #fff;"
                                                        align="center">
                                                        <%#Eval("ContactName")%>
                                                    </td>
                                                    <td style="width: 24%; " align="left">
                                                        <%#Eval("Telephone")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </td>
                                <td align="center">
                                    <%# Convert.ToDateTime(Eval("BackDate")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%# Eval("PeopleCount")%>
                                </td>
                                <td align="center">
                                    <%# Eval("JobName")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%if (len == 0)
                      { %>
                    <tr align="center">
                        <td colspan="5">
                            没有相关数据
                        </td>
                    </tr>
                    <%} %>
                </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="13">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <%}
       else
       {%>
    <div style="height: 500px">
    </div>
    <%} %>

    <script type="text/javascript">
        $(function() {
        $(".tbl").each(function() {
            var height = $(this).parent("td").parent("tr");
            $(this).height(height.height() + 10);
        })
            $("input.save").click(function() {
                var r = /^\+?[1-9][0-9]*$/;
                var duetime = $("input.duetime").val();
                if (r.test(duetime)) {
                    $.newAjax({
                        url: "/UserCenter/WorkAwake/BackAwake.aspx?type=duedayset",
                        type: "POST",
                        data: { "num": duetime },
                        dataType: 'json',
                        cache: false,
                        success: function(d) {
                            switch (d.ret) {
                                case 1:
                                    alert("设置成功");
                                    location.reload();
                                    break;
                                case -1:
                                    alert("服务器繁忙");
                                    break;
                                case -2:
                                    alert("没有设置权限");
                                    break;
                            }
                        }
                    });
                } else {
                    alert("请正确设定日期");
                }
            })
        });
    </script>

</asp:Content>
