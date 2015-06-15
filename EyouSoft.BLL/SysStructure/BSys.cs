/************************************************************
 * 模块名称：管理后台系统管理业务逻辑类
 * 功能说明：实现管理后台系统管理功能
 * 创建人：周文超  2011-4-15 10:27:17
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SysStructure
{
    /// <summary>
    /// 管理后台系统管理业务逻辑类
    /// </summary>
    /// Author:周文超 2011-04-15
    public class BSys
    {
        private readonly EyouSoft.IDAL.SysStructure.ISys dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SysStructure.ISys>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BSys() { }
        #endregion

        #region publice members
        /// <summary>
        /// 创建子系统，返回1成功，其他失败
        /// </summary>
        /// <param name="sysInfo">系统实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CreateSys(Model.SysStructure.MSysInfo sysInfo)
        {
            #region data validate
            if (sysInfo == null) return 0;
            if (sysInfo.Domains == null || sysInfo.Domains.Count < 1) return -1;
            if (sysInfo.AdminInfo == null || string.IsNullOrEmpty(sysInfo.AdminInfo.UserName)) return -2;

            #region 打印单据配置验证
            if (sysInfo.Setting != null && sysInfo.Setting.PrintDocument != null && sysInfo.Setting.PrintDocument.Count > 0)//打印单据配置验证
            {
                IDictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int> dic = new Dictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int>();

                foreach (var item in sysInfo.Setting.PrintDocument)
                {
                    if (dic.ContainsKey(item.PrintTemplateType))
                    {
                        dic[item.PrintTemplateType]++;
                    }
                    else
                    {
                        dic.Add(item.PrintTemplateType, 1);
                    }
                }

                bool isRepeat = false;
                foreach (var item in dic)
                {
                    if (item.Value > 1)
                    {
                        isRepeat = true;
                        break;
                    }
                }

                if (isRepeat)
                {
                    return -3;
                }
            }
            #endregion

            if (sysInfo.ModuleIds == null && sysInfo.ModuleIds.Length < 1)
            {
                sysInfo.ModuleIds = new int[] { -1 };
            }

            if (sysInfo.PartIds == null || sysInfo.PartIds.Length < 1)
            {
                sysInfo.PartIds = new int[] { -1 };
            }

            if (sysInfo.PermissionIds == null || sysInfo.PermissionIds.Length < 1)
            {
                sysInfo.PermissionIds = new int[] { -1 };
            }

            if (sysInfo.Domains != null && sysInfo.Domains.Count > 0)
            {
                foreach (var domain in sysInfo.Domains)
                {
                    domain.Domain = domain.Domain.Replace("http://", "");
                }
            }
            #endregion

            int dalExceptionCode = dal.CreateSys(sysInfo);

            if (dalExceptionCode == 1)
            {
                EyouSoft.BLL.SMSStructure.Account smsbll = new EyouSoft.BLL.SMSStructure.Account();
                smsbll.SetAccountBaseInfo(sysInfo.CompanyInfo.Id);
                smsbll = null;
            }

            return dalExceptionCode;
        }

        /// <summary>
        /// 修改子系统，返回1成功，其他失败
        /// </summary>
        /// <param name="sysInfo">系统实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateSys(Model.SysStructure.MSysInfo sysInfo)
        {
            #region data validate
            if (sysInfo == null) return 0;
            if (sysInfo.SystemId < 1) return 0;
            if (sysInfo.Domains == null || sysInfo.Domains.Count < 1) return -1;
            if (sysInfo.AdminInfo == null || string.IsNullOrEmpty(sysInfo.AdminInfo.UserName)) return -2;

            #region 打印单据配置验证
            if (sysInfo.Setting != null && sysInfo.Setting.PrintDocument != null && sysInfo.Setting.PrintDocument.Count > 0)//打印单据配置验证
            {
                IDictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int> dic = new Dictionary<EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType, int>();

                foreach (var item in sysInfo.Setting.PrintDocument)
                {
                    if (dic.ContainsKey(item.PrintTemplateType))
                    {
                        dic[item.PrintTemplateType]++;
                    }
                    else
                    {
                        dic.Add(item.PrintTemplateType, 1);
                    }
                }

                bool isRepeat = false;
                foreach (var item in dic)
                {
                    if (item.Value > 1)
                    {
                        isRepeat = true;
                        break;
                    }
                }

                if (isRepeat)
                {
                    return -3;
                }
            }
            #endregion

            if (sysInfo.ModuleIds == null || sysInfo.ModuleIds.Length < 1)
            {
                sysInfo.ModuleIds = new int[] { -1 };
            }

            if (sysInfo.PartIds == null || sysInfo.PartIds.Length < 1)
            {
                sysInfo.PartIds = new int[] { -1 };
            }

            if (sysInfo.PermissionIds == null || sysInfo.PermissionIds.Length < 1)
            {
                sysInfo.PermissionIds = new int[] { -1 };
            }

            if (sysInfo.Domains != null && sysInfo.Domains.Count > 0)
            {
                foreach (var domain in sysInfo.Domains)
                {
                    domain.Domain = domain.Domain.Replace("http://", "");
                }
            }
            #endregion

            int dalExceptionCode = dal.UpdateSys(sysInfo);

            if (dalExceptionCode == 1)
            {
                EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyConfig, sysInfo.CompanyInfo.Id));
            }

            return dalExceptionCode;
        }


        /// <summary>
        /// 获取子系统信息，仅取WEBMASTER修改子系统时使用的数据
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public Model.SysStructure.MSysInfo GetSysInfo(int sysId)
        {
            if (sysId < 1) return null;

            Model.SysStructure.MSysInfo sysInfo = dal.GetSysInfo(sysId);

            if (sysInfo != null)
            {
                int companyId=dal.GetCompanyIdBySysId(sysId);
                EyouSoft.BLL.CompanyStructure.CompanyInfo companybll = new EyouSoft.BLL.CompanyStructure.CompanyInfo();
                sysInfo.CompanyInfo = companybll.GetModel(companyId, sysId);
                companybll = null;

                EyouSoft.BLL.CompanyStructure.CompanyUser userbll = new EyouSoft.BLL.CompanyStructure.CompanyUser();
                sysInfo.AdminInfo = userbll.GetAdminModel(companyId);
                userbll = null;

                EyouSoft.BLL.CompanyStructure.Department departmentbll = new EyouSoft.BLL.CompanyStructure.Department();
                sysInfo.DepartmentInfo = departmentbll.GetModel(dal.GetHeadOfficeIdByCompanyId(companyId));
                departmentbll = null;

                EyouSoft.BLL.CompanyStructure.CompanySetting settingbll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
                sysInfo.Setting = settingbll.GetSetting(companyId);
                settingbll = null;

                EyouSoft.BLL.SysStructure.SystemDomain domainbll = new EyouSoft.BLL.SysStructure.SystemDomain();
                sysInfo.Domains = domainbll.GetDomains(sysId);
                domainbll = null;

                if (sysInfo.ModuleIds == null || sysInfo.ModuleIds.Length < 1)
                {
                    sysInfo.ModuleIds = new int[] { -1 };
                }

                if (sysInfo.PartIds == null || sysInfo.PartIds.Length < 1)
                {
                    sysInfo.PartIds = new int[] { -1 };
                }

                if (sysInfo.PermissionIds == null || sysInfo.PermissionIds.Length < 1)
                {
                    sysInfo.PermissionIds = new int[] { -1 };
                }
            }

            return sysInfo;
        }

        /// <summary>
        /// 获取所有子系统信息集合
        /// </summary>
        /// <returns></returns>
        public IList<Model.SysStructure.MLBSysInfo> GetSyss(Model.SysStructure.MSysSearchInfo searchInfo)
        {
            return dal.GetSyss(searchInfo);
        }

        /// <summary>
        /// 更新管理员账号信息，密码为空时不修改密码
        /// </summary>
        /// <param name="webmasterInfo">EyouSoft.Model.SysStructure.MWebmasterInfo</param>
        /// <returns></returns>
        public bool UpdateWebmasterInfo(EyouSoft.Model.SysStructure.MWebmasterInfo webmasterInfo)
        {
            if (webmasterInfo == null) return false;
            if (webmasterInfo.UserId < 1) return false;
            if (string.IsNullOrEmpty(webmasterInfo.Username)) return false;

            return dal.UpdateWebmasterInfo(webmasterInfo);
        }
        #endregion
    }
}
