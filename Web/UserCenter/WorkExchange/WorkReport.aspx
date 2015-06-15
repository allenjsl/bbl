<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="WorkReport.aspx.cs" Inherits="Web.UserCenter.UserInfo.WorkReport" Title="工作汇报_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<form id="form1" runat="server">
 <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> 个人中心>> 工作交流</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:30px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,9) %>
                    </tr>
                  </table>
                </div>
				<table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td width="10" valign="top"><img src="/images/yuanleft.gif"/></td>
            <td><div class="searchbox">
                <label>标题：</label>
                <input style="width:120px;" name="textfield" type="text" class="searchinput searchinput02" id="title" value="<% =qwrModel.Title %>" />
                <label>汇报部门：</label>
                <asp:DropDownList runat="server" ID="ddldep">
                </asp:DropDownList>
                <label>汇报人：</label>
                <input type="text" name="textfield" id="reportpeople" class="searchinput searchinput03" value="<% =qwrModel.OperatorName %>"/>
                <label>汇报时间：</label>
                <input  style="width:80px;"  name="textfield" type="text" id="starttime" class="searchinput time" readonly="readonly" value="<% = qwrModel.CreateSDate==null?"":Convert.ToDateTime(qwrModel.CreateSDate).ToString("yyyy-MM-dd") %>" />
                至
                <input style="width:80px;"  type="text" name="textfield" id="endtime" class="searchinput time"  readonly="readonly" value="<% = qwrModel.CreateEDate==null?"":Convert.ToDateTime(qwrModel.CreateEDate).ToString("yyyy-MM-dd") %>"/>
                <label><img src="/images/searchbtn.gif" style="vertical-align:top;" id="searchbtn" /></label>
              </div></td>
            <td width="10" valign="top"><img src="/images/yuanright.gif"/></td>
          </tr>
        </table>
              <div class="btnbox">
          <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center"><a href="/UserCenter/WorkExchange/WorkReportAdd.aspx?type=add">新 增</a></td>
              <td width="90" align="center"><a href="javascript:;" class="modify">修 改</a></td>
              <td width="90" align="center"><a href="javascript:;" class="del">删 除</a></td>
            </tr>
          </table>
        </div>
           	  <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr bgcolor="#BDDCF4">
                    <th width="8%" align="center" >全选<input type="checkbox" name="checkbox"  class="selall" /></th>
                    <th width="27%" align="center" >标题</th>
                    <th width="15%" align="center" >汇报部门</th>
                    <th width="14%" align="center" >汇报人</th>
                    <th width="13%" align="center" >汇报时间</th>
                    <th width="9%" align="center" >状态</th>
                    <th width="14%" align="center" >操作</th>
                  </tr>
                  <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                        <tr tid="<%# Eval("ReportId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                            <td  align="center" ><input type="checkbox" class="checkbox" /></td>
                            <td align="left" class="pandl3"><a href="javascript:;" class="show"><%# Eval("Title") %></a></td>
                            <td align="center" ><%# Eval("DepartmentName")%></td>
                            <td align="center" ><%# Eval("OperatorName")%></td>
                            <td align="center" ><%# Convert.ToDateTime(Eval("ReportingTime")).ToString("yyyy-MM-dd") %> </td>
                            <td align="center" ><%# Convert.ToInt32(Eval("Status")) == 0 ? "<font color='#FF0000'>未审核</font>" : "已审核"%></td>
                            <td align="center" ><a href="javascript:;" class="modify">修改</a> | <a href="javascript:;" class="del">删除</a></td>
                        </tr>
                    </ItemTemplate>
                  </asp:Repeater>
                  <%if (len == 0)
                    { %>
                    <tr align="center"><td colspan="7">没有相关数据</td></tr>
                  <%} %>
               </table>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td height="30" align="right" class="pageup" colspan="13">
                    
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                        CurrencyPageCssClass="RedFnt" />
                </td>  
                  </tr>
                </table>
           	  </div>

			</div>
			<!-- InstanceEndEditable -->            
	    </div>
	    </form>
	    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
	    <script type="text/javascript">
	    
	    $(function(){
	        $("input.time").focus(function(){
                WdatePicker()
             });
	    
	        $("#searchbtn").click(function(){
	            var title = $("#title").val();
	            var people = $("#reportpeople").val();
	            var dep = $("#<%=ddldep.ClientID %>").val();
	            var stime = $("#starttime").val();
	            var etime = $("#endtime").val();
	            //参数
                var para = { title: title, people: people,dep:dep, stime:stime,etime:etime};

                window.location.href = "/UserCenter/WorkExchange/WorkReport.aspx?" + $.param(para);
                return false;
	        });
	        
	        
	        //修改事件
			$("a.modify").click(function(){
			    var url = "/UserCenter/WorkExchange/WorkReportAdd.aspx?type=modify";
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(!tid){
			        if(RowList.GetCheckedValueList().length==1){
			            tid = RowList.GetCheckedValueList()[0];
			        }
			    }
			    if(tid){
				    location.href = url + "&tid=" + tid;
				}else{
				    alert("请选择修改的列");
				}
				return false;
			});
			
			$("a.show").click(function(){
			    var url = "/UserCenter/WorkExchange/WorkReportShow.aspx?type=show";
			    var that = $(this);
			    var tid = that.parent().parent().attr("tid");
			    if(tid){
			        location.href = url + "&tid=" + tid;
			    }
			});
			
			
			//删除事件
			$("a.del").click(function(){
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(!tid){
			        if(RowList.GetCheckedValueList().length>0){
			            tid = RowList.GetCheckedValueList().join(',');
			        }
			    }
			    if(tid)
			    {
			        if(confirm("确认删除？")){
			            $.newAjax({
			                url:"/UserCenter/WorkExchange/WorkReport.aspx?type=reportdel",
			                type:"POST",
			                data:{"tid":tid},
			                dataType: 'json',
			                success: function(d) {
                                if(d.res){
                                    switch(d.res){
                                        case 1:
                                            alert("删除成功");
                                            location.reload();
                                            break;
                                        case -1:
                                            alert("删除失败");
                                            break;
                                    }
                                }
                            },
                            error: function() {
                                alert("服务器繁忙!");
                            }
			            });
			        }else{
			            return false;
			        }
			    }else{
				    alert("请选择删除的列");
				}
			    return false;
			});
	        
	        
	        ///全选事件
			$("input.selall").click(function(){
			    var that = $(this);
			    if(that.attr("checked")){
			        RowList.SetAllCheckValue("checked");
			    }else{
			        RowList.SetAllCheckValue("");
			    }
			});
	    
	        var RowList = {
                GetListSelectCount: function() {
                    var count = 0;
                    $(".tablelist").find("input.checkbox").each(function() {
                        if ($(this).attr("checked")) {
                            count++;
                        }
                    });
                    return count;
                },
                GetCheckedValueList: function() {
                    var arrayList = new Array();
                    $(".tablelist").find("input.checkbox").each(function() {
                        if ($(this).attr("checked")) {
                            arrayList.push($(this).parent().parent().attr("tid"));
                        }
                    });
                    return arrayList;
                },
                SetAllCheckValue: function(obj){
                    $(".tablelist").find("input.checkbox").each(function(){
                        $(this).attr("checked",obj);
                    });
                }
            }
	        
	    });
	    
	    
	    
	    
	    </script>
</asp:Content>
