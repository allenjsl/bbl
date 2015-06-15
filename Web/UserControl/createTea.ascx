<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="createTea.ascx.cs" Inherits="Web.UserControl.createTea" %>

<script src="/js/childtours.js" type="text/javascript"></script>

<table width="98%" cellspacing="1" cellpadding="0" border="0" align="right">
    <tbody>
        <tr class="odd">
            <th width="9%" height="30" align="center">
                按周生成
            </th>
            <td width="40%" align="left">
                <b>起始日期：</b>
                <input type="text" class="searchinput" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" id="txt_startDate1"
                    name="txt_startDate1">
                <b>结束日期：</b><input type="text" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" class="searchinput" id="txt_endDate1"
                    name="txt_endDate1">
            </td>
            <td width="43%">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <th width="13%" align="left" rowspan="2">
                                周期：
                            </th>
                            <td width="86%">
                                <ul class="ul_week">
                                    <li>
                                        <nobr><input type="checkbox" id="checkbox1" name="weekAll" value="-1">
                                        全选</li>
                                    </nobr>
                                    <li>
                                        <nobr><input type="checkbox" id="checkbox2" name="week" value="2">
                                        星期一</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox2" name="week" value="3">
                                        星期二</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox2" name="week" value="4">
                                        星期三</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox3" name="week" value="5">
                                        星期四</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox3" name="week" value="6">
                                        星期五</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox3" name="week" value="7">
                                        星期六</li>
                                    </nobr>
                                    <li>
                                        <nobr>
                                <input type="checkbox" id="checkbox3" name="week" value="1">星期天</nobr>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
            <td width="8%" align="center">
                <a class="addbtnsytle" href="javascript:void(0)" id="createtea1">生成</a>
            </td>
        </tr>
        <tr class="even">
            <th height="30" align="center">
                按天生成
            </th>
            <td align="left">
                <b>起始日期：</b>
                <input type="text" class="searchinput" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" id="txt_startDate2" name="txt_startDate2">
                <b>结束日期：</b><input type="text" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" class="searchinput" id="txt_endDate2"
                    name="txt_endDate2">
            </td>
            <td>
                <b>周期：</b>每隔
                <input type="text" class="jiesuantext" id="textfield12" name="txt_everyDay">
                天发团
            </td>
            <td align="center">
                <a class="addbtnsytle" href="javascript:void(0)" id="createtea2">生成</a>
            </td>
        </tr>
        <tr class="odd">
            <th height="30" align="center">
                自定义选择
            </th>
            <td align="left">
                <a href="javascript:;" id="linkByDate" >请选择日期</a>
                <%--<b>起始日期：</b>
                <input type="text" class="searchinput" onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" id="txt_startDate3" name="txt_startDate3">
                <b>结束日期：</b><input type="text"  onfocus="WdatePicker({dateFmt:'MM/dd/yyyy'});" class="searchinput" id="txt_endDate3"
                    name="txt_endDate3">--%>
            </td>
            <td>
             <%--   <b>周期：</b><input type="checkbox" id="checkbox" name="chk_Permanent">永久发班
            --%></td>
            <td align="center">
                <%--<a class="addbtnsytle" href="javascript:void(0)" id="createtea3">生成</a>--%>
                &nbsp;
            </td>
        </tr>
    </tbody>
