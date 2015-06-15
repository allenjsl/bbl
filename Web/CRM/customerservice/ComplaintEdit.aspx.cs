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
    /// 添加客户投诉
    /// xuty 2011/1/19
    /// </summary>
    public partial class ComplaintEdit : Eyousoft.Common.Page.BackPage
    {  
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_新增投诉))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.客户关系管理_质量管理_新增投诉, true);
                return;
            }
            string method = Utils.GetFormValue("hidMethod");//操作
            if (method == "save")
            {
                #region 添加客户投诉
                string showMess = "数据保存成功";
                bool result = false;
                EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户bll
                //投诉实体
                EyouSoft.Model.CompanyStructure.CustomerCallBackInfo complaintModel = new EyouSoft.Model.CompanyStructure.CustomerCallBackInfo();
                //投诉结果
                List<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> resultList = new List<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo>();
                EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo infoModel = new EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo
                {
                    Car = (byte)ucContent1.LevelCar,
                    DepartureTime = ucContent1.LeaveDate,
                    Guide = (byte)ucContent1.LevelGuideService,
                    Hotel = (byte)ucContent1.LevelHotelCondition,
                    Journey = (byte)ucContent1.LevelTravel,
                    meals = (byte)ucContent1.LevelFood,
                    RouteID = Utils.GetInt(ucContent1.RouteId),
                    RouteName = ucContent1.RouteName,
                    Shopping = (byte)ucContent1.LevelShopping,
                    Spot = (byte)ucContent1.LevelLandScape, 
                    Remark=ucContent1.Remark
                };
                resultList.Add(infoModel);
                complaintModel.IsCallBack = EyouSoft.Model.EnumType.CompanyStructure.CallBackType.投诉;
                complaintModel.CustomerCallBackResultInfoList = resultList;//投诉结果
                complaintModel.CompanyId = CurrentUserCompanyID;//公司编号
                complaintModel.Time = ucPeople1.VisistDate;//投诉时间
                complaintModel.CallBacker = ucPeople1.VisiterName;//投诉人
                complaintModel.CustomerName = ucPeople1.ByVisisterCompany;//投诉客户
                complaintModel.CustomerId = Utils.GetInt(ucPeople1.ByVisisterCompanyId);//投诉客户ID
                complaintModel.CustomerUser = ucPeople1.ByVisisterName;//接待人
                complaintModel.Remark = Utils.InputText(txtRemarkP.Value);//备注
                result = custBll.AddCustomerCallBack(complaintModel);//执行添加
                if (!result)
                {
                    showMess = "数据保存失败！";
                }
                if (result)
                {
                    MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/CRM/customerservice/CustomerVisit.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                }
                else
                {
                    MessageBox.ShowAndRedirect(this, "数据保存失败！", this.Request.Url.ToString());
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
