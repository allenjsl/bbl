<%@ Page Title="内部交款单" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="SaleShouJuPrint.aspx.cs" Inherits="Web.print.normal.SaleShouJuPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="750" align="center" >
        <tr>
            <th height="30" align="center" >
                <span style="font-size: 20px; font-weight: bold;">内 部 交 款 单</span>
            </th>
        </tr>
        <tr>
            <td height="35" align="right" class="normaltd">
                交款日期：年
                <input type="text" name="textfield" id="textfield" class="underlineTextBox" style="width:50px;"/>
                月<input type="text" name="textfield" id="textfield" class="underlineTextBox"  style="width:50px;"/>
                日
                <input type="text" name="textfield" id="textfield" class="underlineTextBox"  style="width:50px;"/>
            </td>
        </tr>
        <tr>
            <td height="30" align="left" valign="top" class="td_noneborder" style="padding:0px;margin:0px">
                <table width="100%" class="table_normal">
                    <tr>
                        <th width="20" height="25" rowspan="2" align="center"  class="normaltd">
                            序号
                        </th>
                        <th width="95" rowspan="2" align="center"  class="normaltd" >
                            交款单位
                        </th>
                        <th width="70" rowspan="2" align="center"  class="normaltd" >
                            出团日期
                        </th>
                        <th width="141" rowspan="2" align="center"  class="normaltd" >
                            旅游线路
                        </th>
                        <th width="40" rowspan="2" align="center"  class="normaltd" >
                            人数
                        </th>
                        <th colspan="8" align="center"  class="normaltd" >
                            交款金额（元）
                        </th>
                        <th colspan="3" align="center"  class="normaltd" >
                            交款方式<br />
                            用√表示/注明银行
                        </th>
                        <th width="109" rowspan="2" align="center"  class="normaltd" >
                            备注<br />
                            （部分款/全款）
                        </th>
                    </tr>
                    <tr>
                        <th width="24" align="center"  class="normaltd">
                            亿
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            万
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            千
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            百
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            十
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            元
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            角
                        </th>
                        <th width="24" align="center"  class="normaltd">
                            分
                        </th>
                        <th width="36" align="center"  class="normaltd">
                            现<br />
                            金
                        </th>
                        <th width="38" align="center"  class="normaltd">
                            支<br />
                            票
                        </th>
                        <th width="35" align="center"  class="normaltd">
                            电汇<br />
                            银行
                        </th>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd"  >
                            <input id="Text1" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text2" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text3" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text4" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text5" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text6" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text7" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text8" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text9" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text10" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text11" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text12" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text13" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text14" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text15" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text16" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text17" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            <input id="Text18" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text19" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text20" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text21" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text22" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text23" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text24" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text25" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text26" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text27" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text28" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text29" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text30" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text31" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text32" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text33" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text34" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" class="normaltd"  >
                            <input id="Text35" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text36" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text37" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text38" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text39" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text40" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text41" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text42" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text43" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text44" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text45" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text46" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text47" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text48" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text49" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text50" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text51" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            <input id="Text52" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text53" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text54" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text55" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text56" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text57" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text58" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text59" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text60" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text61" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text62" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text63" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text64" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text65" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text66" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text67" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text68" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center"  class="normaltd" >
                            <input id="Text69" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text70" style="width: 95px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text71" style="width: 70px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text72" style="width: 141px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text73" style="width: 40px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text74" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text75" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text76" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text77" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text78" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text79" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text80" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text81" style="width: 24px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text82" style="width: 36px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center"  class="normaltd" >
                            <input id="Text83" style="width: 38px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text84" style="width: 35px" type="text" class="nonelineTextBox" />
                        </td>
                        <td align="center" class="normaltd"  >
                            <input id="Text85" style="width: 109px" type="text" class="nonelineTextBox" />
                        </td>
                    </tr>
                    <tr>
                        <td height="25" colspan="2" align="center" class="normaltd"  >
                            合 计<input id="Text86" style="width: 90px" type="text" class="nonelineTextBox" />
                        </td>
                        <td colspan="15" align="left"  class="normaltd">
                            &nbsp;&nbsp;人民币（大写） 拾 万 仟 佰 拾 元 角 分
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="35" align="center" class="normaltd">
                会计主管：<input type="text" name="textfield2" id="textfield2" class="underlineTextBox" style="width:80px;" />
                记帐<input type="text" name="textfield3" id="textfield3" class="underlineTextBox" style="width:80px;" />
                出纳：<input type="text" name="textfield4" id="textfield4" class="underlineTextBox" style="width:80px;" />
                交款人：
                <input type="text" name="textfield5" id="textfield5" class="underlineTextBox" style="width:80px;" />
                收款人：<input type="text" name="textfield6" id="textfield6" class="underlineTextBox" style="width:80px;" />
            </td>
        </tr>
    </table>
</asp:Content>
