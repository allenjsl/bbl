<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamSettle.aspx.cs" Inherits="Web.TeamPlan.TeamSettle"
    MasterPageFile="~/masterpage/Back.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server">
    <div class="mainbody">
        <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">
                                <asp:Label ID="lblLocation" runat="server" Text=""></asp:Label></span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            ����λ��&gt;&gt;
                            <asp:Label ID="lblLocationT" runat="server" Text=""></asp:Label>
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
        <div class="btnbox">
            <asp:Panel ID="pnlPlan" runat="server">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td align="center" width="90">
                                <a id="a_print" href="<%=hidePrintUrl.Value %>?tourId=<%=hidePlanID.Value %>" target="_blank">��ӡ���㵥</a>
                            </td>
                            <td align="center" width="90">
                                <asp:Panel ID="pnlTiJiao" runat="server">
                                    <a href="javascript:void(0);" id="a_TiJiao">�ύ����</a></asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlAccount" runat="server">
                <table cellspacing="0" cellpadding="0" border="0" align="left">
                    <tbody>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlFuHe" runat="server">
                                    <a href="javascript:void(0);" id="a_FuHe">�ŶӸ���</a></asp:Panel>
                            </td>
                            <td align="center" style="padding-left: 5px;">
                                <asp:Panel ID="pnlJieSu" runat="server">
                                    <a href="javascript:void(0);" id="a_JieSu">�������</a></asp:Panel>
                            </td>
                            <td align="center" style="padding-left: 5px;">
                                <asp:Panel ID="pnlJiDiao" runat="server">
                                    <a href="javascript:void(0);" id="a_JiDiao">�˻ؼƵ�</a></asp:Panel>
                            </td>                           
                            <td align="center" style="padding-left: 5px;">
                              <asp:Panel ID="pnlTuiHuiCaiWu" runat="server">
                                <a href="javascript:void(0);" id="a_CaoZhuo">�˻ز���</a></asp:Panel>
                            </td>         
                             <td align="center" style="padding-left: 5px;">
                                  <a href="/caiwuguanli/TeamAccount.aspx?isAccount=<%=Request.QueryString["Account"] %>">����</a>
                            </td>                    
                            <td align="center" style="padding-left: 5px;">
                                <asp:Panel ID="pnlHeSuanDan" runat="server" Visible="false">
                                    <a href="javascript:void(0);" id="a_HeSuanDan">���㵥</a></asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </div>
        <div class="tablelist">
            <table cellspacing="1" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr class="odd">
                        <td align="center" width="10%">
                            <b>����</b>
                        </td>
                        <th height="40" align="center" width="15%">
                            ������
                        </th>
                        <th align="center" width="15%">
                            �ͻ���λ
                        </th>
                        <th align="center" width="10%">
                            ������
                        </th>
                        <th align="center" width="10%">
                            ���ӷ���
                        </th>
                        <th align="center" width="10%">
                            ���ٷ���
                        </th>
                        <th align="center" width="10%">
                            С��
                            <div style="height: 2px; width: 50px">
                            </div>
                        </th>
                        <th align="center" width="20%" colspan="2">
                            ��ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptListFrist" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center" width="10%">
                                    <input type="hidden" name="hideFrist" value="<%#Eval("OrderId") %>" />
                                </td>
                                <td height="40" align="center" width="15%">
                                    <%#Eval("OrderNo") %>
                                </td>
                                <td align="center" width="15%">
                                    <%#Eval("CompanyName")%>
                                </td>
                                <td align="center" width="10%">
                                    <input type="hidden" name="hideFristIncome_<%#Eval("OrderId") %>" value="<%#Eval("SumPrice")%>" />
                                    ��<span id="lblIncome_<%#Container.ItemIndex+1 %>"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("SumPrice")).ToString("0.00"))%></span>
                                </td>
                                <td align="center" width="10%">
                                    <input type="text" id="txtIncomeAdd_<%#Container.ItemIndex+1 %>" name="txtIncomeAdd_<%#Eval("OrderId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceAddExpense")).ToString("0.00")) %>"
                                        class="searchinput" rel="1" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center" width="10%">
                                    <input type="text" id="txtIncomeReduc_<%#Container.ItemIndex+1 %>" name="txtIncomeReduc_<%#Eval("OrderId") %>"
                                        value=" <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceRedExpense")).ToString("0.00")) %>"
                                        class="searchinput" rel="1" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center" width="10%">
                                    ��<span class="incomeAll" id="lblIncomeAll_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceSum")).ToString("0.00"))%></span>
                                </td>
                                
                                <th align="center" width="20%"   colspan="2">
                                    <input type="text" id="txtIncomeRemarks_<%#Container.ItemIndex+1 %>" name="txtIncomeRemarks_<%#Eval("OrderId") %>"
                                        class="searchinput searchinput02" value="<%#Eval("FinanceRemark")%>" style="width: 185px;" />
                                </th>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" style="text-align:right"><b>����ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_shouru">��0.00</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <td align="center">
                            <b>��������</b>
                            <asp:Panel ID="pnlSecond" runat="server">
                                <a id="add_Second" class="addbtnsytle" href="javascript:void(0);">�������</a>
                            </asp:Panel>
                        </td>
                        <th height="40" align="center">
                            ������Ŀ
                        </th>
                        <th align="center">
                            �ͻ���λ
                        </th>
                        <th align="center">
                            ������
                        </th>
                        <th align="center">
                            ���ӷ���
                        </th>
                        <th align="center">
                            ���ٷ���
                        </th>
                        <th align="center">
                            С��
                        </th>
                        <th align="center" colspan="3">
                            ��ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptListSecond" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center">
                                    <input type="hidden" name="hideSecond" value="<%#Eval("IncomeId") %>" />
                                </td>
                                <td height="40" align="center">
                                    <span id="lblIncomePro_<%#Container.ItemIndex+1 %>">
                                        <%#Eval("Item")%></span>
                                </td>
                                <td>
                                    <%#Eval("CustromCName")%>
                                </td>
                                <td align="center">
                                    <input type="hidden" name="hideOtherIncome_<%#Eval("IncomeId") %>" value="<%#Eval("Amount")%>" />
                                    <span id="lblOtherIncome_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Amount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtOtherAdd_<%#Container.ItemIndex+1 %>" name="txtOtherAdd_<%#Eval("IncomeId") %>"
                                        value="<%#Convert.ToDecimal(Eval("AddAmount")).ToString("0.00")%>" class="searchinput"
                                        rel="2" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="text" id="txtOtherReduc_<%#Container.ItemIndex+1 %>" name="txtOtherReduc_<%#Eval("IncomeId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="2" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    �� <span class="otherIncome" id="lblOtherAll_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00")) %></span>
                                </td>
                                <td align="center" colspan="3">
                                    <input type="text" id="txtOtherRemarks_<%#Container.ItemIndex+1 %>" name="txtOtherRemarks_<%#Eval("IncomeId") %>"
                                        value="<%#Eval("Remark") %>" class="searchinput searchinput02" style="width: 185px;" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" style="text-align:right"><b>��������ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_qitashouru">0.00</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <td align="center">
                            <b>֧��</b>
                        </td>
                        <th height="40" align="center">
                            ֧����Ŀ
                        </th>
                        <th align="center">
                            ��λ����
                        </th>
                        <th align="center">
                            ֧�����
                        </th>
                        <th align="center">
                            ���ӷ���
                        </th>
                        <th align="center">
                            ���ٷ���
                        </th>
                        <th align="center">
                            С��
                        </th>
                        <th align="center" width="50px">
                            ����</th>
                        <th align="center">
                            ��ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptListThird" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center">
                                 
                                    <input type="hidden" name="hideThird" value="<%#Eval("Id") %>" />
                                    <input type="hidden" name="hideSupplierType_<%#Eval("Id") %>" value="<%#Eval("SupplierType") %>" />
                                </td>
                                <td height="40" align="center">
                                    <%#Eval("SupplierType") %>
                                </td>
                                <td align="center">
                                    <%#Eval("SuplierName") %>
                                </td>
                                <td align="center">
                                    <input type="hidden" value="<%#Convert.ToDecimal(Eval("PayAmount")).ToString("0.00")%>"
                                        name="hideExpendIncome_<%#Eval("Id") %>" />
                                    <span id="lblExpendIncome_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("PayAmount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendAdd_<%#Container.ItemIndex+1 %>" name="txtExpendAdd_<%#Eval("Id") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="3" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendLess_<%#Container.ItemIndex+1 %>" name="txtExpendLess_<%#Eval("Id") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="3" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="hidden" value="" id="hideExpendAll_<%#Container.ItemIndex+1 %>" name="hideExpendAll_<%#Eval("Id") %>" />
                                    �� <span class="outAll" id="lblExpendAll_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center">
                                <a href="javascript:void(0);" ref="/TeamPlan/Payment.aspx?id=<%#Eval("Id") %>&tourid=<%=Request.QueryString["id"] %>&type=<%#getType((int)Eval("SupplierType")) %>&money=<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %>&com=<%#Uri.EscapeUriString( Eval("SuplierName").ToString())%>&tourType=<%=hideType.Value %>" class="payment" >����Ǽ�</a>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendRemarks_<%#Container.ItemIndex+1 %>" name="txtExpendRemarks_<%#Eval("Id") %>"
                                        value="<%#Eval("FRemark") %>" class="searchinput searchinput02" style="width: 150px;" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" style="text-align:right"><b>֧���ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_zhichu">0.00</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="rptListThird_S" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center">
                                    <input type="hidden" name="hideThird-S" value="<%#Eval("PlanId") %>" />
                                    <input type="hidden" name="hideSupplierType-S_<%#Eval("PlanId") %>" value="<%#Eval("ServiceType") %>" />
                                </td>
                                <td height="40" align="center">
                                    <%#Eval("ServiceType")%>
                                </td>
                                <td align="center">
                                    <%#Eval("SupplierName")%>
                                </td>
                                <td align="center">
                                    <input type="hidden" value="<%#Convert.ToDecimal(Eval("Amount")).ToString("0.00")%>"
                                        name="hideExpendIncome-S_<%#Eval("PlanId") %>" />
                                    <span id="lblExpendIncome-S_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Amount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendAdd-S_<%#Container.ItemIndex+1 %>" name="txtExpendAdd-S_<%#Eval("PlanId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="3_2" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendLess-S_<%#Container.ItemIndex+1 %>" name="txtExpendLess-S_<%#Eval("PlanId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="3_2" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="hidden" value="" id="hideExpendAll-S_<%#Container.ItemIndex+1 %>" name="hideExpendAll-S_<%#Eval("PlanId") %>" />
                                    �� <span class="outAll" id="lblExpendAll-S_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00"))%></span>
                                </td>
                                  <td align="center">
                                 <a href="javascript:void(0);" ref="/TeamPlan/Payment.aspx?id=<%#Eval("planId") %>&tourid=<%=Request.QueryString["id"] %>&type=<%#getType((int)Eval("ServiceType")) %>&money=<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("TotalAmount").ToString()) %>&com=<%#Uri.EscapeUriString( Eval("SupplierName").ToString())%>&tourType=<%=hideType.Value %>" class="payment" >����Ǽ�</a>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtExpendRemarks-S_<%#Container.ItemIndex+1 %>" name="txtExpendRemarks-S_<%#Eval("PlanId") %>"
                                        value="<%#Eval("FRemark") %>" class="searchinput searchinput02" style="width: 150px;" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" style="text-align:right"><b>֧���ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_danxingzhichu">0.00</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <td align="center">
                            <b>����֧��</b>
                            <asp:Panel ID="pnlFouth" runat="server">
                                <a class="addbtnsytle" href="javascript:void(0);" id="add_Fouth">���֧��</a></asp:Panel>
                        </td>
                        <th height="40" align="center">
                            ֧����Ŀ
                        </th>
                        <th align="center">
                            �ͻ���λ
                        </th>
                        <th align="center">
                            ֧�����
                        </th>
                        <th align="center">
                            ���ӷ���
                        </th>
                        <th align="center">
                            ���ٷ���
                        </th>
                        <th align="center">
                            С��
                        </th>
                        <th align="center" colspan="3">
                            ��ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptListForth" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center">
                                    <input type="hidden" name="hideForth" value="<%#Eval("OutId") %>" />
                                </td>
                                <td height="40" align="center">
                                    <span id="lblOut">
                                        <%#Eval("Item")%></span>
                                </td>
                                <td style="text-align:center"><%#Eval("CustromCName")%></td>
                                <td align="center">
                                    <input type="hidden" name="hideOutIncome_<%#Eval("OutId") %>" value="<%#Convert.ToDecimal(Eval("Amount")).ToString("0.00")%>" />
                                    <span id="lblOutIncome_<%#Container.ItemIndex+1 %>">
                                        <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("Amount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center">
                                    <input type="text" id="txtOutAdd_<%#Container.ItemIndex+1 %>" name="txtOutAdd_<%#Eval("OutId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00")) %>"
                                        class="searchinput" rel="4" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    <input type="text" id="txtOutLess_<%#Container.ItemIndex+1 %>" name="txtOutLess_<%#Eval("OutId") %>"
                                        value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%>"
                                        class="searchinput" rel="4" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" />
                                </td>
                                <td align="center">
                                    ��<span class="otherOut" id="lblOutAll_<%#Container.ItemIndex+1 %>"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00"))%></span>
                                </td>
                                <td align="center" colspan="3">
                                    <input type="text" id="txtOutRemarks_<%#Container.ItemIndex+1 %>" name="txtOutRemarks_<%#Eval("OutId") %>"
                                        value="<%#Eval("Remark") %>" class="searchinput searchinput02" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" style="text-align:right"><b>����֧���ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_qitazhichu">0.00</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                        <td align="center" width="15%">
                            <b>�������</b>
                            <asp:Panel ID="pnlFifth" runat="server">
                                <a class="addbtnsytle" href="javascript:void(0);" id="add_Fifth">��ӷ���</a>
                            </asp:Panel>
                        </td>
                        <th height="40" align="center" width="25%">
                            ������Ŀ
                        </th>
                        <th align="center" width="10%">
                            ������
                        </th>
                        <th align="center" width="15%">
                            ����
                        </th>
                        <th align="center" width="15%">
                            ��Ա
                        </th>
                        <th align="center" colspan="4">
                            ��ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptListFifth" runat="server">
                        <ItemTemplate>
                            <tr class="even">
                                <td align="center">
                                    <input type="hidden" name="hideFifth" value="<%#Eval("ShareId") %>" />
                                </td>
                                <td height="40" align="center">
                                    <span id="lblProfit">
                                        <%#Eval("ShareItem")%></span>
                                </td>
                                <td align="center">
                                    <input type="text" errmsg="��ʽ����ȷ!" valid="IsDecimalTwo" value="<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ShareCost")).ToString("0.00"))%>" class="searchinput fengpei"
                                        name="txtShareCost_<%#Eval("ShareId") %>" />
                                </td>
                                <td align="center">
                                    <span id="lblDivision">
                                        <%#Eval("DepartmentName")%></span>
                                </td>
                                <td align="center">
                                    <span id="lblUserName">
                                        <%#Eval("Saler")%></span>
                                </td>
                                <td align="center" colspan="4">
                                    <input type="text" class="searchinput searchinput02" id="txtProfitRemarks_5" name="txtProfitRemarks_<%#Eval("ShareId") %>"
                                        value="<%#Eval("Remark") %>" style="width: 185px;">
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="2" style="text-align:right"><b>�������ϼƣ�</b></td>
                                <td style="text-align:center" id="heji_td_lirunfenpei">0.00</td>
                                <td colspan="5">&nbsp;</td>
                            </tr>
                            <tr><td colspan="8" style="height:10px;font-size:1px;line-height:10px">&nbsp;</td></tr>
                        </FooterTemplate>
                    </asp:Repeater>
                    <tr class="odd">
                    <th align="center">ë��</th><th align="center">ë����</th><th></th><th></th><td>&nbsp;</td><td colspan="4">&nbsp;</td>
                    </tr>
                    <tr class="even"><th align="center"><span id="ml"></span></th><th align="center" ><span id="mll"></span></th><th></th><th></th><td>&nbsp;</td><td colspan="4">&nbsp;</td>
                    </tr>
                    <tr>
                        <td height="40" align="center" colspan="9">
                            <asp:Panel ID="pnlSave" runat="server">
                                <table cellspacing="0" cellpadding="0" border="0" style="width: 391px">
                                    <tbody>
                                        <tr>
                                            <td height="40" align="center">
                                                <asp:Button ID="btnSave" runat="server" Text="" OnClientClick="return ValiDatorForm.validator(form, 'alert');"
                                                    OnClick="btnSave_Click" Width="1" Height="1" BackColor="Transparent" BorderColor="Transparent" />
                                            </td>
                                            <td height="40" align="center" class="tjbtn02">
                                                <a href="javascript:void(0);" id="a_Save">����</a>
                                            </td>
                                            <td height="40" align="center" class="tjbtn02">
                                                <a href="javascript:window.history.go(-1);">ȡ��</a>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <input type="hidden" value="" id="hidePlanID" runat="server" />
    <input type="hidden" value="" id="hideType" runat="server" />
    <input type="hidden" value="" id="hidePrintUrl" runat="server" />

    <script type="text/javascript">
        function formatFloat(src, pos) {
            return Math.round(src * Math.pow(10, pos)) / Math.pow(10, pos);
        }

        function isnull(v, defaultValue) {
            if (v == null || !v)
                return defaultValue;
            else
                return v;
        }
        $(function() {
            function account() {
                var incomeAll = 0, otherIncome = 0, outAll = 0, otherOut = 0, fengpei = 0;
                $(".incomeAll").each(function() {
                    incomeAll += parseFloat($(this).html());
                });
                $(".otherIncome").each(function() {
                    otherIncome += parseFloat($(this).html());
                });
                $(".outAll").each(function() {
                    outAll += parseFloat($(this).html());
                });
                $(".otherOut").each(function() {
                    otherOut += parseFloat($(this).html());
                });
                $(".fengpei").each(function() {
                    fengpei += parseFloat($(this).val());
                });
                var ml = incomeAll + otherIncome - outAll - otherOut - fengpei;
                var mll = parseFloat(ml) / isnull(parseFloat(incomeAll + otherIncome), 1) * 100;
                mll = ml < 0 ? 0 : mll;
                $("#ml").html("��" + formatFloat(ml, 2));
                $("#mll").html(formatFloat(mll, 2) + "%");

                //������֧����С�Ƶĺϼ���Ϣ
                $("#heji_td_shouru").html("��" + formatFloat(incomeAll, 2));
                $("#heji_td_qitashouru").html("��" + formatFloat(otherIncome, 2));
                $("#heji_td_zhichu").html("��" + formatFloat(outAll, 2));
                $("#heji_td_danxingzhichu").html("��" + formatFloat(outAll, 2));
                $("#heji_td_qitazhichu").html("��" + formatFloat(otherOut, 2));
                $("#heji_td_lirunfenpei").html("��" + formatFloat(fengpei, 2));

                //heJi.init();
            }
            account();

            //����֤
            var form = $(".tablelist").closest("form").get(0);

            FV_onBlur.initValid(form);

            $(".searchinput").keyup(function() {
                var id = $(this).attr("id").split("_")[1];
                var rel = $(this).attr("rel");
                var price = 0;
                var add = 0;
                var less = 0;
                switch (rel) {
                    case "1":
                        if ($.trim($("#txtIncomeAdd_" + id).val()) != "" && parseFloat($("#txtIncomeAdd_" + id).val()).toString() != "NaN") { add = parseFloat($("#txtIncomeAdd_" + id).val()); }
                        if ($.trim($("#txtIncomeReduc_" + id).val()) != "" && parseFloat($("#txtIncomeReduc_" + id).val()).toString() != "NaN") { less = parseFloat($("#txtIncomeReduc_" + id).val()); }

                        price = parseInt((parseFloat($("#lblIncome_" + id).html()) + add - less) * 100) / 100;
                        $("#hideIncomeAll_" + id).val(price);
                        $("#lblIncomeAll_" + id).html(price);
                        break;
                    case "2":
                        if ($.trim($("#txtOtherAdd_" + id).val()) != "" && parseFloat($("#txtOtherAdd_" + id).val()).toString() != "NaN") { add = parseFloat($("#txtOtherAdd_" + id).val()); }
                        if ($.trim($("#txtOtherReduc_" + id).val()) != "" && parseFloat($("#txtOtherReduc_" + id).val()).toString() != "NaN") { less = parseFloat($("#txtOtherReduc_" + id).val()) };
                        price = parseInt((parseFloat($("#lblOtherIncome_" + id).html()) + add - less) * 100) / 100;
                        $("#hideOtherAll_" + id).val(price);
                        $("#lblOtherAll_" + id).html(price);
                        break;
                    case "3":
                        if ($.trim($("#txtExpendAdd_" + id).val()) != "" && parseFloat($("#txtExpendAdd_" + id).val()).toString() != "NaN") { add = parseFloat($("#txtExpendAdd_" + id).val()); }
                        if ($.trim($("#txtExpendLess_" + id).val()) != "" && parseFloat($("#txtExpendLess_" + id).val()).toString() != "NaN") { less = parseFloat($("#txtExpendLess_" + id).val()) };
                        price = parseInt((parseFloat($("#lblExpendIncome_" + id).html()) + add - less) * 100) / 100;
                        $("#hideExpendAll_" + id).val(price);
                        $("#lblExpendAll_" + id).html(price);
                        break;
                    case "3_2":

                        if ($.trim($("#txtExpendAdd-S_" + id).val()) != "" && parseFloat($("#txtExpendAdd-S_" + id).val()).toString() != "NaN") { add = parseFloat($("#txtExpendAdd-S_" + id).val()); }
                        if ($.trim($("#txtExpendLess-S_" + id).val()) != "" && parseFloat($("#txtExpendLess-S_" + id).val()).toString() != "NaN") { less = parseFloat($("#txtExpendLess-S_" + id).val()) };
                        price = parseInt((parseFloat($("#lblExpendIncome-S_" + id).html()) + add - less) * 100) / 100;
                        $("#hideExpendAll-S_" + id).val(price);
                        $("#lblExpendAll-S_" + id).html(price);
                        break;
                    default:
                        if ($.trim($("#txtOutAdd_" + id).val()) != "" && parseFloat($("#txtOutAdd_" + id).val()).toString() != "NaN") { add = parseFloat($("#txtOutAdd_" + id).val()); }
                        if ($.trim($("#txtOutLess_" + id).val()) != "" && parseFloat($("#txtOutLess_" + id).val()).toString() != "NaN") { less = parseFloat($("#txtOutLess_" + id).val()) };
                        price = parseInt((parseFloat($("#lblOutIncome_" + id).html()) + add - less) * 100) / 100;
                        $("#hideOutAll_" + id).val(price);
                        $("#lblOutAll_" + id).html(price);
                        break;
                }
                account();
            });

            $("#a_Save").click(function() {
                var b = ValiDatorForm.validator(form, "alert");
                if (b) {
                    $("#<%=btnSave.ClientID %>").click();
                }
                return false;
            });

            $("#add_Second").click(function() {
                var planId = $("#<%=hidePlanID.ClientID %>").val();
                if (planId != "") {
                    Boxy.iframeDialog({ title: "��������", iframeUrl: "/sanping/Tianjiasr.aspx?tourid=" + planId + "&type=<%=hideType.Value %>", width: "510px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            });

            $("#add_Fouth").click(function() {
                var planId = $("#<%=hidePlanID.ClientID %>").val();
                if (planId != "") {
                    Boxy.iframeDialog({ title: "����֧��", iframeUrl: "/sanping/TianjiaZhichu.aspx?tourid=" + planId + "&type=<%=hideType.Value %>", width: "510px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            });

            $("#add_Fifth").click(function() {
                var planId = $("#<%=hidePlanID.ClientID %>").val();
                if (planId != "") {
                    Boxy.iframeDialog({ title: "����֧��", iframeUrl: "/sanping/TianjiaFengPei.aspx?tourid=" + planId + "&type=<%=hideType.Value %>", width: "510px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            });

            //�ύ����
            $("#a_TiJiao").click(function() {
                UpdateState("TiJiao");
            });
            // �ŶӸ���  a_FuHe
            $("#a_FuHe").click(function() {
                UpdateState("FuHe");
            });
            // ������� a_JieSu
            $("#a_JieSu").click(function() {
                UpdateState("JieSu");
            });
            //�˻ؼƵ� a_JiDiao
            $("#a_JiDiao").click(function() {
                UpdateState("JiDiao");
            });
            //�˻ز���
            $("#a_CaoZhuo").click(function() {
                UpdateState("TuiHuiCaiWu");
                //window.location.href = "/caiwuguanli/TeamAccount.aspx?isAccount=<%=Request.QueryString["Account"] %>";
                //return false;
            });
            //֧�� ��������
            $(".payment").click(function() {
                var href = $(this).attr("ref");
                if (href != "") {
                    Boxy.iframeDialog({ title: "����Ǽ�", iframeUrl: href, width: "810px", height: "350px", draggable: true, data: null, hideFade: true, modal: true });
                }
                return false;
            })
        })

        function UpdateState(type) {
            var tourId = '<%=Request.QueryString["id"] %>';
            $.newAjax({
                type: "Get",
                url: "/TeamPlan/AjaxTeamSettle.ashx?type=" + type + "&tourId=" + tourId,
                cache: false,
                success: function(result) {
                    switch (result) {
                        case "1":
                            alert("�����ɹ�!");
                            if (type == "JieSu") {
                                $("#<%=pnlFuHe.ClientID %>").remove();
                                $("#<%=pnlJieSu.ClientID %>").remove();
                                $("#<%=pnlJiDiao.ClientID %>").remove();
                                $("#<%=pnlSave.ClientID %>").remove();
                            }
                            break;
                        case "-1":
                            alert("����ʧ��!ԭ��:�Ŷӳɱ�δȷ��!")
                            break;
                        case "-2":
                            alert("����ʧ��!ԭ��:δ����!")
                            break;
                        default:
                            alert("����ʧ��!")
                            break;
                    }
                }
            });
        }
    </script>
    
    <script type="text/javascript">
        //������֧����С�ƺϼ�
        /*var heJi = {
            shouru: function() {
                var _sum = 0;
                $(".incomeAll").each(function() {
                    _sum += parseFloat($(this).html());
                });
                $("#heji_td_shouru").html("�� " + formatFloat(_sum, 2));
            },
            qitashouru: function() {
                var _sum = 0;
                $(".otherIncome").each(function() {
                    _sum += parseFloat($(this).html());
                });
                $("#heji_td_qitashouru").html("�� " + formatFloat(_sum, 2));
            },
            zhichu: function() {
                var _sum = 0;
                $(".outAll").each(function() {
                    _sum += parseFloat($(this).html());
                });
                $("#heji_td_zhichu").html("�� " + formatFloat(_sum, 2));
                $("#heji_td_danxingzhichu").html("�� " + formatFloat(_sum, 2));
            },
            qitazhichu: function() {
                var _sum = 0;
                $(".otherOut").each(function() {
                    _sum += parseFloat($(this).html());
                });
                $("#heji_td_qitazhichu").html("�� " + formatFloat(_sum, 2));
            },
            lirunfenpei: function() {
                var _sum = 0;
                $(".fengpei").each(function() {
                    _sum += parseFloat($(this).val());
                });
                $("#heji_td_lirunfenpei").html("�� " + formatFloat(_sum, 2));
            },
            init: function() {
                this.shouru();
                this.qitashouru();
                this.zhichu();
                this.qitazhichu();
                this.lirunfenpei();
            }
        };*/

        //$(document).ready(function() { heJi.init(); });
    </script>
    

    </form>
</asp:Content>
