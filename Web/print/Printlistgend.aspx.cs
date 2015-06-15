using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.print
{
    public partial class Printlistgend :FrontPage
    {
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
            //判断该计划是散拼计划
            if (tourModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                //团队计划打印名单
                model = new PrintUrlModel();
                model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路打印名单);
                model.ShowText = EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路打印名单.ToString();
                list.Add(model);
                //行程单
                if (tourModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    //散拼计划快速发布行程单
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路快速发布行程单);                    
                    model.ShowText = "组团线路行程单";
                    list.Add(model);
                }
                else
                {
                    //散拼计划标准版发布行程单
                    model = new PrintUrlModel();
                    model.PrintUrl = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.组团线路标准发布行程单);                    
                    model.ShowText = "组团线路行程单";
                    list.Add(model);
                }
            }

            if (list != null && list.Count > 0)
            {
                this.rptUPrintUrl.DataSource = list;
                this.rptUPrintUrl.DataBind();
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
}
