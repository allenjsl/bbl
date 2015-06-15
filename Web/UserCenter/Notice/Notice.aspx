<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Notice.aspx.cs" Inherits="Web.UserCenter.Notice.Notice" Title="公告通知_个人中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
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
            <div class="tablelist">
                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                    <tr bgcolor="#BDDCF4">
                        <th width="35%" align="center">
                            标题
                        </th>
                        <th width="12%" align="center">
                            浏览数
                        </th>
                        <th width="18%" align="center">
                            发布人
                        </th>
                        <th width="25%" align="center">
                            发布时间
                        </th>
                        <th width="10%" align="center">
                            操作
                        </th>
                    </tr>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr tid="<%# Eval("Id") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                <td align="left" class="pandl3">
                                    <%# Eval("Title")%>
                                </td>
                                <td align="center">
                                    <%# Eval("ClickNum")%>
                                </td>
                                <td align="center">
                                    <%# Eval("OperateName")%>
                                </td>
                                <td align="center">
                                    <%# Convert.ToDateTime(Eval("IssueTime")).ToString("yyyy-MM-dd HH:mm:ss")%>
                                </td>
                                <td align="center">
                                    <a href="javascript:;" class="show">查看</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <%if (len == 0)
                      { %>
                    <tr align="center">
                        <td colspan="5">
                            没有相关数据
                        </td>
                    </tr>
                    <%} %>
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
    </div>

    <script type="text/javascript">
	    $(function(){
	        $("a.show").bind("click",function(){
	            var that = $(this);
	            var tid = that.parent().parent().attr("tid");
	            if(tid){
	                location.href = "/UserCenter/Notice/NoticeShow.aspx?tid="+tid;
	            }
	            return false;
	        });
	    });
	    
    </script>

</asp:Content>
