<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Quote.aspx.cs" Inherits="Web.xianlu.Quote"  MasterPageFile="~/masterpage/Back.Master" Title="我要报价_线路库_线路产品库"%>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ConProjectControl.ascx" TagName="ProjectControl1" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PriceControl.ascx"   TagName="PriceControl" TagPrefix="Uc3"%>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="c1" ID="Main" runat="server">
<form id="form1" runat="server" enctype="multipart/form-data">
<input type="hidden" value="1" name="issave"/>
<div class="mainbody">
  <div class="lineprotitlebox">
     <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tbody>
           <tr>
             <td width="15%" nowrap="nowrap">
               <span class="lineprotitle">线路产品库</span>
             </td>
             <td width="85%" nowrap="nowrap" align="right">
               所在位置&gt;&gt; 线路产品库
             </td>
           </tr>
        </tbody>       
     </table>
  </div>
  <div class="hr_10"></div>
  <div class="addlinebox">
     <fieldset style="border: 1px solid rgb(189, 220, 244); padding: 5px 0pt;">
        <legend>列表</legend>
        <table width="98%"  cellspacing="1" cellpadding="0" border="0" align="center">
          <tbody>
            <cc1:CustomRepeater ID="Ctp_QuoteList" runat="server">
              <HeaderTemplate>
                  <tr class="odd">
                    <th width="16%" height="30" align="center">询价单位</th>
                    <th width="16%" align="center">联系人</th>
                    <th width="16%" align="center">出团日期</th>
                    <th width="16%" align="center">人数</th>
                    <th width="16%" align="center">我社报价</th>
                    <th width="16%" align="center">操作</th>
                 </tr>
              </HeaderTemplate>
              <ItemTemplate>
                   <tr class="even">
                        <td height="30" align="center"><%# Eval("QuoteUnitsName") %></td>
                        <td align="center"><%# Eval("ContactName")%></td>
                        <td align="center"><%# Eval("TmpLeaveDate", "{0:yyyy-MM-dd}")%></td>
                        <td align="center"><%# Eval("PeopleNum")%></td>
                        <td align="center"><%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("SelfQuoteSum").ToString())%></td>
                        <td align="center">
                           <a href="Javascript:void(0);" name="QouteInfo" ref="<%#Eval("QuoteId") %>"><font class="fblue">查看</font></a>
                           <a href="/xianlu/Quote.aspx?action=update&areaid=<%=Request.QueryString["areaid"] %>&QuoteID=<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>&RouteId=<%# Eval("RouteId") %>"><font class="fblue">修改</font></a>
                           <a href="Javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>"><font class="fblue">删除</font></a>
                        </td>
                  </tr>
              </ItemTemplate>
              <AlternatingItemTemplate>
                   <tr class="odd">
                        <td height="30" align="center"><%# Eval("QuoteUnitsName") %></td>
                        <td align="center"><%# Eval("ContactName")%></td>
                        <td align="center"><%# Eval("TmpLeaveDate", "{0:yyyy-MM-dd}")%></td>
                        <td align="center"><%# Eval("PeopleNum")%></td>
                        <td align="center"><%# EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("SelfQuoteSum").ToString())%></td>
                        <td align="center">
                           <a href="Javascript:void(0);" name="QouteInfo" ref="<%#Eval("QuoteId") %>"><font class="fblue">查看</font></a>
                           <a href="/xianlu/Quote.aspx?action=update&areaid=<%=Request.QueryString["areaid"] %>&QuoteID=<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>&RouteId=<%# Eval("RouteId") %>"><font class="fblue">修改</font></a>
                           <a href="Javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>"><font class="fblue">删除</font></a>
                        </td>
                 </tr>
              </AlternatingItemTemplate>
              <FooterTemplate></FooterTemplate>
            </cc1:CustomRepeater>                                    
          </tbody>
        </table>
     </fieldset>
     <fieldset style="border: 1px solid rgb(189, 220, 244); padding: 5px 0pt;">
      <asp:HiddenField ID="hideType" runat="server" />      
       <legend>我要报价</legend>
        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
          <tbody>
             <tr class="odd">
                <th height="25" align="center" width="10%"><font style="color:Red;">*</font>询价单位：</th>
                <td align="left" width="15%">
                    <input type="text" name="Txt_Inquiry" id="Txt_Inquiry" class="widowkuangtext"  runat="server"  readonly="readonly" style="background-color:#dadada"/>
                    <input type="hidden" id="hidCustId" name="hidCustId" runat="server" />
                    <a href="javascript:void(0);" onclick="return Quote.selCust();"><img src="/images/sanping_04.gif" alt=""></a>           
                </td>
                <th align="center" width="10%">联系人：</th>
                <td  align="left" width="15%">
                    <input type="text" name="Txt_Contact" id="Txt_Contact" class="widowkuangtext"  runat="server"/>
                </td>
                <th  align="center" width="10%">联系电话： </th>
                <td  align="left" width="15%">
                    <input type="text" name="Txt_TelPhone" id="Txt_TelPhone" class="widowkuangtext"  runat="server"/>
                </td>
            </tr>
            <tr class="even">
                <th height="25" align="center"><font style="color:Red;">*</font>预计出团时间：</th>
                <td align="left">
                   <input name="Txt_GroupStarTime" type="text" class="searchinput" id="Txt_GroupStarTime"  runat="server" onfocus="WdatePicker();"/>                           
                </td>
                <th align="center"><font style="color:Red;">*</font>人数：</th>
                <td colspan="3" align="left">
                    <input type="text" name="Txt_Numbers" id="Txt_Numbers" class="widowkuangtext"  runat="server" />
                </td>
            </tr>
           <tr class="odd">
                <th align="center" colspan="2">项目</th>
                <th align="center" colspan="2">具体要求</th>
                <th align="center" colspan="2"><span style="margin-left:160px">操作</span></th>
            </tr>
            <tr class="even">
                <th height="25" align="center" bgcolor="#E3F1FC">客人要求：</th>
                      <td colspan="5" align="left" bgcolor="#BDDCF4"> 
                      <uc2:ProjectControl1  ID="ProjectControl" runat="server"/>
               </td>
             </tr>  
             <tr class="odd">
                <th height="25" align="center">价格组成：</th>
                <td colspan="5" align="left"><Uc3:PriceControl ID="PriceControl1" runat="server"/></td>               
            </tr>
            <tr class="even">
                <td height="25" align="center">
                    备注：
                </td>
                <td colspan="5" align="left">
                    <textarea class="textareastyle" id="Txt_RemarksBottom" name="Txt_RemarksBottom" 
                        runat="server" cols="20" rows="1"></textarea>
                </td>
            </tr>
            <tr>
                <th colspan="6" align="center">
                    <table width="320" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                           <td height="40" align="center"></td>
                           <td height="40" align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton1" runat="server"  OnClientClick="return Quote.CheckForm();" onclick="LinkButton1_Click">保存</asp:LinkButton></td>
                           <td height="40" align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton2" runat="server"  
                                    OnClientClick="return Quote.quoteSucess();" Height="31px">报价成功</asp:LinkButton></td>
                            <td align="center" class="tjbtn02">
                                <a href="/xianlu/LineProducts.aspx";>返回</a>
                            </td>
                        </tr>
                    </table>
                </th>
            </tr>
          </tbody>
        </table>
     </fieldset>
  </div>
  <style>
  .Oprator{ margin-left:28px}
  </style>
  <div id="div1"  style="display:none;" class="tjbtn02">
    <span style="color:Red">*</span>请填写团号：<input type="text"  id="TourCode" runat="server"  /><br />
    <span style="color:Red">*</span>计调员：<asp:DropDownList ID="ddl_Oprator" runat="server" CssClass="Oprator">
     <asp:ListItem Value="0">请选择</asp:ListItem></asp:DropDownList>
     <asp:LinkButton ID="LinkButton3" style="text-align:center; margin-left:80px;" runat="server" OnClientClick="return Quote.QuoteCheckFrom();" onclick="LinkButton2_Click">保存</asp:LinkButton>     
  </div>    
  
