<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="TicketService.aspx.cs" Inherits="Web.SupplierControl.TicketService.TicketService"
    Title="票务_供应商管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">供应商管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置 >>  供应商管理 >> 售票处
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" alt="" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                省份：</label>
                            <uc1:ucProvince runat="server" ID="ucProvince1" />
                            <label>
                                城市：</label>
                            <uc1:ucCity runat="server" ID="ucCity1" />
                            <label>
                                单位名称：</label>
                            <input type="text" class="searchinput" id="txtUnionName" value="<%=unionName %>" />
                            <label>
                                <a href="javascript:void(0);" id="searchbtn">
                                    <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" alt="" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <%if (grantadd)
                          { %><td width="90" align="center">
                              <a href="javascript:;" class="add">新 增</a>
                          </td>
                        <%} %>
                        <%if (grantdel)
                          { %><td width="90" align="center">
                              <a href="javascript:;" class="delall">批量删除</a>
                          </td>
                        <%} %>
                        <%if (grantload)
                          { %><td width="90" align="center">
                              <a href="javascript:;" class="loadxls">
                                  <img src="/images/daoru.gif" />
                                  导 入</a>
                          </td>
                        <%} %>
                        <%if (grantto)
                          { %><td width="90" align="center">
                              <a href="javascript:;" class="toexcel">
                                  <img src="/images/daoru.gif" alt="" width="16" height="16" />
                                  导 出</a>
                          </td>
                        <%} %>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="5%" height="30" align="center" bgcolor="#BDDCF4">
                            全选<input type="checkbox" name="checkbox" class="selall" />
                        </th>
                        <th width="12%" align="center" bgcolor="#BDDCF4">
                            所在地
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            单位名称
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            联系人
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            电话
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            传真
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            交易情况
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            政策
                        </th>
                        <th width="15%" align="center" bgcolor="#bddcf4">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr tid="<%# Eval("Id") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td height="30" align="center">
                                    <input type="checkbox" name="checkbox" class="checkbox" />
                                </td>
                                <td align="center">
                                    <p>
                                        <%# Eval("ProvinceName")%>
                                        <%# Eval("CityName")%></p>
                                </td>
                                <td align="center">
                                    <%# Eval("UnitName")%>
                                </td>
                                <td align="center">
                                    <%# Eval("SupplierContact") == null ? "" : (Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactName%>
                                </td>
                                <td align="center">
                                    <%# Eval("SupplierContact") == null ? "" : (Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactTel%>
                                </td>
                                <td align="center">
                                    <%# Eval("SupplierContact") == null ? "" : (Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactFax%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="trade">共计<%# Eval("TradeNum")%>次</a>
                                </td>
                                <td align="center">
                                    <%# Eval("UnitPolicy") %>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="show">查看</a>
                                    <%if (grantmodify)
                                      { %><a href="javascript:;" class="modify">修改</a>
                                    <%} if (grantdel)
                                      { %>
                                    <a href="javascript:;" class="del">删除</a><%} %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <tr>
                        <th width="5%" height="30" align="center" bgcolor="#BDDCF4">
                            总计
                        </th>
                        <th width="12%" align="center" bgcolor="#BDDCF4">
                            
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            <asp:Label ID="lblAllCount" runat="server" Text=""></asp:Label>
                        </th>
                        <th width="12%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                        <th width="15%" align="center" bgcolor="#bddcf4">
                            
                        </th>
                    </tr>
                    <%if (len == 0)
                      { %>
                    <tr align="center">
                        <td colspan="8">
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

    <script type="text/javascript">
        $(function() {
            $("a.loadxls").click(function() {
                var url = "/SupplierControl/TicketService/TicketLoadExcel.aspx";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "票务EXCEL",
                    modal: true,
                    width: "820px",
                    height: "400px"
                });
                return false;
            });

            $("a.add").click(function() {
                var url = "/SupplierControl/TicketService/TicketAdd.aspx";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "票务新增",
                    modal: true,
                    width: "820px",
                    height: "380px"
                });
                return false;
            });

            $("a.modify").click(function() {
                var that = $(this);
                var url = "/SupplierControl/TicketService/TicketAdd.aspx?type=modify&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "820",
                    height: "380px"
                });
                return false;
            });
            $("a.show").click(function() {
                var that = $(this);
                var url = "/SupplierControl/TicketService/TicketAdd.aspx?type=show&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "820",
                    height: "380px"
                });
                return false;
            });
            $("a.trade").click(function() {
                var that = $(this);
                var url = "/SupplierControl/TicketService/TradeSituation.aspx";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "?tid=" + tid,
                    title: "交易情况",
                    modal: true,
                    width: "820",
                    height: "560px"
                });
                return false;
            });
            $("a.del").click(function() {
                if (confirm("确认删除所选项?")) {
                    var that = $(this);
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/SupplierControl/TicketService/TicketService.aspx?act=ticketdel",
                        dataType: 'json',
                        success: function(msg) {
                            if (msg.res) {
                                switch (msg.res) {
                                    case 1:
                                        alert("删除成功!");
                                        location.reload();
                                        break;
                                    default:
                                        alert("删除失败!");
                                }
                            }
                        },
                        error: function() {
                            alert("服务器繁忙!");
                        }
                    });
                }
                return false;
            });
            $("a.delall").click(function() {

                if (RowList.GetListSelectCount() > 0) {
                    if (confirm("确认删除所选项?")) {
                        var tid = RowList.GetCheckedValueList().join(',');
                        if (!tid) {
                            alert("没有数据不能删除");
                            return false;
                        }
                        $.newAjax({
                            url: "/SupplierControl/TicketService/TicketService.aspx?act=ticketdel",
                            type: "POST",
                            data: { "tid": tid },
                            dataType: 'json',
                            success: function(d) {
                                if (d.res) {
                                    switch (d.res) {
                                        case 1:
                                            alert("删除成功");
                                            location.reload();
                                            break;
                                        case -1:
                                            alert("删除失败");
                                            break;
                                    }
                                }
                            },
                            error: function() {
                                alert("服务器繁忙!");
                            }
                        });
                    }
                } else {
                    alert("请选择要删除的");
                }
                return false;
            });

            $("#searchbtn").click(function() {

                var province = $("#uc_provinceDynamic").val();
                var city = $("#uc_cityDynamic").val();
                var unionName = $.trim($("#txtUnionName").val());
                //参数
                var para = { province: "", city: "", unionName: "" };
                para.province = province;
                para.city = city;
                para.unionName = unionName;
                window.location.href = "/SupplierControl/TicketService/TicketService.aspx?" + $.param(para);
                return false;
            });
            $("a.toexcel").click(function() {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&act=toexcel"; } else { url += "?act=toexcel"; }
                window.location.href = url;
                return false;
            });

            ///全选事件
            $("input.selall").click(function() {
                var that = $(this);
                if (that.attr("checked")) {
                    RowList.SetAllCheckValue("checked");
                } else {
                    RowList.SetAllCheckValue("");
                }
            });

            var RowList = {
                GetListSelectCount: function() {
                    var count = 0;
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        if ($(this).attr("checked")) {
                            count++;
                        }
                    });
                    return count;
                },
                GetCheckedValueList: function() {
                    var arrayList = new Array();
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        if ($(this).attr("checked")) {
                            arrayList.push($(this).parent().parent().attr("tid"));
                        }
                    });
                    return arrayList;
                },
                SetAllCheckValue: function(obj) {
                    $(".tablelist").find("input.checkbox").each(function() {
                        $(this).attr("checked", obj);
                    });
                }
            };
        });
    </script>

    </form>
</asp:Content>
