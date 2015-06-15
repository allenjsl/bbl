using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.administrativeCenter.contractManage
{
    /// <summary>
    /// 功能：行政中心-劳动合同管理
    /// 开发人：孙川
    /// 日期：2011-01-20
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        protected bool InsertFlag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_栏目))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_栏目, true);
                }
                if (CheckGrant(global::Common.Enum.TravelPermission.行政中心_劳动合同管理_新增合同))
                {
                    InsertFlag = true;
                }
            }
        }
    }
}
