<%@ Page Language="C#" Title="团款支出" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="waitkuan.aspx.cs" Inherits="Web.caiwuguanli.waitkuan" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">财务管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt;财务管理 &gt;&gt; 团款支出
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
        <ul class="fbTab">
            <li><a id="two1" href="TeamExpenditure.aspx">应付账款</a></li>
            <li><a class="tabtwo-on" id="two2" href="waitkuan.aspx" class="">付款审批</a></li>
            <li><a id="two3" href="teamPayClear.aspx" class="">已结清账款</a></li>
            <li><a id="two4" href="jidiaoleixing.aspx">批量登记</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div style="display: block;" id="con_two_2">
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    团号：</label>
                                <input type="text" id="txt_teamNum" class="searchinput" runat="server" name="txt_teamNum">
                                <label>
                                    收款单位：</label>
                                <input type="text" class="searchinput searchinput02" runat="server" id="txt_com"
                                    name="txt_com">
                                <label>
                                    申请人：</label>
                                <input type="hidden" id="hd_operateId" />
                                <select id="selOperator" runat="server">
                                </select>
                                <label>状态：</label>
                                <select id="seleState" runat="server">
                                  <option value="-1">请选择</option>
                                  <option value="1">未审批</option>
                                  <option value="2">未支付</option>
                                </select>
                                <label>
                                    申请日期：</label>
                                <input type="text" onfocus="WdatePicker()" id="txt_Date" class="searchinput" runat="server"
                                    name="txt_Date" />-<input type="text" onfocus="WdatePicker()" id="txtRegEDate" class="searchinput" 
                                    name="txtRegEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("regedate") %>" />
                                <label>
                                    <asp:ImageButton ID="ImageButton1" Style="vertical-align: middle;" ImageUrl="../images/searchbtn.gif"
                                        runat="server" OnClick="ImageButton1_Click" /></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="../images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox" style="display: none">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td width="90" align="center">
                                <asp:LinkButton ID="lnkCheck" Visible="false" CssClass="del lnkCheck" runat="server"
                                    OnClick="lnkCheck_Click">审 批</asp:LinkButton>
                            </td>
                            <td width="90" align="center">
                                <asp:LinkButton ID="lnkPay" Visible="false" CssClass="del lnkPay" runat="server"
                                    OnClick="lnkPay_Click">支 付</asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <a href="javascript:();" onclick="All(this)">批量审核</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:();" onclick="All(this)">批量支付</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <th width="3%" bgcolor="#bddcf4" align="center">
                                全选<input type="checkbox" id="allchk" />
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                团号
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                团队名称
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                收款单位
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                付款人
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                申请日期
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                付款金额
                            </th>
                            <th width="20%" bgcolor="#bddcf4" align="center">
                                备注
                            </th>
                            <th width="12%" bgcolor="#bddcf4" align="center">
                                操作
                            </th>
                        </tr>
                        <cc2:CustomRepeater runat="server" ID="rpt_list" OnItemCommand="rpt_list_ItemCommand">
                            <ItemTemplate>
                                <tr class="<%=i%2==0 ?"even":"odd" %>">
                                    <td align="center">
                                        <input type="checkbox" class="chk" name="chk_sel" registerid="<%#Eval("RegisterId")%>"
                                            ischecked="<%#Eval("IsChecked")%>" ispay="<%#Eval("IsPay")%>">
                                    </td>
                                    <td align="center">
                                        <%#((EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Eval("ReceiveType")) == EyouSoft.Model.EnumType.FinanceStructure.OutPlanType.单项服务供应商安排 ? Eval("tourCode") : Eval("tourCode")%>
                                    </td>
                                    <td align="center">
                                        <%#((EyouSoft.Model.EnumType.FinanceStructure.OutPlanType)Eval("ReceiveType")) == EyouSoft.Model.EnumType.FinanceStructure.OutPlanType.单项服务供应商安排 ? "单项服务供应商安排" : Eval("RouteName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ReceiveCompanyName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("StaffName")%>
                                    </td>
                                    <td align="center">
                                        <%#EyouSoft.Common.Utils.GetDateTimeNullable(Eval("PaymentDate").ToString()).Value.ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center">
                                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("PaymentAmount").ToString())%></font>
                                    </td>
                                    <td align="center">
                                        <%#Eval("Remark")%>
                                    </td>
                                    <td align="center">
                                        <%#getUrl(bool.Parse(Eval("IsChecked").ToString()), bool.Parse(Eval("IsPay").ToString()), Eval("RegisterId").ToString(), Eval("ItemId").ToString(),Eval("ReceiveId").ToString())%>
                                    </td>
                                </tr>
                                <%i++; %>
                            </ItemTemplate>
                        </cc2:CustomRepeater>
                        <tr>
                            <th width="12%" bgcolor="#bddcf4" align="left" colspan="6">
                                合计：
                            </th>
                            <td width="10%" bgcolor="#bddcf4" align="center">
                                <font class="fred">￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(paymentAmount)%></font>
                            </td>
                            <th width="20%" bgcolor="#bddcf4" align="center" colspan="2">
                            </th>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="8">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script>
        $(function() {
            $(".lnkPay,.lnkCheck").click(function() {
                var chkval = "";
                $("[name='chk_sel']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                    }
                });
                if (chkval == "") {
                    alert("请选择一条记录!");
                    return false;
                }
            });
            $(".shenpi").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "审批",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });

            $(".pass").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "审批",
                    modal: true,
                    width: "820px",
                    height: "620px"
                });
                return false;
            });
            $("#allchk").click(function() {
                $(".chk[ispay='False']").each(function() {
                    $(this).attr("checked", $("#allchk").attr("checked"));
                })
            });
            $(".chk").each(function() {

                if ($(this).attr("ispay") == "True") {
                    $(this).attr("disabled", true);
                }
            });
        });
        //批量操作
        function All(thiss) {
            var msg = "";
            //判断操作
            if ($(thiss).html() == "批量审核") {
                msg = "审核";
            }
            else {
                msg = "支付";
            }
            var ids = "";
            var url = window.location.pathname;
            var val = 0;
            //根据操作读取Ids
            if (msg == "审核") {
                $(".chk[ischecked='True']:checked").each(function() {
                    val++;

                })
                if (val <= 0) {
                    $(".chk[ischecked='False']:checked").each(function() {
                        ids += ($(this).attr("registerid") + ",");

                    })
                }
                url += "?act=Allpass";
            }
            else {
                $(".chk[ischecked='False'][ispay='False']:checked").each(function() {
                    val++;

                })
                if (val <= 0) {
                    $(".chk[ischecked='True'][ispay='False']:checked").each(function() {
                        ids += ($(this).attr("registerid") + ",");

                    })
                }
                url += "?act=Allpay";
            }
            if (ids.length > 0) {
                $.newAjax({
                    url: url,
                    type: "POST",
                    data: { "ids": ids.substring(0, ids.length - 1) },
                    dataType: 'json',
                    success: function(d) {
                        if (d.res) {
                            switch (d.res) {
                                case 1:
                                    alert("批量" + msg + "成功");
                                    $(':checkbox').attr("checked", "");
                                    location.reload();
                                    break;
                                case -1:
                                    alert("批量" + msg + "失败");
                                    break;
                            }
                        }
                    },
                    error: function() {
                        alert("服务器繁忙!");
                    }
                });
            }
            else {
                if (msg == "审核") {
                    alert("请正确选择" + msg + "的记录,目前存在：" + val + "条不能审核的记录");
                }
                else {
                    alert("请正确选择" + msg + "的条目,目前存在：" + val + "条不能支付的记录");
                }

                return false;
            }
        }    
    </script>

</asp:Content>
