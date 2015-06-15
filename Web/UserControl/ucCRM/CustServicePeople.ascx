<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustServicePeople.ascx.cs" Inherits="Web.UserControl.ucCRM.CustServicePeople" %>
  <script type="text/javascript">
    
          function SetpIdAndName(cid,cname) {
            document.getElementById("<%=txtByVisitCompanyId.ClientID %>").value = cid;
            document.getElementById("<%=txtByVisitCompany.ClientID %>").value = cname;
          }
         
    
  </script>
  <tr class="odd">
    <th width="130" height="30" align="center"><span style="color:Red">*</span><%=IsVisist?"被访客户":"投诉客户" %>：</th>
    <td>
     <input  type="text" id="txtByVisitCompany"  runat="server" class="searchinput" valid="required"  readonly="readonly" errmsg="不为空"/>
      <input type="hidden" id="txtByVisitCompanyId" runat="server" />
     <a href="javascript:;" onclick="return selCust();"><img src="/images/sanping_04.gif" width="28" height="18" /></a>
    <span id="errMsg_<%=txtByVisitCompany.ClientID %>" class="errmsg" ></span>
    </td>
  </tr>
  <tr class="even">
    <th height="30" align="center"><%=isRequiredVisiterName?"<span style='color:Red'>*</span>":""%><%=IsVisist ? "被访人" : "投诉人"%>：</th>
    <td>
    <input type="text" id="txtByVisiter" valid="required" errmsg='不为空' runat="server" class="searchinput" />
    <span id="errMsg_<%=txtByVisiter.ClientID %>" class="errmsg" ></span>
    </td>
  </tr>
  <tr class="odd">
    <th height="30" align="center"><span style="color:Red">*</span><%=IsVisist ? "回访人" : "接待人"%>：</th>
    <td><input  type="text"  id="txtVisiter" runat="server" class="searchinput" valid="required"  errmsg="不为空"/>
   <span id="errMsg_<%=txtVisiter.ClientID %>" class="errmsg" ></span> 
    </td>
  </tr>
  <tr class="even">
    <th height="30" align="center"><span style="color:Red">*</span><%=IsVisist ? "回访时间" : "投诉时间"%>：</th>
    <td><input id="txtVisitDate" type="text"  runat="server" class="searchinput" onfocus="WdatePicker()" valid="required"  errmsg="不为空"/>
     <span id="errMsg_<%=txtVisitDate.ClientID %>" class="errmsg" ></span> </td>
  </tr>
  <script type="text/javascript">
      function selCust() {
          var iframeId = '<%=Request.QueryString["iframeId"] %>';
          parent.Boxy.iframeDialog({ title: "选择客户", iframeUrl: "/CRM/customerservice/SelCustomer.aspx",
              width: 800, height: 400, model: true, data: {
                  desid: iframeId,backFun:"SetpIdAndName"
               }
           });
      }
  </script>