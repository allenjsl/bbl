<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="TeamAccount.aspx.cs" Inherits="Web.caiwuguanli.TeamAccount" %>

<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            color: #6f6f6f;
            padding-right: 10px;
            height: 29px;
        }
    </style>
    <title>�ŶӺ���_�������</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">�������</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt;������� &gt;&gt; �ŶӺ���
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
                <li><a id="a_no" href="/caiwuguanli/TeamAccount.aspx?isAccount=no">������</a></li>
                <li><a id="a_yes" href="/caiwuguanli/TeamAccount.aspx?isAccount=yes">�Ѻ���</a></li>
                <div class="clearboth">
                </div>
            </ul>
            <div class="hr_10">
            </div>
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �źţ�</label>
                                <asp:TextBox ID="txtTeamNum" runat="server" CssClass="searchinput"></asp:TextBox>
                                <label>
                                    ��·���ƣ�</label><uc1:xianluWindow Id="xianluWindow1" runat="server" />
                                &nbsp;
                                <label>
                                    ����ʱ�䣺</label>
                                <asp:TextBox ID="txtTime" runat="server" CssClass="searchinput" onfocus="WdatePicker()"></asp:TextBox>-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                                <br/>
                                <label>���ţ�</label>
                                <uc1:UCSelectDepartment SetPicture="/images/sanping_04.gif" ID="txtQueryDepart" runat="server" />
                                <uc2:selectOperator Title="����Ա��" ID="txtQueryOperator" runat="server" />
                                <label>�ƻ����ͣ�</label>
                                <select name="txtTourType" id="txtTourType">
                                    <%=TourTypeSearchOptionHTML%>
                                </select>
                                <label>
                                    <a href="javascript:void(0);" id="btnSearch">
                                        <img style="vertical-align: top;" src="../images/searchbtn.gif"></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="../images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="hr_10">
            </div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th style="background:#bddcf4; text-align:center;width:4%">���</th>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                �ź�
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                ��·����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ��������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ����Ա
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ֧��
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ë��
                            </th>
                            
                            <th style="background:#bddcf4; text-align:right;width:6%">�˾�ë��</th>
                            <th style="background:#bddcf4; text-align:right;width:6%">ë����</th>
                            
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                �������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="7%">
                                ����
                            </th>
                        </tr>
                        <asp:Repeater ID="rptList" runat="server">
                            <ItemTemplate>
                                <%#Container.ItemIndex %2==0? "<tr bgcolor=\"#e3f1fc\">":"<tr>"%>
                                <td style="text-align:center"><%#(pageSize*(pageIndex-1))+Container.ItemIndex+1 %></td>
                                <td align="center">
                                    <%#Eval("TourCode")%><%#Eval("TourTypeMark") %>
                                </td>
                                <td align="center">
                                    <%#GetUrlByType(Eval("RouteName").ToString(), Eval("TourType").ToString(), Eval("SingleServices"))%>
                                </td>
                                <td align="center">
                                    <%#(int)Eval("TourType") == 2 ? "" :Convert.ToDateTime(Eval("LDate")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td align="center" class="talbe_btn">
                                    <%#Eval("AdultNumber")%>+<font style="font-size: 12px;"><%#Eval("ChildrenNumber")%></font>
                                </td>
                                <td align="center">
                                    <%#Eval("OperatorName")%>
                                </td>
                                <td align="center">
                                    <font class="fred">��
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("InComeAmount")).ToString("0.00"))%></font>
                                </td>
                                <td align="center" class="talbe_btn">
                                    <font class="fred">��
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("OutAmount")).ToString("0.00"))%></font>
                                </td>
                                <td align="center" class="talbe_btn">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("GrossProfit")).ToString("0.00"))%>
                                </td>
                                
                                <td style="text-align:right"><%#Eval("RenJunMaoLi","{0:C2}") %></td>
                                <td style="text-align:right;"><%#string.Format("{0:F2}",(decimal)Eval("MaoLiLv")*100) %>%</td>
                                
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("DistributionAmount")).ToString("0.00"))%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("NetProfit")).ToString("0.00"))%>
                                </td>
                                <td align="center">
                                    <a href="/TeamPlan/TeamSettle.aspx?Account=<%=Request.QueryString["isAccount"]%>&type=account&id=<%#Eval("TourId") %>">�鿴��ϸ</a>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <th bgcolor="#bddcf4" colspan="4" align="left">
                                �ϼƣ�
                            </th>
                            <td bgcolor="#bddcf4" align="center">
                                <asp:Label ID="lblNumber" runat="server" Text="0"></asp:Label>
                            </td>
                            <td bgcolor="#bddcf4">
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <font class="fred">
                                    <asp:Label ID="lblincome" runat="server" Text="0"></asp:Label></font>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <font class="fred">
                                    <asp:Label ID="lblexpenditure" runat="server" Text="0"></asp:Label></font>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <asp:Label ID="lblml" runat="server" Text="0"></asp:Label>
                            </td>
                            <td style="background:#bddcf4; text-align:right"><asp:Literal runat="server" ID="ltrRenJunMaoLi"></asp:Literal></td>
                            <td style="background:#bddcf4; text-align:right"><asp:Literal runat="server" ID="ltrMaoLiLv"></asp:Literal></td>
                            <td bgcolor="#bddcf4" align="center">
                                <asp:Label ID="lblpayOff" runat="server" Text="0"></asp:Label>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <asp:Label ID="lblcl" runat="server" Text="0"></asp:Label>
                            </td>
                            <td bgcolor="#bddcf4">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style1" colspan="11">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="5" pagestyletype="NewButton"
                                    CurrencyPageCssClass="RedFnt" />
                                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            var isAccount = '<%=Request.QueryString["isAccount"]%>';
            $("#btnSearch").click(function() {
                var teamNum = $.trim($("#<%=txtTeamNum.ClientID %>").val());
                var id = <%=xianluWindow1.ClientID %>.Id();
                var name = <%=xianluWindow1.ClientID %>.val();
                var time = $("#<%=txtTime.ClientID %>").val();
                var leaveEndDate=$("#txtLeaveEDate").val();
                
                var _departObj = <%=txtQueryDepart.ClientID %>;
                var _operatorObj = <%=txtQueryOperator.ClientID %>;
                
                var _departids=_departObj.GetId();
                var _operatorids=_operatorObj.GetOperatorId();
                
                var _tourtype=$("#txtTourType").val();
                
                window.location.href = "/caiwuguanli/TeamAccount.aspx?teamNum=" + teamNum + "&xId=" + id + "&time=" + time + "&name=" + escape(name) + "&isAccount=" + isAccount+"&ledate="+leaveEndDate+"&departids="+_departids+"&operatorids="+_operatorids+"&tourtype="+_tourtype;
                return false;
            });

            if (isAccount == "yes") {
                $("#a_yes").addClass("tabtwo-on");
            } else {
                $("#a_no").addClass("tabtwo-on");
            };
            $(".searchbox input[type='text']").keydown(function(event) {
                var e = event;
                if (e.keyCode == 13) {
                    $("#btnSearch").click();
                }
            });
        })
       
    </script>

    </form>
</asp:Content>
