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
var ToursNumberSeparator = "{;#$%}";
var ToursNumberObjSeparator = ",";
var ToursNumberObj = function(date,tournumber){
    this.date = copyDate(date);
    this.tourNumber = tournumber;
};
ToursNumberObj.prototype.toString = function(){
    var s = "";
    var year = this.date.getFullYear().toString();
    var month = this.date.getMonth()+1+"";
    var day = this.date.getDate().toString();
    
    s+= year+"-"+month+"-"+day+ToursNumberSeparator+this.tourNumber;
    
    return s;  
};
ToursNumberObj.prototype.getDateString = function(){
    var s = "";
    var year = this.date.getFullYear().toString();
    var month = this.date.getMonth()+1+"";
    var day = this.date.getDate().toString();
    
    s+= year+"-"+month+"-"+day;
    
    return s;  
}
function convertToursNumberStr(s){
    var splitStr = ToursNumberSeparator;
    var arr = s.split(",");
    var tarr,dates,tournumber,tmps;
    var resultArr = [];
    for(var i=0;i<arr.length;i++){
        tarr = arr[i].split(splitStr);
        dates = tarr[0];
        tournumber = tarr[1];
        var tmpArr = dates.split("-");
        
        resultArr.push(new ToursNumberObj(new Date(Date.parse(tmpArr[1]+"/"+tmpArr[2]+"/"+tmpArr[0])),tournumber));
    }
    
    return resultArr;
}
function GetChildToursNumberList(configObj,companyId){
    var beginDate = copyDate(configObj.beginDate);
    var endDate = copyDate(configObj.endDate);
    var cdate = copyDate(configObj.beginDate);
    var arr = [];
    var arrStr = "";
    var isExists = false;
    while( compareDate(cdate,endDate)<=0 ){
        if(configObj.name=="date"){
            if(configObj._IsExistChildToursRule3(cdate)){
                isExists = true;
            }
        }
        if(configObj.name=="day"){
            if(configObj._IsExistChildToursRule2(cdate)){
                isExists = true;
            }
        }
        if(configObj.name=="week"){
            if(configObj._IsExistChildToursRule1(cdate)){
                isExists = true;
            }
        }
        if(isExists){
            arr.push(encodeURIComponent(cdate.getFullYear()+"-"+(cdate.getMonth()+1)+"-"+cdate.getDate()));
        }
        cdate.setDate(cdate.getDate()+1);
        isExists = false;
    }
    //companyId = "1";
    var resultObj = {
        toString:function(){
            return "";
        }
    };
    if(arr.length>0){
        jQuery.newAjax({
            type:"POST",
            url:"/ashx/GenerateTourNumbers.ashx?companyId="+companyId,
            async:false,
            data:{
                datestr:arr.join(",")
            },
            success:function(data){
                if(data!=""){
                    var resultArr = [];
                    var arr = eval(data);
                    for(var i=0;i<arr.length;i++){
                        resultArr.push(arr[i][0]+ToursNumberSeparator+arr[i][1]);
                    }
                    resultObj={
                        toString:function(){
                            return resultArr.join(ToursNumberObjSeparator);
                        }
                    }
                }
            }
        });
    }
    return resultObj;
}
var ChildToursRule1={
    name:"week",
    beginDate:null,
    endDate:null,
    one:{
        show:false,
        value:0
    },
    two:{
        show:false,
        value:1
    },
    three:{
        show:false,
        value:2
    },
    four:{
        show:false,
        value:3
    },
    five:{
        show:false,
        value:4
    },
    six:{
        show:false,
        value:5
    },
    seven:{
        show:false,
        value:6
    },
	_IsExistChildToursRule1:function(date){
        date = copyDate(date);
        var beginDate = copyDate(this.beginDate);
        var endDate = copyDate(this.endDate);
        var isExist = false;
        
        var res1 = compareDate(date,beginDate);
        var res2 = compareDate(date,endDate);
        
        if(res1>=0 && res2<=0){
            var weekValue = date.getDay();
            switch(weekValue){
                case this.one.value:
                isExist = this.one.show;
                break;
                case this.two.value:
                isExist = this.two.show;
                break;
                case this.three.value:
                isExist = this.three.show;
                break;
                case this.four.value:
                isExist = this.four.show;
                break;
                case this.five.value:
                isExist = this.five.show;
                break;
                case this.six.value:
                isExist = this.six.show;
                break;
                case this.seven.value:
                isExist = this.seven.show;
                break;
            }
        }
        
        return isExist;
    },
	getToursCount:function(){
		var i = 0;
        var beginDate = copyDate(this.beginDate);
        var endDate = copyDate(this.endDate);
        var cdate = copyDate(beginDate);
        while( compareDate(cdate,endDate)<=0 ){
            if(this._IsExistChildToursRule1(cdate)){
                i++;
            }
            cdate.setDate(cdate.getDate()+1);
        }
        return i;
	}
};
var ChildToursRule2={
    name:"day",
    beginDate:null,
    endDate:null,
    days:0,
	getToursCount:function(){
	var i = 0;
        var beginDate = copyDate(this.beginDate);
        var endDate = copyDate(this.endDate);
        var days = this.days+1;
        var cdate = copyDate(beginDate);
        while( compareDate(cdate,endDate)<=0 ){
            i++;
            cdate.setDate(cdate.getDate()+days);
        }
        return i;
	},
	_IsExistChildToursRule2:function(date){
        date = copyDate(date);
        var beginDate = copyDate(this.beginDate);
        var endDate = copyDate(this.endDate);
        var days = this.days+1;
        var isExist = false;
        
        var dateArr = [];
        dateArr.push(beginDate);
        
        var cdate = copyDate(beginDate);
        cdate.setDate(cdate.getDate()+days);
        
        
        while( compareDate(cdate,endDate)<=0 ){
            dateArr.push(copyDate(cdate));
            cdate.setDate(cdate.getDate()+days);
        }

        var i=0;
        for(i=0;i<dateArr.length;i++){
            if(compareDate(dateArr[i],date)==0){
                isExist = true;
                break;
            }
        }
        
        return isExist;
    }
};
var ChildToursRule3={
    name:"date",
    beginDate:null,
    endDate:null,
	getToursCount:function(){
		var i = 0;
        var beginDate = copyDate(this.beginDate);
        var endDate = copyDate(this.endDate);
        var cdate = copyDate(beginDate);
        while( compareDate(cdate,endDate)<=0 ){
            i++;
            cdate.setDate(cdate.getDate()+1);
        }
        return i;
	},
	_IsExistChildToursRule3:function(date){
        var res1 = compareDate(date,this.beginDate);
        var res2 = compareDate(date,this.endDate);
        var isExist = false;
        
         if(res1>=0 && res2<=0){
            isExist = true;
         }
         
         return isExist;
    }
};
var ChildTours = {
    //个性化设置
    personalize: {wbkgc:"#e3d596",highlight:"#9be38e"},
    /*外部调用方法 */
    //初始化日历
    initCalendar: function(option) {
        this.option = $.extend({
            containerId: "calendarContainer",
            configObj:{}
        }, option || {});
        this.currentDate = this.option.configObj.beginDate;
        this.maxDate = copyDate(this.option.configObj.endDate);

        this.currentDate = formatDate(this.currentDate);
        this.maxDate = formatDate(this.maxDate);

        this.SCD =  copyDate(this.currentDate)
        this.SND = copyDate(this.currentDate);
        this.SND.setMonth(this.SND.getMonth()+1);

        this.SCD = formatDate(this.SCD);
        this.SND = formatDate(this.SND);

        this._initCalendarBasic();
        this._createCalendarContainer();
        //初始化当前月日历
        this.createCalendarMonth(false);
        this.createCalendarDays(false);

        //初始化下月日历
        this.createCalendarMonth(true);
        this.createCalendarDays(true);

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
        this.createCalendarDays(false);

        //初始化下月日历
        this.createCalendarMonth(true);
        this.createCalendarDays(true);

        //创建空行
        this._createEmptyRows();

        //绑定订单事件
        var self = this;
        $("#" + this.option.containerId).find("a[rel='addorder']").click(function() {
            if (AddOrder) {
                AddOrder($(this).attr("id"));
            }
            return false;
        });

    },
    /*以下为内部私有方法 */
    html: '<table class="calendarTable"   width="100%" cellspacing="0" cellpadding="0">' +
  '<tbody><tr id="trSixMonth">' +
    '<td height="25" align="left" style="padding: 0px;"><table class="mbkgurl" width="100%" cellspacing="0" cellpadding="0" border="0" style="height:25px" ><tr><td width="45%" align="left">&nbsp;<a href="javascript:void(0);" id="linkPreMonth"><strong>&lt;&lt;上一个月</strong></a></td><td width="55%" align="left"><strong id="titlePreMonth"></strong></td></tr></table></td><td width="10"></td>' +
    '<td height="25" align="right" style="padding: 0px;"><table class="mbkgurl" width="100%" cellspacing="0" cellpadding="0" border="0" style="height:25px" ><tr><td width="45%" align="right">&nbsp;<strong id="titleNextMonth"></strong></td><td width="55%" align="right"><a href="javascript:void(0);" id="linkNextMonth"><strong>下一个月&gt;&gt;</strong></a></td></tr></table></td>' +
  '</tr>' +
  '<tr>' +
    '<td width="355" valign="top" ><div id="thisMonthCalendar"></div></td><td width="10" ></td>' +
    '<td width="355" valign="top"><div id="nextMonthCalendar"></div></td>' +
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
        $("#titleNextMonth").html(this.SDConfig.NY + "年" + this.SDConfig.NM + "月");
        var self = this;
        var cDate = copyDate(self.SCD);
        var nDate = copyDate(self.SND);
        var result = compareDate(self.SCD, this.currentDate);
        if (result == -1 || result == 0) {
            $("#linkPreMonth").css("visibility", "hidden");
        } else {
            $("#linkPreMonth").css("visibility", "visible").unbind().click(function() {
                var a = copyDate(cDate);
                cDate.setMonth(cDate.getMonth() - 1);
                var b = copyDate(cDate);
                self.updateCalendar({
                    nextMonthDate: a,
                    firstMonthDate: b
                });
            });
        }
        result = compareDate(this.SND, this.maxDate);
        if (result == 1 || result == 0) {
            $("#linkNextMonth").css("visibility", "hidden");
        } else {
            $("#linkNextMonth").css("visibility", "visible").unbind().click(function() {
                var a = copyDate(nDate);
                nDate.setMonth(nDate.getMonth() + 1)
                var b = copyDate(nDate);
                self.updateCalendar({
                    firstMonthDate: a,
                    nextMonthDate: b
                });
            });
        }
    },
    //创建日历表格
    createCalendarMonth: function(isNextMonth) {
        var myself = this;
        var tableId = isNextMonth ? this.elements.nMTable : this.elements.cMTable;
        var s = [];
        s.push('<table width="100%"  class="datacol"  cellpadding="0" cellspacing="0" id="' + tableId + '"   style="border-style: solid;border-width: 1px;" cellpadding="0" cellspacing="0"  border="1"  bordercolor="#E3E3E3">');
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
            s.push('<td height="60px" bgcolor="#ffffff" align="center"></td>')
        }
        return s.join('');
    },
    //创建空行
    _createEmptyRows: function() {
        var s = '<tr ><td height="60px" bgcolor="#ffffff" align="center">&nbsp;</td><td height="60px" bgcolor="#ffffff" align="center">&nbsp;</td><td height="60px" bgcolor="#ffffff" align="center"></td><td height="60px" bgcolor="#ffffff" align="center"></td><td height="60px" bgcolor="#ffffff" align="center"></td><td height="60px" bgcolor="#ffffff" align="center"></td><td height="60px" bgcolor="#ffffff" align="center"></td></tr>';
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
            s.push('<td height="60px" bgcolor="#ffffff" align="center"></td>')
        }
        s.push('</tr>')
        return s.join('');
    },
    //创建日历日期信息
    createCalendarDays: function(isNextMonth) {
        var myself = this;
        var obj = isNextMonth ? this.config.JNO : this.config.JCO;//ensure calendar container.
        var sd = 1;//day of date.
        var fd = isNextMonth ? this.SDConfig.NDays : this.SDConfig.CDays;//ensure days of the month.
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
            var thisDateObj = new Date(Date.parse(thisM+"/"+thisD+"/"+thisY));
            var tdTourstr = '<td height="60px" bgcolor="#ffffff" align="center"><font class="sizdate">' + sd + '</font></td>';
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
