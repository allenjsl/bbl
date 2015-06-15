using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理-发票登记
    /// </summary>
    public partial class FaPiaoDengJi : BackPage
    {
        string EditId = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            EditId = Utils.GetQueryStringValue("editid");

            InitPrivs();

            if (!IsPostBack && !string.IsNullOrEmpty(EditId))
            {
                InitEditInfo();
            }
        }

        #region private members
        /// <summary>
        /// init privs
        /// </summary>
        void InitPrivs()
        {
            if (!string.IsNullOrEmpty(EditId))
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_发票管理_修改))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_发票管理_修改, false);
                    return;
                } 
            }
            else
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_发票管理_新增))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_发票管理_新增, false);
                    return;
                }
            }
        }

        /// <summary>
        /// 初始化开票登记信息
        /// </summary>
        void InitEditInfo()
        {
            var info = new EyouSoft.BLL.FinanceStructure.BFaPiao().GetInfo(EditId);

            if (info != null)
            {
                txtKaiPiaoRiQi.Value = info.RiQi.ToString("yyyy-MM-dd");
                txtKaiPiaoJinE.Value = info.JinE.ToString("F2");
                txtKaiPiaoRen.Value = info.KaiPiaoRen;
                txtPiaoHao.Value = info.PiaoHao;
                txtBeiZhu.Value = info.BeiZhu;                
            }

            info = null;
        }
        #endregion

        #region protected members
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        /// <summary>
        /// lbtnSubmit_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            var info = new EyouSoft.Model.FinanceStructure.MFaPiaoInfo();

            info.BeiZhu = Utils.GetFormValue(txtBeiZhu.UniqueID);            
            info.JinE = Utils.GetDecimal(Utils.GetFormValue(txtKaiPiaoJinE.UniqueID));
            info.KaiPiaoRen = Utils.GetFormValue(txtKaiPiaoRen.UniqueID);
            info.KaiPiaoRenId = 0;
            info.PiaoHao = Utils.GetFormValue(txtPiaoHao.UniqueID);
            info.RiQi = Utils.GetDateTime(Utils.GetFormValue(txtKaiPiaoRiQi.UniqueID), DateTime.Now);

            if (!string.IsNullOrEmpty(EditId))
            {
                info.Id = EditId;
            }
            else
            {
                info.CaoZuoRenId = SiteUserInfo.ID;
                info.CompanyId = CurrentUserCompanyID;
                info.CrmId = Utils.GetInt(Utils.GetQueryStringValue("kehudanweiid"));
            }

            int bllRetCode = 0;
            EyouSoft.BLL.FinanceStructure.BFaPiao bll = new EyouSoft.BLL.FinanceStructure.BFaPiao();

            if (!string.IsNullOrEmpty(EditId))
            {
                bllRetCode = bll.Update(info);
            }
            else
            {
                bllRetCode = bll.Insert(info);
            }

            if (bllRetCode == 1)
            {
                RegisterScript("alert('操作成功');reloadParentWindow();");
            }
            else
            {
                RegisterAlertAndReloadScript("操作失败");
            }
        }
        #endregion
    }
}
