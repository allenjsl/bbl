using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.SupplierControl.SightManager
{
    /// <summary>
    /// 创建:万俊
    /// 功能:景点添加
    /// </summary>
    /// 修改:万俊
    /// 时间:2011-5-31
    /// 功能说明:添加多个图片上传功能,取消联系人的必输限制
    public partial class SightAdd : BackPage
    {
        protected bool show = false;//是否查看
        protected int linkman_row = 1;        //联系人的数量,初始为1
        protected int sid = 0;                //景点的ID
        protected string type;                //操作的类型{add,modify,dels}
        protected bool hz_img_state = false;    //用以判断有合作协议来控制层的显隐
        protected bool jd_img_state = false;    //用以判断景点是否又图片来控制层的显隐
        protected int photo_row = 1;            //景点图片的行数,初始为1
        EyouSoft.Model.SupplierStructure.SupplierSpot sight; //生成一个全局景点类
        EyouSoft.BLL.SupplierStructure.SupplierSpot sightBll = new EyouSoft.BLL.SupplierStructure.SupplierSpot();
        private string jd_path;                             //景点图片路径
        private string green_path;                          //合作协议文件路径
        protected void Page_Load(object sender, EventArgs e)
        {
            hz_img_state = false;
            jd_img_state = false;
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_景点_栏目, true);
                return;
            }
            sid = Utils.GetInt(Utils.GetQueryStringValue("sid"));
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            this.ucProvince1.IsFav = true;
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            this.ucCity1.IsFav = true;
            type = Utils.GetQueryStringValue("type");

            //判断操作类型-添加操作
            if (type == "add")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_新增))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_景点_新增, true);
                    return;
                }
                BindStatrs("");
            }
            //修改操作
            else if (type == "modify")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_修改))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_景点_修改, true);
                    return;
                }
                if (!IsPostBack)
                {
                    InitPorAndCity(sid);
                }
                InitPage(sid);
            }
            //删除操作
            else if (type == "dels")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_景点_删除))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_景点_删除, true);
                    return;
                }
                string str = Utils.GetQueryStringValue("ids");
                string[] strs = Utils.GetQueryStringValue("ids").Split('|');

                int[] ints = new int[strs.Length - 1];
                bool result = false;
                for (int i = 0; i < strs.Length - 1; i++)
                {
                    ints[i] = Utils.GetInt(strs[i]);
                }

                result = sightBll.Delete(ints);
                Response.Clear();
                Response.Write(result);
                Response.End();
            }
            else if (type == "show")
            {
                show = true;
                this.btnSave.Visible = false;
                if (!IsPostBack)
                {
                    InitPorAndCity(sid);
                }
                InitPage(sid);

            }
        }

        #region 初始化省份
        protected void InitPorAndCity(int sid)
        {
            EyouSoft.Model.SupplierStructure.SupplierSpot sight_update = new EyouSoft.Model.SupplierStructure.SupplierSpot();
            sight_update = sightBll.GetModel(sid);
            this.ucCity1.CityId = sight_update.CityId;
            this.ucProvince1.ProvinceId = sight_update.ProvinceId;
            this.ucCity1.ProvinceId = sight_update.ProvinceId;
        }

        #endregion
        #region 初始化页面
        protected void InitPage(int sid)
        {
            EyouSoft.Model.SupplierStructure.SupplierSpot sight_update = new EyouSoft.Model.SupplierStructure.SupplierSpot();
            sight_update = sightBll.GetModel(sid);
            if (sid != 0)
            {
                BindLiknMan(sight_update.SupplierContact);
            }
            //判断页面上的景点图片是否经过删除处理,删除过的值为null
            if (this.sight_hidden.Value != "null")
            {
                CheckImg(sight_update);
            }
            if (sight_update.SupplierPic.Count > 0)
            {
                jd_path = sight_update.SupplierPic[0].PicPath;
            }
            if (sight_update.AgreementFile != "")
            {
                green_path = sight_update.AgreementFile;
            }

            BindStatrs(((int)sight_update.Start).ToString());   //绑定星级列表
            this.txt_remark.Value = sight_update.Remark;
            this.txt_zc.Value = sight_update.UnitPolicy;
            this.companyAddress.Value = sight_update.UnitAddress;
            this.companyName.Value = sight_update.UnitName;
            this.single_price.Value = Utils.FilterEndOfTheZeroString(Utils.GetDecimal(Convert.ToString(sight_update.TravelerPrice)).ToString("0.00"));
            this.rl_price.Value = Utils.FilterEndOfTheZeroString(Utils.GetDecimal(Convert.ToString(sight_update.TeamPrice)).ToString("0.00"));
            this.guideworld.Value = sight_update.TourGuide;
            if (this.sight_hidden.Value != "null")
            {
                if (sight_update.SupplierPic.Count > 0)
                {
                    BindPhoto(sight_update.SupplierPic);
                    //this.jd_img_name.Text = sight_update.SupplierPic[0].PicName;
                }
                else
                {
                    photo_row = 0;
                }
            }
            if (this.hz_img_name.Text != "null")
            {
                this.green_file.Value = sight_update.AgreementFile;
            }






        }
        #endregion

        #region 绑定星级选择控件
        protected void BindStatrs(string selectIndex)
        {

            this.sel_star.Items.Clear();
            this.sel_star.Items.Add(new ListItem("-请选择-", "0"));
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = list[i].Value;
                    item.Text = list[i].Text.Substring(1, list[i].Text.Length - 1);
                    this.sel_star.Items.Add(item);
                }
            } if (selectIndex != "")
            {
                this.sel_star.SelectedValue = selectIndex;
            }
        }
        #endregion

        #region 保存事件
        protected void Save()
        {
            EyouSoft.Model.SupplierStructure.SupplierSpot sight = new EyouSoft.Model.SupplierStructure.SupplierSpot();
            GetLinkMnasAndSubmit();

        }
        #endregion

        #region 获取页面控件的值并提交
        protected void GetLinkMnasAndSubmit()
        {
            sight = new EyouSoft.Model.SupplierStructure.SupplierSpot();
            if (Request.Files.Count > 0)
            {
                string filepath = string.Empty;
                string oldfilename = string.Empty;
                IList<EyouSoft.Model.SupplierStructure.SupplierPic> suPics = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
                EyouSoft.Model.SupplierStructure.SupplierPic suPic = new EyouSoft.Model.SupplierStructure.SupplierPic();
                //景点图片
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        //排除掉合作协议的file控件
                        if (Request.Files.GetKey(i).ToString() != "file_green")
                        {
                            bool result_landspace = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[i], "SightImg", out filepath, out oldfilename);
                            if (result_landspace)
                            {
                                suPics = DisonseImg_Sight_Add(filepath, oldfilename, suPics);

                            }

                        }

                    }
                    //如果是修改操作,调用下面的方法
                    if (type == "modify")
                    {
                        suPics = GetImg_Photo(hidImgPhoto.Value, suPics);
                    }
                    if (suPics != null && suPics.Count > 0)
                    {
                        sight.SupplierPic = suPics;
                    }
                }
                //合作协议
                bool result_AgreementFile = EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["file_green"], "SightAgreementFile", out filepath, out oldfilename);
                if (result_AgreementFile)
                {
                    if (filepath != "")
                    {
                        sight.AgreementFile = filepath;
                    }
                    else
                    {
                        if (this.greend_hidden.Value != "null")
                        {
                            sight.AgreementFile = this.green_file.Value;
                        }
                        else
                        {
                            sight.AgreementFile = "";
                        }
                    }

                }

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
            sight.CompanyId = SiteUserInfo.CompanyID;
            //设置景点的省份名字
            sight.CityId = Utils.GetInt(this.city_id.Value);
            sight.ProvinceId = Utils.GetInt(this.pro_id.Value);
            EyouSoft.Model.CompanyStructure.Province Provincemodel = new EyouSoft.BLL.CompanyStructure.Province().GetModel(this.ucProvince1.ProvinceId);
            if (Provincemodel != null)
            {
                sight.ProvinceName = Provincemodel.ProvinceName;
            }
            //设置景点的城市名字
            EyouSoft.Model.CompanyStructure.City citymodel = new EyouSoft.BLL.CompanyStructure.City().GetModel(this.ucCity1.CityId);
            if (citymodel != null)
            {
                sight.CityName = citymodel.CityName;
            }
            //为要更新的景点赋值
            sight.Id = sid;
            sight.Remark = Utils.GetFormValue("txt_remark");
            sight.SupplierContact = contacts;
            sight.Start = (EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar)Utils.GetInt(Utils.GetFormValue(this.sel_star.UniqueID));
            sight.TeamPrice = Utils.GetDecimal(Utils.GetFormValue("rl_price"));
            sight.TourGuide = Utils.GetFormValue("guideworld");
            sight.TravelerPrice = Utils.GetDecimal(Utils.GetFormValue("single_price"));
            sight.UnitAddress = Utils.GetFormValue("companyAddress");
            sight.UnitName = Utils.GetFormValue("companyName");
            sight.UnitPolicy = Utils.GetFormValue("txt_zc");
            sight.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点;
            bool result = false;
            //操作类型判断
            if (type == "add")
            {
                //添加
                result = sightBll.Add(sight);
            }
            else if (type == "modify")
            {
                //更新
                result = sightBll.Update(sight);
            }
            //返回true操作成功,返回false操作失败
            if (result == true)
            {
                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.location='/SupplierControl/SightManager/SightList.aspx';window.parent.Boxy.getIframeDialog('{1}').hide()", "保存成功", Utils.GetQueryStringValue("iframeId")));
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }

        }
        #endregion


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
        //设定该页面为弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        //判断景点图片和合作协议的状态是否有值
        protected void CheckImg(EyouSoft.Model.SupplierStructure.SupplierSpot sight)
        {
            if (sight.AgreementFile != "" && sight.AgreementFile != null)
            {
                //为页面上的合作协议控件赋值
                this.hz_img_name.Text = "查看合作协议";
                this.green_file.Value = sight.AgreementFile;
                hz_img_state = true;
            }

            if (sight.SupplierPic.Count != 0)
            {
                //this.jd_img_name.Text = sight.SupplierPic[0].PicName;
                this.landspace_file.Value = sight.SupplierPic[0].PicName;
                jd_img_state = true;
            }

        }
        //处理合作协议事件
        protected string DisponseImg_geed(string filepath)
        {
            string file = "";
            //用户有新的上传替代之前的合作协议
            if (hz_img_state == false)
            {
                if (filepath != "")
                {
                    file = filepath;
                }

            }
            else if (filepath == "" && hz_img_state == true)
            {
                //如果该景点有合作协议没有删除也未添加新的协议
                if (this.hz_img_name.Text != "null")
                {
                    file = green_file.Value;
                }
            }
            return file;
        }

        //处理景点图片事件
        protected IList<EyouSoft.Model.SupplierStructure.SupplierPic> DisonseImg_sight(string filepath, string fileName)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierPic> pics = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
            EyouSoft.Model.SupplierStructure.SupplierPic pic = new EyouSoft.Model.SupplierStructure.SupplierPic();
            //如果页面上的上传控件不为空,则判断为有新的图片上传
            if (filepath != "")
            {
                pic.PicPath = filepath;
                pic.PicName = fileName;
                pic.SupplierId = SiteUserInfo.CompanyID;

                pics.Add(pic);
            }
            //如果页面上的上传控件为空,但是本身有图片则进入该条件
            else if (filepath == "" && jd_img_state == true)
            {
                //如果页面上用于记录景点图片信息的隐藏域没有被设为null则代表没有删除该图片,如果为null则代表图片被删除并且未添加新的图片,返回的pics为null.
                if (this.sight_hidden.Value != "null")
                {
                    pic.PicName = landspace_name.Value;
                    pic.PicPath = landspace_file.Value;
                    pic.SupplierId = SiteUserInfo.CompanyID;
                    pics.Add(pic);
                }

            }
            return pics;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        //获取图片或者路径
        public string GetPathByType(string type)
        {
            string path = "";
            string green = Convert.ToString(green_path);
            string jd = Convert.ToString(jd_path);
            if (type == "green")
            {
                if (green != "")
                {
                    path = green;
                }
            }
            else if (type == "sight")
            {
                if (jd != "")
                {
                    path = jd;
                }
            }
            return path;

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
