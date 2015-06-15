<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sale_show.aspx.cs" Inherits="Web.sales.Sale_show" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
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
                    <td align="left" colspan="2">
                        <font class="fbred">
                            <asp:Label ID="lblCurFreePosi" runat="server" Text=""></asp:Label></font>
                    </td>
                    <td style="text-align:center;"><b>对方团号：</b></td>
                    <td style="text-align:center;"><asp:Literal runat="server" ID="ltrBuyerTourCode"></asp:Literal></td>
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
                        <asp:Label ID="lblContactName" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        电话：
                    </th>
                    <td>
                        <asp:Label ID="lblContactPhone" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center">
                        手机：
                    </th>
                    <td colspan="2">
                        <asp:Label ID="lblContactMobile" runat="server" Text=""></asp:Label>
                    </td>
                    <th align="center">
                        传真：
                    </th>
                    <td>
                        <asp:Label ID="lblContactFax" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                
                <tr class="even">
                    <th style="text-align:center">
                        对方操作员：
                    </th>
                    <td colspan="4">
                        <asp:Literal runat="server" ID="ltrBuyerContact"></asp:Literal>
                    </td>
                </tr>
                
                <tr class="odd">
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
                <tr class="even">
                    <th height="25" align="center">
                        人数：
                    </th>
                    <td align="left" colspan="4">
                        <div id="SanPingPersonNum" runat="server">
                            成人数：
                            <asp:Label ID="txtDdultCount" runat="server" Text="Label"></asp:Label>
                            儿童数：
                            <asp:Label ID="txtChildCount" runat="server" Text=""></asp:Label>
                        </div>
                        <asp:Label ID="lblTeamPersonNum" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr class="odd">
                    <th height="25" align="center">
                        总金额：
                    </th>
                    <td align="left" colspan="4">
                        ￥<asp:Label ID="txtTotalMoney" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th align="center" rowspan="2">
                        游客信息：
                    </th>
                    <td height="28" colspan="4">
                        <table cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">游客附件</asp:HyperLink>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr class="even">
                    <td align="center" colspan="4">
                        <table id="cusListTable" cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4"
                            align="center" width="90%" style="margin: 10px 0pt;">
                            <tbody>
                                <tr>
                                    <td style="width: 5%" bgcolor="#e3f1fc" align="center">
                                        序号
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
                        <asp:Label ID="txtSpecialRe" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <th height="42" align="center">
                        操作留言：
                    </th>
                    <td align="left" colspan="4">
                        <asp:Label ID="txtOperMes" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="center" colspan="5">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
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

    <script type="text/javascript">
      var OrderEdit = {
        //打开特服窗口
            OpenSpecive: function(tefuID,$obj) {
                parent.Boxy.iframeDialog({
                iframeUrl: "/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid="+tefuID,
                    title: "特服",
                    width: "420px",
                    height: "200px",
                    modal: true
               });
                return false;
             }
      }
      
                  $(".divPrice[val='<%=PriceStandId%>']").parent().parent().find("input[type='radio']").each(function(){
                if($(this).val()=="<%=CustomerLevId%>")
                {
                    $(this).attr("checked","checked");
                }
            });
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
    </script>

    </form>
</body>
</html>
