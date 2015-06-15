<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cleanuptourdata.aspx.cs"
    Inherits="Web.Webmaster._cleanuptourdata" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
        .trspace
        {
            height: 10px;
            font-size: 0px;
        }
        .note
        {
            color: #999;
            margin-left: 5px;
        }
        .required
        {
            color: #ff0000;
        }
        .unrequired
        {
            color: #fff;
        }
    </style>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        //数据清理确认
        function confirmCleanup() {
            //按钮对象
            var btnObject = $("#<%=btnCleanup.ClientID %>");
            //子系统公司编号
            var sysID = $("#<%=ddlSys.ClientID %>").val();
            //出团日期开始
            var lDate = $("#<%=txtLDate.ClientID %>").val();
            //出团日期结束
            var rDate = $("#<%=txtRDate.ClientID %>").val();
            if (sysID == "0") {
                alert("请选择子系统!");
                return false;
            }
            if (lDate == "" || rDate == "") {
                alert("请填写完整的出团日期!");
                return false;
            }
            if (confirm("你确认要清理吗?")) {
                $(btnObject).val("正在整理中...");
                return true;
            } else {
                return false;
            }
        }
    </script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    清除系统业务数据
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                子系统：<asp:DropDownList runat="server" ID="ddlSys">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                出团时间：
                <input type="text" onfocus="WdatePicker()" id="txtLDate" class="input_text" name="txtLDate"
                    style="width: 90px" runat="server" />
                ~
                <input type="text" onfocus="WdatePicker()" id="txtRDate" class="input_text" name="txtRDate"
                    style="width: 90px" runat="server" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCleanup" runat="server" Text="清理" 
                    OnClientClick="return confirmCleanup();" onclick="btnCleanup_Click1"/>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
    1.操作具体有不可逆性，请慎重。<br />
    2.操作前请联系技术人员做好数据备份，防止数据丢失。<br />
    3.该清除操作涉及到的数据有：散拼计划、团队计划、单项业务，以及与之关联的订单、收退款登记、地接安排、机票安排、支出登记、财务、统计分析等等相关数据。
</asp:Content>
