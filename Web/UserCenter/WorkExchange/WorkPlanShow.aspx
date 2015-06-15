<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="WorkPlanShow.aspx.cs" Inherits="Web.UserCenter.WorkExchange.WorkPlanShow"
    Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">个人中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> 个人中心>> 工作交流>> 工作计划查看
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                        <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,10) %>
                    </tr>
                </table>
            </div>
            <% if (isshow)
               { %>
            <div class="tablelist">
                <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            工作计划查看
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>编号：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%= wpModel.PlanNO%>
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>提交人：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wpModel.OperatorName%></label>
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>接收人：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <% =acceptname.ToString() %>
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>计划标题：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wpModel.Title%></label> 
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>计划内容：</strong>
                        </td>
                        <td height="60" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wpModel.Description%></label> 
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>附件：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3 updom">
                            <%if (string.IsNullOrEmpty(wpModel.FilePath))
                              { %>
                            无
                            <%}
                              else
                              {
                            %>
                            <a href="<%=wpModel.FilePath%>" target="_blank">点击查看</a>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>计划说明：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <% =wpModel.Remark%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>预计完成时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%= wpModel.ExpectedDate.ToString("yyyy-MM-dd")%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>实际完成时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%= wpModel.ActualDate.ToString("yyyy-MM-dd")%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>状态：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%= wpModel.Status%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>上级部门评语：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <% =wpModel.DepartmentComment%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>总经理评语：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                           <% =wpModel.ManagerComment%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>填写时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <%= wpModel.CreateTime.ToString("yyyy-MM-dd")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>最后修改时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                                <%= wpModel.LastTime == DateTime.MaxValue ? "" : wpModel.LastTime.ToString("yyyy-MM-dd")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="3" align="center">
                            <table border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="86" height="40" align="center" class="tjbtn02">
                                        <a href="javascript:history.go(-1);">返回</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <%}
               else
               { %>
               工作计划不存在或者无权查看！
            <%} %>
        </div>
        <!-- InstanceEndEditable -->
    </div>
</asp:Content>
