<%@ Page Language="C#" MasterPageFile="/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.administrativeCenter.trainingPlan.Default" Title="培训计划_行政中心" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
        <div class="lineprotitlebox">
           <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <tr>
                <td width="15%" nowrap="nowrap"><span class="lineprotitle">行政中心</span></td>
                <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：行政中心>>培训计划</td>
              </tr>
              <tr>
                <td colspan="2" height="2" bgcolor="#000000"></td>
              </tr>
          </table>  
        </div>
        <div class="lineCategorybox" style="height:30px;"></div>
        <div class="btnbox">
          <table id="table_Insert" border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center">
                <a id="a_Insert" href="javascript:void(0);">新增</a></td>
            </tr>
          </table>
        </div>
        <div class="tablelist" id="divSearch">
            <table width="100%" border="0" cellpadding="0" cellspacing="1">
              <tr>
                <th width="8%" align="center" bgcolor="#BDDCF4"><strong>序号</strong></th>
                <th width="40%" align="center" bgcolor="#bddcf4"><strong>计划标题</strong></th>
                <th width="15%" align="center" bgcolor="#bddcf4"><strong>发布时间</strong></th>
                <th width="13%" align="center" bgcolor="#bddcf4"><strong>发布人</strong></th>
                <th width="15%" align="center" bgcolor="#bddcf4"><strong>操作</strong></th>
              </tr> 
              <cc1:CustomRepeater ID="crptTrainingPlanList" runat="server">
                <ItemTemplate>
                    <tr>
                    <td  align="center" bgcolor="#e3f1fc"><%=GetCount() %></td>
                    <td align="left" bgcolor="#e3f1fc" class="pandl10">
                        <a href="javascript:void(0);" 
                        onclick="Default.OpenPage('see','<%# Eval("Id")%>')"><%# Eval("PlanTitle")%></a></td>
                    <td align="center" bgcolor="#e3f1fc"><%# string.Format("{0:yyyy-MM-dd}", Eval("IssueTime"))%></td>
                    <td align="center" bgcolor="#e3f1fc"><%# Eval("OperatorName")%></td>
                    <td align="center" bgcolor="#e3f1fc">
                        <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
                        onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
                        <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
                        <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
                        onclick="Default.DeleteTrainPlan('<%# Eval("Id")%>')">删除</a></td>
                  </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td  align="center" bgcolor="#bddcf4"><%=GetCount() %></td>
                        <td align="left" bgcolor="#bddcf4" class="pandl10">
                            <a href="javascript:void(0);" 
                            onclick="Default.OpenPage('see','<%# Eval("Id")%>')"><%# Eval("PlanTitle")%></a></td>
                        <td align="center" bgcolor="#bddcf4"><%# string.Format("{0:yyyy-MM-dd}", Eval("IssueTime"))%></td>
                        <td align="center" bgcolor="#bddcf4"><%# Eval("OperatorName")%></td>
                        <td align="center" bgcolor="#bddcf4">
                             <a href="javascript:void(0);"  <%# EditFlag?"":"style='display:none;'" %>
                            onclick="Default.OpenPage('update','<%# Eval("Id")%>')">修改</a>
                            <span <%# DeleteFlag&&EditFlag?"":"style='display:none;'" %>>|</span>
                            <a href="javascript:void(0);"  <%# DeleteFlag?"":"style='display:none;'" %>
                            onclick="Default.DeleteTrainPlan('<%# Eval("Id")%>')">删除</a></td>
                      </tr>
                </AlternatingItemTemplate>
              </cc1:CustomRepeater>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                <td align="right" class="pageup"><cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" /></td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        var Default = {
            OpenForm: function(strurl, strtitle, strwidth, strheight, strdate) {
                Boxy.iframeDialog({ title: strtitle, iframeUrl: strurl, width: strwidth, height: strheight,
                    draggable: true, data: strdate
                });
            },
            OpenPage: function(method, number) {
                if (method == "add") {
                    Default.OpenForm("/administrativeCenter/trainingPlan/EditTrainingPlan.aspx", "添加培训计划", "800px", "480px", "true");
                }
                if (method == "update") {
                    Default.OpenForm("/administrativeCenter/trainingPlan/EditTrainingPlan.aspx", "修改培训计划", "800px", "480px", "TrainPlanID=" + number);
                }
                if (method == "see") {
                    Default.OpenForm("/administrativeCenter/trainingPlan/SeeDetail.aspx", "查看培训计划", "740px", "240px", "TrainPlanID=" + number);
                }
            },
            DeleteTrainPlan: function(ID) {
                if (ID > 0) {
                    if (window.confirm("您确认要删除此培训计划信息吗?\n此操作不可恢复!")) {
                        $.newAjax({
                            type: "GET",
                            dateType: "html",
                            url: "/administrativeCenter/trainingPlan/Default.aspx",
                            data: {
                                TrainPlanID: ID,
                                Method: 'DeleteTrainPlan'
                            },
                            cache: false,
                            async: false,
                            success: function(result) {
                                if (result == "True") {
                                    alert("删除成功！");
                                    window.location.href = "/administrativeCenter/trainingPlan/Default.aspx";
                                } else if (result == "NoPermission") {
                                    alert("没有删除权限！");
                                } else {
                                    alert("删除失败！");
                                }
                            }
                        });
                    }
                } else {
                    alert("没有该记录。");
                }
            }
        };
        $(function() {
            if ("<%=InsertFlag %>" == "True") {
                $("#table_Insert").show();
                $("#a_Insert").click(function() {
                    Default.OpenPage('add', '');
                    return false;
                });
            } else {
                $("#table_Insert").hide();
            }
        });
    </script>
</asp:Content>
