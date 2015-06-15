<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiJieControl.ascx.cs"
    Inherits="Web.UserControl.DiJieControl" %>
<table cellspacing="1" cellpadding="0" border="0" width="100%" id="tblDiJie">
    <tbody>
        <tr class="odd">
            <td height="25" align="center">
                地社名称
            </td>
            <td align="center">
                许可证号
            </td>
            <td align="center">
                联系人
            </td>
            <td align="center">
                电话
            </td>
            <td align="center">
                <a href="javascript:void(0);" onclick="DiJieControl.AddDiJie()">
                    <img height="16" width="15" src="../images/tianjiaicon01.gif" alt="">
                    添加</a>
            </td>
        </tr>
        <%if (this.SetList != null && this.SetList.Count > 0)
          {%>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
                <tr class="even">
                    <td height="25" align="center">
                        <input type="hidden" value="<%#Eval("AgencyId") %>" id="hideDjID_<%#Container.ItemIndex+1 %>"
                            name="hideDjId" />
                        <input type="text" value="<%#Eval("Name") %>" name="txtDjName" readonly="readonly"
                            style="background-color: #dadada;" id="txtDjName_<%#Container.ItemIndex+1 %>"
                            class="searchinput" />&nbsp;&nbsp;<a ref="<%#Container.ItemIndex+1 %>" onclick="DiJieControl.OpenDj(this)"
                                href="javascript:void(0);"><img height="18" width="28" src="../images/sanping_04.gif"></a>
                    </td>
                    <td align="center">
                        <input type="text" name="txtLicense" id="txtLicense_<%#Container.ItemIndex+1 %>"
                            class="searchinput" value="<%#Eval("LicenseNo") %>">
                    </td>
                    <td align="center">
                        <input type="text" value="<%#Eval("ContacterName") %>" name="txtContact" id="txtContact_<%#Container.ItemIndex+1 %>"
                            class="searchinput">
                    </td>
                    <td align="center">
                        <input type="text" value="<%#Eval("Telephone") %>" name="txtPhone" id="txtPhone_<%#Container.ItemIndex+1 %>"
                            class="searchinput">
                    </td>
                    <td align="center">
                        <a onclick="DiJieControl.DeleteDiJie(this)" href="javascript:void(0);">
                            <img height="14" width="14" alt="" src="../images/delicon01.gif" border="0" />删除</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <%}
          else
          { %>
        <tr class="even">
            <td height="25" align="center">
                <input type="hidden" value="" id="hideDjID_1" name="hideDjId" />
                <input type="text" name="txtDjName" id="txtDjName_1" class="searchinput" readonly="readonly"
                    style="background-color: #dadada;">&nbsp;&nbsp;<a ref="1" onclick="DiJieControl.OpenDj(this)"
                        href="javascript:void(0);"><img height="18" width="28" src="../images/sanping_04.gif"></a>
            </td>
            <td align="center">
                <input type="text" name="txtLicense" id="txtLicense_1" class="searchinput">
            </td>
            <td align="center">
                <input type="text" value="" name="txtContact" id="txtContact_1" class="searchinput">
            </td>
            <td align="center">
                <input type="text" name="txtPhone" id="txtPhone_1" class="searchinput">
            </td>
            <td align="center">
                <a onclick="DiJieControl.DeleteDiJie(this)" href="javascript:void(0);">
                    <img height="14" width="14" alt="" src="../images/delicon01.gif" border="0" />删除</a>
            </td>
        </tr>
        <%} %>
    </tbody>
</table>
<asp:HiddenField ID="hideDiJie" runat="server" Value="1" />

<script type="text/javascript">
    var DiJieControl = {
        AddDiJie: function() {
        var number = Number($("#<%=hideDiJie.ClientID %>").val());
            if (number.toString() == "NaN") { number = 1 };
            number = number + 1;
            $("#tblDiJie").append("<tr class=\"even\"><td height=\"25\" align=\"center\"><input type=\"hidden\"  id=\"hideDjID_" + number + "\" name=\"hideDjId\"/><input type=\"text\" class=\"searchinput\" id=\"txtDjName_" + number + "\" name=\"txtDjName\" readonly=\"readonly\"  style=\"background-color:#dadada;\"/>&nbsp;&nbsp;<a href=\"javascript:void(0);\" ref=\"" + number + "\" onclick=\"DiJieControl.OpenDj(this)\"><img height=\"18\" width=\"28\" src=\"../images/sanping_04.gif\"></a></td><td align=\"center\"> <input type=\"text\" class=\"searchinput\" id=\"txtLicense_" + number + "\" name=\"txtLicense\"> </td><td align=\"center\"> <input type=\"text\" value=\"\" name=\"txtContact\" id=\"txtContact_" + number + "\" class=\"searchinput\"></td>  <td align=\"center\"> <input type=\"text\" class=\"searchinput\" id=\"txtPhone_" + number + "\" name=\"txtPhone\"> </td><td align=\"center\"><a href=\"javascript:void(0);\" onclick=\"DiJieControl.DeleteDiJie(this)\"><img height=\"14\" width=\"14\" alt=\"\" src=\"../images/delicon01.gif\" border=\"0\"/>删除</a></td></tr>");
            $("#<%=hideDiJie.ClientID %>").val(number);
        },
        DeleteDiJie: function(obj) {
            if (confirm("确定删除?")) {
                if ($(obj).parent() != null) {
                    $(obj).parent().parent().remove();
                    //$("#<%=hideDiJie.ClientID %>").val(Number($("#<%=hideDiJie.ClientID %>").val()) - 1);
                }
            }
        },
        CheckForm: function() {
            var b = true;
            //地接社信息 验证
            //            var index = 1;
            //            $("#tblDiJie").find("input[name='txtDjName']").each(function() {
            //                var number = $(this).attr("id").split("_")[1];

            //                if (index == 1 && $(this).val() == "") {
            //                    alert("请输入地社名称!");
            //                    b = false;
            //                }
            //                index++;
            //            });
            return b;
        },
        OpenDj: function(obj) {
        var index = $(obj).attr("ref");
        var url = "/TeamPlan/DiJieList.aspx?text=txtDjName_" + index + "&value=hideDjID_" + index + "&index=" + index + "&callback=bind";
            Boxy.iframeDialog({ iframeUrl: url, title: "报名", modal: true, width: "700px", height: "320px" });
        }
    }
    function bind(text, value, index) {
        $("#txtDjName_" + index).val(text);
        $("#hideDjID_" + index).val(value);

        var diJieID = value;
        $.newAjax({
            type: "Get",
            url: "/TeamPlan/AjaxTeamTake.ashx?type=GetInfo&id=" + diJieID + "",
            cache: false,
            success: function(result) {
            if (result != "") {
                $("#txtContact_" + index).val(result.split("||")[0]);
                    $("#txtPhone_"+index).val(result.split("||")[1]);
                    $("#c"+index).val(result.split("||")[3]); 
                }
            }
        });
    }
</script>

