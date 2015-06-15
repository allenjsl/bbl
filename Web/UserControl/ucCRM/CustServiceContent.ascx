<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustServiceContent.ascx.cs" Inherits="Web.UserControl.ucCRM.CustServiceContent" %>
  <%@ Register Src="~/UserControl/xianluWindow.ascx"  TagName="routeTag" TagPrefix="uc1" %>
   <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
      <tr class="odd">
        <th width="90" height="30" align="center">线路名称：</th>
        <td width="250" align="center">
            <uc1:routeTag id="routeTag1" runat="server"  publishType="3" IsClickTextBoxPopUpWindow=true />
        </td>
        <th width="80" align="center">出团日期：</th>
        <td width="180" align="center">
        <input type="text" id="txtLeaveDate" runat="server" class="searchinput" onfocus="WdatePicker()" /></td>
      </tr>
      <tr class="even">
        <th width="90" height="30" align="center">行程安排：</th>
        <td align="center">
        <asp:RadioButtonList ID="rdiTravel" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          </td>
        <th align="center">餐饮质量：</th>
        <td align="center">
       <asp:RadioButtonList ID="rdiFoodLevel" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1"  Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          </td>
      </tr>
      <tr class="odd">
        <th width="90" height="30" align="center">酒店环境：</th>
        <td align="center">
        <asp:RadioButtonList ID="rdiHotelCondition" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList></td>
        <th align="center">景点安排：</th>
        <td align="center">
        <asp:RadioButtonList ID="rdiLandscape" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          
          </td>
      </tr>
      <tr class="even">
        <th width="90" height="30" align="center">导游服务：</th>
        <td align="center">
       <asp:RadioButtonList ID="rdiGuide" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          
          </td>
        <th align="center">购物安排：</th>
        <td align="center">
       <asp:RadioButtonList ID="rdiShopping" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          
          </td>
      </tr>
      <tr class="odd">
        <th width="90" height="30" align="center">车辆安排：</th>
        <td align="center">
         <asp:RadioButtonList ID="rdiCar" runat="server" RepeatDirection ="Horizontal">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          
          </td>
        <th align="center"></th>
        <td align="center">
         <asp:RadioButtonList ID="rdiBrowsDate" runat="server" RepeatDirection ="Horizontal"  Visible="false">
          <asp:ListItem Text="满意" Value="1" Selected="True"></asp:ListItem>
          <asp:ListItem Text="一般" Value="2"></asp:ListItem>
          <asp:ListItem Text="不满意" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
          </td>
      </tr>
      <tr class="even">
        <th height="40" align="center"><%=remarkTitle%>：</th>
        <td><textarea class="textareastyle02" rows="5" cols="45"  id="txtRemark" runat="server"></textarea></td>
     </tr>
   
    </table>