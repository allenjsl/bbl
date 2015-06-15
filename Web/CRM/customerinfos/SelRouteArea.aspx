<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelRouteArea.aspx.cs" Inherits="Web.CRM.customerinfos.SelRouteArea" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
      <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
       .oddTr td { background-color:#e3f1fc}
       .evenTr td{ background-color:#BDDCF4;}
      </style>

    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
<form runat="server" id="departFrom">
   <table width="500" border="0"  align="center" cellpadding="0" cellspacing="1" style="margin:5px auto; ">
    <tr class="odd">
    <td colspan="5" align="left">&nbsp;&nbsp;<input type="checkbox"id="chkAll1" onclick="selAll(this);"/> 全选</td>
    </tr>
        <asp:CustomRepeater runat="server" ID="rptArea">
                    <ItemTemplate>
                    <%# GetListTr()%>
                   <td width="100" height="28" align="center" ><input type="checkbox" class='c1' name="txtDepartInfo" value='<%# Eval("Id") %>' <%# GetAreaChecked((int)Eval("Id"))%> />
                   <span><%# Eval("AreaName") %></span> </td>
                    </ItemTemplate>
                 </asp:CustomRepeater>
                   <%=GetLastTr() %>
	 <tr class="odd">
        <td height="30" colspan="17" align="center" bgcolor="#BDDCF4">
		<table width="340" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="40" align="center" class="jixusave"><a href="javascript:;" onclick="return selectDepart();">选择区域</a></td>
          </tr>
        </table></td>
      </tr>
</table>
   </form>
   <script type="text/javascript">
      //选择部门写入父窗口的隐藏域
       function selectDepart() {
           var selAreas = [];
           var selAreasName = [];
           $(".c1:checked").each(function() {
               selAreas.push($(this).val());
               selAreasName.push($(this).next().html());
           });
           window.parent.Boxy.getIframeWindow('<%=EyouSoft.Common.Utils.GetQueryStringValue("desid") %>').CustEdit.selAreaBack(selAreas.toString(), selAreasName.toString());
           window.parent.Boxy.getIframeDialog('<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>').hide();
           return false;
       }
       //全选
       function selAll(tar) {
          var tarObj=$(tar);
          $("input:checkbox").not(tarObj).attr("checked", tarObj.attr("checked"));
       }
   </script>
   
</body>

</html>





   
