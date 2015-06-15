<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="WorkReportShow.aspx.cs" Inherits="Web.UserCenter.WorkExchange.WorkReportShow" Title="工作汇报查看_工作交流_个人中心"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <form id="form1" enctype="multipart/form-data" runat="server">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">个人中心</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> 个人中心>> 工作交流
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
                        <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,9) %>
                    </tr>
                </table>
            </div>
            <% if (isshow)
               { %>
            <div class="tablelist">
                <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            <strong>工作汇报查看</strong> 
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>标题：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <% =wrModel.Title%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>汇报内容：</strong>
                        </td>
                        <td height="120" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <% =wrModel.Description%>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>附件：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3 updom">
                            <%if (string.IsNullOrEmpty(wrModel.FilePath))
                              { %>
                                <label>无</label>
                            <%}
                              else
                              {
                            %>
                            <a href="<%=wrModel.FilePath %>" target="_blank">
                                点击下载</a>
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>汇报部门：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wrModel.DepartmentName%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>汇报人：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wrModel.OperatorName%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>汇报时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wrModel.ReportingTime.ToString("yyyy-MM-dd")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>状态：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wrModel.Status%></label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>评语：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label><% =wrModel.Comment%></label> 
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
               工作报告不存在或无权查看！
            <%} %>
        </div>
        <!-- InstanceEndEditable -->
        </form>
    </div>
</asp:Content>
