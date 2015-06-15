<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConProjectControl.ascx.cs"
    Inherits="Web.UserControl.ConProjectControl" %>
<table id="conProject" width="100%">
    <%if (this.SetList != null && this.SetList.Count > 0)
      {%>
    <asp:Repeater ID="rptList" runat="server">
        <ItemTemplate>
            <tr class="trcon">
                <td bgcolor="#e3f1fc" align="center" width="100px">
                    <select class="select" name="selectPro" id="selectPro_<%#Container.ItemIndex+1 %>">
                        <option value="0">请选择区域</option>
                    </select>
                    <input type="hidden" value="<%# (int)Eval("ServiceType") %>" name="hd_type"/>
                </td>
                <td bgcolor="#e3f1fc" align="center" width="535px">
                    <textarea class="textareastyle" id="txtStandard_<%#Container.ItemIndex+1 %>" name="txtStandard"
                        style="width: 500px"><%#Eval("Service")%></textarea>
                </td>
                <td bgcolor="#e3f1fc" align="center" width="100px">
                    <a href="javascript:void(0);" onclick="ConProject.AddRow()">
                        <img height="16" width="15" src="/images/tianjiaicon01.gif" border="0" />添加</a>
                    <a href="javascript:void(0);" onclick="ConProject.DeleteRow(this)">
                        <img height="14" width="14" src="/images/delicon01.gif" border="0" />删除</a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <%}
      else
      { %>
    <tr class="trcon">
        <td bgcolor="#e3f1fc" align="center" width="100px">
            <select class="select" name="selectPro" id="selectPro_1">
                <option value="0">请选择</option>
            </select>
        </td>
        <td bgcolor="#e3f1fc" align="center" width="535px">
            <textarea class="textareastyle" id="txtStandard_1" name="txtStandard" style="width: 500px"></textarea>
        </td>
        <td bgcolor="#e3f1fc" align="center" width="100px">
            <a href="javascript:void(0);" onclick="ConProject.AddRow()">
                <img height="16" width="15" src="/images/tianjiaicon01.gif" border="0" />添加</a>
            <a href="javascript:void(0);" onclick="ConProject.DeleteRow(this)">
                <img height="14" width="14" src="/images/delicon01.gif" border="0" />删除</a>
        </td>
    </tr>
    <%} %>
</table>
<asp:HiddenField ID="hideConProject" runat="server" Value="1" />
<asp:HiddenField ID="hideProList" runat="server" Value="1" />

<script type="text/javascript">
    $(function() {
        $("#conProject select").each(function() {

        ConProject.SetSelectVal($(this).attr("id"), ""); 
           // $(this).val($(this).next("[name='hd_type']").val());
        });
    })
    var proArray = $("#<%=hideProList.ClientID %>").val().split("|");
    var arrayCount = Number("<%=this.rptList.Items.Count %>");
    var ConProject = {
        AddRow: function() {
            var number = Number($("#<%=hideConProject.ClientID %>").val());
            if (number == NaN) { number = 1 };
            number = number + 1;
            $("#conProject").append("<tr class=\"trcon\"><td bgcolor=\"#e3f1fc\" align=\"center\" width=\"100px\"><select class=\"select\" name=\"selectPro\" id=\"selectPro_" + number + "\"><option value=\"0\">请选择区域</option></select></td><td bgcolor=\"#e3f1fc\" align=\"center\" width=\"535px\"><textarea class=\"textareastyle\" id=\"txtStandard_" + number + "\" name=\"txtStandard\" style=\"width: 500px\"></textarea></td><td bgcolor=\"#e3f1fc\" align=\"center\" width=\"100px\"><a href=\"javascript:void(0);\" onclick=\"ConProject.AddRow()\" ><img height=\"16\" width=\"15\"  src=\"/images/tianjiaicon01.gif\" border=\"0\" />添加</a><a href=\"javascript:void(0);\" onclick=\"ConProject.DeleteRow(this)\"><img height=\"14\"  width=\"14\" src=\"/images/delicon01.gif\" border=\"0\" />删除</a></td></tr>");
            $("#<%=hideConProject.ClientID %>").val(number);
            ConProject.SetSelectVal("selectPro_" + number, "0");
            //if (number > 1)
               // $("#selectPro_" + number).html($("#selectPro_" + number).parent().parent().prev().find("select").html());
            //$("#selectPro_" + number).val("");
            //alert($("#selectPro_" + number).html())
        },
        DeleteRow: function(obj) {
            if (confirm("确定删除?")) {
                if ($(".trcon").length == 1) {
                    alert("仅剩一条，无法删除!"); return false;
                } else {
                    if ($(obj).parent() != null) {
                        $(obj).parent().parent().remove();
                        $("#<%=hideConProject.ClientID %>").val(Number($("#<%=hideConProject.ClientID %>").val()));
                    }
                }
            }
        },
        SetRowValue: function(val) {
            if (val != null) {
                //[{selectPro:"1",standard:"TestTestTestTestTest"},{selectPro:"1",standard:"TestTestTestTestTest"}]
                $("#conProject").html("");
                $("#<%=hideConProject.ClientID %>").val("0");
                for (var i = 0; i < val.length; i++) {
                    //添加行
                    ConProject.AddRow();
                }
                for (var i = 0; i < val.length; i++) {
                    $("#selectPro_" + (i + 1)).val(val[i].ServiceType);
                    $("#txtStandard_" + (i + 1)).val(val[i].Service);
                }
            }
        },
        SetSelectVal: function(selectID, selectIndex) {
            $("#" + selectID).html("");

            for (var i = 0; i < proArray.length; i++) {
                var obj = eval('(' + proArray[i] + ')');

                var ohid = $("#" + selectID).next("input[name='hd_type']");
                //alert(ohid.length);
                if (ohid.length > 0 && ohid.val() == obj.value) {

                    $("#" + selectID).append("<option value=\"" + obj.value + "\" selected=true>" + obj.text + "</option>");
                } else {
                    $("#" + selectID).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
                }
            }

            if (selectIndex != "") {
                $("#" + selectID).val(selectIndex);
            }
        }

    }

   
</script>

