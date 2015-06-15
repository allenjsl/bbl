/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：QuickAdd.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\QuickAdd.aspx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:10:31
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common;
using System.Text.RegularExpressions;
using EyouSoft.BLL.CompanyStructure;

using Common.Enum;

namespace Web.sanping
{
    public partial class quickAdd : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 报价标准
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> sinfo = null;
        /// <summary>
        /// 默认为0，添加，1.修改,2.复制
        /// </summary>
        public int actType = 0;
        /// <summary>
        /// 操作人
        /// </summary>
        public string hdOprator = "";

        protected string TrafficStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (CheckGrant(TravelPermission.散拼计划_散拼计划_栏目))
                {
                    this.Page.Title = "散拼计划_散拼计划";
                }
                else
                {
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_栏目, false);
                }
                #region 绑定城市线路区域和价格列表
                BindArea();
                BindCity(0);
                BindPriceList();
                if (string.IsNullOrEmpty(Utils.GetQueryStringValue("act")))
                {
                    TrafficStr = GetTrafficList(null);
                }
                #endregion
                //createTea1.setGuiZe("start=01/11/2011&end=01/13/2011&type=1&data=");

                createTea1.companyid = CurrentUserCompanyID;
                /////////////////修改2011.1.15.田想兵 begin/////////////////////////
                string id = Utils.GetQueryStringValue("id");
                if (Request.QueryString["act"] == "update")
                {
                    actType = 1;
                    BindAllInfo(id);

                }

                //////////////////end//////////////////////////
                /////////////////复制2011.1.15.田想兵 begin/////////////////////////

                if (Request.QueryString["act"] == "copy")
                {
                    actType = 2;
                    BindAllInfo(id);
                }
                //////////////////end//////////////////////////

                BindCustomers();
            }
        }

        //////////////////end//////////////////////////
        #region//////////////出港城市///////////////////////
        /// <summary>
        /// 常用城市
        /// by 田想兵 2011.4.6
        /// </summary>
        /// <param name="cityId"></param>
        void BindCity(int cityId)
        {
            EyouSoft.BLL.CompanyStructure.City bllcity = new City();
            IList<EyouSoft.Model.CompanyStructure.City> listcity = bllcity.GetList(CurrentUserCompanyID, null, true);
            ddl_city.DataSource = listcity;
            ddl_city.DataTextField = "CityName";
            ddl_city.DataValueField = "id";
            ddl_city.DataBind();
            ddl_city.SelectedItem.Selected = false;

            if (ddl_city.Items.FindByValue(cityId.ToString()) != null)
            {
                ddl_city.Items.FindByValue(cityId.ToString()).Selected = true;
            }
        }
        #endregion

        /// <summary>
        /// 绑定所有信息    用于修改或复制
        /// </summary>
        /// <returns></returns>
        void BindAllInfo(string id)
        {
            #region 基础信息绑定
            selectXl.Id = "1";
            selectXl.Name = "";
            selectXl.Bind();
            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourInfo binfo = new EyouSoft.Model.TourStructure.TourInfo();
            binfo = (EyouSoft.Model.TourStructure.TourInfo)tour.GetTourInfo(id.ToString());
            if (binfo.Coordinator != null)
                hdOprator = binfo.Coordinator.CoordinatorId.ToString();
            if (ddl_area.Items.FindByValue(binfo.AreaId.ToString()) != null)
            {
                ddl_area.SelectedItem.Selected = false;
                ddl_area.Items.FindByValue(binfo.AreaId.ToString()).Selected = true;
            }
            selectXl.Name = binfo.RouteName;
            selectXl.Id = binfo.RouteId.ToString();
            txt_Days.Text = binfo.TourDays.ToString();
            //txt_pepoleNum.Text = binfo.PlanPeopleNumber.ToString();
            TrafficStr = GetTrafficList(binfo.TourTraffic);
            if (actType == 1)
            {
                trgz.Visible = false;
                trteamNum.Visible = true;
                txt_teamNum.Value = binfo.TourCode;
            }
            txt_startTraffic.Text = binfo.LTraffic;
            txt_endTraffic.Text = binfo.RTraffic;
            txt_remark.Text = binfo.TourQuickInfo.Remark;
            DiJieControl1.SetList = binfo.LocalAgencys;
            #region 绑定价格信息
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = binfo.PriceStandards;


            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new CompanyCustomStand();

            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            int kkk = list.Count;
            //for (int i = 0; i < listStand.Count; i++)
            //{
            //    for (int j = 0; j < listStand[i].CustomerLevels.Count; j++)
            //    { 
            //        //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
            //        var vn= list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
            //        if(vn!=null)
            //        listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
            //    }
            //}

            //    sinfo = listStand;

            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> plist = new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();
            plist = binfo.PriceStandards;
            for (int i = 0; i < listStand.Count; i++)
            {
                int cc = listStand[i].CustomerLevels.Count;
                for (int j = 0; j < list.Count; j++)
                {

                    //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
                    if (cc > j)
                    {
                        var vn = list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
                        if (vn != null)
                            listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
                        else
                        {
                            //plist[i].CustomerLevels.RemoveAt(j);
                            listStand[i].CustomerLevels[j].LevelId = 0;
                        }

                    }
                    var xn = listStand[i].CustomerLevels.Where(x => x.LevelId == list[j].Id).FirstOrDefault();
                    if (xn == null)
                        listStand[i].CustomerLevels.Add(new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo() { LevelId = list[j].Id, LevelType = list[j].LevType, LevelName = list[j].CustomStandName, AdultPrice = 0, ChildrenPrice = 0 });
                }
            }
            sinfo = plist;
            #endregion
            //List<EyouSoft.Model.TourStructure.TourPlanInfo> planInfo = binfo.TourNormalInfo.Plans.ToList();
            //xingcheng1.Bind(planInfo);
            //ConProjectControl1.SetList = binfo.TourNormalInfo.Services;
            //txt_noProject.Value = binfo.TourNormalInfo.BuHanXiangMu;
            //txt_buy.Value = binfo.TourNormalInfo.GouWuAnPai;
            //txt_child.Value = binfo.TourNormalInfo.ErTongAnPai;
            //txt_owner.Value = binfo.TourNormalInfo.ZiFeiXIangMu;
            //txt_Note.Value = binfo.TourNormalInfo.ZhuYiShiXiang;
            //txt_Reminded.Value = binfo.TourNormalInfo.WenXinTiXing;
            txt_xinchen.Text = binfo.TourQuickInfo.QuickPlan;
            txt_fuwu.Text = binfo.TourQuickInfo.Service;
            #region 附件
            if (binfo.Attachs.Count > 0)
            {
                if (binfo.Attachs[0].FilePath.Length > 1)
                {
                    pnlFile.Visible = true;
                    hypFilePath.NavigateUrl = binfo.Attachs[0].FilePath;
                    hd_img.Value = binfo.Attachs[0].FilePath;
                }
            }
            #endregion
            ddl_city.SelectedItem.Selected = false;

            if (ddl_city.Items.FindByValue(binfo.TourCityId.ToString()) != null)
            {
                ddl_city.Items.FindByValue(binfo.TourCityId.ToString()).Selected = true;
            }
            #endregion
        }
        /// <summary>
        /// 绑定区域
        /// </summary>
        void BindArea()
        {

            EyouSoft.BLL.CompanyStructure.Area area = new EyouSoft.BLL.CompanyStructure.Area(SiteUserInfo);
            int userid = SiteUserInfo.ID;
            IList<EyouSoft.Model.CompanyStructure.Area> list = new List<EyouSoft.Model.CompanyStructure.Area>();

            if (Request.QueryString["act"] == "update")
            {
                list = area.GetAreas();
            }
            else
            {
                list = area.GetAreaList(userid);
            }
            ddl_area.Items.Add(new ListItem("-请选择-", "-1"));
            ddl_area.Items.FindByValue("-1").Selected = true;
            foreach (var v in list)
            {
                ddl_area.Items.Add(new ListItem(v.AreaName, v.Id.ToString()));
            }
        }
        /// <summary>
        /// 获取关联交通
        /// </summary>
        /// <returns></returns>
        protected string GetTrafficList(IList<int> selectList)
        {
            EyouSoft.BLL.PlanStruture.PlanTrffic BLL = new EyouSoft.BLL.PlanStruture.PlanTrffic(SiteUserInfo);
            EyouSoft.Model.PlanStructure.TrafficInfo SearchModel = new EyouSoft.Model.PlanStructure.TrafficInfo();
            //SearchModel.IsDelete = false;
            IList<EyouSoft.Model.PlanStructure.TrafficInfo> list = BLL.GetTrafficList(null, SiteUserInfo.CompanyID);
            StringBuilder sb = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (selectList != null && selectList.Count > 0)
                    {
                        if (selectList.Contains(item.TrafficId))
                        {
                            sb.AppendFormat("<li><a href=\"javascript:void(0);\" data-trafficId='{0}' class=\"select\"><span>", item.TrafficId);
                        }
                        else
                        {
                            sb.AppendFormat("<li><a href=\"javascript:void(0);\" data-trafficId='{0}'><span>", item.TrafficId);
                        }
                    }
                    else
                    {
                        sb.AppendFormat("<li><a href=\"javascript:void(0);\" data-trafficId='{0}'><span>", item.TrafficId);
                    }
                    if (item.travelList != null && item.travelList.Count > 0)
                    {
                        for (int i = 0; i < item.travelList.Count; i++)
                        {
                            if (i != item.travelList.Count - 1)
                            {
                                sb.AppendFormat("[{0}、{1}]-", item.travelList[i].LCityName, item.travelList[i].RCityName);
                            }
                            else
                            {
                                sb.AppendFormat("[{0}、{1}]", item.travelList[i].LCityName, item.travelList[i].RCityName);
                            }
                        }
                        sb.Append("<br/>[");
                        for (int i = 0; i < item.travelList.Count; i++)
                        {
                            sb.AppendFormat("{0}", i != item.travelList.Count - 1 ? item.travelList[i].FilghtNum + "/" : item.travelList[i].FilghtNum);
                        }
                        sb.Append("]");
                    }
                    else
                    {
                        sb.AppendFormat("{0}<br/>&nbsp;&nbsp", item.TrafficName);
                    }
                    sb.Append("</span></a></li>");
                }
            }
            return sb.ToString();


        }

        /// <summary>
        /// 报价标准
        /// </summary>
        public string BindPriceList()
        {

            StringBuilder sb = new StringBuilder();
            EyouSoft.BLL.CompanyStructure.CompanyPriceStand price = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();

            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> pricelist = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();
            pricelist = price.GetPriceStandByCompanyId(CurrentUserCompanyID);
            foreach (var v in pricelist)
            {
                sb.Append("<option value=\"" + v.Id.ToString() + "|" + v.PriceStandName + "\">" + v.PriceStandName + "</option>");
            }
            pricelist = null;
            return sb.ToString();
        }
        /// <summary>
        /// 绑定客户等级
        /// </summary>
        void BindCustomers()
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new CompanyCustomStand();

            list = bll.GetCustomStandByCompanyId(CurrentUserCompanyID);
            rpt_Customer.DataSource = list;
            rpt_Customer.DataBind();

        }

        /// <summary>
        /// 已选交通
        /// </summary>
        /// <returns></returns>
        protected IList<int> GetSelectTraffic()
        {
            IList<int> list = new List<int>();
            int[] arr = Utils.GetIntArray(Utils.GetFormValue(hidSelectTtraffic.UniqueID), ",");
            if (arr != null && arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    list.Add(arr[i]);
                }
            }
            return list;
        }

        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            #region 基础数据读取

            if (Request.QueryString["act"] == "update")
            {
                actType = 1;
            }
            if (Request.QueryString["act"] == "copy")
            {
                actType = 2;
            }
            IList<EyouSoft.Model.CompanyStructure.CustomStand> listcus = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.BLL.CompanyStructure.CompanyCustomStand bllCom = new CompanyCustomStand();

            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourInfo info = new EyouSoft.Model.TourStructure.TourInfo();
            if (actType == 1)
            {
                info = (EyouSoft.Model.TourStructure.TourInfo)tour.GetTourInfo(Utils.GetQueryStringValue("id"));
            }
            listcus = bllCom.GetCustomStandByCompanyId(CurrentUserCompanyID);

            int kkk = listcus.Count;

            string[] st = Utils.GetFormValues("eat");
            info.RouteId = Utils.GetInt(selectXl.Id);
            info.AreaId = Utils.GetInt(Utils.GetFormValue(ddl_area.UniqueID));
            info.RouteName = selectXl.Name;
            info.TourDays = EyouSoft.Common.Utils.GetInt(Utils.GetFormValue(txt_Days.UniqueID));
            info.PlanPeopleNumber = 1;//EyouSoft.Common.Utils.GetInt(Utils.GetFormValue(txt_pepoleNum.UniqueID));
            info.TourTraffic = GetSelectTraffic();
            EyouSoft.Model.TourStructure.TourCreateRuleInfo rule = new EyouSoft.Model.TourStructure.TourCreateRuleInfo();
            #region 建团规则
            if (actType == 0 || actType == 2)
            {
                string gz = createTea1.getGuiZe();
                rule.EDate = EyouSoft.Common.Utils.GetDateTime(Utils.GetFromQueryStringByKey(gz, "end"));
                rule.SDate = EyouSoft.Common.Utils.GetDateTime(Utils.GetFromQueryStringByKey(gz, "start"));

                switch (Utils.GetFromQueryStringByKey(gz, "type"))
                {
                    case "1":
                        {
                            rule.Cycle = Utils.GetFromQueryStringByKey(gz, "data");
                            rule.Rule = EyouSoft.Model.EnumType.TourStructure.CreateTourRule.按星期;
                        } break;
                    case "2":
                        {
                            rule.Cycle = Utils.GetFromQueryStringByKey(gz, "data");
                            rule.Rule = EyouSoft.Model.EnumType.TourStructure.CreateTourRule.按天数;
                        } break;
                    case "3":
                        {
                            rule.SDate = null;
                            rule.EDate = null;
                            rule.Cycle = null;
                            rule.Rule = EyouSoft.Model.EnumType.TourStructure.CreateTourRule.按日期;
                        } break;

                }
                info.CreateRule = rule;
            }
            else
            {

            }
            #endregion
            info.LTraffic = txt_startTraffic.Text;
            info.RTraffic = txt_endTraffic.Text;
            IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> local;
            local = DiJieControl1.GetList;
            info.LocalAgencys = local;

            info.CompanyId = CurrentUserCompanyID;
            info.TourQuickInfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo();
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> pricelist = new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();
            string[] ddl_price = Utils.GetFormValues("ddl_price");
            for (int k = 0; k < ddl_price.Length; k++)
            {
                EyouSoft.Model.TourStructure.TourPriceStandardInfo price = new EyouSoft.Model.TourStructure.TourPriceStandardInfo();
                price.StandardId = Utils.GetInt(Utils.GetFormValues("ddl_price")[k].Split('|')[0]);
                price.StandardName = Utils.GetFormValues("ddl_price")[k].Split('|')[1];
                if (ddl_price[k] == "" || price.StandardId == 0)
                    continue;
                IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> listLevels = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();


                string[] crPrice = Utils.GetFormValues("txt_cr_price");


                for (int i = 0; i < kkk; i++)
                {
                    int levId = Utils.GetInt(Utils.GetFormValues("hd_cusStandId")[i]);
                    if (levId == 0)
                        continue;
                    EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo level = new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo();
                    level.AdultPrice = Utils.GetDecimal(Utils.GetFormValues("txt_cr_price")[i + kkk * k]);
                    level.ChildrenPrice = Utils.GetDecimal(Utils.GetFormValues("txt_rt_price")[i + kkk * k]);
                    level.LevelType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                    level.LevelName = Utils.GetFormValues("hd_cusStandName")[i];
                    level.LevelId = levId;
                    listLevels.Add(level);
                }
                price.CustomerLevels = listLevels;
                pricelist.Add(price);
            }
            IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> levelList = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();
            info.OperatorId = SiteUserInfo.ID;
            info.PriceStandards = pricelist;
            EyouSoft.Model.TourStructure.TourQuickPrivateInfo qinfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo();
            qinfo.QuickPlan = Utils.EditInputText(txt_xinchen.Text);
            qinfo.Service = Utils.EditInputText(txt_fuwu.Text);
            qinfo.Remark = Utils.GetFormValue(txt_remark.UniqueID);
            info.TourQuickInfo = qinfo;

            IList<EyouSoft.Model.TourStructure.TourAttachInfo> listAttachs = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
            EyouSoft.Model.TourStructure.TourAttachInfo attInfo = new EyouSoft.Model.TourStructure.TourAttachInfo();
            string fileAtt = "";
            string oldfileAtt = "";
            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(FileUpload1.PostedFile, "sanping", out fileAtt, out oldfileAtt))
            {
                attInfo.FilePath = fileAtt;
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "上传附件失败！");
                return;
            }
            if (attInfo.FilePath.Length < 1)
            {
                attInfo.FilePath = hd_img.Value;
            }
            listAttachs.Add(attInfo);

            info.Attachs = listAttachs;
            //info.TourDays = createTea1.getDays();
            info.Coordinator = new EyouSoft.Model.TourStructure.TourCoordinatorInfo();
            info.Coordinator.CoordinatorId = Utils.GetInt(Utils.GetFormValue("sel_oprator"));
            info.TourCityId = Utils.GetInt(Utils.GetFormValue(ddl_city.UniqueID));

            #endregion
            #region 添加
            if (actType == 0 || actType == 2)
            {
                info.ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick;
                info.Status = EyouSoft.Model.EnumType.TourStructure.TourStatus.正在收客;
                info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
                info.TicketStatus = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;
                IList<EyouSoft.Model.TourStructure.TourChildrenInfo> listChild = new List<EyouSoft.Model.TourStructure.TourChildrenInfo>();
                string[] childTeamNum = Utils.GetFormValue("hidToursNumbers").Split(',');
                string[] tourcodes = new string[childTeamNum.Length];
                for (int i = 0; i < childTeamNum.Length; i++)
                {
                    ///子团
                    EyouSoft.Model.TourStructure.TourChildrenInfo child = new EyouSoft.Model.TourStructure.TourChildrenInfo();
                    child.TourCode = childTeamNum[i].Split('}')[1];
                    child.LDate = Utils.GetDateTime(childTeamNum[i].Split('{')[0]);
                    listChild.Add(child);
                    tourcodes[i] = child.TourCode;
                }
                info.Childrens = listChild;

                if (!CheckGrant(TravelPermission.散拼计划_散拼计划_新增计划))
                {
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_新增计划, false);
                }

                IList<string> list_tourCode = tour.ExistsTourCodes(SiteUserInfo.CompanyID, null, tourcodes);
                if (list_tourCode.Count > 0)
                {
                    Response.Write("<script>alert('" + string.Join(",", list_tourCode.ToArray()) + "团号已经存在!');location.href=location.href;</script>");
                    return;
                }
                int ii = tour.InsertTourInfo(info);
                if (ii > 0)
                {
                    Response.Write("<script>alert('添加成功!');location.href='default.aspx';</script>");
                }
            }
            #endregion
            else
            #region 修改
            {
                if (!Utils.PlanIsUpdateOrDelete(info.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
                info.TourId = Request.QueryString["id"];
                info.Childrens = null;
                info.TourCode = txt_teamNum.Value;
                if (!CheckGrant(TravelPermission.散拼计划_散拼计划_修改计划))
                {
                    Utils.ResponseNoPermit(TravelPermission.散拼计划_散拼计划_修改计划, false);
                }

                IList<string> list_tourCode = tour.ExistsTourCodes(SiteUserInfo.CompanyID, info.TourId, info.TourCode);
                if (list_tourCode.Count > 0)
                {
                    Response.Write("<script>alert('团号已经存在!');location.href=location.href;</script>");
                    return;
                }
                int ii = tour.UpdateTourInfo(info);
                if (ii > 0)
                {
                    Response.Write("<script>alert('修改成功!');location.href='default.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('修改失败!');location.href=location.href;</script>");
                }
            }
            #endregion
        }
    }
    /// <summary>
    /// 客户等级
    /// </summary>
    public class Customers
    {
        public int c_id { get; set; }
        public string c_name { get; set; }
        public decimal cr_price { get; set; }
        public decimal et_price { get; set; }
        public Customers(int c_id, string c_name)
        {
            this.c_id = c_id;
            this.c_name = c_name;
        }
        public Customers(int c_id, string c_name, decimal cr_price, decimal et_price)
        {
            this.c_id = c_id;
            this.c_name = c_name;
            this.cr_price = cr_price;
            this.et_price = et_price;
        }
    }

}
