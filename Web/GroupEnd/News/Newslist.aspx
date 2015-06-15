<%@ Page Title="最新动态" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
    CodeBehind="Newslist.aspx.cs" Inherits="Web.GroupEnd.News.Newslist" %>

<%@ Register Src="~/UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">最新动态</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置 >> 首页 >> 最新动态
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <label>
                                标题：</label>
                            <input type="text" class="searchinput" id="Titles" value="<%=Titles %>" />
                            <label>
                                发布人：</label>
                            <input type="text" class="searchinput" id="OperateName" value="<%=OperateName %>" />
                            <label>
                                发布时间：</label>
                            <input type="text" class="searchinput" id="IssueTime" onfocus="WdatePicker()" value="<%=IssueTime%>" />
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
            <div class="hr_10">
            </div>
            <div class="tablelist" style="border-top: 2px #000000 solid;">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tbody>
                        <tr>
                            <%--<th width="4%" height="30" align="center" bgcolor="#BDDCF4">
                                全选<input type="checkbox" name="checkbox" class="selall" />
                            </th>--%>
                            <th width="37%" align="center" bgcolor="#bddcf4">
                                标题
                            </th>
                            <th width="19%" align="center" bgcolor="#bddcf4">
                                发布人
                            </th>
                            <th width="18%" align="center" bgcolor="#bddcf4">
                                发布时间
                            </th>
                            <th width="15%" align="center" bgcolor="#bddcf4">
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
                                        <a name="ResultA" target="_blank" href="javascript:void(0);" class="show">
                                            <%#
                                                EyouSoft.Common.Utils.GetText(Eval("Title").ToString(),50)%></a>
                                    </td>
                                    <td align="center">
                                        <%# Eval("OperateName")%>
                                    </td>
                                    <td align="center">
                                        <%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0);" class="show">查看</a>
                                        <%--<%  if (grantmodify)
                                          { %><a href="javascript:void(0);" class="modify">修改</a>
                                        <%} if (grantdel)
                                          { %><a href="javascript:void(0);" class="del">删除</a><%} %>--%>
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

    <script type="text/javascript">
        var Others = {
            select: function() {
                var Titles = $.trim($("#Titles").val());
                var OperateName = $.trim($("#OperateName").val());
                var IssueTime = $.trim($("#IssueTime").val());
                //参数集
                var para = { Titles: "",
                    OperateName: "",
                    IssueTime: ""

                };
                para.Titles = Titles;
                para.OperateName = OperateName;
                para.IssueTime = IssueTime;
                window.location.href = "/GroupEnd/News/Newslist.aspx?" + $.param(para);
                return false;
            },
            show: function(that) {
                var url = "/GroupEnd/News/Newsshow.aspx?";
                var tid = that.parent().parent().attr("tid");
                Boxy.iframeDialog({
                    iframeUrl: url + "tid=" + tid,
                    title: "查看",
                    modal: true,
                    width: "820px",
                    height: "330px"
                });
                return false;

            },
            del: function(that) {
                if (confirm("确认删除所选项?")) {
                    var tid = that.parent().parent().attr("tid");
                    $.newAjax({
                        type: "POST",
                        data: { "tid": tid },
                        url: "/SupplierControl/Others/Default.aspx?act=areadel",
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
                            url: "/SupplierControl/Others/Default.aspx?act=areadel",
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
            },
            toxls: function() {
                url = location.pathname + location.search;
                if (url.indexOf("?") >= 0) { url += "&act=toexcel"; } else { url += "?act=toexcel"; }
                window.location.href = url;
                return false;
            }
        }

        $(function() {

            $("a.loadxls").click(function() {
                Others.loadxls(); //导入
                return false;
            });
            $("a.add").click(function() {
                Others.add(); //添加
                return false;
            });
            $("a.show").click(function() {
                Others.show($(this)); //查看
                return false;

            });

            $("a.trade").click(function() {
                Others.trade($(this)); //查看交易记录
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

            $("#searchbtn").click(function() {
                Others.select(); //查询
                return false;
            });
            $(".searchinput").keypress(function(e) {
                if (e.keyCode == 13) {
                    Others.select(); //查询
                    return false;
                }
            });
            $("a.toexcel").click(function() {
                Others.toxls(); //导出
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


            //查询按钮
            $("#Btn_Serach").click(function() {
                LineProductList.OnSearch();
                return false;
            });
            //回车查询
            $("#LineListSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    LineProductList.OnSearch();
                    return false;
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
