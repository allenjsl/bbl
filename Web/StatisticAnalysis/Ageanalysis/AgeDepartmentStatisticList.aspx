<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/masterpage/Back.Master" Title="�������_ͳ�Ʒ���"
    CodeBehind="AgeDepartmentStatisticList.aspx.cs" Inherits="Web.StatisticAnalysis.Ageanalysis.AgeDepartmentStatisticList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="../../UserControl/selectOperator.ascx" TagName="selectOperator"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">

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
                        ����λ��>>ͳ�Ʒ��� >> �������
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
       <%-- <ul class="fbTab">
            <li><a href="/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx" id="two2"
                class="tabtwo-on">������ͳ��</a></li>
            <div class="clearboth">
            </div>
        </ul>--%>
        <ul class="fbTab">
            <li><a href="javascript:void(0)" id="two1" class="tabtwo-on">
                ������Աͳ��</a></li>
            <li><a href="ZhangLingAnKeHuDanWei.aspx" id="two2" class="aw130">
                ���ͻ���λͳ��</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div>
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10" valign="top">
                        <img src="/images/yuanleft.gif" />
                    </td>
                    <td>
                        <div class="searchbox">
                            <uc3:TourTypeList TourType="-1" ID="TourTypeList1" runat="server" />
                            <uc4:RouteAreaList ID="RouteAreaList1" runat="server" />
                            <%--����Ա--%>
                            <uc1:selectOperator ID="SelectSalers" runat="server" />
                            <br />
                            <label>
                                �������ڣ�</label>
                            <input name="txtStartDate" runat="server" type="text" onfocus="WdatePicker()" class="searchinput"
                                id="txtStartDate" />��<input name="txtEndDate" onfocus="WdatePicker()" type="text"
                                    runat="server" class="searchinput" id="txtEndDate" />
                            <label>
                                <img id="btnSearch" src="/images/searchbtn.gif" style="vertical-align: top;cursor:pointer;" /></label>
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
                            <uc2:UCPrintButton ContentId="tbl_GetAgeDepartStaList" ID="UCPrintButton1" runat="server" />
                        </td>
                        <td width="90" align="left">
                            <a href="javascript:void(0);" id="btnExport">
                                <img src="/images/daoru.gif" />
                                �� �� </a>
                        </td>
                        <!-- <td width="90" align="left"><a href="xsfxrent_list.html">ͳ�Ʊ�</a> </td>-->
                    </tr>
                </table>
            </div>
            <div class="tablelist" id="divAgeDepartStaList" runat="server">
                <table width="100%" id="tbl_GetAgeDepartStaList" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <th width="19%" align="center" bgcolor="#BDDCF4">
                            ����Ա
                        </th>
                        <th width="19%" align="center" bgcolor="#bddcf4">
                            ��Ƿ���ܶ�
                        </th>
                        <th width="19%" align="center" bgcolor="#bddcf4">
                            ���Ƿʱ��
                        </th>
                    </tr>
                    <cc1:CustomRepeater ID="crp_GetAgeDepartStaList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <a href="javascript:void(0);" onclick="return GetTotalArrea(<%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorId%>)"
                                        id="link1">
                                        <%#Eval("ArrearageSum","{0:c2}")%></a>
                                </td>
                                <td align="center" bgcolor="#e3f1fc">
                                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#bddcf4">
                                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <a href="javascript:void(0);" onclick="return GetTotalArrea(<%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorId%>)"
                                        id="link1">
                                        <%#Eval("ArrearageSum","{0:c2}")%></a>
                                </td>
                                <td align="center" bgcolor="#bddcf4">
                                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </cc1:CustomRepeater>
                </table>
                <table id="tbl_ExportPage" runat="server" width="100%" border="0" cellspacing="0"
                    cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <cc1:CustomRepeater ID="crp_PrintGetAgeDepartStaList" Visible="false" runat="server">
        <HeaderTemplate>
            <table width="100%" id="Table1" border="1" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="19%" align="center">
                        ����Ա
                    </th>
                    <th width="19%" align="center">
                        ��Ƿ���ܶ�
                    </th>
                    <th width="19%" align="center">
                        ���Ƿʱ��
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                        <%#Eval("ArrearageSum","{0:c2}")%>
                </td>
                <td align="center">
                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td align="center">
                    <%# ((EyouSoft.Model.StatisticStructure.AccountAgeStatistic)GetDataItem()).SalesClerk.OperatorName%>
                </td>
                <td align="center">
                        <%#Eval("ArrearageSum","{0:c2}")%>
                </td>
                <td align="center">
                    <%# GetMaxDay( Eval("MaxArrearageTime").ToString())%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </cc1:CustomRepeater>
    </form>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnSearch").click(function() {
                var params = { tourType: "",routeAreaName:"", Salser:"",SalserId: "" ,startDate:"",endDate:""};
                var SalserControlObj = <%=SelectSalers.ClientID %>; //��ȡ���ۿؼ�����
                params.tourType = <%=TourTypeList1.ClientID %>.GetTourTypeId(); //�Ŷ�����
                params.Salser = SalserControlObj.GetOperatorName(); //������������
                params.SalserId = SalserControlObj.GetOperatorId(); //��������ID
                params.routeAreaName=<%=RouteAreaList1.ClientID %>.GetAreaId();//��·����
                params.startDate=$.trim($("#<%=txtStartDate.ClientID %>").val());//���ſ�ʼ����
                params.endDate=$.trim($("#<%=txtEndDate.ClientID %>").val());//���Ž�������
                window.location.href = "/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx?" + $.param(params);
            });           
            $("#btnExport").click(function() {//����
            if ($("#tbl_GetAgeDepartStaList").find("[id='EmptyData']").length>0) {
                    alert("���������޷�ִ�е�����");
                    return false;
                }
                var goToUrl = "/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx?isExport=1";
                window.open(goToUrl, "Excel����");
                return false;
            });             
        });
    </script>

    <script type="text/javascript">
             ///��Ƿ���ܽ��
             function GetTotalArrea(SalerId){
             var data={tourType: <%=TourTypeList1.ClientID %>.GetTourTypeId(),routeAreaName:<%=RouteAreaList1.ClientID %>.GetAreaId(),startDate:$.trim($("#<%=txtStartDate.ClientID %>").val()),endDate:$.trim($("#<%=txtEndDate.ClientID %>").val()),SalerId:SalerId};
					var url = "/StatisticAnalysis/Ageanalysis/GetArrearageList.aspx";
					Boxy.iframeDialog({
						iframeUrl:url,
						title:"��Ƿ���ܶ�����",
						modal:true,
						data:data,
						width:"750px",
						height:"740px"
					});
					return false;
		    }
    </script>

</asp:Content>
