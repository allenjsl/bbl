<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systemadd.aspx.cs" Inherits="Web.Webmaster.systemadd"
    MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
    .trspace{height: 10px;font-size: 0px;}
    .note{color: #999;margin-left: 5px;}
    .required{color: #ff0000;}
    .unrequired{color: #fff;}
    ul{list-style: none;margin: 0px;padding: 0px;}
    ul li{list-style: none;}
    .pBig{font-weight: bold;line-height: 30px;font-size: 14px;clear: both;margin-top: 10px;background: #eee;}
    .pSmall{float: left;width: 24%;}
    .pSmall li{line-height: 22px;}
    .pSmall li.pSmallTitle{font-weight: bold;line-height: 24px;}
    .pSmallSpace{clear: both;width: 100%;height: 10px;}
    .pno{color: #ff0000;font-weight: normal;}
    .lh24{line-height: 24px;}
    </style>

    <script type="text/javascript" src="/js/webmaster.core.js?v=1"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            addSysDomain(true, '', '');
            //addPrintDocument(true, 0, '');
            initSysSetting(true);

            //栏目(FIRST)checkbox添加事件，全选或取消子栏目及所有权限
            $(".pBig input[type='checkbox']").bind("click", function() {
                $(this).parent().next().find("input[type='checkbox']").attr("checked", this.checked);
            });

            //子栏目(SECOND)checkbox添加事件，全选或取消全选所有权限，选中后并选中栏目
            $(".pSmallTitle input[type='checkbox']").bind("click", function() {
                $(this).parent().parent().find("input[type='checkbox']").attr("checked", this.checked);
                if (this.checked) {
                    $(this).parent().parent().parent().prev().find("input[type='checkbox']").attr("checked", true);
                }
            });

            //权限(THIRD)checkbox添加事件，选中后选中子栏目及栏目
            $(".pThird input[type='checkbox']").bind("click", function() {
                if (!this.checked) return;
                $(this).parent().parent().find("li:eq(0)").find("input[type='checkbox']").attr("checked", true);
                $(this).parent().parent().parent().prev().find("input[type='checkbox']").attr("checked", true);
            });

        });
    </script>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    添加子系统
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                <span class="required">*</span>系统名称：<input type="text" id="txtSysName" name="txtSysName"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>公司名称：<input type="text" id="txtCompanyName" name="txtCompanyName"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系姓名：<input type="text" id="txtRealname" name="txtRealname"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系电话：<input type="text" id="txtTelephone" name="txtTelephone"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系手机：<input type="text" id="txtMobile" name="txtMobile"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td>
                <span class="unrequired">*</span>联系传真：<input type="text" id="txtFax" name="txtFax"
                    class="input_text" maxlength="72" style="width: 520px" />
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>登录账号：<input type="text" id="txtUsername" name="txtUsername"
                    class="input_text" maxlength="72" style="width: 200px" value="admin" /><span class="note">系统管理员账号</span>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>登录密码：<input type="password" id="txtPassword" name="txtPassword"
                    class="input_text" maxlength="72" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>总部名称：<input type="text" id="txtDepartmentName" name="txtDepartmentName"
                    class="input_text" maxlength="72" style="width: 520px" value="总部" />
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="note">域名格式：bbl.gocn.cn，域名若要指向同行入口，请参考<a href="同行平台模板说明.htm" target="_blank">同行平台模板说明</a>。专线入口无需做跳转路径设置。</span>
            </td>
        </tr>
        <!--专线域名区域-->
        <tr id="trDomainAfter">
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>合同到期提醒配置：合同到期提前<input type="text" id="txtContractReminderDays"
                    name="txtContractReminderDays" class="input_text" maxlength="72" style="width: 50px"
                    value="30" />天提醒
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>出团及回团提醒配置：出团提前<input type="text" id="txtLeaveTourReminderDays"
                    name="txtLeaveTourReminderDays" class="input_text" maxlength="72" style="width: 50px"
                    value="3" />天提醒，回团提前<input type="text" id="txtBackTourReminderDays" name="txtBackTourReminderDays"
                        class="input_text" maxlength="72" style="width: 50px" value="3" />天提醒
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>最长留位时间配置：最长留位<input type="text" id="txtReservationTime"
                    name="txtReservationTime" class="input_text" maxlength="72" style="width: 50px"
                    value="1440" />分钟
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>列表默认显示数据配置：列表默认显示前<input type="text" id="txtDisplayBeforeMonth"
                    name="txtDisplayBeforeMonth" class="input_text" maxlength="72" style="width: 50px"
                    value="3" />月数据，后<input type="text" id="txtDisplayAfterMonth" name="txtDisplayAfterMonth"
                        class="input_text" maxlength="72" style="width: 50px" value="3" />月数据
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>价格组成配置：<input type="radio" name="radPriceComponent"
                    id="radPriceComponent0" checked="checked" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价 %>" /><label
                        for="radPriceComponent0">分项报价</label>
                <input type="radio" name="radPriceComponent" id="radPriceComponent1" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.统一报价 %>" /><label
                    for="radPriceComponent1">统一报价</label>
            </td>
        </tr>
        <tr>
            <td class="lh24">
                <span class="required">*</span>机票票款计算公式：<br />
                <input type="radio" name="radAgencyFee" id="radAgencyFee0" checked="checked" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一 %>" /><label
                    for="radAgencyFee0">公式一 (票面价+机建燃油)*人数+代理费</label><br />
                <input type="radio" name="radAgencyFee" id="radAgencyFee1" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二 %>" /><label
                    for="radAgencyFee1">公式二 (票面价+机建燃油)*人数-代理费</label><br />
                <input type="radio" name="radAgencyFee" id="radAgencyFee2" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三 %>" /><label
                    for="radAgencyFee2">公式三 (票面价*百分比+机建燃油)*人数+其它费用</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>申请机票游客勾选配置<br />
                <input type="radio" name="radTicketTravellerCheckedType" id="radTicketTravellerCheckedType0"
                    checked="checked" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.None %>" /><label
                        for="radTicketTravellerCheckedType0">不管是否有退票均可申请</label><br />
                <input type="radio" name="radTicketTravellerCheckedType" id="radTicketTravellerCheckedType1"
                    value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.LeastOne %>" /><label
                        for="radTicketTravellerCheckedType1">有退票（有一个或多个航段退票）即可申请</label><br />
                <input type="radio" name="radTicketTravellerCheckedType" id="radTicketTravellerCheckedType2"
                    value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.All %>" /><label
                        for="radTicketTravellerCheckedType2">退完票（所有航段均退票）才可申请</label><br />
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>统计订单方式配置：<input type="radio" name="radComputeOrderType"
                    id="radComputeOrderType0" checked="checked" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单 %>" /><label
                        for="radComputeOrderType0">统计确认成交订单</label>
                <input type="radio" name="radComputeOrderType" id="radComputeOrderType1" /><label
                    for="radComputeOrderType1" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单 %>">统计有效订单(除不受理、留位过期以外所有)</label>
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <a href="settingnotepad.htm" target="_blank">查看系统配置记事本</a><span class="note">系统配置记事本记录了已开通系统的路径配置信息</span>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>统计分析利润统计团队数页面路径配置：<input type="text" id="txtProfitStatTourPagePath"
                    name="txtProfitStatTourPagePath" class="input_text" maxlength="72" style="width: 300px" />
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>游客信息是否必填：
                <input type="radio" id="radIsRequiredTraveller0" name="radIsRequiredTraveller" value="0"
                    checked="checked" /><label for="radIsRequiredTraveller0">否</label>
                <input type="radio" id="radIsRequiredTraveller1" name="radIsRequiredTraveller" value="1" /><label
                    for="radIsRequiredTraveller1">是</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>团队计划人数配置：
                <input type="radio" id="radTeamNumberOfPeople0" name="radTeamNumberOfPeople" value="0"
                    checked="checked" /><label for="radTeamNumberOfPeople0">仅填写总人数</label>
                <input type="radio" id="radTeamNumberOfPeople1" name="radTeamNumberOfPeople" value="1" /><label
                    for="radTeamNumberOfPeople1">按成人、儿童、全陪填写</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>机票售票处填写时间配置：
                <input type="radio" id="radTicketOfficeFillTime0" name="radTicketOfficeFillTime"
                    value="0" checked="checked" /><label for="radTicketOfficeFillTime0">机票审核后才可填写</label>
                <input type="radio" id="radTicketOfficeFillTime1" name="radTicketOfficeFillTime"
                    value="1" /><label for="radTicketOfficeFillTime1">机票申请时就可填写</label>
            </td>
        </tr>
        <tr>
            <td>
                <span class="required">*</span>完成出票机票款是否自动结清：
                <input type="radio" id="radIsTicketOutRegisterPayment0" name="radTicketOutClear" value="1" /><label
                    for="radIsTicketOutRegisterPayment0">是</label>
                <input type="radio" id="radIsTicketOutRegisterPayment1" name="radTicketOutClear" checked="checked"
                    value="0" /><label for="radIsTicketOutRegisterPayment1">否</label>
            </td>
        </tr>
        <tr>
            <td>回款率是否包含未审批收款：<label><input type="radio" name="radHuiKuanLvSFBHWeiShenHe" value="0" checked="checked" />不包含</label>
            <label><input type="radio" name="radHuiKuanLvSFBHWeiShenHe" value="1" />包含</label>
            </td>
        </tr>
        <tr>
            <td>
                同行平台计划展示方式：<label><input type="radio" name="radSiteTourDisplayType" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团 %>" checked="checked" />明细团</label>
                <label><input type="radio" name="radSiteTourDisplayType" value="<%=(int)EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.子母团 %>" />子母团</label>
            </td>
        </tr>
        <tr>
            <td>
                同行平台模板：<select name="txtSiteTemplate" id="txtSiteTemplate">
                    <option value="<%=(int)EyouSoft.Model.EnumType.SysStructure.SiteTemplate.None %>">请选择</option>
                    <option value="<%=(int)EyouSoft.Model.EnumType.SysStructure.SiteTemplate.模板一 %>">模板一</option>
                </select>&nbsp;&nbsp;<a href="同行平台模板说明.htm" target="_blank">同行平台模板说明</a>
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <!--打印单据区域-->
        <tr id="trPrintDocumentAfter">
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <a href="javascript:displayPermissions();">显示权限</a> <a href="javascript:hidePermissions();">
                    隐藏权限</a> <span class="note">注：加粗的为栏目及子栏目，未加粗的为具体权限，仅勾选权限不勾选栏目该权限将不会生效。</span>
            </td>
        </tr>
        <tr id="trPermissions">
            <td>
                <asp:Literal runat="server" ID="ltrPermissions"></asp:Literal>
                <%--
                <div class="pBig">
                    <input type="checkbox" />行政中心菜单</div>
                <div>
                    <ul class="pSmall">
                        <li class="pSmallTitle">
                            <input type="checkbox" />事务提醒1</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                    </ul>
                    <ul class="pSmall">
                        <li class="pSmallTitle">
                            <input type="checkbox" />事务提醒1</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                    </ul>
                    <ul class="pSmall">
                        <li class="pSmallTitle">
                            <input type="checkbox" />事务提醒1</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                    </ul>
                    <ul class="pSmall">
                        <li class="pSmallTitle">
                            <input type="checkbox" />事务提醒1</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                    </ul>
                    <ul class="pSmallSpace">
                        <li></li>
                    </ul>
                    <ul class="pSmall">
                        <li class="pSmallTitle">
                            <input type="checkbox" />事务提醒1</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                        <li>
                            <input type="checkbox" />事务提醒</li>
                    </ul>
                    <ul class="pSmallSpace">
                        <li></li>
                    </ul>
                </div>
                --%>
            </td>
        </tr>
        <tr>
            <td class="trspace">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCreate" runat="server" Text="创建子系统" OnClick="btnCreate_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
    1.标记<span class="required">*</span>为必填项。<br />
    2.组团路径为组团域名跳转到的页面路径，不填写将使用专线端登录界面。<br />
    3.公司及用户的联系人信息均使用以上的联系人信息<br />
    4.子系统开通后会默认开通管理员角色，默认的管理员角色将拥有为子系统开通的所有权限<br />
    5.管理员账号将拥有管理员角色的所有权限<br />
    6.总部的主管为添加的管理员<br />
    7.子系统开通后会默认“门市”及“同行”两个客户等级。<br />
    8.子系统开通后会默认一个“常规”报价标准。<br />
    9.子系统开通后会默认导入系统定义的省份及城市信息，但不设置任何常用城市。<br />
    10.系统配置信息默认值为芭比莱系统的配置，根据需要更改。<br />
</asp:Content>
