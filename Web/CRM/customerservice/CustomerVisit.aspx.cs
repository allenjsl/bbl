using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.CRM.customerservice
{   
    /// <summary>
    /// 回访记录
    /// xuty 2011/01/18
    /// </summary>
    public partial class CustomerVisit : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected bool HasComplaint;//是否有投诉栏目权限
        protected bool HasVisist=true;//是否有客户回访栏目权限
        protected bool IsAdd;//是否有添加权限
        protected bool IsUpdate;//是否有修改权限
        protected bool IsDelete;//是否有删除权限
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 判断是否有权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_回访栏目))
            {
                HasVisist = false;
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_质量管理_回访栏目, true);
                return;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_投诉栏目))
            {
                HasComplaint = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_删除投诉))
            {
                IsDelete = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_新增投诉))
            {
                IsAdd = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_修改投诉))
            {
                IsUpdate = true;
            }
            #endregion

            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            //获取查询条件
            string method = Utils.GetFormValue("method");
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
           
           
            if (method == "del")
            {   //删除
                string ids = Utils.GetFormValue("ids");
                bool result= custBll.DeleteCustomerCallBackMore(ids.Split(',').ToArray()); 
                Utils.ResponseMeg(result, result?"删除成功！":"删除失败！");
                return;
            }
            int pageCount = 0;
            string visister = Request.QueryString["visister"];//回访人
            visister = !string.IsNullOrEmpty(visister) ? Utils.InputText(Server.UrlDecode(visister)) : "";
            string byVisister = Request.QueryString["byVisister"];//被访人
            byVisister = !string.IsNullOrEmpty(byVisister) ? Utils.InputText(Server.UrlDecode(byVisister)) : "";
            //绑定回访列表
            IList<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> list = custBll.SearchCustomerCallBackList(EyouSoft.Model.EnumType.CompanyStructure.CallBackType.回访,pageSize, pageIndex, CurrentUserCompanyID, byVisister, visister, ref recordCount, ref pageCount);
            if (list != null && list.Count > 0)
            {
                rptVisiter.DataSource = list;
                rptVisiter.DataBind();
                BindExportPage();
            }
            else
            {
                rptVisiter.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无回访信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            //恢复查询关键字
            txtVisiter.Value = visister;//回访人
            txtByVisiter.Value = byVisister;//被访人
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }

}
