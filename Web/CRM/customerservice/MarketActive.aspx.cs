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
    /// 营销活动
    /// xuty 2011/1/18
    /// </summary>
    public partial class MarketActive : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected bool IsAdd;//是否有增加权限
        protected bool IsUpdate;//是否有修改权限
        protected bool IsDelete;//是否有删除权限
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_营销活动_栏目, true);
                return;
            }
            if(CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_删除活动))
            {
                IsDelete = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_新增活动))
            {
                IsAdd = true;
            }
            if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_修改活动))
            {
                IsUpdate = true;
            }
            #endregion
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            //获取查询条件
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
            string method = Utils.GetFormValue("method");
            if (method == "del")
            {   //删除
                int[] ids = Utils.GetFormValue("ids").Split(',').Select(i=>Utils.GetInt(i)).ToArray();
                bool result = custBll.DeleteCustomerMarketingS(ids);
                Utils.ResponseMeg(result, result? "删除成功！":"删除失败！");
                return;
            }
            string active =Request.QueryString["active"];//活动主题
            active = !string.IsNullOrEmpty(active) ? Utils.InputText(Server.UrlDecode(active)) : "";
            string activeDate = Utils.GetQueryStringValue("activeDate");//活动时间
            int pageCount=0;
            //绑定营销活动
            IList<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo > list = custBll.SearchCustomerMarketList(pageSize,pageIndex,CurrentUserCompanyID,active,activeDate,ref recordCount,ref pageCount);
            if (list != null && list.Count > 0)
            {
                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                BindExportPage();
            }
            else
            {
                rptCustomer.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无营销活动信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }
            //恢复查询关键字
            txtActive.Value = active;//活动主题
            txtActiveDate.Value = activeDate;//活动时间
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected string GetStateStr(string state)
        {
            switch (state)
            {
                case "0":
                    return "无状态";
                  
                case "1":
                    return "准备阶段";
                    
                case "2":
                    return "活动进行中";
                case "3":
                    return "活动结束";
     
            }
            return "无状态";
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.UrlParams.Add(Request.QueryString);
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

   }
   
}
