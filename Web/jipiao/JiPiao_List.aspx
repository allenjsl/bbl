<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="JiPiao_List.aspx.cs" Inherits="Web.jipiao.JiPiaoList" Title="��Ʊ����" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .FlightTD
        {
            border-top: solid 1px #fff;
            border-right: solid 1px #fff;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td nowrap="nowrap" width="15%">
                                <span class="lineprotitle">��Ʊ����</span>
                            </td>
                            <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                                ����λ��&gt;&gt; ��Ʊ����
                            </td>
                        </tr>
                        <tr>
                            <td height="2" bgcolor="#000000" colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="hr_10">
            </div>
            <table cellspacing="0" cellpadding="0" border="0" align="center" width="99%">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="/images/yuanleft.gif">
                        </td>
                        <td>
                            <div class="searchbox">
                                <label>
                                    �źţ�</label><input type="text" name="txt_tourCode" id="txt_tourCode" class="searchinput"
                                        style="width: 150px;" />
                                <label>
                                    �Ƶ�Ա��</label>
                                <uc1:selectOperator ID="selectOperator1" runat="server" />
                                <label>
                                    �����/ʱ�䣺</label>
                                <input type="text" class="searchinput" id="StartDate" name="textfield1" class="Wdate">
                                <label>
                                    ���Σ�</label>
                                <input type="text" id="inputGoLine" class="searchinput" name="textfield2" width="50px">
                                <label>
                                    �������ڣ�</label><input type="text" id="txtTimeStart" runat="server" class="searchinput"
                                        name="textfield2" onfocus="WdatePicker()" style="width: 80px;">��<input type="text"
                                            runat="server" id="txtTimeEnd" class="searchinput" onfocus="WdatePicker()" name="textfield2"
                                            style="width: 80px;">
                                <br />
                                <label>
                                    ����ʱ�䣺</label>
                                <input type="text" onfocus="WdatePicker()" id="txtLeaveSDate" class="searchinput"
                                    name="txtLeaveSDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("lsdate") %>" />-<input
                                        type="text" onfocus="WdatePicker()" id="txtLeaveEDate" class="searchinput" name="txtLeaveEDate"
                                        value="<%=EyouSoft.Common.Utils.GetQueryStringValue("ledate") %>" />
                                <label>
                                    ״̬��</label>
                                <asp:DropDownList ID="dpTicketState" runat="server">
                                    <asp:ListItem Value="0">��ѡ��</asp:ListItem>
                                    <asp:ListItem Value="1">��Ʊ����</asp:ListItem>
                                    <asp:ListItem Value="2">���ͨ��</asp:ListItem>
                                    <asp:ListItem Value="3">�ѳ�Ʊ</asp:ListItem>
                                    <asp:ListItem Value="4">����Ʊ</asp:ListItem>
                                </asp:DropDownList>
                                <label>
                                    <a href="javascript:void(0)" id="btnSearch">
                                        <img style="vertical-align: top;" src="/images/searchbtn.gif"></a></label>
                            </div>
                        </td>
                        <td width="10" valign="top">
                            <img src="/images/yuanright.gif">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="btnbox">
                <table cellspacing="0" cellpadding="0" border="0" align="left" width="40%">
                    <tbody>
                        <tr>
                            <td align="center" width="90">
                                <asp:HyperLink ID="hypJiPiao" runat="server" Target="_blank">�������뵥</asp:HyperLink>
                            </td>
                            <td align="center" width="90">
                                <%if (false)
                                  {
                                %>
                                <a id="jiPiaoChuStats" href="javascript:" gourl="JiPiao_tongji.aspx?sign=1">��Ʊͳ��</a>
                                <%
                                    }%>
                            </td>
                            <td align="center" width="90">
                                <%if (false)
                                  {
                                %>
                                <a id="jiPiaoTuiStats" href="javascript:" gourl="JiPiao_tongji.aspx?sign=2">��Ʊͳ��</a>
                                <%
                                    }%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tablelist">
                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <th height="30" bgcolor="#bddcf4" align="center" width="5%">
                                ���
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="13%">
                                �ź�
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                ������
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="8%">
                                �ͻ���λ
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="14%">
                                ��·����
                            </th>
                            <th bgcolor="#bddcf4" align="center">
                                ��Ʊ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                ����Ա
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                �Ƶ�Ա
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="110px">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="100px">
                                ����
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="6%">
                                ״̬
                            </th>
                            <th bgcolor="#bddcf4" align="center" width="10%">
                                ����
                            </th>
                        </tr>
                        <cc1:CustomRepeater ID="repList" runat="server" OnItemDataBound="repList_ItemDataBound">
                            <ItemTemplate>
                                <%#GetColor(Eval("State").ToString())%>
                                <td height="30" align="center">
                                    <%# Container.ItemIndex+1 %>
                                </td>
                                <td align="center">
                                    <%#Eval("TourNum")%>
                                </td>
                                <td align="center">
                                    <%#Eval("OrderNo")%>
                                </td>
                                <td align="center">
                                    <%#Eval("BuyCompanyName")%>
                                </td>
                                <td align="center">
                                    <%#Eval("RouteName")%>
                                </td>
                                <td align="center">
                                    <%#EyouSoft.Common.Utils.GetDateTime(Eval("TicketOutTime").ToString()) == DateTime.MinValue ? "" : Eval("TicketOutTime")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Saler")%>
                                </td>
                                <td align="center">
                                    <%#Eval("Operator")%>
                                </td>
                                <td align="center" colspan="2" style="padding: 0px; margin: 0px;">
                                    <table width="100%" style="border: medium none; border-collapse: collapse; height: 70px;" class="tbl">
                                        <%#GetHtmlByList(Eval("TicketFlights"), EyouSoft.Common.Utils.GetDateTime(Eval("RegisterTime").ToString()))%>
                                    </table>
                                </td>
                                <td align="center">
                                    <%#GetTicketState(Eval("TicketType"),Eval("State"))%>
                                </td>
                                <td align="center" class="jipiao_turn">
                                    <%if (CheckGrant(global::Common.Enum.TravelPermission.��Ʊ����_��Ʊ����_��Ʊ����))
                                      {
                                    %>
                                    <a onclick="JiPiaoList.ChuPiao(this)" id="ChuPiao" visible="false" href="javascript:"
                                        runat="server">��Ʊ</a>
                                    <%
                                        }%>
                                    <%if (CheckGrant(global::Common.Enum.TravelPermission.��Ʊ����_��Ʊ����_��Ʊ����))
                                      {
                                    %>
                                    <a onclick="JiPiaoList.TuiPiao(this)" id="TuiPiao" visible="false" href="javascript:"
                                        runat="server">��Ʊ</a>
                                    <% } %>
                                    <%if (CheckGrant(global::Common.Enum.TravelPermission.��Ʊ����_��Ʊ����_��Ŀ))
                                      {
                                    %>
                                    <a onclick="JiPiaoList.Show(this)" id="Show" href="javascript:void(0)" runat="server">
                                        �鿴</a>
                                    <% } %>
                                </td>
                                </tr>
                            </ItemTemplate>
                        </cc1:CustomRepeater>
                        <tr>
                            <td height="30" align="right" class="pageup" colspan="9">
                                <uc2:ExportPageInfo ID="ExporPageInfoSelect1" CurrencyPageCssClass="RedFnt" LinkType="4"
                                    runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <script type="text/javascript">
     var JiPiaoList={
     
            GetSelectCount: function() {
                var count = 0;
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")==true) {
                        count++;
                    }
                });
                return count;
            },
            
            GetSelectItemValue: function() {
                var arrayList = new Array();
                $(".tablelist").find("input[type='checkbox']").each(function() {
                    if ($(this).attr("checked")==true) {
                        arrayList.push($(this).attr("id"));
                    }
                });
                return arrayList;
            },
            
            ChuPiao:function(obj){
                var url=$(obj).attr("goUrl");
                JiPiaoList.openXLwindow(url,'��Ʊ','900px','480px');
                return false;
            },
            
            TuiPiao:function(obj){
                var url=$(obj).attr("goUrl");
                JiPiaoList.openXLwindow(url,'��Ʊ','900px','480px');
                return false;
            },
            
              Show: function(obj) {
                var url = $(obj).attr("goUrl");
                JiPiaoList.openXLwindow(url, '�鿴', '900px', '580px');
                return false;
            },
            
         openXLwindow:function(url,title,width,height){
            url=url;
        	Boxy.iframeDialog({
			iframeUrl:url,
			title:title,
			modal:true,
			width:width,
			height:height
		});
      }
    }
    
    $(document).ready(function() {
            $(".tbl").each(function() {
                var height = $(this).parent("td").parent("tr");
                $(this).height(height.height() + 10);
            })
    $("#jiPiaoChuStats,#jiPiaoTuiStats").click(function(){
        var url=$(this).attr("goUrl");
         var title=$(this).text();
        JiPiaoList.openXLwindow(url,title,'880px','680px');
        return false;
    });
 
    $("#btnSearch").click(function(){
    if(!checkTime())
    {
        return;
    }
     var data={
       inputDate:$("#StartDate").val(),
       inputGoLine:$("#inputGoLine").val(),
       oper:<%=selectOperator1.ClientID%>.GetOperatorId(),
       operName:<%=selectOperator1.ClientID%>.GetOperatorName(),
       timeStart:$("#<%=txtTimeStart.ClientID %>").val(),
       timeEnd:$("#<%=txtTimeEnd.ClientID %>").val(),
       ticketState:$("#<%=dpTicketState.ClientID %>").val(),
       tourCode:$("#txt_tourCode").val(),
       lsdate:$("#txtLeaveSDate").val(),
       ledate:$("#txtLeaveEDate").val()
     };
     window.location.href="/jipiao/JiPiao_List.aspx?"+$.param(data);
     return false;
    });
    
     $(function(){
     if(queryString("inputDate")!="" && queryString("inputDate")!=null){ 
       $("#StartDate").val(queryString("inputDate"));
       }
       if(queryString("inputGoLine")!="" && queryString("inputGoLine")!=null){ 
       $("#inputGoLine").val(decodeURIComponent(queryString("inputGoLine")));
       }
     if(queryString("tourCode")!="" && queryString("tourCode")!=null){ 
       $("#txt_tourCode").val(queryString("tourCode"));
       }
     });
    
   $("#<%=this.form1.ClientID%>").keydown(function(event) {
      if (event.keyCode == 13) {
          $("#btnSearch").click();
          return false;
       }
     }); 
     
     });
     //�������ʱ��
     function checkTime()
     {
        var timeStart=$("#<%=txtTimeStart.ClientID %>").val();
        var timeEnd=$("#<%=txtTimeEnd.ClientID %>").val();
        if(timeStart>timeEnd)
        {
            alert("��ʼ���ڲ��ܴ��ڽ�ֹ����");
            return false;
        }else
        {
            return true;
        }
     }
    </script>

    </form>
</asp:Content>
