<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelAdd.aspx.cs" Inherits="Web.SupplierControl.Hotels.HotelAdd" %>

<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div>
        <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top: 10px;">
            <tr class="odd">
                <th width="70" height="30" align="center">
                   <SPAN style="COLOR: red">*</SPAN> 省份：
                </th>
                <td width="330" align="left">
                    <uc1:ucProvince ID="ucProvince1" runat="server" />
                </td>
                <th width="70" align="center">
                   <SPAN style="COLOR: red">*</SPAN> 城市：
                </th>
                <td width="330" align="left">
                    <uc2:ucCity ID="ucCity1" runat="server" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    <SPAN style="COLOR: red">*</SPAN>单位名称：
                </th>
                <td align="left">
                    <input type="text" id="unionname" name="unionname" value="<% =Hotelinfo.UnitName %>"
                        class="searchinput searchinput02" />
                </td>
                <th align="center">
                    星级：
                </th>
                <td align="left">
                    <asp:DropDownList ID="HotelStart" runat="server">
                        <asp:ListItem Text="--请选择酒店星级--" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    地址：
                </th>
                <td colspan="3" align="left">
                    <input type="text" id="TxtHotelAddress" name="TxtHotelAddress" value="<% =Hotelinfo.UnitAddress %>"
                        class="searchinput searchinput02" />
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    酒店简介：
                </th>
                <td colspan="3" align="left">
                    <textarea name="HotelIntroduction" id="HotelIntroduction" cols="45" rows="5" class="textareastyle"><%=Hotelinfo.Introduce %></textarea>
                </td>
            </tr>
            <tr class="odd">
                <th height="30" align="center">
                    酒店图片：
                </th>
                <td colspan="3" align="left" class="updom">
