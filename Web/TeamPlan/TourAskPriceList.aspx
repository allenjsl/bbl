<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourAskPriceList.aspx.cs"
    Inherits="Web.TeamPlan.TourAskPriceList" MasterPageFile="~/masterpage/Back.Master"
    Title="�ŶӼƻ�_������ѯ��" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">�ŶӼƻ�</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>> �ŶӼƻ�>> ������ѯ��
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
                    <img src="../images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            �źţ�</label>
                        <input type="text" name="textfield18" id="txt_teamNum" class="searchinput searchinput03"
                            runat="server" />
                        <label>
                            ��·���ƣ�</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_teamName" runat="server" />
                        <label>
                            ������</label>
                        <input type="text" name="textfield" id="txt_dayCount" class="searchinput searchinput03"
                            runat="server" />
                        <label>
                            Ԥ�Ƴ������ڣ�</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_beginDate" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        ��
                        <input type="text" name="textfield" id="txt_endDate" class="searchinput" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        <br />
                        <label>
                            ѯ�����ڣ�</label>
                        <input name="textfield" type="text" class="searchinput" id="txt_AskBegin" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        ��
                        <input type="text" name="textfield" id="txt_AskEnd" class="searchinput" onfocus="WdatePicker()"
                            runat="server" readonly="readonly" />
                        <label>
                            <a href="javascript:void(0);" id="search" onclick="searchQuote()">
                                <img src="../images/searchbtn.gif" style="vertical-align: top;" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:void(0);" onclick="delQuotes()">ɾ ��</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr class="odd">
                    <th width="4%" height="28" align="center" nowrap="nowrap">
                        <input type="checkbox" name="checkbox" id="checkAll" />
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        ��·����
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        ��������
                    </th>
                    <th width="8%" align="center" nowrap="nowrap">
                        ����
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        ѯ�۵�λ
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                        ѯ��ʱ��
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        ѯ����
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        ��ϵ�绰
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        ״̬
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                        ����
                    </th>
                </tr>
                <asp:Repeater ID="rpTour" runat="server">
                    <ItemTemplate>
                        <tr class="even">
                            <td height="28" align="center">
                                <input type="checkbox" name="checkbox" class="check_child" id="child_<%#Eval("ID") %>" /><%# Container.ItemIndex + 1+( this.pageIndex - 1) * this.pageSize%>
                            </td>
                            <td align="center">
                                <%#Eval("RouteName")%>
                            </td>
                            <td align="center">
                                <%#Convert.ToDateTime(Eval("LeaveDate")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td align="center">
                                <%#Eval("PeopleNum")%>
                            </td>
                            <td align="center">
                                <%#Eval("CustomerName")%>
                            </td>
                            <td align="center">
                                <%#Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactName")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactTel")%>
                            </td>
                            <td align="center">
                                <%#Eval("QuoteState")%>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" id="creatQuote" onclick="createQuoteById('<%#Eval("Id") %>')">
                                    ����</a> | <a href="javascript:void(0);" id="delQuote" onclick="delQuoteById('<%#Eval("Id") %>')">
                                        ɾ��</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr class="odd">
                    <th width="4%" height="28" align="center" nowrap="nowrap">
                    �ϼ�</th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="8%" align="center" nowrap="nowrap">
                        <asp:Literal ID="peopleCount" runat="server"></asp:Literal>
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="12%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                    <th width="10%" align="center" nowrap="nowrap">
                    </th>
                </tr>
                <tr>
                    <td height="30" colspan="9" align="right" class="pageup">
                        <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

    <script type="text/javascript" src="../js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
        //�����¼�
        function createQuoteById(id) {
            var url = "/TeamPlan/TourQuotePrice.aspx?type=create&id=" + id;
            Boxy.iframeDialog({
                iframeUrl: url,
                title: "����",
                modal: true,
                width: "850px",
                height: "580px"
            });
            return false;
        }
        //ɾ���¼�
        function delQuoteById(id) {
            if (confirm("ȷ��ɾ��ô")) {
                $.newAjax({
                    url: "TourQuotePrice.aspx?type=del&ids=" + id + "|",
                    type: "GET",
                    cache: false,
                    success: function(result) {
                        if (result == "True") {
                            alert("ɾ���ɹ�");
                            window.location = location;
                        } else {
                            alert("ɾ��ʧ��")
                        }
                    }
                })
            }

            return false;
        }
        //����ɾ���¼�
        function delQuotes() {
            var id = "";
            var count = 0;

            $(".check_child").each(function() {
                if ($(this).attr("checked") == true) {
                    id += $(this).attr("id").split("_")[1] + "|";
                    count++;
                }
            })

            if (count > 0) {
                if (confirm("ȷ��Ҫɾ��ô")) {
                    $.newAjax({
                        url: "TourQuotePrice.aspx?type=del&ids=" + id,
                        type: "GET",
                        cache: false,
                        success: function(result) {
                            if (result == "True") {
                                alert("ɾ���ɹ�");
                                window.location = location;
                            } else {
                                alert("ɾ��ʧ��")
                            }
                        }
                    })
                }
            } else {
                alert("������ѡ��һ������");
                return false;
            }

        }
        //��ѯ�¼�
        function searchQuote() {
            if (checkTime()) {
                var teamNum = $("#<%=txt_teamNum.ClientID %>").val();
                var teamName = $("#<%=txt_teamName.ClientID %>").val();
                var begTime = $("#<%=txt_beginDate.ClientID %>").val();
                var endTime = $("#<%=txt_endDate.ClientID %>").val();
                var dayCount = $("#<%=txt_dayCount.ClientID %>").val();
                var askBegin = $("#<%=txt_AskBegin.ClientID %>").val();
                var askEnd = $("#<%=txt_AskEnd.ClientID %>").val();
                var url = "TourAskPriceList.aspx?teamNum=" + teamNum + "&teamName=" + teamName + "&begTime=" + begTime + "&endTime=" + endTime + "&dayCount=" + dayCount + "&askbegin=" + askBegin + "&askend=" + askEnd;
                window.location.href = url;
            } else {
                alert("��ֹ���ڲ��ܴ�����ʼ����");
                return;
            }

        }

        //�ж�ʱ������
        function checkTime() {
            var res = true;
            var begTime = $("#<%=txt_beginDate.ClientID %>").val();
            var endTime = $("#<%=txt_endDate.ClientID %>").val();

            if (endTime != "" && begTime != "") {
                if (begTime > endTime) {
                    res = false;
                }
            }
            return res;

        }
        $(function() {
            //ȫѡ��ť
            $("#checkAll").click(function() {
                if ($(this).attr("checked") == true) {
                    $(".check_child").attr("checked", true)
                } else {
                    $(".check_child").attr("checked", false)
                }
            })
        })
        
    </script>

</asp:Content>
