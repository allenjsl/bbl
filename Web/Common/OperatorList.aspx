<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperatorList.aspx.cs" Inherits="Web.Common.OperatorList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="/UserControl/UCSelectDepartment.ascx" TagName="UCSelectDepartment" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>请选择操作人员</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="selected" cellspacing="1" cellpadding="0" border="0" align="center" width="600">
            <tr class="odd">
                    <th bgcolor="#bddcf4" colspan="5">
                        已选择人员:
                    </th>
            </tr>
            <%=strSelectedPeopleHtml %>
        </table>
        <table class="select" cellspacing="1" cellpadding="0" border="0" align="center" width="600" style="margin: 20px auto;">
            <tbody>
                <tr class="odd">
                    <th bgcolor="#bddcf4" colspan="5">
                        请选择人员:
                    </th>
                </tr>
                <tr class="odd">
                    <td height="35" bgcolor="#e3f1fc" align="left" class="pandl3" colspan="5">
                        姓名：
                        <input type="text" name="textfield" id="inputOperName" />
                        部门：
                        <uc1:UCSelectDepartment ID="UCSelectDepartment1"   runat="server" SetPicture="/images/sanping_04.gif" />
                        <a href="javascript:;" id="btnSearch"><img border="0" src="/images/searchbtn.gif" alt="" /></a>
                    </td>
                </tr>
                <%=strHtml%>
                <tr class="odd">
                    <td height="35" bgcolor="#e3f1fc" align="center" class="pandl3" colspan="5" id="tdPageContainer">
                        <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                            CurrencyPageCssClass="RedFnt" />
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" bgcolor="#bddcf4" align="center" colspan="17">
                        <table cellspacing="0" cellpadding="0" border="0" align="center" width="340">
                            <tbody>
                                <tr>
                                    <td height="40" align="center" class="jixusave">
                                        <a href="#" id="selectxl">选择</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
        $(function() {

            $("#inputOperName").focus();
            
            var OperatorList = {
                queryString: function(val) {
                    var uri = window.location.search;
                    var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
                    return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
                }
            }

            $("#selectxl").click(function() {
                var isMultiSelect = <%=Request.QueryString["ismultiselect"]=="1"?"true":"false" %>;
                var idList = [];
                var nameList = [];
                var tmpIdArr = [];
                var oInput;
                tmpIdArr= getIdList().split(",");
                
                for(var i=0;i<tmpIdArr.length;i++){
                    if(tmpIdArr[i]==""){
                       continue;
                    }
                    oInput = $(":checked[value='"+tmpIdArr[i]+"']");
                    if(oInput.length>0){
                        idList.push(tmpIdArr[i]);
                        nameList.push(oInput.eq(0).next("label").eq(0).text());
                    }
                }
                
                if(isMultiSelect=="false"){
                    if(idList.length>1){
                        alert("最多只能选择一个");
                        return false;
                    }
                }
                
                var name = "";
                var id = "";
                name = nameList.join(",");
                id = idList.join(",");
                var topID = OperatorList.queryString("topIID");
                if (topID == "null" || topID == null || topID == undefined) {
                    window.parent.$("#" + OperatorList.queryString("txtname")).val(name);
                    window.parent.$("#" + OperatorList.queryString("hdid")).val(id);
                    window.parent.$("#" + OperatorList.queryString("lblID")).html(name);
                }
                else {
                    var doc = window.parent.Boxy.getIframeDocument(OperatorList.queryString('topIID'));
                    var txtNameID = OperatorList.queryString('txtname');
                    var hdid = OperatorList.queryString('hdid');
                    var lblID = OperatorList.queryString("lblID");
                    var oName = doc.getElementById(txtNameID);
                    if(oName){
                        $(oName).val(name);
                    }
                    var oLbl = doc.getElementById(lblID);
                    if(oLbl){
                        $(oLbl).html(name);
                    }
                    $(doc.getElementById(hdid)).val(id);
                }
                if (OperatorList.queryString("callback")) {
                    var callback = OperatorList.queryString("callback");
                    eval("window.parent." + callback + "()");
                }
                try{
                    window.parent.Boxy.getIframeDialog(OperatorList.queryString('iframeId')).hide();
                }catch(e){}
                return false;
            });

            if (OperatorList.queryString('OperName') == null || OperatorList.queryString('OperName') == "null") {
                $("#inputOperName").val("");
            }
            else {
                $("#inputOperName").val(decodeURI(OperatorList.queryString('OperName')));
            }

            $("#btnSearch").click(function() {
                var data = {
                topIID: OperatorList.queryString("topIID"),
                    departID:<%=this.UCSelectDepartment1.ClientID%>.GetId(),
                    departName:<%=this.UCSelectDepartment1.ClientID%>.GetName(),
                    iframeId: OperatorList.queryString('iframeId'),
                    OperName: $("#inputOperName").val(),
                    txtname: OperatorList.queryString("txtname"),
                    hdid: OperatorList.queryString("hdid"),
                    lblID: OperatorList.queryString("lblShowOperName"),
                    nowselected:getIdList()
                };
                window.location.href = "OperatorList.aspx?" + $.param(data);
                return false;
            });
            
            $("#tdPageContainer a").click(function(){
                var data = {
                    nowselected:getIdList()
                }
                var uri = $.param(data);
                
                $(this).attr("href",$(this).attr("href")+"&"+uri);
            });
            
            $(".selected input[type='checkbox']").click(function(){
                var o = $(this);
                var b = this.checked;
                var value = o.val();
                if(b===false){
                    $(".select input[type='checkbox'][value='"+value+"']").removeAttr("checked");
                }else{
                    $(".select input[type='checkbox'][value='"+value+"']").attr("checked","checked");
                }
            });
            $(".select input[type='checkbox']").click(function(){
                var o = $(this);
                var b = this.checked;
                var value = o.val();
                if(b===false){
                    $(".selected input[type='checkbox'][value='"+value+"']").removeAttr("checked");
                }else{
                    $(".selected input[type='checkbox'][value='"+value+"']").attr("checked","checked");
                }
            });
        });
        function getIdList(){
            var idList = ",";
            $(".selected input:checked").each(function(){
                idList+=$(this).val()+",";
            });
            $(".select input:checked").each(function(){
                var value = $(this).val()+",";
                if(idList.indexOf(","+value)==-1){
                    idList+=value;
                }
            });
            if(idList.length>1){
                idList = idList.substr(0,idList.length-1);
            }else{
                idList="";
            }
            return idList;
        }
    </script>

</body>
</html>
