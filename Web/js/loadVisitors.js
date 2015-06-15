/*
zhangxb,2011-01-26,
用于操作游客信息列表，提供添加，删除游客特性；
游客的特服金额设置 自动关联 总金额
*/
function getAmountInTeFuStr(tefuStr) {
    var query = function(key, str) {
        var uri = str;
        var val = key;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    };
    var isAdd = false, isSub = false;
    var tmp;
    tmp = query("ddlOperate", tefuStr);
    if (tmp == "0") {
        isAdd = true;
        isSub = false;
    } else if (tmp == "1") {
        isAdd = false;
        isSub = true;
    }
    tmp = query("txtCost", tefuStr);
    var isNan = false;
    isNan = isNaN(tmp);
    if (!isNan) {
        if (isAdd) {
            return parseFloat(tmp);
        } else if (isSub) {
            return 0 - parseFloat(tmp);
        } else {
            return 0;
        }
    } else {
        return 0;
    }
}
var loadVisitors = {
    init: function(options) {

        this.options = jQuery.extend(loadVisitors.DEFAULTS, options || {});
        this.container = $("#" + this.options.containerId);
        this.deleteContainer = null;
        this.deleteVisitorCount = 0;

        //create deleteContainer.
        this.deleteContainer = $("<table></table>").hide().appendTo("form");

        if (this.options.data == null) {
            //create first empty visitor.
            this.addVisitor();
        } else {
            var arr = this.options.data;
            var length = arr.length;
            for (var i = 0; i < length; i++) {
                this.addVisitor(arr[i]);
            }
        }
    },
    clear: function() {
        this.container.find("tr:gt(0)").remove();
    },
    deleteVisitor: function(visitorTrObj) {

        this.deleteVisitorCount++;
        var count = this.container.find("tr:gt(0)").length;
        //clone this to deleteContainer.
        var visitorId = visitorTrObj.find("input[name='hidVisitorID']").val();
        if (visitorId != "") {

            var cVisitorTrObj = visitorTrObj.clone();
            cVisitorTrObj.find("select[name='ddlVisitorType']").val(visitorTrObj.find("select[name='ddlVisitorType']").val());
            cVisitorTrObj.find("select[name='ddlCardType']").val(visitorTrObj.find("select[name='ddlCardType']").val());
            cVisitorTrObj.find("select[name='ddlSex']").val(visitorTrObj.find("select[name='ddlSex']").val());
            cVisitorTrObj.find("input[name='isdelete']").val("1");
            cVisitorTrObj.find("input[name='tefu']").val("");
            cVisitorTrObj.appendTo(this.deleteContainer);
        }
        if (count > 1) {
            visitorTrObj.remove();
        } else {
            visitorTrObj.find("input[name='txtVisitorName']").val("");
            visitorTrObj.find("select[name='ddlVisitorType']").val("0");
            visitorTrObj.find("select[name='ddlCardType']").val("0");
            visitorTrObj.find("input[name='txtCardNo']").val("");
            visitorTrObj.find("select[name='ddlSex']").val("0");
            visitorTrObj.find("input[name='txtContactTel']").val("");
            visitorTrObj.find("input[name='tefu']").val("");
            visitorTrObj.find("input[name='hidVisitorID']").val("");
        }
        BindIndex("del", null, 0);
        this._autoCompute();
    },
    /*
    参数 数据  
    索引 代表
    0:姓名 1:类型 2:证件名称 3:证件号码 4:性别 5:联系电话  6:特服  7:游客ID  8:游客状态
    */
    addVisitor: function(arr) {

        //debugger
        var visitorCount = this.container.find("tr:gt(0)").length; //current visitors count.
        var self = this, tmp, isfirstTr = false;
        if (arr !== false && visitorCount == 1) {
            if (this.container.find("tr:gt(0)").eq(0).find("input[name='hidVisitorID']").eq(0).val() == "" &&
              this.container.find("tr:gt(0)").eq(0).find("input[name='txtVisitorName']").eq(0).val() == "") {

                tmp = this.container.find("tr:gt(0)").eq(0);
                BindIndex("add", tmp, 0);
                isfirstTr = true;
            } else {

                tmp = $(this._visitorHtml);
                BindIndex("add", tmp, 1);

            }


        } else {
            tmp = $(this._visitorHtml);
            BindIndex("add", tmp, visitorCount);
        }

        if (isfirstTr == false) {
            tmp.find("a[rel='tefulink']").click(function() {
                var tefuid = $(this).next().attr("id");
                self._openTeFu(tefuid);
                return false;
            }).next("input[type='hidden'][name='tefu']").attr("id", "tefu" + (visitorCount + this.deleteVisitorCount + 1));
        }

        //init visitor data.
        if (arr != null && arr !== false) {

            tmp.find("input[name='txtVisitorName']").val(arr[0]);
            tmp.find("select[name='ddlVisitorType']").val(arr[1]);
            tmp.find("select[name='ddlCardType']").val(arr[2]);
            tmp.find("input[name='txtCardNo']").val(arr[3]);
            tmp.find("select[name='ddlSex']").val(arr[4]);
            tmp.find("input[name='txtContactTel']").val(arr[5]);
            if (arr.length >= 7) {
                tmp.find("input[name='tefu']").val(arr[6]);
            }
            if (arr.length >= 8) {
                tmp.find("input[name='hidVisitorID']").val(arr[7]);
            }
            if (arr.length >= 9) {
                if (arr[8] != "") {
                    tmp.find("span[rel='visitorstate']").html("<br />" + arr[8]);
                    //tmp.find("a[rel='addlink']").remove();
                    tmp.find("a[rel='deletelink']").remove();
                } else {
                    tmp.find("span[rel='visitorstate']").hide();
                }
            }
        }

        if (isfirstTr == false) {
            tmp.find("a[rel='addlink']").click(function() {
                self.addVisitor(false);
            });
            tmp.find("a[rel='deletelink']").click(function() {
                var visitorTrObj = $(this).closest("tr");
                self.deleteVisitor(visitorTrObj);
            });
        }

        if (isfirstTr == false) {
            this.container.append(tmp);
        }

        this._autoCompute();

    },
    getTeFuToTalAmount: function() {
        var totalAmount = 0;
        this.container.find("input[name='tefu']").each(function() {
            var name = $(this).closest("tr").find("input[name='txtVisitorName']").val();
            name = $.trim(name);
            if (name == "") {
                return;
            }
            totalAmount += getAmountInTeFuStr(this.value);
        });
    },
    _openTeFu: function(tefuId) {
        var url = "/Common/SpecialService.aspx";
        var iframeid = this._queryString("iframeid");
        var oldtefuValue = $("#" + tefuId).val();
        parent.Boxy.iframeDialog({ iframeUrl: url, width: '400', height: '200', title: '特服', data: {
            tefuid: tefuId,
            desiframeId: iframeid
        }, afterHide: function() {
            var nowtefuValue = $("#" + tefuId).val();
            if (oldtefuValue != nowtefuValue) {
                loadVisitors._autoCompute();
            }
        }
        });
        return false;
    },
    _queryString: function(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    },
    DEFAULTS: {
        containerId: "tblVisitorList",
        autoComputeToTalAmountHandle: null
    },
    _autoCompute: function() {
        //alert("_autoCompute");
        if (this.options.autoComputeToTalAmountHandle) {
            this.options.autoComputeToTalAmountHandle();
        }
    },
    _visitorHtml: '<tr id="xx">' +
         '<td style=\"width: 5%\" bgcolor=\"#e3f1fc\"  class=\"index\" align=\"center\">1</td>' +
        '<td height="25" align="center" bgcolor="#E3F1FC">' +
            '<input name="txtVisitorName" type="text" class="searchinput">' +
            '<input name="hidVisitorID" type="hidden" />' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<select name="ddlVisitorType">' +
            '<option value="0">-请选择-</option>' +
            '<option value="1">成人</option>' +
            '<option value="2">儿童</option>' +
            '</select>' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<select name="ddlCardType">' +
            '<option value="0">-请选择-</option>' +
            '<option value="1">身份证</option>' +
            '<option value="2">护照</option>' +
            '<option value="3">军官证</option>' +
            '<option value="4">台胞证</option>' +
            '<option value="5">港澳通行证</option>' +
            '<option value="6">户口本</option>' +
            '</select>' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<input name="txtCardNo" class="cardNo" onblur="getSex(this)" type="text" class="searchinput searchinput02">' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<select name="ddlSex">' +
            '<option value="0">-请选择-</option>' +
            '<option value="2">男</option>' +
            '<option value="1">女</option>' +
            '</select>' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<input name="txtContactTel" type="text" class="searchinput">' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<a href="javascript:;" rel="tefulink">特服</a>' +
            '<input type="hidden" name="tefu" />' +
        '</td>' +
        '<td align="center" bgcolor="#E3F1FC">' +
            '<a href="javascript:;" rel="addlink" style="padding-left:5px;">添加</a>' +
            '<a href="javascript:;" rel="deletelink" style="padding-left:5px;">删除</a>' +
            '<span rel="visitorstate"></span>' +
            '<input type="hidden" name="isdelete" value="0" />' +
        '</td>' +
    '</tr>'


};
//
function BindIndex(type, thiss, visitorCount) {
    var i = 1;
    switch (type) {
        case "add":
            $(thiss).find("td.index").html(visitorCount + 1)
            break;
        case "del":
            $("td.index").each(function() {
                $(this).html(i);
                i++;
            })
            break;
    }

}
var visitorChecking = {
    /*ischecking=是否验证false时后面的参数允许为空---txtVisitorName=姓名框的name,ddlCardType=证件类型的name,txtContactTel=电话框的name,txtCardNo=生分证框*/
    isChecking: function(ischecking, txtVisitorName, ddlCardType, txtContactTel, txtCardNo) {
        var isYes = false;
        var msg = "";
        var msgs = "";
        if (ischecking.toString() == "True") {
            msg = checking(txtVisitorName, ddlCardType, txtContactTel, txtCardNo);
            if (msg.length > 0) {
                isYes = false;
            }
            else {
                isYes = true;
            }
            msgs = msg;
        }
        else {
            isYes = true;
        }
        msg = { isYes: isYes, msg: msgs }
        return msg;
    }
}
//游客验证
function checking(txtVisitorName, ddlCardType, txtContactTel, txtCardNo) {
    var msg = "";
    //验证姓名不能为空
    $("[name=" + txtVisitorName + "]").each(function() {
        if ($(this).val() == "") {

            msg += "- *游客姓名不能为空! \n";
            return false;
        }
    })
    //定义身份证正则表达式
    var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
    $("[name=" + ddlCardType + "]").each(function() {
        var val = $(this).parent().siblings().find("[name=" + txtCardNo + "]").val();
        switch ($(this).val()) {
            //验证身份证                                                                                              
            case "1":
                if (!(isIdCard.exec(val))) {

                    msg += "- *请输入正确的身份证! \n";
                    return false;
                }
                break;
            //其他非空验证                                                                                             
            default:
                if (val == "") {

                    msg += "- *证件号不能为空! \n";
                    return false;
                }
                break;
        }
    })
    //验证电话
    $("[name=" + txtContactTel + "]").each(function() {
        var lastMatch = /^(13|15|18|14)\d{9}$/;
        var isPhone = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-?)?[1-9]\d{6,7}(\-\d{1,4})?$/;
        if ($(this).val() == "") {
            msg += "- *联系电话不能为空! \n";
            return false;
        }
        else {
            if (!(lastMatch.exec($(this).val())) && !(isPhone.exec($(this).val()))) {
                msg += "- *请输入正确的联系电话! \n";
                return false;
            }
        }
    })
    return msg;
}