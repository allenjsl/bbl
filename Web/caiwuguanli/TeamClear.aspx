<%@ Page Title="团款收入" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="TeamClear.aspx.cs" Inherits="Web.caiwuguanli.TeamClear" %>

<%@ Register src="../UserControl/selectOperator.ascx" tagname="selectOperator" tagprefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
<style>
    .rpt_td{ border-collapse:collapse; border-right:none;}
.rpt_td td {overflow:hidden; border-right:1px solid #fff;border-bottom:1px solid #fff;word-break:break-all; vertical-align:middle;}
</style>
<%--<script>
    $(function() {
        $(".rpt_td").each(function() {
            $(this).height($(this).closest("td").height());
        });

        $(".rpt_td td").each(function() {
            $(this).css("line-height", "30px");
        });
        if (window.screen.width == 1024) {
            //$("#table001").width("800px");
        }
        if (window.screen.width >= 1280) {
            //$("#table001").width("1055px");
            //$(".colspan").width("640px");
            $(".td1").css("width", "77px")
            $(".td2").css("width", "77px");
            $(".td3").css("width", "77px")
            $(".td4").css("width", "77px");
            $(".td5").css("width", "78px")
            $(".td6").css("width", "77px");
            $(".td7").css("width", "110px")
            $(".td8").css("width", "88px");
            var browser = navigator.appName
            var b_version = navigator.appVersion
            var version = b_version.split(";");
            var trim_Version = version[1].replace(/[ ]/g, "");
            if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE7.0") {
                $(".td1").css("width", "76px")
                $(".td2").css("width", "76px");
                $(".td3").css("width", "75px")
                $(".td4").css("width", "75px");
                $(".td5").css("width", "76px")
                $(".td6").css("width", "76px");
                $(".td7").css("width", "116px")
                $(".td8").css("width", "84px");
            }
            else if (browser == "Microsoft Internet Explorer" && trim_Version == "MSIE6.0") {
                $(".td1").css("width", "76px")
                $(".td2").css("width", "76px");
                $(".td3").css("width", "75px")
                $(".td4").css("width", "76px");
                $(".td5").css("width", "76px")
                $(".td6").css("width", "76px");
                $(".td7").css("width", "116px")
                $(".td8").css("width", "84px");
            }

        }
    });
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
           
    <script type="text/javascript">
        $(function() {
            $(".soukuan").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "查看",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });
            $(".tuikuan").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "退款",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });
            $("#btn2").click(function() {
                new Boxy("&lt;div style='width:400px;height:200px;background-color:gray'&gt;自定义HTML显示&lt;/div&gt;", { title: "自定义HTML" });
            });
        });
        
        function DownExcel() {
            var param = "teamtype=" + $("#<%=sl_teamType.ClientID%>").val();
                param += "&order=" + $("#<%=txt_order.ClientID %>").val();
                param += "&teamNum=" + $("#<%=txt_teamNum.ClientID %>").val();
                param += "&com=" + $("#<%=txt_Source.ClientID %>").val();
                param += "&OperatorId="+<%=selectOperator1.ClientID %>.GetOperatorId();
                param += "&OperatorName="+escape(<%=selectOperator1.ClientID %>.GetOperatorName());
                param += "&begin="+$("#<%=txt_goDate.ClientID %>").val();
                param += "&addDate="+$("#<%=txt_addDate.ClientID %>").val();
                param += "&status="+$("#<%=seleStatus.ClientID %>").val();
                param += "&method=downexcel" + "&recordcount=<%=count %>";
                param+="&rdate="+$("#txtRDate").val();
                param+='&regenddate='+$("#txtRegEDate").val();
                location.href = "TeamClear.aspx?" + param;
                return false;
        }
        
        //自定义排序 _v排序类型
        function sortSelf(_v) {
            var params = utilsUri.getUrlParams(["page", "sorttype"]);
            params["sorttype"] = _v;
            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
</script>
			<div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody><tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">财务管理</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">所在位置&gt;&gt;财务管理 &gt;&gt; 团款收入</td>
            </tr>
            <tr>
              <td height="2" bgcolor="#000000" colspan="2"></td>
            </tr>
          </tbody></table>
        </div>
        <div class="hr_10"></div>
        <ul class="fbTab">
          <li><a class="" id="two1" href="srtuankuan_list.aspx">应收账款</a></li>
          <li><a id="two2" href="#" class="tabtwo-on">已结清账款</a></li>
           <li><a class="" id="two3" href="piliangshenhe.aspx">批量审核</a></li>
          <div class="clearboth"></div>
        </ul>
        <div class="hr_10"></div>
        <div style="display: block;" id="con_two_2">
        	<table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
              <tbody><tr>
                <td width="10" valign="top"><img src="../images/yuanleft.gif"></td>
                <td><div class="searchbox">
                    <label>团队类型：</label>
                  <select id="sl_teamType" name="sl_teamType" runat="server" >
                    <option value="-1">请选择</option>
                    <option value="0">散拼计划</option>
                    <option value="1">团队计划</option>
                    <option value="2">单项服务</option>
                    <%--<option value="1">组团散拼</option>
                    <option value="2">地接散拼</option>
                    <option value="3">出境散拼</option>
                    <option value="4">组团团队</option>
                      <option value="5">地接团队</option>
                      <option value="6">出境团队</option>
                      <option value="7">入境团队</option>
                      <option value="8">无计划散客</option>
                      <option value="9">单项服务</option>--%>
                  </select>
                    <label>订单号：</label> <input type="text" id="txt_order" runat="server" class="searchinput" name="txt_order">
                    <label>团号：</label> <input type="text" id="txt_teamNum" runat="server" class="searchinput" name="txt_teamNum">
                    <label>客源单位：</label> <input type="text" class="searchinput searchinput02" id="txt_Source" name="txt_Source" runat="server"><br>
                    <uc2:selectoperator ID="selectOperator1" runat="server" Title="销售员" />
                    <label>出团时间：</label> <input type="text" onfocus="WdatePicker()" id="txt_goDate" class="searchinput" name="txt_goDate" runat="server"> 
                    至 
                    <input type="text" onfocus="WdatePicker()" id="txtRDate" class="searchinput" name="txtRdate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("rdate") %>" />
                    <label>收款日期：</label> <input type="text" onfocus="WdatePicker()" id="txt_addDate" class="searchinput" name="txt_addDate" runat="server" />
                    至
                    <input type="text" onfocus="WdatePicker()" id="txtRegEDate" class="searchinput" name="txtRegEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("regenddate") %>" />
                    <label>状态：</label><select id="seleStatus" runat="server">
                      <option  value="-1">--请选择--</option><option value="1">未审批</option>
                    </select>
                    <label>
            <asp:ImageButton ID="ImageButton1" style=" vertical-align:middle;" runat="server" ImageUrl="/images/searchbtn.gif" 
                        onclick="ImageButton1_Click"/></label>
                <a href="javascript:;" onclick="DownExcel();return false;">
                    <img src="/images/export.xls.png" alt="" width="68" height="27" style="margin-top:0px;border:0px" />
                </a>
                  </div></td>
                <td width="10" valign="top"><img src="../images/yuanright.gif"></td>
              </tr>
            </tbody></table>
            <div class="hr_10"></div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
          <table width="100%" cellspacing="1" cellpadding="0" border="0">
            <tbody><tr>
              <%--<th width="3%" bgcolor="#bddcf4" align="center">&nbsp;</th>--%>
              <th  bgcolor="#bddcf4" align="center" width="8%">团号</th>
              <th  bgcolor="#bddcf4" align="center" width="20%">线路名称</th>
              <th  bgcolor="#bddcf4" align="center" width="8%"><a onclick="return sortSelf(0)" style="color: " href="javascript:void(0)">↑</a>出团日期<a onclick="return sortSelf(1)" style="color: " href="javascript:void(0)">↓</a></th>
              <%--<th  bgcolor="#bddcf4" align="center" width="11%">状态</th>--%>
              <th  bgcolor="#bddcf4" align="center" width="15%">客源单位</th>
              <th  bgcolor="#bddcf4" align="center">人数</th>
              <th  bgcolor="#bddcf4" align="center">销售员</th>              
                        <th bgcolor="#bddcf4" align="center">
                            对方操作人
                        </th>
              <th bgcolor="#bddcf4" align="center">对方团号</th>
              <th  bgcolor="#bddcf4" align="center">应收款</th>
              <th  bgcolor="#bddcf4" align="center">已收款</th>
<%--              <th width="7%" bgcolor="#bddcf4" align="center">未收款</th>
              <th width="7%" bgcolor="#bddcf4" align="center">待审核</th>--%>              
              <th bgcolor="#bddcf4" align="center">操作</th>
            </tr>
            <asp:Repeater runat="server" ID="rpt_list1" OnItemDataBound="rpt_list1_ItemDataBound">
            <ItemTemplate>
            <tr class="<%=i%2==0 ?"even":"odd" %>">
              <%--<td align="center"><input type="checkbox" id="checkbox" name="checkbox"></td>--%>
              <td align="center" <asp:Literal ID="Literal1" runat="server"></asp:Literal>><%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? Eval("TourNo") : Eval("TourNo")%><asp:Literal ID="ltrTourType" runat="server"></asp:Literal></td>
              <td align="center" <asp:Literal ID="Literal2" runat="server"></asp:Literal>><%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ? "单项服务" : Eval("RouteName")%></td>
              <td align="center" <asp:Literal ID="Literal3" runat="server"></asp:Literal>><%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 ?"":DateTime.Parse( Eval("LeaveDate").ToString()).ToString("yyyy-MM-dd")%></td>
              
               <%--<td align="center" <asp:Literal ID="Literal4" runat="server"></asp:Literal>><%#Eval("TourStatus")%></td>--%>
             <%j=0; %>
              <asp:Repeater ID="rpt_sList" runat="server">
              <ItemTemplate>
              <%if(j>0){ %>
              
              <tr class='<%=i%2==0?"even":"odd"%>'>
              <%} %>
              <td align="center" width="82" class="td7"><%#Eval("CompanyName")%></td>
               <td align="center"  class="td1" style="width:58px;_width:57px;" class="talbe_btn"><%#Eval("PepoleNum")%></td>
              <td align="center"  class="td2" style="width:58px;_width:57px;"><%#Eval("SalerName")%></td>              
               <td align="center" class="td4" style="width: 58px; _width: 57px;"><%#Eval("BuyerContactName")%></td>
               <td><%#Eval("BuyerTourCode") %></td>
              <td align="center" class="td3" style="width:58px;_width:56px;"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("FinanceSum").ToString())%></font></td>
              <td align="center" width="57" class="talbe_btn td4"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("HasCheckMoney").ToString())%></font></td>
   <%--           <td align="center" width="58" class="talbe_btn td5"><font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotReciveMoney").ToString())%></font></td>
              <td align="center" width="58" class="td6"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotCheckMoney").ToString())%></td>--%>
              <td align="center" width="65" class="td8"><a id="link1" class="soukuan" href="/sales/sale_shoukuan.aspx?act=pass&OrderID=<%#Eval("OrderId") %>"><font class="fblue">查看</font></a> <a class="tuikuan" href="/sales/sale_tuikuan.aspx?act=pass&OrderID=<%#Eval("OrderId") %>"><font class="fblue">退款</font></a></td>
              </tr>
             <%j++; %>
              </ItemTemplate>
              </asp:Repeater> 
            </tr>
            <%i++; %>
            </ItemTemplate>
            
            </asp:Repeater>
            <tr>
              <th bgcolor="#bddcf4"  colspan="4" align="left">合计：</th>
              <td width="7%" bgcolor="#bddcf4" align="center"><%=peopleNumber%></td>
              <td width="7%" bgcolor="#bddcf4" align="center"></td><td bgcolor="#bddcf4"></td><td bgcolor="#bddcf4"></td>
              <td width="7%" bgcolor="#bddcf4" align="center" class="talbe_btn td4"><font class="fred">￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(financeSum)%></font></td>
              <td width="7%" bgcolor="#bddcf4" align="center" class="talbe_btn td4"><font class="fred">￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(hasCheckMoney)%></font></td>
              <td bgcolor="#bddcf4" align="center" colspan="2"></td>
            </tr>
            <tr>
              <td height="30" align="right" class="pageup" colspan="12">              
                  <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />              
              </td>
            </tr>
          </tbody></table>
        </div>
        </div>
      </div>
			<!-- InstanceEndEditable -->            </div>
      
    </form>
</asp:Content>
