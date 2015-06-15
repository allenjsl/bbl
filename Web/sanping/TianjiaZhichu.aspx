<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TianjiaZhichu.aspx.cs"
    Inherits="Web.sanping.TianjiaZhichu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" src="/js/jquery.js"></script>

    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hd_addMoney" runat="server" />
    <input type="hidden" id="hd_subMoney" runat="server" />
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top: 10px;">
        <tr class="odd">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>付款日期：
            </th>
            <td width="283">
                <asp:TextBox ID="txt_date" class="searchinput" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});"
                    errmsg="*请输入付款日期！" valid="required" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>支出项目：
            </th>
            <td>
                <asp:TextBox ID="txt_ziqu" class="searchinput" errmsg="*请输入支出项目!" valid="required"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="odd">
            <th width="117" height="35" align="right">
                客户单位：
            </th>
            <td>
                <input type="hidden" id="hd_teamId" name="hd_teamId" />
                <input type="text" id="txt_com" runat="server" class="searchinput searchinput02"
                    name="txt_com" /><a href="/CRM/customerservice/SelCustomer.aspx?method=selectTeam"
                        class="selectTeam"><img src="../images/sanping_04.gif" width="28" height="18"></a>
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>付款人：
            </th>
            <td>
                <asp:TextBox ID="txt_sendUser" class="searchinput" errmsg="*请输入付款人!" valid="required"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="odd">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>支出金额：
            </th>
            <td>
                <input name="txt_money" type="text" class="searchinput" errmsg="*请输入支出金额!|*请输入正确的格式!"
                    runat="server" valid="required|isMoney" id="txt_money" />
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="40" align="right">
                备注：
            </th>
            <td>
                <textarea class="textareastyle02" id="txt_Remarks" name="txt_Remarks" runat="server"></textarea>
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="40" align="right">
                <font class="xinghao">*</font>支出方式：
            </th>
            <td>
                <asp:DropDownList ID="ddlPayType" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="40" align="right">
                付款状态：
            </th>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Value="True">
                
                已付</asp:ListItem>
                    <asp:ListItem Value="false" Selected="True">未付</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="center">
                <table width="200" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="40" align="center">
                        </td>
                        <td height="40" align="center" class="tjbtn02">
                            <asp:LinkButton ID="lbtn_Submit" runat="server" OnClick="lbtn_Submit_Click">保存</asp:LinkButton>
                        </td>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();return false;">
                                取消</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">

        //计算长度
        String.prototype.len = function() { return this.replace(/[^\x00-\xff]/g, "**").length; }

        function selectTeam(id, name) {
            $("#hd_teamId").val(id);
            $("#txt_com").val(name);
        }
        $(function() {

            $(".selectTeam").click(function() {
                var url = $(this).attr("href");
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选用客户单位",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: iframeId,
                        backfun: "selectTeam"
                    }
                });
                return false;

                ///根据选择的组团社绑定报价等级
            });
            function FloatAdd(arg1, arg2) {
                var r1, r2, m;
                try { r1 = arg1.toString().split(".")[1].length } catch (e) { r1 = 0 }
                try { r2 = arg2.toString().split(".")[1].length } catch (e) { r2 = 0 }
                m = Math.pow(10, Math.max(r1, r2))
                return (arg1 * m + arg2 * m) / m
            }


            $("#<%=lbtn_Submit.ClientID %>").click(function() {
                var form = $(this).closest("form").get(0);
                if ($("#txt_Remarks").val().len() > 500) {
                    alert("备注长度不得超过500个字节!");
                    return false;
                }
                if ($("#<%=ddlPayType.ClientID %>").val() == "0") {
                    alert("请选择支付方式");
                    return false;
                }

                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=lbtn_Submit.ClientID %>").closest("form").get(0));
        });
        
        <%if (!CheckGrant(Common.Enum.TravelPermission.财务管理_杂费支出_支出审核)){%>
        //设置下拉菜单为只读
        function setSelectReadOnly(selectObj, disable) {
            var length = selectObj.options.length
            var selectedIndex = selectObj.selectedIndex;            
            for (var i = 0; i < length; i++) {
                if (i != selectedIndex) {
                    selectObj.options[i].disabled = disable;
                }
            }
        }

        $(document).ready(function() {
            setSelectReadOnly($("#<%=ddlStatus.ClientID %>")[0], true);
        }); 
        <%} %>  
    </script>

</body>
</html>
