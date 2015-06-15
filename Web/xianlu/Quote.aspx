<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Quote.aspx.cs" Inherits="Web.xianlu.Quote"  MasterPageFile="~/masterpage/Back.Master" Title="��Ҫ����_��·��_��·��Ʒ��"%>

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
               <span class="lineprotitle">��·��Ʒ��</span>
             </td>
             <td width="85%" nowrap="nowrap" align="right">
               ����λ��&gt;&gt; ��·��Ʒ��
             </td>
           </tr>
        </tbody>       
     </table>
  </div>
  <div class="hr_10"></div>
  <div class="addlinebox">
     <fieldset style="border: 1px solid rgb(189, 220, 244); padding: 5px 0pt;">
        <legend>�б�</legend>
        <table width="98%"  cellspacing="1" cellpadding="0" border="0" align="center">
          <tbody>
            <cc1:CustomRepeater ID="Ctp_QuoteList" runat="server">
              <HeaderTemplate>
                  <tr class="odd">
                    <th width="16%" height="30" align="center">ѯ�۵�λ</th>
                    <th width="16%" align="center">��ϵ��</th>
                    <th width="16%" align="center">��������</th>
                    <th width="16%" align="center">����</th>
                    <th width="16%" align="center">���籨��</th>
                    <th width="16%" align="center">����</th>
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
                           <a href="Javascript:void(0);" name="QouteInfo" ref="<%#Eval("QuoteId") %>"><font class="fblue">�鿴</font></a>
                           <a href="/xianlu/Quote.aspx?action=update&areaid=<%=Request.QueryString["areaid"] %>&QuoteID=<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>&RouteId=<%# Eval("RouteId") %>"><font class="fblue">�޸�</font></a>
                           <a href="Javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>"><font class="fblue">ɾ��</font></a>
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
                           <a href="Javascript:void(0);" name="QouteInfo" ref="<%#Eval("QuoteId") %>"><font class="fblue">�鿴</font></a>
                           <a href="/xianlu/Quote.aspx?action=update&areaid=<%=Request.QueryString["areaid"] %>&QuoteID=<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>&RouteId=<%# Eval("RouteId") %>"><font class="fblue">�޸�</font></a>
                           <a href="Javascript:void(0);" majorid="<%# DataBinder.Eval(Container.DataItem,"QuoteId")%>"><font class="fblue">ɾ��</font></a>
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
       <legend>��Ҫ����</legend>
        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
          <tbody>
             <tr class="odd">
                <th height="25" align="center" width="10%"><font style="color:Red;">*</font>ѯ�۵�λ��</th>
                <td align="left" width="15%">
                    <input type="text" name="Txt_Inquiry" id="Txt_Inquiry" class="widowkuangtext"  runat="server"  readonly="readonly" style="background-color:#dadada"/>
                    <input type="hidden" id="hidCustId" name="hidCustId" runat="server" />
                    <a href="javascript:void(0);" onclick="return Quote.selCust();"><img src="/images/sanping_04.gif" alt=""></a>           
                </td>
                <th align="center" width="10%">��ϵ�ˣ�</th>
                <td  align="left" width="15%">
                    <input type="text" name="Txt_Contact" id="Txt_Contact" class="widowkuangtext"  runat="server"/>
                </td>
                <th  align="center" width="10%">��ϵ�绰�� </th>
                <td  align="left" width="15%">
                    <input type="text" name="Txt_TelPhone" id="Txt_TelPhone" class="widowkuangtext"  runat="server"/>
                </td>
            </tr>
            <tr class="even">
                <th height="25" align="center"><font style="color:Red;">*</font>Ԥ�Ƴ���ʱ�䣺</th>
                <td align="left">
                   <input name="Txt_GroupStarTime" type="text" class="searchinput" id="Txt_GroupStarTime"  runat="server" onfocus="WdatePicker();"/>                           
                </td>
                <th align="center"><font style="color:Red;">*</font>������</th>
                <td colspan="3" align="left">
                    <input type="text" name="Txt_Numbers" id="Txt_Numbers" class="widowkuangtext"  runat="server" />
                </td>
            </tr>
           <tr class="odd">
                <th align="center" colspan="2">��Ŀ</th>
                <th align="center" colspan="2">����Ҫ��</th>
                <th align="center" colspan="2"><span style="margin-left:160px">����</span></th>
            </tr>
            <tr class="even">
                <th height="25" align="center" bgcolor="#E3F1FC">����Ҫ��</th>
                      <td colspan="5" align="left" bgcolor="#BDDCF4"> 
                      <uc2:ProjectControl1  ID="ProjectControl" runat="server"/>
               </td>
             </tr>  
             <tr class="odd">
                <th height="25" align="center">�۸���ɣ�</th>
                <td colspan="5" align="left"><Uc3:PriceControl ID="PriceControl1" runat="server"/></td>               
            </tr>
            <tr class="even">
                <td height="25" align="center">
                    ��ע��
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
                                <asp:LinkButton ID="LinkButton1" runat="server"  OnClientClick="return Quote.CheckForm();" onclick="LinkButton1_Click">����</asp:LinkButton></td>
                           <td height="40" align="center" class="tjbtn02">
                                <asp:LinkButton ID="LinkButton2" runat="server"  
                                    OnClientClick="return Quote.quoteSucess();" Height="31px">���۳ɹ�</asp:LinkButton></td>
                            <td align="center" class="tjbtn02">
                                <a href="/xianlu/LineProducts.aspx";>����</a>
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
    <span style="color:Red">*</span>����д�źţ�<input type="text"  id="TourCode" runat="server"  /><br />
    <span style="color:Red">*</span>�Ƶ�Ա��<asp:DropDownList ID="ddl_Oprator" runat="server" CssClass="Oprator">
     <asp:ListItem Value="0">��ѡ��</asp:ListItem></asp:DropDownList>
     <asp:LinkButton ID="LinkButton3" style="text-align:center; margin-left:80px;" runat="server" OnClientClick="return Quote.QuoteCheckFrom();" onclick="LinkButton2_Click">����</asp:LinkButton>     
  </div>    
  
