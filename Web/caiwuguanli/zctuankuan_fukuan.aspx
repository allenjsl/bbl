<%@ Page Language="C#" Title="应付账款" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="zctuankuan_fukuan.aspx.cs" Inherits="Web.caiwuguanli.zctuankuan_fukuan" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    
         <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
           
    <script type="text/javascript">
        $(function() {
            var sum = 0;
            $(".yfmoney").each(function() {
                sum += parseFloat($(this).html())
            });
            $("#<%=txt_PaidMoney.ClientID %>").val(sum);
            $(".dengji").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });

            $("#btn2").click(function() {
                new Boxy("&lt;div style='width:400px;height:200px;background-color:gray'&gt;自定义HTML显示&lt;/div&gt;", { title: "自定义HTML" });
            });
        });
</script>
			<div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody><tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">财务管理</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">所在位置&gt;&gt;财务管理 &gt;&gt; 团款支出</td>
            </tr>
            <tr>
              <td height="2" bgcolor="#000000" colspan="2"></td>
            </tr>
          </tbody></table>
        </div>
        <div class="hr_10"></div>   
        <ul class="fbTab">
          <li><a class="tabtwo-on" id="two1" href="TeamExpenditure.aspx">应付账款</a></li>
          <li><a  id="two2" href="waitkuan.aspx" class="">付款审批</a></li>
          <li><a  id="two3" href="teamPayClear.aspx" class="">已结清账款</a></li>
          <div class="clearboth"></div>
        </ul>
        <div class="hr_10"></div>
        <div id="con_two_1">
       	  <table width="99%" cellspacing="1" cellpadding="0" border="0" align="center">
              <tbody><tr class="odd">
                <th width="10%" height="30" align="center">团号： </th>
                <td width="40%"><input type="text" id="txt_teamNum" runat="server" class="searchinput" name="txt_teamNum"></td>
                <th width="10%" align="center">线路名称：</th>
                <td width="40%"><input type="text" class="searchinput searchinput02" runat="server" id="txt_teamName" name="txt_teamName"></td>
            </tr>
            <tr class="even">
              <th height="30" align="center">销售员：</th>
                <td width="40%">
                    <asp:Literal ID="lt_seller" runat="server"></asp:Literal>
                </td>
                <th align="center">计调员：</th>
                <td width="40%">
                    <asp:Literal ID="lt_actor" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="odd">
              <th height="30" align="center">团队状态：</th>
                <td width="40%"><input type="text" id="txt_teamStatus" runat="server" class="searchinput" name="txt_teamStatus"></td>
                <th align="center">团队总成本<strong>：</strong></th>
                <td width="40%"><input type="text" id="txt_teamTraffic" runat="server" class="searchinput" name="txt_teamTraffic"></td>
            </tr>
            <tr class="even">
              <th height="30" align="center">已付金额：</th>
                <td width="40%">
                  <input type="text" id="txt_PaidMoney" runat="server" class="searchinput" name="txt_PaidMoney"></td>
                <th align="center">&nbsp;</th>
                <td width="40%">&nbsp;</td>
            </tr>
            <tr>
              <th height="30" align="left" colspan="4"><span class="fukuanT">票务:</span></th>
            </tr>
            <tr>
              <td height="30" align="left" colspan="4"><table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
                <tbody><tr class="odd">
                  <th width="20%" height="25" align="center">单位名称</th>
                  <th width="20%" align="center">应付金额</th>
                  <th width="20%" align="center">已付金额</th>
                  <th width="20%" align="center">未付金额</th>
                  <th width="20%" align="center">操作</th>
                </tr>
                <asp:Repeater runat="server" ID="rpt_piaowu">
                <ItemTemplate>
                <tr class="even">
                  <td height="25" align="center"><%#Eval("SuplierName")%></td>
                  <td align="center"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %></font></td>
                  <td align="center"><font class="fred">￥<span class="yfmoney"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("PayedAmount").ToString())%></span></font></td>
                  <td align="center"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalAmount").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("PayedAmount").ToString()))%></font></td>
                  <td align="center"><a class="dengji" href="dengji.aspx?id=<%#Eval("Id") %>&comId=<%#Eval("SupplierId") %>&tourid=<%=Request.QueryString["tourId"] %>&type=<%#(int)(Eval("SupplierType")) %>&money=<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %>&com=<%#Uri.EscapeUriString( Eval("SuplierName").ToString())%>"><font class="fblue"><%# EyouSoft.Common.Utils.GetDecimal(Eval("TotalAmount").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("PayedAmount").ToString())!=0?"登记":""%></font></a></td>
                </tr>
                </ItemTemplate>                
                </asp:Repeater>
              </tbody></table></td>
            </tr>       
            <tr>
              <th height="30" align="left" colspan="4"><span class="fukuanT">地接:</span></th>
            </tr>
            <tr>
              <td height="30" align="left" colspan="4"><table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
                <tbody><tr class="odd">
                  <th width="20%" height="25" align="center">单位名称</th>
                  <th width="20%" align="center">应付金额</th>
                  <th width="20%" align="center">已付金额</th>
                  <th width="20%" align="center">未付金额</th>
                  <th width="20%" align="center">操作</th>
                </tr>
                <asp:Repeater runat="server"  ID="rpt_dijie">
                <ItemTemplate>
                <tr class="even">
                  <td height="25" align="center"><%#Eval("SuplierName")%></td>
                  <td align="center"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %></font></td>
                  <td align="center"><font class="fred">￥<span class="yfmoney"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("PayedAmount").ToString())%></span></font></td>
                  <td align="center"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalAmount").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("PayedAmount").ToString()))%></font></td>
                  <td align="center"><a class="dengji" href="dengji.aspx?id=<%#Eval("Id") %>&comId=<%#Eval("SupplierId") %>&tourid=<%=Request.QueryString["tourId"] %>&type=<%#getType((int)Eval("SupplierType")) %>&money=<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %>&com=<%#Uri.EscapeUriString( Eval("SuplierName").ToString())%>"><font class="fblue"><%# EyouSoft.Common.Utils.GetDecimal(Eval("TotalAmount").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("PayedAmount").ToString())!=0?"登记":""%></font></a></td>
                </tr>
                </ItemTemplate>
                </asp:Repeater>
              </tbody></table></td>
            </tr>           
            </tbody></table>
        </div>
        
      </div>
			<!-- InstanceEndEditable -->            </div>
    </form>
</asp:Content>

