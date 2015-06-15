using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.PlanStructure;

namespace Web.print
{
    /// <summary>
    /// 打印列表页面
    /// </summary>
    public partial class printlist : BackPage
    {
        /// <summary>
        /// 指定当前计划是否已完成出票
        /// </summary>
        protected bool TicketStatus = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                if (tourId != "")
                {
                    this.hideTourId.Value = tourId;
                }
                DataInit(tourId);
            }
        }

        protected string GetUrl(string showText, string printUrl)
        {
            return showText == EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单.ToString() ? printUrl : (printUrl + "?tourid=" + hideTourId.Value);
        }

        protected void rptUPrintUrlItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string result = "";
            PrintUrlModel model = e.Item.DataItem as PrintUrlModel;
            if (model != null)
            {
                Literal ltr = e.Item.FindControl("ltrLink") as Literal;
                if (ltr != null)
                {
                    //判断当前打印单类型
                    if (model.ShowText == EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单.ToString())//当前打印单 是送团单
                    {
                        //判断当前计划是否完成出票
                        if (TicketStatus == false)//没有
                        {
                            result = string.Format("<a href=\"{0}\">{1}</a>", model.PrintUrl, model.ShowText);
                        }
                        else//有
                        {
                            result = string.Format("<a href='{0}' target='_blank'>{1}</a>", model.PrintUrl + "?tourid=" + hideTourId.Value, model.ShowText);
                        }
                    }
                    else//其他打印单
                    {
                        result = string.Format("<a href='{0}' target='_blank'>{1}</a>", model.PrintUrl + "?tourid=" + hideTourId.Value, model.ShowText);
                    }

                    ltr.Text = result;
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            this.PageType = PageType.boxyPage;
            base.OnPreInit(e);
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        protected void DataInit(string tourId)
        {

            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour tourBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourBaseInfo tourModel = tourBll.GetTourInfo(tourId);
            //声明bll对象
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            //声明保存打印URL 的集合
            IList<PrintUrlModel> list = new List<PrintUrlModel>();
            //声明临时对象 用来保存url 和显示文字
            PrintUrlModel model = null;
            //判断该计划是团队计划
            if (tourModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                EyouSoft.Model.TourStructure.TourTeamInfo teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)tourModel;
                //团队计划打印名单
                model = new PrintUrlModel();
                model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划打印名单);
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划打印名单.ToString();
                if (model.PrintUrl != "")
                {
                    list.Add(model);
                }
                //行程单
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    //团队计划快速发布行程单
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划快速发布行程单);
                    model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划快速发布行程单.ToString();
                    if (model.PrintUrl != "")
                    {
                        list.Add(model);
                    }
                }
                else
                {
                    //团队计划标准版发布行程单
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划标准发布行程单);
                    model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划标准发布行程单.ToString();
                    if (model.PrintUrl != "")
                    {
                        list.Add(model);
                    }
                }

                //团队确认单
                model = new PrintUrlModel();
                model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队确认单);
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队确认单.ToString();
                if (model.PrintUrl != "")
                {
                    list.Add(model);
                }

                //报价单
                model = new PrintUrlModel();
                model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.报价单);
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.报价单.ToString();
                if (model.PrintUrl != "")
                {
                    list.Add(model);
                }

                //团队送团单
                model = new PrintUrlModel();
                if (teamModel.TicketStatus == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票)
                {
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单);

                }
                else
                {
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单);
                    TicketStatus = true;
                }
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单.ToString();
                if (model.PrintUrl != "")
                    list.Add(model);


                //原行程下载
                model = new PrintUrlModel();
                if (teamModel.Attachs != null && teamModel.Attachs.Count > 0)
                {
                    model.PrintUrl = teamModel.Attachs[0].FilePath.ToString().Trim();
                    model.ShowText = "原行程下载";
                }
                if (model.PrintUrl != null && model.PrintUrl != "")
                    list.Add(model);



            }
            if (tourModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                EyouSoft.Model.TourStructure.TourInfo teamModel = (EyouSoft.Model.TourStructure.TourInfo)tourModel;

                model = new PrintUrlModel();
                model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划打印名单);
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划打印名单.ToString();
                if (model.PrintUrl != "")
                    list.Add(model);
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划标准发布行程单);
                    model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划标准发布行程单.ToString();
                    if (model.PrintUrl != "")
                        list.Add(model);
                }
                if (teamModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划快速发布行程单);
                    model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散拼计划快速发布行程单.ToString();
                    if (model.PrintUrl != "")
                        list.Add(model);
                }

                //model = new PrintUrlModel();
                //model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散客确认单);
                //model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.散客确认单.ToString();
                //if (model.PrintUrl != "")
                //    list.Add(model);


                //散拼送团单
                model = new PrintUrlModel();
                if (teamModel.TicketStatus == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票)
                {
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单);
                    TicketStatus = true;

                }
                else
                {
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单);
                    TicketStatus = true;
                }

                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团单.ToString();
                if (model.PrintUrl != "")
                    list.Add(model);


                //原行程下载
                model = new PrintUrlModel();
                if (teamModel.Attachs != null && teamModel.Attachs.Count > 0)
                {
                    model.PrintUrl = teamModel.Attachs[0].FilePath.ToString().Trim();
                    model.ShowText = "原行程下载";
                }
                if (model.PrintUrl != null && model.PrintUrl != "")
                    list.Add(model);


            }






            //结算通知单
            model = new PrintUrlModel();
            model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算通知单);
            model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算通知单.ToString();
            if (model.PrintUrl != "")
                list.Add(model);
            //结算明细单
            //model = new PrintUrlModel();
            //model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算明细单);
            //model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.结算明细单.ToString();
            //if (model.PrintUrl != "")
            //{
            //    list.Add(model);
            //}

            //地接确认单
            model = new PrintUrlModel();
            model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.地接确认单);
            model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.地接确认单.ToString();
            if (model.PrintUrl != "")
            {
                list.Add(model);
            }

            //送团任务单
            model = new PrintUrlModel();
            model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团任务单);
            model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.送团任务单.ToString();
            if (model.PrintUrl != "")
            {
                list.Add(model);
            }

            if (list != null && list.Count > 0)
            {
                this.rptUPrintUrl.DataSource = list;
                this.rptUPrintUrl.DataBind();
            }

        }
    }



    public class PrintUrlModel
    {
        private string printUrl;

        public string PrintUrl
        {
            get { return printUrl; }
            set { printUrl = value; }
        }
        private string showText;

        public string ShowText
        {
            get { return showText; }
            set { showText = value; }
        }
    }
}
