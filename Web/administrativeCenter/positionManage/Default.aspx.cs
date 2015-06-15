using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.Collections;
using Common.Enum;

namespace Web.administrativeCenter.positionManage
{
    /// <summary>
    /// 功能：行政中心-职务管理列表
    /// 开发人：孙川
    /// 日期：2011-01-12
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        int PageIndex = 1;
        int PageSize = 20;
        int CurrentPage = 0;
        int RecordCount;
        #endregion

        #region 权限参数
        protected bool EditFlag = false;        //修改
        protected bool DeleteFlag = false;      //删除
        protected bool InsertFlag = false;      //新增
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            int positionID = Utils.GetInt(Request.QueryString["PositionID"],-1);        //职务ID
            string method = Utils.GetQueryStringValue("method");
            PageIndex = Utils.GetInt( Request.QueryString["Page"],1);

            if (!IsPostBack && method == "")
            {
                if (!CheckGrant(TravelPermission.行政中心_职务管理_栏目))
                {
                    Utils.ResponseNoPermit(TravelPermission.行政中心_职务管理_栏目, true);
                }
                if (CheckGrant(TravelPermission.行政中心_职务管理_新增职务))
                {
                    InsertFlag = true;
                }
                if (CheckGrant(TravelPermission.行政中心_职务管理_修改职务))
                {
                    EditFlag = true;
                }
                if (CheckGrant(TravelPermission.行政中心_职务管理_删除职务))
                {
                    DeleteFlag = true;
                }
                BindData();
            }
            if (positionID != -1 )
            {
                bool result = false;
                //删除前判断职务是否有人在担任
                //有则给出提示
                EyouSoft.BLL.AdminCenterStructure.DutyManager bllDutyManager = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
                if(method=="isHasPersonnel")            //判断该职务是否有人担任
                {
                    if (bllDutyManager.IsHasBeenUsed(CurrentUserCompanyID, positionID))
                    {
                        Response.Clear();
                        Response.Write("1");            //  有人担任该职务
                        Response.End();
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("0");
                        Response.End();
                    }
                }
                if (method == "deletePositionInfo")     //删除
                {
                    result = bllDutyManager.Delete(CurrentUserCompanyID, positionID);
                    if (result)
                    {
                        Response.Clear();
                        Response.Write("1");            //删除成功
                        Response.End();
                    }
                    else
                    {
                        Response.Clear();
                        Response.Write("0");
                        Response.End();
                    }
                }
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            EyouSoft.BLL.AdminCenterStructure.DutyManager bllDuty = new EyouSoft.BLL.AdminCenterStructure.DutyManager();
            IList<EyouSoft.Model.AdminCenterStructure.DutyManager> lDutyModel =  bllDuty.GetList(PageSize, PageIndex,ref RecordCount, CurrentUserCompanyID);
            if (lDutyModel != null && lDutyModel.Count != 0)
            {
                this.rptData.DataSource = lDutyModel;
                this.rptData.DataBind();
                this.BindPage();
            }
            else
            {
                this.rptData.EmptyText = "<tr><td colspan=\"6\"><div style=\"text-align:center;  margin-top:75px; margin-bottom:75px;\">没有相关的数据!</span></div></td></tr>";
                this.ExportPageInfo1.Visible = false;
            }
        }

        /// <summary>
        /// 设置分页控件参数
        /// </summary>
        private void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }

        /// <summary>
        /// 序号
        /// </summary>
        protected int GetCount()
        {
            return ++CurrentPage + (PageIndex - 1) * PageSize;
        }
    }
}
