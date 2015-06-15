using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.basicinfo
{
    /// <summary>
    /// 品牌管理
    /// xuty 2011/1/13
    /// </summary>
    public partial class BrandManage : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int itemIndex;//编号
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_品牌管理栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_品牌管理栏目, true);
                return;
            }
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);//获取页码
            itemIndex = (pageIndex - 1) * pageSize + 1;
            int brandId = Utils.GetInt(Utils.GetQueryStringValue("brandId"));
            EyouSoft.BLL.CompanyStructure.CompanyBrand brandBll = new EyouSoft.BLL.CompanyStructure.CompanyBrand();//初始化brandBll
            //删除品牌
            if (brandId != 0)
            {
                bool result = brandBll.Delete(brandId);
                MessageBox.ShowAndRedirect(this, result ? "删除成功" : "删除失败", "/systemset/basicinfo/BrandManage.aspx");
                return;
            }
            //绑定品牌列表
            IList<EyouSoft.Model.CompanyStructure.CompanyBrand> list = brandBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (list != null && list.Count > 0)
            {
                rptBrand.DataSource = list;
                rptBrand.DataBind();
                BindExportPage();
            }
            else
            {
                rptBrand.EmptyText = "<tr><td colspan='6' align='center'>对不起，暂无品牌信息！</td></tr>";
                this.ExportPageInfo1.Visible = false;
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
