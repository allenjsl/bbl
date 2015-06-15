using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Collections;

namespace Web.SupplierControl.Shopping
{
    /// <summary>
    /// 购物-详细
    /// </summary>
    public partial class TradeSituation : Eyousoft.Common.Page.BackPage
    {


        #region 分页变量
        protected int pageSize = 4;
        protected int pageIndex = 1;
        protected int recordCount;
        protected int len = 0;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.供应商管理_购物_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.供应商管理_购物_栏目, false);

            }
            if (!IsPostBack)
            {
                int tid = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                bind(tid);
                
            }
        }

        private void bind(int tid)
        {
            //初始化条件
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            EyouSoft.BLL.SupplierStructure.SupplierShopping ssBLL=new EyouSoft.BLL.SupplierStructure.SupplierShopping();

            //IList<EyouSoft.Model.SupplierStructure.SupplierShopping> list = ssBLL.GetModel(tid);
        }
    }
}
