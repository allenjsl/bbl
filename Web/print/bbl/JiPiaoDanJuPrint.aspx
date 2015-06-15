<%@ Page Title="我爱中华内部交款单" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="JiPiaoDanJuPrint.aspx.cs" Inherits="Web.print.bbl.JiPiaoDanJuPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="759" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <th height="30" align="center">
                <span style="font-size: 20px; font-weight: bold;"><%=SiteUserInfo.SysId == 3 ? "诚 信 假 期 ":" 我 爱 中 华 "%> 内 部 交 款 单</span>
            </th>
        </tr>
        <tr>
            <td height="35" align="right" class="PrintText">
                交款日期：年
                <input type="text" name="textfield" id="textfield" class="underlineTextBox" />
                月<input type="text" name="textfield" id="textfield" class="underlineTextBox" />
                日
                <input type="text" name="textfield" id="textfield" class="underlineTextBox" />
            </td>
        </tr>
        <tr>
            <td height="30" align="left" valign="top">
                <table width="100%" class="table_normal2">
                    <tr>
                        <th width="20" height="25" rowspan="2" align="center"  >
                            序号
                        </th>
                        <th width="95" rowspan="2" align="center"  >
                            交款单位
                        </th>
                        <th width="70" rowspan="2" align="center"  >
                            出团日期
                        </th>
                        <th width="141" rowspan="2" align="center"  >
                            旅游线路
                        </th>
                        <th width="40" rowspan="2" align="center"  >
                            人数
                        </th>
                        <th colspan="8" align="center"  >
                            交款金额（元）
                        </th>
                        <th colspan="3" align="center"  >
                            交款方式<br />
                            用√表示/注明银行
                        </th>
                        <th width="109" rowspan="2" align="center"  >
                            备注<br />
                            （部分款/全款）
                        </th>
                    </tr>
                    <tr>
                        <th width="24" align="center"  >
                            亿
                        </th>
                        <th width="24" align="center"  >
                            万
                        </th>
                        <th width="24" align="center"  >
                            千
                        </th>
                        <th width="24" align="center"  >
                            百
                        </th>
                        <th width="24" align="center"  >
                            十
                        </th>
                        <th width="24" align="center"  >
                            元
                        </th>
                        <th width="24" align="center"  >
                            角
                        </th>
                        <th width="24" align="center"  >
                            分
                        </th>
                        <th width="36" align="center"  >
                            现<br />
                            金
                        </th>
                        <th width="38" align="center"  >
                            支<br />
                            票
                        </th>
                        <th width="35" align="center"  >
                            电汇<br />
                            银行
                        </th>
                    </tr>
                    <tr>
                        <td height="25" align="center"  >
                            <input id="Text1" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text2" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text3" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text4" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text5" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text6" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text7" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text8" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text9" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text10" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text11" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text12" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text13" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text14" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text15" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text16" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text17" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  >
                            <input id="Text18" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text19" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text20" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text21" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text22" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text23" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text24" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text25" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text26" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text27" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text28" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text29" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text30" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text31" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text32" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text33" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text34" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  >
                            <input id="Text35" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text36" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text37" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text38" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text39" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text40" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text41" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text42" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text43" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text44" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text45" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text46" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text47" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text48" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text49" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text50" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text51" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  >
                            <input id="Text52" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text53" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text54" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text55" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text56" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text57" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text58" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text59" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text60" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text61" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text62" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text63" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text64" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text65" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text66" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text67" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text68" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  >
                            <input id="Text69" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text70" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text71" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text72" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text73" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text74" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text75" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text76" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text77" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text78" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text79" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text80" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text81" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text82" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text83" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text84" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  >
                            <input id="Text85" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td height="25" colspan="2" align="center"  >
                            合 计<input id="Text86" style="width: 90px" type="text" class="nonelineTextBox" />
                        </td>
                        <td colspan="15" align="left"  >
                            &nbsp;&nbsp;人民币（大写） 拾 万 仟 佰 拾 元 角 分
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="35" align="center" class="PrintText">
                会计主管：<input type="text" name="textfield2" id="textfield2" class="underlineTextBox" />
                记帐<input type="text" name="textfield3" id="textfield3" class="underlineTextBox" />
                出纳：<input type="text" name="textfield4" id="textfield4" class="underlineTextBox" />
                交款人：
                <input type="text" name="textfield5" id="textfield5" class="underlineTextBox" />
                收款人：<input type="text" name="textfield6" id="textfield6" class="underlineTextBox" />
            </td>
        </tr>
    </table>
</asp:Content>
