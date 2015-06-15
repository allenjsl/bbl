<%@ Page Language="C#" Title="������ͳ��_Ӫ������_��·��Ʒ��" AutoEventWireup="true" CodeBehind="MarketingAnalysisList.aspx.cs" Inherits="Web.MarketingAnalysisList" MasterPageFile="~/masterpage/Back.Master" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register assembly="ControlLibrary" namespace="ControlLibrary" tagprefix="cc1" %>

<%@ Register src="~/UserControl/selectOperator.ascx" tagname="selectOperator" tagprefix="uc2" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<asp:Content ID="cphMarketing" runat="server" ContentPlaceHolderID="c1">

    <form id="form1" runat="server">
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>
    <div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">��·��Ʒ��</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">����λ��>> ��·��Ʒ��>> Ӫ������</td>
            </tr>
            <tr>
              <td colspan="2" height="2" bgcolor="#000000"></td>
            </tr>
          </table>
        </div>
         <div class="hr_10"></div>
        <ul class="fbTab">
          <li><a href="#" id="two1" class="tabtwo-on">������ͳ��</a></li>
          <li><a href="/xianlu/MarketingAnalysis/TypeStatisticList.aspx" id="two2">������ͳ��</a></li>
          <li><a href="/xianlu/MarketingAnalysis/TimeStatisticsList.aspx"id="two3">��ʱ��ͳ��</a></li>
          <div class="clearboth"></div>
        </ul>
        <div class="hr_10"></div>
        <div id="con_two_1">
        	<table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
                <td><div class="searchbox" id="diySearchArea">
                    <label>���ţ�</label> 
                    <uc1:UCSelectDepartment ID="UCselectDepart1" runat="server" SetPicture="/images/sanping_04.gif" />
                    <input value="<%=Request.QueryString["Department"] %>" name="txtDepartment" type="text"  class="searchinput" id="txtDepartment"/>
                    <uc2:selectOperator ID="selectOperator2" Title="����Ա" 
                        runat="server" />
&nbsp;<label>�µ�ʱ�䣺</label>
                    <input name="txtTime0" type="text"  class="searchinput" id="txtTime0" 
                    value="<%=Request.QueryString["TimeStar"] %>"
                        size="20" onfocus="WdatePicker();"/> ��   
                    <input type="text" name="txtTiemTo0" id="txtTiemTo0" class="searchinput" value="<%=Request.QueryString["TimeEnd"] %>" onfocus="WdatePicker();" />
                    <label>
                        <input id="btnSearch0" type="image" value="button" 
                        src="/images/searchbtn.gif" style="vertical-align:top;" alt="��ѯ" /></label>
                  </div></td>
                <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
              </tr>
            </table> 
            <div class="btnbox">
              <table width="40%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                  <td width="90" align="center"><a id="aPrint" href="javascript:void(0);" target="_blank" onclick="MarketAnalysitList.GetPrintInfo(aPrint);return false;">�� ӡ</a></td>
                  <td width="90" align="center">
                      <a href="/xianlu/MarketingAnalysisList.aspx?IsExcel=1"><img src="/images/daoru.gif"/> �� ��</a></td>
                  <td width="90" align="center"><a href="javascript:;" id="linkTable">ͳ�Ʊ�</a></td>
                  <td width="90" align="center"><a href="javascript:;" id="linkStats">ͳ��ͼ</a></td>
                </tr>
              </table>         
            </div>
          <div class="tablelist">
          <table id="StatTable" width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                  <th width="13%" align="center" bgcolor="#BDDCF4">��·����</th>
                  <th width="15%" align="center" bgcolor="#bddcf4">����</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">����Ա</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">�Ƶ�Ա</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">����</th>
                  <th width="13%" align="center" bgcolor="#bddcf4">������</th>
                </tr>
              <cc1:CustomRepeater ID="CustomRepeater1" runat="server" 
              EmptyText="<tr><td colspan='6' align='center'>����ͳ������</td></tr>">
              <HeaderTemplate>
                
              </HeaderTemplate>
              <ItemTemplate>
                <tr bgcolor="#E3F1FC">
                  <td align="center">�Ϻ���</td>
                  <td align="center">���۲�</td>
                  <td align="center">2006-04-05</td>
                  <td align="center">����</td>
                  <td align="center">20</td>
                  <td align="center" class="talbe_btn">20��10</td>
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
        var Params={LineArea:"",Department: "",DepartmentId:"",Xiaosy: "",XiaosyId: "",TimeStar: "",TimeEnd: "",Number:"",ManDays:""};
        var MarketAnalysitList={
            //��ѯ
            OnSearch: function()
            {
                //��ȡѡ�ÿؼ�
                 var departControl = <%=UCselectDepart1.ClientID %>;  
                 var salesManControl=<%=selectOperator2.ClientID %>;
                 var JiPYContror=<%=selectOperator1.ClientID %>;
                 Params.Department = departControl.GetName();
                 Params.DepartmentId = departControl.GetId();
                 Params.TimeEnd=$("#txtTiemTo0").val();
                 Params.TimeStar = $("#txtTime0").val();     
                 Params.Xiaosy = salesManControl.GetOperatorName();
                 Params.XiaosyId=salesManControl.GetOperatorId();
                 
                 location.href = "/xianlu/MarketingAnalysis/MarketingAnalysisList.aspx?"+$.param(Params);
            },
            //��ӡ
           GetPrintInfo: function(ControlID) {
              MarketAnalysitList.OnSearch();
              var url="PrintAnalysit.aspx?LineArea="+Params.LineArea+"&Department="+Params.Department+"&SalesMan="+Params.Xiaosy+"&Jidy="+Params.Jidy+"&Number="+Params.Number+"&ManDays="+Params.ManDays;
              window.open(url);
           }
       };
           
           
           $(document).ready(function()
           {
            //�س���ѯ
             $("#diySearchArea input").bind("keypress", function(e) {
                 if (e.keyCode == 13) {
                     MarketAnalysitList.OnSearch();
                     return false;
                 }
             });
             $("#btnSearch0").click(function() {
                 MarketAnalysitList.OnSearch();
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
                s = "/xianlu/MarketingAnalysis/MarketingAnalysisList.aspx?getXml=1"+s;
                $("#chat").html("��������ͳ��ͼ...");
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


