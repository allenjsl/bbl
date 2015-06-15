using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.fixedAssetsManage
{
    public partial class EditFixedAssets : Eyousoft.Common.Page.BackPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }

        #region 页面参数
        protected string Number = string.Empty;
        protected string AssetName = string.Empty;
        protected DateTime BuyTime=new DateTime(1990,01,01);
        protected decimal? Cost = 0.0M;
        protected string Reamrk = string.Empty;
        #endregion  

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            int FixedID = Utils.GetInt(Request["FixedID"],-1);
            string Method = Utils.GetFormValue("hidMethod");

            if (!IsPostBack && FixedID != -1)
            {
                //初始化
                EyouSoft.BLL.AdminCenterStructure.FixedAsset bllFixedAsset = new EyouSoft.BLL.AdminCenterStructure.FixedAsset();
                EyouSoft.Model.AdminCenterStructure.FixedAsset modelFixedAsset = bllFixedAsset.GetModel(CurrentUserCompanyID, FixedID);
                if (modelFixedAsset != null)
                {
                    this.hidFixedID.Value=modelFixedAsset.Id.ToString();
                    Number=modelFixedAsset.AssetNo;
                    AssetName = modelFixedAsset.AssetName;
                    BuyTime = modelFixedAsset.BuyDate;
                    Cost = modelFixedAsset.Cost;
                    Reamrk = modelFixedAsset.Remark;
                }

            }
            if (Method == "save")
            {
                EyouSoft.BLL.AdminCenterStructure.FixedAsset bllFixedAsset = new EyouSoft.BLL.AdminCenterStructure.FixedAsset();
                EyouSoft.Model.AdminCenterStructure.FixedAsset modelFixedAsset = new EyouSoft.Model.AdminCenterStructure.FixedAsset();
                modelFixedAsset.AssetNo = Utils.GetFormValue("txt_Number");
                modelFixedAsset.AssetName = Utils.GetFormValue("txt_AssetName");
                modelFixedAsset.BuyDate = Utils.GetDateTime(Request.Form["txt_BuyTime"], new DateTime(1990, 01, 01));
                modelFixedAsset.Cost = Utils.GetDecimal(Request.Form["txt_Cost"]);
                modelFixedAsset.Remark = Utils.GetFormValue("txt_Reamrk", 250);
                modelFixedAsset.CompanyId = CurrentUserCompanyID;

                if (FixedID == -1)
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_固定资产管理_新增资产))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_固定资产管理_新增资产, true);
                    }
                    if (bllFixedAsset.Add(modelFixedAsset))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "保存成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/fixedAssetsManage/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "保存失败!");
                    }
                }
                if (FixedID != -1)
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_固定资产管理_修改资产))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_固定资产管理_删除资产, true);
                    }
                    modelFixedAsset.Id = Utils.GetInt(this.hidFixedID.Value);
                    
                    if (bllFixedAsset.Update(modelFixedAsset))
                    {
                        MessageBox.ResponseScript(this, string.Format("alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();window.parent.location='{2}';", "修改成功！", Utils.GetQueryStringValue("iframeId"), "/administrativeCenter/fixedAssetsManage/Default.aspx"));
                    }
                    else
                    {
                        MessageBox.Show(this.Page, "修改失败!");
                    }
                }
                
            }
        }
    }
}
