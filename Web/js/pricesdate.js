function createStyleRule(selector, declaration) {
    if (!document.getElementsByTagName || !(document.createElement || document.createElementNS)) return;
    var agt = navigator.userAgent.toLowerCase();
    var is_ie = ((agt.indexOf("msie") != -1) && (agt.indexOf("opera") == -1));
    var is_iewin = (is_ie && (agt.indexOf("win") != -1));
    var is_iemac = (is_ie && (agt.indexOf("mac") != -1));
    if (is_iemac) return; // script doesn't work properly in IE/Mac
    var head = document.getElementsByTagName("head")[0];
    var style = (typeof document.createElementNS != "undefined") ? document.createElementNS("http://www.w3.org/1999/xhtml", "style") : document.createElement("style");
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

function copyDate(date) {
    var d = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    return d;
}

function formatDate(date) {
    var d = new Date(date.getFullYear(), date.getMonth(), 1);
    return d;
}

function compareDate(DateOne, DateTwo) {
    var OneMonth = DateOne.getMonth() + 1;
    var OneDay = DateOne.getDate();
    var OneYear = DateOne.getFullYear();

    var TwoMonth = DateTwo.getMonth() + 1;
    var TwoDay = DateTwo.getDate();
    var TwoYear = DateTwo.getFullYear();

    if (Date.parse(OneMonth + "/" + OneDay + "/" + OneYear) >
	Date.parse(TwoMonth + "/" + TwoDay + "/" + TwoYear)) {
        return 1;
    }
    else if (Date.parse(OneMonth + "/" + OneDay + "/" + OneYear) <
	Date.parse(TwoMonth + "/" + TwoDay + "/" + TwoYear)) {
        return -1;
    } else {
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


var QGD = {
    /*外部调用方法 */
    //初始化日历
    initCalendar: function(option) {
        this.option = $.extend({
            containerId: "calendarContainer"
        }, option || {});
        this.currentDate = this.option.currentDate;

        this.maxDate = copyDate(this.option.currentDate);

        this.currentDate = formatDate(this.currentDate);
        this.maxDate = formatDate(this.maxDate);

        this.maxDate.setMonth(this.maxDate.getMonth() + 5);

        this.SCD = this.option.firstMonthDate;
        this.SND = this.option.nextMonthDate;

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

        //绑定状态事件
        var self = this;
        $("#" + this.option.containerId).find("span[rel='Status']").click(function() {
            if (updateStatus) {
                updateStatus($(this).attr("id"), $(this).attr("tfId"));
            }
            return false;
        });

        //修改事件
        $("#" + this.option.containerId).find("a[rel='updatePrices']").click(function() {
            if (updatePrices) {
                updatePrices($(this).attr("id"), $(this).attr("date"), $(this).attr("tfId"));
            }
            return false;
        });

        //添加事件
        $("#" + this.option.containerId).find("a[rel='addPrices']").click(function() {
            if (addPrices) {
                addPrices($(this).attr("date"));
            }
        });
    }, updateCalendar: function(option) {
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

        //绑定状态事件
        var self = this;
        $("#" + this.option.containerId).find("span[rel='Status']").click(function() {
            if (updateStatus) {
                updateStatus($(this).attr("id"), $(this).attr("tfId"));
            }
            return false;
        });

        //修改事件
        $("#" + this.option.containerId).find("a[rel='updatePrices']").click(function() {
            if (updatePrices) {
                updatePrices($(this).attr("id"), $(this).attr("date"), $(this).attr("tfId"));
            }
            return false;
        });

        //添加事件
        $("#" + this.option.containerId).find("a[rel='addPrices']").click(function() {
            if (addPrices) {
                addPrices($(this).attr("date"));
            }
        });
    },
    /*以下为内部私有方法 */
    html: '<table class="table-rili" width="100%" border="0" cellspacing="0" cellpadding="0">' +
  '<tbody>' + ' <tr><td colspan="7" class="bg"><a href="javascript:" class="left-rilibtn" id="linkPreMonth">上月</a><a id="titleCurrentMonth"></a><a id="linkNextMonth" href="javascript:" class="right-rilibtn">下月</a>成本价应用范围：散客线路</td></tr></tbody></table>' + '<div id="thisMonthCalendar"></div> ',
    p: parent,
    //页面元素配置信息
    elements: {
        //生成的当前月表格id
        cMTable: 'cMonthTable',
        //生成的下一个月表格id
        nMTable: 'nMonthTable',
        //存放当前月表格容器id
        cMTd: "thisMonthCalendar"
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
        //模板团编号
        TI: 0,
        //当前子团信息
        CC: [],
        //父页面盛放子团的jQuery对象
        JPO: null,
        //日历当前月表格jQuery对象
        JCO: null,
        //日历下个月表格jQuery对象
        JNO: null,
        //是否登录
        isLogin: false,
        //子团集合
        Childrens: []
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
            //alert("请指定日历容器");
            return false;
        }

        if (this.firstLoad == undefined || this.firstLoad == false) {
            $("#" + this.option.containerId).html(this.html);
            this.firstLoad = true;
        }
        $("#titleCurrentMonth").html(this.SDConfig.CY + "年" + this.SDConfig.CM + "月");

        var self = this;
        var cDate = copyDate(self.SCD);
        var nDate = copyDate(self.SND);
        //上个月
        $("#linkPreMonth").unbind().click(function() {
            var a = copyDate(cDate);
            cDate.setMonth(cDate.getMonth() - 1);
            var b = copyDate(cDate);
            self.updateCalendar({
                nextMonthDate: a,
                firstMonthDate: b
            });

            if (typeof (self.option.prevfn) == "function") {
                self.option.prevfn();
            }
        });

        //下个月
        $("#linkNextMonth").unbind().click(function() {
            var a = copyDate(nDate);
            nDate.setMonth(nDate.getMonth() + 1)
            var b = copyDate(nDate);
            self.updateCalendar({
                firstMonthDate: a,
                nextMonthDate: b
            });
            if (typeof (self.option.nextfn) == "function") {
                self.option.nextfn();
            }
        });

    },
    //创建日历表格
    createCalendarMonth: function(isNextMonth) {
        var tableId = isNextMonth ? this.elements.nMTable : this.elements.cMTable;
        var s = [];
        s.push('<table class="table-rili" width="100%" border="0" cellspacing="0" cellpadding="0" id="' + tableId + '">');
        s.push('<tr><th align="center"><label for="' + isNextMonth + '0">日</label></th><th align="center"><label for="' + isNextMonth + '1">一</label></th><th align="center"><label for="' + isNextMonth + '2">二</label></th><th  align="center"><label for="' + isNextMonth + '3">三</label></th><th align="center"><label for="' + isNextMonth + '4">四</label></th><th align="center"><label for="' + isNextMonth + '5">五</label></th><th align="center"><label for="' + isNextMonth + '6">六</label></th></tr>');
        s.push('</table>');

        $("#" + (isNextMonth ? this.elements.nMTd : this.elements.cMTd)).html(s.join(''));
        if (isNextMonth) {
            this.config.JNO = $("#" + tableId);
        } else {
            this.config.JCO = $("#" + tableId);
        }

    },
    //创建前面的空白天数
    createStartEmptyDays: function(monthFirstDayOfWeek) {
        var s = [];
        for (var i = 0; i < monthFirstDayOfWeek; i++) {
            s.push('<td class="huibg">&nbsp;</td>')
        }
        return s.join('');
    },
    //创建空行
    _createEmptyRows: function() {
        var s = '<tr><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td><td class="huibg">&nbsp;</td></tr>';
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
            s.push('<td class="huibg">&nbsp;</td>')
        }
        s.push('</tr>')
        return s.join('');
    },
    //创建日历日期信息
    createCalendarDays: function(isNextMonth) {
        var myself = this;
        var obj = isNextMonth ? this.config.JNO : this.config.JCO;
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
            }

            if (i == 1) {
                s.push(this.createStartEmptyDays(sdOfWeek));
                i = i + sdOfWeek;
            }

            var thisDate = new Date(this.SDConfig.CY, this.SDConfig.CM, sd);

            var thisY = this.SDConfig.CY;
            var thisM = this.SDConfig.CM;

            if (thisM <= 9) {
                thisM = "0" + thisM;
            }

            var thisD = sd <= 9 ? "0" + sd : sd;
            var thisDate = thisY + "-" + thisM + "-" + thisD;

            var _pDateStr = "";

            if (this.config.Childrens != null && this.config.Childrens != "") {
                var objJson = eval('(' + this.config.Childrens + ')');
                if (objJson.data != null && objJson.data.length > 0) {
                    for (var j = 0; j < objJson.data.length; j++) {
                        var LeaveDate = objJson.data[j].SDateTime;
                        if (thisDate == LeaveDate) {
                            _pDateStr = "<td><div class='day-style'><ul><li class='topbg'>" + (i - sdOfWeek) + "<a class=\"link2 update-btn\" href=\"javascript:\" rel='updatePrices' id=\"" + objJson.data[j].PricesID + "\"  date=\"" + objJson.data[j].SDateTime + "\" tfId=\"" + objJson.data[j].TrafficId + "\"></a><a href='/print/hkmj/OrderVisitorsListPrint.aspx?tfID=" + objJson.data[j].TrafficId + "&date=" + objJson.data[j].SDateTime + "' target='_blank' class='beizhu-ck' id=\"" + objJson.data[j].PricesID + "\"></a></li><li>成本:<b class='font14green'>" + parseFloat(objJson.data[j].TicketPrices).toFixed(2) + "</b></li>  <li>总:<b class='font14blue'>" + objJson.data[j].TicketNums + "</b>余:<b class='font14red'>" + objJson.data[j].ShengYu + "</b></li><li><a href='javascript:' class='rel1' ><span " + (objJson.data[j].Status == 0 ? "class='font12green'" : "class='fred'") + " rel='Status' id=\"" + objJson.data[j].PricesID + "\" tfId=\"" + objJson.data[j].TrafficId + "\">" + (objJson.data[j].Status == 0 ? "正常" : "停售") + "</span></a></li></ul></div></td>";
                            break;
                        }
                        else {
                            _pDateStr = "<td><div class='day-style'><ul><li class='topbg'>" + (i - sdOfWeek) + "<a class=\"link2 add-btn\" href=\"javascript:\" rel='addPrices'  date='" + thisDate + "'></a><a href='/print/hkmj/OrderVisitorsListPrint.aspx?tfID=&date=" + thisDate + "' target='_blank' class='beizhu-ck' id=\"\"></a></li><li>成本:<b class='font14green'>0</b></li>  <li>总:<b class='font14blue'>0</b>余:<b class='font14red'>0</b></li><li><a href='javascript:' class='rel1' ><span class='font12green' id=\"0\">正常</span></a></li></ul></div></td>";
                        }
                    }
                }
            }
            else {
                _pDateStr = "<td><div class='day-style'><ul><li class='topbg'>" + (i - sdOfWeek) + "<a class=\"link2 add-btn\" href=\"javascript:\" rel='addPrices'  date='" + thisDate + "' ></a><a href='/print/hkmj/OrderVisitorsListPrint.aspx?tfID=&date=" + thisDate + "' target='_blank' class='beizhu-ck' id=\"\"></a></li><li>成本:<b class='font14green'>0</b></li>  <li>总:<b class='font14blue'>0</b>余:<b class='font14red'>0</b></li><li><a href='javascript:' class='rel1' ><span class='font12green' id=\"0\">正常</span></a></li></ul></div></td>";
            }

            s.push(_pDateStr);

            if ((i) % (7) == 0) { s.push('</tr>'); }

            if (isNextMonth) {
                this.config.NI = 9 + sdOfWeek;
            } else {
                this.config.CI = 9 + sdOfWeek;
            }

            sd++;
            i++;
        } while (sd <= fd)

        s.push(this.createEndEmptyDays(7 - (i - 1) % 7));
        obj.append(s.join(''));

    }
};

