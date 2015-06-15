//汪奇志 2012-02-21 js utils uri
var utilsUri = {
    createUri: function(url, params) {
        if (url == null || url.length < 1) url = window.location.pathname;
        var isHaveParam = false;
        var isHaveQuestionMark = false;
        var questionMark = "?";
        var questionMarkIndex = url.indexOf(questionMark);
        var urlLength = url.length;

        if (questionMarkIndex == urlLength - 1) {
            isHaveQuestionMark = true;
        } else if (questionMarkIndex != -1) {
            isHaveParam = true;
        }

        if (isHaveParam == true) {
            for (var key in params) {
                url = url + "&" + key + "=" + params[key];
            }
        } else {
            if (isHaveQuestionMark == false) {
                url += questionMark;
            }
            for (var key in params) {
                url = url + key + "=" + params[key] + "&";
            }
            url = url.substr(0, url.length - 1);
        }

        return url;
    },
    getUrlParams: function(removeParams) {
        var argsArr = new Object();
        var query = window.location.search;
        query = query.substring(1);
        var pairs = query.split("&");

        for (var i = 0; i < pairs.length; i++) {
            var sign = pairs[i].indexOf("=");
            if (sign == -1) {
                continue;
            }

            var aKey = pairs[i].substring(0, sign);
            var aValue = pairs[i].substring(sign + 1);

            //移除不需要要的键
            var isRemove = false
            for (var j = 0; j < removeParams.length; j++) {
                if (aKey.toLowerCase() == removeParams[j].toLowerCase()) {
                    isRemove = true;
                    break;
                }
            }

            if (isRemove) {
                continue;
            }

            argsArr[aKey] = aValue;
        }

        return argsArr;
    },
    encode: function(url) {
        url = url || "";
        if (url.indexOf("?") == -1) return url;
        var _arr = url.split("?");
        if (_arr.length != 2) return url;

        var argsArr = new Object();
        var query = _arr[1];
        var pairs = query.split("&");

        for (var i = 0; i < pairs.length; i++) {
            var sign = pairs[i].indexOf("=");
            if (sign == -1) {
                continue;
            }

            var aKey = pairs[i].substring(0, sign);
            var aValue = pairs[i].substring(sign + 1);

            argsArr[aKey] = encodeURIComponent(aValue);
        }

        return this.createUri(_arr[0], argsArr);
    }
};
