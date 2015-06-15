<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SingUp.aspx.cs" Inherits="Web.GroupEnd.FitHairDay.SingUp" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/back.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="880" border="0" align="center" cellpadding="2" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="99" align="center">
                    线路名称：
                </th>
                <td width="233" align="center">
                    <asp:Literal ID="lit_RouteName" runat="server"></asp:Literal>
                </td>
                <th width="120" align="center">
                    线路类型：
                </th>
                <td width="146" align="center">
                    <asp:Literal ID="lit_AreaName" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr class="even">
                <th height="55" align="center">
                    <span style="color: Red">*</span> 结算价：
                </th>
                <td colspan="4" align="left" valign="middle">
                    <input type="hidden" id="hd_PriceStandId" name="hd_PriceStandId" runat="server" />
                    <input type="hidden" id="hd_cr_price" name="hd_cr_price" runat="server" />
                    <input type="hidden" id="hd_rt_price" name="hd_rt_price" runat="server" />
                    <input type="hidden" id="hd_LevelID" name="hd_LevelID" runat="server" />
                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                        <td colspan="3" align="left">
                            <%if (sinfo == null)
                              {%>
                            <div style="width: 100%">
                                <div class="" style="width: 100%; background: none repeat scroll 0 0 #E3F1FC; clear: both;
                                    float: left; margin: 0; padding: 0; width: 760px;">
                                    <div class="left">
                                        <div>
                                            <input type="hidden" value="" /><span id="spStandName" name="StandName"><span></div>
                                    </div>
                                    <asp:Repeater ID="rpt_Customer" runat="server">
                                        <ItemTemplate>
                                            <div class="even" style="float: left; height: 60px; margin: 0; padding: 0; text-align: center;
                                                width: 180px">
                                                <div class="" style="background: none repeat scroll 0 0 #E3F1FC;">
                                                    <%#Eval("CustomStandName")%><input type="hidden" name="hd_cusStandType" value="<%#Eval("LevType") %>"><input
                                                        type="hidden" name="hd_cusStandId" value="<%#Eval("Id") %>" /><input type="hidden"
                                                            name="hd_cusStandName" value="<%#Eval("CustomStandName") %>" /></div>
                                                <div>
                                                    成人价： <span name="sp_cr_price"></span>儿童价： <span name="sp_et_price"></span>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <%}
                              else
                              {
                                  for (int i = 0; i < sinfo.Count; i++)
                                  {
                            %>
                            <div style="width: 100%;">
                                <div style="background: none repeat scroll 0 0 #E3F1FC; clear: both; float: left;
                                    margin: 0; padding: 0; width: 760px;" class="dlprice">
                                    <div class="left even">
                                        <div style="background: none repeat scroll 0 0 #E3F1FC;">
                                            <input class="standardclass" type="hidden" value="<%=sinfo[i].StandardId %>" name="hd_PriceStandName" /><span
                                                id="StandName" name="StandName"><%=sinfo[i].StandardName %><span>
                                        </div>
                                    </div>
                                    <%foreach (var v in sinfo[i].CustomerLevels)
                                      {
                                    %>
                                    <div class="even" style="float: left; height: 60px; margin: 0; padding: 0; text-align: center;
                                        width: 180px">
                                        <div style="background: none repeat scroll 0 0 #E3F1FC;">
                                            <%=v.LevelName%>
                                            <input type="hidden" name="hd_cusStandType" value="<%=(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)(v.LevelType)) %>">
                                            <input type="hidden" name="hd_cusStandId" value="<%=v.LevelId %>" />
                                            <input type="hidden" name="hd_cusStandName" value="<%=v.LevelName%>" />
                                        </div>
                                        <div>
                                            <input type="radio" name="radio" class="radio_select" id="radio" value="<%=v.LevelId %>" />
                                            成人价：<span name="sp_cr_price"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(v.AdultPrice )%></span>
                                            儿童价：<span name="sp_et_price"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(v.ChildrenPrice) %></span>
                                        </div>
                                    </div>
                                    <%} %>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <%}
                              } %>
                        </td>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    人数：
                </th>
                <td align="left">
                    <span style="color: Red">*</span>成人：<input name="TxtAdultPrice" type="text" id="TxtAdultPrice"
                        class="searchinput" valid="required|isNumber" errmsg="请输入成人数|请输入数字" />
                    <span id="errMsg_TxtAdultPrice" class="errmsg"></span>儿童：<input name="TxtChildrenPrice"
                        type="text" id="TxtChildrenPrice" class="searchinput" />
                </td>
                <th align="center">
                    <span style="color: Red">*</span> 出团日期：
                </th>
                <td align="center">
                    <input name="TxtSartTime" type="text" id="TxtSartTime" class="searchinput" onfocus="WdatePicker();"
                        valid="required" errmsg="请选择出团日期!" />
                    <span id="errMsg_TxtSartTime" class="errmsg"></span>
                </td>
            </tr>
            <tr class="even">
                <th rowspan="2" align="center">
                    游客名单：
                </th>
                <td colspan="3" align="right">
                    <table width="45%" border="0" align="right" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="85%" align="right">
                                上传附件：<input type="file" name="fuiLoadAttachment" id="fuiLoadAttachment" />
                            </td>
                            <td align="center">
                                <uc1:LoadVisitors ID="LoadVisitors1" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <td colspan="3" align="center">
                    <table width="100%" border="0" align="center" id="tblVisitorList" cellpadding="0"
                        cellspacing="1" bgcolor="#BDDCF4" style="margin: 10px 0;">
                        <tr>
                            <td height="5%" align="center" bgcolor="#E3F1FC">
                                编号
                            </td>
                            <td height="25" align="center" bgcolor="#E3F1FC">
                                姓名
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                类型
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                证件名称
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                证件号码
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                性别
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                联系电话
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                特服
                            </td>
                            <td align="center" bgcolor="#E3F1FC">
                                操作
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th height="42" align="center">
                    特殊要求说明：
                </th>
                <td colspan="3" align="left">
                    <textarea class="textareastyle02" rows="5" cols="45" runat="server" id="txt_Special"
                        name="txt_Special"></textarea>
                </td>
            </tr>
            <tr>
                <th colspan="4" align="center">
                    <table width="320" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td height="40" align="center">
                            </td>
                            <td align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton1" CommandName="submit" runat="server" OnClick="LinkButton1_Click"
                                    OnClientClick="return checkForm()">提交申请</asp:LinkButton>
                            </td>
                            <td height="40" align="center" class="tjbtn02">
                                <a href="javascript:void(0);" id="A1" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()">
                                    关闭</a>
                            </td>
                        </tr>
                    </table>
                </th>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        $(document).ready(function() {
            loadVisitors.init();
            $("input[name='radio']").click(function() {
                var that = $(this);
                $("#hd_cr_price").val($(this).next("[name='sp_cr_price']").html());
                $("#hd_rt_price").val($(this).nextAll("[name='sp_et_price']").html());
                //报价等级编号
                $("#<%=hd_LevelID.ClientID %>").val(that.parent().parent().find("[name='hd_cusStandId']").val());
                //报价标准编号
                $("#<%=hd_PriceStandId.ClientID %>").val(that.parent().parent().parent().parent().parent().find(".standardclass").val());
            });
        });
        //根据用户输入的身份证号判断性别
        function getSex(obj) {
            var val = $(obj).val();
            var tr = $(obj).parent().parent();
            var sex = tr.children().children("select[name='ddlSex']");
            var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
            if (isIdCard.exec(val)) {
                if (15 == val.length) {// 15位身份证号码
                    if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                        sex.val(2);
                    else
                        sex.val(1);
                }

                if (18 == val.length) {// 18位身份证号码
                    if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                        sex.val(2);
                    else
                        sex.val(1);
                }
            } else {
                sex.val(0);
            }
        }
        //判断方法
        function checkForm() {
            var form = $("#<%=LinkButton1.ClientID %>").closest("form").get(0);
            if (ValiDatorForm.validator(form, "span") && checkType()) {
                return true;
            } else {
                return false;
            }
        }
        function CheckNumber() {
            if (parseFloat($("TxtAdultPrice").val()) <= 0) {
                alert("请填写大于0的整数!");
                return false;
            }
        }
        //判断结算方式
        function checkType() {
            var count = 0;
            $(".radio_select").each(function() {
                if ($(this).attr("checked") == true) {
                    count++;
                }
            })
            if (count != 0) {
                return true;
            } else {
                alert("请选择一种结算方式");
                return false;
            }
        }
    </script>

    </form>
</body>
</html>
