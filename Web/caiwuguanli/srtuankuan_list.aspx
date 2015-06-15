<%@ Page Language="C#" Title="�ſ�����" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true"
    CodeBehind="srtuankuan_list.aspx.cs" Inherits="Web.caiwuguanli.srtuankuan_list" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Src="../UserControl/selectXianlu.ascx" TagName="selectXianlu" TagPrefix="uc1" %>
<%@ Register src="../UserControl/selectOperator.ascx" tagname="selectOperator" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    
    <script type="text/javascript" src="/js/utilsUri.js"></script>
<style>
    .rpt_td{ border-collapse:collapse; border-right:none;}
.rpt_td td {overflow:hidden; border-right:1px solid #fff;border-bottom:1px solid #fff;word-break:break-all; vertical-align:middle;}
.tablelist table th { white-space:normal}
</style>
<%--<script>
    $(function() {
        $(".rpt_td").each(function() {
            $(this).height( $(this).closest("td").height());
        });

        $(".rpt_td td").each(function() {
            $(this).css("line-height", "30px");
        });
    });
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
           
    <script type="text/javascript">
        $(function() {

            $(".soukuan").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�տ�",
                    modal: true,
                    width: "820px",
                    height: "220px",
                    afterHide: function() {
                        parent.location.href = parent.location.href;
                    }
                });
                return false;
            });
            $(".tuikuan").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�˿�",
                    modal: true,
                    width: "820px",
                    height: "220px",
                    afterHide: function() {
                        parent.location.href = parent.location.href;
                    }
                });
                return false;
            });
            $("#link3").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�˿�",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });
                return false;
            });
            $(".change").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�����Ϣ",
                    modal: true,
                    width: "400px",
                    height: "200px"
                });
                return false;
            });
            $(".showSale").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "������Ϣ",
                    modal: true,
                    width: "890px",
                    height: "600px"
                });
                return false;
            });
            $("#btn2").click(function() {
                new Boxy("&lt;div style='width:400px;height:200px;background-color:gray'&gt;�Զ���HTML��ʾ&lt;/div&gt;", { title: "�Զ���HTML" });
            });
        });

        //�Զ������� _v��������
        function sortSelf(_v) {
            var params = utilsUri.getUrlParams(["page", "sorttype"]);
            params["sorttype"] = _v;
            window.location.href = utilsUri.createUri(null, params);

            return false;
        }
