<%@ Page Language="C#" Title="按时间统计_营销分析_线路产品库" AutoEventWireup="true" CodeBehind="TypeStatisticList.aspx.cs" Inherits="Web.xianlu.TypeStatisticList" MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register assembly="ControlLibrary" namespace="ControlLibrary" tagprefix="cc1" %>
<%@ Register src="~/UserControl/selectOperator.ascx" tagname="selectOperator" tagprefix="uc2" %>


<asp:Content ID="cphMarketing" runat="server" ContentPlaceHolderID="c1">

    <form id="form1" runat="server">
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>
    <div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">线路产品库</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> 线路产品库>> 营销分析</td>
            </tr>
            <tr>
              <td colspan="2" height="2" bgcolor="#000000"></td>
            </tr>
          </table>
        </div>
         <div class="hr_10"></div>
        <ul class="fbTab">
          <li><a href="/xianlu/MarketingAnalysis/MarketingAnalysisList.aspx" id="two1">按区域统计</a></li>
          <li><a href="javascript:;" id="two2" class="tabtwo-on">按类型统计</a></li>
          <li><a href="/xianlu/MarketingAnalysis/TimeStatisticsList.aspx" id="two3">按时间统计</a></li>
          <div class="clearboth"></div>
        </ul>
        <div class="hr_10"></div>
        <!--按类型统计-->
        <div id="con_two_2">
        	<table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
                <td><div class="searchbox">
                    <label>部门：</label> 
                    <input name="textfield" type="text"  class="searchinput" id="textfield"/>
                    <uc2:selectOperator ID="selectOperator2" runat="server" Title="销售员" />
&nbsp;<uc2:selectOperator ID="selectOperator3" runat="server" Title="计调员" />
&nbsp;<label>时间段：</label>
                    <input name="textfield" type="text"  class="searchinput" id="textfield" onfocus="WdatePicker();"/> 至 <input type="text" name="textfield" id="textfield" class="searchinput" onfocus="WdatePicker();" />
                    <label>
                        <input id="btnSearchType" type="image" value="button"  src="/images/searchbtn.gif" style="vertical-align:top;" alt="查询" /></label>
                  </div></td>
                <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
              </tr>
            </table>  
            <div class="btnbox">
              <table width="40%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                  <td width="90" align="center"><a id="aPrint" href="javascript:void(0);" target="_blank" onclick="TypeStatisticList.GetPrintInfo(aPrint);return false;">打 印</a></td>
                  <td width="90" align="center"><a href="/xianlu/TypeStatisticList.aspx?IsExcel=1"><img src="/images/daoru.gif"/> 导 出</a></td>
                  <td width="90" align="center"><a href="javascript:;" id="linkTable">统计表</a></td>
                  <td width="90" align="center"><a href="javascript:;" id="linkStats">统计图</a></td>
                </tr>
              </table>
            </div>
            <div class="tablelist">
            <table id="StatTable" width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                  <th width="13%" align="center" bgcolor="#BDDCF4">团队类型</th>
                  <th width="15%" align="center" bgcolor="#bddcf4">部门</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">销售员</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">计调员</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">人数</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">人天数</th>
                </tr>
              <cc1:CustomRepeater ID="CustomRepeater2" runat="server"
               EmptyText="<tr><td colspan='6' align='center'>暂无统计数据</td></tr>">
              <ItemTemplate>
                 <tr bgcolor="#E3F1FC">
                  <td align="center">团队</td>
                  <td align="center">销售部</td>
                  <td align="center">2006-04-05</td>
                  <td align="center">张三</td>
                  <td align="center">20</td>
                  <td align="center" class="talbe_btn">20×10</td>
                </tr>
              </ItemTemplate>
              </cc1:CustomRepeater>
           
            <tr>
              <td height="30" colspan="7" align="right" class="pageup">
                  <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
              </td>
            </tr>
          </table>
          
          <table id="StatImage" style="display:none;" width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td align="center"><div id="chat"></div></td>
              </tr>
            </table>
        </div>
        </div>
        
      </div>
      
      <script type="text/javascript" language="javascript">
        var Params={Type:"",Department: "",Xiaosy: "",XiaosyId: "",Jidy: "",JidyId: "",TimeStar: "",TimeEnd: "",Number:"",ManDays:""};
        var TypeStatisticList={
            //查询
            OnSearch: function()
            {
                //获取选用控件
                 var salesManControl=<%=selectOperator2.ClientID %>;
                 var JiPYContror=<%=selectOperator3.ClientID %>;
                 Params.Department = $("#txtDepartment").val().trim();
                 Params.Jidy =JiPYContror.GetOperatorName();      //选用名称  
                 Params.JidyId=JiPYContror.GetOperatorId();      //选用Id
                 Params.TimeEnd=$("#txtTimeOff").val().trim();
                 Params.TimeStar = $("#txtTimeOn").val().trim();     
                 Params.Xiaosy = salesManControl.GetOperatorName();
                 Params.XiaosyId=salesManControl.GetOperatorId();
                 
                 location.href = "/xianlu/MarketingAnalysis/TypeStatisticList.aspx?"+$.param(Params);
            },
            
           //打印
           GetPrintInfo: function(ControlID) { 
              TypeStatisticList.OnSearch();             
              var url="PrintTypeStatistic.aspx?Type="+Params.Type+"&Department="+Params.Department+"&SalesMan="+Params.Xiaosy+"&Jidy="+Params.Jidy+"&Number="+Params.Number+"&ManDays="+Params.ManDays;
              window.open(url);
           }
           
           };
           
           //回车查询
           $(document).ready(function()
           {
             $("#diySearchArea input").bind("keypress", function(e) {
                 if (e.keyCode == 13) {
                     TypeStatisticList.OnSearch();
                     return false;
                 }
             });
             $("#btnSearchType").click(function() {
                 TypeStatisticList.OnSearch();
                 return false;
             });
             
             $("#linkTable").click(function(){
                $("#StatImage").hide();
                $("#StatTable").show();
             });
             
             $("#linkStats").click(function(){
                $("#StatImage").show();
                $("#StatTable").hide();
                
                var s = window.location.search;
                if(s.indexOf("?")!=-1){
                    s = s.replace("?","");
                }
                if(s!=""){
                    s = "&"+s;
                }
                s = "/xianlu/MarketingAnalysis/TypeStatisticList.aspx?getXml=1"+s;
                $("#chat").html("正在生成统计图...");
                $.newAjax({
                    type:"get",
                    url:s,
                    cache:false,
                    success:function(data){
                        var chart1 = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn3D.swf ", "chart1Id", "500", "400", "0", "1");		   			
                        chart1.setDataXML(data);
                        chart1.render("chat");
                    }
                });
                
                return false;
             });
                
           });
      </script>
    </form>

</asp:Content>
