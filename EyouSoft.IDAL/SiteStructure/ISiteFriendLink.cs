using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 友情链接 接口类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public interface ISiteFriendLink
    {
        /// <summary>
        /// 获得当前公司的友情链接列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SiteStructure.SiteFriendLink> GetSiteFriendLink(int CompanyId, int pageSize, int pageIndex, ref int recordCount);
       
        /// <summary>
        /// 获得友情链接实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.SiteStructure.SiteFriendLink GetSiteFriendLink(int Id,int companyId);

        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        bool AddFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink);
        
        /// <summary>
        /// 修改友情链接
        /// </summary>
        /// <param name="SiteFriendLink"></param>
        /// <returns></returns>
        bool UpdateFriendLink(EyouSoft.Model.SiteStructure.SiteFriendLink SiteFriendLink);

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DelFriendLink(int Id,int companyId);
    }
}
