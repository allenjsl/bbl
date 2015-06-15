//webmaster.core.js
//汪奇志 2011-04-18

//需要引用jQuery
//方法piframeResize()定义在母版页

//添加域名tr 
//isAddbtn=true显示添加按钮 isAddBtn=false显示删除按钮
//v:域名值
//url:域名跳转到的URL
function addSysDomain(isAddbtn, v, url, t) {
    var s = [];
    s.push("<tr>");
    s.push('<td><span class="required">*</span>');
    s.push('域名类型：<select name="txtDomainType"><option value="0" ' + (t == 0 ? 'selected="selected"' : '') + '>专线入口</option><option value="1" ' + (t == 1 ? 'selected="selected"' : '') + '>同行入口</option></select>');
    s.push('&nbsp;&nbsp;域名：<input type="text" name="txtDomain" class="input_text" style="width: 150px" value="' + v + '" />');
    s.push('&nbsp;&nbsp;跳转路径：<input type="text" name="txtDomainPath" class="input_text" style="width: 200px" value="' + url + '" >');
    isAddbtn ? s.push(' <a href="javascript:addSysDomain(false,\'\',\'\',0)">添加</a>') : s.push(' <a href="javascript:void(0)" onclick="deleteSysDomain(this)">删除</a>');
    s.push("</td>");
    s.push("</tr>");

    $("#trDomainAfter").before(s.join(""));
    piframeResize()
}

//删除域名tr
function deleteSysDomain(obj) {
    $(obj).parent().parent().remove();
    piframeResize()
}

//添加打印单据配置tr 
//isAddBtn=true显示添加按钮 isAddBtn=false显示删除按钮
//v:选中的值 单据类型
//path:单据配置路径
function addPrintDocument(isAddBtn, v, path) {
    var s = [];
    s.push("<tr>");
    s.push("<td>");
    s.push('打印单据配置：' + getPrintDocumentSelect(v));
    s.push('&nbsp;页面路径：<input type="text" name="txtPrintDocumentV" class="input_text" maxlength="72" style="width: 280px" value="' + path + '" />');
    isAddBtn ? s.push(' <a href="javascript:void(0)" onclick="addPrintDocument(false,0,\'\')">添加</a>') : s.push(' <a href="javascript:void(0)" onclick="deletePrintDocument(this)">删除</a>');
    s.push("</td>");
    s.push("</tr>");

    $("#trPrintDocumentAfter").before(s.join(""));
    piframeResize()
}

//删除打印单据tr
function deletePrintDocument(obj) {
    $(obj).parent().parent().remove();
    piframeResize()
}

//获取打印单据类型select input 
//需在页面注册变量printDocumentTypes 格式如var printDocumentTypes=[{Value:0,Text:"请选择单据类型"}];
//v:选中的值
function getPrintDocumentSelect(v) {
    var s = [];

    s.push('<select name="txtPrintDocumentK">');
    s.push('<option value="0">请选择单据类型</option>');
    for (var i = 0; i < printDocumentTypes.length; i++) {
        s.push('<option value="' + printDocumentTypes[i].Value + '" ' + (printDocumentTypes[i].Value == v ? 'selected="selected"' : '') + '>' + printDocumentTypes[i].Value + '  ' + printDocumentTypes[i].Text + '</option>');
    }    
    s.push('</select>')

    return s.join("");
}

//显示权限
function displayPermissions() {
    $("#trPermissions").show();
    piframeResize()
}

//隐藏权限
function hidePermissions() {
    $("#trPermissions").hide();
    piframeResize()
}

//修改系统信息页面初始化原系统域名  需在页面注册变量sysDomains格式为域名信息业务实体集合JSON DATA
function initDomains() {
    if (sysDomains == null || sysDomains.length < 1) {
        addSysDomain(true, '', '',0);
        return;
    }

    for (var i = 0; i < sysDomains.length; i++) {
        if (i == 0) { addSysDomain(true, sysDomains[i].Domain, sysDomains[i].Url,sysDomains[i].DomainType); continue }
        addSysDomain(false, sysDomains[i].Domain, sysDomains[i].Url,sysDomains[i].DomainType); 
    }
}

