<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardVersion.aspx.cs"
    Inherits="Web.TeamPlan.StandardVersion" MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc4" %>
<%@ Register Src="/UserControl/PriceControl.ascx" TagName="PriceControl" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>团队计划_标准版发布</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server" enctype="multipart/form-data">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">团队计划</span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt; 团队计划&gt;&gt; 团队计划
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <asp:Panel ID="pnlFast" runat="server">
                <li><a href="FastVersion.aspx">快速发布</a> </li>
            </asp:Panel>
            <li><a class="tabtwo-on">标准版发布</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="right" width="11%" style="height: 20px">
                                <font class="xinghao">*</font>出团日期：
                            </td>
                            <td align="left" colspan="3" style="width: 260px; height: 25px">
                                <asp:TextBox ID="txtLDate" CssClass="searchinput" runat="server" onfocus="WdatePicker({onpicked:getTourCode})"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="20%" style="*width: 10%">
                                <font class="xinghao">*</font>团号：
                            </td>
                            <td width="85%" colspan="2">
                                <asp:TextBox ID="txtTeamNum" runat="server" CssClass="searchinput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>线路区域：
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlLineArea" runat="server" CssClass="select">
                                    <asp:ListItem Value="0">--请选择--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>计调员：
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlSeller" runat="server">
                                    <asp:ListItem Value="-1" Text="--请选择--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>组团社：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtGroupsName" runat="server" ReadOnly="true" BackColor="#dadada"></asp:TextBox>
                                <a href="javascript:void(0);" id="a_GetGroups" class="selectTeam">
                                    <img src="../images/sanping_04.gif" width="28px" height="18px" border="0"></a>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>线路名称：
                            </td>
                            <td colspan="2">
                                <uc1:xianluWindow Id="xianluWindow1" runat="server" isRead="true" />
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                <font class="xinghao">*</font>关联交通：
                            </td>
                            <td align="left">
                                <select id="selectTraffic" name="selectTraffic">
                                    <%=strTraffic %>
                                </select>
                                剩余空位：<asp:Label runat="server" ID="lbshengyu"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>天数：
                            </td>
                            <td colspan="2">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td align="center" width="65">
                                                <asp:TextBox ID="txtDayCount" runat="server" CssClass="searchinput searchinput03"
                                                    na="txtDayCount"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <span style="display: <%=NumConfig==0?"block":"none"%>;"><font class="xinghao">*</font>人数：
                                                    <asp:TextBox ID="txtPeopelCount" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hid_PeopelCount" />
                                                </span><span style="display: <%=NumConfig==1?"block":"none"%>;">人数 <font class="xinghao">
                                                    *</font>成人数：
                                                    <asp:TextBox ID="txt_crNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hid_Num" />
                                                    儿童数：
                                                    <asp:TextBox ID="txt_rtNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    全陪：
                                                    <asp:TextBox ID="txt_allNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                </span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                去程航班/时间：
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtStartTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                                &nbsp; 回程航班/时间：
                                <asp:TextBox ID="txtEndTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                集合时间：
                            </td>
                            <td align="left" colspan="2">
                                <%--<asp:TextBox ID="txtGathered" CssClass="searchinput" runat="server" onfocus="WdatePicker()"></asp:TextBox><asp:DropDownList
                                    ID="ddl_jh_date" runat="server" DataTextField="Text" DataValueField="Value">
                                    <asp:ListItem>0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>6</asp:ListItem>
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>8</asp:ListItem>
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    <asp:ListItem>13</asp:ListItem>
                                    <asp:ListItem>14</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>16</asp:ListItem>
                                    <asp:ListItem>17</asp:ListItem>
                                    <asp:ListItem>18</asp:ListItem>
                                    <asp:ListItem>19</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>22</asp:ListItem>
                                    <asp:ListItem>23</asp:ListItem>
                                </asp:DropDownList>
                                点 --%>
                                <asp:TextBox runat="server" ID="txtGathered" CssClass="searchinput" Style="width: 150px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;集合地点：
                                <asp:TextBox ID="txtPlace" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                集合标志：
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;<asp:TextBox ID="txtLogo" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;
                                <uc5:selectOperator ID="selectOperator1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                出港城市：
                            </td>
                            <td align="left" colspan="2">
                                <asp:DropDownList ID="ddlCityList" runat="server" CssClass="select">
                                    <asp:ListItem Value="0">--请选择--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right">
                                地接社信息：
                            </td>
                            <td align="left" colspan="2">
                                <uc2:DiJieControl ID="DiJieControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="10" align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td height="10" align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right">
                                <font class="xinghao">*</font>行程安排：
                            </td>
                            <td align="left" colspan="2">
                                <uc4:xingcheng ID="xingcheng1" runat="server" daysControlName="txtDayCount" />
                            </td>
                        </tr>
                        <tr>
                            <td height="40" background="/images/bg003.gif" align="right">
                                添加附件：
                            </td>
                            <td background="/images/bg003.gif" width="30%">
                                <input type="file" id="fileUpLoad" name="fileField" />
                            </td>
                            <td background="/images/bg003.gif">
                                <asp:Panel ID="pnlFile" runat="server">
                                    <img src="/images/open-dx.gif" />
                                    <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                                    <a href="javascript:void(0);" onclick="StandardVersion.DeleteFile()">
                                        <img src="/images/fujian_x.gif" /></a>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" colspan="3">
                                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr bgcolor="#bddcf4">
                                            <th height="25" align="center" colspan="2">
                                            </th>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                包含项目：
                                            </td>
                                            <td>
                                                <uc3:PriceControl ID="PriceControl1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                不含项目：
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNoProject" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                购物安排：
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtBuyPlan" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                自费项目：
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtSelfProject" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                &nbsp;儿童安排：
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtChildPlan" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                注意事项：
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNote" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                温馨提醒：
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtTips" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                内部信息：
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNeiBu" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="10" align="center" colspan="2">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!--无计划发布-->
            <asp:Panel ID="pelAdd" runat="server">
                <ul class="tjbtn">
                    <li><a href="javascript:void(0);" id="btnSave">保存</a> </li>
                    <li><a href="javascript:void(0);" id="btnBack">返回</a>
                        <asp:Button ID="btnSubmit" runat="server" BackColor="Transparent" BorderColor="Transparent"
                            OnClick="btnSubmit_Click" Width="0px" Height="0" /></li>
                    <div class="clearboth">
                    </div>
                </ul>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hideID" runat="server" />
    <asp:HiddenField ID="hideType" runat="server" />
    <asp:HiddenField ID="hideData" runat="server" />
    <asp:HiddenField ID="hideGroupsId" runat="server" />
    <asp:HiddenField ID="hideOldTeamNum" runat="server" />
    <asp:Literal ID="litMsg" runat="server"></asp:Literal>
    <input type="hidden" id="hd_Numconfig" value="<%=NumConfig %>" />

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
     function getTourCode() {
         if ($("#<%=txtLDate.ClientID %>").val() == "") {
            alert("请选择出团日期!");
            $("#<%=txtLDate.ClientID %>").focus();
            return false;
           }
           else {
            $.newAjax({
                type: "POST",
                url: "/ashx/GenerateTourNumbers.ashx?companyId=<%=SiteUserInfo.CompanyID%>",
                async: false,
                data: {
                    datestr: $("#<%=txtLDate.ClientID %>").val()
                },
                success: function(data) {
                    if (data != "") {
                        var resultArr = [];
                        var arr = eval(data);
                        for (var i = 0; i < arr.length; i++) {
                            $("#<%=txtTeamNum.ClientID %>").val(arr[i][1]);
                           }
                      }
                   }
               });
            }
         }
        $(function() {
            //绑定保存事件
            $("#btnSave").click(function() {
                if (StandardVersion.CheckForm()) {
                    $("#<%=btnSubmit.ClientID%>").click();
                }
                return false;
            });
            //返回事件
            $("#btnBack").click(function() {
               window.location.href="/TeamPlan/TeamPlanList.aspx";
               return false;
            });
            //送团人 
            $("#a_SendMan").click(function(){
                
            })
             //选择地接社事件
            $("#a_GetGroups").click(function() {
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
                var url = "/CRM/customerservice/SelCustomer.aspx?method=SetGroupsVal";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选用组团社",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: iframeId,
                        backfun: "SetGroupsVal"
                    }
                });
                return false;
            })
                        //选择线路区域
            $("#<%=ddlLineArea.ClientID %>").change(function() {
                var areaId = $(this).val();
                $("#<%=ddlSeller.ClientID %>").html("<option value='-1'>--请选择--</option>");
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=GetAreaUser&areaId=" + areaId,
                    cache: false,
                    success: function(result) {
                        if (result != "") {
                            var array = result.split("|||");
                            for (var i = 0; i < array.length; i++) {
                                if (array[i] != "") {
                                    var obj = eval('('+array[i]+')');
                                    $("#<%=ddlSeller.ClientID %>").append("<option value='" + obj.uid + "'>" + obj.uName + "</option>");
                                }
                            }
                        }
                    }
                });
            });
            //加载价格
            if($("#<%=txtLDate.ClientID %>").val()){
                $.newAjax({
                    type: "GET",
                    dataType: "json",
                    url: "/sanping/SanPing_jion.aspx",
                    async: false,
                    data: {
                        act: "getPrice",
                        startDate: $("#<%=txtLDate.ClientID %>").val(),
                        trafficId: $("#selectTraffic").find("option:selected").val()
                    },
                    success: function(result) {
                        $("#selectTraffic").find("option:selected").attr("data-shengyu", result.shengyu);
                        $("#selectTraffic").find("option:selected").attr("data-price", result.result);
                        $("#<%=lbshengyu.ClientID %>").text(result.shengyu);
                    }
                })
            }
            $("#selectTraffic").change(function() {
                var self = $(this), price = $.trim(self.find("option:selected").attr("data-price")),
                    trafficId = self.find("option:selected").val(),
                    startDate = $("#<%=txtLDate.ClientID %>").val();
                if(!startDate){alert("请选择团号");return false;}
                if (trafficId) {
                    $.newAjax({
                        type: "GET",
                        dataType: "json",
                        url: "/sanping/SanPing_jion.aspx",
                        async: false, //同步执行ajax 默认true为异步请求
                        data: { act: "getPrice", startDate: startDate, trafficId: trafficId },
                        success: function(result) {
                            self.find("option:selected").attr("data-shengyu", result.shengyu);
                            self.find("option:selected").attr("data-price", result.result);
                        }
                    })
                }
                $("#<%=lbshengyu.ClientID %>").text(self.find("option:selected").attr("data-shengyu"));
            })
            
        })


        var StandardVersion = {
            CheckForm: function() {
                var b = true;
                //线路区域
                var lineArea = $("#<%=ddlLineArea.ClientID %>").val();
                //线路ID
                var xl_ID = $('#<%=xianluWindow1.FindControl("hd_xl_id").ClientID%>').val();
                var xl_Name = $('#<%=xianluWindow1.FindControl("txt_xl_Name").ClientID%>').val();
                var dayCount = $("#<%=txtDayCount.ClientID %>").val();
                var peopelCount=0;
                if( parseInt( $("#hd_Numconfig").val())==0){
                    peopelCount = $("#<%=txtPeopelCount.ClientID %>").val();
                }else{
                    peopelCount=$("#<%=txt_crNum.ClientID %>").val();
                }
                //组团社
                var groupId =  $.trim($("#<%=hideGroupsId.ClientID %>").val());
                //团号
                var teamNum = $.trim($("#<%=txtTeamNum.ClientID %>").val());
                //计调员
                var settleId = $("#<%=ddlSeller.ClientID %>").val();
                //出团日期
                var txtLDate = $.trim($("#<%=txtLDate.ClientID %>").val());
                if (lineArea == "0") {
                    alert("请选择线路区域!");
                    return false;
                }
                 if(settleId=="-1")
                {
                     alert("请选择计调员");
                    return false;
                }
                 //判断组团社是否选择
                if(groupId=="")
                {
                    alert("请选择组团社")
                   return false;
                }
                if (xl_ID == "" && xl_Name == "") {
                    alert("请选择线路名称!");
                    return false;
                }
                
                //判断团号是否为空
                if(teamNum=="")
                {
                    alert("团号不能为空!");
                    return false;
                }
                 //判断团号是否存在
                if (teamNum != "" && teamNum!=$("#<%=hideOldTeamNum.ClientID %>").val()) {
                    if (!StandardVersion.AjaxCheckTeamNum()) {
                        alert("该团号已经存在!")
                        return false;
                    }
                }
                if($("#selectTraffic").val()==""){
                    alert("请选择关联交通!");
                    return false;
                }
                if (dayCount == "") {
                    if (Number(dayCount).toString() == "NaN" || Number(dayCount)<=0) {
                        alert("天数格式不正确!");
                    } else {
                        alert("请输入天数!");
                    }
                    return false;
                }
                if (peopelCount == "") {
                    if (Number(peopelCount).toString() == "NaN") {
                        alert("人数格式不正确!");
                    } else {
                        alert("请输入人数!");
                    }
                    return false;
                }
                if($("#<%=hideType.ClientID %>").val()=="Add"||$("#<%=hideType.ClientID %>").val()=="Copy"){//添加或者复制
                    if( parseInt( $("#hd_Numconfig").val())!=0){
                       var countNum=(parseInt($("#<%=txt_crNum.ClientID %>").val())||0)+(parseInt($("#<%=txt_rtNum.ClientID %>").val())||0)+
                       (parseInt($("#<%=txt_allNum.ClientID %>").val())||0);
                       var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0;
                       if(planNum<countNum){
                        alert("总人数超过计划人数！");
                        return false;
                       }
                    }else{
                        var countNum=parseInt($("#<%=txtPeopelCount.ClientID %>").val())||0;
                        var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0;
                        if(planNum<countNum){
                            alert("总人数超过计划人数！");
                            return false;
                        }
                    }
                }
                else{//修改
                    if( parseInt( $("#hd_Numconfig").val())!=0){
                       var countNum=(parseInt($("#<%=txt_crNum.ClientID %>").val())||0)+(parseInt($("#<%=txt_rtNum.ClientID %>").val())||0)+
                            (parseInt($("#<%=txt_allNum.ClientID %>").val())||0);
                       var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0+(parseInt($("#<%=hid_Num.ClientID %>").val())||0);
                       if(planNum<countNum){
                            alert("总人数超过计划人数！");
                            return false;
                        }
                    }else{
                        var countNum=parseInt($("#<%=txtPeopelCount.ClientID %>").val())||0;
                        var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0+(parseInt($("#<%=hid_PeopelCount.ClientID %>").val())||0);
                        if(planNum<countNum){
                            alert("总人数超过计划人数！");
                            return false;
                        }
                    }
                }
                
                if(txtLDate=="")
                {
                    alert("请输入出团日期!");
                    return false;
                }
                return true;
            },
           AjaxGetLineInfo: function() {
                var xl_ID = <%=xianluWindow1.ClientID%>.Id();
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=Standard&id=" + xl_ID,
                    cache: false,
                    success: function(result) {
                        if (result != "") {
                            var json = eval('(' + result + ')');
                            //天数
                            $("#<%=txtDayCount.ClientID %>").val(json.dayCount);
                            //行程安排
                            <%=xingcheng1.ClientID %>.bind(json.travel);

                            //KE.html("",json.travel);
                            //文件上传
                            //$("#fileUpLoad").val(json.filePath);
                            //包含项目
                           // ConProject.SetRowValue(json.project);
                            //不包含项目
                            
                            $("#<%=txtNoProject.ClientID %>").val(json.noPro.toString().replace(/##n##/g,"\n"));
                            //购物安排
                            $("#<%=txtBuyPlan.ClientID %>").val(json.buyPlan.toString().replace(/##n##/g,"\n"));
                            //儿童安排
                            $("#<%=txtChildPlan.ClientID %>").val(json.childPlan.toString().replace(/##n##/g,"\n"));
                            //自费项目
                            $("#<%=txtSelfProject.ClientID %>").val(json.selfPro.toString().replace(/##n##/g,"\n"));
                            //注意事项
                            $("#<%=txtNote.ClientID %>").val(json.note.toString().replace(/##n##/g,"\n"));
                            //温馨提示
                            $("#<%=txtTips.ClientID %>").val(json.tips.toString().replace(/##n##/g,"\n"));
                        }
                    }
                });
            },
            DeleteFile:function(){
                if(confirm("确认删除?"))
                {
                    $("#<%=pnlFile.ClientID %>").html("");
                    $("#<%=hideData.ClientID %>").val("");
                }
            }, AjaxCheckTeamNum: function() {
                var bool = false;
                var teamNum = $("#<%=txtTeamNum.ClientID %>").val();
                $.newAjax({
                    type: "Get",
                    async: false,
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=CheckTeamNum&teamNum=" + teamNum,
                    cache: false,
                    success: function(result) {
                        if (result == "OK") {
                            bool = true;
                        }
                    }
                });
                return bool;
            }
        }
         function SetGroupsVal(val, txt) {
                $("#<%=hideGroupsId.ClientID %>").val(val);
                $("#<%=txtGroupsName.ClientID %>").val(txt);
            }
       
    </script>

    </form>
</asp:Content>
