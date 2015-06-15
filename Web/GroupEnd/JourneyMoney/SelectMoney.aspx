<%@ Page Title="我要询价" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
    CodeBehind="SelectMoney.aspx.cs" Inherits="Web.GroupEnd.JourneyMoney.SelectMoney" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tbody>
                        <tr>
                            <td>
                                <span class="lineprotitle">行程报价</span>
                            </td>
                            <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right">
                                所在位置&gt;&gt; <a href="#">行程报价</a>&gt;&gt; 我要询价
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" bgcolor="#000000" height="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table class="xtnav" border="0" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td class="xtnav-on" align="center">
                                <a href="#">我要询价</a>
                            </td>
                            <td align="center">
                                <a href="DownloadMoney.aspx">下载报价及行程</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="btnbox">
                <table align="left" border="0" cellpadding="0" cellspacing="0" width="30%">
                    <tbody>
                        <tr>
                            <td align="center" width="90">
                                <a href="javascript:;" class="add">新 增</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:;" class="update">修 改</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:;" class="alldel">删 除</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table border="0" cellpadding="0" cellspacing="1" width="99%">
                    <tr>
                        <th align="center" bgcolor="#bddcf4" height="28" width="6%">
                            选择
                        </th>
                        <td align="center" bgcolor="#bddcf4" width="28%">
                            <strong>线路名称</strong>
                        </td>
                        <td align="center" bgcolor="#bddcf4" width="16%">
                            <strong>出团日期</strong>
                        </td>
                        <th align="center" bgcolor="#bddcf4" width="10%">
                            人数
                        </th>
                        <th align="center" bgcolor="#bddcf4" width="10%">
                            询价人
                        </th>
                        <th align="center" bgcolor="#bddcf4" width="16%">
                            状态
                        </th>
                        <td align="center" bgcolor="#bddcf4" width="10%">
                            <strong>操作</strong>
                        </td>
                    </tr>
                    <asp:Repeater ID="retList" runat="server">
                        <ItemTemplate>
                            <tr tid="<%# Eval("Id") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <th align="center" height="28" width="6%">
                                    <input type="checkbox" name="checkbox" class="checkbox" />
                                </th>
                                <td align="center" width="28%">
                                    <%# Eval("RouteName") %>
                                </td>
                                <td align="center" width="16%">
                                    <%# Eval("LeaveDate", "{0:yyyy-MM-dd}")%>
                                </td>
                                <td align="center" width="10%">
                                    <%#Eval("PeopleNum") %>
                                </td>
                                <td align="center" width="10%">
                                    <%# Eval("ContactName") %>
                                </td>
                                <td align="center" width="16%">
                                    <%#Eval("QuoteState") %>
                                </td>
                                <td align="center" width="10%">
                                    <a href="javascript:void(0);" class="modify">
                                        <%# (EyouSoft.Model.EnumType.TourStructure.QuoteState)Eval("QuoteState") == EyouSoft.Model.EnumType.TourStructure.QuoteState.已成功 ? "查看" : "修改"%>
                                        </a> <a href="javascript:void(0);" class="del">删除</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td height="30" colspan="7" align="right" valign="middle" bgcolor="#FFFFFF" class="pageup">
                            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <script type="text/javascript">
            var operate = {


                add: function() {//跳转新增页面
                    window.location.href = "/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx?type=Add";
                    return false;
                },


                update: function(tid) {//跳转修改页面
                    if (tid == null) {
                        var url = "/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx?type=Update&tid=";
                        var chkval = "";
                        $("[name='checkbox']").each(function() {
                            if ($(this).attr("checked")) {
                                chkval += $(this).parent().parent().attr("tid") + ",";

                            }
                        });
                        if (chkval != "") {
                            if (chkval.split(",")[1] == "") {
                                var url = url + chkval.split(",")[0];

                                window.location.href = url;
                            } else {
                                alert("只能修改一条记录！");
                            }
                        } else {
                            alert("请选择一条记录！");
                        }
                        return false;
                    }
                    window.location.href = "/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx?type=Update&tid=" + tid;
                },


                del: function(tid) {
                    if (confirm("确认删除所选项?")) {
                        $.newAjax({
                            type: "POST",
                            data: { "tid": tid },
                            url: "/GroupEnd/JourneyMoney/SelectMoney.aspx?act=del",
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
                },
                delall: function(RowList) {
                    if (RowList.GetListSelectCount() > 0) {
                        if (confirm("确认删除所选项?")) {
                            var tid = RowList.GetCheckedValueList().join(',');
                            $.newAjax({
                                url: "/GroupEnd/JourneyMoney/SelectMoney.aspx?act=del",
                                type: "POST",
                                data: { "tid": tid },
                                dataType: 'json',
                                success: function(d) {
                                    if (d.res) {
                                        switch (d.res) {
                                            case 1:
                                                alert("删除成功");
                                                $(':checkbox').attr("checked", "");
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
                }

            }
            $(function() {
                $("a.add").click(function() {
                    operate.add();
                    return false;
                });
                $("a.modify").click(function() {
                    var tid = $(this).parent().parent().attr("tid");
                    operate.update(tid);
                    return false;
                });
                $("a.update").click(function() {
                    operate.update(null);
                    return false;

                });

                $("a.del").click(function() {//删除
                    var tid = $(this).parent().parent().attr("tid");
                    operate.del(tid); //删除
                    return false;

                });
                $("a.alldel").click(function() {//批量删除

                    operate.delall(RowList); //删除
                    return false;
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

    </div>
</asp:Content>
