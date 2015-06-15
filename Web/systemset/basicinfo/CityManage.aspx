<%@ Page Title="城市管理_基础设置_系统设置" Language="C#" MasterPageFile="~/masterpage/Back.Master" AutoEventWireup="true" CodeBehind="CityManage.aspx.cs" Inherits="Web.systemset.basicinfo.CityManage" %>
<%@ Register Src="~/UserControl/ucSystemSet/BasicInfoHeaderMenu.ascx" TagName="headerMenu" TagPrefix="uc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .even td{ background-color:#BDDCF4}
    .odd td{ background-color:#E3F1FC}
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="c1" runat="server">
<div class="mainbody">
     <uc1:headerMenu id="cu_HeaderMenu" runat="server" TabIndex="1" />
    <div class="btnbox-xt">
            	<table border="0" align="left" cellpadding="0" cellspacing="0" style="margin-left:8px;">
                  <tr>
                    <td width="91"><a href="javascript:;" onClick="return CityManage.openDialog('ProvinceEdit.aspx','添加省份');">添加省份</a></td>
                    <td width="91"><a href="javascript:;" onClick="return CityManage.openDialog('CityEdit.aspx','添加城市');">添加城市</a></td>
                  </tr>
                  <tr>
                    <td height="30" colspan="3" align="left"><font color="#FF0000"><strong>注：点击地区名称可进行修改</strong></font></td>
                  </tr>
              </table>
            </div>
    <table width="99%" border="0" align="left" cellpadding="0" cellspacing="1">
                  <tr>
                    <td width="20%" align="center" bgcolor="#BDDCF4"><strong>编号</strong></td>
                    <td width="20%" align="center" bgcolor="#BDDCF4"><strong>省份名称</strong></td>
                    <td width="20%" align="center" bgcolor="#BDDCF4"><strong>城市名称</strong></td>
                    <th width="20%" align="center" bgcolor="#BDDCF4">常用城市</th>
                    <td width="20%" align="center" bgcolor="#BDDCF4"><strong>操作</strong></td>
                  </tr>
                  <%=proAndCityHtml%>
              
                  
                  <tr>
                    <td height="30" colspan="5" align="center" valign="middle" bgcolor="#FFFFFF"  class="pageup">
                     <uc2:ExportPageInfo ID="ExportPageInfo1" CurrencyPageCssClass="RedFnt" LinkType="4"  runat="server"></uc2:ExportPageInfo>
                    </td>
                  </tr>
                </table>

</div>
<script type="text/javascript">
    var CityManage =
    {
        //打开弹窗
        openDialog: function(p_url, p_title, p_height) {
            Boxy.iframeDialog({ title: p_title, iframeUrl: p_url, width: "550px", height: p_height });
        },
        updateCity: function(cId) {
            CityManage.openDialog("/systemset/basicinfo/CityEdit.aspx?cId=" + cId, "修改城市", "150px");
            return false;
        },
        updatePro: function(pId) {
            CityManage.openDialog("/systemset/basicinfo/ProvinceEdit.aspx?proId=" + pId, "修改省份", "120px");
            return false;
        },
        //删除城市
        delCity: function(cityId, tar) {
            if (!confirm("你是否确认要删除该城市？")) {
                return false;
            }
            CityManage.doAjax("delCity", cityId);
        },
        //删除省份
        delPro: function(proId, tar) {
            if (!confirm("删除省份将会删除该省份下的所有城市，是否确认删除？")) {
                return false;
            }
            CityManage.doAjax("delPro", proId);
        },
        //设置为常用城市
        setCity: function(cId, tar) {
    
            var cityObj = $(tar);
            var ischecked = cityObj.attr("checked"); 
            var mess = ischecked ? "你是否要设置该城市为常用城市？" : "你是否要取消该城市为常用城市？";
            if (!confirm(mess)) {
                cityObj.attr("checked", !ischecked);
                return false;
            }
            CityManage.doAjax(cityObj.attr("isFav"), cId, cityObj);
        },
        //统一Ajax调用
        doAjax: function(doType, cid, tarp) {
            $.newAjax(
              {
                  url: "/systemset/basicinfo/CityManage.aspx",
                  data: { method: doType, id: cid },
                  dataType: "json",
                  cache: false,
                  type: "get",
                  success: function(result) {

                      if (result.success == '1') {
                          if (doType == "True" || doType == "False") {
                              if (doType == "False") { tarp.attr("isFav", "True"); alert("已取消常用城市！"); }
                              else { tarp.attr("isFav", "False"); alert("已设置为常用城市！"); }
                          }
                          else {
                              if (doType == "delCity" || doType == "delPro") {
                                  alert("删除成功！");
                                  window.location = "CityManage.aspx";
                              }
                          }
                      }
                      else {
                          alert(result.message);
                      }


                  },
                  error: function() {
                      alert("操作失败!");
                  }
              })
        }
    }
</script>
</asp:Content>
