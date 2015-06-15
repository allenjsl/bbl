using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.Restaurants
{
    public partial class RestaurantsAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        //操作类型 添加or 修改
        protected string type = string.Empty;
        //餐馆编号
        protected int tid = 0;
        protected bool show = false;//是否查看
        #endregion

        //餐馆业务逻辑类和实体类
        protected EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo Restaurantinfo = null;
        EyouSoft.BLL.SupplierStructure.SupplierRestaurant RestaurantBll = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化业务逻辑类和实体类
            RestaurantBll = new EyouSoft.BLL.SupplierStructure.SupplierRestaurant();
            Restaurantinfo = new EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo();

            //初始化城市和省份
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            if (!IsPostBack)
            {
                //判断权限
                if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_栏目))
                {
                    type = Utils.GetQueryStringValue("type");
                    switch (type)
                    {
                        case "modify":
                            if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_修改))
                            {
                                bind();
                            }
                            else
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_餐馆_修改, false);
                            }
                            break;
                        case "show":
                            show = true;
                            bind();

                            break;
                        default:
                            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_餐馆_新增))
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_餐馆_新增, false);
                            }
                            break;
                    }
                }
            }
        }

        #region 初始化城市省份和酒店星级数据
        protected void bind()
        {
            int tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                Restaurantinfo = RestaurantBll.GetRestaurantInfo(tid);
                this.ucProvince1.ProvinceId = Restaurantinfo.ProvinceId;
                this.ucCity1.CityId = Restaurantinfo.CityId;
                this.ucCity1.ProvinceId = Restaurantinfo.ProvinceId;
            }
        }
        #endregion     

        protected void LinkButton_Click(object sender, EventArgs e)
        {
            //餐馆编号
            int tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                Restaurantinfo = RestaurantBll.GetRestaurantInfo(tid);
                Restaurantinfo.Id = tid;
            }
            else
            {
                Restaurantinfo.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆;
            }

            //省份编号
            Restaurantinfo.ProvinceId = this.ucProvince1.ProvinceId;
            EyouSoft.Model.CompanyStructure.Province Provincemodel = new EyouSoft.BLL.CompanyStructure.Province().GetModel(this.ucProvince1.ProvinceId);
            if (Provincemodel != null)
                Restaurantinfo.ProvinceName = Provincemodel.ProvinceName;

            //城市编号
            Restaurantinfo.CityId = this.ucCity1.CityId;
            EyouSoft.Model.CompanyStructure.City citymodel = new EyouSoft.BLL.CompanyStructure.City().GetModel(this.ucCity1.CityId);
            if (citymodel != null)
                Restaurantinfo.CityName = citymodel.CityName;
            
            //单位名称
            Restaurantinfo.UnitName = Utils.GetFormValue("Txtunitsnname");            
            //菜系
            Restaurantinfo.Cuisine=Utils.GetFormValue("TxtCuisine");            
            //单位地址
            Restaurantinfo.UnitAddress=Utils.GetFormValue("TxtAddress");            
            //餐馆简介
            Restaurantinfo.Introduce=Utils.GetFormValue("ResProfile");            
            //导游词
            Restaurantinfo.TourGuide=Utils.GetFormValue("TourGuids");            
            //当前公司编号
            Restaurantinfo.CompanyId=this.SiteUserInfo.CompanyID;            
            //当前操作员编号
            Restaurantinfo.OperatorId=this.SiteUserInfo.ID;            
            //备注
            Restaurantinfo.Remark=Utils.GetFormValue("TxtRemarks");
            Restaurantinfo.SupplierContact=new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            Restaurantinfo.IsDelete = false;
            string[] accmanname = Utils.GetFormValues("inname");
            string[] accmandate = Utils.GetFormValues("indate");
            string[] accmanphone = Utils.GetFormValues("inphone");
            string[] accmanmobile = Utils.GetFormValues("inmobile");
            string[] accmanqq = Utils.GetFormValues("inqq");
            string[] accmanemail = Utils.GetFormValues("inemail");
            string[] accmanefax = Utils.GetFormValues("inefax");
            for (int i = 0; i < accmanname.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact scModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                scModel.ContactName= accmanname[i];
                scModel.JobTitle=accmandate[i];
                scModel.ContactTel=accmanphone[i];
                scModel.ContactMobile=accmanmobile[i];
                scModel.QQ = accmanqq[i];
                scModel.Email = accmanemail[i];
                scModel.ContactFax = accmanefax[i];
                scModel.CompanyId = SiteUserInfo.CompanyID;
                scModel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆;
                Restaurantinfo.SupplierContact.Add(scModel);                
            }

            int res = 0;
            if (tid > 0)
            {
                //修改餐馆信息
                res = RestaurantBll.UpdateRestaurantInfo(Restaurantinfo);
            }
            else
            {
                //添加保险餐馆信息
                res = RestaurantBll.InsertRestaurantInfo(Restaurantinfo);
            }

            if (res > 0)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide(); {2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/Restaurants/Restaurantslist.aspx';"));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
