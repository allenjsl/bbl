using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 轮换图片 接口类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public interface ISiteChangePic
    {
        /// <summary>
        /// 得到轮换图片列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId, int pageSize, int pageIndex, ref int recordCount);
        IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId);
        /// <summary>
        /// 得到轮换图片实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="companyId">专线公司编号</param>
        /// <returns></returns>
        EyouSoft.Model.SiteStructure.SiteChangePic GetSiteChange(int Id,int companyId);
        
        /// <summary>
        /// 添加轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        bool AddSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic);
       
        /// <summary>
        /// 修改轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        bool UpdateSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic);
        
        /// <summary>
        /// 删除轮换图片
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        bool DelSiteChangePic(int Id, int CompanyId);
    }
}
