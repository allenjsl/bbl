<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewChildTours.aspx.cs" Inherits="Web.sanping.ViewChildTours" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>子团列表</title>
    <style>
    table {
    border-collapse: collapse;
    }
     td {
    color: #333333;
    font-size: 12px;
    line-height: 20px;
}
    </style>
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/childtours.js"></script>
    <script>
    $(function(){
//        ChildToursRule3.beginDate = new Date(Date.parse("2/10/2011"));
//        ChildToursRule3.endDate =new Date(Date.parse("4/5/2011"));
        
//        ChildToursRule2.beginDate = new Date(Date.parse("2/10/2011"));
//        ChildToursRule2.endDate =new Date(Date.parse("4/20/2011"));
//        ChildToursRule2.days = 1;
        
        
        var type="<%=Request.QueryString["type"] %>";
        switch(type)
        {
            case "1":{
                ChildToursRule1.beginDate = new Date(Date.parse("<%=Request.QueryString["start"] %>"));
                ChildToursRule1.endDate =new Date(Date.parse("<%=Request.QueryString["end"] %>"));
                var data="<%=Request.QueryString["data"] %>";
                ChildToursRule1.one.show = data.indexOf("1")>-1?true:false;
                ChildToursRule1.two.show = data.indexOf("2")>-1?true:false;
                ChildToursRule1.three.show = data.indexOf("3")>-1?true:false;
                ChildToursRule1.four.show = data.indexOf("4")>-1?true:false;
                ChildToursRule1.five.show = data.indexOf("5")>-1?true:false;
                ChildToursRule1.six.show =data.indexOf("6")>-1?true:false;
                ChildToursRule1.seven.show = data.indexOf("7")>-1?true:false;
                ChildTours.initCalendar({                
                    configObj:ChildToursRule1
                });
            }break;
            case "2":{
                ChildToursRule2.beginDate = new Date(Date.parse("<%=Request.QueryString["start"] %>"));
                ChildToursRule2.endDate =new Date(Date.parse("<%=Request.QueryString["end"] %>"));
                var data="<%=Request.QueryString["data"] %>";
                
                ChildToursRule2.days =parseInt(data);
                ChildTours.initCalendar({                
                    configObj:ChildToursRule2
                });
            }break;
            case "3":{
                ChildToursRule3.beginDate = new Date(Date.parse("<%=Request.QueryString["start"] %>"));
                ChildToursRule3.endDate =new Date(Date.parse("<%=Request.QueryString["end"] %>"));
                ChildTours.initCalendar({                
                    configObj:ChildToursRule3
                });
                
            }break;
        }
                //生成子团团号列表
                var s = parent.document.getElementById("hidToursNumbers").value;
                if(s==""){
                    return;
                }
                var arr = convertToursNumberStr(s);
                var cmonth = copyDate(arr[0].date);
                var isAddBegin=false,isAddEnd = true;
                var s ="";  
                for(var i = 0;i<arr.length;i++){
                    if(compareDateByMonth(cmonth,arr[i].date)<0){
                        cmonth = copyDate(arr[i].date);
                        isAddEnd = false;
                        isAddBegin = false;
                    }
                    if(!isAddEnd){
                        s+='</fieldset><div style="margin:10px;"></div>';
                        isAddEnd=true;
                    }
                    if(!isAddBegin){
                        s+='<fieldset style="font-size: 12px;">'+'<legend>'+cmonth.getFullYear().toString()+'年'+(cmonth.getMonth()+1)+'月</legend>';
                        isAddBegin = true;
                    }
                    s+='<label style="padding-left:60px;padding-bottom:10px;">'+arr[i].getDateString()+'<input type="hidden" name="date" value="'+arr[i].getDateString()+'" /><input name="tournumber" style="width:100px;" type="text" value="'+arr[i].tourNumber+'" /></label>';
                }
                
                $("#childToursList").html(s);
                
        
        
    });
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
        
       parent.document.getElementById("hidToursNumbers").value=arr.join(",");
       parent.Boxy.getIframeDialog("<%=Request.QueryString["iframeid"] %>").hide();
    }
    </script>
</head>
<body>
    <div id="calendarContainer" style="width:735px">
    
    </div>
    <table width="100%" cellspacing="0" cellpadding="5" border="0">
        <tbody><tr>
            <td align="center">
                <input type="button" style="height: 25px;" value="保存团号修改" id="btnSave" onclick="saveUpdate();">
            </td>
        </tr>
    </tbody></table>
    <div id="childToursList" style="width:735px;margin-top:10px;">
    </div>
</body>
</html>
