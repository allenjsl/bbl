<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceControl.ascx.cs"
    Inherits="Web.UserControl.PriceControl" %>
<table cellspacing="1" cellpadding="0" border="0" width="100%" id="tblPrice">
    <tbody>
        <tr class="odd">
            <td height="25" align="center" width="15%" rowspan="2">
                项目
            </td>
            <td align="center" width="20%" rowspan="2">
                接待标准
            </td>
            <td align="center" colspan="3" width="24%">
                地接报价
            </td>
            <td align="center" colspan="3" width="24%">
                &nbsp;&nbsp; 我社报价
            </td>
            <td align="center" width="15%" rowspan="2">
                <a href="javascript:void(0);" onclick="PriceControl.AddPrice()">
                    <img height="16" width="15" src="/images/tianjiaicon01.gif" alt="" />
                    添加</a>
            </td>
        </tr>
        <tr class="odd">
            <td align="center">
                单价
            </td>
            <td align="center">
                人数
            </td>
            <td align="center">
                总计
            </td>
            <td width="5%" align="center">
                单价
            </td>
            <td width="5%" align="center">
                人数
            </td>
            <td align="center">
                总计
            </td>
        </tr>
        <%if (this.SetList != null && this.SetList.Count > 0)
          {%>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <tr class="even">
                    <td height="25" align="center">
                        <select id="selectPrice_<%#Container.ItemIndex+1 %>" name="selectPrice">
                            <option value="<%#Eval("ServiceType") %>">
                                <%#Eval("ServiceType") %></option>
                        </select>
                    </td>
                    <td align="center">
                        <input type="text" class="searchinput searchinput02" id="txtStandard_<%#Container.ItemIndex+1 %>"
                            name="Standard" value="<%#Eval("Service") %>">
                    </td>
                    <td width="5%" align="center">
                        <input type="text" class="djcss" style="width: 90%; border: solid 1px #93b7ce" id="djPrice_<%#Container.ItemIndex+1 %>"
                            name="oneDjPrice" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToInt32(Eval("LocalUnitPrice")).ToString("0.00"))%>" />
                    </td>
                    <td width="5%" align="center">
                        <input type="text" class="djcss" style="width: 90%; border: solid 1px #93b7ce" id="djCount_<%#Container.ItemIndex+1 %>"
                            value="<%#Eval("LocalPeopleNumber") %>" name="oneDjCount" />
                    </td>
                    <td align="center">
                        <input type="text" class="searchinput" id="txtDjPrice_<%#Container.ItemIndex+1 %>"
                            name="txtDjPrice" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("LocalPrice")).ToString("0.00")) %>">
                    </td>
                    <td width="5%" align="center">
                        <input type="text" class="wscss" style="width: 90%; border: solid 1px #93b7ce" id="wsPrice_<%#Container.ItemIndex+1 %>"
                            name="oneWsPrice" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToInt32(Eval("SelfUnitPrice")).ToString("0.00"))%>" />
                    </td>
                    <td width="5%" align="center">
                        <input type="text" class="wscss" style="width: 90%; border: solid 1px #93b7ce" id="wsCount_<%#Container.ItemIndex+1 %>"
                            name="oneWsCount" value="<%#Eval("SelfPeopleNumber") %>" />
                    </td>
                    <td align="center">
                        <input type="text" class="searchinput" id="txtWsPrice_<%#Container.ItemIndex+1 %>"
                            name="txtWsPrice" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("SelfPrice")).ToString("0.00")) %>">
                    </td>
                    <td align="center">
                        <a href="javascript:void(0);" onclick="PriceControl.DeletePrice(this)">
                            <img height="14" width="14" alt="" src="/images/delicon01.gif" border="0" />删除</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <%}
          else
          { %>
        <tr class="even">
            <td align="center">
                <select id="select1" name="selectPrice">
                    <option value="-1">--请选择--</option>
                </select>
            </td>
            <td align="center">
                <input type="text" class="searchinput searchinput02" id="txtStandard_1" name="Standard" />
            </td>
            <td width="5%" align="center">
                <input type="text" class="djcss" style="width: 90%; border: solid 1px #93b7ce" id="djPrice_1"
                    name="oneDjPrice" value="" />
            </td>
            <td width="5%" align="center">
                <input type="text" class="djcss" style="width: 90%; border: solid 1px #93b7ce" id="djCount_1"
                    name="oneDjCount" value="" />
            </td>
            <td align="center">
                <input type="text" class="searchinput" id="txtDjPrice_1" name="txtDjPrice" />
            </td>
            <td width="5%" align="center">
                <input type="text" class="wscss" style="width: 90%; border: solid 1px #93b7ce" id="wsPrice_1"
                    name="oneWsPrice" value="" />
            </td>
            <td width="5%" align="center">
                <input type="text" class="wscss" style="width: 90%; border: solid 1px #93b7ce" id="wsCount_1"
                    name="oneWsCount" value="" />
            </td>
            <td align="center">
                <input type="text" class="searchinput" id="txtWsPrice_1" name="txtWsPrice" />
            </td>
            <td align="center">
                <a href="javascript:void(0);" onclick="PriceControl.DeletePrice(this)">
                    <img height="14" width="14" alt="" src="/images/delicon01.gif" border="0" />删除</a>
            </td>
        </tr>
        <%} %>
    </tbody>
