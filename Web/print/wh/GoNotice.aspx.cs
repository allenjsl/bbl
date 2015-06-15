using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.print.wh
{
    /// <summary>
    /// 出团通知书
    /// </summary>
    /// 柴逸宁
    /// 2011.4.27

    public partial class GoNotice : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 出团日期
        /// </summary>
        protected DateTime? xcTime = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明散拼计划对象
            EyouSoft.Model.TourStructure.TourBaseInfo tModel = bll.GetTourInfo(Utils.GetQueryStringValue("tourId"));
            if (tModel != null)
            {
                if (tModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)tModel;
                    Bind(model);
                }
                else
                { 
                    EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)tModel;
                    Bind(model);
                }
            }
            else
            {
                Response.Clear();
                Response.Write("未找到信息！");
                Response.End();
            }
        }
        /// <summary>
        /// 团队计划出团任务书
        /// </summary>
        /// <param name="model"></param>
        private void Bind(EyouSoft.Model.TourStructure.TourTeamInfo model)
        {
            //发团时间
            xcTime = model.LDate;
            //出发交通
            lblGoTraffic.Text = model.LTraffic;
            //返回交通
            lblEndTraffic.Text = model.RTraffic;
            //出团时间
            lblLDate.Text = model.LDate.ToShortDateString();
            //联系人
            EyouSoft.BLL.CompanyStructure.CompanyUser csBLL = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            string SentPeoples = "";
            //判断是否有联系人
            if (model.SentPeoples.Count >= 0)
            {
                for (int i = 0; i < model.SentPeoples.Count; i++)
                {
                    //联系人数据
                    SentPeoples = "";
                    string tel = csBLL.GetUserBasicInfo(model.SentPeoples[i].OperatorId).ContactTel;
                    SentPeoples += "联系人：" + model.SentPeoples[i].OperatorName + "" + tel;
                }
            }
            else
            {
                SentPeoples = "暂无联系人";
            }
            //联系人
            lblSentPeoples.Text = SentPeoples;
            //集合时间
            lblGatheringTime.Text = model.GatheringTime;
            //集合地点
            lblGatheringPlace.Text = model.GatheringPlace;
            //集合标志
            lblGatheringSign.Text = model.GatheringSign;
            //返航时间
            lblRTraffic.Text = model.RTraffic;
            //线路名称
            lblRouteName.Text = model.RouteName;
            //地接安排
            if (model.LocalAgencys != null)
            {
                rptlist.DataSource = model.LocalAgencys;
                rptlist.DataBind();
            }
            //标准发布
            if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                //标准发布数据不为空
                if (model.TourNormalInfo != null)
                {
                    //行程信息
                    xc_list.DataSource = model.TourNormalInfo.Plans;
                    xc_list.DataBind();
                    //不包含项目
                    lblBuHanXiangMu.Text = model.TourNormalInfo.BuHanXiangMu;
                    //自费项目
                    lblZiFeiXIangMu.Text = model.TourNormalInfo.ZiFeiXIangMu;
                    //儿童安排
                    lblErTongAnPai.Text = model.TourNormalInfo.ErTongAnPai;
                    //购物安排
                    lblGouWuAnPai.Text = model.TourNormalInfo.GouWuAnPai;
                    //购物安排
                    lblGouWuAnPai.Text = model.TourNormalInfo.GouWuAnPai;
                    //注意事项
                    lblZhuYiShiXiang.Text = model.TourNormalInfo.ZhuYiShiXiang;
                    
                    //包含项目数据绑定
                    rpt_sList.DataSource = model.Services;
                    rpt_sList.DataBind();
                    //显示标准
                    pnlProject.Visible = true;
                    //隐藏快速
                    xcquick.Visible = false;
                }


                //lt_desc.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.WenXinTiXing);
            }
            else//快速发布
            {
                //隐藏标准
                pnlProject.Visible = false;
                //显示快速
                xcquick.Visible = true;
                if (model.TourQuickInfo != null)
                {
                    lblQuickPlan.Text = model.TourQuickInfo.QuickPlan;
                    lblKs.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourQuickInfo.Remark);

                }
            }
        }
        /// <summary>
        /// 散拼出团任务书
        /// </summary>
        /// <param name="model"></param>
        private void Bind(EyouSoft.Model.TourStructure.TourInfo model)
        {
            //发团时间
            xcTime = model.LDate;
            //出发交通
            lblGoTraffic.Text = model.LTraffic;
            //返回交通
            lblEndTraffic.Text = model.RTraffic;
            //出团时间
            lblLDate.Text = model.LDate.ToShortDateString();
            //联系人
            EyouSoft.BLL.CompanyStructure.CompanyUser csBLL = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            string SentPeoples = "";
            //判断是否有联系人
            if (model.SentPeoples.Count >= 0)
            {
                for (int i = 0; i < model.SentPeoples.Count; i++)
                {
                    //联系人数据
                    SentPeoples = "";
                    string tel = csBLL.GetUserBasicInfo(model.SentPeoples[i].OperatorId).ContactTel;
                    SentPeoples += "联系人：" + model.SentPeoples[i].OperatorName + "" + tel;
                }
            }
            else
            {
                SentPeoples = "暂无联系人";
            }
            //联系人
            lblSentPeoples.Text = SentPeoples;
            //集合时间
            lblGatheringTime.Text = model.GatheringTime;
            //集合地点
            lblGatheringPlace.Text = model.GatheringPlace;
            //集合标志
            lblGatheringSign.Text = model.GatheringSign;
            //返航时间
            lblRTraffic.Text = model.RTraffic;
            //线路名称
            lblRouteName.Text = model.RouteName;
            //地接安排
            if (model.LocalAgencys != null)
            {
                rptlist.DataSource = model.LocalAgencys;
                rptlist.DataBind();
            }
            //标准发布
            if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                //标准发布数据不为空
                if (model.TourNormalInfo != null)
                {
                    //行程信息
                    xc_list.DataSource = model.TourNormalInfo.Plans;
                    xc_list.DataBind();
                    //不包含项目
                    lblBuHanXiangMu.Text = model.TourNormalInfo.BuHanXiangMu;
                    //自费项目
                    lblZiFeiXIangMu.Text = model.TourNormalInfo.ZiFeiXIangMu;
                    //儿童安排
                    lblErTongAnPai.Text = model.TourNormalInfo.ErTongAnPai;
                    //购物安排
                    lblGouWuAnPai.Text = model.TourNormalInfo.GouWuAnPai;
                    //购物安排
                    lblGouWuAnPai.Text = model.TourNormalInfo.GouWuAnPai;
                    //注意事项
                    lblZhuYiShiXiang.Text = model.TourNormalInfo.ZhuYiShiXiang;
                    //包含项目数据绑定
                    rpt_sList.DataSource = model.TourNormalInfo.Services;
                    rpt_sList.DataBind();
                    //显示标准
                    pnlProject.Visible = true;
                    //隐藏快速
                    xcquick.Visible = false;
                }


                //lt_desc.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.WenXinTiXing);
            }
            else//快速发布
            {
                //隐藏标准
                pnlProject.Visible = false;
                //显示快速
                xcquick.Visible = true;
                if (model.TourQuickInfo != null)
                {
                    lblQuickPlan.Text = model.TourQuickInfo.QuickPlan;
                    lblKs.Text = model.TourQuickInfo.Remark;

                }
            }
        }
        /// <summary>
        /// 吃饭转化
        /// </summary>
        /// <param name="eat"></param>
        /// <returns></returns>
        public string getEat(string eat)
        {
            string returnValue = "";
            if (eat.Contains("2"))
            {
                returnValue += "早，";
            }
            if (eat.Contains("3"))
            {
                returnValue += "中，";
            }
            if (eat.Contains("4"))
            {
                returnValue += "晚，";
            }
            return returnValue.TrimEnd('，');
        }
    }
}
