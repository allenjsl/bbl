using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.print.wh
{
    /// <summary>
    /// 单项服务确认单
    /// 李晓欢
    /// 2011-4-26
    /// </summary>
    /// 修改人：柴逸宁
    /// 时间：2011-5-30
    /// 修改备注：将TO、TEL、FAX改成供应商单位的信息
    public partial class SingleServerPrint : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourid = Utils.GetQueryStringValue("Tourid");
                string planId = Utils.GetQueryStringValue("planId");
                GetSingleServer(tourid, planId);
            }
        }

        protected void GetSingleServer(string Tourid, string planId)
        {
            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(this.SiteUserInfo);
            EyouSoft.Model.TourStructure.TourSingleInfo EditModel = null;
            EditModel = (EyouSoft.Model.TourStructure.TourSingleInfo)tourBll.GetTourInfo(Tourid);
            if (EditModel != null)
            {
                //计划人数
                this.PelpeoNumber.Value = EditModel.PlanPeopleNumber.ToString();
                //确认时间
                this.ConfirmDate.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                //供应商信息集合
                IList<EyouSoft.Model.TourStructure.PlanSingleInfo> planlist = EditModel.Plans;

                if (planId != "")
                {
                    EyouSoft.Model.TourStructure.PlanSingleInfo singleModel = new EyouSoft.BLL.TourStructure.Tour().GetSinglePlanInfo(planId);
                    if (singleModel != null)
                    {
                        //获取供应商信息--供应商名称
                        this.txtTo.Value = singleModel.SupplierName;
                        //获取供应商信息--供应商Id
                        int supplierid = singleModel.SupplierId;
                        //实例化供应商基础信息BLL
                        EyouSoft.BLL.CompanyStructure.SupplierBaseHandle csBLL = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
                        //实例化供应商基础信息Model
                        EyouSoft.Model.CompanyStructure.SupplierBasic model = new EyouSoft.Model.CompanyStructure.SupplierBasic();
                        //根据供应商基础信息ID获取供应商实体
                        model = csBLL.GetSupplierBase(supplierid);
                        //供应商基础信息不为空并且联系人不为空
                        if (model != null && model.SupplierContact != null)
                        {
                            //供应商 联系人电话
                            this.txtTel.Value = model.SupplierContact[0].ContactTel;
                            //供应商联系人Fax
                            this.txtFax.Value = model.SupplierContact[0].ContactFax;
                        }


                        this.lblServiceType.Text = "<strong>服务类别：</strong>" + singleModel.ServiceType.ToString();
                        this.lblSupplierName.Text = "<strong>供应商：</strong>" + singleModel.SupplierName;
                        this.lblArrange.Text = "<strong>具体安排：</strong>" + singleModel.Arrange;
                    }
                    else
                    {
                        this.pnlIsShow.Visible = false;
                        this.Repeaterlist.DataSource = planlist;
                        this.Repeaterlist.DataBind();
                    }
                }
                else
                {
                    //客户单位名称
                    this.txtTo.Value = EditModel.BuyerCName;
                    EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(EditModel.BuyerCId);
                    if (cusModel != null)
                    {
                        //组团 联系人电话
                        this.txtTel.Value = cusModel.Phone;
                        //组团联系人Fax
                        this.txtFax.Value = cusModel.Fax;
                    }
                    this.pnlIsShow.Visible = false;
                    this.Repeaterlist.DataSource = planlist;
                    this.Repeaterlist.DataBind();
                }






                #region 专线
                EyouSoft.Model.CompanyStructure.ContactPersonInfo userInfoModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserBasicInfo(EditModel.OperatorId);
                if (userInfoModel != null)
                {
                    //主要联系人电话
                    this.txtTelS.Value = userInfoModel.ContactTel;
                    //专线FAX
                    this.txtFaxS.Value = userInfoModel.ContactFax;

                    EyouSoft.Model.CompanyStructure.CompanyInfo companyModel = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(EditModel.CompanyId, SiteUserInfo.SysId);
                    if (companyModel != null)
                    {
                        this.txtFr.Value = companyModel.CompanyName;
                    }
                }
                #endregion


                //游客信息
                IList<EyouSoft.Model.TourStructure.TourOrderCustomer> customer = EditModel.Customers.Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                ;
                if (customer.Count > 0 && customer != null)
                {
                    this.repCustomer.DataSource = customer;
                    this.repCustomer.DataBind();
                }
            }
        }
    }
}
