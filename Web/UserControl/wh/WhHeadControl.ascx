<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WhHeadControl.ascx.cs"
    Inherits="Web.UserControl.wh.WhHeadControl" %>
<link href="../../css/style.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .slider
    {
        position: absolute;
    }
    .slider li
    {
        list-style: none;
        display: inline;
    }
    .slider img, .slider2 img
    {
        width: 960px;
        height: 145px;
        display: block;
    }
    .slider li .slider2 li
    {
        z-index: 1;
    }
    .slider2 li
    {
        float: left;
    }
    .slider2
    {
        width: <%=960*index%> px;
    }
    .num
    {
        position: absolute;
        right: 5px;
        bottom: 5px;
    }
    .num li
    {
        float: left;
        color: #FF7300;
        text-align: center;
        line-height: 16px;
        width: 16px;
        height: 16px;
        font-family: Arial;
        font-size: 12px;
        cursor: pointer;
        overflow: hidden;
        margin: 3px 1px;
        border: 1px solid #FF7300;
        background-color: #fff;
    }
    .num li.on
    {
        color: #fff;
        line-height: 21px;
        width: 21px;
        height: 21px;
        font-size: 16px;
        margin: 0 1px;
        border: 0;
        background-color: #FF7300;
        font-weight: bold;
    }
    .nav ul .child_menu li
    {
        background: none;
        border: 1px solid #328dcc;
        float: left;
        width: 120px;
        float: left;
        padding: 0;
        background: #fff;
        border-right: none;
    }
    .nav ul .child_menu li a
    {
        color: #3285cc;
    }
    .nav ul li a:hover
    {
        color: Red;
        text-decoration: none;
    }
</style>
<div class="header" >
    <div class="logo">
        <a href="/GroupEnd/whlogin.aspx" title="首页">
            <asp:Image ID="imgLogo" runat="server" ImageUrl="/images/page_icon_03.gif" />
        </a>
        <div class="clearBoth">
        </div>
    </div>
    <ul class="toplinks">
        <li runat="server" id="liSetHome"><a id="SetHome" href="javascript:" onclick="_g.setHomeIfNecessary(false)"
            style="behavior: url(#default#homepage)">&nbsp;&nbsp;设为首页</a></li>
        <li runat="server" id="liAddFav">&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);"
            onclick="add_collect('<%=EyouSoft.Common.Utils.GetTitleByCompany("",null)%>', '<%=Request.Url.ToString()%>');">加入收藏</a></li>
    </ul>
    <div class="clearBoth">
    </div>
    <div class="nav_left" style="width:955px;">
        <div class="nav_right">
            <div class="nav" style="z-index: 2; position: relative;">
                <ul id="ul_nav" style="width: 815px;">
                    <li class="ul_navli"><a href="/GroupEnd/whlogin.aspx" title="">首 页</a></li>
                    <li style="position: relative;" class="ul_navli" onmousemove="$('#Introduction').show();"
                        onmouseout="$('#Introduction').hide();"><a href="/GroupEnd/wh/about_us.aspx" title="">
                            公司介绍</a>
                        <ul class="child_menu" id="Introduction" onmouseover="$(this).show();" onmouseout="$(this).hide();"
                            style="z-index: 9999; display: none; position: absolute; top: 30px; width: 515px;">
                            <li style="z-index: 9999;"><a href="/GroupEnd/wh/about_us.aspx?mod=Introduction"
                                title="">企业简介</a></li>
                            <li><a href="/GroupEnd/wh/about_us.aspx?mod=KeyRoute" title="">主营线路</a></li>
                            <li><a href="/GroupEnd/wh/about_us.aspx?mod=Schooling" title="">企业文化</a></li>
                            <li style="border-right: solid 1px #328dcc;"><a href="/GroupEnd/wh/about_us.aspx?mod=Link"
                                title="">联系我们</a></li>
                        </ul>
                    </li>
                    <li class="ul_navli"><a href="/GroupEnd/wh/TourRouting.aspx" title="">旅游线路</a></li>
                    <li class="ul_navli"><a href="/GroupEnd/wh/HotRouting.aspx" title="">热点线路</a></li>
                    <li class="ul_navli"><a href="/GroupEnd/wh/HotelRouting.aspx" title="">酒店</a></li>
                    <li class="ul_navli"><a href="/GroupEnd/wh/whpolicy.aspx" title="">机票政策</a></li>
                    <li class="ul_navli"><a href="/GroupEnd/wh/SightRouting.aspx" title="">景点</a></li>
                </ul>
            </div>
            <div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    // JavaScript Document
    function add_collect(title, url) {
        if (window.sidebar) {
            window.sidebar.addPanel(title, url, "");
        } else if (!!window.ActiveXObject && !!document.documentMode) {
            window.external.addToFavoritesBar(url, title, "slice");
        } else if (document.all) {
            window.external.AddFavorite(url, title);
        } else if (window.opera && window.print) {
            return true;
        }
    }
</script>

<!--bannerEnd-->
