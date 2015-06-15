using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.sanping
{
    /// <summary>
    /// 添加分配
    /// by 田想兵 2010.12
    /// </summary>
    public partial class TianjiaFengPei : Eyousoft.Common.Page.BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string tourId = Request.QueryString["tourId"];

                //操作类型
                string type = Utils.GetQueryStringValue("type");
                //团队计划
                if (type == "team")
                {
                    //团队计划：团队结算权限判断
                    if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
                    {
                        if (!CheckGrant(TravelPermission.团队计划_团队计划_团队结算))
                        {
                            Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_团队结算, false);
                        }
                    }
                    else
                    {
                        Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_栏目, false);
                    }

                }
                else
                    //财务管理
                    if (type == "account")
                    {
                        //财务管理：权限判断
                        if (CheckGrant(TravelPermission.财务管理_团队核算_栏目))
                        {
                            if (!CheckGrant(TravelPermission.财务管理_团队核算_利润分配添加))
                            {
                                Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_利润分配添加, false);
                            }
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_栏目, false);
                        }
                    }
                    else
                    //散拼计划
                    //if (type == "san")
                    {
                        //团队计划：团队结算权限判断
                        if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                        {
                            if (!CheckGrant(TravelPermission.散拼计划_散拼计划_团队结算))
                            {
                                Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_团队结算, false);
                            }
                        }
                        else
                        {
                            Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                        }
                    }

                if (tourId != "")
                {
                    BindInfo(tourId);
                }
            }
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        /// <param name="id"></param>
        void BindInfo(string id)
        {
            EyouSoft.BLL.FinanceStructure.TourProfitSharer bll = new EyouSoft.BLL.FinanceStructure.TourProfitSharer(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.TourProfitShareInfo model = bll.GetModel(id);
            if (model != null)
            {
                txt_Income.Text = model.ShareItem;
                txt_IncomeMoney.Text = model.ShareCost.ToString();
                txt_Remarks.Value = model.Remark;
                UCSelectDepartment1.GetDepartId = model.DepartmentId.ToString();
                UCSelectDepartment1.GetDepartmentName = model.DepartmentName;
                selectOperator1.OperId = model.SaleId.ToString();
                selectOperator1.OperName = model.Saler;
            }
        }
        /// <summary>
        /// 事件提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_Submit_Click(object sender, EventArgs e)
        {
            EyouSoft.BLL.FinanceStructure.TourProfitSharer bll = new EyouSoft.BLL.FinanceStructure.TourProfitSharer(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.TourProfitShareInfo model = null;

            if (Utils.GetInt(Request.QueryString["id"]) > 0)
            {
                model = bll.GetModel(Utils.GetQueryStringValue("id"));
            }
            else
            {
                model = new EyouSoft.Model.FinanceStructure.TourProfitShareInfo();
            }

            model.CompanyId = CurrentUserCompanyID;
            model.CreateTime = DateTime.Now;
            model.DepartmentId = Utils.GetInt(UCSelectDepartment1.GetDepartId);
            model.DepartmentName = UCSelectDepartment1.GetDepartmentName;
            model.Remark = EyouSoft.Common.Utils.GetFormValue("txt_Remarks");
            model.OperatorId = SiteUserInfo.ID;
            model.SaleId = EyouSoft.Common.Utils.GetInt(selectOperator1.OperId);
            model.Saler = selectOperator1.OperName;
            model.ShareCost = EyouSoft.Common.Utils.GetDecimal(txt_IncomeMoney.Text);
            model.ShareItem = txt_Income.Text;
            model.TourId = Request.QueryString["tourId"];
            if (bll.AddTourShare(model) > 0)
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('添加成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('添加失败!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }
        }
        /// <summary>
        /// 弹窗
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }
    }
}
