using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.Hotels
{
    /// <summary>
    /// 新增供应商管理酒店信息
    /// 李晓欢
    /// 2011-3-8
    /// </summary>
    /// 修改:万俊
    /// 时间:2011-5-31
    /// 功能说明:添加多个图片上传功能,取消联系人的必输限制
    public partial class HotelAdd : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        //操作类型
        protected string type = string.Empty;
        //酒店编号
        protected int tid = 0;
        /// <summary>
        /// 记录图片列表的行数
        /// </summary>
        protected int photo_row = 1;
        //酒店业务逻辑类和实体类
        protected EyouSoft.Model.SupplierStructure.SupplierHotelInfo Hotelinfo = null;
        EyouSoft.BLL.SupplierStructure.SupplierHotel HotelBll = null;

        protected bool show = false;//是否查看

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //实例化酒店业务逻辑类和实体类
            Hotelinfo = new EyouSoft.Model.SupplierStructure.SupplierHotelInfo();
            HotelBll = new EyouSoft.BLL.SupplierStructure.SupplierHotel();

            //初始化省份城市
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;

            if (!IsPostBack)
            {
                BindHotelStart();
                //操作权限判断
                if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_栏目))
                {
                    type = Utils.GetQueryStringValue("type");
                    switch (type)
                    {
                        case "modify":
                            if (CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_修改))
                            {
                                Databind();
                            }
                            else
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_酒店_修改, false);
                            }
                            break;
                        case "show":
                            show = true;
                            Databind();
                            break;
                        default:
                            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_酒店_新增))
                            {
                                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_酒店_新增, false);
                            }
                            photo_row = 0;
                            break;
                    }
                }
            }
        }

        #region 绑定酒店星级
        protected void BindHotelStart()
        {
            //清空下拉框选项
            this.HotelStart.Items.Clear();
            this.HotelStart.Items.Add(new ListItem("--请选择酒店星级--", "-1"));
            List<EnumObj> HotelStart = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar));
            if (HotelStart.Count > 0 && HotelStart != null)
            {
                for (int i = 0; i < HotelStart.Count; i++)
                {
                    ListItem item = new ListItem();
                    switch (HotelStart[i].Value)
                    {
                        case "1": item.Text = "3星以下"; break;
                        case "2": item.Text = "挂3"; break;
                        case "3": item.Text = "准3"; break;
                        case "4": item.Text = "挂4"; break;
                        case "5": item.Text = "准4"; break;
                        case "6": item.Text = "挂5"; break;
                        case "7": item.Text = "准5"; break;
                        default: break;
                    }
                    item.Value = HotelStart[i].Value;
                    this.HotelStart.Items.Add(item);
                }
            }
        }
        #endregion

        #region 初始化酒店信息
        private void Databind()
        {
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                Hotelinfo = HotelBll.GetHotelInfo(tid);
                this.ucProvince1.ProvinceId = Hotelinfo.ProvinceId;
                this.ucCity1.CityId = Hotelinfo.CityId;
                this.ucCity1.ProvinceId = Hotelinfo.ProvinceId;
                if (this.HotelStart.Items.FindByValue(((int)Hotelinfo.Star).ToString()) != null)
                    this.HotelStart.Items.FindByValue(((int)Hotelinfo.Star).ToString()).Selected = true;
                if (Hotelinfo.SupplierPic != null && Hotelinfo.SupplierPic.Count > 0)
                {
                    BindPhoto(Hotelinfo.SupplierPic);
                }
                else
                {
                    photo_row = 0;
                }

            }
        }
        #endregion

        //弹窗设置
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //接受参数
            tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
            if (tid > 0)
            {
                Hotelinfo = HotelBll.GetHotelInfo(tid);
                Hotelinfo.Id = tid;
            }
            else
            {
                Hotelinfo.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店;
            }
            //省份编号
            Hotelinfo.ProvinceId = this.ucProvince1.ProvinceId;
            EyouSoft.Model.CompanyStructure.Province Provincemodel = new EyouSoft.BLL.CompanyStructure.Province().GetModel(this.ucProvince1.ProvinceId);
            if (Provincemodel != null)
            {
                Hotelinfo.ProvinceName = Provincemodel.ProvinceName;
            }
            //城市编号
            Hotelinfo.CityId = this.ucCity1.CityId;
            EyouSoft.Model.CompanyStructure.City citymodel = new EyouSoft.BLL.CompanyStructure.City().GetModel(this.ucCity1.CityId);
            if (citymodel != null)
            {
                Hotelinfo.CityName = citymodel.CityName;
            }

            //单位名称
            string unionname = Utils.GetFormValue("unionname");
            //酒店星级
            int HotelStart = Utils.GetInt(Utils.GetFormValue(this.HotelStart.UniqueID));
            //酒店地址
            string TxtHotelAddress = Utils.GetFormValue("TxtHotelAddress");
            //酒店简介
            string HotelIntroduction = Utils.GetFormValue("HotelIntroduction");

            #region 酒店图片
            if (Request.Files.Count > 0)
            {
                string msg = "";
                if (!EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "file_landspace", new[] { ".gif", ".jpg", ".jpeg", ".png" }, null, out msg))
                {
                    MessageBox.ResponseScript(this, ";alert('" + msg + "');");
                    BindHotelStart();
                    return;
                }
                Hotelinfo.SupplierPic = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
                EyouSoft.Model.SupplierStructure.SupplierPic SupplierPic = new EyouSoft.Model.SupplierStructure.SupplierPic();
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                //if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["HotelImage"], "SupplierControlFile", out filepath, out oldfilename))
                //{
                //    if (filepath.Trim() != "" && oldfilename.Trim() != "")
                //    {
                //        SupplierPic.PicPath = filepath;
                //        SupplierPic.PicName = oldfilename;
                //        Hotelinfo.SupplierPic.Add(SupplierPic);
                //    }
                //}
                //遍历files取出file.
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    bool result_landspace = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[i], "SightImg", out filepath, out oldfilename);
                    if (result_landspace)
                    {
                        Hotelinfo.SupplierPic = DisonseImg_Sight_Add(filepath, oldfilename, Hotelinfo.SupplierPic);

                    }

                }
            }
            //判断是新增还是修改,修改调用下马的方法
            if (tid > 0)
            {
                //取出页面上未修改的图片
                Hotelinfo.SupplierPic = GetImg_Photo(this.hidImgPhoto.Value, Hotelinfo.SupplierPic);
            }
            #endregion

            //导游词
            string TourGuids = Utils.GetFormValue("TourGuids");

            //当前公司编号
            Hotelinfo.CompanyId = this.SiteUserInfo.CompanyID;
            //当前操作人编号
            Hotelinfo.OperatorId = this.SiteUserInfo.ID;

            #region 联系人信息
            Hotelinfo.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            string[] UserName = Utils.GetFormValues("inname");
            string[] Userdate = Utils.GetFormValues("indate");
            string[] Userphone = Utils.GetFormValues("inphone");
            string[] Usermobile = Utils.GetFormValues("inmobile");
            string[] Userqq = Utils.GetFormValues("inqq");
            string[] Usermail = Utils.GetFormValues("inemail");
            string[] Userfax = Utils.GetFormValues("inefax");
            for (int i = 0; i < UserName.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.SupplierContact scmodel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                scmodel.CompanyId = SiteUserInfo.CompanyID;
                scmodel.ContactName = UserName[i];
                scmodel.JobTitle = Userdate[i];
                scmodel.ContactTel = Userphone[i];
                scmodel.ContactMobile = Usermobile[i];
                scmodel.QQ = Userqq[i];
                scmodel.Email = Usermail[i];
                scmodel.ContactFax = Userfax[i];
                scmodel.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店;
                Hotelinfo.SupplierContact.Add(scmodel);
            }
            #endregion

            #region 价格信息
            Hotelinfo.RoomTypes = new List<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo>();
            string[] Chamber = Utils.GetFormValues("Chamber");
            string[] SalaesPrices = Utils.GetFormValues("SalaesPrices");
            string[] SettlementPrice = Utils.GetFormValues("SettlementPrice");
            string[] radiobtnValue = Utils.GetFormValues("hd_rbtn");
            for (int j = 0; j < Chamber.Length; j++)
            {
                EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo roomtype = new EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo();
                roomtype.AccountingPrice = Utils.GetDecimal(SettlementPrice[j]);
                if (radiobtnValue[j] == "1") { roomtype.IsBreakfast = true; }
                else { roomtype.IsBreakfast = false; }
                roomtype.Name = Chamber[j];
                roomtype.SellingPrice = Utils.GetDecimal(SalaesPrices[j]);
                Hotelinfo.RoomTypes.Add(roomtype);
            }
            #endregion

            //备注
            string HotelRemarks = Utils.GetFormValue("HotelRemarks");

            Hotelinfo.UnitName = unionname;
            Hotelinfo.UnitAddress = TxtHotelAddress;
            Hotelinfo.Star = (EyouSoft.Model.EnumType.SupplierStructure.HotelStar)Enum.Parse(typeof(EyouSoft.Model.EnumType.SupplierStructure.HotelStar), HotelStart.ToString());
            Hotelinfo.Introduce = HotelIntroduction;
            Hotelinfo.TourGuide = TourGuids;
            Hotelinfo.Remark = HotelRemarks;
            Hotelinfo.IssueTime = System.DateTime.Now;

            int res = 0;
            if (tid > 0)
            {
                //修改酒店信息
                res = HotelBll.UpdateHotelInfo(Hotelinfo);
            }
            else
            {
                //添加酒店信息
                res = HotelBll.InsertHotelInfo(Hotelinfo);
            }

            if (res > 0)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide(); {2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), tid > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/SupplierControl/Hotels/HotelList.aspx';"));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }

        /// <summary>
        /// 绑定景点图片
        /// </summary>
        /// <param name="pics">数据源</param>
        protected void BindPhoto(IList<EyouSoft.Model.SupplierStructure.SupplierPic> pics)
        {
            this.repPhotos.DataSource = pics;
            this.repPhotos.DataBind();
        }
        /// <summary>
        /// 取得页面上的img(没改动的图片)
        /// </summary>
        /// <param name="values">所有没改动图片的值</param>
        /// <param name="pics">更新图片时的集合</param>
        /// <returns>图片集合</returns>
        protected IList<EyouSoft.Model.SupplierStructure.SupplierPic> GetImg_Photo(string values, IList<EyouSoft.Model.SupplierStructure.SupplierPic> pics)
        {
            //分割values
            string[] strs = values.Split('♀');
            //遍历strs
            foreach (string str in strs)
            {
                //再次分割取得路径和文件名
                EyouSoft.Model.SupplierStructure.SupplierPic pic = new EyouSoft.Model.SupplierStructure.SupplierPic();
                string[] picvalue = str.Split('♂');
                if (picvalue.Length == 2)
                {
                    pic.PicName = picvalue[1];
                    pic.PicPath = picvalue[0];
                    pics.Add(pic);
                }


            }
            return pics;
        }

        /// <summary>
        /// 处理多图片上传_添加新景点
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="pics">图片集合</param>
        /// <returns>图片集合</returns>
        protected IList<EyouSoft.Model.SupplierStructure.SupplierPic> DisonseImg_Sight_Add(string filePath, string fileName, IList<EyouSoft.Model.SupplierStructure.SupplierPic> pics)
        {
            EyouSoft.Model.SupplierStructure.SupplierPic pic = new EyouSoft.Model.SupplierStructure.SupplierPic();
            //如果页面上的上传控件不为空,则判断为有新的图片上传
            if (filePath != "")
            {
                pic.PicPath = filePath;
                pic.PicName = fileName;
                pic.SupplierId = SiteUserInfo.CompanyID;
                pics.Add(pic);
            }
            return pics;
        }

    }
}
