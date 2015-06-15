<%@ Page Title="新增我要询价_组团端_芭比来_组团_芭比来" Language="C#" MasterPageFile="~/masterpage/Front.Master"
    AutoEventWireup="true" CodeBehind="SelectMoneyAdd.aspx.cs" Inherits="Web.GroupEnd.JourneyMoney.SelectMoneyAdd" %>

<%@ Register Src="~/UserControl/xianluWindow.ascx" TagName="xianluWindow" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<%@ Register Src="~/UserControl/ConProjectControl.ascx" TagName="ConProjectControl"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/LoadVisitors.ascx" TagName="LoadVisitors" TagPrefix="uc11" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/kindeditor/kindeditor.js"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td nowrap="nowrap" width="15%">
                            <span class="lineprotitle">行程报价</span>
                        </td>
                        <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right" nowrap="nowrap"
                            width="85%">
                            所在位置&gt;&gt; 行程报价&gt;&gt; 我要询价
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" bgcolor="#000000" height="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <div class="addlinebox">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>询价单位：
                        </td>
                        <td width="86%">
                            <label id="customerName" name="customerName">
                                <%=customerName %></label>
                            <%--<input class="searchinput searchinput02" id="customerName" name="customerName" readonly="readonly"
                                type="text" value="<%=customerName %>" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>联系人：
                        </td>
                        <td>
                            <label id="routeName" name="routeName">
                                <%=routeName %></label>
                            <%--<input class="searchinput searchinput02" id="routeName" name="routeName" readonly="readonly"
                                type="text" value="<%=routeName %>" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>联系电话：
                        </td>
                        <td>
                            <input class="searchinput searchinput02" id="vontactTel" name="vontactTel" type="text"
                                value="<%=vontactTel %>" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>线路名称：
                        </td>
                        <td>
                            <uc1:xianluWindow Id="xianluWindow1" runat="server" publishType="4" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>预计出团日期：
                        </td>
                        <td>
                            <asp:TextBox ID="txtLDate" CssClass="searchinput" runat="server" onfocus="WdatePicker()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>人数：
                        </td>
                        <td>
                            <%--<asp:TextBox ID="number" class="searchinput searchinput03"  name="number"runat="server"></asp:TextBox>--%>
                            <input class="searchinput searchinput03" id="number" name="number" type="text" value="<%= tsModel.PeopleNum %>" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">
                            <font class="xinghao">*</font>行程要求：
                        </td>
                        <td>
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            <asp:TextBox ID="txt_xinchen" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%if ((tsModel) != null && tsModel.XingCheng != null && tsModel.XingCheng.PlanAccessory != "")
                                          { %>
                                        <td colspan="3" align="left" class="updom">
                                            <a target="_blank" href="<%=tsModel.XingCheng.PlanAccessory  %>">合作协议</a><img style="cursor: pointer"
                                                src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                                        </td>
                                        <%}
                                          else
                                          { %>
                                        <td colspan="3" align="left" class="updom">
                                            上传附件：<input type="file" name="workAgree" /><asp:Literal ID="lstMsg" runat="server"></asp:Literal>
                                        </td>
                                        <%} %>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="hr_10" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">
                            <font class="xinghao">*</font>客人要求：
                        </td>
                        <td align="left">
                            <table id="userlist" align="left" border="0" cellpadding="2" cellspacing="1" width="95%">
                                <tbody>
                                    <tr class="odd">
                                        <td align="center" width="15%">
                                            项目
                                        </td>
                                        <td align="center" width="70%">
                                            具体要求
                                        </td>
                                        <td align="center" width="15%">
                                            操作
                                        </td>
                                    </tr>
                                    <tr class="even">
                                        <td align="center" colspan="3">
                                            <uc2:ConProjectControl ID="ConProjectControl1" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <%if (!tup)
                      { %>
                    <tr>
                        <td class="hr_10" colspan="2">
                        </td>
                    </tr>
                    <td align="right" width="15%">
                        价格组成 ：
                    </td>
                    <td>
                        <table border="0" cellpadding="2" cellspacing="1" width="95%" id="tblPrice">
                            <tr class="odd">
                                <td height="25" align="center" width="15%">
                                    项目
                                </td>
                                <td align="center" width="20%">
                                    接待标准
                                </td>
                                <td align="center" width="24%">
                                    我社报价
                                </td>
                            </tr>
                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                                        <td height="25" align="center">
                                            <%#Eval("ServiceType")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("Service")%>
                                        </td>
                                        <td align="center">
                                            <%#Eval("SelfPrice", "{0:c2}")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <%if (mlen == 0)
                              { %>
                            <tr align="center">
                                <td colspan="4">
                                    没有相关数据
                                </td>
                            </tr>
                            <%} %>
                        </table>
                    </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table border="0" cellpadding="2" cellspacing="1" width="95%">
                                <tr>
                                    <td style="text-align: right">
                                        <span>总计：<input type="text" class="searchinput" name="txtAllPrice" id="txtAllPrice"
                                            runat="server" /></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td class="hr_10" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" rowspan="2">
                            游客信息：<input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                        </td>
                        <td height="28" align="center" colspan="4">
                            <table cellspacing="0" cellpadding="0" border="0" align="right" width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">游客附件</asp:HyperLink>
                                        </td>
                                        <td align="right">
                                            上传附件：
                                            <input type="file" name="fileField" id="fileField" />
                                        </td>
                                        <td align="center">
                                            <a href="/Common/LoadVisitors.aspx" id="import">
                                                <img src="/images/sanping_03.gif">
                                                导入</a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr class="even">
                        <td align="center" colspan="4">
                            <div style="width: 100px; color: Red;" id="div_msg">
                            </div>
                            <table id="cusListTable" cellspacing="1" cellpadding="0" border="0" bgcolor="#bddcf4"
                                align="center" width="95%" style="margin: 10px 0pt;">
                                <tbody>
                                    <tr>
                                        <td height="5%" align="center" bgcolor="#E3F1FC">
                                            编号
                                        </td>
                                        <td height="25" bgcolor="#e3f1fc" align="center">
                                            姓名
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            类型
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            证件名称
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            证件号码
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            性别
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            联系电话
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            特服
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            操作 <a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>
                                        </td>
                                    </tr>
                                    <%=cusHtml%>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">
                            <font class="xinghao">*</font>特殊要求说明：
                        </td>
                        <td>
                            <textarea name="quotePlan" id="quotePlan" class="textareastyle"><% =tsModel.SpecialClaim %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="10">
                            <asp:Label ID="lblRemarks" runat="server" Text="专线备注："></asp:Label>
                        </td>
                        <td align="left" height="10">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Height="99px" Width="511px"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
            <ul class="tjbtn">
                <%if (tup)
                  { %><li>
                      <asp:LinkButton ID="linkSave" runat="server" OnClientClick="return Savetest()" OnClick="linkSave_Click">保存</asp:LinkButton></li><%} %>
                <li><a href="javascript:history.go(-1)">返回</a> </li>
                <div class="clearboth">
                </div>
            </ul>
        </div>
    </div>
    <div class="clearboth">
    </div>
    <input type="hidden" value="<%=tid %>" name="tid" />
    </form>

    <script type="text/javascript">
    
    function isnull(v, defaultValue) {
        if (v == null || !v)
            return defaultValue;
        else
            return v;
    }
        function CreateCusList(array) {
            $("#div_msg").html("正在加载名单...");
            var url = "/ashx/CreateCurList.ashx";
            $.newAjax({
                type: "Post",
                url: url,
                cache: false,
                data:"array=" +encodeURIComponent(array),
                dataType: "html",
                success: function(data) {
                 $("#div_msg").html("");
                    $("#cusListTable tr:last").after(data);
                    
            OrderEdit.RemoveDisabled();
                }
            });
        }
        
        function tres(res, idValue, msg) {
            if (idValue == "" || idValue == 0) {
                res += msg + "\n";
                return res;
            }
            return res;

        }
        $("img.close").click(function() {//删除原有协议添加新的协议
            var that = $(this);
            $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //生成添加协议的控件
        });

        function unbindadd() {//添加事件绑定
            demand.add();
            return false;
        }
        function unbinddel(thar) {//删除事件绑定
            demand.del(thar);
            return false;
        }

        function htmltxt() {
            var cls = $("#userlist").find("tr").length % 2 ? "even" : "odd";
            var html = "<tr class=\"" + cls + "\">" + ""
            return html;
        }

        var demand = {

            add: function() {//联系人添加
                var html = htmltxt(); //生成html
                $("#userlist").append(html); //追加行
                $("a.add").unbind().bind("click", function() {//取消所有的添加按钮事件绑定，进行重新绑定
                    unbindadd(); //绑定的事件
                });
                $("a.del").unbind().bind("click", function() {//取消所有的删除按钮事件绑定，进行重新绑定
                    unbinddel($(this)); //绑定的事件
                });
            },
            del: function(that) {//联系人删除
                var trlist = $("#userlist").find("tr"); //获取联系人人数
                if (trlist.length > 2) {//联系人人数大于2人
                    that.parent().parent().remove(); //删除本身
                } else {
                    that.parent().parent().find("input").val(""); //不删除
                }


            }
        }


        KE.init({
            id: '<%=txt_xinchen.ClientID %>', //编辑器对应文本框id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //宽高不可变
            items: keMore //功能模式(keMore:多功能,keSimple:简易)
        });
        function Savetest() {
            var res = "";
            var vontactTel = $("#vontactTel").attr("value"); //练习电话
            res = tres(res, vontactTel, "请填写联系电话！")

            var xianluWindow1 =<%=xianluWindow1.ClientID %>.val().length; //线路名称
            res = tres(res, xianluWindow1, "请填写线路名称！")

            var txtLDate = $("#<%=txtLDate.ClientID %>").val(); //预计出团日期
            res = tres(res, txtLDate, "请选择预计出团时间！")

            var number = $("#number").attr("value"); //人数
            res = tres(res, number, "请填写人数！")

            var txt_xinchen = document.getElementById("<%=this.txt_xinchen.ClientID%>");
            var values = txt_xinchen.value;
            res = tres(res, values, "请填写行程要求！")

            $("[name='txtStandard']").each(function() {
                if ($(this).val() == "") {
                    res += "客人要求不能为空！\n";
                };
            });

            var quotePlan = $("#quotePlan").attr("value"); //特殊要求说明
            res = tres(res, quotePlan, "请填写特殊要求说明！")

            var form = $(this).closest("form").get(0); //查找form
            //点击按纽触发执行的验证函数
            if (res != "") {
                alert(res);
                return false;
            }
/////////////游客验证
             var msg = "";
            var isb = true;
                var hd_IsRequiredTraveller = $("#<%=hd_IsRequiredTraveller.ClientID %>").val();
                if(false){
                //验证姓名不能为空
                $("[name='cusName']").each(function() {
               
                    if (isnull($(this).val() == "") ) {
                        isb = false;
                        msg += "- *游客姓名不能为空! \n";
                        return false;
                    }
                })
                 //定义身份证正则表达式
                var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                $("[name='cusCardType']").each(function() {
                    var val = $(this).parent().siblings().find("[name='cusCardNo']").val();
                    switch ($(this).val()) {
                        //验证身份证                    
                        case "1":
                            if (!(isIdCard.exec(val)) ) {
                                isb = false;
                                msg += "- *请输入正确的身份证! \n";
                                return false;
                            }
                            break;
                        //其他非空验证                   
                        default:
                            if (val == "" && isa) {
                                isb = false;
                                msg += "- *证件号不能为空! \n";
                                return false;
                            }
                            break;
                    }
                })
                //验证电话
                $("[name='cusPhone']").each(function() {
                    var lastMatch = /^(13|15|18|14)\d{9}$/;
                    var isPhone = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-?)?[1-9]\d{6,7}(\-\d{1,4})?$/;
                    if (isnull($(this).val() == "")) {
                        isb = false;
                        msg += "- *联系电话不能为空! \n";
                        return false;
                    }
                    else {
                        if (!(lastMatch.exec($(this).val())) && !(isPhone.exec($(this).val()))) {
                            msg += "- *请输入正确的联系电话! \n";
                            return false;
                        }
                    }
                })
                
                }
            
            if (msg.length > 0) {
                alert(msg);
                 return false;
            }
            return true;
        };
        $(function() {

            KE.create('<%=txt_xinchen.ClientID %>', 0); //创建编辑器
            //保存

        });


        var OrderEdit = {

            AddCus: function() {
                OrderEdit.CreateTR();
            },

            DelCus: function($obj) {
                if ($obj.prev("input[name='cusState']").val() == "ADD") {
                    $obj.parent().parent().remove();
                }
                else {
                    $obj.parent().parent().css("display", "none");
                    $obj.prev("input[name='cusState']").val("DEL");
                    $obj.parent().parent().remove();
                }
                var i=1;
                $("td[index]").each(function(){
                $(this).html(i);
                i++;
                })
            },
            
            //数据提交时，却除某些需要提交控件的disabled属性.
            RemoveDisabled:function(){
              $("select[name='cusType']").each(function() {
                    $(this).removeAttr("disabled");
                });
            },


            //检测输入是否是大于等于0整数
            CheckIsInt: function($obj) {
                var re = /^[0-9]([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            },

            //检测总金额输入是否正确
            CheckIsFloat: function($obj) {
                var re = /^[0-9]*.?([0-9])*$/;
                if (re.test($obj.val())) {

                    return false;
                }
                else {
                    return true;
                }
            },

           
            
             open:function(obj){
                    var pos=OrderEdit.getPosition(obj);
                    $("#tbl_last").show().css({left:Number(pos.Left)+"px",top:Number(pos.Top-70)+"px"});
                    return false;
                },
                
                close:function(){
                    $("#tbl_last").hide()
                },
                
                getPosition:function(obj){
                    var objPosition={Top:0,Left:0}
                    var offset = $(obj).offset();
                    objPosition.Left=offset.left;
                    objPosition.Top=offset.top+$(obj).height();
                    return objPosition;
                },

            //打开特服窗口
            OpenSpecive: function(tefuID) {
                parent.Boxy.iframeDialog({
                iframeUrl: "/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid="+tefuID,
                    title: "特服",
                    width: "420px",
                    height: "200px",
                    modal: true,
                    afterHide:function(){
                      //OrderEdit.GetSumMoney();
                   }
               });
                return false;
             },

            CreateTR: function() {
            var i=1;
            $("td[index]").each(function(){
            i++;
            })
                var d = new Date();
                var tefuID=d.getTime();
                var TR = '<tr>'
            + '<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index="0" align=\"center\">'+i+'</td><td height="25" bgcolor="#e3f1fc" align="center">'
            + '    <input type="text" class="searchinput" id="Text1" MaxLength="50"  name="cusName" />'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select1" title="请选择"  name="cusType">'
                    + '<option value=""  selected="selected">请选择</option>'
                    + '<option value="1">成人</option>'
                    + '<option value="2">儿童</option>'
                + '</select>'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select2" name="cusCardType">'
                    + '<option value="0" selected="selected">请选择证件</option>'
                    + '<option value="1">身份证</option>'
                    + '<option value="2">护照</option>'
                    + '<option value="3">军官证</option>'
                    + '<option value="4">台胞证</option>'
                    + '<option value="5">港澳通行证</option>'
                    + '<option value="6">户口本</option>'
                + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                + ' <input type="text" class="searchinput searchinput02" id="text2" onblur="getSex(this)" MaxLength="150" name="cusCardNo">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" name="ddlSex" align="center">'
                 + '<select id="Select3" class="ddlSex" name="cusSex">'
                     + '<option value="0" selected="selected">请选择</option>'
                     + '<option value="1">男</option>'
                     + '<option value="2">女</option>'
                 + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                 + '<input type="text" class="searchinput" id="Text3" MaxLength="50" name="cusPhone">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="6%">'
              + '<input type="hidden" name="txtItem" value="" />'
              + '<input type="hidden" name="txtServiceContent" value="" />'
              + '<input type="hidden" name="ddlOperate" value="" />'
              + '<input type="hidden" name="txtCost" value="" />'
               + '<input id="' + tefuID + '" type="hidden" name="specive" value=""/>'
                 + '<a sign="speService" href="javascript:void(0)" onclick="OrderEdit.OpenSpecive('+tefuID+')">特服</a>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="12%">'
              + '<input type="hidden" name="cusID" value="" />'
             + '<a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">添加</a>&nbsp;'
             + '<input type="hidden" name="cusState" value="ADD" />'
             + '<a sign="del" href="javascript:void(0)" onclick="OrderEdit.DelCus($(this))">删除</a>'
             + '</td>'
         + '</tr>';
                $("#cusListTable tr:last").after(TR);
            }

        }
                            //根据用户输入的身份证号判断性别
    function getSex(obj)
    {
        var val=$(obj).val();
        var tr =$(obj).parent().parent();
        var sex=tr.children().children("select[class='ddlSex']");
        var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                        if (isIdCard.exec(val)) {
                    if (15 == val.length) {// 15位身份证号码
                        if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                            sex.val(1);
                        else
                            sex.val(2);
                    }

                    if (18 == val.length) {// 18位身份证号码
                        if (parseInt(val.charAt(16) / 2) * 2 != val.charAt(16))
                            sex.val(1);
                        else
                            sex.val(2);
                    }
                } else {
                sex.val(0);
                }
    }
        $(function(){
            $("#import").click(function(){                
                parent.Boxy.iframeDialog({
                    iframeUrl: $(this).attr("href"),
                    width: "853px",
                    height: "514px",
                    async: false,
                    title: "导入",
                    modal: true
                });
                return false;
            });
            OrderEdit.RemoveDisabled();
        });
    </script>

</asp:Content>
