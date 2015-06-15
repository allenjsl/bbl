<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProvinceAndCity.ascx.cs" Inherits="Web.UserControl.ProvinceAndCity" %>
<!--样式-->
<style type="text/css">
#span_Province_<%=ClientID%> input{width:80px;}
#span_City_<%=ClientID%> input{width:80px;}
</style>

<script type="text/javascript">
    window["<%=ClientID %>"] = {
        _pn$: "#txtProvinceName_<%=ClientID %>",
        _pi$: "#txtProvinceId_<%=ClientID %>",
        _cn$: "#txtCityName_<%=ClientID %>",
        _ci$: "#txtCityId_<%=ClientID %>",
        _parentiframeid: '<%=EyouSoft.Common.Utils.GetQueryStringValue("iframeId") %>',
        _isSingleProvince: false,
        _isSingleCity: false,
        //获取省份信息
        getProvince: function() {
            return { "id": $(this._pi$).val(), "name": $(this._pn$).val() };
        },
        //获取城市信息
        getCity: function() {
            return { "id": $(this._ci$).val(), "name": $(this._cn$).val() };
        },
        //设置省份信息
        setProvince: function(id, name) {
            $(this._pi$).val(id);
            $(this._pn$).val(name);
        },
        //设置城市信息
        setCity: function(id, name) {
            $(this._ci$).val(id);
            $(this._cn$).val(name);
        },
        _openPWindow: function() {
            var self = window["<%=ClientID %>"];
            var _t = self._isSingleProvince ? 'radio' : 'checkbox';
            parent.Boxy.iframeDialog({
                iframeUrl: '/Common/ProvinceAndCity.aspx?s=p&t=' + _t + '&parentiframeid=' + self._parentiframeid + '&wuc=<%=ClientID %>',
                title: "选择省份",
                modal: true,
                width: 600,
                height: 400,
                data: {}
            });
        },
        _openCWindow: function() {
            var self = window["<%=ClientID %>"];
            var _p = self.getProvince();
            var _t = self._isSingleCity ? 'radio' : 'checkbox';
            if (_p.id.length < 1) { alert("请先选择省份"); return; }
            parent.Boxy.iframeDialog({
                iframeUrl: '/Common/ProvinceAndCity.aspx?s=c&t=' + _t + '&parentiframeid=' + self._parentiframeid + '&wuc=<%=ClientID %>',
                title: "选择城市",
                modal: true,
                width: 600,
                height: 400,
                data: {}
            });
        },
        _ready: function() {
            $(this._pn$).bind("click", this._openPWindow);
            $(this._cn$).bind("click", this._openCWindow);
        },
        //初始化 provinceContainer：省份容器ClientID cityContainer：城市容器ClientID isSingleProvince：是否单选省份 isSingleCity：是否单选城市
        replace: function(provinceContainer, cityContainer, isSingleProvince, isSingleCity) {
            this._isSingleCity = isSingleCity;
            this._isSingleProvince = isSingleProvince;
            if (provinceContainer == null || provinceContainer == "undefined" || provinceContainer.length < 1) provinceContainer = "span_Province_<%=ClientID %>";
            if (cityContainer == null || cityContainer == "undefined" || cityContainer.length < 1) cityContainer = "span_City_<%=ClientID %>";

            var s = [];
            s.push('省份：')
            s.push('<input type="text" class="searchinput" name="txtProvinceName_<%=ClientID %>" id="txtProvinceName_<%=ClientID %>" readonly="readonly" />');
            s.push('<input type="hidden" name="txtProvinceId_<%=ClientID %>" id="txtProvinceId_<%=ClientID %>" />');
            s.push('&nbsp;<a onclick="return <%=ClientID %>._openPWindow()" href="###"><img src="/images/sanping_04.gif" alt="选择省份" /></a>');

            $("#" + provinceContainer).html(s.join(''));

            s = [];
            s.push('城市：');
            s.push('<input type="text" class="searchinput" name="txtCityName_<%=ClientID %>" id="txtCityName_<%=ClientID %>" readonly="readonly" />');
            s.push('<input type="hidden" name="txtCityId_<%=ClientID %>" id="txtCityId_<%=ClientID %>" />');
            s.push('&nbsp;<a onclick="return <%=ClientID %>._openCWindow()" href="###"><img src="/images/sanping_04.gif" alt="选择城市" /></a>');

            $("#" + cityContainer).html(s.join(''));

            this._ready();
        }
    };
</script>

<span id="span_Province_<%=ClientID %>"></span>
<span id="span_City_<%=ClientID %>"></span>
