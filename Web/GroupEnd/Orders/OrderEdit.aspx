<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs" Inherits="Web.GroupEnd.Orders.OrderEidt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            height: 40px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <input id="hd_lineID" type="hidden" runat="server" />
        <input type="hidden" id="hd_BuyCompanyId" name="hd_BuyCompanyId" runat="server" />
        <input type="hidden" id="hd_PriceStandId" name="hd_PriceStandId" runat="server" />
        <input type="hidden" id="hd_cr_price" name="hd_cr_price" runat="server" />
        <input type="hidden" id="hd_rt_price" name="hd_rt_price" runat="server" />
        <input type="hidden" id="hd_LevelID" name="hd_LevelID" runat="server" />
        <input type="hidden" id="TourID" name="TourID" runat="server" />
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="880" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <th height="25" align="center" width="100px">
                        线路名称：
                    </th>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblLineName" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        出团时间：
                    </th>
                    <td align="center">
                        <asp:Label ID="lblChuTuanDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        当前空位：
                    </th>
                    <td align="center" colspan="2">
                        <font class="fbred">
                            <asp:Label ID="lblCurFreePosi" runat="server" Text=""></asp:Label></font>
                    </td>
                    <th height="25" align="center">
                        <asp:Label ID="lblLeaveState" runat="server" Text="留位时间："></asp:Label>
                    </th>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblLeave" runat="server" Text="Label"></asp:Label>
                        <font class="fbred">
                </tr>
                <tr class="odd">
                    <th height="25" align="center">
                        出发交通：
                    </th>
                    <td align="center" colspan="2">
                        <asp:Label ID="lblChuFanTra" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        返程交通：
                    </th>
                    <td align="center">
                        <asp:Label ID="lblBackTra" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center">
                        联系人：
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtContactName" runat="server" class="searchinput"></asp:TextBox>
                    </td>
                    <th align="center">
                        电话：
                    </th>
                    <td>
                        <asp:TextBox ID="txtContactPhone" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center">
                        手机：
                    </th>
                    <td colspan="2">
                        <asp:TextBox ID="txtContactMobile" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                    <th align="center">
                        传真：
                    </th>
                    <td>
                        <asp:TextBox ID="txtContactFax" runat="server" class="searchinput" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th height="55" align="center">
                        结算价：
                    </th>
                    <td align="left" valign="middle" colspan="4">
                        <table cellspacing="0" cellpadding="0" border="0" align="left">
                            <tbody>
                                <%=price%>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="25" align="center">
                        人数：
                    </th>
                    <td align="left" colspan="4">
                        <div id="SanPingPersonNum" runat="server">
                            成人数：
                            <asp:TextBox ID="txtDdultCount" runat="server" class="searchinput searchinput03"
                                MaxLength="6" valid="isNumber" errmsg="请输入有效成人数"></asp:TextBox>
                            儿童数：
                            <asp:TextBox ID="txtChildCount" runat="server" class="searchinput searchinput03"
                                MaxLength="6" valid="isNumber" errmsg="请输入有效儿童数"></asp:TextBox>
                        </div>
                        <asp:TextBox ID="lblTeamPersonNum" runat="server" Visible="false" class="searchinput searchinput03"
                            MaxLength="6" valid="isNumber" errmsg="请输入有效人数"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th height="25" align="center">
                        总金额：
                    </th>
                    <td align="left" colspan="4">
                        <asp:TextBox ID="txtTotalMoney" runat="server" class="searchinput" MaxLength="10"
                            valid="required|isMoney" errmsg="请输入金额|请输入有效金额"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center" rowspan="2">
                        游客信息：
                    </th>
                    <td height="28" align="center" colspan="4">
                        <table cellspacing="0" cellpadding="0" border="0" align="right" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">游客附件</asp:HyperLink>
                                    </td>
                                    <td align="right">
                                        上传附件：
                                        <input type="file" name="fileField" id="fileField" />
                                    </td>
                                    <td align="center">
                                        <a href="/Common/LoadVisitors.aspx?topID=<%=Request.QueryString["iframeId"]%>" id="import">
                                            <img src="/images/sanping_03.gif">
                                            导入</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <td align="center" colspan="4">
                        <table id="cusListTable" cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4"
                            align="center" width="95%" style="margin: 10px 0pt;">
                            <tbody>
                                <tr>
                                    <td height="5%" align="center" bgcolor="#E3F1FC">
                                        编号
                                    </td>
                                    <td height="25" bgcolor="#e3f1fc" align="center">
                                        姓名
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        类型
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        证件名称
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        证件号码
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        性别
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        联系电话
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        特服
                                    </td>
                                    <td bgcolor="#e3f1fc" align="center">
                                        操作 <a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>
                                    </td>
                                </tr>
                                <%=cusHtml%>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="odd">
                    <th align="center">
                        特殊要求说明：
                    </th>
                    <td align="left" colspan="4">
                        <asp:TextBox ID="txtSpecialRe" runat="server" class="textareastyle02" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center" class="style1">
                        操作留言：
                    </th>
                    <td align="left" colspan="4" class="style1">
                        <asp:TextBox ID="txtOperMes" runat="server" TextMode="MultiLine" class="textareastyle02"
                            MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="5">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click" Visible="false">确认提交</asp:LinkButton>
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <script src="/js/loadVisitors.js" type="text/javascript"></script>

    <script type="text/javascript">
                    //根据用户输入的身份证号判断性别
    function getSex(obj)
    {
        var val=$(obj).val();
        var tr =$(obj).parent().parent();
        var sex=tr.children().children("select[class='ddlSex']");
        var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                        if (isIdCard.exec(val)) {
                    if (15 == val.length) {// 15位身份证号码
                        if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                            sex.val(1);
                        else
                            sex.val(2);
                    }

                    if (18 == val.length) {// 18位身份证号码
                        if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                            sex.val(1);
                        else
                            sex.val(2);
                    }
                } else {
                sex.val(0);
                }
    }
        $(document).ready(function() {
        
            //打开导入窗口
            $("a[id='import']").click(function() {
                parent.Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    width: "853px",
                    height: "514px",
                    async: false,
                    title: "导入",
                    modal: true
                });
                return false;
            });

            $("#<%=txtChildCount.ClientID%>").change(function() {
                if (OrderEdit.CheckIsInt($(this))) {
                    alert("输入的儿童数应为整数");
                    $(this).focus().select();
                }
                 else{
                  OrderEdit.GetSumMoney();
                }
            });

            $("#<%=txtDdultCount.ClientID%>").change(function() {
                if (OrderEdit.CheckIsInt($(this))) {
                    alert("输入的成人数应为整数");
                    $(this).focus();
                }
                else{
                  OrderEdit.GetSumMoney();
                }
            });

            //提交表单时去除游客类型的disabled
            $("#<%=lbtnSubmit.ClientID%>").click(function() {
                var form = $(this).closest("form").get(0);
                //点击按纽触发执行的验证函数
                var msg=ValiDatorForm.validator(form, "alert");
                if(!msg){
                  return false;
                }
                OrderEdit.RemoveDisabled();
            });

            //检测总金额输入是否合法
            $("#<%=txtTotalMoney.ClientID%>").change(function() {
                if (OrderEdit.CheckIsFloat($(this))) {
                    alert("总金额输入非法");
                    $(this).focus().select();
                }
            });
            
            $(":radio[id='radio<%=PriceStandId%>']").attr("checked","checked");
            
            $("[name='radio']").click(function() {
            if ($(this).attr("checked")) {
                $("#hd_PriceStandId").val($(this).val());
                $("#hd_cr_price").val($("input[name='radio']:checked").parent().parent().find("[name='sp_cr_price']").html());
                $("#hd_rt_price").val($("input[name='radio']:checked").parent().parent().find("[name='sp_et_price']").html());
                OrderEdit.GetSumMoney();
            }
            });
            
        });

        function CreateCusList(array) {
            var url = "/ashx/CreateCurList.ashx?type=order&trCount="+($("#cusListTable").find("tr").length-1)+"&array=" + encodeURIComponent(array);
            $.newAjax({
                type: "Get",
                url: url,
                cache: false,
                dataType: "html",
                success: function(data) {
                    $("#cusListTable tr:last").after(data);
                }
            });
        }

        var OrderEdit = {

            AddCus: function() {
                OrderEdit.CreateTR();
            },

            DelCus: function($obj) {
                if ($obj.prev("input[name='cusState']").val() == "ADD") {
                    $obj.parent().parent().remove();
                }
                else {
                    $obj.parent().parent().css("display", "none");
                    $obj.prev("input[name='cusState']").val("DEL");
                }
                 var i=1;
                $("td[index]").each(function(){
                if($(this).parent().css("display")!= "none")
                {
                    $(this).html(i);
                    i++;
                }
                })
            },


            //检测输入是否是大于等于0整数
            CheckIsInt: function($obj) {
                var re = /^[0-9]([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            },
            

            //检测总金额输入是否正确
            CheckIsFloat: function($obj) {
                var re = /^[0-9]*.?([0-9])*$/;
                if (re.test($obj.val())) {

                    return false;
                }
                else {
                    return true;
                }
            },

            //根据选择计算标准，计算总金额
            GetSumMoney: function() {
                var specMoney=0;
                var adult = Number($("#<%=txtDdultCount.ClientID%>").val());
                var child = Number($("#<%=txtChildCount.ClientID%>").val());
                var adultP=Number($("input[name='radio']:checked").parent().parent().find("[name='sp_cr_price']").html());
                var childP=Number($("input[name='radio']:checked").parent().parent().find("[name='sp_et_price']").html());
                $("input[name='specive']").each(function(){
                  specMoney+=getAmountInTeFuStr($(this).val());
                });
                $("#<%=txtTotalMoney.ClientID%>").val(adult*adultP+child*childP+specMoney);
                
            },
            
            
            //数据提交时，却除某些需要提交控件的disabled属性.
            RemoveDisabled:function(){
              $("select[name='cusType']").each(function() {
                    $(this).removeAttr("disabled");
                });
                $("#<%=lblTeamPersonNum.ClientID%>").removeAttr("disabled");
                $("#<%=txtDdultCount.ClientID%>").removeAttr("disabled");
                $("#<%=txtChildCount.ClientID%>").removeAttr("disabled");
                $("#<%=txtTotalMoney.ClientID%>").removeAttr("disabled");
            },

            
            //打开特服窗口
            OpenSpecive: function(tefuID) {
                parent.Boxy.iframeDialog({
                iframeUrl: "/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid="+tefuID,
                    title: "特服",
                    width: "420px",
                    height: "200px",
                    modal: true,
                    afterHide:function(){
                      OrderEdit.GetSumMoney();
                   }
               });
             
                return false;
             },

            CreateTR: function() {
             var i=1;
            $("td[index]").each(function(){
            if($(this).parent().css("display")!= "none")
            {
            i++;
            }
            }) 
                var d = new Date();
                var tefuID=d.getTime();
                var TR = '<tr>'
              + '<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index="0" align=\"center\">'+i+'</td><td height="25" bgcolor="#e3f1fc" align="center">'
            + '    <input type="text" class="searchinput" id="Text1" MaxLength="50"  name="cusName" />'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select1" title="请选择" valid="required" errmsg="请选择类型!" name="cusType">'
                    + '<option value="" selected="selected">请选择</option>'
                    + '<option value="1">成人</option>'
                    + '<option value="2">儿童</option>'
                + '</select>'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select2" name="cusCardType">'
                    + '<option value="0" selected="selected">请选择证件</option>'
                    + '<option value="1">身份证</option>'
                    + '<option value="2">护照</option>'
                    + '<option value="3">军官证</option>'
                    + '<option value="4">台胞证</option>'
                    + '<option value="5">港澳通行证</option>'
                + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                + ' <input type="text" class="searchinput searchinput02" onblur="getSex(this)" id="text2" name="cusCardNo">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                 + '<select id="Select3" name="cusSex" class="ddlSex">'
                     + '<option value="0"  selected="selected">请选择</option>'
                     + '<option value="1">男</option>'
                     + '<option value="2">女</option>'
                 + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                 + '<input type="text" class="searchinput" id="Text3" name="cusPhone">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="6%">'
              + '<input type="hidden" name="txtItem" value="" />'
              + '<input type="hidden" name="txtServiceContent" value="" />'
              + '<input type="hidden" name="ddlOperate" value="" />'
              + '<input type="hidden" name="txtCost" value="" />'
               + '<input id="' + tefuID + '" type="hidden" name="specive" value=""/>'
                 + '<a sign="speService" href="javascript:void(0);" onclick="OrderEdit.OpenSpecive('+tefuID+')">特服</a>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="12%">'
              + '<input type="hidden" name="cusID" value="" />'
             + '<a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>&nbsp;'
             + '<input type="hidden" name="cusState" value="ADD" />'
             + '<a sign="del" href="javascript:void(0)" onclick="OrderEdit.DelCus($(this))">删除</a>'
             + '</td>'
         + '</tr>';
                $("#cusListTable tr:last").after(TR);
            }

        }
    </script>

    </form>
</body>
</html>
