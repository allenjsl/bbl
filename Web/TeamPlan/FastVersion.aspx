<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FastVersion.aspx.cs" Inherits="Web.TeamPlan.FastVersion"
    MasterPageFile="~/masterpage/Back.Master" ValidateRequest="false" %>

<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PriceControl.ascx" TagName="PriceControl" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 10px;
        }
    </style>
    <title>团队计划_快速发布</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
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
            <li><a class="tabtwo-on">快速发布</a></li>
            <li>
                <asp:Panel ID="pnlStandar" runat="server">
                    <a href="StandardVersion.aspx" class="">标准版发布</a>
                </asp:Panel>
            </li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%" id="con_two_1" style="display: block;">
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
                        <td height="30" align="right">
                            <font class="xinghao">*</font>团号：
                        </td>
                        <td width="89%">
                            <asp:TextBox ID="txtTeamNum" runat="server" CssClass="searchinput"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" width="11%">
                            <font class="xinghao">*</font>线路区域：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLineArea" runat="server" CssClass="select">
                                <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" width="11%">
                            <font class="xinghao">*</font>计调员：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSeller" runat="server">
                                <asp:ListItem Value="-1" Text="--请选择--"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" width="11%">
                            <font class="xinghao">*</font>组团社：
                        </td>
                        <td>
                            <asp:TextBox ID="txtGroupsName" runat="server" ReadOnly="true" BackColor="#dadada"></asp:TextBox>
                            <a href="javascript:void(0);" id="a_GetGroups" class="selectTeam">
                                <img src="../images/sanping_04.gif" width="28px" height="18px" border="0"></a>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right" width="11%">
                            <font class="xinghao">*</font>线路名称：
                        </td>
                        <td>
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
                        <td height="30" align="right" width="11%" style="height: 15px">
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
                            出港城市：
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlCityList" runat="server" CssClass="select">
                                <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right">
                            出发交通：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtStartTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                            返程交通：
                            <asp:TextBox ID="txtEndTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right">
                            附件：
                        </td>
                        <td align="left">
                            <input type="file" name="fileField" />
                            <asp:Panel ID="pnlFile" runat="server">
                                <img src="/images/open-dx.gif" />
                                <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                                <a href="javascript:void(0);" onclick="FastVersion.DeleteFile()">
                                    <img src="/images/fujian_x.gif" /></a>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="right">
                            地接社信息：
                        </td>
                        <td align="left">
                            <uc2:DiJieControl ID="DiJieControl1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td height="10" align="center" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="right">
                            <font class="xinghao">*</font>价格组成：
                        </td>
                        <td align="center">
                            <uc3:PriceControl ID="PriceControl1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="center">
                            <font class="xinghao">*</font>行程安排：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTravel" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="10" align="center" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="center">
                            <font class="xinghao">*</font>服务标准：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtServices" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" align="center">
                            备注：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <!--快速发布-->
            <!--无计划发布-->
            <asp:Panel ID="pelAdd" runat="server">
                <ul class="tjbtn">
                    <li><a href="javascript:void(0);" id="btnSave">保存</a> </li>
                    <li><a href="javascript:void(0);" id="btnBack">返回</a>
                        <asp:Button ID="btnSubmit" runat="server" BackColor="Transparent" BorderColor="Transparent"
                            OnClientClick="return FastVersion.CheckForm()" OnClick="btnSubmit_Click" Width="0"
                            Height="0" /></li>
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

    <script src="/js/kindeditor/kindeditor.js" type="text/javascript"></script>

    <script type="text/javascript">
        //初始化编辑器
        KE.init({
            id: '<%=txtTravel.ClientID%>', //编辑器对应文本框id
            width: '625px',
            height: '330px',
            skinsPath: '../js/kindeditor/skins/',
            pluginsPath: '../js/kindeditor/plugins/',
            scriptPath: '../js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txtServices.ClientID%>', //编辑器对应文本框id
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

            //初始化编辑器
            KE.create('<%=txtTravel.ClientID%>', 0); //创建编辑器
            KE.create('<%=txtServices.ClientID%>', 0); //创建编辑器

            //绑定保存事件
            $("#btnSave").click(function() {

                if (FastVersion.CheckForm()) {
                    //提交表单
                    $("#<%=btnSubmit.ClientID %>").click();
                    return false;
                }
            });
            //返回事件
            $("#btnBack").click(function() {
                 window.location.href="/TeamPlan/TeamPlanList.aspx";
               return false;
            });
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

        var FastVersion = {
            CheckForm: function() {
                //线路区域
                var lineArea = $("#<%=ddlLineArea.ClientID %>").val();
                //线路ID
                var xl_ID = $('#<%=xianluWindow1.FindControl("hd_xl_id").ClientID%>').val();
                //线路名称
                var xl_Name = $('#<%=xianluWindow1.FindControl("txt_xl_Name").ClientID%>').val();
                //天数
                var dayCount = $("#<%=txtDayCount.ClientID %>").val();
                //人数
                var peopelCount;
                if( parseInt( $("#hd_Numconfig").val())==0){
                    peopelCount = $("#<%=txtPeopelCount.ClientID %>").val();
                }else{
                    peopelCount=$("#<%=txt_crNum.ClientID %>").val();
                }
                //出团日期
                var rDate = $.trim($("#<%=txtLDate.ClientID %>").val());
                //团号
                var teamNum = $.trim($("#<%=txtTeamNum.ClientID %>").val());
                //组团社
                var groupId = $.trim($("#<%=hideGroupsId.ClientID %>").val());
                //计调员
                var settleId = $("#<%=ddlSeller.ClientID %>").val();
                //判断线路区域
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
                if (groupId == "") {
                    alert("请选择组团社")
                    return false;
                }
                //判断是否输入线路名称
                if (xl_Name == "") {
                    alert("请选择线路名称!");
                    return false;
                }
                //判断天数是否为数据且大于0
                if (dayCount != "" && Number(dayCount) > 0) {
                    if (Number(dayCount).toString() == "NaN") {
                        alert("天数格式不正确!");
                        return false;
                    }
                } else {
                    alert("请输入天数!"); return false;
                }

                //判断人数是否为数字
                if (peopelCount != "" && peopelCount!="0") {
                    if (Number(peopelCount).toString() == "NaN") {
                        alert("人数格式不正确!");
                        return false;
                    } 
                    
                }else {
                        alert("请输入人数!");
                         return false;
                    }
                //判断是否输入了出团日期
                if (rDate == "") {
                    alert("请输入出团日期!");
                    return false;
                }

                //判断团号是否为空
                if (teamNum == "") {
                    alert("团号不能为空!");
                    return false;
                }
                //判断团号是否存在
                if (teamNum != "" && teamNum != $("#<%=hideOldTeamNum.ClientID %>").val()) {
                    if (!FastVersion.AjaxCheckTeamNum()) {
                        alert("该团号已经存在!")
                        return false;
                    }
                }
                if($("#selectTraffic").val()==""){
                    alert("请选择关联交通!");
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
                
                //获得地接社信息验证
                if (!DiJieControl.CheckForm()) {

                    return false;
                }
                //获得价格组成验证
                if (!PriceControl.CheckForm()) {
                    return false;
                }

                return true;
            },
            AjaxGetLineInfo: function() {
                var xl_ID = <%=xianluWindow1.ClientID%>.Id();
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=Fast&id=" + xl_ID,
                    cache: false,
                    dataType: "json",
                    success: function(result) {
                        if (result != "") {
                            var json = result;
                            $("#<%=txtDayCount.ClientID %>").val(json.dayCount.toString().replace(/##n##/g, "\n"));
                           
                            KE.remove("<%=txtTravel.ClientID %>",0);
							$("#<%=txtTravel.ClientID %>").val(json.travel.toString().replace(/##n##/g, "\n"));
							KE.create('<%=txtTravel.ClientID %>', 0); //创建编辑器
							
							KE.remove("<%=txtServices.ClientID %>",0);
							$("#<%=txtServices.ClientID %>").val(json.services.toString().replace(/##n##/g, "\n"));
							KE.create('<%=txtServices.ClientID %>', 0); //创建编辑器
                        }
                    }
                });
            },
            AjaxCheckTeamNum: function() {
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
            },DeleteFile:function(){
                if(confirm("确认删除?"))
                {
                    $("#<%=pnlFile.ClientID %>").html("");
                    $("#<%=hideData.ClientID %>").val("");
                }
            }
        }
        function SetGroupsVal(val, txt) {
            $("#<%=hideGroupsId.ClientID %>").val(val);
            $("#<%=txtGroupsName.ClientID %>").val(txt);
        }
        
    </script>

    </form>
</asp:Content>
