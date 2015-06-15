<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="SanPing_JiPiaoAdd.aspx.cs" Inherits="Web.sanping.SanPing_JiPiaoAdd" %>

<%@ Import Namespace="System.Collections.Generic" %>
<%@ Register Src="../UserControl/jipiaoUpdate.ascx" TagName="jipiaoUpdate" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tb_blist
        {
            border-collapse: collapse;
            table-layout: fixed;
            word-wrap: break-word;
        }
        .tb_blist td, th
        {
            border: 1px solid white;
            padding: 0;
        }
        .tb_slist
        {
            margin: 0;
            padding: 0;
            height: 45px;
            table-layout: fixed;
            word-wrap: break-word;
        }
        .tb_slist td
        {
            border: none;
            border: solid #fff;
            border-width: 0 0 1px 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server" id="myform">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">
                                <%=lt_jihua.Text %></span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt;
                            <asp:Literal ID="lt_jihua" runat="server"></asp:Literal>&gt;&gt; 机票申请
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
        <div class="addlinebox">
            <fieldset style="border: 1px solid rgb(163, 211, 248); padding: 5px 0pt;">
                <legend>列表</legend>
                <table width="98%" cellspacing="0" cellpadding="0" border="0" align="center" class="tb_blist">
                    <tbody>
                        <tr class="odd">
                            <th width="7%" height="30">
                                日期
                            </th>
                            <th width="7%">
                                航段
                            </th>
                            <th width="7%">
                                航班号/时间
                            </th>
                            <th width="7%">
                                航空公司
                            </th>
                            <th width="7%">
                                折扣
                            </th>
                            <th width="7%">
                                PNR
                            </th>
                            <th width="7%">
                                票款
                            </th>
                            <th width="7%">
                                <%if (config_Agency.HasValue && config_Agency.Value == EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三)
                                  {%>
                                其它费用
                                <%}
                                  else
                                  { %>
                                代理费
                                <%} %>
                            </th>
                            <th width="7%">
                                人数
                            </th>
                            <th width="7%">
                                总费用
                            </th>
                            <th width="7%">
                                操作
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list1_ItemDataBound">
                            <ItemTemplate>
                                <tr class='<%#Container.ItemIndex%2==0?"even":"odd"%>'>
                                    <%j = 0; %>
                                    <td colspan="5">
                                        <table width="100%" class="tb_slist" cellspacing="0" cellpadding="0">
                                            <asp:Repeater ID="rpt_sList" runat="server">
                                                <ItemTemplate>
                                                    <%if (j > 0)
                                                      { %>
                                                    <tr>
                                                        <%} %>
                                                        <td align="center" width="20%">
                                                            <%#DateTime.Parse(Eval("DepartureTime").ToString()).ToString("yyyy-MM-dd")%>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <%#Eval("FligthSegment")%>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <%#Eval("TicketTime")%>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <%#Eval("AireLine")%>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("Discount").ToString())%>
                                                        </td>
                                                    </tr>
                                                    <%j++; %>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </td>
                                    <%i++; %>
                                    <td align="center">
                                        <%#Eval("PNR")%>
                                    </td>
                                    <td align="center">
                                        ￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundAdult")).TotalMoney.ToString())%>
                                        /￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundChildren")).TotalMoney.ToString())%>
                                    </td>
                                    <td align="center">
                                        ￥<%# getOtherMoney(EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundAdult")).AgencyPrice.ToString()), EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundAdult")).OtherPrice.ToString()))%>
                                        /￥<%#getOtherMoney(EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundChildren")).AgencyPrice.ToString()), EyouSoft.Common.Utils.FilterEndOfTheZeroString(((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundChildren")).OtherPrice.ToString()))%>
                                    </td>
                                    <td align="center">
                                        <a href="VisitorList.aspx?ticketIssueId=<%#Eval("ApplyId") %>" class="showVisitor">
                                            <%# ((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundAdult")).PeopleCount.ToString()%>
                                            /<%#((EyouSoft.Model.PlanStructure.TicketKindInfo)Eval("FundChildren")).PeopleCount.ToString()%>
                                        </a>
                                    </td>
                                    <td align="center">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString())%>
                                    </td>
                                    <td align="center">
                                        <%#getStatus((EyouSoft.Model.EnumType.PlanStructure.TicketState)Eval("Status"))%>
                                        <span style="display: <%#(EyouSoft.Model.EnumType.PlanStructure.TicketState)Eval("Status")==EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请?"block":"none"%>">
                                            <a class="lnk_update" style="display: <%#Eval("Status").ToString()=="2"?"none;":"block;"%>"
                                                href="sanping_jipiaoAdd.aspx?tourId=<%=Request.QueryString["tourId"] %>&id=<%#Eval("ApplyId") %>&type=<%=Request.QueryString["type"] %>">
                                                <font class="fblue">修改</font></a> <a href="javascript:void(0)" idv="<%#Eval("ApplyId") %>"
                                                    style="display: <%#Eval("Status")=="2"?"none;":"block;"%>" class="lnk_delete"><font
                                                        class="fblue">删除</font></a></span>
                                    </td>
                                </tr>
                                <%i++; %>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="odd" id="tr" runat="server"> 
                            <td width="42%" height="30" align="center">
                                合计
                            </td>
                            <td colspan="5" align="center"></td>
                             <td width="7%" align="center">
                                <asp:Literal ID="litFundAdult" runat="server"></asp:Literal>/<asp:Literal ID="LitFunChildren" runat="server"></asp:Literal>
                            </td>
                            <td width="7%" align="center">
                               <asp:Literal ID="LitAgencyAdult" runat="server"></asp:Literal>/<asp:Literal ID="LitAgencyChild" runat="server"></asp:Literal>                    
                            </td>
                             <td width="7%" align="center">
                                <asp:Literal ID="LitAdultCount" runat="server"></asp:Literal>/<asp:Literal ID="litChildRenCount"  runat="server"></asp:Literal>
                            </td>
                            <td width="7%" align="center">
                                <asp:Literal ID="LitTotalCount" runat="server"></asp:Literal>
                            </td>
                            <td width="7%" align="center"></td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
            <fieldset style="border: 1px solid rgb(163, 211, 248); padding: 5px 0pt;">
                <legend>申请机票</legend>
                <uc1:jipiaoUpdate ID="jipiaoUpdate1" runat="server" isWindow="false" />
            </fieldset>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            $(".showVisitor").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "机票申请游客信息",
                    modal: true,
                    width: "700px",
                    height: "400px"
                });
                return false;
            });
            $(".lnk_delete").click(function() {
                var id = $(this).attr("idv");
                if (confirm("确认删除?")) {
                    $.newAjax({
                        url: "/sanping/SanPing_JiPiaoAdd.aspx?act=del&id=" + id+"&tourid=<%=Request.QueryString["tourid"] %>",
                        type: "POST",
                        dataType: "json",
                        success: function(r) {
                            if (parseInt(r) > 0) {
                                alert("删除成功!");
                                location.href = location.href;
                            } else if (parseInt(r) < 0){
                            alert("该团已提交财务，不允许对它进行任何操作!");
                            }else{
                                alert("删除失败!请稍后再试!");
                            }
                        }
                    });

                }
                return false;
            });
        });
    </script>

</asp:Content>
