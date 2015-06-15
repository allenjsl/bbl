<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="QuickAdd.aspx.cs" Inherits="Web.sanping.quickAdd"
    Title="散拼计划 - 快速添加" %>

<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/createTea.ascx" TagName="createTea" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->

        <script language="javascript">
            $("#link1").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "选择操作员",
                    modal: true,
                    width: "620",
                    height: "450px"
                });
                return false;
            });
        </script>

        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td width="15%" nowrap="nowrap">
                                <span class="lineprotitle">散拼计划</span>
                            </td>
                            <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                所在位置&gt;&gt; 散拼计划&gt;&gt; 快速发布
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
                <li><a class="tabtwo-on" id="two1" href="#">快速发布</a></li>
                <%if (EyouSoft.Common.Utils.GetQueryStringValue("act") != "update")
                  {
                %>
                <li><a id="A1" href="add.aspx">标准发布</a></li>
                <%} %>
                <div class="clearboth">
                </div>
            </ul>
            <div class="addlinebox">
                <div id="con_two_1">
                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>线路名称：
                                </td>
                                <td colspan="3">
                                    <uc1:xianluWindow Id="selectXl" runat="server" callBack="bindxlInfo" publishType="1"
                                        isRead="true" />
                                </td>
                            </tr>
                            <tr>
                                <td width="13%" height="30" align="right">
                                    <font class="xinghao">*</font>线路区域：
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddl_area" CssClass="select" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <font class="xinghao">*</font>计调员:
                                </td>
                                <td colspan="4">
                                    <input type="hidden" id="hd_oprator" name="hd_oprator" value="<%=hdOprator %>" /><select
                                        id="sel_oprator" name="sel_oprator"><option value="">请先选择线路区域</option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>天数：
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txt_Days" CssClass="searchinput searchinput03" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <font class="xinghao">*</font>关联交通：
                                </td>
                                <td colspan="4">
                                    <div class="jiaotong_list" id="jiaotong_list">
                                        <asp:HiddenField runat="server" ID="hidSelectTtraffic" />
                                        <ul>
                                            <%=TrafficStr %>
                                        </ul>
                                        <div class="clearboth">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" height="30" align="right">
                                    出港城市：
                                </td>
                                <td colspan="4">
                                    <table width="300" cellspacing="0" cellpadding="0" border="0">
                                        <tbody>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <asp:DropDownList ID="ddl_city" runat="server" AppendDataBoundItems="True">
                                                        <asp:ListItem Value="0">请选择</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tbody runat="server" id="trgz">
                                <tr>
                                    <td width="10%" height="30" align="right">
                                        <font class="xinghao">*</font>建团规则：
                                    </td>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" class="xuexian" colspan="4">
                                        <uc2:createTea ID="createTea1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right">
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                        <div id="teamInfo">
                                            <a href="javascript:void(0)" class="teamInfo">请选择建团规则进行生成</a>
                                        </div>
                                        <input type="hidden" id="hidToursNumbers" name="hidToursNumbers" />
                                    </td>
                                </tr>
                            </tbody>
                            <tbody runat="server" id="trteamNum" visible="false">
                                <tr>
                                    <td width="10%" height="30" align="right">
                                        <font class="xinghao">*</font>团号：
                                    </td>
                                    <td colspan="4">
                                        <input type="text" id="txt_teamNum" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                            <tr>
                                <td height="30" align="right">
                                    出发交通：
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txt_startTraffic" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                                    返程交通：
                                    <asp:TextBox ID="txt_endTraffic" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="right">
                                    地接社信息：
                                </td>
                                <td align="left" class="odd" colspan="3">
                                    <div class="div_dijieInfo">
                                        <uc5:DiJieControl ID="DiJieControl1" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <font class="xinghao"></font>价格组成：
                                </td>
                                <td colspan="3" align="left">
                                    <%if (sinfo == null)
                                      {%>
                                    <div style="background: url(/images/trbg.png) repeat;">
                                        <div class="dlprice" style="width: 470px">
                                            <div class="left" style="width: 155px;">
                                                <div class="title">
                                                    报价标准</div>
                                                <div>
                                                    <select name="ddl_price">
                                                        <%=BindPriceList()%>
                                                    </select>
                                                </div>
                                            </div>
                                            <asp:Repeater ID="rpt_Customer" runat="server">
                                                <ItemTemplate>
                                                    <div class="left" style="width: 155px;">
                                                        <div class="title">
                                                            <%#Eval("CustomStandName")%><input type="hidden" name="hd_cusStandType" value="<%#(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)Eval("LevType")) %>"><input
                                                                type="hidden" name="hd_cusStandId" value="<%#Eval("Id") %>" /><input type="hidden"
                                                                    name="hd_cusStandName" value="<%#Eval("CustomStandName") %>" /></div>
                                                        <div>
                                                            <font class="xinghao">*</font>成人
                                                            <input class="jiesuantext" value="" id="text5" name="txt_cr_price" type="text">
                                                            儿童
                                                            <input class="jiesuantext" value="" id="text6" name="txt_rt_price" type="text">
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <div style="clear: both;">
                                            </div>
                                        </div>
                                        <div class="act">
                                            <div class="title">
                                                操作</div>
                                            <div>
                                                <a href="javascript:void(0)" class="addPrice">
                                                    <img src="/images/tianjiaicon01.gif" alt="" height="16" width="15">
                                                    添加 </a><a href="javascript:void(0)" class="delPrice">
                                                        <img width="15" height="16" src="/images/delicon01.gif" alt="">
                                                        删除 </a>
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
                                    <div style="background: url(/images/trbg.png) repeat;">
                                        <div class="dlprice" style="width: 470px">
                                            <div class="left" style="width: 155px;">
                                                <div class="title">
                                                    报价标准</div>
                                                <div>
                                                    <select name="ddl_price">
                                                        <%=BindPriceList()%>
                                                    </select>

                                                    <script type="text/javascript">
                                            $(function(){
                                                $("[name='ddl_price']").eq(<%=i %>).children("[value^=<%=sinfo[i].StandardId %>|]").get(0).selected=true;
                                                });
                                                    </script>

                                                </div>
                                            </div>
                                            <%foreach (var v in sinfo[i].CustomerLevels)
                                              {
                                                  if (v.LevelId != 0)
                                                  {
                                            %>
                                            <div class="left" style="width: 155px;">
                                                <div class="title">
                                                    <%=v.LevelName%>
                                                    <input type="hidden" name="hd_cusStandType" value="<%=(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)(v.LevelType)) %>">
                                                    <input type="hidden" name="hd_cusStandId" value="<%=v.LevelId %>" />
                                                    <input type="hidden" name="hd_cusStandName" value="<%=v.LevelName%>" />
                                                </div>
                                                <div>
                                                    <font class="xinghao">*</font>成人
                                                    <input class="jiesuantext" id="text5" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( v.AdultPrice )%>"
                                                        name="txt_cr_price" type="text">
                                                    儿童
                                                    <input class="jiesuantext" id="text6" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(v.ChildrenPrice) %>"
                                                        name="txt_rt_price" type="text">
                                                </div>
                                            </div>
                                            <%}
                                              } %>
                                            <div style="clear: both;">
                                            </div>
                                        </div>
                                        <div class="act">
                                            <div class="title">
                                                操作</div>
                                            <div>
                                                <a href="javascript:void(0)" class="addPrice">
                                                    <img src="/images/tianjiaicon01.gif" alt="" height="16" width="15">
                                                    添加 </a><a href="javascript:void(0)" class="delPrice">
                                                        <img width="15" height="16" src="/images/delicon01.gif" alt="">
                                                        删除 </a>
                                            </div>
                                        </div>
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                    <%
                                        }
                                      } %>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    行程安排：
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txt_xinchen" runat="server" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    附件：
                                </td>
                                <td align="left" colspan="3">
                                    <div style="float: left; margin-right: 10px;">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlFile" runat="server" Visible="false">
                                        <img src="/images/open-dx.gif" /><input type="hidden" runat="server" id="hd_img"
                                            runat="server" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink ID="hypFilePath" runat="server"
                                            Target="_blank">查看附件</asp:HyperLink>
                                        <a href="javascript:void(0);" id="DeleFile">
                                            <img src="/images/fujian_x.gif" /></a>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    服务标准：
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="txt_fuwu" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right">
                                    备注：
                                </td>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="txt_remark" runat="server" TextMode="MultiLine" Height="87px" Width="630px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--快速发布-->
                <ul class="tjbtn">
                    <li>
                        <asp:LinkButton ID="lbtn_save" runat="server" OnClick="LinkButton1_Click">保存</asp:LinkButton></li>
                    <li><a href="javascript:history.back()">返回</a> </li>
                    <div class="clearboth">
                    </div>
                </ul>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    </form>

    <script type="text/javascript">
        //初始化编辑器
        KE.init({
            id: '<%=txt_xinchen.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txt_fuwu.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        
        $(function() {

            KE.create('<%=txt_xinchen.ClientID %>', 0); //创建编辑器
            KE.create('<%=txt_fuwu.ClientID %>', 0); //创建编辑器
        })


        function bindxlInfo() {
            var xlId=<%=selectXl.ClientID%>.Id();
            $.newAjax({url:"/ashx/xianluInfo.ashx",type:"POST",data:"id="+xlId+"&companyId=<%=CurrentUserCompanyID %>", dataType:"json",success: function(r) {
                $("#<%=ddl_area.ClientID %>").val(r.xlArea);
               
               KE.remove("<%=txt_xinchen.ClientID %>",0);
				$("#<%=txt_xinchen.ClientID %>").val(r.xlXianchen);
				KE.create('<%=txt_xinchen.ClientID %>', 0); //创建编辑器
                
				
				KE.remove("<%=txt_fuwu.ClientID %>",0);
				$("#<%=txt_fuwu.ClientID %>").val(r.xlFuwu);
                KE.create('<%=txt_fuwu.ClientID %>', 0); //创建编辑器
                
                $("#<%=txt_Days.ClientID %>").val(r.xlDays);
                
                    $("#<%=ddl_area.ClientID %>").change();
                }
            });
        }
        $(function() {
            $("#jiaotong_list ul li a").click(function(){
                var temp=$(this);
                if(!temp.attr("class")){
                    temp.addClass("select")
                }else{
                    temp.removeClass()
                }
             })
             $("#<%=txt_teamNum.ClientID %>").blur(function() {
                if($(this).val()=="")
                {
                    alert("请填写团号!");
                }else{
                jQuery.newAjax({
                    type: "POST",
                    url: "/xianlu/PublishedPlan.aspx?act=ExitTourNo",
                    async: false,
                    data: {
                    tourCode: $("#<%=txt_teamNum.ClientID %>").val(),
                    tourId:"<%= Request.QueryString["id"] %>"
                    },
                    success: function(data) {
                        if (data == "1") {
                            alert("已存在团号");
                        }
                    }
                });
                }
            });
            $("#DeleFile").click(function(){
                $(this).parent().remove();
            });
        if($("#<%=ddl_area.ClientID %>").val()!="-1")
            {
                $.getJSON("/ashx/getAreaOprator.ashx?areaId="+$("#<%=ddl_area.ClientID %>").val(),{date:new Date()},function(r){
                    $("#sel_oprator").get(0).length=0;
                    $("#sel_oprator").get(0).options[0]=new Option("请选择","");
                    for(var i= 0 ; i<r.length;i++)
                    {
                        $("#sel_oprator").get(0).options[i+1]=new Option(r[i].ContactName,r[i].UserId);
                    }
            $("#sel_oprator").val($("#hd_oprator").val());
                });
            }else
            {
                $("#sel_oprator").get(0).length=0;
                $("#sel_oprator").get(0).options[0]=new Option("-请选择-","");
            }
            $("#<%=ddl_area.ClientID %>").change(function(){
                if($(this).val()!="-1")
                {
                    $.getJSON("/ashx/getAreaOprator.ashx?areaId="+$(this).val(),{date:new Date()},function(r){
                        $("#sel_oprator").get(0).length=0;
                        $("#sel_oprator").get(0).options[0]=new Option("-请选择-","");
                        for(var i= 0 ; i<r.length;i++)
                        {
                            $("#sel_oprator").get(0).options[i+1]=new Option(r[i].ContactName,r[i].UserId);
                        }
                    });                     
                }else
                {
                    $("#sel_oprator").get(0).length=0;
                    $("#sel_oprator").get(0).options[0]=new Option("-请选择-","");
                }
            });
            $(".addDijie").bind("click", function() {
                $(this).parent().clone(true).insertAfter($(this).parent()).find("input").val("").find("select").val("-1");
                return false;
            });
            $(".addPrice").bind("click", function() {
                $(this).parent().parent().parent().clone(true).insertAfter($(this).parent().parent().parent()).find("input").val("").parents(".dlprice").find("select").val("-1");
                return false;
            });
            $(".delDijie").bind("click", function() {
                if ($(this).parent().parent().children(".div_dijieInfo").length > 1)
                    $(this).parent().remove();
                return false;
            });
            $(".delPrice").bind("click", function() {
                if ($(this).parent().parent().parent().parent()[0].children.length > 1)
                    $(this).parent().parent().parent().remove();
                return false;
            });
            $("#<%=lbtn_save.ClientID %>").click(function() {

            var arrstr=[];
            $("#jiaotong_list a[class='select']").each(function(){
                var self=$(this);
                arrstr.push(self.attr("data-trafficId"));
            })
            $("#<%=hidSelectTtraffic.ClientID %>").val(arrstr.join(','));
            if($.trim($("#<%=hidSelectTtraffic.ClientID %>").val())==""){
                alert("请选择关联交通！");
                return false;
            }
            var p="";
            $("[name='ddl_price']").each(function(){
                p+=$(this).val()+",";
            });
            var ap=p.split(",");
            var isp=false;
            for(var k=0;k<ap.length;k++)
            {                
                for(var j=0;j<k;j++)
                {
                    if(ap[j]==ap[k])
                    {
                        alert("不能选择多个相同的报价等级！");
                        return false;
                    }
                }
            }
                if ($("#<%=ddl_area.ClientID %>").val() == "-1") {
                    alert("请选择线路区域!");
                    $("#<%=ddl_area.ClientID %>").focus();
                    return false;
                }
                if( $("#sel_oprator").val()=="")
                {
                    alert("请选择计调员!");
                    $("#sel_oprator").focus();
                    return false;
                }
                if (<%=selectXl.ClientID %>.val().length == 0) {
                    alert("请填写线路名称!");
                    return false;
                }
                if ($("#<%=txt_Days.ClientID %>").val().length == 0) {
                    alert("请填写旅行天数!");
                    $("#<%=txt_Days.ClientID %>").focus();
                    return false;
                }
                if(!/^\d+$/.test($("#<%=txt_Days.ClientID %>").val()))
                {
                    alert("请正确填写旅行天数!");
                    $("#<%=txt_Days.ClientID %>").select();
                    return false;
                }

                if(queryString("act")!="update"){
                if(!<%=createTea1.ClientID %>.validate())
                {
                    return false;
                }
                }
                var ea=false;
                $("[name='txt_cr_price']").each(function(index,domEle){
                    var de=/^[0-9]+([.]\d{1,2})?$/;
                    if($(this).val().length==0)
                    {
                        alert("成人价不能为空！");
                        $(this).focus();
                        ea=false;
                        return false;
                    }
                    if(!de.test($(this).val()))
                    {
                        alert("请正解填写成人价！");
                        $(this).select();
                        ea=false;
                        return false;
                    }
                    if($("[name='txt_rt_price']").get(index).value.length>0)
                    {                        
                        if(!de.test($("[name='txt_rt_price']").get(index).value))
                        {
                            alert("请正解填写儿童价！");
                            $("[name='txt_rt_price']").get(index).select();
                            ea=false;
                            return false;
                        }
                    }
                    ea=true;
                });
                if(!ea){return false;}
            });
        });
        
    function queryString(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    }
    </script>

</asp:Content>
