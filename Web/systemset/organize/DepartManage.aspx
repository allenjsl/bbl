<%@ Page Title="部门设置_组织机构_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="DepartManage.aspx.cs" Inherits="Web.systemset.organize.DepartManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
                <div class="lineprotitlebox">
                   <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="15%" nowrap="nowrap"><span class="lineprotitle">系统设置</span></td>
                        <td width="85%" nowrap="nowrap" align="right" style="padding:0 10px 2px 0; color:#13509f;">所在位置：系统设置>> 组织机构</td>
                      </tr>
                      <tr>
                        <td colspan="2" height="2" bgcolor="#000000"></td>
                      </tr>
                  </table>  
                </div>
                <div class="lineCategorybox" style="height:50px;">
                	<table border="0" cellpadding="0" cellspacing="0" class="xtnav">
                      <tr>
                      <% if (CheckGrant(Common.Enum.TravelPermission.系统设置_组织机构_部门设置栏目))
                         { %>
                        <td width="100" align="center" class="xtnav-on"><a href="/systemset/organize/DepartManage.aspx">部门名称</a></td>
                        <%} if (CheckGrant(Common.Enum.TravelPermission.系统设置_组织机构_部门人员栏目))
                         { %>
                        <td width="100" align="center"><a href="/systemset/organize/DepartEmployee.aspx">部门人员</a></td><%} %>
                      </tr>
                   </table>
               </div>
                 <a href="javascript:;" onclick="return DM.addD('top','');" style="display:<%=styleDiaplay %>">新 增</a>
                 
            	<table width="775" border="0" align="center" cellpadding="0" cellspacing="0"  style="border:1px solid #5D8FD1;">
                 <%=depStrHTML %>
                </table>

			</div>
			<script type="text/javascript">
			    //打开弹窗
			    var DM = {
			        openDialog: function(p_url, p_title, tar_a) {
			            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "640px", height: "380px", tar: tar_a });
			        },
			        delD: function(dId, tar_a, pId) {
			            var canDel = false;
			            if (!confirm("你确定要删除该部门吗？")) return false;
			            $.newAjax(
                      {
                          url: "/systemset/organize/DepartManage.aspx",
                          data: { departId: dId, method: "candel" },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          async: false,
                          success: function(result) {
                              if (result.success != "1") {
                                  canDel = false;
                                  alert(result.message);
                              }
                              else {
                                  canDel = true;
                              }

                          },
                          error: function() {
                              alert("操作失败!");
                          }
                      })
			            if (!canDel) {
			                return false;
			            }

			            $.newAjax(
                      {
                          url: "/systemset/organize/DepartManage.aspx",
                          data: { departId: dId, method: "del" },
                          dataType: "json",
                          cache: false,
                          type: "get",
                          success: function(result) {
                              if (result.success == "1") {

                                  $(tar_a).closest("tr").remove();
                                  var arrTr = [];
                                  DM.delRe(dId, arrTr);
                                  for (var i in arrTr) {
                                      arrTr[i].remove();
                                  }
                                  alert("删除成功！");

                              }
                          },
                          error: function() {
                              alert("操作失败!");
                          }
                      })
			            return false;
			        },
			        //修改部门
			        updateD: function(dId, tar_a) {
			            DM.openDialog("/systemset/organize/DepartEdit.aspx?method2=update&departId=" + dId, "修改部门", tar_a);
			            return false;
			        },
			        //添加下级部门
			        addD: function(dId, tar_a) {
			            DM.openDialog("/systemset/organize/DepartEdit.aspx?method2=add&departId=" + dId, "添加部门");
			            return false;
			        },
			        //添加下级部门后回调
			        callbackAddD: function(pId) {
			            var arrTr = [];
			            DM.delRe(pId, arrTr);
			            for (var i in arrTr) {
			                arrTr[i].remove();
			            }
			            var tar = $("#strong" + pId).parent().get(0);
			            if ($(tar).prev("img").attr("src").indexOf("7") > 0) {

			                $(tar).click(function() {
			                    DM.getSonD(this, pId);
			                });
			                $(tar).prev("img").click(function() {
			                    DM.getSonD2(this);
			                });
			            }
			            DM.getSonD(tar, pId, true);

			        },
			        //修改部门后回调
			        callbackUpdateD: function(nowId, nowName, updateP) {
			            if (updateP == "True") {
			                $("#strong" + nowId).remove();
			            }
			            else {
			                $("#strong" + nowId).html(nowName);
			            }
			        }, //点击图片
			        getSonD2: function(tar) {
			            $(tar).next("a").click();
			            return false;
			        },
			        //获取子部门
			        getSonD: function(tar_a, dId, back) {
			            var tarObj = $(tar_a);
			            var tarImg = tarObj.prev("img");
			            if (tarImg.attr("src").indexOf("5") > 0 || back) {
			                tarImg.attr("src", "/images/organization_06.gif");
			            }
			            else if (tarImg.attr("src").indexOf("6") > 0) {
			                var arrTr = [];
			                DM.delRe(dId, arrTr);
			                for (var i in arrTr) {
			                    arrTr[i].remove();
			                }
			                tarImg.attr("src", "/images/organization_05.gif");
			                return false;
			            }
			            $.newAjax(
                       {
                           url: "/systemset/organize/DepartManage.aspx",
                           data: { departId: dId, step: $(tar_a).attr("step") },
                           dataType: "text",
                           cache: false,
                           type: "get",
                           success: function(result) {
                               $("#noDepart").remove();
                               $(tar_a).closest("tr").after(result);
                           },
                           error: function() {
                               alert("操作失败!");
                           }
                       })
			        },
			        delRe: function(id, arr1) {
			            var arr = $("tr[parentid='" + id + "']");
			            if (arr) {
			                arr.each(function() {
			                    arr1.push($(this));
			                    DM.delRe($(this).attr("sid"), arr1);

			                });

			            }
			        }
			    }
			   
			</script>
</asp:Content>
