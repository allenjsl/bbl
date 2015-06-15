<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamTake.aspx.cs" Inherits="Web.TeamPlan.TeamTake" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        .tbl td
        {
            border: solid 1px #fff;
        }
        .tbl
        {
            border-collapse: collapse;
            border: 0;
            width: 100%;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 910px; margin: 0 auto;">
        <table width="100%" cellspacing="1" cellpadding="0" border="0">
            <tbody>
                <tr class="odd">
                    <th nowrap="nowrap" width="10%" align="center">
                        地接社
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        接团时间
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        送团时间
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        团款
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        返利
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        结算费用
                    </th>
                    <th nowrap="nowrap" width="10%" align="center">
                        支付方式
                    </th>
                    <th nowrap="nowrap" width="20%" align="center" colspan="3">
                        操作
                    </th>
                </tr>
                <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <%#Container.ItemIndex%2==0?"<tr class=\"even\">":"<tr>" %>
                        <td align="center">
                            <%#Eval("LocalTravelAgency")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDateTime(Eval("ReceiveTime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDateTime(Eval("DeliverTime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Fee")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Commission")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Convert.ToDecimal(Eval("Settlement")).ToString("0.00")%>
                        </td>
                        <td align="center">
                            <%#Eval("PayType")%>
                        </td>
                        <td align="center">
                            <%if (this.hideIsEidt.Value != "No")
                              { %>
                            <a href="/TeamPlan/TeamTake.aspx?id=<%#Eval("ID").ToString().Trim() %>&planId=<%#hidePlanId.Value %>&planType=<%=hidePlanType.Value %>&doType=Update&iframeid=<%=Request.QueryString["iframeId"] %>">
                                修改</a>
                            <%}
                              else
                              {  %>
                            <a href="/TeamPlan/TeamTake.aspx?id=<%#Eval("ID").ToString().Trim() %>&planId=<%#hidePlanId.Value %>&planType=<%=hidePlanType.Value %>&doType=View&iframeid=<%=Request.QueryString["iframeId"] %>">
                                查看</a>
                            <%} %>
                        </td>
                        <td align="center">
                            <%if (this.hideIsEidt.Value != "No")
                              { %>
                            <a href="javascript:void(0);" onclick="TeamTake.DeleteTravel('<%#Eval("ID").ToString().Trim() %>')">
                                删除</a>
                            <%} %>
                        </td>
                        <td align="center">
                        </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td height="30" align="right" class="pageup" colspan="10">
                        <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="900">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center" width="90">
                        <span class="errmsg">*</span>地接社名称：
                    </th>
                    <td align="center" width="238">
                        <asp:TextBox ID="txtDiJieName" CssClass="searchinput searchinput02" runat="server"
                            ReadOnly="true" BackColor="#dadada" Width="150px"></asp:TextBox>
                        <a href="javascript:void(0);" id="btnDiJie">
                            <img src="/images/sanping_04.gif" /></a>
                    </td>
                    <th align="center" width="124">
                        联系人：
                    </th>
                    <td align="center" width="62">
                        <asp:TextBox ID="txtContact" ReadOnly="true" CssClass="searchinput" runat="server"
                            BackColor="#dadada"></asp:TextBox>
                    </td>
                    <th align="center" width="132">
                        联系电话：
                    </th>
                    <td align="center" width="247">
                        <asp:TextBox ID="txtPhone" ReadOnly="true" CssClass="searchinput" runat="server"
                            BackColor="#dadada"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        接团时间：
                    </th>
                    <td align="left" colspan="5">
                        <table cellspacing="0" cellpadding="0" border="0" width="70%">
                            <tbody>
                                <tr>
                                    <td align="center" width="46%">
                                        <asp:TextBox onfocus="WdatePicker()" ID="txtBeginTime" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                                    </td>
                                    <th align="center" width="27%">
                                        送团时间：
                                    </th>
                                    <td align="left" width="27%">
                                        <asp:TextBox onfocus="WdatePicker()" ID="txtEndTime" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center" colspan="6">
                        <asp:Panel ID="pnlJieDai" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td width="85">
                                        接待行程：
                                    </td>
                                    <td align="left"><a href="javascript:TourSelectAll();">全选</a></td><td align="left">
                                        <asp:Repeater ID="rptJieDai" runat="server">
                                            <ItemTemplate>
                                                <div id="divTravelDays" style="width: 700px;">
                                                    <input type="checkbox" value="2010-01-01" id="cbxTravel_<%#Container.ItemIndex %>"
                                                        name="cbxTravel" />&nbsp;&nbsp;<label for="cbxTravel_<%#Container.ItemIndex %>"><%#Eval("TraveDate")%></label>
                                                    &nbsp;&nbsp;&nbsp;<%#Eval("PlanContent") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
                <tr class="even">
                    <th align="center">
                        价格组成
                    </th>
                    <td align="center" colspan="5">
                        <table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" align="center"
                            width="95%" style="margin: 10px 0pt;">
                            <tbody>
                                <tr>
                                    <td bgcolor="#e3f1fc" align="center">
                                        <asp:Panel ID="pnlProjectB" runat="server">
                                            <table class="tbl" id="tblProject">
                                                <tr>
                                                    <td height="30" bgcolor="#e3f1fc" align="center" width="110px">
                                                        项目
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center" width="520px">
                                                        具体要求
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center" width="80px">
                                                        价格项目
                                                    </td>
                                                    <td bgcolor="#e3f1fc" align="center">
                                                        <a href="javascript:void(0);" onclick="TeamTake.AddProRow()">
                                                            <img height="16" width="15" src="/images/tianjiaicon01.gif">添加</a>
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptProjectB" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="107px">
                                                                <select id="sltProject_<%#Container.ItemIndex+1 %>" name="sltProject">
                                                                    <option value="<%#Eval("PriceTpyeId")%>">
                                                                        <%#Eval("PriceTpyeId")%></option>
                                                                </select>
                                                            </td>
                                                            <td width="524px" align="center">
                                                                <textarea name="txtClaim" id="txtClaim_<%#Container.ItemIndex+1 %>" class="textareastyle02"
                                                                    cols="20" rows="1" ><%#Eval("Remark")%></textarea>
                                                            </td>
                                                            <td width="77px">
                                                                <input type="text" class="searchinput" id="txtProPrice_<%#Container.ItemIndex+1 %>"
                                                                    name="txtProPrice" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Price")).ToString("0.00")) %>">
                                                            </td>
                                                            <td>
                                                                <a href="javascript:void(0);" onclick="TeamTake.DeleteRow(this)">
                                                                    <img height="14" width="14" src="/images/delicon01.gif">删除</a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" bgcolor="#e3f1fc" align="center">
                                        <asp:Panel ID="pnlProjectA" runat="server">
                                            <table style="width: 100%;" class="tbl" id="tblPricePro">
                                                <tr>
                                                    <th align="center" bgcolor="#e3f1fc" height="25">
                                                        价格项目
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc" width="405px">
                                                        备注
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc">
                                                        单人价格
                                                    </th>
                                                    <th align="center" bgcolor="#e3f1fc">
                                                        人数
                                                    </th>
                                                    <td align="center" bgcolor="#e3f1fc">
                                                        <a href="javascript:void(0);" onclick="TeamTake.AddPriceProRow()">
                                                            <img height="16" src="/images/tianjiaicon01.gif" width="15">添加</img></a>
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptProjectA" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="107px">
                                                                <input id="txtPricePro_<%#Container.ItemIndex+1 %>" class="searchinput" name="txtPricePro"
                                                                    type="text" value="<%#Eval("Title") %>" maxlength="50" />
                                                            </td>
                                                            <td width="403px">
                                                                <textarea id="txtBak_<%#Container.ItemIndex+1 %>" class="textareastyle02" cols="20"
                                                                    name="txtBak" rows="1" ><%#Eval("Remark")%></textarea>
                                                            </td>
                                                            <td width="117px">
                                                                <input id="txtOnePrice_<%#Container.ItemIndex+1 %>" class="searchinput" name="txtOnePrice"
                                                                    type="text" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Price")).ToString("0.00")) %>" />
                                                            </td>
                                                            <td width="78px">
                                                                <input id="txtPeoCount_<%#Container.ItemIndex+1 %>" class="searchinput" name="txtPeoCount"
                                                                    type="text" value="<%#Eval("PeopleCount")%>" />
                                                            </td>
                                                            <td>
                                                                <a href="javascript:void(0);" onclick="TeamTake.DeleteRow(this)">
                                                                    <img height="14" src="/images/delicon01.gif" width="14">删除</img></a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd" id="tr_Price">
                    <th height="25" align="center" width="84">
                        &nbsp;
                    </th>
                    <th align="right" width="238">
                        团款：
                    </th>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtTeamPrice" CssClass="searchinput" runat="server"></asp:TextBox>
                        元
                    </td>
                    <th align="right" width="132">
                        返款：
                    </th>
                    <td align="left" width="247">
                        <asp:TextBox ID="txtBackPrice" CssClass="searchinput" runat="server"></asp:TextBox>
                        元
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center" width="84">
                        &nbsp;
                    </th>
                    <th align="right" width="238">
                        <span class="errmsg">*</span> 结算费用：
                    </th>
                    <td align="left" colspan="2">
                        <asp:TextBox ID="txtSettlePrice" CssClass="searchinput" runat="server"></asp:TextBox>
                        元
                    </td>
                    <th align="right" width="132">
                        支付方式：
                    </th>
                    <td align="left" width="247">
                        <asp:DropDownList ID="dllPayType" runat="server">
                            <asp:ListItem Value="财务现收" Text="财务现收"></asp:ListItem>
                            <asp:ListItem Value="财务现付" Text="财务现付"></asp:ListItem>
                            <asp:ListItem Value="签单挂账" Text="签单挂账"></asp:ListItem>
                            <asp:ListItem Value="银行电汇" Text="银行电汇"></asp:ListItem>
                            <asp:ListItem Value="转账支票" Text="转账支票"></asp:ListItem>
                            <asp:ListItem Value="导游现收" Text="导游现收"></asp:ListItem>
                            <asp:ListItem Value="导游现付" Text="导游现付"></asp:ListItem>
                            <asp:ListItem Value="预存款支付" Text="预存款支付"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="60" align="center">
                        地接社需知：
                    </th>
                    <td align="left" colspan="5">
                        <asp:TextBox ID="txtNotes" CssClass="textareastyle" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th height="60" align="center">
                        备注：
                    </th>
                    <td align="left" colspan="5">
                        <label>
                            <asp:TextBox ID="txtRemarks" CssClass="textareastyle" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </label>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="6">
                        <asp:Panel ID="pnlEdit" runat="server">
                            <table cellspacing="0" cellpadding="0" border="0" width="320">
                                <tbody>
                                    <tr>
                                        <td height="40" align="center">
                                        </td>
                                        <td height="40" align="center" class="tjbtn02">
                                            <asp:LinkButton ID="linkSave" OnClientClick="return TeamTake.CheckForm()" runat="server"
                                                OnClick="btnSave_Click">保存</asp:LinkButton>
                                        </td>
                                        <td align="center" class="tjbtn02">
                                            <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                                                id="linkCancel" href="javascript:void(0);">关闭</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:HiddenField ID="hideProCount" runat="server" Value="1" />
    <asp:HiddenField ID="hidePriceProCount" runat="server" Value="1" />
    <asp:HiddenField ID="hideId" runat="server" />
    <asp:HiddenField ID="hidePlanId" runat="server" />
    <asp:HiddenField ID="hideDiJieID" runat="server" />
    <asp:HiddenField ID="hideProjectList" runat="server" Value="1" />
    <asp:HiddenField ID="hideDoType" runat="server" Value="" />
    <asp:HiddenField ID="hidePlanType" runat="server" Value="" />
    <asp:HiddenField ID="hideCommission" runat="server" Value="" />
    <asp:HiddenField ID="hideTypeAorB" runat="server" Value="" />
    <asp:HiddenField ID="hideXingCheng" runat="server" Value="" />
    <asp:HiddenField ID="hideIsEidt" runat="server" Value="" />

    <script type="text/javascript">
        var proArray = $("#<%=hideProjectList.ClientID %>").val().split("|");
        var TeamTake = {
            AddProRow: function() {
                var number = Number($("#<%=hideProCount.ClientID %>").val());
                if (number == NaN) { number = 1; }
                number++;
                $("#tblProject").append("<tr><td width=\"107px\"><select id=\"sltProject_" + number + "\" name=\"sltProject\"><option value=\"-1\">--请选择--</option></select></td><td width=\"524px\" align=\"center\"><textarea name=\"txtClaim\" id=\"txtClaim_" + number + "\" class=\"textareastyle02\" cols=\"20\" rows=\"1\"></textarea></td><td width=\"78px\"><input type=\"text\" class=\"searchinput\" id=\"txtProPrice_" + number + "\" name =\"txtProPrice\"></td><td><a href=\"javascript:void(0);\" onclick=\"TeamTake.DeleteRow(this)\"><img height=\"14\" width=\"14\" src=\"/images/delicon01.gif\" >删除</a></td></tr>");
                $("#<%=hideProCount.ClientID %>").val(number);
                TeamTake.SetSelectVal("sltProject_" + number, "-1");
                TeamTake.SetTypeBBlur();
            },
            AddPriceProRow: function() {
                var number = Number($("#<%=hidePriceProCount.ClientID %>").val());
                if (number == NaN) { number = 1; }
                number++;

                $("#tblPricePro").append("<tr><td width=\"107px\"><input type=\"text\" class=\"searchinput\" id=\"txtPricePro_" + number + "\" name=\"txtPricePro\" /></td><td width=\"403px\"><textarea name=\"txtBak\" id=\"txtBak_" + number + "\" class=\"textareastyle02\" cols=\"20\" rows=\"1\"></textarea></td><td width=\"117px\"><input type=\"text\" class=\"searchinput\" id=\"txtOnePrice_" + number + "\" name=\"txtOnePrice\"/></td><td width=\"78px\"><input type=\"text\" class=\"searchinput\" id=\"txtPeoCount_" + number + "\" name=\"txtPeoCount\" /></td><td><a href=\"javascript:void(0);\" onclick=\"TeamTake.DeleteRow(this)\"><img height=\"14\" width=\"14\" src=\"/images/delicon01.gif\" >删除</a></td></tr>");
                $("#<%=hidePriceProCount.ClientID %>").val(number);
                TeamTake.SetTypeABlur();
            },
            DeleteRow: function(obj) {
                if (confirm("确定删除?")) {
                    if ($(obj).parent() != null) {
                        $(obj).parent().parent().remove();
                    }
                }
            },
            DeleteTravel: function(id) {
                if($("#<%=hideIsEidt.ClientID %>").val()=="No")
                {
                    alert("该计划已经核算结束!不能删除!");
                    return;
                }
                if (confirm("确定删除?")) {
                    if (id != "") {
                        $.newAjax({
                            type: "Get",
                            url: "/TeamPlan/AjaxTeamTake.ashx?type=Delete&id=" + id,
                            cache: false,
                            success: function(result) {
                                if (result == "OK") {
                                    alert("删除成功!");
                                    window.location.reload();
                                } else {
                                    alert("删除失败");
                                }
                            }
                        });
                    }
                }
            },
            SetSelectVal: function(selectID, selectIndex) {
                $("#" + selectID).html("");
                for (var i = 0; i < proArray.length; i++) {
                    var obj = eval('(' + proArray[i] + ')');
                    $("#" + selectID).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }

                if (selectIndex != "") {
                    $("#" + selectID).val(selectIndex);
                }
            }, CheckForm: function() {
                var txtDiJieName = $("#<%=txtDiJieName.ClientID %>").val();
                var txtSettlePrice = $("#<%=txtSettlePrice.ClientID %>").val();
                if (txtDiJieName == "") {
                    alert("请选择地接社");
                    return false;
                }
                if (txtSettlePrice == "") {
                    alert("请输入结算金额");
                    return false;
                }
                return true;
            }, SetTypeABlur: function() {
                $("#tblPricePro").find("input[name='txtOnePrice']").blur(function() {
                    var allPrice = 0;
                    var txtBackPrice = 0;
                    var rebate = parseFloat($("#<%=hideCommission.ClientID %>").val());
                    
                    $("#tblPricePro").find("input[name='txtOnePrice']").each(function() {
                        var onePrice = parseFloat($(this).val());
                        var count = parseInt($("#txtPeoCount_" + $(this).attr("id").split("_")[1]).val());
                        if (onePrice > 0 && count > 0 && rebate >= 0) {
                            allPrice = allPrice + onePrice  * count;
                            txtBackPrice = txtBackPrice + rebate * count;
                        }
                    });
                    
                    $("#<%=txtTeamPrice.ClientID %>").val(parseInt(allPrice*100)/100);
                    $("#<%=txtBackPrice.ClientID %>").val(parseInt( txtBackPrice*100)/100);
                    $("#<%=txtSettlePrice.ClientID %>").val( parseInt((allPrice - txtBackPrice)*100)/100);
                });
                $("#tblPricePro").find("input[name='txtPeoCount']").blur(function() {
                    var allPrice = 0;
                    var txtBackPrice = 0;
                    var rebate = parseFloat($("#<%=hideCommission.ClientID %>").val());
                    $("#tblPricePro").find("input[name='txtPeoCount']").each(function() {
                    var onePrice = parseFloat($("#txtOnePrice_" + $(this).attr("id").split("_")[1]).val());
                    var count = parseInt($(this).val());
                        if (onePrice > 0 && count > 0 && rebate >= 0) {
                            allPrice = allPrice + onePrice * count;
                            txtBackPrice = txtBackPrice + rebate * count;
                        }
                    });
                   $("#<%=txtTeamPrice.ClientID %>").val(parseInt(allPrice*100)/100);
                    $("#<%=txtBackPrice.ClientID %>").val(parseInt( txtBackPrice*100)/100);
                    $("#<%=txtSettlePrice.ClientID %>").val( parseInt((allPrice - txtBackPrice)*100)/100);
                });
            }, SetTypeBBlur: function() {
                $("#tblProject").find("input[name='txtProPrice']").blur(function() {
                    var allPrice = 0;
                   
                    $("#tblProject").find("input[name='txtProPrice']").each(function() {
                        var onePrice = parseFloat($.trim($(this).val()));
                        if (onePrice != NaN && onePrice > 0) {
                            allPrice = allPrice + onePrice;
                        }
                    });
                    $("#<%=txtTeamPrice.ClientID %>").val(parseInt (allPrice*100) /100);
                    $("#<%=txtSettlePrice.ClientID %>").val(parseInt( allPrice*100)/100);
                });
            },SetXingCheng:function(){
                var days =$("#<%=hideXingCheng.ClientID %>").val();
                var dayArray = days.split(",");
                if(dayArray.length>0)
                {
                
                    for(var i=0;i<dayArray.length;i++)
                    {
                        if(dayArray[i]!="")
                        {
                            if($("#<%=pnlJieDai.ClientID %>").find("input[type='checkbox']").eq(parseInt(dayArray[i])-1).attr("type")!=null){
            $("#<%=pnlJieDai.ClientID %>").find("input[type='checkbox']").eq(parseInt(dayArray[i])-1).attr("checked","checked");
           }
                        }
                    }
                }
                
            }
        }
        $(function() {
            //选用地接按钮事件
            $("#btnDiJie").click(function() {
                var url = "/TeamPlan/DiJieList.aspx?text=<%=txtDiJieName.ClientID %>&value=<%=hideDiJieID.ClientID %>&callBackFun=GetDjInfo&ifid=<%=EyouSoft.Common.Utils.GetQueryStringValue("IframeId") %>&sType=<%= (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.地接 %>";
              parent.Boxy.iframeDialog({ iframeUrl: url, title: "报名", modal: true, width: "700px", height: "320px" });
            });

            //项目 下拉框 选中
            if ($("#tblProject select").length == 0) {
                //新增时添加一行
                TeamTake.AddProRow();
            } else {
                $("#tblProject select").each(function() {
                    TeamTake.SetSelectVal($(this).attr("id"), $(this).val());
                });
            }
            if($("#<%=hideDoType.ClientID %>").val()=="Add")
            {
            //新增时添加一行
            TeamTake.AddPriceProRow();
            }
            //设置价格事件
            TeamTake.SetTypeABlur();
            TeamTake.SetTypeBBlur();
            //团款 返款 文本框 离开焦点事件
            $("#tr_Price input[type='text']").blur(function(){
                var teamPrice = parseFloat($("#<%=txtTeamPrice.ClientID %>").val());
                var backPrice = parseFloat($("#<%=txtBackPrice.ClientID %>").val());
                
                if(isNaN(teamPrice)) teamPrice=0;
                if(isNaN(backPrice)) backPrice=0;
                
                $("#<%=txtSettlePrice.ClientID %>").val(teamPrice-backPrice);
                                
                /*if(teamPrice>0 && backPrice>0)
                {
                    $("#<%=txtSettlePrice.ClientID %>").val(teamPrice-backPrice);
                }else if (teamPrice>0) {
                     $("#<%=txtSettlePrice.ClientID %>").val(teamPrice);
                }else if(backPrice>0) {
                     $("#<%=txtSettlePrice.ClientID %>").val(0-backPrice);
                }else {
                    $("#<%=txtSettlePrice.ClientID %>").val('');
                }*/
                
            })
            //设置接待行程选中
            TeamTake.SetXingCheng();
        })

        function GetDjInfo() {
            var diJieID = $("#<%=hideDiJieID.ClientID %>").val();
            $.newAjax({
                type: "Get",
                url: "/TeamPlan/AjaxTeamTake.ashx?type=GetInfo&id=" + diJieID + "",
                cache: false,
                success: function(result) {
                    if (result != "") {
                        $("#<%=txtContact.ClientID %>").val(result.split("||")[0]);
                        $("#<%=txtPhone.ClientID %>").val(result.split("||")[1]);
                        $("#<%=hideCommission.ClientID %>").val(result.split("||")[2]);
                    }
                }
            });
        }
        //全选
        var isquanxuan = true;
        function TourSelectAll() {
            if (isquanxuan) {
                $("#divTravelDays>input").each
                    (function() {
                        $(this).attr("checked", "checked");
                    });
            }
            else {
                $("#divTravelDays>input").each
                    (function() {
                        $(this).attr("checked", "");
                    });
            }
            isquanxuan = !isquanxuan;
        }
        //控制地接显示的效果
        $(document).ready(function(){
            if($("#<%=hideDoType.ClientID %>").val()=="View")
            {
                $("input").each(function(){
                    $(this).attr("disabled","disabled").css("color","#ccc");
                });
                $("select").each(function(){
                    $(this).attr("disabled","disabled").css("color","#ccc");
                });
                $("textarea").each(function(){
                    $(this).attr("disabled","disabled").css("color","#ccc");
                });
                $("#tblProject tr>td:last-child").remove();
            }
        });
    </script>

    </form>
</body>
</html>