</script>
			<div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody><tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">�������</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);">����λ��&gt;&gt;������� &gt;&gt; �ſ�����</td>
            </tr>
            <tr>
              <td height="2" bgcolor="#000000" colspan="2"></td>
            </tr>
          </tbody></table>
        </div>
        <div class="hr_10"></div>
        <ul class="fbTab">
          <li><a class="tabtwo-on" id="two1" href="#">Ӧ���˿�</a></li>
          <li><a id="two2" href="TeamClear.aspx" class="">�ѽ����˿�</a></li>
          <li><a id="two3" href="piliangshenhe.aspx" class="">�������</a></li>
          <div class="clearboth"></div>
        </ul>
        <div class="hr_10"></div>
        <div id="con_two_1" style="display: block;">
        	<table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
              <tbody><tr>
                <td width="10" valign="top"><img src="../images/yuanleft.gif"></td>
                <td><div class="searchbox">
                    <label>�źţ�</label> <input type="text" id="txt_teamNum" class="searchinput" name="txt_teamNum" runat="server">
                    <label>�����ţ�</label> <input type="text" id="txt_order" class="searchinput" name="txt_order" runat="server">
                    <label>��Դ��λ��</label> <input type="text" class="searchinput searchinput" id="txt_com" name="txt_com" runat="server">
                    
                    <label>
                    <uc2:selectOperator ID="selectOperator1" runat="server" Title="����Ա" /><br />
                    ����ʱ�䣺</label> <input type="text" onfocus="WdatePicker()" id="txt_goDate" class="searchinput" name="txt_goDate" runat="server">
                     ��</label> <input type="text" onfocus="WdatePicker()" id="txt_endDate" class="searchinput" name="txt_endDate" runat="server">
                    <label>�տ����ڣ�</label> <input type="text" onfocus="WdatePicker()" id="txt_addDate" class="searchinput" name="txt_addDate" runat="server" />
                    ��
                    <input type="text" onfocus="WdatePicker()" id="txtRegEDate" class="searchinput" name="txtRegEDate" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("regenddate") %>" />
                    <label>״̬��</label><select id="seleStatus" runat="server">
                      <option  value="-1">--��ѡ��--</option><option value="1">�տ�δ����</option>
                        <option value="2">�˿����</option>
                    </select>
                    <br />
                    ����ɸѡ��
                    <select id="txtQueryAmountType" name="txtQueryAmountType">
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.None %>">��ѡ��</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.Ӧ�տ� %>">Ӧ�տ�</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.���տ� %>">���տ�</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.δ�տ� %>">δ�տ�</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.���մ���� %>">���մ����</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.�˿����� %>">�˿�����</option>
                    </select>
                    <select name="txtQueryAmountOperator" id="txtQueryAmountOperator">
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None %>">��ѡ��</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.���ڵ��� %>">���ڵ���</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.���� %>">����</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.С�ڵ��� %>">С�ڵ���</option>                        
                    </select>                   
                    <input type="text" id="txtQueryAmount" name="txtQueryAmount" class="searchinput" value="<%=EyouSoft.Common.Utils.GetQueryStringValue("queryAmount") %>" />
                     �ƻ����ͣ�<select id="txtQueryTourType">
                        <option value="-1">��ѡ��</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourType.ɢƴ�ƻ� %>">ɢƴ�ƻ�</option>
                        <option value="<%=(int)EyouSoft.Model.EnumType.TourStructure.TourType.�ŶӼƻ� %>">�ŶӼƻ�</option>
                    </select>
                    
                    <script type="text/javascript">
                        $(document).ready(function() {
                            $("#txtQueryAmountType").attr("value", '<%=EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("queryAmountType"))%>');
                            $("#txtQueryAmountOperator").attr("value", '<%=EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("queryAmountOperator"))%>');

                            $("#txtQueryTourType").attr("value", '<%=EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("queryTourType"),-1)%>')
                        });
                    </script>
                    
                    <label><asp:ImageButton ID="ImageButton1" style=" vertical-align:middle;" ImageUrl="/images/searchbtn.gif" 
                        runat="server" onclick="ImageButton1_Click" /></label>
                    
                    <a href="javascript:;" onclick="DownExcel();return false;">
                        <img src="/images/export.xls.png" alt="" width="68" height="27" style="margin-top:0px;border:0px" />
                    </a>
                       
                  </div></td>
                <td width="10" valign="top"><img src="../images/yuanright.gif"></td>
              </tr>
            </tbody></table>
            <div class="hr_10"></div>
            <div style="border-top: 2px solid rgb(0, 0, 0);" class="tablelist">
          <table width="100%" id="table001" cellspacing="1" cellpadding="0" border="0">
            <tbody><tr>
              <%--<th width="3%" bgcolor="#bddcf4" align="center">&nbsp;</th>--%>
              <th width="9%" bgcolor="#bddcf4" align="center">�ź�</th>
              <th width="9%" bgcolor="#bddcf4" align="center">��·����</th>
              <th width="8%" bgcolor="#bddcf4" align="center"><a onclick="return sortSelf(0)" style="color: " href="javascript:void(0)">��</a>��������<a onclick="return sortSelf(1)" style="color: " href="javascript:void(0)">��</a></th>
              <%--<th width="7%" bgcolor="#bddcf4" align="center">״̬</th>   --%>           
              <th width="8%" bgcolor="#bddcf4" align="center">��Դ��λ</th>
              <th width="5%" bgcolor="#bddcf4" align="center">����</th>
              <th width="7%" bgcolor="#bddcf4" align="center">����Ա</th>
              <th width="7%" bgcolor="#bddcf4" align="center">�Է�������</th>
              <th width="6%" bgcolor="#bddcf4" align="center">�Է��ź�</th>
              <th width="6%" bgcolor="#bddcf4" align="center">Ӧ�տ�</th>
              <th width="6%" bgcolor="#bddcf4" align="center">���տ�</th>
              <th width="7%" bgcolor="#bddcf4" align="center">δ�տ�</th>
              <th width="6%" bgcolor="#bddcf4" align="center">�����</th>
              <th width="6%" bgcolor="#bddcf4" align="center">�˿����</th>
              <th width="10%" bgcolor="#bddcf4" align="center">����</th>
            </tr>
            
            <cc2:CustomRepeater runat="server" ID="rpt_list1" OnItemDataBound="rpt_list1_ItemDataBound">
            <ItemTemplate>
            <tr class="<%=i%2==0 ?"even":"odd" %>">
             <%-- <td align="center"><input type="checkbox" id="checkbox" name="checkbox"></td>--%>
              <td align="center" <asp:Literal ID="Literal1" runat="server"></asp:Literal>><%#isSingle(Eval("TourType")) ? Eval("TourNo") : Eval("TourNo")%><asp:Literal ID="ltrTourType" runat="server"></asp:Literal></td>
              <td align="center" <asp:Literal ID="Literal2" runat="server"></asp:Literal>><%#(EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType") == EyouSoft.Model.EnumType.TourStructure.TourType.������� ? "�������" : Eval("RouteName")%></td>
              <td align="center" <asp:Literal ID="Literal3" runat="server"></asp:Literal>>
                <%#isSingle(Eval("TourType")) ? "" : DateTime.Parse(Eval("LeaveDate").ToString()).ToString("yyyy-MM-dd")%>
              </td>              
               <%--<td align="center" <asp:Literal ID="Literal4" runat="server"></asp:Literal>><%#Eval("TourStatus")%></td>--%>
               <%j=0; %>
              <asp:Repeater ID="rpt_sList" runat="server">
              <ItemTemplate>
              
              <%if(j>0){ %>
              
              <tr class='<%=i%2==0?"even":"odd"%>'>
              <%} %>
              <td align="center" width="82" class="td7"><%#Eval("CompanyName")%></td>
              <td align="center"  class="td1" style="width:58px;_width:60px;" class="talbe_btn"><a href="/sales/sale_show.aspx?orderID=<%#Eval("orderID") %>&act=show" class="showSale"><%#Eval("PepoleNum")%></a></td>
              <td align="center"  class="td2" style="width:58px;_width:58px;"><%#Eval("SalerName")%></td>
              <th align="center"><%#Eval("BuyerContactName") %></th>
              <td style="text-align:center"><%#Eval("BuyerTourCode") %></td>
              <td align="center" class="td3" style="width:58px;_width:58px;"><font class="fred">��<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("FinanceSum").ToString())%></font><%#isChange(Eval("OrderId").ToString(), (EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo)Eval("OrderAmountPlusInfo"))%></td>
              <td align="center" width="57" class="talbe_btn td4"><font class="fred">��<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("HasCheckMoney").ToString())%></font></td>
              <td align="center" width="58" class="talbe_btn td5"><font class="fred">��<%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotReciveMoney").ToString())%></font></td>
              <td align="center" width="58" class="td6"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString( Eval("NotCheckMoney").ToString())%></td>              
              <td align="center" width="58" class="td6"><%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Eval("NotCheckTuiMoney").ToString())%></td>              
              <td align="center" width="65" class="td8"><a id="link1" class="soukuan" href="/sales/sale_shoukuan.aspx?act=pass&OrderID=<%#Eval("OrderId") %>"><font class="fblue">�տ�</font></a> <a class="tuikuan" href="/sales/sale_tuikuan.aspx?act=pass&OrderID=<%#Eval("OrderId") %>"><font class="fblue">�˿�</font></a></td>
            
            </tr>
             <%j++; %>
              </ItemTemplate>
            </asp:Repeater>
            </tr>   
            <%i++; %>
            </ItemTemplate>
                </cc2:CustomRepeater>
            <tr>
              <%--<th width="3%" bgcolor="#bddcf4" align="center">&nbsp;</th>--%>
              <th bgcolor="#bddcf4"  colspan="4" align="left">�ϼƣ�</th>
              <td width="" bgcolor="#bddcf4" align="center"><%=peopleNumber%></td>
              <td width="" bgcolor="#bddcf4" align="center"></td>
                <td width="" bgcolor="#bddcf4" align="center">
                </td>
                <td  bgcolor="#bddcf4"></td>
              <td width="" bgcolor="#bddcf4" align="center" class="talbe_btn td4"><font class="fred">��<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(financeSum)%></font></td>
              <td width="" bgcolor="#bddcf4" align="center" class="talbe_btn td4"><font class="fred">��<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(hasCheckMoney)%></font></td>
              <td width="" bgcolor="#bddcf4" align="center" class="talbe_btn td4"><font class="fred">��<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(financeSum - hasCheckMoney)%></font></td>
              <td width="" bgcolor="#bddcf4" align="center"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(notCheckMoney)%></td>
              <td width="" bgcolor="#bddcf4" align="center"><%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(NotCheckTuiMoney)%></td>
              <td width="10%" bgcolor="#bddcf4" align="center" colspan="2"></td>
            </tr>
            <tr>
              <td height="30" align="right" class="pageup" colspan="14">              
                  <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />              
              </td>
            </tr>
          </tbody></table>
        </div>
        </div>
      </div>
			<!-- InstanceEndEditable -->            </div>
      
    </form>
    <script>
        $(function() {
            $("#<%=ImageButton1.ClientID %>").click(function() {
                var param = "tourCode=" + $("#<%=txt_teamNum.ClientID%>").val();
                param += "&orderCode=" + $("#<%=txt_order.ClientID %>").val();
                param += "&companyName=" + $("#<%=txt_com.ClientID %>").val();
                param += "&companyId=";
                param += "&OperatorId="+<%=selectOperator1.ClientID %>.GetOperatorId();
                param += "&OperatorName="+escape(<%=selectOperator1.ClientID %>.GetOperatorName());
                param += "&beginDate="+$("#<%=txt_goDate.ClientID %>").val();
                param += "&endDate="+$("#<%=txt_endDate.ClientID %>").val();
                param += "&addDate="+$("#<%=txt_addDate.ClientID %>").val();
                param += "&status="+$("#<%=seleStatus.ClientID %>").val();
                param+="&queryAmountType="+$("#txtQueryAmountType").val();
                param+="&queryAmountOperator="+$("#txtQueryAmountOperator").val();
                param+="&queryAmount="+$("#txtQueryAmount").val();
                param+="&regenddate="+$("#txtRegEDate").val();
                param+="&queryTourType="+$("#txtQueryTourType").val();
                location.href = "srtuankuan_list.aspx?" + param;
                return false;
            });
        });
        
        function DownExcel() {
            var param = "tourCode=" + $("#<%=txt_teamNum.ClientID%>").val();
                param += "&orderCode=" + $("#<%=txt_order.ClientID %>").val();
                param += "&companyName=" + $("#<%=txt_com.ClientID %>").val();
                param += "&companyId=";
                param += "&OperatorId="+<%=selectOperator1.ClientID %>.GetOperatorId();
                param += "&OperatorName="+escape(<%=selectOperator1.ClientID %>.GetOperatorName());
                param += "&beginDate="+$("#<%=txt_goDate.ClientID %>").val();
                param += "&endDate="+$("#<%=txt_endDate.ClientID %>").val();
                param += "&addDate="+$("#<%=txt_addDate.ClientID %>").val();
                param += "&status="+$("#<%=seleStatus.ClientID %>").val();
                param+="&queryAmountType="+$("#txtQueryAmountType").val();
                param+="&queryAmountOperator="+$("#txtQueryAmountOperator").val();
                param+="&queryAmount="+$("#txtQueryAmount").val();
                param+="&regenddate="+$("#txtRegEDate").val();
                param+="&queryTourType="+$("#txtQueryTourType").val();
                param += "&method=downexcel" + "&recordcount=<%=count %>";
                location.href = "srtuankuan_list.aspx?" + param;
                return false;
        }
    </script>
</asp:Content>
