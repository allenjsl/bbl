<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadVisitors.aspx.cs" Inherits="Web.SingleServe.LoadVisitors" %>

<%@ Register Src="~/UserControl/ExcelFileUploadControl.ascx" TagName="upload" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>从文件导入页</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/swfupload/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/js/jquery.js"></script>
    <script type="text/javascript" src="/js/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="/js/loadExcel.js"></script>
    <style type="text/css">
        .s1
        {
          width:120px;
        }
        body
        {
            font-size: 12px;
        }
        #visitorList
        {
            margin: 0px;
            padding: 0px;
            list-style-type: none;
            width: 800px;
            overflow: hidden;
            border-left: 1px solid #efefef;
            border-top: 1px solid #efefef;
            margin-top: 10px;
        }
        #visitorList li
        {
            height: 20px;
            line-height: 20px;
            float: left;
            list-style-type: none;
            border-bottom: 1px solid #efefef;
            border-right: 1px solid #efefef;
        }
        #visitorList li.No
        {
            width: 30px;
            text-align: center;
            color: #666666;
            font-weight: bold;
        }
        #visitorList li.VisitorName
        {
            width: 40px;
            text-align: center;
            color: #666666;
            padding-left: 2px;
        }
        #visitorList li.VisitorType
        {
            width: 50px;
            text-align: center;
            color: #666666;
        }
        #visitorList li.CardType
        {
            width: 60px;
            text-align: center;
            color: #666666;
            padding-left: 2px;
        }
        #visitorList li.CardNo
        {
            width: 70px;
            text-align: center;
            color: #666666;
            padding-left: 2px;
        }
        #visitorList li.Sex
        {
            width: 30px;
            text-align: center;
            color: #666666;
            padding-left: 2px;
        }
        #visitorList li.ContactTel
        {
            width: 80px;
            text-align: center;
            color: #666666;
            padding-left: 2px;
        }
    </style>

    <script type="text/javascript">
        var LoadVisitors = {
            queryString: function(val) {
                var uri = window.location.search;
                var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
                return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
            }
        }
        
        function createVisitorsList(arr){
            var topID = LoadVisitors.queryString("topID");
            var type="<%=Request.QueryString["type"] %>";
            var doc = window.parent.Boxy.getIframeDocument(topID);
            var win = window.parent.Boxy.getIframeWindow(topID);
            
            //win.loadVisitors.clear();
            var length = arr.length;
            for(var i=0;i<length;i++){
                win.loadVisitors.addVisitor(arr[i]);
            }
            win.loadVisitors._autoCompute();
        }

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
                var topID = LoadVisitors.queryString("topID");
                var type="<%=Request.QueryString["type"] %>";
                var win = topID? window.parent.Boxy.getIframeWindow(topID):window.parent;
                var arr = loadXls.bindIndex([$("#lstVisitorName").val(), $("#lstVisitorType").val(), $("#lstCardName").val(),
                                     $("#lstCardNo").val(), $("#lstSex").val(), $("#lstTel").val()]);
                if(arr.length==0){
                    alert("请在源数据中选择数据导入");
                    return false;
                }
                if(type=="otherload"){
                    createVisitorsList(arr);
                }else{
                   win.CreateCusList(arr);                                     
                }
               window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
            });
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--上传控件开始--%>
        <uc1:upload UploadSuccessJavaScriptFunCallBack="loadexcel" runat="server" />
        <%--上传控件结束--%>
        <br />
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
                                    <input type="button" id="reset" value="清空"/>
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
                                            <label>姓名：</label> 
                                                <select alt="姓名" id="lstVisitorName" name="lstVisitorName" class="s1">
                                                </select>
                                        </td>
                                        <td>
                                            <label>类型：</label> 
                                            <select alt="类型" id="lstVisitorType" name="lstVisitorType" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>证件名称：</label> 
                                            <select alt="证件名称" id="lstCardName" name="lstCardName" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label>证件号码：</label> 
                                                <select alt="证件号码" id="lstCardNo" name="lstCardNo" class="s1">
                                                </select>
                                        </td>
                                        <td>
                                            <label>性别：</label> 
                                            <select alt="性别" id="lstSex" name="lstSex" class="s1">
                                            </select>
                                        </td>
                                        <td>
                                            <label>联系电话：</label> 
                                            <select alt="联系电话" id="lstTel" name="lstTel" class="s1">
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="shenghui" align="center">
                                        &nbsp;<%--<asp:Button ID="btn_ImportInfo" OnClientClick="return inff.checkVisitorsInfo();"
                                            Text="确定" runat="server"></asp:Button>--%>
                                        <input id="ok" type="button" value="确定" />
                                        &nbsp;<input name="Canseled" type="button" value="关闭" onclick="window.parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
