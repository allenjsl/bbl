<%@ Page Title="散客天天发-散拼计划" Language="C#" ValidateRequest="false" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="DaydayQuickAdd.aspx.cs" Inherits="Web.sanping.DaydayQuickAdd" %>

<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register src="/UserControl/createTea.ascx" tagname="createTea" tagprefix="uc2" %>
<%@ Register src="/UserControl/DiJieControl.ascx" tagname="DiJieControl" tagprefix="uc5" %>
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
                                 所在位置&gt;&gt; 散客天天发&gt;&gt; 快速发布
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
                <li><a id="A1" href="DaydayAdd.aspx">标准发布</a></li>
                <%} %>
                <div class="clearboth">
                </div>
            </ul>
            <div class="addlinebox">
                <div id="con_two_1">
                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                        <tbody>
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
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>线路名称：
                                </td>
                                <td colspan="3">
                                    <uc1:xianluWindow Id="selectXl" runat="server" callBack="bindxlInfo" publishType="1"/>
                                </td>
                            </tr>     
                            <tr>
                                <td width="10%" height="30" align="right">
                                    <font class="xinghao">*</font>天数：
                                </td>
                                <td colspan="3">
                                    <table width="300" cellspacing="0" cellpadding="0" border="0">
                                        <tbody>
                                            <tr>
                                                <td width="65" align="left">
                                                    <asp:TextBox ID="txt_Days" CssClass="searchinput searchinput03" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
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
                                <div style="background: url(/images/trbg.png) repeat;width:680px;">
                                    <div class="dlprice" style="width:470px">
                                       <div class="left" style="width:155px;">
                                            <div class="title">
                                                报价标准</div>
                                            <div>
                                            <select name="ddl_price"><%=BindPriceList()%></select>
                                            </div>
                                        </div>
                                        <asp:Repeater ID="rpt_Customer" runat="server" >
                                        <ItemTemplate>
                                        <div class="left" style="width:155px;">
                                            <div class="title">
                                                <%#Eval("CustomStandName")%><input type="hidden" name="hd_cusStandType" value="<%#(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)Eval("LevType")) %>"><input type="hidden" name="hd_cusStandId" value="<%#Eval("Id") %>"/><input type="hidden" name="hd_cusStandName" value="<%#Eval("CustomStandName") %>"/></div>
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
                                                添加 </a>
                                            <a href="javascript:void(0)" class="delPrice">
                                                <img width="15" height="16" src="/images/delicon01.gif" alt="">
                                                删除 </a>
                                        </div>
                                    </div>
                                    <div style="clear:both;"></div>
                                    </div>
                                    <%}
                                      else
                                      {
                                          for (int i = 0; i < sinfo.Count;i++ )
                                          { 
                                            %>
                                            
                                <div style="background: url(/images/trbg.png) repeat;width:680px;">
                                    <div class="dlprice" style="width:470px">
                                        <div class="left" style="width:155px;">
                                            <div class="title">
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
                                          {
                                              if (v.LevelId != 0)
                                              {
                                              %>
                                        <div class="left" style="width:155px;">
                                            <div class="title">
                                                <%=v.LevelName%>
                                                <input type="hidden" name="hd_cusStandType" value="<%=(int)((EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)(v.LevelType)) %>">
                                                <input type="hidden" name="hd_cusStandId" value="<%=v.LevelId %>"/>
                                                <input type="hidden" name="hd_cusStandName" value="<%=v.LevelName%>"/>
                                                </div>
                                            <div>
                                                <font class="xinghao">*</font>成人
                                                <input class="jiesuantext" id="text5" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal( v.AdultPrice )%>" name="txt_cr_price" type="text">
                                                儿童
                                                <input class="jiesuantext" id="text6" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(v.ChildrenPrice) %>" name="txt_rt_price" type="text">
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
                                            <a href="javascript:void(0)"  class="addPrice">
                                                <img src="/images/tianjiaicon01.gif" alt="" height="16" width="15">
                                                添加 </a>
                                            <a href="javascript:void(0)" class="delPrice">
                                                <img width="15" height="16" src="/images/delicon01.gif" alt="">
                                                删除 </a>
                                        </div>
                                    </div>
                                    <div style="clear:both;"></div>
                                    
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
                                    <div style="float:left; margin-right:0px;"><asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                    <asp:Panel ID="pnlFile" runat="server" Visible="false">
                                        <img src="/images/open-dx.gif" /><input type="hidden" runat="server" id="hd_img" runat="server" />
                                       &nbsp;&nbsp;<asp:HyperLink ID="hypFilePath" runat="server" Target="_blank">查看附件</asp:HyperLink>
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
                                </td>
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
                                    <asp:TextBox ID="txt_remark" runat="server" TextMode="MultiLine" Height="87px" 
                                        Width="630px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--快速发布-->
                <ul class="tjbtn">
                    <li><asp:LinkButton ID="lbtn_save"
                            runat="server" onclick="LinkButton1_Click">保存</asp:LinkButton></li>
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
                KE.html("<%=txt_xinchen.ClientID %>", r.xlXianchen)
                KE.html("<%=txt_fuwu.ClientID %>", r.xlFuwu)
                $("#<%=txt_Days.ClientID %>").val(r.xlDays);
                }
            });
        }
        $(function() {
            $("#DeleFile").click(function(){
                $(this).parent().remove();
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
                var ea=false;
                $("[name='txt_cr_price']").each(function(){
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
