<%@ Page Language="C#" Title="ɢ�����췢" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="DaydayPublish.aspx.cs" Inherits="Web.sanping.DaydayPublish" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>

<%@ Register src="../UserControl/selectXianlu.ascx" tagname="selectXianlu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <form id="form1" runat="server">
<div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<!--
<script type="text/javascript">
$(function(){
	$("#link1").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"���˼�",
			modal:true,
			width:"420px",
			height:"125px"
		});
		return false;
	});
	$("#link2").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"��ͯ��",
			modal:true,
			width:"420px",
			height:"125px"
		});
		return false;
});
	
	$("#link3").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"����",
			modal:true,
			width:"900px",
			height:"550px"
		});
		return false;
	});
	$("#link4").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"���ŵؽ�",
			modal:true,
			width:"900px",
			height:"620px"
		});
		return false;
	});
	$("#link5").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"��Ʊ",
			modal:true,
			width:"900px",
			height:"480px"
		});
		return false;
	});
	$("#link6").click(function(){
		var url = $(this).attr("href");
		Boxy.iframeDialog({
			iframeUrl:url,
			title:"���ݴ�ӡ",
			modal:true,
			width:"200px",
			height:"170px"
		});
		return false;
	});
	$("#btn2").click(function(){
		new Boxy("&lt;div style='width:400px;height:200px;background-color:gray'&gt;�Զ���HTML��ʾ&lt;/div&gt;",{title:"�Զ���HTML"});
	});
});
</script>
-->
      <div class="mainbody">
        <div class="lineprotitlebox">
          <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tbody><tr>
              <td width="15%" nowrap="nowrap"><span class="lineprotitle">ɢƴ�ƻ�</span></td>
              <td width="85%" nowrap="nowrap" align="right" style="padding: 0pt 10px 2px 0pt; color: rgb(19, 80, 159);"> ����λ��&gt;&gt; ɢƴ�ƻ�&gt;&gt; ɢ�����췢</td>
            </tr>
            <tr>
              <td height="2" bgcolor="#000000" colspan="2"></td>
            </tr>
          </tbody></table>
        </div>
        <div class="lineCategorybox">
        
        
            <uc1:selectXianlu ID="selectXianlu1" runat="server" />
        
        
        </div>
        <table width="99%" cellspacing="0" cellpadding="0" border="0" align="center">
          <tbody><tr>
            <td width="10" valign="top"><img src="../images/yuanleft.gif"></td>
            <td><div class="searchbox">
                <label>&nbsp;&nbsp;&nbsp;&nbsp;��·���ƣ�</label>
                <asp:TextBox ID="txt_xianlu" class="searchinput searchinput02" runat="server" ></asp:TextBox>
