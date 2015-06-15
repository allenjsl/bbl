<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FaPiaoChaKan.aspx.cs" Inherits="Web.caiwuguanli.FaPiaoChaKan" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>财务管理-发票查看</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>
    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/js/jquery.boxy.js" type="text/javascript"></script>
    
    <style type="text/css">
    td{ text-align:center; height:30px;}
    
    td.money,th.money{text-align:right;}
    td.money span,th.money span{ padding-right:10px;}
    </style>  
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder runat="server" ID="phLB" Visible="true">
        <table width="100%" cellspacing="1" cellpadding="0" border="0" align="center">
            <tr class="odd">
                <th style="width: 15%; height: 30px;">
                    开票日期
                </th>
                <th class="money" style="width: 15%;">
                    <span>开票金额</span>
                </th>
                <th style="width: 15%;">
                    开票人
                </th>
                <th style="width: 15%;">
                    票号
                </th>
                <th style="width: 30%;">
                    备注
                </th>
                <th>操作</th>
            </tr>
            <asp:Repeater runat="server" ID="rptLB">
                <ItemTemplate>
                    <tr <%#(Container.ItemIndex + 1)%2==0?"class='odd'":"class='even'" %>>
                        <td>
                            <%#Eval("RiQi","{0:yyyy-MM-dd}") %>
                        </td>
                        <td class="money">
                           <span><%#Eval("JinE","{0:C2}") %></span>
                        </td>
                        <td>
                            <%#Eval("KaiPiaoRen")%>
                        </td>
                        <td>
                            <%#Eval("PiaoHao") %>
                        </td>
                        <td>
                            <%#Eval("BeiZhu") %>
                        </td>
                        <td><a href="javascript:void(0)" onclick="faPiao.xiuGai('<%#Eval("Id") %>')">修改</a> <a href="javascript:void(0)" onclick="faPiao.shanChu('<%#Eval("Id") %>')">删除</a></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr class="odd">
                <td>
                    
                </td>
                <td class="money">
                   <b><span>合计:<asp:Literal runat="server" ID="ltrKaiPiaoJinEHeJi"></asp:Literal></span></b>
                </td>
                <td>
                    
                </td>
                <td>
                    
                </td>
                <td>
                    
                </td>
                <td>
                
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="text-align:right">
                    <cc1:exportpageinfo id="paging" runat="server" linktype="5" />
                </td>
            </tr>
        </table>
        </asp:PlaceHolder>
        
        <asp:PlaceHolder runat="server" ID="phEmpty" Visible="true">
        <div style="line-height:30px; text-align:center; font-size:14px;">暂无开票登记信息！</div>
        </asp:PlaceHolder>
    </div>
    </form>
    
    <script type="text/javascript">
        var faPiao = {
            //发票修改
            xiuGai: function(editId) {
                Boxy.iframeDialog({
                    iframeUrl: "/caiwuguanli/fapiaodengji.aspx?editid=" + editId,
                    title: "修改开票登记",
                    modal: true,
                    width: "820px",
                    height: "220px"
                });

                return false;
            },
            //删除发票登记
            shanChu: function(delId) {
                if (!confirm("你确定要删除该发票登记信息吗？删除后数据不可恢复。")) return false;

                $.ajax({
                    type: "GET"
                    , cache: false
                    , url: "FaPiaoChaKan.aspx?dotype=delete&delid=" + delId
                    , dataType: "text"
                    , async: false
                    , success: function(ret) {
                        switch (ret) {
                            case "1":
                                alert("删除成功");
                                window.location.href = window.location.href;
                                break;
                            case "-1":
                                alert("删除失败：你没有删除权限");
                                break;
                            default:
                                alert("删除失败");
                                break;
                        }
                    }
                });
            }
        };
    </script> 
</body>
</html>
