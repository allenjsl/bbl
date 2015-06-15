<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelCustomer.aspx.cs" Inherits="Web.CRM.customerservice.SelCustomer" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox" style="height: 30px;">
                        省份：<uc1:ucProvince ID="ucProvince1" runat="server" />
                        城市：
                        <uc3:ucCity ID="ucCity1" runat="server" />
                        <label>
                            单位名称：</label>
                        <input type="text" runat="server" class="searchinput" id="txtCompanyName" />
                        <label>
                            联系人：</label>
                        <input type="text" runat="server" class="searchinput" id="txtContact" />
                        电话：<input type="text" runat="server" id="txt_tel" class="searchinput" />
                        <label>
                            <a href="javascript:void(0);" id="a_search">
                                <img src="/images/searchbtn.gif" border="0" />
                            </a>
                        </label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <div class="tablelist" style="margin-left: 5px">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
                <tr>
                    <th width="3%" align="center" bgcolor="#BDDCF4">
                        &nbsp;
                    </th>
                    <th width="16%" align="center" bgcolor="#bddcf4">
                        单位名称
                    </th>
                    <th width="10%" align="center" bgcolor="#BDDCF4">
                        所在地
                    </th>
                    <th width="10%" align="center" bgcolor="#bddcf4">
                        联系人
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        电话
                    </th>
                </tr>
                <asp:CustomRepeater ID="rptCustomer" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="radio" class="c1" value='<%# Eval("Id") %>' data-isshow="<%# Eval("IsRequiredTourCode")%>"
                                    name="custName" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Name") %><%#Convert.ToString(Eval("JieSuanType")) == "None" ? "" : "【" + Eval("JieSuanType") + "】"%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ProvinceName") %><%# Eval("CityName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ContactName") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc" class="tel">
                                <%# Eval("Phone") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td align="center" bgcolor="#e3f1fc">
                                <input type="radio" class="c1" value='<%# Eval("Id") %>' data-isshow="<%# Eval("IsRequiredTourCode")%>"
                                    name="custName" />
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("Name") %><%#Convert.ToString(Eval("JieSuanType")) == "None" ? "" : "【" + Eval("JieSuanType") + "】"%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc">
                                <%# Eval("ProvinceName") %><%# Eval("CityName")%>
                            </td>
                            <td align="center" bgcolor="#e3f1fc" class="Contact">
                                <%# Eval("ContactName") %>
                            </td>
                            <td align="center" bgcolor="#e3f1fc" class="tel">
                                <%# Eval("Phone") %>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:CustomRepeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                        <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"
                            runat="server"></uc2:ExportPageInfo>
                    </td>
                </tr>
            </table>
            <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td height="40" align="center">
                    </td>
                    <td height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="return save();">保存</a>
                    </td>
                    <td height="40" align="center" class="tjbtn02">
                        <a href="javascript:;" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                            关闭</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        //查询
        function searchCust() {
            //执行查询
            var companyName = $("#<%=txtCompanyName.ClientID %>").val(); //单位名称
            var contacter = $("#<%=txtContact.ClientID %>").val(); //联系人
            window.location.href = window.location.href.replace(/[&]cname=.*/g, "") + "&cname=" + encodeURIComponent(companyName) + "&contact=" + encodeURIComponent(contacter);
            return false;
        }
        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
        function save() {
            var cName = "";
            var cId = "";
            var cContacter = "";
            var cTel = "";
            var cIshow = "";
            if ($("[name='custName']:checked").length)
                $(":checked").each(function() {

                    cId = $(this).val();
                    cName = $(this).closest("td").next("td").html();
                    cName = cName.indexOf('【') > 0 ? cName.substr(0, cName.indexOf('【')) : cName;
                    cContacter = $(this).closest("td").next("td").next("td").next("td").html();
                    cTel = $(this).closest("td").next("td").next("td").next("td").next("td").html();
                    cIshow = $(this).parent().find("input[data-isshow]").attr("data-isshow");
                });
            else {
                alert("请选择组团社!");
                return;
            }
            var iframeId = queryString("desid");
            var callFun = queryString("backFun");

            if (iframeId == "") {

                window.parent.window[callFun]($.trim(cId), $.trim(cName), $.trim(cContacter), $.trim(cTel), $.trim(cIshow));

            }
            else {
                top.Boxy.getIframeWindow(iframeId).window[callFun]($.trim(cId), $.trim(cName), $.trim(cContacter), $.trim(cTel), $.trim(cIshow));

            }
            top.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            return false;
        }


        $(function() {
            $("#a_search").click(function() {
                var para = { pid: "", cid: "", comName: "", contactName: "", phone: "", backFun: "", iframeId: "", desid: "", method: "" };
                para.pid = $("#uc_provinceDynamic").val();
                para.cid = $("#uc_cityDynamic").val();
                para.comName = $.trim($('#<%=this.txtCompanyName.ClientID %>').val());
                para.contactName = $.trim($('#<%=this.txtContact.ClientID %>').val());
                para.phone = $.trim($('#<%=this.txt_tel.ClientID %>').val());
                para.backFun = '<%=Request.QueryString["backFun"] %>';
                para.iframeId = '<%=Request.QueryString["iframeId"] %>';
                para.desid = '<%=Request.QueryString["desid"] %>';
                para.method = '<%=Request.QueryString["method"] %>';
                window.location.href = "/CRM/customerservice/SelCustomer.aspx?" + $.param(para);
                return false;
            })


        })
                  
    </script>

</body>
</html>
