<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Provinces.ascx.cs" Inherits="Web.UserControl.Provinces" %>

<script type="text/javascript">
    function changePro_<%=IsIndex %>(tar) {
        $.newAjax(
            { url: "/ashx/GetCitys.ashx",
                data: { companyId: "<%=CompanyId %>", provinceId: $(tar).val(), isFav: "<%=IsFav.HasValue?(IsFav.Value?1:0):2 %>" },
                dataType: "text",
                cache: false,
                async: false,
                type: "get",
                success: function(result) {                
                    $("#uc_cityDynamic<%=IsIndex %>").html(result);
                },
                error: function() {

                }
            })
    }
</script>

<select id="uc_provinceDynamic<%=IsIndex %>" name="uc_provinceDynamic<%=IsIndex %>"
    onchange="changePro_<%=IsIndex %>(this);">
    <%=provinceHtml%>
</select>
