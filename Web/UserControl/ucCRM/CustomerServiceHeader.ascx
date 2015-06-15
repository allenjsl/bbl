<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerServiceHeader.ascx.cs" Inherits="Web.UserControl.ucCRM.CustomerServiceHeader" %>
   <div class="lineprotitlebox">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">客户关系管理</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：客户关系管理 >> 客户服务 >> <%=UseMap%></td>
            </tr>
            <tr>
              <td colspan="2" height="2" bgcolor="#000000"></td>
            </tr>
          </table>
        </div>
        <div class="lineCategorybox" style="height:40px;">
    	<table border="0" cellpadding="0" cellspacing="0" class="xtnav">
          <tr>
            <%if (HasMarket)
              { %>
            <td width="100" align="center" id="crmTab1"><a href="/CRM/customerservice/MarketActive.aspx">营销活动</a></td><%} if (HasCare)
              { %>
            <td width="100" align="center" id="crmTab2"><a href="/CRM/customerservice/CustomerCare.aspx">客户关怀</a></td><%} if (HasComplaint || HasVisist)
              { %>
            <td width="100" align="center" id="crmTab3"><a href="/CRM/customerservice/<%=HasVisist?"CustomerVisit.aspx":"CustomerComplaint.aspx"%>">质量管理</a></td><%} %>
          </tr>
        </table>
       </div>
       <script type= "text/javascript">
     
         $(document).ready(function() {
          $("#crmTab<%=TabIndex %>").addClass("xtnav-on");
       });
         </script>