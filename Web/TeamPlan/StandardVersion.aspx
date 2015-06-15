<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StandardVersion.aspx.cs"
    Inherits="Web.TeamPlan.StandardVersion" MasterPageFile="~/masterpage/Back.Master" %>

<%@ Register Src="/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Src="/UserControl/DiJieControl.ascx" TagName="DiJieControl" TagPrefix="uc2" %>
<%@ Register Src="/UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc4" %>
<%@ Register Src="/UserControl/PriceControl.ascx" TagName="PriceControl" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/selectOperator.ascx" TagName="selectOperator" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>�ŶӼƻ�_��׼�淢��</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form runat="server" enctype="multipart/form-data">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table cellspacing="0" cellpadding="0" border="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">�ŶӼƻ�</span>
                        </td>
                        <td nowrap="nowrap" align="right" width="85%" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            ����λ��&gt;&gt; �ŶӼƻ�&gt;&gt; �ŶӼƻ�
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
        <ul class="fbTab">
            <asp:Panel ID="pnlFast" runat="server">
                <li><a href="FastVersion.aspx">���ٷ���</a> </li>
            </asp:Panel>
            <li><a class="tabtwo-on">��׼�淢��</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox">
            <div class="lineprotitlebox">
                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                    <tbody>
                        <tr>
                            <td align="right" width="11%" style="height: 20px">
                                <font class="xinghao">*</font>�������ڣ�
                            </td>
                            <td align="left" colspan="3" style="width: 260px; height: 25px">
                                <asp:TextBox ID="txtLDate" CssClass="searchinput" runat="server" onfocus="WdatePicker({onpicked:getTourCode})"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="20%" style="*width: 10%">
                                <font class="xinghao">*</font>�źţ�
                            </td>
                            <td width="85%" colspan="2">
                                <asp:TextBox ID="txtTeamNum" runat="server" CssClass="searchinput"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>��·����
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlLineArea" runat="server" CssClass="select">
                                    <asp:ListItem Value="0">--��ѡ��--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>�Ƶ�Ա��
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlSeller" runat="server">
                                    <asp:ListItem Value="-1" Text="--��ѡ��--"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>�����磺
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtGroupsName" runat="server" ReadOnly="true" BackColor="#dadada"></asp:TextBox>
                                <a href="javascript:void(0);" id="a_GetGroups" class="selectTeam">
                                    <img src="../images/sanping_04.gif" width="28px" height="18px" border="0"></a>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>��·���ƣ�
                            </td>
                            <td colspan="2">
                                <uc1:xianluWindow Id="xianluWindow1" runat="server" isRead="true" />
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                <font class="xinghao">*</font>������ͨ��
                            </td>
                            <td align="left">
                                <select id="selectTraffic" name="selectTraffic">
                                    <%=strTraffic %>
                                </select>
                                ʣ���λ��<asp:Label runat="server" ID="lbshengyu"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" width="11%">
                                <font class="xinghao">*</font>������
                            </td>
                            <td colspan="2">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td align="center" width="65">
                                                <asp:TextBox ID="txtDayCount" runat="server" CssClass="searchinput searchinput03"
                                                    na="txtDayCount"></asp:TextBox>
                                            </td>
                                            <td align="left">
                                                <span style="display: <%=NumConfig==0?"block":"none"%>;"><font class="xinghao">*</font>������
                                                    <asp:TextBox ID="txtPeopelCount" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hid_PeopelCount" />
                                                </span><span style="display: <%=NumConfig==1?"block":"none"%>;">���� <font class="xinghao">
                                                    *</font>��������
                                                    <asp:TextBox ID="txt_crNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hid_Num" />
                                                    ��ͯ����
                                                    <asp:TextBox ID="txt_rtNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                    ȫ�㣺
                                                    <asp:TextBox ID="txt_allNum" runat="server" CssClass="searchinput searchinput03"></asp:TextBox>
                                                </span>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                ȥ�̺���/ʱ�䣺
                            </td>
                            <td align="left" colspan="2">
                                <asp:TextBox ID="txtStartTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                                &nbsp; �س̺���/ʱ�䣺
                                <asp:TextBox ID="txtEndTraffic" runat="server" CssClass="searchinput searchinput02"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                ����ʱ�䣺
                            </td>
                            <td align="left" colspan="2">
                                <%--<asp:TextBox ID="txtGathered" CssClass="searchinput" runat="server" onfocus="WdatePicker()"></asp:TextBox><asp:DropDownList
                                    ID="ddl_jh_date" runat="server" DataTextField="Text" DataValueField="Value">
                                    <asp:ListItem>0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                    <asp:ListItem>5</asp:ListItem>
                                    <asp:ListItem>6</asp:ListItem>
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>8</asp:ListItem>
                                    <asp:ListItem>9</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>11</asp:ListItem>
                                    <asp:ListItem>12</asp:ListItem>
                                    <asp:ListItem>13</asp:ListItem>
                                    <asp:ListItem>14</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>16</asp:ListItem>
                                    <asp:ListItem>17</asp:ListItem>
                                    <asp:ListItem>18</asp:ListItem>
                                    <asp:ListItem>19</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>22</asp:ListItem>
                                    <asp:ListItem>23</asp:ListItem>
                                </asp:DropDownList>
                                �� --%>
                                <asp:TextBox runat="server" ID="txtGathered" CssClass="searchinput" Style="width: 150px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;���ϵص㣺
                                <asp:TextBox ID="txtPlace" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                ���ϱ�־��
                            </td>
                            <td align="left" colspan="2">
                                &nbsp;<asp:TextBox ID="txtLogo" CssClass="searchinput searchinput02" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;
                                <uc5:selectOperator ID="selectOperator1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right">
                                ���۳��У�
                            </td>
                            <td align="left" colspan="2">
                                <asp:DropDownList ID="ddlCityList" runat="server" CssClass="select">
                                    <asp:ListItem Value="0">--��ѡ��--</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right">
                                �ؽ�����Ϣ��
                            </td>
                            <td align="left" colspan="2">
                                <uc2:DiJieControl ID="DiJieControl1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td height="10" align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td height="10" align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right">
                                <font class="xinghao">*</font>�г̰��ţ�
                            </td>
                            <td align="left" colspan="2">
                                <uc4:xingcheng ID="xingcheng1" runat="server" daysControlName="txtDayCount" />
                            </td>
                        </tr>
                        <tr>
                            <td height="40" background="/images/bg003.gif" align="right">
                                ��Ӹ�����
                            </td>
                            <td background="/images/bg003.gif" width="30%">
                                <input type="file" id="fileUpLoad" name="fileField" />
                            </td>
                            <td background="/images/bg003.gif">
                                <asp:Panel ID="pnlFile" runat="server">
                                    <img src="/images/open-dx.gif" />
                                    <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                                    <a href="javascript:void(0);" onclick="StandardVersion.DeleteFile()">
                                        <img src="/images/fujian_x.gif" /></a>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" colspan="3">
                                <table cellspacing="1" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr bgcolor="#bddcf4">
                                            <th height="25" align="center" colspan="2">
                                            </th>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                ������Ŀ��
                                            </td>
                                            <td>
                                                <uc3:PriceControl ID="PriceControl1" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                ������Ŀ��
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNoProject" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                ���ﰲ�ţ�
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtBuyPlan" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                �Է���Ŀ��
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtSelfProject" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                &nbsp;��ͯ���ţ�
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtChildPlan" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                ע�����
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNote" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#e3f1fc" align="center">
                                                ��ܰ���ѣ�
                                            </td>
                                            <td bgcolor="#e3f1fc" align="left">
                                                <asp:TextBox ID="txtTips" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="60" bgcolor="#bddcf4" align="center">
                                                �ڲ���Ϣ��
                                            </td>
                                            <td bgcolor="#bddcf4" align="left">
                                                <asp:TextBox ID="txtNeiBu" TextMode="MultiLine" runat="server" CssClass="textareastyle"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="10" align="center" colspan="2">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!--�޼ƻ�����-->
            <asp:Panel ID="pelAdd" runat="server">
                <ul class="tjbtn">
                    <li><a href="javascript:void(0);" id="btnSave">����</a> </li>
                    <li><a href="javascript:void(0);" id="btnBack">����</a>
                        <asp:Button ID="btnSubmit" runat="server" BackColor="Transparent" BorderColor="Transparent"
                            OnClick="btnSubmit_Click" Width="0px" Height="0" /></li>
                    <div class="clearboth">
                    </div>
                </ul>
            </asp:Panel>
        </div>
    </div>
    <asp:HiddenField ID="hideID" runat="server" />
    <asp:HiddenField ID="hideType" runat="server" />
    <asp:HiddenField ID="hideData" runat="server" />
    <asp:HiddenField ID="hideGroupsId" runat="server" />
    <asp:HiddenField ID="hideOldTeamNum" runat="server" />
    <asp:Literal ID="litMsg" runat="server"></asp:Literal>
    <input type="hidden" id="hd_Numconfig" value="<%=NumConfig %>" />

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
     function getTourCode() {
         if ($("#<%=txtLDate.ClientID %>").val() == "") {
            alert("��ѡ���������!");
            $("#<%=txtLDate.ClientID %>").focus();
            return false;
           }
           else {
            $.newAjax({
                type: "POST",
                url: "/ashx/GenerateTourNumbers.ashx?companyId=<%=SiteUserInfo.CompanyID%>",
                async: false,
                data: {
                    datestr: $("#<%=txtLDate.ClientID %>").val()
                },
                success: function(data) {
                    if (data != "") {
                        var resultArr = [];
                        var arr = eval(data);
                        for (var i = 0; i < arr.length; i++) {
                            $("#<%=txtTeamNum.ClientID %>").val(arr[i][1]);
                           }
                      }
                   }
               });
            }
         }
        $(function() {
            //�󶨱����¼�
            $("#btnSave").click(function() {
                if (StandardVersion.CheckForm()) {
                    $("#<%=btnSubmit.ClientID%>").click();
                }
                return false;
            });
            //�����¼�
            $("#btnBack").click(function() {
               window.location.href="/TeamPlan/TeamPlanList.aspx";
               return false;
            });
            //������ 
            $("#a_SendMan").click(function(){
                
            })
             //ѡ��ؽ����¼�
            $("#a_GetGroups").click(function() {
                var iframeId = '<%=Request.QueryString["iframeId"] %>';
                var url = "/CRM/customerservice/SelCustomer.aspx?method=SetGroupsVal";
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "ѡ��������",
                    modal: true,
                    width: "820px",
                    height: "520px",
                    data: {
                        desid: iframeId,
                        backfun: "SetGroupsVal"
                    }
                });
                return false;
            })
                        //ѡ����·����
            $("#<%=ddlLineArea.ClientID %>").change(function() {
                var areaId = $(this).val();
                $("#<%=ddlSeller.ClientID %>").html("<option value='-1'>--��ѡ��--</option>");
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=GetAreaUser&areaId=" + areaId,
                    cache: false,
                    success: function(result) {
                        if (result != "") {
                            var array = result.split("|||");
                            for (var i = 0; i < array.length; i++) {
                                if (array[i] != "") {
                                    var obj = eval('('+array[i]+')');
                                    $("#<%=ddlSeller.ClientID %>").append("<option value='" + obj.uid + "'>" + obj.uName + "</option>");
                                }
                            }
                        }
                    }
                });
            });
            //���ؼ۸�
            if($("#<%=txtLDate.ClientID %>").val()){
                $.newAjax({
                    type: "GET",
                    dataType: "json",
                    url: "/sanping/SanPing_jion.aspx",
                    async: false,
                    data: {
                        act: "getPrice",
                        startDate: $("#<%=txtLDate.ClientID %>").val(),
                        trafficId: $("#selectTraffic").find("option:selected").val()
                    },
                    success: function(result) {
                        $("#selectTraffic").find("option:selected").attr("data-shengyu", result.shengyu);
                        $("#selectTraffic").find("option:selected").attr("data-price", result.result);
                        $("#<%=lbshengyu.ClientID %>").text(result.shengyu);
                    }
                })
            }
            $("#selectTraffic").change(function() {
                var self = $(this), price = $.trim(self.find("option:selected").attr("data-price")),
                    trafficId = self.find("option:selected").val(),
                    startDate = $("#<%=txtLDate.ClientID %>").val();
                if(!startDate){alert("��ѡ���ź�");return false;}
                if (trafficId) {
                    $.newAjax({
                        type: "GET",
                        dataType: "json",
                        url: "/sanping/SanPing_jion.aspx",
                        async: false, //ͬ��ִ��ajax Ĭ��trueΪ�첽����
                        data: { act: "getPrice", startDate: startDate, trafficId: trafficId },
                        success: function(result) {
                            self.find("option:selected").attr("data-shengyu", result.shengyu);
                            self.find("option:selected").attr("data-price", result.result);
                        }
                    })
                }
                $("#<%=lbshengyu.ClientID %>").text(self.find("option:selected").attr("data-shengyu"));
            })
            
        })


        var StandardVersion = {
            CheckForm: function() {
                var b = true;
                //��·����
                var lineArea = $("#<%=ddlLineArea.ClientID %>").val();
                //��·ID
                var xl_ID = $('#<%=xianluWindow1.FindControl("hd_xl_id").ClientID%>').val();
                var xl_Name = $('#<%=xianluWindow1.FindControl("txt_xl_Name").ClientID%>').val();
                var dayCount = $("#<%=txtDayCount.ClientID %>").val();
                var peopelCount=0;
                if( parseInt( $("#hd_Numconfig").val())==0){
                    peopelCount = $("#<%=txtPeopelCount.ClientID %>").val();
                }else{
                    peopelCount=$("#<%=txt_crNum.ClientID %>").val();
                }
                //������
                var groupId =  $.trim($("#<%=hideGroupsId.ClientID %>").val());
                //�ź�
                var teamNum = $.trim($("#<%=txtTeamNum.ClientID %>").val());
                //�Ƶ�Ա
                var settleId = $("#<%=ddlSeller.ClientID %>").val();
                //��������
                var txtLDate = $.trim($("#<%=txtLDate.ClientID %>").val());
                if (lineArea == "0") {
                    alert("��ѡ����·����!");
                    return false;
                }
                 if(settleId=="-1")
                {
                     alert("��ѡ��Ƶ�Ա");
                    return false;
                }
                 //�ж��������Ƿ�ѡ��
                if(groupId=="")
                {
                    alert("��ѡ��������")
                   return false;
                }
                if (xl_ID == "" && xl_Name == "") {
                    alert("��ѡ����·����!");
                    return false;
                }
                
                //�ж��ź��Ƿ�Ϊ��
                if(teamNum=="")
                {
                    alert("�źŲ���Ϊ��!");
                    return false;
                }
                 //�ж��ź��Ƿ����
                if (teamNum != "" && teamNum!=$("#<%=hideOldTeamNum.ClientID %>").val()) {
                    if (!StandardVersion.AjaxCheckTeamNum()) {
                        alert("���ź��Ѿ�����!")
                        return false;
                    }
                }
                if($("#selectTraffic").val()==""){
                    alert("��ѡ�������ͨ!");
                    return false;
                }
                if (dayCount == "") {
                    if (Number(dayCount).toString() == "NaN" || Number(dayCount)<=0) {
                        alert("������ʽ����ȷ!");
                    } else {
                        alert("����������!");
                    }
                    return false;
                }
                if (peopelCount == "") {
                    if (Number(peopelCount).toString() == "NaN") {
                        alert("������ʽ����ȷ!");
                    } else {
                        alert("����������!");
                    }
                    return false;
                }
                if($("#<%=hideType.ClientID %>").val()=="Add"||$("#<%=hideType.ClientID %>").val()=="Copy"){//��ӻ��߸���
                    if( parseInt( $("#hd_Numconfig").val())!=0){
                       var countNum=(parseInt($("#<%=txt_crNum.ClientID %>").val())||0)+(parseInt($("#<%=txt_rtNum.ClientID %>").val())||0)+
                       (parseInt($("#<%=txt_allNum.ClientID %>").val())||0);
                       var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0;
                       if(planNum<countNum){
                        alert("�����������ƻ�������");
                        return false;
                       }
                    }else{
                        var countNum=parseInt($("#<%=txtPeopelCount.ClientID %>").val())||0;
                        var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0;
                        if(planNum<countNum){
                            alert("�����������ƻ�������");
                            return false;
                        }
                    }
                }
                else{//�޸�
                    if( parseInt( $("#hd_Numconfig").val())!=0){
                       var countNum=(parseInt($("#<%=txt_crNum.ClientID %>").val())||0)+(parseInt($("#<%=txt_rtNum.ClientID %>").val())||0)+
                            (parseInt($("#<%=txt_allNum.ClientID %>").val())||0);
                       var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0+(parseInt($("#<%=hid_Num.ClientID %>").val())||0);
                       if(planNum<countNum){
                            alert("�����������ƻ�������");
                            return false;
                        }
                    }else{
                        var countNum=parseInt($("#<%=txtPeopelCount.ClientID %>").val())||0;
                        var planNum=parseInt($("#<%=lbshengyu.ClientID %>").text())||0+(parseInt($("#<%=hid_PeopelCount.ClientID %>").val())||0);
                        if(planNum<countNum){
                            alert("�����������ƻ�������");
                            return false;
                        }
                    }
                }
                
                if(txtLDate=="")
                {
                    alert("�������������!");
                    return false;
                }
                return true;
            },
           AjaxGetLineInfo: function() {
                var xl_ID = <%=xianluWindow1.ClientID%>.Id();
                $.newAjax({
                    type: "Get",
                    url: "/TeamPlan/AjaxFastVersion.ashx?type=Standard&id=" + xl_ID,
                    cache: false,
                    success: function(result) {
                        if (result != "") {
                            var json = eval('(' + result + ')');
                            //����
                            $("#<%=txtDayCount.ClientID %>").val(json.dayCount);
                            //�г̰���
                            <%=xingcheng1.ClientID %>.bind(json.travel);

                            //KE.html("",json.travel);
                            //�ļ��ϴ�
                            //$("#fileUpLoad").val(json.filePath);
                            //������Ŀ
                           // ConProject.SetRowValue(json.project);
                            //��������Ŀ
                            
                            $("#<%=txtNoProject.ClientID %>").val(json.noPro.toString().replace(/##n##/g,"\n"));
                            //���ﰲ��
                            $("#<%=txtBuyPlan.ClientID %>").val(json.buyPlan.toString().replace(/##n##/g,"\n"));
                            //��ͯ����
                            $("#<%=txtChildPlan.ClientID %>").val(json.childPlan.toString().replace(/##n##/g,"\n"));
                            //�Է���Ŀ
                            $("#<%=txtSelfProject.ClientID %>").val(json.selfPro.toString().replace(/##n##/g,"\n"));
                            //ע������
                            $("#<%=txtNote.ClientID %>").val(json.note.toString().replace(/##n##/g,"\n"));
                            //��ܰ��ʾ
                            $("#<%=txtTips.ClientID %>").val(json.tips.toString().replace(/##n##/g,"\n"));
                        }
                    }
                });
            },
            DeleteFile:function(){
                if(confirm("ȷ��ɾ��?"))
                {
                    $("#<%=pnlFile.ClientID %>").html("");
                    $("#<%=hideData.ClientID %>").val("");
                }
            }, AjaxCheckTeamNum: function() {
                var bool = false;
                var teamNum = $("#<%=txtTeamNum.ClientID %>").val();
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
                return bool;
            }
        }
         function SetGroupsVal(val, txt) {
                $("#<%=hideGroupsId.ClientID %>").val(val);
                $("#<%=txtGroupsName.ClientID %>").val(txt);
            }
       
    </script>

    </form>
</asp:Content>
