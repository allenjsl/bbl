<%@ Page Title="机票政策" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="TickePoliyList.aspx.cs" Inherits="Web.systemset.ToGoTerrace.TickePoliyList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">系统设置</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> <a href="#">系统设置 >> 同行平台</a>>> 机票政策
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                    <tr>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/BaseManage.aspx">基础设置</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/RotateImg.aspx">轮换图片</a>
                        </td>
                        <td width="100" align="center" class="xtnav-on">
                            <a href="javascript:void(0)">机票政策</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/FriendshipLink.aspx">友情链接</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="btnbox">
                <table width="15%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="center">
                            <a href="javascript:;" class="add">新 增</a>
                        </td>
                        <%--<td width="90" align="center">
                            <a href="javascript:;" class="delall">批量删除</a>
                        </td>--%>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tbody>
                        <tr>
                            <%--<th width="3%" height="30" align="center" bgcolor="#BDDCF4">
                                全选<input type="checkbox" name="checkbox" class="selall" />
                            </th>--%>
                            <th width="85%" align="center" bgcolor="#BDDCF4">
                                标题
                            </th>
                            <th width="10%" align="center" bgcolor="#BDDCF4">
                                操作
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <tr tid="<%# Eval("Id") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <%--<td height="30" align="center">
                                        <input type="checkbox" name="checkbox" class="checkbox" />
                                    </td>--%>
                                    <td align="center">
                                        <%# EyouSoft.Common.Utils.GetText( Eval("Title").ToString(),90,true)%>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0);" class="modify">修改</a> <a href="javascript:void(0);"
                                            class="del">删除</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (len == 0)
                          { %>
                        <tr align="center">
                            <td colspan="2">
                                没有相关数据
                            </td>
                        </tr>
                        <%} %>
                    </tbody>
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
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        $(function() {
            //删除原有图片
            $("img.close").click(function() {
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //生成添加协议的控件
            });
            $("#Save").click(function() {
                var form = $(this).closest("form").get(0); //查找form
                form.submit();
            });
            $("a.add").click(function() {
                Others.add(); //添加
                return false;
            });
            $("a.modify").click(function() {
                Others.modify($(this)); //修改
                return false;
            });
            $("a.del").click(function() {
                Others.del($(this)); //删除
                return false;
            });
            $("a.delall").click(function() {
                Others.delall(RowList); //批量删除
                return false;
            });
        })
        var Others = {
            add: function() {
                var url = "/systemset/ToGoTerrace/TicketPolicyAdd.aspx";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "新增",
                    modal: true,
                    width: "820px",
                    height: "400px",
                    afterHide: function() {
                        window.parent.location.href = "/systemset/ToGoTerrace/TickePoliyList.aspx";
                    }
                });
                return false;
            },
            modify: function(that) {
                var url = "/systemset/ToGoTerrace/TicketPolicyAdd.aspx?type=modify&";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "修改",
                    modal: true,
                    width: "820",
                    height: "400px"
                });
                return false;
            },
            del: function(that) {
                if (confirm("确认删除所选项?")) {
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/systemset/ToGoTerrace/TickePoliyList.aspx?act=areadel",
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
                    var tid = RowList.GetCheckedValueList().join(',');
                    if (!tid) {
                        alert("没有数据不能删除!"); return false;
                    }
                    if (confirm("确认删除所选项?")) {

                        $.newAjax({
                            url: "/systemset/ToGoTerrace/TickePoliyList.aspx?act=areadel",
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
    
    </script>

    </form>
</asp:Content>
