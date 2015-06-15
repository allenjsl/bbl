using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using EyouSoft.Common;
using System.Collections.Generic;

namespace Web.caiwuguanli
{
    public partial class Jidiaoleixing : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面初始绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目, false);
                }
                BindInfo();
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        void BindInfo()
        {
            int count = 0;
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo SearchInfo = new EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo();
            #region 查询参数
            string ddltype = Utils.GetQueryStringValue("tourtype");

            string teamNum = Utils.GetQueryStringValue("tourCode");
            txt_teamNum.Value = teamNum;

            string com = Utils.GetQueryStringValue("companyName");
            txt_com.Value = com;

            string comtype = Utils.GetQueryStringValue("comType");

            string goDate = Utils.GetQueryStringValue("beginDate");
            txt_godate.Value = goDate;
            if (ddltype != "-1" && ddltype != "")
            {
                select.Value = ddltype;

                SearchInfo.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)Utils.GetInt(ddltype);
            }
            if (teamNum != "")
            {
                SearchInfo.TourCode = teamNum;
            }
            if (com != "")
            {
                SearchInfo.GYSName = com;
            }
            if (comtype != "-1" && comtype != "")
            {
                ddl_comType.SelectedValue = comtype;
                SearchInfo.ZhiChuLeiBie = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Utils.GetInt(comtype);
            }
            SearchInfo.CTSTime = Utils.GetDateTimeNullable(goDate);
            SearchInfo.CTETime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("rdate"));
            #endregion

            EyouSoft.BLL.FinanceStructure.BZhiChu newbll = new EyouSoft.BLL.FinanceStructure.BZhiChu();
            IList<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo> List = newbll.GetJiDiaoZhiChuLB(CurrentUserCompanyID, 20, Utils.GetInt(Utils.GetQueryStringValue("page")), ref count, SearchInfo);
            if (List != null && List.Count > 0)
            {
                rpt_list1.DataSource = List;
                rpt_list1.DataBind();
            }

            //合计
            decimal ZhichuMoney, yidengjiMoney, yizhifuMoney = 0;
            newbll.GetJiDiaoZhiChuLBHeJi(CurrentUserCompanyID, SearchInfo, out ZhichuMoney, out yidengjiMoney, out yizhifuMoney);
            this.lbyidengjimoney.Text = yidengjiMoney.ToString("c2");
            this.lbyizhifumoney.Text = yizhifuMoney.ToString("c2");
            this.lbzhichumoney.Text = ZhichuMoney.ToString("c2");
            //未登记=支出金额-已登记
            //未支付=支出金额-已支付
            this.lbweizhifumoney.Text = (ZhichuMoney - yizhifuMoney).ToString("c2");
            this.lbweidengjimoney.Text = (ZhichuMoney - yidengjiMoney).ToString("c2");
            this.rpt_list1.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='11' align='center'>暂时没有数据！</td></tr>";
            #region 分页
            ExportPageInfo1.intPageSize = 20;
            ExportPageInfo1.intRecordCount = count;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            BindInfo();
        }
    }
}
