<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TourTypeList.ascx.cs" Inherits="Web.UserControl.TourTypeList" %>
<label>团队类型：</label>
<asp:DropDownList ID="ddlTourType" runat="server">
</asp:DropDownList>
<script type="text/javascript">
var <%=this.ClientID %>={
    ///获取团队类型ID
    GetTourTypeId:function()
    {
       return $.trim($("#<%=ddlTourType.ClientID %>").val());
    }
}
</script>
