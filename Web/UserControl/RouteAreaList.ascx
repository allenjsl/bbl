<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RouteAreaList.ascx.cs"
    Inherits="Web.UserControl.RouteAreaList" %>
<label>
   <b>线路区域：</b></label>
    <asp:DropDownList ID="ddlRouteArea" runat="server">
    </asp:DropDownList>

<script type="text/javascript">
var <%=this.ClientID %>={
    ///获取线路区域ID
    GetAreaId:function()
    {
       return $.trim($("#<%=ddlRouteArea.ClientID %>").val());
    }
}
</script>

