<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineProducts.aspx.cs" Inherits="Web.line.LineProducts"
    MasterPageFile="~/masterpage/Back.Master" Debug="true" Title="线路产品库" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="c1" ID="Content1" runat="server">
    <form id="form1" runat="server">
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">线路产品库</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 线路产品库>> 线路管理
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" id="LineSearch">
            <tr>
                <td width="10" valign="top"><img src="/images/yuanleft.gif" alt="" /></td>
                <td>
                    <div class="searchbox">
                        <label>线路类型：</label>
                        <asp:DropDownList ID="xl_ddlXianluType" runat="server">
                            <asp:ListItem Text="--请选择线路类型--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <label>线路名称：</label>
                        <input type="text" runat="server" name="xl_XianlName" id="xl_XianlName" class="searchinput searchinput02" />
                        <label>发布日期：</label>
                        <input name="xl_XianlStarTime" type="text" class="searchinput" id="xl_XianlStarTime"
                            runat="server" onfocus="WdatePicker();" />
                        至
                        <input type="text" name="xl_XianlEndTime" id="xl_XianlEndTime" class="searchinput"
                            runat="server" onfocus="WdatePicker();" />
                        <br />
                        <label>天数：</label>
                        <input type="text" runat="server" name="xl_xianlDateNumber" id="xl_xianlDateNumber"
                            class="searchinput searchinput03" />
                        <label>&nbsp;&nbsp;发布人：</label>
                        <input name="xl_XianlAuthor" runat="server" type="text" class="searchinput" id="xl_XianlAuthor" />
                        <label>
                            <a href="javascript:void(0);" id="BtnSubmit">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="查询" /></a></label>
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
                    <td width="90" align="center">
                        <asp:Panel ID="AddLine" runat="server"><a href="javascript:void(0);" id="AddArea">新 增</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="updateLine" runat="server"><a href="javascript:viod(0);" id="LineUpdate">修 改</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="CopyLine" runat="server"><a href="javascript:viod(0);" id="LineCopy">复 制</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="penDelete" runat="server"><a href="javascript:void(0);" id="btnDelete">删 除</a></asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="tab_xianlulist">
                <tr>
                    <th width="3%" height="30" align="center" bgcolor="#BDDCF4">&nbsp;</th>
                    <th width="20%" align="center" bgcolor="#BDDCF4">线路名称</th>
                    <th width="12%" align="center" bgcolor="#BDDCF4">线路区域</th>
                    <th width="6%" align="center" bgcolor="#bddcf4">天数</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">发布日期</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">发布人</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">上团数</th>
                    <th width="9%" align="center" bgcolor="#bddcf4">收客数</th>
                    <%--<th width="8%" align="center" bgcolor="#bddcf4">&nbsp;</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">&nbsp;</th>--%>
                    <th width="9%" align="center" bgcolor="#bddcf4">操作</th>
                </tr>
                <cc1:CustomRepeater ID="LineProductList" runat="server" EmptyText="<tr><td colspan='11' align='center'>暂无数据</td></tr>">
                    <ItemTemplate>
                        <tr bgcolor="#E3F1FC">
                            <td height="30" align="center">
                                <input type="checkbox" id="checkboxid" name="checkboxid" ref="<%#Eval("RouteId") %>"
                                    reltype="<%#Eval("ReleaseType").ToString() %>" />
                            </td>
                            <td align="center">
                                <a name="ResultA" target="_blank" href="<%# ReturnUrl(Eval("ReleaseType").ToString())%>?RouteID=<%#Eval("RouteId") %>">
                                    <%#Eval("RouteName")%></a>
                            </td>                            
                            <td align="center"><%# Eval("AreaName")%></td>                                                        
                            <td align="center">
                                <%#Eval("RouteDays")%>
                            </td>
                            <td align="center">
                                <%# Eval("CreateTime", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" resultid="A" ref="<%#Eval("OperatorId") %>">
                                    <%#Eval("OperatorName") %></a>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" resultid="B" ref="<%#Eval("RouteId") %>"><font class="fbred">
                                    <%# Eval("TourCount")%></font></a>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" resultid="C" ref="<%#Eval("RouteId") %>"><font class="fbred">
                                    <%# Eval("VisitorCount")%></font></a>
                            </td>
                            <%--<td align="center" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.线路产品库_线路产品库_发布计划))
                                   {%>
                                <a href="PublishedPlan.aspx" resultid="D" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">发布计划</a>
                                <%}%>
                            </td>
                            <td align="center" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.线路产品库_线路产品库_我要报价))
                                   {%>
                                <a href="javascript:void(0);" name="QuoteA" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">我要报价</a>
                                <%}%>
                            </td>--%>
                            <td align="center">
                                <a name="ResultA" href="/xianlu/<%# Eval("ReleaseType").ToString()=="Quick"?"UpdateQuote":"UpdateLineProducts" %>.aspx?Action=update&id=<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                    <font class="fblue">修改</font></a> <a name="ResultA" href="javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                        <font class="fblue">删除</font></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td height="30" align="center" bgcolor="#BDDCF4">
                                <input type="checkbox" id="checkboxid" name="checkboxid" ref="<%#Eval("RouteId") %>"
                                    reltype="<%#Eval("ReleaseType").ToString() %>" />
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <a name="ResultA" target="_blank" href="<%# ReturnUrl(Eval("ReleaseType").ToString())%>?RouteID=<%#Eval("RouteId") %>">
                                    <%#Eval("RouteName")%></a>
                            </td>                            
                            <td align="center" bgcolor="#BDDCF4"><%# Eval("AreaName")%></td>                            
                            <td align="center" bgcolor="#BDDCF4">
                                <%#Eval("RouteDays")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <%# Eval("CreateTime", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <a href="javascript:void(0);" resultid="A" ref="<%#Eval("OperatorId") %>">
                                    <%#Eval("OperatorName") %></a>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <a href="javascript:void(0);" resultid="B" ref="<%#Eval("RouteId") %>"><font class="fbred">
                                    <%# Eval("TourCount")%></font></a>
                            </td>
                            <td align="center" bgcolor="#BDDCF4">
                                <a href="javascript:void(0);" resultid="C" ref="<%#Eval("RouteId") %>"><font class="fbred">
                                    <%# Eval("VisitorCount")%></font></a>
                            </td>
                           <%-- <td align="center" bgcolor="#BDDCF4" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.线路产品库_线路产品库_发布计划))
                                   {%>
                                <a href="javascript:void(0)" resultid="D" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">发布计划</a>
                                <%}%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.线路产品库_线路产品库_我要报价))
                                   {%>
                                <a href="javascript:void(0);" name="QuoteA" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">我要报价</a>
                                <%}%>
                            </td>--%>
                            <td align="center" bgcolor="#BDDCF4">
                                <a name="ResultA" href="/xianlu/<%# Eval("ReleaseType").ToString()=="Quick"?"UpdateQuote":"UpdateLineProducts" %>.aspx?Action=update&id=<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                    <font class="fblue">修改</font></a> <a name="ResultA" href="javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                        <font class="fblue">删除</font></a>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </cc1:CustomRepeater>
            </table>
            <table width="100%">
                <tr>
                    <td height="30" align="right" colspan="10" class="pageup">
                        <cc2:ExportPageInfo ID="LinePro_ExportPageInfo1" runat="server" LinkType="4" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        var LineProducts = {
            OnSearch: function() {
                var LineType = $.trim($("#<%=xl_ddlXianluType.ClientID %>").val()); //线路类型
                var LineName = $.trim($("#<%=xl_XianlName.ClientID %>").val()); //线路名称
                var StarTime = $.trim($("#<%=xl_XianlStarTime.ClientID %>").val()); //线路开始发布时间 
                var EndTime = $.trim($("#<%=xl_XianlEndTime.ClientID %>").val());    //线路结束发布时间 
                var DateNumber = $.trim($("#<%=xl_xianlDateNumber.ClientID %>").val()); //线路天数
                var Author = $.trim($("#<%=xl_XianlAuthor.ClientID %>").val());  //线路发布人
                //参数
                var param = { LineType: "", LineName: "", StarTime: "", EndTime: "", DateNumber: "", Author: "" };
                param.LineType = LineType;
                param.LineName = LineName;
                param.StarTime = StarTime;
                param.EndTime = EndTime;
                param.DateNumber = DateNumber;
                param.Author = Author;

                window.location.href = "/xianlu/LineProducts.aspx?" + $.param(param);
            },
            //被选中的编号
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
                        arrayList.push($(this).attr("ref"));
                    }
                });
                return arrayList;
            },
            //删除线路信息
            OnDelete: function(RouteId) {
                $.newAjax({
                    type: "post",
                    url: "/xianlu/LineProducts.aspx?action=delete&RouteId=" + RouteId,
                    cache: false,
                    data: $("form").serializeArray(),
                    success: function(msg) {
                        if (msg == "1") {
                            alert("删除成功!");
                            window.location.href = "/xianlu/LineProducts.aspx";
                        } else {
                            alert("删除失败!");
                        }
                    },
                    error: function() {
                        alert("对不起，操作失败!");
                    }
                });
                return false;
            },
            //弹出对话框
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }

        };

        $(document).ready(function() {
            //单项删除线路信息
            $("a[majorid]").click(function() {
                if (confirm('您确定要删除此项数据吗?\n\n此操作不可恢复!')) {
                    var Routeid = $(this).attr("majorid");
                    LineProducts.OnDelete(Routeid);
                };
                return false;
            });
            //批量删除线路信息
            $("#btnDelete").click(function() {
                if (LineProducts.GetListSelectCount() == 0) {
                    alert("请选择一行需要删除的数据!");
                    return false;
                } else {
                    if (confirm("确定要删除此条记录吗?")) {
                        var list = LineProducts.GetCheckedValueList();
                        if (list != null && list.length > 0) {
                            var idList = list;
                            LineProducts.OnDelete(idList);
                        }
                    }
                }
                return false;
            });
            //修改线路信息
            $("#LineUpdate").click(function() {
                if (LineProducts.GetListSelectCount() > 1) {
                    alert("只能选择一条需要修改的数据!");
                    return false;
                }
                else if (LineProducts.GetListSelectCount() == 1) {
                    var list = LineProducts.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        var reltype = $("input[name='checkboxid']:checked:first").attr("reltype");
                        if (reltype == "Quick") {
                            window.location.href = "/xianlu/UpdateQuote.aspx?Action=UpdateT&UpdateID=" + list[0];
                        }
                        else {
                            window.location.href = "/xianlu/UpdateLineProducts.aspx?Action=UpdateT&UpdateID=" + list[0];
                        }
                    }
                } else {
                    alert("请选择一行数据!");
                    return false;
                }
                return false;
            });

            //复制线路信息
            $("#LineCopy").click(function() {
                if (LineProducts.GetListSelectCount() > 1) {
                    alert("只能选择一条需要复制的数据!");
                    return false;
                }
                else if (LineProducts.GetListSelectCount() == 1) {
                    var list = LineProducts.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        var reltype = $("input[name='checkboxid']:checked:first").attr("reltype");
                        if (reltype == "Quick") {
                            window.location.href = "/xianlu/UpdateQuote.aspx?Action=Copy&CopyID=" + list[0];
                        }
                        else {
                            window.location.href = "/xianlu/UpdateLineProducts.aspx?Action=Copy&CopyID=" + list[0];
                        }
                    }
                } else {
                    alert("请选择一条要复制的数据!");
                    return false;
                }
                return false;
            });

            $("#AddArea").click(function() {
                window.location.href = "/xianlu/AddLineProducts.aspx";
                return false;
            });

            //根据类型弹出对话框
            $("#tab_xianlulist").find("a[name!='ResultA']").click(function() {
                var ResultID = $(this).attr("resultID");
                switch (ResultID) {
                    case 'A':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/Published.aspx", "发布人", "400", "250", "ID=" + ID);
                        } break;
                    case 'B':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/GroupsNumber.aspx", "上团数", "740", "550", "ID=" + ID);
                        } break;
                    case 'C':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/CloseNumber.aspx", "收客数", "710", "550", "ID=" + ID);
                        } break;
                    case 'D':
                        {
                            var ID = $(this).attr("ref");
                            var areaId = $(this).attr("AreaId");
                            LineProducts.openDialog("/xianlu/PublishedPlan.aspx", "发布计划", "900", "550", "ID=" + ID + "&areaid=" + areaId);
                        }
                    default: break;
                }
                return false;
            });

            //查询
            $("#BtnSubmit").click(function() {
                LineProducts.OnSearch();
                return false;
            });

            //回车查询
            $("#LineSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    LineProducts.OnSearch();
                    return false;
                }
            });

            //我要报价
            $("#tab_xianlulist").find("a[name='QuoteA']").click(function() {
                var RouteID = $(this).attr("ref");
                var Areaid = $(this).attr("AreaId");
                window.location.href = "/xianlu/Quote.aspx?Areaid=" + Areaid + "&RouteId=" + RouteID;
                return false;
            });
        });
    </script>

    </form>
</asp:Content>
