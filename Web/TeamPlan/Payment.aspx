<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs"
    Inherits="Web.TeamPlan.Payment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>无标题文档</title>
    <link type="text/css" rel="stylesheet" href="../css/sytle.css">

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</head>
<body>
    <form runat="server" id="myform">
    <table width="800" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px;">
        <tbody>
            <tr class="odd">
                <th width="110" height="30" bgcolor="#bddcf4" align="center">
                    付款时间
                </th>
                <th width="125" bgcolor="#bddcf4" align="center">
                    支付方式
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    付款金额
                </th>
                <th width="125" bgcolor="#bddcf4" align="center">
                    是否开票
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    付款人
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    发票/收据号
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    备注
                </th>
                <th width="80" bgcolor="#bddcf4" align="center">
                    操作
                </th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_list" OnItemDataBound="rpt_list_ItemDataBound">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <input type="hidden" value="<%#Eval("RegisterId") %>" class="registid" />
                            <font class="xinghao">*</font><input type="text" value="<%#DateTime.Parse( Eval("PaymentDate").ToString()).ToString("yyyy-MM-dd")%>"
                                onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'});" class="searchinput" name="t_payDate">
                        </td>
                        <td align="center">
                            <font class="xinghao">*</font><asp:DropDownList CssClass="sel_PayType" ID="sel_PayType"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <font class="xinghao">*</font><input type="text" class="t_payMoney searchinput" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("PaymentAmount").ToString())%>"
                                class="searchinput" />
                        </td>
                        <td align="center">
                            <font class="xinghao">*</font><asp:DropDownList CssClass="sel_isbill" ID="sel_isbill"
                                runat="server">
                            </asp:DropDownList>
                        </td>
                        <td align="center">
                            <font class="xinghao">*</font><input type="text" class="searchinput" value="<%#Eval("StaffName")%>"
                                name="t_staffname" />
                        </td>
                        <td align="center">
                            <input type="text" class="searchinput" value="<%#Eval("BillNo")%>" name="t_billno" />
                        </td>
                        <td align="center">
                            <textarea rows="2" cols="20" id="t_desc" name="t_desc"><%#Eval("Remark")%></textarea>
                        </td>
                        <td align="center">
                            <a href="javascript:void(0)" class="save" style="display: <%#((bool)Eval("IsChecked"))==false?"block":"none"%>">
                                保存</a> <span style="display: <%#((bool)Eval("IsPay"))==false?"block":"none"%>">
                                    <%#((bool)Eval("IsChecked"))==true?"已审核":""%></span><span><%#((bool)Eval("IsPay")) == true ? "已支付" : ""%></span>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="even">
                <td height="30" align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*请选择日期" valid="required"
                        id="txt_payDate" onfocus="WdatePicker();" class="searchinput"
                        name="txt_payDate">
                </td>
                <td align="center">
                    <font class="xinghao">*</font><asp:DropDownList errmsg="*请选择支付方式" valid="required"
                        ID="ddlPayType" runat="server">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*请填写付款金额|*请填写正确的付款金额" valid="required|isMoney"
                        id="txt_fukuan" class="searchinput" runat="server" />
                </td>
                <td align="center">
                    <font class="xinghao">*</font><select name="sel_is" id="sel_is"><option value="1">是</option>
                        <option value="2">否</option>
                    </select>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*请填写付款人" valid="required"
                        id="txt_Lxr" class="searchinput" name="txt_Lxr">
                </td>
                <td align="center">
                    <input type="text" id="txt_code" class="searchinput" name="txt_code" />
                </td>
                <td align="center">
                    <textarea rows="2" cols="20" id="txt_desc" name="txt_desc"></textarea>
                </td>
                <td align="center">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">保存</asp:LinkButton>
                </td>
            </tr>
        </tbody>
    </table>
    <table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
        
    </table>
      <asp:HiddenField ID="hideTourType" runat="server" />
    </form>

    <script>

    function queryString(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    }

    $(function() {

        var money = queryString("money");

        var sum = 0;
        $(".fkmoney").each(function() {
            sum += parseFloat($(this).html())
        });

        $(".save").click(function() {
            var payDate = $(this).closest("tr").find("[name='t_payDate']").val();
            var payType = $(this).closest("tr").find(".sel_PayType").val();
            var payMoney = $(this).closest("tr").find(".t_payMoney").val();
            var isbill = $(this).closest("tr").find(".sel_isbill").val();
            var staffname = $(this).closest("tr").find("[name='t_staffname']").val();
            var billno = $(this).closest("tr").find("[name='t_billno']").val();
            var desc = $(this).closest("tr").find("[name='t_desc']").val();
            var id="<%=Request.QueryString["id"] %>";
            var registid =$(this).closest("tr").find(".registid").val();
            
            if(!/^\d{4}-\d{1,2}-\d{1,2}$/.test(payDate))
            {
                alert("请填写正确的日期!");
                return false;
            }
            
            if(!/^\d+(\.\d+)?$/.test(payMoney))
            {
                alert("请填写正确的付款金额!");
                return false;
            }
            if(staffname.length==0)
            {
                alert("请填写付款人!");
                return false;
            }
            $.newAjax({
                type: "POST",
                dateType: "html",
                url: "/TeamPlan/Payment.aspx?act=update",
                data: {id:id,registid:registid,payDate:payDate,payType:payType,payMoney:payMoney,isbill:isbill,staffname:staffname,billno:billno,desc:desc},
                cache: false,
                success: function(html) {
                    alert(html);
                    location.href = location.href;
                },
                error: function() {
                    alert("保存出错!");
                    location.href = location.href;
                }
            });
        });


        $("#<%=LinkButton1.ClientID %>").click(function() {
            if (parseFloat($("#txt_fukuan").val()) + sum > money) {
                alert("付款总额不能超过应付金额!");
                return false;
            }

            return ValiDatorForm.validator($("#myform").get(0), "alert");
        });
        $(".selectTeam").click(function() {
            var url = $(this).attr("href");
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "选用组团社",
                modal: true,
                width: "820px",
                height: "520px"
            });
            return false;
            ///根据选择的组团社绑定报价等级
        });
    });
</script>

</body>
</html>
