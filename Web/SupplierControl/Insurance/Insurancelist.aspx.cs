using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.SupplierControl.Insurance
{
    /// <summary>
    ///供应商管理 保险
    ///李晓欢
    ///2011-3-8
    /// </summary>
    public partial class Insurancelist : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        //单位名称
        protected string UnitsName = string.Empty;
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        //菜系
        protected string TxtUnitsName = string.Empty;
        protected int len = 0;
        //操作类型 删除or导出excel
        protected string action = string.Empty;

        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        //保险业务逻辑类和实体类
        EyouSoft.BLL.SupplierStructure.SupplierInsurance Insurancebll = null;
        EyouSoft.Model.SupplierStructure.SupplierInsurance Insurancemodle = null;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化保险业务逻辑类和实体类
            Insurancebll = new EyouSoft.BLL.SupplierStructure.SupplierInsurance();
            Insurancemodle = new EyouSoft.Model.SupplierStructure.SupplierInsurance();

            //权限判断
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_保险_栏目, true);
            }
            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_新增);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_删除);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_修改);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_导出);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_保险_导入);

            if (!this.Page.IsPostBack)
            {
                //操作类型 删除or导出excel
                action = Utils.GetQueryStringValue("action");
                switch (action)
                {
                    case "toexcel":
                        {
                            if (grantto)
                            {
                                CreateExcel("Insurance" + DateTime.Now.ToShortDateString());
                            }
                        }
                        break;
                    case "Insurancedel":
                        {
                            if (grantdel)
                            {
                                InsuranceDel();
                            }
                        }
                        break;
                    default:
                        DataInit();
                        break;
                }
            }
        }

        #region 删除
        /// <summary>
        /// 删除数据
        /// </summary>
        private void InsuranceDel()
        {
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            //删除酒店信息
            res = Insurancebll.Delete(tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }
        #endregion

        #region 初始化
        protected void DataInit()
        {
            EyouSoft.Model.SupplierStructure.SupplierQuery search = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            //初使化条件
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            TxtUnitsName = Utils.GetQueryStringValue("UnitsName");
            //查询单位名称
            search.UnitName = TxtUnitsName;

            IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> list = null;
            list = Insurancebll.GetList(PageSize, PageIndex, ref RecordCount, SiteUserInfo.CompanyID, search);
            if (list.Count > 0 && list != null)
            {
                //列表统计
                len = list.Count;
                //列表数据绑定                       
                this.replist.DataSource = list;
                this.replist.DataBind();
                //设置分页
                BindPage();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
            }

            list = null;
        }
        #endregion

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            TxtUnitsName = Utils.GetQueryStringValue("UnitsName");
            //列表数据绑定
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";
            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", "所在地", "单位名称", "联系人", "电话", "传真", "交易情况");
            //查询实体
            EyouSoft.Model.SupplierStructure.SupplierQuery search = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            //查询单位名称
            search.UnitName = TxtUnitsName;

            int tmpLen = Utils.GetInt(Utils.GetFormValue("hidLen"));
            if (tmpLen <= 0)
                tmpLen = 100;

            //保险集合
            IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> list = null;
            list = Insurancebll.GetList(tmpLen, 1, ref RecordCount, SiteUserInfo.CompanyID, search);
            foreach (EyouSoft.Model.SupplierStructure.SupplierInsurance sl in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n",
                    sl.ProvinceName + "" + sl.CityName, sl.UnitName,
                    sl.SupplierContact == null ? "" : sl.SupplierContact[0].ContactName,
                    sl.SupplierContact == null ? "" : sl.SupplierContact[0].ContactTel,
                sl.SupplierContact == null ? "" : sl.SupplierContact[0].ContactFax, sl.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
            list = null;
        }
        #endregion

        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = PageSize;
            this.ExporPageInfoSelect1.CurrencyPage = PageIndex;
            this.ExporPageInfoSelect1.intRecordCount = RecordCount;
        }
        #endregion
    }
}
