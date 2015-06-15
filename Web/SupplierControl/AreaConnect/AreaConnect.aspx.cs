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
using System.Text;

namespace Web.SupplierControl.AreaConnect
{
    /// <summary>
    /// 供应商管理-地接
    /// </summary>
    /// dj 2011-01-17
    public partial class AreaConnect : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        protected int pageSize = 20;
        /// <summary>
        /// 显示第几页
        /// </summary>
        protected int pageIndex = 1;
        /// <summary>
        /// 总记录条数
        /// </summary>
        protected int recordCount;

        /// <summary>
        /// 省份id
        /// </summary>
        protected int province = 0;
        /// <summary>
        /// 城市id
        /// </summary>
        protected int city = 0;
        /// <summary>
        /// 查询条件
        /// </summary>
        protected string unionName = string.Empty;
        /// <summary>
        /// 操作
        /// </summary>
        protected string act = string.Empty;
        /// <summary>
        /// 业务逻辑层
        /// </summary>
        EyouSoft.BLL.CompanyStructure.CompanySupplier csBll = null;
        /// <summary>
        /// 记录条数
        /// </summary>
        protected int len = 0;


        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_地接_栏目, true);
            }

            //权限附值
            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_新增);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_修改);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_删除);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_导入);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_地接_导出);
            //获取当前用户所在公司id
            this.ucProvince1.CompanyId = CurrentUserCompanyID;
            //设为常用城市
            this.ucProvince1.IsFav = true;
            //获取当前用户所在公司id
            this.ucCity1.CompanyId = CurrentUserCompanyID;
            //是否设为常用城市
            this.ucCity1.IsFav = true;
            //初始化比BLL
            csBll = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            //操作赋值
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    //导出
                    case "toexcel":
                        if (grantto)
                        {
                            CreateExcel("area" + DateTime.Now.ToShortDateString());
                        }
                        break;
                    //删除
                    case "areadel":
                        if (grantdel)
                        {
                            AreaDel();
                        }
                        break;
                    default:
                        //加载数据
                        DataInit();
                        break;
                }
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        private void AreaDel()
        {
            string[] stid = Utils.GetFormValue("tid").Split(',');
            int[] tid = new int[stid.Length];
            for (int i = 0; i < stid.Length; i++)
            {
                tid[i] = Utils.GetInt(stid[i]);
            }
            bool res = false;
            res = csBll.DeleteSupplierInfo(tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }

        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            //获取查询省份
            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            //获取查询城市
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));
            //获取查询名称
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            //初始化List
            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> list = null;
            //分页获取数据
            list = csBll.GetList(pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, province, city, unionName, this.CurrentUserCompanyID);
            //获取记录条数
            len = list.Count;
            //绑定数据
            this.rptList.DataSource = list;
            this.rptList.DataBind();
            //设置分页
            BindPage();

            EyouSoft.Model.CompanyStructure.MSupplierSearchInfo searchModel = new EyouSoft.Model.CompanyStructure.MSupplierSearchInfo();

            if (city != 0)
            {
                searchModel.CityId = city;
            }

            if (province != 0)
            {
                searchModel.ProvinceId = province;
            }

            searchModel.Name = unionName;

            this.lblAllCount.Text = new EyouSoft.BLL.CompanyStructure.CompanySupplier().GetTimesGYSSummary(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, searchModel).ToString();

            this.ucProvince1.ProvinceId = province;
            this.ucCity1.CityId = city;
            this.ucCity1.ProvinceId = province;

        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            //查询省份
            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            //查询省份
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));
            //查询条件
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            //定义List
            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> list = null;
            //获取1条记录
            list = csBll.GetList(1, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, province, city, unionName, this.CurrentUserCompanyID);
            //判断记录条数
            if (recordCount != 0)
            {
                //有记录获取所以记录
                list = csBll.GetList(recordCount, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接, province, city, unionName, this.CurrentUserCompanyID);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", "所在地", "单位名称", "联系人", "电话", "传真", "交易情况");
            foreach (EyouSoft.Model.CompanyStructure.CompanySupplier cs in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n",
                    cs.ProvinceName + " " + cs.CityName,
                    cs.UnitName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactName,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactTel,
                    cs.SupplierContact == null ? "" : cs.SupplierContact[0].ContactFax,
                    cs.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
        }


        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }
        #endregion
    }
}
