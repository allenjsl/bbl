using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Eyousoft.Common.Page;
using Common.Enum;
using EyouSoft.Common;

namespace Web.masterpage
{
    public partial class Back : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 外logo
        /// </summary>
        protected string BigLogo = "";
        protected BackPage backPage = null;
        /// <summary>
        /// 获取弹窗提醒间隔时间，单位毫秒
        /// </summary>
        protected int TanChuangTiXingInterval = 600000;

        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化backPage
            backPage = this.Page as BackPage;
            if (backPage == null)
            {
                throw new Exception("页面没有正确继承BackPage");
            }

            TanChuangTiXingInterval = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTanChuangTiXingInterval(backPage.CurrentUserCompanyID) * 1000;

            if (!Page.IsPostBack)
            {
                //初始化外Logo
                BigLogo = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetCompanyLogo(backPage.SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户);

                AutoPositionLinks();
                InitLinksByPermissions();
            }
        }

        /// <summary>
        /// 根据当前用户权限 ，初始化 可视的链接
        /// </summary>
        private void InitLinksByPermissions()
        {
            //线路产品库
            //线路管理
            if (backPage.CheckGrant(TravelPermission.线路产品库_线路产品库_栏目) == false)
            {
                divXianLu.Visible = false;
            }

            //散拼计划
            if (backPage.CheckGrant(TravelPermission.散拼计划_散拼计划_栏目) == false)
            {
                divSanPing.Visible = false;
            }

            #region 散客天天发
            if (backPage.CheckGrant(TravelPermission.散拼计划_散客天天发_栏目) == false)
            {
                liDayDay.Visible = false;
            }
            #endregion

            //团队计划
            bool teamGrant = backPage.CheckGrant(TravelPermission.团队计划_团队计划_栏目);
            if (!teamGrant)
            {
                liTeamPlan.Visible = false;
            }
            //询价
            bool teamPriceGrant = backPage.CheckGrant(TravelPermission.团队计划_组团社询价_栏目);
            if (!teamPriceGrant)
            {
                liTeamPrice.Visible = false;
            }
            //团队 上传报价
            bool teamQuoteGrant = backPage.CheckGrant(TravelPermission.团队计划_上传报价_栏目);
            if (!teamQuoteGrant)
            {
                liTourQuoteList.Visible = false;
            }
            //子栏目都没 那么隐藏团队 大栏目
            if (!teamGrant && !teamPriceGrant && !teamQuoteGrant)
            {
                divTeamPlan.Visible = false;
            }

            //单项服务
            if (backPage.CheckGrant(TravelPermission.单项服务_单项服务_栏目) == false)
            {
                divSingleServer.Visible = false;
            }

            //销售管理-订单中心
            bool orderGrant = backPage.CheckGrant(TravelPermission.销售管理_订单中心_栏目);
            if (orderGrant == false)
            {
                liOrderList.Visible = false;
            }

            //销售管理-销售收款
            bool saleGrant = backPage.CheckGrant(TravelPermission.销售管理_销售收款_栏目);
            if (saleGrant == false)
            {
                liSaleList.Visible = false;
            }

            //出票统计
            if (!backPage.CheckGrant(TravelPermission.机票管理_机票管理_出票统计))
            {
                liJiPiaotj.Visible = false;
            }
            //退票统计
            if (!backPage.CheckGrant(TravelPermission.机票管理_机票管理_退票统计))
            {
                linkTicketRefund.Visible = false;
            }

            //如果 都没有 销售管理-订单中心和销售管理-销售收款 两个 栏目权限，则隐藏销售管理大栏目
            if (orderGrant == false && saleGrant == false)
            {
                divSales.Visible = false;
            }

            //机票管理
            if (backPage.CheckGrant(TravelPermission.机票管理_机票管理_栏目) == false && backPage.CheckGrant(TravelPermission.机票管理_机票管理_退票统计) == false)
            {
                divJiPiao.Visible = false;
            }

            //供应商管理-地接
            bool areaConnectGrant = backPage.CheckGrant(TravelPermission.供应商管理_地接_栏目);
            if (areaConnectGrant == false)
            {
                liAreaConnect.Visible = false;
            }
            //供应商管理-票务
            bool ticketServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_票务_栏目);
            if (ticketServiceGrant == false)
            {
                liTicketService.Visible = false;
            }
            //供应商管理-酒店
            bool hotelServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_酒店_栏目);
            if (hotelServiceGrant == false)
            {
                liHotelService.Visible = false;
            }
            //供应商管理-餐馆
            bool estServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_餐馆_栏目);
            if (estServiceGrant == false)
            {
                liRestService.Visible = false;
            }
            //供应商管理-车队
            bool carServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_车队_栏目);
            if (carServiceGrant == false)
            {
                liCarService.Visible = false;
            }
            //供应商管理-景点
            bool areaServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_景点_栏目);
            if (areaServiceGrant == false)
            {
                liAreaService.Visible = false;
            }
            //供应商管理-购物
            bool shopServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_购物_栏目);
            if (shopServiceGrant == false)
            {
                liShopService.Visible = false;
            }
            //供应商管理-保险
            bool insuranceServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_保险_栏目);
            if (insuranceServiceGrant == false)
            {
                liInsuranceService.Visible = false;
            }
            //供应商管理-其它
            bool otherServiceGrant = backPage.CheckGrant(TravelPermission.供应商管理_其它_栏目);
            if (otherServiceGrant == false)
            {
                liOtherService.Visible = false;
            }
            //供应商管理-航空公司
            bool airLineGrant = backPage.CheckGrant(TravelPermission.供应商管理_航空公司栏目);
            if (airLineGrant == false)
            {
                liAirLine.Visible = false;
            }

            //如果 都没有 则隐藏供应商管理大栏目
            if (areaConnectGrant == false && ticketServiceGrant == false && hotelServiceGrant == false && estServiceGrant == false && carServiceGrant == false && areaServiceGrant == false && shopServiceGrant == false && insuranceServiceGrant == false && otherServiceGrant == false && airLineGrant == false)
            {
                divSupplier.Visible = false;
            }

            //客户关系管理-客户资料
            bool customerListGrant = backPage.CheckGrant(TravelPermission.客户关系管理_客户资料_栏目);
            if (customerListGrant == false)
            {
                liCustomerList.Visible = false;
            }
            //客户关系管理-客户服务
            //客户关系管理-客户服务-营销活动
            bool marketActiveGrant = backPage.CheckGrant(TravelPermission.客户关系管理_营销活动_栏目);
            //客户关系管理-客户服务-客户关怀
            bool customerCareGrant = backPage.CheckGrant(TravelPermission.客户关系管理_客户关怀_栏目);
            //客户关系管理-客户服务-质量管理
            bool customerVisitGrant = backPage.CheckGrant(TravelPermission.客户关系管理_质量管理_回访栏目)
                || backPage.CheckGrant(TravelPermission.客户关系管理_质量管理_投诉栏目);
            //如果都没有 营销活动，客户关怀，质量管理 的的栏目权限，则隐藏客户关系管理-客户服务链接
            if (marketActiveGrant == false && customerCareGrant == false && customerVisitGrant == false)
            {
                liMarketActive.Visible = false;
            }
            else//有
            {
                //默认将 营销活动，客户关怀，质量管理 的栏目中，
                //第一个有权限的栏目链接 作为 客户关系管理-客户服务 链接
                if (marketActiveGrant == true)//营销活动
                {
                    linkMarketActive.HRef = "/CRM/customerservice/MarketActive.aspx";
                }
                else if (customerCareGrant == true)//客户关怀
                {
                    linkMarketActive.HRef = "/CRM/customerservice/CustomerCare.aspx";
                }
                else if (customerVisitGrant == true)//质量管理
                {
                    //默认将 质量管理 中的 客户回访 ，客户投诉 ，
                    //第一个有权下的栏目链接 作为 质量管理的链接初始化
                    if (backPage.CheckGrant(TravelPermission.客户关系管理_质量管理_回访栏目) == true)
                    {
                        linkMarketActive.HRef = "/CRM/customerservice/CustomerVisit.aspx";
                    }
                    else if (backPage.CheckGrant(TravelPermission.客户关系管理_质量管理_投诉栏目) == true)
                    {
                        linkMarketActive.HRef = "/CRM/customerservice/CustomerComplaint.aspx";
                    }
                }
            }
            //客户关系管理-销售分析
            //客户关系管理-销售分析-人次统计
            bool personStatisticsGrant = backPage.CheckGrant(TravelPermission.客户关系管理_销售分析_人次统计栏目);
            //客户关系管理-销售分析-利润统计
            bool profitStatisticalGrant = backPage.CheckGrant(TravelPermission.客户关系管理_销售分析_利润统计栏目);
            //客户关系管理-销售分析-帐龄分析
            bool timeStatisticsGrant = backPage.CheckGrant(TravelPermission.客户关系管理_销售分析_帐龄分析栏目);
            //如果都没有 人次统计，利润统计，帐龄分析 的栏目权限，则隐藏 客户关系管理-销售分析 链接
            if (personStatisticsGrant == false && profitStatisticalGrant == false && timeStatisticsGrant == false)
            {
                liAreaList.Visible = false;
            }
            else//有
            {
                //默认将 人次统计，利润统计，帐龄分析 的栏目中，
                //第一个有权限的栏目链接 作为 客户关系管理-销售分析 链接
                if (personStatisticsGrant == true)
                {
                    linkAreaList.HRef = "/CRM/PersonStatistics/AreaList.aspx";
                }
                else if (profitStatisticalGrant == true)
                {
                    linkAreaList.HRef = "/CRM/ProfitStatistical/AreaList.aspx";
                }
                else if (timeStatisticsGrant == true)
                {
                    linkAreaList.HRef = "/CRM/TimeStatistics/AreaList.aspx";
                }
            }
            //客户关系管理-销售分析-销售统计
            bool sellStatGrant = backPage.CheckGrant(TravelPermission.客户关系管理_销售统计_栏目);
            if (sellStatGrant == false)
            {
                liSellStat.Visible = false;
            }

            //客户关系管理_返佣统计
            bool backMoneyStatGrant = backPage.CheckGrant(TravelPermission.客户关系管理_返佣统计_栏目);
            if (backMoneyStatGrant == false)
            {
                liBackMoney.Visible = false;
            }


            //如果都没有 客户资料，客户服务，销售分析 的栏目权限的话 则隐藏 客户关系管理 栏目
            if (liCustomerList.Visible == false && liMarketActive.Visible == false && liAreaList.Visible == false && sellStatGrant == false && backMoneyStatGrant == false)
            {
                divCRM.Visible = false;
            }

            //财务管理
            //财务管理-团队核算
            bool teamAccountGrant = backPage.CheckGrant(TravelPermission.财务管理_团队核算_栏目);
            if (teamAccountGrant == false)
            {
                liTeamAccount.Visible = false;
            }
            //财务管理-团款收入
            bool srTuanKuanListGrant = backPage.CheckGrant(TravelPermission.财务管理_团款收入_栏目);
            if (srTuanKuanListGrant == false)
            {
                liSrTuanKuanList.Visible = false;
            }
            //财务管理-杂费收入
            bool inComeGrant = backPage.CheckGrant(TravelPermission.财务管理_杂费收入_栏目);
            if (inComeGrant == false)
            {
                liIncome.Visible = false;
            }
            //财务管理-团款支出
            bool teamExpenditureGrant = backPage.CheckGrant(TravelPermission.财务管理_团款支出_栏目);
            if (teamExpenditureGrant == false)
            {
                liTeamExpend.Visible = false;
            }
            //财务管理-杂费支出
            bool otherExpenditureGrant = backPage.CheckGrant(TravelPermission.财务管理_杂费支出_栏目);
            if (otherExpenditureGrant == false)
            {
                liOtherExpend.Visible = false;
            }
            //财务管理-出纳登帐
            bool chuNaGrant = backPage.CheckGrant(TravelPermission.财务管理_出纳登帐_栏目);
            if (chuNaGrant == false)
            {
                liChuNa.Visible = false;
            }
            //财务管理-机票审核
            bool jiPiaoAuditGrant = backPage.CheckGrant(TravelPermission.财务管理_机票审核_栏目);
            if (jiPiaoAuditGrant == false)
            {
                liJipiaoAudit.Visible = false;
            }

            //财务管理-发票管理
            bool faPiaoGuanLiQX = backPage.CheckGrant(TravelPermission.财务管理_发票管理_栏目);
            if (!faPiaoGuanLiQX) liFaPiaoGuanLi.Visible = faPiaoGuanLiQX;

            //如果都没有 团队核算，团款收入，杂费收入，团款支出，杂费支出，出纳登帐，机票审核，发票管理
            //的栏目权限的话 则隐藏 财务管理 栏目
            if (teamAccountGrant == false && srTuanKuanListGrant == false && inComeGrant == false
                && teamExpenditureGrant == false && otherExpenditureGrant == false
                && chuNaGrant == false && jiPiaoAuditGrant == false && !faPiaoGuanLiQX)
            {
                divCaiWu.Visible = false;
            }

            //统计分析
            //统计分析-人次统计
            bool preAreaStatGrant = backPage.CheckGrant(TravelPermission.统计分析_人次统计_人次统计栏目);
            if (preAreaStatGrant == false)
            {
                liPreAreaStat.Visible = false;
            }
            //统计分析-员工业绩表
            bool empIncomeStatGrant = backPage.CheckGrant(TravelPermission.统计分析_员工业绩_员工业绩栏目);
            if (empIncomeStatGrant == false)
            {
                liEmpIncomeStat.Visible = false;
            }
            //统计分析-利润统计
            bool proAreaStatGrant = backPage.CheckGrant(TravelPermission.统计分析_利润统计_利润统计栏目);
            if (proAreaStatGrant == false)
            {
                liProAreaStat.Visible = false;
            }
            //统计分析-收入对帐单
            bool incAreaStatGrant = backPage.CheckGrant(TravelPermission.统计分析_收入对账单_收入对账单栏目);
            if (incAreaStatGrant == false)
            {
                liIncAreaStat.Visible = false;
            }
            //统计分析-支出对帐单
            bool outAreaStatGrant = backPage.CheckGrant(TravelPermission.统计分析_支出对账单_支出对账单栏目);
            if (outAreaStatGrant == false)
            {
                liOutAreaStat.Visible = false;
            }
            //统计分析-帐龄分析表
            bool ageAreaStatGrant = backPage.CheckGrant(TravelPermission.统计分析_帐龄分析_帐龄分析栏目);
            if (ageAreaStatGrant == false)
            {
                liAgeAreaStat.Visible = false;
            }
            //统计分析-现金流量表
            bool casDayStatGrant = backPage.CheckGrant(TravelPermission.统计分析_现金流量_现金流量栏目);
            if (casDayStatGrant == false)
            {
                liCasDayStat.Visible = false;
            }
            //统计分析-销售统计栏目
            bool soldStatGrant = backPage.CheckGrant(TravelPermission.统计分析_销售统计_销售统计栏目);
            if (soldStatGrant == false)
            {
                liSoldStat.Visible = false;
            }
            //统计分析-销售利润统计
            bool saleprofitGrant = backPage.CheckGrant(TravelPermission.统计分析_销售利润统计_销售利润统计栏目);
            if (saleprofitGrant == false)
            {
                liSellProfit.Visible = false;
            }
            //统计分析-区域销售栏目
            bool areaSoldStatGrant = backPage.CheckGrant(TravelPermission.统计分析_区域销售统计_区域销售栏目);
            if (areaSoldStatGrant == false)
            {
                liAreaSoldStat.Visible = false;
            }
            //统计分析-回款率分析栏目
            bool isHuiKuanLv = backPage.CheckGrant(TravelPermission.统计分析_回款率分析_回款率分析栏目);
            if (!isHuiKuanLv) liHuiKuanLv.Visible = false;
            bool isZhiChuZhangLing = backPage.CheckGrant(TravelPermission.统计分析_支出账龄分析_支出账龄分析栏目);
            if (!isZhiChuZhangLing) liZhiChuZhangLing.Visible = false;
            //如果都没有 人次统计，员工业绩表，利润统计，收入对帐单，支出对帐单，帐龄分析表，现金流量表，销售统计，区域销售统计、回款率分析、支出账龄分析
            //栏目权限的话，则隐藏  统计分析  栏目
            if (!preAreaStatGrant && !empIncomeStatGrant && !proAreaStatGrant && !incAreaStatGrant
                && !outAreaStatGrant && !ageAreaStatGrant && !casDayStatGrant && !soldStatGrant && !areaSoldStatGrant
                && !isHuiKuanLv && !isZhiChuZhangLing)
            {
                divStat.Visible = false;
            }

            //行政中心
            //行政中心-职务管理
            bool positionManageGrant = backPage.CheckGrant(TravelPermission.行政中心_职务管理_栏目);
            if (positionManageGrant == false)
            {
                liPositionManage.Visible = false;
            }
            //行政中心-人事档案
            bool personalFielsGrant = backPage.CheckGrant(TravelPermission.行政中心_人事档案_栏目);
            if (personalFielsGrant == false)
            {
                liPersonnelFiles.Visible = false;
            }
            //行政中心-考勤管理
            bool attendanceManageGrant = backPage.CheckGrant(TravelPermission.行政中心_考勤管理_栏目);
            if (attendanceManageGrant == false)
            {
                liAttendanceManage.Visible = false;
            }
            //行政中心-内部通讯录
            bool addressListGrant = backPage.CheckGrant(TravelPermission.行政中心_内部通讯录_栏目);
            if (addressListGrant == false)
            {
                liAddressList.Visible = false;
            }
            //行政中心-规章制度
            bool bylawGrant = backPage.CheckGrant(TravelPermission.行政中心_规章制度_栏目);
            if (bylawGrant == false)
            {
                liBylaw.Visible = false;
            }
            //行政中心-会议记录管理
            bool cahierManageGrant = backPage.CheckGrant(TravelPermission.行政中心_会议记录_栏目);
            if (cahierManageGrant == false)
            {
                liCahierManage.Visible = false;
            }
            //行政中心-劳动合同管理
            bool contractManageGrant = backPage.CheckGrant(TravelPermission.行政中心_劳动合同管理_栏目);
            if (contractManageGrant == false)
            {
                liContractManage.Visible = false;
            }
            //行政中心-固定资产管理
            bool fixedAssetsManageGrant = backPage.CheckGrant(TravelPermission.行政中心_固定资产管理_栏目);
            if (fixedAssetsManageGrant == false)
            {
                liFixedAssetsManage.Visible = false;
            }
            //行政中心-培训计划
            bool trainingPlanGrant = backPage.CheckGrant(TravelPermission.行政中心_培训计划_栏目);
            if (trainingPlanGrant == false)
            {
                liTrainingPlan.Visible = false;
            }

            //如果都没有 职务管理，人事档案，考勤管理，内部通讯录，规章制度，会议记录管理，劳动合同管理
            //,固定资产管理,培训计划
            //栏目权限的话，则隐藏  行政中心  栏目
            if (!positionManageGrant && !personalFielsGrant && !attendanceManageGrant
                && !addressListGrant && !bylawGrant && !cahierManageGrant
                && !contractManageGrant && !fixedAssetsManageGrant && !trainingPlanGrant)
            {
                divAdministrativeCenter.Visible = false;
            }

            //系统设置
            //系统设置-基础设置
            bool cityManageGrant = backPage.CheckGrant(TravelPermission.系统设置_基础设置_城市管理栏目);
            bool routeAreaGrant = backPage.CheckGrant(TravelPermission.系统设置_基础设置_线路区域栏目);
            bool priceStandGrant = backPage.CheckGrant(TravelPermission.系统设置_基础设置_报价标准栏目);
            bool customerLevelGrant = backPage.CheckGrant(TravelPermission.系统设置_基础设置_客户等级栏目);
            bool brandManageGrant = backPage.CheckGrant(TravelPermission.系统设置_基础设置_品牌管理栏目);

            //如果都没有 基础设置 子栏目 权限 ，则隐藏 基础设置 链接
            if (!cityManageGrant && !routeAreaGrant && !priceStandGrant
                && !customerLevelGrant && !brandManageGrant)
            {
                liCityManage.Visible = false;
            }
            else//有
            {
                //默认 将 城市管理，线路区域，报价标准，客户等级，品牌管理 栏目中
                //第一个有权限的子栏目链接 作为 基础设置 链接 初始化
                if (cityManageGrant == true)
                {
                    linkCityManage.HRef = "/systemset/basicinfo/CityManage.aspx";
                }
                else if (routeAreaGrant == true)
                {
                    linkCityManage.HRef = "/systemset/basicinfo/RouteArea.aspx";
                }
                else if (priceStandGrant == true)
                {
                    linkCityManage.HRef = "/systemset/basicinfo/PriceStand.aspx";
                }
                else if (customerLevelGrant == true)
                {
                    linkCityManage.HRef = "/systemset/basicinfo/CustomerLevel.aspx";
                }
                else if (brandManageGrant == true)
                {
                    linkCityManage.HRef = "/systemset/basicinfo/BrandManage.aspx";
                }
            }

            //系统设置-组织机构
            bool departManageGrant = backPage.CheckGrant(TravelPermission.系统设置_组织机构_部门设置栏目);
            bool departPeopleManageGrant = backPage.CheckGrant(TravelPermission.系统设置_组织机构_部门人员栏目);
            //如果都没有 组织机构 子栏目 权限 ，则隐藏 组织机构 链接
            if (!departManageGrant && !departPeopleManageGrant)//没有
            {
                liDepartManage.Visible = false;
            }
            else//有
            {
                //默认 将 部门设置，部门人员设置 栏目中
                //第一个有权限的子栏目链接 作为 组织机构 链接 初始化
                if (departManageGrant == true)
                {
                    linkDepartManage.HRef = "/systemset/organize/DepartManage.aspx";
                }
                else if (departPeopleManageGrant == true)
                {
                    linkDepartManage.HRef = "/systemset/organize/DepartEmployee.aspx";
                }
            }

            //系统设置-角色管理
            bool rolesManageGrant = backPage.CheckGrant(TravelPermission.系统设置_角色管理_角色管理栏目);
            if (!rolesManageGrant)
            {
                liRolesManage.Visible = false;
            }

            //系统设置-公司信息
            bool companyInfoGrant = backPage.CheckGrant(TravelPermission.系统设置_公司信息_公司信息栏目);
            if (!companyInfoGrant)
            {
                liCompanyInfo.Visible = false;
            }

            //系统设置-信息管理
            bool infoListGrant = backPage.CheckGrant(TravelPermission.系统设置_信息管理_信息管理栏目);
            if (!infoListGrant)
            {
                liInfoList.Visible = false;
            }

            //系统设置-系统配置
            bool configSetGrant = backPage.CheckGrant(TravelPermission.系统设置_系统配置_系统设置栏目);
            if (!configSetGrant)
            {
                liConfigSet.Visible = false;
            }

            //系统设置-系统日志
            bool logListGrant = backPage.CheckGrant(TravelPermission.系统设置_系统日志_系统日志栏目);
            if (!logListGrant)
            {
                liLogList.Visible = false;
            }
            //系统设置-同行平台
            bool peerGrant = backPage.CheckGrant(TravelPermission.系统设置_同行平台栏目);
            if (!peerGrant)
            {
                liPeer.Visible = false;
            }

            //如果都没有 基础设置，组织机构，角色管理，公司信息，信息管理，系统配置，系统日志，
            //子栏目 权限 ，则隐藏 系统设置 栏目
            if (liCityManage.Visible == false && liDepartManage.Visible == false
                && liRolesManage.Visible == false && liCompanyInfo.Visible == false
                && liInfoList.Visible == false && liConfigSet.Visible == false
                && liLogList.Visible == false && linkPeer.Visible == false)
            {
                divSystemSet.Visible = false;
            }

            //个人中心
            //个人中心-事务提醒
            bool appectAwakeGrant = backPage.CheckGrant(TravelPermission.个人中心_事务提醒_收款提醒栏目);
            bool payAwakeGrant = backPage.CheckGrant(TravelPermission.个人中心_事务提醒_付款提醒栏目);
            bool outAwakeGrant = backPage.CheckGrant(TravelPermission.个人中心_事务提醒_出团提醒栏目);
            bool backAwakeGrant = backPage.CheckGrant(TravelPermission.个人中心_事务提醒_回团提醒栏目);
            bool dueAwakeGrant = backPage.CheckGrant(TravelPermission.个人中心_事务提醒_合同到期提醒栏目);

            //如果都没有 事务提醒 子栏目 权限的 话 隐藏  事务提醒 链接
            if (!appectAwakeGrant && !payAwakeGrant && !outAwakeGrant && !backAwakeGrant && !dueAwakeGrant)//没有
            {
                liAppectAwake.Visible = false;
            }
            else//有
            {
                //默认将 收款提醒，付款提醒，出团提醒，回团提醒，合同到期中，
                //第一个有权限的子栏目链接，作为 事务提醒的 链接 进行初始化
                if (appectAwakeGrant == true)
                {
                    linkAppectAwake.HRef = "/UserCenter/WorkAwake/AppectAwake.aspx";
                }
                else if (payAwakeGrant == true)
                {
                    linkAppectAwake.HRef = "/UserCenter/WorkAwake/PayAwake.aspx";
                }
                else if (outAwakeGrant == true)
                {
                    linkAppectAwake.HRef = "/UserCenter/WorkAwake/OutAwake.aspx";
                }
                else if (backAwakeGrant == true)
                {
                    linkAppectAwake.HRef = "/UserCenter/WorkAwake/BackAwake.aspx";
                }
                else if (dueAwakeGrant == true)
                {
                    linkAppectAwake.HRef = "/UserCenter/WorkAwake/DueAwake.aspx";
                }
            }

            //个人中心-公告通知
            bool noticeGrant = backPage.CheckGrant(TravelPermission.个人中心_公告通知_查看公告);
            if (!noticeGrant)
            {
                liNotice.Visible = false;
            }
            //个人中心 - 留言板
            bool messageGrant = backPage.CheckGrant(TravelPermission.个人中心_留言板_栏目);
            if (!messageGrant)
            {
                liMessage.Visible = false;
            }
            //个人中心 - 送团任务表
            bool sendGrant = backPage.CheckGrant(TravelPermission.个人中心_送团任务表_栏目);
            if (!sendGrant)
            {
                liTasksList.Visible = false;
            }

            //个人中心-文档管理
            bool domManageGrant = backPage.CheckGrant(TravelPermission.个人中心_文档管理_栏目);
            if (!domManageGrant)
            {
                liDomManage.Visible = false;
            }

            //个人中心-工作交流
            bool workReportGrant = backPage.CheckGrant(TravelPermission.个人中心_工作交流_工作汇报栏目);
            bool workPlanGrant = backPage.CheckGrant(TravelPermission.个人中心_工作交流_工作计划栏目);
            bool workExchangeGrant = backPage.CheckGrant(TravelPermission.个人中心_工作交流_交流专区栏目);
            //如果都没有 工作汇报，工作计划，交流专区 三个字栏目权限的话，隐藏 工作交流 链接
            if (!workReportGrant && !workPlanGrant && !workExchangeGrant)//没有
            {
                liWorkReport.Visible = false;
            }
            else//有
            {
                //默认将 工作汇报，工作计划，交流专区中，
                //第一个有权限的子栏目链接 作为 工作交流 的链接进行初始化
                if (workReportGrant == true)
                {
                    linkWorkReport.HRef = "/UserCenter/WorkExchange/WorkReport.aspx";
                }
                else if (workPlanGrant == true)
                {
                    linkWorkReport.HRef = "/UserCenter/WorkExchange/WorkPlan.aspx";
                }
                else if (workExchangeGrant == true)
                {
                    linkWorkReport.HRef = "/UserCenter/WorkExchange/Exchange.aspx";
                }
            }


            //短信中心
            if (backPage.CheckGrant(TravelPermission.短信中心_短信中心_栏目) == false)
            {
                divSMS.Visible = false;
            }
        }

        /// <summary>
        /// 根据打开的页面 ，自动定位 左边菜单那
        /// </summary>
        private void AutoPositionLinks()
        {
            string currentPageUrl = Request.Url.AbsolutePath.ToLower();
            string showStyle = "display:'';";
            string highLineClass = "listIn";
            string h2ShowClass = "firstNav";

            //线路产品库
            if (currentPageUrl.Equals("/xianlu/LineProducts.aspx", StringComparison.OrdinalIgnoreCase)//线路管理
                || currentPageUrl.Equals("/xianlu/AddLineProducts.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/xianlu/Add_xl_Standard.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/xianlu/Quote.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/xianlu/UpdateQuote.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/xianlu/UpdateLineProducts.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Xianlu.Attributes["class"] = h2ShowClass;
                ulXianlu.Attributes["style"] = showStyle;
                linkLineProducts.Attributes["class"] = highLineClass;
            }
            else if (//营销分析
                (currentPageUrl.Equals("/CRM/PersonStatistics/AreaList.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "xianlu")
                || (currentPageUrl.Equals("/CRM/PersonStatistics/DepartmentalList.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "xianlu")
                || (currentPageUrl.Equals("/CRM/PersonStatistics/TimeList.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "xianlu"))
            {
                h2Xianlu.Attributes["class"] = h2ShowClass;
                ulXianlu.Attributes["style"] = showStyle;
                linkMarketing.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//散拼
                "/sanping/Default.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/sanping/QuickAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/sanping/Add.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/sanping/update.aspx", StringComparison.OrdinalIgnoreCase)
                || (currentPageUrl.Equals("/TeamPlan/TeamSettle.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "san")
                || (currentPageUrl.Equals("/sanping/SanPing_JiPiaoAdd.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "1"))
            {
                h2SanPing.Attributes["class"] = h2ShowClass;
                ulSanPing.Attributes["style"] = showStyle;
                linkSanPing.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//散客天天发
                "/sanping/DaydayPublish.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/sanping/DaydayAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/sanping/DaydayQuickAdd.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SanPing.Attributes["class"] = h2ShowClass;
                ulSanPing.Attributes["style"] = showStyle;
                linkDayDay.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//团队计划
                "/TeamPlan/TeamPlanList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/TeamPlan/FastVersion.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/TeamPlan/StandardVersion.aspx", StringComparison.OrdinalIgnoreCase)
                || (currentPageUrl.Equals("/TeamPlan/TeamSettle.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "team")
                || (currentPageUrl.Equals("/sanping/SanPing_JiPiaoAdd.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") != "1"))
            {
                h2TeamPlan.Attributes["class"] = h2ShowClass;
                ulTeamPlan.Attributes["style"] = showStyle;
                linkTeamPlan.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//上传报价
                "/TeamPlan/TourQuoteList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2TeamPlan.Attributes["class"] = h2ShowClass;
                ulTeamPlan.Attributes["style"] = showStyle;
                linkTourQuoteList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//询价
                "/TeamPlan/TourAskPriceList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2TeamPlan.Attributes["class"] = h2ShowClass;
                ulTeamPlan.Attributes["style"] = showStyle;
                linkTeamPrice.Attributes["class"] = highLineClass;
            }

            else if (currentPageUrl.Equals(//单项服务
                "/SingleServe/SingleServeList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SingleServer.Attributes["class"] = h2ShowClass;
                ulSingleServer.Attributes["style"] = showStyle;
                linkSingleSeverList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//销售管理-订单中心
                "/sales/Order_List.aspx", StringComparison.OrdinalIgnoreCase
                ))
            {
                h2Sales.Attributes["class"] = h2ShowClass;
                ulSales.Attributes["style"] = showStyle;
                linkOrderList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//销售管理-销售收款
                "/sales/Sale_List.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Sales.Attributes["class"] = h2ShowClass;
                ulSales.Attributes["style"] = showStyle;
                linkSaleList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//机票管理
                "/jipiao/JiPiao_List.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2JiPiao.Attributes["class"] = h2ShowClass;
                ulJiPiao.Attributes["style"] = showStyle;
                linkJiPiaoList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//机票管理:退票统计
                "/jipiao/JiPiao_TuiList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2JiPiao.Attributes["class"] = h2ShowClass;
                ulJiPiao.Attributes["style"] = showStyle;
                linkTicketRefund.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//出票统计
                "/jipiao/TicketStatistics/AirwaysStat.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals(//出票统计
                "/jipiao/TicketStatistics/TicketStatisticslist.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals(//出票统计
                "/jipiao/TicketStatistics/DateStats.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals(//出票统计
                "/jipiao/TicketStatistics/TicketDepartList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2JiPiao.Attributes["class"] = h2ShowClass;
                ulJiPiao.Attributes["style"] = showStyle;
                linkliJiPiaotj.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/jipiao/Traffic/trafficList.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals("/jipiao/Traffic/travelList.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals("/jipiao/Traffic/trafficpricesList.aspx", StringComparison.OrdinalIgnoreCase)) //交通管理
            {
                h2JiPiao.Attributes["class"] = h2ShowClass;
                ulJiPiao.Attributes["style"] = showStyle;
                linktrafficlist.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/jipiao/Traffic/trafficStatisList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2JiPiao.Attributes["class"] = h2ShowClass;
                ulJiPiao.Attributes["style"] = showStyle;
                linktrafficStatis.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-地接
            "/SupplierControl/AreaConnect/AreaConnect.aspx", StringComparison.OrdinalIgnoreCase))
            {

                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkAreaConnect.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-票务
                "/SupplierControl/TicketService/TicketService.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkTicketService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-酒店
                "/SupplierControl/Hotels/HotelList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkHotelService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-餐馆
                "/SupplierControl/Restaurants/Restaurantslist.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkRestService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-车队
                "/SupplierControl/CarsManager/CarsList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkCarService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-景点
                "/SupplierControl/SightManager/SightList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkAreaService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-购物
                "/SupplierControl/Shopping/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkShopService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-保险
                "/SupplierControl/Insurance/Insurancelist.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkInsuranceService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-其它
                "/SupplierControl/Others/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkOtherService.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//供应商管理-航空公司
                "/SupplierControl/AirLine/AirLineList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Supplier.Attributes["class"] = h2ShowClass;
                ulSupplier.Attributes["style"] = showStyle;
                linkAirLine.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//客户关系管理-客户资料
               "/CRM/customerinfos/CustomerList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CRM.Attributes["class"] = h2ShowClass;
                ulCRM.Attributes["style"] = showStyle;
                linkCustomerList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//客户关系管理-客户服务
                "/CRM/customerservice/MarketActive.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/customerservice/CustomerCare.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/customerservice/CustomerVisit.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/customerservice/CustomerComplaint.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CRM.Attributes["class"] = h2ShowClass;
                ulCRM.Attributes["style"] = showStyle;
                linkMarketActive.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//客户关系管理-销售分析
                "/CRM/PersonStatistics/AreaList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/PersonStatistics/DepartmentalList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/PersonStatistics/TimeList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/ProfitStatistical/AreaList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/ProfitStatistical/DepartmentalList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/ProfitStatistical/TypeList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/ProfitStatistical/TimeList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/CRM/TimeStatistics/AreaList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CRM.Attributes["class"] = h2ShowClass;
                ulCRM.Attributes["style"] = showStyle;
                linkAreaList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//客户关系管理-销售统计
                "/CRM/SellStat/SellStat.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CRM.Attributes["class"] = h2ShowClass;
                ulCRM.Attributes["style"] = showStyle;
                linkSellStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//客户关系管理-返佣统计
                 "/CRM/ReturnStatistics/ReturnList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CRM.Attributes["class"] = h2ShowClass;
                ulCRM.Attributes["style"] = showStyle;
                linkBackMoney.Attributes["class"] = highLineClass;
            }
            else if ( //财务管理-团队核算
                   (currentPageUrl.Equals("/TeamPlan/TeamSettle.aspx", StringComparison.OrdinalIgnoreCase) && Utils.GetQueryStringValue("type") == "account")
               || currentPageUrl.Equals("/caiwuguanli/TeamAccount.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkTeamAccount.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-团款收入
                "/caiwuguanli/srtuankuan_list.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/caiwuguanli/TeamClear.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals("/caiwuguanli/piliangshenhe.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkSrTuanKuanList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-杂费收入
                "/caiwuguanli/Income.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkIncome.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-团款支出
                "/caiwuguanli/TeamExpenditure.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/caiwuguanli/zctuankuan_fukuan.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/caiwuguanli/waitkuan.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/caiwuguanli/teamPayClear.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals("/caiwuguanli/single_fukuan.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals("/caiwuguanli/jidiaoleixing.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkTeamExpend.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-杂费支出
                "/caiwuguanli/Expenditure.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkOtherExpend.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-出纳登帐
                "/caiwuguanli/chunadz_list.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkChuNa.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//财务管理-机票审核
                "/caiwuguanli/JiPiaoAudit.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkJipiaoAudit.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-人次统计
                "/StatisticAnalysis/PersonTime/PerAreaStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/PersonTime/PerDepartmentStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/PersonTime/PerTimeStatisticList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkPreAreaStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-员工业绩表
                "/StatisticAnalysis/EmployeeAchievementsTime/EmpIncomeStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/EmployeeAchievementsTime/EmpProfitStatisticList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkEmpIncomeStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-利润统计
                "/StatisticAnalysis/ProfitStatistic/ProAreaStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/ProfitStatistic/ProDepartmentStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/ProfitStatistic/ProTypeStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/ProfitStatistic/ProTimeStatisticList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkProAreaStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-收入对帐单
                "/StatisticAnalysis/IncomeAccount/IncDepartmentStatisticList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkIncAreaStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-支出对帐单
                "/StatisticAnalysis/OutlayAccount/OutDepartmentStatisticList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkOutAreaStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-帐龄分析表
                "/StatisticAnalysis/Ageanalysis/AgeDepartmentStatisticList.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/StatisticAnalysis/Ageanalysis/ZhangLingAnKeHuDanWei.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkAgeAreaStat.Attributes["class"] = highLineClass;
            }
            //统计分析-销售利润统计
            else if (currentPageUrl.Equals("/StatisticAnalysis/SaleProfitcount/SaleProfitcount.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkProfit.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-现金流量表
            "/StatisticAnalysis/CashFlow/CasDayStaList.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals("/StatisticAnalysis/CashFlow/CasMonthStaList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkCasDayStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-销售统计分析
                "/StatisticAnalysis/SoldStatistic/SoldStaList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkSoldStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//统计分析-区域销售统计
                "/StatisticAnalysis/SoldStatistic/AreaSoldStaList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                linkAreaSoldStat.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-职务管理
                "/administrativeCenter/positionManage/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkPositionManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-人事档案
                "/administrativeCenter/personnelFiles/Default.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/administrativeCenter/personnelFiles/Edit.aspx", StringComparison.OrdinalIgnoreCase))
            {
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkPersonnelFiels.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-考勤管理
                "/administrativeCenter/attendanceManage/Default.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/administrativeCenter/attendanceManage/PersonnelSalary.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkAttendanceManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-内部通讯录
                "/administrativeCenter/addressList/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkAddresssList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-规章制度
                "/administrativeCenter/bylaw/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkBylaw.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-会议记录管理
                "/administrativeCenter/cahierManage/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkCahierManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-劳动合同管理
                "/administrativeCenter/contractManage/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkContractManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-固定资产管理
                "/administrativeCenter/fixedAssetsManage/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkFixedAssetsManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//行政中心-培训计划
                "/administrativeCenter/trainingPlan/Default.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2AdministrativeCenter.Attributes["class"] = h2ShowClass;
                ulAdministrativeCenter.Attributes["style"] = showStyle;
                linkTrainingPlan.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-基础设置
                "/systemset/basicinfo/CityManage.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/systemset/basicinfo/RouteArea.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/systemset/basicinfo/PriceStand.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/systemset/basicinfo/CustomerLevel.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/systemset/basicinfo/BrandManage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkCityManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-组织机构
                "/systemset/organize/DepartManage.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/systemset/organize/DepartEmployee.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkDepartManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-角色管理
                "/systemset/rolemanage/RolesManage.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkRolesManage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-公司信息
                "/systemset/companyInfo.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkCompanyInfo.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-信息管理
                "/systemset/infomanage/InfoList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkInfoList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-信息管理
                "/systemset/infomanage/InfoEdit.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkInfoList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-系统配置
                "/systemset/configset.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkConfigSet.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//系统设置-系统日志
                "/systemset/systemlog/LogList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkLogList.Attributes["class"] = highLineClass;
            } if (currentPageUrl.Equals(//系统设置-同行平台
             "/systemset/ToGoTerrace/BaseManage.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals(
             "/systemset/ToGoTerrace/RotateImg.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals(
             "/systemset/ToGoTerrace/TickePoliyList.aspx", StringComparison.OrdinalIgnoreCase) || currentPageUrl.Equals(
             "/systemset/ToGoTerrace/FriendshipLink.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SystemSet.Attributes["class"] = h2ShowClass;
                ulSystemSet.Attributes["style"] = showStyle;
                linkPeer.Attributes["class"] = highLineClass;
            }

            else if (currentPageUrl.Equals(//个人中心-事务提醒
            "/UserCenter/WorkAwake/AppectAwake.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals("/UserCenter/WorkAwake/PayAwake.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals("/UserCenter/WorkAwake/OutAwake.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals("/UserCenter/WorkAwake/BackAwake.aspx", StringComparison.OrdinalIgnoreCase)
            || currentPageUrl.Equals("/UserCenter/WorkAwake/DueAwake.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkAppectAwake.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-公告通知
                "/UserCenter/Notice/Notice.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/Notice/NoticeShow.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkNotice.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-文档管理
                "/UserCenter/DomManager/DomManager.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkDomManager.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-工作交流
                "/UserCenter/WorkExchange/WorkReport.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/WorkPlan.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/Exchange.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/WorkPlanAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/WorkReportAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/ExchangeAdd.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/WorkPlanShow.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/WorkReportShow.aspx", StringComparison.OrdinalIgnoreCase)
                || currentPageUrl.Equals("/UserCenter/WorkExchange/ExchangeShow.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkWorkReport.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-个人信息
                "/UserCenter/UserInfo/UserInfo.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkUserInfo.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-发送短信
                "/SMS/SendSms.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SMS.Attributes["class"] = h2ShowClass;
                ulSMS.Attributes["style"] = showStyle;
                linkSendSMS.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-客户列表
                "/SMS/SmsCustomerList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SMS.Attributes["class"] = h2ShowClass;
                ulSMS.Attributes["style"] = showStyle;
                linkSMSCustomerList.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-发送历史
                "/SMS/SendHistory.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SMS.Attributes["class"] = h2ShowClass;
                ulSMS.Attributes["style"] = showStyle;
                linkSendHistory.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-常用短信
                "/SMS/CommonSms.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SMS.Attributes["class"] = h2ShowClass;
                ulSMS.Attributes["style"] = showStyle;
                linkCommonSMS.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-帐户信息
                "/SMS/AccountInfo.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2SMS.Attributes["class"] = h2ShowClass;
                ulSMS.Attributes["style"] = showStyle;
                linkAccountInfo.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-送团任务表
                "/UserCenter/SendTasks/TasksList.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linSendTasks.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals(//个人中心-留言板
                "/UserCenter/Message/MessageBoard.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2UserCenter.Attributes["class"] = h2ShowClass;
                ulUserCenter.Attributes["style"] = showStyle;
                linkMessage.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/StatisticAnalysis/HuiKuanLv/HuiKuanLv.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                AHuiKuanLv.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/StatisticAnalysis/ZhiChuZhangLing/ZhiChuZhangLing.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2Stat.Attributes["class"] = h2ShowClass;
                ulStat.Attributes["style"] = showStyle;
                AZhiChuZhangLing.Attributes["class"] = highLineClass;
            }
            else if (currentPageUrl.Equals("/caiwuguanli/fapiaoguanli.aspx", StringComparison.OrdinalIgnoreCase))
            {
                h2CaiWu.Attributes["class"] = h2ShowClass;
                ulCaiWu.Attributes["style"] = showStyle;
                linkFaPiaoGuanLi.Attributes["class"] = highLineClass;
            }
            //
        }
    }
}
