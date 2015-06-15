<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" Title="ɢƴ�ƻ�" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Web.sanping.Default" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <style type="text/css">
        #provinceList li
        {
            list-style: none;
            float: left;
            white-space: nowrap;
        }
        #cityList li
        {
            list-style: none;
            float: left;
            white-space: nowrap;
        }
        .tbtn .selector
        {
            position: relative;
        }
        .a_city input
        {
            border: 1px solid #A2C8D2;
        }
        #st_province
        {
            font-size: 14px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td width="15%" nowrap="nowrap">
                                <span class="lineprotitle">ɢƴ�ƻ�</span>
                            </td>
                            <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt; ɢƴ�ƻ�&gt;&gt; ɢƴ�ƻ�
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tbtn">
                <div style="float: left; padding-top: 5px;">
                    <input type="hidden" name="hd_areaId" id="hd_areaId" value="<%=Request.QueryString["areaId"] %>" />
                    ��ǰΪ<input type="hidden" name="hd_province" id="hd_province" value="<%=Request.QueryString["ProvinceId"] %>" /><input
                        type="hidden" name="hd_city" id="hd_city" value="<%=Request.QueryString["cityId"] %>" />
                    <span class="selector"><a href="javascript:void(0)" onclick="return false;"><strong
                        id="st_province">
                        <asp:Literal ID="lt_province" runat="server">ʡ��</asp:Literal></strong> </a>
                        <img src="/images/icodown.gif">
                        <div class="tip" style="display: none; border: 1px solid #5794C3; line-height: 1.7;
                            position: absolute; background: #fff; width: 310px; top: 14px; *top: 15px; left: 0px;">
                            <table border="0" width="100%" cellspacing="0">
                                <tr>
                                    <td width="100%" align="left" style="padding: 5px;">
                                        <table width="100%" cellspacing="0">
                                            <tr>
                                                <td bgcolor="#D9E7F1">
                                                    ��ѡ��ʡ��
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="provinceList">
                                                    ����
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </span>
                </div>
                <span>
                    <div width="100%">
                        <div class="tip" style="display: block; border: 0px solid #5794C3; float: left; background: #fff;
                            left: 0px;">
                            <table border="0" width="100%" cellspacing="0">
                                <tr>
                                    <td width="100%" align="left">
                                        <table width="100%" border="0" cellspacing="0">
                                            <tr>
                                                <td id="cityList">
                                                    <%=cityState==""?"��ѡ��ʡ��":"" %>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </span>
            </div>
            <div class="lineCategorybox">
                <uc1:selectXianlu ID="selectXianlu1" runat="server" />
            </div>
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �źţ�</label>
                                <asp:TextBox ID="txt_team" class="searchinput searchinput03" runat="server" Width="80px"></asp:TextBox>
                                <label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;��·���ƣ�</label>
                                <asp:TextBox ID="txt_xianlu" class="searchinput searchinput02" runat="server"></asp:TextBox>
                                <label>
                                    ������</label>
                                <asp:TextBox ID="txt_days" class="searchinput searchinput03" runat="server"></asp:TextBox>
                                <label>
                                    �������ڣ�</label>
                                <asp:TextBox ID="txt_beginDate" class="searchinput" onfocus="WdatePicker()" runat="server"></asp:TextBox>
                                ��
                                <asp:TextBox ID="txt_endDate" class="searchinput" onfocus="WdatePicker()" runat="server"></asp:TextBox>
                                <br />
                                <label>
                                    �տ�״̬��</label>
                                <asp:DropDownList ID="ddl_cusStatus" runat="server">
                                    <asp:ListItem Value="-1">��ѡ���տ�״̬</asp:ListItem>
                                    <asp:ListItem Value="0">�����տ�</asp:ListItem>
                                    <asp:ListItem Value="1">ֹͣ�տ�</asp:ListItem>
                                    <asp:ListItem Value="2">�г�;��</asp:ListItem>
                                    <asp:ListItem Value="3">���ű���</asp:ListItem>
                                    <asp:ListItem Value="4">�������</asp:ListItem>
                                    <asp:ListItem Value="5">�������</asp:ListItem>
                                </asp:DropDownList>
                                <label>
                                    ����״̬��</label>
                                <asp:DropDownList ID="ddl_orderStatus" runat="server">
                                    <asp:ListItem Value="-1">��ѡ�񶩵�״̬</asp:ListItem>
                                    <asp:ListItem Value="1">δ����</asp:ListItem>
                                    <asp:ListItem Value="2">����λ</asp:ListItem>
                                    <asp:ListItem Value="3">��λ����</asp:ListItem>
                                    <asp:ListItem Value="4">������</asp:ListItem>
                                    <asp:ListItem Value="5">�ѳɽ�</asp:ListItem>
                                </asp:DropDownList>
                                <label>
                                    <uc2:selectOperator ID="selectOperator1" runat="server" Title="�Ƶ�Ա" TextClass="searchinput" />
                                </label>
                                <label>
                                    <uc2:selectOperator ID="selectOperator2" runat="server" Title="����Ա" TextClass="searchinput" />
                                </label>
                                <br />
                                <label>
                                    �ο�������</label>
                                <asp:TextBox ID="txt_Name" class="searchinput searchinput03" runat="server" Width="80px"></asp:TextBox>
                                <label>
                                    <asp:ImageButton ID="ImageButton1" Style="vertical-align: top;" ImageUrl="../images/searchbtn.gif"
                                        runat="server" OnClick="ImageButton1_Click" />
                                </label>
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
                            <td width="90" align="center">
                                <a href="QuickAdd.aspx">�� ��</a>
                            </td>
                            <td width="90" align="center">
                                <a href="update.aspx" id="update">�� ��</a>
                            </td>
                            <td width="90" align="center">
                                <a href="update.aspx?act=copy" id="copy">�� ��</a>
                            </td>
                            <td width="90" align="center">
                                <asp:LinkButton ID="LinkButton1" CssClass="del" runat="server" OnClick="LinkButton1_Click">ɾ ��</asp:LinkButton>
                            </td>
                            <td width="90" align="center">
                                <a href="javascript:();" onclick="HandStatus('mk')">�� ��</a>
                            </td>
                            <td width="90" align="center">
                                <a href="javascript:();" onclick="HandStatus('ts')">ͣ ��</a>
                            </td>
                            <td width="90" align="center">
                                <a href="javascript:void(0);" onclick="changeStatus(1)">�� ��</a>
                            </td>
                            <td width="90" align="center">
                                <a href="javascript:void(0);" onclick="changeStatus(2)">�� ��</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr class="odd">
                            <th width="3%" height="30" nowrap="nowrap" align="center">
                                ѡ��
                            </th>
                            <th width="7%" nowrap="nowrap" align="center">
                                �ź�
                            </th>
                            <th width="10%" nowrap="nowrap" align="center">
                                ��·����
                            </th>
                            <th width="9%" nowrap="nowrap" align="center">
                                <div>
                                    <a href="javascript:void(0)" style="color: <%=Request.Url.ToString().Contains("order=1")?"red":""%>"
                                        onclick="return order(1)">��</a>��������<a href="javascript:void(0)" style="color: <%=Request.Url.ToString().Contains("order=2")?"red":""%>"
                                            onclick="return order(2)">��</a></div>
                                <div>
                                    ��������</div>
                            </th>
                            <th width="8%" nowrap="nowrap" align="center">
                                �۸�
                            </th>
                            <th width="8%" nowrap="nowrap" align="center">
                                ͬ�м�
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                �ƻ�
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                ʵ��
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                ��λ
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                δ����
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                ʣ��
                            </th>
                            <th width="5%" nowrap="nowrap" align="center">
                                ����
                            </th>
                            <th width="7%" nowrap="nowrap" align="center">
                                <a href="javascript:void(0)" style="color: <%=Request.Url.ToString().Contains("order=3")?"red":""%>"
                                    onclick="return order(3)">��</a>״̬<a href="javascript:void(0)" style="color: <%=Request.Url.ToString().Contains("order=4")?"red":""%>"
                                        onclick="return order(4)">��</a>
                            </th>
                            <th width="6%" nowrap="nowrap" align="center">
                                �Ƶ�
                            </th>
                            <th width="7%" nowrap="nowrap" align="center">
                                ����
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                            <ItemTemplate>
                                <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                                    <td height="30" align="center">
                                        <input type="hidden" value="<%#(int)Eval("ReleaseType")==0?0:1 %>" class="ReleaseType" />
                                        <input type="checkbox" id="checkbox" value="<%#Eval("TourId") %>" name="checkbox"
                                            onclick="ShowBoxy(this,event)"><%# Container.ItemIndex + 1+( this.pageIndex - 1) * this.pageSize%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("TourCode") %>
                                    </td>
                                    <td align="center">
                                        <a target="_blank" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType"))+"?tourId="+Eval("tourId").ToString() %>">
                                            <%#Eval("RouteName")%></a><div style="clear: both;">
                                                <%#Eval("HandStatus").ToString().Trim() != "��" ? Eval("HandStatus").ToString().Trim() == "�ֶ�ͣ��" ? "<a class=\"tings\">" + Eval("HandStatus") + "</a>" : "<a class=\"keman\">" + Eval("HandStatus") + "</a>" : ""%>
                                                <%-- <span class="state2">--%>
                                                <%#Eval("TourRouteStatus").ToString().Trim() != "��" ? Eval("TourRouteStatus").ToString().Trim() == "�ؼ�" ? "<span class=\"state1\">" + Eval("TourRouteStatus") + "</span>" : "<span class=\"state2\">" + Eval("TourRouteStatus") + "</span>" : ""%>
                                            </div>
                                    </td>
                                    <td align="center">
                                        <div>
                                            <%#DateTime.Parse( Eval("LDate").ToString()).ToString("yyyy-MM-dd") %></div>
                                        <div>
                                            <%#DateTime.Parse(Eval("RDate").ToString()).ToString("yyyy-MM-dd")%></div>
                                    </td>
                                    <td align="center">
                                        ����:<a href="chengrenj.aspx?tourId=<%#Eval("tourId") %>" class="crj"><font class="fred"><asp:Label
                                            ID="Lable1" runat="server" Text=""></asp:Label></font></a><br>
                                        ��ͯ:<a href="chengrenj.aspx?tourId=<%#Eval("tourId") %>" class="crj"><font class="fred"><asp:Label
                                            ID="Lable2" runat="server" Text=""></asp:Label></font></a>
                                    </td>
                                    <td align="center">
                                        ����:<%# GetTHAdultPrice(Eval("tourId").ToString())%><br />
                                        ��ͯ:<%#GetTHChildRenPrice(Eval("tourId").ToString())%>
                                    </td>
                                    <td align="center">
                                        <span name="jihua">
                                            <%#Eval("PlanPeopleNumber") %></span>
                                    </td>
                                    <td align="center">
                                        <a href="/sales/Order_List.aspx?tuanHao=<%#Eval("tourCode") %>&status=5">
                                            <%#(int)Eval("PeopleNumberShiShou") - (int)Eval("PeopleNumberLiuWei") - (int)Eval("PeopleNumberWeiChuLi")%></a>
                                    </td>
                                    <td align="center">
                                        <span name="liouwei"><a href="/sales/Order_List.aspx?tuanHao=<%#Eval("tourCode") %>&status=2">
                                            <%#Eval("PeopleNumberLiuWei")%></a></span>
                                    </td>
                                    <td align="center">
                                        <span name="weiquli"><a href="/sales/Order_List.aspx?tuanHao=<%#Eval("tourCode") %>&status=1">
                                            <%#Eval("PeopleNumberWeiChuLi")%></a></span>
                                    </td>
                                    <td align="center">
                                        <span name="shengyu">
                                            <%#ShengYu(Eval("PlanPeopleNumber").ToString(), Eval("PeopleNumberShiShou").ToString(), Eval("PeopleNumberLiuWei").ToString(), Eval("PeopleNumberWeiChuLi").ToString())%></span>
                                    </td>
                                    <td align="center">
                                        <%
                                            int status = (int)(((System.Collections.Generic.List<EyouSoft.Model.TourStructure.TourBaseInfo>)rpt_list.DataSource)[i++].Status);
                                            //Response.Write(status.ToString());
                                            if (status != (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.������� && status != (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.�������)
                                            {
                                        %>
                                        <a href="SanPing_jion.aspx?tourId=<%#Eval("TourId") %>" class="baoming">����</a>
                                        <%}
                                            else
                                            { %>
                                        <span style="color: Gray">����</span>
                                        <%} %>
                                    </td>
                                    <td align="center">
                                        <%#getStatus((EyouSoft.Model.EnumType.PlanStructure.TicketState)Eval("TicketStatus"), (EyouSoft.Model.EnumType.TourStructure.TourStatus)Eval("Status"))%>
                                    </td>
                                    <td align="center">
                                        <%if (CheckGrant(Common.Enum.TravelPermission.ɢƴ�ƻ�_ɢƴ�ƻ�_���ŵؽ�))
                                          { %>
                                        <a href="/TeamPlan/TeamTake.aspx?planType=<%#Eval("ReleaseType")%>&type=2&planId=<%#Eval("TourId") %>"
                                            class="addDijie"><font class="fblue">���ŵؽ�</font></a><br>
                                        <%}
                                          //if (CheckGrant(Common.Enum.TravelPermission.ɢƴ�ƻ�_ɢƴ�ƻ�_�����Ʊ))
                                          //{
                                        %>
                                        <%--<a href="SanPing_JiPiaoAdd.aspx?type=1&tourId=<%#Eval("tourId") %>&orderId=<%#Eval("OrderId") %>">
                                            <font class="fblue">�����Ʊ</font></a>
                                        <%} %>--%>
                                    </td>
                                    <td align="center">
                                        <a class="btnDaYin" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font class="fblue">
                                            ���ݴ�ӡ</font></a><br>
                                        <%
                                            if (CheckGrant(Common.Enum.TravelPermission.ɢƴ�ƻ�_ɢƴ�ƻ�_�Ŷӽ���))
                                            { %><a href="/TeamPlan/TeamSettle.aspx?type=san&id=<%#Eval("tourId") %>"><font class="fblue">�Ŷӽ���</font></a><%} %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="15">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <div id="divShortcutMenu" style="display: none; width: 120px; border: 1px solid gray;
        position: absolute; background-color: White;">
        <input type="hidden" id="hid_TourID">
        <table width="92%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin-top: 3px;
            border-bottom: 1px solid gray;">
            <tbody>
                <tr class="usedea">
                    <td align="left" class="chekboxtitle">
                        ������ݷ�ʽ
                    </td>
                    <td align="left">
                        <img src="/images/close.gif" style="cursor: pointer;" onclick="closeTip(this)">
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="92%" cellspacing="0" cellpadding="0" border="0" align="center" id="TourMarket"
            style="margin-top: 3px; border-bottom: 1px solid gray;">
            <tbody>
                <tr>
                    <td align="left" class="chekboxtitle" colspan="2">
                        �ƹ�״̬��
                    </td>
                </tr>
                <tr>
                    <td align="left" class="usedea" colspan="2">
                        <a id="1381" href="javascript:void(0)" onclick="changeStatus(1); return false;"><span
                            style="white-space: nowrap;">�ؼ�</span></a> <a id="1382" href="javascript:void(0)"
                                onclick="changeStatus(2); return false;"><span style="white-space: nowrap;">����</span></a>
                        <a id="A1" href="javascript:void(0)" onclick="changeStatus(0); return false;"><span
                            style="white-space: nowrap;">ȡ��</span></a>
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="92%" cellspacing="0" cellpadding="0" border="0" align="center" style="margin-top: 3px;
            border-bottom: 1px solid gray;">
            <tbody>
                <tr>
                    <td align="left" class="chekboxtitle">
                        �տ�״̬��
                    </td>
                </tr>
                <tr>
                    <td align="left" class="usedea">
                        <a id="2" href="javascript:void(0)" onclick="HandStatus('mk')">����</a> <a id="0" href="javascript:void(0)"
                            onclick="HandStatus('ts')">ͣ��</a> <a href="javascript:void(0)" onclick="HandStatus('w')">
                                ����</a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        var valInt = /^\d+,$/;
        var valorder = /order=\d/;
        function order(v) {
            var Url = "<%=Request.Url.ToString() %>";
            if (Url.indexOf("order=", 0) > 0) {
                Url = Url.replace(/order=\d/, "order=" + v);
            } else
                if (Url.indexOf("?", 0) > 0) {
                Url += "&order=" + v;
            } else {
                Url += "?order=" + v;
            }
            location.href = Url;
            return false;
        }
        $(function() {
            $(".addDijie").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�ؽ���",
                    modal: true,
                    width: "960px",
                    height: "500px"
                });
                return false;
            });

            //�б��ӡ��ť�¼�
            $(".btnDaYin").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "��ӡ", iframeUrl: "/print/printlist.aspx?tourId=" + id, width: "250px", height: "300px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            $(".del").click(function() {
                var chkval = "";
                $("[name='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                    }
                });
                if (chkval == "") {
                    alert("��ѡ��һ����¼!");
                    return false;
                }
                return confirm('�Ƿ�ȷ��ɾ��?');
            });
            $("#copy").click(function() {
                var chkval = "";
                var ReleaseType = "";
                $("[name='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                        ReleaseType = $(this).prev("input").val();
                    }
                });
                if (chkval != "") {
                    if (chkval.split(",")[1] == "") {
                        if (ReleaseType == "0") {
                            location.href = "add.aspx?act=copy&id=" + chkval.split(",")[0];
                        } else {
                            location.href = "QuickAdd.aspx?act=copy&id=" + chkval.split(",")[0];
                        }
                    } else {
                        alert("ֻ�ܸ���һ����¼��");
                    }
                } else {
                    alert("��ѡ��һ����¼��");
                }
                return false;
            });
            $("#update").click(function() {
                var chkval = "";
                var ReleaseType = "";
                $("[name='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                        ReleaseType = $(this).prev("input").val();
                    }
                });
                if (chkval != "") {
                    if (chkval.split(",")[1] == "") {
                        if (ReleaseType == "0") {
                            location.href = "add.aspx?act=update&id=" + chkval.split(",")[0];
                        } else {
                            location.href = "QuickAdd.aspx?act=update&id=" + chkval.split(",")[0];
                        }
                    } else {
                        alert("ֻ���޸�һ����¼��");
                    }
                } else {
                    alert("��ѡ��һ����¼��");
                }
                return false;
            });
            $("[name='txt_trueNum']").blur(function(e, d) {
                if (/^\d+$/.test($(this).val())) {
                    var jihua = $(this).parent().prev().find("[name='jihua']").text();
                    var liouwei = $(this).parent().next().text();
                    var weiquli = $(this).parent().next().next().find("[name='weiquli']").text();
                    var shishou = $(this).val();
                    var shengyu = $(this).parent().next().next().next().find("[name='shengyu']");
                    var sy = parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli);

                    if (parseInt(shishou) > parseInt(jihua)) {
                        alert("ʵ�ղ��ܴ��ڼƻ�!");
                        //                        $(this).select();
                        return false;
                    }
                    //$.newAjax("/sanping/default.aspx", {}, function(r) {
                    //shengyu.html(parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli) - parseInt(shishou));
                    // });

                    $.newAjax({ url: "/sanping/default.aspx?act=updatenum", data: "tourid=" + $(this).attr("tourId") + "&num=" + shishou, type: "POST", dataType: "html", success: function(r) {

                        if (r == "yes") {
                            //shengyu.html(parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli) - parseInt(shishou));
                        } else {
                            alert("�޸�ʧ��!");
                            // $(this).get(0).reset();
                        }
                    }
                    });
                } else {
                    alert("����ȷ��д����");
                    $(this).focus();
                }

            });
            //����
            $(".baoming").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "����",
                    modal: true,
                    width: "880px",
                    height: "600px"
                });
                return false;
            });
            $(".crj").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�۸����",
                    modal: true,
                    width: "460px",
                    height: "150px"
                });
                return false;
            });
        });
        
