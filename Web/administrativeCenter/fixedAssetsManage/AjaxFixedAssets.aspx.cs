using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.administrativeCenter.fixedAssetsManage
{
    public partial class AjaxFixedAssets : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 权限参数
        protected bool EditFlag = false;
        protected bool DeleteFlag = false;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int FixedAssetID = Utils.GetInt(Request.QueryString["FixedAssetID"], -1);
            string method = Utils.GetQueryStringValue("Method");
            PageIndex = Utils.GetInt(Request.QueryString["Page"], -1);

            if (!IsPostBack && FixedAssetID == -1)
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_固定资产管理_修改资产))
                {
                    EditFlag = true;
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_固定资产管理_删除资产))
                {
                    DeleteFlag = true;
                }
                string Number = Utils.InputText(Utils.GetQueryStringValue("Number"));
                string AssetName = Utils.InputText(Utils.GetQueryStringValue("AssetName"));
                DateTime? BuyTimeStart = Utils.GetDateTimeNullable(Request.QueryString["BuyTimeStart"]);
                DateTime? BuyTimeEnd = Utils.GetDateTimeNullable(Request.QueryString["BuyTimeEnd"]);
                EyouSoft.BLL.AdminCenterStructure.FixedAsset bllFixedAsset = new EyouSoft.BLL.AdminCenterStructure.FixedAsset();
                IList<EyouSoft.Model.AdminCenterStructure.FixedAsset> listFixedAsset = bllFixedAsset.GetList(PageSize, PageIndex, ref RecordCount, CurrentUserCompanyID, Number, AssetName, BuyTimeStart, BuyTimeEnd);

                if (listFixedAsset != null && listFixedAsset.Count > 0)
                {
                    this.crptFixedAssetsList.DataSource = listFixedAsset;//绑定数据
                    this.crptFixedAssetsList.DataBind();
                    BindPage();
                }
                else
                {
                    this.crptFixedAssetsList.EmptyText = "<tr><td colspan=\"11\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                    this.ExporPageInfoSelect1.Visible = false;
                }
            }
            if (method == "DeleteFixedAsset" && FixedAssetID != -1)//删除
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_固定资产管理_删除资产))
                {
                    Response.Clear();
                    Response.Write("NoPermission");
                    Response.End();
                }
                else
                {
                    EyouSoft.BLL.AdminCenterStructure.FixedAsset bllFixedAsset = new EyouSoft.BLL.AdminCenterStructure.FixedAsset();
                    if (bllFixedAsset.Delete(CurrentUserCompanyID, FixedAssetID))
                    {
                        Response.Clear();
                        Response.Write("True");
                        Response.End();
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("False");
                        Response.End();
                    }
                }
            }
        }


        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.HrefType = Adpost.Common.ExporPage.HrefTypeEnum.JsHref;
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "Default.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "Default.LoadData(this);", 0);
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
        }
    }
}
