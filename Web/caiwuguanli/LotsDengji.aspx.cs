using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    public partial class LotsDengji : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_付款登记, false);
                }
                DdlPayTypeInit();//绑定支付方式
            }
        }
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.FinanceStructure.MZhiChuPiLiangDengJiInfo newmodel = new EyouSoft.Model.FinanceStructure.MZhiChuPiLiangDengJiInfo();
            newmodel.FKFangShi = (EyouSoft.Model.EnumType.TourStructure.RefundType)(Utils.GetInt(Utils.GetFormValue("ddlPayType")));
            newmodel.BeiZhu = t_desc.Value;
            newmodel.FKRenId = SiteUserInfo.ID;
            newmodel.FKRenName = txtStaffName.Text;
            newmodel.FKTime = Utils.GetDateTime(txtPayDate.Text);

            IList<string> ls = new List<string>();
            string[] anpaiIds = Utils.GetQueryStringValue("anpaiIds").Split(',');
            for (int i = 0; i < anpaiIds.Length; i++)
            {
                ls.Add(anpaiIds[i]);
            }
            newmodel.AnPaiIds = ls.ToArray();
            EyouSoft.BLL.FinanceStructure.BZhiChu newbll = new EyouSoft.BLL.FinanceStructure.BZhiChu();
            int result = newbll.PiLiangDengJi(CurrentUserCompanyID, this.SiteUserInfo.ID, newmodel);
            if (result == 1)
            {
                Utils.ShowMsgAndCloseBoxy("登记成功！", Utils.GetQueryStringValue("IframeId"), true);
            }
            else
            {
                Response.Write("<script>alert('登记失败!');</script>");
            }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="ddl"></param>
        protected void DdlPayTypeInit()
        {
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                ddlPayType.DataSource = list;
                ddlPayType.DataBind();
            }
        }
        /// <summary>
        /// 弹出窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
