<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weichuli.aspx.cs" Inherits="Web.sanping.weichuli" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>成人价弹出框</title>
<link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
<script>
    $(function() {
        $(".Generation").click(function() {
            parent.Boxy.iframeDialog({
                iframeUrl: $(this).attr("href"),
                title: "生成计划",
                width: 900,
                height: 600,
                modal: true
            });
            return false;
        });
    });
</script>
</head>
<body>
<table width="900" border="0" align="center" cellpadding="0" cellspacing="1" style="margin-top:10px;">
  <tr class="odd">
    <th width="130" align="center">线路名称</th>
    <th width="90" align="center">出团日期</th>
    <th width="100" align="center">价格</th>
    <th width="60" align="center">人数</th>
    <th width="140" align="center">客户单位</th>
    <th width="80" align="center">联系人</th>
    <th width="100" align="center">电话</th>
    <th width="110" align="center">操作</th>
  </tr>  
  <cc2:CustomRepeater  runat="server" ID="rpt_list">
  <ItemTemplate>
  <tr class="<%#Container.ItemIndex%2==0?"even":"odd" %>">
    <td align="center"><%#Eval("RouteName")%></td>
    <td align="center"><%#Eval("LDate","{0:d}")%></td>
    <td align="center">成人：<%#Eval("AdultPrice","{0:c2}")%><br />
      儿童：<%#Eval("ChildrenPrice","{0:c2}")%></td>
    <td align="center"><%#Eval("PeopleNumber")%></td>
    <td align="center"><%#Eval("ApplyCompanyName")%></td>
    <td align="center"><%#Eval("ApplyContacterName")%></td>
    <td align="center"><%#Eval("ApplyContacterTelephone")%></td>
    <td align="center"><a class="look" target="_blank" href="/print/bbl/DayDayPublishVisitorsListPrint.aspx?ApplyId=<%#Eval("ApplyId") %>&tourId=<%#Eval("tourId") %>">查看名单</a><br /><a href="<%#(EyouSoft.Model.EnumType.TourStructure.ReleaseType)Eval("ReleaseType")== EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal?"GenerationPlan.aspx":"GenerationQickPlan.aspx"%>?ApplyId=<%#Eval("ApplyId") %>&tourId=<%#Eval("tourId") %>" class="Generation">生成计划</a></td>
  </tr>
  </ItemTemplate>
  </cc2:CustomRepeater>
  <tr><td align="right" colspan=8><cc1:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" pagestyletype="NewButton"
                                    CurrencyPageCssClass="RedFnt" /></td></tr>
</table>
</body>
</html>

