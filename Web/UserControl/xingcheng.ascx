<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="xingcheng.ascx.cs" Inherits="Web.UserControl.xingcheng" %>
<%if (listplan != null && listplan.Count > 0)
  {  %>
<asp:Repeater ID="rpt_xingcheng" runat="server" OnItemDataBound="rpt_xingcheng_ItemDataBound">
    <ItemTemplate>
        <table width="100%" cellspacing="1" class="xianchen" cellpadding="0" border="0">
            <tbody>
                <tr>
                    <th height="30" bgcolor="#bddcf4" align="right">
                        区间：
                    </th>
                    <td bgcolor="#bddcf4" align="center">
                        <input type="text" value="<%#Eval("Interval") %>" class="searchinput" id="textfield5"
                            name="txt_qujian">
                        至
                        <input type="text" value="<%#Eval("Vehicle") %>" class="searchinput" id="textfield6"
                            name="txt_jiaotong">
                    </td>
                    <th bgcolor="#bddcf4" align="center">
                        住宿：
                    </th>
                    <td bgcolor="#bddcf4" align="center">
                        <input type="text" value="<%#Eval("Hotel") %>" class="searchinput" id="textfield7"
                            name="txt_zhushu">
                    </td>
                    <th bgcolor="#bddcf4" align="center">
                        就餐：
                    </th>
                    <td bgcolor="#bddcf4" align="center">
                        <input type="checkbox" id="checkbox4" name="chk_eat" value="1">
                        全
                        <input type="checkbox" id="checkbox5" name="chk_eat" value="2">
                        早
                        <input type="checkbox" id="checkbox6" name="chk_eat" value="3">
                        中
                        <input type="checkbox" id="checkbox7" name="chk_eat" value="4">
                        晚
                        <input type="hidden" name="eat" value="<%#Eval("Dinner") %>" />
                    </td>
                </tr>
                <tr>
                    <th height="30" bgcolor="#e3f1fc" align="right">
                        行程内容：
                    </th>
                    <td bgcolor="#e3f1fc" align="left" colspan="5">
                        <textarea class="textareastyle" id="textarea" name="txt_xiancheng"><%#Eval("plan") %></textarea>
                    </td>
                </tr>
                <tr bgcolor="#bddcf4">
                    <th height="30" align="center">
                        添加图片：
                    </th>
                    <td align="left" colspan="5">
                        <div style="float: left; margin-right: 10px;">
                            <input type="file" value="<%#Eval("FilePath") %>" id="fileField" name="file_xc_img" />
                        </div>
                        <input type="hidden" value="<%#Eval("FilePath") %>" name="hd_fileUrl" />
                        <asp:Panel ID="pnlFile" Visible="false" runat="server">
                            <img src="/images/open-dx.gif" />
                            <asp:HyperLink ID="hlink_FileName" runat="server" Target="_blank">查看图片</asp:HyperLink>
                            <a href="javascript:void(0);" class="DeleFile">
                                <img src="/images/fujian_x.gif" /></a></asp:Panel>
                    </td>
                </tr>
                <tr bgcolor="#e3f1fc">
                    <th height="30" align="center">
                        操作：
                    </th>
                    <td align="left" colspan="5">
                        <table width="30%" cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <a href="javascript:void(0)" class="insert">
                                            <img alt="插入" src="/images/jiahao.gif">
                                            插入</a>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0)" class="move">
                                            <img alt="移动" src="/images/yidongicon.gif">
                                            移动</a>
                                    </td>
                                    <td align="center">
                                        <a href="javascript:void(0)" class="delxc">
                                            <img alt="删除" src="/images/delicon.gif">
                                            删除</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </ItemTemplate>
