<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SellStat.aspx.cs" Inherits="Web.CRM.SellStat.SellStat"
    MasterPageFile="~/masterpage/Back.Master" Title="销售统计_客户关系管理" %>

<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc9" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc4" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc5" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ProvinceAndCity.ascx" TagName="ProvinceAndCity" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <style type="text/css">
        #pringList td
        {
            padding: 3px;
        }
    </style>
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">客户关系管理 </span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：客户关系管理>> 销售统计
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <form runat="server" id="custForm">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        发团日期：<asp:TextBox ID="txtToLeaveDate" CssClass="searchinput" onfocus="WdatePicker()"
                            runat="server"></asp:TextBox>
                        &nbsp;至&nbsp;
                        <asp:TextBox ID="txtEndLeaveDate" CssClass="searchinput" onfocus="WdatePicker()"
                            runat="server"></asp:TextBox>
                        线路区域：
                        <asp:DropDownList ID="ddl_LineType" runat="server" AppendDataBoundItems="True" DataTextField="AreaName"
                            DataValueField="Id">
                        </asp:DropDownList>
                        组团社名称：<input type="hidden" name="hd_teamId" id="hd_teamId" value="<%=hd_teamId %>" />
                        <input type="text" id="txt_teamName" errmsg="*请选择组团社" valid="required" name="txt_teamName"
                            value="<%=txt_teamName %>" readonly="readonly"  /><a href="/CRM/customerservice/SelCustomer.aspx?method=selectTeam"
                                class="selectTeam"><img src="/images/sanping_04.gif" width="28" height="18" alt="" /></a>
                        对方操作员：
                        <asp:DropDownList ID="czy" runat="server">
                        </asp:DropDownList>
                        <br />
                        <uc5:selectOperator ID="selectOperator2" runat="server" Title="责任销售：" />
                        <uc5:selectOperator ID="selectOperator1" runat="server" Title="订单操作人：" />
                        <!--省份城市选取控件开始-->
                        <uc1:ProvinceAndCity runat="server" ID="ProvinceAndCity1" />

                        <script type="text/javascript">
                            window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                        </script>
                        <br/>
                        上单日期：
                        <input type="text" onfocus="WdatePicker()" id="txtShangDanSDate" class="searchinput" name="txtShangDanSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("sdsdate") %>" style="width: 100px" />-<input type="text" onfocus="WdatePicker()" id="txtShangDanEDate" class="searchinput" name="txtShangDanEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("sdedate") %>" style="width: 100px" />
                        

                        <!--省份城市选取控件结束-->
                        <label>
                            <a href="javascript:;" id="select">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" alt="" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        </form>
        <div class="btnbox">
            <table width="20%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <%if (IsImportOut)
                      { %>
                    <td width="90" align="center">
                        <uc9:UCPrintButton ID="UCPrintButton1" ContentId="pringList" runat="server" />
                    </td>
                    <td width="90" align="center">
                        <a href="javascript:;" id="toexcel">
                            <img src="/images/daoru.gif" alt="" width="16" height="16" />
                            导 出</a>
                    </td>
                    <%} %>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <table id="pringList" width="100%" border="0" cellpadding="0" cellspacing="1" align="center">
                <tr>
                    <th width="85px" align="center" bgcolor="#bddcf4">
                        所属地区
                    </th>
                    <th width="85px" align="center" bgcolor="#bddcf4">
                        线路区域
                    </th>
                    <th align="center" bgcolor="#bddcf4">
                        线路名称
                    </th>
                    <th width="135px" align="center" bgcolor="#bddcf4">
                        组团社名称
                    </th>
                    <th width="56px" align="center" bgcolor="#bddcf4">
                        责任销售
                    </th>
                    <th width="56px" align="center" bgcolor="#bddcf4">
                        订单操作人
                    </th>
                    <th width="56px" align="center" bgcolor="#bddcf4">
                        所属部门
                    </th>
                    <th width="85px" align="center" bgcolor="#BDDCF4">
                        对方操作人员
                    </th>
                    <th width="32px" align="center" bgcolor="#bddcf4">
                        人数
                    </th>
                    <th width="60px" align="center" bgcolor="#bddcf4">
                        金额
                    </th>
                    <th width="86px" align="center" bgcolor="#BDDCF4">
                        出团日期
                    </th>
                </tr>
                <asp:CustomRepeater ID="rptCustomer" runat="server">
                    <ItemTemplate>
                        <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td width="85px" align="center" bgcolor="#e3f1fc">
                                <%#Eval("ProvinceAndCity")%>
                            </td>
                            <td width="85px" align="center" bgcolor="#e3f1fc">
                                <%#Eval("AreaName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%#Eval("RouteName")%>
                            </td>
                            <td width="135px" align="center" bgcolor="#e3f1fc">
                                <a href="###" onclick="openKeHuWindow(<%#Eval("KeHuDanWeiId") %>)"><%#Eval("CustomerName")%></a>
                            </td>
                            <td width="56px" align="center" bgcolor="#e3f1fc">
                                <%# Eval("ZeRenSelllerName")%>
                            </td>
                            <td width="56px" align="center" bgcolor="#e3f1fc">
                                <%# Eval("SalerName") %>
                            </td>
                            <td width="56px" align="center" bgcolor="#e3f1fc">
                                <%--<%# Eval("Logistics") == null ? "" : GetListConverToStrSalesClerk(Eval("Logistics") as System.Collections.Generic.IList<EyouSoft.Model.StatisticStructure.StatisticOperator>)%>--%>
                                <%#Eval("DepartName") == "" ||Eval("DepartName") ==null ? "暂无部门" : Eval("DepartName")%>
                            </td>
                            <td width="85px" align="center" bgcolor="#e3f1fc">
                                <%#Eval("OperatorName")%>
                            </td>
                            <td width="32px" align="center" bgcolor="#e3f1fc">
                                <%#Eval("PeopleNum")%>
                            </td>
                            <td width="60px" align="center" bgcolor="#e3f1fc">
                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString())%>
                            </td>
                            <td width="76px" align="center" bgcolor="#e3f1fc">
                                <%#Eval("LeaveDate", "{0:yyyy-MM-dd}")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:CustomRepeater>
                <tr>
                    <th width="85px" align="center" bgcolor="#bddcf4">
                        金额总计
                    </th>
                    <th colspan="7" bgcolor="#bddcf4">
                    </th>
                    <th width="32px" align="right" bgcolor="#bddcf4">
                        <asp:Label ID="lblPeopleNum" runat="server" Text=""></asp:Label>
                    </th>
                    <th width="60px" align="right" bgcolor="#bddcf4">
                        <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                    </th>
                    <th width="76px" align="right" bgcolor="#bddcf4">
                    </th>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                        <asp:Label ID="lblMsg" runat="server" Text="暂无数据!" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        $(function() {

            $("#select").click(function() {
                search(null);
                return false;
            });
            $("#toexcel").click(function() {
                search("toexcel")
                return false;
            });
            $("[name='txt_teamName']").change(function() {
                $("#hd_teamId").val("");
            });

            $(".selectTeam").click(function() {
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
                var url = $(this).attr("href");
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选用组团社",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: iframeId,
                        backfun: "selectTeam"
                    }

                });
                return false;
            });
            
            $("#txt_teamName").click(function() {
                $(".selectTeam").click();
                return false;
            });
            
        });
        function Getczy() {
            var zId = $("#hd_teamId").val();
            $.newAjax({
                type: "POST",
                url: "/CRM/SellStat/SellStat.aspx?type=teamSelect&zId=" + zId,
                dataType: 'html',
                success: function(ret) {
                    $("#<%=czy.ClientID %>").html(ret);
                },
                error: function() {
                    alert("服务器繁忙!");
                }
            });

        };
        function selectTeam(id, name) {
            $("#hd_teamId").val(id);
            $("#txt_teamName").val(name);
            if(id)
            Getczy();
        };
        function search(type) {
            var todate =  encodeURIComponent( $("#<%= txtToLeaveDate.ClientID%>").val());
            var endate = $("#<%= txtEndLeaveDate.ClientID%>").val();
            var ddlLineType = $("#<%= ddl_LineType.ClientID%>").val();
            
            var jdyId = <%= selectOperator1.ClientID%>.GetOperatorId();
            var xsyId = <%= selectOperator2.ClientID%>.GetOperatorId();
            var hd_teamId = $("#hd_teamId").val();
            var hd_teamName = encodeURIComponent ($("#txt_teamName").val());
            var jdyName = encodeURIComponent(<%= selectOperator1.ClientID%>.GetOperatorName());
            var xsyName =encodeURIComponent( <%= selectOperator2.ClientID%>.GetOperatorName());
            var czyId = $("#<%=czy.ClientID %>").val();           
            var cat = "";
            if (type == "toexcel") {
                cat = "type=toexcel&"
            }
           
           var province = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().id;
           var cityId = window["<%=ProvinceAndCity1.ClientID %>"].getCity().id;
           var shangDanSDate=$("#txtShangDanSDate").val();
           var shangDanEDate=$("#txtShangDanEDate").val();
           
             location.href = "SellStat.aspx?" +  cat + "todate=" + todate + "&endate=" +
            endate + "&ddlLineType=" + ddlLineType +
            "&hd_teamId=" + hd_teamId + "&hd_teamName=" + hd_teamName +
            "&cityId=" + cityId + "&province=" + province +
            "&jdyId=" + jdyId + "&jdyName=" + jdyName +
            "&xsyId=" + xsyId + "&xsyName=" + xsyName + "&czyId=" + czyId + "&zId=" + hd_teamId+"&sdsdate="+shangDanSDate+"&sdedate="+shangDanEDate;
            

        };
       
       //打开查看客户资料窗口
       function openKeHuWindow(_v) {
            parent.Boxy.iframeDialog({
                iframeUrl: '/CRM/customerinfos/CustomerEdit.aspx?type=show&custId='+_v,
                title: "查看客户资料",
                modal: true,
                width: 950,
                height: 550,
                data: {}
            });
       }
    </script>

</asp:Content>
