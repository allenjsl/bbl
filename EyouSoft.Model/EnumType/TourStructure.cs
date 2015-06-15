/*Author:汪奇志 2011-01-18*/
using System;

namespace EyouSoft.Model.EnumType
{
    public class TourStructure
    {
        #region 团队状态
        /// <summary>
        /// 团队状态
        /// </summary>
        public enum TourStatus
        {
            /// <summary>
            /// 正在收客，单项服务操作中
            /// </summary>
            正在收客 = 0,
            /// <summary>
            /// 停止收客(客满)
            /// </summary>
            停止收客 = 1,
            /// <summary>
            /// 行程途中
            /// </summary>
            行程途中 = 2,
            /// <summary>
            /// 回团报账
            /// </summary>
            回团报账 = 3,
            /// <summary>
            /// 财务核算
            /// </summary>
            财务核算 = 4,
            /// <summary>
            /// 核算结束
            /// </summary>
            核算结束 = 5
        }
        #endregion

        #region 团队类型
        /// <summary>
        /// 团队类型
        /// </summary>
        public enum TourType
        {            
            /// <summary>
            /// 散拼计划
            /// </summary>
            散拼计划 = 0,
            /// <summary>
            /// 团队计划
            /// </summary>
            团队计划 = 1,
            /// <summary>
            /// 单项服务
            /// </summary>
            单项服务 = 2
        }
        #endregion

        #region 订单相关枚举
        /// <summary>
        /// 收退款方式
        /// </summary>
        public enum RefundType
        {
            /// <summary>
            /// 财务现收
            /// </summary>
            财务现收 = 1,
            /// <summary>
            /// 财务现付
            /// </summary>
            财务现付 = 2,
            /// <summary>
            /// 导游现收
            /// </summary>
            导游现收 = 3,
            /// <summary>
            /// 银行电汇
            /// </summary>
            银行电汇 = 4,
            /// <summary>
            /// 转账支票
            /// </summary>
            转账支票 = 5,
            /// <summary>
            /// 导游现付
            /// </summary>
            导游现付 = 6,
            /// <summary>
            /// 签单挂账
            /// </summary>
            签单挂账 = 7,
            /// <summary>
            /// 预存款支付
            /// </summary>
            预存款支付 = 8,
            /// <summary>
            /// 银行到账(私人)
            /// </summary>
            银行到账_私人 = 9
        }

        /// <summary>
        /// 订单来源类型
        /// </summary>
        public enum OrderType
        {
            /// <summary>
            /// 组团下单
            /// </summary>
            组团下单 = 0,
            /// <summary>
            /// 代客预定
            /// </summary>
            代客预定 = 1,
            /// <summary>
            /// 团队计划
            /// </summary>
            团队计划 = 2,
            /// <summary>
            /// 单项服务
            /// </summary>
            单项服务 = 3
        }

        /// <summary>
        /// 订单状态枚举
        /// </summary>
        public enum OrderState
        {
            /// <summary>
            /// 未处理
            /// </summary>
            未处理 = 1,
            /// <summary>
            /// 已留位
            /// </summary>
            已留位 = 2,
            /// <summary>
            /// 留位过期
            /// </summary>
            留位过期 = 3,
            /// <summary>
            /// 不受理
            /// </summary>
            不受理 = 4,
            /// <summary>
            /// 已成交
            /// </summary>
            已成交 = 5
        }

        /// <summary>
        /// 订单游客显示类型
        /// </summary>
        public enum CustomerDisplayType
        {
            /// <summary>
            /// 未知
            /// </summary>
            None = 0,
            /// <summary>
            /// 附件方式
            /// </summary>
            附件方式 = 1,
            /// <summary>
            /// 输入方式
            /// </summary>
            输入方式 = 2
        }
        /// <summary>
        /// 收退款关联编号类型
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// 订单编号
            /// </summary>
            订单 = 1,
            /// <summary>
            /// 团款其他收入
            /// </summary>
            团款其他收入 = 2
        }

        /// <summary>
        /// 团队上团数和收客数修改类型
        /// </summary>
        public enum UpdateTourType
        {
            /// <summary>
            /// 修改上团数
            /// </summary>
            修改上团数 = 0,
            /// <summary>
            /// 修改收客数
            /// </summary>
            修改收客数 = 1,
            /// <summary>
            /// 修改上团数和收客数
            /// </summary>
            修改上团数和收客数 = 2
        }
        /// <summary>
        /// 日志区别收款/退款操作源头
        /// </summary>
        public enum LogRefundSource
        {
            /// <summary>
            /// 销售管理中进行的销售收款/退款操作
            /// </summary>
            销售管理_销售收款,
            /// <summary>
            /// 财务管理中进行的销售收款/退款操作
            /// </summary>
            财务管理_团款收入
        }
        #endregion 订单相关枚举

