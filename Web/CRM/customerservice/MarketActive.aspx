<%@ Page Title="营销活动_客户关系管理" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="MarketActive.aspx.cs" Inherits="Web.CRM.customerservice.MarketActive" %>
<%@ Register Src="~/UserControl/ucCRM/CustomerServiceHeader.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
   <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="1"  UseMap="营销活动" />
   <div class="hr_10"></div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
            <td><div class="searchbox">
                <label>活动主题： <input type="text"  class="searchinput" id="txtActive" runat="server"/></label>
                <label>活动时间：</label> <input  type="text"  class="searchinput" id="txtActiveDate" runat="server" onfocus="WdatePicker()"/>
                <label><a href="javascript:;" onclick="return MarketA.search();"><img src="/images/searchbtn.gif" style="vertical-align:top;"/></a></label>
              </div></td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
        <div class="btnbox">
          <table width="45%" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
            <%if (IsAdd)
              { %>
              <td width="90" align="center"><a href="javascript:;" onclick="return MarketA.openDialog('/CRM/customerservice/ActiveEdit.aspx','新增营销活动');">新 增</a> </td><%} if(IsDelete)
              { %>
              <td width="90" align="center"><a href="javascript:;" onclick="return MarketA.del('');">删 除</a></td><%} %>
            </tr>
          </table>
        </div>
        <div class="tablelist">
          <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
              <th width="5%" height="30" align="center" bgcolor="#BDDCF4"><input type="checkbox" id="chkAll" onclick="MarketA.changeAll(this);" />全选</th>
              <th width="15%" align="center" bgcolor="#BDDCF4">活动时间 </th>
              <th width="15%" align="center" bgcolor="#bddcf4">活动主题</th>
              <th width="20%" align="center" bgcolor="#bddcf4">活动内容</th>
              <th width="15%" align="center" bgcolor="#bddcf4">主办人</th>
              <th width="15%" align="center" bgcolor="#bddcf4">活动状态</th>
              <th width="15%" align="center" bgcolor="#bddcf4">操作</th>
            </tr>
              <asp:CustomRepeater ID="rptCustomer" runat="server">
                  <ItemTemplate>
            <tr>
              <td height="30"  align="center" bgcolor="#e3f1fc"><input type="checkbox" class="c1" value='<%# Eval("Id") %>'/></td>
              <td align="center" bgcolor="#e3f1fc"><%# Convert.ToDateTime(Eval("Time")).ToString("yyyy-MM-dd") %></td>
              <td align="center" bgcolor="#e3f1fc"><%# Eval("Theme") %></td>
              <td align="center" bgcolor="#e3f1fc"><%#EyouSoft.Common.Utils.GetText2(Eval("Content").ToString(),20,true) %></td>
              <td align="center" bgcolor="#e3f1fc"><%# Eval("Sponsor")%></td>
              <td align="center" bgcolor="#e3f1fc"><%# GetStateStr(Eval("State").ToString())%></td>
              <td align="center" bgcolor="#e3f1fc"><%if (IsUpdate)
                                                     { %><a href="javascript:;" onclick="return MarketA.update('<%# Eval("Id") %>')">修改</a><%} if (IsDelete)
                                                     { %> <a href="javascript:;" onclick="return MarketA.del('<%# Eval("Id") %>')">删除</a><%} %></td>
            </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
            <tr>
              <td height="30"  align="center" bgcolor="#BDDCF4"><input type="checkbox" class="c1" value='<%# Eval("Id") %>'/></td>
              <td align="center" bgcolor="#BDDCF4"><%# Convert.ToDateTime(Eval("Time")).ToString("yyyy-MM-dd") %></td>
              <td align="center" bgcolor="#BDDCF4"><%# Eval("Theme") %></td>
              <td align="center" bgcolor="#BDDCF4"><%# EyouSoft.Common.Utils.GetText2(Eval("Content").ToString(), 20, true)%></td>
              <td align="center" bgcolor="#BDDCF4"><%# Eval("Sponsor")%></td>
              <td align="center" bgcolor="#BDDCF4"><%# GetStateStr(Eval("State").ToString()) %></td>
              <td align="center" bgcolor="#BDDCF4"><%if (IsUpdate)
                                                     { %><a href="javascript:;" onclick="return MarketA.update('<%# Eval("Id") %>')">修改</a><%} if (IsDelete)
                                                     {%> <a href="javascript:;" onclick="return MarketA.del('<%# Eval("Id") %>')">删除</a><%} %></td>
            </tr>
            </AlternatingItemTemplate>
        </asp:CustomRepeater>
          </table>
          
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td align="right">
                <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
              </td>
            </tr>
          </table>
        </div>
        </div>
          <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
          <script type="text/javascript">
              var MarketA = {
                  changeAll: function(tar) {
                      $("input:checkbox").not("#chkAll").attr("checked", $(tar).attr("checked"));
                  },
                  openDialog: function(p_url, p_title) {
                      Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "520px", height: "310px" });
                  },
                  //修改营销活动
                  update: function(cId) {
                      MarketA.openDialog("/CRM/customerservice/ActiveEdit.aspx?aId=" + cId, "修改营销活动");
                      return false;
                  },
                  //删除
                  del: function(cId) {
                      var custIds = "";
                      if (cId == "") {
                          var IdArr = [];
                          $(":checked").not("#chkAll").each(function() {
                              IdArr.push($(this).val());
                          });
                          if (IdArr.length > 0) {
                              if (!confirm("你确认要删除所选的营销活动吗？"))
                                  return false;
                          }
                          else {
                              alert("请选择要删除的营销活动！");
                              return false;
                          }
                          custIds = IdArr.toString();
                      }
                      else if (!confirm("你确认要删除该营销活动吗？")) {
                          return false;
                      }
                      else {
                          custIds = cId;
                      }

                      MarketA.postAjax("del", custIds);
                  },
                  //提交操作
                  postAjax: function(p_method, p_ids) {
                      $.newAjax(
                      {
                          url: "/CRM/customerservice/MarketActive.aspx",
                          data: { method: p_method, ids: p_ids },
                          dataType: "json",
                          cache: false,
                          type: "post",
                          success: function(result) {
                              if (result.success == "1") {
                                  alert(result.message);
                                  window.location = "/CRM/customerservice/MarketActive.aspx";
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
                  //查询
                  search: function() {
                      //执行查询
                      var active = $("#<%=txtActive.ClientID %>").val(); //活动主题
                      var activeDate = $("#<%=txtActiveDate.ClientID %>").val(); //活动时间
                      window.location = "/CRM/customerservice/MarketActive.aspx?active=" + encodeURIComponent(active) + "&activeDate=" + activeDate;
                      return false;
                  }
              }
              $(document).ready(function() {
              $("#<%=txtActive.ClientID %>,#<%=txtActiveDate.ClientID %>").keydown(function(event) {
                      if (event.keyCode == 13) {
                          MarketA.search();
                      }
                  });
              });
			  </script>
</asp:Content>
