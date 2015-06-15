using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    /// <summary>
    /// 描述：网站配置相关枚举
    /// 修改记录：
    /// 1. 2011-3-28 PM 曹胡生 创建
    /// </summary>
    public class SiteStructure
    {
        /// <summary>
        /// 网站图片表关联类型
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// 其它
            /// </summary>
            其它 = 0,
            /// <summary>
            /// 轮换图片
            /// </summary>
            轮换图片 = 1
        }

        /// <summary>
        /// 网站友情链接类型
        /// </summary>
        public enum LinkType
        {
            /// <summary>
            /// 其它
            /// </summary>
            其它 = 0,
            /// <summary>
            /// 文字
            /// </summary>
            文字 = 1,
            /// <summary>
            /// 图片
            /// </summary>
            图片 = 2
        }
    }
}
