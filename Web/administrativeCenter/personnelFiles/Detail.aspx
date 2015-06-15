<%@ Page Language="C#" MasterPageFile="/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs"   ValidateRequest="false" Inherits="Web.administrativeCenter.personnelFiles.Detail" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">

    
        <table width="695" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000"
            style="border-collapse:collapse; margin:5px auto; ">
           <tr>
            <td height="35" colspan="4" align="center"><h2><strong>
                <asp:Label ID="lblPersonnelTitle" runat="server"></asp:Label></strong></h2></td>
          </tr>
          <tr>
            <th colspan="4" align="center" bgcolor="#eeeeee">==========基本信息==========</th>
          </tr>
          <tr>
            <td width="222" class="pandl3" align="left">档案编号：<span id="Span1">
                <asp:Label ID="lblFileNo" runat="server" Text=""></asp:Label></span></td>
            <td width="183" class="pandl3" align="left">姓 名：<asp:Label ID="lblName" runat="server" Text=""></asp:Label></td>
            <td width="183" class="pandl3" align="left">性 别：<asp:Label ID="lblSex" runat="server" Text=""></asp:Label></td>
            <td width="97" rowspan="5" align="center">
                <asp:Image ID="imgPersonnelPhoto" runat="server" Width="85" Height="100" /></td>
          </tr>
          <tr>
            <td class="pandl3" align="left">身份证号：<span id="Span2">
                <asp:Label ID="lblCardID" runat="server" Text=""></asp:Label></span></td>
            <td class="pandl3" align="left">出生日期：<asp:Label ID="lblBirthday" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">所属部门：<span id="Span3">
                <asp:Label ID="lblDepartment" runat="server" Text=""></asp:Label></span></td>
          </tr>
          <tr>
            <td class="pandl3" align="left">职    务：<span id="Span4">
                <asp:Label ID="lblPosition" runat="server" Text=""></asp:Label></span></td>
            <td class="pandl3" align="left">类    型：<span id="Span5">
                <asp:Label ID="lblType" runat="server" Text=""></asp:Label> </span></td>
            <td class="pandl3" align="left">员工状态：<span id="Span6">
                <asp:Label ID="lblState" runat="server" Text=""></asp:Label></span></td>
          </tr>
          <tr>
            <td class="pandl3" align="left">入职时间：<asp:Label ID="lblEntryDate" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">工    龄：<asp:Label ID="lblWorkerLife" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">民    族：<asp:Label ID="lblNational" runat="server" Text=""></asp:Label></td>
          </tr>
          <tr>
            <td class="pandl3" align="left">籍    贯：<label ID="lblBirthplace"><%=Birthplace %></label></td>
            <td class="pandl3" align="left">政治面貌：<asp:Label ID="lblPolitical" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">婚姻状态：<span id="Span7">
                <asp:Label ID="lblMarriageState" runat="server" Text=""></asp:Label> </span></td>
          </tr>
          <tr>
            <td class="pandl3" align="left">联系电话：<asp:Label ID="lblTel" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">手    机：<asp:Label ID="lblMobile" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">离职时间：<asp:Label ID="lblLeftDate" runat="server" Text=""></asp:Label></td>
            <td class="pandl3">&nbsp;</td>
          </tr>
          <tr>
            <td class="pandl3" align="left">QQ：<asp:Label ID="lblQQ" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">MSN：<asp:Label ID="lblMSN" runat="server" Text=""></asp:Label></td>
            <td class="pandl3" align="left">E-mail：<asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
            <td class="pandl3">&nbsp;</td>
          </tr>
          <tr>
            <td colspan="4" class="pandl3" align="left">住    址：<asp:Label ID="lblAddress" runat="server" Text=""></asp:Label></td>
          </tr>
          <tr>
            <td colspan="4" class="pandl3" align="left">备    注：<asp:Label ID="lblRemark" runat="server" Text=""></asp:Label></td>
          </tr>
    </table>
   
    <asp:Repeater ID="rptEducationInfo" runat="server">
        <HeaderTemplate>
        <table width="695" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" 
        style="border-collapse:collapse; margin:5px auto;">
          <tr>
            <th colspan="7" align="center" bgcolor="#eeeeee">==========学历信息==========</th>
          </tr>
          <tr>
            <td width="74" align="center">开始时间</td>
            <td width="74" align="center">结束时间</td>
            <td width="50" align="center">学历</td>
            <td width="105" align="center">所学专业</td>
            <td width="113" align="center">毕业院校</td>
            <td width="52" align="center">状态</td>
            <td width="211" align="center">备注</td>
          </tr>
        </HeaderTemplate>
        <ItemTemplate>
          <tr>
            <td align="center"><%# string.Format("{0:yyyy-MM-dd}", Eval("StartDate"))%></td>
            <td align="center"><%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%></td>
            <td align="center"><%# Eval("Degree") %></td>
            <td align="center"><%# Eval("Professional")%></td>
            <td align="center"><%# Eval("SchoolName")%></td>
            <td align="center"><%# Eval("StudyStatus").ToString()=="true"?"毕业":"在读" %></td>
            <td align="center"><%# Eval("Remark")%></td>
          </tr>
        </ItemTemplate>
        <FooterTemplate>
           </table>
        </FooterTemplate>
    </asp:Repeater>
        
    
        
     <asp:Repeater ID="rptRecord" runat="server">
         <HeaderTemplate>
         <table width="695" border="1" align="center" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse; margin:5px auto;">
          <tr>
            <th colspan="6" align="center" bgcolor="#eeeeee">==========履历信息==========</th>
          </tr>
          <tr>
            <td width="74" align="center">开始时间</td>
            <td width="74" align="center">结束时间</td>
            <td width="80" align="center">工作地点</td>
            <td width="139" align="center">工作单位</td>
            <td width="105" align="center">从事职业</td>
            <td width="209" align="center">备注</td>
          </tr>
         </HeaderTemplate>
        <ItemTemplate>
          <tr>
            <td align="center"><%# string.Format("{0:yyyy-MM-dd}", Eval("StartDate"))%></td>
            <td align="center"><%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%></td>
            <td align="center"><%# Eval("WorkPlace")%></td>
            <td align="center"><%# Eval("WorkUnit")%></td>
            <td align="center"><%# Eval("TakeUp")%></td>
            <td align="center"><%# Eval("Remark")%></td>
          </tr>
        </ItemTemplate>
        <FooterTemplate>
          </table>  
        </FooterTemplate>
      </asp:Repeater>
</asp:Content>
