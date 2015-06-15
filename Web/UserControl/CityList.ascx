<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CityList.ascx.cs" Inherits="Web.UserControl.CityList" %>
<select id="uc_cityDynamic" name="uc_cityDynamic">
<%=cityHtml%>
</select>
<script type="text/javascript">
    var provinceAndCity_city = {}
    function SetCity(CityId) {
        $("#uc_cityDynamic").attr("value", CityId);
    }
    provinceAndCity_city["<%=this.ClientID %>"] =
     {
         cityId: "uc_cityDynamic"
     }
</script>
