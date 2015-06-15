<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectDepartment.aspx.cs" Inherits="Web.Common.SelectDepartment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>行政中心-选择部门</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    
    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" style=" margin:0; padding:0;">
    <div id="Data" width="100%">
       <table width="750" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
        <tr class="odd">
            <th colspan="5" bgcolor="#BDDCF4">请选择部门</th>
        </tr>
        <%if (deplist != null)
          {int temp = 0;
          foreach (EyouSoft.Model.CompanyStructure.Department dep in deplist)
          {%>
        <%if (temp % 5 == 0)
          { %> <tr class="even"><%} temp++; %>
        <td width="20%" height="28" bgcolor="#E3F1FC" class="pandl3">
            <input type="checkbox" class="depcheck" value="<%= dep.Id %>" /><label><%= dep.DepartName%></label> 
        </td>
        <%if (temp % 5 == 0)
          { %></tr> <%} %>
        <% 
          }
          } %> 
      
        <tr class="odd">
            <td height="30" align="center" bgcolor="#BDDCF4" colspan="5">
	        <table width="340" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td height="40" align="center" class="jixusave"><a href="javascript:;" onclick=""  id="depsel">确定</a></td>
              </tr>
            </table>  
            </td>
        </tr>
        </table>
    </div>

    
    <script type="text/javascript">
        $(function() {

            $("#depsel").bind("click", function() {
                var topid = '<%=Request.QueryString["topid"] %>';
                var hfid = '<%=Request.QueryString["hfid"] %>';
                var txtputid = '<%=Request.QueryString["txtinputid"] %>';
                var txtid = '<%=Request.QueryString["txtid"] %>';
                var ismuch = '<%=Request.QueryString["ismuch"] %>';
                var name = [];
                var id = [];
                $("input.depcheck").each(function() {
                    if ($(this).attr("checked")) {
                        name.push($.trim($(this).next("label").text()));
                        id.push($(this).val());
                    }
                });
                if (ismuch) {
                    if (id.length > parseInt(ismuch)) {
                        alert("选择部门不能大于" + ismuch + "个");
                        return false;
                    }
                }
                if (!topid) {
                    window.parent.$('#' + hfid).val(id.join(','));
                    window.parent.$('#' + txtputid).val(name.join(','));
                    window.parent.$('#' + txtid).text("(" + name.join(',') + ")");
                    window.parent.Boxy.getIframeDialog('#<%=Request.QueryString["iframeId"] %>').hide();
                } else {
                    var doc = window.parent.Boxy.getIframeDocument(topid);
                    $(doc.getElementById(hfid)).val(id.join(','));
                    $(doc.getElementById(txtputid)).val(name.join(','));
                    $(doc.getElementById(txtid)).text("(" + name.join(',') + ")");
                    window.parent.Boxy.getIframeDialog('#<%=Request.QueryString["iframeId"] %>').hide();
                }
                return false;
            });

            var oldid = '<%=Request.QueryString["id"] %>'.split(",");
            if (oldid) {
                for (var i in oldid) {
                    $("input.depcheck[value=" + oldid[i] + "]").attr("checked", "checked");
                }
            }
        });
    </script>
    </form>
</body>
</html>
