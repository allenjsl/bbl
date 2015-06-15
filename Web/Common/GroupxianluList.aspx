<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupxianluList.aspx.cs"
    Inherits="Web.Common.GroupxianluList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>线路选用框</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="mainbody">
        <div class="hr_10">
        </div>
        <table width="1000" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="../images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            线路名称：</label>
                        <input type="text" name="txt_xianluName" runat="server" id="txt_xianluName" class="searchinput searchinput02" />
                        <label>
                            出团时间：</label>
                        <input name="txt_date" type="text" runat="server" class="searchinput" id="txt_date"
                            onfocus="WdatePicker()" />
                        <label>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/searchbtn.gif"
                                OnClick="ImageButton1_Click" />
                        </label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="../images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table width="900" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="even">
                <asp:Repeater runat="server" ID="rptList">
                    <ItemTemplate>
                        <%if (i % 4 == 0)
                          { %>
                        </tr>
                        <%if (k % 2 == 0)
                          { %>
                        <tr class="odd">
                            <%}
                          else
                          {%>
                            <tr class="even">
                                <%}
                          k++;
                          }
                                %>
                                <td width="250">
                                    <input type="radio" name="radio" id="radio2" area="<%#Eval("AreaId") %>" value="<%#Eval("RouteId")%>" /><%#Eval("RouteName").ToString().Trim()%>
                                </td>
                                <%i++; %>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
            <tr>
                <td height="40" align="right" class="pageup" colspan="4"> 
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="4" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="#" id="selectxl">选用线路</a>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
    function queryString(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    }
    $(function() {
        $("#selectxl").click(function() {
            if(queryString("PiframeId"))
            {
                var doc = window.parent.Boxy.getIframeDocument(queryString("PiframeId"));
                doc.getElementById(queryString("txtname")).value=$("[type=radio]:checked").parent().text();
                doc.getElementById(queryString("hdid")).value=$("[type=radio]:checked").val();
                if (queryString("callback")) {
                    var callback = queryString("callback");
                    eval(doc+"."+callback+"()");
                }
            }else
            {
                window.parent.$("#" + queryString("txtname")).val($.trim($("[type=radio]:checked").parent().text()));
                window.parent.$("#" + queryString("hdid")).val($("[type=radio]:checked").val());
                if (queryString("callback")) {
                    var callback = queryString("callback");
                    eval("window.parent."+callback+"()");
                }
            }
            window.parent.Boxy.getIframeDialog("<%=Request.QueryString["iframeid"] %>").hide();
        });
    });
    </script>

    </form>
</body>
</html>













 