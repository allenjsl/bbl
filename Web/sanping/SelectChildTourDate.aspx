<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectChildTourDate.aspx.cs" Inherits="Web.sanping.SelectChildTourDate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>选择出团日期</title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <meta http-equiv="expires" content="0" />
    <style>
        body
        {
            margin: auto 0;
            padding: 0;
        }
        td
        {
            font-size: 12px;
            line-height: 120%;
        }
        table
        {
            border-collapse: collapse;
        }
        img
        {
            border: none;
        }
        .hui
        {
            color: #aaaaaa;
        }
    </style>
</head>
<body>
    <div id="div1">
    </div><br />
    <table width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td align="center">
                <input type="button" name="btnSave" id="btnSave" value=" 生 成 " style="height:25px;" onclick="QGD.SubmitClick();" />
            </td>
        </tr>
    </table>
    <div id="divTourCodeHTML" style="width:780px;margin-top:10px;">
    </div>
    <table id="tblNext" style="display:none;" width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td align="center">
                <input type="button" id="Button1" value=" 保存团号修改 " style="height:25px;" onclick="saveUpdate()" />
                <input type="button" id="Button2" value=" 取 消 " style="height:25px;margin-left:10px;" onclick="closeself()" />
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="/js/jquery.js"></script>

    <script type="text/javascript" src="/js/groupdate.js"></script>
    <script type="text/javascript">
    var ToursNumberSeparator = "{;#$%}";
    var ToursNumberObjSeparator = ",";
    var ToursNumberObj = function(date,tournumber){
        this.date = copyDate(date);
        this.tourNumber = tournumber;
    };
    ToursNumberObj.prototype.toString = function(){
        var s = "";
        var year = this.date.getFullYear().toString();
        var month = this.date.getMonth()+1+"";
        var day = this.date.getDate().toString();
        
        s+= year+"-"+month+"-"+day+ToursNumberSeparator+this.tourNumber;
        
        return s;  
    };
    ToursNumberObj.prototype.getDateString = function(){
        var s = "";
        var year = this.date.getFullYear().toString();
        var month = this.date.getMonth()+1+"";
        var day = this.date.getDate().toString();
        
        s+= year+"-"+month+"-"+day;
        
        return s;  
    }
    function convertToursNumberStr(s){
        var splitStr = ToursNumberSeparator;
        var arr = s.split(",");
        var tarr,dates,tournumber,tmps;
        var resultArr = [];
        for(var i=0;i<arr.length;i++){
            tarr = arr[i].split(splitStr);
            dates = tarr[0];
            tournumber = tarr[1];
            var tmpArr = dates.split("-");
            
            resultArr.push(new ToursNumberObj(new Date(Date.parse(tmpArr[1]+"/"+tmpArr[2]+"/"+tmpArr[0])),tournumber));
        }
        
        return resultArr;
    }
    function GetChildToursNumberList(dateArr,companyId){
        var arr = [],tmpDate;
        for(var i=0;i<dateArr.length;i++){
            arr.push(encodeURIComponent(dateArr[i]));
        }
        //companyId = "1";
        var resultObj = {
            toString:function(){
                return "";
            }
        };
        if(arr.length>0){
            jQuery.newAjax({
                type:"POST",
                url:"/ashx/GenerateTourNumbers.ashx?companyId="+companyId,
                async:false,
                data:{
                    datestr:arr.join(",")
                },
                success:function(data){
                    if(data!=""){
                        var resultArr = [];
                        var arr = eval(data);
                        for(var i=0;i<arr.length;i++){
                            resultArr.push(arr[i][0]+ToursNumberSeparator+arr[i][1]);
                        }
                        resultObj={
                            toString:function(){
                                return resultArr.join(ToursNumberObjSeparator);
                            },
                            arr:resultArr
                        }
                    }
                }
            });
        }
        return resultObj;
    }
    function saveUpdate(){
        var arr = [];
        var isValid = true;
        $("input[name='tournumber']").each(function(){
            var $t = $(this);
            var $d = $(this).prev("input[name='date'][type='hidden']");
            var tn = $.trim($t.val());
            if(tn==""){
                isValid = false;
                return false;
            }
            arr.push($d.val()+ToursNumberSeparator+tn);
        }); 
        
        if(!isValid){
            alert("团号不能为空");
            return;
        }
        
        var parentIframeid = "<%=Request.QueryString["parentiframeid"] %>";
        
        if(parentIframeid==""){
        
        $(parent.document.getElementById("linkByDate")).text("已选择了"+arr.length+"个出发日期");
       parent.document.getElementById("hidToursNumbers").value=arr.join(",");
       parent.$(".hd_guize").val("ViewChildTours.aspx?start=&end=&type=3");
       }else{
            var parentwindow = parent.Boxy.getIframeWindow(parentIframeid);
            $(parentwindow.document.getElementById("linkByDate")).text("已选择了"+arr.length+"个出发日期");
            parentwindow.document.getElementById("hidToursNumbers").value=arr.join(",");
       }
       parent.Boxy.getIframeDialog("<%=Request.QueryString["iframeid"] %>").hide();
    }
    function closeself(){
        parent.Boxy.getIframeDialog("<%=Request.QueryString["iframeid"] %>").hide();
    }
    </script>
    <script type="text/javascript">
    
    $(document).ready(function(){
    
    QGD.initCalendar({
         containerId:"div1",
         currentDate:<%=CurrentDate %>,
         firstMonthDate:<%=CurrentDate %>,
         nextMonthDate:<%=NextDate %>,
         listcontainer:"divTourCodeHTML",
         companyId:<%=Request.QueryString["companyId"] %>,
         parentiframeid:"<%=Request.QueryString["parentiframeid"] %>"
        });
    });
    </script>
    <script type="text/javascript" src="/js/groupdate.js"></script>
</body>
</html>
