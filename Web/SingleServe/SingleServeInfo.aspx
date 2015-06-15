<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingleServeInfo.aspx.cs"
    Inherits="Web.SingleServe.SingleServeInfo" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新增特服</title>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/jquery.js"></script>

    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>

    <script type="text/javascript" src="/js/loadVisitors.js"></script>

    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .errmsg
        {
            color: red;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="1">
        <tr class="odd">
            <th align="center">
                订单号：
            </th>
            <td align="center">
                <input id="txtOrderNo" name="txtOrderNo" disabled="true" type="text" runat="server" />
            </td>
            <th align="center">
                <span class="errmsg">*</span>销售员：
            </th>
            <td align="center">
                <uc2:selectOperator ID="selectOperator1" IsMulutSelec="false" ValidMsg="*请选择销售员！"
                    IsValid="true" runat="server" />
            </td>
            <th align="center">
                <span class="errmsg">*</span> 客户单位：
            </th>
            <td align="left" colspan="2">
                &nbsp;<input id="txtCustomerCompany" runat="server" onfocus="CheckCustomerCompany()"
                    name="txtCustomerCompany" type="text" errmsg="*请选择客户单位！" valid="required" />
                <a href="javascript:;" onclick="return CheckCustomerCompany();">
                    <img src="/images/sanping_04.gif" width="28" height="18" /></a>
                <asp:HiddenField ID="hfSelectCustId" runat="server" />
            </td>
        </tr>
        <tr class="even">
            <th align="center">
                &nbsp;<span class="errmsg">*</span>委托日期&nbsp;
            </th>
            <td align="center">
                <input id="TxtStartTime" name="TxtStartTime" type="text" runat="server" valid="required"
                    onfocus="WdatePicker({onpicked:getTourCode});" errmsg="*请选择委托日期!" />
            </td>
            <th align="center">
                &nbsp;<span class="errmsg">*</span>团号：
            </th>
            <td align="center">
                <input id="txtTourCode" name="txtTourCode" type="text" runat="server" valid="required"
                    errmsg="*请输入团号!" />
            </td>
            <th align="center">
                联系人：&nbsp;
            </th>
            <td align="left" colspan="2">
                &nbsp;<input id="txtContactName" name="txtContactName" type="text" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th align="center">
                &nbsp; 电话：
            </th>
            <td align="left" colspan="6">
                &nbsp;
                <input id="txtTel" type="text" runat="server" name="txtTel" />
            </td>
        </tr>
        <tr class="even" id="GuestRequestTR">
            <th align="center">
                客户要求：
            </th>
            <th align="center">
                项目
            </th>
            <th align="center">
                具体要求
            </th>
            <th align="center">
                价格
            </th>
            <th>
                备注
            </th>
            <td align="center" colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr class="odd" name="tr_GuestRequest">
            <th align="center">
                &nbsp;
            </th>
            <td align="center">
                <asp:DropDownList ID="ddlGuestList" runat="server">
                </asp:DropDownList>
            </td>
            <td align="left">
                <textarea name="txtGuestRequest"></textarea>
            </td>
            <td align="center">
                <input name="txtGPrice" maxlength="10" onblur="InitTotalIncome()" type="text" />
            </td>
            <td>
                <textarea id="txtGuestRemark" name="txtGuestRemark"></textarea>
            </td>
            <td align="center" colspan="2">
                <a href="javascript:" onclick="AddGuestRequest(this)">
                    <img src="/images/tianjiaicon01.gif" style="width: 15; height: 16px;" />添加 </a>
                <a href="javascript:" onclick="RemoveGuestRequest(this)">
                    <img src="/images/delicon01.gif" style="width: 14px; height: 14px;" />删除 </a>
            </td>
        </tr>
        <tr class="even">
            <th align="center">
                <span class="errmsg">*</span> 合计收入：
            </th>
            <td colspan="6" align="left">
                <input name="txtTotalIncome" maxlength="20" valid="required|isMoney|custom" errmsg="*请输入合计收入金额!|*请输入合法的合计收入金额!"
                    runat="server" type="text" id="txtTotalIncome" custom="InitGrossProfit" value="" />
            </td>
        </tr>
        <tr class="odd">
            <th align="center">
                <span class="errmsg">*</span>人数：
            </th>
            <td colspan="6" align="left">
                <input id="txtPersonNum" name="txtPersonNum" maxlength="3" runat="server" type="text"
                    value="" errmsg="*请输入人数！|*请设置正确的人数！" valid="required|RegInteger" />
            </td>
        </tr>
        <tr class="even">
            <th height="30" rowspan="2" align="center">
                游客信息：
            </th>
            <td height="30" colspan="6" align="center">
                <table width="50%" border="0" align="right" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="80%" align="center" class="updom">
                            <div id="divfileinfo">
                                <a runat="server" href="" id="aFile">查看附件</a>
                                <img style="cursor: pointer" src="/images/fujian_x.gif" width="14" height="13" id="ImgClose"
                                    class="close" alt="删除" />
                                <asp:HiddenField runat="server" ID="hFileInfo" />
                            </div>
                            <div id="divFileUpload" style="display: none;">
                                上传附件：<input type="file" runat="server" name="fuiLoadAttachment" id="fuiLoadAttachment" />
                            </div>
                        </td>
                        <td width="20%" align="left">
                            <uc1:LoadVisitors ID="LoadVisitors1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <td colspan="6" align="center">
                <table width="90%" border="0" align="center" id="tblVisitorList" cellpadding="0"
                    cellspacing="1" bgcolor="#BDDCF4" style="margin: 10px 0;">
                    <tr>
                        <td height="5%" align="center" bgcolor="#E3F1FC">
                            编号
                        </td>
                        <td height="25" align="center" bgcolor="#E3F1FC">
                            姓名
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            类型
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件名称
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            证件号码
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            性别
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            联系电话
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            特服
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            操作
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="even" id="SupplierTR">
            <th align="center">
                供应商安排：
            </th>
            <th align="center">
                服务类别
            </th>
            <th align="center">
                供应商名称
            </th>
            <th align="center">
                具体安排
            </th>
            <th align="center">
                结算费用
            </th>
            <th align="center">
                备注
            </th>
            <td align="center" width="85">
                &nbsp;
            </td>
        </tr>
        <tr class="odd" name="tr_SupplierObj">
            <th align="center">
                <input type="hidden" name="hPlanID" value="0" />
            </th>
            <td align="center">
                <asp:DropDownList ID="ddlSupperlierList" runat="server">
                </asp:DropDownList>
            </td>
            <td align="left">
                <input name="txtSupperName" type="text" readonly="readonly" /><input type="hidden"
                    name="hSupperID" value="0" />
                <a href="javascript:;" name="ASel" onclick="return CheckCustomer(this);">
                    <img src="/images/sanping_04.gif"></a>
            </td>
            <td align="left">
                <textarea name="txtSupplierRequest"></textarea>
            </td>
            <td align="left">
                <input name="txtAccountCost" maxlength="10" onblur="InitTotalOutlay()" type="text" />
            </td>
            <td align="center">
                <textarea id="txtSupRemark" name="txtSupRemark"></textarea>
            </td>
            <td align="center">
                <a href="javascript:" onclick="AddSupplierPlan(this)">
                    <img src="/images/tianjiaicon01.gif" style="width: 15; height: 16px;" />添加</a>
                <a href="javascript:" onclick="RemoveSupplierObj(this)">
                    <img src="/images/delicon01.gif" style="width: 14px; height: 14px;" />删除</a>
                <a href="javascript:void(0);" class="openConfrim" name="openConfrim">确认单</a>
            </td>
        </tr>
        <tr class="even">
            <th align="center">
                <span class="errmsg">*</span> 合计支出：
            </th>
            <td colspan="6" align="left">
                <input name="txtTotalOutlay" maxlength="20" valid="required|isMoney|custom" errmsg="*请输入合计支出金额!|*请输入合法的合计支出金额!"
                    readonly="readonly" style="background-color: #dadada;" type="text" runat="server"
                    id="txtTotalOutlay" custom="InitGrossProfit" />
            </td>
        </tr>
        <tr class="odd">
            <th align="center">
                <span class="errmsg">*</span>毛利：
            </th>
            <td colspan="6" align="left">
                <input name="txtGrossProfit" maxlength="20" valid="required|isNumber" errmsg="*请输入毛利金额!|*请输入合法的毛利金额!"
                    type="text" runat="server" id="txtGrossProfit" />
            </td>
        </tr>
        <tr>
            <th colspan="6" align="center">
                <table width="320" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="40" align="center">
                            <asp:HiddenField ID="hfOBuyerId" runat="server" />
                            <asp:HiddenField ID="hfSaler" runat="server" />
                        </td>
                        <%if (!IsChecked && !IsChecked1)
                          { %>
                        <td height="40" align="center" class="tjbtn02">
                            <input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                            <asp:LinkButton ID="lbtnSave" runat="server" Text="保存" OnClientClick="return CheckTeamNum()"
                                OnClick="lbtnSave_Click"></asp:LinkButton>
                        </td>
                        <%} %>
                        <%if (!string.IsNullOrEmpty(EditId) && !IsChecked && !IsChecked1)
                          { %>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return SubmitFinance();">提交财务</a>
                        </td>
                        <%} %>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()">
                                关闭</a>
                        </td>
                    </tr>
                </table>
            </th>
        </tr>
    </table>
    <asp:HiddenField ID="hideOldTeamNum" runat="server" />
    </form>
</body>
</html>

<script type="text/javascript">
    var query = function(key, str) {
        var uri = str;
        var val = key;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    };
//    $(function(){
//        $("#<%=txtTourCode.ClientID %>").blur(function(){
//            var teamNum = $("#<%=txtTourCode.ClientID %>").val();
//        //判断团号是否存在
//            var bool = false;
//            $.newAjax({
//                type: "Get",
//                url: "/TeamPlan/AjaxFastVersion.ashx?type=CheckTeamNum&teamNum=" + teamNum+"&id="+<%=EditId %>,
//                cache: false,
//                success: function(result) {
//                    if (result != "OK") {
//                       alert("该团号已经存在!");
//                       //$("#<%=txtTourCode.ClientID %>").focus();
//                    }
//                }
//            });
//        });
//    });
    function getTourCode() {
        if ($("#<%=TxtStartTime.ClientID %>").val() == "") {
            alert("请选择出团日期!");
            $("#<%=TxtStartTime.ClientID %>").focus();
            return false;
        }
        else {
            $.newAjax({
                type: "POST",
                url: "/ashx/GenerateTourNumbers.ashx?companyId=<%=SiteUserInfo.CompanyID%>",
                async: false,
                data: {
                    datestr: $("#<%=TxtStartTime.ClientID %>").val()
                },
                success: function(data) {
                    if (data != "") {
                        var resultArr = [];
                        var arr = eval(data);
                        for (var i = 0; i < arr.length; i++) {
                            $("#<%=txtTourCode.ClientID %>").val(arr[i][1]);
                        }
                    }
                }
            });
        }
    }
    $(function() {

        if ($.trim($("#<%= aFile.ClientID %>").attr("href")) != "") {
            $("#ImgClose").click(function() {
                $("#divFileUpload").show();
                $("#divfileinfo").hide();
                $("#<%= hFileInfo.ClientID %>").val("");
                return false
            });
        }
        else {
            $("#divFileUpload").show();
            $("#divfileinfo").hide();
        }
        $("#<%=lbtnSave.ClientID %>").click(function() {
            var operaterId = <%=selectOperator1.ClientID %>.GetOperatorId();
            $("#<%=hfSaler.ClientID %>").val(operaterId);
            var form = $(this).closest("form").get(0);
            //点击按纽触发执行的验证函数
            var flag = ValiDatorForm.validator(form, "alert");
            if (flag) {
                $("input[name='tefu']").each(function() {
                    var ServiceStr = $(this).val();
                    if ($.trim(ServiceStr) == "")
                        return false;
                    var txtItem = query("txtItem", ServiceStr);
                    var txtServiceContent = query("txtServiceContent", ServiceStr);
                    var ddlOperate = query("ddlOperate", ServiceStr);
                    var txtCost = query("txtCost", ServiceStr);
                    if (txtItem != null) {
                        txtItem = decodeURIComponent(txtItem);
                    }
                    if (txtServiceContent != null) {
                        txtServiceContent = decodeURIComponent(txtServiceContent);
                    }
                    if (txtCost != null) {
                        txtCost = decodeURIComponent(txtCost);
                    }
                    $(this).val(txtItem + "$" + txtServiceContent + "$" + txtCost + "$" + ddlOperate);
                });
            }
            return flag;
        });
        
        
        //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
        
        FV_onBlur.initValid($("#tblVisitorList").closest("form").get(0));

        var VisitorData = null; //游客信息数组
        
        
        if ("<%= IsUpdate %>".toLowerCase() == "true") {
            InitService();
            VisitorData = "<%= VisitorArr %>";
            
            VisitorData = eval(VisitorData);
            if (VisitorData.length == 0)
                VisitorData = null;
        }
        
        //供应商打开确认单
        $(".openConfrim").click(function(){
            var planId = $(this).attr("ref");
            window.open('/print/wh/SingleServerPrint.aspx?tourid=<%=Request.QueryString["EditId"]%>&planId='+planId);
            //EditId
        })
        
        loadVisitors.init({ data: VisitorData, autoComputeToTalAmountHandle: function() { } });

    })
    ///添加客人要求////
    function AddGuestRequest(guestObj) {
        var newRow = $(guestObj).parent().parent().clone(); //克隆一行
        newRow.find("select[id='<%= ddlGuestList.ClientID %>']").val("0");
        newRow.find("textarea[name='txtGuestRequest']").val("");
        newRow.find("input[name='txtGPrice']").val("");
        newRow.find("textarea[name='txtGuestRemark']").val("");
        $("tr[name='tr_GuestRequest']:last").after(newRow); //添加一行
    }
    ///////删除////
    function RemoveGuestRequest(guestObj) {
        var guestArray = $("tr[name=tr_GuestRequest]").length;
        if (guestArray > 1) {
            $(guestObj).parent().parent().remove();
            InitTotalIncome();
            return true;
        }
        else {
            alert("客人要求至少保留一行！");
            return false;
        }
        return true;

    }
    ////////供应商安排//
    function AddSupplierPlan(supplierObj) {
        var newRow = $(supplierObj).parent().parent().clone(); //克隆一行
        
        newRow.find("select[id='<%= ddlSupperlierList.ClientID %>']").val("0");
        newRow.find("input[name='hPlanID']").val("0");
        newRow.find("input[name='hSupperID']").val("0");
        newRow.find("input[name='txtSupperName']").val("").removeAttr("disabled");
       // newRow.find(":a[name='ASel']").hide();
        newRow.find("textarea[name='txtSupplierRequest']").val("");
        newRow.find("input[name='txtAccountCost']").val("");
        newRow.find("textarea[name='txtSupRemark']").val("");
        newRow.find("a[name='openConfrim']").remove();
        $("tr[name='tr_SupplierObj']:last").after(newRow); //添加一行
    }
    ///////删除////
    function RemoveSupplierObj(supplierObj) {
        var supplierArray = $("tr[name=tr_SupplierObj]").length;
        if (supplierArray > 1) {
            $(supplierObj).parent().parent().remove();
            InitTotalOutlay();
            return true;
        }
        else {
            alert("供应商安排至少保留一行！");
            return false;
        }
        return true;
    }
    ///根据人数动态创建游客信息
    function InitGuest(obj, frm) {
        var CurrGuestCount = $("#tblVisitorList").find("tr:gt(0)").length;
        var GuestCount = $(obj).val();
        if (parseInt(GuestCount) <= 0) {
            $(obj).val(CurrGuestCount);
            return false;
        }
        if (CurrGuestCount < GuestCount) {
            for (var i = parseInt(CurrGuestCount, 10); i < parseInt(GuestCount, 10); i++) {
                loadVisitors.addVisitor();
            }
        }
        else {
            $("#tblVisitorList").find("tr:gt(" + GuestCount + ")").remove();
        }
        return true;
    }
    ///动态计算所有支出
    function InitTotalOutlay(obj, frm) {
        var TotalOutlay = 0;
        $("input[name='txtAccountCost']").each(function() {
            if (RegExps.isMoney.test($(this).val()))
                TotalOutlay += parseFloat($(this).val());
        });
        $("#<%= txtTotalOutlay.ClientID %>").val(ForDight(TotalOutlay, 2));
        InitGrossProfit()
        return true;
    }
    ///动态计算所有收入
    function InitTotalIncome(obj, frm) {
        var TotalIncome = 0;
        $("input[name='txtGPrice']").each(function() {
            if (RegExps.isMoney.test($(this).val()))
                TotalIncome += parseFloat($(this).val())
        });
        /*--------计算特服信息 开始-------*/
        $("input[name='tefu']").each(function() {
            var ServiceStr = $(this).val();
            if ($.trim(ServiceStr) == "")
                return false;
            var ddlOperate = query("ddlOperate", ServiceStr);
            var txtCost = query("txtCost", ServiceStr);
            if (ddlOperate == "0")
                TotalIncome += parseFloat(txtCost);
            else
                TotalIncome -= parseFloat(txtCost);
        });
        /*------计算特服信息 结束--------*/
        $("#<%= txtTotalIncome.ClientID %>").val(ForDight(TotalIncome, 2));

        InitGrossProfit()
        return true;
    }
    ///动态计算毛利
    function InitGrossProfit() {
        var TotalOutlaySum = $("#<%= txtTotalOutlay.ClientID %>").val(); //所有支出
        var TotalIncomeSum = $("#<%= txtTotalIncome.ClientID %>").val(); //所有收入
        TotalOutlaySum = $.trim(TotalOutlaySum) == "" ? 0 : TotalOutlaySum;
        TotalIncomeSum = $.trim(TotalIncomeSum) == "" ? 0 : TotalIncomeSum;
        if (RegExps.isMoney.test(TotalOutlaySum) && RegExps.isMoney.test(TotalIncomeSum)) {
            $("#<%= txtGrossProfit.ClientID %>").val(ForDight(parseFloat(TotalIncomeSum) - parseFloat(TotalOutlaySum), 2));
        }
        return true;
    }
    ///----修改时初始化"客户要求"与"供应商安排列表"
    function InitService() {
        var SupperListXML = '<%= SupplierJSON %>'; //供应商安排列表
        var GuestRequest = <%= GuestRequestJSON %>; //客户要求列表
        if (SupperListXML != "") {
            SupperListXML = eval(SupperListXML);
        }
        if (SupperListXML.length > 0) {
            for (var i = 0; i < SupperListXML.length; i++) {
                var supplierDOM = $("tr[name=tr_SupplierObj]").eq(0).clone();
                var options = SupperListXML[i];
                supplierDOM.find(":a[name='ASel']").hide();
                supplierDOM.find("input[name='txtSupperName']").removeAttr("readonly");
                supplierDOM.find("select[id='<%= ddlSupperlierList.ClientID %>']").val(options["ServiceType"]);
                supplierDOM.find("input[name='hPlanID']").val(options["PlanId"]);
                supplierDOM.find("input[name='hSupperID']").val(options["SupplierId"]);
                supplierDOM.find("input[name='txtSupperName']").val(options["SupplierName"]);
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.保险 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.地接 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.购物 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.景点 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.酒店 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.其它 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.小交通 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                } if (options["ServiceType"] == "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.用餐 %>") {
                    supplierDOM.find("input[name='txtSupperName']").attr("readonly", true);
                    supplierDOM.find(":a[name='ASel']").show();
                }
                supplierDOM.find("textarea[name='txtSupplierRequest']").val(options["Arrange"]);
                supplierDOM.find("input[name='txtAccountCost']").val(options["Amount"]).blur(function() { InitTotalOutlay(); });
                supplierDOM.find("textarea[name='txtSupRemark']").val(options["Remark"]);
                supplierDOM.find("a[name='openConfrim']").attr("ref",options["PlanId"]);
                $("#SupplierTR").after(supplierDOM);
            }
            $("tr[name=tr_SupplierObj]:last").remove()
        }
        if (GuestRequest.length > 0) {
            for (var i = 0; i < GuestRequest.length; i++) {
                var guestDOM = $("tr[name=tr_GuestRequest]").eq(0).clone();
                var options = GuestRequest[i];
                guestDOM.find("select[id='<%= ddlGuestList.ClientID %>']").val(options["ServiceType"]);
                guestDOM.find("textarea[name='txtGuestRequest']").val(options["Requirement"]);
                guestDOM.find("input[name='txtGPrice']").val(options["SelfPrice"]).blur(function() { InitTotalIncome(); });
                guestDOM.find("textarea[name='txtGuestRemark']").val(options["Remark"]);
                $("#GuestRequestTR").after(guestDOM);
            }
            $("tr[name=tr_GuestRequest]:last").remove();
        }
    }
    ///----供应商安排"服务类别"点击事件
    function ChecklocaCustomer(obj) {
   
        switch ($(obj).val()) {
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.保险 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.地接 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.购物 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.景点 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.酒店 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.其它 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.小交通 %>":
            case "<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.用餐 %>":
                $(obj).parent().parent().find(":a[name='ASel']").show();
                $(obj).parent().parent().find("input[name='txtSupperName']").val("");
                $(obj).parent().parent().find("input[name='txtSupperName']").attr("readonly", true);
                break;
            default:
                $(obj).parent().parent().find(":a[name='ASel']").hide();
                $(obj).parent().parent().find("input[name='txtSupperName']").removeAttr("readonly");
                $(obj).parent().parent().find("input[name='hSupperID']").val("0");
                break;
        }
    }
    ///------选择供应商
    function CheckCustomer(obj) {
        var sType="<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.地接 %>";
        if( parseInt( $(obj).parent().prev().find("select").val()) >=0)
        {
            sType = $(obj).parent().prev().find("select").val();
        }
        
        var url = "/TeamPlan/DiJieList.aspx?sType="+sType;
        var iframeId = '<%=Request.QueryString["iframeId"] %>';
        parent.Boxy.iframeDialog({ title: "选择供应商", iframeUrl: url,
            width: 800, height: 400, model: true, data: {
                desid: iframeId, CBackFun: "SetCustomer", index: $(":a[name='ASel']").index($(obj))
            }
        });
    }
    //---------选择供应商回调函数
    function SetCustomer(CompanyName, CompanyId, ControlIndex) {
        $("input[name='txtSupperName']").eq(ControlIndex).val(CompanyName);
        $("input[name='hSupperID']").eq(ControlIndex).val(CompanyId);
    }
    ///------选择客户
    function CheckCustomerCompany(obj) {
        var url = "/CRM/customerservice/SelCustomer.aspx";
        var iframeId = '<%=Request.QueryString["iframeId"] %>';
        parent.Boxy.iframeDialog({ title: "选择客户单位", iframeUrl: url,
            width: 800, height: 400, model: true, data: {
                desid: iframeId, backFun: "SetpIdAndNameCompany"
            }
        });
    }
    //---------选择客户回调函数
    function SetpIdAndNameCompany(cid, cname) {
        $("#<%=hfSelectCustId.ClientID %>").val(cid);
        $("#txtCustomerCompany").val(cname);
        
        //获得联系人电话
        $.newAjax
                ({
                    type: "POST",
                    url: "/SingleServe/SingleServeInfo.aspx?type=getInfo&cid="+cid,
                    cache: false,
                    success: function(html) {
                      if(html!="")
                      {
                        $("#<%=txtContactName.ClientID %>").val(html.split("|")[0]);
                         $("#<%=txtTel.ClientID %>").val(html.split("|")[1]);
                      }
                    }
                });
    }
    function ForDight(Dight, How) {
        Dight = Math.round(Dight * Math.pow(10, How)) / Math.pow(10, How);
        return Dight;
    }

    //---------提交财务
    function SubmitFinance() {

        if (confirm('您确定要提交财务吗？')) {
            $.newAjax
                ({
                    type: "POST",
                    url: "/TeamPlan/AjaxTeamSettle.ashx?type=TiJiao&tourId=<%=EditId %>",
                    cache: false,
                    success: function(html) {
                        if (html == "1") {
                            alert("提交财务成功！");     
                            location.href = "/SingleServe/SingleServeList.aspx";
                            window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide();
                            //window.parent.
                        } else {
                            alert("提交财务失败");
                            return false;
                        }
                    },
                    error: function() {
                        alert("对不起，操作失败！");
                    }
                });
            return false;
        }
    }

    function CheckTeamNum() {
        var msg="";
        var teamNum = $("#<%=txtTourCode.ClientID %>").val();
        //判断团号是否存在
        if (teamNum != "" && teamNum != $("#<%=hideOldTeamNum.ClientID %>").val()) {
//            if (!AjaxCheckTeamNum()) {
//                msg+="该团号已经存在! \n";
//                return false;
//            }
        }
           var hd_IsRequiredTraveller = $("#hd_IsRequiredTraveller").val();
            //游客验证（hd_IsRequiredTraveller是否验证根据配置false时后面的参数允许为""，txtVisitorName姓名框name，ddlCardType=证件类型name，txtContactTel=电话框name）
            var msg = visitorChecking.isChecking(hd_IsRequiredTraveller, "txtVisitorName", "ddlCardType", "txtContactTel","txtCardNo");
            if (!msg.isYes) {
                alert(msg.msg);
                return false
            }
        return true;
    }

    function AjaxCheckTeamNum() {
        var teamNum = $("#<%=txtTourCode.ClientID %>").val();
        //判断团号是否存在
        var bool = false;
        $.newAjax({
            type: "Get",
            async: false,
            url: "/TeamPlan/AjaxFastVersion.ashx?type=CheckTeamNum&teamNum=" + teamNum,
            cache: false,
            success: function(result) {
                if (result == "OK") {
                    bool = true;
                }
            }
        });
        return bool;
    }
    //根据用户输入的身份证号判断性别
    function getSex(obj)
    {
        var val=$(obj).val();
        var tr =$(obj).parent().parent();
        var sex=tr.children().children("select[name='ddlSex']");
        var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                        if (isIdCard.exec(val)) {
                    if (15 == val.length) {// 15位身份证号码
                        if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                            sex.val(2);
                        else
                            sex.val(1);
                    }

                    if (18 == val.length) {// 18位身份证号码
                        if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                            sex.val(2);
                        else
                            sex.val(1);
                    }
                } else {
                sex.val(0);
                }
    }

</script>

