using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 轮换图片 业务类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteChangePic : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.SiteStructure.ISiteChangePic idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SiteStructure.ISiteChangePic>();

        /// <summary>
        /// 得到轮换图片列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            if (CompanyId == 0) return null;
            return idal.GetSiteChange(CompanyId,pageSize,pageIndex,ref recordCount);
        }

        public IList<EyouSoft.Model.SiteStructure.SiteChangePic> GetSiteChange(int CompanyId)
        {
            if (CompanyId == 0) return null;
            return idal.GetSiteChange(CompanyId);
        }
        /// <summary>
        /// 得到轮换图片实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="companyId">专线公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteChangePic GetSiteChange(int Id,int CompanyId)
        {
            if(Id==0 || CompanyId==0)return null;
            return idal.GetSiteChange(Id, CompanyId);
        }

        /// <summary>
        /// 添加轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        public bool AddSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic)
        {
            if(SiteChangePic==null || SiteChangePic.CompanyId==0)return false;
            return idal.AddSiteChangePic(SiteChangePic);
        }

        /// <summary>
        /// 修改轮换图片
        /// </summary>
        /// <param name="SiteChangePic"></param>
        /// <returns></returns>
        public bool UpdateSiteChangePic(EyouSoft.Model.SiteStructure.SiteChangePic SiteChangePic)
        {
            if (SiteChangePic == null || SiteChangePic.CompanyId == 0 || SiteChangePic.Id==0) return false;
            return idal.UpdateSiteChangePic(SiteChangePic);
        }

        /// <summary>
        /// 删除轮换图片
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public bool DelSiteChangePic(int Id, int CompanyId)
        {
            if (Id == 0 || CompanyId == 0) return false;
            return idal.DelSiteChangePic(Id,CompanyId);
        }
    }
}