</table>
<table cellspacing="1" cellpadding="0" border="0" width="100%" id="tblDown">
    <tr class="odd">
        <td height="2" align="center" width="15%">
        </td>
        <td align="center" width="20%">
        </td>
        <td align="center" width="24%">
        </td>
        <td align="center" width="24%">
        </td>
        <td align="center" width="15%">
        </td>
    </tr>
    <tr class="even">
        <td height="25" align="center">
        </td>
        <td colspan="3" align="justify">
            <div style="text-align: left; float: left;display:<%=NumConfig==1?"block":"none"%>;">
                <span style="margin-right: 0px;">成人单价合计：<input type="text" class="searchinput" name="txt_crPrice"
                    id="txt_crPrice" runat="server" /></span><span style="margin-right: 0px;"> 儿童单价合计：<input
                        type="text" class="searchinput" name="txt_rtPrice" id="txt_rtPrice" runat="server" /></span><span
                            style="margin-right: 0px;"> 全陪单价合计：<input type="text" class="searchinput" name="txt_allPrice"
                                id="txt_allPrice" runat="server" /></span></div>
            <div style="text-align: right; float: right">
                <span style="margin-right: 0px;">总计：<input type="text" class="searchinput" name="txtAllPrice"
                    id="txtAllPrice" runat="server" /></span></div>
        </td>
        <td align="center">
        </td>
    </tr>
</table>
<asp:HiddenField ID="hidePrice" runat="server" Value="1" />
<asp:HiddenField ID="hideProList" runat="server" Value="1" />