<asp:HiddenField ID="hidImgPhoto" runat="server" />
&nbsp;<table cellspacing="1" cellpadding="0" border="0" bgcolor="#ffffff" width="100%"
                        id="tbl_photo">
                        <tr class="odd">
                            <td align="center" width="30%">
                                选择图片
                            </td>
                            <td align="center" width="40%">
                                图片预览(点击图片查看原图)
                            </td>
                            <td align="center" width="30%">
                                操作
                            </td>
                        </tr>
                        <%if (photo_row == 0)
                          { %>
                        <tr class="even">
                            <td align="center">
                                <input type="file" class="file_landspace" name="file_landspace" onchange="changePhoto(this)" />
                            </td>
                            <td align="center">
                                -
                            </td>
                            <td align="center">
                                <a href="javascript:void(0);" class="add_photo" id="a1">
                                    <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />添加</a>
                                <a href='javascript:void(0);' onclick="del_photo_first(this)">
                                    <img height='16' width='15' src='../../images/delicon01.gif' alt='' />清空本行</a>
                            </td>
                        </tr>
                        <%}
                          else
                          { %>
                        <asp:Repeater ID="repPhotos" runat="server">
                            <ItemTemplate>
                                <tr class="even">
                                    <td align="center">
                                        <input type="file" class="file_landspace" name="file_landspace" onchange="changePhoto(this)" />
                                    </td>
                                    <td align="center">
                                        <a href="<%#Eval("PicPath") %>" target="_blank" class="viewPhoto">
                                            <img name="view_photo" class="view_photo" str="<%#Eval("PicPath") %>♂<%#Eval("PicName") %>♀"
                                                width="80px;" height="60px;" title="点击查看大图"  src="<%#Eval("PicPath") %>" /></a>
                                    </td>
                                    <td align="center">
                                        <%if (photo_row == 1)
                                          { %>
                                        <a href="javascript:void(0);" class="add_photo">
                                            <img height="16" width="15" src="../../images/tianjiaicon01.gif" alt="" />添加</a>
                                        <a href='javascript:void(0);' onclick='del_photo_first(this)'>
                                            <img height='16' width='15' src='../../images/delicon01.gif' alt='' />清空本行</a>
                                        <%}
                                          else
                                          { %><a href='javascript:void(0);' onclick='del_photo(this)'>
                                              <img height='16' width='15' src='../../images/delicon01.gif' alt='' />删除</a>
                                        <%} %>
                                    </td>
                                </tr>
                                <%photo_row++; %>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%} %>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    导游词：
                </th>
                <td colspan="3" align="left">
                    <textarea name="TourGuids" id="TourGuids" cols="45" rows="5" class="textareastyle"><%=Hotelinfo.TourGuide %></textarea>
                </td>
            </tr>
            <tr class="odd">
                <th align="center">
                    联系人：
                </th>
                <td colspan="3" align="left">
                    <table id="userlist" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF">
                        <tr class="odd">
                            <th width="12%" height="30" align="center">姓名</th>
                            <th width="12%" align="center">职务</th>
                            <th width="12%" align="center">电话</th>
                            <th width="12%" align="center">手机</th>
                            <th width="12%" align="center">QQ</th>
                            <th width="12%" align="center">E-mail</th>
                            <th width="12%" align="center">传真</th>
                            <th width="15%" align="center">操作</th>
                        </tr>
                        <%if (Hotelinfo.SupplierContact  != null && Hotelinfo.SupplierContact.Count > 0)
                          {
                              int temp = 0;
                              foreach (EyouSoft.Model.CompanyStructure.SupplierContact sc in Hotelinfo.SupplierContact)
                              {
                        %>
                        <tr class="<% =temp%2==0?"even":"odd" %>">
                            <th height="30" align="center">
                                <input type="text" name="inname" class="searchinput inname" value="<%=sc.ContactName %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" value="<%=sc.JobTitle %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="searchinput inphone" value="<%=sc.ContactTel %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="searchinput inmobile" value="<%=sc.ContactMobile %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" value="<%=sc.QQ %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inemail" class="searchinput inemail" value="<%=sc.Email %>" />
                            </th>
                            <th align="center">
                                <input type="text" name="inefax" class="searchinput inefax" value="<%=sc.ContactFax %>" />
                            </th>
                            <td align="center">
                               <%if (!show)
                                 { %> <a href="javascript:;" class="add">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />添加</a>
                               <a href="javascript:;" class="del">
                                    <img src="/images/delicon01.gif" width="14" height="14" />删除</a><%} %>
                            </td>
                        </tr>
                        <% temp++;}}
                          else
                          { %>
                        <tr class="even">
                            <th height="30" align="center">
                                <input type="text" name="inname" class="searchinput inname" />
                            </th>
                            <th align="center">
                                <input type="text" name="indate" class="searchinput indate" />
                            </th>
                            <th align="center">
                                <input type="text" name="inphone" class="searchinput inphone" />
                            </th>
                            <th align="center">
                                <input type="text" name="inmobile" class="searchinput inmobile" />
                            </th>
                            <th align="center">
                                <input type="text" name="inqq" class="searchinput inqq" />
                            </th>
                            <th align="center">
                                <input type="text" name="inemail" class="searchinput inemail" />
                            </th>
                             <th align="center">
                                <input type="text" name="inefax" class="searchinput inefax" />
                            </th>
                            <td align="center">
                               <%if (!show)
                                 { %><a href="javascript:;" class="add">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />
                                    添加</a> <a href="javascript:;" class="del">
                                        <img src="/images/delicon01.gif" width="14" height="14" />
                                        删除</a><%} %>
                            </td>
                        </tr>
                        <%} %>
                    </table>
                </td>
            </tr>
            <tr class="even">
                <th height="30" align="center">
                    价格：
                </th>
                <td colspan="3" align="left">
                    <table id="pricesList" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF">
                        <tr class="odd">
                            <th width="20%" height="30" align="center">房型</th>
                            <th width="20%" align="center">前台销售价</th>
                            <th width="20%" align="center">结算价</th>
                            <th width="26%" align="center">含早</th>
                            <th width="14%" align="center">操作</th>
                        </tr>
                        <%if ( Hotelinfo.RoomTypes != null && Hotelinfo.RoomTypes.Count > 0)
                          {
                              int temp = 0;
                              foreach (EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo roomtype in Hotelinfo.RoomTypes)
                              {
                        %>
                        <tr class="<% =temp%2==0?"even":"odd" %>">
                           <th height="30" align="center">
                                <input name="Chamber" type="text" class="searchinput Chamber" value="<%= roomtype.Name %>"/>                                
                            </th>
                            <th align="center">
                                <input name="SalaesPrices" type="text" class="searchinput SalaesPrices" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(roomtype.SellingPrice.ToString()).ToString("0.00")) %>"/>
                            </th>
                            <th align="center">
                                <input name="SettlementPrice" type="text" class="searchinput SettlementPrice" value="<%=EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(roomtype.AccountingPrice.ToString()).ToString("0.00")) %>"/>
                            </th>
                            <th align="center">
                                <input type='hidden' name='hd_rbtn' value="<%=roomtype.IsBreakfast?"1":"2"%>"/><input name="radiobutton<%=temp %>"  class="radiobutton" <%=roomtype.IsBreakfast?"checked":"" %> type="radio"  value="1"/>
                                是
                                <input type="radio" name="radiobutton<%=temp %>"  <%=roomtype.IsBreakfast?"":"checked" %> class="radiobutton" value="2"/>否
                            </th>
                            <td align="center">
                                <%if (!show)
                                  { %> <a href="javascript:void(0);" class="addprices">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />添加</a>
                                <a href="javascript:viod(0);" class="delprices">
                                   <img src="/images/delicon01.gif" width="14" height="14" />删除</a><%} %>
                            </td>
                        </tr>
                        <% temp++;}
                }
                else
                { %>                                                                 
                        <tr class="even">
                            <th height="30" align="center">
                                <input name="Chamber" type="text" class="searchinput Chamber"/>
                            </th>
                            <th align="center">
                                <input name="SalaesPrices" type="text" class="searchinput SalaesPrices"/>
                            </th>
                            <th align="center">
                                <input name="SettlementPrice" type="text" class="searchinput SettlementPrice" />
                            </th>
                            <th align="center">
                                <input type='hidden' name='hd_rbtn' value="1"/><input name="radiobutton"  class="radiobutton" type="radio"  value="1" checked="checked" />
                                是
                                <input type="radio" name="radiobutton"  class="radiobutton"  value="2" />否
                            </th>
                            <td align="center">
                                 <%if (!show)
                                   { %><a href="javascript:void(0);" class="addprices">
                                    <img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" />添加</a>
                                <a href="javascript:viod(0);" class="delprices">
                                   <img src="/images/delicon01.gif" width="14" height="14" />删除</a><%} %>
                            </td>
                        </tr>
                         <%} %>
                    </table>
                </td>
            </tr>
            <tr class="odd">
                <th height="60" align="center">备注:</th>
                <td colspan="3">
                    <textarea name="HotelRemarks" id="HotelRemarks" cols="45" rows="5" class="textareastyle"><%=Hotelinfo.Remark %></textarea>
                </td>
            </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                <%if (!show)
                  { %>
                    <asp:LinkButton ID="LinkButton1" runat="server"  OnClientClick="return checkForm();"
                        onclick="LinkButton1_Click" >保存</asp:LinkButton><%} %>
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">
                        关闭</a>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        function checkForm() {
            var res = true;
            
            var provinceid = $("#" + provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId).val();
            var cityid = $("#" + provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId).val();

            if (provinceid == "0") {            
                alert("请选择省份!");
                $(this).focus();
                res = false;
                return false;
            };
            if (cityid == "0") {
                alert("请选择城市!");
                $(this).focus();
                res = false;
                return false;
            };
            if ($("#unionname").val() == "") {
                alert("请填写单位名称!");
                $(this).focus();
                res = false;
                return false;
            }        
//            $("#userlist").find("input.inname").each(function() {
//                if (!$.trim($(this).val())) {
//                    alert("请填写联系人姓名!");
//                    $(this).focus();
//                    res = false;
//                }
//                return res;
//            });
            res = checkPhoto();
//            if (res) {
//                $("#userlist").find("input.inmobile").each(function() {
//                    if (!$.trim($(this).val())) {
//                        alert("请填写联系人手机号码!");
//                        $(this).focus();
//                        res = false;
//                    }
//                    return res;
//                });
//            }
            getImgStr();
            return res;
        };

        function getuserinput() {
            var cls = $("#userlist").find("tr").length % 2 ? "even" : "odd";
            var html = "<tr class=\"" + cls + "\">" +
                                "<th height=\"30\" align=\"center\"><input  type=\"text\" name=\"inname\"  class=\"searchinput inname\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"indate\" class=\"searchinput indate\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inphone\" class=\"searchinput inphone\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inmobile\" class=\"searchinput inmobile\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inqq\" class=\"searchinput inqq\" /></th>" +
                                "<th align=\"center\"><input  type=\"text\" name=\"inemail\" class=\"searchinput inemail\" /></th>" +
                                 "<th align=\"center\"><input type=\"text\" name=\"inefax\" class=\"searchinput inefax\" /></th>" +
                                "<td align=\"center\"><a href=\"javascript:;\" class=\"add\" ><img height=\"16\" width=\"15\" alt=\"\" src=\"/images/tianjiaicon01.gif\" /> 添加</a> <a href=\"javascript:;\" class=\"del\" ><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a></td>" +
                                "</tr>";
            return html;
        };
        function add() {
            var html = getuserinput();
            $("#userlist").append(html);
            $("a.add").unbind().bind("click", function() {
                add();
                return false;
            });
            $("a.del").unbind().bind("click", function() {
                var that = $(this);
                del(that);
                return false;
            });
        };
        function del(that) {
            var trlist = $("#userlist").find("tr");
            if (trlist.length > 2) {
                that.parent().parent().remove();
            }
        };
        
        $(document).ready(function() {
            var proid = $("#" + provinceAndCity_province["<%=ucProvince1.ClientID %>"].provinceId).val();
            var cityid = $("#" + provinceAndCity_city["<%=ucCity1.ClientID %>"].cityId).val();

            $("img.close").click(function() {
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"HotelImage\" />");
                return false;
            });
           
            $("a.add").bind("click", function() {
                add();
                return false;
            });
            $("a.del").bind("click", function() {
                var that = $(this);
                del(that);
                return false;
            });

            $(".radiobutton").click(function() {
                $(this).prevAll("[name='hd_rbtn']").val($(this).val());
            });
            
            var count = 1;
            //价格组成
            function getpriceinput() {
                var cls = $("#userlist").find("tr").length % 2 ? "even" : "odd";
                var html = "<tr class=\"" + cls + "\">" +
                                "<th height=\"30\" align=\"center\"><input name=\"Chamber\" type=\"text\" class=\"searchinput Chamber\"/></th>" +
                                "<th align=\"center\"><input name=\"SalaesPrices\" type=\"text\" class=\"searchinput SalaesPrices\"/></th>" +
                                "<th align=\"center\"><input name=\"SettlementPrice\" type=\"text\" class=\"searchinput SettlementPrice\" /></th>" +
                                "<th align=\"center\"><input type='hidden' value='1' name='hd_rbtn'/><input name=\"radiobutton" + (count + 1) + "\" class=\"radiobutton" + (count + 1) + "\" type=\"radio\"  value=\"1\" />是<input type=\"radio\" name=\"radiobutton" + (count + 1) + "\"  class=\"radiobutton" + (count + 1) + "\" value=\"2\"/>否</th>" +
                                "<td align=\"center\"><a href=\"javascript:;\" class=\"addprices\" ><img height=\"16\" width=\"15\" alt=\"\" src=\"/images/tianjiaicon01.gif\" /> 添加</a> <a href=\"javascript:;\" class=\"delprices\" ><img src=\"/images/delicon01.gif\" width=\"14\" height=\"14\" /> 删除</a></td>" +
                                "</tr>";
                return html;
            };
            function addprices() {
                var html = getpriceinput();
                $("#pricesList").append(html);

                $(".radiobutton" + (count + 1) + "").click(function() {
                    $(this).prevAll("[name='hd_rbtn']").val($(this).val());
                });
                count++;
                $("a.addprices").unbind().bind("click", function() {
                    addprices();
                    return false;
                });
                $("a.delprices").unbind().bind("click", function() {
                    var that = $(this);
                    delprices(that);
                    return false;
                });
            };
            function delprices(that) {
                var trlist = $("#pricesList").find("tr");
                if (trlist.length > 2) {
                    that.parent().parent().remove();
                }
            };
            $("a.addprices").bind("click", function() {
                addprices();
                return false;
            });
            $("a.delprices").bind("click", function() {
                var that = $(this);
                delprices(that);
                return false;
            });

        });
        //首行图片删除处理,不删除本行,删除图片预览
        function del_photo_first(obj) {
            var tr = $(obj).parent().parent();
            var td = tr.children().children("a[class='viewPhoto']").children("img").parent().parent();
            var td_first = tr.children().children("input").parent();
            tr.children().children("a[class='viewPhoto']").children("img").remove();
            td.html("-");
            var input = tr.children().children("input");
            tr.children().children("input").remove();
            td_first.html("<input type='file' name='file_landspace' onchange='changePhoto(this)' />")

        }
        //删除图片
        function del_photo(obj) {
            $(obj).parent().parent().remove();
            return false;
        }
        //添加图片行
        $(".add_photo").click(function() {
            //            $("#tbl_photo").append(" <tr  class=\"even\"><td align=\"center\"><input type=\"file\" name=\"file_landspace\"  style=\"float: left\" /></td><td align=\"center\">-</td><td align=\"center\"><a href=\"javascript:void(0);\" class=\"add_photo\" ><img height=\"16\" width=\"15\" src=\"../../images/tianjiaicon01.gif\"  />添加</a>    <a href=\"javascript:void(0);\" onclick=\"del_photo(this)\" ><img height=\"16\" width=\"15\" src=\"../../images/delicon01.gif\"  />删除</a></td></tr>");
            $("#tbl_photo").append("<tr height='22px' class='even'><td align='center'><input class=\"file_landspace\" type=\"file\" name=\"file_landspace\" /></td><td align='center'>-</td><td align='center'><a href=\"javascript:void(0);\" onclick=\"del_photo(this)\" ><img height=\"16\" width=\"15\" src=\"../../images/delicon01.gif\"  />删除</a></td></tr>");
            return false;
        })
        //更新时操作.如果选择图片,则清空那行的图片.
        function changePhoto(obj) {
            var tr = $(obj).parent().parent();
            var td = tr.children().children("a[class='viewPhoto']").children("img").parent().parent();
            tr.children().children("a[class='viewPhoto']").children("img").remove();
            td.html("-");
        }
        //保存时将所有图片的值保存到一个hid控件中
        function getImgStr() {
            var str = "";
            $(".view_photo").each(function() {
                str += $(this).attr("str");
            })
            $("#<%=hidImgPhoto.ClientID %>").val(str);
        }

        //验证图片上传格式
        function checkPhoto() {
            var img_type;
            var res = true;
            $(".file_landspace").each(function() {
                if ($(this).val() != "") {
                    img_type = $(this).val().substring($(this).val().lastIndexOf('.'), $(this).val().length);
                    if (img_type.toLowerCase() != ".jpg" && img_type.toLowerCase() != ".jpeg" && img_type.toLowerCase() != ".png" && img_type.toLowerCase() != ".gif") {
                        alert("请选择正确格式的图片文件");
                        $(this).focus();
                        res = false;
                        return false;
                    }
                }
            })
            return res;
        }
    </script>

</body>
</html>
