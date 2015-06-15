<%@ Page Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="DomManager.aspx.cs" Inherits="Web.UserCenter.DomManager.DomManager" Title="文档管理_个人中心" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
    <div class="mainbody">
			<!-- InstanceBeginEditable name="EditRegion3" -->
			<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">个人中心</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置>> 个人中心>> 文档管理</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div><div class="lineCategorybox" style="height:30px;"></div>
              
             
            <div class="btnbox">
            	<table border="0" align="left" cellpadding="0" cellspacing="0">
                  <tr>
                    <%if(grantadd){%><td width="90" align="center"><a href="javascript:;" class="add">新 增</a></td><%} %>
					<%if(grantmodify){%><td width="90" align="center"><a href="javascript:;" class="modify">修改</a></td><%} %>
					<%if(grantload){%><td width="90" align="center"><a href="javascript:;" class="show">查 看</a></td><%} %>
					<%if(grantload){%><td width="90" align="center"><a href="javascript:;" id="load">下载</a></td><%} %>
                  </tr>
              </table>
            </div>
           	  <div class="tablelist">
            	<table width="100%" border="0" cellpadding="0" cellspacing="1">
                  <tr>
                    <th width="10%" align="center" bgcolor="#BDDCF4" >全选
                    <input type="checkbox" name="checkbox" class="selall" /></th>
                    <th width="40%" align="center" bgcolor="#BDDCF4">文档名称</th>
                    <th width="18%" align="center" bgcolor="#bddcf4">上传时间</th>
                    <th width="14%" align="center" bgcolor="#bddcf4">上传人</th>
                    <th width="18%" align="center" bgcolor="#bddcf4">操作</th>
                  </tr>
                  <asp:Repeater ID="rptList" runat="server">
                    <ItemTemplate>
                       <tr tid="<%# Eval("DocumentId") %>" bgcolor="<%# Container.ItemIndex%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <td  align="center"><input type="checkbox" class="checkbox" /></td>
                        <td align="left" class="pandl3"><%# Eval("DocumentName")%></td>
                        <td align="center"><%# Convert.ToDateTime(Eval("CreateTime")).ToString("yyyy-MM-dd HH:mm:ss")%></td>
                        <td align="center"><%# Eval("OperatorName")%></td>
                        <td align="center"><%if (grantmodify){%><a href="javascript:;" class="modify" >修改 </a><%} if (grantdel)
                                             { %> <a href="javascript:;" class="del"> 删除</a><%} if (grantload)
                                             { %> <a href="<%# Eval("FilePath") %>" target="_blank" class="down">下载</a><%} %></td>
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
                    <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="4"  PageStyleType="NewButton" CurrencyPageCssClass="RedFnt" />
                    </td>
                  </tr>
                </table>
           	  </div>
			</div>
			<!-- InstanceEndEditable -->            
	    </div>
	    
	    <script type="text/javascript">
			$(function(){
	        //新增事件
			$("a.add").click(function(){
			    var url = "/UserCenter/DomManager/DomAdd.aspx";
			    Boxy.iframeDialog({
					iframeUrl:url,
					title:"新增文档",
					modal:true,
					width:"520px",
					height:"210px"
				});
				return false;
			});	
			//修改事件
			$("a.modify").click(function(){
			    var url = "/UserCenter/DomManager/DomUpdate.aspx?";
			    var that = $(this);
			    var tid=that.parent().parent().attr("tid");
			    if(!tid){
			        if(RowList.GetListSelectCount()>0){
			            tid = RowList.GetCheckedValueList()[0];
			        }
			    }
			    if(tid){
				    Boxy.iframeDialog({
					    iframeUrl:url + "tid=" + tid,
					    title:"修改文档",
					    modal:true,
					    width:"520px",
					    height:"210px"
				    });
				}else{
				    alert("请选择修改的列");
				}
				return false;
			});
			
			$("a.down").click(function(){
			    if(!$(this).attr("href")){
			        alert("没有相关文档");
			        return false;
			    }
			});
			
			//查看事件
			$("a.show").click(function(){
			    var url = "/UserCenter/DomManager/DomShow.aspx?";
			    var that = $(this);
			    if(RowList.GetCheckedValueList().length==1){
			       var tid = RowList.GetCheckedValueList()[0];
			       Boxy.iframeDialog({
					    iframeUrl:url + "tid=" + tid,
					    title:"查看文档",
					    modal:true,
					    width:"520px",
					    height:"210px"
				    });
			    }else{
				    alert("请选择查看的列");
				}
				return false;
			});
			
			//下载事件
			$("#load").click(function(){
			    var that = $(this);
			    if(RowList.GetCheckedValueList().length==1){
			        var t = RowList.GetCheckedValueList()[0];
			        var href = $("tr[tid='"+t+"']").find("a.down").attr("href");
			        if(!href){
			            alert("没有相关文档");
			            return false;
			        }else{
			            that.attr("href",href);
			        }
			        
			    }else{
				    alert("请选择下载的列");
				    return false;
				}
			});
			//删除事件
			$("a.del").click(function(){
			    if(window.confirm("确认删除该文档？")){
			        var that = $(this);
			        var tid=that.parent().parent().attr("tid");
			        $.newAjax({
			            url:"/UserCenter/DomManager/DomManager.aspx?type=domdel",
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
        };
    });
	</script>
</asp:Content>
