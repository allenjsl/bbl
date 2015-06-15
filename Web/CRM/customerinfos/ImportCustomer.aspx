<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportCustomer.aspx.cs"
    Inherits="Web.CRM.customerinfos.ImportCustomer" %>

<%@ Register Src="~/UserControl/ExcelFileUploadControl.ascx" TagName="upload" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>地接Excel</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/swfupload/default.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/js/jquery.js"></script>

    <script type="text/javascript" src="/js/swfupload/swfupload.js"></script>

    <script type="text/javascript" src="/js/loadExcel.js"></script>

    <script type="text/javascript">
        function loadexcel(array) {
            loadXls.init(array, "#tablelist", ".s1");
            $("#selectall").bind("click", function() {
                loadXls.selectAll(true);
            });
            $("#reset").bind("click", function() {
                loadXls.selectAll(false);
            });
            $("#selectback").bind("click", function() {
                loadXls.selectback();
            });
            $("#ok").bind("click", function() {
                var datas = loadXls.bindIndex([$("#sname").val(), $("#slice").val(), $("#sadd").val(),
                                  $("#scode").val(), $("#scon").val(), $("#stel").val(), $("#smob").val(), $("#sfax").val()]);

                if (!datas || datas == "") {
                    alert("请选择要导入的客户！");
                    return false;
                }
                $.newAjax({
                    type: "post",
                    dataType: "json",
                    url: "ImportCustomer.aspx?" + $.param({ sname: $("#sname").val(), slice: $("#slice").val(), sadd: $("#sadd").val(), scode: $("#scode").val(), scon: $("#scon").val(), stel: $("#stel").val(), smob: $("#smob").val(), sfax: $("#sfax").val() }),
                    data: { custData: datas, method: "save" },
                    cache: false,
                    success: function(result) {
                        if (result.success == "1") {
                            alert("导入成功！");
                            window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                            window.parent.location = "/CRM/customerinfos/CustomerList.aspx";
                        }
                        else {
                            alert(result.message);
                        }
                    }
                });
            });
        }
    </script>

</head>
<body>
    <form id="form" runat="server">
    <div style="padding: 10px">
        <uc1:upload ID="Upload1" UploadSuccessJavaScriptFunCallBack="loadexcel" runat="server"  UploadFrom="客户单位" />
        <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
            <tr>
                <td>
                    <fieldset>
                        <legend>源数据预览&nbsp;&nbsp;&nbsp;&nbsp;</legend>
                        <table height="30" cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
                            <tr>
                                <td>
                                    <div id="tablelist">
                                    </div>
                                    <input type="button" id="selectall" value="全选" />
                                    <input type="button" id="reset" value="清空">
                                    <input type="button" id="selectback" value="反选" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
            <tbody>
                <tr>
                    <td style="padding-top: 15px;">
                        <fieldset>
                            <legend>请设置对应字段</legend>
                            <table cellspacing="0" id="tbl_Cell" cellpadding="5" width="98%" align="center" border="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            <label>
                                                单位名称：</label>
                                            <select id="sname" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>
                                                许可证号：</label>
                                            <select id="slice" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>
                                                地址：</label>
                                            <select id="sadd" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                邮编：</label>
                                            <select id="scode" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>
                                                传真：</label>
                                            <select id="sfax" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>
                                                主要联系人：</label>
                                            <select id="scon" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>
                                                电话：</label>
                                            <select id="stel" class="s1">
                                            </select>
                                        </td>
                                        <td colspan="2">
                                            <label>
                                                手机：</label>
                                            <select id="smob" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <a href="javascript:;" id="ok">保存</a>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </tbody>
        </table>
    </form>
    </div>
</body>
</html>
