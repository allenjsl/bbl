using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 基础设置实体
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteBasicConfig
    {
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 网站标题
        /// </summary>
        public string SiteTitle { get; set; }
        /// <summary>
        /// 网站Meta
        /// </summary>
        public string SiteMeta { get; set; }
        /// <summary>
        /// 是否显示设为首页
        /// </summary>
        public bool IsSetHome { get; set; }
        /// <summary>
        /// 是否显示设为收藏
        /// </summary>
        public bool IsSetFavorite { get; set; }
        /// <summary>
        /// 网站Logo路径
        /// </summary>
        public string LogoPath { get; set; }
        /// <summary>
        /// 公司介绍(企业简介)
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 版权说明
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// 专线介绍[联系我们]
        /// </summary>
        public string SiteIntro { get; set; }
        /// <summary>
        /// 主营线路
        /// </summary>
        public string MainRoute { get; set; }
        /// <summary>
        /// 企业文化
        /// </summary>
        public string CorporateCulture { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string LianXiFangShi { get; set; }
    }
}
