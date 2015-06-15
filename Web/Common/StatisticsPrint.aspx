<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" MasterPageFile="~/masterpage/Print.Master"
    CodeBehind="StatisticsPrint.aspx.cs" Inherits="Web.Common.StatisticsPrint" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="PrintC1">
    <style type="text/css" media="screen">
        .PrintTable
        {
            border-collapse: collapse; /* 关键属性：合并表格内外边框(其实表格边框有2px，外面1px，里面还有1px哦) */
            border: solid #000; /* 设置边框属性；样式(solid=实线)、颜色(#999=灰) */
            border-width: 1px 0 0 1px; /* 设置边框状粗细：上 右 下 左 = 对应：1px 0 0 1px */
        }
        .PrintTable th, .PrintTable td
        {
            border: solid #000;
            border-width: 0 1px 1px 0;
            padding: 2px;
        }
    </style>
    <div id="printDiv"></div>

    
    <script type="text/javascript">

        $(function() {
            if ($("#printDiv").html() == "") {//如果此div中没有内容，就发送请求
                var parentFrame = queryString("parentFrame");
                var url = $(parent.document.getElementById("A_Print")).attr("parentUrl");
                if($(parent.document.getElementById("A_Print")).attr("parentUrl").indexOf("?")>0){
                    url+="&IsAll=1";
                }
                else{
                    url+="?IsAll=1";
                }
                
                
                var contentid = $(parent.document.getElementById("A_Print")).attr("ContentId");
                if (parentFrame && parentFrame!="null") {

                    var doc = window.parent.Boxy.getIframeDocument(parentFrame);
                    contentid = $(doc.getElementById("A_Print")).attr("ContentId");
                    url = $(doc.getElementById("A_Print")).attr("parentUrl");
                    if(url.indexOf("?")>0){
                        url+="&IsAll=1";
                    }
                    else{
                        url+="?IsAll=1";
                    }
                }
                $.ajax({
                    type: 'get',
                    //url: $(opener.document.getElementById("A_Print")).attr("parentUrl") + "?&IsAll=1",
                    url: url,
                    dataType: "html",
                    success: function(html) {
                        var ContentId = contentid; //$(opener.document.getElementById("A_Print")).attr("ContentId");
                        var PrintDom = $(html).find("[id='" + ContentId + "']");
                        $(PrintDom).attr("class", "PrintTable");
                        $(PrintDom).find("*").each(function() {
                            $(this).removeAttr("href");
                            $(this).removeAttr("bgColor");
                            $(this).removeAttr("style");
                        });
                        $("#printDiv").append(PrintDom);
                    }
                });
            }
        });
        function queryString(val) {
            var uri = window.location.search;
            var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
            return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
        }
    </script>

</asp:Content>
