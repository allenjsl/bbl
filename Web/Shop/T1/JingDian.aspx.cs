using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.Shop.T1
{
    /// <summary>
    /// 景点
    /// </summary>
    /// 汪奇志 2012-04-03
    public partial class JingDian : System.Web.UI.Page
    {
        #region attributes
        /// <summary>
        ///页记录数
        /// </summary>
        protected int pageSize = 10;
        /// <summary>
        /// 当前页索引
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总记录数
        /// </summary>
        protected int recordCount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            InitJingDian();
            InitProvince();
        }

        #region private members
        /// <summary>
        /// init jingdian
        /// </summary>
        void InitJingDian()
        {
            var searchInfo = new EyouSoft.Model.SupplierStructure.SupplierQuery();

            searchInfo.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("provinceid"));
            searchInfo.CityId = Utils.GetInt(Utils.GetQueryStringValue("cityid"));
            searchInfo.UnitName = Utils.GetQueryStringValue("name");

            var items = new EyouSoft.BLL.SupplierStructure.SupplierSpot().GetList(pageSize, pageIndex, ref recordCount, Master.CompanyId, searchInfo);

            if (items != null && items.Count > 0)
            {
                rpt.DataSource = items;
                rpt.DataBind();

                divPaging.Visible = true;
                divEmpty.Visible = false;

                paging.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
                paging.UrlParams.Add(Request.QueryString);
                paging.intPageSize = pageSize;
                paging.CurrencyPage = pageIndex;
                paging.intRecordCount = recordCount;
            }
            else
            {
                divPaging.Visible = false;
                divEmpty.Visible = true;
            }
        }

        /// <summary>
        /// init province
        /// </summary>
        protected void InitProvince()
        {
            txtProvince.Items.Clear();
            txtProvince.Items.Add(new ListItem("-请选择-", "0"));
            var items = new EyouSoft.BLL.CompanyStructure.Province().GetList(Master.CompanyId);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    txtProvince.Items.Add(new ListItem(item.ProvinceName, item.Id.ToString()));
                }
            }
        }
        #endregion
    }
}
