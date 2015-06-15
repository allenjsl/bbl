<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="selectXianlu.ascx.cs" Inherits="Web.UserControl.selectXianlu" %>
  <ul class="xllist">  
   <%=sb.ToString() %>
  </ul>
  <script>
      $(function() {
          $("#allxlImg").css("cursor", "pointer").toggle(function() {
          $(".lineCategorybox").height($(".xllist").height()).css("overflow", "auto");
          }, function() {
              $(".lineCategorybox").css("overflow", "hidden").height("62px");
          });
      });
  </script>