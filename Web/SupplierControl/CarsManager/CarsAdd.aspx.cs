using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.Collections;



namespace Web.SupplierControl.CarsManager
{
    /// <summary>
    /// 功能:车队添加、更新
    /// 创建:万俊
    /// </summary>
    public partial class CarsAdd : BackPage
    {
        protected bool show = false;//是否查看
        protected int cid = 0;          //全局变量,车队ID
        protected string type;          //操作类型
        protected int linkman_row = 1;  //联系人的数量,初始为1
        protected int car_row = 1;      //车辆的数量,初始为1
        protected string img;
        protected IList<EyouSoft.Model.CompanyStructure.SupplierContact> linkmans;
        protected IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> cars;
        protected EyouSoft.Model.SupplierStructure.SupplierCarInfo car;
        //生成车队BLL
        EyouSoft.BLL.SupplierStructure.SupplierCarTeam carTeamBll = new EyouSoft.BLL.SupplierStructure.SupplierCarTeam();
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断登陆账号是否有该栏目的权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_车队_栏目, true);
                return;
            }
            cid = Utils.GetInt(Utils.GetQueryStringValue("cid"));
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;
            linkmans = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            cars = new List<EyouSoft.Model.SupplierStructure.SupplierCarInfo>();
            type = Utils.GetQueryStringValue("type");
            //依旧是根据操作类型的权限判断
            if (type == "add")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_新增))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_车队_新增, true);
                    return;
                }
            }
            else
                //更新操作
                if (type == "modify")
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_修改))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_车队_修改, true);
                        return;
                    }
                    //生成车队信息类
                    if (!IsPostBack)
                    {
                        InitPorAndCity(cid);
                    }
                    DataInit(cid);
                }
                //删除操作
                else if (type == "dels")
                {
                    if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_车队_删除))
                    {
                        Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_车队_删除, true);
                        return;
                    }
                    //取得页面上取得的id集合
                    string str = Utils.GetQueryStringValue("ids");
                    string[] strs = Utils.GetQueryStringValue("ids").Split('|');

                    int[] ints = new int[strs.Length - 1];
                    bool result = false;
                    for (int i = 0; i < strs.Length - 1; i++)
                    {
                        ints[i] = Utils.GetInt(strs[i]);
                    }

                    result = carTeamBll.DeleteCarTeamInfo(ints);
                    Response.Clear();
                    Response.Write(result);
                    Response.End();
                }
                else if (type == "show")
                {
                    show = true;
                    if (!IsPostBack)
                    {
                        InitPorAndCity(cid);
                    }
                    DataInit(cid);
                }

        }
        /// <summary>
        /// 初始化省市
        /// </summary>
        /// <param name="cid"></param>
        protected void InitPorAndCity(int cid)
        {
            EyouSoft.Model.SupplierStructure.SupplierCarTeam carTeam = new EyouSoft.Model.SupplierStructure.SupplierCarTeam();
            carTeam = carTeamBll.GetCarTeamInfo(cid);
            this.ucCity1.CityId = carTeam.CityId;
            this.ucProvince1.ProvinceId = carTeam.ProvinceId;
            this.ucCity1.ProvinceId = carTeam.ProvinceId;
        }

        /// <summary>
        /// 初始化页面的数据
        /// </summary>
        /// <param name="cid"></param>
        protected void DataInit(int cid)
        {
            EyouSoft.Model.SupplierStructure.SupplierCarTeam carTeam = carTeamBll.GetCarTeamInfo(cid);
            if (carTeam.Id != 0)
            {
                this.companyName.Value = carTeam.UnitName;
                this.companyAddress.Value = carTeam.UnitAddress;
                this.companyRemark.Value = carTeam.Remark;
                linkmans = carTeam.SupplierContact;
                BindLiknMan(carTeam.SupplierContact);
                BindCarInfo(carTeam.CarsInfo);
            }

        }

        #region 绑定联系人列表
        //更新操作的时候调用该事件
        protected void BindLiknMan(IList<EyouSoft.Model.CompanyStructure.SupplierContact> contacts)
        {
            if (contacts != null && contacts.Count > 0)
            {
                this.rep_linkman.DataSource = contacts;
                this.rep_linkman.DataBind();
                this.hideContactCount.Value = contacts.Count.ToString();
            }
            else
            {
                this.hideContactCount.Value = "1";
            }

        }
        #endregion

        #region 绑定车队列表
        protected void BindCarInfo(IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> carInfos)
        {
            if (carInfos != null && carInfos.Count > 0)
            {
                this.rep_car.DataSource = carInfos;
                this.rep_car.DataBind();
                this.hideCarCount.Value = carInfos.Count.ToString();
            }
            else
            {
                hideCarCount.Value = "0";

            }

        }
        #endregion

        //设定该页面为弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            #region 数据验证
            string msg = "请选择图片文件";
            if (!EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "car_img", new[] { ".jpg", ".png", ".gif", ".bmp", ".jpeg" }, null, out msg))
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, Utils.ShowMsg("请选择图片文件上传!"));
                return;
            }
            #endregion


            //创建车辆信息类

            //创建车队信息类
            EyouSoft.Model.SupplierStructure.SupplierCarTeam carTeam = new EyouSoft.Model.SupplierStructure.SupplierCarTeam();


            if (Utils.GetInt(Utils.GetQueryStringValue("cid")) != 0)
            {
                carTeam = carTeamBll.GetCarTeamInfo(cid);
            }


            //获取页面上所有联系人的信息
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> contacts = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            string[] linkman_names = Utils.GetFormValues("linkman_name");
            string[] linkman_bussiness = Utils.GetFormValues("linkman_bussiness");
            string[] linkman_phone = Utils.GetFormValues("linkman_phone");
            string[] linkman_tel = Utils.GetFormValues("linkman_tel");
            string[] linkman_qq = Utils.GetFormValues("linkman_qq");
            string[] linkman_email = Utils.GetFormValues("linkman_email");
            string[] linkman_fax = Utils.GetFormValues("linkman_fax");
            //判断页面上添加联系人的数量
            if (linkman_names.Length >= 1)
            {
                for (int i = 0; i < linkman_names.Length; i++)
                {
                    //创建供应商联系人类
                    EyouSoft.Model.CompanyStructure.SupplierContact contact = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    contact.QQ = linkman_qq[i];
                    contact.Email = linkman_email[i];
                    contact.ContactName = linkman_names[i];
                    contact.ContactMobile = linkman_phone[i];
                    contact.ContactTel = linkman_tel[i];
                    contact.JobTitle = linkman_bussiness[i];
                    contact.ContactFax = linkman_fax[i];
                    contacts.Add(contact);
                }
            }
            //获取页面上所有车辆的信息
            IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> carInfos = new List<EyouSoft.Model.SupplierStructure.SupplierCarInfo>();
            string[] car_types = Utils.GetFormValues("car_type");
            string[] car_nums = Utils.GetFormValues("car_num");
            string[] car_prices = Utils.GetFormValues("car_price");
            string[] car_drivers = Utils.GetFormValues("car_driver");
            string[] car_driverPhones = Utils.GetFormValues("car_driverPhone");
            string[] car_worlds = Utils.GetFormValues("car_world");
            string[] hid_img = Utils.GetFormValues("hid_img");

            //判断页面上的添加车辆的数量
            if (car_types.Length >= 1)
            {
                for (int i = 0; i < car_types.Length; i++)
                {
                    EyouSoft.Model.SupplierStructure.SupplierCarInfo carInfo = new EyouSoft.Model.SupplierStructure.SupplierCarInfo();
                    string imagePath = "";
                    string imgName = "";
                    EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[i], "CarFile", out imagePath, out imgName);
                    carInfo.CarNumber = car_nums[i];
                    carInfo.CarType = car_types[i];
                    carInfo.DriverName = car_drivers[i];
                    carInfo.DriverPhone = car_driverPhones[i];
                    carInfo.GuideWord = car_worlds[i];
                    if (imagePath == "" && hid_img[i] != "")
                    {
                        carInfo.Image = hid_img[i];
                    }
                    else
                    {
                        carInfo.Image = imagePath;
                    }

                    carInfo.Price = Utils.GetDecimal(car_prices[i], 0);
                    carInfo.PrivaderId = SiteUserInfo.CompanyID;
                    carInfos.Add(carInfo);
                }
            }

            carTeam.CarsInfo = carInfos;
            carTeam.SupplierContact = contacts;
            carTeam.Remark = Utils.GetFormValue("companyRemark");
            carTeam.UnitAddress = Utils.GetFormValue("companyAddress");
            carTeam.UnitName = Utils.GetFormValue("companyName");
            carTeam.CityId = Utils.GetInt(this.city_id.Value);
            carTeam.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队;
            carTeam.CompanyId = SiteUserInfo.CompanyID;
            carTeam.ProvinceId = Utils.GetInt(this.pro_id.Value);
            //设置车队信息的省份名字
            EyouSoft.Model.CompanyStructure.Province Provincemodel = new EyouSoft.BLL.CompanyStructure.Province().GetModel(this.ucProvince1.ProvinceId);
            if (Provincemodel != null)
            {
                carTeam.ProvinceName = Provincemodel.ProvinceName;
            }
            //设置车队信息的城市名字
            EyouSoft.Model.CompanyStructure.City citymodel = new EyouSoft.BLL.CompanyStructure.City().GetModel(this.ucCity1.CityId);
            if (citymodel != null)
            {
                carTeam.CityName = citymodel.CityName;
            }
            int result = 0;
            if (Utils.GetInt(Utils.GetQueryStringValue("cid")) != 0)
            {
                result = carTeamBll.UpdateCarTeam(carTeam);
            }
            else
            {
                result = carTeamBll.InsertCarTeamInfo(carTeam);
            }

            if (result > 0)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/SupplierControl/CarsManager/CarsList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", "保存成功", Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }

        //判断是否有车辆图片
        protected string ImageHave(string image)
        {
            if (image.Trim() != "" && image != null)
            {
                img = "<a target='_blank' style='float:right' href='" + image + "' />查看</a>";
            }
            else
            {
                img = "";
            }
            return img;
        }
    }
}
