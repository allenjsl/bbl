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
    /// 编辑报价标准
    /// xuty 2011/1/13
    /// </summary>
    public partial class PriceStandEdit : Eyousoft.Common.Page.BackPage
    {
        protected string priceStand;//报价标准
        protected bool IsSystem;//系统默认
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_报价标准栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_报价标准栏目, false);
                return;
            }
            priceStand = Utils.GetFormValue("txtPrcStand");//获取报价
            int pId = Utils.GetInt(Utils.GetQueryStringValue("priId"));//报价Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存成功！";//提示消息
            EyouSoft.Model.CompanyStructure.CompanyPriceStand priceModel = null;
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand priceStandBll = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();//初始化报价bll
            //无操作方式则为获取数据
            if (method == "")
            {
                #region 初始化加载数据
                if (pId != 0)
                {   
                    priceModel = priceStandBll.GetModel(pId);
                    if (priceModel != null)
                    {
                        priceStand = priceModel.PriceStandName;
                        IsSystem = priceModel.IsSystem;
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存
                if (priceStand == "")
                {
                    MessageBox.Show(this, "报价标准不为空！");
                    return;
                }
                bool result = false;
                priceModel = new EyouSoft.Model.CompanyStructure.CompanyPriceStand();
                priceModel.CompanyId = CurrentUserCompanyID;
                priceModel.OperatorId = 0;
                priceModel.IssueTime = DateTime.Now;
                priceModel.PriceStandName = priceStand;
                if (pId != 0)
                {
                    priceModel.Id = pId;
                    result = priceStandBll.Update(priceModel);//修改数据
                }
                else
                {
                    result = priceStandBll.Add(priceModel);//添加数据
                }
                if (!result)
                {
                    showMess = "数据保存失败";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "PriceStandEdit.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/systemset/basicinfo/PriceStand.aspx';window.parent.Boxy.getIframeDialog('{1}').hide();", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                #endregion
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
