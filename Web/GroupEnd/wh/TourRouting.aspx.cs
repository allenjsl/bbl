using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Model.EnumType;

namespace Web.GroupEnd
{
    /// <summary>
    /// 创建:万俊
    /// 功能:组团端_登陆页_旅游线路
    /// </summary>
    public partial class TourRouting : System.Web.UI.Page
    {
        #region 设置分页变量
        protected int pageIndex = 1;
        protected int recordCount;
        protected int pageSize = 10;
        #endregion
        protected int companyId = 0;
        EyouSoft.BLL.TourStructure.TourEveryday tourBll = new EyouSoft.BLL.TourStructure.TourEveryday();

        protected string prices = "";
        //登录公司ID
        EyouSoft.SSOComponent.Entity.UserInfo userModel = EyouSoft.Security.Membership.UserProvider.GetUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            //设置公司ID
            //判断 当前域名是否是专线系统为组团配置的域名
            EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
            companyId = domain.CompanyId;
            //设置导航
            this.WhHeadControl1.NavIndex = 2;
            if (!IsPostBack)
            {
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);     //取得页面上的pageindex值.
                BindDatas();
            }
            //设置导航用户控件的公司ID
            this.WhHeadControl1.CompanyId = companyId;
            //设置专线介绍用户控件的公司ID
            this.WhLineControl1.CompanyId = companyId;
            //设置友情链接公司ID
            this.WhBottomControl1.CompanyId = companyId;
            #region 设置页面title,meta
            //声明基础设置实体对象
            EyouSoft.Model.SiteStructure.SiteBasicConfig configModel = new EyouSoft.BLL.SiteStructure.SiteBasicConfig().GetSiteBasicConfig(domain.CompanyId);
            if (configModel != null)
            {
                //添加Meta
                Literal keywork = new Literal();
                keywork.Text = "<meta name=\"keywords\" content= \"" + configModel.SiteMeta + "\" />";
                this.Page.Header.Controls.Add(keywork);
            }
            #endregion
        }

        #region 绑定数据
        protected void BindDatas()
        {
            EyouSoft.Model.TourStructure.TourEverydaySearchInfo searchInfo = new EyouSoft.Model.TourStructure.TourEverydaySearchInfo();
            IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> list = tourBll.GetTourEverydays(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
            this.rptTourList.DataSource = list;
            if (list.Count > 0)
            {
                //显示分页控件
                this.ExporPageInfoSelect1.Visible = true;
                //数据绑定
                this.rptTourList.DataBind();
                //绑定分页
                BindExportPage();
                //隐藏无数据提示控件
                this.lblMsg.Visible = false;
            }
            else
            {
                //隐藏分页控件
                this.ExporPageInfoSelect1.Visible = false;
                this.lblMsg.Text = "未查找到相关线路信息";
                //显示无数据提示控件
                this.lblMsg.Visible = true;
            }

        }
        #endregion

        #region 绑定分页控件
        protected void BindExportPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams.Add(Request.QueryString);
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion

        #region 获得打印单链接
        /// <summary>
        /// 获得散拼计划的打印单
        /// </summary>
        /// <param name="tourid"></param>
        /// <param name="releaseType"></param>
        /// <returns></returns>
        protected string getUrl(string tourid, int releaseType)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            if (releaseType == 0)
            {
                return bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
            }
            else
            {
                return bll.GetPrintPath(companyId, CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
            }
        }
        #endregion

        #region 登录账户 取当前登录账户所在组团单位的客户等级的价格信息
        protected string BindPricesList(string Tourid, int CustomerLevel)
        {
            System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(userModel);
            EyouSoft.Model.TourStructure.TourEverydayInfo model = new EyouSoft.Model.TourStructure.TourEverydayInfo();
            //散客天天发信息业务实体
            model = bl.GetTourEverydayInfo(Tourid);
            //报价等级集合
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            //根据公司编号获取客户等级信息
            list = bll.GetCustomStandByCompanyId(userModel.CompanyID);
            stringPrice.Append("<tr bgcolor=\"#dfedfc\">");
            stringPrice.Append("<td height=\"20\" align=\"center\" width=\"40%\" bgcolor=\"#d0eafb\">报价标准</td>");
            stringPrice.Append("<td style=\"text-align:left\" width=\"60%\" bgcolor=\"#d0eafb\">价格</td>");
            for (int i = 0; i < listStand.Count; i++)
            {
                for (int j = 0; j < listStand[i].CustomerLevels.Count; j++)
                {
                    if (listStand[i].CustomerLevels[j].LevelId == CustomerLevel)
                    {
                        stringPrice.Append("<tr>");
                        stringPrice.AppendFormat("<td height=\"20\" width=\"50px\" align=\"center\" bgcolor=\"#f6fafd\">{0}&nbsp;&nbsp;</td>", listStand[i].StandardName);
                        stringPrice.Append("<td style=\"text-align:left\" bgcolor=\"#f6fafd\">");
                        stringPrice.AppendFormat("成人价：{0}&nbsp;&nbsp;<br/>", Utils.FilterEndOfTheZeroDecimal(listStand[i].CustomerLevels[j].AdultPrice));
                        stringPrice.AppendFormat("儿童价：{0}", Utils.FilterEndOfTheZeroDecimal(listStand[i].CustomerLevels[j].ChildrenPrice));
                        stringPrice.Append("</td></tr>");
                        break;
                    }
                }
            }
            stringPrice.Append("</tr>");
            prices = stringPrice.ToString();
            return prices;
        }
        #endregion

        #region 未登录用户 取所有报价标准中的门市价的成人价和儿童价
        protected string customlevtypeprice(string tourid)
        {
            if (userModel != null && userModel.ID > 0)
            {
                return BindPricesList(tourid, userModel.TourCompany.CustomerLevel);
            }
            else
            {
                System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
                stringPrice.Append("<tr bgcolor=\"#dfedfc\">");
                stringPrice.Append("<td height=\"20\" align=\"center\" width=\"40%\" bgcolor=\"#d0eafb\">报价标准</td>");
                stringPrice.Append("<td style=\"text-align:left\" width=\"60%\" bgcolor=\"#d0eafb\">价格</td>");

                EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(userModel);
                EyouSoft.Model.TourStructure.TourEverydayInfo model = new EyouSoft.Model.TourStructure.TourEverydayInfo();
                //散客天天发信息业务实体
                model = bl.GetTourEverydayInfo(tourid);
                //报价等级集合
                IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;
                IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
                EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
                //根据公司编号获取客户等级信息
                if (userModel != null)
                {
                    list = bll.GetCustomStandByCompanyId(userModel.CompanyID);
                }
                else
                {
                    EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                    if (domain != null)
                    {
                        list = bll.GetCustomStandByCompanyId(domain.CompanyId);
                    }
                }

                for (int i = 0; i < listStand.Count; i++)
                {
                    for (int j = 0; j < listStand[i].CustomerLevels.Count; j++)
                    {
                        if (listStand[i].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                        {
                            stringPrice.Append("<tr>");
                            stringPrice.AppendFormat("<td height=\"20\" width=\"50px\" align=\"center\" bgcolor=\"#f6fafd\">{0}&nbsp;&nbsp;</td>", listStand[i].StandardName);
                            stringPrice.Append("<td style=\"text-align:left\" bgcolor=\"#f6fafd\">");
                            stringPrice.AppendFormat("成人价：{0}&nbsp;&nbsp;<br/>", Utils.FilterEndOfTheZeroDecimal(listStand[i].CustomerLevels[j].AdultPrice));
                            stringPrice.AppendFormat("儿童价：{0}", Utils.FilterEndOfTheZeroDecimal(listStand[i].CustomerLevels[j].ChildrenPrice));
                            stringPrice.Append("</td></tr>");
                            break;
                        }
                    }
                }
                stringPrice.Append("</tr>");
                prices = stringPrice.ToString();
                return prices;
            }
        }
        #endregion


        protected string getPeoPlePrice(string Tourid)
        {
            string Prices = "";

            EyouSoft.BLL.TourStructure.TourEveryday bl = new EyouSoft.BLL.TourStructure.TourEveryday(userModel);
            EyouSoft.Model.TourStructure.TourEverydayInfo model = new EyouSoft.Model.TourStructure.TourEverydayInfo();
            //散客天天发信息业务实体
            model = bl.GetTourEverydayInfo(Tourid);
            //报价等级集合
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = model.PriceStandards;
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
            //根据公司编号获取客户等级信息
            if (userModel != null)
            {
                list = bll.GetCustomStandByCompanyId(userModel.CompanyID);
            }
            else
            {
                EyouSoft.Model.SysStructure.SystemDomain domain = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomain(Request.Url.Host.ToLower());
                if (domain != null)
                {
                    list = bll.GetCustomStandByCompanyId(domain.CompanyId);
                }
            }

            #region 登录状态
            if (userModel != null && userModel.ID > 0)
            {
                if (listStand != null && listStand.Count > 0)
                {
                    for (int i = 0; i < listStand.Count; i++)
                    {
                        if (listStand[0].CustomerLevels != null)
                        {
                            for (int j = 0; j < listStand[0].CustomerLevels.Count; j++)
                            {
                                if (listStand[0].CustomerLevels[j].LevelId == userModel.TourCompany.CustomerLevel)
                                {
                                    Prices = Utils.FilterEndOfTheZeroString(listStand[0].CustomerLevels[j].AdultPrice.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(listStand[0].CustomerLevels[j].ChildrenPrice.ToString("0.00"));
                                    break;
                                }
                                else
                                {
                                    Prices = "";
                                }
                            }
                        }
                    }
                }

            }
            #endregion

            #region 未登录
            if (userModel == null)
            {
                for (int i = 0; i < listStand.Count; i++)
                {
                    if (listStand[i].CustomerLevels != null)
                    {
                        for (int j = 0; j < listStand[i].CustomerLevels.Count; j++)
                        {
                            if (listStand[i].CustomerLevels[j].LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                            {
                                Prices = Utils.FilterEndOfTheZeroString(listStand[i].CustomerLevels[j].AdultPrice.ToString("0.00")) + "/" + Utils.FilterEndOfTheZeroString(listStand[i].CustomerLevels[j].ChildrenPrice.ToString("0.00"));
                            }
                        }
                    }
                }

            }
            #endregion

            return Prices;
        }

        /// <summary>
        /// 获得附件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected string GetAttachs(object item)
        {
            string str = "";
            IList<EyouSoft.Model.TourStructure.TourAttachInfo> attachs = (List<EyouSoft.Model.TourStructure.TourAttachInfo>)item;
            if (attachs != null && attachs.Count > 0)
            {
                str = "<a href=\"" + attachs[0].FilePath + "\" target=\"_blank\">点击下载</a>";
            }
            else
            {
                str = "<a href=\"javascript:void(0);\">无附件</a>";
            }

            return str;
        }
    }
}
