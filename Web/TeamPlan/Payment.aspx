<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs"
    Inherits="Web.TeamPlan.Payment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>�ޱ����ĵ�</title>
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
                    ����ʱ��
                </th>
                <th width="125" bgcolor="#bddcf4" align="center">
                    ֧����ʽ
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    ������
                </th>
                <th width="125" bgcolor="#bddcf4" align="center">
                    �Ƿ�Ʊ
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    ������
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    ��Ʊ/�վݺ�
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    ��ע
                </th>
                <th width="80" bgcolor="#bddcf4" align="center">
                    ����
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
                                ����</a> <span style="display: <%#((bool)Eval("IsPay"))==false?"block":"none"%>">
                                    <%#((bool)Eval("IsChecked"))==true?"�����":""%></span><span><%#((bool)Eval("IsPay")) == true ? "��֧��" : ""%></span>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="even">
                <td height="30" align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*��ѡ������" valid="required"
                        id="txt_payDate" onfocus="WdatePicker();" class="searchinput"
                        name="txt_payDate">
                </td>
                <td align="center">
                    <font class="xinghao">*</font><asp:DropDownList errmsg="*��ѡ��֧����ʽ" valid="required"
                        ID="ddlPayType" runat="server">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*����д������|*����д��ȷ�ĸ�����" valid="required|isMoney"
                        id="txt_fukuan" class="searchinput" runat="server" />
                </td>
                <td align="center">
                    <font class="xinghao">*</font><select name="sel_is" id="sel_is"><option value="1">��</option>
                        <option value="2">��</option>
                    </select>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><input type="text" errmsg="*����д������" valid="required"
                        id="txt_Lxr" class="searchinput" name="txt_Lxr">
                </td>
                <td align="center">
                    <input type="text" id="txt_code" class="searchinput" name="txt_code" />
                </td>
                <td align="center">
                    <textarea rows="2" cols="20" id="txt_desc" name="txt_desc"></textarea>
                </td>
                <td align="center">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">����</asp:LinkButton>
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
                        id="linkCancel" href="javascript:;">�ر�</a>
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
                alert("����д��ȷ������!");
                return false;
            }
            
            if(!/^\d+(\.\d+)?$/.test(payMoney))
            {
                alert("����д��ȷ�ĸ�����!");
                return false;
            }
            if(staffname.length==0)
            {
                alert("����д������!");
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
                    alert("�������!");
                    location.href = location.href;
                }
            });
        });


        $("#<%=LinkButton1.ClientID %>").click(function() {
            if (parseFloat($("#txt_fukuan").val()) + sum > money) {
                alert("�����ܶ�ܳ���Ӧ�����!");
                return false;
            }

            return ValiDatorForm.validator($("#myform").get(0), "alert");
        });
        $(".selectTeam").click(function() {
            var url = $(this).attr("href");
            parent.Boxy.iframeDialog({
                iframeUrl: url,
                title: "ѡ��������",
                modal: true,
                width: "820px",
                height: "520px"
            });
            return false;
            ///����ѡ���������󶨱��۵ȼ�
        });
    });
</script>

</body>
</html>
