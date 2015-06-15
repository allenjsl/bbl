<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeployManager.aspx.cs"
    Inherits="Web.GroupEnd.SystemSetting.DeployManager" MasterPageFile="~/masterpage/Front.Master" %>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
<form id="configForm" runat="server" enctype="multipart/form-data">
   <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
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
              <li><a  href="AccountManager.aspx" class="">帐号管理</a></li>
              <li><a href="CompanyInfo.aspx" class="">公司信息</a></li>            
              <li><a class="tabtwo-on">配置管理</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="addlinebox">
            <!-- 配置管理-->
             <input type="hidden" id="hidMethod" value="save" name="hidMethod" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_3">
                <tr>
                    <td colspan="3" height="10">
                    </td>
                </tr>
                 <tr>
                   <td width="16%" height="35"  align="right" bgcolor="#e3f1fc">公司Logo</td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file" name="fileLogo" id="fileLogo" />&nbsp;<%=PageLogo%>
                    <span id="hMess" style="color:Red; display:none;">图片格式不正确</span>
                    <input type="hidden" id="hidfileLogo" runat="server" />
                    </td>
                </tr>
                <tr>
                   <td width="16%" height="35"  align="right" bgcolor="#e3f1fc">打印页眉</td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file" name="fileHeader" id="fileHeader" />&nbsp;<%=pageHeader %>
                    <span id="hMess" style="color:Red; display:none;">图片格式不正确</span>
                    <input type="hidden" id="hidFileHeader" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc">打印页脚</td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file" name="fileFooter" id="fileFooter" />&nbsp;<%=pageFooter%>
                    <span id="hMess" style="color:Red; display:none;">图片格式不正确</span>
                    <input  type="hidden" id="hidFileFooter" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td width="16%" height="35"  align="right" bgcolor="#e3f1fc">打印模板</td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file" name="fileModel" id="fileModel" />&nbsp;<%=pageModel%>
                    <span id="hMess" style="color:Red; display:none;">图片格式不正确</span>
                    <input  type="hidden" id="hidFileModel" runat="server"/>
                    </td>
                </tr>
                <tr>
                     <td width="16%" height="35"  align="right" bgcolor="#e3f1fc">打印公章</td>
                    <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file" name="fileSeal" id="fileSeal" />&nbsp;<%=departSeal%>
                    <span id="Span3" style="color:Red; display:none;">图片格式不正确</span>
                    <input  type="hidden" id="hidfileSeal" runat="server"/>
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="Javascript:Void(0);"  onclick="return Save();">保存</a></li>
                <li><a href="Javascript:Void(0);" id="IsHistory">返回</a> </li>
                <div class="clearboth"></div>
            </ul>
        </div>
    </div>
    <script type="text/javascript">
        function Save() {
            // 验证上传文件格式
            var mess = "";
            var hV = $("#fileHeader").val();//页眉
            var fV = $("#fileFooter").val(); //页脚
            var sV = $("#fileSeal").val(); //公章
            var mV = $("#fileModel").val(); //模板
            var logo = $("#fileLogo").val(); //公司logo
            if (logo != "") {
                if (!/.jpg|.jpeg|.bmp|.gif$/i.test(logo)) { mess += "公司logo格式不正确\n"; }
            }
            if (hV != "") {
                if (!/.jpg|.jpeg|.bmp|.gif$/i.test(hV)) { mess += "页眉格式不正确\n"; }
            }
            if (fV != "") {
                if (!/.jpg|.jpeg|.bmp|.gif$/i.test(fV)) { mess += "页脚格式不正确\n"; }
            }
            if (sV != "") {
                if (!/.jpg|.jpeg|.bmp|.gif$/i.test(sV)) { mess += "公章格式不正确\n"; }
            }
            if (mV != "") {
                if (!/.dot|.doc|.docx|.gif$/i.test(mV)) { mess += "模板格式不正确\n"; }
            }            
            if (mess != "") {
                alert(mess);
                return false;
            }
            $("#<%=configForm.ClientID %>").submit();
            return false;
        }
    </script>
  </form>
</asp:Content>
