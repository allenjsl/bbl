<%@ Page Title="部门人员_组织机构_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master"
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
                        <span class="lineprotitle">系统设置</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：系统设置>> 组织机构
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
                    <% if (CheckGrant(Common.Enum.TravelPermission.系统设置_组织机构_部门设置栏目))
                       { %>
                    <td width="100" align="center">
                        <a href="/systemset/organize/DepartManage.aspx">部门名称</a>
                    </td>
                    <%}
                       if (CheckGrant(Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目))
                       { %>
                    <td width="100" align="center" class="xtnav-on">
                        <a href="/systemset/organize/DepartEmployee.aspx">部门人员</a>
                    </td>
                    <%} %>
                </tr>
            </table>
        </div>
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript;" onclick="return DepartEmp.add();">新 增</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.update();">修 改</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.copy('');">复 制</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return DepartEmp.del('');">删 除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="9%" align="center" bgcolor="#BDDCF4">
                        全选
                        <input type="checkbox" name="checkbox" id="chkAll" onclick="DepartEmp.checkAll(this);" />
                    </th>
                    <th width="15%" align="center" bgcolor="#BDDCF4">
                        所属部门
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        监管部门
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        姓名
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        性别
                    </th>
                    <th width="13%" align="center" bgcolor="#bddcf4">
                        电话
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        手机
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        QQ
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        状态
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        授权
                    </th>
                </tr>
                <asp:CustomRepeater ID="rptEmployee" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%--checkbox isAllowDelete 是否允许删除 0:不可删除--%>
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
                                    <%#((bool)Eval("IsEnable")) ? "√" : "×"%></a>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" target="_blank" onclick="return DepartEmp.setPermit('<%# Eval("Id") %>')">
                                    授权</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td height="35" align="center" bgcolor="#e3f1fc">
                                <%--checkbox isAllowDelete 是否允许删除 0:不可删除--%>
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
                                    <%#((bool)Eval("IsEnable")) ? "√" : "×"%></a>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <a href="javascript:;" target="_blank" onclick="return DepartEmp.setPermit('<%# Eval("Id") %>')">
                                    授权</a>
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
                    //打开弹窗
                    openDialog: function(p_url, p_title, p_width, p_height) {
                        Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
                    },
                    //修改员工
                    update: function() {
                        var chks = $("input:checked").not("#chkAll")
                        var eId = "";
                        if (chks.length != 1) {
                            alert("请选择一位员工");
                            return false;
                        }
                        else
                            eId = chks.val();
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx?empId=" + eId, "修改员工", "800px", "380px");
                        return false;
                    },
                    //复制添加员工
                    copy: function() {
                        var eId = "";
                        if ($(".c1:checked").length == 1) {
                            eId = $(".c1:checked").val();
                        }
                        else {
                            alert("请选择一位，且仅为一位员工！");
                            return false;
                        }
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx?copy=copy&empId=" + eId, "新增员工", "800px", "380px");
                        return false;
                    },
                    //添加员工
                    add: function() {
                        DepartEmp.openDialog("/systemset/organize/EmployeeEdit.aspx", "新增员工", "800px", "380px");
                        return false;
                    },
                    del: function() {
                        var ids = "";
                        $(".c1:checked").each(function() {
                            if ($(this).attr("isAllowDelete") == "0") {
                                alert("管理员账号不能删除！");
                                $(this).attr("checked", false);
                                return true;
                            }
                            ids += $(this).val() + ",";
                        });
                        if (ids != "") {
                            if (confirm("你确定要删除所选部门人员吗？")) {
                                window.location = "/systemset/organize/DepartEmployee.aspx?method=del&ids=" + ids;
                            }
                        }
                        else {
                            alert("请选择要删除的员工！");
                        }
                        return false;
                    },
                    //授权
                    setPermit: function(eId) {
                        DepartEmp.openDialog("/systemset/organize/SetPermit.aspx?empId=" + eId, "授权", "900px", "500px");
                        return false;
                    },
                    //设置状态
                    setState: function(sid, tar) {
                        var sonFont = $(tar);
                        var nowM = ""; //当前操作
                        if (sonFont.html() == "×") {
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
                                        alert("已关闭");
                                        sonFont.html("×");
                                    }
                                    else {
                                        alert("已开启");
                                        sonFont.html("√");
                                    }
                                }
                                else {
                                    alert("设置失败！");
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
