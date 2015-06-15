<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="/masterpage/Back.Master"
    CodeBehind="PerAreaStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.PersonTime.PerAreaStatisticList"
    Title="������ͳ��_�˴�ͳ��_ͳ�Ʒ���" %>

<%@ Import Namespace="EyouSoft.Common" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Src="../../UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
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
            <li><a href="/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx" id="two1" class="tabtwo-on">
                ������ͳ��</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx" id="two2">
                ������ͳ��</a></li>
            <li><a href="/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx" id="two3">��ʱ��ͳ��</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <%-- ��ѯ������ʼ--%>
                        <div class="searchbox">
                            <label>
                                ���ţ�</label>
                            <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
                            <uc2:selectOperator Title="����Ա��" ID="selectSals" runat="server" />
                            <label>
                                ����ʱ�䣺</label>
                            <input name="txtAreaStarTime" type="text" runat="server" class="searchinput" id="txtAreaStarTime"
                                onfocus="WdatePicker()" />
                            ��
                            <input name="txtAreaEndTime" type="text" runat="server" class="searchinput" id="txtAreaEndTime"
                                onfocus="WdatePicker()" />&nbsp;
                            <label>�ƻ����ͣ�</label>
                            <select name="txtTourType" id="txtTourType">
                                <%=TourTypeSearchOptionHTML%>
                            </select>&nbsp;
                            <label>
                                <input id="btnSearchByArea" src="/images/searchbtn.gif" style="vertical-align: top;
                                    cursor: pointer;" type="image" value="button" alt="��ѯ" />
                            </label>                            
                        </div>
                        <%--��ѯ��������--%>
                    </td>
                    <td width="10" valign="top">
                        <img src="/images/yuanright.gif" />
                    </td>
                </tr>
            </table>
            <%-- ������ʼ--%>
            <div class="btnbox">
                <table border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="90" align="left">
                            <uc3:UCPrintButton ContentId="tbl_PerAreaStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a id="linkExport" href="javascript:void(0);">
                                <img src="/images/daoru.gif" />����</a>
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnPerStaTbl">ͳ�Ʊ�</a>
                        </td>
                        <td width="90" align="left">
                            <a href='/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx' id="linkCartogram">
                                ͳ��ͼ</a>
                        </td>
                    </tr>
                </table>
            </div>
            <%--��������--%>
            <%--�б�������ʾ��ʼ--%>
            <div class="tablelist" id="divPerAreaList" runat="server">
                <table width="100%" id="tbl_PerAreaStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="20%" align="center" bgcolor="#BDDCF4">
                            ��·����
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            ����Ա
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            �Ƶ�Ա
                        </th>
                        <th width="20%" align="center" bgcolor="#bddcf4">
                            ����
                        </th>
                        <th align="center" bgcolor="#bddcf4">�Ŷ���</th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_PerAreaStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("AreaName")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#Eval("AreaName")%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4">
                                    <%#Eval("PeopleCount")%>
                                </td>
                                <td align="center" bgcolor="#BDDCF4"><%#Eval("TuanDuiShu")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                    <tr>
                        <td colspan="3" align="right" bgcolor="#BDDCF4" style="font-weight: bold;">
                            �ϼ�
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                            <%= AllPopleNum %>
                        </td>
                        <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;"><%= TuanDuiShuHeJi %></td>
                    </tr>
                </table>
                <table width="100%" border="0" id="tbl_ExportPage" runat="server" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                        </td>
                    </tr>
                </table>
            </div>
            <%-- �б����ݽ��� --%>
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
        </div>
    </div>
    <%-- ��ӡ��Reapete������ʼ������ʾ--%>
    <cc1:CustomRepeater ID="crp_PrintPerAreaStaList" runat="server" Visible="false">
        <HeaderTemplate>
            <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="16%" align="center">
                        ��·����
                    </th>
                    <th width="12%" align="center">
                        ����Ա
                    </th>
                    <th width="12%" align="center">
                        �Ƶ�Ա
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
                    <%#Eval("AreaName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
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
                    <%#Eval("AreaName")%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).SalesClerk)%>
                </td>
                <td align="center">
                    <%#GetAccouter(((EyouSoft.Model.StatisticStructure.InayaAreatStatistic)GetDataItem()).Logistics)%>
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
    <%-- ��ӡ��Reapete��������--%>

    <script type="text/javascript">
        function GetParams(IsCartogram) { 
            var params = {DepartId:"",DepartName:"", StartTime: "", EndTime: "", Salser: "", SalserId: "",IsCartogram:"",tourtype:"" };
            var SalerControlObj = <%=selectSals.ClientID %>;//��ȡ����Ա�ؼ�
            var Salser=SalerControlObj.GetOperatorName();//����Ա����
            var SalsID=SalerControlObj.GetOperatorId();//����ԱID
            var StartTime=$.trim($("#<%=txtAreaStarTime.ClientID%>").val());//��ʼʱ��
            var EndTime=$.trim($("#<%=txtAreaEndTime.ClientID%>").val());//����ʱ��  
            params.DepartId=<%=UCselectDepart.ClientID %>.GetId();//��ȡ����ID 
            params.DepartName=<%=UCselectDepart.ClientID %>.GetName();//��ȡ��������
            params.Salser=Salser;
            params.SalserId=SalsID;
            params.StartTime=StartTime;
            params.EndTime=EndTime;
            if(IsCartogram!=null)
                params.IsCartogram=IsCartogram;
            params.tourtype=$("#txtTourType").val();
            return params;
        }
    $(function() {    
        $("#btnSearchByArea").click(function() {//��ѯ
                window.location.href = "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx?" + $.param(GetParams());
                return false;
        });       
        $("#linkExport").click(function(){//����
               if($("#tbl_PerAreaStaList").find("[id='EmptyData']").length>0)
               {
                    alert("���������޷�ִ�е�����");
                    return false;
               }
              var goToUrl = "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx?isExport=1";
              window.open(goToUrl,"Excel����");
              return false;
        });  
         $("#linkCartogram").click(function(){//ͳ��ͼ 
             $("#<%= divPerAreaList.ClientID %>").hide();
             $("#divContainer").show();
             $.newAjax({
                     type: "GET",
                     dataType: 'text',
                     url: "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx",
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
            $("#<%=divPerAreaList.ClientID%>").show();
            $("#divContainer").hide();
        });
    });   
    </script>

    </form>
</asp:Content>
