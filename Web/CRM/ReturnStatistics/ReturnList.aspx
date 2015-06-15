<%@ Page Title="返佣统计_客户关系管理" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="ReturnList.aspx.cs" Inherits="Web.CRM.ReturnStatistics.ReturnList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">客户关系管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 客户关系管理 >> 返佣统计
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
        <form id="form1" runat="server">
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            客户单位：</label><asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                        <label>
                            组团社名称：</label><input type="hidden" name="hd_teamId" id="hd_teamId" value="<%=hd_teamId %>" /><input
                                type="text" id="txt_teamName" errmsg="*请选择组团社" valid="required" name="txt_teamName"
                                value="<%=txt_teamName %>" /><a href="/CRM/customerservice/SelCustomer.aspx?method=selectTeam"
                                    class="selectTeam"><img src="/images/sanping_04.gif" width="28" height="18"></a>
                        <label>
                            对方操作员：</label>
                        <asp:DropDownList ID="czy" runat="server" AppendDataBoundItems="True">
                            <asp:ListItem>--请选择--</asp:ListItem>
                        </asp:DropDownList>
                        <input id="hd_ContactId" type="hidden" runat="server" />
                        <uc1:selectoperator id="selectOperator1" title="我社操作员" textclass="searchinput" runat="server"
                            isshowlabel="true" />
                        <br />
                        <label>
                            出团日期：</label><asp:TextBox ID="txtLeaveDateStart" CssClass="searchinput" onfocus="WdatePicker()"
                                runat="server"></asp:TextBox>
                        &nbsp;至&nbsp;
                        <asp:TextBox ID="txtLeaveDateEnd" CssClass="searchinput" onfocus="WdatePicker()"
                            runat="server"></asp:TextBox>
                        <label>
                            下单日期：</label><asp:TextBox ID="txtOrderDateStart" CssClass="searchinput" onfocus="WdatePicker()"
                                runat="server"></asp:TextBox>
                        &nbsp;至&nbsp;
                        <asp:TextBox ID="txtOrderDateEnd" CssClass="searchinput" onfocus="WdatePicker()"
                            runat="server"></asp:TextBox>
                        <label>
                            <a href="javascript:void(0);" id="select">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        <div class="tablelist">
            <table width="100%" id="printtable" border="0" cellpadding="0" cellspacing="1">
                <tr bgcolor="#BDDCF4">
                    <th width="26%" align="center">
                        客户单位
                    </th>
                    <th width="16%" align="center">
                        对方操作员
                    </th>
                    <th width="16%" align="center">
                        返佣金额
                    </th>
                    <th width="16%" align="center">
                        现返金额
                    </th>
                    <th width="16%" align="center">
                        后返金额
                    </th>
                </tr>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>" customerid="<%#Eval("CustomerId")%>"
                            contactid="<%#Eval("ContactId")%>">
                            <td align="center">
                                <%#Eval("CustomerName")%>
                            </td>
                            <td align="center">
                                <%#Eval("ContactName")%>
                            </td>
                            <td align="center">
                                <%#Eval("CommissionAmount", "{0:c2}")%>
                            </td>
                            <td align="center">
                                <a type="earlier" href="javascript:();" onclick="GoDetail(this)">
                                    <%#Eval("BeforeAmount", "{0:c2}")%></a>
                            </td>
                            <td align="center">
                                <a type="behind" href="javascript:();" onclick="GoDetail(this)">
                                    <%#Eval("AfterAmount", "{0:c2}")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="5" align="center">
                        <asp:Label ID="lblMsg" runat="server" Text="暂无数据" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td height="30" align="right" class="pageup" colspan="13">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
        </form>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        function GoDetail(thiss) {
        
            var customerid = $(thiss).parent().parent().attr("customerid");
            var contactid = $(thiss).parent().parent().attr("contactid");
            var type = $(thiss).attr("type") == "earlier" ? "1" : "2";
            var url = "/CRM/ReturnStatistics/MonerList.aspx?type=" + type + "&customerid=" + customerid + "&contactid=" + contactid
            var CustomerName = $("#<%= txtCustomerName.ClientID%>").val();
            var czyId = $("#<%=czy.ClientID %>").val();
            var LeaveDateStart = $("#<%= txtLeaveDateStart.ClientID%>").val();
            var LeaveDateEnd = $("#<%= txtLeaveDateEnd.ClientID%>").val();
            var OrderDateStart = $("#<%= txtOrderDateStart.ClientID%>").val();
            var OrderDateEnd = $("#<%= txtOrderDateEnd.ClientID%>").val();
            var OperatorId=<%=selectOperator1.ClientID%>.GetOperatorId();
            url = url+
            "&CustomerName=" + CustomerName +
            "&czyId=" + czyId +
            "&LeaveDateStart=" + LeaveDateStart +
            "&LeaveDateEnd=" + LeaveDateEnd +
            "&OrderDateStart=" + OrderDateStart +
            "&OrderDateEnd=" + OrderDateEnd +
            "&OperatorId=" + OperatorId ;
            var title=type=="1"?"现返"+ "明细":"后返" + "明细";
            
            Boxy.iframeDialog({
                iframeUrl: url,
                title: title,
                modal: true,
                width: "720px",
                height: "400px"
            });
            return false;
        }
        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        function selectTeam(id, name) {
            $("#hd_teamId").val(id);
            $("#txt_teamName").val(name);
            if (id)
                Getczy();
        };
        function Getczy() {
            var zId = $("#hd_teamId").val();
            $.newAjax({
                type: "POST",
                url: "/CRM/ReturnStatistics/ReturnList.aspx?type=teamSelect&zId=" + zId,
                dataType: 'html',
                success: function(ret) {
                $("#<%=czy.ClientID %>").html(ret);
                var hd_ContactId = $("#<%= hd_ContactId.ClientID%>").val();
                $("#<%= czy.ClientID%>").val(hd_ContactId);
                },
                error: function() {
                    alert("服务器繁忙!");
                }
            });

        };
        $(function() {
            var hd_teamId = $("#hd_teamId").val();
            if ($.trim(hd_teamId) != "") {
                Getczy();
                
            }
            $("[name='txt_teamName']").focus(function() {
                $(".selectTeam").click();
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
            $("#select").click(function() {
                search();
                return false;
            })

        })
        function search() {
            var CustomerName = $("#<%= txtCustomerName.ClientID%>").val();
            var czyId = $("#<%=czy.ClientID %>").val();
            var LeaveDateStart = $("#<%= txtLeaveDateStart.ClientID%>").val();
            var LeaveDateEnd = $("#<%= txtLeaveDateEnd.ClientID%>").val();
            var OrderDateStart = $("#<%= txtOrderDateStart.ClientID%>").val();
            var OrderDateEnd = $("#<%= txtOrderDateEnd.ClientID%>").val();
            var OperatorId=<%=selectOperator1.ClientID%>.GetOperatorId();
            var OperatorIdName=<%=selectOperator1.ClientID %>.GetOperatorName();
            var hd_teamId = $("#hd_teamId").val();
            var txt_teamName = $("#txt_teamName").val();
            var url = window.location.pathname +
            "?CustomerName=" + CustomerName +
            "&czyId=" + czyId +
            "&LeaveDateStart=" + LeaveDateStart +
            "&LeaveDateEnd=" + LeaveDateEnd +
            "&OrderDateStart=" + OrderDateStart +
            "&OrderDateEnd=" + OrderDateEnd +
            "&OperatorId=" + OperatorId +
            "&hd_teamId=" + hd_teamId +
            "&txt_teamName=" + txt_teamName+
            "&OperatorIdName="+OperatorIdName;
            location.href = url;

        };
       
    </script>

</asp:Content>
