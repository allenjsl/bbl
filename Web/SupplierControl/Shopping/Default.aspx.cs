using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.SupplierControl.Shopping
{
    /// <summary>
    /// 工商管理-购物
    /// 柴逸宁
    /// 2011.3.9
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        #region 变量
        //分页变量
        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;

        protected int province = 0;//省份id
        protected int city = 0;//城市id



        protected string unionName = string.Empty;//查询条件
        EyouSoft.BLL.SupplierStructure.SupplierShopping csBLL = null;

        protected int len = 0;//列表长度

        //权限变量
        protected bool grantadd = false;//新增
        protected bool grantmodify = false;//修改
        protected bool grantdel = false;//删除
        protected bool grantload = false;//导入
        protected bool grantto = false;//导出

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_栏目, false);

            }
            string act = string.Empty;//操作
            //权限赋值
            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_新增);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_修改);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_删除);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_导入);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_导出);


            ucProvince1.CompanyId = CurrentUserCompanyID;
            ucProvince1.IsFav = true;
            ucCity1.CompanyId = CurrentUserCompanyID;
            ucCity1.IsFav = true;

            csBLL = new EyouSoft.BLL.SupplierStructure.SupplierShopping();
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");//获取操作类型
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        if (grantto)
                        {
                            CreateExcel("area" + DateTime.Now.ToShortDateString());//导出
                        }
                        break;
                    case "areadel":
                        if (grantdel)
                        {
                            AreaDel();//删除
                        }
                        break;
                    default:
                        DataInit();
                        break;
                }
            }

        }



        /// <summary>
        /// 初使化
        /// </summary>
        private void DataInit()
        {
            //初使化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);

            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));
            unionName = Utils.GetQueryStringValue("unionName");

            IList<EyouSoft.Model.SupplierStructure.SupplierShopping> list = null;

            //获取查询条件
            EyouSoft.Model.SupplierStructure.SupplierQuery queryModel = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            queryModel.UnitName = unionName;
            queryModel.ProvinceId = province;
            queryModel.CityId = city;


            list = csBLL.GetList(pageSize, pageIndex, ref recordCount, CurrentUserCompanyID, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物, queryModel);
            if (!(list == null))
            {
                len = list.Count;
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                list = null;
            }
            //设置分页
            BindPage();

            this.ucProvince1.ProvinceId = province;
            this.ucCity1.CityId = city;
            this.ucCity1.ProvinceId = province;
        }


        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="FileName"></param>
        private void CreateExcel(string FileName)
        {
            province = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("province"));
            city = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("city"));

            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            IList<EyouSoft.Model.SupplierStructure.SupplierShopping> list = null;
            EyouSoft.Model.SupplierStructure.SupplierQuery queryModel = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            list = csBLL.GetList(1, 1, ref recordCount, CurrentUserCompanyID, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物, queryModel);
            if (recordCount != 0)
            {
                list = csBLL.GetList(recordCount, 1, ref recordCount, CurrentUserCompanyID, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物, queryModel);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", "所在地", "单位名称", "销售产品", "联系人", "电话", "传真", "交易情况");
            foreach (EyouSoft.Model.SupplierStructure.SupplierShopping ss in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n",
                    ss.ProvinceName + " " + ss.CityName,
                    ss.UnitName,
                    ss.SaleProduct,
                    ss.SupplierContact == null ? "" : ss.SupplierContact[0].ContactName,
                    ss.SupplierContact == null ? "" : ss.SupplierContact[0].ContactTel,
                    ss.SupplierContact == null ? "" : ss.SupplierContact[0].ContactFax,
                    ss.TradeNum);
            }
            Response.Write(sb.ToString());
            Response.End();
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
            res = csBLL.DeleteShopping(tid);
            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
            Response.End();
        }
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
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
