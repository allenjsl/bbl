<%@ Page Title="轮换图片-同行平台" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="RotateImg.aspx.cs" Inherits="Web.systemset.ToGoTerrace.RotateImg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">系统设置</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> <a href="#">系统设置 >> 同行平台</a>>> 轮换图片
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
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/BaseManage.aspx">基础设置</a>
                        </td>
                        <td width="100" align="center" class="xtnav-on">
                            <a href="javascript:void(0)">轮换图片</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/TickePoliyList.aspx">机票政策</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/FriendshipLink.aspx">友情链接</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="800" border="0" align="center" cellpadding="2" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            添加轮换图片  <span style="color:#666">(上传图片尺寸:宽710px,高290px)</span>
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>图片1上传：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (con > 0)
                              {
                                  if (!string.IsNullOrEmpty(list[0].FilePath))
                                  { %>
                            <a target="_blank" href="<%=list[0].FilePath %>">图片预览</a>&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                                onclick="del(<%=list[0].Id %>,0)">删除</a>
                            <%}
                                  else
                                  { %>
                            <input type="file" name="workAgree0" />
                            <%}%><input name="tid1" type="hidden" value="<%=list[0].Id %>" /><%
                                                                                                 }
                              else
                              {%><input type="file" name="workAgree0" /><% }%>
                            <asp:Literal ID="lstMsg1" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <b>自定义链接：</b>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_URL1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>图片2上传：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (con > 1)
                              {
                                  if (!string.IsNullOrEmpty(list[1].FilePath))
                                  { %>
                            <a target="_blank" href="<%=list[1].FilePath %>">图片预览</a>&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                                onclick="del(<%=list[1].Id %>,1)">删除</a>
                            <%}
                                  else
                                  { %>
                            <input type="file" name="workAgree1" />
                            <%}%><input name="tid2" type="hidden" value="<%=list[1].Id %>" /><%
                                                                                                 }
                              else
                              {%><input type="file" name="workAgree1" /><% }%>
                            <asp:Literal ID="lstMsg2" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <b>自定义链接：</b>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_URL2" runat="server"></asp:TextBox>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>图片3上传：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (con > 2)
                              {
                                  if (!string.IsNullOrEmpty(list[2].FilePath))
                                  { %>
                            <a target="_blank" href="<%=list[2].FilePath %>">图片预览</a>&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                                onclick="del(<%=list[2].Id %>,2)">删除</a>
                            <%}
                                  else
                                  { %>
                            <input type="file" name="workAgree2" />
                            <%}%><input name="tid3" type="hidden" value="<%=list[2].Id %>" /><%
                                                                                                 }
                              else
                              {%><input type="file" name="workAgree2" /><% }%>
                            <asp:Literal ID="lstMsg3" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <b>自定义链接：</b>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_URL3" runat="server"></asp:TextBox>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>图片4上传：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (con > 3)
                              {
                                  if (!string.IsNullOrEmpty(list[3].FilePath))
                                  { %>
                            <a target="_blank" href="<%=list[3].FilePath %>">图片预览</a>&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                                onclick="del(<%=list[3].Id %>,3)">删除</a>
                            <%}
                                  else
                                  { %>
                            <input type="file" name="workAgree3" />
                            <%}%><input name="tid4" type="hidden" value="<%=list[3].Id %>" /><%
                                                                                                 }
                              else
                              {%><input type="file" name="workAgree3" /><% }%>
                            <asp:Literal ID="lstMsg4" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <b>自定义链接：</b>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_URL4" runat="server"></asp:TextBox>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>图片5上传：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <%if (con > 4)
                              {
                                  if (!string.IsNullOrEmpty(list[4].FilePath))
                                  { %>
                            <a target="_blank" href="<%=list[4].FilePath %>">图片预览</a>&nbsp;&nbsp;&nbsp; <a href="javascript:void(0);"
                                onclick="del(<%=list[4].Id %>,4)">删除</a>
                            <%}
                                  else
                                  { %>
                            <input type="file" name="workAgree4" />
                            <%}%><input name="tid5" type="hidden" value="<%=list[4].Id %>" /><%
                                                                                                 }
                              else
                              {%><input type="file" name="workAgree4" /><% }%>
                            <asp:Literal ID="lstMsg5" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <b>自定义链接：</b>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_URL5" runat="server"></asp:TextBox>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="3" align="center">
                            <table width="459" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="137" height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return checking()"
                                            OnClick="LinkButton1_Click">保存</asp:LinkButton>
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
    <div class="clearboth">
    </div>

    <script type="text/javascript">
        $(function() {
            //删除原有图片
            $("img.close").click(function() {
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //生成添加协议的控件
            });
            $("#Save").click(function() {
                var form = $(this).closest("form").get(0); //查找form
                form.submit();
            });


        })


        function del(id, bid) {
            if (confirm("确认删除所选项?")) {
                $.newAjax({
                    type: "POST",
                    url: "RotateImg.aspx?id=" + id + "&bid=" + bid,
                    dataType: 'json',
                    success: function(msg) {
                        if (msg.res) {
                            switch (msg.res) {
                                case 1:
                                    alert("删除成功!");
                                    location.reload();
                                    break;
                                default:
                                    alert("删除失败!");
                            }
                        }
                    },
                    error: function() {
                        alert("服务器繁忙!");
                    }
                });
            }
            return false;
        }
        function checking() {
            var result = true;
            
            var msg = "";
            $(".tablelist").find("input[type='file']").each(function() {
                if ($.trim($(this).val()) != "") {
                    if (!/\.jpg|\.jpeg|\.gif|\.png$/i.test($(this).val())) {
                        var name = $(this).attr("name");
                        var index = parseInt(name.substring(name.length - 1)) + 1;
                        msg += ("图片 " + index + " 类型必须是：.gif,jpeg,jpg,png中的一种\n");
                        result = false;
                    }
                    
                }
            });
            if (!result) {
                alert(msg);
            }
            return result;
        }
    </script>

    </form>
</asp:Content>
