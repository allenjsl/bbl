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
    /// 选择客户
    /// xuty 2011/01/19
    /// </summary>
    public partial class SelCustomer : Eyousoft.Common.Page.BackPage
    {
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 20;
        protected int pageCount;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Bind();
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void Bind()
        {


            #region 设置公司ID产生省份城市列表
            ucProvince1.CompanyId = CurrentUserCompanyID;
            ucProvince1.IsFav = true;
            ucProvince1.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            ucCity1.CompanyId = CurrentUserCompanyID;
            ucCity1.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            ucCity1.IsFav = true;
            ucCity1.CityId = Utils.GetInt(Utils.GetQueryStringValue("cid"));

            txtCompanyName.Value = Server.HtmlDecode(Utils.GetQueryStringValue("comName"));
            txtContact.Value = Server.HtmlDecode(Utils.GetQueryStringValue("contactName"));
            txt_tel.Value = Utils.GetQueryStringValue("phone");
            #endregion
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

            EyouSoft.Model.CompanyStructure.MCustomerSeachInfo searchModel = new EyouSoft.Model.CompanyStructure.MCustomerSeachInfo();
            if (Utils.GetInt(Utils.GetQueryStringValue("pid")) > 0)
            {
                searchModel.ProvinceId = Utils.GetInt(Utils.GetQueryStringValue("pid"));
            }
            if (Utils.GetInt(Utils.GetQueryStringValue("cid")) > 0)
            {
                searchModel.CityId = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            }
            searchModel.ContactName = Server.HtmlDecode(Utils.GetQueryStringValue("contactName"));
            searchModel.CustomerName = Server.HtmlDecode(Utils.GetQueryStringValue("comName"));
            searchModel.ContactTelephone = Utils.GetQueryStringValue("phone");

            
            //int cityid = ucCity1.CityId;
            //int provinceId = ucProvince1.ProvinceId;
            //if (cityid > 0)
            //    searchModel.CityId = cityid;
            //if (provinceId > 0)
            //    searchModel.ProvinceId = provinceId;
            //searchModel.ContactName = contacter;
            //searchModel.CustomerName = companyName;
            //searchModel.ContactTelephone = txt_tel.Value;
            //绑定客户列表
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
            IList<EyouSoft.Model.CompanyStructure.CustomerInfo> list = custBll.GetCustomers(CurrentUserCompanyID, pageSize, pageIndex, ref recordCount, searchModel);
            if (list != null && list.Count > 0)
            {
                rptCustomer.DataSource = list;
                rptCustomer.DataBind();
                BindExportPage();
            }
            else
            {
                rptCustomer.EmptyText = "<tr><td colspan='10' align='center'>对不起，暂无客户资料信息！</td></tr>";
                ExportPageInfo1.Visible = false;
            }

        }
        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

    }

}
