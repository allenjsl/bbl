<%@ Page Title="客户资料_客户关系管理" Language="C#" MasterPageFile="~/masterpage/Back.Master"
    AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="Web.CRM.customerinfos.CustomerList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ProvinceAndCity.ascx" TagName="ProvinceAndCity" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="/js/utilsUri.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">客户关系管理</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置：客户关系管理>> 客户资料
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <form runat="server" id="custForm">
        <input runat="server" type="hidden" name="Customerids" id="Customerids" />
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" />
                </td>
                <td>
                    <div class="searchbox">
                        <!--省份城市选取控件开始-->
                        <uc1:ProvinceAndCity runat="server" ID="ProvinceAndCity1" />

                        <script type="text/javascript">
                            window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                        </script>

                        <!--省份城市选取控件结束-->
                        <label>
                            单位名称：</label>
                        <input type="text" runat="server" class="searchinput" id="txtCompanyName" />
                        <label>
                            联系人：</label>
                        <input type="text" runat="server" class="searchinput" id="txtContact" />
                        联系电话：<input type="text" class="searchinput" id="txtTelephone" name="txtTelephone" />
                        <label>
                            责任销售：</label>
                        <asp:DropDownList ID="ddlSalePeople" runat="server">
                        </asp:DropDownList>
                        <label>
                            <a href="javascript:;" onclick="return Cust.search();">
                                <img src="/images/searchbtn.gif" style="vertical-align: top;" /></a></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" />
                </td>
            </tr>
        </table>
        <style>
            .Oprator
            {
                margin-left: 20px;
            }
            .trth td
            {
                font-weight: bold;
            }
        </style>
        <div id="CustomerSeller" style="display: none;" class="tjbtn02">
            <input type="hidden" id="SellerName" name="SellerName" runat="server" />
            销售员：<asp:DropDownList ID="ddl_Oprator" runat="server" CssClass="Oprator">
            </asp:DropDownList>
            <asp:LinkButton ID="LinkButton1" Style="text-align: center; margin-left: 75px; margin-top: 5px"
                runat="server" OnClientClick="return Cust.CheckSeller();" OnClick="LinkButton1_Click">修改</asp:LinkButton>
        </div>
        </form>
        <div class="btnbox">
            <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <%if (IsAdd)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.openDialog('/CRM/customerinfos/CustomerEdit.aspx','新增客户资料','950px','550px');"
                            id="link2">新 增</a>
                    </td>
                    <%}
                      if (CheckGrant(Common.Enum.TravelPermission.客户关系管理_客户资料_批量指定责任销售))
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.UpdateSeller();" id="UpdateSeller">修改销售员</a>
                    </td>
                    <%} if (IsDelete)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.del('<%# Eval("Id") %>',this)">删除</a>
                    </td>
                    <%}
                      if (IsImportIn)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.ImportCust();">
                            <img src="/images/daoru.gif" />
                            导 入</a>
                    </td>
                    <%} if (IsImportOut)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.DownExcel();">
                            <img src="/images/daoru.gif" alt="" width="16" height="16" />
                            导 出</a>
                    </td>
                    <%}%>
                </tr>
            </table>
        </div>
        <div class="tablelist">
            <div style="float: right;">
                <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="4" PageStyleType="NewButton"
                    CurrencyPageCssClass="RedFnt" />
            </div>
            <table width="100%" border="0" cellpadding="0" cellspacing="1" style="table-layout: fixed">
                <tr class="trth">
                    <td width="40px" align="center" bgcolor="#bddcf4">
                        <input type="checkbox" id="checkAll" name="checkAll" onclick="Cust.changeAll(this);" />
                    </td>
                    <td width="110px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="1"?"red":""%>" onclick="return order(1)">
                            ↑</a>单位名称<a href="javascript:void(0)" style="color: <%=Orderbyid=="2"?"red":""%>"
                                onclick="return order(2)">↓</a>
                    </td>
                    <td width="65px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="3"?"red":""%>" onclick="return order(3)">
                            ↑</a>联系人<a href="javascript:void(0)" style="color: <%=Orderbyid=="4"?"red":""%>"
                                onclick="return order(4)">↓</a>
                    </td>
                    <td width="60px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="5"?"red":""%>" onclick="return order(5)">
                            ↑</a>电话<a href="javascript:void(0)" style="color: <%=Orderbyid=="6"?"red":""%>" onclick="return order(6)">↓</a>
                    </td>
                    <td width="60px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="7"?"red":""%>" onclick="return order(7)">
                            ↑</a>手机<a href="javascript:void(0)" style="color: <%=Orderbyid=="8"?"red":""%>" onclick="return order(8)">↓</a>
                    </td>
                    <td width="56px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="9"?"red":""%>" onclick="return order(9)">
                            ↑</a>传真<a href="javascript:void(0)" style="color: <%=Orderbyid=="10"?"red":""%>"
                                onclick="return order(10)">↓</a>
                    </td>
                    <td width="80px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="11"?"red":""%>" onclick="return order(11)">
                            ↑</a>责任销售<a href="javascript:void(0)" style="color: <%=Orderbyid=="12"?"red":""%>"
                                onclick="return order(12)">↓</a>
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="13"?"red":""%>" onclick="return order(13)">
                            ↑</a>交易情况<a href="javascript:void(0)" style="color: <%=Orderbyid=="14"?"red":""%>"
                                onclick="return order(14)">↓</a>
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        质量反馈
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="17"?"red":""%>" onclick="return order(17)">
                            ↑</a>结算方式<a href="javascript:void(0)" style="color: <%=Orderbyid=="18"?"red":""%>"
                                onclick="return order(18)">↓</a>
                    </td>
                    <td width="65px" align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="19"?"red":""%>" onclick="return order(19)">
                            ↑</a>所在地<a href="javascript:void(0)" style="color: <%=Orderbyid=="20"?"red":""%>"
                                onclick="return order(20)">↓</a>
                    </td>
                    <td width="55px" align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="21"?"red":""%>" onclick="return order(21)">
                            ↑</a>地址<a href="javascript:void(0)" style="color: <%=Orderbyid=="22"?"red":""%>"
                                onclick="return order(22)">↓</a>
                    </td>
                    <td width="80px" align="center" bgcolor="#bddcf4">
                        操作
                    </td>
                </tr>
                <asp:CustomRepeater ID="rptCustomer" runat="server">
                    <ItemTemplate>
                        <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                            <th width="3%" align="center" class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
                                <input type="checkbox" name="ckCustomer" value="<%# Eval("Id") %>" /><%# Container.ItemIndex + 1+( this.pageIndex - 1) * this.pageSize%>
                            </th>
                            <td width="17%" align="center">
                                <a href="javascript:;" onclick="return Cust.show('<%# Eval("Id") %>')">
                                    <%# Eval("Name")%><%#Convert.ToString(Eval("JieSuanType")) == "None" ? "" : "【" + Eval("JieSuanType") + "】"%></a>
                            </td>
                            <td width="6%" align="center">
                                <a href="javascript:" name="show" ref="<%# Eval("Id") %>">
                                    <%# Eval("ContactName") %></a>
                            </td>
                            <td align="left" style="word-wrap: break-word; overflow: hidden;">
                                <%# Eval("Phone") %>
                            </td>
                            <td width="8%" align="left" style="word-wrap: break-word; overflow: hidden;">
                                <%# Eval("Mobile")%>
                            </td>
                            <td width="8%" align="center" style="word-wrap: break-word; overflow: hidden;">
                                <%# Eval("Fax") %>
                            </td>
                            <td width="6%" align="center">
                                <%# Eval("Saler")%>
                            </td>
                            <td width="6%" align="center">
                                <a href="javascript:;" onclick="return Cust.getTrade('<%# Eval("Id") %>');">共计<%# Eval("TradeNum")%>次</a>
                            </td>
                            <td width="8%" align="center">
                                满意度:<%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal((decimal)Eval("SatNum")*100)%>%<br />
                                投诉:<%# Eval("ComplaintNum") %>
                            </td>
                            <td width="6%" align="center">
                                <%# Eval("AccountWay")%>
                            </td>
                            <td width="8%" align="center">
                                <%# Eval("ProvinceName") %>&nbsp;<%# Eval("CityName")%>
                            </td>
                            <td width="8%" align="center" style="word-wrap: break-word; overflow: hidden;">
                                <%# Eval("Adress")%>
                            </td>
                            <td width="8%" align="center">
                                <%if (IsUpdate)
                                  { %>
                                <a href="javascript:;" onclick="return Cust.update('<%# Eval("Id") %>')">修改</a>
                                <%} if (IsStop)
                                  { %>
                                <a href="javascript:;" onclick="return Cust.stop('<%# Eval("Id")%>',this)">
                                    <%# (bool)Eval("IsEnable") ? "停用" : "启用"%></a><%}
                                    %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:CustomRepeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="right">
                        <cc1:ExporPageInfoSelect ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        function order(v) {
            var Url = "<%=Request.Url.ToString() %>";
            if (Url.indexOf("order=", 0) > 0) {
                Url = Url.replace(/order=\d*/, "order=" + v);
            } else if (Url.indexOf("?", 0) > 0) {
                Url += "&order=" + v;
            } else {
                Url += "?order=" + v;
            }
            location.href = Url;
            return false;
        };

        var Cust = {//导入客户
            ImportCust: function() {
                Cust.openDialog('/CRM/customerinfos/ImportCustomer.aspx', '导入', '800px', '500px');
                return false;
            }, //全选
            changeAll: function(obj) {
                $("input:checkbox").not("[name='checkAll']").attr("checked", $(obj).attr("checked"));
            }, //打开弹窗
            openDialog: function(p_url, p_title, p_width, p_height) {
                Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
            },
            //修改客户
            update: function(cId) {
                Cust.openDialog("/CRM/customerinfos/CustomerEdit.aspx?custId=" + cId, "修改客户", "950px", "550px");
                return false;
            },
            show: function(cId) {
                Cust.openDialog("/CRM/customerinfos/CustomerEdit.aspx?type=show&custId=" + cId, "查看详细信息", "950px", "550px");
                return false;
            },
            //批量修改责任销售员
            UpdateSeller: function() {
                var idlist = "";
                $("input[type='checkbox'][name='ckCustomer']:checked").each(function() {
                    idlist += $(this).val() + ',';
                })
                if (idlist.length == 0) {
                    alert("请选择一行数据!");
                    return false;
                }
                else {
                    $("#<%=Customerids.ClientID %>").val(idlist.substring(0, idlist.length - 1));
                    var CustomerSellerhtml = $("#CustomerSeller").html();
                    var boxy = new Boxy("<p style='width:250px;height:100px;margin:25px 25px 25px 25px;' class='tjbtn02'>" + CustomerSellerhtml + "</p>", { title: "责任销售员", modal: true });
                    boxy.boxy.find("#<%=ddl_Oprator.ClientID %>").attr("name", "ddl_Oprator");
                    $("[name='ddl_Oprator']").change(function() {
                        $("#<%=SellerName.ClientID %>").val($(this).find("option:selected").text());
                        $("#<%= ddl_Oprator.ClientID %>").val($(this).val());
                    });
                }
            },
            CheckSeller: function() {
                if ($("#<%=ddl_Oprator.ClientID %>").val() == "") {
                    alert("请选择责任销售员!");
                    return false;
                } return true;
            },
            //查看交易情况
            getTrade: function(cid) {

                Boxy.iframeDialog({
                    iframeUrl: 'CustomerTrade.aspx?cid=' + cid,
                    title: "交易情况",
                    modal: true,
                    width: 850,
                    height: 600,
                    data: {}
                });
                return false;
            },
            //删除
            del: function(cId) {
                var idlist = "";
                $("input[type='checkbox'][name='ckCustomer']:checked").each(function() {
                    idlist += $(this).val() + ',';
                })
                if (idlist.length == 0) {
                    alert("请选择需要删除的客户编号!");
                    return false;
                }
                else {
                    if (confirm("你确认要删除该客户资料吗？")) {
                        Cust.postAjax("del", idlist);
                    }
                }
            },
            //停用
            stop: function(cIds, tar) {
                var method1 = $(tar).html() == "停用" ? "stop" : "start";
                var custIds = "";
                if (cIds) {
                    custIds = cIds;
                    if (!confirm(method1 == "stop" ? "你确定要停用该客户吗？" : "你确定要启用该客户吗？"))
                        return false;
                } else { return false; }
                Cust.postAjax(method1, custIds, tar);
            },
            //提交操作
            postAjax: function(p_method, p_ids, tar) {
                $.newAjax(
                      {
                          url: "/CRM/customerinfos/CustomerList.aspx",
                          data: { method: p_method, ids: p_ids },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert(result.message);
                                  if (p_method == "stop") {
                                      $(tar).html("启用"); return false;
                                  }
                                  else if (p_method == "start") { $(tar).html("停用"); return false; }
                                  window.location = "/CRM/customerinfos/CustomerList.aspx";
                              }
                              else {
                                  alert("操作失败！");
                              }
                          },
                          error: function() {
                              alert("操作时发生未知错误！");
                          }
                      })
            },
            //导出excel
            DownExcel: function() {
                //return Cust.search("downexcel");
                var params = utilsUri.getUrlParams(["page"]);
                params["method"] = "downexcel";
                params["recordcount"] = "<%=recordCount %>";
                window.location.href = utilsUri.createUri(null, params);
                return false;
            },
            //查询
            search: function(isWhat) {
                //执行查询
                var companyName = $("#<%=txtCompanyName.ClientID %>").val(); //单位名称
                var saler = $("#<%=ddlSalePeople.ClientID %>").val(); //责任销售
                var contacter = $("#<%=txtContact.ClientID %>").val(); //联系人
                var province = window["<%=ProvinceAndCity1.ClientID %>"].getProvince().id;
                var city = window["<%=ProvinceAndCity1.ClientID %>"].getCity().id;
                var telephone = $("#txtTelephone").val();
                var theUrl = "/CRM/customerinfos/CustomerList.aspx?province=" + province + "&city=" + city + "&saler=" + encodeURIComponent(saler) + "&companyName=" + encodeURIComponent(companyName) + "&contacter=" + encodeURIComponent(contacter) + "&telephone=" + telephone;
                if (isWhat && isWhat == "downexcel") {
                    theUrl += "&method=downexcel" + "&recordcount=<%=recordCount %>";
                }
                window.location = theUrl;
                return false;
            }
        }
        $(document).ready(function() {
            $("#<%=txtCompanyName.ClientID %>,#<%=txtContact.ClientID %>").keydown(function(event) {
                if (event.keyCode == 13) {
                    Cust.search();
                }
            });

            $("[name='show']").click(function() {
                var cid = $(this).attr("ref");
                Cust.openDialog("/CRM/customerinfos/Customerinfo.aspx?custId=" + cid, "联系人信息", "710px", "300px");
                return false;
            })
        });
    </script>

</asp:Content>
