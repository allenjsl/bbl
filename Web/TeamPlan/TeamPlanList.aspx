<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamPlanList.aspx.cs" Inherits="Web.TeamPlan.TeamPlanList"
    MasterPageFile="~/masterpage/Back.Master" Title="�ŶӼƻ�_�ƻ��б�" %>

<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">�ŶӼƻ�</span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            ����λ��&gt;&gt; �ŶӼƻ�&gt;&gt; �ŶӼƻ�
                        </td>
                    </tr>
                    <tr>
                        <td height="2px" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
            <tbody>
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif">
                    </td>
                    <td>
                        <div class="searchbox" runat="server" id="searchDiv">
                            <label>
                                �źţ�</label>
                            <asp:TextBox ID="txtTeamNumber" runat="server" CssClass="searchinput searchinput03"
                                Width="80px"></asp:TextBox>
                            <label>
                                �Ŷ����ƣ�</label>
                            <asp:TextBox ID="txtTeamName" runat="server" CssClass="searchinput"></asp:TextBox>
                            <label>
                                ������</label>
                            <asp:TextBox ID="txtDayCount" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                            <label>
                                �������ڣ�</label>
                            <asp:TextBox ID="txtBeginDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>
                            ��
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox><br />
                            <uc1:selectOperator ID="selectOperator1" Title="ҵ��Ա" TextClass="searchinput" runat="server"
                                IsShowLabel="true" />&nbsp
                            <uc1:selectOperator ID="selectOperator2" Title="�Ƶ�Ա" TextClass="searchinput" runat="server"
                                IsShowLabel="true" />
                            �Ŷ�״̬��<select id="txtTourStatus">
                                <option value="-1">��ѡ��</option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.�����տ�%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.�����տ�%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.ֹͣ�տ�%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.ֹͣ�տ�%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.�г�;��%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.�г�;��%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.���ű���%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.���ű���%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.�������%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.�������%></option>
                                <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourStatus.�������%>">
                                    <%=EyouSoft.Model.EnumType.TourStructure.TourStatus.�������%></option>
                            </select>
                            <label>
                                �ο�������</label>
                            <asp:TextBox ID="txt_Name" class="searchinput searchinput03" runat="server" Width="80px"></asp:TextBox>
                            <label>
                                <a href="javascript:void(0);" id="btnSearch">
                                    <img style="vertical-align: top;" src="../images/searchbtn.gif" border="0" /></a></label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif">
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
                                <a href="/TeamPlan/FastVersion.aspx">�� ��</a>
                            </asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="pnlUpdate" runat="server">
                                <a href="javascript:void(0);" id="btnUpdate">�� ��</a></asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px">
                            <asp:Panel ID="pnlCopy" runat="server">
                                <a href="javascript:void(0);" id="btnCopy">�� ��</a></asp:Panel>
                        </td>
                        <td align="center" style="padding-left: 5px;">
                            <asp:Panel ID="penDelete" runat="server">
                                <a href="javascript:void(0);" id="btnDelete">ɾ ��</a>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="tablelist">
            <table cellspacing="1" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr class="odd">
                        <th nowrap="nowrap" align="center">
                            ���
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            �ź�
                        </th>
                        <th nowrap="nowrap" align="center" width="10%">
                            ��·����
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            ��������<br />
                            ��������
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            ������
                        </th>
                        <th nowrap="nowrap" align="center" width="7%">
                            ��ϵ��
                        </th>
                        <th nowrap="nowrap" align="center" width="9%">
                            �绰
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            ����
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            ����
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            ���ſ�
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            ״̬
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            �Ƶ�
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            ����
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr class='<%#Container.ItemIndex %2==0?"even":"odd" %>'>
                                <td align="center">
                                    <input type="checkbox" id="checkbox" name="checkbox" ref='<%#Eval("TourId") %>' reltype='<%#Eval("ReleaseType").ToString() %>'
                                        tournum="<%#Eval("TourCode") %>" state="<%#Eval("Status").ToString() %>"><%# Container.ItemIndex + 1 + (this.pageIndex - 1) * this.pageSize%>
                                </td>
                                <td align="center">
                                    <%#Eval("TourCode") %>
                                </td>
                                <td align="center">
                                    <%#GetPrintUrl(Eval("ReleaseType").ToString(),Eval("TourId").ToString())%>
                                    <%#Eval("RouteName")%>
                                    </a>
                                </td>
                                <td align="center">
                                    <%#Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd") %>
                                    <br />
                                    <%#Convert.ToDateTime(Eval("LDate")).AddDays(Convert.ToInt32(Eval("TourDays"))-1).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyerCId") %>">
                                        <%#Eval("BuyerCName")%></a>
                                </td>
                                <td align="center">
                                    <%#Eval("BuyerContacterName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("BuyerContacterTelephone")%>
                                </td>
                                <td align="center">
                                    <%#Eval("TourDays")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:void(0);" class="AddPeople" ref="<%#Eval("OrderId") %>" rex="<%#Eval("TourId") %>" ticketType="<%# (int)Eval("TicketStatus") %>">
                                        <%#Eval("PlanPeopleNumber")%></a>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()).ToString("0.00"))%>
                                </td>
                                <td align="center">
                                    <%#GetStatusByTicket(Eval("Status").ToString(), Eval("TicketStatus").ToString())%>
                                </td>
                                <td align="center">
                                    <%if (CheckGrant(Common.Enum.TravelPermission.�ŶӼƻ�_�ŶӼƻ�_���ŵؽ�))
                                      { %>
                                    <a class="btnDiJie" href="javascript:void(0);" ref="<%#Eval("TourId") %>" ret="<%#Eval("ReleaseType").ToString()%>">
                                        <font class="fblue">���ŵؽ�</font></a><br>
                                    <%} %>
                                    <%--<%if (CheckGrant(Common.Enum.TravelPermission.�ŶӼƻ�_�ŶӼƻ�_�����Ʊ))
                                      { %>
                                    <a class="btnJiPiao" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font
                                        class="fblue">��Ʊ����</font></a>
                                    <%} %>--%>
                                </td>
                                <td align="center">
                                    <a class="btnDaYin" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font class="fblue">
                                        ���ݴ�ӡ</font></a><br>
                                    <%if (CheckGrant(Common.Enum.TravelPermission.�ŶӼƻ�_�ŶӼƻ�_�Ŷӽ���))
                                      { %>
                                    <a class="btnJieSuan" href="javascript:void(0);" ref="<%#Eval("TourId") %>"><font
                                        class="fblue">�Ŷӽ���</font></a>
                                    <%} %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <th nowrap="nowrap" align="center" width="3%">
                            �ϼ�
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="10%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                            <br />
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="7%">
                        </th>
                        <th nowrap="nowrap" align="center" width="9%">
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            <asp:Literal ID="lt_peopleNum" runat="server"></asp:Literal>
                        </th>
                        <th nowrap="nowrap" align="center" width="6%">
                            <asp:Literal ID="lt_paraSum" runat="server"></asp:Literal>
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                        <th nowrap="nowrap" align="center" width="8%">
                        </th>
                    </tr>
                    <tr>
                        <td height="30" align="right" class="pageup" colspan="13">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                            <asp:Label ID="lblMsg" runat="server" Text="δ�ҵ�����!"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //��ѯ��ť�¼�
            $("#btnSearch").click(function() {
                //�ź�
                var teamNumber = $.trim($("#<%=txtTeamNumber.ClientID %>").val());
                //�Ŷ�����
                var teamName = $.trim($("#<%=txtTeamName.ClientID %>").val());
                //�ο�����                
                var orderName = $.trim($("#<%=txt_Name.ClientID %>").val());
                //����
                var dayCount = $.trim($("#<%=txtDayCount.ClientID %>").val());
                //������ʼ����
                var beginDate = $.trim($("#<%=txtBeginDate.ClientID %>").val());
                //������������
                var endDate = $.trim($("#<%=txtEndDate.ClientID %>").val());
                //��·����ID
                var areaId = '<%=Request.QueryString["xlid"]%>';
                //ҵ��Ա���
                var SellerId=<%=selectOperator1.ClientID%>.GetOperatorId();
                //ҵ��Ա����
                var SellerName=<%=selectOperator1.ClientID%>.GetOperatorName();
                //�Ƶ�Ա���
                var CoordinatorId=<%=selectOperator2.ClientID%>.GetOperatorId();
                //�Ƶ�Ա����
                var CoordinatorName=<%=selectOperator2.ClientID%>.GetOperatorName();
                //����
                var para = { orderName: "", teamNumber: "", teamName: "", dayCount: "", beginDate: "", endDate: "", areaId: "", SellerId: "", SellerName: "", CoordinatorId: "", CoordinatorName: "", tourStatus: $("#txtTourStatus").val() };
                para.teamNumber = teamNumber;
                para.teamName = teamName;
                para.orderName = orderName;
                para.dayCount = dayCount;
                para.beginDate = beginDate;
                para.endDate = endDate;
                para.areaId = areaId;
                para.CoordinatorId = CoordinatorId;
                para.CoordinatorName = CoordinatorName;
                para.SellerId = SellerId;
                para.SellerName = SellerName;
                window.location.href = "/TeamPlan/TeamPlanList.aspx?" + $.param(para);
                return false;
            });
            //�޸İ�ť�¼�
            $("#btnUpdate").click(function() {
                if (TeamPlanList.GetListSelectCount() == 1) {
                    var list = TeamPlanList.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        if ($(this).attr("relType") == "Normal") {
                            window.location.href = "/TeamPlan/StandardVersion.aspx?type=Update&id=" + list[0];
                        } else {
                            window.location.href = "/TeamPlan/FastVersion.aspx?type=Update&id=" + list[0];
                        }

                    }
                } else {
                    alert("��ѡ��һ������!");
                    return false;
                }
                return false;
            });
            //���ư�ť�¼�
            $("#btnCopy").click(function() {
                if (TeamPlanList.GetListSelectCount() == 1) {
                    var list = TeamPlanList.GetCheckedValueList();
                    if (list != null && list.length > 0) {
                        window.location.href = "/TeamPlan/FastVersion.aspx?type=Copy&id=" + list[0];
                    }
                } else {
                    alert("��ѡ��һ������!");
                    return false;
                }
                return false;
            });
            //ɾ����ť�¼�
            $("#btnDelete").click(function() {
                if (TeamPlanList.GetListSelectCount() >= 1) {
                    //�ж� �Ƿ��мƻ� �ύ�����������
                    var msg = "";
                    $(".tablelist").find("input[type='checkbox']").each(function() {
                        if ($(this).attr("checked")) {
                            if ($(this).attr("state") == "�������" || $(this).attr("state") == "�������") {
                                msg += "�źţ�" + $(this).attr("tourNum") + "��" + $(this).attr("state") + ",�޷�ɾ�� \n";
                            }
                        }
                    });

                    if (msg != "") {
                        alert(msg);
                        return;
                    }

                    if (confirm("ȷ��ɾ����?")) {
                        var list = TeamPlanList.GetCheckedValueList();
                        if (list != null && list.length > 0) {
                            var idList = list;
                            $.newAjax({
                                type: "Get",
                                url: "/TeamPlan/AjaxTeamPlanList.ashx?type=Delete&idList=" + idList,
                                cache: false,
                                success: function(result) {
                                    if (result == "OK") {
                                        alert("ɾ���ɹ�!");
                                        window.location.href = "/TeamPlan/TeamPlanList.aspx";
                                        return false;
                                    } else {
                                        alert("ɾ��ʧ��");
                                        return false;
                                    }
                                },
                                error: function() {
                                    alert("ɾ��ʧ��! ���Ժ�����!");
                                    return false;
                                }
                            });
                        }
                    }
                } else {
                    alert("��ѡ��һ������!");
                    return false;
                }
                return false;
            });
            //�б�ؽӰ�ť�¼�
            $(".btnDiJie").click(function() {
                var id = $(this).attr("ref");
                var planType = $(this).attr("ret");
                Boxy.iframeDialog({ title: "�ؽ�", iframeUrl: "/TeamPlan/TeamTake.aspx?planId=" + id + "&planType=" + planType + "&type=team", width: "950px", height: "550px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            //�б��Ʊ��ť�¼�
            //            $(".btnJiPiao").click(function() {
            //                var id = $(this).attr("ref");
            //                window.location.href = "/sanping/SanPing_JiPiaoAdd.aspx?type=2&tourId=" + id;
            //                return false;
            //            });
            //�б��ӡ��ť�¼�
            $(".btnDaYin").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "��ӡ", iframeUrl: "/print/printlist.aspx?tourId=" + id, width: "250px", height: "300px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            //�б�����¼�
            $(".btnJieSuan").click(function() {
                var id = $(this).attr("ref");
                window.location.href = "/TeamPlan/TeamSettle.aspx?type=team&id=" + id;
                return false;
            });

            //�ı���س��¼�
            $(".searchbox input[type='text']").keydown(function(event) {
                var e = event;
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            });

            //�б�����ÿ���Ϣ
            $(".AddPeople").click(function() {
                var orderId = $(this).attr("ref");
                var tourId = $(this).attr("rex");
                var ticketType = $(this).attr("ticketType");
                if (ticketType == 0) {
                    Boxy.iframeDialog({ title: "�ÿ���Ϣ", iframeUrl: "/TeamPlan/TeamPlanAddMan.aspx?orderId=" + orderId + "&tourId=" + tourId, width: "850px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            });

            //��������Ϣҳ
            $(".openByerInfo").click(function() {
                var buyerId = $(this).attr("ref");
                Boxy.iframeDialog({ title: "��������Ϣ", iframeUrl: "/CRM/customerinfos/CustomerDetails.aspx?cId=" + buyerId, width: "775px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })
        })

        var TeamPlanList = {
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
            }
        };
    </script>

    </form>
</asp:Content>
