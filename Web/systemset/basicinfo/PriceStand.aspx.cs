using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.basicinfo
{
    public partial class PriceStand : Eyousoft.Common.Page.BackPage
    {
        protected int itemIndex = 1;
        protected int itemIndex2 = 1;
        protected int pageIndex;
        protected int recordCount;
        protected int pageSize = 6;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_基础设置_报价标准栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_基础设置_报价标准栏目, true);
                return;
            }
            int prcId = Utils.GetInt(Utils.GetQueryStringValue("prcId"));//报价标准Id
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            itemIndex2 = (pageIndex - 1) * pageSize + 1;
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand priceStandBll = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();//初始化报价bll
            //报价Id不为空执行删除操作
            if (prcId != 0)
            {
                if (Utils.GetQueryStringValue("method") == "ispublish")//验证该区域是否有线路存在
                {
                    Utils.ResponseMeg(priceStandBll.IsUsed(prcId,CurrentUserCompanyID,1), "");
                    return;
                }
                bool result=priceStandBll.Delete(prcId);
                MessageBox.ShowAndRedirect(this, result ? "删除成功" : "删除失败", "/systemset/basicinfo/PriceStand.aspx");
                return;
            }
            //绑定报价标准
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> priceList = priceStandBll.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID);
            if (priceList != null && priceList.Count > 0)
            {
                rptPriStand.DataSource = priceList;
                rptPriStand.DataBind();
                BindExportPage();
            }
            else
            {
                rptPriStand.EmptyText = "<tr><td colspan='6' align='center'>对不起，暂无报价标准信息！</td></tr>";
                this.ExportPageInfo1.Visible = false;
            }
        }

        /// <summary>
        /// 绑定列表时获取换行操作
        /// </summary>
        /// <returns></returns>
        protected string GetListTr()
        {
            string strHtml = "";
            if (itemIndex == 1)
                strHtml= "<tr class='oddTr'>";
            else if (itemIndex % 4 == 1)
                strHtml= "</tr><tr class='oddTr'>";
             else if (itemIndex % 2 == 1)
                strHtml= "</tr><tr class='evenTr'>";
            itemIndex++;
            return strHtml; 
        }

        /// <summary>
        /// 获取绑定列表的最后一项
        /// </summary>
        /// <returns></returns>
        protected string GetLastTr()
        {
            if (recordCount == 0)
                return "";
            if (itemIndex % 2 == 0)
                return "<td colspan='3'></td></tr>";
            return "</tr>";
        }

        protected string GetExecute(Boolean IsSystem, int Id)
        {
           if(IsSystem)
           {
               return "<span class='issys'>默认报价不允许操作</span>";
           }
           else
           {
               return string.Format("<a href=\"javascript:;\" onclick=\"return PriceStand.update('{0}');\">修改 </a>|<a href=\"/systemset/basicinfo/PriceStand.aspx?prcId={0}\" onclick=\"return PriceStand.delTip('{0}');\"> 删除</a>", Id);
           }
        }

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindExportPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
 }
