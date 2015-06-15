<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="Web.GroupEnd.SystemSetting.CompanyInfo"
    MasterPageFile="~/masterpage/Front.Master"  Title="ϵͳ����"%>
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
                        <span class="lineprotitle">ϵͳ����</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                        ����λ��>> ϵͳ����
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
              <li><a href="AccountManager.aspx" class="">�ʺŹ���</a></li>
              <li><a href="Javascript:void(0);" class="tabtwo-on">��˾��Ϣ</a></li>            
              <li><a href="DeployManager.aspx" class="">���ù���</a></li>
            <div class="clearboth"></div>
        </ul>
        <div class="addlinebox">
            <!--��˾��Ϣ-->
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_2">
                <tr>
                    <td width="10%" height="40" align="right">
                        <font class="xinghao"></font>ʡ�ݣ�
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
                        <font class="xinghao"></font>���У�
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
                        <font class="xinghao">*</font>��λ���ƣ�
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <input type="text" name="Txt_UnitsName" id="Txt_UnitsName" class="searchinput searchinput02" runat="server" valid="required" errmsg="����д��λ����!"/>
                        <span id="errMsg_<%=Txt_UnitsName.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="35" align="right">
                        <font class="xinghao"></font>���֤�ţ�
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
                        <font class="xinghao"></font>��˾��ַ��
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
                        <font class="xinghao"></font>�ʱࣺ
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
                        <font class="xinghao"></font>�����ʺţ�
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
                        <font class="xinghao"></font>��Ӷ���ͣ�
                    </td>
                    <td width="2%">
                        &nbsp;
                    </td>
                    <td>
                        <asp:DropDownList ID="DDlReBateType" runat="server">
                             <asp:ListItem Text="��ѡ��Ӷ����" Value="0"></asp:ListItem>
                             <asp:ListItem Text="�ַ�" Value="1"></asp:ListItem>
                             <asp:ListItem Text="��" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao">*</font>��Ҫ��ϵ�ˣ�
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Contact" id="Txt_Contact" class="searchinput searchinput02"  valid="required" errmsg="����д��Ҫ��ϵ��!" runat="server"/>
                        <span id="errMsg_<%=Txt_Contact.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao">*</font>�绰��
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                        <input type="text" name="Txt_Phone" id="Txt_Phone" class="searchinput searchinput02" valid="required" errmsg="����д�绰����!" runat="server"/>
                        <span id="errMsg_<%=Txt_Phone.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="35" align="right">
                        <font class="xinghao"></font>�ֻ���
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
                        <font class="xinghao"></font>���棺
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
                        <font class="xinghao"></font>��ע��
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
                <li><a href="javascript:void(0);" id="BtnIsSave">����</a></li>
                <li><a href="javascript:void(0);" id="IsHistory">����</a> </li>
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
                            alert("ϵͳ���óɹ�!");
                        }
                        else {
                            alert(msg.errMsg);
                            return false;
                        }
                    },
                    error: function() {
                        alert("��������,����������!");
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
                //��֤��ʼ��
                var form = $("#<%=form1.ClientID %>").get(0);
                var validatorshow = ValiDatorForm.validator(form, "span"); //��ȡ��ʾ��Ϣ
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
