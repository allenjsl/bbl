<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectControl.ascx.cs"
    Inherits="Web.UserControl.ProjectControl" %>
<table width="100%" border="0" cellspacing="1" cellpadding="0" id="tabProject">
    <%if (this.SetList.Count > 0 && this.SetList != null)
      {%>
    <asp:Repeater ID="repProject" runat="server">
        <ItemTemplate>
            <tr class="ProjectTr">
                <td align="center" bgcolor="#E3F1FC">
                    <select id="ddl_Project_<%#Container.ItemIndex+1 %>" name="ddl_Project">
                        <option value="<%#Eval("ServiceType") %>">
                            <%#Eval("ServiceType")%></option>
                    </select>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <textarea name="Txt_XianlProject" id="Txt_XianlProject_<%#Container.ItemIndex+1 %>"
                        class="textareastyle"><%#Eval("Service") %>
                </textarea>
                </td>
                <td align="center" bgcolor="#E3F1FC">
                    <a href="javascript:void(0);" id="addProject">
                        <img src="../images/tianjiaicon01.gif" alt="" width="15" height="16" />添加</a>
                    <a href="javascript:void(0);" id="delProject">
                        <img src="../images/delicon01.gif" alt="" width="14" height="14" />删除</a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
    <%}
      else
      { %>
    <tr class="ProjectTr">
        <td align="center" bgcolor="#E3F1FC">
            <select id="ddl_Project" name="ddl_Project">
            </select>
        </td>
        <td align="center" bgcolor="#E3F1FC">
            <textarea name="Txt_XianlProject" id="Txt_XianlProject" class="textareastyle" runat="server"></textarea>
        </td>
        <td align="center" bgcolor="#E3F1FC">
            <a href="javascript:void(0);" id="addProject">
                <img src="../images/tianjiaicon01.gif" alt="" width="15" height="16" />添加</a>
            <a href="javascript:void(0);" id="delProject">
                <img src="../images/delicon01.gif" alt="" width="14" height="14" />删除</a>
        </td>
    </tr>
    <%} %>
</table>
<asp:HiddenField ID="hidePrice" runat="server" Value="1" />
<asp:HiddenField ID="hideProList" runat="server" Value="1" />

<script type="text/javascript" language="javascript">
    var proArray = $("#<%=hideProList.ClientID %>").val().split("|");
    var arrayCount = Number("<%=this.repProject.Items.Count %>");

    var Project = {
        CheckForm: function() {
            var b = true;
            //价格组成 验证
            var indexT = 1;
            $("#tabProject").find("input[name='ddl_Project']").each(function() {
                var number = $(this).attr("id").split("_")[1];
                if (indexT == 1 && $(this).val() == "") {
                    alert("请选择一个包含项目!");
                    return false;
                }
                indexT++;
            });
            return false;
        },
        SetSelectVal: function(selectID, selectIndex) {
            $("#" + selectID).html("");
            for (var i = 0; i < proArray.length; i++) {
                var obj = eval('(' + proArray[i] + ')');

                $("#" + selectID).append("<option value=\"" + obj.value + "\">" + obj.text + "</option>");
            }

            if (selectIndex != "") {
                $("#" + selectID).val(selectIndex);
            }
        }
    };

    window.onload = function() {
        $("#tabProject select").each(function() {
            PriceControl.SetSelectVal($(this).attr("id"), $(this).val());
        });

        //添加项目
        $("#addProject").bind("click", function() {
            $(this).parents(".ProjectTr").clone(true).insertAfter($(this).parents(".ProjectTr")).children("input").val("").find("select").val("-1");
        });
        //删除项目
        $("#delProject").bind("click", function() {
            if ($(this).parents(".ProjectTr").parent().find(".ProjectTr").length > 1) {
                if (confirm("你确定要删除该项吗?")) {
                    $(this).parents(".ProjectTr").remove();
                    return false;
                }
            }
            else {
                alert("请至少要保留一项!");
                return false;
            }
        });
    }


    $(function() {

    });
</script>

