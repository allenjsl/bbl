using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 友情链接 业务类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteFriendLink : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.SiteStructure.ISiteFriendLink idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SiteStructure.ISiteFriendLink>();

        /// <summary>
        /// 获得当前公司的友情链接列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.SiteFriendLink> GetSiteFriendLink(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            if (CompanyId == 0) return null;
            return idal.GetSiteFriendLink(CompanyId,pageSize,pageIndex,ref recordCount);
        }

        /// <summary>
        /// 获得友情链接实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteFriendLink GetSiteFriendLink(int Id, int CompanyId)
        {
            if (Id == 0 || CompanyId == 0) return null;
            return idal.GetSiteFriendLink(Id,CompanyId);
        }

        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        public bool AddFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink)
        {
            if (SiteFriendLink == null || SiteFriendLink.CompanyId == 0) return false;
            return idal.AddFriendLink(SiteFriendLink);
        }

        /// <summary>
        /// 修改友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        public bool UpdateFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink)
        {
            if (SiteFriendLink == null || SiteFriendLink.CompanyId == 0 || SiteFriendLink.Id==0) return false;
            return idal.UpdateFriendLink(SiteFriendLink);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelFriendLink( int Id,int CompanyId)
        {
            if (CompanyId == 0 || Id == 0) return false;
            return idal.DelFriendLink(Id, CompanyId);
        }
    }
}
