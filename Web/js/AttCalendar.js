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
	var OneMonth = DateOne.getMonth()+1; 
	var OneDay = DateOne.getDate();
	var OneYear = DateOne.getFullYear();  
	  
	var TwoMonth = DateTwo.getMonth()+1;  
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
function jsonDateToDateTime(jsonDate) {
    var reg = /\/Date\((-?[0-9]+)(\+[0-9]+)?\)\//g;
    if (reg.test(jsonDate)) {
        var ticks = Number(jsonDate.replace(reg, '$1'));
        if (!isNaN(ticks)) {
            jsonDate = new Date(Number(jsonDate.replace(reg, '$1')));
        } else {
            jsonDate = new Date();
        }
    }
    return jsonDate;
}

var ChildTours = {
    //个性化设置
    personalize: { wbkgc: "#e3f1fc", highlight: "#9be38e" },
    /*外部调用方法 */
    //初始化日历
    initCalendar: function(option) {
        this.option = $.extend({
            containerId: "calendarContainer",
            configObj: {
                beginDate: null
            }
        }, option || {});
        this.currentDate = this.option.configObj.beginDate;
        this.currentDate = formatDate(this.currentDate);

        this.SCD = copyDate(this.currentDate)
        this.SND = copyDate(this.currentDate);
        this.SND.setMonth(this.SND.getMonth() + 1);

        this.SCD = formatDate(this.SCD);
        this.SND = formatDate(this.SND);

        this._initCalendarBasic();
        this._createCalendarContainer();
        //初始化当前月日历
        this.createCalendarMonth(false);
        this.createCalendarDays(false, eval("(" + this.option.configObj.json + ")"));

        //初始化下月日历
        //        this.createCalendarMonth(true);
        //        this.createCalendarDays(true, eval("(" + this.option.configObj.json + ")"));

        //创建空行
        this._createEmptyRows();
    },
    updateCalendar: function(option) {
        this.option = $.extend(this.option, option || {});

        this.config.CR = 0;
        this.config.NR = 0;

        this.SCD = this.option.firstMonthDate;
        this.SND = this.option.nextMonthDate;

        this._initCalendarBasic();
        this._createCalendarContainer();

        //初始化当前月日历
        this.createCalendarMonth(false);
        this.createCalendarDays(false, eval("(" + this.option.json + ")"));

        //初始化下月日历
        //this.createCalendarMonth(true);
        //this.createCalendarDays(true);

        //创建空行
        this._createEmptyRows();

    },
    /*以下为内部私有方法 */
    html: '<table  border="0" align="center" style="border:1px solid #BDDCF4; margin-top:20px;"   width="100%" cellspacing="0" cellpadding="0">' +
    '<tbody><tr id="trSixMonth">' +
    '<td align="left" style="padding: 0px;">&nbsp;<a id="linkPreMonth" href="javascript:void(0);" style="visibility: visible;"><strong>&lt;&lt;上一个月</strong></a></td>' +
'<td align="center"><strong id="titlePreMonth">2011年1月</strong></td>' +
    '<td align="right" style="padding: 0px;"><a id="linkNextMonth" href="javascript:void(0);" style="visibility: visible;"><strong>下一个月&gt;&gt;</strong></a></td>' +
  '</tr>' +
  '<tr>' +
    '<td colspan="3" valign="top" ><div id="thisMonthCalendar"></div></td>' +
  '</tr>' +
'</tbody></table>',
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
        nMTd: "nextMonthCalendar"
    },
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
        //父页面盛放子团的jQuery对象
        JPO: null,
        //日历当前月表格jQuery对象
        JCO: null,
        //日历下个月表格jQuery对象
        JNO: null,
        //是否登录
        isLogin: false,
        //子团集合
        Childrens: [],
        //端口(日历上的查看详细报价文件在网站首页项目中)
        stringPort: ""
    },
    //初始化日历基础数据
    _initCalendarBasic: function() {
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
        $("#titlePreMonth").html(this.SDConfig.CY + "年" + this.SDConfig.CM + "月");
        var self = this;
        var cDate = copyDate(self.SCD);
        var nDate = copyDate(self.SND);
        $("#linkPreMonth").css("visibility", "visible").unbind().click(function() {
            var a = copyDate(cDate);
            cDate.setMonth(cDate.getMonth() - 1);
            var b = copyDate(cDate);
            var reJson = LookDetail.SetMonthCalendar(b.getFullYear(), b.getMonth() + 1);            //取上月的考勤记录
            self.updateCalendar({
                nextMonthDate: a,
                firstMonthDate: b,
                json: '' + reJson + ''
            });

        });
        $("#linkNextMonth").css("visibility", "visible").unbind().click(function() {
            var a = copyDate(nDate);
            nDate.setMonth(nDate.getMonth() + 1)
            var b = copyDate(nDate);
            var reJson = LookDetail.SetMonthCalendar(b.getFullYear(), a.getMonth()+1);            //取下月的考勤记录
            self.updateCalendar({
                firstMonthDate: a,
                nextMonthDate: b,
                json: '' + reJson + ''
            });
        });
    },
    //创建日历表格
    createCalendarMonth: function(isNextMonth) {
        var myself = this;
        var tableId = isNextMonth ? this.elements.nMTable : this.elements.cMTable;
        var s = [];
        s.push('<table width="100%"  border="1" align="center" bordercolor="#BDDCF4" style="border-collapse:collapse;"  cellpadding="0" cellspacing="0" id="' + tableId + '"   style="border-style: solid;border-width: 1px;" cellpadding="0" cellspacing="0"    ');
        s.push('<tr class="weektitle"><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '0">日</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '1">一</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '2">二</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '3">三</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '4">四</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '5">五</label></td><td width="55" bgcolor="' + this.personalize.wbkgc + '" align="center"><label for="' + isNextMonth + '6">六</label></td></tr>');
        s.push('</table>');
        $("#" + (isNextMonth ? this.elements.nMTd : this.elements.cMTd)).html(s.join(''));

        if (isNextMonth) {
            this.config.JNO = $("#" + tableId);
        } else {
            this.config.JCO = $("#" + tableId);
        }

        var obj = isNextMonth ? this.config.JNO : this.config.JCO;

    },
    //创建前面的空白天数
    createStartEmptyDays: function(monthFirstDayOfWeek) {
        var s = [];
        for (var i = 0; i < monthFirstDayOfWeek; i++) {
            s.push('<td height="46px" bgcolor="#ffffff" align="center"></td>')
        }
        return s.join('');
    },
    //创建空行
    _createEmptyRows: function() {
        var s = '<tr ><td height="46px" bgcolor="#ffffff" align="center">&nbsp;</td><td height="46px" bgcolor="#ffffff" align="center">&nbsp;</td><td height="46px" bgcolor="#ffffff" align="center"></td><td height="46px" bgcolor="#ffffff" align="center"></td><td height="46px" bgcolor="#ffffff" align="center"></td><td height="46px" bgcolor="#ffffff" align="center"></td><td height="46px" bgcolor="#ffffff" align="center"></td></tr>';
        if (this.config.CR > this.config.NR) {
            for (var i = 0; i < this.config.CR - this.config.NR; i++) {
                //this.config.JNO.append(s);
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
            s.push('<td height="46px" bgcolor="#ffffff" align="center"></td>')
        }
        s.push('</tr>')
        return s.join('');
    },
    //创建日历日期信息
    createCalendarDays: function(isNextMonth, json) {
        var myself = this;
        var obj = isNextMonth ? this.config.JNO : this.config.JCO; //ensure calendar container.
        var sd = 1; //day of date.
        var fd = isNextMonth ? this.SDConfig.NDays : this.SDConfig.CDays; //ensure days of the month.
        //ensure the first day week value.
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
            }

            if (i == 1) {
                s.push(this.createStartEmptyDays(sdOfWeek));
                i = i + sdOfWeek;
            }

            var thisY = isNextMonth ? this.SDConfig.NY : this.SDConfig.CY;
            var thisM = isNextMonth ? this.SDConfig.NM : this.SDConfig.CM;
            if (thisM <= 9) {
                thisM = "0" + thisM;
            }
            var thisD = sd <= 9 ? "0" + sd : sd;
            var thisDate = thisY + "-" + thisM + "-" + thisD;
            var thisDateObj = new Date(Date.parse(thisM + "/" + thisD + "/" + thisY));
            var tdTourstr = '<td height="46px" width="100px" bgcolor="#ffffff" align="center" valign="top">' + json[sd] + '</td>';
            /*
            var tdTourstr = '<td height="46px" bgcolor="#ffffff" align="center" valign="top">' + sd + '<br/>' + json[sd] + '</td>';
            if(this.option.configObj.name=="date"){
            var res = this.option.configObj._IsExistChildToursRule3(thisDateObj);
				
                if(res){
            tdTourstr = '<td height="60px" bgcolor="'+this.personalize.highlight+'" align="center"><font class="sizdate">' + sd + '</font></td>';
            }
            }
            if(this.option.configObj.name=="day"){
            var res = this.option.configObj._IsExistChildToursRule2(thisDateObj);
            if(res){
            tdTourstr = '<td height="60px" bgcolor="'+this.personalize.highlight+'" align="center"><font class="sizdate">' + sd + '</font></td>';
            }
            }
            if(this.option.configObj.name=="week"){
            var res = this.option.configObj._IsExistChildToursRule1(thisDateObj);
            if(res){
            tdTourstr = '<td height="60px" bgcolor="'+this.personalize.highlight+'" align="center"><font class="sizdate">' + sd + '</font></td>';
            }
            }
            */
            s.push(tdTourstr);

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

    }
};
