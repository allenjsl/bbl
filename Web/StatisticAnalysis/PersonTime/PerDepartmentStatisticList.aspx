<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/masterpage/Back.Master" Title="������ͳ��_�˴�ͳ��_ͳ�Ʒ���"
    CodeBehind="PerDepartmentStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.PersonTime.PerDepartmentStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc3" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

    <script type="text/javascript" src="/js/FusionChartsFree/FusionCharts.js"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">ͳ�Ʒ���</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>>ͳ�Ʒ��� >> �˴�ͳ��>>������ͳ��
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a href="/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx" id="two1">������ͳ��</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx" id="two2"
                class="tabtwo-on">������ͳ��</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx" id="two3">��ʱ��ͳ��</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <!-- ������ͳ��-->
        <div id="con_two_3" style="display: block;">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">                           
                           <uc4:RouteAreaList  ID="RouteAreaList1" runat="server" />
                            &nbsp; <%-- ����Ա--%>
                            <uc2:selectOperator Title="����Ա��" ID="SelectSalser" runat="server" />
                            <label>
                                ʱ��Σ�</label>
                            <input name="txtOrderStarTime" type="text" runat="server" class="searchinput" id="txtOrderStarTime"
                                onfocus="WdatePicker()" />
                            ��
                            <input name="txtOrderEndTime" type="text" runat="server" class="searchinput" id="txtOrderEndTime"
                                onfocus="WdatePicker()" />&nbsp;
                            <label>�ƻ����ͣ�</label>
                            <select name="txtTourType" id="txtTourType">
                                <%=TourTypeSearchOptionHTML%>
                            </select>&nbsp;
                            <label>
                                <input id="ImgbtnSearchByDepartment" src="/images/searchbtn.gif" style="vertical-align: top;cursor:pointer;"
                                    type="image" value="button" alt="��ѯ"/>
                            </label>
                        </div>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc3:UCPrintButton ContentId="tbl_PerDepartStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                �� �� </a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnPerStaTbl">ͳ�Ʊ�</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnCartogram">ͳ��ͼ</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divPerDepartList" runat="server">
                <table width="100%" id="tbl_PerDepartStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="25%" align="center" bgcolor="#BDDCF4">
                            ����
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            ����Ա
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4">
                            ����
                        </th>
                        <th width="25%" align="center" bgcolor="#bddcf4"> �Ŷ���</th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_PerDepartStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("DepartName")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#bddcf4"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                        <td colspan="2" align="right" bgcolor="#BDDCF4" style="font-weight: bold;">
                            �ϼ�
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                            <%= AllPopleNum %>
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;"><%=TuanDuiShuHeJi %></td>
                    </tr>
                </table>
                <table width="100%" id="tbl_PageInfo" runat="server" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc3:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <%--ͳ��ͼ��ʼ--%>
        <div class="tablelist">
            <table id="tbl_Cartogram" width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <div id="divContainer">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%-- ͳ��ͼ����--%>
        <cc1:CustomRepeater ID="crp_PrintPerDepartStaList" Visible="false" runat="server">
            <HeaderTemplate>
                <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="10%" align="center">
                            ����
                        </th>
                        <th width="12%" align="center">
                            ����Ա
                        </th>
                        <th width="10%" align="center">
                            ����
                        </th>
                        <th width="12%" align="center">
                            �Ŷ���
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("DepartName")%>
                    </td>
                    <td align="center">
                        <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center">
                        <%#Eval("TuanDuiShu")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("DepartName")%>
                    </td>
                    <td align="center">
                        <%#GetSalsers(((EyouSoft.Model.StatisticStructure.InayaDepartStatistic)GetDataItem()).SalesClerk)%>
                    </td>
                    <td align="center">
                        <%#Eval("PeopleCount")%>
                    </td>
                    <td align="center">
                        <%#Eval("TuanDuiShu")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </cc1:CustomRepeater>
    </div>
    </form>

    <script type="text/javascript">
        function GetParams(IsCartogram) { 
            var params = {RouteAreaId:"", Salser: "", SalserId: "" ,OrderStartTime:"",OrderEndTime:"",IsCartogram:"",tourtype:""};
            var SalerControlObj = <%=SelectSalser.ClientID %>;//��ȡ����Ա�ؼ�
            var Salser=SalerControlObj.GetOperatorName();//����Ա����
            var SalserId=SalerControlObj.GetOperatorId();//����ԱID
            var RouteAreaId=<%=RouteAreaList1.ClientID %>.GetAreaId();//��·����
            params.Salser=Salser;
            params.SalserId=SalserId;
            params.RouteAreaId=RouteAreaId;  
            params.OrderStartTime=$("#<%=txtOrderStarTime.ClientID %>").val();    
            params.OrderEndTime=$("#<%=txtOrderEndTime.ClientID %>").val();
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            params.tourtype=$("#txtTourType").val();
            return params;
        }
    $(function() {    
        $("#ImgbtnSearchByDepartment").click(function() {//��ѯ
                    window.location.href = "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx?" + $.param(GetParams());
                    return false;
        });       
        $("#btnExport").click(function(){//����
                   if($("#tbl_PerDepartStaList").find("[id='EmptyData']").length>0)
                   {
                        alert("���������޷�ִ�е�����");
                        return false;
                   }
                  var goToUrl = "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx?isExport=1";
                  window.open(goToUrl,"Excel����");
                  return false;
        });  
         $("#btnCartogram").click(function(){//ͳ��ͼ  
             $("#<%= divPerDepartList.ClientID %>").hide();
             $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx",
                     data: GetParams(1),
                     cache: false,
                     success: function(CartogramXml) {
                        if(CartogramXml!="")
                        {
                                var routeAreaStaticList = new FusionCharts("/js/FusionChartsFree/Charts/FCF_MSColumn2D.swf", "chart1Id", "750", "300", "0", "1");	   			
			                    routeAreaStaticList.setDataXML(CartogramXml);			
			                    routeAreaStaticList.render("divContainer");	
                        }
                        else 
                        {
                            $("#<%= divPerDepartList.ClientID %>").css("display","none");
                            $("#divContainer").html("��ʱû������ͳ�ƣ�");
                        }             	
                     },
                     error: function(xhr, s, errorThrow) {
                         $("#divContainer").html("δ�ܳɹ���ȡ��Ӧ���")
                     }
                 });
			    return false;
        });  
          $("#btnPerStaTbl").click(function(){//ͳ�Ʊ�
                $("#divContainer").css("display","none");
                $("#<%= divPerDepartList.ClientID %>").css("display","block");
                $("#divCartogram").css("display","none");
        });        

    });    
    </script>

</asp:Content>
