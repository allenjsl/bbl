<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddLineProducts.aspx.cs"
    Inherits="Web.xianlu.AddLineProducts" MasterPageFile="~/masterpage/Back.Master"  ValidateRequest="false" %>
<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="c1" ID="Content1" runat="server">

 <form id="form1" runat="server" enctype="multipart/form-data">
   <input type="hidden" value="1" name="issave"/>
    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>
    
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">��·��Ʒ��</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                         ����λ��>> ��·��Ʒ��
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
            <li><a class="tabtwo-on">���ٷ���</a></li>
            <li><a href="/xianlu/Add_xl_Standard.aspx" class="">��׼�淢��</a></li>     
            <div class="clearboth"></div>
        </ul>
        <div class="addlinebox" id="AddLineboxList">
            <table width="100%" cellspacing="0" cellpadding="0" border="0" id="con_two_1">
                <tr>
                    <td height="30" width="15%" align="right">
                        <font class="xinghao">*</font>��·����
                    </td>
                    <td width="86%">
                      <select id="ddlLineType" runat="server" valid="required"  errmsg="��ѡ����·����!"></select>
                       <span id="errMsg_<%=ddlLineType.ClientID %>" class="errmsg" ></span>
                    </td>
                </tr>
                <tr>
                    <td height="30" width="15%" align="right">
                        <font class="xinghao">*</font>��·���ƣ�
                    </td>
                    <td>
                      <input type="text" class="searchinput searchinput02" id="txt_LineName" name="txt_LineName" runat="server" valid="required"  errmsg="����д��·����!" />
                      <span id="errMsg_<%=txt_LineName.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr>
                    <td height="30" width="15%" align="right">
                        <font class="xinghao"></font>��·������
                    </td>
                    <td>
                        <input type="text" class="searchinput searchinput02" id="txt_Description" name="txt_Description" runat="server">
                    </td>
                </tr>
                <tr>
                    <td height="30" width="15%" align="right">
                        <font class="xinghao">*</font>����������
                    </td>
                    <td>
                        <input type="text" class="searchinput searchinput03" id="txt_Days" name="txt_Days" runat="server" valid="required|range"  errmsg="����д��������!|�����������0!" min="1" />
                        <span id="errMsg_<%=txt_Days.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
               <tr>
                    <td height="40" align="right">
                        <font class="xinghao"></font>��Ӹ�����
                    </td>
                    <td>
                        <input type="file" name="fileUpLoad" id="fileUpLoad" />
                    </td>
                </tr>
                <tr>
                    <td width="15%" align="right">
                        <font class="xinghao"></font>�г̰��ţ�
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Travel" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="hr_10" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td width="15%" align="right">
                        <font class="xinghao"></font>�����׼��
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Services" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="hr_10" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td width="15%" align="right">
                        <font class="xinghao"></font>��ע��
                    </td>
                    <td>
                        <textarea name="txt_Remarks" id="txt_Remarks" class="txt_Remarks" runat="server" rows="5" cols="70"></textarea>
                    </td>
                </tr>
                <tr>
                    <td height="10" align="right" colspan="2">
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="javascript:viod(0);" id="AddXl_Save">����</a></li>
                <li><a href="javascript:viod(0);" id="AddXl_History">����</a> </li>
                <div class="clearboth"></div>
            </ul>
        </div>
    </div>
    
    <script src="../js/kindeditor/kindeditor.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
     //��ʼ���༭��
        KE.init({
        id: '<%=txt_Travel.ClientID %>', //�༭����Ӧ�ı���id
            width: '630px',
            height: '240px',
            skinsPath: '../js/kindeditor/skins/',
            pluginsPath: '../js/kindeditor/plugins/',
            scriptPath: '../js/kindeditor/skins/',
            resizeMode: 0, //��߲��ɱ�
            items: keMore //����ģʽ(keMore:�๦��,keSimple:����)
        });
        KE.init({
        id: '<%=txt_Services.ClientID %>', //�༭����Ӧ�ı���id
            width: '630px',
            height: '240px',
            skinsPath: '../js/kindeditor/skins/',
            pluginsPath: '../js/kindeditor/plugins/',
            scriptPath: '../js/kindeditor/skins/',
            resizeMode: 0, //��߲��ɱ�
            items: keMore //����ģʽ(keMore:�๦��,keSimple:����)
        });

        var AddXlPlan = {
            OnSave: function() {
                $("#<%=form1.ClientID %>").submit();
            },
            OnHistory: function() {
                window.location.href = "/xianlu/LineProducts.aspx";
                return false;
            }
        };

        $(document).ready(function() {
            KE.create('<%=txt_Travel.ClientID %>', 0); //�����༭��
            KE.create('<%=txt_Services.ClientID %>', 0); //�����༭��

            //��ȡ����֤
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));

            $("#AddXl_Save").click(function() {
                var form = $("#<%=form1.ClientID %>").get(0);
                var vResult = ValiDatorForm.validator(form, "span");
                if (vResult) {
                    AddXlPlan.OnSave();
                }
                return false;
            });
            $("#AddXl_History").click(function() {
                AddXlPlan.OnHistory();
                return false;
            });
        });
    </script>
    
  </form>
</asp:Content>