        #region 订单游客相关枚举
        /// <summary>
        /// 订单游客退团状态枚举
        /// </summary>
        public enum CustomerStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            正常 = 0,
            /// <summary>
            /// 已退团
            /// </summary>
            已退团 = 1,
        }
        /// <summary>
        /// 订单游客退票状态枚举
        /// </summary>
        public enum CustomerTicketStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            正常 = 0,
            /// <summary>
            /// 退票未审核
            /// </summary>
            退票未审核 = 1,
            /// <summary>
            /// 已退票
            /// </summary>
            退票已审核 = 2

        }
        /// <summary>
        /// 游客类型
        /// </summary>
        public enum VisitorType
        {
            /// <summary>
            /// 未知
            /// </summary>
            未知 = 0,
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
        /// 证件类型枚举
        /// </summary>
        public enum CradType
        {
            /// <summary>
            /// 未知
            /// </summary>
            未知 = 0,
            /// <summary>
            /// 身份证
            /// </summary>
            身份证 = 1,
            /// <summary>
            /// 护照
            /// </summary>
            护照 = 2,
            /// <summary>
            /// 军官证
            /// </summary>
            军官证 = 3,
            /// <summary>
            /// 台胞证
            /// </summary>
            台胞证 = 4,
            /// <summary>
            /// 港澳通行证
            /// </summary>
            港澳通行证 = 5,
            /// <summary>
            /// 户口本
            /// </summary>
            户口本=6
        }
        #endregion

        #region 计划及线路发布类型
        /// <summary>
        /// 计划及线路发布类型
        /// </summary>
        public enum ReleaseType
        {
            /// <summary>
            /// 标准发布
            /// </summary>
            Normal = 0,
            /// <summary>
            /// 快速发布
            /// </summary>
            Quick
        }
        #endregion

        #region 包含项目项目类型
        /// <summary>
        /// 包含项目项目类型
        /// </summary>
        public enum ServiceType
        {
            /// <summary>
            /// 地接
            /// </summary>
            地接 = 0,
            /// <summary>
            /// 酒店
            /// </summary>
            酒店 = 1,
            /// <summary>
            /// 用餐
            /// </summary>
            用餐 = 2,
            /// <summary>
            /// 景点
            /// </summary>
            景点 = 3,
            /// <summary>
            /// 保险
            /// </summary>
            保险 = 4,
            /// <summary>
            /// 大交通
            /// </summary>
            大交通 = 5,
            /// <summary>
            /// 导服
            /// </summary>
            导服 = 6,
            /// <summary>
            /// 购物
            /// </summary>
            购物 = 7,
            /// <summary>
            /// 小交通
            /// </summary>
            小交通 = 8,
            /// <summary>
            /// 其它
            /// </summary>
            其它 = 9,
            /// <summary>
            /// 签证
            /// </summary>
            签证
        }
        #endregion

        #region 散拼计划建团规则
        /// <summary>
        /// 散拼计划建团规则
        /// </summary>
        public enum CreateTourRule
        {
            /// <summary>
            /// 按星期
            /// </summary>
            按星期,
            /// <summary>
            /// 按天数
            /// </summary>
            按天数,
            /// <summary>
            /// 按日期
            /// </summary>
            按日期
        }
        #endregion

        #region 计划排序方式
        /// <summary>
        /// 计划排序方式
        /// </summary>
        public enum TourSortType
        {
            /// <summary>
            /// 默认 未出团在前，已出团在后，均按出团日期升序
            /// </summary>
            默认 = 0,
            /// <summary>
            /// 出团日期升序
            /// </summary>
            出团日期升序,
            /// <summary>
            /// 出团日期降序
            /// </summary>
            出团日期降序,
            /// <summary>
            /// 收客状态升序
            /// </summary>
            收客状态升序,
            /// <summary>
            /// 收客状态降序
            /// </summary>
            收客状态降序
        }
        #endregion

        #region 散客天天发申请处理状态
        /// <summary>
        /// 散客天天发申请处理状态
        /// </summary>
        public enum TourEverydayHandleStatus
        {
            /// <summary>
            /// 未处理
            /// </summary>
            未处理 = 0,
            /// <summary>
            /// 已处理
            /// </summary>
            已处理 = 1
        }
        #endregion

        #region 线路询价状态
        /// <summary>
        /// 线路询价状态
        /// </summary>
        public enum QuoteState
        {
            /// <summary>
            /// 未处理,专线端未做任何处理
            /// </summary>
            未处理 = 0,
            /// <summary>
            /// 未成功，专线端修改了，但未成团
            /// </summary>
            已处理 = 1,
            /// <summary>
            /// 已成功，专线端已报价且已生成团
            /// </summary>
            已成功 = 2
        }
        #endregion

        #region 团队推广状态
        /// <summary>
        /// 团队推广状态
        /// </summary>
        public enum TourRouteStatus
        {
            /// <summary>
            /// 无
            /// </summary>
            无 = 0,
            /// <summary>
            /// 特价
            /// </summary>
            特价 = 1,
            /// <summary>
            /// 促销
            /// </summary>
            促销 = 2
        }
        #endregion

        #region 手动设置的计划状态
        /// <summary>
        /// 手动设置的计划状态
        /// </summary>
        public enum HandStatus
        {
            /// <summary>
            /// 无
            /// </summary>
            无 = 0,
            /// <summary>
            /// 手动停收
            /// </summary>
            手动停收 = 1,
            /// <summary>
            /// 手动客满
            /// </summary>
            手动客满 = 2
        }
        #endregion
    }
}
