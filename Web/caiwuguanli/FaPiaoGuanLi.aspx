<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaPiaoGuanLi.aspx.cs" Inherits="Web.caiwuguanli.FaPiaoGuanLi" MasterPageFile="~/masterpage/Back.Master" Title="发票管理_财务管理" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
    <script src=" /js/utilsUri.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
    <style type="text/css">
    .lb_tbl{border:0px;width:100%;margin:0px auto; /*border-top:1px solid #efefef;border-left:1px solid #efefef;*/}
    .lb_tbl td{ /*width:25%;border-bottom:1px solid #efefef;border-right:1px solid #efefef;*/ height:27px; text-align:center;}
    .lb_tbl tr.head{font-weight:bold; /*background:#f6f6f6;*/}
    .lb_tbl td.money{text-align:right;}
    .lb_tbl td.money span{ padding-right:10px;}
    .lb_tbl td.left1 {text-align:left;}
    .lb_tbl td.left1 span {padding-left:10px}
    </style>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="c1">
    <form runat="server" id="form1">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">财务管理-发票管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>>财务管理 >> 发票管理
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
                        <input type="text" onfocus="WdatePicker()" id="txtCTSTime" class="searchinput" name="txtCTLTime" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ctstime") %>" style="width: 80px" />-<input type="text" onfocus="WdatePicker()" id="txtCTETime" class="searchinput" name="txtCTETime" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ctetime") %>" style="width: 80px" />
                        <label>开票时间：</label>
                        <input type="text" onfocus="WdatePicker()" id="txtKPSTime" class="searchinput" name="txtKPSTime" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kpstime") %>" style="width: 80px" />-<input type="text" onfocus="WdatePicker()" id="txtKPETime" class="searchinput" name="txtKPETime" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kpetime") %>" style="width: 80px" />                        
                        <label>开&nbsp;票&nbsp;人：</label>
                        <input type="text" id="txtKaiPiaoRen" name="txtKaiPiaoRen" class="searchinput searchinput02" style="width:80px;" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kaipiaoren") %>" /><br />
                        <label>客户单位：</label>
                        <input type="text" name="txtKeHuMingCheng" id="txtKeHuMingCheng" class="searchinput searchinput02" style="width: 150px" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("kehumingcheng") %>" />
                        <a id="btnSearch" href="javascript:void(0);"><img src="/images/searchbtn.gif" style="vertical-align: top;" alt="查询" /></a>                         
                    </td>
                </tr>
            </table>
            
            <div class="btnbox" style="height:1px; margin:0px; padding:0px;"></div>
            
            <div class="tablelist">                
                <!--发票管理列表开始--> 
                <asp:PlaceHolder runat="server" ID="phLB" Visible="true">
                <table cellpadding="0" cellspacing="1" class="lb_tbl">
                    <tr class="odd head">
                        <td class="left1"  style="width:31%"><span>客户单位</span></td>
                        <td class="money" style="width:18%"><span>交易总额</span></td>
                        <td class="money" style="width:18%"><span>开票总额</span></td>
                        <td class="money" style="width:18%"><span>未开票金额</span></td>
                        <td style="width:15%">操作</td>
                    </tr>
                    <asp:Repeater runat="server" ID="rptFaPiaoGuanLi">
                        <ItemTemplate>
                            <tr <%#(Container.ItemIndex + 1)%2==0?"class='odd'":"class='even'" %>>
                                <td class="left1"><span class="selector_crmname"><%#Eval("CrmName") %></span></td>
                                <td class="money"><span><%#Eval("JiaoYiJinE","{0:C2}") %></span></td>
                                <td class="money"><span><%#Eval("KaiPiaoJinE", "{0:C2}")%></span></td>
                                <td class="money"><span><%#Eval("WeiKaiPiaoJinE", "{0:C2}")%></span></td>
                                <td><a href="javascript:void(0)" onclick="faPiao.dengJi(<%#Eval("CrmId") %>,this)">登记</a>&nbsp;&nbsp;<a href="javascript:void(0)" onclick="faPiao.chaKan(<%#Eval("CrmId") %>,this)">查看</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr class="odd head">
                        <td></td>
                        <td class="money"><span>合计:<asp:Literal runat="server" ID="ltrJiaoYiJinEHeJi"></asp:Literal></span></td>
                        <td class="money"><span><asp:Literal runat="server" ID="ltrKaiPiaoJinEHeJi"></asp:Literal></span></td>
                        <td class="money"><span><asp:Literal runat="server" ID="ltrWeiKaiPiaoJinEHeJi"></asp:Literal></span></td>
                        <td></td>
                    </tr>
                </table>
                <table id="tbl_ExportPage" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right">
                            <cc1:exportpageinfo id="paging" runat="server" linktype="5" />
                        </td>
                    </tr>
                </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder runat="server" ID="phEmpty" Visible="true">
                    <div>未找到任何数据！</div>
                </asp:PlaceHolder>
                <!--发票管理列表结束-->
            </div>
        </div>       
    </div>
    </form>
    
    <script type="text/javascript">
        var faPiao = {
            //查询
            search: function() {
                var params = { "ctstime": $("#txtCTSTime").val()
                , "ctetime": $("#txtCTETime").val()
                , "kpstime": $("#txtKPSTime").val()
                , "kpetime": $("#txtKPETime").val()
                , 'kehumingcheng': encodeURIComponent($("#txtKeHuMingCheng").val())
                , 'kaipiaoren': encodeURIComponent($("#txtKaiPiaoRen").val())
                };

                window.location.href = utilsUri.createUri(null, params);
                return false;
            },
            //发票登记
            dengJi: function(keHuDanWeiId, aobj) {
                var keHuDanWeiMingCheng = $(aobj).closest("tr").find(".selector_crmname").html();

                Boxy.iframeDialog({
                    iframeUrl: "/caiwuguanli/fapiaodengji.aspx?kehudanweiid=" + keHuDanWeiId,
                    title: "开票登记-" + keHuDanWeiMingCheng,
                    modal: true,
                    width: "820px",
                    height: "220px"
                });

                return false;
            },
            //发票查看
            chaKan: function(keHuDanWeiId, aobj) {
                var keHuDanWeiMingCheng = $(aobj).closest("tr").find(".selector_crmname").html();
                var params = utilsUri.getUrlParams(["page"]);
                params["kehudanweiid"] = keHuDanWeiId;
                params["kaipiaoren"] = encodeURIComponent(decodeURIComponent(params["kaipiaoren"] || ""));

                Boxy.iframeDialog({
                    iframeUrl: utilsUri.createUri("/caiwuguanli/fapiaochakan.aspx", params),
                    title: "开票明细-" + keHuDanWeiMingCheng,
                    modal: true,
                    width: "850px",
                    height: "470px",
                    afterHide: function() { parent.location.href = parent.location.href; }
                });

                return false;
            }
        };

        $(document).ready(function() {
            $("#btnSearch").bind("click", function() { faPiao.search(); return false; });
        });
    </script> 
</asp:Content>
