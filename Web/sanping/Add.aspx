<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" ValidateRequest="false"
    AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Web.sanping.add" Title="散拼计划 - 标准发布" %>

<%@ Register Src="/UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/createTea.ascx" TagName="createTea" TagPrefix="uc3" %>
<%@ Register Src="/UserControl/ConProjectControl.ascx" TagName="ConProjectControl"
    TagPrefix="uc4" %>
<%@ Register Src="/UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc5" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc22" %>
<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc6" %>
<%@ Register Src="../UserControl/createTea.ascx" TagName="createTea" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc7" %>
<%@ Register Src="../UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc8" %>
<%@ Register Src="../UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc9" %>
<%@ Register Src="../UserControl/ConProjectControl.ascx" TagName="ConProjectControl"
    TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
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
                                所在位置&gt;&gt; 散拼计划&gt;&gt; 标准发布
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
                <%if (EyouSoft.Common.Utils.GetQueryStringValue("act") != "update")
                  {
                %>
                <li><a id="A1" href="QuickAdd.aspx">快速发布</a></li>
                <%} %>
                <li><a class="tabtwo-on" id="two1" href="#">标准发布</a></li>
            </ul>
            <div class="clearboth">
            </div>
            <div class="addlinebox">
                <div id="con_two_2">
                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>线路名称：
                                </td>
                                <td colspan="4">
                                    <uc6:xianluWindow Id="selectXl" runat="server" callBack="bindxlInfo" publishType="2"
                                        isRead="true" />
                                </td>
                            </tr>
                            <tr>
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>线路区域：
                                </td>
                                <td colspan="4">
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
                                <td colspan="4">
                                    <asp:TextBox ID="txt_Days" na="txt_Days" CssClass="searchinput searchinput03" runat="server"></asp:TextBox>
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
                                    <td colspan="4">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td height="25" class="xuexian" colspan="4">
                                        <uc1:createTea ID="createTea1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="30" align="right">
                                        &nbsp;
                                    </td>
                                    <td colspan="4">
                                        <div id="teamInfo">
                                            <a href="javascript:void(0)" class="teamInfo">请选择建团规则进行生成</a></div>
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
                                <td height="30" align="right" rowspan="2">
                                    出团信息：
                                </td>
                                <td align="left">
                                    参考去程航班/时间：<asp:TextBox ID="txt_startTraffic" Width="150px" CssClass="searchinput"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    参考回程航班/时间：
                                    <asp:TextBox ID="txt_endTraffic" CssClass="searchinput" Width="150px" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <uc7:selectOperator Title="送团人：" ID="SelectgroupPepole" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    集合时间：
                                    <asp:TextBox ID="txt_jh_date" CssClass="searchinput" runat="server" Style="width: 180px"></asp:TextBox>
                                </td>
                                <td>
                                    集合地点：
                                    <asp:TextBox ID="txt_jh_area" CssClass="searchinput" runat="server" Style="width: 180px"></asp:TextBox>
                                </td>
                                <td>
                                    集合标志：<asp:TextBox ID="txt_jh_logo" CssClass="searchinput" runat="server" Style="width: 180px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="right">
                                    地接社信息：
                                </td>
                                <td align="left" class="odd" colspan="3">
                                    <div class="div_dijieInfo">
                                        <uc8:DiJieControl ID="DiJieControl1" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" align="center" colspan="5">
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
                                                            <%#Eval("CustomStandName")%><input type="hidden" name="hd_cusStandType" value="<%#Eval("LevType") %>"><input
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
                                                  {%>
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
                                              }%>
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
                                <td height="10" align="center" colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="right">
                                    行程安排：
                                </td>
                                <td align="left" colspan="4">
                                    <uc9:xingcheng ID="xingcheng1" runat="server" daysControlName="txt_Days" />
                                </td>
                            </tr>
                            <tr>
                                <td height="40" background="/images/bg003.gif" align="right">
                                    添加附件：
                                </td>
                                <td background="/images/bg003.gif" colspan="4">
                                    <div style="float: left; margin-right: 10px;">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlFile" runat="server" Visible="false">
                                        <img src="/images/open-dx.gif" /><input type="hidden" runat="server" id="hd_img"
                                            runat="server" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:HyperLink
                                            ID="hypFilePath" runat="server" Target="_blank">查看附件</asp:HyperLink>
                                        <a href="javascript:void(0);" id="DeleFile">
                                            <img src="/images/fujian_x.gif" /></a>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td height="30" align="right" colspan="5">
                                    <table width="100%" cellspacing="1" cellpadding="0" border="0">
                                        <tbody>
                                            <tr bgcolor="#bddcf4">
                                                <th height="25" width="70" align="center">
                                                    &nbsp;
                                                </th>
                                                <th align="center" width="100px">
                                                    项目
                                                    <th align="center" width="535px">
                                                        接待标准
                                                    </th>
                                                    <th align="center" width="100px">
                                                        操作
                                                    </th>
                                            </tr>
                                            <tr class="trbhxm">
                                                <td height="60" bgcolor="#e3f1fc" align="center">
                                                    包含项目：
                                                </td>
                                                <td bgcolor="#e3f1fc" align="center" colspan="3">
                                                    <uc10:ConProjectControl ID="ConProjectControl1" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#bddcf4" align="center">
                                                    不含项目：
                                                </td>
                                                <td bgcolor="#bddcf4" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_noProject" runat="server" name="txt_noProject"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#e3f1fc" align="center">
                                                    购物安排：
                                                </td>
                                                <td bgcolor="#e3f1fc" align="left" colspan="3">
                                                    <textarea class="textareastyle" runat="server" id="txt_buy" name="txt_buy"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#bddcf4" align="center">
                                                    自费项目：
                                                </td>
                                                <td bgcolor="#bddcf4" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_owner" runat="server" name="txt_owner"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#e3f1fc" align="center">
                                                    儿童安排：
                                                </td>
                                                <td bgcolor="#e3f1fc" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_child" runat="server" name="txt_child"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#bddcf4" align="center">
                                                    注意事项：
                                                </td>
                                                <td bgcolor="#bddcf4" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_Note" runat="server" name="txt_Note"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="60" bgcolor="#e3f1fc" align="center">
                                                    温馨提醒：
                                                </td>
                                                <td bgcolor="#e3f1fc" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_Reminded" runat="server" name="txt_Reminded"></textarea>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10" align="center" colspan="4">
                                                </td>
                                            </tr>
                                            <tr bgcolor="#bddcf4">
                                                <th height="30" align="center" style="border: 1px solid rgb(147, 181, 215);" colspan="4">
                                                    内部信息（内部注意事项，内部资料等）
                                                </th>
                                            </tr>
                                            <tr>
                                                <td height="10" align="center" colspan="4">
                                                </td>
                                            </tr>
                                            <tr bgcolor="#bddcf4">
                                                <td bgcolor="#e3f1fc" height="30" align="center">
                                                    内部信息：
                                                </td>
                                                <td bgcolor="#e3f1fc" align="left" colspan="3">
                                                    <textarea class="textareastyle" id="txt_nbinfo" runat="server" name="txt_nbinfo"></textarea>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <ul class="tjbtn">
                    <li>
                        <asp:LinkButton ID="lbtn_save" runat="server" OnClick="lbtn_save_Click">保存</asp:LinkButton></li>
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

    function isnull(v, defaultValue) {
        if (v == null || !v)
            return defaultValue;
        else
            return v;
    }
        $(function() {
            $("#DeleFile").click(function(){
                $(this).parent().remove();
            });
            $(".add_bhxm").click(function(){
                $(this).parents(".trbhxm").clone(true).insertAfter($(this).parents(".trbhxm")).children("input").val("").find("select").val("-1");
            });
        })

        function bindxlInfo() {
            var xlId=<%=selectXl.ClientID%>.Id();
             $.newAjax({url:"/ashx/xianluInfo.ashx?type=1",data:"id="+xlId+"&companyId=<%=CurrentUserCompanyID %>",type:"POST", dataType:"json",success: function(r) {
                    $("#<%=ddl_area.ClientID %>").val(r.xlArea);
                    $("#<%=txt_Days.ClientID %>").val(r.xlDays);            
                    <%=xingcheng1.ClientID %>.bind(r.xclist);
                    $("#<%=txt_noProject.ClientID %>").val(isnull(r.noProject,""));
                    $("#<%=txt_buy.ClientID %>").val(isnull(r.buy,""));
                    $("#<%=txt_child.ClientID %>").val(isnull(r.child,""));
                    $("#<%=txt_owner.ClientID %>").val(isnull(r.owner,""));
                    $("#<%=txt_Note.ClientID %>").val(isnull(r.Notes,""));
                    $("#<%=txt_Reminded.ClientID %>").val(isnull(r.Reminder,""));
                    ConProject.SetRowValue(r.projectList);
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
               
            if($("#<%=ddl_area.ClientID %>").val()!="-1")
            {
                $.getJSON("/ashx/getAreaOprator.ashx?areaId="+$("#<%=ddl_area.ClientID %>").val(),{date:new Date()},function(r){
                    $("#sel_oprator").get(0).length=0;
                    $("#sel_oprator").get(0).options[0]=new Option("-请选择-","");
                    for(var i= 0 ; i<r.length;i++)
                    {
                        $("#sel_oprator").get(0).options[i+1]=new Option(r[i].ContactName,r[i].UserId);
                    }
            $("#sel_oprator").val($("#hd_oprator").val());
                });
            }else
            {
                $("#sel_oprator").get(0).length=0;
                //$("#sel_oprator").get(0).options[0]=new Option("请先选择线路区域","");
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
                    //$("#sel_oprator").get(0).options[0]=new Option("请先选择线路区域","");
                    $("#sel_oprator").get(0).options[0]=new Option("-请选择-","");
                }
            });
            $(".addDijie").bind("click", function() {
                $(this).parent().clone(true).insertAfter($(this).parent()).children("input").val("").find("select").val("-1");
            });
            $(".addPrice").bind("click", function() {
                $(this).parent().parent().parent().clone(true).insertAfter($(this).parent().parent().parent()).find("input").val("").parents(".dlprice").find("select").val("-1");
            });
            $(".delDijie").bind("click", function() {
                if ($(this).parent().parent().children(".div_dijieInfo").length > 1)
                    $(this).parent().remove();
            });
            $(".delPrice").bind("click", function() {
                if ($(this).parent().parent().parent().parent()[0].children.length > 1)
                    $(this).parent().parent().parent().remove();
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
        
    </script>

</asp:Content>
