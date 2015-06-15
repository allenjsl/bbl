<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerationPlan.aspx.cs" Inherits="Web.sanping.GenerationPlan" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc22" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>成人价弹出框</title>
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
<script src="/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
</head>
<body>
    <form runat="server" id="myform" enctype="multipart/form-data">
        <table width="880" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
            <tr class="even">
                <th height="25" width="120" align="right">
                    <font class="xinghao">*</font>团号：
                </th>
                <td align="left">
                <input type="hidden" id="hd_Ldate" runat="server" name="hd_Ldate" />
                    <input type="text" class="searchinput" id="txt_tuanhao" name="txt_tuanhao"/>
                </td>
                <td align="right"><font class="xinghao">*</font>计调员:</td>
                <td><input type="hidden" id="hd_area" name="hd_area" runat="server" value=""/><select id="sel_oprator" valid="required" errmsg="*请选择计调员" name="sel_oprator"><option value="">请先选择线路区域</option></select></td>
            </tr>
            <tr class="odd">
                <th align="right">
                    去程航班/时间
                </th>
                <td align="left">
                    <input type="text" class="searchinput searchinput02" id="txt_qcDate" name="txt_qcDate"/>
                </td>
                <th align="right">
                    回程航班/时间
                </th>
                <td align="left">
                    <input type="text" class="searchinput searchinput02" id="txt_hcDate" name="txt_hcDate"/>
                </td>
            </tr>
            <tr class="even">
                <th align="right">
                    <font class="xinghao">*</font>预控人数
                </th>
                <td align="left">
                    <input type="text" class="searchinput" errmsg="*请输入预控人数!|*请正确输入预控人数!" valid="required|isNumber" id="txt_ykrs" name="txt_ykrs" runat="server"/>
                    <input type="hidden" id="hd_num" runat="server" />
                </td>
                <th align="right">
                    集合方式
                </th>
                <td align="left">
                    <input type="text" class="searchinput searchinput02" name="txt_jhfs" id="txt_jhfs"/>
                </td></tr>
            <tr class="odd">
                <th align="right">
                    集合时间
                </th>
                <td align="left">
                    <input type="text" class="searchinput" id="txt_jhdate" name="txt_jhdate" />
                </td>
                <th align="right">
                    集合地点
                </th>
                <td align="left">
                    <input type="text" class="searchinput searchinput02" id="txt_jhdd" name="txt_jhdd"/>
                </td></tr>
            <tr class="even">
                <th align="right">
                    集合标志
                </th>
                <td align="left">
                    <input type="text" class="searchinput searchinput02" id="txt_jhbz" name="txt_jhbz"/>
                </td>
                <th align="right">
                    送团人
                </th>
                <td align="left">
                    <uc22:selectoperator Title="" ID="SelectgroupPepole" runat="server" />
                </td></tr>
            <tr class="odd">
                <th align="right">
                    文件上传
                </th>
                <td align="left" colspan="3">
                      <div style="float:left; margin-right:10px;"><asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlFile" runat="server" Visible="false">
                            <img src="/images/open-dx.gif" /><input type="hidden" runat="server" id="hd_img" runat="server" />
                           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hypFilePath" runat="server" Target="_blank">查看</asp:HyperLink>
                            <a href="javascript:void(0);" id="DeleFile">
                                <img src="/images/fujian_x.gif" /></a>
                        </asp:Panel>
                </td>                
                </tr>
                <tr class="even">
                <th align="right">价格组成 <td colspan="3" align="left">
