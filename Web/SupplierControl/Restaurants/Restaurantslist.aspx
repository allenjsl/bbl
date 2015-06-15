<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Restaurantslist.aspx.cs" Inherits="Web.SupplierControl.Restaurants.Restaurants"  Title="餐馆_供应商管理" MasterPageFile="~/masterpage/Back.Master"%>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register  Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register  Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="head" ID="content1" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="content2" runat="server">
<form id="form1" runat="server">
	<div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">供应商管理</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;"> 所在位置 >>  供应商管理 >> 餐馆</td>
            </tr>
            <tr>
              <td colspan="2" height="2" bgcolor="#000000"></td>
            </tr>
          </table>
        </div>
        <div class="hr_10"></div>       
        
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
            <td><div class="searchbox">
                <label>省份：</label>
                <uc1:ucProvince runat="server" ID="ucProvince1" />
                <label>城市：</label>
                <uc1:ucCity runat="server" ID="ucCity1" />
                <label>单位名称：</label> <input name="TxtUnitsname" type="text"  class="searchinput" id="TxtUnitsname" value="<%=TxtUnitsName %>"/>
                <label>菜系：</label>
                <input name="TxtCuisine" type="text"  class="searchinput" id="TxtCuisine" value="<%=CuisineValue %>"/>
                <label><a href="javascript:void(0);" id="Searchbtn"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a></label>               
              </div></td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
        <div class="btnbox">
          <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <%if (grantadd){ %> <td width="90" align="center"><a href="javascript:viod(0);" class="add">新 增</a> </td><%} %>
               <%if (grantdel){ %><td width="90" align="center"><a href="javascript:viod(0);" class="delall">批量删除</a></td><%} %>
               <%if (grantload){ %><td width="90" align="center"><a href="javascript:viod(0);" class="loadxls"><img src="/images/daoru.gif"/> 导 入</a></td><%} %>
               <%if (grantto){ %><td width="90" align="center"><a href="javascript:viod(0);" class="toexcel"><img src="/images/daoru.gif" alt="" width="16" height="16" /> 导 出</a></td><%} %>
            </tr>
          </table>
        </div>
        <div class="tablelist">
          <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
              <th width="3%" height="30" align="center" bgcolor="#BDDCF4">全选<input type="checkbox" name="checkbox" class="selall" /></th>
              <th width="12%" align="center" bgcolor="#BDDCF4">所在地</th>
              <th width="12%" align="center" bgcolor="#bddcf4">单位名称</th>
              <th width="12%" align="center" bgcolor="#bddcf4">菜系</th>
              <th width="12%" align="center" bgcolor="#bddcf4">联系人</th>
              <th width="12%" align="center" bgcolor="#bddcf4">电话</th>
              <th width="12%" align="center" bgcolor="#bddcf4">传真</th>
              <th width="12%" align="center" bgcolor="#bddcf4">交易情况</th>
              <th width="12%" align="center" bgcolor="#bddcf4">操作</th>
            </tr>
            <asp:Repeater ID="replist" runat="server">
              <ItemTemplate>
                  <tr tid="<%# Eval("Id") %>"  bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                      <td height="30"  align="center"><input type="checkbox" name="checkbox" class="selectedAll" /></td>
                      <td align="center"><p><%# Eval("ProvinceName")%><%# Eval("CityName")%></p></td>
                      <td align="center"><%# Eval("UnitName")%></td>
                      <td align="center"><%# Eval("Cuisine")%></td>
                      <td align="center"><%# Eval("SupplierContact") ==null?"":(Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactName%></td>
                      <td align="center"><%# Eval("SupplierContact") ==null?"":(Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactTel%></td>
                      <td align="center"><%# Eval("SupplierContact") ==null?"":(Eval("SupplierContact") as System.Collections.Generic.List<EyouSoft.Model.CompanyStructure.SupplierContact>)[0].ContactFax%></td>
                      <td align="center">共计<%# Eval("TradeNum")%>次</td>
                      <td align="center"><a href="javascript:void(0);" class="show">查看 </a><% if (grantmodify){ %><a href="javascript:viod(0);" class="modify">修改</a><%} if (grantdel){ %> <a href="javascript:;" class="del">删除</a><%} %></td>
                </tr>
              </ItemTemplate>
            </asp:Repeater>  
              <%if (len == 0)
                    { %>
                    <tr align="center"><td colspan="9">没有相关数据</td></tr>
                  <%} %>         
          </table>
          <input type="hidden" name="hidRecordCount" id="hidRecordCount" value="<%=len %>"/>
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td align="right">
                <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
              </td>
            </tr>
          </table>
        </div>
      </div>
    </form>
      <script type="text/javascript">
          var Restaurants = {
              GetListSelectCount: function() {
                  var count = 0;
                  $(".tablelist").find("input[type='checkbox']").each(function() {
                      if ($(this).attr("checked")) {
                          count++;
                      }
                  });
                  return count;
              },
              GetCheckedValueList: function() {
                  var arrayList = new Array();
                  $(".tablelist").find("input[type='checkbox']").each(function() {
                      if ($(this).attr("checked")) {
                          arrayList.push($(this).parent().parent().attr("tid"));
                      }
                  });
                  return arrayList;
              },
              SetAllCheckValue: function(obj) {
                  $(".tablelist").find("input.selectedAll").each(function() {
                      $(this).attr("checked", obj);
                  });
              },
              Search: function() {
                  var province = $("#uc_provinceDynamic").val();
                  var city = $("#uc_cityDynamic").val();
                  var UnitsName = $.trim($("#TxtUnitsname").val());
                  var CuisineValue = $.trim($("#TxtCuisine").val());
                  //参数
                  var para = { province: "", city: "", UnitsName: "", CuisineValue: "" };
                  para.province = province;
                  para.city = city;
                  para.UnitsName = UnitsName;
                  para.CuisineValue = CuisineValue;
                  window.location.href = "/SupplierControl/Restaurants/Restaurantslist.aspx?" + $.param(para);
              }
          };
          $(document).ready(function() {
              ///全选事件
              $("input.selall").click(function() {
                  var that = $(this).attr("checked");
                  if (that) {
                      Restaurants.SetAllCheckValue("checked");
                  } else {
                      Restaurants.SetAllCheckValue("");
                  }
              });

              //回车查询
              $(".searchbox input").bind("keypress", function(e) {
                  if (e.keyCode == 13) {
                      Restaurants.Search();
                      return false;
                  }
              });
              //查询
              $("#Searchbtn").click(function() {
                  Restaurants.Search();
                  return false;
              });
              //导入酒店excel
              $("a.loadxls").click(function() {
                  var url = "/SupplierControl/Restaurants/RestaurantsLoadExcel.aspx";
                  Boxy.iframeDialog({
                      iframeUrl: url,
                      title: "餐馆EXCEL",
                      modal: true,
                      width: "820px",
                      height: "400px"
                  });
                  return false;
              });
              //导出excel
              $("a.toexcel").click(function() {
                  url = location.pathname + location.search;
                  if (url.indexOf("?") >= 0) { url += "&action=toexcel"; } else { url += "?action=toexcel"; }
                  window.location.href = url;
                  return false;
              });
              //新增
              $("a.add").click(function() {
                  var url = "/SupplierControl/Restaurants/RestaurantsAdd.aspx";
                  Boxy.iframeDialog({
                      iframeUrl: url,
                      title: "新增餐馆信息",
                      modal: true,
                      width: "820px",
                      height: "480px"
                  });
                  return false;
              });
              //删除
              $("a.del").click(function() {
                  if (confirm("确认删除所选项?")) {
                      var that = $(this);
                      var tid = that.parent().parent().attr("tid");
                      $.newAjax({
                          type: "POST",
                          data: { "tid": tid },
                          url: "/SupplierControl/Restaurants/Restaurantslist.aspx?action=Restaurantdel",
                          dataType: 'json',
                          success: function(msg) {
                              if (msg.res) {
                                  switch (msg.res) {
                                      case 1:
                                          alert("删除成功!");
                                          location.reload();
                                          break;
                                      default:
                                          alert("删除失败!");
                                  }
                              }
                          },
                          error: function() {
                              alert("服务器繁忙!");
                          }
                      });
                  }
                  return false;
              });
              //批量删除
              $("a.delall").click(function() {
                  if (Restaurants.GetListSelectCount() > 0) {
                      var tid = Restaurants.GetCheckedValueList().join(',');
                      if (!tid) { alert("没有数据不能删除!"); return false; }
                      if (confirm("确认删除所选项?")) {
                          $.newAjax({
                              url: "/SupplierControl/Restaurants/Restaurantslist.aspx?action=Restaurantdel",
                              type: "POST",
                              data: { "tid": tid },
                              dataType: 'json',
                              success: function(d) {
                                  if (d.res) {
                                      switch (d.res) {
                                          case 1:
                                              alert("删除成功");
                                              location.reload();
                                              break;
                                          case -1:
                                              alert("删除失败");
                                              break;
                                      }
                                  }
                              },
                              error: function() {
                                  alert("服务器繁忙!");
                              }
                          });
                      }
                  } else {
                      alert("请选择要删除的数据!");
                  }
                  return false;
              });
              //修改
              $("a.modify").click(function() {
                  var that = $(this);
                  var url = "/SupplierControl/Restaurants/RestaurantsAdd.aspx?type=modify&";
                  var tid = that.parent().parent().attr("tid");
                  Boxy.iframeDialog({
                      iframeUrl: url + "tid=" + tid,
                      title: "修改",
                      modal: true,
                      width: "820",
                      height: "480px"
                  });
                  return false;
              });

              $("a.show").click(function() {
                  var that = $(this);
                  var url = "/SupplierControl/Restaurants/RestaurantsAdd.aspx?type=show&";
                  var tid = that.parent().parent().attr("tid");
                  Boxy.iframeDialog({
                      iframeUrl: url + "tid=" + tid,
                      title: "查看",
                      modal: true,
                      width: "820",
                      height: "480px"
                  });
                  return false;

              })
          });
      </script>
</asp:Content>

