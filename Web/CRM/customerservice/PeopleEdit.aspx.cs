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
    /// 投诉、回访人信息修改
    /// xuty 2011/01/19
    /// </summary>
    public partial class PeopleEdit : Eyousoft.Common.Page.BackPage
    {
        protected bool HasPermit;//判断是否有权限
        protected void Page_Load(object sender, EventArgs e)
        { 
            string method = Utils.GetFormValue("hidMethod");//操作
            string mtype = Utils.GetQueryStringValue("mtype");//获取判断是否是回访内容，都则为投诉

            #region 判断权限
            if (mtype == "visit")
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_修改回访))
                {
                    HasPermit = true;
                }
            }
            else
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_修改投诉))
                {
                    HasPermit = true;
                }
            }
            #endregion

            int id=Utils.GetInt(Utils.GetQueryStringValue("id"));
            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
            EyouSoft.Model.CompanyStructure.CustomerCallBackInfo backModel = custBll.GetCustomerCallBackModel(id);
            if (method == "save")
            {
               if (backModel == null) backModel = new EyouSoft.Model.CompanyStructure.CustomerCallBackInfo();
               backModel.Time= ucPeople1.VisistDate;//投诉时间
               backModel.CallBacker=  ucPeople1.VisiterName;//接待人
               backModel.CustomerName= ucPeople1.ByVisisterCompany;//投诉客户
               backModel.CustomerId= Utils.GetInt(ucPeople1.ByVisisterCompanyId);//投诉客户ID
               backModel.CustomerUser = ucPeople1.ByVisisterName;
               backModel.Remark = Utils.InputText(txtRemarkP.Value);//备注
               string showMess = custBll.UpdateCustomerCallBack(backModel)?"数据保存成功！":"数据保存失败！";
                //如果不是客户来访则取投诉意见
                if (mtype == "visit")
                {
                    ucPeople1.IsVisist = true;
                }
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.document.location.reload();window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                if (mtype == "visit")
                {
                   ucPeople1.IsVisist= true;
                }
                if (backModel != null)
                {
                    ucPeople1.VisistDate = backModel.Time;//投诉时间
                    ucPeople1.VisiterName = backModel.CallBacker;//接待人
                    ucPeople1.ByVisisterCompany = backModel.CustomerName;//投诉客户
                    ucPeople1.ByVisisterCompanyId = backModel.CustomerId.ToString();//投诉客户ID
                    ucPeople1.ByVisisterName = backModel.CustomerUser;
                    txtRemarkP.Value = backModel.Remark;//备注
                }
                //如果不是来访客户则显示投诉意见
                
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
