<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineProductsList.aspx.cs"
    Inherits="Web.GroupEnd.LineProductsList" MasterPageFile="~/masterpage/Front.Master"
    Title="ɢ�ͱ���" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc11" %>
<%@ Register Src="~/UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
    <style type="text/css">
        .style9
        {
            width: 150px;
            height: 80px;
            margin: 0px auto;
            margin-bottom: 20px;
            border: 1px solid #BFD1EB;
            background-color: #F3FAFF;
        }
        #provinceList li
        {
            list-style: none;
            float: left;
            white-space: nowrap;
        }
        #cityList li
        {
            list-style: none;
            float: left;
            white-space: nowrap;
        }
        .tbtn .selector
        {
            position: relative;
        }
        .a_city input
        {
            border: 1px solid #A2C8D2;
        }
        #st_province{font-size:14px;}
    </style>

    <script src="/js/tourdate.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
    <form id="form1" runat="server">

    <script type="text/javascript" src="/js/jquery.tipsy.js"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">ɢ�ͱ���</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>> ɢ�ͱ���
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="tbtn">
            <div style="float: left; padding-top: 5px;">
                <input type="hidden" name="hd_areaId" id="hd_areaId" value="<%=Request.QueryString["areaId"] %>" />
                ��ǰΪ<input type="hidden" name="hd_province" id="hd_province" value="<%=Request.QueryString["ProvinceId"] %>" /><input
                    type="hidden" name="hd_city" id="hd_city" value="<%=Request.QueryString["cityId"] %>" />
                <span class="selector"><a href="javascript:void(0)" onclick="return false;"><strong
                    id="st_province">
                    <asp:Literal ID="lt_province" runat="server">ʡ��</asp:Literal></strong> </a>
                    <img src="/images/icodown.gif">
                    <div class="tip" style="display: none; border: 1px solid #5794C3; line-height: 1.7;
                        position: absolute; background: #fff; width: 310px; top: 13px; *top: 15px; left: 0px;">
                        <table border="0" width="100%" cellspacing="0">
                            <tr>
                                <td width="100%" align="left" style="padding: 5px;">
                                    <table width="100%" cellspacing="0">
                                        <tr>
                                            <td bgcolor="#D9E7F1">
                                                ��ѡ��ʡ��
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="provinceList">
                                                ����
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </span>
            </div>
            <span>
                <div width="100%">
                    <div class="tip" style="display: block; border: 0px solid #5794C3; float: left; background: #fff;
                        left: 0px;">
                        <table border="0" width="100%" cellspacing="0">
                            <tr>
                                <td width="100%" align="left">
                                    <table width="100%" border="0" cellspacing="0">
                                        <tr>
                                            <td id="cityList">
                                                <%=cityState==""?"��ѡ��ʡ��":"" %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </span>
        </div>
        <div class="lineCategorybox">
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        </div>
        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" id="LineListSearch">
            <tr>
                <td width="10" valign="top">
                    <img src="/images/yuanleft.gif" alt="" />
                </td>
                <td>
                    <div class="searchbox">
                        <label>
                            ��·����</label>
                        <asp:DropDownList ID="DDl_LineType" runat="server">
                            <asp:ListItem Text="--��ѡ����·����--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <label>
                            ��·���ƣ�</label>
                        <input type="text" runat="server" name="xl_XianlName" id="xl_XianlName" class="searchinput searchinput02"
                            style="width: 200px;" />
                        <label>
                            �������ڣ�</label>
                        <input name="BeginTime" type="text" class="searchinput" id="BeginTime" onfocus="WdatePicker();"
                            runat="server" />
                        ��
                        <input type="text" name="EndTime" id="EndTime" class="searchinput" onfocus="WdatePicker();"
                            runat="server" />
                        <label>
                            <img src="/images/searchbtn.gif" style="vertical-align: top; cursor: pointer;" id="Btn_Serach"
                                alt="��ѯ" /></label>
                    </div>
                </td>
                <td width="10" valign="top">
                    <img src="/images/yuanright.gif" alt="" />
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        <div class="tablelist" style="border-top: 2px #000000 solid;">
            <table width="100%" border="0" cellpadding="0" cellspacing="1" id="LineProductList"
                name="LineProductList">
                <tr>
                    <th width="9%" align="center" bgcolor="#BDDCF4">
                        �ź�
                    </th>
                    <th width="15%" align="center" bgcolor="#BDDCF4">
                        ��·����
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        ��������
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        ���ۼ�
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        �����
                    </th>
                    <th width="8%" align="center" bgcolor="#bddcf4">
                        ʣ������
                    </th>
                    <th width="9%" align="center" bgcolor="#bddcf4">
                        ����
                    </th>
                </tr>
                <cc2:CustomRepeater ID="LineProductLists" runat="server" EmptyText="<tr><td colspan='9' align='center'>��������</td></tr>" OnItemDataBound="rtpTours_ItemDataBound">
                    <ItemTemplate>
                        <tr bgcolor="<%#(Container.ItemIndex+1)%2==0?"#BDDCF4":"#E3F1FC" %>">
                            <td align="center">
                                <%# Eval("TourCode")%>
                            </td>
                            <td>
                                <%#Eval("TourRouteStatus").ToString().Trim() != "��" ? Eval("TourRouteStatus").ToString().Trim() == "�ؼ�" ? "<span class=\"state1\">" + Eval("TourRouteStatus") + "</span>" : "<span class=\"state2\">" + Eval("TourRouteStatus") + "</span>" : ""%>
                                <a target="_blank" id="<%#Eval("tourId") %>" contactname="<%#Eval("ContacterName") %>"
                                    contacttel="<%#Eval("ContacterTelephone") %>" contactqq="<%#Eval("ContacterQQ") %>"
                                    css="fly_div" href="<%# ReturnUrl(Eval("ReleaseType").ToString())%>?tourId=<%#Eval("tourId") %>"
                                    onmouseover="viwContact(this,event)" onmouseout="closeContact()">
                                    <%# Eval("RouteName")%>
                                </a>
                            </td>
                            <td align="center">
                                <asp:Literal runat="server" ID="ltrLDate"></asp:Literal>
                            </td>
                            <td align="center">
                                <font class="fred">��<%# GetAdultPrice(Eval("TourId").ToString())%></font>
                            </td>
                            <td align="center">
                                <font class="fred">��<%# GetChildrenPrice(Eval("TourId").ToString())%></font>
                            </td>
                            <td align="center" class="talbe_btn">
                                <%#DisponseLastPeople(Convert.ToInt32(Eval("RemainPeopleNumber")))%><input type="hidden"
                                    value="<%#Eval("RemainPeopleNumber") %>" id="lastPeople_<%#Eval("TourId") %>" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td align="center">
                                <%#Eval("HandStatus").ToString().Trim() == "��" ? "<a href=\"javascript:viod(0);\" name=\"Result\" result=\"C\" ref=" + Eval("TourId") + "><font class=\"fblue\">����</font></a>" : Eval("HandStatus").ToString().Trim() == "�ֶ�ͣ��" ? "<a class=\"tings\">ͣ��</a>" : "<a class=\"manke\">����</a>"%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </cc2:CustomRepeater>
            </table>
        </div>
        <table width="100%">
            <tr>
                <td height="30" colspan="9" align="right" class="pageup">
                    <cc11:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div_view" style="display: none" class="style9">
        <table border="0px">
            <tr>
                <td>
                    ��ϵ�ˣ�
                </td>
                <td>
                    <label id="contactName" />
                </td>
            </tr>
            <tr>
                <td>
                    �绰��
                </td>
                <td>
                    <label id="contactTel" />
                </td>
            </tr>
            <tr>
                <td>
                    QQ��
                </td>
                <td>
                    <label id="contactQQ" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        //���������ʾ
        function viwContact(obj, event) {
            var str = obj;
            //ȡ��ID
            var id = str.toString().substring(str.toString().lastIndexOf("=") + 1, str.toString().length);
            //����IDȡֵ
            var name = $("#" + id).attr("contactName");
            var tel = $("#" + id).attr("contactTel");
            var qq = $("#" + id).attr("contactQQ");
            //Ϊ������Ŀؼ���ֵ
            $("#contactName").html(name);
            $("#contactTel").html(tel);
            $("#contactQQ").html(qq);
            //���������������
            $("#div_view").css({ position: "absolute", 'top': event.clientY + document.documentElement.scrollTop, 'left': event.clientX + document.documentElement.scrollLeft + 30, 'z-index': 2 });
            $("#div_view").css("display", "block");
            $("#div_view").css("width", 200);
        }
        //�����������
        function closeContact() {
            $("#div_view").css("display", "none");
        }
        var LineProductList = {
            OnSearch: function() {
                //��·����
                var AreaID = $.trim($("#<%=DDl_LineType.ClientID %>").val());
                //��·����
                var AreaName = $.trim($("#<%=xl_XianlName.ClientID %>").val());
                //���ſ�ʼʱ��
                var StarTime = $.trim($("#<%=BeginTime.ClientID %>").val());
                //���Ž���ʱ��
                var EndTime = $.trim($("#<%=EndTime.ClientID %>").val());
                //����ID
                var cityID=$.trim($("#hd_city").val());
                var provinceID=$.trim($("#hd_province").val());
                var Params = { AreaID: "", AreaName: "", StarTime: "", EndTime: "" ,cityID:"",ProvinceId:""};
                Params.AreaID = AreaID;
                Params.AreaName = AreaName;
                Params.StarTime = StarTime;
                Params.EndTime = EndTime;
                Params.cityID=cityID;
                Params.ProvinceId=provinceID;
                window.location.href = "/GroupEnd/LineList/LineProductsList.aspx?" + $.param(Params);
            },
            //�����Ի���
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }
        };
        $(document).ready(function() {
            //��ѯ��ť
            $("#Btn_Serach").click(function() {
                LineProductList.OnSearch();
                return false;
            });
            //�س���ѯ
            $("#LineListSearch input").bind("keypress", function(e) {
                if (e.keyCode == 13) {
                    LineProductList.OnSearch();
                    return false;
                }
            });
            //�������͵����Ի���
            $("[name=LineProductList]").find("[name='Result']").click(function() {
                var Result = $(this).attr("Result");
                switch (Result) {
                    case 'A':
                        {
                            var ID = $(this).attr("ref");
                            LineProductList.openDialog("/GroupEnd/Orders/LineInfo.aspx", "��·��Ϣ", "230", "150", "TourID=" + ID);
                        }
                        break;
                    case 'B':
                        {
                            var ID = $(this).attr("ref");
                            LineProductList.openDialog("/GroupEnd/Orders/GroundOperators.aspx", "�ؽ�����Ϣ", "550", "250", "TourID=" + ID);
                        }
                        break;
                    case 'C':
                        {
                            var ID = $(this).attr("ref");
                            if ($("#lastPeople_" + ID).val() <= 0) {
                                alert("������������");
                            } else {
                                LineProductList.openDialog("/GroupEnd/LineList/SignUp.aspx", "����", "900", "450", "tourId=" + ID);
                            }

                        }
                        break;
                    default: break;
                }
                return false;
            });

        });
        function createHtml(id) {
            $.newAjax({
                url: "/GroupEnd/Orders/LineInfo.aspx?TourID=" + id,
                type: "GET",
                cache: false,
                success: function(result) {

                }
            })
        }

