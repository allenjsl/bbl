<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineProducts.aspx.cs" Inherits="Web.line.LineProducts"
    MasterPageFile="~/masterpage/Back.Master" Debug="true" Title="��·��Ʒ��" %>

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
                        <span class="lineprotitle">��·��Ʒ��</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>> ��·��Ʒ��>> ��·����
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
                        <label>��·���ͣ�</label>
                        <asp:DropDownList ID="xl_ddlXianluType" runat="server">
                            <asp:ListItem Text="--��ѡ����·����--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <label>��·���ƣ�</label>
                        <input type="text" runat="server" name="xl_XianlName" id="xl_XianlName" class="searchinput searchinput02" />
                        <label>�������ڣ�</label>
                        <input name="xl_XianlStarTime" type="text" class="searchinput" id="xl_XianlStarTime"
                            runat="server" onfocus="WdatePicker();" />
                        ��
                        <input type="text" name="xl_XianlEndTime" id="xl_XianlEndTime" class="searchinput"
                            runat="server" onfocus="WdatePicker();" />
                        <br />
                        <label>������</label>
                        <input type="text" runat="server" name="xl_xianlDateNumber" id="xl_xianlDateNumber"
                            class="searchinput searchinput03" />
                        <label>&nbsp;&nbsp;�����ˣ�</label>
                        <input name="xl_XianlAuthor" runat="server" type="text" class="searchinput" id="xl_XianlAuthor" />
                        <label>
                            <a href="javascript:void(0);" id="BtnSubmit">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="��ѯ" /></a></label>
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
                        <asp:Panel ID="AddLine" runat="server"><a href="javascript:void(0);" id="AddArea">�� ��</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="updateLine" runat="server"><a href="javascript:viod(0);" id="LineUpdate">�� ��</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="CopyLine" runat="server"><a href="javascript:viod(0);" id="LineCopy">�� ��</a></asp:Panel>
                    </td>
                    <td width="90" align="center">
                        <asp:Panel ID="penDelete" runat="server"><a href="javascript:void(0);" id="btnDelete">ɾ ��</a></asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="tab_xianlulist">
                <tr>
                    <th width="3%" height="30" align="center" bgcolor="#BDDCF4">&nbsp;</th>
                    <th width="20%" align="center" bgcolor="#BDDCF4">��·����</th>
                    <th width="12%" align="center" bgcolor="#BDDCF4">��·����</th>
                    <th width="6%" align="center" bgcolor="#bddcf4">����</th>
                    <th width="10%" align="center" bgcolor="#bddcf4">��������</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">������</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">������</th>
                    <th width="9%" align="center" bgcolor="#bddcf4">�տ���</th>
                    <%--<th width="8%" align="center" bgcolor="#bddcf4">&nbsp;</th>
                    <th width="8%" align="center" bgcolor="#bddcf4">&nbsp;</th>--%>
                    <th width="9%" align="center" bgcolor="#bddcf4">����</th>
                </tr>
                <cc1:CustomRepeater ID="LineProductList" runat="server" EmptyText="<tr><td colspan='11' align='center'>��������</td></tr>">
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
                                <% if (CheckGrant(Common.Enum.TravelPermission.��·��Ʒ��_��·��Ʒ��_�����ƻ�))
                                   {%>
                                <a href="PublishedPlan.aspx" resultid="D" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">�����ƻ�</a>
                                <%}%>
                            </td>
                            <td align="center" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.��·��Ʒ��_��·��Ʒ��_��Ҫ����))
                                   {%>
                                <a href="javascript:void(0);" name="QuoteA" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">��Ҫ����</a>
                                <%}%>
                            </td>--%>
                            <td align="center">
                                <a name="ResultA" href="/xianlu/<%# Eval("ReleaseType").ToString()=="Quick"?"UpdateQuote":"UpdateLineProducts" %>.aspx?Action=update&id=<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                    <font class="fblue">�޸�</font></a> <a name="ResultA" href="javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                        <font class="fblue">ɾ��</font></a>
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
                                <% if (CheckGrant(Common.Enum.TravelPermission.��·��Ʒ��_��·��Ʒ��_�����ƻ�))
                                   {%>
                                <a href="javascript:void(0)" resultid="D" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">�����ƻ�</a>
                                <%}%>
                            </td>
                            <td align="center" bgcolor="#BDDCF4" class="talbe_btn">
                                <% if (CheckGrant(Common.Enum.TravelPermission.��·��Ʒ��_��·��Ʒ��_��Ҫ����))
                                   {%>
                                <a href="javascript:void(0);" name="QuoteA" ref="<%#Eval("RouteId") %>" AreaId="<%# Eval("AreaId") %>">��Ҫ����</a>
                                <%}%>
                            </td>--%>
                            <td align="center" bgcolor="#BDDCF4">
                                <a name="ResultA" href="/xianlu/<%# Eval("ReleaseType").ToString()=="Quick"?"UpdateQuote":"UpdateLineProducts" %>.aspx?Action=update&id=<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                    <font class="fblue">�޸�</font></a> <a name="ResultA" href="javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"RouteId")%>">
                                        <font class="fblue">ɾ��</font></a>
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
                var LineType = $.trim($("#<%=xl_ddlXianluType.ClientID %>").val()); //��·����
                var LineName = $.trim($("#<%=xl_XianlName.ClientID %>").val()); //��·����
                var StarTime = $.trim($("#<%=xl_XianlStarTime.ClientID %>").val()); //��·��ʼ����ʱ�� 
                var EndTime = $.trim($("#<%=xl_XianlEndTime.ClientID %>").val());    //��·��������ʱ�� 
                var DateNumber = $.trim($("#<%=xl_xianlDateNumber.ClientID %>").val()); //��·����
                var Author = $.trim($("#<%=xl_XianlAuthor.ClientID %>").val());  //��·������
                //����
                var param = { LineType: "", LineName: "", StarTime: "", EndTime: "", DateNumber: "", Author: "" };
                param.LineType = LineType;
                param.LineName = LineName;
                param.StarTime = StarTime;
                param.EndTime = EndTime;
                param.DateNumber = DateNumber;
                param.Author = Author;

                window.location.href = "/xianlu/LineProducts.aspx?" + $.param(param);
            },
            //��ѡ�еı��
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
            //ɾ����·��Ϣ
            OnDelete: function(RouteId) {
                $.newAjax({
                    type: "post",
                    url: "/xianlu/LineProducts.aspx?action=delete&RouteId=" + RouteId,
                    cache: false,
                    data: $("form").serializeArray(),
                    success: function(msg) {
                        if (msg == "1") {
                            alert("ɾ���ɹ�!");
                            window.location.href = "/xianlu/LineProducts.aspx";
                        } else {
                            alert("ɾ��ʧ��!");
                        }
                    },
                    error: function() {
                        alert("�Բ��𣬲���ʧ��!");
                    }
                });
                return false;
            },
            //�����Ի���
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }

        };

        $(document).ready(function() {
            //����ɾ����·��Ϣ
            $("a[majorid]").click(function() {
                if (confirm('��ȷ��Ҫɾ������������?\n\n�˲������ɻָ�!')) {
                    var Routeid = $(this).attr("majorid");
                    LineProducts.OnDelete(Routeid);
                };
                return false;
            });
            //����ɾ����·��Ϣ
            $("#btnDelete").click(function() {
                if (LineProducts.GetListSelectCount() == 0) {
                    alert("��ѡ��һ����Ҫɾ��������!");
                    return false;
                } else {
                    if (confirm("ȷ��Ҫɾ��������¼��?")) {
                        var list = LineProducts.GetCheckedValueList();
                        if (list != null && list.length > 0) {
                            var idList = list;
                            LineProducts.OnDelete(idList);
                        }
                    }
                }
                return false;
            });
            //�޸���·��Ϣ
            $("#LineUpdate").click(function() {
                if (LineProducts.GetListSelectCount() > 1) {
                    alert("ֻ��ѡ��һ����Ҫ�޸ĵ�����!");
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
                    alert("��ѡ��һ������!");
                    return false;
                }
                return false;
            });

            //������·��Ϣ
            $("#LineCopy").click(function() {
                if (LineProducts.GetListSelectCount() > 1) {
                    alert("ֻ��ѡ��һ����Ҫ���Ƶ�����!");
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
                    alert("��ѡ��һ��Ҫ���Ƶ�����!");
                    return false;
                }
                return false;
            });

            $("#AddArea").click(function() {
                window.location.href = "/xianlu/AddLineProducts.aspx";
                return false;
            });

            //�������͵����Ի���
            $("#tab_xianlulist").find("a[name!='ResultA']").click(function() {
                var ResultID = $(this).attr("resultID");
                switch (ResultID) {
                    case 'A':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/Published.aspx", "������", "400", "250", "ID=" + ID);
                        } break;
                    case 'B':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/GroupsNumber.aspx", "������", "740", "550", "ID=" + ID);
                        } break;
                    case 'C':
                        {
                            var ID = $(this).attr("ref");
                            LineProducts.openDialog("/xianlu/CloseNumber.aspx", "�տ���", "710", "550", "ID=" + ID);
                        } break;
                    case 'D':
                        {
                            var ID = $(this).attr("ref");
                            var areaId = $(this).attr("AreaId");
                            LineProducts.openDialog("/xianlu/PublishedPlan.aspx", "�����ƻ�", "900", "550", "ID=" + ID + "&areaid=" + areaId);
                        }
                    default: break;
                }
                return false;
            });

            //��ѯ
            $("#BtnSubmit").click(function() {
                LineProducts.OnSearch();
                return false;
            });

            //�س���ѯ
            $("#LineSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    LineProducts.OnSearch();
                    return false;
                }
            });

            //��Ҫ����
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
