<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="JiPiaoAudit.aspx.cs" Inherits="Web.caiwuguanli.JiPiaoAudit" Title="机票审核_财务管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<%@ Import Namespace="EyouSoft.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .FlightTD
        {
            border-top: solid 1px #fff;
            border-right: solid 1px #fff;
            text-align: center;
        }
    </style>
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
                                <span class="lineprotitle">财管管理</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                所在位置&gt;&gt; 财管管理 &gt;&gt; 机票审核
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
                            <img src="/images/yuanleft.gif" />
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    <uc1:selectOperator ID="selectOperator1" runat="server" Title="计调员" TextClass="searchinput" />
                                    航班号/时间：</label>
                                <input type="text" class="searchinput" id="inputDate" name="textfield1" class="Wdate" />
                                <label>
                                    航段：</label>
                                <input type="text" id="inputGoLine" class="searchinput" name="textfield2" />
                                <label>
                                    团号：</label><label><input type="text" id="txtTourCode" name="txtTourCode" class="searchinput"
                                        style="width: 90px" value="<%=Utils.GetQueryStringValue("tourCode") %>" /></label>
                                <label>
                                    审核时间：</label>
                                <input type="text" class="searchinput Wdate" id="txtSVerifyTime" onfocus="WdatePicker()"
                                    value="<%=Utils.GetQueryStringValue("sVerifyTime") %>" />
                                -
                                <input type="text" class="searchinput Wdate" id="txtEVerifyTime" onfocus="WdatePicker()"
                                    value="<%=Utils.GetQueryStringValue("eVerifyTime") %>" />
                                <br />
                                <label>
                                    出团时间：</label>
                                <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput"
                                    name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input
                                        type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate"
                                        value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                                <label>
                                    <a href="javascript:void(0)" id="btnSearch">
                                        <img style="vertical-align: top;" src="../images/searchbtn.gif"></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th height="30" bgcolor="#bddcf4" align="center" width="4%">
                                序号
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                团号
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                订单号
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                客户单位
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="14%">
                                线路名称
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                销售员
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                计调员
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="110px">
                                日期
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="100px">
                                航段
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                状态
                            </th>
                            <th style="width: 11%; text-align: center; background-color: #bddcf4">
                                审核时间
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                操作
                            </th>
                        </tr>
                        <cc1:CustomRepeater ID="repList" runat="server">
                            <ItemTemplate>
                                <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <td height="30" align="center">
                                        <%#Container.ItemIndex+1%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("TourNum")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("OrderNo")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("BuyCompanyName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("RouteName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Saler")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Operator")%>
                                    </td>
                                    <td align="center" colspan="2" style="padding: 0; margin: 0;">
                                        <table width="100%" style="border: none; border-collapse: collapse; text-align: center;"
                                            class="tbl">
                                            <%#GetHtmlByList(Eval("TicketFlights"), EyouSoft.Common.Utils.GetDateTime(Eval("RegisterTime").ToString()))%>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <%#(EyouSoft.Model.EnumType.PlanStructure.TicketState)Eval("State")%>
                                    </td>
                                    <td style="text-align: center">
                                        <%#((EyouSoft.Model.EnumType.PlanStructure.TicketState)Eval("State")).ToString()=="机票申请"?"":Eval("VerifyTime")==null ? "" : string.Format("{0:yyyy-MM-dd HH:mm}",Eval("VerifyTime"))%>
                                    </td>
                                    <td align="center" class="jipiao_turn">
                                        <a style="margin-left: 15px;" onclick="JiPiaoList.Audit(this);" href="javascript:void(0);"
                                            gourl="/caiwuguanli/JipiaoValidate.aspx?id=<%#Eval("RefundId")%>">查看</a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </cc1:CustomRepeater>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="10">
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
        var JiPiaoList = {

            GetSelectCount: function() {
                var count = 0;
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked") == true) {
                        count++;
                    }
                });
                return count;
            },

            GetSelectItemValue: function() {
                var arrayList = new Array();
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked") == true) {
                        arrayList.push($(this).attr("id"));
                    }
                });
                return arrayList;
            },
            //机票审核页面
            Audit: function(obj) {
                var url = $(obj).attr("goUrl");
                JiPiaoList.openXLwindow(url, '机票审核', '900px', '480px');      
                return false;              
            },
            openXLwindow: function(url, title, width, height) {
                url = url;
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: title,
                    modal: true,
                    width: width,
                    height: height
                });
            }
        }

        $(document).ready(function() {

            $(".tbl").each(function() {
                var height = $(this).parent("td").parent("tr");
                $(this).height(height.height() + 10);
            })
            $("#jiPiaoChuStats,#jiPiaoTuiStats").click(function() {
                var url = $(this).attr("goUrl");
                var title = $(this).text();
                JiPiaoList.openXLwindow(url, title, '880px', '680px');
                return false;
            });
            
            $("#btnSearch").click(function() {
                var data = {
                    inputDate: $("#inputDate").val(),
                    inputGoLine: $("#inputGoLine").val(),
                    oper:<%=selectOperator1.ClientID%>.GetOperatorId(),
                    operName:<%=selectOperator1.ClientID%>.GetOperatorName(),
                    tourCode:$.trim($("#txtTourCode").val()),
                    sVerifyTime:$.trim($("#txtSVerifyTime").val()),
                    eVerifyTime:$.trim($("#txtEVerifyTime").val()),
                    lsdate:$("#txtLeaveSDate").val(),
                    ledate:$("#txtLeaveEDate").val()
                };
                window.location.href = "/caiwuguanli/JiPiaoAudit.aspx?" + $.param(data);
                return false;
            });

            $(function() {
                if (queryString("inputDate") != "" && queryString("inputDate") != null) {
                    $("#inputDate").val(queryString("inputDate"));
                }
                if (queryString("inputGoLine") != "" && queryString("inputGoLine") != null) {
                    $("#inputGoLine").val(queryString("inputGoLine"));
                }
                if (queryString("inputBackLine") != "" && queryString("inputBackLine") != null) {
                    $("#inputBackLine").val(queryString("inputBackLine"));
                }
            });

            $("#<%=this.form1.ClientID%>").keydown(function(event) {
                if (event.keyCode == 13) {
                    $("#btnSearch").click();
                    return false;
                }
            });
        });

    </script>

    </form>
</asp:Content>
