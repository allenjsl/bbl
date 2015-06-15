<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cashier_Register.aspx.cs" Inherits="Web.caiwuguanli.dengjiChuLa" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="/js/jquery.js"></script>
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="500" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top: 10px;">
        <tr class="even">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>到款时间：
            </th>
            <td width="283">
                <asp:TextBox ID="txt_date" onfocus="WdatePicker();" class="searchinput" errmsg="*请输入付款日期！" valid="required"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="odd">
            <th width="117" height="35" align="right">
                客户单位：
            </th>
            <td>

                <input type="hidden" name="hd_teamId" id="hd_teamId" value="" /><input type="text"  class="searchinput" id="txt_teamName" name="txt_teamName" errmsg="*请选择客户单位！"/><a href="/CRM/customerservice/SelCustomer.aspx?method=selectTeam" class="selectTeam"><IMG src="../images/sanping_04.gif" width=28 height=18></a>
            </td>
        </tr>
        <tr class="even">
            <th width="117" height="35" align="right">
                联系人：
            </th>
            <td>
                <asp:TextBox ID="txt_sendUser" class="searchinput" errmsg="*请输入联系人!"
                    runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr class="odd">
            <th width="117" height="35" align="right">
                <font class="xinghao">*</font>到款金额：             </th>
            <td>
                <input name="txt_money" type="text" class="searchinput" runat="server" id="txt_money"  errmsg="*请输入支出金额!" valid="required" />
            </td>
        </tr>
        <tr class="even"><th width="117" align="right">
        到款银行：
        </th><td>
        <input type="text" class="searchinput" id="txt_bank" name="txt_bank"/>
        </td></tr>
        <tr class="odd">
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
                  <asp:DropDownList ID="ddlPayType" runat="server"  errmsg="*请选择支出方式！" valid="required">
            </asp:DropDownList>
            </td>
        </tr>
        <tr class="odd">
            <th width="117" height="40" align="right">
                电话：
            </th>
          
            <td>
        <input type="text" class="searchinput" name="txt_phone" id="txt_phone"/>
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

    <script>

        function selectTeam(id, name) {
            $("#hd_teamId").val(id);
            $("#txt_teamName").val(name);
        }
        
        $(function() {
            $("#txt_teamName").click(function(){
                $(".selectTeam").click();
            });
            $(".selectTeam").click(function() {
                var url = $(this).attr("href");
                parent.Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选用客户单位",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: "<%=Request.QueryString["iframeId"] %>",
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
                //点击按纽触发执行的验证函数
                return ValiDatorForm.validator(form, "alert");
            });
            //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
            FV_onBlur.initValid($("#<%=lbtn_Submit.ClientID %>").closest("form").get(0));
        });
    </script>

</body>
</html>
