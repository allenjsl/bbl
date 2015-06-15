<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CityEdit.aspx.cs" Inherits="Web.systemset.basicinfo.CityEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>城市编辑_基础设置_系统设置</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
      
</head>
<body>
    <form id="ce_form" runat="server" >
    <input type="hidden" name="hidMethod" id="hidMethod" value="save" />
   <table width="500" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
      <tr class="odd">
        <th width="18%" height="30" align="right"><span style="color:Red">*</span>所属省份：</th>
        <td bgcolor="#E3F1FC">
          <select id="selProvince" name="selProvince" runat="server" valid="required"  errmsg="省份不为空">
          </select>
           <span id="errMsg_<%=selProvince.ClientID %>" class="errmsg" ></span>
          </td>
      </tr>
	  <tr class="odd">
        <th width="18%" height="30" align="right"><span style="color:Red">*</span>城市名称：</th>
        <td bgcolor="#E3F1FC"> 
        <input name="txtCityName" type="text" class="xtinput" id="txtCityName"   maxlength="20"  size="40" value="<%=cityName %>"  valid="required"  errmsg="城市不为空" />
        <span id="errMsg_txtCityName" class="errmsg" ></span><input type="text" style="display:none;"/>
        </td>
      </tr>
      <tr class="odd">
        <td height="30" colspan="2" align="left" bgcolor="#E3F1FC">
          <table width="324" border="0" cellspacing="0" cellpadding="0">
         <tr>
            <td width="90" height="40" align="center"></td>
            <td width="76" height="40" align="center" class="tjbtn02"><a href="javascript:;" onclick="return save('');">保存</a></td>
            <td width="158" height="40" align="center"><span class="tjbtn02"><a href="javascipt:;" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide();return false;">关闭</a></span></td>
          </tr>
          </table> 
        </td>
      </tr>
    </table>
    </form>
    <script src="/js/jquery.js" type="text/javascript"></script>
       <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
       <script type="text/javascript">
           $(document).ready(function() {
              FV_onBlur.initValid($("#<%=ce_form.ClientID %>").get(0));
          });

          function isExist() {
             
          }
       //保存城市信息
        function save(method) {
            var isSuccess = ValiDatorForm.validator($("#<%=ce_form.ClientID %>").get(0), "span");
            if (!isSuccess) { return false; }
            $.newAjax(
            { url: "/systemset/basicinfo/CityEdit.aspx",
                data: { cityId: "<%=cId %>", cityName: $("#txtCityName").val(),isExist:"isExist" },
                dataType: "json",
                cache: false,
                async: false,
                type: "post",
                success: function(result) {
                    if (result.success == "1") {
                        alert("该城市已经存在！");
                        isSuccess = false;
                    }

                },
                error: function() {
                    alert("数据保存失败！");
                }
            })
            if (!isSuccess) { return false; }
            if (method == "continue") {
                document.getElementById("hidMethod").value = "continue";
            }
            //提交表单
            $("#<%=ce_form.ClientID %>").get(0).submit();
            return false;
        }
    </script>
</body>
</html>
