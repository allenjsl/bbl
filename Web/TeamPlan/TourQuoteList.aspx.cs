using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 上团报价列表页
    /// 功能：上团报价
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-21 
    /// </summary>
    public partial class TourQuoteList : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        //声明上传报价bll对象
        private EyouSoft.BLL.TourStructure.QuoteAttach bll = new EyouSoft.BLL.TourStructure.QuoteAttach();

        protected void Page_Load(object sender, EventArgs e)
        {
            //上传报价权限判断
            if (!CheckGrant(TravelPermission.团队计划_上传报价_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.团队计划_上传报价_栏目, true);
            }

            if (!IsPostBack)
            {
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);

                //初始化方法
                string fileTitle = Utils.GetQueryStringValue("title");
                this.txtTitle.Text = fileTitle;
                DateTime? addDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("addDate"));
                if (addDate != null)
                {
                    this.txtAddDate.Text = Convert.ToDateTime(addDate).ToString("yyyy-MM-dd");
                }
                DataInit(fileTitle, addDate);
                
                
            }
            //操作类型
            string type = Utils.GetQueryStringValue("type");
            if (type != "")
            {
                //数据删除
                if (type == "Delete")
                {
                    //获得列表需要删除的ID 集合
                    string[] idList = Utils.GetQueryStringValue("id").Split(',');
                    if (idList.Count() > 0)
                    {
                        bool result = true;
                        for (int i = 0; i < idList.Count(); i++)
                        {
                            if (idList[i].Trim() != "")
                            {
                                //删除数据
                               result = bll.DeleteQuote(Utils.GetInt(idList[i]));
                            }
                        }
                        //返回信息
                        Response.Clear();
                        if (result)
                        {
                            Response.Write("Ok");
                        }
                        else
                        {
                            Response.Write("Error");
                        }
                        Response.End();
                    }
                }
            }

        }

        #region 初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected void DataInit(string fileTitle,DateTime? addTime)
        {
            //查询model
            EyouSoft.Model.TourStructure.QuoteAttach searchModel = new EyouSoft.Model.TourStructure.QuoteAttach();
            searchModel.FileName = fileTitle;
            searchModel.AddTime = addTime;
            searchModel.CompanyId = SiteUserInfo.CompanyID;
            //声明list
            IList<EyouSoft.Model.TourStructure.QuoteAttach> list = bll.GetQuoteList(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, searchModel);
            //判断list
            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                //设置分页
                BindPage();
                //隐藏提示
                lblMsg.Visible = false;
            }
            else
            { 
                //没有数据隐藏分页控件
                this.ExportPageInfo1.Visible = false;
                lblMsg.Visible = true;
            }
            //释放服务器资源
            list = null;
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
}