</asp:Repeater>
<%}
  else
  { %>
<table width="100%" cellspacing="1" class="xianchen" cellpadding="0" border="0">
    <tbody>
        <tr>
            <th height="30" bgcolor="#bddcf4" align="right">
                区间：
            </th>
            <td bgcolor="#bddcf4" align="center">
                <input type="text" value="" class="searchinput" id="textfield5" name="txt_qujian">
                至
                <input type="text" value="" class="searchinput" id="textfield6" name="txt_jiaotong">
            </td>
            <th bgcolor="#bddcf4" align="center">
                住宿：
            </th>
            <td bgcolor="#bddcf4" align="center">
                <input type="text" value="" class="searchinput" id="textfield7" name="txt_zhushu">
            </td>
            <th bgcolor="#bddcf4" align="center">
                就餐：
            </th>
            <td bgcolor="#bddcf4" align="center">
                <input type="checkbox" id="checkbox4" name="chk_eat" value="1">
                全
                <input type="checkbox" id="checkbox5" name="chk_eat" value="2">
                早
                <input type="checkbox" id="checkbox6" name="chk_eat" value="3">
                中
                <input type="checkbox" id="checkbox7" name="chk_eat" value="4">
                晚
                <input type="hidden" name="eat" />
            </td>
        </tr>
        <tr>
            <th height="30" bgcolor="#e3f1fc" align="right">
                行程内容：
            </th>
            <td bgcolor="#e3f1fc" align="left" colspan="5">
                <textarea class="textareastyle" id="textarea" name="txt_xiancheng"></textarea>
            </td>
        </tr>
        <tr bgcolor="#bddcf4">
            <th height="30" align="center">
                添加图片：
            </th>
            <td align="left" colspan="5">
                <input type="file" value="" id="fileField" name="file_xc_img" />
                <input type="hidden" value="" name="hd_fileUrl" value="" />
            </td>
        </tr>
        <tr bgcolor="#e3f1fc">
            <th height="30" align="center">
                操作：
            </th>
            <td align="left" colspan="5">
                <table width="30%" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td align="center">
                                <a href="javascript:void(0)" class="insert">
                                    <img alt="插入" src="/images/jiahao.gif">
                                    插入</a>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0)" class="move">
                                    <img alt="移动" src="/images/yidongicon.gif">
                                    移动</a>
                            </td>
                            <td align="center">
                                <a href="javascript:void(0)" class="delxc">
                                    <img alt="删除" src="/images/delicon.gif">
                                    删除</a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
<%} %>

