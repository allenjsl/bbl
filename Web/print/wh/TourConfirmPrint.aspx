<%@ Page Title="团队确认单" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="TourConfirmPrint.aspx.cs" Inherits="Web.print.wh.TourConfirmPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="759px">
        <tr>
            <td align="center" height="30" valign="top">
                <table align="center" class="table_normal" width="100%">
                    <tr>
                        <td align="center" class="normaltd" colspan="5" height="50" style="font-size: 20px;
                            font-weight: bold;" valign="middle">
                            <strong>关于
                                <asp:Label ID="lblTid" runat="server" Text=""></asp:Label>
                                团队散客 的确认单</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>接收人：</strong><asp:Label ID="lblSetMan" runat="server"></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="22%">
                            <strong>姓名：</strong><asp:Label ID="lblSetName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="21%">
                            <strong>传真：</strong><asp:Label ID="lblSetFAX" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top" width="21%">
                            <strong>电话：</strong><asp:Label ID="lblSetPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>发送人：</strong><asp:Label ID="lblGetMan" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>姓名：</strong><asp:Label ID="lblGetName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>传真：</strong><asp:Label ID="lblGetFAX" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="top">
                            <strong>电话：</strong><asp:Label ID="lblGetPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>根据贵社的订购要求，客人参加散拼团的行程及服务标准整理如下，请仔细核对相关项目及费用，如果无误，请盖章确认后，回传至我公司，谢谢您的支持！</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="3" style="padding-left: 10px;" valign="top">
                            <strong>线路名称：</strong><asp:Label ID="lblLineName" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>天数：</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label ID="lblDaySum"
                                runat="server" Text=""></asp:Label>天
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="3" style="padding-left: 10px;" valign="top">
                            <strong>出发交通：</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblGoTraffic" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="left" class="normaltd" colspan="2" style="padding-left: 10px;" valign="top">
                            <strong>返程交通：</strong><span class="Apple-converted-space">&nbsp;</span><asp:Label
                                ID="lblEndTraffic" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            以上航班号和航班时间仅供参考，如果有变动我社会提前告知，如无变动则不另行通知。
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>送团方式：</strong>集合时间<asp:Label ID="lblGatheringTime" runat="server" Text=""></asp:Label>集合地点<asp:Label
                                ID="lblGatheringPlace" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="normaltd" colspan="5" style="padding-left: 10px;" valign="top">
                            <strong>订购人数</strong>：<asp:Label ID="lblManSum" runat="server" Text=""></asp:Label><strong>(大人)</strong>
                        </td>
                    </tr>
                </table>
                <div class="Placeholder20">
                    &nbsp;</div>
                <table width="100%" class="table_normal2">
                    <asp:Repeater ID="xc_list" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="left" height="25" style="padding-left: 10px;" width="33%">
                                    <strong>第
                                        <%#Container.ItemIndex+1%>天</strong>&nbsp;&nbsp;&nbsp;
                                    <%#((DateTime)xcTime).AddDays((int)Container.ItemIndex).ToString("yyyy-MM-dd")%>
                                    <%# EyouSoft.Common.Utils.ConvertWeekDayToChinese(((DateTime)xcTime).AddDays((int)Container.ItemIndex))%></strong>
                                </td>
                                <td align="left" style="padding-left: 10px;" width="16%">
                                    <strong>
                                        <%#Eval("Interval")%></strong>
                                </td>
                                <td align="left" style="padding-left: 10px;" width="15%">
                                    <strong>
                                        <%#Eval("Vehicle")%></strong>
                                </td>
                                <td align="left" style="padding-left: 10px;" width="19%">
                                    <strong>住：<%#Eval("Hotel")%></strong>
                                </td>
                                <td align="left" style="padding-left: 10px;" width="17%">
                                    <strong>餐：<%#getEat(Eval("Dinner").ToString())%></strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="5" height="25" style="padding-left: 10px;">
                                    &nbsp;&nbsp;<%#Eval("Plan")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" height="30" valign="top">
                <div id="tabProject20" class="Placeholder20" runat="server">
                    &nbsp;</div>
                <table id="tabProject" runat="server" class="table_normal" width="100%">
                    <tr>
                        <td align="left" height="30" class="normaltd" colspan="2">
                            <strong>服务标准及说明：</strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="30" valign="top">
                            包含项目：
                        </td>
                        <td align="left" class="normaltd" height="25" style="padding-left: 10px;" width="89%">
                            <table width="100%" style="padding: 0px; margin: 0px">
                                <asp:Repeater ID="rpt_sList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <% li++; %>
                                            <td align="left" class='<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex(li, 1, sListRowsCount )%>'
                                                height="25" style="padding-left: 10px;">
                                                <%# (Container.ItemIndex+1)+"、"+Eval("ServiceType").ToString()+"："+Eval("Service").ToString()%>
                                                &nbsp;&nbsp;单价：￥&nbsp;<%#Convert.ToDecimal(Eval("SelfUnitPrice")).ToString("0.00")%>&nbsp;人数：￥&nbsp;<%#Eval("SelfPeopleNumber")%>
                                                &nbsp; 总计：￥&nbsp;
                                                <%#Convert.ToDecimal( Eval("SelfPrice")).ToString("0.00")%>&nbsp;元
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            结算价：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblTotalAmount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            不含项目：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblBuHanXiangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            自费项目：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblZiFeiXIangMu" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            儿童安排：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblErTongAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            购物安排：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblGouWuAnPai" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            赠送项目：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:TextBox ID="txtPresent" runat="server" Width="90%" BorderStyle="None"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="normaltd" height="25">
                            注意事项：
                        </td>
                        <td align="left" class="normaltd" style="padding-left: 10px;">
                            <asp:Label ID="lblZhuYiShiXiang" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="tabxcquick" class="table_normal2" width="100%" runat="server">
                    <tr>
                        <td align="left">
                            <strong>行程安排：</strong>
                            <br />
                            &nbsp;&nbsp;<asp:Label ID="lblQuickPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>服务标准及说明：</strong><br />
                            &nbsp;&nbsp;<asp:Label ID="lblKs" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table class="table_normal2" width="100%" style="margin-top: 10px;" cellpadding="0"
                    cellspacing="0">
                    <tr style="border: 1px solid #000">
                        <td align="left" class="normaltd" style="padding-left: 10px;" valign="middle" width="11%"
                            rowspan="<%=visitorRowsCount+1 %>">
                            <strong>客人信息：</strong>
                        </td>
                        <asp:Repeater ID="rptVisitorList" runat="server">
                            <ItemTemplate>
                                <tr style="border: 1px solid #000">
                                    <td align="left" class="normaltd" valign="top" width="15px">
                                        <% mi++; %>
                                        <%#Container.ItemIndex+1%>：
                                    </td>
                                    <td width="70px" align="center">
                                        <%#Eval("VisitorName")%>
                                    </td>
                                    <td width="40px" align="center">
                                        <%#Eval("VisitorType").ToString()%>
                                    </td>
                                    <td>
                                        <%#Eval("CradNumber")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </table>
            </td>
            <%-- <div class="Placeholder20">
                &nbsp;</div>--%>
        </tr>
    </table>
    <div class="Placeholder20">
        &nbsp;</div>
    <table class="table_normal2" width="100%">
        <tr>
            <td align="left" class="normaltd" colspan="3" height="25">
                <strong>如果您确认无误,请将款打入我公司以下帐号:</strong>
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                公司全称：浙江邮电旅行社有限公司
            </td>
            <td align="left" height="25" style="padding-left: 10px;" width="33%">
                开户行：交通银行杭州城北支行
            </td>
            <td align="left" style="padding-left: 10px;" width="30%">
                帐号：331066100018010042682
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                户名：张奔
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                开户行：建设银行 龙卡
            </td>
            <td align="left" style="padding-left: 10px;">
                帐号：6227 0015 4180 0293 354
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                户名：张奔
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                开户行：工商银行 牡丹卡
            </td>
            <td align="left" style="padding-left: 10px;">
                帐号：622202 1202024799174
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                户名：张奔
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                开户行：农业银行 金穗卡
            </td>
            <td align="left" style="padding-left: 10px;">
                帐号：62284 8032 09784 20414
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                户名：张奔
            </td>
            <td align="left" height="25" style="padding-left: 10px;">
                开户行：建设银行 （宁波）
            </td>
            <td align="left" style="padding-left: 10px;">
                帐号：6227 0015 9550 0281 598
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0 0 0;">
        &nbsp;&nbsp;</div>
    <table class="table_normal2" width="100%">
        <tr>
            <td align="left" style="padding-left: 10px;" width="37%">
                组团社单位盖章：
                <asp:Label ID="lblSetMan2" runat="server" Text=""></asp:Label><br />
                联系电话：
                <asp:Label ID="lblSetPhone2" runat="server" Text=""></asp:Label><span class="Apple-converted-space">&nbsp;</span><br />
                确认日期：
                <%=DateTime.Now.ToString("yyyy-MM-dd") %>
            </td>
            <td align="left" height="25" style="padding-left: 10px;" width="33%">
                专线商单位盖章：
                <asp:Label ID="lblGetMan2" runat="server" Text=""></asp:Label><br />
                联系电话：
                <asp:Label ID="lblGetPhone2" runat="server" Text=""></asp:Label><span class="Apple-converted-space">&nbsp;</span><br />
                确认日期：
                <%=DateTime.Now.ToString("yyyy-MM-dd") %>
            </td>
        </tr>
    </table>
    </td> </tr> </table>

    <script type="text/javascript">
        $(function() {
            $(".tbl").each(function() {
                var height = $(this).parent("td").parent("tr");
                $(this).height(height.height() + 10);
            })
            var priceAll = 0;
            $("#tblProject").find("input[name='txtPrice']").each(function() {
                var price = parseFloat($(this).val());
                if (price.toString() != "NaN" && price > 0) {
                    priceAll = priceAll + price;
                }
            });
            priceAll = parseInt(priceAll * 100) / 100;
            $("#txt_allPrice").val(priceAll);
        })
         
    </script>

</asp:Content>
