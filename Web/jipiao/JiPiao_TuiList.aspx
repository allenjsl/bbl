<%@ Page Title="��Ʊͳ��_��Ʊ����" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="JiPiao_TuiList.aspx.cs" Inherits="Web.jipiao.JiPiao_TuiList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .FlightTD
        {
            border-top: solid 1px #fff;
            border-right: solid 1px #fff;
            text-align: center;
        }
    </style>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">��Ʊ����</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt; ��Ʊ����&gt;&gt; ��Ʊͳ��
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
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �źţ�
                                </label>
                                <asp:TextBox ID="txtTeamNum" runat="server" CssClass="searchinput">
                                </asp:TextBox>
                                <label>
                                    ��·����:
                                </label>
                                <asp:TextBox ID="txtAreaLine" runat="server" CssClass="searchinput">
                                </asp:TextBox>
                                <label>
                                    �������ڣ�</label>
                                <asp:TextBox ID="txtOutDate" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                                </asp:TextBox>
                                <label>
                                    ��</label>
                                <asp:TextBox ID="txtLeaveDateEnd" runat="server" CssClass="searchinput" onfocus="WdatePicker()">
                                </asp:TextBox>
                                <label>
                                    ���Σ�</label>
                                <asp:TextBox ID="txtAirSegment" runat="server" CssClass="searchinput">
                                </asp:TextBox>
                                <label>
                                    ���չ�˾��</label>
                                <asp:DropDownList ID="ddlAirCompany" runat="server">
                                </asp:DropDownList>
                                <label>
                                    ������:</label>
                                <asp:HiddenField ID="hideSupplierId" runat="server" />
                                <asp:TextBox ID="txtGroupsName" runat="server" CssClass="searchinput">
                                </asp:TextBox><a href="javascript:void(0);" id="a_GetGroups" class="selectTeam">
                                    <img src="/images/sanping_04.gif" width="28px" height="18px" border="0"></a>
                                <br />
                                <label>
                                    ���ţ�</label>
                                <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="UCselectDepart" runat="server" />
                                &nbsp;<uc2:selectOperator Title="�������Ա��" ID="SelectOperator" runat="server" />
                                <a href="javascript:void(0)" id="btnSearch">
                                    <img style="vertical-align: top;" src="/images/searchbtn.gif"></a>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox" style="display: none;">
                <table cellspacing="0" cellpadding="0" border="0" align="left" width="40%">
                    <tbody>
                        <tr>
                            <td align="center" width="90">
                            </td>
                            <td align="center" width="90">
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th height="30" bgcolor="#bddcf4" align="center" width="8%">
                                �ź�
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                Ʊ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ��������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ��Ʊ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="80px">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="80px">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="100px">
                                �����/ʱ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="100px">
                                ���չ�˾
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="50px">
                                �ۿ�
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                Ʊ��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                ���������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                ������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                �Է�������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="5%">
                                �˻ؽ��
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <tr bgcolor="<%#Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                    <td align="center">
                                        <%#Eval("TourNo")%>
                                    </td>
                                    <td align="center">
                                        <%#getPiaoNum(Eval("RefundFlight"))%>
                                    </td>
                                    <td align="center">
                                        <%#Convert.ToDateTime(Eval("LeaveDate")).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("RefundName")%>
                                    </td>
                                    <td align="center" colspan="5">
                                        <table width="100%" style="border: solid 1px #fff; border-collapse: collapse;" class="tbl">
                                            <%#GetHtmlByList(Eval("RefundFlight"), EyouSoft.Common.Utils.GetDateTime(Eval("LeaveDate").ToString()))%>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TicketPrice")).ToString("0.00"))%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("OperatorName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("BuyCompanyName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("BuyOrderOperatorName") %>
                                    </td>
                                    <td align="right">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("RefundAmount")).ToString("0.00"))%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td height="30" align="right" bgcolor="#BDDCF4" style="font-weight: bold;" colspan="13">
                                �ϼ�
                            </td>
                            <td align="center" bgcolor="#BDDCF4" style="font-weight: bold;">
                                <%= RefundAmount.ToString("F2") %>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="13">
                                <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                                    runat="server" />
                                <asp:Label ID="lblMsg" runat="server" Text="δ�ҵ�����!"></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript">
        $(function() {
            $(".tbl").each(function() {
                var height = $(this).parent("td").parent("tr");
                $(this).height(height.height() + 10);
            })
            $("#btnSearch").click(function() {
                var UCselectDepart = <%= UCselectDepart.ClientID %>;
                var SelectOperator = <%= SelectOperator.ClientID %>;
                var para = {
                    teamNum: "", areaLine: "", outStartDate: "", outEndDate: "", airSegment: "", airCompany: "", supplierId: "",
                    supplierName: "", depName: "", depId: "", opName: "", opId: ""
                };
                para.teamNum = $("#<%=txtTeamNum.ClientID %>").val();
                para.areaLine = $("#<%=txtAreaLine.ClientID %>").val();
                para.outStartDate = $("#<%=txtOutDate.ClientID %>").val();
                para.outEndDate = $("#<%=txtLeaveDateEnd.ClientID %>").val();
                para.airSegment = $("#<%=txtAirSegment.ClientID %>").val();
                para.airCompany = $("#<%=ddlAirCompany.ClientID %>").val();
                para.supplierId = $("#<%=hideSupplierId.ClientID %>").val();
                para.supplierName = $("#<%=txtGroupsName.ClientID %>").val();
                //��������
                para.depName = UCselectDepart.GetName();
                //����ID
                para.depId = UCselectDepart.GetId();
                //����Ա����
                para.opName = SelectOperator.GetOperatorName();
                //����ԱID
                para.opId = SelectOperator.GetOperatorId();

                window.location.href = "/jipiao/JiPiao_TuiList.aspx?" + $.param(para);

                return false;
            })

            //ѡ��ؽ����¼�
            $("#a_GetGroups").click(function() {
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
                var url = "/CRM/customerservice/SelCustomer.aspx?method=SetGroupsVal";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "ѡ��������",
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

            $(".searchbox").find("input[type='text']").keydown(function(e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            });



        })

        function SetGroupsVal(val, txt) {
            $("#<%=hideSupplierId.ClientID %>").val(val);
            $("#<%=txtGroupsName.ClientID %>").val(txt);
        }
    </script>

    </form>
</asp:Content>
