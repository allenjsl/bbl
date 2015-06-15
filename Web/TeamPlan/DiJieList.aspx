<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiJieList.aspx.cs" Inherits="Web.TeamPlan.DiJieList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>线路选用框</title>
    <link href="../css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <style type="text/css">
        .divList
        {
            width: 221px;
            height: 30px;
            float: left;
            border: solid 1px #fff;
            background-color: #dfeefb;
            line-height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="hr_10">
        </div>
        <table border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="../images/yuanleft.gif" />
                </td>
                <td width="650px">
                    <div class="searchbox">
                        <label runat="server" id="lbSupplerTypeName">
                            地接社名称：</label>
                        <asp:TextBox ID="txtDjName" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                        &nbsp;<label><a href="javascript:void(0);" id="btnSearch"><img src="../images/searchbtn.gif"
                            style="vertical-align: top;" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div style="width: 670px; margin: 0 auto;" id="divList">
            <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                    <div class="divList">
                        <input type="radio" name="radio" id="radio_<%#Container.ItemIndex %>" value="<%#Eval("Id") %>" /><label
                            id="lbl_<%#Container.ItemIndex %>" for="radio_<%#Container.ItemIndex %>"><%#Eval("UnitName")%></label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <table width="670px" align="center">
            <tr>
                <td height="30" align="right" class="pageup" colspan="13">
                    <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:void(0);" id="selectxl">选用</a>
                </td>
            </tr>
        </table>
    </div>
    `
    <asp:HiddenField ID="hideText" runat="server" />
    <asp:HiddenField ID="hideValue" runat="server" />
    <asp:HiddenField ID="hideCallBackFun" runat="server" />

    <script type="text/javascript">
        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        $(function() {
            $("#selectxl").click(function() {
                if ($("#divList").find("[type=radio]:checked").length < 1) { alert("请选择地接社."); return; }
                $("#divList").find("[type=radio]:checked").each(function() {
                    var value = $(this).val();
                    var text = $("#lbl_" + $(this).attr("id").split("_")[1]).text();
                    var iframeId = queryString("desid");
                    var callFun = queryString("CBackFun");
                    var index = queryString("index");
                    if (callFun) {
                        top.Boxy.getIframeWindow(iframeId).window[callFun](text, value, index);
                    }
                    else {
                        if (queryString("callback")) {
                            var callback = queryString("callback");
                            eval("window.parent." + callback + "('" + text + "','" + value + "'," + index + ")");
                        } else {
                            var ifid = '<%=EyouSoft.Common.Utils.GetQueryStringValue("ifid") %>';
                            if (ifid != "") {
                                var doc = window.parent.Boxy.getIframeWindow(ifid);

                                if (doc.$("#" + $("#<%=hideText.ClientID %>").val()) != null) {
                                    doc.$("#" + $("#<%=hideText.ClientID %>").val()).val(text);
                                }
                                if (doc.$("#" + $("#<%=hideValue.ClientID %>").val()) != null) {
                                    doc.$("#" + $("#<%=hideValue.ClientID %>").val()).val(value);
                                }
                                if ($("#<%=hideCallBackFun.ClientID %>").val() != "") {
                                    doc["" + $("#<%=hideCallBackFun.ClientID %>").val() + ""]();
                                }
                            } else {

                                if (window.parent.$("#" + $("#<%=hideText.ClientID %>").val()) != null) {
                                    window.parent.$("#" + $("#<%=hideText.ClientID %>").val()).val(text);
                                }

                                if (window.parent.$("#" + $("#<%=hideValue.ClientID %>").val()) != null) {
                                    window.parent.$("#" + $("#<%=hideValue.ClientID %>").val()).val(value);
                                }
                                if ($("#<%=hideCallBackFun.ClientID %>").val() != "") {
                                    window.parent["" + $("#<%=hideCallBackFun.ClientID %>").val() + ""]();
                                }
                            }
                        }

                    }
                    parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                })

            });

            $("#btnSearch").click(function() {
                var para = { text: "", value: "", djName: "", iframeId: "", Page: "1", sType: '<%=Request.QueryString["sType"] %>', ifid: "", callBackFun: "" };
                para.text = $("#<%=hideText.ClientID %>").val();
                para.value = $("#<%=hideValue.ClientID %>").val();
                para.iframeId = '<%=Request.QueryString["iframeId"] %>';
                para.djName = $("#<%=txtDjName.ClientID %>").val();
                para.ifid = '<%=Request.QueryString["ifid"] %>';
                para.callBackFun = "GetDjInfo";
                window.location.href = "/TeamPlan/DiJieList.aspx?" + $.param(para);
            });
        });
    </script>

    </form>
</body>
</html>
