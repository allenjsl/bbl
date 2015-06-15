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
using System.Collections.Generic;
using EyouSoft.Common;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 添加人：刘飞
    /// 时间：2012-8-23
    /// 功能：批量审核列表
    /// </summary>
    public partial class piliangshenhe : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string dotype = Utils.GetQueryStringValue("dotype");
            if (!string.IsNullOrEmpty(dotype))
            {
                Ajax(dotype);
            }
            if (!IsPostBack)
            {
                BindList();
            }
        }

        /// <summary>
        /// 绑定审核列表数据
        /// </summary>
        private void BindList()
        {
            string tourcode = Utils.GetQueryStringValue("tourcode");
            string ordercode = Utils.GetQueryStringValue("ordercode");
            string companyname = Utils.GetQueryStringValue("companyname");
            string leaveStime = Utils.GetQueryStringValue("leaveSTime").Trim();
            string leaveEtime = Utils.GetQueryStringValue("leaveETime").Trim();
            string skStime = Utils.GetQueryStringValue("skStime").Trim();
            string skEtime = Utils.GetQueryStringValue("skEtime").Trim();
            string sltstatus = Utils.GetQueryStringValue("sltstatus");
            EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchmodel = new EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo();
            searchmodel.CTETime = Utils.GetDateTimeNullable(leaveEtime);
            searchmodel.CTSTime = Utils.GetDateTimeNullable(leaveStime);
            searchmodel.KeHuMingCheng = companyname;
            searchmodel.OrderCode = ordercode;
            searchmodel.SKETime = Utils.GetDateTimeNullable(skEtime);
            searchmodel.SKSTime = Utils.GetDateTimeNullable(skStime);
            bool? status = false;
            if (sltstatus == "0")
            {
                status = null;
            }
            else if (sltstatus == "2")
            {
                status = true;
            }

            searchmodel.SKStatus = status;
            searchmodel.TourCode = tourcode;

            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? tongJiDingDanFangShi = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (tongJiDingDanFangShi.HasValue) searchmodel.TongJiDingDanFangShi = tongJiDingDanFangShi.Value;

            pageIndex = Utils.GetQueryStringValue("page") != "" ? Utils.GetInt(Utils.GetQueryStringValue("page")) : 1;
            EyouSoft.BLL.FinanceStructure.BShouRu bll = new EyouSoft.BLL.FinanceStructure.BShouRu();
            IList<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo> list = bll.GetShouKuanShenHeLB(this.SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, searchmodel);
            if (list != null && list.Count > 0)
            {
                decimal TotalMoney = 0;
                rptList.DataSource = list;
                rptList.DataBind();
                bll.GetShouKuanShenHeLBHeJi(this.SiteUserInfo.CompanyID, searchmodel, out TotalMoney);
                this.lbTotleMoney.Text = "￥" + TotalMoney.ToString("F2");
            }
            else
            {
                lbemptydata.Text = "<tr><td style='text-align:center' colspan='8'>暂无数据!</td></tr>";
            }
            #region 分页
            ExportPageInfo1.intPageSize = pageSize;
            ExportPageInfo1.intRecordCount = recordCount;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
            #endregion

        }
        /// <summary>
        /// 做审核/取消审核操作
        /// </summary>
        /// <param name="dotype">操作类型</param>
        /// <returns></returns>
        private string Operater(string dotype)
        {
            string msg = string.Empty;
            string dengjiId = Utils.GetQueryStringValue("dengjiid").Trim(',');
            string orderId = Utils.GetQueryStringValue("orderid").Trim(',');
            if (dotype == "check")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_收款审核))
                {
                    msg = "您没有收款审核的权限";
                    return msg;
                }
                EyouSoft.BLL.FinanceStructure.BShouRu bll = new EyouSoft.BLL.FinanceStructure.BShouRu();
                string[] ArrDengji = dengjiId.Split(',');
                string[] ArrOrderid = orderId.Split(',');
                int result = 0;
                if (ArrDengji.Length != ArrOrderid.Length)
                {
                    msg = "数据错误，请重试!";
                    return msg;
                }
                for (int i = 0; i < ArrDengji.Length; i++)
                {
                    result = bll.ShenHeShouKuan(ArrDengji[i], ArrOrderid[i], this.SiteUserInfo.CompanyID, this.SiteUserInfo.ID);
                    if (result != 1)
                    {
                        msg = "审核失败，请重试!";
                        return msg;
                    }
                }
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "审核成功!";
                }

            }
            else if (dotype == "cencer")
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_取消收款审核))
                {
                    msg = "您没有取消收款审核的权限";
                    return msg;
                }
                EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder();
                if (bll.CancelIncomeChecked(this.SiteUserInfo.CompanyID, orderId, dengjiId))
                {
                    msg = "取消审核成功!";
                }
                else
                {
                    msg = "取消审核失败，请重试!";
                }
            }
            else
            {
                msg = "操作失败，请重试!";
            }
            return msg;
        }
        /// <summary>
        /// Ajax 操作
        /// </summary>
        /// <param name="dotype"></param>
        private void Ajax(string dotype)
        {
            string msgresult = Operater(dotype);
            //返回ajax操作结果
            Response.Clear();
            Response.Write(msgresult);
            Response.End();
        }

    }
}
