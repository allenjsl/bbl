<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShoppingLoadExcel.aspx.cs"
    Inherits="Web.SupplierControl.Shopping.ShoppingLoadExcel" %>

<%@ Register Src="~/UserControl/ExcelFileUploadControl.ascx" TagName="upload" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                var datas = loadXls.bindIndex([$("#sprovince").val(), $("#scity").val(), $("#sunionname").val(),
                                     $("#saleProduct").val(), $("#guideword").val(), $("#saddress").val(),
                                      $("#contactName").val(), $("#jobTitle").val(), $("#contactTel").val(),
                                      $("#contactMobile").val(), $("#QQ").val(), $("#Email").val(),
                                      $("#sremark").val()]);

                if (datas.length > 0) {
                    for (var i = 0; i < datas.length; i++) {
                        for (var j = 0; j < datas[i].length; j++) {
 
                            datas[i][j] = encodeURIComponent(datas[i][j]);
                        }
                    }
                    $.newAjax({
                        type: "post",
                        dataType: "json",
                        url: "ShoppingLoadExcel.aspx?act=load",
                        data: { dataxls: datas.join(';') },
                        cache: false,
                        success: function(r) {
                            if (r.res) {
                                switch (r.res) {
                                    case 1:
                                        alert("导入成功");
                                        parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                                        parent.location.reload();
                                        break;
                                    default:
                                        alert("导入失败");
                                        break;
                                }
                            }
                        },
                        error: function() {
                            alert("服务器忙！");
                        }
                    });
                } else {
                    alert("请选择导入数据！");
                }
            });

        }
    </script>

</head>
<body>
    <form id="form" runat="server">
    <uc1:upload UploadSuccessJavaScriptFunCallBack="loadexcel" runat="server" UploadFrom="购物" />
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
                                <input type="button" id="reset" value="清空" />
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
                                            省份：</label>
                                        <select id="sprovince" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            城市：</label>
                                        <select id="scity" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            单位名称：</label>
                                        <select id="sunionname" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            销售商品：</label>
                                        <select id="saleProduct" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            导游词：</label>
                                        <select id="guideword" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            地址：</label>
                                        <select id="saddress" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            联系人姓名：</label>
                                        <select id="contactName" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            联系人职务：</label>
                                        <select id="jobTitle" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            联系人电话：</label>
                                        <select id="contactTel" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            联系人手机：</label>
                                        <select id="contactMobile" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            联系人QQ：</label>
                                        <select id="QQ" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>
                                            联系人E-mail：</label>
                                        <select id="Email" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            备注：</label>
                                        <select id="sremark" class="s1">
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
</body>
</html>
