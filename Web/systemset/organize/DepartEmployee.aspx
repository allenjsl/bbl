<%@ Page Title="������Ա_��֯����_ϵͳ����" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="DepartEmployee.aspx.cs" Inherits="Web.systemset.organize.DepartEmployee" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">ϵͳ����</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ�ã�ϵͳ����>> ��֯����
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 50px;">
            <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                <tr>
                    <% if (CheckGrant(Common.Enum.TravelPermission.ϵͳ����_��֯����_����������Ŀ))
                       { %>
                    <td width="100" align="center">
                        <a href="/systemset/organize/DepartManage.aspx">��������</a>
                    </td>
                    <%}
                       if (CheckGrant(Common.Enum.TravelPermission.ϵͳ����_��֯����_������Ա��Ŀ))
                       { %>
                    <td width="100" align="center" class="xtnav-on">
                        <a href="/systemset/organize/DepartEmployee.aspx">������Ա</a>
                    </td>
                    <%} %>
                </tr>
            </table>
        </div>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript;" onclick="return DepartEmp.add();">�� ��</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.update();">�� ��</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.copy('');">�� ��</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.del('');">ɾ ��</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="9%" align="center" bgcolor="#BDDCF4">
                        ȫѡ
                        <input type="checkbox" name="checkbox" id="chkAll" onclick="DepartEmp.checkAll(this);" />
                    </th>
                    <th width="15%" align="center" bgcolor="#BDDCF4">
                        ��������
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        ��ܲ���
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        ����
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        �Ա�
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        �绰
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        �ֻ�
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        QQ
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        ״̬
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        ��Ȩ
                    </th>
                </tr>
                <asp:CustomRepeater ID="rptEmployee" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%--checkbox isAllowDelete �Ƿ�����ɾ�� 0:����ɾ��--%>
                                <%#(bool)Eval("IsAdmin") == true ? "<input type=\"checkbox\" isAllowDelete=\"0\" class=\"c1\" value='" + Eval("Id") + "'/>" : "<input type=\"checkbox\"  isAllowDelete=\"1\" class=\"c1\" value='" + Eval("Id") + "'/>"%>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# Eval("DepartName") %>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# Eval("SuperviseDepartName") %>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactName%>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactSex%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactTel%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactMobile%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).QQ%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" onclick="return DepartEmp.setState('<%# Eval("Id") %>',this)">
                                    <%#((bool)Eval("IsEnable")) ? "��" : "��"%></a>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" target="_blank" onclick="return DepartEmp.setPermit('<%# Eval("Id") %>')">
                                    ��Ȩ</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%--checkbox isAllowDelete �Ƿ�����ɾ�� 0:����ɾ��--%>
                                <%#(bool)Eval("IsAdmin") == true ? "<input type=\"checkbox\" isAllowDelete=\"0\" class=\"c1\" value='" + Eval("Id") + "'/>" : "<input type=\"checkbox\"  isAllowDelete=\"1\" class=\"c1\" value='" + Eval("Id") + "'/>"%>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# Eval("DepartName") %>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# Eval("SuperviseDepartName") %>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactName%>
                            </td>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactSex%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactTel%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).ContactMobile%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# ((EyouSoft.Model.CompanyStructure.ContactPersonInfo)Eval("PersonInfo")).QQ%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" onclick="return DepartEmp.setState('<%# Eval("Id") %>',this)">
                                    <%#((bool)Eval("IsEnable")) ? "��" : "��"%></a>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" target="_blank" onclick="return DepartEmp.setPermit('<%# Eval("Id") %>')">
                                    ��Ȩ</a>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:CustomRepeater>
                <tr>
                    <td height="30" colspan="10" align="right" class="pageup">
                        <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"
                            runat="server"></uc2:ExportPageInfo>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/javascript">
        var DepartEmp =
                {
                    //�򿪵���
                    openDialog: function(p_url, p_title, p_width, p_height) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
                    },
                    //�޸�Ա��
                    update: function() {
                        var chks = $("input:checked").not("#chkAll")
                        var eId = "";
                        if (chks.length != 1) {
                            alert("��ѡ��һλԱ��");
                            return false;
                        }
                        else
                            eId = chks.val();
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx?empId=" + eId, "�޸�Ա��", "800px", "380px");
                        return false;
                    },
                    //�������Ա��
                    copy: function() {
                        var eId = "";
                        if ($(".c1:checked").length == 1) {
                            eId = $(".c1:checked").val();
                        }
                        else {
                            alert("��ѡ��һλ���ҽ�ΪһλԱ����");
                            return false;
                        }
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx?copy=copy&empId=" + eId, "����Ա��", "800px", "380px");
                        return false;
                    },
                    //���Ա��
                    add: function() {
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx", "����Ա��", "800px", "380px");
                        return false;
                    },
                    del: function() {
                        var ids = "";
                        $(".c1:checked").each(function() {
                            if ($(this).attr("isAllowDelete") == "0") {
                                alert("����Ա�˺Ų���ɾ����");
                                $(this).attr("checked", false);
                                return true;
                            }
                            ids += $(this).val() + ",";
                        });
                        if (ids != "") {
                            if (confirm("��ȷ��Ҫɾ����ѡ������Ա��")) {
                                window.location = "/systemset/organize/DepartEmployee.aspx?method=del&ids=" + ids;
                            }
                        }
                        else {
                            alert("��ѡ��Ҫɾ����Ա����");
                        }
                        return false;
                    },
                    //��Ȩ
                    setPermit: function(eId) {
                        DepartEmp.openDialog("/systemset/organize/SetPermit.aspx?empId=" + eId, "��Ȩ", "900px", "500px");
                        return false;
                    },
                    //����״̬
                    setState: function(sid, tar) {
                        var sonFont = $(tar);
                        var nowM = ""; //��ǰ����
                        if (sonFont.html() == "��") {
                            nowM = "start";
                        }
                        else {
                            nowM = "stop";
                        }
                        $.newAjax({
                            type: "get",
                            dataType: "json",
                            url: "DepartEmployee.aspx?method=setState",
                            data: { hidMethod: nowM, ids: sid },
                            cache: false,
                            success: function(result) {
                                if (result.success == "1") {
                                    if (nowM == "stop") {
                                        alert("�ѹر�");
                                        sonFont.html("��");
                                    }
                                    else {
                                        alert("�ѿ���");
                                        sonFont.html("��");
                                    }
                                }
                                else {
                                    alert("����ʧ�ܣ�");
                                }
                            }
                        });
                    },
                    checkAll: function(chk) {

                        var chked = $(chk).attr("checked");
                        $(".c1:checkbox").attr("checked", chked);
                    }

                }
    </script>

</asp:Content>
