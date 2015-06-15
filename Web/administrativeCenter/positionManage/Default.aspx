<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.positionManage.Default" Title="职务管理_行政中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<%@ Import Namespace="EyouSoft.Common" %>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
     <form id="form1" runat="server">
		<div class="mainbody">
            <div class="lineprotitlebox">
               <table width="100%" border="0" cellspacing="0" cellpadding="0">

                  <tr>
                    <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                    <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>职务管理</td>
                  </tr>
                  <tr>
                    <td colspan="2" height="2" bgcolor="#000000"></td>
                  </tr>
              </table>  
            </div>
            <div class="lineCategorybox" style="height:30px;"></div>
            <div class="btnbox">
              <table border="0" align="left" cellpadding="0" cellspacing="0" id="table_Insert">
                <tr><td width="90" align="center"><a id="a_Insert" href="javascript:void(0);">新 增</a>
                </td></tr>
              </table>
            </div>
       	    <div class="tablelist" id="divPosition">
       	        <table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="10%" align="center" bgcolor="#BDDCF4">序号</th>
                    <th width="18%" align="center" bgcolor="#BDDCF4">职务名称</th>
                    <th width="20%" align="center" bgcolor="#bddcf4">职务说明</th>
                    <th width="20%" align="center" bgcolor="#bddcf4">职务具体要求</th>
                    <th width="20%" align="center" bgcolor="#bddcf4">备注</th>
                    <th width="12%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
              <cc2:CustomRepeater ID="rptData" runat="server">
                <ItemTemplate>
                    <tr>
                    <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
                    <td align="center" bgcolor="#e3f1fc" class="pandl3"><%# Eval("JobName") %></td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl3">
                        <%# Utils.GetText2(Eval("Help").ToString(),11,true)%></td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl3">
                        <%# Utils.GetText2(Eval("Requirement").ToString(),11,true)%></td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl3">
                        <%# Utils.GetText2(Eval("Remark").ToString(),11,true)%></td>
                    <td align="center" bgcolor="#e3f1fc">
                        <a href="javascript:void(0);" <%#!EditFlag?"style='display:none'":"" %> 
                        onclick="Default.lookPositionAoccount('<%# Eval("Id")%>');">修改</a>
                        <span <%#DeleteFlag&&EditFlag?"":"style='display:none'" %>>|</span>
                        <a <%#!DeleteFlag?"style='display:none'":"" %> href="javascript:void(0);" 
                        onclick="Default.isHasPersonnel('<%# Eval("Id")%>');" >删除</a></td>
                     </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                     <tr>
                    <td  align="center" bgcolor="#BDDCF4"><%=GetCount() %></td>
                    <td align="center" bgcolor="#BDDCF4" class="pandl3"><%# Eval("JobName") %></td>
                    <td align="left" bgcolor="#BDDCF4" class="pandl3">
                        <%# Utils.GetText2(Eval("Help").ToString(),11,true)%></td>
                    <td align="left" bgcolor="#BDDCF4" class="pandl3">
                        <%# Utils.GetText2(Eval("Requirement").ToString(),11,true)%></td>
                    <td align="left" bgcolor="#BDDCF4" class="pandl3">
                        <%# Utils.GetText2(Eval("Remark").ToString(),11,true)%></td>
                    <td align="center" bgcolor="#BDDCF4">
                        <a href="javascript:void(0);" <%#!EditFlag?"style='display:none'":"" %> 
                        onclick="Default.lookPositionAoccount('<%# Eval("Id")%>');">修改</a>
                        <span <%#DeleteFlag&&EditFlag?"":"style='display:none'" %>>|</span>
                        <a <%#!DeleteFlag?"style='display:none'":"" %> href="javascript:void(0);" 
                        onclick="Default.isHasPersonnel('<%# Eval("Id")%>');" >删除</a></td>
                     </tr>
                </AlternatingItemTemplate>
              </cc2:CustomRepeater>
              </table>
                
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td align="right" class="pageup">
                        <cc1:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4" runat="server"></cc1:ExportPageInfo>
                    </td>
                  </tr>
                </table>
       	  </div>
		</div>
	</form>
	<script type="text/javascript">
	    var Default = {
	        openForm: function(strurl, strtitle, strwidth, strheight, strdate) {
	            Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight, draggable: true, data: strdate });
	        },
	        lookPositionAoccount: function(ID) {
	            if (ID == 0) {
	                Default.openForm("/administrativeCenter/positionManage/Add.aspx", "职务管理添加", "520", "380", "");
	                return false;
	            } else {
	                Default.openForm("/administrativeCenter/positionManage/Add.aspx", "职务管理修改", "520", "380", "PositionID="+ID);
	                return false;
	            }
	        },
	        deletePositionInfo: function(ID) {
	            if (ID > 0) {
	                if (window.confirm("您确认要删除此职务信息吗?\n此操作不可恢复!")) {
	                    $.newAjax({
	                        url: "/administrativeCenter/positionManage/Default.aspx",
	                        data:{
	                            PositionID:ID,
	                            method:'deletePositionInfo'
	                        },
	                        dataType: "html",
	                        cache: false,
	                        type: "GET",
	                        success: function(result) {
	                            if (result == "1") {
	                                alert("职位信息删除成功!");
	                                window.location.href = "/administrativeCenter/positionManage/Default.aspx";
	                            } else {
	                                alert("职位信息删除失败!");
	                            }
	                        }
	                    });
	                }
	            } else {
	                alert("没有该职位信息，请确认！");
	            }
	        },
	        isHasPersonnel: function(ID) {
	            if (ID > 0) {
	                $.newAjax({
	                    url: "/administrativeCenter/positionManage/Default.aspx",
	                    data:{
	                        PositionID:ID,
	                        method:'isHasPersonnel'
	                    },
	                    dataType: "html",
	                    cache: false,
	                    type: "GET",
	                    success: function(result) {
	                        if (result == "1") {
	                            if (window.confirm("该职务有人担任，确认删除？")) {
	                                Default.deletePositionInfo(ID);
	                            }
	                        } else {
	                            Default.deletePositionInfo(ID);
	                        }
	                        return false;
	                    }
	                });
	            } else {
	                alert("没有该职位信息，请确认！");
	            }
	        }
	    };
	    $(function() {
	        if ("<%=InsertFlag %>" == "True") {
	            $("#table_Insert").show();
	            $("#a_Insert").click(function() {
	                Default.lookPositionAoccount(0);
	                return false;
	            });
	        } else {
	            $("#table_Insert").hide();
	        }
	    });
	</script>
</asp:Content>
