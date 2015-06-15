<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="piliangshenhe.aspx.cs" Inherits="Web.caiwuguanli.piliangshenhe" Title="批量审核" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/utilsUri.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td width="15%" nowrap="nowrap">
                            <span class="lineprotitle">财务管理</span>
                        </td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">
                            所在位置&gt;&gt;财务管理 &gt;&gt; 批量审核
                        </td>
                    </tr>
                    <tr>
                        <td height="2" bgcolor="#000000" colspan="2">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="hr_10">
        </div>
        <ul class="fbTab">
            <li><a class="" id="two1" href="srtuankuan_list.aspx">应收账款</a></li>
            <li><a class="" href="TeamClear.aspx" id="two2">已结清账款</a></li>
            <li><a class="tabtwo-on" href="piliangshenhe.aspx" id="two3">批量审核</a></li>
            <div class="clearboth">
            </div>
        </ul>
        <div class="hr_10">
        </div>
        <div style="display: block;" id="con_two_1">
            <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td width="10" valign="top">
                            <img src="../images/yuanleft.gif">
                        </td>
                        <td>
                            <form id="form1" method="get" action="">
                            <div class="searchbox">
                                团号:<input name="tourcode" type="text" id="tourcode" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("tourcode") %>'
                                    class="searchinput" />
                                订单号:<input name="ordercode" id="ordercode" type="text" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("ordercode") %>'
                                    class="searchinput" />
                                客源单位:<input name="companyname" type="text" id="companyname" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("companyname") %>'
                                    class="searchinput" />
                                出团时间:<input name="leaveSTime" id="leaveSTime" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("leaveSTime") %>'
                                    type="text" onfocus="WdatePicker()" class="searchinput" />-<input type="text" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("leaveETime") %>'
                                        id="leaveETime" name="leaveETime" onfocus="WdatePicker()" class="searchinput" /><br />
                                收款日期:<input type="text" id="skStime" name="skStime" value='<%=EyouSoft.Common.Utils.GetQueryStringValue("skStime") %>'
                                    class="searchinput" onfocus="WdatePicker()" />-<input type="text" id="skETime" name="skETime"
                                        value='<%=EyouSoft.Common.Utils.GetQueryStringValue("skEtime") %>' class="searchinput"
                                        onfocus="WdatePicker()" />
                                审批状态:<select id="sltstatus" name="sltstatus">
                                    <option value="0">请选择</option>
                                    <option value="1">未审核</option>
                                    <option value="2">已审核</option>
                                </select>
                                <button type="submit" id="btsearch" style="vertical-align: middle; background-image: url(/images/searchbtn.gif);
                                    background-repeat: no-repeat; width: 62px; height: 21px; border-width: 0px;">
                                </button>
                            </div>
                            </form>
                        </td>
                        <td width="10" valign="top">
                            <img src="../images/yuanright.gif" style="background-repeat: repeat-x" alt="" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="margin-bottom: 5px; margin-top: 5px" class="btnbox">
                <a id="lotcheck" href="javascript:void(0)">批量审核</a>
            </div>
            <div class="tablelist" style="border-top: 1px solid #000;">
                <table width="100%" cellspacing="1" cellpadding="0" border="0" id="table001">
                    <tbody>
                        <tr>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                <input type="checkbox" name="ckall" />
                                序号
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                团号
                            </th>
                            <%--<th width="15%" bgcolor="#bddcf4" align="center">
                                订单号
                            </th>--%>
                            <th width="15%" bgcolor="#bddcf4" align="center">
                                客源单位
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                收款日期
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                收款人
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                收款金额
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                收款方式
                            </th>
                            <th width="15%" bgcolor="#bddcf4" align="center">
                                收款备注
                            </th>
                            <th width="10%" bgcolor="#bddcf4" align="center">
                                操作
                            </th>
                        </tr>
                        <asp:Repeater runat="server" ID="rptList">
                            <ItemTemplate>
                                 <tr class="<%#(Container.ItemIndex+1)%2==0 ?"odd":"even" %>">
                                    <td align="center" rowspan="1">
                                        <input type="checkbox" name="index" data-djid='<%#Eval("SKDengJiId") %>' data-orderid='<%#Eval("OrderId") %>' />
                                        <%#Container.ItemIndex + 1 + (EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("page")==""?"1":EyouSoft.Common.Utils.GetQueryStringValue("page"))-1) * pageSize%>
                                    </td>
                                    <td align="center" rowspan="1">
                                        <%#Eval("TourCode")%>
                                    </td>
                                    <%--<td align="center" rowspan="1">
                                        <%#Eval("OrderCode") %>
                                    </td>--%>
                                    <td width="82" align="center" class="td7">
                                        <%#Eval("KeHuMingCheng")%>
                                    </td>
                                    <td align="center" class="td1">
                                        <%#Convert.ToDateTime(Eval("SKRiQi")).ToString("yyyy-MM-dd")%>
                                    </td>
                                    <td align="center"><%#Eval("SKRenName") %></td>
                                    <td align="center" class="td2">
                                        <font color="red"><%#Eval("SKJinE", "{0:c2}")%></font>
                                    </td>
                                    <td align="center">
                                        <%#Eval("SKFangShi")!=null?((EyouSoft.Model.EnumType.TourStructure.RefundType)(Convert.ToInt32(Eval("SKFangShi")))).ToString():""%>
                                    </td>
                                    <td align="center" rowspan="1">
                                        <%#Eval("SKBeiZhu") %>
                                    </td>
                                    <td width="65" align="center" class="td8">
                                        <%#Convert.ToBoolean(Eval("SKStatus")) ? "<a href='javascript:void(0)' data-type='operator' data-class='cencer'>取消审核</a>" : "<a href='javascript:void(0)' data-type='operator' data-class='check'>审核</a>"%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Literal ID="lbemptydata" runat="server"></asp:Literal>
                        <tr>
                            <td bgcolor="#bddcf4" align="center">
                                合计：
                            </td>
                            <td bgcolor="#bddcf4" align="center" colspan="4">
                            </td>
                            <td width="" bgcolor="#bddcf4" align="center" class="talbe_btn td4">
                                <font class="fred" style="font-weight: bold">
                                    <asp:Literal ID="lbTotleMoney" runat="server"></asp:Literal></font>
                            </td>
                            <td bgcolor="#bddcf4" align="center" colspan="3">
                            </td>
                        </tr>
                        <tr>
                            <td height="30" align="right" colspan="12" class="pageup">
                                <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function(){
           shenhepage.PageInit();
        })
        var shenhepage={
            GoAjax: function(url) {
                $.newAjax({
                    type: "post",
                    cache: false,
                    url: url,
                    dataType: "string",
                    success: function(msg) {
                        alert(msg);
                        location.reload(); 
                    }
                });
            },
            params:null,
            PageInit:function(){
                 $(".tablelist").find("input[name='ckall']").click(function(){
                    if(this.checked){
                        $(".tablelist").find("input[name='index']").attr("checked","checked");
                    }
                    else{
                        $(".tablelist").find("input[name='index']").removeAttr("checked")
                    }
                })
                $("#sltstatus").val('<%=EyouSoft.Common.Utils.GetQueryStringValue("sltstatus")==""?"1":EyouSoft.Common.Utils.GetQueryStringValue("sltstatus") %>');
                if($("#sltstatus").val()=="2"){
                    $("#lotcheck").hide();
                }
                else{
                    $("#lotcheck").show();
                }
                shenhepage.params={dengjiid:"",orderId:""};
               
                //审核/取消审核 操作
                $(".tablelist").find("a[data-type='operator']").click(function(){
                   var dotype=$(this).attr("data-class");
                   var tip=(dotype=="check"?"审核":"取消审核");
                   if(window.confirm("您确定要"+tip+"该收款吗?"))
                   {
                       shenhepage.params.dengjiid= $(this).closest("tr").find("input[type='checkbox']").attr("data-djid");
                       shenhepage.params.orderId= $(this).closest("tr").find("input[type='checkbox']").attr("data-orderid");
                       if(window.location.href.indexOf('?')>=0){
                            shenhepage.GoAjax(window.location.href+"&dotype="+dotype+"&"+$.param(shenhepage.params));
                        }
                        else{
                             shenhepage.GoAjax(window.location.href+"?dotype="+dotype+"&"+$.param(shenhepage.params));
                        }
                    }
                })
                $("#lotcheck").click(function(){
                    var dengjiids="";
                    var orderids="";
                    $(".tablelist").find("input[type='checkbox'][name='index']:checked").each(function(){
                       if($(this).closest("tr").find("a[data-type='operator']").attr("data-class")=="check"){
                           dengjiids+= $(this).attr("data-djid")+",";
                           orderids+=$(this).attr("data-orderid")+",";
                       }
                    })
                    if(dengjiids=="" || orderids==""){
                        alert("请选择需要审核的条目!");
                        return false;
                    }
                    if(confirm("您确定要审核选中的收款吗?"))
                    {
                        shenhepage.params.dengjiid=dengjiids;
                        shenhepage.params.orderid=orderids;
                        if(window.location.href.indexOf('?')>=0){
                            shenhepage.GoAjax(window.location.href+"&dotype=check&"+$.param(shenhepage.params));
                        }
                        else{
                             shenhepage.GoAjax(window.location.href+"?dotype=check&"+$.param(shenhepage.params));
                        }
                    }
                })
            }
        }
    </script>

</asp:Content>
