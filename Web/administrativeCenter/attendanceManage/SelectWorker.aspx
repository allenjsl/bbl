<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectWorker.aspx.cs" Inherits="Web.administrativeCenter.attendanceManage.SelectWorker" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="600" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:20px auto;">
        <tr class="odd">
            <th colspan="5" bgcolor="#BDDCF4">请选择人员</th>
        </tr>
        <tr class="odd">
            <td colspan="5" align="left" bgcolor="#E3F1FC" class="addbtnsytle">
            <table width="590" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr align="left">
                <td>
                    姓名：<input type="text" id="txt_WorkerName" name="txt_WorkerName"  runat="server" />
                    部门：<select id="dpDepartment" name="dpDepartment" runat="server">
                        <option value="0">--请选择--</option>
                    </select>
                </td>
                <td>
                     <a href="javascript:void(0);" onclick="return SelectWorker.OnSearch();">
                     <img src="/images/searchbtn.gif" /></a>
                </td>
              </tr>
            </table>
            </td>
          </tr>
          <tr class="odd" id="PersonnelDataList">
             <td width="100%" colspan="5" height="28" bgcolor="#E3F1FC" class="pandl3" >
                 <asp:DataList ID="dlPersonnelList" runat="server" RepeatColumns="5" RepeatDirection="Vertical" Width="100%">
                    <ItemTemplate>
                       <input name="chb_NameID" type="checkbox" value="<%# Eval("Id") %>"  /><label name="lbl_Name"><%# Eval("UserName")%></label>(<%# Eval("ContactSex")%>)
                    </ItemTemplate>
                  </asp:DataList>
                  <div id="NoData" runat="server" style=" width:100%; height:50px; text-align:center;" visible="false">没有人员数据！</div>
             </td>
          </tr>
	      <tr class="odd">
            <td height="35" colspan="5" align="center" bgcolor="#E3F1FC" class="pandl3">
                <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" />
            </td>
          </tr>
          <tr class="odd">
            <td height="30" colspan="17" align="center" bgcolor="#BDDCF4">
		    <table width="340" border="0" align="center" cellpadding="0" cellspacing="0">
              <tr>
                <td height="40" align="center" class="jixusave">
                <a href="javascript:void(0);" onclick="SelectWorker.BeSure();">确定</a></td>
              </tr>
            </table>
            </td>
          </tr>
    </table>
    
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var data = { iframeid: "", desid: "", method: "", userName: "", departmentId: "" };
        var SelectWorker = {
            QueryString: function(val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            },
            BeSure: function() {
                var iframeid = SelectWorker.QueryString("desid");   //父窗体
                var desid = SelectWorker.QueryString("iframeid");   //当前窗体
                var doc = top.Boxy.getIframeDocument(iframeid);
                var personnelObj = SelectWorker.GetCkPersonnelList();
                $(doc.getElementById("txt_Name")).attr("value", personnelObj.Name.toString());
                $(doc.getElementById("hiddenID")).attr("value", personnelObj.ID.toString());
                top.Boxy.getIframeDialog(desid).hide();
            },
            GetCkPersonnelList: function() {//得到选中的人员
                var arrID = new Array();
                var arrName = new Array();
                $("#PersonnelDataList").find(":checked").each(function() {
                    arrID.push($(this).val());
                    arrName.push($(this).next("label").html());
                });
                var Personnel = {
                    ID: arrID,
                    Name: arrName
                };
                return Personnel;
            },
            OnSearch: function() {
                var desid = SelectWorker.QueryString("desid");  //父窗体
                var iframeid = SelectWorker.QueryString("iframeid");  //当前窗体
                data.iframeid = iframeid;
                data.desid = desid;
                data.method = "OnSearch";
                data.userName = $("#<%=txt_WorkerName.ClientID %>").val();
                data.departmentId = $("#<%=dpDepartment.ClientID %>").val();
                window.location.href = "/administrativeCenter/attendanceManage/SelectWorker.aspx?" + $.param(data);
                return false;
            }
        };
    </script>
    </form>
</body>
</html>
