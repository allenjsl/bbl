<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuoteInfo.aspx.cs" Inherits="Web.xianlu.QuoteInfo" %>

<%@ Register Src="~/UserControl/PriceControl.ascx" TagName="PriceControl" TagPrefix="Uc1" %>
<%@ Register Src="~/UserControl/ProjectControl.ascx" TagName="ProjectControl1" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>报价详细信息</title>
    <link rel="Stylesheet" type="text/css" href="../css/sytle.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <table width="98%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
          <tbody>
             <tr class="odd">
                <th height="25" align="center" width="10%">询价单位：</th>
                <td align="left" width="15%">
                    <asp:Label ID="Txt_Inquiry" runat="server"></asp:Label>
                </td>
                <th align="center" width="10%">联系人：</th>
                <td  align="left" width="15%">
                   <asp:Label ID="Txt_Contact" runat="server"></asp:Label>
                </td>
                <th  align="center" width="10%">联系电话： </th>
                <td  align="left" width="15%">
                    <asp:Label ID="Txt_TelPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="even">
                <th height="25" align="center">预计出团时间：</th>
                <td align="left">
                   <asp:Label ID="Txt_GroupStarTime" runat="server"></asp:Label>
                </td>
                <th align="center">人数：</th>
                <td colspan="3" align="left">
                    <asp:Label ID="Txt_Numbers" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="odd">
                <th height="25" align="center" style=" width:10%">客人要求：</th>
                <th align="center" style=" width:15%">项目</th>
                <th colspan="4" align="center" style="width:75%">具体要求</th>
            </tr> 
           <asp:Repeater ID="rptList" runat="server">
              <ItemTemplate>
                  <tr class="even">
                      <th height="25" align="center" bgcolor="#E3F1FC"></th>       
                      <td align="center" style="width:15%"><%# (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), Eval("ServiceType").ToString())%></td>
                      <td colspan="4" align="center" style="width:75%"><%# Eval("Service")%></td>                                         
                  </tr>   
               </ItemTemplate>
             </asp:Repeater> 
             <tr class="odd">
                 <th height="25" align="center">价格组成:</th>
                        <td height="25" align="center" width="15%">项目</td>
                        <td align="center" width="20%">接待标准</td>
                        <td align="center" width="24%">地接报价</td>
                        <td align="center" width="24%" colspan="2">我社报价</td>
              </tr>
              <asp:Repeater ID="repPrices" runat="server">                             
                     <ItemTemplate>
                        <tr class="even">
                          <th height="25" align="center"></th>
                            <td height="25" align="center" width="15%"><%# (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), Eval("ServiceType").ToString())%></td>
                            <td align="center" width="20%"><%# Eval("Service")%></td>
                            <td align="center" width="24%"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("LocalPrice")).ToString("0.00"))%></td>
                            <td align="center" width="24%" colspan="2"><%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("SelfPrice")).ToString("0.00"))%></td>
                        </tr> 
                     </ItemTemplate>                                            
            </asp:Repeater>                          
            <tr class="even">
                <td height="25" align="center">
                    备注：
                </td>
                <td colspan="5" align="left">
                     <asp:Label ID="Txt_RemarksBottom" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th colspan="6" align="center">
                  <table width="320" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="40" align="center"></td>
                            <td height="40" align="center" class="tjbtn02">
                               <a href="javascript:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">
                                    关闭</a>
                            </td>
                        </tr>
                    </table>
                </th>
            </tr>
          </tbody>
        </table>
    </div>
    </form>
</body>
</html>
