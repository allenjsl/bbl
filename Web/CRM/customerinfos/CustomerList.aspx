<%@ Page Title="�ͻ�����_�ͻ���ϵ����" Language="C#" MasterPageFile="~/masterpage/Back.Master"
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
                        <span class="lineprotitle">�ͻ���ϵ����</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ�ã��ͻ���ϵ����>> �ͻ�����
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
                        <!--ʡ�ݳ���ѡȡ�ؼ���ʼ-->
                        <uc1:ProvinceAndCity runat="server" ID="ProvinceAndCity1" />

                        <script type="text/javascript">
                            window["<%=ProvinceAndCity1.ClientID %>"].replace(null, null, true, false);
                        </script>

                        <!--ʡ�ݳ���ѡȡ�ؼ�����-->
                        <label>
                            ��λ���ƣ�</label>
                        <input type="text" runat="server" class="searchinput" id="txtCompanyName" />
                        <label>
                            ��ϵ�ˣ�</label>
                        <input type="text" runat="server" class="searchinput" id="txtContact" />
                        ��ϵ�绰��<input type="text" class="searchinput" id="txtTelephone" name="txtTelephone" />
                        <label>
                            �������ۣ�</label>
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
            ����Ա��<asp:DropDownList ID="ddl_Oprator" runat="server" CssClass="Oprator">
            </asp:DropDownList>
            <asp:LinkButton ID="LinkButton1" Style="text-align: center; margin-left: 75px; margin-top: 5px"
                runat="server" OnClientClick="return Cust.CheckSeller();" OnClick="LinkButton1_Click">�޸�</asp:LinkButton>
        </div>
        </form>
        <div class="btnbox">
            <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
                <tr>
                    <%if (IsAdd)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.openDialog('/CRM/customerinfos/CustomerEdit.aspx','�����ͻ�����','950px','550px');"
                            id="link2">�� ��</a>
                    </td>
                    <%}
                      if (CheckGrant(Common.Enum.TravelPermission.�ͻ���ϵ����_�ͻ�����_����ָ����������))
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.UpdateSeller();" id="UpdateSeller">�޸�����Ա</a>
                    </td>
                    <%} if (IsDelete)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.del('<%# Eval("Id") %>',this)">ɾ��</a>
                    </td>
                    <%}
                      if (IsImportIn)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.ImportCust();">
                            <img src="/images/daoru.gif" />
                            �� ��</a>
                    </td>
                    <%} if (IsImportOut)
                      { %>
                    <td width="90" align="center">
                        <a href="javascript:;" onclick="return Cust.DownExcel();">
                            <img src="/images/daoru.gif" alt="" width="16" height="16" />
                            �� ��</a>
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
                            ��</a>��λ����<a href="javascript:void(0)" style="color: <%=Orderbyid=="2"?"red":""%>"
                                onclick="return order(2)">��</a>
                    </td>
                    <td width="65px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="3"?"red":""%>" onclick="return order(3)">
                            ��</a>��ϵ��<a href="javascript:void(0)" style="color: <%=Orderbyid=="4"?"red":""%>"
                                onclick="return order(4)">��</a>
                    </td>
                    <td width="60px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="5"?"red":""%>" onclick="return order(5)">
                            ��</a>�绰<a href="javascript:void(0)" style="color: <%=Orderbyid=="6"?"red":""%>" onclick="return order(6)">��</a>
                    </td>
                    <td width="60px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="7"?"red":""%>" onclick="return order(7)">
                            ��</a>�ֻ�<a href="javascript:void(0)" style="color: <%=Orderbyid=="8"?"red":""%>" onclick="return order(8)">��</a>
                    </td>
                    <td width="56px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="9"?"red":""%>" onclick="return order(9)">
                            ��</a>����<a href="javascript:void(0)" style="color: <%=Orderbyid=="10"?"red":""%>"
                                onclick="return order(10)">��</a>
                    </td>
                    <td width="80px" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="11"?"red":""%>" onclick="return order(11)">
                            ��</a>��������<a href="javascript:void(0)" style="color: <%=Orderbyid=="12"?"red":""%>"
                                onclick="return order(12)">��</a>
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="13"?"red":""%>" onclick="return order(13)">
                            ��</a>�������<a href="javascript:void(0)" style="color: <%=Orderbyid=="14"?"red":""%>"
                                onclick="return order(14)">��</a>
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        ��������
                    </td>
                    <td width="8%" align="center" bgcolor="#bddcf4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="17"?"red":""%>" onclick="return order(17)">
                            ��</a>���㷽ʽ<a href="javascript:void(0)" style="color: <%=Orderbyid=="18"?"red":""%>"
                                onclick="return order(18)">��</a>
                    </td>
                    <td width="65px" align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="19"?"red":""%>" onclick="return order(19)">
                            ��</a>���ڵ�<a href="javascript:void(0)" style="color: <%=Orderbyid=="20"?"red":""%>"
                                onclick="return order(20)">��</a>
                    </td>
                    <td width="55px" align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0)" style="color: <%=Orderbyid=="21"?"red":""%>" onclick="return order(21)">
                            ��</a>��ַ<a href="javascript:void(0)" style="color: <%=Orderbyid=="22"?"red":""%>"
                                onclick="return order(22)">��</a>
                    </td>
                    <td width="80px" align="center" bgcolor="#bddcf4">
                        ����
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
                                    <%# Eval("Name")%><%#Convert.ToString(Eval("JieSuanType")) == "None" ? "" : "��" + Eval("JieSuanType") + "��"%></a>
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
                                <a href="javascript:;" onclick="return Cust.getTrade('<%# Eval("Id") %>');">����<%# Eval("TradeNum")%>��</a>
                            </td>
                            <td width="8%" align="center">
                                �����:<%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal((decimal)Eval("SatNum")*100)%>%<br />
                                Ͷ��:<%# Eval("ComplaintNum") %>
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
                                <a href="javascript:;" onclick="return Cust.update('<%# Eval("Id") %>')">�޸�</a>
                                <%} if (IsStop)
                                  { %>
                                <a href="javascript:;" onclick="return Cust.stop('<%# Eval("Id")%>',this)">
                                    <%# (bool)Eval("IsEnable") ? "ͣ��" : "����"%></a><%}
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

        var Cust = {//����ͻ�
            ImportCust: function() {
                Cust.openDialog('/CRM/customerinfos/ImportCustomer.aspx', '����', '800px', '500px');
                return false;
            }, //ȫѡ
            changeAll: function(obj) {
                $("input:checkbox").not("[name='checkAll']").attr("checked", $(obj).attr("checked"));
            }, //�򿪵���
            openDialog: function(p_url, p_title, p_width, p_height) {
                Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: p_width, height: p_height });
            },
            //�޸Ŀͻ�
            update: function(cId) {
                Cust.openDialog("/CRM/customerinfos/CustomerEdit.aspx?custId=" + cId, "�޸Ŀͻ�", "950px", "550px");
                return false;
            },
            show: function(cId) {
                Cust.openDialog("/CRM/customerinfos/CustomerEdit.aspx?type=show&custId=" + cId, "�鿴��ϸ��Ϣ", "950px", "550px");
                return false;
            },
            //�����޸���������Ա
            UpdateSeller: function() {
                var idlist = "";
                $("input[type='checkbox'][name='ckCustomer']:checked").each(function() {
                    idlist += $(this).val() + ',';
                })
                if (idlist.length == 0) {
                    alert("��ѡ��һ������!");
                    return false;
                }
                else {
                    $("#<%=Customerids.ClientID %>").val(idlist.substring(0, idlist.length - 1));
                    var CustomerSellerhtml = $("#CustomerSeller").html();
                    var boxy = new Boxy("<p style='width:250px;height:100px;margin:25px 25px 25px 25px;' class='tjbtn02'>" + CustomerSellerhtml + "</p>", { title: "��������Ա", modal: true });
                    boxy.boxy.find("#<%=ddl_Oprator.ClientID %>").attr("name", "ddl_Oprator");
                    $("[name='ddl_Oprator']").change(function() {
                        $("#<%=SellerName.ClientID %>").val($(this).find("option:selected").text());
                        $("#<%= ddl_Oprator.ClientID %>").val($(this).val());
                    });
                }
            },
            CheckSeller: function() {
                if ($("#<%=ddl_Oprator.ClientID %>").val() == "") {
                    alert("��ѡ����������Ա!");
                    return false;
                } return true;
            },
            //�鿴�������
            getTrade: function(cid) {

                Boxy.iframeDialog({
                    iframeUrl: 'CustomerTrade.aspx?cid=' + cid,
                    title: "�������",
                    modal: true,
                    width: 850,
                    height: 600,
                    data: {}
                });
                return false;
            },
            //ɾ��
            del: function(cId) {
                var idlist = "";
                $("input[type='checkbox'][name='ckCustomer']:checked").each(function() {
                    idlist += $(this).val() + ',';
                })
                if (idlist.length == 0) {
                    alert("��ѡ����Ҫɾ���Ŀͻ����!");
                    return false;
                }
                else {
                    if (confirm("��ȷ��Ҫɾ���ÿͻ�������")) {
                        Cust.postAjax("del", idlist);
                    }
                }
            },
            //ͣ��
            stop: function(cIds, tar) {
                var method1 = $(tar).html() == "ͣ��" ? "stop" : "start";
                var custIds = "";
                if (cIds) {
                    custIds = cIds;
                    if (!confirm(method1 == "stop" ? "��ȷ��Ҫͣ�øÿͻ���" : "��ȷ��Ҫ���øÿͻ���"))
                        return false;
                } else { return false; }
                Cust.postAjax(method1, custIds, tar);
            },
            //�ύ����
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
                                      $(tar).html("����"); return false;
                                  }
                                  else if (p_method == "start") { $(tar).html("ͣ��"); return false; }
                                  window.location = "/CRM/customerinfos/CustomerList.aspx";
                              }
                              else {
                                  alert("����ʧ�ܣ�");
                              }
                          },
                          error: function() {
                              alert("����ʱ����δ֪����");
                          }
                      })
            },
            //����excel
            DownExcel: function() {
                //return Cust.search("downexcel");
                var params = utilsUri.getUrlParams(["page"]);
                params["method"] = "downexcel";
                params["recordcount"] = "<%=recordCount %>";
                window.location.href = utilsUri.createUri(null, params);
                return false;
            },
            //��ѯ
            search: function(isWhat) {
                //ִ�в�ѯ
                var companyName = $("#<%=txtCompanyName.ClientID %>").val(); //��λ����
                var saler = $("#<%=ddlSalePeople.ClientID %>").val(); //��������
                var contacter = $("#<%=txtContact.ClientID %>").val(); //��ϵ��
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
                Cust.openDialog("/CRM/customerinfos/Customerinfo.aspx?custId=" + cid, "��ϵ����Ϣ", "710px", "300px");
                return false;
            })
        });
    </script>

</asp:Content>
