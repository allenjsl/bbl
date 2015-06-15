using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.GroupEnd.FitHairDay
{
    /// <summary>
    /// 组团端 散客天天发 申请
    /// 创建人:李晓欢
    /// 创建时间:2011-03-22
    /// </summary>
    public partial class SingUp : Eyousoft.Common.Page.FrontPage
    {
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> sinfo = null;

        EyouSoft.Model.TourStructure.TourEverydayInfo toureveryday = null;
        EyouSoft.BLL.TourStructure.TourEveryday blltour = null;
        IList<EyouSoft.Model.CompanyStructure.CustomStand> list = null;
        public string url = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            toureveryday = new EyouSoft.Model.TourStructure.TourEverydayInfo();
            blltour = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);

            if (!this.Page.IsPostBack)
            {
                //设置LoadVisitors1控件的CurrentPageIframeId属性
                LoadVisitors1.CurrentPageIframeId = Request.QueryString["iframeId"];

                BindPrices();
                BindCustomers();
                string tourid = EyouSoft.Common.Utils.GetQueryStringValue("tourid");
                if (!string.IsNullOrEmpty(tourid) && tourid != null)
                {
                    toureveryday = blltour.GetTourEverydayInfo(tourid);
                    if (toureveryday != null)
                    {
                        this.lit_RouteName.Text = toureveryday.RouteName;
                        url = getUrl(tourid, (int)toureveryday.ReleaseType) + "?tourId=" + tourid;
                    }
                    EyouSoft.Model.CompanyStructure.Area Area = new EyouSoft.BLL.CompanyStructure.Area().GetModel(toureveryday.AreaId);
                    if (Area != null)
                    {
                        this.lit_AreaName.Text = Area.AreaName;
                    }
                }

            }
        }

        #region 线路连接行程单
        public string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
            }
        }
        #endregion

        /// <summary>
        /// 绑定客户等级
        /// </summary>
        void BindCustomers()
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            //int kkk = 0;
            list = bll.GetCustomStandByCompanyId(SiteUserInfo.CompanyID);
            rpt_Customer.DataSource = list;
            rpt_Customer.DataBind();
        }


        #region 绑定结算价格
        protected void BindPrices()
        {
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourEverydayInfo model = new EyouSoft.Model.TourStructure.TourEverydayInfo();
            model = bl.GetTourEverydayInfo(Request.QueryString["tourid"]);
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            list = bll.GetCustomStandByCompanyId(SiteUserInfo.CompanyID);
            int kkk = list.Count;
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> plist = model.PriceStandards;
            for (int i = 0; i < listStand.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (listStand[i].CustomerLevels.Count > j)
                    {
                        var vn = list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
                        if (vn != null)
                            listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
                        else
                            listStand[i].CustomerLevels.RemoveAt(j);
                    }
                    var xn = listStand[i].CustomerLevels.Where(x => x.LevelId == list[j].Id).FirstOrDefault();
                    if (xn == null)
                        listStand[i].CustomerLevels.Add(new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo() { LevelId = list[j].Id, LevelType = list[j].LevType, LevelName = list[j].CustomStandName, AdultPrice = 0, ChildrenPrice = 0 });
                }
            }
            sinfo = plist;
        }
        #endregion

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //散客天天发申请
            EyouSoft.Model.TourStructure.TourEverydayApplyInfo TourEverydayApplyInfo = new EyouSoft.Model.TourStructure.TourEverydayApplyInfo();
            EyouSoft.BLL.TourStructure.TourEveryday tourbll = new EyouSoft.BLL.TourStructure.TourEveryday();
            //散拼天天发业务逻辑和实体类
            string TourId = Utils.GetQueryStringValue("tourId");
            toureveryday = blltour.GetTourEverydayInfo(TourId);
            if (TourId != null && TourId != "")
            {
                if (toureveryday != null)
                {
                    TourEverydayApplyInfo.CompanyId = toureveryday.CompanyId;
                }
            }
            TourEverydayApplyInfo.TourId = Utils.GetQueryStringValue("tourId");
            TourEverydayApplyInfo.OperatorId = SiteUserInfo.ID;
            TourEverydayApplyInfo.AdultNumber = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("TxtAdultPrice"));
            TourEverydayApplyInfo.ApplyCompanyId = SiteUserInfo.TourCompany.TourCompanyId;
            TourEverydayApplyInfo.ApplyId = SiteUserInfo.ID.ToString();
            TourEverydayApplyInfo.ApplyTime = System.DateTime.Now;
            TourEverydayApplyInfo.ChildrenNumber = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue("TxtChildrenPrice"));
            TourEverydayApplyInfo.HandleStatus = EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus.未处理;
            TourEverydayApplyInfo.LDate = EyouSoft.Common.Utils.GetDateTime(EyouSoft.Common.Utils.GetFormValue("TxtSartTime"));
            TourEverydayApplyInfo.LevelId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.hd_LevelID.UniqueID));
            TourEverydayApplyInfo.StandardId = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetFormValue(this.hd_PriceStandId.UniqueID));
            TourEverydayApplyInfo.SpecialRequirement = EyouSoft.Common.Utils.GetFormValue("txt_Special");

            #region
            TourEverydayApplyInfo.Traveller = new EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo();
            TourEverydayApplyInfo.Traveller.TravellerDisplayType = EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.附件方式;
            #region 游客附件信息
            EyouSoft.Model.TourStructure.TourAttachInfo Attachinfo = new EyouSoft.Model.TourStructure.TourAttachInfo();
            string fileAtt = string.Empty;
            string oldfileAtt = string.Empty;
            string VisitorInfoFile = "/uploadFiles/zutuanFile/";
            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["fuiLoadAttachment"], VisitorInfoFile, out fileAtt, out oldfileAtt))
            {
                if (fileAtt.Trim() != "" && oldfileAtt.Trim() != "")
                {
                    TourEverydayApplyInfo.Traveller.TravellerFilePath = fileAtt;
                }
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "上传附件失败！");
                return;
            }
            #endregion
            TourEverydayApplyInfo.Traveller.Travellers = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            //获取所有的游客姓名
            string[] cus_arr = Utils.GetFormValues("txtVisitorName");
            //获取特服信息
            string[] uri = Utils.GetFormValues("tefu");
            string[] cardType = Utils.GetFormValues("ddlCardType");
            decimal orderprice = 0;
            for (int k = 0; k < cus_arr.Length; k++)
            {
                if (Utils.GetFormValues("txtVisitorName")[k] == "")
                {
                    break;
                }
                //订单游客信息实体
                EyouSoft.Model.TourStructure.TourOrderCustomer item = new EyouSoft.Model.TourStructure.TourOrderCustomer();

                item.VisitorName = Utils.GetFormValues("txtVisitorName")[k];
                //游客类型
                item.VisitorType = Utils.GetFormValues("ddlVisitorType")[k] == "1" ? EyouSoft.Model.EnumType.TourStructure.VisitorType.成人 : EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                #region 游客证件类型
                //switch (Utils.GetFormValues("ddlCardType")[k])
                //{
                //    case "1":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.身份证;
                //        } break;
                //    case "2":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.护照;
                //        } break;
                //    case "3":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.军官证;
                //        } break;
                //    case "4":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.台胞证;
                //        } break;
                //    case "5":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.港澳通行证;
                //        } break;
                //    default:
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.未知;
                //        } break;
                //}
                #endregion
                item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(cardType[k]);
                //游客证件号码
                item.CradNumber = Utils.GetFormValues("txtCardNo")[k];
                #region 游客性别
                switch (Utils.GetFormValues("ddlSex")[k])
                {
                    case "2":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                        } break;
                    case "1":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                        } break;
                    default:
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                        } break;
                }
                #endregion

                //游客联系电话
                item.ContactTel = Utils.GetFormValues("txtContactTel")[k];

                //游客特服信息实体类
                EyouSoft.Model.TourStructure.CustomerSpecialService css = new EyouSoft.Model.TourStructure.CustomerSpecialService();
                //特服费用
                css.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
                //增加费用还是减少费用
                css.IsAdd = Utils.GetFromQueryStringByKey(uri[k], "ddlOperate") == "0" ? true : false;
                css.IssueTime = DateTime.Now;
                //项目
                css.ProjectName = Utils.GetFromQueryStringByKey(uri[k], "txtItem");
                //服务内容
                css.ServiceDetail = Utils.GetFromQueryStringByKey(uri[k], "txtServiceContent");
                //游客特服信息
                item.SpecialServiceInfo = css;
                item.IssueTime = DateTime.Now;
                item.CompanyID = SiteUserInfo.CompanyID;

                //游客所有的信息添加到订单
                TourEverydayApplyInfo.Traveller.Travellers.Add(item);
                orderprice += Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
            }
            #endregion

            int result = tourbll.ApplyTourEveryday(TourEverydayApplyInfo);
            if (result > 0)
            {
                Response.Write("<script>alert('提交成功');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide()</script>");
            }
            else
            {
                Response.Write("<script>alert('提交失败!');location.href=location.href;</script>");
            }

        }
    }
}
