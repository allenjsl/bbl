<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add_xl_Standard.aspx.cs" Inherits="Web.xianlu.Add_xl_Standard"  MasterPageFile="~/masterpage/Back.Master"%>

<%@ Register Src="~/UserControl/xingcheng.ascx" TagName="xingcheng" TagPrefix="uc1"%>
<%@ Register Src="../UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ProjectControl.ascx" TagName="ProjectControl1" TagPrefix="uc2" %>
<%@ Register src="/UserControl/ConProjectControl.ascx" tagname="ConProjectControl" tagprefix="uc4" %>

<asp:Content ContentPlaceHolderID="c1" ID="Content1" runat="server">
<script src="/js/ValiDatorForm.js" type="text/javascript"></script>
 <form id="form1" runat="server"  enctype="multipart/form-data">
 <input type="hidden" value="1" name="issave" />
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="15%" nowrap="nowrap">
                        <span class="lineprotitle">线路产品库</span>
                    </td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                     所在位置>> 线路产品库
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
            <li><a href="/xianlu/AddLineProducts.aspx" class="">快速发布</a></li>
            <li><a class="tabtwo-on">标准版发布</a></li>           
            <div class="clearboth"></div>
        </ul>
        <div class="addlinebox" id="AddLineboxList">
            <!-- 标准发布版-->
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="con_two_2">
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>线路区域：
                    </td>
                    <td width="86%">
                       <select id="ddlLineType" runat="server" valid="required"  errmsg="请选择线路区域!"></select>
                       <span id="errMsg_<%=ddlLineType.ClientID %>" class="errmsg" ></span>
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>线路名称：
                    </td>
                    <td>
                         <input type="text" name="txt_LineName" id="txt_LineName" class="searchinput searchinput02"  runat="server" valid="required"  errmsg="请输入线路名称!"/> 
                         <span id="errMsg_<%=txt_LineName.ClientID %>" class="errmsg" ></span>                 
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao"></font>线路描述：
                    </td>
                    <td>
                        <input type="text" name="txt_Description" id="txt_Description" class="searchinput searchinput02"  runat="server" />                        
                    </td>
                </tr>
                <tr>
                    <td width="10%" height="30" align="right">
                        <font class="xinghao">*</font>旅游天数：
                    </td>
                    <td>
                        <input type="text" name="txt_Days" na="txt_Days" id="txt_Days" class="searchinput searchinput03"  runat="server" valid="required|range"  errmsg="请输入旅游天数!|天数必须大于0!"  min="1"/>
                        <span id="errMsg_<%=txt_Days.ClientID %>" class="errmsg" ></span>
                    </td>
                </tr>
                <tr>
                  <td colspan="2">
                    <uc1:xingcheng  ID="xingcheng1" runat="server" daysControlName="txt_Days"/>
                  </td>
                </tr>
                <tr>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td height="40" align="right" background="/images/bg003.gif">
                        <font class="xinghao"></font>添加附件：
                    </td>
                    <td background="/images/bg003.gif">
                        <input type="file" name="fileUpLoad" id="fileUpLoad" />
                    </td>
                </tr>
                <tr>
                    <td height="10" colspan="2" align="right">
                       <table width="100%" cellpadding="0" cellspacing="1" border="0">
                          <tr class="odd">
                              <th height="25" align="center" width="13%">&nbsp</th>
                              <th align="center" width="12%">项目</th>
                               <th align="center" width="59%">接待标准</th>
                               <th align="center" width="16%">操作</th>
                          </tr>  
                       </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <table width="100%" border="0" cellspacing="1" cellpadding="0">
                             <tr class="even">
                                  <th height="25" align="center" bgcolor="#E3F1FC" width="13%">包含项目：</th>
                                      <td colspan="5" align="left" bgcolor="#BDDCF4">
                                        <uc4:ConProjectControl ID="ConProjectControl1" runat="server" />                                
                                  </td>
                             </tr>  
                            <tr>
                                <td height="60" align="center" bgcolor="#BDDCF4">不含项目：
                                </td>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="txt_ProjectNo" id="txt_ProjectNo" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="60" align="center" bgcolor="#E3F1FC">购物安排：
                                </td>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="txt_ShoppingPlan" id="txt_ShoppingPlan" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="60" align="center" bgcolor="#BDDCF4">
                                    自费项目：
                                </td>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="txt_ExpenseProject" id="txt_ExpenseProject" class="textareastyle"  runat="server"></textarea>
                                </td>
                            </tr>
                              <tr>
                                <td height="60" align="center" bgcolor="#E3F1FC">
                                    儿童安排：
                                </td>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="txt_ChildrenPlan" id="txt_ChildrenPlan" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="60" align="center" bgcolor="#BDDCF4">
                                    注意事项：
                                </td>
                                <td colspan="3" align="left" bgcolor="#BDDCF4">
                                    <textarea name="txt_Notes" id="txt_Notes" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="60" align="center" bgcolor="#E3F1FC">
                                    温馨提醒：
                                </td>
                                <td colspan="3" align="left" bgcolor="#E3F1FC">
                                    <textarea name="Txt_Reminder" id="Txt_Reminder" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="4" align="center">
                                </td>
                            </tr>
                            <tr bgcolor="#BDDCF4">
                                <th height="30" colspan="4" align="center" style="border: 1px #93B5D7 solid;">
                                    内部信息（内部注意事项，内部资料等）
                                </th>
                            </tr>
                            <tr>
                                <td height="10" colspan="4" align="center">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="left">
                                    <textarea name="Txt_Infromation" id="Txt_Infromation" class="textareastyle" runat="server"></textarea>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <ul class="tjbtn">
                <li><a href="javascript:viod(0);" id="AddXl_Save">保存</a></li>
                <li><a href="javascript:viod(0);" id="AddXl_History">返回</a> </li>
                <div class="clearboth">
                </div>
            </ul>
        </div>
    </div>    
    <asp:Literal ID="litMsg" runat="server"></asp:Literal>
    
    <script type="text/javascript" language="javascript">
        var AddXlStandard = {
            OnSave: function() {
                $("#<%=form1.ClientID %>").submit();
            },
            OnHistory: function() {
                window.location.href = "/xianlu/LineProducts.aspx";
                return false;
            }
        };
        $(document).ready(function() {
            //获取表单验证
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));

            //保存
            $("#AddXl_Save").click(function() {
                var form = $("#<%=form1.ClientID %>").get(0);
                var vResult = ValiDatorForm.validator(form, "span");
                if (vResult) {
                    AddXlStandard.OnSave();                    
                }
                return false;
            });

            //返回
            $("#AddXl_History").click(function() {
                AddXlStandard.OnHistory();
                return false;
            });
        });
    </script>
    
  </form>
</asp:Content>

