using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司系统配置BLL
    /// Author 2011-01-22
    /// </summary>
    public class CompanySetting
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanySetting Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanySetting>();

        #region public members
        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        public bool SetCompanySetting(EyouSoft.Model.CompanyStructure.CompanyFieldSetting model)
        {
            bool dalResult = Dal.SetCompanySetting(model);

            if (dalResult)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, model.CompanyId));

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_系统配置.ToString() + "更新了系统配置信息。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "更新系统配置";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_系统配置;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion
            }

            return dalResult;
        }
        /// <summary>
        /// 设置提前X天出团提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ReminderDays">提醒天数</param>
        /// <returns></returns>
        public bool SetLeaveTourReminderDays(int CompanyId, int ReminderDays)
        {
            if (CompanyId <= 0 || ReminderDays <= 0)
                return false;
            
            bool dalResult=Dal.SetValue(CompanyId, "LeaveTourDays", ReminderDays.ToString());

            if (dalResult)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId));
            }

            return dalResult;
        }
        /// <summary>
        /// 设置提前X天回团提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ReminderDays">提醒天数</param>
        /// <returns></returns>
        public bool SetBackTourReminderDays(int CompanyId, int ReminderDays)
        {
            if (CompanyId <= 0 || ReminderDays <= 0)
                return false;
            bool dalResult = Dal.SetValue(CompanyId, "BackTourDays", ReminderDays.ToString());

            if (dalResult)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId));
            }

            return dalResult;
        }
        /// <summary>
        /// 设置提前X天合同提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ReminderDays">提醒天数</param>
        /// <returns></returns>
        public bool SetContractReminderDays(int CompanyId, int ReminderDays)
        {
            if (CompanyId <= 0 || ReminderDays <= 0)
                return false;
            bool dalResult =Dal.SetValue(CompanyId, "ContractReminderDays", ReminderDays.ToString());

            if (dalResult)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId));
            }

            return dalResult;
        }
        /// <summary>
        /// 设置公司的LOGO
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Logo">LOGO文件路径</param>
        /// <returns></returns>
        public bool SetCompanyLogo(int CompanyId, string Logo)
        {
            if (CompanyId <= 0 || string.IsNullOrEmpty(Logo))
                return false;
            bool dalResult = Dal.SetValue(CompanyId, "CompanyLogo", Logo);

            if (dalResult)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId));
            }

            return dalResult;
        }
        /// <summary>
        /// 获取指定公司的系统配置信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyFieldSetting GetSetting(int CompanyId)
        {
            if (CompanyId <= 0)
            {
                return null;
            }

            EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = (EyouSoft.Model.CompanyStructure.CompanyFieldSetting)EyouSoft.Cache.Facade.EyouSoftCache.GetCache(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId));

            if (config == null)
            {
                config = Dal.GetSetting(CompanyId);

                if (config != null)
                {
                    if (config.TanChuangTiXingInterval < 59) config.TanChuangTiXingInterval = 600;
                    EyouSoft.Cache.Facade.EyouSoftCache.Add(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, CompanyId), config);
                }
            }

            return config;
        }
        /// <summary>
        /// 出团提前多少天提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetLeaveTourReminderDays(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.LeaveTourReminderDays;
            else
                return 0;
        }
        /// <summary>
        /// 获取统计分析利润统计团队数页面路径配置
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public string GetProfitStatTourPagePath(int CompanyId)
        {
            if (CompanyId <= 0)
                return "";
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.ProfitStatTourPagePath;
            else
                return "";
        }

        /// <summary>
        /// 获取机票票款计算公式
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? GetAgencyFee(int CompanyId)
        {
            if (CompanyId <= 0)
                return null;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.AgencyFeeInfo;
            else
                return null;
        }
        /// <summary>
        /// 获取指定公司的LOGO
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public string GetCompanyLogo(int CompanyId,EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType CompanyType)
        {
            if (CompanyId <= 0)
                return string.Empty;
            string CompanyLogo = string.Empty;
            if (CompanyType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
                if (model != null)
                    CompanyLogo = model.CompanyLogo;
                model=null;
            }
            else
            {
                int BrandId =0;
                EyouSoft.Model.CompanyStructure.CustomerInfo CustomerModel = new DAL.CompanyStructure.Customer().GetCustomerModel(CompanyId);
                if (CustomerModel != null)
                {
                    BrandId= CustomerModel.BrandId;
                    CustomerModel = null;
                }
                if (BrandId > 0)
                {
                    EyouSoft.Model.CompanyStructure.CompanyBrand BrandModel = new DAL.CompanyStructure.CompanyBrand().GetModel(BrandId);
                    if (BrandModel != null)
                        CompanyLogo = BrandModel.Logo1;
                    BrandModel = null;
                }
            }
            return CompanyLogo;
        }
        /// <summary>
        /// 回团提前多少天提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetBackTourReminderDays(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.BackTourReminderDays;
            else
                return 0;
        }
        /// <summary>
        /// 合同提前多少天提醒
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetContractReminderDays(int CompanyId)
        { 
            if(CompanyId<=0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.ContractReminderDays;
            else
                return 0;
        }
        /// <summary>
        /// 列表控制显示后几个月
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetDisplayAfterMonth(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.DisplayAfterMonth;
            else
                return 0;
        }
        /// <summary>
        /// 列表控制显示前几个月
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetDisplayBeforeMonth(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.DisplayBeforeMonth;
            else
                return 0;
        }
        /// <summary>
        /// 最长留位时间
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetReservationTime(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.ReservationTime;
            else
                return 0;
        }
        /// <summary>
        /// 系统价格组成类型
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.PriceComponent GetPriceComponent(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.PriceComponent;
            else
                return 0;
        }
        /// <summary>
        /// 公司打印模版集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.PrintDocument> GetPrintDocument(int CompanyId)
        {
            if (CompanyId <= 0)
                return null;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.PrintDocument;
            else
                return null;
        }
        /// <summary>
        /// 获取指定公司的打印文件
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetCompanyPrintFile(int CompanyId)
        {
            if (CompanyId <= 0)
                return null;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.CompanyPrintFile;
            else
                return null;
        }
        /// <summary>
        /// 获取公司打印单据路径
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="type">打印单据类型</param>
        /// <returns></returns>
        public string GetPrintPath(int companyId, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType type)
        {
            string path = string.Empty;
            IList<EyouSoft.Model.CompanyStructure.PrintDocument> items = this.GetPrintDocument(companyId);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.PrintTemplateType == type) { path = item.PrintTemplate; break; }
                }
            }

            return path;
        }

        /// <summary>
        /// 获取公司统计订单方式
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public Model.EnumType.CompanyStructure.ComputeOrderType? GetComputeOrderType(int CompanyId)
        {
            if (CompanyId <= 0)
                return null;

            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = this.GetSetting(CompanyId);
            if (model != null)
                return model.ComputeOrderType;
            else
                return null;
        }

        /// <summary>
        /// 获取申请机票游客勾选配置
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <returns></returns>
        public Model.EnumType.CompanyStructure.TicketTravellerCheckedType GetTicketTravellerCheckedType(int companyId)
        {
            Model.EnumType.CompanyStructure.TicketTravellerCheckedType ticketTravellerCheckedType = EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.All;

            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            if (settings != null)
            {
                ticketTravellerCheckedType = settings.TicketTravellerCheckedType;
            }

            return ticketTravellerCheckedType;
        }

        /// <summary>
        /// 获取收款提醒配置
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType GetReceiptRemindType(int companyId)
        {
            EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType receiptRemindType = EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.AllUser;

            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            if (settings != null)
            {
                receiptRemindType = settings.ReceiptRemindType;
            }

            return receiptRemindType;
        }

        /// <summary>
        /// 获取游客信息是否必填配置
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <returns></returns>
        public bool GetIsRequiredTraveller(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            return settings != null ? settings.IsRequiredTraveller : false;
        }

        /// <summary>
        /// 获取团队计划人数配置
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople GetTeamNumberOfPeople(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            return settings != null ? settings.TeamNumberOfPeople : EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.OnlyTotalNumber;
        }

        /// <summary>
        /// 获取机票售票处填写时间配置
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime GetTicketOfficeFillTime(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            return settings != null ? settings.TicketOfficeFillTime : EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime.Ticket;
        }

        /// <summary>
        /// 获取回款率是否包含未审核收款
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public bool GetHuiKuanLvSFBHWeiShenHe(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);
            return settings != null ? settings.HuiKuanLvSFBHWeiShenHe : false;
        }

        /// <summary>
        /// 获取同行平台计划展示方式
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType GetSiteTourDisplayType(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);
            return settings != null ? settings.SiteTourDisplayType : EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;
        }

        /// <summary>
        /// 获取同行平台模板编号，返回0时未选择或未开通同行模块
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.SysStructure.SiteTemplate GetSiteTemplateId(int companyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);
            return settings != null ? settings.SiteTemplate : EyouSoft.Model.EnumType.SysStructure.SiteTemplate.None;
        }

        /// <summary>
        /// 获取弹窗提醒间隔时间（单位：秒），默认值600秒。
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int GetTanChuangTiXingInterval(int companyId)
        {
            int interval = 600;
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting settings = this.GetSetting(companyId);

            if (settings != null && settings.TanChuangTiXingInterval > 59)
            {
                interval = settings.TanChuangTiXingInterval;
            }

            return interval;
        }
        #endregion
    }
}
