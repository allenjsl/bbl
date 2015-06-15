<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarsLoadExcel.aspx.cs" Inherits="Web.SupplierControl.CarsManager.CarsLoadExcel" %>
<%@ Register Src="~/UserControl/ExcelFileUploadControl.ascx" tagname="upload" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/swfupload/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.js"></script>
    <script type="text/javascript" src="/js/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/js/loadExcel.js"></script>
    <script type="text/javascript">
    function loadexcel(array){
        loadXls.init(array,"#tablelist",".s1");
        //$(".tablelist").html(loadXls.createTable());
        
        ///确定反回的选定数组
        //loadXls.bindIndex([0,2]);
        $("#selectall").bind("click",function(){
            loadXls.selectAll(true);
        });
        $("#reset").bind("click",function(){
            loadXls.selectAll(false);
        });
        $("#selectback").bind("click",function(){
            loadXls.selectback();
        });
        $("#ok").bind("click",function(){
            var datas = loadXls.bindIndex([$("#sprovince").val(),$("#scity").val(),$("#scarteamname").val(),
                                     $("#saddress").val(), $("#scar_guideworld").val(), $("#slinkman_name").val(), $("#slinkman_job").va,$("#slinkman_tel").val(),$("#slinkman_phone").val(),$("#slinkman_qq").val(),$("#slinkman_email").val(),$("#car_type").val(),$("#car_num").val(),$("#car_price").val(),$("#car_driver").val(),$("#car_driverphone").val(),$("#car_remark").val()]);
            
            if(datas.length>0){
                for(var i=0;i<datas.length;i++){
                    for(var j=0;j<datas[i].length;j++){
                        datas[i][j] = encodeURIComponent(datas[i][j]);
                    }
                }
                $.newAjax({
                    type: "post",
                    dataType: "json",
                    url: "CarsLoadExcel.aspx?act=load",
                    data: { dataxls: datas.join(';')},
                    cache: false,
                    success: function(r) {
                        if(r.res){
                            switch(r.res){
                                case 1:
                                    alert("导入成功");
                                    parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                                    window.parent.location = "/SupplierControl/CarsManager/CarsList.aspx";
                                    break;
                                default:
                                    alert("导入失败");
                                    break;
                            }
                        }
                    },
                    error:function(){
                        alert("服务器忙！");
                    }
                });
            }else{
                alert("请选择导入数据！");
            }
        });
    }
    </script>
</head>
<body>
    <form id="form" runat="server">
    <uc1:upload ID="Upload1" UploadSuccessJavaScriptFunCallBack="loadexcel" runat="server" UploadFrom="车队" />
    <table cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
        <tr>
            <td>
                <fieldset>
                    <legend>源数据预览&nbsp;&nbsp;&nbsp;&nbsp;</legend>
                    <table height="30" cellspacing="0" cellpadding="0" width="98%" align="center" border="0">
                        <tr>
                            <td>
                                <div id="tablelist"></div>
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
                                        <label>省份：</label>
                                        <select id="sprovince" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>城市：</label>
                                        <select  id="scity" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>车队名称：</label>
                                        <select id="scarteamname" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>地址：</label>
                                        <select id="saddress" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>导游词：</label>
                                        <select  id="scar_guideworld" class="s1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>联系人姓名：</label>
                                        <select id="slinkman_name" class="s1">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>联系人职务：</label>
                                        <select id="slinkman_job" class="s1" name="D1">
                                        </select>
                                    </td>
                                    <td>
                                        <label>联系人电话：</label>
                                        <select  id="slinkman_tel" class="s1" name="D2">
                                        </select>
                                    </td>
                                    <td>
                                        <label>联系人手机：</label>
                                        <select id="slinkman_phone" class="s1" name="D3">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>联系人QQ：</label>
                                        <select id="slinkman_qq" class="s1" name="D4">
                                        </select>
                                    </td>
                                    <td>
                                        <label>联系人E-mail：</label>
                                        <select  id="slinkman_email" class="s1" name="D5">
                                        </select>
                                    </td>
                                    <td>
                                        <label>车型：</label>
                                        <select id="car_type" class="s1" name="D6">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>车牌号：</label>
                                        <select id="car_num" class="s1" name="D7">
                                        </select>
                                    </td>
                                    <td>
                                        <label>价格：</label>
                                        <select  id="car_price" class="s1" name="D8">
                                        </select>
                                    </td>
                                    <td>
                                        <label>司机：</label>
                                        <select id="car_driver" class="s1" name="D9">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>司机电话：</label>
                                        <select id="car_driverphone" class="s1" name="D10">
                                        </select>
                                    </td>
                                    <td>
                                        <label>备注：</label>
                                        <select  id="car_remark" class="s1" name="D11">
                                        </select>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                 <td height="40" align="center" class="tjbtn02"><a href="javascript:;" id="ok">保存</a></td>
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