<%--                <label>&nbsp;&nbsp;&nbsp;&nbsp;��·����</label>
                <asp:TextBox ID="txt_xianluArea" class="searchinput searchinput02" runat="server" ></asp:TextBox>--%>
                <label>������</label> 
                <asp:TextBox ID="txt_days" class="searchinput searchinput03" runat="server"></asp:TextBox>               
                
                <label>
                    <asp:ImageButton ID="ImageButton1" style="vertical-align: top;" 
                    ImageUrl="../images/searchbtn.gif" runat="server" 
                    onclick="ImageButton1_Click" />
                </label>
              </div></td>
            <td width="10" valign="top"><img src="../images/yuanright.gif"></td>
          </tr>
        </tbody></table>
        <div class="btnbox">
          <table cellspacing="0" cellpadding="0" border="0" align="left">
            <tbody><tr>
              <td width="90" align="center"><a href="DaydayQuickAdd.aspx">�� ��</a></td>
              <td width="90" align="center"><a href="update.aspx" id="update">�� ��</a></td>
              
            </tr>
          </tbody></table>
        </div>
        <div class="tablelist">
          <table width="100%" cellspacing="1" cellpadding="0" border="0">
            <tbody><tr class="odd">
              <th width="3%" height="30" nowrap="nowrap" align="center">ѡ��</th>
              <th width="18%" nowrap="nowrap" align="center">��·���� </th>
              <th width="9%" nowrap="nowrap" align="center">����</th>
              <th width="8%" nowrap="nowrap" align="center">���м۸�</th>
              <th width="8%" nowrap="nowrap" align="center">ͬ�м۸�</th>
              <th width="5%" nowrap="nowrap" align="center">�Ѵ���</th>
              <th width="5%" nowrap="nowrap" align="center">δ����</th>
              <th width="7%" nowrap="nowrap" align="center">����</th>
            </tr>          
            <asp:Repeater runat=server ID="rpt_list">
            <ItemTemplate>            
                <tr class="<%=i%2==0?"even":"old" %>">
              <td height="30" align="center"><input type="hidden" value="<%#(int)Eval("ReleaseType")==0?0:1 %>" class="ReleaseType"/>
              <input type="checkbox" id="checkbox" value="<%#Eval("TourId") %>" name="checkbox"></td>
              <td align="center"><a target="_blank" href="<%#getUrl(Eval("tourid").ToString(),(int)Eval("ReleaseType"))+"?tourId="+Eval("tourId").ToString()+"&action=tourevery" %>"><%#Eval("RouteName")%></a></td>
              <td align="center"><%#Eval("TourDays").ToString()%></td>
              <td align="center">���˼�:<%#Eval("MSAdultPrice","{0:c2}") %><br />��ͯ��:<%#Eval("MSChildrePricen", "{0:c2}")%></td>
              <td align="center">���˼�:<%#Eval("THAdultPrice", "{0:c2}")%><br />��ͯ��:<%#Eval("THChildrenPrice", "{0:c2}")%></td>
              <td align="center"><a href="yichuli.aspx?tourid=<%#Eval("TourId") %>&status=5" class="yichuli"><%#Eval("HandleNumber")%></a></td>              
              <td align="center"><span name="weiquli"><a class="weichuli" href="weichuli.aspx?tourid=<%#Eval("TourId") %>&status=1" class="weichuli"><%#Eval("UntreatedNumber")%></a></span></td>              
              <td align="center">
                  <a href="DaydayPublish.aspx?id=<%#Eval("TourId") %>&act=delete" class="del">ɾ��</a></td>
            </tr>
            </ItemTemplate>
            </asp:Repeater>
            <tr>
              <td height="30" align="right" class="pageup" colspan="8">              
                  <cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
              </td>
            </tr>
          </tbody></table>
        </div>
      </div>
      <!-- InstanceEndEditable -->            </div>
    </form>
    <script type="text/javascript">
        var valInt = /^\d+,$/;
        var valorder = /order=\d/;
        function order(v) {
            var Url = "<%=Request.Url.ToString() %>";
            if (Url.indexOf("order=", 0) > 0) {
                Url = Url.replace(/order=\d/, "order=" + v);
            } else
                if (Url.indexOf("?", 0) > 0) {
                Url += "&order=" + v;
            } else {
                Url += "?order=" + v;
            }
            location.href=Url;
        }
        $(function() {
            $(".addDijie").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�ؽ���",
                    modal: true,
                    width: "960px",
                    height: "500px"
                });
                return false;
            });

            $(".yichuli").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�Ѵ���",
                    modal: true,
                    width: "900px",
                    height: "550px"
                });
                return false;
            });
            $(".weichuli").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "δ����",
                    modal: true,
                    width: "900px",
                    height: "550px"
                });
                return false;
            });

            //�б��ӡ��ť�¼�
            $(".btnDaYin").click(function() {
                var id = $(this).attr("ref");
                Boxy.iframeDialog({ title: "��ӡ", iframeUrl: "/print/printlist.aspx?tourId=" + id, width: "250px", height: "300px", draggable: true, data: null, hideFade: true, modal: true });
                return false;
            });
            $(".del").click(function() {
                return confirm('�Ƿ�ȷ��ɾ��?');
            });
            $("#copy").click(function() {
                var chkval = "";
                var ReleaseType = "";
                $("[name='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                        ReleaseType = $(this).prev("input").val();
                    }
                });
                if (chkval != "") {
                    if (chkval.split(",")[1] == "") {
                        if (ReleaseType == "0") {
                            location.href = "DaydayAdd.aspx?act=copy&id=" + chkval.split(",")[0];
                        } else {
                            location.href = "DaydayQuickAdd.aspx?act=copy&id=" + chkval.split(",")[0];
                        }
                    } else {
                        alert("ֻ�ܸ���һ����¼��");
                    }
                } else {
                    alert("��ѡ��һ����¼��");
                }
                return false;
            });
            $("#update").click(function() {
                var chkval = "";
                var ReleaseType = "";
                $("[name='checkbox']").each(function() {
                    if ($(this).attr("checked")) {
                        chkval += $(this).val() + ",";
                        ReleaseType = $(this).prev("input").val();
                    }
                });
                if (chkval != "") {
                    if (chkval.split(",")[1] == "") {
                        if (ReleaseType == "0") {
                            location.href = "DaydayAdd.aspx?act=update&id=" + chkval.split(",")[0];
                        } else {
                            location.href = "DaydayQuickAdd.aspx?act=update&id=" + chkval.split(",")[0];
                        }
                    } else {
                        alert("ֻ���޸�һ����¼��");
                    }
                } else {
                    alert("��ѡ��һ����¼��");
                }
                return false;
            });
            $("[name='txt_trueNum']").blur(function(e, d) {
                if (/^\d+$/.test($(this).val())) {
                    var jihua = $(this).parent().prev().find("[name='jihua']").text();
                    var liouwei = $(this).parent().next().text();
                    var weiquli = $(this).parent().next().next().find("[name='weiquli']").text();
                    var shishou = $(this).val();
                    var shengyu = $(this).parent().next().next().next().find("[name='shengyu']");
                    var sy = parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli);

                    if (parseInt(shishou) > parseInt(jihua)) {
                        alert("ʵ�ղ��ܴ��ڼƻ�!");
                        //                        $(this).select();
                        return false;
                    }
                    //$.newAjax("/sanping/default.aspx", {}, function(r) {
                    //shengyu.html(parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli) - parseInt(shishou));
                    // });

                    $.newAjax({ url: "/sanping/default.aspx?act=updatenum", data: "tourid=" + $(this).attr("tourId") + "&num=" + shishou, type: "POST", dataType: "html", success: function(r) {

                        if (r == "yes") {
                            //shengyu.html(parseInt(jihua) - parseInt(liouwei) - parseInt(weiquli) - parseInt(shishou));
                        } else {
                            alert("�޸�ʧ��!");
                            // $(this).get(0).reset();
                        }
                    }
                    });
                } else {
                    alert("����ȷ��д����");
                    $(this).focus();
                }

            });
            //����
            $(".baoming").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "����",
                    modal: true,
                    width: "920px",
                    height: "500px"
                });
                return false;
            });
            $(".crj").click(function() {
                var url = $(this).attr("href");
                Boxy.iframeDialog({
                    iframeUrl: url,
                    title: "�۸����",
                    modal: true,
                    width: "460px",
                    height: "150px"
                });
                return false;
            });
        });
    </script>
</asp:Content>
