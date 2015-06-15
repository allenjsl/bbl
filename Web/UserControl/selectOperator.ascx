<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="selectOperator.ascx.cs"
    Inherits="Web.UserControl.selectOperator" %>
<asp:Label ID="lbl_show_title" runat="server" Text=""></asp:Label>
<asp:TextBox ID="txt_op_Name" runat="server" class="searchinput2 depinput"></asp:TextBox>
<input type="hidden" id="hd_op_id" runat="server" />
<a onclick="return <%=this.ClientID %>.openXLwindow('/Common/OperatorList.aspx','请选择<%=Title %>','<%=Width%>','<%=Height%>')"
    style="cursor: pointer;">
    <img src="/images/sanping_04.gif"></a>
<asp:Label ID="lblShowOperName" runat="server"  Text="()" Visible="false"></asp:Label>
<script src="/js/jquery.js" type="text/javascript"></script>

<script type="text/javascript">

var <%=this.ClientID %> = {
    //获得操作者姓名
    GetOperatorName:function(){
        return $("#<%=txt_op_Name.ClientID %>").val();
    },
     //获得操作者姓名
    GetOperatorlblName:function(){
        return $("#<%=lblShowOperName.ClientID %>").text();
    },
    //获得操作者ID
    GetOperatorId:function(){ 
        return $("#<%=hd_op_id.ClientID %>").val();
    },
    queryString:function(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    },
    openXLwindow:function(url,title,width1,height1){
         var topIID=<%=this.ClientID %>.queryString('iframeId');
         var selected = $("#<%=hd_op_id.ClientID %>").val();
         url=url+"?txtname=<%=txt_op_Name.ClientID %>&hdid=<%=hd_op_id.ClientID %>&topIID="+topIID+"&lblID=<%=lblShowOperName.ClientID%>&callback=<%=callBack %>";
        	parent.Boxy.iframeDialog({
			iframeUrl:url,
			title:title,
			modal:true,
			width:width1,
			height:height1,
			data:{
			    selectedidlist:selected,
			    ismultiselect:<%=_IsMultiSelect?1:0 %>
			}
		});
		return false;
    },
    setV:function(id,name) {
        $("#<%=hd_op_id.ClientID %>").val(id);
        $("#<%=txt_op_Name.ClientID %>").val(name);
    }
}
$("#<%=txt_op_Name.ClientID %>").focus(function(){
    <%=this.ClientID %>.openXLwindow('/Common/OperatorList.aspx','请选择<%=Title %>','<%=Width%>','<%=Height%>');
});

</script>

