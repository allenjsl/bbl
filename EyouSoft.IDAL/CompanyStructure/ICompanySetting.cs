using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司系统配置数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface ICompanySetting
    {
        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        bool SetCompanySetting(EyouSoft.Model.CompanyStructure.CompanyFieldSetting model);
        /// <summary>
        /// 获取指定公司的配置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="FileKey"></param>
        /// <returns></returns>
        string GetValue(int CompanyId,string FileKey);
        /// <summary>
        /// 设置指定公司的配置信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="FieldKey">配置key</param>
        /// <param name="FieldValue">配置value</param>
        /// <returns></returns>
        bool SetValue(int CompanyId, string FieldKey, string FieldValue);
        /// <summary>
        /// 获取指定公司的系统配置信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyFieldSetting GetSetting(int CompanyId);
    }
}
