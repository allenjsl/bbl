<%@ Page Title="角色管理_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="RolesManage.aspx.cs" Inherits="Web.systemset.rolemanage.RolesManage" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .oddTr td
        {
            background: #e3f1fc;
        }
        .evenTr td
        {
            background: #BDDCF4;
        }
    </style>
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
                        所在位置：系统设置>> 角色管理
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="lineCategorybox" style="height: 30px;">
        </div>
        <form runat="server" id="roleManageForm">
        <div class="btnbox">
            <table border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Role.add();">新 增</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Role.copy();">复 制</a>
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Role.del();">删 除</a>
                    </td>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="8%" align="center" bgcolor="#BDDCF4">
                        全选
                        <input id="chkAll" type="checkbox" onclick="Role.selAll(this);" />
                    </th>
                    <th width="8%" align="center" bgcolor="#BDDCF4">
                        编号
                    </th>
                    <th width="24%" align="center" bgcolor="#BDDCF4">
                        角色名称
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        全选
                        <input type="checkbox" onclick="Role.selAll(this);" />
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        编号
                    </th>
                    <th width="24%" align="center" bgcolor="#bddcf4">
                        角色名称
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        操作
                    </th>
                </tr>
                <asp:CustomRepeater runat="server" ID="rptRoles">
                    <ItemTemplate>
                        <%# GetListTr()%>
                        <td align="center">
                            <%#Eval("RoleName").ToString() != "管理员"? "<input type=\"checkbox\" class=\"c1\" value='"+Eval("Id") +"' />":"" %>
                        </td>
                        <td align="center">
                            <%# itemIndex2++%>
                        </td>
                        <td align="center">
                            <%# Eval("RoleName") %>
                        </td>
                        <td align="center">
                            <a href="javascript:;" onclick="return Role.update('<%#Eval("Id") %>');">修改</a>
                            <%#Eval("RoleName").ToString() != "管理员"?"|<a href=\"javascript:;\" onclick=\"return Role.del('"+Eval("Id")+"');\">删除</a>":"" %>
                        </td>
                    </ItemTemplate>
                </asp:CustomRepeater>
                <%=GetLastTr() %>
                <tr>
                    <td height="30" colspan="8" align="right" class="pageup">
                        <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"
                            runat="server"></uc2:ExportPageInfo>
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>

    <script type="text/javascript">
        var Role =
			     {
			         //打开弹窗
			         openDialog: function(p_url, p_title) {
			             Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "900px", height: "500px" });
			         },
			         //复制添加角色
			         copy: function() {
			             var chks = $("input:checked").not("#chkAll");
			             if (chks.length != 1) {
			                 alert("请选择一个要复制角色！");
			                 return false;
			             }
			             Role.openDialog("/systemset/rolemanage/RoleEdit.aspx?method=copy&roleId=" + chks.val(), "添加角色");
			             return false;
			         },
			         //添加角色
			         add: function() {
			             Role.openDialog("/systemset/rolemanage/RoleEdit.aspx", "添加角色");
			             return false;
			         },
			         selAll: function(tar) {
			             var isChecked = $(tar).attr("checked");
			             $(":checkbox").attr("checked", isChecked);
			         },
			         del: function(theId) {
			             var mess = "你确定要删除该角色吗？";
			             var ids = "";
			             if (theId) {
			                 ids = theId;
			             }
			             else {
			                 var idArr = [];
			                 $(".c1:checked").each(function() {
			                     idArr.push($(this).val());
			                 });
			                 ids = idArr.toString();
			                 mess = "你确定要删除所选角色吗？";
			             }
			             if (ids != "") {
			                 if (confirm(mess)) {
			                     window.location = "/systemset/rolemanage/RolesManage.aspx?method=del&roleIds=" + ids;
			                 }
			             }
			             else {
			                 alert("请选择要删除的角色！");
			             }
			             return false;
			         },
			         update: function(rId) {
			             Role.openDialog("/systemset/rolemanage/RoleEdit.aspx?method2=update&roleId=" + rId, "修改角色");
			         }
			     }
			     
    </script>

</asp:Content>
