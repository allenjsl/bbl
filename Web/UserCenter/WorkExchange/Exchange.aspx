<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="Exchange.aspx.cs" Inherits="Web.UserCenter.UserInfo.Exchange" Title="工作交流_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
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
                <div class="lineCategorybox" style="height:50px;">
                  <table border="0" cellpadding="0" cellspacing="0" class="grzxnav">
                    <tr>
                      <%= Web.Common.AwakeTab.createExchangeTab(SiteUserInfo,11) %>
                    </tr>
                  </table>
                </div>
				
              <div class="btnbox">
          <table border="0" align="left" cellpadding="0" cellspacing="0">
            <tr>
              <td width="90" align="center"><a href="/UserCenter/WorkExchange/ExchangeAdd.aspx?type=add">新 增</a></td>
              <td width="90" align="center"><a href="javascript:;" class="modify">修 改</a></td>
              <td width="90" align="center"><a href="javascript:;" class="del">删 除</a></td>
            </tr>
          </table>
        </div>
           	  <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr bgcolor="#BDDCF4">
                    <th width="9%" align="center" >全选
                      <input type="checkbox" name="checkbox" class="selall" /></th>
                    <th width="22%" align="center" >标题</th>
                    <th width="21%" align="center" >发布时间</th>
                    <th width="11%" align="center" >发布人</th>
                    <th width="12%" align="center" >浏览次数</th>
                    <th width="13%" align="center" >回复数</th>
                    <th width="12%" align="center" >操作</th>
                  </tr>
                  <asp:Repeater ID="rptList" runat="server">
                  <ItemTemplate>
                    <tr tid="<%# Eval("ExchangeId")%>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <td  align="center" ><input type="checkbox" name="checkbox2" class="checkbox" /></td>
                        <td align="left"  class="pandl3"><a href="javascript:;" class="show"><%# Eval("Title")%></a></td>
                        <td align="center"  class="pandl3"><%# Convert.ToDateTime(Eval("CreateTime"))%></td>
                        <td align="center" ><%# Eval("OperatorName")%></td>
                        <td align="center" ><%# Eval("Clicks")%></td>
                        <td align="center" ><%# Eval("Replys")%></td>
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
	    <script type="text/javascript">
	    $(function(){
	        //修改事件
			$("a.modify").click(function(){
			    var url = "/UserCenter/WorkExchange/ExchangeAdd.aspx?type=modify";
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(!tid){
			        if(RowList.GetListSelectCount()==1){
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
			
			//查看事件
			$("a.show").click(function(){
			    var url = "/UserCenter/WorkExchange/ExchangeShow.aspx?";
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(tid){
				    location.href = url + "tid=" + tid;
				}
				return false;
			});
			
			 //删除事件
			$("a.del").click(function(){
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(!tid){
			        if(RowList.GetListSelectCount()>0){
			            tid = RowList.GetCheckedValueList().join(',');
			        }
			    }
			    if(tid){
			        if(confirm("确认删除？")){
				        $.newAjax({
				            url:"/UserCenter/WorkExchange/Exchange.aspx?type=delchange",
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
