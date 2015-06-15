using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Text;

namespace Web.SupplierControl.Others
{
    /// <summary>
    /// 供应商管理-其他
    /// 作者：柴逸宁
    /// </summary>
    /// 时间：2011-03-08
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        #region 变量

        protected int pageSize = 20;
        protected int pageIndex = 1;
        protected int recordCount;

        protected string unionName = string.Empty;


        EyouSoft.BLL.CompanyStructure.CompanySupplier csBLL = null;
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
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_其它_栏目, true);
            }
            string act = string.Empty;
            //权限附值
            grantadd = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_新增);
            grantmodify = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_修改);
            grantdel = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_删除);
            grantload = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_导入);
            grantto = CheckGrant(global::Common.Enum.TravelPermission.供应商管理_其它_导出);



            csBLL = new EyouSoft.BLL.CompanyStructure.CompanySupplier();
            act = EyouSoft.Common.Utils.GetQueryStringValue("act");
            if (!IsPostBack)
            {
                switch (act)
                {
                    case "toexcel":
                        if (grantto)
                        {
                            CreateExcel("area" + DateTime.Now.ToShortDateString());
                        }
                        break;
                    case "areadel":
                        if (grantdel)
                        {
                            AreaDel();
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
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");

            IList<EyouSoft.Model.SupplierStructure.SupplierOther> list = null;
            list = csBLL.GetOtherList(pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他, unionName, CurrentUserCompanyID);

            if (!(list == null))
            {
                len = list.Count;
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                list = null;
            }

            //设置分页
            BindPage();


        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            unionName = EyouSoft.Common.Utils.GetQueryStringValue("unionName");
            IList<EyouSoft.Model.SupplierStructure.SupplierOther> list = null;


            //EyouSoft.Model.SupplierStructure.SupplierQuery queryModel = new EyouSoft.Model.SupplierStructure.SupplierQuery();
            list = csBLL.GetOtherList(1, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他, unionName, CurrentUserCompanyID);
            if (recordCount != 0)
            {
                list = csBLL.GetOtherList(recordCount, 1, ref recordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他, unionName, CurrentUserCompanyID);
            }
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", "所在地", "单位名称", "联系人", "电话", "传真", "交易情况");
            foreach (EyouSoft.Model.SupplierStructure.SupplierOther cs in list)
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
            res = csBLL.DeleteSupplierInfo(tid);

            Response.Clear();
            Response.Write(string.Format("{{\"res\":{0}}}", res ? 1 : -1));
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
