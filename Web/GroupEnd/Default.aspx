<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.GroupEnd.Default"
    MasterPageFile="~/masterpage/Front.Master" Title="首页"  %>

<asp:Content ContentPlaceHolderID="head" ID="head1" runat="server">
    <script src="/js/tourdate.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="Content1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
        <!--公告-->
        <div class="s_news"><img src="/images/index2_06.gif" />
            <marquee scrollamount="4" height="31px" width="680px"  onmouseout="start()"  onmousemove="stop()">
                    <ul>
                        <% if (newslist != null && newslist.Count > 0)
                           {
                               foreach (EyouSoft.Model.PersonalCenterStructure.NoticeNews notice in newslist)
                               { %>
                        <li style="width:340px"><a href="/GroupEnd/NoticeShow.aspx?tid=<%=notice.Id %>"  title="<% =notice.Title %>"><% =notice.Title.Length>20?notice.Title.Substring(0,18)+"...":notice.Title %></a></li>
                        <%}
                           } %>
                        <div class="clearboth"></div>
                     </ul></marquee>                                                 
            <div class="clearboth"></div>
        </div>
        <div class="hr_10">
        </div>
        <!--订单提醒-->
        <span class="orderformwarnT">订单提醒</span>
          <table width="99%" border="0" cellspacing="0" cellpadding="0" class="orderformwarn">
          	<tr>
              <td height="15" colspan="2" align="left" valign="top"></td>
       	    </tr>
            <tr>
              <td width="2%" height="25" align="left" valign="top">&nbsp;</td>

        	  <td width="98%" align="left"><img src="/images/ztindex04.jpg" alt="" width="16" height="11" /> <%=remindnum %>家留位过期。
                  <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label></td>
            </tr>
   	    </table>

        
        <div class="hr_10">
        </div>
        <table width="99%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="22%" valign="top" class="index_biankuang">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="CompanyInfo">
                        <tr>
                            <td height="23" align="center" background="/images/ztindex05.jpg">
                                &nbsp;
                            </td>
                            <td align="left" background="/images/ztindex05.jpg">
                                <span class="logintitle">
                                    <img src="/images/ztindex06.jpg" />用户登录</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 25px; line-height: 25px;">
                                <font class="fbred">欢迎您，<% =SiteUserInfo.ContactInfo.ContactName %></font><br />
                                <a href="Javascript:;" class="modifyinfo">编辑信息</a> <a href="Javascript:;" class="modifypas">修改密码</a><br />
                                电话：<% =SiteUserInfo.ContactInfo.ContactTel %><br />
                                手机：<% =SiteUserInfo.ContactInfo.ContactMobile %><br />
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="1%">
                    &nbsp;
                </td>
                <td width="75%" valign="top" class="index_biankuang">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="4%" height="23" align="center" background="/images/ztindex05.jpg">
                                &nbsp;
                            </td>
                            <td width="96%" height="23" align="left" background="/images/ztindex05.jpg">
                                <span class="logintitle">
                                    <img src="/images/ztindex07.jpg" alt="业务咨询" />业务咨询</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="1" cellpadding="0" border="0" bgcolor="#b5d6e6" width="100%">
                        <thead bgcolor="#ffffff">
                        <tr align="center">
                          <th height="30" width="20%">联系人</th>
                          <th width="20%">电话 </th>
                          <th width="20%">手机</th>
                          <th width="20%">传真</th>
                          <th width="20%">QQ</th>
                        </tr>
                        </thead>
                        <tbody id="content" bgcolor="#ffffff">
                        <% if (list != null)
                           {
                               int z; for (z = 0; z < list.Count; z++)
                               { %>
                        <tr align="center">
                          <td height="30" ><% =list[z].ContactName%></td>
                          <td><% =list[z].ContactTel%></td>
                          <td><% =list[z].ContactMobile%></td>
                          <td><% =list[z].ContactFax%></td>
                          <td><% =list[z].QQ%></td>
                        </tr>
                        <%} for (; z < 3; z++)
                               { %>
                           <tr align="center">
                              <td height="30"></td>
                              <td></td>
                              <td></td>
                              <td></td>
                              <td></td>
                           </tr>
                        <%}
                           }%>
                        </tbody>
                        <tfoot>
                        <tr bgcolor="#ffffff">
                          <td colspan="5" align="center"><a style="cursor:pointer" href="javascript:;" class="prepage" page="0">上一页</a> <a style="cursor:pointer" href="javascript:;" class="nextpage" page="2" max="<%=(recordcount-1)/pagesize+1 %>">下一页</a></td>
                        </tr>
                        </tfoot>
                   </table>
                </td>
            </tr>
        </table>
        <div class="hr_10">
        </div>
        
        <% for(int q =0;q<areaname.Count;q++)
          { %>
         <table cellspacing="0" cellpadding="0" border="0" width="99%">
             <tbody>
                <tr>
                    <td width="49%" valign="top">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <%if (arealist[q] != null)
                          {
                              int i; %>
                        <tbody>
                        <tr>
                            <td style="border-bottom: 1px solid rgb(186, 217, 232);" colspan="4"><span class="zxtitle" style="width:300px;"><%= areaname[q] %><a href="/GroupEnd/LineList/LineProductsList.aspx?xlid=<%=areaIdList[q] %>" style=" font-size:12px; float:right" >更多</a></span>
     </td>
                        </tr>
                        <tr>
                            <th height="30" align="center" width="8%">序号</th>
                            <th align="center" width="53%">线路名称</th>
                            <th align="center" width="20%"><span>建议门市价</span></th>
                            <th align="center" width="20%">出团时间</th>
                        </tr>
                        <% for(i=0;i<arealist[q].Count;i++)
                          { %>
                        <tr <%= i==0?"bgcolor='#fcf6d7'":"" %> tid="<% =arealist[q][i].TourId %>" >
                            <td height="30"  align="center" class="xuexian02"><%=i+1%></td>
                            <td align="center" class="xuexian02"><a href="javascript:;" class="show"><% =arealist[q][i].RouteName%></a></td>
                            <td align="center" class="xuexian02"><font class="fred">￥<%=GetPrice(arealist[q][i].TourId)%></font></td>
                            <td align="center" class="xuexian02"><%=GetLDateString(arealist[q][i]) %></td>
                        </tr>
                        <%} if (i > 0)
                           {
                               for (; i < 4; i++)
                               { %>
                        <tr <%= i==0?"bgcolor='#fcf6d7'":"" %> height="30">
                            <td height="30"  align="center" class="xuexian02"></td>
                            <td align="center" colspan="3" class="xuexian02"></td>
                        </tr>
                        <%}
                           }
                           else
                           { %>
                           <tr  height="120">
                                <td rowspan="4" colspan="4" align="center">暂无数据</td>
                           </tr>
                           <%} %>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        </tbody>
                        <%} %>
                    </table>
                    </td>
                    <td width="1%">&nbsp;</td>
                    <td width="49%" valign="top">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <%if (q<areaname.Count-1&&arealist[++q] != null)
                          {
                              int i; %>
                        <tbody>
                        <tr>
                            <td style="border-bottom: 1px solid rgb(186, 217, 232);" colspan="4"><span class="zxtitle" style="width:300px"><%= areaname[q]%><a href="/GroupEnd/LineList/LineProductsList.aspx?xlid=<%=areaIdList[q] %>" style="font-size:12px; float:right" >更多</a></span></td>
                        </tr>
                        <tr>
                            <th height="30" align="center" width="8%">序号</th>
                            <th align="center" width="53%">线路名称</th>
                            <th align="center" width="20%"><span>建议门市价</span></th>
                            <th align="center" width="20%">出团时间</th>
                        </tr>
                        <% for (i = 0; i < arealist[q].Count; i++)
                          { %>
                        <tr <%= i==0?"bgcolor='#fcf6d7'":"" %> tid="<% =arealist[q][i].TourId %>" >
                            <td height="30"  align="center" class="xuexian02"><%=i+1%></td>
                            <td align="center" class="xuexian02"><a href="javascript:;" class="show"><% =arealist[q][i].RouteName%></a></td>
                            <td align="center" class="xuexian02"><font class="fred">￥<%=GetPrice(arealist[q][i].TourId)%></font></td>
                            <td align="center" class="xuexian02"><%=GetLDateString(arealist[q][i]) %></td>
                        </tr>
                        <%} if (i > 0)
                           {
                               for (; i < 4; i++)
                               { %>
                        <tr <%= i==0?"bgcolor='#fcf6d7'":"" %> height="30">
                            <td height="30"  align="center" class="xuexian02"></td>
                            <td align="center" colspan="3" class="xuexian02"></td>
                        </tr>
                        <%}
                           }
                           else
                           { %>
                           <tr  height="120">
                                <td rowspan="4" colspan="4" align="center">暂无数据</td>
                           </tr>
                           <%} %>
                        <tr>
                            <td colspan="4">&nbsp;</td>
                        </tr>
                        </tbody>
                        <%} %>  
                    </table></td>
                  </tr>
              </tbody>
          </table>
         <%} %>
         
        
    </div>

    <script type="text/javascript" language="javascript">
        var HomePage = {
            //弹出对话框
            openDialog: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
            }
        };
        $(function() {
            $("a.modifyinfo").bind("click",function(){
                HomePage.openDialog("/GroupEnd/LineList/EditInfo.aspx", "编辑信息", "850", "450",null);
            });
            $("a.modifypas").bind("click",function(){
                HomePage.openDialog("/GroupEnd/LineList/UpdatePwd.aspx", "修改密码", "350", "200", null);
            });
            
            $("a.show").bind("click",function(){
            var url = "/GroupEnd/LineList/LineProductsList.aspx?isdetails=1&tourid=";
                var that = $(this);
                var tid = that.parent().parent().attr("tid");
                location.href = url + tid;
                return false;
            });
            
            $("a.prepage").click(function(){
                var page=$(this).attr("page");
                if(page>0){
                    getpage(page,-1);
                }else{
                    alert("已经是第一页");
                }
                return false;
            });
            $("a.nextpage").click(function(){
                var page = $(this).attr("page");
                var maxpage = $(this).attr("max");
                if(page>maxpage){
                    alert("已经是最后一页");
                }else{
                    getpage(page,1);
                }
                return false;
            });
            function getpage(page,add){
                $.newAjax({
                    type: "GET",
                    dataType: "json",
                    url: "/GroupEnd/Default.aspx",
                    data: {act:"getcontent",page:page},
                    cache: false,
                    success: function(d) {
                        if(d.content){
                            var html = "";
                            var i = 0;
                            for(;i< d.content.length;i++){
                                html+="<tr align='center'>"+
                                  "<td height='30' >"+d.content[i].name+"</td>"+
                                  "<td>"+d.content[i].phone+"</td>"+
                                  "<td>"+d.content[i].mobile+"</td>"+
                                  "<td>"+d.content[i].fax+"</td>"+
                                  "<td>"+d.content[i].qq+"</td>"+
                                "</tr>";
                            }
                            for(;i<3;i++){
                                html+="<tr align='center'>"+
                                  "<td height='30' ></td>"+
                                  "<td></td>"+
                                  "<td></td>"+
                                  "<td></td>"+
                                  "<td></td>"+
                                "</tr>";
                            }
                            $("#content").html(html);
                            $("a.prepage").attr("page",parseInt($("a.prepage").attr("page"))+parseInt(add));
                            $("a.nextpage").attr("page",parseInt($("a.nextpage").attr("page"))+parseInt(add));
                        }
                    }
                });
            }

            tourdate.init();
        });
    </script>

    </form>
</asp:Content>
