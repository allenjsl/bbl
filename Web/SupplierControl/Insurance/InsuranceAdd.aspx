<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsuranceAdd.aspx.cs" Inherits="Web.SupplierControl.Insurance.InsuranceAdd" %>
<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <input  type="hidden" value="1" name="issave"/>
    <div>
    <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top:10px;">
    <tr class="odd">
    <th width="70" height="30" align="center"><SPAN style="COLOR: red">*</SPAN>省份：</th>
    <td width="330" align="left">
       <uc1:ucProvince ID="ucProvince1" runat="server" />
       <input type="hidden" name="proname" id="proname" />
     </td>
    <th width="70" align="center"><SPAN style="COLOR: red">*</SPAN>城市：</th>
    <td width="330" align="left">
       <uc2:ucCity ID="ucCity1" runat="server" />
       <input type="hidden" name="cityname" id="cityname" /> 
    </td>
  </tr>
  <tr class="even">
    <th height="30" align="center"><SPAN style="COLOR: red">*</SPAN>单位名称： </th>
    <td colspan="3" align="left">
       <input type="text" id="Txtunitsnname" name="Txtunitsnname" value="<% =Insurancemodel.UnitName%>"
                        class="searchinput searchinput02" /></td>
  </tr>
  <tr class="odd">
    <th height="30" align="center">地址： </th>
    <td colspan="3" align="left"> 
      <input type="text" id="TxtAddress" name="TxtAddress" value="<% =Insurancemodel.UnitAddress %>"
                        class="searchinput searchinput02" /></td>
  </tr>
  <tr class="even">
    <th height="30" align="center">合作协议： </th>
     <td colspan="3" align="left" class="updom">
            <%if (!string.IsNullOrEmpty(Insurancemodel.AgreementFile))
              { %>
              <a href="<%=Insurancemodel.AgreementFile%>" target="_blank">查看合作协议</a><img src="/images/fujian_x.gif" style="cursor:pointer" width="14" height="13" class="close" alt="" />
            <%}
              else
              { %>
            <input type="file" name="workAgree"/><asp:Literal ID="litmsg" runat="server"></asp:Literal>
            <%} %>
            </td>
  </tr>
  <tr class="even">
    <th align="center">联系人：</th>
    <td colspan="3" align="left">
       <table id="userlist" width="100%" border="0" cellpadding="0" cellspacing="1" bgcolor="#FFFFFF">
              <tr class="odd">
                <th width="12%" height="30" align="center"><SPAN style="COLOR: red">*</SPAN>姓名</th>
                <th width="12%" align="center">职务</th>
                <th width="12%" align="center">电话</th>
                <th width="12%" align="center"><SPAN style="COLOR: red">*</SPAN>手机</th>
                <th width="12%" align="center">QQ</th>
                <th width="12%" align="center">E-mail</th>
                <th width="12%" align="center">传真</th>
                <th width="15%" align="center">操作</th> 
              </tr>
              <%if (Insurancemodel.SupplierContact != null && Insurancemodel.SupplierContact.Count > 0)
                {
                    int temp = 0;
                    foreach (EyouSoft.Model.CompanyStructure.SupplierContact sl in Insurancemodel.SupplierContact)
                    {
                         %>
              <tr class="<% =temp%2==0?"even":"odd" %>">
                <th height="30" align="center"><input  type="text" name="inname" class="searchinput inname" value="<%=sl.ContactName %>" /></th>
                <th align="center"><input  type="text" name="indate" class="searchinput indate" value="<%=sl.JobTitle %>" /></th>
                <th align="center"><input  type="text" name="inphone" class="searchinput inphone" value="<%=sl.ContactTel %>" /></th>
                <th align="center"><input  type="text" name="inmobile" class="searchinput inmobile" value="<%=sl.ContactMobile %>" /></th>
                <th align="center"><input  type="text" name="inqq" class="searchinput inqq" value="<%=sl.QQ %>" /></th>
                <th align="center"><input  type="text" name="inemail" class="searchinput inemail" value="<%=sl.Email %>" /></th>
                 <th align="center"><input  type="text" name="inefax" class="searchinput inefax" value="<%=sl.ContactFax %>" /></th>
                <td align="center"> <%if (!show)
                                      { %><a href="javascript:;" class="add"><img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" /> 添加</a> <a href="javascript:;" class="del"><img src="/images/delicon01.gif" width="14" height="14"  /> 删除</a><%} %></td>
              </tr>
              <% temp++;}
                }
                else
                { %>
                <tr class="even">
                    <th height="30" align="center"><input  type="text" name="inname" class="searchinput inname" /></th>
                    <th align="center"><input  type="text" name="indate" class="searchinput indate" /></th>
                    <th align="center"><input  type="text" name="inphone" class="searchinput inphone" /></th>
                    <th align="center"><input  type="text" name="inmobile" class="searchinput inmobile" /></th>
                    <th align="center"><input  type="text" name="inqq" class="searchinput inqq" /></th>
                    <th align="center"><input  type="text" name="inemail" class="searchinput inemail" /></th>
                    <th align="center"><input  type="text" name="inefax" class="searchinput inefax" /></th>
                    <td align="center"> <%if (!show)
                                          { %><a href="javascript:;" class="add"><img height="16" width="15" alt="" src="/images/tianjiaicon01.gif" /> 添加</a> <a href="javascript:;" class="del"><img src="/images/delicon01.gif" width="14" height="14"  /> 删除</a><%} %></td>
                  </tr>
              <%} %>
            </table>
    </td>
  </tr>
  <tr class="odd">
    <th height="60" align="center">备注：</th>
    <td colspan="3"><textarea name="TxtRemarks" id="TxtRemarks" cols="45" rows="5" class="textareastyle" ><%=Insurancemodel.Remark%></textarea></td>
  </tr>
</table>
    <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td height="40" align="center"></td>
        <td height="40" align="center" class="tjbtn02">
            <%if (!show)
              { %><asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return checkForm();" onclick="LinkButton1_Click" >保存</asp:LinkButton><%} %></td>
        <td height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()";>关闭</a></td>
      </tr>
    </table>
    </div>
    <input type="hidden" name="tid" value="<%=tid %>" />
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
            if ($("#Txtunitsnname").val() == "") {
                alert("请填写单位名称!");
                $(this).focus();
                res = false;
                return false;
            };
            
            $("#userlist").find("input.inname").each(function() {
                if (!$.trim($(this).val())) {
                    alert("请填写联系人姓名!");
                    $(this).focus();
                    res = false;
                }
                return res;
            });
            if (res) {
                $("#userlist").find("input.inmobile").each(function() {
                    if (!$.trim($(this).val())) {
                        alert("请填写联系人手机号码!");
                        $(this).focus();
                        res = false;
                    }
                    return res;
                });
            }
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
                                "<th align=\"center\"><input  type=\"text\" name=\"inefax\" class=\"searchinput inefax\" /></th>" +
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
                if (confirm("你确定要删除吗?")) {
                    var that = $(this);
                    del(that);
                    return false;
                }
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
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />");
            });

            $("a.add").bind("click", function() {
                add();
                return false;
            });
            $("a.del").bind("click", function() {
                if (confirm("你确定要删除吗?")) {
                    var that = $(this);
                    del(that);
                    return false;
                }
                return false;
            });
        });
    </script>
</body>
</html>
