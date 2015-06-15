<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Web.administrativeCenter.personnelFiles.Edit" Title="行政中心_人事档案" %>

<%@ Register Src="/UserControl/UCSelectDepartment.ascx" TagPrefix="cc1" TagName="SDepartment" %>
<%@ Register  Src="/UserControl/ProvinceList.ascx" TagName="ucProvince" TagPrefix="uc1" %>
<%@ Register  Src="/UserControl/CityList.ascx" TagName="ucCity" TagPrefix="uc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="EditForm" runat="server" name="EditForm" style="margin:0; padding:0;" enctype="multipart/form-data">
    <div class="mainbody">
        <div class="lineprotitlebox">
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">行政中心>>人事档案</td>
              </tr>
              <tr>
                <td colspan="2" height="2" bgcolor="#000000"></td>
              </tr>
          </table>  
        </div>
        <div class="lineCategorybox" style="height:30px;">
        </div>
   	    <div class="tablelist">
            <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
              <tr>
                <th colspan="6" align="center" bgcolor="#BDDCF4">==========基本信息==========</th>
              </tr>
              <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc">
                    <span style=" color:Red;">*</span>
                    <strong>档案编号：</strong></td>
                <td width="22%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_FileNo" id="txt_FileNo" type="text" size="20" class="searchinput2"
                        valid="required" errmsg="请输入档案编号" value="<%=FileNo %>" /><br />
                    <span id="errMsg_txt_FileNo"  style="color:Red;"></span>
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <span style=" color:Red;">*</span>
                    <strong>姓 名：</strong>
                </td>
                <td width="25%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Name" id="txt_Name" type="text" size="18" class="searchinput2" 
                        valid="required" errmsg="请输入姓名" value="<%=Name %>" /><br />
                    <span id="errMsg_txt_Name"  style="color:Red;"></span>
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>性 别：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                     <select id="dpSex" name="dpSex" runat="server" >
                      <option value="0" selected="selected">--请选择--</option>
                      <option value="1">女</option>
                      <option value="2">男</option>
                    </select>
                </td>
              </tr>
		      <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>身份证号：</strong></td>
                <td width="22%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_CardID" id="txt_CardID"  type="text" size="20" class="searchinput2"  value="<%=CardID %>"
                     onfocus="PersonFileEdit.GetBirthDay('txt_CardID');" onblur="PersonFileEdit.GetBirthDay('txt_CardID');" />
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>出生日期：</strong>
                </td>
                <td width="25%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Birthday" id="txt_Birthday"  type="text" size="18" class="searchinput2" onfocus="WdatePicker()" value="<%=string.Format("{0:yyyy-MM-dd}",Birthday) %>" />
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>所属部门：</strong>
                </td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <cc1:SDepartment id="ucDepartemnt" runat="server"></cc1:SDepartment>
                </td>
		      </tr>
		      <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>员工照片：</strong></td>
                <td height="35" colspan="3" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input type="file"  id="FileEmployeePicture" name="FileEmployeePicture" 
                     style="background-color:#CBCBCB;float:left;"  />&nbsp;&nbsp;&nbsp;&nbsp;<div id="file" style="float:left" runat="server"><%=WorkerPicture %><img id="del_file" style="cursor:pointer;" src="../../images/closebox2.gif" /></div>
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>职 务：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                   <asp:DropDownList ID="ddlJobPostion" runat="server" name="ddlJobPostion" Width="150px" ></asp:DropDownList>
                </td>
              </tr>
		      <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>类 型：</strong></td>
                <td width="22%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <select id="dpWorkerType" name="dpWorkerType"  runat="server" >
                      <option value="0">正式员工</option>
                      <option value="1" selected="selected">试用期</option>
                      <option value="2">学徒期</option>
                    </select>
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>员工状态：</strong>
                </td>
                <td width="25%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <select id="dpWorkerState" name="dpWorkerState" runat="server">
                      <option value="0" selected="selected">在职</option>
                      <option value="1">离职</option>
                    </select>
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>入职日期：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_EntryDate" id="txt_EntryDate" onfocus="WdatePicker()" type="text" size="18" class="searchinput2"  value="<%=EntryDate %>" onchange="PersonFileEdit.GetWorkerYear();" />
                </td>
              </tr>
		      <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>工 龄：</strong></td>
                <td width="22%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <label id="lblWorkerYear">0</label>&nbsp;&nbsp;年
                    <%--<input name="dpWorkLife" id="dpWorkLife"  type="text" size="18" class="searchinput2" value="<%=dpWorkLife %>" />--%>
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>婚姻状态：</strong>
                </td>
                <td width="25%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <select id="dpMarriageState" runat="server">
                      <option value="0" selected="selected">未婚</option>
                      <option value="1">已婚</option>
                    </select>
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>离职日期：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_LeftDate" id="txt_LeftDate"  type="text" onfocus="WdatePicker()" size="18" class="searchinput2" value="<%=LeftDate %>" />
                </td>
              </tr>
		      <tr>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>籍 贯：</strong>
                </td>
                <td width="26%" colspan="3" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <uc1:ucProvince id="ucProvince1" runat="server"></uc1:ucProvince>
                    <uc2:ucCity id="ucCity1" runat="server"></uc2:ucCity>
                </td>
                 <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>民 族：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_National" id="txt_National"  type="text" size="18" class="searchinput2"  value="<%=National %>" />
                </td>
              </tr>
		      <tr>
		        <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>政治面貌：</strong></td>
                <td width="20%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Political" id="txt_Political" type="text" size="18" class="searchinput2" value="<%=Political %>" />
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>手 机：</strong>
                </td>
                <td width="25%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Mobile" id="txt_Mobile"  type="text" size="18" class="searchinput2" value="<%=Mobile %>" />
                </td>
                <td width="10%" height="35" align="right" bgcolor="#e3f1fc" class="pandl3"><strong>QQ：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_QQ" id="txt_QQ"  type="text" size="18" class="searchinput2" value="<%=QQ %>" />
                </td>
              </tr>
		      <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>MSN：</strong></td>
                <td width="22%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_MSN" id="txt_MSN"  type="text" size="18" class="searchinput2" value="<%=MSN %>" />
                </td>
                <td width="10%" height="35" colspan="-1" align="right" bgcolor="#e3f1fc" class="pandl3">
                    <strong>E-mail：</strong></td>
                <td height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Email" id="txt_Email"  type="text" size="18" class="searchinput2" value="<%=Email %>" />
                </td>
                 <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>联系电话：</strong></td>
                <td width="23%" height="35" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Telephone"  id="txt_Telephone" type="text" size="18" class="searchinput2" value="<%=Telephone %>" />
                </td>
              </tr>
              <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>住 址：</strong></td>
                <td height="35" colspan="5" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Address" id="txt_Address" class="searchinput2"  type="text" size="80" value="<%=Address %>" />
                </td>
              </tr>
              <tr>
                <td width="10%" height="35"  align="right" bgcolor="#e3f1fc"><strong>备  注：</strong></td>
                <td height="35" colspan="5" align="left" bgcolor="#FAFDFF" class="pandl3">
                    <input name="txt_Remark" id="txt_Remark" class="searchinput2"  type="text" size="80" value="<%=Remark %>" />
                </td>
              </tr>
            </table>
            
		    <table id="tab_Record" width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
              <tr>
                <th colspan="8" align="center" bgcolor="#BDDCF4">==========学历信息==========</th>
              </tr>
              <tr>
                <td width="16%" height="35"  align="center" bgcolor="#e3f1fc"><strong>开始时间</strong></td>
                <td width="16%" height="35" align="center" bgcolor="#e3f1fc"><strong>结束时间</strong></td>
                <td width="8%" height="35" colspan="-1" align="center" bgcolor="#e3f1fc"><strong>学历</strong></td>
                <td width="13%" height="35" align="center" bgcolor="#e3f1fc"><strong>所学专业</strong></td>
                <td width="13%" height="35" align="center" bgcolor="#e3f1fc"><strong>毕业院校</strong></td>
                <td width="8%" height="35" align="center" bgcolor="#e3f1fc"><strong>状态</strong></td>
                <td width="18%" align="center" bgcolor="#e3f1fc"><strong>备注</strong></td>
                <td width="7%" align="center" bgcolor="#e3f1fc"><strong>操作</strong></td>
              </tr>
              <asp:Repeater ID="rpt_Record" runat="server" >
                <ItemTemplate>
                    <tr>
                        <td width="15%" height="35"  align="center" bgcolor="#FAFDFF">
                            <input name="txt_RecordStartDate" class="searchinput2" onfocus="WdatePicker()" size="10" type="text" value="<%# string.Format("{0:yyyy-MM-dd}", Eval("StartDate"))%>" > 
                        </td>
                        <td width="15%" height="35" align="center" bgcolor="#FAFDFF">
                            <input name="txt_RecordEndDate" class="searchinput2" onfocus="WdatePicker()" size="10" type="text"  value="<%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%>">
                        </td>
                        <td width="9%" height="35" colspan="-1" align="center" bgcolor="#FAFDFF">
                            <select name="EducationGrade">
                              <%# GetEducationGrade((int)((EyouSoft.Model.EnumType.AdminCenterStructure.DegreeType)Eval("Degree")))%>
                            </select>
                        </td>
                        <td width="13%" height="35" align="center" bgcolor="#FAFDFF">
                            <input name="txt_Profession" type="text" size="13" class="searchinput2" value="<%# Eval("Professional")%>" />
                        </td>
                        <td width="13%" height="35" align="center" bgcolor="#FAFDFF">
                            <input name="txt_Graduation" type="text" size="13" class="searchinput2" value="<%# Eval("SchoolName")%>" />
                        </td>
                        <td width="10%" height="35" align="center" bgcolor="#FAFDFF">
                            <select name="EducationState"> 
                                <%# GetEducationState((bool)(Eval("StudyStatus")))%>                    
                            </select>
                        </td>
                        <td width="18%" align="center" bgcolor="#FAFDFF">
                            <input name="txt_RecordRemark" type="text" size="18" class="searchinput2" value="<%# Eval("Remark")%>" />
                        </td>
                        <td width="7%" align="center" bgcolor="#FAFDFF" class="add-xz">
                            <a href="javascript:void(0);" onclick="PersonFileEdit.DeleteRow(this,1);">删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
              </asp:Repeater>
		      <tr>
                <td width="15%" height="35"  align="center" bgcolor="#FAFDFF">
                    <input name="txt_RecordStartDate" class="searchinput2" onfocus="WdatePicker()" size="10" type="text" >
                </td>
                <td width="15%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_RecordEndDate" class="searchinput2" onfocus="WdatePicker()" size="10" type="text" >
                </td>
                <td width="9%" height="35" colspan="-1" align="center" bgcolor="#FAFDFF">
                    <select name="EducationGrade">
                      <option value="0">初中</option>
                      <option value="1">高中</option>
                      <option value="2">中专</option>
                      <option value="3" selected='selected'>专科</option>
                      <option value="4">本科</option>
                      <option value="5">硕士</option>
                      <option value="6">博士</option>
                    </select>
                </td>
                <td width="13%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_Profession" type="text" size="13" class="searchinput2" />
                </td>
                <td width="13%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_Graduation" type="text" size="13" class="searchinput2" />
                </td>
                <td width="10%" height="35" align="center" bgcolor="#FAFDFF">
                    <select name="EducationState">
                      <option value="1">毕业</option>
                      <option value="0">在读</option>
                    </select>
                </td>
                <td width="18%" align="center" bgcolor="#FAFDFF">
                    <input name="txt_RecordRemark" type="text" size="18" class="searchinput2" />
                </td>
                <td width="7%" align="center" bgcolor="#FAFDFF" class="add-xz">
                    <a href="javascript:void(0);" onclick="PersonFileEdit.AddRow('Record',this);">添加</a>
                </td>
            </tr>
            </table>
            
		    <table id="tab_Resume" width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
              <tr>
                <th colspan="7" align="center" bgcolor="#BDDCF4">==========履历信息==========</th>
              </tr>
              <tr>
                <td width="16%" height="35"  align="center" bgcolor="#e3f1fc"><strong>开始时间</strong></td>
                <td width="16%" height="35" align="center" bgcolor="#e3f1fc"><strong>结束时间</strong></td>
                <td width="15%" height="35" colspan="-1" align="center" bgcolor="#e3f1fc"><strong>工作地点</strong></td>
                <td width="15%" height="35" align="center" bgcolor="#e3f1fc"><strong>工作单位</strong></td>
                <td width="14%" height="35" align="center" bgcolor="#e3f1fc"><strong>从事职业</strong></td>
                <td width="17%" height="35" align="center" bgcolor="#e3f1fc"><strong>备注</strong></td>
                <td width="7%" align="center" bgcolor="#e3f1fc"><strong>操作</strong></td>
              </tr>
              <asp:Repeater ID="rpt_Resume" runat="server">
                <ItemTemplate>
		          <tr>
                    <td width="16%" height="35"  align="center" bgcolor="#FAFDFF">
                        <input name="txt_ResumeStartDate" onfocus="WdatePicker()" class="searchinput2" size="10" value="<%# string.Format("{0:yyyy-MM-dd}", Eval("StartDate"))%>"  >
                    </td>
                    <td width="16%" height="35" align="center" bgcolor="#FAFDFF">
                        <input name="txt_ResumeEndDate" onfocus="WdatePicker()" class="searchinput2" size="10" value="<%# string.Format("{0:yyyy-MM-dd}", Eval("EndDate"))%>">
                    </td>
                    <td width="15%" height="35" colspan="-1" align="center" bgcolor="#FAFDFF">
                        <input name="txt_WorkPlace" type="text" size="13" class="searchinput2" value="<%# Eval("WorkPlace") %>" />
                    </td>
                    <td width="15%" height="35" align="center" bgcolor="#FAFDFF">
                        <input name="txt_WorkUnit" type="text" size="13" class="searchinput2" value="<%# Eval("WorkUnit") %>" />
                    </td>
                    <td width="14%" height="35" align="center" bgcolor="#FAFDFF">
                        <input name="txt_Job" type="text" size="13" class="searchinput2" value="<%# Eval("TakeUp") %>" />
                    </td>
                    <td width="17%" height="35" align="center" bgcolor="#FAFDFF">
                        <input name="txt_ResumeRemark" type="text" size="18" class="searchinput2" value="<%# Eval("Remark") %>" />
                    </td>
                    <td width="7%" align="center" bgcolor="#FAFDFF">
                        <span class="add-xz"><a href="javascript:void(0);" onclick="PersonFileEdit.DeleteRow(this);">删除</a></span>
                    </td>
                  </tr>
                </ItemTemplate>
              </asp:Repeater>
		      <tr>
                 <td width="16%" height="35"  align="center" bgcolor="#FAFDFF">
                    <input name="txt_ResumeStartDate" onfocus="WdatePicker()" class="searchinput2" size="10"  >
                </td>
                <td width="16%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_ResumeEndDate" onfocus="WdatePicker()" class="searchinput2" size="10">
                </td>
                <td width="15%" height="35" colspan="-1" align="center" bgcolor="#FAFDFF">
                    <input name="txt_WorkPlace" type="text" size="13" class="searchinput2" />
                </td>
                <td width="15%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_WorkUnit" type="text" size="13" class="searchinput2" />
                </td>
                <td width="14%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_Job" type="text" size="13" class="searchinput2" />
                </td>
                <td width="17%" height="35" align="center" bgcolor="#FAFDFF">
                    <input name="txt_ResumeRemark" type="text" size="18" class="searchinput2" />
                </td>
                <td width="7%" align="center" bgcolor="#FAFDFF">
                    <span class="add-xz"><a href="javascript:void(0);" onclick="PersonFileEdit.AddRow('',this);">添加</a></span>
                </td>
              </tr>
            </table>
       
   	        <table width="780" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#BDDCF4">
              <tr>
                <td height="30" colspan="6" align="center" >
                    <div id="submitAdd" style="display:none">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                      <td width="86" height="40" align="center" class="tjbtn02">
                        <a href="javascript:void(0);" onclick="return PersonFileEdit.Save('Save');">保存</a>
                      </td>
				     <%-- <td width="158" height="40" align="center" class="jixusave">
                        <a href="javascript:void(0);" onclick="return PersonFileEdit.Save('SaveAndAdd');">保存并继续添加</a>
				      </td>--%>
                      <td width="86" height="40" align="center" class="tjbtn02">
                        <a href="/administrativeCenter/personnelFiles/Default.aspx">返回</a>
                      </td>
                    </tr>
                    </table>
                    </div>
                    <div id="submitUpdate"  style="display:none">
                    <table border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                          <td width="86" height="40" align="center" class="tjbtn02">
                            <a href="javascript:void(0);" onclick="return PersonFileEdit.Save('Update');">修改</a></td>
                          <td width="86" height="40" align="center" class="tjbtn02">
                            <a href="/administrativeCenter/personnelFiles/Default.aspx">返回</a>
                          </td>
                        </tr>
                    </table>
                    </div>
                </td>
            </tr>
          </table>
          
   	  </div>
	</div>
	<input id="hiddenID"  type="hidden" runat="server" value="" />
    <input id="hiddenMethod" name="hiddenMethod" type="hidden" value="" />
    <input id="hiddenPhoto" name="hiddenPhoto" type="hidden" runat="server" value="" />
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script src="/js/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/js/back.js" type="text/javascript"></script>
    <script type="text/javascript">
        var PersonFileEdit = {
            Save: function(Method) {
                if (Method == "SaveAndAdd") {//保存并继续添加
                    document.getElementById("hiddenMethod").value = "SaveAndAdd";
                }
                else if (Method == "Update") {//修改
                    document.getElementById("hiddenMethod").value = "Update";
                }
                else if (Method == "Save") {
                    document.getElementById("hiddenMethod").value = "Save";
                }
                //验证表单必填项
                var isValidator = ValiDatorForm.validator($("#<%=EditForm.ClientID %>").get(0), "span");
                if (!isValidator) {
                    return false;
                }

                document.forms["<%=EditForm.ClientID %>"].submit();
                return false;
            },
            GetBirthDay: function(txt_CardID) {//如果省份证号码合法得到生日
                var CardID = $("#" + txt_CardID).val();
                var reg = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                if (reg.test(CardID)) {
                    var birthday = CardID.slice(6, 10) + "-" + CardID.slice(10, 12) + "-" + CardID.slice(12, 14);

                    $("#txt_Birthday").attr("value", birthday);
                }
            },
            AddRow: function(Type, Obj) {//添加行
                var str_Record = [];
                var str_Resume = [];
                if (Type == "Record") {//学历信息
                    var myTable = $("#tab_Record");
                    str_Record.push("<tr><td width=\"15%\" height=\"35\"  align=\"center\" bgcolor=\"#FAFDFF\"><input class=\"searchinput2\" size=\"10\" type=\"text\" name=\"txt_RecordStartDate\" onfocus=\"WdatePicker()\"></td>");
                    str_Record.push("<td width=\"15%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\"><input class=\"searchinput2\" size=\"10\" type=\"text\" name=\"txt_RecordEndDate\" onfocus=\"WdatePicker()\"></td>");
                    str_Record.push("<td width=\"9%\" height=\"35\" colspan=\"-1\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Record.push("<select name=\"EducationGrade\">");
                    str_Record.push("<option value=\"0\">初中</option>");
                    str_Record.push("<option value=\"1\">高中</option>");
                    str_Record.push("<option value=\"2\">中专</option>");
                    str_Record.push("<option value=\"3\" selected='selected'>专科</option>");
                    str_Record.push("<option value=\"4\">本科</option>");
                    str_Record.push("<option value=\"5\">硕士</option>");
                    str_Record.push("<option value=\"6\">博士</option></select></td>");
                    str_Record.push("<td width=\"13%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Record.push("<input name=\"txt_Profession\" type=\"text\" size=\"13\" class=\"searchinput2\" /></td>");
                    str_Record.push("<td width=\"13%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Record.push("<input name=\"txt_Graduation\" type=\"text\" size=\"13\" class=\"searchinput2\" /></td>");
                    str_Record.push("<td width=\"10%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Record.push("<select name=\"EducationState\">");
                    str_Record.push("<option value=\"1\">毕业</option>");
                    str_Record.push("<option value=\"0\">在读</option></select></td>");
                    str_Record.push("<td width=\"18%\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Record.push("<input name=\"txt_RecordRemark\" type=\"text\" size=\"18\" class=\"searchinput2\" /></td>");
                    str_Record.push("<td width=\"7%\" align=\"center\" bgcolor=\"#FAFDFF\" class=\"add-xz\">");
                    str_Record.push("<a href=\"javascript:void(0);\" onclick=\"PersonFileEdit.DeleteRow(this)\">删除</a></td></tr>");
                    $(Obj).closest("tr").before(str_Record.join(''));
                }
                if (Type == "") {//履历信息
                    var myTable = $("#tab_Resume");
                    str_Resume.push("<tr><td width=\"16%\" height=\"35\"  align=\"center\" bgcolor=\"#FAFDFF\"><input name=\"txt_ResumeStartDate\" class=\"searchinput2\" size=\"10\" onfocus=\"WdatePicker()\"></td>");
                    str_Resume.push("<td width=\"16%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\"><input name=\"txt_ResumeEndDate\" class=\"searchinput2\" size=\"10\" onfocus=\"WdatePicker()\"></td>");
                    str_Resume.push("<td width=\"15%\" height=\"35\" colspan=\"-1\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Resume.push("<input name=\"txt_WorkPlace\" type=\"text\" size=\"13\" class=\"searchinput2\" /></td>");
                    str_Resume.push("<td width=\"15%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Resume.push("<input name=\"txt_WorkUnit\" type=\"text\" size=\"13\" class=\"searchinput2\" /></td>");
                    str_Resume.push("<td width=\"14%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Resume.push("<input name=\"txt_Job\" type=\"text\" size=\"13\" class=\"searchinput2\" /></td>");
                    str_Resume.push("<td width=\"17%\" height=\"35\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Resume.push("<input name=\"txt_ResumeRemark\" type=\"text\" size=\"18\" class=\"searchinput2\" /></td>");
                    str_Resume.push("<td width=\"7%\" align=\"center\" bgcolor=\"#FAFDFF\">");
                    str_Resume.push("<span class=\"add-xz\"><a href=\"javascript:void(0);\" onclick=\"PersonFileEdit.DeleteRow(this)\">删除</a></span></td></tr>");
                    $(Obj).closest("tr").before(str_Resume.join(''));
                }
            },
            DeleteRow: function(Obj) { //删除行
                $(Obj).closest("tr").remove();
            },
            GetWorkerYear: function() {        //获取工龄
                var timeArr = $("#txt_EntryDate").val().split("-");
                var lblWorkerYear = $("#lblWorkerYear");
                if (timeArr.length >= 3) {
                    var entryDate = new Date(timeArr[0], timeArr[1] - 1, timeArr[2]);
                    var nowDate = serverDate;
                    var timeDiff = nowDate.getTime() - entryDate.getTime();
                    var workerYear = timeDiff / (1000 * 60 * 60 * 24 * 365);
                    lblWorkerYear.html(Math.round(workerYear * 10) / 10);
                }
            }
        };
        $(function() {
            PersonFileEdit.GetWorkerYear();
            var ID = $("#<%=hiddenID.ClientID %>").val();
            if (ID == "-1") {//初始化
                document.getElementById("submitUpdate").style.display = "none";
                document.getElementById("submitAdd").style.display = "";
            } else {
                document.getElementById("submitAdd").style.display = "none";
                document.getElementById("submitUpdate").style.display = "";
            }
            //得到焦点
            FV_onBlur.initValid($("#<%=EditForm.ClientID %>").get(0));
        });
        $("#del_file").click(function(){
            if(confirm("确定要删除么?"))
            {
             $("#<%=hiddenPhoto.ClientID %>").val("");
             $("#<%=file.ClientID %>").css("display","none");
            }
        })
    </script>
    </form>
</asp:Content>
