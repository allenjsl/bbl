<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxSingleServeList.aspx.cs"
    Inherits="Web.SingleServe.AjaxSingleServeList" EnableEventValidation="false"
    EnableViewState="false" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<form runat="server" id="form1">
<div class="btnbox">
    <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
        <tr>
            <td width="90" align="center">
                <a href="/SingleServe/SingleServeInfo.aspx" runat="server" id="linkAdd">新 增</a>
            </td>
            <td width="90" align="center">
                <a href="javascript:void(0);" id="linkUpdate" runat="server">修 改</a>
            </td>
            <td width="90" align="center">
                <asp:linkbutton runat="server" onclientclick="return SingleServe.OnBatchDelete();"
                    id="linkDelete" text="删 除" onclick="linkDelete_Click"></asp:linkbutton>
            </td>
        </tr>
    </table>
</div>
<div class="tablelist" id="rpt_SingleServeList">
    <table width="100%" border="0" cellpadding="0" cellspacing="1">
        <tr>
            <th width="3%" align="center" bgcolor="#BDDCF4">
                <input type="checkbox" onclick="SingleServe.CheckAll(this)" />
            </th>
            <th width="12%" align="center" bgcolor="#BDDCF4">
                订单号
            </th>
            <th align="center" bgcolor="#bddcf4" width="7%">
                团号
            </th>
            <th width="12%" align="center" bgcolor="#bddcf4">
                客户单位
            </th>
            <th width="10%" align="center" bgcolor="#bddcf4">
                联系人
            </th>
            <th width="12%" align="center" bgcolor="#bddcf4">
                电话
            </th>
            <th width="14%" align="center" bgcolor="#bddcf4">
                服务类别
            </th>
            <th width="10%" align="center" bgcolor="#bddcf4">
                操作员
            </th>
            <th width="8%" align="center" bgcolor="#bddcf4">
                状态
            </th>
            <th width="12%" align="center" bgcolor="#bddcf4">
                操作
            </th>
        </tr>
        <cc1:CustomRepeater ID="crp_SingleServeList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center" bgcolor="#e3f1fc">
                        <input type="checkbox" value="<%# Eval("TourId") %>" name="ckSingleServe"  <%#(int)Eval("Status")==4||(int)Eval("Status")==5?"disabled":"" %>/>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("OrderNo")%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("TourCode")%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyerId") %>">
                            <%#Eval("BuyerCName")%></a>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("BuyerContacterName") %>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("BuyerContacterTelephone")%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("Services")%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#Eval("OperatorName")%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <%#  GetSingleServeStatu(Convert.ToString(Eval("Status")))%>
                    </td>
                    <td align="center" bgcolor="#e3f1fc">
                        <% if (EditFlag)
                           { %>
                        <a href="javascript:void(0);" onclick="SingleServe.UpdateSingleServe('<%#Eval("TourId") %>');">
                            <%# EyouSoft.Common.Utils.PlanIsUpdateOrDelete(Convert.ToString(Eval("Status")))==true?"修改":"查看"%>
                        </a>
                        <% } if (DeleteFlag)
                           {%>
                        <a href="javascript:void(0);" onclick="SingleServe.DeleteSingleServe('<%#Eval("TourId") %>');">
                           <%# EyouSoft.Common.Utils.PlanIsUpdateOrDelete(Convert.ToString(Eval("Status")))==true?"删除":""%>  </a><%} %>
                        <%# GetPrintUrl(Eval("TourId").ToString())%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center" bgcolor="#BDDCF4">
                        <input type="checkbox" value="<%# Eval("TourId") %>" name="ckSingleServe" <%#(int)Eval("Status")==4||(int)Eval("Status")==5?"disabled":"" %>/>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("OrderNo")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("TourCode")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyerId") %>">
                            <%#Eval("BuyerCName")%></a>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("BuyerContacterName") %>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("BuyerContacterTelephone")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("Services")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#Eval("OperatorName")%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                        <%#GetSingleServeStatu(Convert.ToString(Eval("Status")))%>
                    </td>
                    <td align="center" bgcolor="#BDDCF4">
                      <% if (EditFlag)
                           { %>
                        <a href="javascript:void(0);" onclick="SingleServe.UpdateSingleServe('<%#Eval("TourId") %>');">
                            <%# EyouSoft.Common.Utils.PlanIsUpdateOrDelete(Convert.ToString(Eval("Status")))==true?"修改":"查看"%>
                        </a>
                        <% } if (DeleteFlag)
                           {%>
                        <a href="javascript:void(0);" onclick="SingleServe.DeleteSingleServe('<%#Eval("TourId") %>');">
                           <%# EyouSoft.Common.Utils.PlanIsUpdateOrDelete(Convert.ToString(Eval("Status")))==true?"删除":""%>  </a><%} %>
                        <%# GetPrintUrl(Eval("TourId").ToString())%>
                    </td>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </cc1:CustomRepeater>
        <table width="99%" runat="server" id="tbl_ExportPage" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td height="30" align="right">
                    <cc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" />
                </td>
            </tr>
        </table>
</div>
</form>

<script type="text/javascript">
    //定义一个JS单项服务对象
    var SingleServe = {
        DeleteSingleServe: function(TourId) {//项删除单项服务
            if (confirm('您确定要删除此单项服务信息吗？\n\n此操作不可恢复！')) {
                $.newAjax
                ({
                    type: "POST",
                    url: "/SingleServe/AjaxSingleServeList.aspx?DeleteId=" + TourId,
                    cache: false,
                    success: function(html) {
                        if (html == "0") {
                            alert("删除失败!");
                            return false;
                        } else {
                            alert("删除成功!");
                            location.href = "/SingleServe/SingleServeList.aspx";
                        }
                    },
                    error: function() {
                        alert("对不起，操作失败!");
                    }
                });
                return false;
            }
        },
        UpdateSingleServe: function(TourId) {//修改 
            Boxy.iframeDialog({
                iframeUrl: "/SingleServe/SingleServeInfo.aspx?EditId=" + TourId,
                title: "修改",
                modal: true,
                width: "980px",
                height: "507px"
            });
            return false;
        },
        OnBatchDelete: function() {//批量删除
            var CheckedCount = $(":checkbox[name='ckSingleServe'][checked='true']").length;
            if (CheckedCount <= 0) {
                alert("请选择您要删除的数据项!");
                return false;
            }
            return confirm("您确定要执行删除吗?");
        },
        //全选
        CheckAll: function(obj) {
            $("input[name='ckSingleServe']").each(function() {
                $(this).attr("checked", $(obj).attr("checked"));
            });
        }

    };
    //////////////////////////////////////////////////////////////////////////
    $(document).ready(function() {
        $("#linkUpdate").click(function() {//选择性的修改，一次只能修改一条记录
            var count = $(":checkbox[name='ckSingleServe'][checked='true']").length;
            if (count == 0) {
                alert("请选择修改项!");
                return false;
            }
            if (count > 1) {
                alert("只能修改一个!");
                return false;
            }
            var CurrServerId = $(":checkbox[name='ckSingleServe'][checked='true']:eq(0)").val();
            Boxy.iframeDialog({
                iframeUrl: '/SingleServe/SingleServeInfo.aspx?EditId=' + CurrServerId,
                title: "修改",
                modal: true,
                width: "980px",
                height: "507px"
            });
            return false;
        });
        $("#linkAdd").click(function() {//弹出新增窗口
            var url = $(this).attr("href");
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "新增",
                modal: true,
                width: "980px",
                height: "507px",
                scrolling: "yes"
            });
            return false;
        });
        //打开客户单位信息页
        $(".openByerInfo").click(function() {
            var buyerId = $(this).attr("ref");
            Boxy.iframeDialog({ title: "客户单位信息", iframeUrl: "/CRM/customerinfos/CustomerDetails.aspx?cId=" + buyerId, width: "775px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
            return false;
        });

    });
</script>