$(function() {
            $.ajax({
                type: "Get",
                async: true,
                url: "default.aspx?act=getprovince",
                cache: true,
                dataType: "json",
                success: function(result) {
                    //alert(result);
                    var str = "";
                    var queryProvince="<%=Request.QueryString["ProvinceId"] %>";
                    if(queryProvince==""&&result.length>0)
                    {
                       //queryProvince=67;
                       //$("#hd_province").val(queryProvince);
                    }
                    for (var i = 0; i < result.length; i++) {
                        if(result[i].ProvinceName=="�㽭"&&queryProvince==""){queryProvince=result[i].Id;$("#hd_province").val(queryProvince);}
                        }
                    for (var i = 0; i < result.length; i++) {
                        str += "<li><a href='javascript:void(0)' class='a_province";
                        if(result[i].Id==queryProvince)
                        str+=" red";
                        str += "' tip='" + result[i].Id + "'>" + result[i].ProvinceName + "</a>&nbsp;&nbsp;|&nbsp;&nbsp;</li>";
                    }
                    if(str!="" )
                    {
                        $("#provinceList").html(str);
 
                    }
                        



                    
                    $(".a_province").live("click", function() {
                        $("#hd_province").val($(this).attr("tip"));
                        $("#st_province").html($(this).html());
                        $(".tip").hide();
                        $.ajax({
                            type: "Get",
                            async: true,
                            url: "default.aspx?act=getcity&provinceId=" + $(this).attr("tip"),
                            cache: true,
                            dataType: "json",
                            success: function(result) {
                                if (result.length > 0) {
                                    $("#hd_city").val(result[0].Id);
                                     $("#<%=ImageButton1.ClientID %>").click();
                                }
                            }
                        });
                        return false;
                    })
                    
                    var proId="<%=Request.QueryString["ProvinceId"] %>";
            if(proId=="")
            {
                proId=queryProvince;
            }
            $.ajax({
                type: "Get",
                async: true,
                url: "default.aspx?act=getcity&provinceId="+proId,
                cache: true,
                dataType: "json",
                success: function(result) {
                    var str = "";
                    var queryCity="<%=Request.QueryString["CityId"] %>";
                    if(queryCity=="")
                    {
                                           for (var i = 0; i < result.length; i++) {
                        str += "<a href='javascript:void(0)' class='a_city";
                        //if(i==0){
                        //    str+="' tip='" + result[i].Id + "'><input type='button' style='width:60px;line-height:20px;height:20px;cursor:pointer;color:red' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        //}else
                        {
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }
                    } 
                    }else
                    {
                                          for (var i = 0; i < result.length; i++) {
                        str += "<a href='javascript:void(0)' class='a_city";
                        if(result[i].Id==queryCity){
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;color:red' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }else
                        {
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }
                    }  
                    }

                    if(str!="")
                    $("#cityList").html(str);
                    $(".a_city").live("click", function() {
                        $("#hd_city").val($(this).attr("tip"));
                        //$(".tip").hide();
                        $("#st_city").html($(this).html());
                        $("#<%=ImageButton1.ClientID %>").click();
                        return false;
                    });

                }
            });
                }
            });
            
            $(".selector").hover(function() { $(this).children(".tip").show(); }, function() { $(this).children(".tip").hide(); });
            var queryArea ='<%=Request.QueryString["areaid"] %>';
            $(".menu-t").each(function(){
                if($(this).attr("tip")==queryArea)
                {
                    $(this).addClass("red");
                }   
                $(this).bind("click",function(){
                    $(this).addClass("red");
                    $("#hd_areaId").val($(this).attr("tip"));
                    $("#<%=ImageButton1.ClientID %>").click();
                    return false;
                });
            });
        });
       
        function HandStatus(type)
        {
            var str="";
             $("input[type='checkbox']:checked").each(function(){
                         str+= $(this).attr("value")+",";
                    })
            if(str.length<=0)
            {
                alert("��ѡ���¼��")
                return false;
            }
            $.ajax({
                      type: "POST",
                      async: true,
                      url: "/sanping/Default.aspx?act=HandStatus&hs="+type+"&touids=" + str.substring(0,str.length-1),
                      cache: true,
                      dataType: "html",
                      success: function(result) {
                        if(result!="1")
                        {
                         alert(result)
                        }
                        else
                        {
                        alert("���óɹ���")
                        location.reload();
                        }
                      }
                   });
           return false;
        }
        function ShowBoxy(obj,event)
        {
            if(obj.checked){
                document.getElementById("divShortcutMenu").style.display="block";
                document.getElementById("divShortcutMenu").style.left=event.clientX+10+"px";
                var top = document.body.scrollTop ? document.body.scrollTop : document.documentElement.scrollTop;               
                document.getElementById("divShortcutMenu").style.top=event.clientY+top+"px";                                
            }else
            document.getElementById("divShortcutMenu").style.display="none";
        }
        function closeTip(){
            document.getElementById("divShortcutMenu").style.display="none";
            return false;
        }
        function changeStatus(status){
            var chk="";
            $("[name='checkbox']:checked").each(function(){
                chk+=$(this).val()+","
            });
            if(chk.length<=0)
            {
                alert("��ѡ�������¼!");
                return false;
            }else{
                chk=chk.substring(0,chk.length-1);
            }
            $.newAjax({ url: "/sanping/default.aspx?act=changeStatus", data: "tourid=" +chk + "&status=" + status, type: "POST", dataType: "text", success: function(r) {

                        if (r == "yes") {
                            location.reload();
                        } else {
                            alert("�޸�ʧ��!");
                            // $(this).get(0).reset();
                        }
                    }
                    });
            return false;
        }
    </script>

</asp:Content>
