<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProvinceList.ascx.cs" Inherits="Web.UserControl.ProvinceList" %>
<script type="text/javascript">
        function changePro(tar) {
            $.newAjax(
            { url: "/ashx/GetCitys.ashx",
                data: { companyId: "<%=CompanyId %>", provinceId: $(tar).val(), isFav: "<%=IsFav.HasValue?(IsFav.Value?1:0):2 %>" },
                dataType: "text",
                cache: false,
                async: false,
                type: "get",
                success: function(result) {
                    $("#uc_cityDynamic").html(result);
                },
                error: function() {

                }
            })
        }

        var provinceAndCity_province = {};
        function SetProvince(ProvinceId) {
            $("#uc_provinceDynamic").attr("value", ProvinceId);
        }
        provinceAndCity_province["<%=this.ClientID %>"] =
        {
           provinceId: "uc_provinceDynamic"
        }
</script>
<select id="uc_provinceDynamic" name="uc_provinceDynamic" onchange="changePro(this);">
<%=provinceHtml%>
</select>
