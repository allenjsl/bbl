<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="xianluWindow.ascx.cs" Inherits="Web.UserControl.xianluWindow" %>
<script>
var <%=this.ClientID %> = {
    val:function(){
        return $("#<%=txt_xl_Name.ClientID %>").val();
    },
    Id:function(){ 
        return $("#<%=hd_xl_id.ClientID %>").val();
        },
    AreaId:function(){
        return $("#<%=hd_xl_areaId.ClientID %>").val();
    }
}
function openXLwindow(url){
        url=url+"?txtname=<%=txt_xl_Name.ClientID %>&hdid=<%=hd_xl_id.ClientID %>&callback=<%=callBack %>&publishtype=<%=publishType %>&PiframeId=<%=Request.QueryString["iframeId"] %>";
        	window.parent.Boxy.iframeDialog({
			iframeUrl:url,
			title:"线路",
			modal:true,
			width:"900px",
			height:"450px"
		});
		return false;
}
</script>
<input type="hidden" id="hd_xl_id" runat="server" />
<input type="hidden" id="hd_xl_areaId" runat="server" />
<asp:TextBox ID="txt_xl_Name" runat="server" Width="222px"></asp:TextBox>
<%--<input type="text" class="searchinput searchinput02" id="textfield" name="textfield">--%>
<%if (publishType==4){ %>
<a onclick="return openXLwindow('/Common/GroupxianluList.aspx')" href="javascript:void(0)"><img src="/images/sanping_04.gif" style="vertical-align:middle;float:left;"></a>

<%}else{ %>
<a onclick="return openXLwindow('/Common/xianluList.aspx')" href="javascript:void(0)"><img src="/images/sanping_04.gif"></a>
<%} %>
