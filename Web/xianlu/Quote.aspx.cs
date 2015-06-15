using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Eyousoft.Common;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.xianlu
{
    /// <summary>
    /// 模块名称:线路信息列表我要报价弹出页面
    /// 创建时间:2011-01-12
    /// 创建人:lixh
    /// </summary>
    public partial class Quote : BackPage
    {
        #region Private Mebers
        protected int PageSize = 20;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        #endregion

        EyouSoft.Model.RouteStructure.QuoteTeamInfo ModelRouteTeaminfo = null;
        EyouSoft.BLL.RouteStructure.Quote BllQuote = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 删除报价信息
            string action = Utils.GetQueryStringValue("action");
            if (action == "delete")
            {
                string QuoteID = Utils.GetQueryStringValue("Majorid");
                if (!string.IsNullOrEmpty(QuoteID) && Utils.GetInt(QuoteID) > 0)
                {
                    this.DelQouteList(Utils.GetInt(QuoteID));
                }
            }
            #endregion

            #region 初始化报价信息
            if (action == "update")
            {
                this.InitBindQuoteList();
                this.hideType.Value = "update";                
                string QuoteID = Utils.GetQueryStringValue("QuoteID");                     
                if (!string.IsNullOrEmpty(QuoteID) && Utils.GetInt(QuoteID) > 0)
                {
                    BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
                    ModelRouteTeaminfo = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();
                    ModelRouteTeaminfo = BllQuote.GetQuoteInfo(Utils.GetInt(QuoteID));
                    if (ModelRouteTeaminfo != null)
                    {
                        //询价单位编号
                        this.hidCustId.Value = ModelRouteTeaminfo.QuoteUnitsId.ToString();
                        //询价单位
                        this.Txt_Inquiry.Value = ModelRouteTeaminfo.QuoteUnitsName;
                        //联系人
                        this.Txt_Contact.Value = ModelRouteTeaminfo.ContactName;
                        //联系电话
                        this.Txt_TelPhone.Value = ModelRouteTeaminfo.ContactTel;
                        //预计出团时间
                        this.Txt_GroupStarTime.Value = ModelRouteTeaminfo.TmpLeaveDate.ToString("yyyy-MM-dd");
                        //人数
                        this.Txt_Numbers.Value = ModelRouteTeaminfo.PeopleNum.ToString();
                        //客人要求
                        this.ProjectControl.SetList = ModelRouteTeaminfo.Requirements;
                        this.ProjectControl.SetDataList();
                        //价格组成
                        this.PriceControl1.SetList = ModelRouteTeaminfo.Services;
                        this.PriceControl1.TotalAmount = ModelRouteTeaminfo.SelfQuoteSum;
                        //备注
                        this.Txt_RemarksBottom.Value = ModelRouteTeaminfo.Remark;                       
                    }
                }
                ModelRouteTeaminfo = null;
                BllQuote = null;
            }

            if(!this.Page.IsPostBack)
            {
                this.InitBindQuoteList();
                #region 计调员
                int areaid = Utils.GetInt(Utils.GetQueryStringValue("areaid"));
                if (areaid > 0)
                {
                    EyouSoft.BLL.CompanyStructure.Area area = new EyouSoft.BLL.CompanyStructure.Area();
                    EyouSoft.Model.CompanyStructure.Area marea = area.GetModel(areaid);
                    if (marea != null)
                    {
                        ddl_Oprator.DataSource = marea.AreaUserList;
                        ddl_Oprator.DataTextField = "ContactName";
                        ddl_Oprator.DataValueField = "userid";
                        ddl_Oprator.DataBind();
                    }
                }
                #endregion
            }
            #endregion     
     
        }

        #region  删除报价信息
        protected void DelQouteList(int QouteID)
        {
            bool IsTrue = false;
            BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
            IsTrue = BllQuote.DeleteTourTeamQuote(QouteID);
            if (IsTrue)
            {
                Response.Clear();
                Response.Write("1");
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write("No");
                Response.End();
            }
            BllQuote = null;
        }
        #endregion

        #region 绑定报价列表
        protected void InitBindQuoteList()
        {
            BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
            int RouteID = Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("RouteId"));
            if (RouteID > 0)
            {
                this.Ctp_QuoteList.DataSource = BllQuote.GetQuotesTeam(SiteUserInfo.ID, RouteID, PageSize, PageIndex, ref RecordCount);
                this.Ctp_QuoteList.DataBind();
            }
            BllQuote = null;
        }
        #endregion

        #region 给实体赋值
        protected bool SetQuoteTeamValues()
        {
            bool result = true;
            ModelRouteTeaminfo = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();
            //线路编号
            ModelRouteTeaminfo.RouteId = Utils.GetInt(Utils.GetQueryStringValue("RouteId"));
            //报价人编号
            ModelRouteTeaminfo.OperatorId = SiteUserInfo.ID;                        

            //询价单位
            if (string.IsNullOrEmpty(Utils.GetString(Utils.GetFormValue(this.Txt_Inquiry.UniqueID), "")))
            {
                SetErrorMsg(false, "请填写询价单位!");
                return false;
            }           
            ModelRouteTeaminfo.QuoteUnitsName = Utils.GetString(Utils.GetFormValue(this.Txt_Inquiry.UniqueID), "");
            ModelRouteTeaminfo.QuoteUnitsId = Utils.GetInt(Utils.GetFormValue(this.hidCustId.UniqueID));

            //联系人姓名
            ModelRouteTeaminfo.ContactName = Utils.GetString(Utils.GetFormValue(this.Txt_Contact.UniqueID), "");
            //联系人电话
            ModelRouteTeaminfo.ContactTel = Utils.GetString(Utils.GetFormValue(this.Txt_TelPhone.UniqueID), "");

            //预计出团时间
            if (string.IsNullOrEmpty(Utils.GetString(Utils.GetFormValue(this.Txt_GroupStarTime.UniqueID), "")))
            {
                SetErrorMsg(false, "请填写预计出团时间!");
                return false;
            }
            ModelRouteTeaminfo.TmpLeaveDate = Convert.ToDateTime(Utils.GetString(Utils.GetFormValue(this.Txt_GroupStarTime.UniqueID), ""));

            //人数
            if (Utils.GetInt(Utils.GetFormValue(this.Txt_Numbers.UniqueID)) <= 0)
            {
                SetErrorMsg(false, "请填写大于0的整数!");
                return false;
            }
            else
                ModelRouteTeaminfo.PeopleNum = Utils.GetInt(Utils.GetFormValue(this.Txt_Numbers.UniqueID));

            //客人要求
            ModelRouteTeaminfo.Requirements = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
            ModelRouteTeaminfo.Requirements = this.ProjectControl.GetDataList();

            //备注
            ModelRouteTeaminfo.Remark = this.Txt_RemarksBottom.Value.ToString();

            //价格组成
            ModelRouteTeaminfo.Services = new List<EyouSoft.Model.TourStructure.TourTeamServiceInfo>();
            ModelRouteTeaminfo.Services = this.PriceControl1.GetList;

            //我社报价总金额
            ModelRouteTeaminfo.SelfQuoteSum = this.PriceControl1.TotalAmount;

            //报价发布时间
            ModelRouteTeaminfo.CreateTime = System.DateTime.Now;
            return result;
        }
        #endregion

        #region 提示操作信息方法
        /// <summary>
        /// 提示提交信息
        /// </summary>
        /// <param name="IsSuccess">true 执行成功 flase 执行失败</param>
        /// <param name="Msg">提示信息</param>
        private void SetErrorMsg(bool isSuccess, string msg)
        {
            var s = "{{isSuccess:{0},errMsg:'{1}'}}";
            Response.Clear();
            Response.Write(string.Format(s, isSuccess.ToString().ToLower(), msg));
            Response.End();
        }
        #endregion

        #region 设置弹窗页面
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }
        #endregion

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
            ModelRouteTeaminfo = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();

            if (SetQuoteTeamValues())
            {
                string ActionType = Utils.GetFormValue(this.hideType.UniqueID);
                if (ActionType == "update")
                {
                    ModelRouteTeaminfo.QuoteId = Utils.GetInt(Utils.GetQueryStringValue("QuoteID"));
                    if (BllQuote.UpdateTourTeamQuote(ModelRouteTeaminfo) > 0)
                        Utils.ShowAndRedirect("报价信息修改成功!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                    else
                        Utils.ShowAndRedirect("报价信息修改失败!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                }
                else
                {
                    //提交报价信息
                    if (BllQuote.InsertTourTeamQuote(ModelRouteTeaminfo) > 0)
                        Utils.ShowAndRedirect("提交报价成功!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                    else
                        Utils.ShowAndRedirect("提交报价失败!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                }
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            BllQuote = new EyouSoft.BLL.RouteStructure.Quote();
            ModelRouteTeaminfo = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();

            #region 提交报价信息
            SetQuoteTeamValues();          
            #endregion

            #region 报价写入团队计划
            EyouSoft.BLL.TourStructure.Tour Tour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourTeamInfo TourInfo = new EyouSoft.Model.TourStructure.TourTeamInfo();                                   
           
            //计调员信息实体
            TourInfo.Coordinator = new EyouSoft.Model.TourStructure.TourCoordinatorInfo();
            //计调员编号
            TourInfo.Coordinator.CoordinatorId = Utils.GetInt(Request.Form[ddl_Oprator.UniqueID]);            

            //预计出团时间
            if (Utils.GetDateTimeNullable(Utils.GetFormValue(this.Txt_GroupStarTime.UniqueID)) == null)
            {
                SetErrorMsg(false, "请填写预计出团时间!");
                return;
            }
            TourInfo.LDate = Convert.ToDateTime(Utils.GetString(Utils.GetFormValue(this.Txt_GroupStarTime.UniqueID), ""));

            //人数
            if (Utils.GetInt(Utils.GetFormValue(this.Txt_Numbers.UniqueID.Trim())) <= 0)
            {
                SetErrorMsg(false,"请填写大于0的整数!");
                return;
            }
            else
            TourInfo.PlanPeopleNumber = Utils.GetInt(Utils.GetFormValue(this.Txt_Numbers.UniqueID.Trim()));

            //我社报价总金额            
            TourInfo.TotalAmount = this.PriceControl1.TotalAmount;

            //报价发布时间
            TourInfo.CreateTime = System.DateTime.Now;

            if (new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTeamNumberOfPeople(SiteUserInfo.CompanyID) == EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
            {
                TourInfo.TourTeamUnit = new EyouSoft.Model.TourStructure.MTourTeamUnitInfo()
                {
                    NumberCr = TourInfo.PlanPeopleNumber,
                    NumberEt = 0,
                    NumberQp = 0,
                    UnitAmountCr = 0,
                    UnitAmountEt = 0,
                    UnitAmountQp = 0
                };
            }
            else
            {
                TourInfo.TourTeamUnit = null;
            }

            TourInfo.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.团队计划; 

            int RouteID = Utils.GetInt(Request.QueryString["RouteId"]);
            var routeinfo = new EyouSoft.BLL.RouteStructure.Route().GetRouteInfo(RouteID);
            if (routeinfo != null)
            {
                TourInfo.ReleaseType = routeinfo.ReleaseType;
                TourInfo.AreaId = routeinfo.AreaId;
                TourInfo.RouteName = routeinfo.RouteName;
                TourInfo.CompanyId = SiteUserInfo.CompanyID;
                TourInfo.OperatorId = SiteUserInfo.ID;
                TourInfo.SellerId = SiteUserInfo.ID;
                TourInfo.RouteId = RouteID;
                TourInfo.BuyerCId = Utils.GetInt(Utils.GetFormValue(this.hidCustId.UniqueID));
                TourInfo.BuyerCName = Utils.GetFormValue(this.Txt_Inquiry.UniqueID);
                //线路天数
                TourInfo.TourDays = routeinfo.RouteDays;
                //团号
                TourInfo.TourCode = TourCode.Value;

                //价格组成
                TourInfo.Services = this.PriceControl1.GetList;

                if (TourInfo.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    TourInfo.TourQuickInfo = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo()
                    {
                        Remark = routeinfo.RouteQuickInfo.Remark,
                        QuickPlan = routeinfo.RouteQuickInfo.QuickPlan,
                        Service = routeinfo.RouteQuickInfo.Service
                    };
                }
                else
                {
                    TourInfo.TourNormalInfo = new EyouSoft.Model.TourStructure.TourTeamNormalPrivateInfo();
                    TourInfo.TourNormalInfo.Plans = routeinfo.RouteNormalInfo.Plans;
                    TourInfo.TourNormalInfo.BuHanXiangMu = routeinfo.RouteNormalInfo.BuHanXiangMu;
                    TourInfo.TourNormalInfo.ErTongAnPai = routeinfo.RouteNormalInfo.ErTongAnPai;
                    TourInfo.TourNormalInfo.GouWuAnPai = routeinfo.RouteNormalInfo.GouWuAnPai;
                    TourInfo.TourNormalInfo.NeiBuXingXi = routeinfo.RouteNormalInfo.NeiBuXingXi;
                    TourInfo.TourNormalInfo.Plans = routeinfo.RouteNormalInfo.Plans;
                    TourInfo.TourNormalInfo.WenXinTiXing = routeinfo.RouteNormalInfo.WenXinTiXing;
                    TourInfo.TourNormalInfo.ZhuYiShiXiang = routeinfo.RouteNormalInfo.ZhuYiShiXiang;
                    TourInfo.TourNormalInfo.ZiFeiXIangMu = routeinfo.RouteNormalInfo.ZiFeiXIangMu;
                }             
            }

            if (Utils.GetFormValue(this.hideType.UniqueID) == "update")
            {
                //报价编号
                ModelRouteTeaminfo.QuoteId = Utils.GetInt(Utils.GetQueryStringValue("QuoteID"));
                int QuoteID = BllQuote.UpdateTourTeamQuote(ModelRouteTeaminfo);
                if (QuoteID > 0)
                {
                    TourInfo.QuoteId = QuoteID;
                    if (Tour.InsertTeamTourInfo(TourInfo) > 0)
                    {
                        Utils.ShowAndRedirect("报价信息提交成功!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                    }
                }
                else
                {
                    Utils.ShowAndRedirect("报价信息提交失败!", "/xianlu/Quote.aspx?RouteID=" + Utils.GetQueryStringValue("RouteId") + "&Areaid=" + Utils.GetQueryStringValue("Areaid"));
                }
            }
            else
            {
                int QuoteId = BllQuote.InsertTourTeamQuote(ModelRouteTeaminfo);
                if (QuoteId > 0)
                {
                    TourInfo.QuoteId = QuoteId;
                    int insertTourResult = Tour.InsertTeamTourInfo(TourInfo);
                    if (insertTourResult > 0)
                    {
                        //提示报价完成操作成功
                        Utils.ShowAndRedirect("报价完成操作成功!", "/xianlu/Quote.aspx?areaid=" + Utils.GetQueryStringValue("areaid") + "&RouteID=" + Utils.GetQueryStringValue("RouteId"));
                    }
                }
                else
                {
                    //提示报价完成操作失败
                    Utils.ShowAndRedirect("报价完成操作失败!", "/xianlu/Quote.aspx?areaid=" + Utils.GetQueryStringValue("areaid") + "&RouteID=" + Utils.GetQueryStringValue("RouteId"));
                }
            }

            #endregion
        }


    }
}
