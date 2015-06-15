<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HuiKuanLv.aspx.cs" Inherits="Web.StatisticAnalysis.HuiKuanLv.HuiKuanLv" MasterPageFile="~/masterpage/Back.Master" Title="回款率分析_统计分析" %>


<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/UCPrintButton.ascx" TagName="UCPrintButton" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/TourTypeList.ascx" TagName="TourTypeList" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/RouteAreaList.ascx" TagName="RouteAreaList" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
    <script src=" /js/utilsUri.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <style type="text/css">
    .huikuanlvtable{border:0px;width:100%;margin:0px auto; /*border-top:1px solid #efefef;border-left:1px solid #efefef;*/}
    .huikuanlvtable td{ /*width:20%;border-bottom:1px solid #efefef;border-right:1px solid #efefef;*/ height:27px; text-align:center;}
    .huikuanlvtable tr.head{font-weight:bold; /*background:#f6f6f6;*/}
    .huikuanlvtable td.money{text-align:right;}
    .huikuanlvtable td.money span{ padding-right:10px;}
    .huikuanvtablecurrent{font-weight:bold;}
    </style>
    
    <script type="text/javascript">
        //search
        function search() {
            var _departObj = eval("<%=txtQueryDepart.ClientID %>");
            var _operatorObj = eval("<%=txtQuerySeller.ClientID %>");
            var params = { "kehumingcheng": $.trim($("#txtKeHuMingCheng").val())
                , "lsdate": $("#txtLeaveSDate").val()
                , "ledate": $("#txtLeaveEDate").val()
                , "departids": _departObj.GetId()
                , "sellerids": _operatorObj.GetOperatorId()
                , 'departnames': _departObj.GetName()
                , 'sellernames': _operatorObj.GetOperatorName()
            };

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
        
        //to xls
        function toXls() {
            $("#txtXlsHTML").val($("#divXls").html());
            $("#<%=form1.ClientID %>").submit();            
            return false;
        }
        $(document).ready(function() {
            $("#btnSearch").bind("click", function() { search(); return false; });
            $("#huikuanlvtable tr:last").addClass("huikuanvtablecurrent");
        });
    </script>

</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    <form runat="server" id="form1">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析-回款率分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 回款率分析
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10"></div>
        <div>            
            <table cellpadding="0" cellspacing="0" style="width: 100%; margin-top: 10px; margin-bottom: 10px; border: 0px;">
                <tr>
                    <td>                        
                        <label>出团时间：</label>
                        <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput" name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" style="width: 100px" />-<input type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" style="width: 100px" />
                        <label>部门：</label>
                        <uc1:ucselectdepartment setpicture="/images/sanping_04.gif" id="txtQueryDepart" runat="server" style="width: 80px;" />
                        <uc2:selectOperator Title="销售员：" ID="txtQuerySeller" runat="server" Style="width: 80px;" /><br/>
                        <div style="height:5px;font-size:1px;">&nbsp;</div>
                        <label>客户单位：</label>
                        <input type="text" name="txtKeHuMingCheng" id="txtKeHuMingCheng" class="searchinput searchinput02" style="width: 150px" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kehumingcheng") %>" />
                        <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a> 
                        <a onclick="toXls();return false;" href="###"><img width="68" height="27" style="margin-top: 0px; border: 0px" alt="导出" src="/images/export.xls.png"></a>
                    </td>
                </tr>
            </table>
            <div class="btnbox" style="height:1px; margin:0px; padding:0px;"></div>
            <div class="tablelist">
                <input type="hidden" name="txtXlsHTML" id="txtXlsHTML" />
                <input type="hidden" name="istoxls" id="istoxls" value="1" />
                <!--回款率统计表开始-->                
                <div id="divXls">
                    <b style="line-height:25px; margin-top:10px; margin-bottom:10px"><asp:Literal runat="server" ID="ltrHuiKuanLvBiaoTi"></asp:Literal></b>
                    <%--<asp:Repeater runat="server" ID="rptHuiKuanLv">
                    <ItemTemplate>
                        应收款：<%#Eval("YingShouKuan") %><br/>
                        已收款：<%#Eval("YiShouKuan") %><br />
                        回款率：<%#Eval("HuiKuanLvBaiFenBi") %><br />
                        统计时间：<%#Eval("TongJiShiJian") %>
                        <br />
                        <br/>
                    </ItemTemplate>
                </asp:Repeater>--%>                    
                    <table cellpadding="0" cellspacing="1" class="huikuanlvtable" id="huikuanlvtable">
                        <tr class="head odd">
                            <td>统计时间</td>
                            <td class="money"><span>应收款</span></td>
                            <td class="money"><span>已收款</span></td>
                            <td class="money"><span>未收款</span></td>
                            <td>回款率</td>
                        </tr>
                        <asp:Repeater runat="server" ID="rptHuiKuanLv">
                            <ItemTemplate>
                                <tr <%#(Container.ItemIndex + 1)%2==0?"class='odd'":"class='even'" %>>
                                    <td><%#Eval("TongJiShiJian") %></td>
                                    <td class="money"><span><%#Eval("YingShouKuan","{0:C2}") %></span></td>
                                    <td class="money"><span><%#Eval("YiShouKuan", "{0:C2}")%></span></td>
                                    <td class="money"><span><%#Eval("WeiShouKuan", "{0:C2}")%></span></td>
                                    <td><%#Eval("HuiKuanLvBaiFenBi") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <!--回款率统计表结束-->
            </div>
        </div>
       
    </div>
    </form> 
</asp:Content>
