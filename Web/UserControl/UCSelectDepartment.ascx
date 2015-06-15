<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSelectDepartment.ascx.cs" Inherits="Web.UserControl.UCSelectDepartment" %>

<asp:HiddenField ID="hfDepartId" runat="server" Value="" />
<asp:HiddenField ID="LabelSelNum" runat="server" Value="" />
<input name="txt_Department" runat="server" id="txt_Department" type="text" size="18" class="searchinput2 depinput" />
<a href="javascript:;" class="seld" style="cursor: pointer;" >
    <asp:Image ID="imgPath" runat="server"  ImageUrl="/images/sanping_04.gif"/>
</a>
<asp:Label ID="lblShowOperName" runat="server" Text="()" Visible="false"></asp:Label>
<script src="/js/jquery.js" type="text/javascript"></script>

<script type="text/javascript">
    
    
    
    $(function(){
        $("a.seld").click(function(){
            var ismuch = $("#" + "<%= LabelSelNum.ClientID %>").val();
            var id = $("#" + "<%= hfDepartId.ClientID %>").val();
            var topid = '<%=Request.QueryString["iframeId"] %>';
            var url = "/Common/SelectDepartment.aspx?ismuch="+ismuch+"&topid="+topid+"&id="+id+"&hfid=<%= hfDepartId.ClientID %>&txtid=<%= lblShowOperName.ClientID %>&txtinputid=<%=txt_Department.ClientID %>&m=m";
	        parent.Boxy.iframeDialog({
			    iframeUrl:url,
			    title:"选择部门",
			    modal:true,
			    width:"790px",
			    height:"260px"
		    });
		    return false;
        });
        
        $("input.depinput").focus(function(){
            $(this).next("a.seld").click()
        });
        
    });
    var <%=this.ClientID %> = {
        //获得Id
        GetId:function(){
            return $("#" + "<%= hfDepartId.ClientID %>").val();
        },
        //获得名字
        GetName: function(){
            return $("#" + "<%= txt_Department.ClientID %>").val();
        }
    }
</script>