<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Order_List.aspx.cs" Inherits="Web.sales.OrderList" Title="��������" Debug="true" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">���۹���</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt; ���۹���&gt;&gt; ��������
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
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox" style="height: 80px">
                                <label>
                                    �����ţ�</label><input type="text" id="inputOrderLot" class="searchinput" name="textfield3"
                                        runat="server">
                                <label>
                                    �źţ�</label><input type="text" id="inputTuanHao" class="searchinput" name="textfield2"
                                        runat="server">
                                <label>
                                    �ͻ���λ��</label><input type="text" id="inputCus" class="searchinput" name="textfield"
                                        runat="server">
                                <label>
                                    �������ڣ�</label><input type="text" id="inputChuTuanStartDate" class="searchinput" name="textfield"
                                        onfocus="WdatePicker()" runat="server">
                                ��
                                <input type="text" id="inputChuTuanEndDate" class="searchinput" name="textfield"
                                    onfocus="WdatePicker()" runat="server">
                                <label>
                                    �µ�ʱ�䣺</label><input type="text" id="inputOrderStartDate" class="searchinput" name="textfield"
                                        onfocus="WdatePicker()" runat="server">
                                ��
                                <input type="text" id="inputOrderEndDate" class="searchinput" name="textfield" onfocus="WdatePicker()"
                                    runat="server">
                                <br />
                                <uc1:selectOperator ID="selectOperator1" Title="����Ա" TextClass="searchinput" runat="server"
                                    IsShowLabel="true" />
                                &nbsp;&nbsp;����״̬:<asp:DropDownList ID="ddlOrderState" runat="server">
                                </asp:DropDownList>
                                &nbsp;<label><a href="javascript:void(0)" id="btnSearch"><img style="vertical-align: top;"
                                    src="/images/searchbtn.gif"></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left" width="45%">
                    <tbody>
                        <tr>
                            <%-- <td align="center" width="90">
                                <a href="javascript:void(0)" id="orderShow">�� ��</a>
                            </td>--%>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="orderEdit">�� ��</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="orderTuiPiao">�� Ʊ</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="orderTuiTuan">�� ��</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th height="30" bgcolor="#bddcf4" align="center" width="3%">
                                &nbsp;
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                �ź�
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="11%">
                                ��·����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ��������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ����״̬
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                �ͻ���λ
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                ��ϵ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                �绰
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                ��ӡ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ����
                            </th>
                        </tr>
                        <cc1:CustomRepeater ID="repList" runat="server" OnItemDataBound="repList_ItemDataBound">
                            <ItemTemplate>
                                <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <td height="30" align="center">
                                        <input type="checkbox" id="<%#Eval("ID")%>&tourid=<%#Eval("tourid") %>&tourNo=<%#Eval("tourNo") %>"
                                            name="checkbox">
                                    </td>
                                    <td align="center">
                                        <%#Eval("TourNo")%>
                                    </td>
                                    <td align="center">
                                        <a href="" title="<%#Eval("RouteName")%>" isxingchen="True" target="_blank">
                                            <%#Eval("RouteName")%></a>
                                        <input id="hd_XingChengDan" type="hidden" runat="server" />
                                    </td>
                                    <td align="center">
                                        <%# string.Format("{0:d}", Eval("LeaveDate"))%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("OrderNo")%>
                                    </td>
                                    <td align="center" data-class='orderState'>
                                        <%#Eval("OrderState").ToString() == "����λ" ? "��λ��<br /><span style=\"color: Red\">" + Convert.ToDateTime(Eval("SaveSeatDate")).ToString("MM-dd HH:mm") + " </span>" : Eval("OrderState").ToString().Trim() == "������" ? "��ȡ��" : Eval("OrderState")%>
                                    </td>
                                    <td align="center">
                                        <a href="" target="_blank" isperson="True">
                                            <%#Eval("PeopleNumber")%></a>
                                        <input id="hd_PrintMingDan" type="hidden" runat="server" />
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyCompanyID") %>">
                                            <%#Eval("BuyCompanyName")%></a>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ContactName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ContactTel")%>
                                    </td>
                                    <td align="center">
                                        <a href="<%# GetPrintUrl(Eval("TourClassId").ToString(),Eval("TourId").ToString(),Eval("ID").ToString())%>"
                                            target="_blank">ȷ�ϵ�</a> <a href="<%# GetSettledPrintUrl(Eval("TourClassId").ToString(),Eval("TourId").ToString(),Eval("ID").ToString())%>"
                                                target="_blank">���㵥</a> <a href="/print/hkmj/chutuantongzhiD.aspx?Tourid=<%#Eval("TourId").ToString() %>&orderId=<%#Eval("ID").ToString() %>"
                                                    target="_blank">����֪ͨ��</a>
                                    </td>
                                    <td align="center" data-isext='<%#Eval("IsExtsisTicket") %>'>
                                        <a name="shenqing" onclick="OrderList.GoOper('<%#Eval("ID")%>&tourId=<%#Eval("tourId") %>','<%#Eval("TourNo")%>','chupiao')"
                                            href="javascript:void(0)">�����Ʊ</a>
                                        <%if (CheckGrant(global::Common.Enum.TravelPermission.���۹���_��������_�����޸�))
                                          {
                                        %>
                                        <a name="update" onclick="OrderList.GoOper('<%#Eval("ID")%>&tourId=<%#Eval("tourId") %>','<%#Eval("TourNo")%>','modify')"
                                            href="javascript:void(0)">�޸�<br />
                                        </a>
                                        <%
                                            }%>
                                        <%if (CheckGrant(global::Common.Enum.TravelPermission.���۹���_��������_��Ʊ����))
                                          {
                                        %>
                                        <a onclick="OrderList.GoOper('<%#Eval("ID")%>&tourId=<%#Eval("tourId") %>','<%#Eval("TourNo")%>','tuiPiao')"
                                            href="javascript:void(0)">��Ʊ</a>
                                        <%
                                            }%>
                                        <%if (CheckGrant(global::Common.Enum.TravelPermission.���۹���_��������_���Ų���))
                                          {
                                        %>
                                        <a onclick="OrderList.GoOper('<%#Eval("ID")%>&tourId=<%#Eval("tourId") %>','<%#Eval("TourNo")%>','tuiTuan')"
                                            href="javascript:void(0)">����</a>
                                        <%
                                            }%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </cc1:CustomRepeater>
                    </tbody>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="right">
                                <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                                    runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
        var OrderList = {

            GetSelectCount: function() {
                var count = 0;
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked") == true) {
                        count++;
                    }
                });
                return count;
            },

            GetSelectItemValue: function() {
                var arrayList = new Array();
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked") == true) {
                        arrayList.push($(this).attr("id"));
                    }
                });
                return arrayList;
            },

            GoOper: function(id, tourNo, sign) {
                switch (sign) {
                    case "chupiao":
                        this.openXLwindow('TicketOut.aspx?id=' + id, '�����Ʊ', '900px', '400px');
                        break;
                    case "show":
                        this.openXLwindow('Order_show.aspx?id=' + id, '�鿴', '900px', '560px');
                        break;
                    case "modify":
                        this.openXLwindow('Order_edit.aspx?id=' + id, '�����޸�', '900px', '560px');
                        break;
                    case "tuiTuan":
                        this.openXLwindow('Order_tuituan.aspx?id=' + id + '&tourNo=' + tourNo, '����', '720px', '200px');
                        break;
                    case "tuiPiao":
                        this.openXLwindow('Order_tuipiao.aspx?id=' + id, '��Ʊ', '720px', '220px');
                        break;
                    default:
                        return false;
                }
            },

            Search: function() {
                var data = {
                    orderLot: $("#<%=inputOrderLot.ClientID%>").val(),
                    tuanHao: $("#<%=inputTuanHao.ClientID%>").val(),
                    cusHao: $("#<%=inputCus.ClientID%>").val(),
                    orderStart: $("#<%=inputOrderStartDate.ClientID%>").val(),
                    orderEnd: $("#<%=inputOrderEndDate.ClientID%>").val(),
                    operId:<%=selectOperator1.ClientID%>.GetOperatorId(),
                    operName:<%=selectOperator1.ClientID%>.GetOperatorName(),
                    chuTuanStart: $("#<%=inputChuTuanStartDate.ClientID%>").val(),
                    chuTuanEnd: $("#<%=inputChuTuanEndDate.ClientID%>").val(),
                    status:$("#<%=ddlOrderState.ClientID %>").val()
                };
                window.location.href = "/sales/Order_List.aspx?" + $.param(data);
            },

            openXLwindow: function(url, title, width, height) {
                url = url;
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: title,
                    modal: true,
                    width: width,
                    height: height
                });
                return false;
            }
        }
        $(document).ready(function() {
            $("#inputOrderLot").focus();

            $("#orderShow,#link1").click(function() {
                if (OrderList.GetSelectCount() > 0) {
                    var list = OrderList.GetSelectItemValue();
                    if (list[0] != undefined && list.length == 1) {
                        OrderList.openXLwindow('Order_show.aspx?id=' + list[0], '�鿴', '900px', '560px');
                    }
                    else {
                        alert("ֻ��ѡ��һ����¼����!");
                        return false;
                    }
                }
                else {
                    alert("��ѡ��Ҫ�޸ĵļ�¼");
                    return false;
                }
            });

            $("#orderEdit").click(function() {
                if (OrderList.GetSelectCount() > 0) {
                    var list = OrderList.GetSelectItemValue();
                    if (list[0] != undefined && list.length == 1) {
                        OrderList.openXLwindow('Order_edit.aspx?id=' + list[0], '�޸�', '900px', '560px');
                    }
                    else {
                        alert("ֻ��ѡ��һ����¼����!");
                        return false;
                    }
                }
                else {
                    alert("��ѡ��Ҫ�޸ĵļ�¼");
                    return false;
                }
            });

            $("#orderTuiTuan").click(function() {
                if (OrderList.GetSelectCount() > 0) {
                    var list = OrderList.GetSelectItemValue();
                    if (list[0] != undefined && list.length == 1) {
                        OrderList.openXLwindow('Order_tuituan.aspx?id=' + list[0], '����', '720px', '200px');
                    }
                    else {
                        alert("ֻ��ѡ��һ����¼����!");
                        return false;
                    }
                }
                else {
                    alert("��ѡ��Ҫ���ŵļ�¼");
                    return false;
                }
            });

            $("#orderTuiPiao").click(function() {
                if (OrderList.GetSelectCount() > 0) {
                    var list = OrderList.GetSelectItemValue();
                    if (list[0] != undefined && list.length == 1) {
                        OrderList.openXLwindow('Order_tuipiao.aspx?id=' + list[0], '��Ʊ', '720px', '220px');
                    }
                    else {
                        alert("ֻ��ѡ��һ����¼����!");
                        return false;
                    }
                }
                else {
                    alert("��ѡ��Ҫ��Ʊ�ļ�¼");
                    return false;
                }
            });

            $("#btnSearch").click(function() {
                OrderList.Search();
                return false;
            });

            $("a[IsXingChen='True']").each(function() {
                $(this).attr("href", $(this).next("input").val());
            });

            $("a[IsPerson='True']").each(function() {
                $(this).attr("href", $(this).next("input").val());
            });


            $("#<%=this.form1.ClientID%>").keydown(function(event) {
                if (event.keyCode == 13) {
                    $("#btnSearch").click();
                    return false;
                }
            });
            
             //�򿪿ͻ���λ��Ϣҳ
            $(".openByerInfo").click(function() {
                var buyerId = $(this).attr("ref");
                Boxy.iframeDialog({ title: "�ͻ���λ��Ϣ", iframeUrl: "/CRM/customerinfos/CustomerDetails.aspx?cId=" + buyerId, width: "775px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })
            
            $("a[name='update']").each(function(){
                var self=$(this);
                if($.trim(self.closest("tr").find("td[data-class='orderState']").text())!="�ѳɽ�"){
                    self.closest("td").find("a[name='shenqing']").remove();
                }
//                if( self.closest("td").attr("data-isext")=="True"){
//                    self.closest("tr").find("input[name='checkbox']").attr("disabled","true");
//                    self.remove()
//                }
            })
        });
    </script>

    </form>
</asp:Content>
