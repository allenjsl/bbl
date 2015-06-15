/*	dynamicCSS.js v1.0 <http://www.bobbyvandersluis.com/articles/dynamicCSS.php>
	Copyright 2005 Bobby van der Sluis
	This software is licensed under the CC-GNU LGPL <http://creativecommons.org/licenses/LGPL/2.1/>
*/
/* Cross-browser dynamic CSS creation
        - Based on Bobby van der Sluis' solution: http://www.bobbyvandersluis.com/articles/dynamicCSS.php
*/     
function createStyleRule(selector, declaration) {
	if (!document.getElementsByTagName || !(document.createElement || document.createElementNS)) return;
	var agt = navigator.userAgent.toLowerCase();
	var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));
	var is_iewin = (is_ie && (agt.indexOf("win") != -1));
	var is_iemac = (is_ie && (agt.indexOf("mac") != -1));
	if (is_iemac) return; // script doesn't work properly in IE/Mac
	var head = document.getElementsByTagName("head")[0]; 
	var style = (typeof document.createElementNS != "undefined") ?  document.createElementNS("http://www.w3.org/1999/xhtml", "style") : document.createElement("style");
	if (!is_iewin) {
		var styleRule = document.createTextNode(selector + " {" + declaration + "}");
		style.appendChild(styleRule); // bugs in IE/Win
	}
	style.setAttribute("type", "text/css");
	style.setAttribute("media", "screen"); 
	head.appendChild(style);
	if (is_iewin && document.styleSheets && document.styleSheets.length > 0) {
		var lastStyle = document.styleSheets[document.styleSheets.length - 1];
		if (typeof lastStyle.addRule == "object") { // bugs in IE/Mac and Safari
			lastStyle.addRule(selector, declaration);
		}
	}
}
function copyDate(date){
	var d = new Date(date.getFullYear(),date.getMonth(),date.getDate());
	return d;
}
function formatDate(date){
    var d = new Date(date.getFullYear(),date.getMonth(),1);
    return d;
}
function compareDate(DateOne,DateTwo)  
{   
	var OneMonth = DateOne.getMonth() + 1; 
	var OneDay = DateOne.getDate();
	var OneYear = DateOne.getFullYear();  
	  
	var TwoMonth = DateTwo.getMonth() + 1;  
	var TwoDay = DateTwo.getDate();  
	var TwoYear = DateTwo.getFullYear();  
  
	if (Date.parse(OneMonth+"/"+OneDay+"/"+OneYear) >  
	Date.parse(TwoMonth+"/"+TwoDay+"/"+TwoYear)){  
		return 1;  
	}  
	else if(Date.parse(OneMonth+"/"+OneDay+"/"+OneYear) <  
	Date.parse(TwoMonth+"/"+TwoDay+"/"+TwoYear)){  
		return -1;  
	}else{
		return 0;
	}
  
} 
function compareDateByMonth(DateOne,DateTwo){
    var OneMonth = DateOne.getMonth()+1; 
	var OneDay = 1;
	var OneYear = DateOne.getFullYear();  
	  
	var TwoMonth = DateTwo.getMonth()+1;  
	var TwoDay = 1;  
	var TwoYear = DateTwo.getFullYear();  
  
	if (Date.parse(OneMonth+"/"+OneDay+"/"+OneYear) >  
	Date.parse(TwoMonth+"/"+TwoDay+"/"+TwoYear)){  
		return 1;  
	}  
	else if(Date.parse(OneMonth+"/"+OneDay+"/"+OneYear) <  
	Date.parse(TwoMonth+"/"+TwoDay+"/"+TwoYear)){  
		return -1;  
	}else{
		return 0;
	}
}
Array.prototype.remove = function(dx)
{
	if(isNaN(dx)||dx> this.length) { return false;}
	for(var i=0,n=0;i<this.length;i++)
	{
		if(this[i]!=this[dx])
		{
			this[n++]=this[i]
		}
	}
	this.length-=1
}
var QGD = {
    html: '<table class="calendarTable" width="100%" cellspacing="0" cellpadding="0">' +
  '<tbody><tr>' +
    '<td height="25" bgcolor="#def2fc" align="center" style="padding: 3px;">&nbsp;<a href="javascript:void(0);" id="linkPreMonth">上一月</a>&nbsp;&nbsp;</td>' +
    '<td height="25" bgcolor="#def2fc" align="center" style="padding: 3px;">&nbsp; <a href="javascript:void(0);" id="linkNextMonth">下一月</a>&nbsp;</td>' +
  '</tr>' +
  '<tr>' +
    '<td width="50%" valign="top"><div id="thisMonthCalendar"></div></td>' +
    '<td width="50%" valign="top"><div id="nextMonthCalendar"></div></td>' +
  '</tr>' +
'</tbody></table>',

    //初始化日历
    initCalendar: function(option) {
        this.option = $.extend({
            containerId: "calendarContainer",
            tourLeaveDate: "hidTourLeaveDate",
            listcontainer: "divTourCodeHTML"
        }, option || {});
        this.currentDate = this.option.currentDate;
        this.maxDate = copyDate(this.option.currentDate);

        this.maxDate = formatDate(this.maxDate);


        this.maxDate.setMonth(this.maxDate.getMonth() + 5);

        this.SCD = formatDate(this.option.firstMonthDate);
        this.SND = formatDate(this.option.nextMonthDate);

        this.initCalendarBasic();
        this._createCalendarContainer();

        //初始化当前月日历
        this.createCalendarMonth(false);
        this.createCalendarDays(false);

        //初始化下月日历
        this.createCalendarMonth(true);
        this.createCalendarDays(true);

        //创建空行
        this.createEmptyRows();

        //初始化已选数据
        if (this.option.parentiframeid == undefined || this.option.parentiframeid == "") {
            var s = parent.document.getElementById("hidToursNumbers").value;
        } else {
        var s = parent.Boxy.getIframeWindow(this.option.parentiframeid).document.getElementById("hidToursNumbers").value;
        }
        if (s != "") {
            var arr = convertToursNumberStr(s);
            QGD.config.arrLeaveDate
            for (var i = 0; i < arr.length; i++) {
                QGD.config.arrLeaveDate.push(arr[i].getDateString());
            }
            this.InitCheckedDate();
            this.createChildrenTourHTML(s);
        }
    },
    updateCalendar: function(option) {
        this.option = $.extend(this.option, option || {});

        this.config.CR = 0;
        this.config.NR = 0;

        this.SCD = this.option.firstMonthDate;
        this.SND = this.option.nextMonthDate;

        this.initCalendarBasic();
        this._createCalendarContainer();

        //初始化当前月日历
        this.createCalendarMonth(false);
        this.createCalendarDays(false);

        //初始化下月日历
        this.createCalendarMonth(true);
        this.createCalendarDays(true);

        //创建空行
        this.createEmptyRows();
    },
    p: parent,
    //页面元素配置信息
    elements: {
        //生成的当前月表格id
        cMTable: 'cMonthTable',
        //生成的下一个月表格id
        nMTable: 'nMonthTable',
        //存放当前月表格容器id
        cMTd: "thisMonthCalendar",
        //存放下一个月表格容器id
        nMTd: "nextMonthCalendar",
        //线路区域下拉表单id
        tourArea: "txtTourArea",
        // 父页面存放出团日期
        tourLeaveDate: "hidTourLeaveDate",
        //子团容器
        childrenTour: "ulChildrenTours",
        //正在生成团队的提示信息
        loading: "loading",
        //存放要新增的子团
        ITJ: "txtInsertChildrenTours",
        //存放要修改的子团
        UTJ: "txtUpdateChildrenTours",
        //存放要删除的子团
        DTJ: "txtDeleteChildrenTours"
    },
    maxDate: new Date(),
    //服务器当前日期
    currentDate: new Date(),
    //显示的第一个月的日期
    SCD: new Date(),
    //服务器下一个月日期
    SND: new Date(),
    //日历基础数据
    SDConfig: { CY: 2010, CM: 1, CD: 1, CDays: 31, NY: 2010, NM: 2, NDays: 28 },
    //配置信息
    config: {
        //当前月行数
        CR: 0,
        //下一个月行数
        NR: 0,
        //当前月1号在表格内td的index
        CI: 0,
        //下一个月1号在表格内td的index
        NI: 0,
        // 存放选中的出团日期
        arrLeaveDate: [],
        // 原来的子团出团日期
        arrOldLeaveDate: [],
        // 原来的子团团号
        arrOldTourCode: [],
        //模板团编号
        TI: 0,
        //原有子团信息
        TC: [],
        //当前子团信息
        CC: [],
        //父页面盛放子团的jQuery对象
        JPO: null,
        //日历当前月表格jQuery对象
        JCO: null,
        //日历下个月表格jQuery对象
        JNO: null,
        //父页面线路区域下拉列表的jQuery对象
        JPTAO: null,
        //是否有新增子团
        ISI: false,
        //是否有修改子团
        ISU: false,
        //是否有删除子团
        ISD: false,
        //empty template children tour info
        ETC: {}
    },
    //初始化日历基础数据
    initCalendarBasic: function() {
        this.SDConfig.CY = this.SCD.getFullYear();
        this.SDConfig.CM = this.SCD.getMonth() + 1;
        this.SDConfig.CD = this.SCD.getDate();
        this.SDConfig.CDays = new Date(this.SDConfig.CY, this.SDConfig.CM, 0).getDate();

        this.SDConfig.NY = this.SND.getFullYear();
        this.SDConfig.NM = this.SND.getMonth() + 1;
        this.SDConfig.NDays = new Date(this.SDConfig.NY, this.SDConfig.NM, 0).getDate();
    },
    /*private*/
    _createCalendarContainer: function() {
        if (!document.getElementById(this.option.containerId)) {
            alert("请指定日历容器");
            return false;
        }

        if (this.firstLoad == undefined || this.firstLoad == false) {
            $("#" + this.option.containerId).html(this.html);
            this.firstLoad = true;
        }
        var self = this;
        var cDate = copyDate(self.SCD);
        var nDate = copyDate(self.SND);
        var result = compareDate(self.SCD, this.currentDate);
        //---------zxb add by 20110407 begin --------------
        //cancel the limit that is the pre month not be less than the current month temporarily.
        result=1;
        //---------zxb add by 20110407 end--------------
        if (result == -1 || result == 0) {
            $("#linkPreMonth").css("display", "none");
        } else {
            $("#linkPreMonth").css("display", "").unbind().click(function() {
                var a = copyDate(cDate);
                cDate.setMonth(cDate.getMonth() - 1);
                var b = copyDate(cDate);
                self.GetCheckedDate();
                self.updateCalendar({
                    nextMonthDate: a,
                    firstMonthDate: b
                });

                self.InitCheckedDate();
            });
        }
        result = compareDate(this.SND, this.maxDate);
        if (result == 1 || result == 0) {
            $("#linkNextMonth").css("display", "none");
        } else {
            $("#linkNextMonth").css("display", "").unbind().click(function() {
                var a = copyDate(nDate);
                nDate.setMonth(nDate.getMonth() + 1)
                var b = copyDate(nDate);
                self.GetCheckedDate();
                self.updateCalendar({
                    firstMonthDate: a,
                    nextMonthDate: b
                });

                self.InitCheckedDate();
            });
        }
    },
    //创建日历表格
    createCalendarMonth: function(isNextMonth) {
        var myself = this;
        var tableId = isNextMonth ? this.elements.nMTable : this.elements.cMTable;
        var s = [];
        s.push('<table width="100%" border="1" cellpadding="0" cellspacing="0" id="' + tableId + '">');
        s.push('<tr><th colspan="8"><input type="checkbox" id="' + isNextMonth + 'ckAll"><label for="' + isNextMonth + 'ckAll">全选 ' + (isNextMonth ? this.SDConfig.NY : this.SDConfig.CY) + '年' + (isNextMonth ? this.SDConfig.NM : this.SDConfig.CM) + '月整月</label></th></tr>');
        s.push('<tr class="weektitle"><td style="width:16%"><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '0"><label for="' + isNextMonth + '0">日</label></td></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '1"><label for="' + isNextMonth + '1">一</label></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '2"><label for="' + isNextMonth + '2">二</label></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '3"><label for="' + isNextMonth + '3">三</label></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '4"><label for="' + isNextMonth + '4">四</label></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '5"><label for="' + isNextMonth + '5">五</label></td><td style="width: 12%"><input type="checkbox" id="' + isNextMonth + '6"><label for="' + isNextMonth + '6">六</label></td></tr>');
        s.push('</table>');
        $("#" + (isNextMonth ? this.elements.nMTd : this.elements.cMTd)).html(s.join(''));

        if (isNextMonth) {
            this.config.JNO = $("#" + tableId);
        } else {
            this.config.JCO = $("#" + tableId);
        }

        var obj = isNextMonth ? this.config.JNO : this.config.JCO;

        //年全选绑定事件
        obj.find('tr th input[type="checkbox"]').bind("click", function() { myself.selectMonth(this, isNextMonth) });
        //列全选绑定事件
        obj.find('tr:gt(0) input[type="checkbox"]').bind("click", function() { myself.selectColumn(this, isNextMonth) });
        //列mouseover mouseout事件
        obj.find('tr:eq(1) td').bind("mousemove", function() { myself.mouseoverColumn(this); }).bind("mouseout", function() { myself.mouseoutColumn(this); })

    },
    //创建前面的空白天数
    createStartEmptyDays: function(monthFirstDayOfWeek) {
        var s = [];
        for (var i = 0; i < monthFirstDayOfWeek; i++) {
            s.push('<td></td>')
        }
        return s.join('');
    },
    //创建空行
    createEmptyRows: function() {
        var s = '<tr style="height:21px;"><td>&nbsp;</td><td>&nbsp;</td><td></td><td></td><td></td><td></td><td></td><td></td></tr>';
        if (this.config.CR > this.config.NR) {
            for (var i = 0; i < this.config.CR - this.config.NR; i++) {
                this.config.JNO.append(s);
            }
        } else {
            for (var i = 0; i < this.config.NR - this.config.CR; i++) {
                this.config.JCO.append(s);
            }
        }
    },
    //创建后面的空白天数
    createEndEmptyDays: function(days) {
        if (days == 7) return;
        var s = [];
        for (var i = 0; i < days; i++) {
            s.push('<td></td>')
        }
        s.push('</tr>')
        return s.join('');
    },
    //创建日历日期信息
    createCalendarDays: function(isNextMonth) {
        var myself = this;
        var obj = isNextMonth ? this.config.JNO : this.config.JCO;
        var tmpyear = isNextMonth ? this.SDConfig.NY : this.SDConfig.CY;
        var tmpmonth = isNextMonth ? this.SDConfig.NM : this.SDConfig.CM;
        var sd = 1;
        var fd = isNextMonth ? this.SDConfig.NDays : this.SDConfig.CDays;
        var sdOfWeek = isNextMonth ? new Date(this.SDConfig.NY, this.SDConfig.NM - 1, 1).getDay() : new Date(this.SDConfig.CY, this.SDConfig.CM - 1, 1).getDay();
        var s = [];
        var i = 1;
        var isCurrentMonth = false;
        if (this.SDConfig.CY == this.currentDate.getFullYear()
			&& this.SDConfig.CM == (this.currentDate.getMonth() + 1)
			&& isNextMonth == false) {
            isCurrentMonth = true;
        }

        do {
            if ((i) % (7) == 1) {

                if (isNextMonth) {
                    this.config.NR++;
                } else {
                    this.config.CR++;
                }

                s.push('<tr>');
                var inputid = isNextMonth ? 'cw_' + sd : 'nw_' + sd;
                s.push('<td><input type="checkbox" id="' + inputid + '"><label for="' + inputid + '">第' + parseInt((sd + sdOfWeek + 6) / 7) + '周</label></td>');
            }

            if (i == 1) {
                s.push(this.createStartEmptyDays(sdOfWeek));
                i = i + sdOfWeek;
            }

            var tmpDate = tmpyear + "-" + tmpmonth + "-" + sd;
            s.push('<td class="days"><input type="checkbox"' + (sd < this.currentDate.getDate() && isCurrentMonth ? ' disabled="disabled"' : '') + ' value="' + tmpDate + '"><span>' + sd + '</span></td>');

            if ((i) % (7) == 0) { s.push('</tr>'); }

            if (isNextMonth) {
                this.config.NI = 9 + sdOfWeek;
            } else {
                this.config.CI = 9 + sdOfWeek;
            }

            sd++;
            i++
        } while (sd <= fd)

        s.push(this.createEndEmptyDays(7 - (i - 1) % 7));
        obj.append(s.join(''));
        //行全选绑定事件
        obj.find('tr:gt(1)').find('td:eq(0) input[type=checkbox]').bind("click", function() { myself.selectRow(this, isNextMonth); });
        //天选中绑定事件
        obj.find('td.days input[type=checkbox]').bind("click", function() { myself.selectDay(this, isNextMonth); });
        //行mouseover mouseout事件
        obj.find("tr:gt(1)").find("td:eq(0)").bind("mousemove", function() { myself.mouseoverRow(this); }).bind("mouseout", function() { myself.mouseoutRow(this); })
    },
    //选中事件
    setChecked: function(obj, isNextMonth, checked) {
        if (checked) {
            obj.checked = checked;
        } else {
            obj.checked = checked;
        }
    },
    //月份全选
    selectMonth: function(obj, isNextMonth) {
        var myself = this;
        $(obj).closest("table").find("input[type=checkbox]:enabled").each(function() { myself.setChecked(this, isNextMonth, obj.checked); })
    },
    //列全选
    selectColumn: function(obj, isNextMonth) {
        var myself = this;
        $(obj).closest("table").find("tr:gt(1)").each(function() { $(this).find("td:eq(" + $(obj).closest("tr").find("td").index($(obj).parent()) + ") input:[type=checkbox]:enabled").each(function() { myself.setChecked(this, isNextMonth, obj.checked) }); });
    },
    //行全选
    selectRow: function(obj, isNextMonth) {
        var myself = this;
        $(obj).closest("tr").find("input[type=checkbox]:enabled:gt(0)").each(function() { myself.setChecked(this, isNextMonth, obj.checked); });
    },
    //天选中
    selectDay: function(obj, isNextMonth) {
        this.setChecked(obj, isNextMonth, obj.checked);
    },
    //onmouseover 星期 列
    mouseoverColumn: function(obj) {
        $(obj).closest("table").find("tr:gt(1)").each(function() { $(this).find("td:eq(" + $(obj).parent().find("td").index($(obj)) + ")").css({ background: '#e3d596' }) });
    },
    //onmouseout 星期 列
    mouseoutColumn: function(obj) {
        $(obj).closest("table").find("tr:gt(1)").each(function() { $(this).find("td:eq(" + $(obj).parent().find("td").index($(obj)) + ")").css({ background: '#ffffff' }) });
    },
    //onmouseover 第N周 行
    mouseoverRow: function(obj) {
        $(obj).parent().children("td").css({ background: '#e3d596' });
    },
    //onmouseout 第N周 行
    mouseoutRow: function(obj) {
        $(obj).parent().children("td").css({ background: '#ffffff' });
    },
    //选中子团的日期
    initCalendarChecked: function() {
        var tmp = (this.config.CC.length < 1) ? this.config.TC : this.config.CC;

        for (var i = 0; i < tmp.length; i++) {
            var arr = tmp[i].TravelPeriod.split("-");
            var m = arr[1];
            var d = arr[2];

            if (m == this.SDConfig.CM) {
                this.config.JCO.find('td.days input[type="checkbox"]:eq(' + (d - 1) + ')').attr("checked", true);
            } else {
                this.config.JNO.find('td.days input[type="checkbox"]:eq(' + (d - 1) + ')').attr("checked", true);
            }
        }
    },
    //获取星期
    getWeek: function(date) {
        var d = date.split("-");
        var dayOfWeek = new Date(d[0], d[1] - 1, d[2]).getDay();

        var s = "";

        switch (dayOfWeek) {
            case 0: s = "日"; break;
            case 1: s = "一"; break;
            case 2: s = "二"; break;
            case 3: s = "三"; break;
            case 4: s = "四"; break;
            case 5: s = "五"; break;
            case 6: s = "六"; break;
        }

        return s;
    },
    //生成子团HTML
    createChildrenTourHTML: function(s) {
        $("#" + this.option.listcontainer).html("正在生成...");
        var s = s;
        if (s == "") {
            return;
        }
        var arr = convertToursNumberStr(s);
        var cmonth = copyDate(arr[0].date);
        var cmonthArr = [];
        cmonthArr.push(cmonth);

        for (var i = 0; i < arr.length; i++) {
            var ise = false;
            for (var j = 0; j < cmonthArr.length; j++) {
                if (compareDateByMonth(cmonthArr[j], arr[i].date) == 0) {
                    ise = true;
                    break;
                }
            }
            if (ise == false) {
                cmonthArr.push(copyDate(arr[i].date));
            }
        }

        var s = "";
        var k = 0;
        for (i = 0; i < cmonthArr.length; i++) {
            s += '<fieldset style="font-size: 12px;">' + '<legend>' + cmonthArr[i].getFullYear().toString() + '年' + (cmonthArr[i].getMonth() + 1) + '月</legend>';
            for (var j = 0; j < arr.length; j++) {
                if (compareDateByMonth(cmonthArr[i], arr[j].date) == 0) {
                    s += '<label style="padding-left:60px;padding-bottom:10px;">' + arr[j].getDateString() + '<input type="hidden" name="date" value="' + arr[j].getDateString() + '" /><input name="tournumber" style="width:100px;" type="text" value="' + arr[j].tourNumber + '" /></label>';
                    if ((k + 1) % 3 == 0) {
                        s += "<div style='margin:10px;'></div>";
                    }
                    k++;
                }
            }
            k = 0;
            s += '</fieldset><div style="margin:10px;"></div>';
        }

        //var isAddBegin=false,isAddEnd = true;

        //        var count = 3;
        //        for(var i = 0;i<arr.length;i++){
        //            
        //            if(compareDateByMonth(cmonth,arr[i].date)<0){
        //                cmonth = copyDate(arr[i].date);
        //                isAddEnd = false;
        //                isAddBegin = false;
        //            }
        //            if(!isAddEnd){
        //                s+='</fieldset><div style="margin:10px;"></div>';
        //                isAddEnd=true;
        //            }
        //            if(!isAddBegin){
        //                s+='<fieldset style="font-size: 12px;">'+'<legend>'+cmonth.getFullYear().toString()+'年'+(cmonth.getMonth()+1)+'月</legend>';
        //                isAddBegin = true;
        //            }
        //            s+='<label style="padding-left:60px;padding-bottom:10px;">'+arr[i].getDateString()+'<input type="hidden" name="date" value="'+arr[i].getDateString()+'" /><input name="tournumber" style="width:100px;" type="text" value="'+arr[i].tourNumber+'" /></label>';
        //            if((i+1)%3==0){
        //                s+="<div style='margin:10px;'></div>";
        //            }
        //        }

        //$("#childToursList").html(s);
        $("#" + this.option.listcontainer).html(s);
        $("#tblNext").show();
    },
    //子团全选
    selectAll: function(obj) {
        $("#" + this.elements.childrenTour).find("input[type=checkbox]").attr("checked", obj.checked);
    },
    // 获取选中的日期
    GetCheckedDate: function() {
        var myself = this;
        var objs = [{ obj: this.config.JCO, year: this.SDConfig.CY, month: this.SDConfig.CM }, { obj: this.config.JNO, year: this.SDConfig.NY, month: this.SDConfig.NM}];

        //获取选中的日期
        for (var i = 0; i < objs.length; i++) {

            objs[i].obj.find('td.days input[type="checkbox"]:enabled').each(function() {
                var tmpDate = $(this).val();
                if (QGD.config.arrLeaveDate.length > 0) {
                    var oid = -1;
                    for (var h = 0; h < QGD.config.arrLeaveDate.length; h++) {
                        if (tmpDate == QGD.config.arrLeaveDate[h]) {
                            if (!$(this).attr("checked")) {
                                QGD.config.arrLeaveDate.remove(h);
                            }
                            oid = h;
                            break;
                        }
                    }
                    if (oid == -1) {
                        if ($(this).attr("checked")) {
                            QGD.config.arrLeaveDate.push(tmpDate);
                        }
                    }
                } else {
                    if ($(this).attr("checked")) {
                        QGD.config.arrLeaveDate.push(tmpDate);
                    }
                }
            });
        }
    },
    // 初始化选中的日期
    InitCheckedDate: function() {
        var myself = this;
        var objs = [{ obj: this.config.JCO, year: this.SDConfig.CY, month: this.SDConfig.CM }, { obj: this.config.JNO, year: this.SDConfig.NY, month: this.SDConfig.NM}];

        if (QGD.config.arrLeaveDate.length > 0) {
            for (var i = 0; i < objs.length; i++) {
                objs[i].obj.find('td.days input[type="checkbox"]').each(function() {
                    var tmpDate = $(this).val();
                    for (var h = 0; h < QGD.config.arrLeaveDate.length; h++) {
                        if (tmpDate == QGD.config.arrLeaveDate[h]) {
                            $(this).attr("checked", "checked");
                            break;
                        }
                    }
                });
            }
        }
    },
    SubmitClick: function() {
        this.GetCheckedDate();

        if (QGD.config.arrLeaveDate.length == 0) {
            alert('请选择出团日期!');
            return;
        }

        var obj = GetChildToursNumberList(QGD.config.arrLeaveDate, this.option.companyId);
        this.createChildrenTourHTML(obj.toString());

    }
};
createStyleRule(".calendarTable table","border-collapse:collapse;");
createStyleRule(".calendarTable td","font-size:12px; line-height:120%;");
createStyleRule(".calendarTable .weektitle td","background: #e3d596;text-align: left;");
createStyleRule(".calendarTable .days","text-align: left;");
createStyleRule(".calendarTable th","background: #0099cc;width: 50%;border-bottom: 1px solid #efefef;");
$(document).ready(function() {/* QGD.initChildrenTours();*/ });