</div>
<script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
  <script type="text/javascript">
      var bool = false;
      //选择组团社
      function SetpIdAndName(cid, cname, cContacter, cTel) {
          document.getElementById("<%=hidCustId.ClientID %>").value = cid;
          document.getElementById("<%=Txt_Inquiry.ClientID %>").value = cname;
          $("#<%=Txt_Contact.ClientID %>").val(cContacter);
          $("#<%=Txt_TelPhone.ClientID %>").val(cTel);
      }
      var Quote = {
          //选择组团用户
          selCust: function() {
              parent.Boxy.iframeDialog({ title: "选择客户", iframeUrl: "/CRM/customerservice/SelCustomer.aspx",
                  width: 800, height: 400, model: true, data: {
                      backFun: "SetpIdAndName", desid: ""
                  }
              });
          },
          Ondel: function(majorid) {
              $.newAjax({
                  type: "POST",
                  url: "/xianlu/Quote.aspx?action=delete&Majorid=" + majorid,
                  cache: false,
                  data: $("form").serializeArray(),
                  success: function(msg) {
                      if (msg == "1") {
                          alert("删除成功！");
                          var _href = window.location.href;
                          window.location.href = _href;
                      } else {
                          alert("删除失败！");
                      }
                  }
              });
          },
          //弹出对话框
          openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
              Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
          },
          CheckForm: function() {
              var result = true;
              //验证组团社
              var Inquiry = $("#<%=Txt_Inquiry.ClientID %>").val();
              if (Inquiry == "") {
                  alert("请选择询价单位!");
                  result = false;
                  return false;
              };
              //验证出团时间
              var rdate = $("#<%=Txt_GroupStarTime.ClientID %>").val();
              if (rdate == "") {
                  alert("请选择出团时间!");
                  result = false;
                  return false;
              };

              //验证输入的人数是否是大于0的整数
              var pNumber = $("#<%=Txt_Numbers.ClientID %>").val();
              //判断正整数
              var re = /^[1-9]+[0-9]*]*$/;
              if (!re.test(pNumber)) {
                  alert("请输入人数!");                  
                  result = false;
                  return false;
              }
              return result;
          },
          //报价验证
          quoteSucess: function() {
              if (this.CheckForm()) {
                  var TourCodehtml = $("#div1").html();
                  var boxy = new Boxy("<p style='width:250px;height:100px;margin:25px 25px 25px 25px;' class='tjbtn02'>" + TourCodehtml + "</p>", { title: "团号", modal: true });
                  boxy.boxy.find("#<%=TourCode.ClientID %>").attr("name", "TourCode");
                  boxy.boxy.find("#<%=ddl_Oprator.ClientID %>").attr("name", "ddl_Oprator");
                  $("[name='ddl_Oprator']").change(function() {
                      $("#<%= ddl_Oprator.ClientID %>").val($(this).val())
                  });
              }
              return false;
          },
          //报价成功验证
          QuoteCheckFrom: function() {
              var tourCode = $.trim($("input[name='TourCode']").val());
              //团号验证
              if (tourCode != "") {
                  Quote.AjaxCheckTeamNum(); //验证输入的团号是否重复
                  if (bool) {
                      $("#<%=TourCode.ClientID %>").val(tourCode);
                      return true;
                  }
                  else {
                      alert("该团号已经存在!")
                      return false;
                  }
              }
              else {
                  alert("请填写团号!");
                  return false;
              }

              //计调员选择
              if ($.trim($("#<%=ddl_Oprator.ClientID %>").val()) == "") {
                  alert("请选择计调员!"); return false;
              } else {
                  $("#<%=ddl_Oprator.ClientID %>").val($.trim($("input[name='ddl_Oprator']").val()));
              }
              return true;
          },
          AjaxCheckTeamNum: function() {
              $("#<%=TourCode.ClientID %>").val($.trim($("input[name='TourCode']").val()));
              var teamNum = $("#<%= TourCode.ClientID %>").val();
              $.newAjax({
                  type: "Get",
                  async: false,
                  url: "/TeamPlan/AjaxFastVersion.ashx?type=CheckTeamNum&teamNum=" + teamNum,
                  cache: false,
                  success: function(result) {
                      if (result == "OK") {
                          bool = true;
                      }
                  }
              });
          }
      };

      window.onload = function() {
          //删除事件
          $("a[majorid]").click(function() {
              if (confirm("你确定要删除此条信息吗?")) {
                  var majorid = $(this).attr("majorid");
                  Quote.Ondel(majorid);
              }
              return false;
          });

          //查看
          $(".addlinebox").find("a[name='QouteInfo']").click(function() {
              var QuoteID = $(this).attr("ref");
              Quote.openDialog("/xianlu/QuoteInfo.aspx", "报价详细信息", "950", "250", "QuoteID=" + QuoteID);
              return false;
          });

          $("#<%=Txt_Numbers.ClientID %>").blur(function() {
              var val = parseInt($(this).val());
              if (val > 0) {

              } else {
                  val = 0;
              }

              $("#tblPrice").find("input[name='oneDjCount']").each(function() {
                  $(this).val(val);
              })
          })
      }
    </script>
</form>    
</asp:Content>
