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
    /// 编辑客户等级
    /// xuty 2011/1/24
    /// </summary>
    public partial class CustomerLevelEdit : Eyousoft.Common.Page.BackPage
    {
        protected string custLevel;//客户等级
        protected bool IsSystem;//是否系统默认
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_客户等级栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_客户等级栏目, false);
                return;
            }
            custLevel = Utils.GetFormValue("txtCustLevel");//获取客户等级
            int custId = Utils.GetInt(Utils.GetQueryStringValue("custId"));//客户等级Id
            string method = Utils.GetFormValue("hidMethod");//获取当前操作(保存/继续)
            string showMess = "数据保存完成";//提示消息
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand customStandBll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();//初始化custbll
            EyouSoft.Model.CompanyStructure.CustomStand customStandModel = null;
            //无操作方式则为获取数据
            if (method == "")
            {
                #region 初次加载数据
                if (custId != 0)
                {
                    customStandModel = customStandBll.GetModel(custId);
                    if (customStandModel != null)
                    {
                        custLevel = customStandModel.CustomStandName;
                        IsSystem = customStandModel.IsSystem;
                    }
                    return;
                }
                #endregion
            }
            else
            {
                #region 保存
                if (custLevel == "")
                {
                    MessageBox.Show(this, "客户等级不为空！");
                }
                bool result = false;
                customStandModel = new EyouSoft.Model.CompanyStructure.CustomStand();
                customStandModel.CustomStandName = custLevel;
                customStandModel.CompanyId = CurrentUserCompanyID;
                customStandModel.IssueTime = DateTime.Now;
                customStandModel.IsSystem = false;
                customStandModel.OperatorId = 0;
                customStandModel.LevType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                if (custId != 0)
                {
                    customStandModel.Id = custId;
                    result=customStandBll.Update(customStandModel);
                }
                else
                {
                    result = customStandBll.Add(customStandModel);
                }
                if (!result)
                {
                    showMess = "数据保存失败";
                }
                //继续添加则刷新页面,否则关闭当前窗口
                if (method == "continue")
                {
                    MessageBox.ShowAndRedirect(this, showMess, "CustomerLevelEdit.aspx");
                }
                else
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/systemset/basicinfo/CustomerLevel.aspx';window.parent.Boxy.getIframeDialog('{1}').hide();", showMess, Utils.GetQueryStringValue("iframeId")));
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
