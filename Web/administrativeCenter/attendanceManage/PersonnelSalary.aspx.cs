using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using Common.Enum;
using EyouSoft.Common;
using EyouSoft.Model.AdminCenterStructure;
using EyouSoft.Common.Function;

namespace Web.administrativeCenter.attendanceManage
{
    /// <summary>
    /// 人事考勤表
    /// </summary>
    /// 创建人：柴逸宁
    /// 创建时间：2011-10-19
    public partial class PersonnelSalary : BackPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(TravelPermission.行政中心_考勤管理_工资表))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(TravelPermission.行政中心_考勤管理_工资表, true);
                    return;
                }
                hd_Year.Value = Utils.GetInt(Utils.GetQueryStringValue("Year"), DateTime.Now.Year).ToString();
                hd_Month.Value = Utils.GetInt(Utils.GetQueryStringValue("Month"), DateTime.Now.Month).ToString();
                hd_Date.Value = (DateTime.Now.Year).ToString();
                BindList();
            }


        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {
            MWageSearchInfo queryModel = new MWageSearchInfo();

            IList<MWageInfo> ls = new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo().GetWages(
                CurrentUserCompanyID,
                Utils.GetInt(Utils.GetQueryStringValue("Year"), DateTime.Now.Year),
                Utils.GetInt(Utils.GetQueryStringValue("Month"), DateTime.Now.Month),
                queryModel);
            if (ls != null && ls.Count > 0)
            {
                rpt_list.DataSource = ls;
                rpt_list.DataBind();
            }
        }

        protected void lbtn_Save_Click(object sender, EventArgs e)
        {
            //职位
            string[] position = Utils.GetFormValues("txt_position");
            int count = position.Length;
            //姓名
            string[] name = Utils.GetFormValues("txt_name");
            //岗位工资
            string[] postWage = Utils.GetFormValues("txt_postWage");
            //全勤奖
            string[] presentday = Utils.GetFormValues("txt_presentday");
            //奖金
            string[] moneyaward = Utils.GetFormValues("txt_moneyaward");
            //饭补
            string[] mealreplenish = Utils.GetFormValues("txt_mealreplenish");
            //话补
            string[] phonereplenish = Utils.GetFormValues("txt_phonereplenish");
            //油补
            string[] gasolinereplenish = Utils.GetFormValues("txt_gasolinereplenish");
            //加班费
            string[] overtime = Utils.GetFormValues("txt_overtime");
            //迟到
            string[] belate = Utils.GetFormValues("txt_belate");
            //病假
            string[] fallill = Utils.GetFormValues("txt_fallill");
            //事假
            string[] matter = Utils.GetFormValues("txt_matter");
            //行政处罚
            string[] administrationpunish = Utils.GetFormValues("txt_administrationpunish");
            //业务罚款
            string[] businesspunish = Utils.GetFormValues("txt_businesspunish");
            //欠款
            string[] arrearage = Utils.GetFormValues("txt_arrearage");
            //应发工资
            string[] should = Utils.GetFormValues("txt_should");
            //五险
            string[] insure = Utils.GetFormValues("txt_insure");
            //社保
            string[] societyinsure = Utils.GetFormValues("txt_societyinsure");
            //实发工资
            string[] reality = Utils.GetFormValues("txt_reality");

            IList<MWageInfo> addls = new List<MWageInfo>();


            for (int i = 0; i < count; i++)
            {
                MWageInfo model = new MWageInfo();
                model.ZhiWei = position[i];
                model.XingMing = name[i];
                model.GangWeiGongZi = Utils.GetDecimal(postWage[i], 0);
                model.QuanQinJiang = Utils.GetDecimal(presentday[i], 0);
                model.JiangJin = Utils.GetDecimal(moneyaward[i], 0);
                model.FanBu = Utils.GetDecimal(mealreplenish[i], 0);
                model.HuaBu = Utils.GetDecimal(phonereplenish[i], 0);
                model.YouBu = Utils.GetDecimal(gasolinereplenish[i], 0);
                model.JiaBanFei = Utils.GetDecimal(overtime[i], 0);
                model.ChiDao = Utils.GetDecimal(belate[i], 0);
                model.BingJia = Utils.GetDecimal(fallill[i], 0);
                model.ShiJia = Utils.GetDecimal(matter[i], 0);
                model.XingZhengFaKuan = Utils.GetDecimal(administrationpunish[i], 0);
                model.YeWuFaKuan = Utils.GetDecimal(businesspunish[i], 0);
                model.QianKuan = Utils.GetDecimal(arrearage[i], 0);
                model.YingFaGongZi = Utils.GetDecimal(should[i], 0);
                model.WuXian = Utils.GetDecimal(insure[i], 0);
                model.SheBao = Utils.GetDecimal(societyinsure[i], 0);
                model.ShiFaGongZi = Utils.GetDecimal(reality[i], 0);
                if (position[i].Length > 0 ||
                name[i].Length > 0 ||
                postWage[i].Length > 0 ||
                presentday[i].Length > 0 ||
                moneyaward[i].Length > 0 ||
                mealreplenish[i].Length > 0 ||
                phonereplenish[i].Length > 0 ||
                gasolinereplenish[i].Length > 0 ||
                overtime[i].Length > 0 ||
                belate[i].Length > 0 ||
                fallill[i].Length > 0 ||
                matter[i].Length > 0 ||
                administrationpunish[i].Length > 0 ||
                businesspunish[i].Length > 0 ||
                arrearage[i].Length > 0 ||
                should[i].Length > 0 ||
                insure[i].Length > 0 ||
                societyinsure[i].Length > 0 ||
                reality[i].Length > 0)
                {
                    addls.Add(model);
                }
            }
            if (
            new EyouSoft.BLL.AdminCenterStructure.PersonnelInfo().SetWages(
                CurrentUserCompanyID,
               Utils.GetInt(Utils.GetFormValue(sel_Year.UniqueID)),
                 Utils.GetInt(Utils.GetFormValue(sel_Month.UniqueID)),
                SiteUserInfo.ID,
                addls)
                )
            {
                MessageBox.ResponseScript(this,
                    string.Format("; alert('保存成功');location.href='/administrativeCenter/attendanceManage/PersonnelSalary.aspx?Year={0}&Month={1}';",
                    Utils.GetFormValue(sel_Year.UniqueID),
                    Utils.GetFormValue(sel_Month.UniqueID)));
            }
            else
            {
                MessageBox.ShowAndReturnBack(this, "操作失败", 1);
            }
        }


    }
}