//设置input radio 选中值
//objName：input name
//checkedValue:选中的值
//布尔类型 为真时选中1值 假时选中0值
function checkedRadio(objName, checkedValue) {
    var $obj = $('input[name="' + objName + '"]');

    var isChecked = false;
    $obj.each(function() {
        if (checkedValue == true && this.value == "1") { this.checked = true; isChecked = true; return false; }
        if (checkedValue == false && this.value == "0") { this.checked = true; isChecked = true; return false; }
        if (this.value == checkedValue) { this.checked = true; isChecked = true; return false; }
    });
    
    if (!isChecked) {
        $obj[0].checked = true;
    }
}

//设置input checkbox 选中值
//objName:input id
function checkedCheckbox(objId) {
    $('#' + objId).attr("checked", true);
}

//修改系统信息页面初始化系统配置  需在页面注册变量sysSetting 格式为系统配置信息业务实体JSON DATA
//isNormalPrint 是否使用常规的打印配置信息
function initSysSetting(isNormalPrint) {
    isNormalPrint = isNormalPrint || false;
    if (sysSetting == null) return;
    $("#txtContractReminderDays").val(sysSetting.ContractReminderDays);
    $("#txtLeaveTourReminderDays").val(sysSetting.LeaveTourReminderDays);
    $("#txtBackTourReminderDays").val(sysSetting.BackTourReminderDays);
    $("#txtReservationTime").val(sysSetting.ReservationTime);
    $("#txtDisplayBeforeMonth").val(sysSetting.DisplayBeforeMonth);
    $("#txtDisplayAfterMonth").val(sysSetting.DisplayAfterMonth);
    $("#txtProfitStatTourPagePath").val(sysSetting.ProfitStatTourPagePath);

    checkedRadio("radPriceComponent", sysSetting.PriceComponent);
    checkedRadio("radAgencyFee", sysSetting.AgencyFeeInfo);
    checkedRadio("radTicketTravellerCheckedType", sysSetting.TicketTravellerCheckedType);
    checkedRadio("radComputeOrderType", sysSetting.ComputeOrderType);
    checkedRadio("radIsRequiredTraveller", sysSetting.IsRequiredTraveller);
    checkedRadio("radTeamNumberOfPeople", sysSetting.TeamNumberOfPeople);
    checkedRadio("radTicketOfficeFillTime", sysSetting.TicketOfficeFillTime);

    checkedRadio("radTicketOutClear", sysSetting.IsTicketOutRegisterPayment);
    checkedRadio("radHuiKuanLvSFBHWeiShenHe", sysSetting.HuiKuanLvSFBHWeiShenHe);
    checkedRadio("radSiteTourDisplayType", sysSetting.SiteTourDisplayType);

    $("#txtSiteTemplate").val(sysSetting.SiteTemplate)
    
    if (sysSetting.PrintDocument != null && sysSetting.PrintDocument.length > 0) {
        for (var i = 0; i < sysSetting.PrintDocument.length; i++) {
            if (isNormalPrint) sysSetting.PrintDocument[i].PrintTemplate = sysSetting.PrintDocument[i].PrintTemplate.toLowerCase().replace("/print/bbl/", "/print/normal/");
            if (i == 0) { addPrintDocument(true, sysSetting.PrintDocument[i].PrintTemplateType, sysSetting.PrintDocument[i].PrintTemplate); continue; }
            addPrintDocument(false, sysSetting.PrintDocument[i].PrintTemplateType, sysSetting.PrintDocument[i].PrintTemplate);
        }
    } else {
        addPrintDocument(true, '', '');
    }
}

//修改系统信息页面初始化系统栏目、子栏目、权限  需在页面注册变量sysPermissions 格式为sysPermissions={first:[-1],second:[-1],third:[-1]}
function intSysPermission() {
    var data = sysPermissions.first;

    for (var i = 0; i < data.length; i++) {
        checkedCheckbox("chk_p_big_" + data[i]);
    }
    data = sysPermissions.second;
    for (var i = 0; i < data.length; i++) {
        checkedCheckbox("chk_p_small_" + data[i]);
    }
    data = sysPermissions.third;
    for (var i = 0; i < data.length; i++) {
        checkedCheckbox("chk_p_third_" + data[i]);
    }
}

