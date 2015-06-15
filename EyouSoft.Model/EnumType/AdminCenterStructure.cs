using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    public class AdminCenterStructure
    {
        /// <summary>
        /// 公司档案在职类型
        /// </summary>
        public enum PersonalType
        {
            /// <summary>
            /// 正式
            /// </summary>
            正式 = 0,
            /// <summary>
            /// 试用
            /// </summary>
            试用 = 1,
            /// <summary>
            /// 学徒
            /// </summary>
            学徒 = 2
        }
        /// <summary>
        /// 学历类型
        /// </summary>
        public enum DegreeType
        {
            /// <summary>
            /// 初中
            /// </summary>
            初中 = 0,
            /// <summary>
            /// 高中
            /// </summary>
            高中 = 1,
            /// <summary>
            /// 中专
            /// </summary>
            中专 = 2,
            /// <summary>
            /// 专科
            /// </summary>
            专科 = 3,
            /// <summary>
            /// 本科
            /// </summary>
            本科 = 4,
            /// <summary>
            /// 硕士
            /// </summary>
            硕士 = 5,
            /// <summary>
            /// 博士
            /// </summary>
            博士 = 6
        }
        /// <summary>
        /// 考勤状况
        /// </summary>
        public enum WorkStatus
        {
            /// <summary>
            /// 准点
            /// </summary>
            准点 = 0,
            /// <summary>
            /// 迟到
            /// </summary>
            迟到 = 1,
            /// <summary>
            /// 准点
            /// </summary>
            早退 = 2,
            /// <summary>
            /// 旷工
            /// </summary>
            旷工 = 3,
            /// <summary>
            /// 休假
            /// </summary>
            休假 = 4,
            /// <summary>
            /// 外出
            /// </summary>
            外出 = 5,
            /// <summary>
            /// 出团
            /// </summary>
            出团 = 6,
            /// <summary>
            /// 请假
            /// </summary>
            请假 = 7,
            /// <summary>
            /// 加班
            /// </summary>
            加班 = 8
        }
        /// <summary>
        /// 合同状态
        /// </summary>
        public enum ContractStatus
        {
            /// <summary>
            /// 未到期
            /// </summary>
            未到期 = 0,
            /// <summary>
            /// 到期未处理
            /// </summary>
            到期未处理 = 1,
            /// <summary>
            /// 到期已处理
            /// </summary>
            到期已处理 = 2
        }
        /// <summary>
        /// 培训计划接收对象类型
        /// </summary>
        public enum AcceptType
        {
            /// <summary>
            /// 所有
            /// </summary>
            所有 = 0,
            /// <summary>
            /// 指定部门
            /// </summary>
            指定部门 = 1,
            /// <summary>
            /// 指定人
            /// </summary>
            指定人 = 2
        }
    }
}
