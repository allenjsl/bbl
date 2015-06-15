<%@ Page Title="系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="BaseManage.aspx.cs" Inherits="Web.systemset.ToGoTerrace.BaseManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form if="form1" runat="server" enctype="multipart/form-data">
    <div class="hr_10">
    </div>
    <div class="mainbody">
        <!-- InstanceBeginEditable name="EditRegion3" -->
        <div class="mainbody">
            <div class="lineprotitlebox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">系统设置</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0 10px 2px 0; color: #13509f;">
                            所在位置>> <a href="#">系统设置 >> 同行平台</a>>> 基础设置
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="2" bgcolor="#000000">
                        </td>
                    </tr>
                </table>
            </div>
            <div class="lineCategorybox" style="height: 50px;">
                <table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                    <tr>
                        <td width="100" align="center" class="xtnav-on">
                            <a href="javascript:void(0)">基础设置</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/RotateImg.aspx">轮换图片</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/TickePoliyList.aspx">机票政策</a>
                        </td>
                        <td width="100" align="center">
                            <a href="/systemset/ToGoTerrace/FriendshipLink.aspx">友情链接</a>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="tablelist">
                <table width="800" border="0" align="center" cellpadding="2" cellspacing="1" bgcolor="#BDDCF4">
                    <tr>
                        <th colspan="3" align="center" bgcolor="#BDDCF4">
                            添加基础设置
                        </th>
                    </tr>
                    <tr>
                        <td width="16%" height="35" align="right" bgcolor="#e3f1fc">
                            <strong>网站标题：</strong>
                        </td>
                        <td width="84%" height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <input name="txt_Title" type="text" size="50" value="<%=SiteTitle==""?"":SiteTitle %>" />
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>Meta关键字：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <input name="txt_Meta" type="text" size="50" value="<%=SiteMeta==""?"":SiteMeta %>" />
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>设为首页：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <input type="radio" name="radioIndex" id="radio" value="1" <%=IsSetHome?"checked='checked'":"" %> />
                            显示
                            <input type="radio" name="radioIndex" id="radio2" value="2" <%=IsSetHome?"":"checked='checked'" %> />
                            不显示
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>添加到收藏夹：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <input type="radio" name="radio" id="radio3" value="1" <%=IsSetFavorite?"checked='checked'":"" %> />
                            显示
                            <input type="radio" name="radio" id="radio4" value="2" <%=IsSetFavorite?"":"checked='checked'" %> />
                            不显示
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>企业标志(LOGO)：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="updom">
                            <a runat="server" id="a_imgHave" target="_blank">企业标志(LOGO)</a><img runat="server"
                                id="delimg" style="cursor: pointer" src="/images/fujian_x.gif" width="14" height="13"
                                class="close" alt="" />
                            <a runat="server" id="a_imgNone">
                                <input type="file" name="workAgree" /></a>&nbsp;<span style="color: #666">(上传图片尺寸:宽467px,高65px)</span>
                            <asp:Literal ID="lstMsg" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>企业简介：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_gsjs" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>联系我们：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_Introduce" runat="server" TextMode="MultiLine" Height="107px"
                                Width="504px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>主营线路：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txtKeyRoute" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>企业文化：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txtSchooling" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>版权说明：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txt_Copyright" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right" bgcolor="#e3f1fc">
                            <strong>联系我们（左）：</strong>
                        </td>
                        <td height="35" colspan="2" align="left" bgcolor="#FAFDFF" class="pandl3">
                            <asp:TextBox ID="txtLianXiFangShi" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="30" colspan="3" align="center">
                            <table width="459" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="137" height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">保存</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <!-- InstanceEndEditable -->
    </div>
    <div class="clearboth">
    </div>
    </form>

    <script type="text/javascript">

        KE.init({
            id: '<%=txtKeyRoute.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txtSchooling.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txt_gsjs.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txt_Copyright.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txt_Introduce.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });
        KE.init({
            id: '<%=txtLianXiFangShi.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore_HaveImage //功能模式(keMore:多功能,keSimple:简易)
        });

        $(function() {
            KE.create('<%=txt_gsjs.ClientID %>', 0); //创建编辑器
            KE.create('<%=txt_Copyright.ClientID %>', 0); //创建编辑器
            KE.create('<%=txt_Introduce.ClientID %>', 0); //创建编辑器
            KE.create('<%=txtKeyRoute.ClientID %>', 0); //创建编辑器
            KE.create('<%=txtSchooling.ClientID %>', 0); //创建编辑器
            KE.create("<%=txtLianXiFangShi.ClientID %>", 0);
            //删除原有loge
            $("img.close").click(function() {
                var that = $(this);
                $("td.updom").html("<input type=\"file\" name=\"workAgree\" />&nbsp;<span style='color: #666'>(上传图片尺寸:宽467px,高65px)</span>"); //生成添加协议的控件
            });

        })
    </script>

</asp:Content>
