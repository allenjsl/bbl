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
    /// 营销活动编辑
    /// xuty 2011/1/18
    /// </summary>
    public partial class ActiveEdit : Eyousoft.Common.Page.BackPage
    {
        protected bool HasPermit;//是否有权限
        protected void Page_Load(object sender, EventArgs e)
        {  
            string method = Utils.GetFormValue("hidMethod");//当前操作
            int activeId = Utils.GetInt(Utils.GetQueryStringValue("aid"));//活动Id
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户bll
            EyouSoft.Model.CompanyStructure.CustomerMarketingInfo marketModel = null;//营销活动实体
            if (method == "save")
            {
                #region 保存
                marketModel = new EyouSoft.Model.CompanyStructure.CustomerMarketingInfo();
                marketModel.Theme=Utils.InputText(txtActive.Value);//活动主题
                marketModel.Content=Utils.InputText(txtActiveContant.Value);//活动内容
                marketModel.Time=Utils.GetDateTime(Utils.InputText(txtActiveDate.Value),DateTime.Now);//活动时间
                marketModel.Effect= Utils.InputText(txtActiveResult.Value);//活动效果
                marketModel.Sponsor= Utils.InputText(txtActiveUser.Value);//主办人
                marketModel.Participant= Utils.InputText(txtMeeter.Value);//参加单位
                marketModel.State=(byte)Utils.GetInt(Utils.InputText(selActiveState.Value));//活动状态
                marketModel.OperatorId = SiteUserInfo.ID;//添加人
                marketModel.CompanyId = CurrentUserCompanyID;//公司编号
                marketModel.IssueTime = DateTime.Now;//添加时间
                string showMess = "数据保存成功";
                bool result = false;
                if (activeId != 0)
                {                    
                    //修改
                    marketModel.Id = activeId;
                    result = custBll.UpdateCustomerMarket(marketModel);
                }
                else
                {
                    //添加
                    result = custBll.AddCustomerMarket(marketModel);
                }
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/CRM/customerservice/MarketActive.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                #endregion
            }
            else
            {
                #region 初始化营销活动
                if (activeId != 0)
                {
                    marketModel = custBll.GetCustomerMarketModel(activeId);
                    //判断权限
                    if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_修改活动))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_营销活动_修改活动, true);
                        return;
                    }
                    if (marketModel != null)
                    {
                        txtActive.Value = marketModel.Theme;//活动主题
                        txtActiveContant.Value = marketModel.Content;//活动内容
                        txtActiveDate.Value = marketModel.Time.ToString("yyyy-MM-dd");//活动时间
                        txtActiveResult.Value = marketModel.Effect;//活动效果
                        txtActiveUser.Value = marketModel.Sponsor;//主办人
                        txtMeeter.Value = marketModel.Participant;//参加单位
                        selActiveState.Value = marketModel.State.ToString();//活动状态
                    }
                }
                else
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_营销活动_新增活动))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_营销活动_新增活动, true);
                        return;
                    }
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