$(function() {
            $.ajax({
                type: "Get",
                async: true,
                url: "/GroupEnd/whlogin.aspx?act=getprovince",
                cache: true,
                dataType: "json",
                success: function(result) {
                    //alert(result);
                    var str = "";
                    var queryProvince="<%=Request.QueryString["ProvinceId"] %>";
                    if(queryProvince=="")
                    if(queryProvince==""&&result.length>0)
                    {
                       //queryProvince=67;
                       //$("#hd_province").val(queryProvince);
                    }
                    for (var i = 0; i < result.length; i++) {
                        if(result[i].ProvinceName=="�㽭"&&queryProvince==""){queryProvince=result[i].Id;$("#hd_province").val(queryProvince);}
                        }
                    for (var i = 0; i < result.length; i++) {
                        str += "<li><a href='javascript:void(0)' class='a_province";
                        if(result[i].Id==queryProvince)
                        str+=" red";
                        str += "' tip='" + result[i].Id + "'>" + result[i].ProvinceName + "</a>&nbsp;&nbsp;|&nbsp;&nbsp;</li>";
                    }
                    if(str!="" )
                    {
                        $("#provinceList").html(str);
 
                    }
                        



                    
                    $(".a_province").live("click", function() {
                        $("#hd_province").val($(this).attr("tip"));
                        $("#st_province").html($(this).html());
                        $(".tip").hide();
                        $.ajax({
                            type: "Get",
                            async: true,
                            url: "/GroupEnd/whlogin.aspx?act=getcity&provinceId=" + $(this).attr("tip"),
                            cache: true,
                            dataType: "json",
                            success: function(result) {
                                if (result.length > 0) {
                                    $("#hd_city").val(result[0].Id);
                                     $("#Btn_Serach").click();
                                }
                            }
                        });
                        return false;
                    })
               var proId="<%=Request.QueryString["ProvinceId"] %>";
            
            if(proId=="")
            {
                proId=queryProvince;
            }
            $.ajax({
                type: "Get",
                async: true,
                url: "/GroupEnd/whlogin.aspx?act=getcity&provinceId="+proId,
                cache: true,
                dataType: "json",
                success: function(result) {
                    var str = "";
                    var queryCity="<%=Request.QueryString["CityId"] %>";
                    if(queryCity=="")
                    {
                                           for (var i = 0; i < result.length; i++) {
                        str += "<a href='javascript:void(0)' class='a_city";
                        //if(i==0)
                        //{
                        //    str+="' tip='" + result[i].Id + "'><input type='button' style='width:60px;line-height:20px;height:20px;cursor:pointer;color:red' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        //}else
                        {
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }
                    } 
                    }else
                    {
                                          for (var i = 0; i < result.length; i++) {
                        str += "<a href='javascript:void(0)' class='a_city";
                        if(result[i].Id==queryCity){
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;color:red' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }else
                        {
                            str+="' tip='" + result[i].Id + "'><input type='button' style='line-height:20px;height:20px;cursor:pointer;' value="+result[i].CityName+" /></a>&nbsp;&nbsp;";
                        }
                    }  
                    }

                    if(str!="")
                    $("#cityList").html(str);
                    $(".a_city").live("click", function() {
                        $("#hd_city").val($(this).attr("tip"));
                        //$(".tip").hide();
                        $("#st_city").html($(this).html());
                        $("#Btn_Serach").click();
                        return false;
                    });

                }
            });
                }
                
            });

            $(".selector").hover(function() { $(this).children(".tip").show(); }, function() { $(this).children(".tip").hide(); });
            var queryArea ='<%=Request.QueryString["areaid"] %>';
            $(".menu-t").each(function(){
                if($(this).attr("tip")==queryArea)
                {
                    $(this).addClass("red");
                }   
                $(this).bind("click",function(){
                    $(this).addClass("red");
                    $("#hd_areaId").val($(this).attr("tip"));
                    $("#Btn_Serach").click();
                    return false;
                });
            });
            
            tourdate.init();
        });

    </script>

    </form>
</asp:Content>
