<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvinceAndCity.aspx.cs" Inherits="Web.Common.ProvinceAndCity" %>
<%--<%@ Register Src="~/UserControl/ProvinceAndCity.ascx" TagName="ProvinceAndCity" TagPrefix="uc1" %>--%>
<!DOCTYPE html>

<html>
<head runat="server">
    <title>省份城市选择</title>
    <script type="text/javascript" src="/js/jquery.1.5.2.min.js"></script>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
    <style type="text/css">
    body {margin:0px;padding:0px;font-size:12px;}
    ul {list-style:none;width:98%;margin:0px auto; padding:0px; margin-top:5px;}
    ul li{list-style:none;width:25%; float:left; height:29px; line-height:29px; padding:0px;}
    ul li.jg{ clear:both; width:100%;height:1px; background:#efefef; font-size:1px;}
    </style>
    
    <script type="text/javascript">        
        //params["t"]:checkbox:复选 radio:单选
        //params["s"]:p:省份选择 c城市选择
        //params["parentiframeid"]:parent window iframe id
        //params["wuc"]:parent window 用户控件编号
        //params["iframeId"]:current iframe id
        
        var params = utilsUri.getUrlParams([]);
        var province = [];

        function init() {
            if (province == null||province.length<1) return;

            if (params["s"] == "p") {
                initProvince();
            } else if (params["s"] == "c") {
                initCity();
            }

            setChecked();
        }

        function initProvince() {
            var s = [];

            for (var i = 0; i < province.length; i++) {
                s.push('<li><label><input type="' + params["t"] + '" id="txt_' + province[i].Id + '" name="txt_" value="' + province[i].Id + '" /><span>' + province[i].ProvinceName + '</span></label></li>');
            }

            $("#ulContainer").append(s.join(''));
        }

        function initCity() {
            var s = [];
            var _parent = getParent();
            var _arr = _parent.getProvince().id.split(',');

            for (var i = 0; i < _arr.length; i++) {
                for (var j = 0; j < province.length; j++) {
                    if (_arr[i] != province[j].Id) continue;
                    if (province[j].CityList == null || province[j].CityList.length < 1) continue;
                    s.push('<li class="jg"></li>')                 
                    for (var k = 0; k < province[j].CityList.length; k++) {
                        s.push('<li><label><input type="' + params["t"] + '" id="txt_' + province[j].CityList[k].Id + '" name="txt_" value="' + province[j].CityList[k].Id + '" /><span>' + province[j].CityList[k].CityName + '</span></label></li>');
                    }
                }
            }
            
            $("#ulContainer").append(s.join(''));
        }

        function btnClick() {
            var ids = [];
            var names = [];
            $("input[name='txt_']:checked").each(function() {
                ids.push($(this).val());
                names.push($(this).next().html());
            });

            if (ids.length < 1) {
                var _name = "城市";
                if (params["s"] == "p") _name = "省份";
                alert("请选择" + _name);
                return;
            }

            var _parent= getParent();

            if (params["s"] == "p") {
                _parent.setProvince(ids.join(','), names.join(','));
            } else if (params["s"] == "c") {
                _parent.setCity(ids.join(','), names.join(','));
            }

            window.parent.Boxy.getIframeDialog(params["iframeId"]).hide();
        }

        function getParent() {
            var _parent = null;
            if (params["parentiframeid"].length < 1) {
                _parent = window.parent.window[params["wuc"]];

            } else {
                _parent = window.parent.Boxy.getIframeWindow(params["parentiframeid"])[params["wuc"]];
            }
            return _parent;
        }

        function setChecked() {
            var _parent = getParent();

            var checkedValue = null;
            if (params["s"] == "p") {
                checkedValue = _parent.getProvince();
            } else if (params["s"] == "c") {
                checkedValue = _parent.getCity();
            }

            if (checkedValue.id.length > 0) {
                var _arr = checkedValue.id.split(',');
                for (var i = 0; i < _arr.length; i++) {
                    $("#txt_" + _arr[i]).attr("checked", "checked");
                }
            }
        }

        function btnClearClick() {
            var _parent = getParent();

            if (params["s"] == "p") {
                _parent.setProvince('', '');
            } else if (params["s"] == "c") {
                _parent.setCity('', '');
            }

            window.parent.Boxy.getIframeDialog(params["iframeId"]).hide();
        }

        $(document).ready(function() {
            init();
            $("#btn").bind("click", btnClick);
            $("#btnClear").bind("click", btnClearClick);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">    
    <ul id="ulContainer">
        
    </ul>
    <div style="clear:both;width:98%; margin:0px auto; height:30px; line-height:30px; text-align:center;">
        <input type="button" value="选择" id="btn" />
        <%--<input type="button" value="清除已选中" id="btnClear" />--%>
    </div>
    <%--    
    test wuc 
    
    <br />
    <br />
    <uc1:ProvinceAndCity runat="server" ID="ProvinceAndCity1" />
    <script type="text/javascript">
        window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
    </script>    
    --%>
    </form>
</body>
</html>
