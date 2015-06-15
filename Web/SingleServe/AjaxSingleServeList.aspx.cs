using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common;
using Eyousoft.Common.Page;
using Common.Enum;

namespace Web.SingleServe
{
    /// <summary>
    /// 页面功能：Ajax获取单项服务列表
    /// Author:liuym
    /// Date:2011-01-12
    /// </summary>
    /// 修改记录
    /// 修改人：柴逸宁
    /// 修改时间：2011-5-31
    /// 修改备注：前台添加客户单位详细页面链接
    public partial class AjaxSingleServeList : BackPage
    {
        #region Protected Members
        protected int PageIndex = 1;//页码
        protected int PageSize = 20;//每页显示的记录：20条
        protected int RecordCount = 0;//总记录
        protected string OrderNo = string.Empty;//订单号
        protected string TourCode = ""; //团号
        protected DateTime? OrderStartTime = null;//下单开始时间
        protected DateTime? OrderEndTime = null;//下单结束时间
        protected string Operator=string.Empty;//操作员        
        protected string CustomerCompany = string.Empty;//客户单位
        protected string DeleteId = string.Empty;//删除ID
        protected bool EditFlag = true;//修改权限
        protected bool DeleteFlag = true;//删除权限
        protected bool AddFlag = true;//新增权限
        EyouSoft.BLL.TourStructure.Tour tourBll = null;
        #endregion

        protected string PrintUrl = "";

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            tourBll = new EyouSoft.BLL.TourStructure.Tour(this.SiteUserInfo);

            #region 权限验证
            //单项服务删除权限
            if (!CheckGrant(TravelPermission.单项服务_单项服务_删除服务))
            {
                this.linkDelete.Visible = false;
                DeleteFlag = false;
            }
            //单项服务修改权限
            if (!CheckGrant(TravelPermission.单项服务_单项服务_修改服务))
            {
                this.linkUpdate.Visible = false;
                EditFlag = false;
            }
            //单项服务新增权限
            if (!CheckGrant(TravelPermission.单项服务_单项服务_新增服务))
            {
                this.linkAdd.Visible = false;
                AddFlag = false;
            }
            #endregion

            #region 项模板列删除
            DeleteId = Request.QueryString["DeleteId"];
            if (DeleteId != "" && Request.QueryString["DeleteId"] != null)//删除ID
            {
                //调用删除的方法
                if (tourBll.Delete(DeleteId))
                {
                    Response.Clear();
                    Response.Write("1");
                    Response.End();
                }
            }
            #endregion

            if (!IsPostBack)
            {
                ////判断权限
                if (!CheckGrant(TravelPermission.单项服务_单项服务_栏目))
                {
                    Utils.ResponseNoPermit(TravelPermission.单项服务_单项服务_栏目, true);
                    return;
                }
                //绑定列表
                BindSingleServeList();
            }
        }
        #endregion

        #region 绑定单项服务列表
        private void BindSingleServeList()
        {
            #region 获取参数
            //获取页码
            if (Utils.GetInt(Request.QueryString["Page"]) > 1)
            {
                PageIndex = Utils.GetInt(Request.QueryString["Page"]);
            }
            OrderNo = Utils.InputText(Request.QueryString["OrderNo"]);//订单号
            TourCode = Utils.GetQueryStringValue("TourCode");   //团号
            OrderStartTime = Utils.GetDateTimeNullable(Request.QueryString["OrderStartTime"]);
            OrderEndTime = Utils.GetDateTimeNullable(Request.QueryString["OrderEndTime"]);
            Operator = Request.QueryString["Operator"];//操作员
            CustomerCompany = Utils.InputText(Request.QueryString["CustomerCompany"]);//客户单位
            EyouSoft.Model.TourStructure.TourSingleSearchInfo model = new EyouSoft.Model.TourStructure.TourSingleSearchInfo();
            #endregion

            #region 查询Model赋值
            model.OperatorId = Utils.ConvertToIntArray(Utils.InputText(Request.QueryString["OperatorId"]));
            model.OrderSTime = OrderStartTime;//下单开始日期
            model.OrderETime = OrderEndTime;//下单结束日期
            model.BuyerCName = CustomerCompany;//客户单位名称
            model.OrderCode = OrderNo;//订单号
            model.TourCode = TourCode; //团号
            #endregion

            IList<EyouSoft.Model.TourStructure.LBSingleTourInfo> list = tourBll.GetToursSingle(this.SiteUserInfo.CompanyID, PageSize, PageIndex, ref RecordCount, model);
            if (list != null && list.Count > 0)
            {
                //调用获取列表集合的方法
                this.tbl_ExportPage.Visible = true;
                this.crp_SingleServeList.DataSource = list;
                this.crp_SingleServeList.DataBind();
                // 绑定分页控件
                BindPage();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_SingleServeList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='10' align='center'>暂时没有数据！</td></tr>";
            }
            //释放资源
            list = null;
            model = null;
            tourBll = null;
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
            this.ExporPageInfoSelect1.HrefType = Adpost.Common.ExporPage.HrefTypeEnum.JsHref;
            this.ExporPageInfoSelect1.AttributesEventAdd("onclick", "SingleServeList.LoadData(this);", 1);
            this.ExporPageInfoSelect1.AttributesEventAdd("onchange", "SingleServeList.LoadData(this);", 0);
        }
        #endregion

        #region 批量删除
        protected void linkDelete_Click(object sender, EventArgs e)
        {
            bool Result = false;
            string[] ckVals = Utils.GetFormValues("ckSingleServe");
            if (ckVals.Length == 0)
            {
                Utils.ShowAndRedirect("订单状态已经提交到财务,不能删除!", "/SingleServe/SingleServeList.aspx");
            }
            if (ckVals.Length > 0)
            {
                for (int i = 0; i < ckVals.Length; i++)
                {
                    Result = tourBll.Delete(ckVals[i]);
                }
            }
            Utils.ShowAndRedirect(Result ? "删除成功!" : "删除失败!", "/SingleServe/SingleServeList.aspx");
        }
        #endregion

        #region 单项服务列表状态
        public string GetSingleServeStatu(string SingleSeveStatu)
        {
            if (SingleSeveStatu == "财务核算" || SingleSeveStatu == "核算结束")
            {
                return SingleSeveStatu;
            }
            else
            {
                return "操作中";
            }
        }
        #endregion

        #region 确认单
        protected string GetPrintUrl(string Tourid)
        {
            string Url = "";
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            Url = bll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.单项服务确认单);
            if (Url != "" && !string.IsNullOrEmpty(Url))
            {
                PrintUrl = "<a style=\"margin-left:3px\" href=\"" + Url + "?tourid=" + Tourid + "\"  target=\"_blank\">确认单</a>";
            }
            return PrintUrl;
        }
        #endregion        
    }
}
