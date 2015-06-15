<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetCartogramUnpaidList.aspx.cs"
    Inherits="Web.StatisticAnalysis.OutlayAccount.GetUnpaidList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未付</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="800" border="0" align="center" cellpadding="0" cellspacing="1">
            <tr class="odd">
                <th width="130" height="30" align="center">
                    线路名称
                </th>
                <th width="130" align="center">
                    团号
                </th>
                <th  width="130" align="center">
                    出团日期
                </th>
                <th  width="130" align="center">
                    供应商
                </th>
                <th width="130" align="center">
                    人数
                </th>
                <th width="130" align="center">
                    计调员
                </th>
                <th width="130" align="center">
                    总收入
                </th>
                <th width="130" align="center">
                    已收
                </th>
                <th width="130" align="center">
                    未付
                </th>
            </tr>
            <cc1:CustomRepeater ID="crp_CartogramUnpayList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            线路名称
                        </td>
                        <td align="center">
                            NB123456789
                        </td>
                        <td align="center">
                            2010-11-12
                        </td>
                        <td align="center">
                            供应商名称
                        </td>
                        <td align="center">
                            10
                        </td>
                        <td align="center">
                            张三
                        </td>
                        <td align="center">
                            <font class="fred">￥1000.00</font>
                        </td>
                        <td align="center">
                            <font class="fred">￥1000.00</font>
                        </td>
                        <td align="center">
                            <font class="fred">￥0.00</font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                 <tr class="odd">
                        <td height="30" align="center">
                            线路名称
                        </td>
                        <td align="center">
                            NB123456789
                        </td>
                        <td align="center">
                            2010-11-12
                        </td>
                        <td align="center">
                            供应商名称
                        </td>
                        <td align="center">
                            10
                        </td>
                        <td align="center">
                            张三
                        </td>
                        <td align="center">
                            <font class="fred">￥1000.00</font>
                        </td>
                        <td align="center">
                            <font class="fred">￥1000.00</font>
                        </td>
                        <td align="center">
                            <font class="fred">￥0.00</font>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
        </table>
         <table width="100%" border="0" id="tbl_ExportPage" runat="server" cellspacing="0"
                cellpadding="0">
                <tr>
                    <td align="right">
                        <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                    </td>
                </tr>
            </table>
    </div>
    </form>
</body>
</html>
