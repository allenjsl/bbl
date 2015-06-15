<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trafficList.aspx.cs" Inherits="Web.jipiao.Traffic.trafficList"
    MasterPageFile="~/masterpage/Back.Master" Title="交通管理"%>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="head" ID="contentHead" runat="server">
<style type="text/css">
.lable{ margin-left:450px;}
</style>
</asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="contentBody" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">交通管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        <b>当前用您所在位置：</b> >> 交通管理 >> 交通列表
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
        <form action="trafficList.aspx" method="get">
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            交通名称：</label>
                        <input type="text" id="trfficName" class="searchinput searchinput02" name="trfficName"
                            value="<%=EyouSoft.Common.Utils.GetQueryStringValue("trfficName") %>" />
                        <label>
                          <a id="btnSubmit" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"  alt="" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        </form>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="left">
                        <a href="javascript:" id="link1" data_class="trafficAdd">新增交通</a>
                    </td>
                    <td width="90" align="left">
                        <a href="javascript:" class="link2" data_class="trafficUpdate">修 改</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                        全选
                        <input type="checkbox" id="checkAll" />
                    </th>
                    <th width="40%" align="center" bgcolor="#BDDCF4">
                        交通名称
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        天数
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        儿童价
                    </th>
                    <th width="6%" align="center" bgcolor="#bddcf4">
                        状态
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="repTrfficList" runat="server">
                    <ItemTemplate>
                        <tr class='<%#Container.ItemIndex %2==0?"even":"odd" %>'>
                            <td height="30" align="center" bgcolor="#e3f1fc">
                                <input type="checkbox" name="trafficchecks" data_id="<%# Eval("TrafficId") %>" />
                            </td>
                            <td align="left" bgcolor="#e3f1fc" class="pandl3">
                                <%# Eval("TrafficName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("TrafficDays")%>天
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# EyouSoft.Common.Utils.GetDecimal(Eval("ChildPrices").ToString()).ToString("0.00") == "0.00" ? "按成人价" : EyouSoft.Common.Utils.GetDecimal(Eval("ChildPrices").ToString()).ToString("0.00")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("Status")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="/jipiao/Traffic/trafficAdd.aspx?trfficID=" data_trfficid="<%# Eval("TrafficId") %>"
                                    data_class="update">修改</a> | <a href="javascript:" data_status="status" data_trafficid="<%# Eval("TrafficId") %>"
                                        data_statu="<%# Eval("Status").ToString() == "停售"?0:1 %>">
                                        <%#Eval("Status").ToString() == "停售" ? "正常" : "停售"%>
                                    </a>| <a href="/jipiao/Traffic/travelList.aspx?trfficID=<%# Eval("TrafficId") %>">行程设定</a> | 
                                    <a href="/jipiao/Traffic/trafficpricesList.aspx?trfficID=<%# Eval("TrafficId") %>">价格</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <asp:Label ID="labMsg" runat="server" CssClass="lable"></asp:Label>
                    <td align="right" >                        
                        <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        $(function() {
            //新增
            $("a[data_class='trafficAdd']").click(function() {
                Boxy.iframeDialog({ title: "新增", iframeUrl: "/jipiao/Traffic/trafficAdd.aspx?type=add", width: "700px", height: "130px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            //修改
            $("a[data_class='trafficUpdate']").click(function() {
                var chkList = $(".tablelist").find("input[type='checkbox'][name='trafficchecks']:checked");
                var chkLength = chkList.length;
                if (chkLength == 1) {
                    var id = chkList.attr("data_id");
                    Boxy.iframeDialog({ title: "修改", iframeUrl: "/jipiao/Traffic/trafficAdd.aspx?type=update&trfficID=" + id, width: "700px", height: "130px", draggable: true, data: null, hideFade: true, modal: true });
                } else if (chkLength == 0) {
                    alert("请选择一条记录进行修改!");
                }
                else {
                    alert("只能选择一条记录进行修改!");
                }
                return false;
            });

            //修改
            $("a[data_class='update']").click(function() {
                var id = $(this).attr("data_trfficID");
                Boxy.iframeDialog({ title: "修改", iframeUrl: "/jipiao/Traffic/trafficAdd.aspx?type=update&trfficID=" + id, width: "700px", height: "130px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });

            //全选
            $("#checkAll").click(function() {
                if ($(this).attr("checked")) {
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        $(this).attr("checked", "checked");
                    })
                } else {
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        $(this).attr("checked", "");
                    });
                }
            });
            //设置状态 
            $("a[data_status='status']").click(function() {
                var trafficId = $(this).attr("data_trafficid");
                var status = $(this).attr("data_statu");
                if (trafficId != null && trafficId != undefined && trafficId != '') {
                    $.ajax({
                        type: 'GET',
                        url: '/jipiao/Traffic/trafficStatus.ashx?type=status&ID=' + trafficId + "&status=" + status,
                        cache: false,
                        success: function(ret) {
                            var obj = eval('(' + ret + ')');
                            alert(obj.msg);
                            window.location.reload();
                        },
                        error: function() {
                            alert("服务器繁忙，请稍后在试!");
                        }
                    });
                }
                return false;
            });

            //查询
            $("#btnSubmit").click(function() {
                var tfName = $.trim($("#trfficName").val());
                var params = { trfficName: tfName };
                window.location.href = "/jipiao/Traffic/trafficList.aspx?" + $.param(params);
                return false;
            });
        });
    </script>

</asp:Content>