<script type="text/javascript">
    var proArray = $("#<%=hideProList.ClientID %>").val().split("|");

    var arrayCount = Number("<%=this.rptList.Items.Count %>");

    $(function() {
        $("#tblPrice select").each(function() {
            PriceControl.SetSelectVal($(this).attr("id"), $(this).val());
        });

        //设置我社报价blur事件
        PriceControl.WsPriceBindBlur();



    })
    var PriceControl = {
        AddPrice: function() {
            var number = Number($("#<%=hidePrice.ClientID %>").val());
            var onedjCo = $("#tblPrice").find("input[name='oneDjCount']").eq(0).val();
            if (onedjCo > 0) { } else { onedjCo = 0; }
            if (number.toString() == "NaN") { number = 1 };
            number = number + 1;
            $("#tblPrice").append("<tr class=\"even\"><td height=\"25\" align=\"center\"><select id=\"selectPrice_" + number + "\" name=\"selectPrice\"></select></td><td align=\"center\"><input type=\"text\" class=\"searchinput searchinput02\" id=\"txtStandard_" + number + "\" name=\"Standard\"></td><td width=\"5%\" align=\"center\"><input type=\"text\" class=\"djcss\" style=\"width: 90%; border: solid 1px #93b7ce\" id=\"djPrice_" + number + "\" name=\"oneDjPrice\" value=\"\" /></td><td width=\"5%\" align=\"center\"><input type=\"text\" class=\"djcss\" style=\"width: 90%; border: solid 1px #93b7ce\" id=\"djCount_" + number + "\" name=\"oneDjCount\" value=\"" + onedjCo + "\" /></td><td align=\"center\"><input type=\"text\" class=\"searchinput\" id=\"txtDjPrice_" + number + "\" name=\"txtDjPrice\"></td> <td width=\"5%\" align=\"center\"><input type=\"text\" class=\"wscss\" style=\"width: 90%; border: solid 1px #93b7ce\" id=\"wsPrice_" + number + "\" name=\"oneWsPrice\" value=\"\" /></td><td width=\"5%\" align=\"center\"> <input type=\"text\" class=\"wscss\" style=\"width: 90%; border: solid 1px #93b7ce\" id=\"wsCount_" + number + "\" name=\"oneWsCount\" value=\"\" /></td><td align=\"center\"> <input type=\"text\" class=\"searchinput\" id=\"txtWsPrice_" + number + "\" name=\"txtWsPrice\"></td><td align=\"center\"><a href=\"javascript:void(0);\" onclick=\"PriceControl.DeletePrice(this)\"><img height=\"14\" width=\"14\" alt=\"\" src=\"/images/delicon01.gif\" border=\"0\"/>删除</a></td></tr>");
            $("#<%=hidePrice.ClientID %>").val(number);
            PriceControl.SetSelectVal("selectPrice_" + number, "0");
            //设置我社报价blur事件
            PriceControl.WsPriceBindBlur();
        },
        DeletePrice: function(obj) {
            if ($("#tblPrice").find("input[name='oneDjCount']").length > 1) {
                if (confirm("确定删除?")) {
                    if ($(obj).parent() != null) {
                        $(obj).parent().parent().remove();
                        // $("#<%=hidePrice.ClientID %>").val(Number($("#<%=hidePrice.ClientID %>").val()) - 1);
                    }
                }
            }
        },
        CheckForm: function() {
            var b = true;
            //价格组成 验证
            var indexT = 1;
            $("#tblPrice").find("input[name='selectPrice']").each(function() {
                var number = $(this).attr("id").split("_")[1];
                if (indexT == 1 && $(this).val() == "") {
                    alert("请选择一个项目!");
                    b = false;
                }
                indexT++;
            });
            return b;
        },
        SetSelectVal: function(selectID, selectIndex) {
            $("#" + selectID).html("");
            for (var i = 0; i < proArray.length; i++) {
                var obj = eval('(' + proArray[i] + ')');
                if (selectIndex != null && selectIndex != "" && obj.text == selectIndex) {
                    $("#" + selectID).append("<option selected='true' value=\"" + obj.value + "\">" + obj.text + "</option>");
                } else {
                    $("#" + selectID).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }
            }

        }, WsPriceBindBlur: function() {
            $("#tblPrice").find("input[name='txtWsPrice']").blur(function() {
                var allPrice = 0;
                $("#tblPrice").find("input[name='txtWsPrice']").each(function() {
                    if ($.trim($(this).val()) != "") {
                        if (parseFloat($.trim($(this).val())) != NaN && parseFloat($.trim($(this).val())) > 0) {
                            allPrice = allPrice + parseFloat($.trim($(this).val()));
                        }
                    }
                })
                if (allPrice != NaN) {
                    $("#<%=txtAllPrice.ClientID %>").val(allPrice);
                }
            });


            $(".djcss").blur(function() {
                var indexNum = $(this).attr("id").split("_")[1];

                var price = Number($("#djPrice_" + indexNum).val());
                var count = Number($("#djCount_" + indexNum).val());
                if (price > 0 && count > 0) {
                    var sum = parseInt(price * count * 100) / 100;
                    $("#txtDjPrice_" + indexNum).val(sum);
                }
            });

            $(".wscss").blur(function() {
                var indexNum = $(this).attr("id").split("_")[1];
                var price = Number($("#wsPrice_" + indexNum).val());
                var count = Number($("#wsCount_" + indexNum).val());
                if (price > 0 && count > 0) {
                    var sum = parseInt(price * count * 100) / 100;
                    $("#txtWsPrice_" + indexNum).val(sum);
                }

                //           var onePriceAll = 0;
                //                $("#tblPrice").find("input[name='oneWsPrice']").each(function() {
                //                    if ($.trim($(this).val()) != "" && Number($(this).val()) > 0) {
                //                        onePriceAll += Number($(this).val());
                //                    }
                //                })

                //                if (onePriceAll > 0) {
                //                    $("#").val(onePriceAll);
                //                }



                var allPrice = 0;
                $("#tblPrice").find("input[name='txtWsPrice']").each(function() {
                    if ($.trim($(this).val()) != "") {
                        if (parseFloat($.trim($(this).val())) != NaN && parseFloat($.trim($(this).val())) > 0) {
                            allPrice = allPrice + parseFloat($.trim($(this).val()));
                        }
                    }
                })
                if (allPrice > 0) {
                    $("#<%=txtAllPrice.ClientID %>").val(allPrice);
                }
            });
        }

    }
</script>

