<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="permissions.aspx.cs" Inherits="Web.Webmaster.permissions" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:content runat="server" contentplaceholderid="Scripts" id="ScriptsContent">
    <style type="text/css">
    .trspace{height:10px;  font-size:0px;}
    .note {color:#999; margin-left:5px; }
    .required {color:#ff0000}
    .unrequired{color:#fff}
    ul {list-style:none; margin:0px; padding:0px;}
    ul li{list-style:none;}
    .pBig{font-weight:bold;line-height:30px;font-size:14px; clear:both; margin-top:10px; background:#eee}
    .pSmall{float:left;width:24%;}
    .pSmall li{line-height:22px}
    .pSmall li.pSmallTitle{font-weight:bold;line-height:24px;}
    .pSmallSpace{clear:both;width:100%; height:10px;}
    .pno{color:#ff0000; font-weight:normal;}
    .lh24 { line-height:24px;}
	</style>
	
	<script type="text/javascript">
	    function setSecond() {
	        var firstValue = $("#txtThirdFirst").val();
	        var data = [];
	        for (var i = 0; i < permissions.length; i++) {
	            if (permissions[i].Id == firstValue) { data = permissions[i].PermissionClass; break; }
	        }            
	        var spanThirdSecondHtml = [];
	        spanThirdSecondHtml.push('<select id="txtThirdSecond" name="txtThirdSecond"><option value="0">请选择子栏目</option>');
	        for (var i = 0; i < data.length; i++) {
	            spanThirdSecondHtml.push('<option value="' + data[i].Id + '">' + data[i].ClassName + '</option>');
	        }
	        spanThirdSecondHtml.push('</select>');	        
	        $("#spanThirdSecond").html(spanThirdSecondHtml.join(''));
	    }
	
	    function init() {
	        if (permissions == null || permissions.length < 1) return;
	        var spanSecondFirstHtml = [];
	        var spanThirdFirstHtml = [];
	        var spanThirdSecondHtml = [];

	        spanSecondFirstHtml.push('<select id="txtSecondFirst" name="txtSecondFirst"><option value="0">请选择大栏目</option>');
	        spanThirdFirstHtml.push('<select id="txtThirdFirst" name="txtThirdFirst" onchange="setSecond()"><option value="0">请选择大栏目</option>');
	        spanThirdSecondHtml.push('<select id="txtThirdSecond" name="txtThirdSecond"><option value="0">请选择子栏目</option></select>');

	        for (var i = 0; i < permissions.length; i++) {
	            spanSecondFirstHtml.push('<option value="' + permissions[i].Id + '">' + permissions[i].CategoryName + '</option>');
	            spanThirdFirstHtml.push('<option value="' + permissions[i].Id + '">' + permissions[i].CategoryName + '</option>')
	        }
	        
	        spanSecondFirstHtml.push('</select>');
	        spanThirdFirstHtml.push('</select>');

	        $("#spanSecondFirst").html(spanSecondFirstHtml.join(''));
	        $("#spanThirdFirst").html(spanThirdFirstHtml.join(''));
	        $("#spanThirdSecond").html(spanThirdSecondHtml.join(''));
	    }

	    $(document).ready(function() {
	        init();
	    });
    </script>
	
</asp:content>
<asp:content runat="server" contentplaceholderid="PageTitle" id="TitleContent">
    系统权限基础数据
</asp:content>
<asp:content runat="server" contentplaceholderid="PageContent" id="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>添加大栏目：<input type="text" id="txtFirst" name="txtFirst" class="input_text" maxlength="72" style="width: 200px" runat="server" /> 
                <asp:Button ID="btnAddFirst" runat="server" Text="添加大栏目" onclick="btnAddFirst_Click" /></td>
        </tr>
        <tr>
            <td>
                添加子栏目：<span id="spanSecondFirst"></span><input type="text" id="txtSecond" name="txtSecond" class="input_text" maxlength="72" style="width: 200px" runat="server" />
                <asp:Button ID="btnAddSecond" runat="server" Text="添加子栏目" onclick="btnAddSecond_Click" />
            </td>
        </tr>
        <tr>
            <td>
                添加权限值：<span id="spanThirdFirst"></span><span id="spanThirdSecond"></span><input type="text" id="txtThird" name="txtThird" class="input_text" maxlength="72" style="width: 200px" runat="server" />
                <asp:Button ID="btnAddThird" runat="server" Text="添加权限值" onclick="btnAddThird_Click" />
            </td>
        </tr>
        <tr id="trPermissions">
            <td>
                <asp:Literal runat="server" ID="ltrPermissions"></asp:Literal>                
            </td>
        </tr>
    </table>
</asp:content>
<asp:content runat="server" contentplaceholderid="PageRemark" id="RemarkContent">
</asp:content>
