using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;
using System.Text.RegularExpressions;
using EyouSoft.BLL.CompanyStructure;
using Common.Enum;

namespace Web.sanping
{
    public partial class DaydayAdd : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 价格等级
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> sinfo = null;
        /// <summary>
        /// 默认为0，添加，1.修改,2.复制
        /// </summary>
        public int actType = 0;
        /// <summary>
        /// 列表
        /// </summary>
        public string bindPriceList = "";
        /// <summary>
        /// 操作人
        /// </summary>
        public string hdOprator = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CheckGrant(TravelPermission.散拼计划_散客天天发_新增))
                {
                    // Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_新增计划, false);
                }
                else
                {
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散客天天发_新增, false);
                }
                BindArea();
                bindPriceList = BindPriceList();
                BindCustomers();
                /////////////////修改2011.1.15.田想兵 begin/////////////////////////
                string id = Utils.GetQueryStringValue("id");
                if (Request.QueryString["act"] == "update")
                {
                    if (id != "")
                    {
                        actType = 1;
                        BindAllInfo(id);
                    }
                }
                //////////////////end//////////////////////////
                /////////////////复制2011.1.15.田想兵 begin/////////////////////////

                if (Request.QueryString["act"] == "copy")
                {
                    if (id != "")
                    {
                        actType = 2;
                        BindAllInfo(id);
                    }
                }
                //////////////////end//////////////////////////

            }
        }

        /// <summary>
        /// 绑定所有信息    用于修改或复制
        /// </summary>
        /// <returns></returns>
        void BindAllInfo(string id)
        {

            selectXl.Id = "1";
            selectXl.Name = "";
            selectXl.Bind();
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourEverydayInfo model = new EyouSoft.Model.TourStructure.TourEverydayInfo();
            model = bl.GetTourEverydayInfo(id);

            #region 价格绑定
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;

            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new CompanyCustomStand();

            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            int kkk = list.Count;

            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> plist = model.PriceStandards;
            for (int i = 0; i < listStand.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
                    if (listStand[i].CustomerLevels.Count > j)
                    {
                        var vn = list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
                        if (vn != null)
                            listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
                        else
                            listStand[i].CustomerLevels[j].LevelId = 0;
                            //listStand[i].CustomerLevels.RemoveAt(j);
                    }
                    var xn = listStand[i].CustomerLevels.Where(x => x.LevelId == list[j].Id).FirstOrDefault();
                    if (xn == null)
                        listStand[i].CustomerLevels.Add(new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo() { LevelId = list[j].Id, LevelType = list[j].LevType, LevelName = list[j].CustomStandName, AdultPrice = 0, ChildrenPrice = 0 });
                }
            }
            sinfo = plist;
            #endregion
            #region 基础数据绑定
            //行程
            List<EyouSoft.Model.TourStructure.TourPlanInfo> planInfo = model.TourNormalInfo.Plans.ToList();
            xingcheng1.Bind(planInfo);
            selectXl.Id = model.RouteId.ToString();
            selectXl.Name = model.RouteName;
            selectXl.Bind();
            ddl_area.SelectedItem.Selected = false;
            ddl_area.Items.FindByValue(model.AreaId.ToString()).Selected = true;
            txt_Days.Text = model.TourDays.ToString();
            ConProjectControl1.SetList = model.TourNormalInfo.Services;
            txt_noProject.Value = model.TourNormalInfo.BuHanXiangMu;
            txt_buy.Value = model.TourNormalInfo.GouWuAnPai;
            txt_owner.Value = model.TourNormalInfo.ZiFeiXIangMu;
            txt_child.Value = model.TourNormalInfo.ErTongAnPai;
            txt_Note.Value = model.TourNormalInfo.ZhuYiShiXiang;
            txt_Reminded.Value = model.TourNormalInfo.WenXinTiXing;
            txt_nbinfo.Value = model.TourNormalInfo.NeiBuXingXi;
            //附件
            if (model.Attachs.Count > 0)
            {
                if (model.Attachs[0].FilePath.Length > 1)
                {
                    pnlFile.Visible = true;
                    hypFilePath.NavigateUrl = model.Attachs[0].FilePath;
                    hd_img.Value = model.Attachs[0].FilePath;
                }
            }
            #endregion
        }

        /// <summary>
        /// 绑定区域
        /// </summary>
        void BindArea()
        {

            EyouSoft.BLL.CompanyStructure.Area area = new EyouSoft.BLL.CompanyStructure.Area(SiteUserInfo);
            int userid = SiteUserInfo.ID;
            IList<EyouSoft.Model.CompanyStructure.Area> list = new List<EyouSoft.Model.CompanyStructure.Area>();

            if (Request.QueryString["act"] == "update")
            {
                list = area.GetAreas();
            }
            else
            {
                list = area.GetAreaList(userid);
            }
            ddl_area.Items.Add(new ListItem("请选择线路区域", "-1"));
            ddl_area.Items.FindByValue("-1").Selected = true;
            foreach (var v in list)
            {
                ddl_area.Items.Add(new ListItem(v.AreaName, v.Id.ToString()));
            }
        }
        /// <summary>
        /// 报价标准
        /// </summary>
        public string BindPriceList()
        {
            StringBuilder sb = new StringBuilder();
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand price = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> pricelist = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();
            pricelist = price.GetPriceStandByCompanyId(CurrentUserCompanyID);

            foreach (var v in pricelist)
            {
                sb.Append("<option value=\"" + v.Id.ToString() + "|" + v.PriceStandName + "\">" + v.PriceStandName + "</option>");
            }
            pricelist = null;
            return sb.ToString();
        }
        /// <summary>
        /// 绑定客户等级
        /// </summary>
        void BindCustomers()
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new CompanyCustomStand();
            //int kkk = 0;
            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            rpt_Customer.DataSource = list;
            rpt_Customer.DataBind();
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_save_Click(object sender, EventArgs e)
        {
            #region 获取基础数据
            EyouSoft.BLL.TourStructure.TourEveryday bll = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourEverydayInfo model=new EyouSoft.Model.TourStructure.TourEverydayInfo();
            model.AreaId =Utils.GetInt( Utils.GetFormValue(ddl_area.UniqueID));
            model.CompanyId = CurrentUserCompanyID;
            model.CreateTime = DateTime.Now;
            model.OperatorId = SiteUserInfo.ID;
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> pricelist = new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();

            IList<EyouSoft.Model.CompanyStructure.CustomStand> listcus = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bllCom = new CompanyCustomStand();
            listcus = bllCom.GetCustomStandByCompanyId(CurrentUserCompanyID);

            int kkk = listcus.Count;
            for (int k = 0; k < Utils.GetFormValues("ddl_price").Length; k++)
            {
                EyouSoft.Model.TourStructure.TourPriceStandardInfo price = new EyouSoft.Model.TourStructure.TourPriceStandardInfo();
                price.StandardId = Utils.GetInt(Utils.GetFormValues("ddl_price")[k].Split('|')[0]);
                if (price.StandardId == 0)
                    continue;
                price.StandardName = Utils.GetFormValues("ddl_price")[k].Split('|')[1];
                IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> listLevels = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();


                string[] crPrice = Utils.GetFormValues("txt_cr_price");


                for (int i = 0; i < kkk; i++)
                {
                    EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo level = new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo();
                    level.AdultPrice = Utils.GetDecimal(Utils.GetFormValues("txt_cr_price")[i + kkk * k]);
                    level.ChildrenPrice = Utils.GetDecimal(Utils.GetFormValues("txt_rt_price")[i + kkk * k]);
                    level.LevelType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                    level.LevelName = Utils.GetFormValues("hd_cusStandName")[i];
                    level.LevelId = Utils.GetInt(Utils.GetFormValues("hd_cusStandId")[i]);
                    if (level.LevelId == 0)
                        continue;
                    listLevels.Add(level);
                }
                price.CustomerLevels = listLevels;
                pricelist.Add(price);
            }
            IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> levelList = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();
            model.PriceStandards = pricelist;

            model.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal;
            model.RouteId = Utils.GetInt(selectXl.Id);
            model.RouteName = selectXl.Name;
            model.TourDays = Utils.GetInt(Utils.GetFormValue(txt_Days.UniqueID));
            EyouSoft.Model.TourStructure.TourNormalPrivateInfo privateInfo = new EyouSoft.Model.TourStructure.TourNormalPrivateInfo();
            privateInfo.BuHanXiangMu = Utils.GetFormValue(txt_noProject.UniqueID);
            privateInfo.ErTongAnPai = Utils.GetFormValue(txt_child.UniqueID);
            privateInfo.GouWuAnPai = Utils.GetFormValue(txt_buy.UniqueID);
            privateInfo.NeiBuXingXi = Utils.GetFormValue(txt_nbinfo.UniqueID);
            privateInfo.Plans = xingcheng1.GetValues();
            privateInfo.Services = ConProjectControl1.GetDataList();
            privateInfo.WenXinTiXing = Utils.GetFormValue(txt_Reminded.UniqueID);
            privateInfo.ZhuYiShiXiang = Utils.GetFormValue(txt_Note.UniqueID);
            privateInfo.ZiFeiXIangMu = Utils.GetFormValue(txt_owner.UniqueID);
            model.TourNormalInfo = privateInfo;

            #region 附件

            IList<EyouSoft.Model.TourStructure.TourAttachInfo> listAttachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
            EyouSoft.Model.TourStructure.TourAttachInfo attInfo = new EyouSoft.Model.TourStructure.TourAttachInfo();
            string fileAtt = "";
            string oldfileAtt = "";
            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(FileUpload1.PostedFile, "dayDayPublish", out fileAtt, out oldfileAtt))
            {
                attInfo.FilePath = fileAtt;
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "上传附件失败！");
                return;
            }
            if (attInfo.FilePath.Length < 1)
            {
                attInfo.FilePath = hd_img.Value;
            }
            listAttachs.Add(attInfo);
            model.Attachs = listAttachs;
            #endregion 
            #endregion
            #region 数据库修改或添加
            if (Request.QueryString["act"] == "update")
            {
                model.TourId = Utils.GetQueryStringValue("id");
                int i  = bll.UpdateTourEverydayInfo(model);
                if(i>0)
                {
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "修改成功!", "DaydayPublish.aspx");
                }else
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "修改失败!", Request.Url.ToString());

            }
            else
            {
                int i = bll.InsertTourEverydayInfo(model);
                if (i > 0)
                {
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "添加成功!", "DaydayPublish.aspx");
                }
                else
                    EyouSoft.Common.Function.MessageBox.ShowAndRedirect(this.Page, "添加失败!", Request.Url.ToString());
            }
            #endregion
        }

    }
}
