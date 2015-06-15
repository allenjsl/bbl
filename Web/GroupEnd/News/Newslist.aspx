<%@ Page Title="���¶�̬" Language="C#" MasterPageFile="~/masterpage/Front.Master" AutoEventWireup="true"
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
                            <span class="lineprotitle">���¶�̬</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            ����λ�� >> ��ҳ >> ���¶�̬
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
                                ���⣺</label>
                            <input type="text" class="searchinput" id="Titles" value="<%=Titles %>" />
                            <label>
                                �����ˣ�</label>
                            <input type="text" class="searchinput" id="OperateName" value="<%=OperateName %>" />
                            <label>
                                ����ʱ�䣺</label>
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
                                ȫѡ<input type="checkbox" name="checkbox" class="selall" />
                            </th>--%>
                            <th width="37%" align="center" bgcolor="#bddcf4">
                                ����
                            </th>
                            <th width="19%" align="center" bgcolor="#bddcf4">
                                ������
                            </th>
                            <th width="18%" align="center" bgcolor="#bddcf4">
                                ����ʱ��
                            </th>
                            <th width="15%" align="center" bgcolor="#bddcf4">
                                ����
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
                                        <a href="javascript:void(0);" class="show">�鿴</a>
                                        <%--<%  if (grantmodify)
                                          { %><a href="javascript:void(0);" class="modify">�޸�</a>
                                        <%} if (grantdel)
                                          { %><a href="javascript:void(0);" class="del">ɾ��</a><%} %>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%if (len == 0)
                          { %>
                        <tr align="center">
                            <td colspan="5">
                                û���������
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
                //������
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
                    title: "�鿴",
                    modal: true,
                    width: "820px",
                    height: "330px"
                });
                return false;

            },
            del: function(that) {
                if (confirm("ȷ��ɾ����ѡ��?")) {
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
                            url: "/SupplierControl/Others/Default.aspx?act=areadel",
                            type: "POST",
                            data: { "tid": tid },
                            dataType: 'json',
                            success: function(d) {
                                if (d.res) {
                                    switch (d.res) {
                                        case 1:
                                            alert("ɾ���ɹ�");
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
                Others.loadxls(); //����
                return false;
            });
            $("a.add").click(function() {
                Others.add(); //���
                return false;
            });
            $("a.show").click(function() {
                Others.show($(this)); //�鿴
                return false;

            });

            $("a.trade").click(function() {
                Others.trade($(this)); //�鿴���׼�¼
                return false;
            });
            $("a.modify").click(function() {
                Others.modify($(this)); //�޸�
                return false;
            });
            $("a.del").click(function() {
                Others.del($(this)); //ɾ��
                return false;
            });
            $("a.delall").click(function() {
                Others.delall(RowList); //����ɾ��
                return false;
            });

            $("#searchbtn").click(function() {
                Others.select(); //��ѯ
                return false;
            });
            $(".searchinput").keypress(function(e) {
                if (e.keyCode == 13) {
                    Others.select(); //��ѯ
                    return false;
                }
            });
            $("a.toexcel").click(function() {
                Others.toxls(); //����
                return false;
            });

            ///ȫѡ�¼�
            $("input.selall").click(function() {
                var that = $(this);
                if (that.attr("checked")) {
                    RowList.SetAllCheckValue("checked");
                } else {
                    RowList.SetAllCheckValue("");
                }
            });


            //��ѯ��ť
            $("#Btn_Serach").click(function() {
                LineProductList.OnSearch();
                return false;
            });
            //�س���ѯ
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
