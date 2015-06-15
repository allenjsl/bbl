using System;
namespace EyouSoft.Model.EnumType
{
    /*Author:李焕超 2011-01-21*/
    public class PlanStructure
    {
        /// <summary>
        /// 机票种类 团队或散拼
        /// </summary>
        public enum TicketType
        {
            /// <summary>
            /// 计划申请机票
            /// </summary>
            团队申请机票 = 1,
            /// <summary>
            /// 订单申请机票
            /// </summary>
            订单申请机票 = 2,
            /// <summary>
            /// 游客退票
            /// </summary>
            订单退票 = 3
        }

        /// <summary>
        /// 机票状态 团队申请机票、订单申请机票时对应机票状态 订单退票时 申请=1 退票完成=4
        /// </summary>
        public enum TicketState
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// 机票申请
            /// </summary>
            机票申请 = 1,
            /// <summary>
            /// 财务审核通过（未出票）
            /// </summary>
            审核通过 = 2,
            /// <summary>
            /// 已出票
            /// </summary>
            已出票 = 3,
            /// <summary>
            /// 已退票
            /// </summary>
            已退票 = 4

        }

        /// <summary>
        /// 机票退票状态
        /// </summary>
        public enum TicketRefundSate
        {
            /// <summary>
            /// 退票申请
            /// </summary>
            未退票 = 1,
            /// <summary>
            /// 已退票
            /// </summary>
            已退票 = 4
        }

        /// <summary>
        /// 票款类型
        /// </summary>
        public enum KindType
        {
            /// <summary>
            /// 成人
            /// </summary>
            成人 = 1,
            /// <summary>
            /// 儿童
            /// </summary>
            儿童 = 2
        }

        /// <summary>
        /// 航空公司
        /// </summary>
        public enum FlightCompany
        {
            /// <summary>
            /// 国际航空
            /// </summary>
            国际航空 = 1,
            /// <summary>
            /// 南方航空
            /// </summary>
            南方航空 = 2,
            /// <summary>
            /// 上海航空
            /// </summary>
            上海航空 = 3,
            /// <summary>
            /// 厦门航空
            /// </summary>
            厦门航空 = 4,
            /// <summary>
            /// 东方航空
            /// </summary>
            东方航空 = 5,
            /// <summary>
            /// 山东航空
            /// </summary>
            山东航空 = 6,
            /// <summary>
            /// 深圳航空
            /// </summary>
            深圳航空 = 7,
            /// <summary>
            /// 四川航空
            /// </summary>
            四川航空 = 8,
            /// <summary>
            /// 海南航空
            /// </summary>
            海南航空 = 9,
            /// <summary>
            /// 中联航空
            /// </summary>
            中联航空 = 10,
            /// <summary>
            /// 吉祥航空
            /// </summary>
            吉祥航空,
            /// <summary>
            /// 翔鹏航空
            /// </summary>
            翔鹏航空,
            /// <summary>
            /// 鹰联航空
            /// </summary>
            鹰联航空,
            /// <summary>
            /// 首都航空
            /// </summary>
            首都航空,
            /// <summary>
            /// 昆明航空
            /// </summary>
            昆明航空,
            /// <summary>
            /// 奥凯航空
            /// </summary>
            奥凯航空,
            /// <summary>
            /// 金鹿航空
            /// </summary>
            金鹿航空,
            /// <summary>
            /// 西部航空
            /// </summary>
            西部航空,
            /// <summary>
            /// 新加坡航空
            /// </summary>
            新加坡航空,
            /// <summary>
            /// 华夏航空
            /// </summary>
            华夏航空,
            /// <summary>
            /// 天津航空
            /// </summary>
            天津航空,
            /// <summary>
            /// 大新华航空
            /// </summary>
            大新华航空,
            /// <summary>
            /// 全日空
            /// </summary>
            全日空,
            /// <summary>
            /// 日本航空
            /// </summary>
            日本航空,
            /// <summary>
            /// 西藏航空
            /// </summary>
            西藏航空
        }

        #region 交通类型枚举

        /// <summary>
        /// 交通类型枚举
        /// </summary>
        public enum TrafficType
        {
            /// <summary>
            /// 飞机
            /// </summary>
            飞机 = 0,
            /// <summary>
            /// 火车
            /// </summary>
            火车 = 1,
            /// <summary>
            /// 其它
            /// </summary>
            其它 = 2
        }
        #endregion

        #region 舱位枚举

        /// <summary>
        /// 舱位枚举
        /// </summary>
        public enum Space
        {
            /// <summary>
            /// 公务舱
            /// </summary>
            公务舱 = 0,
            /// <summary>
            /// 头等舱
            /// </summary>
            头等舱 = 1,
            /// <summary>
            /// 经济舱
            /// </summary>
            经济舱 = 2
        }
        #endregion

        #region  交通状态

        /// <summary>
        /// 交通状态
        /// </summary>
        public enum TrafficStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            正常 = 0,
            /// <summary>
            /// 停售
            /// </summary>
            停售 = 1
        }
        #endregion

        #region  票状态

        /// <summary>
        /// 交通下面每天票的状态
        /// </summary>
        public enum TicketStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            正常 = 0,
            /// <summary>
            /// 停售
            /// </summary>
            停售 = 1
        }
        #endregion
    }
}