<%
                                                                        {
                                          for (int i = 0; i < sinfo.Count;i++ )
                                          { 
                                            %>
                                            
                                <div>
                                    <div class="dlprice" style="width:750px;background:none;">
                                        <div class="left" style="width:155px; background:none;">
                                            <div class="title" style="background:none;">
                                                报价标准</div>
                                            <div>
                                            <select name="ddl_price"><%=BindPriceList()%></select>
                                             <script>
                                            $(function(){
                                                $("[name='ddl_price']").eq(<%=i %>).children("[value^=<%=sinfo[i].StandardId %>|]").get(0).selected=true;
                                                });
                                            </script>
                                            </div>
                                        </div>
                                        <%foreach (var v in sinfo[i].CustomerLevels)
                                          { %>
                                        <div class="left" style="width:190px; background:none;overflow:auto;">
                                            <div class="title" style="background:none;">
                                                <%=v.LevelName%>
                                                <input type="hidden" name="hd_cusStandType" value="<%=(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)(v.LevelType)) %>">
                                                <input type="hidden" name="hd_cusStandId" value="<%=v.LevelId %>"/>
                                                <input type="hidden" name="hd_cusStandName" value="<%=v.LevelName%>"/>
                                                </div>
                                            <div>
                                                成人
                                                <input class="jiesuantext" id="text5" value="￥<%= v.AdultPrice.ToString("###,##0.00")%>" name="txt_cr_price" type="text">
                                                儿童
                                                <input class="jiesuantext" id="text6" value="￥<%= v.ChildrenPrice.ToString("###,##0.00") %>" name="txt_rt_price" type="text">
                                            </div>
                                        </div>
                                        <%} %>
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                    <div style="clear:both;"></div>
                                    
                                    </div>
                                            <%
                                        } 
                                      } %></td>
                </tr>
                
                <tr class="odd">
                <th align="right"></th>
                <td colspan="3" class="tjbtn02" align="center">
                <asp:LinkButton ID="LinkButton1" CommandName="submit" runat="server" onclick="LinkButton1_Click">生成计划</asp:LinkButton>
                </td>
                </tr>
        </table>
    </form>
    <script>

        $(function() {
        jQuery.newAjax({
            type: "POST",
            url: "/ashx/GenerateTourNumbers.ashx?companyId=<%=SiteUserInfo.CompanyID%>",
            async: false,
            data: {
            datestr: $("#<%=hd_Ldate.ClientID %>").val()
        },
            datatype:"json",
            success: function(data) {
                if (data != "") {
                    var resultArr = [];
                    var arr = eval(data);
                    for (var i = 0; i < arr.length; i++) {
                        //resultArr.push(arr[i][0] + ToursNumberSeparator + arr[i][1]);
                        $("#txt_tuanhao").val(arr[i][1]);
                    }
                }
            }
        });


        $("#txt_tuanhao").blur(function() {
            if ($(this).val() == "") {
                alert("请填写团号!");
            } else {
                jQuery.newAjax({
                    type: "POST",
                    url: "/xianlu/PublishedPlan.aspx?act=ExitTourNo",
                    async: false,
                    data: {
                        tourCode: $("#txt_tuanhao").val(),
                        tourId: null
                    },
                    success: function(data) {
                        if (data == "1") {
                            alert("已存在团号");
                        }
                    }
                });
            }
        });
        if ($("#<%=hd_area.ClientID %>").val() != "") {
            $.getJSON("/ashx/getAreaOprator.ashx?areaId=" + $("#<%=hd_area.ClientID %>").val(), { date: new Date() }, function(r) {
                    $("#sel_oprator").get(0).length = 0;
                    $("#sel_oprator").get(0).options[0] = new Option("请选择", "");
                    for (var i = 0; i < r.length; i++) {
                        $("#sel_oprator").get(0).options[i + 1] = new Option(r[i].ContactName, r[i].UserId);
                    }
                    $("#sel_oprator").val($("#hd_oprator").val());
                });
            }
            
            $("#DeleFile").click(function() {
                $(this).parent().remove();
            });
            $("[name='ddl_price']").each(function() {
                $(this).hide();
                $(this).before($(this).val().split("|")[1]);
            });
            $(".dlprice input").not("[type='hidden']").each(function() {
            $(this).hide();
            $(this).before($(this).val());
        });
        $("#<%=LinkButton1.ClientID %>").click(function() {
            var form = $(this).closest("form").get(0);
			var oldnum = isnull(parseInt( $("#hd_num").val()),0);
			var newnum = isnull(parseInt($("#txt_ykrs").val()),0);
			if(newnum<oldnum)
			{
				alert("预控人数不能小于申请人数!");
				$("#txt_ykrs").focus();
				return false;
			}
            //点击按纽触发执行的验证函数
            return ValiDatorForm.validator(form, "alert");
        });
        //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
        FV_onBlur.initValid($("#<%=LinkButton1.ClientID %>").closest("form").get(0));
        })
			
		function isnull(v, defaultValue) {
			if (v == null || !v)
				return defaultValue;
			else
				return v;
		}
    </script>
</body>
</html>
