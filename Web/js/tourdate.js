//出团日期日历 汪奇志 2012-04-05
//几点说明：
//1.点击出团时间 以日历方式浮动显示该团下所有未出发计划。
//2.初始化时遍历含有data_selector="tour_date"的a标签，a标签需要自定义data_tourid="<%=计划编号%>"、data_leavedate="出团日期(yyyy-MM-dd)"属性。
//4.页面加载完成后调用tourdate.init()初始化日历;

var tourdate = {
    //riliselector 呈现日历容器的选择器 currentTourId:当前要呈现日历的计划编号 leaveDate:出团日期
    options: { "riliselector": "#tour_rili", "currentTourId": "", "leaveDate": "2012-01-01" }
    //json date to datetime
    , jsonDateToDateTime: function(jsondate) {
        return new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", ""), 10));
    }
    //是否登录
    , _isLogin: function() {
        var ret = false;
        $.ajax({
            type: "GET",
            async: false,
            url: "/shop/t1/default.aspx?doType=isLogin",
            cache: false,
            success: function(response) {
                if (response == "0") {
                    ret = true;
                }
            }
        });

        return ret;
    }
    //报名
    , _baoMing: function(tourid, renshushengyu) {
        if (renshushengyu == 0) { alert("已客满，不能报名。"); return; }
        if (!this._isLogin()) { alert("您尚未登录,请先登录系统!"); return; }

        window.location.href = "/groupend/linelist/lineproductslist.aspx?isdetails=1&tourid=" + tourid;
        return false;
    }
    //获取日历HTML date:起始出团日期
    , _getRiLiHTML: function(date) {
        var year = date.getFullYear();
        var month = date.getMonth();
        var date1 = new Date(year, month, 1);
        //起始天
        var sd = date1.getDate();
        //起始天星期
        var sdOfWeek = date1.getDay();
        date1.setMonth(date1.getMonth() + 1, 0);
        //结束天
        var fd = date1.getDate();

        var s = [];

        date1.setMonth(date1.getMonth() - 1, 1);

        s.push('<ul class="head">');

        s.push('<li style="width:40px;text-align:center;">');

        if (date <= this.options.leaveDate) {
            s.push('<span style="color:#999">上一月</span>');
        } else {
            s.push('<a href="javascript:void(0)" onclick="tourdate._initRiLi(new Date(' + date1.getFullYear() + ',' + date1.getMonth() + ',1))">上一月</a>');
        }
        s.push('</li>');

        s.push('<li style="width:230px;text-align:center;font-weight:bold;">');
        s.push(year + '年' + (month + 1) + '月 全部发团计划');
        s.push('</li>');

        date1.setMonth(date1.getMonth() + 2, 1);

        s.push('<li style="width:40px;text-align:center;">');
        s.push('<a href="javascript:void(0)" onclick="tourdate._initRiLi(new Date(' + date1.getFullYear() + ',' + date1.getMonth() + ',1))">下一月</a>');
        s.push('</li>');

        s.push('<li style="width:40px;text-align:right;">');
        s.push('<a href="javascript:void(0)" onclick="tourdate.close()">关闭</a>&nbsp;&nbsp;');
        s.push('</li>');

        s.push('</ul>');

        s.push('<ul class="rili">');
        s.push('<li class="rilihead">日</li><li class="rilihead">一</li><li class="rilihead">二</li><li class="rilihead">三</li><li class="rilihead">四</li><li class="rilihead">五</li><li class="rilihead">六</li>');

        //创建空白日期
        for (var i = 0; i < sdOfWeek; i++) {
            s.push('<li>&nbsp;</li>')
        }

        do {
            s.push('<li class="riliday" id="rili_day_' + this.options.currentTourId + '_' + +year + '_' + (month + 1) + '_' + sd + '" title="' + year + '-' + (month + 1) + '-' + sd + '">');
            s.push('<p class="day">' + sd + '</p>');
            s.push("</li>");

            sd++;
        } while (sd <= fd)

        s.push('</ul>');

        return s.join('')
    }
    //获取计划数据 date:日期，将获取该日期指定的月份数据
    , _getTours: function(date) {
        var data = [];

        $.ajax({
            type: "POST",
            url: "/shop/t1/default.aspx?doType=getTours",
            data: { "year": date.getFullYear(), "month": date.getMonth() + 1, "tourid": this.options.currentTourId },
            async: false,
            cache: false,
            dataType: "json",
            success: function(response) {
                data = response;
            }
        });

        return data;
    }
    //绑定计划数据 data:相应月份相关数据
    , _bind: function(data) {
        if (data == undefined || data == null || data.length == 0) return;
        var _self = this;
        $.each(data, function() {
            var dayData = this;
            var d = _self.jsonDateToDateTime(dayData.LDate);
            var liselector = "#rili_day_" + _self.options.currentTourId + '_' + d.getFullYear() + "_" + (d.getMonth() + 1) + "_" + d.getDate();

            $(liselector).append('<p style="color:#999">剩' + dayData.RenShuShengYu + '</p>');
            $(liselector).append('<p style="color:#ff6600">￥' + dayData.JiaGeCR + '</p>');

            $(liselector).css({ "background": "#efefef", "line-height": "16px", "cursor": "pointer" });
            $(liselector).hover(function() { $(this).css({ "background": "#9be38e" }); }, function() { $(this).css({ "background": "#efefef" }); })

            $(liselector).click(function() { return _self._baoMing(dayData.TourId, dayData.RenShuShengYu); });
        });
    }
    //加载提示
    , _loading: function() {
        $(this.options.riliselector).html('<p style="height:50px; line-height:50px;">&nbsp;<b>正在加载数据，请稍候....</b></p>');
    }
    //初始化日历 date 日期，日历将呈现该日期指定的月份数据
    , _initRiLi: function(date) {
        var cacheKey = "cache_tour_" + this.options.currentTourId + "_" + date.getFullYear() + "_" + date.getMonth();
        var data = $("div").data(cacheKey);

        if (data == undefined) {
            this._loading();
            data = this._getTours(date);
        }

        $("div").data(cacheKey, data);

        $(this.options.riliselector).html(this._getRiLiHTML(date));

        this._bind(data);
    }
    //关闭日历
    , close: function() {
        $(this.options.riliselector).hide();
        this.options.currentTourId = "";
        this.options.leaveDate = null;
    }
    //点击相应a标签事件，tourid：计划编号 leavedate：出团日期 aobj:a标签
    , _click: function(tourid, leavedate, aobj) {
        if (this.options.currentTourId == tourid) return;

        this.close();

        this.options.currentTourId = tourid;
        var _date = leavedate.split("-");
        //this.options.leaveDate = new Date(_date[0], _date[1], 0);
        this.options.leaveDate = new Date(_date[0], _date[1] - 1, 1);

        //日历呈现位置，浮动窗口左上角起于a标签左下角，当浮动窗口超出屏幕右边区域时，浮动窗口右上角起于a标签右下角。
        var offset = $(aobj).offset();
        if ((offset.left + 350) > $(window).width()) offset.left = offset.left + $(aobj).width() - 350;
        offset.top = offset.top + $(aobj).height() + 2;
        $(this.options.riliselector).css({ "top": offset.top + "px", "left": offset.left + "px" });

        this._loading();
        $(this.options.riliselector).show();
        this._initRiLi(new Date(this.options.leaveDate.getFullYear(), this.options.leaveDate.getMonth(), 1));
    }
    //初始化样式
    , _initStyle: function() {
        var s = [];
        s.push('<style type="text\/css">');
        s.push('.tour_date{z-index: 10; position: absolute; display: none; background:#fff;width:350px; border:1px solid #bbb;FILTER: progid:dXImageTransform.Microsoft.Shadow(color:black,direction:145,strength:3);-webkit-box-shadow: 4px 4px 8px 5px #999;-moz-box-shadow: 4px 4px 8px 5px #999}');
        s.push('.tour_date ul{padding:0px;margin:0px; list-style:none;width:100%;}');
        s.push('.tour_date ul li{padding:0px;margin:0px;list-style:none; float:left;width:50px;height:50px; text-align:center; line-height:50px;}');
        s.push('.tour_date ul.head li{background:#f6f6f6; line-height:50px; height:50px;}');
        s.push('.tour_date ul.rili li.rilihead{background:#bdebee; line-height:40px;}');
        s.push('.tour_date ul.rili li.riliday{}');
        s.push('.tour_date ul.rili li p.day{font-weight:bold;}');
        s.push('<\/style');

        $("body").append(s.join("\n"));
    }
    //初始化
    , init: function() {
        var _self = this;
        _self._initStyle();
        //向页面追加日历容器tour_rili        
        $("body").append('<div id="tour_rili" class="tour_date" />');
        $("a[data_selector='tour_date']").each(function() {
            var _tourid = $(this).attr("data_tourid");
            var _leavedate = $(this).attr("data_leavedate");

            $(this).bind("click", function() { _self._click(_tourid, _leavedate, this); })
        });
    }
};
