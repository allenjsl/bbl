using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    /// <summary>
    /// 个人中心-相关枚举
    /// </summary>
    /// 鲁功源 2011-01-17
    public class PersonalCenterStructure
    {
        /// <summary>
        /// 审核状态
        /// </summary>
        public enum CheckState
        { 
            /// <summary>
            /// 未审核
            /// </summary>
            未审核=0,
            /// <summary>
            /// 已审核
            /// </summary>
            已审核=1
        }
        /// <summary>
        /// 工作计划审核状态
        /// </summary>
        public enum PlanCheckState
        {
            /// <summary>
            /// 进行中
            /// </summary>
            进行中=1,
            /// <summary>
            /// 取消
            /// </summary>
            取消=2,
            /// <summary>
            /// 未完成
            /// </summary>
            未完成=3,
            /// <summary>
            /// 效果不理想完成
            /// </summary>
            效果不理想完成=4,
            /// <summary>
            /// 效果理想完成
            /// </summary>
            效果理想完成=5
        }
        /// <summary>
        /// 发布对象类型
        /// </summary>
        public enum AcceptType
        { 
            /// <summary>
            /// 所有
            /// </summary>
            所有=0,
            /// <summary>
            /// 指定部门
            /// </summary>
            指定部门=1,
            /// <summary>
            /// 指定组团
            /// </summary>
            指定组团=2,
            /// <summary>
            /// 指定人
            /// </summary>
            指定人=3
        }
    }
}
