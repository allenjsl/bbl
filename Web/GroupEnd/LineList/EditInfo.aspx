<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditInfo.aspx.cs" Inherits="Web.GroupEnd.Orders.EditInfo" Title="信息编辑" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>编辑信息</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
       <table width="800" border="0" align="center" cellpadding="0" cellspacing="0" style="margin:10px;">
              <tr class="odd" style="font-size:14px;">
                <th width="2%" height="34" align="center" >&nbsp;</th>
                <th width="24%" align="center" ><font class="font12bblue">编辑信息</font></th>
                <td width="74%">&nbsp;</td>
              </tr>
              <tr class="even">
                <td height="25">&nbsp;</td>
                <td height="35" align="right">企业名称：</td>
                <td><%= SiteUserInfo.TourCompany.CompanyName %></td>
              </tr>
              <tr class="odd">
                <td height="25">&nbsp;</td>
                <td height="35" align="right">电话：</td>
                <td><input type="text" name="CompanyTEl" id="CompanyTEl" class="companyInfo" value="<% =cModel.PersonInfo.ContactTel %>"  /></td>
              </tr>
              <tr class="even">
                <td height="25">&nbsp;</td>
                <td height="35" align="right">手机：</td>
                <td><input type="text" name="CompanyPhone" id="CompanyPhone" class="companyInfo"  value="<% =cModel.PersonInfo.ContactMobile %>" /></td>
              </tr>
              <tr style="font-size:14px;" class="odd">
                <th width="2%" height="34" align="center" >&nbsp;</th>
                <th width="24%" align="center" ><font class="font12bblue">下载文档页眉页脚设置</font></th>
                <td width="74%">&nbsp;</td>
              </tr>
              <tr class="even">
                <td height="25">&nbsp;</td>
                <th height="35" align="right">打印页眉：</th>
                <td>
                <%if (string.IsNullOrEmpty(cptModel.PageHeadFile))
                  { %>
                    <input type="file" name="PrintTop" />
                <%}
                  else
                  {
                 %>
                 <a target="_blank" href="<%=cptModel.PageHeadFile%>">查看页眉</a><img src="/images/fujian_x.gif" style="cursor:pointer" width="14" height="13" class="close" alt="" name="PrintTop" />
                 <%} %>
                </td> 
              </tr>
              <tr>
                <td height="1" colspan="3" bgcolor="#93DAF0"></td>
              </tr>
              <tr class="odd">
                <td height="25">&nbsp;</td>
                <th height="35" align="right">打印页脚：</th>
                <td>
                <%if (string.IsNullOrEmpty(cptModel.PageFootFile))
                  { %>
                    <input type="file" name="PrintFooter" />
                <%}
                  else
                  {
                 %>
                 <a target="_blank" href="<%=cptModel.PageFootFile%>">查看页脚</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" name="PrintFooter" />
                 <%} %>
                </td> 
              </tr>
              <tr>
                <td height="1" colspan="3" bgcolor="#93DAF0"></td>
              </tr>
              <tr class="even">
                <td height="25">&nbsp;</td>
                <th height="35" align="right">打印模板：</th>
                <td>
                <%if (string.IsNullOrEmpty(cptModel.TemplateFile))
                  { %>
                    <input type="file" name="PrintTemplate" />
                <%}
                  else
                  {
                 %>
                 <a target="_blank" href="<%=cptModel.TemplateFile%>">查看打印模板</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" name="PrintTemplate" />
                 <%} %>
                </td> 
              </tr>
              <tr class="odd">
                <td height="25">&nbsp;</td>
                <th height="35" align="right">打印公章：</th>
                <td>
                <%if (string.IsNullOrEmpty(cptModel.CustomerStamp))
                  { %>
                    <input type="file" name="PrintCachet" />
                <%}
                  else
                  {
                 %>
                 <a target="_blank" href="<%=cptModel.CustomerStamp%>">查看打印公章</a><img style="cursor:pointer" src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" name="PrintCachet" />
                 <%} %>
                </td> 
              </tr>
              <tr>
                <td height="1" colspan="3" bgcolor="#93DAF0"></td>
              </tr>
         </table>
		<ul class="tjbtn">
            <li><a href="Javascript:;" id="btn_Save">保存</a></li>
            <li><a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">关闭</a></li>
            <div class="clearboth"></div>
          </ul>
    </div>
    <script type="text/javascript">
        $(function() {
            $("#btn_Save").click(function() {
                var form = $(this).closest("form").get(0);
                form.submit();
                return false;
            });
            $("img.close").click(function(){
                var that = $(this);
                var name = that.attr("name");
                that.parent().html("<input name=\""+name+"\" type=\"file\" />");
            });
        });
    </script>
    </form>    
</body>
</html>
