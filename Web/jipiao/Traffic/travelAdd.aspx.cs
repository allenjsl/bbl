using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.jipiao
{
    /// <summary>
    /// 行程添加
    /// 李晓欢 2012-09-10
    /// </summary>
    public partial class travelAdd : Eyousoft.Common.Page.BackPage
    {
        //是否停靠
        protected string IsStop = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 设置公司ID初始化省份城市列表
            ucLProvince.CompanyId = CurrentUserCompanyID;
            ucLProvince.IsFav = true;
            ucLProvince.IsIndex = "1";
            ucLCity.CompanyId = CurrentUserCompanyID;
            ucLCity.IsFav = true;
            ucLCity.IsIndex = "1";
            ucRprovince.CompanyId = CurrentUserCompanyID;
            ucRprovince.IsFav = true;
            ucRprovince.IsIndex = "2";
            ucRcity.CompanyId = CurrentUserCompanyID;
            ucRcity.IsFav = true;
            ucRcity.IsIndex = "2";
            #endregion

            string action = Utils.GetQueryStringValue("action");
            if (!string.IsNullOrEmpty(action) && action == "save")
            {
                Response.Clear();
                Response.Write(PageSave());
                Response.End();
            }

            if (!IsPostBack)
            {
                BindFlightCompany();
                if (Utils.GetQueryStringValue("type") == "update")
                {
                    int trId = Utils.GetInt(Utils.GetQueryStringValue("travelId"));
                    if (trId > 0)
                    {
                        InitTravel(trId);
                    }
                }
            }
        }

        /// <summary>
        /// 绑定航空公司
        /// </summary>
        protected void BindFlightCompany()
        {
            List<EyouSoft.Common.EnumObj> flightCompanylist = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));
            if (flightCompanylist != null && flightCompanylist.Count > 0)
            {
                this.seleLineCompanyNamev.DataTextField = "text";
                this.seleLineCompanyNamev.DataValueField = "value";
                this.seleLineCompanyNamev.DataSource = flightCompanylist;
                this.seleLineCompanyNamev.DataBind();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void InitTravel(int trId)
        {
            EyouSoft.Model.PlanStructure.TravelInfo model = new EyouSoft.BLL.PlanStruture.PlanTrffic().GettravelModel(trId);
            if (model != null)
            {
                this.txtSerialNum.Value = model.SerialNum.ToString();
                if (this.seleTfrricType.Items.FindByValue(((int)model.TrafficType).ToString()) != null)
                {
                    this.seleTfrricType.Items.FindByValue(((int)model.TrafficType).ToString()).Selected = true;
                }
                //出发省份 城市
                ucLProvince.ProvinceId = model.LProvince;
                ucLCity.CityId = model.LCity;
                ucLCity.ProvinceId = model.LProvince;
                //抵达省份 城市
                ucRprovince.ProvinceId = model.RProvince;
                ucRcity.CityId = model.RCity;
                ucRcity.ProvinceId = model.RProvince;
                this.txtFlightNum.Value = model.FilghtNum;
                if (this.seleLineCompanyNamev.Items.FindByValue(((int)model.FlightCompany).ToString()) != null)
                {
                    this.seleLineCompanyNamev.Items.FindByValue(((int)model.FlightCompany).ToString()).Selected = true;
                }
                this.txtLTime.Value = model.LTime;
                this.txtRTime.Value = model.RTime;
                if (this.ddlSpace.Items.FindByValue(((int)model.Space).ToString()) != null)
                {
                    this.ddlSpace.Items.FindByValue(((int)model.Space).ToString()).Selected = true;
                }
                this.txtAirPlaneType.Value = model.AirPlaneType;
                this.txtIntervalDays.Value = model.IntervalDays.ToString();
                //是否停靠
                IsStop = model.IsStop == false ? "1" : "0";
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        protected string PageSave()
        {
            string ret = string.Empty;
            EyouSoft.Model.PlanStructure.TravelInfo travelModel = new EyouSoft.Model.PlanStructure.TravelInfo();
            travelModel.TrafficId = Utils.GetInt(Utils.GetQueryStringValue("tfID"));
            travelModel.AirPlaneType = Utils.GetFormValue(this.txtAirPlaneType.UniqueID);
            travelModel.CompanyId = this.SiteUserInfo.CompanyID;
            travelModel.FilghtNum = Utils.GetFormValue(this.txtFlightNum.UniqueID);
            travelModel.FlightCompany = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Utils.GetInt(Utils.GetFormValue(this.seleLineCompanyNamev.UniqueID));
            travelModel.InsueTime = DateTime.Now;
            travelModel.IntervalDays = Utils.GetInt(Utils.GetFormValue(this.txtIntervalDays.UniqueID));
            travelModel.IsStop = Utils.GetFormValue("IsStop") == "0" ? true : false;
            travelModel.LCity = ucLCity.CityId;
            travelModel.LProvince = ucLProvince.ProvinceId;
            travelModel.LTime = Utils.GetFormValue(this.txtLTime.UniqueID);
            travelModel.RTime = Utils.GetFormValue(this.txtRTime.UniqueID);
            travelModel.Operater = this.SiteUserInfo.UserName;
            travelModel.OperaterID = this.SiteUserInfo.ID;
            travelModel.RCity = ucRcity.CityId;
            travelModel.RProvince = ucRprovince.ProvinceId;
            travelModel.SerialNum = Utils.GetInt(Utils.GetFormValue(this.txtSerialNum.UniqueID));
            travelModel.TrafficType = (EyouSoft.Model.EnumType.PlanStructure.TrafficType)Utils.GetInt(Utils.GetFormValue(this.seleTfrricType.UniqueID));
            travelModel.Space = (EyouSoft.Model.EnumType.PlanStructure.Space)Utils.GetInt(Utils.GetFormValue(this.ddlSpace.UniqueID));
            string type = Utils.GetQueryStringValue("type");
            if (!string.IsNullOrEmpty(type))
            {
                bool retult = false;
                if (type == "add")
                {
                    retult = new EyouSoft.BLL.PlanStruture.PlanTrffic().AddPlanTravel(travelModel);
                    if (retult)
                    {
                        ret = "{\"ret\":\"1\",\"msg\":\"添加成功!\"}";
                    }
                    else
                    {
                        ret = "{\"ret\":\"0\",\"msg\":\"添加失败!\"}";
                    }
                }
                else
                {
                    travelModel.TravelId = Utils.GetInt(Utils.GetQueryStringValue("trID"));
                    retult = new EyouSoft.BLL.PlanStruture.PlanTrffic().UpdatePlanTravel(travelModel);
                    if (retult)
                    {
                        ret = "{\"ret\":\"1\",\"msg\":\"修改成功!\"}";
                    }
                    else
                    {
                        ret = "{\"ret\":\"0\",\"msg\":\"修改失败!\"}";
                    }
                }
            }
            return ret;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
    }
}
