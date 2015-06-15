using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.basicinfo
{   
    /// <summary>
    /// 客户等级
    /// xuty 2011/1/13
    /// </summary>
    public partial class CustomerLevel : Eyousoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int itemIndex2 = 1;
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 40;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_客户等级栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_客户等级栏目, true);
                return;
            }
            int custId = Utils.GetInt(Utils.GetQueryStringValue("custId"));//客户等级Id
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex2 = (pageIndex - 1) * pageSize + 1;
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand customStandBll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();//初始化custbll
            //报价Id不为空执行删除操作
            if (custId !=0)
            {
                if (Utils.GetQueryStringValue("method") == "ispublish")//验证该区域是否有线路存在
                {
                    EyouSoft.BLL.CompanyStructure.CompanyPriceStand priceStandBll = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();//初始化报价bll
                    Utils.ResponseMeg(priceStandBll.IsUsed(custId, CurrentUserCompanyID, 0), "");
                    return;
                }
                bool result = customStandBll.Delete(custId);
                MessageBox.ShowAndRedirect(this, result ? "删除成功" : "删除失败", "/systemset/basicinfo/CustomerLevel.aspx");
                return;
            }
            //绑定客户等级列表
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = customStandBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                rptCustLevel.DataSource = list;
                rptCustLevel.DataBind();
                BindExportPage();
            }
            else
            {
                rptCustLevel.EmptyText = "<tr><td colspan='6' align='center'>对不起，暂无客户等级信息！</td></tr>";
                this.ExportPageInfo1.Visible = false;
              
            }
        }

        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml = "<tr class='oddTr'>";
            else if (itemIndex % 4 == 1)
                strHtml = "</tr><tr class='oddTr'>";
            else if (itemIndex % 2 == 1)
                strHtml = "</tr><tr class='evenTr'>";
            itemIndex++;
            return strHtml;
        }

        /// <summary>
        /// 获取绑定列表的最后一项
        /// </summary>
        /// <returns></returns>
        protected string GetLastTr()
        {
            if (recordCount == 0)
                return "";
            if (itemIndex % 2 == 0)
                return "<td colspan='3'></td></tr>";
            return "</tr>";
        }

        protected string GetExecute(bool IsSytsem, int Id)
        {
            if (IsSytsem)
            {
                return "<span class='issys'>默认客户等级不允许操作</span>";
            }
            else
            {
                return string.Format("<a href=\"javascript:;\" onclick=\"return CustLevel.update('{0}');\">修改 </a>|<a href=\"/systemset/basicinfo/CustomerLevel.aspx?custId={0}\" onclick=\"return CustLevel.delTip('{0}');\"> 删除</a>", Id);
            }
        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion

     

    }
   
}