</table>
<input type="hidden" id="hd_guize" class="hd_guize" runat="server" />
<input type="hidden" id="hd_days" runat="server" />
<script type="text/javascript">
    var <%=this.ClientID %>={
        validate:function(){
            if($("#hidToursNumbers").val()!=""&&$("#hidToursNumbers").val()!=null)
            {
                return true;
            }else
            {
                alert("请生成建团规则!");
                return false;
            }
        }
    };
    $(function(){
        $("#linkByDate").click(function(){
            Boxy.iframeDialog({
                            iframeUrl: "/sanping/SelectChildTourDate.aspx",
                            title: "建团日期",
                            modal: true,
                            width: "800px",
                            height: "450px",
                            data:{
                                companyId:<%=companyid %>
                            }
                        });
                        return false;
        });
    });
    $(function() {
        $("#teamInfo").hide();
        var reg = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        var gz="<%=guize %>";
        var hd_days ="<%=hd_days.ClientID %>"
        var hd_guize="<%=hd_guize.ClientID %>";
        if(gz!="")
        {
            $("#"+hd_guize).val(gz);            
            $("#teamInfo").show();
            $(".teamInfo").attr("href", "ViewChildTours.aspx?"+gz);
             $(".teamInfo").click(function() {
                Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    title: "报名",
                    modal: true,
                    width: "800px",
                    height: "450px"
                });
                return false;
            });
        }
        $("#createtea1").click(function() {
            var chk="",b=false;
            $("[name='week']:checked").each(function() {
                chk += $(this).val() + ",";
            });
            if (!reg.test($("#txt_startDate1").val())) {
                alert("请正确输入开始日期!");
                $("#txt_startDate1").focus();
                return false;
            }
            if (!reg.test($("#txt_endDate1").val())) {
                alert("请正确输入结束日期!");
                $("#txt_endDate1").focus();
                return false;
            }
            if(chk=="")
            {
                alert("请选择周期!");
                return false;
            }
            b=true;
            if(b)
            {
                ChildToursRule1.beginDate=new Date( $("#txt_startDate1").val());
                ChildToursRule1.endDate=new Date($("#txt_endDate1").val());
                ChildToursRule1.one.show= chk.indexOf("1")>-1?true:false;
                ChildToursRule1.two.show= chk.indexOf("2")>-1?true:false;
                ChildToursRule1.three.show= chk.indexOf("3")>-1?true:false;
                ChildToursRule1.four.show= chk.indexOf("4")>-1?true:false;
                ChildToursRule1.five.show= chk.indexOf("5")>-1?true:false;
                ChildToursRule1.six.show= chk.indexOf("6")>-1?true:false;
                ChildToursRule1.seven.show= chk.indexOf("7")>-1?true:false;
                var gz_count=ChildToursRule1.getToursCount();
                
                $("#hidToursNumbers").val(GetChildToursNumberList(ChildToursRule1,<%=companyid %>).toString());
                $("#"+hd_days).val(gz_count);
                $("#teamInfo").html('<a href="javascript:void(0)" class="teamInfo">按周生成，已成功生成'+gz_count+'个规则</a>').show();  
                
                var url = "ViewChildTours.aspx?start=" + $("#txt_startDate1").val() + "&end=" + $("#txt_endDate1").val() + "&type=1&data=" + chk;
                $("#"+hd_guize).val(url)
                $(".teamInfo").attr("href", url);
                $(".teamInfo").unbind("click");
                $(".teamInfo").click(function() {
                    Boxy.iframeDialog({
                        iframeUrl: $(this).attr("href"),
                        title: "建团日期",
                        modal: true,
                        width: "800px",
                        height: "450px"
                    });
                    return false;
                });
            }
            return false;
        });

        $("#createtea2").click(function() {
            var b=false;
            var url = "ViewChildTours.aspx?start=" + $("#txt_startDate2").val() + "&end=" + $("#txt_endDate2").val() + "&type=2&data=" + $("[name='txt_everyDay']").val();

            if (!reg.test($("#txt_startDate2").val())) {
                alert("请正确输入开始日期!");
                $("#txt_startDate2").focus();
                return false;
            }
            if (!reg.test($("#txt_endDate2").val())) {
                alert("请正确输入结束日期!");
                $("#txt_endDate2").focus();
                return false;
            }
            if($("[name='txt_everyDay']").val()=="")
            {
                alert("请选择周期!");
                $("[name='txt_everyDay']").focus();
                return false;
            }
            if($("[name='txt_everyDay']").val()=="")
            {
                alert("请选择周期!");
                $("[name='txt_everyDay']").focus();
                return false;
            }
            if(!/^\d+$/.test($("[name='txt_everyDay']").val()))
            {
                alert("请正确填写周期!");
                $("[name='txt_everyDay']").select();
                return false;
            }
            b=true;
            if(b)
            {               
                ChildToursRule2.beginDate=new Date( $("#txt_startDate2").val());
                ChildToursRule2.endDate=new Date($("#txt_endDate2").val());
                ChildToursRule2.days=parseInt($("[name='txt_everyDay']").val());
                var gz_count=ChildToursRule2.getToursCount();
                
                $("#hidToursNumbers").val(GetChildToursNumberList(ChildToursRule2,<%=companyid %>).toString());
                $("#"+hd_days).val(gz_count);
                $("#teamInfo").html('<a href="javascript:void(0)" class="teamInfo">按天生成，已成功生成'+gz_count+'个规则</a>').show();  
                $("#"+hd_guize).val("start=" + $("#txt_startDate2").val() + "&end=" + $("#txt_endDate2").val() + "&type=2&data=" + $("[name='txt_everyDay']").val());
                    $(".teamInfo").attr("href", url);
                    $(".teamInfo").unbind("click");
                    $(".teamInfo").click(function() {
                        Boxy.iframeDialog({
                            iframeUrl: $(this).attr("href"),
                            title: "建团日期",
                            modal: true,
                            width: "800px",
                            height: "450px"
                        });
                        return false;
                    });
            }
            return false;
        });

        $("#createtea3").click(function() {
            var b=false;
            
            if($("[name='chk_Permanent']").attr("checked"))
            {
                b=true;                
                $("#teamInfo").html("已成功添加永久发班!").show();  
                return false;
            } 
            var url = "ViewChildTours.aspx?start=" + $("#txt_startDate3").val() + "&end=" + $("#txt_endDate3").val() + "&type=3";
            $("#"+hd_guize).val(url);
            if (!reg.test($("#txt_startDate3").val())) {
                alert("请正确输入开始日期!");
                $("#txt_startDate3").focus();
                return false;
            }
            if (!reg.test($("#txt_endDate3").val())) {
                alert("请正确输入结束日期!");
                $("#txt_endDate3").focus();
                return false;
            }
            b=true;
            if(b)
            { 
                ChildToursRule3.beginDate=new Date( $("#txt_startDate3").val());
                ChildToursRule3.endDate=new Date($("#txt_endDate3").val());
                var gz_count=ChildToursRule3.getToursCount();
                
                $("#hidToursNumbers").val(GetChildToursNumberList(ChildToursRule3,<%=companyid %>).toString());
                $("#"+hd_days).val(gz_count);
                $("#teamInfo").html('<a href="javascript:void(0)" class="teamInfo">按日期生成，已成功生成'+gz_count+'个规则</a>').show();  
                $("#"+hd_guize).val("start=" + $("#txt_startDate3").val() + "&end=" + $("#txt_endDate3").val() + "&type=3");              
            }
            $(".teamInfo").attr("href", url);
            $(".teamInfo").unbind("click");
            $(".teamInfo").click(function() {
                Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    title: "建团日期",
                    modal: true,
                    width: "800px",
                    height: "450px"
                });
                return false;
            });
            return false;
        });
        $("[name='weekAll']").click(function() {
            $("[name='week']").attr("checked", !$("[name='week']").attr("checked"));
        });
    });
</script>