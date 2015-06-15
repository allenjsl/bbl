<%@ Page Title="��Ҫѯ��" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
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
                                <span class="lineprotitle">�г̱���</span>
                            </td>
                            <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right">
                                ����λ��&gt;&gt; <a href="#">�г̱���</a>&gt;&gt; ��Ҫѯ��
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
                                <a href="#">��Ҫѯ��</a>
                            </td>
                            <td align="center">
                                <a href="DownloadMoney.aspx">���ر��ۼ��г�</a>
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
                                <a href="javascript:;" class="add">�� ��</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:;" class="update">�� ��</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:;" class="alldel">ɾ ��</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table border="0" cellpadding="0" cellspacing="1" width="99%">
                    <tr>
                        <th align="center" bgcolor="#bddcf4" height="28" width="6%">
                            ѡ��
                        </th>
                        <td align="center" bgcolor="#bddcf4" width="28%">
                            <strong>��·����</strong>
                        </td>
                        <td align="center" bgcolor="#bddcf4" width="16%">
                            <strong>��������</strong>
                        </td>
                        <th align="center" bgcolor="#bddcf4" width="10%">
                            ����
                        </th>
                        <th align="center" bgcolor="#bddcf4" width="10%">
                            ѯ����
                        </th>
                        <th align="center" bgcolor="#bddcf4" width="16%">
                            ״̬
                        </th>
                        <td align="center" bgcolor="#bddcf4" width="10%">
                            <strong>����</strong>
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
                                        <%# (EyouSoft.Model.EnumType.TourStructure.QuoteState)Eval("QuoteState") == EyouSoft.Model.EnumType.TourStructure.QuoteState.�ѳɹ� ? "�鿴" : "�޸�"%>
                                        </a> <a href="javascript:void(0);" class="del">ɾ��</a>
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


                add: function() {//��ת����ҳ��
                    window.location.href = "/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx?type=Add";
                    return false;
                },


                update: function(tid) {//��ת�޸�ҳ��
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
                                alert("ֻ���޸�һ����¼��");
                            }
                        } else {
                            alert("��ѡ��һ����¼��");
                        }
                        return false;
                    }
                    window.location.href = "/GroupEnd/JourneyMoney/SelectMoneyAdd.aspx?type=Update&tid=" + tid;
                },


                del: function(tid) {
                    if (confirm("ȷ��ɾ����ѡ��?")) {
                        $.newAjax({
                            type: "POST",
                            data: { "tid": tid },
                            url: "/GroupEnd/JourneyMoney/SelectMoney.aspx?act=del",
                            dataType: 'json',
                            success: function(msg) {
                                if (msg.res) {
                                    switch (msg.res) {
                                        case 1:
                                            alert("ɾ���ɹ�!");
                                            location.reload();
                                            break;
                                        default:
                                            alert("ɾ��ʧ��!");
                                    }
                                }
                            },
                            error: function() {
                                alert("��������æ!");
                            }
                        });
                    }
                    return false;
                },
                delall: function(RowList) {
                    if (RowList.GetListSelectCount() > 0) {
                        if (confirm("ȷ��ɾ����ѡ��?")) {
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
                                                alert("ɾ���ɹ�");
                                                $(':checkbox').attr("checked", "");
                                                location.reload();
                                                break;
                                            case -1:
                                                alert("ɾ��ʧ��");
                                                break;
                                        }
                                    }
                                },
                                error: function() {
                                    alert("��������æ!");
                                }
                            });
                        }
                    } else {
                        alert("��ѡ��Ҫɾ����");
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

                $("a.del").click(function() {//ɾ��
                    var tid = $(this).parent().parent().attr("tid");
                    operate.del(tid); //ɾ��
                    return false;

                });
                $("a.alldel").click(function() {//����ɾ��

                    operate.delall(RowList); //ɾ��
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
