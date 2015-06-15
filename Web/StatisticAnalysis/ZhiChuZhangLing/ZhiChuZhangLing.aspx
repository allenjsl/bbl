<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZhiChuZhangLing.aspx.cs" Inherits="Web.StatisticAnalysis.ZhiChuZhangLing.ZhiChuZhangLing" MasterPageFile="~/masterpage/Back.Master" Title="支出账龄分析_统计分析" %>

<%@ Register Src="~/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<asp:content id="head1" runat="server" contentplaceholderid="head">
    <script src=" /js/utilsUri.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    
    <style type="text/css">
    .fx_tbl{border:0px;width:100%;margin:0px auto; /*border-top:1px solid #efefef;border-left:1px solid #efefef;*/}
    .fx_tbl td{ /*width:20%;border-bottom:1px solid #efefef;border-right:1px solid #efefef;*/ height:27px; text-align:center;}
    .fx_tbl tr.head{font-weight:bold; /*background:#f6f6f6;*/}
    .fx_tbl tr.jg{ background:#f6f6f6;}
    .fx_tbl td.money{text-align:right;}
    .fx_tbl td.money span{ padding-right:10px;}
    .fx_tbl td.left1 {text-align:left;}
    .fx_tbl td.left1 span {padding-left:10px}
    </style>
    
    <script type="text/javascript">
        //search
        function search() {
            var _departObj = eval("<%=txtQueryDepart.ClientID %>");
            var _operatorObj = eval("<%=txtQueryOperator.ClientID %>");
            var params = { "gongyingshang": $.trim($("#txtGongYingShang").val())
                , "lsdate": $("#txtLeaveSDate").val()
                , "ledate": $("#txtLeaveEDate").val()
                , "departids": _departObj.GetId()
                , "operatorids": _operatorObj.GetOperatorId()
                , "gongyingshangleixing": $("#txtGongYingShangLeiXing").val()
            };

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }

        //order by
        function order(_v) {
            var params = utilsUri.getUrlParams(["page"]);

            if (params["sorttype"] == _v) { alert("当前数据已经按此规则排序"); return false };

            params["sorttype"] = _v;

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
        
        //to xls
        function toXls() {
            var params = utilsUri.getUrlParams([]);
            params["recordcount"] = recordCount;
            params["istoxls"] = 1;

            if (params["recordcount"] < 1) { alert("暂时没有任何数据供导出"); return false; }

            window.location.href = utilsUri.createUri(null, params);
            return false;
        }
        $(document).ready(function() {
            $("#btnSearch").bind("click", function() { search(); return false; });
            $("#huikuanlvtable tr:last").addClass("huikuanvtablecurrent");
            $("#txtGongYingShangLeiXing").val('<%=EyouSoft.Common.Utils.GetQueryStringValue("gongyingshangleixing") %>');
        });
    </script>

</asp:content>
<asp:content id="Content1" runat="server" contentplaceholderid="c1">
    <form runat="server" id="form1">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">统计分析-支出账龄分析</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>统计分析 >> 支出账龄分析
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
                        <uc2:selectOperator Title="计划操作员：" ID="txtQueryOperator" runat="server" Style="width: 80px;" /><br/>
                        <div style="height:5px;font-size:1px;">&nbsp;</div>
                        <label>供应商类型：</label>
                        <select name="txtGongYingShangLeiXing" id="txtGongYingShangLeiXing">
                            <option value="-1">请选择</option>
                            <option value="1">地接</option>
                            <option value="2">票务</option>
                        </select>
                        <label>供应商：</label>
                        <input type="text" name="txtGongYingShang" id="txtGongYingShang" class="searchinput searchinput02" style="width: 150px" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("gongyingshang") %>" />
                        <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;"></a> 
                        <a onclick="toXls();return false;" href="###"><img width="68" height="27" style="margin-top: 0px; border: 0px" alt="导出" src="/images/export.xls.png"></a>
                    </td>
                </tr>
            </table>
            
            <div class="btnbox" style="height:1px; margin:0px; padding:0px;"></div>
            
            <div class="tablelist">
                <!--支出账龄分析表开始--> 
                <asp:PlaceHolder runat="server" ID="phFXB" Visible="false">
                <table cellpadding="0" cellspacing="1" class="fx_tbl">
                    <tr class="odd head">
                        <td class="left1"><span>供应商</span></td>
                        <td class="money"><span><a onclick="return order(5)" style="color: " href="javascript:void(0)">↑</a>交易总额<a onclick="return order(4)" style="color: " href="javascript:void(0)"> ↓</a></span></td>
                        <td class="money"><span><a onclick="return order(1)" style="color: " href="javascript:void(0)">↑</a>欠款总额<a onclick="return order(0)" style="color: " href="javascript:void(0)"> ↓</a></span></td>
                        <td><a onclick="return order(3)" style="color: " href="javascript:void(0)">↑</a>最长拖欠时间<a onclick="return order(2)" style="color: " href="javascript:void(0)"> ↓</a></td>
                    </tr>
                    <asp:Repeater runat="server" ID="rptZhiChuZhangLing">
                        <ItemTemplate>
                            <tr <%#(Container.ItemIndex + 1)%2==0?"class='odd'":"class='even'" %>>
                                <td class="left1" style="width:30%"><span>[<%#Eval("GongYingShangLeiXing") %>]&nbsp;<%#Eval("GongYingShang") %></span></td>
                                <td class="money"><span><%#Eval("JiaoYiZongE","{0:C2}") %></span></td>
                                <td class="money"><span><%#Eval("QianKuanZongE", "{0:C2}")%></span></td>
                                <td><%#Eval("EarlyTime","{0:yyyy-MM-dd}")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr class="odd head">
                        <td></td>
                        <td class="money"><span>合计:<asp:Literal runat="server" ID="ltrZongJinEHeJi"></asp:Literal></span></td>
                        <td class="money"><span><asp:Literal runat="server" ID="ltrWeiFuKuanHeJi"></asp:Literal></span></td>
                        <td></td>
                    </tr>
                </table>
                <table id="tbl_ExportPage" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc1:exportpageinfo id="ExportPageInfo1" runat="server" linktype="4" />
                        </td>
                    </tr>
                </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phEmpty" Visible="false">
                    <div>未统计到任何数据！</div>
                </asp:PlaceHolder>
                <!--支出账龄分析表结束-->
            </div>
        </div>
       
    </div>
    </form> 
</asp:content>
