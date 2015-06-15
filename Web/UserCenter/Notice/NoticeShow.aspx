<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="NoticeShow.aspx.cs" Inherits="Web.UserCenter.Notice.NoticeShow" Title="公告通知_个人中心" %>

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
                            所在位置>>个人中心>>公告通知
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height:50px;">
            </div>
            <div class="tablelist">
                <table width="880" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            公告通知查看
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>标题：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                            <% =nModel.Title %>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#BDDCF4">
                            <strong>内容：</strong>
                        </td>
                        <td height="100" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                            <% =nModel.Content %>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>附件：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (!string.IsNullOrEmpty(nModel.UploadFiles))
                              { %>
                                <a href="<% =nModel.UploadFiles%>" target="_blank">点击下载</a>
                            <%}
                              else
                              { %>
                                无
                            <%} %>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#BDDCF4">
                            <strong>发布人：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                            <% =nModel.OperatorName%>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>发布时间：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <label>
                            <% =nModel.IssueTime.ToString("yyyy-MM-dd HH:mm:ss") %>
                            </label>
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
        </div>
        <!-- InstanceEndEditable -->
    </div>
</asp:Content>
