using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.CashFlow
{
    /// <summary>
    /// 页面功能：现金流量表--补充现金
    /// Author:Liuym
    /// Date:2011-1-21
    /// </summary>
    public partial class AddCash : BackPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_现金流量_现金流量_补充现金))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_现金流量_现金流量_补充现金, true);
                return;
            }
            #endregion
            if (!IsPostBack)
            {               
                this.lblAddTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

        }

        #region 补充现金
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string AddCash = this.txtAddCash.Value.Trim();//补充现金
            DateTime AddTime = DateTime.Now;//补充时间
            string iframeId = Utils.GetQueryStringValue("IframeId");
            EyouSoft.BLL.StatisticStructure.CompanyCashFlow CashFlowBll = new EyouSoft.BLL.StatisticStructure.CompanyCashFlow(this.SiteUserInfo);
            EyouSoft.Model.StatisticStructure.CompanyCashFlow Model = new EyouSoft.Model.StatisticStructure.CompanyCashFlow();
            Model.IssueTime = AddTime;
            Model.OperatorId = this.SiteUserInfo.ID;
            Model.CompanyId = this.SiteUserInfo.CompanyID;
            Model.CashReserve = decimal.Parse(AddCash);
            Model.CashType = EyouSoft.Model.EnumType.StatisticStructure.CashType.现金储备;
            if (AddCash != "" && AddTime != null)
            {
                //调用保存方法
                if (CashFlowBll.Add(Model))
                {
                    Utils.ShowMsgAndCloseBoxy("添加成功！", iframeId, true);
                }
                else
                {
                    Utils.ShowMsgAndCloseBoxy("添加失败！", iframeId, false);
                }
            }
            else
            {
                Page.RegisterStartupScript("提示", Utils.ShowMsg("输入信息不完整，无法保存！"));
            }
            //释放资源
            Model = null;
            CashFlowBll = null;

        }
        #endregion
    }
}
