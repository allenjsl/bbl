<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="Web.GroupEnd.SystemSetting.CompanyInfo"
    MasterPageFile="~/masterpage/Front.Master"  Title="系统设置"%>
<%@ Register Src="~/UserControl/CityList.ascx" TagName="CityList" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ProvinceList.ascx" TagName="ProvinceList" TagPrefix="uc2" %>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
   <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <form id="form1" runat="server">    
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">系统设置</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        所在位置>> 系统设置
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="2" bgcolor="#000000">
                    </td>
                </tr>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
              <li><a href="AccountManager.aspx" class="">帐号管理</a></li>
              <li><a href="Javascript:void(0);" class="tabtwo-on">公司信息</a></li>            
              <li><a href="DeployManager.aspx" class="">配置管理</a></li>
            <div class="clearboth"></div>
        </ul>
        <div class="addlinebox">
            <!--公司信息-->
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_2">
                <tr>
                    <td width="10%" height="40" align="right">
                        <font class="xinghao"></font>省份：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td width="86%">
                       <uc2:ProvinceList  ID="ProvinceList1" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao"></font>城市：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <uc1:CityList  ID="CityList1" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao">*</font>单位名称：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_UnitsName" id="Txt_UnitsName" class="searchinput searchinput02" runat="server" valid="required" errmsg="请填写单位名称!"/>
                        <span id="errMsg_<%=Txt_UnitsName.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao"></font>许可证号：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_LicenseNumber" id="Txt_LicenseNumber" class="searchinput searchinput02" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao"></font>公司地址：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_CompanyAddress" id="Txt_CompanyAddress" class="searchinput searchinput02"   runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao"></font>邮编：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_Code" id="Txt_Code" class="searchinput searchinput02" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao"></font>银行帐号：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_AccountNumber" id="Txt_AccountNumber" class="searchinput searchinput02"  runat="server" >
                    </td>
                </tr>
                <tr style="display:none;">
                    <td height="35" align="right">
                        <font class="xinghao"></font>返佣类型：
                    </td>
                    <td width="2%">
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="DDlReBateType" runat="server">
                             <asp:ListItem Text="请选择返佣类型" Value="0"></asp:ListItem>
                             <asp:ListItem Text="现返" Value="1"></asp:ListItem>
                             <asp:ListItem Text="后返" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao">*</font>主要联系人：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Contact" id="Txt_Contact" class="searchinput searchinput02"  valid="required" errmsg="请填写主要联系人!" runat="server"/>
                        <span id="errMsg_<%=Txt_Contact.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao">*</font>电话：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Phone" id="Txt_Phone" class="searchinput searchinput02" valid="required" errmsg="请填写电话号码!" runat="server"/>
                        <span id="errMsg_<%=Txt_Phone.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao"></font>手机：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Mobile" id="Txt_Mobile" class="searchinput searchinput02" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao"></font>传真：
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Fox" id="Txt_Fox" class="searchinput searchinput02" runat=server/>
                    </td>
                </tr>
                <tr align="left">
                    <td height="35" align="right">
                        <font class="xinghao"></font>备注：
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <textarea name="Txt_Remirke" id="Txt_Remirke" cols="45" rows="5" class="textareastyle" runat="server"></textarea>
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="javascript:void(0);" id="BtnIsSave">保存</a></li>
                <li><a href="javascript:void(0);" id="IsHistory">返回</a> </li>
                <div class="clearboth"></div>
            </ul>
        </div>
    </div>
    <script type="text/javascript">
        var Company = {
            OnSave: function() {
                $.newAjax({
                    type: "POST",
                    url: "/GroupEnd/SystemSetting/CompanyInfo.aspx?issave=1",
                    cache: false,
                    data: $(".addlinebox").find("*").serialize(),
                    dataType: 'json',
                    success: function(msg) {
                        if (msg.isSuccess) {
                            alert("系统设置成功!");
                        }
                        else {
                            alert(msg.errMsg);
                            return false;
                        }
                    },
                    error: function() {
                        alert("操作有误,请重新再试!");
                        return false;
                    }
                });
            },
            OnHistory: function() {
                window.location.href = "";
                return false;
            }
        };


        $(document).ready(function() {
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));
            $("#BtnIsSave").click(function() {
                //验证初始化
                var form = $("#<%=form1.ClientID %>").get(0);
                var validatorshow = ValiDatorForm.validator(form, "span"); //获取提示信息
                if (validatorshow) {
                    Company.OnSave();
                    return true;
                }
                return false;
            });
            $("#IsHistory").click(function() {
                Company.OnHistory();
                return false;
            });
        });
    </script>
  </form>
</asp:Content>
