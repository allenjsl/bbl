<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicInfoHeaderMenu.ascx.cs" Inherits="Web.UserControl.ucSystemSet.BasicInfoHeaderMenu" %>
 <div class="lineprotitlebox">
   <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;"> 所在位置：系统设置>> 基础设置</td>
      </tr>
      <tr>
        <td colspan="2" height="2" bgcolor="#000000"></td>
      </tr>
  </table>  
</div>
  <div class="lineCategorybox" style="height:50px;">
    <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
      <tr>
      <% if(backPage.CheckGrant(Common.Enum.TravelPermission.系统设置_基础设置_城市管理栏目)){%>
        <td width="100" align="center" id="basicTab1"><a href="/systemset/basicinfo/CityManage.aspx">城市管理</a></td><%}
        if(backPage.CheckGrant(Common.Enum.TravelPermission.系统设置_基础设置_线路区域栏目)){                                                                                                     %>
        <td width="100" align="center" id="basicTab2"><a href="/systemset/basicinfo/RouteArea.aspx">线路区域</a></td><%}
        if (backPage.CheckGrant(Common.Enum.TravelPermission.系统设置_基础设置_报价标准栏目))
        { %>
        <td width="100" align="center" id="basicTab3"><a href="/systemset/basicinfo/PriceStand.aspx">报价标准</a></td><%}

       if (backPage.CheckGrant(Common.Enum.TravelPermission.系统设置_基础设置_客户等级栏目))
        { %>
        <td width="100" align="center" id="basicTab4"><a href="/systemset/basicinfo/CustomerLevel.aspx">客户等级</a></td><%}
        if (backPage.CheckGrant(Common.Enum.TravelPermission.系统设置_基础设置_品牌管理栏目))
        {%>
        <td width="100" align="center" id="basicTab5"><a href="/systemset/basicinfo/BrandManage.aspx">品牌管理</a></td><%} %>
      </tr>
    </table>
</div>
<script type="text/javascript">
    $(document).ready(function() {
        $("#basicTab<%=TabIndex %>").addClass("xtnav-on");
    });
</script>