<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTrainingPlan.aspx.cs" Inherits="Web.administrativeCenter.trainingPlan.EditTrainingPlan" ValidateRequest="false" %>

<%@ Import Namespace="EyouSoft.Common" %>
<%@ Register Src="/UserControl/UCSelectDepartment.ascx" TagPrefix="cc1" TagName="selectDepartment" %>
<%@ Register Src="/UserControl/selectOperator.ascx" TagPrefix="cc2" TagName="selectOperator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/kindeditor/kindeditor.js" cache="true" ></script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="760" border="0" align="center" cellpadding="0" cellspacing="1" 
        style="margin-top:20px;">
      <tr class="odd">
        <th width="17%" height="30" align="right"><font color="#FF0000">*</font>计划标题：</th>
        <td height="30" colspan="3" bgcolor="#E3F1FC">
            <input name="txt_PlanTitle" type="text" class="xtinput" id="txt_PlanTitle" size="20" valid="required" errmsg="请输入计划标题" value="<%=PlanTitle %>" /><span id="errMsg_txt_PlanTitle"  style="color:Red;"></td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right"><font color="#FF0000">*</font>计划内容：</th>
        <td height="30" colspan="3" bgcolor="#E3F1FC" class="pandl4">
            <textarea name="txt_PlanContent"  class="xtinput" id="txt_PlanContent" style=" width:550px; height:180px;"  valid="required" errmsg="请输入计划内容" ><%=PlanContent %></textarea><span id="errMsg_txt_PlanContent"  style="color:Red;"></td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right"><font color="#FF0000">*</font>发送对象：</th>
        <td height="30" colspan="3" bgcolor="#E3F1FC" class="pandl4">
          <input type="checkbox" name="chb_All" id="chb_All" value="0" <%= ChbAllPersonnel?"checked":"" %>  />
          <label for="chb_All" style=" cursor:hand;"> 所有人</label>
           
        <input type="checkbox" name="chb_Department" id="chb_Department" value="1" <%= ChbDepartment?"checked":"" %> />
          <label for="chb_Department"  style=" cursor:hand;">指定部门</label>
          <a onclick="" href="javascript:void(0);">
            <cc1:selectDepartment id="selectDepartment1" runat="server" IsShowTextLabel="true" IsShowText="false" SetPicture="/images/sanping_04.gif" ></cc1:selectDepartment> 
          <input type="checkbox" name="chb_Personnel" id="chb_Personnel" value="2" <%= ChbPersonnel?"checked":"" %> />
          <label for="chb_Personnel" style=" cursor:hand;">指定人员</label>  
          
           <cc2:selectOperator id="selectOperator1"  runat="server" IsShowText="false" IsShowTextLabel="true"></cc2:selectOperator> 
           <span id="spanError"   style="color:Red; display:none;">请选择发送对象</span>
        </td>
      </tr>
	  <tr class="odd">
        <th width="17%" height="30" align="right">发布人：</th>
        <td width="32%" bgcolor="#E3F1FC">
            <input name="txt_Publisher" type="text" class="xtinput" id="txt_Publisher" size="20" value="<%=Publisher %>"/></td>
        <th width="17%" height="30" align="right">发布时间：</th>
        <td width="34%" bgcolor="#E3F1FC">
            <input name="txt_PublishTime" type="text" class="xtinput" id="txt_PublishTime" size="20" onfocus="WdatePicker()"   value="<%= string.Format("{0:yyyy-MM-dd}",PublishTime)%>" /></td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="10" align="center" bgcolor="#E3F1FC">
          <table border="0" align="center" cellpadding="0" cellspacing="0">
             <tr>
                <td width="76" height="40" align="center" class="tjbtn02">
                    <a href="javascript:void(0);" onclick="return EditTrainingPlan.Save('save');">保存</a></td>
                <td width="158" height="40" align="center" class="tjbtn02">
                    <a href="javascript:void(0);" onclick="window.parent.Boxy.getIframeDialog('<%=Utils.GetQueryStringValue("iframeId") %>').hide();">关闭</a></td>
              </tr>
          </table>        
        </td>
      </tr>
    </table>
    <input id="hidMethod" name="hidMethod" type="hidden" value="" />
    <input id="hidTrainPlanID" name="hidTrainPlanID" type="hidden" value="" runat="server" />
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
    //初始化编辑器
    KE.init({
        id: 'txt_PlanContent', //编辑器对应文本框id
        width: '550px',
        height: '180px',
        skinsPath: '/js/kindeditor/skins/',
        pluginsPath: '/js/kindeditor/plugins/',
        scriptPath: '/js/kindeditor/skins/',
        resizeMode: 0, //宽高不可变
        items: keSimple //功能模式(keMore:多功能,keSimple:简易)
    });
    var EditTrainingPlan = {
        Save: function(method) {//保存
            var Result = false;
            Result = ValiDatorForm.validator($("#form1").get(0), "span");
            if ($("input:checked").length == 0) {
                $("#spanError").show();
                Result = false;
            } else {
                $("#spanError").hide();
            }

            if (!Result) {
                return false;
            }



            $("#hidMethod").attr("value", method);
            $("#<%=form1.ClientID %>").get(0).submit();
            return false;
        }
    };
    $(function() {
        setTimeout(function() {
            KE.create('txt_PlanContent', 0); //创建编辑器
        }, 100);
        FV_onBlur.initValid($("#form1").get(0));
    });
    </script>
    </form>
</body>
</html>
