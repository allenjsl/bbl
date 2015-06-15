<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Sale_List.aspx.cs" Inherits="Web.sales.List" Title="销售管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">销售管理</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                所在位置&gt;&gt; 销售管理&gt;&gt; 销售收款
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    出团日期：</label><input type="text" onfocus="WdatePicker()" id="txt_Date" class="searchinput"
                                        runat="server" name="txt_Date" />
                                至
                                <input type="text" onfocus="WdatePicker()" id="txt_Date1" class="searchinput" runat="server"
                                    name="txt_Date" />
                                <label>
                                    订单号：</label><input type="text" id="inputOrderLot" class="searchinput" name="inputOrderLot" />
                                <uc1:selectOperator ID="selectOrderOperator" runat="server" TextClass="searchinput"
                                    Title="订单操作人：" />
                                <label>
                                    团号：</label><input type="text" id="inputTuanHao" class="searchinput" name="inputTuanHao" />
                                <label>
                                    客户单位：</label><input type="text" id="inputCus" class="searchinput" name="inputCus" /><br>
                                <label>
                                    线路名称：</label><input type="text" class="searchinput searchinput02" id="inputLineName"
                                        name="textfield4" />
                                <uc1:selectOperator ID="selectOperator1" runat="server" TextClass="searchinput" Title="销售员：" />
                                是否结清：
                                <select id="selSettle" name="selSettle">
                                    <option value="-1" selected="selected">请选择</option>
                                    <option value="1">已结清</option>
                                    <option value="0">未结清</option>
                                </select>
                                <label>
                                    <a href="javascript:void(0)" id="btnSearch">
                                        <img style="vertical-align: top;" src="../images/searchbtn.gif"></a></label></div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left" width="65%">
                    <tbody>
                        <tr>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="salesShow">查 看</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="salesShouKuai">收 款</a>
                            </td>
                            <td align="center" width="90">
                                <a href="javascript:void(0)" id="salesTuiKuai">退 款</a>
                            </td>
                            <td align="center" width="90">
                                <asp:HyperLink ID="hypSale" runat="server" Target="_blank">打印收据</asp:HyperLink>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th height="30" bgcolor="#bddcf4" align="center" width="3%">
                                &nbsp;
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                团号
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                出团日期
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="14%">
                                线路名称
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                销售员
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                订单号
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                合同金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="4%">
                                人数
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="12%">
                                客户单位
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                已收
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                已收待审核
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                未收
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                操作
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                查看明细
                            </th>
                        </tr>
                        <cc1:CustomRepeater ID="repList" runat="server" OnItemDataBound="repList_ItemDataBound">
                            <ItemTemplate>
                                <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <td height="30" align="center">
                                        <input type="checkbox" id="<%#Eval("Id") %>&tourid=<%#Eval("tourid") %>" name="checkbox">
                                    </td>
                                    <td align="center">
                                        <%#Eval("TourNo")%>
                                    </td>
                                    <td align="center">
                                        <%# Eval("LeaveDate","{0:yyyy-MM-dd}")%>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <%#Eval("SalerName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("OrderNo")%>
                                    </td>
                                    <td align="center">
                                        <%#(Eval("FinanceSum","{0:c2}"))%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("PeopleNumber")%>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0);" class="openByerInfo" ref="<%#Eval("BuyCompanyID") %>">
                                            <%#Eval("BuyCompanyName")%></a>
                                    </td>
                                    <td align="center">
                                        <%# Eval("HasCheckMoney","{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("NotCheckMoney","{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("NotReceived","{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%if (CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_收款登记))
                                          {
                                        %>
                                        <a onclick="SaleList.GoOper('<%#Eval("Id")%>&tourId=<%#Eval("tourId") %>','shouKuai')"
                                            href="javascript:void(0)">收款</a>
                                        <%
                                            }%>
                                        <%if (CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_退款登记))
                                          {
                                        %>
                                        <a onclick="SaleList.GoOper('<%#Eval("Id") %>&tourId=<%#Eval("tourId") %>','tuiKuai')"
                                            href="javascript:void(0)">退款</a>
                                        <%
                                            }%>
                                    </td>
                                    <td align="center">
                                        <a onclick="SaleList.GoOper('<%#Eval("Id") %>','show')" href="javascript:void(0)">查看</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </cc1:CustomRepeater>
                        <tr bgcolor="#bddcf4">
                            <th align="center" colspan="2">
                                总计:
                            </th>
                            <td align="center" colspan="4">
                            </td>
                            <td align="center">
                                <asp:Literal ID="LitFinanceSum" runat="server" Text="0"></asp:Literal>
                            </td>
                            <td align="center">
                                <asp:Literal ID="LitPeopleCount" runat="server"></asp:Literal>
                            </td>
                            <td align="center">
                            </td>
                            <td align="center">
                                <asp:Literal ID="LitHasCheckMoney" runat="server" Text="0"></asp:Literal>
                            </td>
                            <td align="center">
                                <asp:Literal ID="LitNotCheckMoney" runat="server" Text="0"></asp:Literal>
                            </td>
                            <td align="center">
                                <asp:Literal ID="LitNotReceived" runat="server" Text="0"></asp:Literal>
                            </td>
                            <th align="center" colspan="2">
                            </th>
                        </tr>
                    </tbody>
                </table>
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="right">
                                <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                                    runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
     var SaleList={
     
            GetSelectCount: function() {
                var count = 0;
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")==true) {
                        count++;
                    }
                });
                return count;
            },
            
            GetSelectItemValue: function() {
                var arrayList = new Array();
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")==true) {
                        arrayList.push($(this).attr("id"));
                    }
                });
                return arrayList;
            },
            
           GoOper:function(id,sign){
              switch(sign){
               case "show":
                 this.openXLwindow('Sale_show.aspx?OrderID='+id,'查看','900px','550px');
                 break;
               case "tuiKuai": 
                 this.openXLwindow('Sale_tuikuan.aspx?OrderID='+id,'退款','820px','300px');
                 break;
               case "shouKuai":  
                 this.openXLwindow('Sale_shoukuan.aspx?OrderID='+id,'收款','820px','300px');
                 break;
                 default:
                 return false;
              }
            },
            
         openXLwindow:function(url,title,width,height){
            url=url;
        	Boxy.iframeDialog({
			iframeUrl:url,
			title:title,
			modal:true,
			width:width,
			height:height
		});
		return false;
      }
    }
    
    $("#salesShow").click(function(){
      if(SaleList.GetSelectCount()>0){
        var list=SaleList.GetSelectItemValue();
       if(list[0]!=undefined && list.length==1){
           SaleList.GoOper(list[0],'show');
        }
        else{
           alert("只能选择一条记录操作!");
           return false;

        }
      }
      else{
        alert("请选择要查看的记录");
        return false;
      }
    });
    
     $("#salesTuiKuai").click(function(){
      if(SaleList.GetSelectCount()>0){
        var list=SaleList.GetSelectItemValue();
        if(list[0]!=undefined && list.length==1){
         SaleList.GoOper(list[0],'tuiKuai');
        }
        else{
           alert("只能选择一条记录操作!");
           return false;
        } 
      }
      else{
        alert("请选择要退款的记录");
        return false;
      }
    });
    
     $("#salesShouKuai").click(function(){
      if(SaleList.GetSelectCount()>0){
        var list=SaleList.GetSelectItemValue();
       if(list[0]!=undefined && list.length==1){
          SaleList.GoOper(list[0],'shouKuai');
        }
        else{
           alert("只能选择一条记录操作!");
           return false;
        } 
      } 
     else{
        alert("请选择要收款的记录");
        return false;
     }
    });
    
    $("#btnSearch").click(function(){
     var data={
       date: $("#<%=this.txt_Date.ClientID %>").val(),
       LDate: $("#<%=this.txt_Date1.ClientID %>").val(),
       orderOperId:<%=selectOrderOperator.ClientID%>.GetOperatorId(),
       orderOperName:<%=selectOrderOperator.ClientID%>.GetOperatorName(),
       orderLot:$("#inputOrderLot").val(),
       tuanHao:$("#inputTuanHao").val(),
       cusHao:$("#inputCus").val(),
       operID:<%=selectOperator1.ClientID%>.GetOperatorId(),
       operName:<%=selectOperator1.ClientID%>.GetOperatorName(),
       lineName:$("#inputLineName").val(),
       settle:$("#selSettle").val()
     };
     window.location.href="/sales/Sale_List.aspx?"+$.param(data);
     return false;
    });
    
      $("#<%=this.form1.ClientID%>").keydown(function(event) {
        if (event.keyCode == 13) {
           $("#btnSearch").click();
           return false;
          }
     }); 
     
     $(function(){
       if(queryString("orderLot")!="" && queryString("orderLot")!=null){ 
          $("#inputOrderLot").val(decodeURIComponent(queryString("orderLot")));
       }
       if(queryString("tuanHao")!="" && queryString("tuanHao")!=null){
         $("#inputTuanHao").val(decodeURIComponent(queryString("tuanHao")));
       }
       if(queryString("cusHao")!="" && queryString("cusHao")!=null){
         $("#inputCus").val(decodeURIComponent(queryString("cusHao")));
       }
        if(queryString("lineName")!="" && queryString("lineName")!=null){
         $("#inputLineName").val(decodeURIComponent(queryString("lineName")));
       }
       
       $("#selSettle").val(queryString("settle")==null?"":queryString("settle"));
        //打开客户单位信息页
            $(".openByerInfo").click(function() {
                var buyerId = $(this).attr("ref");
                Boxy.iframeDialog({ title: "客户单位信息", iframeUrl: "/CRM/customerinfos/CustomerDetails.aspx?cId=" + buyerId, width: "775px", height: "280px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            })
     });
    
    </script>

    </form>
</asp:Content>
