<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaiwuSettledPrint.aspx.cs"
    Inherits="Web.print.hkmj.CaiwuSettledPrint" MasterPageFile="~/masterpage/Print.Master" Title="结算单" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="body" runat="server">
    <style  type="text/css">
    .input160{width:160px; height:18px; border-bottom:1px #000 solid; border-left:0; border-right:0; border-top:0; text-align:center;margin:0 4px;font-weight:bold;}
    .input60{width:60px; height:18px;border-bottom:1px #000 solid; border-left:0; border-right:0; border-top:0;text-align:center; margin:0 4px;font-weight:bold;}
    .input120{width:120px; height:18px; border:0; text-align:left;}
    .input180{width:180px; height:18px; border-bottom:1px #000 solid; border-left:0; border-right:0; border-top:0; text-align:center;margin:0 4px;font-weight:bold;}
     .input700{width:700px; height:18px; border-bottom:1px #000 solid; border-left:0; border-right:0; border-top:0; text-align:center;margin:0 4px;font-weight:bold;}
    </style>
    
    <table  width="760" border="0" align="center" cellpadding="0" cellspacing="0" class="table_normal">
        <tr>
            <td colspan="2">
                <table width="760" border="0" align="center" cellpadding="0" cellspacing="1"  >
                    <tr>
                        <th height="25" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            海口民间旅行社财务结算单 
                        </th>
                    </tr>
                    <tr>
                        <td width="20%" height="25" align="left" bgcolor="#FFFFFF" style="text-indent: 16px;" class="td_r_b_border">
                            <input type="text" class="input160" value="<%=buyCompanyName %>" />                            
                            旅行社<input  type="text" class="input60" value="<%=buyCompanyContentName %>" />经理：您好！
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="left" bgcolor="#FFFFFF" style="text-indent: 20px;" class="td_r_b_border">
                            感谢贵社的支持，现将贵社<asp:Literal ID="litRouteName" runat="server"></asp:Literal> 结算单传于贵处，如有疑问或不祥之处请与我社联系，亦可回传确认时在此单的下方注明；如无异议，请按照双方达成的付款方式付款：
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" >
                <table width="760" border="0" align="center" cellpadding="0" cellspacing="1" class="table_noneborder">
                    <tr>
                        <th width="15%" height="25" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            参团日期
                        </th>
                        <th width="11%" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            人数
                        </th>
                        <th width="32%" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            费用明细
                        </th>
                        <th width="11%" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            小计
                        </th>
                        <th width="11%" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            客人代表
                        </th>
                        <th width="20%" align="center" bgcolor="#FFFFFF" class="td_r_b_border">
                            备注
                        </th>
                    </tr>
                    <tr>
                        <td height="25" align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litLdate" runat="server"></asp:Literal>
                        </td>
                        <td align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litPeopleCount" runat="server"></asp:Literal>
                        </td>
                        <td align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litCostDetail" runat="server"></asp:Literal>
                        </td>
                        <td align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litCostSum" runat="server"></asp:Literal>
                        </td>
                        <td align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litVistors" runat="server"></asp:Literal>
                        </td>
                        <td align="center" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <asp:Literal ID="litremark" runat="server"></asp:Literal>
                        </td>
                    </tr>                  
                    <tr>
                        <td height="25" colspan="6" align="left" valign="middle" bgcolor="#FFFFFF" class="td_r_b_border">
                            <strong>备注：</strong><input  type="text"  value="" class="input700" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="left" class="td_r_b_border">
                付款方式：出团前时付
                <input type="text" class="input60" value="" />元；余款
                <input type="text" class="input60" value="" />元；
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="left" class="td_r_b_border">
                质量保证：质量异议按照国家有关法规条例处理。
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="left" class="td_r_b_border">
                请贵社接到此单后，立即加盖公章后回传确认，
            </td>
        </tr>
        <tr>
            <td height="30" colspan="2" align="left" class="td_r_b_border">
                以便我们开始操作，多谢合作！
            </td>
        </tr>
        <tr id="trAccountNameView" runat="server">
            <th height="30" colspan="2" align="left" class="td_r_b_border">
                户&nbsp;名：<asp:Literal ID="lithuming" runat="server"></asp:Literal>
            </th>
        </tr>
        <tr id="trBankView" runat="server">
            <th height="30" colspan="2" align="left" class="td_r_b_border">
                <asp:Literal ID="litkaihuhang" runat="server"></asp:Literal>：<asp:Literal ID="litzhanghao" runat="server"></asp:Literal>
            </th>
        </tr>
        <tr>
            <td width="50%" height="30" align="left" class="td_r_b_border">
                组团社名称：<asp:Literal ID="litbuyCompanyName" runat="server"></asp:Literal>
            </td>
            <td width="50%" align="left" class="td_r_b_border">
                <input type="text" value="海口民间旅行社有限公司" class="input180" />
            </td>
        </tr>
        <tr>
            <td height="30" align="left" class="td_r_b_border">
                经理签字：<input  type="text"  value="" class="input160" />
            </td>
            <td height="30" align="left" class="td_r_b_border">
                经理签字：<input  type="text"  id="litContectName" runat="server" class="input160" />
            </td>
        </tr>
        <tr>
            <td height="30" align="left" class="td_r_b_border">
                确认日期：<input  type="text" class="input160" />
            </td>
            <td height="30" align="left" class="td_r_b_border">
                确认日期：<input type="text" class="input160"  id="litQuerenDate" runat="server" />
            </td>
        </tr>
        </table>
</asp:Content>
