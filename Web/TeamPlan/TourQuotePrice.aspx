<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TourQuotePrice.aspx.cs"
    Inherits="Web.TeamPlan.TourQuotePrice" %>

<%@ Register Src="../UserControl/PriceControl.ascx" TagName="PriceControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ConProjectControl.ascx" TagName="ConProjectControl"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>无标题文档</title>
    <link href="../css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="../css/boxy.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="../js/jquery.boxy.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 145px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" method="post" enctype="multipart/form-data">
    <table width="880" border="0" align="center" cellpadding="0" cellspacing="1">
        <tr class="odd">
            <th height="30" align="center" class="style1">
                询价单位：
            </th>
            <td width="370">
                &nbsp;
                <input name="txt_quoteCompany" type="text" id="txt_quoteCompany" class="searchinput"
                    readonly="readonly" runat="server" /><asp:HiddenField ID="hid_QuoteCompanyId" runat="server" />
            </td>
            <th width="110" align="center">
                <span style="color: Red">*</span> 线路名称：
            </th>
            <td width="370">
                &nbsp;<uc3:xianluWindow Id="xianluWindow1" runat="server" />
                <span id="routname_valid" style="color: Red"></span>
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="center" class="style1">
                联系人：
            </th>
            <td>
                &nbsp;
                <input name="txt_contactName" type="text" id="txt_contactName" class="searchinput"
                    readonly="readonly" runat="server" />
            </td>
            <th align="center">
                联系电话：
            </th>
            <td>
                &nbsp;
                <input name="txt_contactTel" type="text" id="txt_contactTel" class="searchinput"
                    runat="server" readonly="readonly" />
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="center" class="style1">
                <span style="color: Red">*</span>预计出团日期：
            </th>
            <td>
                &nbsp;
                <input name="txt_BegTime" type="text" id="txt_BegTime" class="searchinput" runat="server"
                    onfocus="WdatePicker()" />
            </td>
            <th align="center">
                人数：
            </th>
            <td>
                &nbsp;
                <input name="txt_peopleCount" type="text" id="txt_peopleCount" class="searchinput"
                    runat="server" valid="isNumber" errmsg="请输入数字" />
                <span id="errMsg_txt_peopleCount" class="errmsg"></span>
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="center" class="style1">
                行程要求：
            </th>
            <td colspan="3">
                <table width="100%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtRoutingNeed" name="txtRoutingNeed" runat="server"></asp:TextBox><br />
                            <input type="file" id="routingFile" name="routingFile" style="float: left" /><div
                                id="div_img" style="float: left">
                                <%if (GetImg(file))
                                  { %>
                                <a id="file_view" target="_blank" href="<%=file %>">查看附件</a><a id="del_img" href="javascript:void(0);"
                                    onclick="del_img()"><img src="/images/fujian_x.gif" /></a><asp:HiddenField ID="hid_img_name"
                                        runat="server" />
                                <asp:HiddenField ID="hid_img_path" runat="server" />
                                <%} %>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="center" class="style1">
                客人要求：
            </th>
            <td colspan="3">
                <table width="100%" border="0" cellspacing="1" cellpadding="2">
                    <tr>
                        <td width="15%" height="28" align="center" valign="middle">
                            项目：
                        </td>
                        <td width="70%" align="center">
                            具体要求：
                        </td>
                        <td width="15%" align="center">
                            操作
                        </td>
                    </tr>
                    <tr>
                        <td height="28" align="center" colspan="3">
                            <uc2:ConProjectControl ID="ConProjectControl1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="center" class="style1">
                特殊要求说明：
            </th>
            <td colspan="3">
                <table width="100">
                    <tr>
                        <td>
                            <textarea class="textareastyle02" style="height: 200px; width: 650px" id="txt_need"
                                name="txt_need" runat="server"></textarea>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <th align="center" class="style1">
                机票折扣：
            </th>
            <td colspan="3">
                <input name="txt_agio" type="text" id="txt_agio" class="searchinput" runat="server" />
                (%)
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="center" class="style1">
                价格组成
            </th>
            <td colspan="3">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td height="25" align="center">
                            &nbsp;<uc1:PriceControl ID="PriceControl1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="center" class="style1">
                备注：
            </th>
            <td colspan="3">
                <textarea style="height: 200px; width: 650px" id="txt_remark" name="txt_remark" runat="server"></textarea>
                &nbsp;
            </td>
        </tr>
        <tr class="even">
            <th align="center" rowspan="2">
                游客信息：<input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
            </th>
            <td height="28" align="center" colspan="4">
                <table cellspacing="0" cellpadding="0" border="0" align="right" width="100%">
                    <tbody>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">游客附件</asp:HyperLink>
                            </td>
                            <td align="right">
                                上传附件：
                                <input type="file" name="fileField" id="fileField" />
                            </td>
                            <td align="center">
                                <a href="/Common/LoadVisitors.aspx?topID=<%=Request.QueryString["iframeId"]%>" id="import">
                                    <img src="/images/sanping_03.gif">
                                    导入</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr class="even">
            <td align="center" colspan="4">
                <div style="width: 100px; color: Red;" id="div_msg">
                </div>
                <table id="cusListTable" cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4"
                    align="center" width="95%" style="margin: 10px 0pt;">
                    <tbody>
                        <tr>
                            <td style="width: 5%" bgcolor="#e3f1fc" align="center">
                                序号
                            </td>
                            <td height="25" bgcolor="#e3f1fc" align="center">
                                姓名
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                类型
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                证件名称
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                证件号码
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                性别
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                联系电话
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                特服
                            </td>
                            <td bgcolor="#e3f1fc" align="center">
                                操作 <a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>
                            </td>
                        </tr>
                        <%=cusHtml%>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="center" class="style1">
                报价结果：
            </th>
            <td>
                <asp:Label ID="lblResult" runat="server" Text="Label"></asp:Label>
                &nbsp;
            </td>
            <th align="center">
                &nbsp;
            </th>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="40" align="center">
            </td>
            <td align="center" class="tjbtn02">
                <asp:LinkButton ID="lkBtn_save" runat="server" OnClick="lkBtn_save_Click" Width="76px"
                    OnClientClick="return checkForm_Save()">保存</asp:LinkButton>
            </td>
            <td align="center" class="tjbtn02">
                <a onclick="view_Div()" id="submit" runat="server" href="javascript:;">报价成功</a>
            </td>
            <td align="center" class="tjbtn02">
                <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()"
                    id="linkCancel" href="javascript:;">返回</a>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hid_dayCount" runat="server" />
    <asp:HiddenField ID="hid_tourCode" runat="server" />
    <asp:HiddenField ID="hid_areaId" runat="server" />
    <asp:HiddenField ID="hid_peopleName" runat="server" />
    <asp:HiddenField ID="hid_peopleId" runat="server" />
    <div style="display: none" id="div_view">
        <table height="200px" style="width: 408px">
            <tr class="odd">
                <td align="right" style="width: 35%">
                    <span style="color: Red">*</span> 团号：
                </td>
                <td>
                    <input id="tourCode" name="tourCode" class="searchinput" type="text" runat="server" />
                    <span style="color: Red" id="errMsg_tourCode"></span>
                </td>
            </tr>
            <tr class="even">
                <td align="right">
                    <span style="color: Red">*</span> 区域路线：
                </td>
                <td>
                    <asp:DropDownList ID="dlArea" runat="server">
                        <asp:ListItem Value="-1">--请选择--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <td align="right">
                    <span style="color: Red">*</span> 计调员：
                </td>
                <td>
                    <asp:DropDownList ID="dlPeople" runat="server">
                        <asp:ListItem Value="-1">--请选择--</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;
                </td>
            </tr>
            <tr class="even">
                <td align="right">
                    <span style="color: Red">*</span>天数：
                </td>
                <td>
                    <input id="txtDayCount" name="tourCode" class="searchinput" type="text" runat="server" />
                    <span style="color: Red" id="dayCount"></span>
                </td>
            </tr>
            <tr class="odd">
                <td align="center" colspan="2">
                    <table>
                        <tr>
                            <td class="tjbtn02" align="center">
                                <asp:LinkButton ID="lkbCreateTour" OnClientClick="return checkForm()" runat="server"
                                    OnClick="lkbCreateTour_Click">提交</asp:LinkButton>
                            </td>
                            <td class="tjbtn02" align="center">
                                <a onclick="boxyClose()" id="A1" href="javascript:;">返回</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script src="/js/kindeditor/kindeditor.js" type="text/javascript"></script>

    <script src="../js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        var box;
        //初始化编辑器
        KE.init({
            id: '<%=txtRoutingNeed.ClientID%>', //编辑器对应文本框id
            width: '625px',
            height: '330px',
            skinsPath: '../js/kindeditor/skins/',
            pluginsPath: '../js/kindeditor/plugins/',
            scriptPath: '../js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
    </script>

    <script type="text/javascript">
$(function(){
            //打开导入窗口
            $("a[id='import']").click(function() {
                parent.Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    width: "853px",
                    height: "514px",
                    async: false,
                    title: "导入",
                    modal: true
                });
                return false;
            });
});
        function isnull(v, defaultValue) {
            if (v == null || !v)
                return defaultValue;
            else
                return v;
        }

        function CreateCusList(array) {
            $("#div_msg").html("正在加载名单...");
            var url = "/ashx/CreateCurList.ashx";
            $.newAjax({
                type: "Post",
                url: url,
                cache: false,
                data: "array=" + encodeURIComponent(array),
                dataType: "html",
                success: function(data) {
                    $("#div_msg").html("");
                    $("#cusListTable tr:last").after(data);

                    OrderEdit.RemoveDisabled();
                }
            });
        }
        var OrderEdit = {

            AddCus: function() {
                OrderEdit.CreateTR();
            },

            DelCus: function($obj) {
                if ($obj.prev("input[name='cusState']").val() == "ADD") {
                    $obj.parent().parent().remove();
                }
                else {
                    $obj.parent().parent().css("display", "none");
                    $obj.prev("input[name='cusState']").val("DEL");
                    $obj.parent().parent().remove();
                }
                var i=1;
                $("td[index]").each(function(){
                $(this).html(i);
                i++;
                })
            },
            
            //数据提交时，却除某些需要提交控件的disabled属性.
            RemoveDisabled:function(){
              $("select[name='cusType']").each(function() {
                    $(this).removeAttr("disabled");
                });
            },


            //检测输入是否是大于等于0整数
            CheckIsInt: function($obj) {
                var re = /^[0-9]([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            },

            //检测总金额输入是否正确
            CheckIsFloat: function($obj) {
                var re = /^[0-9]*.?([0-9])*$/;
                if (re.test($obj.val())) {

                    return false;
                }
                else {
                    return true;
                }
            },

           
            
             open:function(obj){
                    var pos=OrderEdit.getPosition(obj);
                    $("#tbl_last").show().css({left:Number(pos.Left)+"px",top:Number(pos.Top-70)+"px"});
                    return false;
                },
                
                close:function(){
                    $("#tbl_last").hide()
                },
                
                getPosition:function(obj){
                    var objPosition={Top:0,Left:0}
                    var offset = $(obj).offset();
                    objPosition.Left=offset.left;
                    objPosition.Top=offset.top+$(obj).height();
                    return objPosition;
                },

            //打开特服窗口
            OpenSpecive: function(tefuID) {
                parent.Boxy.iframeDialog({
                iframeUrl: "/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid="+tefuID,
                    title: "特服",
                    width: "420px",
                    height: "200px",
                    modal: true,
                    afterHide:function(){
                      //OrderEdit.GetSumMoney();
                   }
               });
                return false;
             },

            CreateTR: function() {
            var i=1;
            $("td[index]").each(function(){
            i++;
            })
                var d = new Date();
                var tefuID=d.getTime();
                var TR = '<tr>'
          + '<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index="0" align=\"center\">'+i+'</td><td height="25" bgcolor="#e3f1fc" align="center">'
            + '    <input type="text" class="searchinput" id="Text1" MaxLength="50"  name="cusName" />'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select1" title="请选择"  name="cusType">'
                    + '<option value=""  selected="selected">请选择</option>'
                    + '<option value="1">成人</option>'
                    + '<option value="2">儿童</option>'
                + '</select>'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select2" name="cusCardType">'
                    + '<option value="0" selected="selected">请选择证件</option>'
                    + '<option value="1">身份证</option>'
                    + '<option value="2">护照</option>'
                    + '<option value="3">军官证</option>'
                    + '<option value="4">台胞证</option>'
                    + '<option value="5">港澳通行证</option>'
                    + '<option value="6">户口本</option>'
                + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                + ' <input type="text" class="searchinput searchinput02" id="text2" onblur="getSex(this)" MaxLength="150" name="cusCardNo">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" name="ddlSex" align="center">'
                 + '<select id="Select3" class="ddlSex" name="cusSex">'
                     + '<option value="0" selected="selected">请选择</option>'
                     + '<option value="1">男</option>'
                     + '<option value="2">女</option>'
                 + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                 + '<input type="text" class="searchinput" id="Text3" MaxLength="50" name="cusPhone">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="6%">'
              + '<input type="hidden" name="txtItem" value="" />'
              + '<input type="hidden" name="txtServiceContent" value="" />'
              + '<input type="hidden" name="ddlOperate" value="" />'
              + '<input type="hidden" name="txtCost" value="" />'
               + '<input id="' + tefuID + '" type="hidden" name="specive" value=""/>'
                 + '<a sign="speService" href="javascript:void(0)" onclick="OrderEdit.OpenSpecive('+tefuID+')">特服</a>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="12%">'
              + '<input type="hidden" name="cusID" value="" />'
             + '<a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>&nbsp;'
             + '<input type="hidden" name="cusState" value="ADD" />'
             + '<a sign="del" href="javascript:void(0)" onclick="OrderEdit.DelCus($(this))">删除</a>'
             + '</td>'
         + '</tr>';
                $("#cusListTable tr:last").after(TR);
            }

        }
        $(function() {
            //初始化编辑器
            KE.create('<%=txtRoutingNeed.ClientID%>', 0); //创建编辑器
            //选择线路区域
            $("#<%=dlArea.ClientID %>").change(function() {
                var areaId = $(this).val();
                $("#<%=dlPeople.ClientID %>").html("<option value='-1'>--请选择--</option>");
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=GetAreaUser&areaId=" + areaId,
                    cache: false,
                    success: function(result) {
                        if (result != "") {
                            var array = result.split("|||");
                            for (var i = 0; i < array.length; i++) {
                                if (array[i] != "") {
                                    var obj = eval('(' + array[i] + ')');
                                    $("#<%=dlPeople.ClientID %>").append("<option value='" + obj.uid + "'>" + obj.uName + "</option>");
                                }
                            }
                        }
                    }
                });
            });

        })
        //删除图片操作
        function del_img() {
            if (confirm("确定要删除么")) {
                $("#div_img").css("display", "none");
                $("#hid_img_path").val("null");
            }
            return false;
        }

        //保存验证
        function checkForm_Save() {
            var form = $("#<%=lkBtn_save.ClientID %>").closest("form").get(0);
            if (checkVali.checkRoute() && ValiDatorForm.validator(form, "span") && checkPhoto() && checkLeaveDate()) {
                /////////////游客验证
             var msg = "";
            var isb = true;
                var hd_IsRequiredTraveller = $("#<%=hd_IsRequiredTraveller.ClientID %>").val();
                if(hd_IsRequiredTraveller!="False"){
                //验证姓名不能为空
                $("[name='cusName']").each(function() {
               
                    if (isnull($(this).val() == "") ) {
                        isb = false;
                        msg += "- *游客姓名不能为空! \n";
                        return false;
                    }
                })
                 //定义身份证正则表达式
                var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                $("[name='cusCardType']").each(function() {
                    var val = $(this).parent().siblings().find("[name='cusCardNo']").val();
                    switch ($(this).val()) {
                        //验证身份证                    
                        case "1":
                            if (!(isIdCard.exec(val)) ) {
                                isb = false;
                                msg += "- *请输入正确的身份证! \n";
                                return false;
                            }
                            break;
                        //其他非空验证                   
                        default:
                            if (val == "" && isa) {
                                isb = false;
                                msg += "- *证件号不能为空! \n";
                                return false;
                            }
                            break;
                    }
                })
                //验证电话
                $("[name='cusPhone']").each(function() {
                    var lastMatch = /^(13|15|18|14)\d{9}$/;
                    var isPhone = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-?)?[1-9]\d{6,7}(\-\d{1,4})?$/;
                    if (isnull($(this).val() == "")) {
                        isb = false;
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
                
                }
            
            if (msg.length > 0) {
                alert(msg);
                 return false;
            }
                return true;
            } else {
                return false;
            }
        }

        //打开团号,计调员窗体
        function view_Div() {
            if ($("#txt_BegTime").val() != "") {
                $.newAjax({
                    type: "POST",
                    url: "/ashx/GenerateTourNumbers.ashx?companyId=<%=SiteUserInfo.CompanyID%>",
                    async: false,
                    data: {
                        datestr: $("#txt_BegTime").val()
                    },
                    success: function(data) {
                        if (data != "") {
                            var resultArr = [];
                            var arr = eval(data);
                            for (var i = 0; i < arr.length; i++) {
                                $("#<%=tourCode.ClientID %>").val(arr[i][1]);
                            }
                        }
                    }
                });
                box = new Boxy($("#div_view"), { closeText: "X", title: "报价", modal: "true", cenete: "true", fixed: "false" });
                box.resize(150, 200);
                return false;
            } else {
                alert("预计出团日期不能为空");
                $("#txt_BegTime").focus();
                return;
            }

        }
        //关闭弹窗
        function boxyClose() {
            if (box != null) {
                box.hide();
            }
            return false;
        }
        //报价验证
        function checkForm() {
            if (checkVali.checkTourCode() && checkTourRepeat() && checkVali.checkAgeaChange() && checkVali.checkCood() && checkVali.checkDayCount()) {
                $("#<%=hid_tourCode.ClientID %>").val($("#<%=tourCode.ClientID %>").val());
                $("#<%=hid_areaId.ClientID %>").val($("#<%=dlArea.ClientID %> option:selected").val());
                $("#<%=hid_peopleId.ClientID %>").val($("#<%=dlPeople.ClientID %> option:selected").val());
                $("#<%=hid_peopleName.ClientID %>").val($("#<%=dlPeople.ClientID %> option:selected").text());
                $("#<%=hid_dayCount.ClientID %>").val($("#<%=txtDayCount.ClientID %>").val());
                return true;
            } else {
                return false;
            }
        }
        //验证
        var checkVali = {
            //验证团号
            checkTourCode: function() {
                if ($("#<%=tourCode.ClientID %>").val().trim() == "") {
                    $("#errMsg_tourCode").html("请输入团号");
                    return false;
                } else {
                    $("#errMsg_tourCode").html("");
                    return true;
                }
            },

            //验证区域选择
            checkAgeaChange: function() {

                if ($("#<%=dlArea.ClientID%>").val() == "-1") {
                    alert("请选择区域路线");
                    return false;
                } else {
                    return true;
                }
            },

            //验证计调员
            checkCood: function() {
                if ($("#<%=dlPeople.ClientID%>").val() == "-1") {
                    alert("请选择计调员");
                    return false;
                } else {
                    return true;
                }
            },
            //验证线路是否有数据
            checkRoute: function() {
                var routName = $('#<%=xianluWindow1.FindControl("txt_xl_Name").ClientID%>').val();
                if (routName != null && routName.trim() != "") {
                    return true;
                } else {
                    $("#routname_valid").html("请选择线路");
                    $('#<%=xianluWindow1.FindControl("txt_xl_Name").ClientID%>').focus();
                    return false;
                }
            },
            //验证天数
            checkDayCount: function() {
                var dayCount = $('#<%=txtDayCount.ClientID %>').val();
                if (dayCount != null && dayCount.trim() != "") {
                    if (dayCount <= 0) {
                        $("#dayCount").html("天数必须大于0");
                        $('#<%=txtDayCount.ClientID %>').focus();
                        return false;
                    } else {
                        return true;
                    }

                } else {

                    $("#dayCount").html("请输入天数");
                    $('#<%=txtDayCount.ClientID %>').focus();
                    return false;
                }
            }

        }

        //验证图片上传格式
        function checkPhoto() {
            var img_type;
            var res = true;
            if ($("#routingFile").val() != "") {
                img_type = $("#routingFile").val().substring($("#routingFile").val().lastIndexOf('.'), $("#routingFile").val().length);
                if (img_type.toLowerCase() != ".jpg" && img_type.toLowerCase() != ".jpeg" && img_type.toLowerCase() != ".png" && img_type.toLowerCase() != ".gif" && img_type.toLowerCase() != ".xls" && img_type.toLowerCase() != ".doc" && img_type.toLowerCase() != ".docx" && img_type.toLowerCase() != ".rar" && img_type.toLowerCase() != ".txt") {
                    alert("请选择正确格式的图片文件");
                    $("#routingFile").focus();
                    res = false;
                    return false;
                }
            }
            return res;
        }

        //验证团号是否存在
        function checkTourRepeat() {
            var res;
            var teamNum = $("#tourCode").val();
            $.newAjax({
                url: "/TeamPlan/AjaxFastVersion.ashx?type=CheckTeamNum&teamNum=" + teamNum,
                type: "GET",
                cache: false,
                async: false,
                success: function(result) {
                    res = true;
                    if (result == "OK") {
                        res = true;
                        $("#errMsg_tourCode").html("");
                    } else {
                        res = false;
                        $("#errMsg_tourCode").html("团号已存在");
                    }
                }
            })
            return res;
        }
        //验证预计出团日期
        function checkLeaveDate() {
            if ($("#txt_BegTime").val() != "") {
                return true;
            } else {
                alert("预计出团日期不能为空");
                $("#txt_BegTime").focus();
                return false;
            }
        }   
            OrderEdit.RemoveDisabled();
            function getSex(obj)
    {
        var val=$(obj).val();
        var tr =$(obj).parent().parent();
        var sex=tr.children().children("select[class='ddlSex']");
        var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                        if (isIdCard.exec(val)) {
                    if (15 == val.length) {// 15位身份证号码
                        if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                            sex.val(1);
                        else
                            sex.val(2);
                    }

                    if (18 == val.length) {// 18位身份证号码
                        if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                            sex.val(1);
                        else
                            sex.val(2);
                    }
                } else {
                sex.val(0);
                }
    }
    </script>

</body>
</html>