<script type="text/javascript">
var <%=this.ClientID %>={
    setcount:function(){
        var va=/^\d+$/;
        var count=$("[na='<%=daysControlName %>']").val();
        if(!va.test(count))
        {
           alert("请填写正确的行程天数！");
           //$("[na='<%=daysControlName %>']").select();
        }else
        {
            var l= $(".xianchen").length;
            if(l<count){
                for(var i = l;i<count;i++){
                    $(".insert").eq(i-1).click();
                }
            }else
            {
                for(var i = l;i>count;i--)
                $(".delxc").eq(i-1).click();
            }
        }
    },
    bind:function(jsonlist){
       var count = $(jsonlist).length; 
       
       for(var i =0;i<count;i++){
           if(i>0){
                $(".xianchen").eq(0).clone(true).insertAfter($(".xianchen").eq(0)).find("input,textarea").val(""); 
           }  
       }
       for(var i =0;i<count;i++){
//           if(i>0){
//                $(".xianchen").eq(0).clone(true).insertAfter($(".xianchen").eq(0)).find("input,textarea").val(""); 
//           }  
            $("[name='txt_qujian']").eq(i).val(jsonlist[i].qujian);    
            $("[name='txt_jiaotong']").eq(i).val(jsonlist[i].jiaotong); 
            $("[name='txt_zhushu']").eq(i).val(jsonlist[i].zhushu); 
            $("[name='chk_eat']").eq(i*4+0).attr("checked",jsonlist[i].eatOne==1?true:false);
            $("[name='chk_eat']").eq(i*4+1).attr("checked",jsonlist[i].eatTwo==1?true:false);
            $("[name='chk_eat']").eq(i*4+2).attr("checked",jsonlist[i].eatThree==1?true:false);
            $("[name='chk_eat']").eq(i*4+3).attr("checked",jsonlist[i].eatFour==1?true:false);
            $("[name='txt_xiancheng']").eq(i).val(jsonlist[i].content.toString().replace(/##n##/g,"\n"));
            $("[name='hd_fileUrl']").eq(i).val(jsonlist[i].img);
            if(jsonlist[i].img){
                var pic='<span><img src="/images/open-dx.gif" /><a id="hlink_FileName" Target="_blank" href="'+(jsonlist[i].img)+'">查看图片</a><a href="javascript:void(0);" class="DeleFile"><img src="/images/fujian_x.gif" /></a></span>';
                $("[name='hd_fileUrl']").eq(i).after(pic);
            }
            //行程用餐赋值
            var _eatArr=[];
            if(jsonlist[i].eatOne==1) _eatArr.push(1);
            if(jsonlist[i].eatTwo==1) _eatArr.push(2);
            if(jsonlist[i].eatThree==1) _eatArr.push(3);
            if(jsonlist[i].eatFour==1) _eatArr.push(4);            
            if(_eatArr.length>0) $("[name='eat']").eq(i).val(_eatArr.join(","));            
       }
    }
};
    $(function() {
        $("[name='eat']").each(function(){
            if( $(this).val().length>0)
            {
                var str = $(this).val().split(",");
                for(var i = 0;i <str.length;i++)
                {
                    for(var j = 0;j<$(this).prevAll("[name='chk_eat']").length;j++)
                    {
                        if(str[i]==$(this).prevAll("[name='chk_eat']").eq(j).val())
                        {
                            $(this).prevAll("[name='chk_eat']").eq(j).attr("checked",true);
                        }
                    }
                }
            }
        });
        $(".DeleFile").live("click",function(){
            if(confirm("是否确认删除?")){
             $(this).parent().prev("[name='hd_fileUrl']").val("");
             $(this).parent().remove();
            
            }
        });
        function Calculation(){
            var counttextbox=$("[na='<%=daysControlName %>']");
            counttextbox.val($(".xianchen").length);
        }
        $("[na='<%=daysControlName %>']").blur(function(){
             <%=this.ClientID %>.setcount();
        });
        $(".insert").click(function() {
            $(this).parents(".xianchen").clone(true).insertAfter($(this).parents(".xianchen")).find("input,textarea").not("[name='chk_eat']").val("").attr("checked",false); 
            $(this).parents(".xianchen").next().find("[name='chk_eat']").attr("checked",false);
        checkEat();
            Calculation();
        });
        $(".move").click(function() {
            $(this).parents(".xianchen").insertBefore($(this).parents(".xianchen").prev(".xianchen"));
        });
        $(".delxc").click(function() {
            if ($(".xianchen").length > 1)
                $(this).parents(".xianchen").remove();
                Calculation();
        });
        checkEat();
        function checkEat(){
        $("[name='chk_eat']").each(function(){
        $(this).unbind("click");
            $(this).bind("click",function() {
                var next = $(this).nextAll("[name='eat']");
                if ($(this).attr("checked")) {
                    if($(this).val()=="1")
                    {
                        $(this).nextAll("[name='chk_eat']").attr("checked",true);
                        next.val("2,3,4,");
                    }else{
                        next.val(next.val() + $(this).val() + ",");
                    }
                } else {       
                    next.val(next.val().replace($(this).val()+",",""));
                    if($(this).val()=="1")
                    {      
                        $(this).nextAll("[name='chk_eat']").attr("checked",false);
                        next.val("");
                    }
                    $(this).prevAll("[name='chk_eat']").each(function(){
                    if($(this).val()=="1")
                    {
                    $(this).attr("checked",false);
                    }
                    });
                }
            });
        });
        }
        $("[name='file_xc_img']").change(function(){
            $(this).next().val("");
        });
    });
</script>

