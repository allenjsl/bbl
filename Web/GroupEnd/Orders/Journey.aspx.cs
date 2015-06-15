using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using EyouSoft.Common;
using EyouSoft.BLL.TourStructure;
using EyouSoft.Model.TourStructure;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 页面功能：显示行程内容
    /// </summary>
    /// 创建人：柴逸宁
    /// 创建时间：2011-06-21
    public partial class Journey : Eyousoft.Common.Page.FrontPage
    {
        /// <summary>
        /// 出团时间
        /// </summary>
        protected DateTime? xcTime = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //隐藏全部行程信息
                HideXC();
                //绑定行程
                BindxlXianchen();
            }
        }
        /// <summary>
        /// 绑定行程内容
        /// </summary>
        void BindxlXianchen()
        {
            string tourid = Utils.GetQueryStringValue("tourid");
            //线路编号
            if (tourid.Length > 0)
            {
                //线路BLL
                Tour bll = new Tour();
                //获取实体
                TourBaseInfo baseModel = bll.GetTourInfo(tourid);
                //实例化行程安排
                IList<XingCheng> xclist = new List<XingCheng>();
                switch (baseModel.TourType)
                {

                    case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划:
                        #region 散拼计划
                        //强转散拼计划实体
                        TourInfo spModel = (TourInfo)baseModel;
                        //判断发布类型
                        if (spModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                        {
                            //显示标准发布行程信息
                            rptXC.Visible = true;
                            //遍历行程性息
                            foreach (var l in spModel.TourNormalInfo.Plans)
                            {
                                xclist.Add(new XingCheng(l.Interval, l.Vehicle, l.Hotel, l.Dinner.Contains("1") ? 1 : 0, l.Dinner.Contains("2") ? 1 : 0, l.Dinner.Contains("3") ? 1 : 0, l.Dinner.Contains("4") ? 1 : 0, l.Plan, l.FilePath));
                            }
                            //出团时间
                            xcTime = spModel.LDate;

                        }
                        else
                        {
                            //显示快速发布的行程信息
                            lblXC.Visible = true;
                            //显示内容赋值
                            if (spModel.TourQuickInfo.QuickPlan.Length > 0)
                            {
                                lblXC.Text = "<br/> 内容：" + spModel.TourQuickInfo.QuickPlan;
                            }
                            else
                            {
                                lblXC.Text = "<br/> 暂无数据！";
                            }
                        }

                        #endregion
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划:
                        #region 团队计划
                        //强转团队计划实体
                        TourTeamInfo tdmodel = (TourTeamInfo)baseModel;
                        //判断发布类型
                        if (tdmodel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                        {
                            //显示标准发布行程信息
                            rptXC.Visible = true;
                            //遍历行程性息
                            foreach (var l in tdmodel.TourNormalInfo.Plans)
                            {
                                xclist.Add(new XingCheng(l.Interval, l.Vehicle, l.Hotel, l.Dinner.Contains("1") ? 1 : 0, l.Dinner.Contains("2") ? 1 : 0, l.Dinner.Contains("3") ? 1 : 0, l.Dinner.Contains("4") ? 1 : 0, l.Plan, l.FilePath));
                            }
                            //出团时间
                            xcTime = tdmodel.LDate;
                        }
                        else
                        {
                            //显示快速发布的行程信息
                            lblXC.Visible = true;
                            //显示内容赋值
                            if (tdmodel.TourQuickInfo.QuickPlan.Length > 0)
                            {
                                lblXC.Text = "<br/>内容：" + tdmodel.TourQuickInfo.QuickPlan;
                            }
                            else
                            {
                                lblXC.Text = "<br/> 暂无数据！";
                            }

                        }
                        #endregion
                        break;
                }
                //判断标准发布数据是否存在
                if (rptXC != null)
                {
                    rptXC.DataSource = xclist;
                    rptXC.DataBind();
                }
                else
                {
                    //显示提示
                    lblMsg.Visible = true;
                    //隐藏列表
                    rptXC.Visible = false;
                }
            }

        }
        /// <summary>
        /// 隐藏全部行程信息
        /// </summary>
        private void HideXC()
        {
            lblXC.Visible = false;
            rptXC.Visible = false;
            lblMsg.Visible = false;
        }
        /// <summary>
        /// 转化吃饭时间
        /// </summary>
        /// <param name="eat">值</param>
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
        protected override void OnInit(EventArgs e)
        {
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnInit(e);
        }
    }
}
