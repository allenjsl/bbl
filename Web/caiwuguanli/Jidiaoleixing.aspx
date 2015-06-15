<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="Jidiaoleixing.aspx.cs" Inherits="Web.caiwuguanli.Jidiaoleixing" Title="计调支出应付账款" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/utilsUri.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">财务管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt;财务管理 &gt;&gt; 团款支出
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
            <li><a id="two1" href="TeamExpenditure.aspx">应付账款</a></li>
            <li><a id="two2" href="waitkuan.aspx" class="">付款审批</a></li>
            <li><a id="two3" href="teamPayClear.aspx" class="">已结清账款</a></li>
            <li><a id="two4" class="tabtwo-on" href="jidiaoleixing.aspx">批量登记</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div id="con_two_1" style="display: block;">
            <table width="99%" cellspacing="1" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    团队类型：</label>
                                <select id="select" name="select" runat="server">
                                    <option value="-1">请选择</option>
                                    <option value="0">散拼计划</option>
                                    <option value="1">团队计划</option>
                                </select>
                                <label>
                                    团号：</label>
                                <input type="text" id="txt_teamNum" class="searchinput" name="txt_teamNum" runat="server">
                                <label>
                                    供应商名称：</label>
                                <input type="text" class="searchinput searchinput02" id="txt_com" name="txt_com"
                                    runat="server">
                                <label>
                                    单位类别：</label>
                                <asp:DropDownList ID="ddl_comType" runat="server">
                                    <asp:ListItem Value="-1">请选择</asp:ListItem>
                                    <asp:ListItem Value="2">票务</asp:ListItem>
                                    <asp:ListItem Value="1">地接</asp:ListItem>
                                </asp:DropDownList>
                                <br>
                                <label>
                                    出团时间：</label>
                                <input type="text" id="txt_godate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'});"
                                    class="searchinput" name="txt_godate" runat="server">
                                至
                                <input type="text" id="txtRDate" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'});" class="searchinput"
                                    name="txtRDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("rdate") %>" />
                                <label>
                                    <asp:ImageButton ID="ImageButton1" Style="vertical-align: middle;" runat="server"
                                        ImageUrl="../images/searchbtn.gif" OnClick="ImageButton1_Click" />
                                </label>
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
            <div class="btnbox">
                <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
                    <tr>
                        <td td width="90" align="left">
                            <a href="javascript:void(0);" onclick="piliangdengji()">批量登记</a>
                        </td>
                    </tr>
                </table>
            </div>
            <%-- 序号（序号+复选框）、团号、出团时间、线路名称、支付项目、供应商名称、支出金额、已支付金额、未支付金额、已登记金额、未登记金额、操作（登记）--%>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
                <table width="100%" cellspacing="1" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <th bgcolor="#bddcf4" align="center" style="width: 5%">
                                序号<input type="checkbox" id="allchk" />
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 11%">
                                团号
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 9%; display: none">
                                线路名称
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 9%">
                                出团时间
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 9%">
                                支付项目
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 11%">
                                供应商名称
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 8%">
                                支出金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 10%">
                                已支付金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 10%">
                                未支付金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 10%">
                                已登记金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 10%">
                                未登记金额
                            </th>
                            <th bgcolor="#bddcf4" align="center" style="width: 7%">
                                操作
                            </th>
                        </tr>
                        <cc2:CustomRepeater runat="server" ID="rpt_list1">
                            <ItemTemplate>
                                <tr class="<%#(Container.ItemIndex+1)%2==0 ?"odd":"even" %>">
                                    <td align="center" valign="middle">
                                        <%#Container.ItemIndex + 1 + (EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("page")==""?"1":EyouSoft.Common.Utils.GetQueryStringValue("page"))-1) * 20%>
                                        <input type="checkbox" class="chk" data-anpaiid='<%#Eval("AnPaiId") %>' />
                                    </td>
                                    <td align="center">
                                        <input type="hidden" id="hd_tourType" name="hd_tourType" value="<%#Eval("TourType") %>" />
                                        <%#Eval("TourCode")%>
                                    </td>
                                    <td align="center" style="display: none">
                                        <%#Eval("RouteName")%>
                                    </td>
                                    <td align="center">
                                        <%# DateTime.Parse(Eval("CTTime").ToString()).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ZhiChuLeiBie")%>
                                    </td>
                                    <td align="center" data-class='gysname'>
                                        <%#Eval("GYSName")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("ZhiChuJinE", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("YiZhiFuJinE", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("WeiZhiFuJinE", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("YiDengJiJinE", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <%#Eval("WeiDengJiJinE", "{0:c2}")%>
                                    </td>
                                    <td align="center">
                                        <a href='dengji.aspx?id=<%#Eval("anpaiId") %>&com=<%#Server.UrlEncode(Eval("GYSName").ToString()) %>&type=<%#(int)Eval("ZhiChuLeiBie") %>&IsJidiao=1&tourid=<%#Eval("TourId") %>&comid=<%#Eval("GYSId") %>'
                                            class="dengji"><font class='fblur'>登记</font></a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </cc2:CustomRepeater>
                        <tr>
                            <td bgcolor="#bddcf4" align="left">
                                合计：
                            </td>
                            <td bgcolor="#bddcf4" align="left" colspan="4">
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <span style="display: block; font-weight: bold;" class="fred">
                                    <asp:Label ID="lbzhichumoney" runat="server" Text=""></asp:Label></span>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <span style="display: block; font-weight: bold;" class="fred">
                                    <asp:Label ID="lbyizhifumoney" runat="server" Text=""></asp:Label></span>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <span style="display: block; font-weight: bold;" class="fred">
                                    <asp:Label ID="lbweizhifumoney" runat="server" Text=""></asp:Label></span>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <span style="display: block; font-weight: bold;" class="fred">
                                    <asp:Label ID="lbyidengjimoney" runat="server" Text=""></asp:Label>
                                </span>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                                <span style="display: block; font-weight: bold;" class="fred">
                                    <asp:Label ID="lbweidengjimoney" runat="server" Text=""></asp:Label></span>
                            </td>
                            <td bgcolor="#bddcf4" align="center">
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="11">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {
            $(".dengji").click(function() {
                var url = $(this).attr("href");
                //var url = utilsUri.encode($(this).attr("href"));
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "登记",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });
            $("#<%=ImageButton1.ClientID %>").click(function() {

                var param = "tourCode=" + $("#<%=txt_teamNum.ClientID%>").val();
                param += "&tourtype=" + $("#<%=select.ClientID %>").val();
                param += "&companyName=" + encodeURIComponent($("#<%=txt_com.ClientID %>").val());
                param += "&comType=" + $("#<%=ddl_comType.ClientID %>").val();
                param += "&beginDate=" + $("#<%=txt_godate.ClientID %>").val();
                param += "&rdate=" + $("#txtRDate").val();
                location.href = "jidiaoleixing.aspx?" + param;
                return false;
            });
            $("#allchk").click(function() {

                $(".chk").each(function() {
                    $(this).attr("checked", $("#allchk").attr("checked"));
                })
            })
        });
        function piliangdengji() {
            var anpaiIds = "";
            $(".chk:checked").each(function() {
                anpaiIds += ($(this).attr("data-anpaiId") + ",");
            })
            if (anpaiIds.length > 0) {
                var iframeRequestParams = {anpaiIds: anpaiIds.substring(0, anpaiIds.length - 1) };
                var iframeUrl = utilsUri.createUri("/caiwuguanli/lotsdengji.aspx", iframeRequestParams);
                Boxy.iframeDialog({
                    iframeUrl: iframeUrl,
                    title: "批量登记",
                    data: {},
                    modal: true,
                    width: "920px",
                    height: "200px"
                });
            }
            else {
                alert("请选择登记条目！");
            }
            return false;
        }
    </script>

</asp:Content>
