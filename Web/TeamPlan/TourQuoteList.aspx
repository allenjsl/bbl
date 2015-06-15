<%@ Page Title="上团报价" Language="C#" AutoEventWireup="true" CodeBehind="TourQuoteList.aspx.cs"
    Inherits="Web.TeamPlan.TourQuoteList" MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="Form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">团队计划</span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt; 团队计划 &gt;&gt; 上传报价
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="../images/yuanleft.gif">
                    </td>
                    <td>
                        <div class="searchbox" id="searchbox">
                            <label>
                                标题：</label>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="searchinput"></asp:TextBox>
                            <label>
                                发布时间：</label>
                            <asp:TextBox ID="txtAddDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                            <label>
                                <a href="javascript:void(0);" id="btnSearch">
                                    <img style="vertical-align: top;" src="/images/searchbtn.gif"></a></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="../images/yuanright.gif">
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btnbox">
            <table cellspacing="0" cellpadding="0" border="0" align="left">
                <tbody>
                    <tr>
                        <td align="center">
                            <asp:Panel ID="pnlAdd" runat="server">
                                <a href="javascript:void(0);" id="btnAdd">新 增</a>
                            </asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="pnlUpdate" runat="server">
                                <a href="javascript:void(0);" id="btnUpdate">修 改</a></asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="penDelete" runat="server">
                                <a href="javascript:void(0);" id="btnDelete">删 除</a>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table cellspacing="1" cellpadding="0" border="0" width="100%" id="tablelist">
                <tbody>
                    <tr class="odd">
                        <th nowrap="nowrap" height="28" align="center" width="6%">
                            <input type="checkbox" id="cbxAll" />
                        </th>
                        <th nowrap="nowrap" align="center" width="31%">
                            标题
                        </th>
                        <th nowrap="nowrap" align="center" width="20%">
                            有效期
                        </th>
                        <th nowrap="nowrap" align="center" width="20%">
                            发布时间
                        </th>
                        <th nowrap="nowrap" align="center" width="20%">
                            负责人
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td nowrap="nowrap" align="center">
                                    <input type="checkbox" id="<%#Eval("id") %>" />
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="updateQuote" ref="<%#Eval("id") %>">
                                        <%#Eval("FileName")%></a>
                                </td>
                                <td align="center">
                                    <%#Eval("ValidityStart") == null ? "" : Convert.ToDateTime(Eval("ValidityStart")).ToString("yyyy-MM-dd")%>
                                    至
                                    <%#Eval("ValidityEnd")==null?"":Convert.ToDateTime(Eval("ValidityEnd")).ToString("yyyy-MM-dd") %>
                                </td>
                                <td align="center">
                                    <%#Eval("AddTime") == null ? "" : Convert.ToDateTime(Eval("AddTime")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <%#Eval("OperatorName")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="5">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                            <asp:Label ID="lblMsg" runat="server" Text="未找到数据!"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#btnAdd").click(function() {
                Boxy.iframeDialog({ title: "上传报价", iframeUrl: "/TeamPlan/TourQuoteAdd.aspx?type=add", width: "500px", height: "270px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })

            $("#btnSearch").click(function() {
                var title = $("#<%=txtTitle.ClientID %>").val();
                var addDate = $("#<%=txtAddDate.ClientID %>").val();
                var para = { title: "", addDate: "" }
                para.title = title;
                para.addDate = addDate;
                window.location.href = "/TeamPlan/TourQuoteList.aspx?" + $.param(para);
                return false;
            });

            $("#btnDelete").click(function() {
                if (TourQuoteList.GetListSelectCount() > 0) {
                    if (confirm("确定删除吗?")) {
                        var list = TourQuoteList.GetCheckedValueList();
                        if (list != null && list.length > 0) {
                            var idList = "";
                            for (var i = 0; i < list.length; i++) {
                                idList += list[i] + ",";
                            }
                            $.newAjax({
                                type: "Get",
                                url: "/TeamPlan/TourQuoteList.aspx?type=Delete&id=" + idList,
                                cache: false,
                                success: function(result) {
                                    if (result == "Ok") {
                                        alert("删除成功!");
                                        window.location.href = "/TeamPlan/TourQuoteList.aspx";
                                        return false;
                                    } else {
                                        alert("删除失败");
                                        return false;
                                    }
                                },
                                error: function() {
                                    alert("删除失败! 请稍后在试!");
                                    return false;
                                }
                            });
                        }
                    }
                } else {
                    alert("请选择一行数据!");
                }
                return false;
            })

            $("#btnUpdate").click(function() {
                if (TourQuoteList.GetListSelectCount() == 1) {
                    var list = TourQuoteList.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        var id = list[0];
                        Boxy.iframeDialog({ title: "上传报价", iframeUrl: "/TeamPlan/TourQuoteAdd.aspx?type=update&id=" + id, width: "500px", height: "270px", draggable: true, data: null, hideFade: true, modal: true });
                        return false;
                    }
                } else {
                    alert("请选择一条数据修改!");
                }
                return false;
            })

            $(".updateQuote").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "上传报价", iframeUrl: "/TeamPlan/TourQuoteAdd.aspx?type=update&id=" + id, width: "500px", height: "270px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })

            $("#searchbox input[type='text']").keydown(function(e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            })

            $("#cbxAll").click(function() {
                if ($(this).attr("checked")) {
                    $("#tablelist input[type='checkbox']").attr("checked", "checked");
                } else {
                    $("#tablelist input[type='checkbox']").attr("checked", "");
                }
            })
        })

        var TourQuoteList = {
            GetListSelectCount: function() {
                var count = 0;
                $("#tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        count++;
                    }
                });
                return count;
            },
            GetCheckedValueList: function() {
                var arrayList = new Array();
                $("#tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        arrayList.push($(this).attr("id"));
                    }
                });
                return arrayList;
            }
        };
    </script>

    </form>
</asp:Content>
