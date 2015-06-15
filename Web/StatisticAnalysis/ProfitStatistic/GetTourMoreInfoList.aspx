<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetTourMoreInfoList.aspx.cs" Inherits="Web.StatisticAnalysis.ProfitStatistic.GetTourMoreInfoList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style>
    .rpt_td{ border-collapse:collapse; border-right:none;}
.rpt_td td {line-height:22px; height:30px; overflow:hidden; border-right:1px solid #fff;border-bottom:1px solid #fff;word-break:break-all; vertical-align:middle;}
</style>
    <script src="/js/jquery.js" type="text/javascript"></script>
<script>
    $(function() {
        $(".rpt_td").each(function() {
            $(this).height($(this).closest("td").height());
        });

        $(".rpt_td td").each(function() {
            $(this).css("line-height", "30px");
        });
    });
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" vertical-align:middle" align="center">
  <table width="1600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
  <tr  class="odd">
    <th width="168" bgcolor="#BDDCF4">线路</th>
    <th width="46" height="30" bgcolor="#BDDCF4">团号</th>
    <th width="55" bgcolor="#BDDCF4">联字号</th>
    <%--<th width="61" bgcolor="#BDDCF4">人数</th>--%>
    <th width="156" bgcolor="#BDDCF4">组团社</th>
    <th width="61" bgcolor="#BDDCF4">人数</th>
    <th width="72" bgcolor="#BDDCF4">应收费用</th>
    <th width="86" bgcolor="#BDDCF4">已收费用</th>
    <th width="73" bgcolor="#BDDCF4">未收费用</th>
    <th width="119" bgcolor="#BDDCF4">地接社</th>
    <th width="60" bgcolor="#BDDCF4">人数</th>
    <th width="76" bgcolor="#BDDCF4">应付地接款</th>
    <th width="76" bgcolor="#BDDCF4">已付地接款</th>
    <th width="76" bgcolor="#BDDCF4">未付地接款</th>
    <th width="60" bgcolor="#BDDCF4">交通</th>
    <th width="60" bgcolor="#BDDCF4">人数</th>
    <th width="63" bgcolor="#BDDCF4">应付交通</th>
    <th width="69" bgcolor="#BDDCF4">已付交通款</th>
    <th width="67" bgcolor="#BDDCF4">未付交通款</th>
    <th width="86" bgcolor="#BDDCF4">报销</th>
    <th width="70" bgcolor="#BDDCF4">毛利</th>
  </tr>

            <cc1:CustomRepeater ID="crp_GetTourDetailList" runat="server" 
          onitemdatabound="crp_GetTourDetailList_ItemDataBound">
                <ItemTemplate>
                      <tr class="even">
                        <td align="center"><%#Eval("PlanSingles")!=null?"单项服务":Eval("RouteName")%></td>
                        <td height="30" align="center"><%#Eval("TourCode")%></td>
                        <td align="center"><%#Eval("PKID")%></td>
                        <%--<td align="center"><%#Eval("RealityPeopleNumber")%></td>--%>
                        <td align="left" colspan="5">
                                            
                        <cc1:CustomRepeater runat="server" ID="rpt_list1">
                            <HeaderTemplate>                            
                            <table class="rpt_td" width="100%">
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr>
                            <td align="center" style="width:148px;_width:152px*width:150px;"><font class="fbred"><%#Eval("BuyCompanyName")%></font></td>
                            <td align="center" style="width:56px"><%#Eval("PeopleNumber")%></td>
                            <td align="center" style="width:68px;*width:70px;"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%></font></td>
                            <td align="center" style="width:78px"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("ReceivedAmount").ToString())%></font></td>
                            <td align="center" style="width:70px"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("UnReceivedAmount").ToString())%></font></td>
                            </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </table></FooterTemplate>
                            </cc1:CustomRepeater>
                        </td>
                        <td align="center" colspan="5">                        
                        <cc1:CustomRepeater runat="server" ID="rpt_list2">
                            <HeaderTemplate>                            
                            <table class="rpt_td" width="100%">
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr>
                            <td align="center" style="width:128px;*width:124px;"><%#Eval("SupplierName")%></td>
                            <td align="center" style="width:62px;"><%#Eval("PeopleNumber")%></td>
                        <td align="center" style="width:80px;"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%></td>
                        <td align="center" style="width:80px;"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("PaidAmount").ToString())%></font></td>
                        <td align="center" style="width:80px;"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("UnPaidAmount").ToString())%></td></tr>
                        </ItemTemplate>
                            <FooterTemplate>
                            </table></FooterTemplate>
                        </cc1:CustomRepeater>
                        
                        </td>
                        <td align="center" colspan="5">   
                                                                    
                        <cc1:CustomRepeater runat="server" ID="rpt_list3">
                            <HeaderTemplate>                            
                            <table class="rpt_td" width="100%">
                            </HeaderTemplate>
                            <ItemTemplate>
                            <tr>
                        <td align="center" style="width:58px;*width:58px;"><%#Eval("SupplierName")%></td>
                        <td style="width:58px;*width:58px;"><%#Eval("PeopleNumber")%></td>
                        <td align="center" style="width:63px;*width:62px;"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%></font></td>
                        <td align="center" style="width:66px;*width:64px;"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("PaidAmount").ToString())%></font></td>
                        <td align="center" style="width:63px;*width:64px;"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("UnPaidAmount").ToString())%></font></td>
                        </tr>
                            </ItemTemplate>
                            <FooterTemplate></table></FooterTemplate> 
                            </cc1:CustomRepeater>
                        </td>
                        <td align="center"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("ReimburseAmount").ToString())%></td>
                        <td align="center"><font class="fbred"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("GrossProfit").ToString())%></font></td>
                       
                       </tr>
                </ItemTemplate>
            </cc1:CustomRepeater>
            <tr id="tbl_ExportPage" runat="server">           
                <td height="30" colspan="8" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
