using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 轮换图片实体
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteChangePic
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
        public int CompanyId { get; set; }
        /// <summary>
        /// 与其它表的关联编号
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 所属类型
        /// </summary>
        public EyouSoft.Model.EnumType.SiteStructure.ItemType ItemType { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 图片链接到的URL
        /// </summary>
        public string URL { get; set; }
    }
}