</div>
<script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>
<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
  <script type="text/javascript">
      var bool = false;
      //ѡ��������
      function SetpIdAndName(cid, cname, cContacter, cTel) {
          document.getElementById("<%=hidCustId.ClientID %>").value = cid;
          document.getElementById("<%=Txt_Inquiry.ClientID %>").value = cname;
          $("#<%=Txt_Contact.ClientID %>").val(cContacter);
          $("#<%=Txt_TelPhone.ClientID %>").val(cTel);
      }
      var Quote = {
          //ѡ�������û�
          selCust: function() {
              parent.Boxy.iframeDialog({ title: "ѡ��ͻ�", iframeUrl: "/CRM/customerservice/SelCustomer.aspx",
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
                          alert("ɾ���ɹ���");
                          var _href = window.location.href;
                          window.location.href = _href;
                      } else {
                          alert("ɾ��ʧ�ܣ�");
                      }
                  }
              });
          },
          //�����Ի���
          openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
              Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
          },
          CheckForm: function() {
              var result = true;
              //��֤������
              var Inquiry = $("#<%=Txt_Inquiry.ClientID %>").val();
              if (Inquiry == "") {
                  alert("��ѡ��ѯ�۵�λ!");
                  result = false;
                  return false;
              };
              //��֤����ʱ��
              var rdate = $("#<%=Txt_GroupStarTime.ClientID %>").val();
              if (rdate == "") {
                  alert("��ѡ�����ʱ��!");
                  result = false;
                  return false;
              };

              //��֤����������Ƿ��Ǵ���0������
              var pNumber = $("#<%=Txt_Numbers.ClientID %>").val();
              //�ж�������
              var re = /^[1-9]+[0-9]*]*$/;
              if (!re.test(pNumber)) {
                  alert("����������!");                  
                  result = false;
                  return false;
              }
              return result;
          },
          //������֤
          quoteSucess: function() {
              if (this.CheckForm()) {
                  var TourCodehtml = $("#div1").html();
                  var boxy = new Boxy("<p style='width:250px;height:100px;margin:25px 25px 25px 25px;' class='tjbtn02'>" + TourCodehtml + "</p>", { title: "�ź�", modal: true });
                  boxy.boxy.find("#<%=TourCode.ClientID %>").attr("name", "TourCode");
                  boxy.boxy.find("#<%=ddl_Oprator.ClientID %>").attr("name", "ddl_Oprator");
                  $("[name='ddl_Oprator']").change(function() {
                      $("#<%= ddl_Oprator.ClientID %>").val($(this).val())
                  });
              }
              return false;
          },
          //���۳ɹ���֤
          QuoteCheckFrom: function() {
              var tourCode = $.trim($("input[name='TourCode']").val());
              //�ź���֤
              if (tourCode != "") {
                  Quote.AjaxCheckTeamNum(); //��֤������ź��Ƿ��ظ�
                  if (bool) {
                      $("#<%=TourCode.ClientID %>").val(tourCode);
                      return true;
                  }
                  else {
                      alert("���ź��Ѿ�����!")
                      return false;
                  }
              }
              else {
                  alert("����д�ź�!");
                  return false;
              }

              //�Ƶ�Աѡ��
              if ($.trim($("#<%=ddl_Oprator.ClientID %>").val()) == "") {
                  alert("��ѡ��Ƶ�Ա!"); return false;
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
          //ɾ���¼�
          $("a[majorid]").click(function() {
              if (confirm("��ȷ��Ҫɾ��������Ϣ��?")) {
                  var majorid = $(this).attr("majorid");
                  Quote.Ondel(majorid);
              }
              return false;
          });

          //�鿴
          $(".addlinebox").find("a[name='QouteInfo']").click(function() {
              var QuoteID = $(this).attr("ref");
              Quote.openDialog("/xianlu/QuoteInfo.aspx", "������ϸ��Ϣ", "950", "250", "QuoteID=" + QuoteID);
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
