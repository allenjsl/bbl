<%@ Page Title="������Ҫѯ��_���Ŷ�_�ű���_����_�ű���" Language="C#" MasterPageFile="~/masterpage/Front.Master"
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
                            <span class="lineprotitle">�г̱���</span>
                        </td>
                        <td style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);" align="right" nowrap="nowrap"
                            width="85%">
                            ����λ��&gt;&gt; �г̱���&gt;&gt; ��Ҫѯ��
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
                            <font class="xinghao">*</font>ѯ�۵�λ��
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
                            <font class="xinghao">*</font>��ϵ�ˣ�
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
                            <font class="xinghao">*</font>��ϵ�绰��
                        </td>
                        <td>
                            <input class="searchinput searchinput02" id="vontactTel" name="vontactTel" type="text"
                                value="<%=vontactTel %>" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>��·���ƣ�
                        </td>
                        <td>
                            <uc1:xianluWindow Id="xianluWindow1" runat="server" publishType="4" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>Ԥ�Ƴ������ڣ�
                        </td>
                        <td>
                            <asp:TextBox ID="txtLDate" CssClass="searchinput" runat="server" onfocus="WdatePicker()"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="30" width="15%">
                            <font class="xinghao">*</font>������
                        </td>
                        <td>
                            <%--<asp:TextBox ID="number" class="searchinput searchinput03"  name="number"runat="server"></asp:TextBox>--%>
                            <input class="searchinput searchinput03" id="number" name="number" type="text" value="<%= tsModel.PeopleNum %>" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">
                            <font class="xinghao">*</font>�г�Ҫ��
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
                                            <a target="_blank" href="<%=tsModel.XingCheng.PlanAccessory  %>">����Э��</a><img style="cursor: pointer"
                                                src="/images/fujian_x.gif" width="14" height="13" class="close" alt="" />
                                        </td>
                                        <%}
                                          else
                                          { %>
                                        <td colspan="3" align="left" class="updom">
                                            �ϴ�������<input type="file" name="workAgree" /><asp:Literal ID="lstMsg" runat="server"></asp:Literal>
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
                            <font class="xinghao">*</font>����Ҫ��
                        </td>
                        <td align="left">
                            <table id="userlist" align="left" border="0" cellpadding="2" cellspacing="1" width="95%">
                                <tbody>
                                    <tr class="odd">
                                        <td align="center" width="15%">
                                            ��Ŀ
                                        </td>
                                        <td align="center" width="70%">
                                            ����Ҫ��
                                        </td>
                                        <td align="center" width="15%">
                                            ����
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
                        �۸���� ��
                    </td>
                    <td>
                        <table border="0" cellpadding="2" cellspacing="1" width="95%" id="tblPrice">
                            <tr class="odd">
                                <td height="25" align="center" width="15%">
                                    ��Ŀ
                                </td>
                                <td align="center" width="20%">
                                    �Ӵ���׼
                                </td>
                                <td align="center" width="24%">
                                    ���籨��
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
                                    û���������
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
                                        <span>�ܼƣ�<input type="text" class="searchinput" name="txtAllPrice" id="txtAllPrice"
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
                            �ο���Ϣ��<input type="hidden" value="" runat="server" id="hd_IsRequiredTraveller">
                        </td>
                        <td height="28" align="center" colspan="4">
                            <table cellspacing="0" cellpadding="0" border="0" align="right" width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hykCusFile" runat="server" Visible="false">�ο͸���</asp:HyperLink>
                                        </td>
                                        <td align="right">
                                            �ϴ�������
                                            <input type="file" name="fileField" id="fileField" />
                                        </td>
                                        <td align="center">
                                            <a href="/Common/LoadVisitors.aspx" id="import">
                                                <img src="/images/sanping_03.gif">
                                                ����</a>
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
                                            ���
                                        </td>
                                        <td height="25" bgcolor="#e3f1fc" align="center">
                                            ����
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            ����
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            ֤������
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            ֤������
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            �Ա�
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            ��ϵ�绰
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            �ط�
                                        </td>
                                        <td bgcolor="#e3f1fc" align="center">
                                            ���� <a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">���</a>
                                        </td>
                                    </tr>
                                    <%=cusHtml%>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="15%">
                            <font class="xinghao">*</font>����Ҫ��˵����
                        </td>
                        <td>
                            <textarea name="quotePlan" id="quotePlan" class="textareastyle"><% =tsModel.SpecialClaim %></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" height="10">
                            <asp:Label ID="lblRemarks" runat="server" Text="ר�߱�ע��"></asp:Label>
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
                      <asp:LinkButton ID="linkSave" runat="server" OnClientClick="return Savetest()" OnClick="linkSave_Click">����</asp:LinkButton></li><%} %>
                <li><a href="javascript:history.go(-1)">����</a> </li>
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
            $("#div_msg").html("���ڼ�������...");
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
        $("img.close").click(function() {//ɾ��ԭ��Э������µ�Э��
            var that = $(this);
            $("td.updom").html("<input type=\"file\" name=\"workAgree\" />"); //�������Э��Ŀؼ�
        });

        function unbindadd() {//����¼���
            demand.add();
            return false;
        }
        function unbinddel(thar) {//ɾ���¼���
            demand.del(thar);
            return false;
        }

        function htmltxt() {
            var cls = $("#userlist").find("tr").length % 2 ? "even" : "odd";
            var html = "<tr class=\"" + cls + "\">" + ""
            return html;
        }

        var demand = {

            add: function() {//��ϵ�����
                var html = htmltxt(); //����html
                $("#userlist").append(html); //׷����
                $("a.add").unbind().bind("click", function() {//ȡ�����е���Ӱ�ť�¼��󶨣��������°�
                    unbindadd(); //�󶨵��¼�
                });
                $("a.del").unbind().bind("click", function() {//ȡ�����е�ɾ����ť�¼��󶨣��������°�
                    unbinddel($(this)); //�󶨵��¼�
                });
            },
            del: function(that) {//��ϵ��ɾ��
                var trlist = $("#userlist").find("tr"); //��ȡ��ϵ������
                if (trlist.length > 2) {//��ϵ����������2��
                    that.parent().parent().remove(); //ɾ������
                } else {
                    that.parent().parent().find("input").val(""); //��ɾ��
                }


            }
        }


        KE.init({
            id: '<%=txt_xinchen.ClientID %>', //�༭����Ӧ�ı���id
            width: '630px',
            height: '240px',
            skinsPath: '/js/kindeditor/skins/',
            pluginsPath: '/js/kindeditor/plugins/',
            scriptPath: '/js/kindeditor/skins/',
            resizeMode: 0, //��߲��ɱ�
            items: keMore //����ģʽ(keMore:�๦��,keSimple:����)
        });
        function Savetest() {
            var res = "";
            var vontactTel = $("#vontactTel").attr("value"); //��ϰ�绰
            res = tres(res, vontactTel, "����д��ϵ�绰��")

            var xianluWindow1 =<%=xianluWindow1.ClientID %>.val().length; //��·����
            res = tres(res, xianluWindow1, "����д��·���ƣ�")

            var txtLDate = $("#<%=txtLDate.ClientID %>").val(); //Ԥ�Ƴ�������
            res = tres(res, txtLDate, "��ѡ��Ԥ�Ƴ���ʱ�䣡")

            var number = $("#number").attr("value"); //����
            res = tres(res, number, "����д������")

            var txt_xinchen = document.getElementById("<%=this.txt_xinchen.ClientID%>");
            var values = txt_xinchen.value;
            res = tres(res, values, "����д�г�Ҫ��")

            $("[name='txtStandard']").each(function() {
                if ($(this).val() == "") {
                    res += "����Ҫ����Ϊ�գ�\n";
                };
            });

            var quotePlan = $("#quotePlan").attr("value"); //����Ҫ��˵��
            res = tres(res, quotePlan, "����д����Ҫ��˵����")

            var form = $(this).closest("form").get(0); //����form
            //�����Ŧ����ִ�е���֤����
            if (res != "") {
                alert(res);
                return false;
            }
/////////////�ο���֤
             var msg = "";
            var isb = true;
                var hd_IsRequiredTraveller = $("#<%=hd_IsRequiredTraveller.ClientID %>").val();
                if(false){
                //��֤��������Ϊ��
                $("[name='cusName']").each(function() {
               
                    if (isnull($(this).val() == "") ) {
                        isb = false;
                        msg += "- *�ο���������Ϊ��! \n";
                        return false;
                    }
                })
                 //�������֤������ʽ
                var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                $("[name='cusCardType']").each(function() {
                    var val = $(this).parent().siblings().find("[name='cusCardNo']").val();
                    switch ($(this).val()) {
                        //��֤���֤                    
                        case "1":
                            if (!(isIdCard.exec(val)) ) {
                                isb = false;
                                msg += "- *��������ȷ�����֤! \n";
                                return false;
                            }
                            break;
                        //�����ǿ���֤                   
                        default:
                            if (val == "" && isa) {
                                isb = false;
                                msg += "- *֤���Ų���Ϊ��! \n";
                                return false;
                            }
                            break;
                    }
                })
                //��֤�绰
                $("[name='cusPhone']").each(function() {
                    var lastMatch = /^(13|15|18|14)\d{9}$/;
                    var isPhone = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-?)?[1-9]\d{6,7}(\-\d{1,4})?$/;
                    if (isnull($(this).val() == "")) {
                        isb = false;
                        msg += "- *��ϵ�绰����Ϊ��! \n";
                        return false;
                    }
                    else {
                        if (!(lastMatch.exec($(this).val())) && !(isPhone.exec($(this).val()))) {
                            msg += "- *��������ȷ����ϵ�绰! \n";
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

            KE.create('<%=txt_xinchen.ClientID %>', 0); //�����༭��
            //����

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
            
            //�����ύʱ��ȴ��ĳЩ��Ҫ�ύ�ؼ���disabled����.
            RemoveDisabled:function(){
              $("select[name='cusType']").each(function() {
                    $(this).removeAttr("disabled");
                });
            },


            //��������Ƿ��Ǵ��ڵ���0����
            CheckIsInt: function($obj) {
                var re = /^[0-9]([0-9])*$/;
                if (re.test($obj.val())) {
                    return false;
                }
                else {
                    return true;
                }
            },

            //����ܽ�������Ƿ���ȷ
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

            //���ط�����
            OpenSpecive: function(tefuID) {
                parent.Boxy.iframeDialog({
                iframeUrl: "/Common/SpecialService.aspx?desiframeId=<%=Request.QueryString["iframeId"]%>&tefuid="+tefuID,
                    title: "�ط�",
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
                + '<select id="Select1" title="��ѡ��"  name="cusType">'
                    + '<option value=""  selected="selected">��ѡ��</option>'
                    + '<option value="1">����</option>'
                    + '<option value="2">��ͯ</option>'
                + '</select>'
            + '</td>'
            + '<td bgcolor="#e3f1fc" align="center">'
                + '<select id="Select2" name="cusCardType">'
                    + '<option value="0" selected="selected">��ѡ��֤��</option>'
                    + '<option value="1">���֤</option>'
                    + '<option value="2">����</option>'
                    + '<option value="3">����֤</option>'
                    + '<option value="4">̨��֤</option>'
                    + '<option value="5">�۰�ͨ��֤</option>'
                    + '<option value="6">���ڱ�</option>'
                + '</select>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center">'
                + ' <input type="text" class="searchinput searchinput02" id="text2" onblur="getSex(this)" MaxLength="150" name="cusCardNo">'
             + '</td>'
             + '<td bgcolor="#e3f1fc" name="ddlSex" align="center">'
                 + '<select id="Select3" class="ddlSex" name="cusSex">'
                     + '<option value="0" selected="selected">��ѡ��</option>'
                     + '<option value="1">��</option>'
                     + '<option value="2">Ů</option>'
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
                 + '<a sign="speService" href="javascript:void(0)" onclick="OrderEdit.OpenSpecive('+tefuID+')">�ط�</a>'
             + '</td>'
             + '<td bgcolor="#e3f1fc" align="center" width="12%">'
              + '<input type="hidden" name="cusID" value="" />'
             + '<a sign="add" href="javascript:void(0)" onclick="OrderEdit.AddCus()">���</a>&nbsp;'
             + '<input type="hidden" name="cusState" value="ADD" />'
             + '<a sign="del" href="javascript:void(0)" onclick="OrderEdit.DelCus($(this))">ɾ��</a>'
             + '</td>'
         + '</tr>';
                $("#cusListTable tr:last").after(TR);
            }

        }
                            //�����û���������֤���ж��Ա�
    function getSex(obj)
    {
        var val=$(obj).val();
        var tr =$(obj).parent().parent();
        var sex=tr.children().children("select[class='ddlSex']");
        var isIdCard = /(^\d{15}$)|(^\d{17}[0-9Xx]$)/;
                        if (isIdCard.exec(val)) {
                    if (15 == val.length) {// 15λ���֤����
                        if (parseInt(val.charAt(14) / 2) * 2 != val.charAt(14))
                            sex.val(1);
                        else
                            sex.val(2);
                    }

                    if (18 == val.length) {// 18λ���֤����
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
                    title: "����",
                    modal: true
                });
                return false;
            });
            OrderEdit.RemoveDisabled();
        });
    </script>

</asp:Content>
