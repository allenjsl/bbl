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
    /// 投诉、回访内容编辑
    /// xuty 2011/1/19
    /// </summary>
    public partial class ContentEdit : Eyousoft.Common.Page.BackPage
    {
        protected bool hasPermit;//是否有修改权限
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = Utils.GetFormValue("hidMethod");//操作
            string mtype = Utils.GetQueryStringValue("mtype");//获取判断是否是回访内容，都则为投诉
            int id = Utils.GetInt(Utils.GetQueryStringValue("id"));

            #region 判断权限
            if (mtype == "visit")
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_修改回访))
                {
                    hasPermit = true;//是否显示保存按钮
                }
            }
            else
            {
                if (CheckGrant(global::Common.Enum.TravelPermission.客户关系管理_质量管理_修改投诉))
                {
                    hasPermit = true;//是否显示保存按钮
                }
            }
            #endregion

            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户bll
            //投诉或回访结果
            IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> resultList=custBll.GetCustomerCallbackResultList(id);
            EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo infoModel=null;//投诉或回访实体
            if (resultList != null)
            {
                 infoModel = resultList.FirstOrDefault();
            }
            if (method == "save")
            {
                #region 保存投诉或回访
                if (infoModel == null) infoModel = new EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo();
                infoModel.Car = (byte)ucContent1.LevelCar;
                infoModel.DepartureTime = ucContent1.LeaveDate;
                infoModel.Guide = (byte)ucContent1.LevelGuideService;
                infoModel.Hotel = (byte)ucContent1.LevelHotelCondition;
                infoModel.Journey = (byte)ucContent1.LevelTravel;
                infoModel.meals = (byte)ucContent1.LevelFood;
                infoModel.RouteID = Utils.GetInt(ucContent1.RouteId);
                infoModel.RouteName = ucContent1.RouteName;
                infoModel.Shopping = (byte)ucContent1.LevelShopping;
                infoModel.Spot = (byte)ucContent1.LevelLandScape;
                infoModel.CustomerCareforId = id;
                infoModel.Remark = ucContent1.Remark;
             
               //如果不是客户来访则取投诉意见
                if (mtype == "visit")
                {
                    ucContent1.IsVisist = true;
                    //保存投诉
                }
                string showMess = custBll.UpdateCustomerCallbackResult(infoModel) ? "数据保存成功！" : "数据保存失败！";
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide()", showMess, Utils.GetQueryStringValue("iframeId")));
                #endregion
            }
            else
            {
                #region 初始化投诉或回访结果
                if (infoModel != null)
                {
                    ucContent1.LevelCar=infoModel.Car;//车辆安排
                    ucContent1.LevelFood = infoModel.meals;//餐饮
                    ucContent1.LevelGuideService = infoModel.Guide;//导游
                    ucContent1.LevelHotelCondition = infoModel.Hotel;//酒店
                    ucContent1.LevelLandScape = infoModel.Spot;//景点
                    ucContent1.LevelShopping = infoModel.Shopping;//购物安排
                    ucContent1.LevelTravel = infoModel.Journey;//行程
                    ucContent1.RouteId=infoModel.RouteID.ToString();//线路ID
                    ucContent1.RouteName=infoModel.RouteName;//线路名称
                    ucContent1.Remark = infoModel.Remark;//备注
                    ucContent1.LeaveDate = infoModel.DepartureTime;//出团时间
                    //如果不是来访客户则显示投诉意见
                }
                if (mtype == "visit")
                {
                    ucContent1.IsVisist = true;
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
