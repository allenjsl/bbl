<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublishedPlan.aspx.cs"
    Inherits="Web.xianlu.PublishedPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="/css/sytle.css" />

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript" src="/js/jquery.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <style>
        .left, .title, .dlprice1 .title
        {
            background: none;
            background-color: none;
        }
        .style1
        {
            width: 578px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;"
            id="tb_PublishedPlab">
            <tr class="odd">
                <th width="120px" height="30" align="right">
                    <div style="width: 201px; height: 19px;">
                        <font class="xinghao">*</font>出团日期：</div>
                </th>
                <td class="style1">
                <div style="width: 120px;">
                    <a href="javascript:void(0);" id="linkByDate">请选择出团日期</a>
                    <input type="hidden" id="hidToursNumbers" name="hidToursNumbers" runat="server" errmsg="*请选择出团日期！"
                        valid="required" />
                        </div>
                </td>
                <th width="120px" align="right">
                    <div style="width: 95px; height: 25px;">
                        <font class="xinghao">*</font>计调员：</div>
                </th>
                <td colspan="3" align="left" width="70%">
                    <input type="hidden" id="hd_oprator" name="hd_oprator" value="<%=hdOprator %>" /><select
                        id="sel_oprator" errmsg="*请选择计调员！" valid="required" name="sel_oprator"><option value="">
                            请先选择线路区域</option>
                    </select>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="right">
                    <font class="xinghao">*</font>出发交通：
                </th>
                <td class="style1">
                    <input type="text" name="Txt_StartTraffic" id="Txt_StartTraffic" class="searchinput" style="width:120px"
                        runat="server" errmsg="*请输入出发交通！" valid="required" />
                </td>
                <th align="right">
                    <font class="xinghao">*</font>返回交通：
                </th>
                <td>
                    <input type="text" name="Txt_EndTraffic" id="Txt_EndTraffic" class="searchinput" style="width:120px"
                        runat="server" errmsg="*请输入返回交通！" valid="required" />
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="right">
                    <font class="xinghao">*</font>预控人数：
                </th>
                <td colspan="3">
                    <input type="text" name="Txt_PreControlNumber" id="Txt_PreControlNumber" class="searchinput"
                        runat="server" errmsg="*请输入预控人数！|*请输入正确的预控人数！" valid="required|isNumber" />
                </td>
            </tr>
            <tr class="odd">
                <td colspan="4">
                    <div style="background: url(/images/trbg.png) repeat;">
                        <div class="dlprice1" style="margin: 0; padding: 0; clear: both; background: url(/images/trbg.png) repeat;">
                            <table>
                                <tr>
                                    <td>
                                        <div class="left" style="float: left; width: 150px; margin: 0; padding: 0; height: 60px;
                                            text-align: center;">
                                            <div class="title" style="background-color: #bddcf4; height: 30px; line-height: 30px;">
                                                <font class="xinghao">*</font>报价标准
                                            </div>
                                            <div>
                                                <select name="ddl_price" id="ddl_price" errmsg="*请选择报价标准！" valid="required" runat="server">
                                                </select>
                                            </div>
                                        </div>
                                    </td>
                                    <asp:Repeater ID="rpt_Customer" runat="server">
                                        <ItemTemplate>
                                            <td>
                                                <div class="left" style="float: left; width: 150px; margin: 0; padding: 0; height: 60px;
                                                    text-align: center;">
                                                    <div class="title" style="background-color: #bddcf4; height: 30px; line-height: 30px;">
                                                        <%#Eval("CustomStandName")%>
                                                        <input type="hidden" name="hd_cusStandType" value="<%#Eval("LevType") %>"><input
                                                            type="hidden" name="hd_cusStandId" value="<%#Eval("Id") %>" /><input type="hidden"
                                                                name="hd_cusStandName" value="<%#Eval("CustomStandName") %>" />
                                                    </div>
                                                    <div>
                                                        成人
                                                        <input class="jiesuantext" value="" id="text5" name="txt_cr_price" type="text">
                                                        儿童
                                                        <input class="jiesuantext" value="" id="text6" name="txt_rt_price" type="text">
                                                    </div>
                                                </div>
                                            </td>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <td>
                                        <div class="left">
                                            <div class="title" style="width: 100px; text-align: center;">
                                                操作</div>
                                            <div>
                                                <a href="javascript:void(0)" class="addPrice">
                                                    <img src="/images/tianjiaicon01.gif" alt="" height="16" width="15">
                                                    添加 </a><a href="javascript:void(0)" class="delPrice">
                                                        <img width="15" height="16" src="/images/delicon01.gif" alt="">
                                                        删除 </a>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="clear: both;">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr class="odd">
                <td height="30" colspan="4" align="center">
                    <table width="320" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="40" align="center">
                            </td>
                            <td height="30" align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">保存</asp:LinkButton>
                            </td>
                            <td height="30" align="center" class="tjbtn02">
                                <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();">
                                    关闭</a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        $(document).ready(function() {

            var areaId = queryString("areaid");
            if (areaId != "") {
                $.getJSON("/ashx/getAreaOprator.ashx?areaId=" + areaId, { date: new Date() }, function(r) {
                    $("#sel_oprator").get(0).length = 0;
                    $("#sel_oprator").get(0).options[0] = new Option("请选择", "");
                    for (var i = 0; i < r.length; i++) {
                        $("#sel_oprator").get(0).options[i + 1] = new Option(r[i].ContactName, r[i].UserId);
                    }
                    $("#sel_oprator").val($("#hd_oprator").val());
                });
            } else {
                $("#sel_oprator").get(0).length = 0;
                $("#sel_oprator").get(0).options[0] = new Option("请先选择线路区域", "");
            }


            $("#<%=LinkButton1.ClientID %>").click(function() {
                FV_onBlur.initValid($("#<%=LinkButton1.ClientID %>").closest("form").get(0));


                var form = $(this).closest("form").get(0);

                if (parseInt($("#<%=Txt_PreControlNumber.ClientID %>").val()) < 1) {
                    alert("请输入正确的预控人数!");
                    return false;
                }
                
                var list = new Array();
                var count = 0;
                $("[name='ddl_price']").each(function() {
                    if ($(this).find("option:selected") != "") {
                        list.push($(this).val());
                    }
                });

                var flag = false;

                for (var i = 0; i < list.length; i++) {
                    for (var j = 0; j < list.length; j++) {
                        if (list[i] == list[j] && i != j) {
                            flag = true;
                            break;
                        }
                    }

                }
                if (flag) {
                    alert("不能选择多个相同的报价等级!");
                    count = 0;
                    return false;
                }
                list = null;


                return ValiDatorForm.validator(form, "alert");
            });

            $(".addPrice").bind("click", function() {
                $(this).closest(".dlprice1").clone(true).insertAfter($(this).closest(".dlprice1")).find("input").val("").parents(".dlprice1").find("select").val("-1");
            });
            $(".delPrice").bind("click", function() {
            var lenth=$(this).closest(".dlprice1").parent()[0].children.length;
                if (lenth > 1)
                    $(this).closest(".dlprice1").remove();
            });
            $("#linkByDate").click(function() {
                parent.Boxy.iframeDialog({
                    iframeUrl: "/sanping/SelectChildTourDate.aspx",
                    title: "发布计划日期",
                    modal: true,
                    width: "800px",
                    height: "450px",
                    data: {
                      companyId:<%=SiteUserInfo.CompanyID %>,
                      parentiframeid:"<%=Request.QueryString["iframeid"] %>"
                }
            });
            return false;
        });
    });
        
        
    </script>

    </form>
</body>
</html>
