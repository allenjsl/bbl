using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 友情链接实体
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteFriendLink
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int SortId { get; set; }
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId { get; set;}
        /// <summary>
        /// 友情链接类型
        /// </summary>
        public  EyouSoft.Model.EnumType.SiteStructure.LinkType LinkType{ get; set; }
        /// <summary>
        /// 友情链接名称
        /// </summary>
        public string LinkName { get; set; }
        /// <summary>
        /// 友情链接路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 友情链接URL
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 友情链接添加时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}
