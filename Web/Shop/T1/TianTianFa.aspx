<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TianTianFa.aspx.cs" Inherits="Web.Shop.T1.TianTianFa" MasterPageFile="~/Shop/T1/Default.Master" Title="天天发计划" %>
<%--天天发计划--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
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
                <asp:Repeater ID="rpt" runat="server">
                    <ItemTemplate>
                        <tr align="center">
                            <td colspan="4">
                                <a target="_blank" title="<%#Eval("RouteName") %>" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType")) %>">
                                    <%#Eval("RouteName")%></a>
                            </td>
                            <td>
                                <%#GetAttachs(Eval("Attachs"))%>
                            </td>
                            <td>
                                <a href="javascript:void(0)" target="_blank" ref="/groupend/fithairday/fithairdaylist.aspx?tourid=<%#Eval("tourId") %>" class="apply">
                                    <img alt="提交申请" style="width: 66px; height: 18px" src="/images/tjsq.gif" /></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            
            <div style="text-align:center; margin-top:10px;" runat="server" id="divEmpty" visible="false">
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
            $(".apply").click(function() {
                var url = $(this).attr("ref");
                $.ajax({
                    type: "Get",
                    async: false,
                    url: "/shop/t1/default.aspx?doType=isLogin",
                    cache: false,
                    success: function(response) {
                        if (response == "0") {
                            window.location.href = url;
                        } else {
                            alert("您尚未登录,请先登录系统!");
                        }
                    }
                });
                return false;
            })

        });
    </script>
</asp:Content>
