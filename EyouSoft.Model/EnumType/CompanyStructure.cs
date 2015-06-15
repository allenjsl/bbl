using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    /// <summary>
    /// 系统设置-相关枚举
    /// </summary>
    /// 李焕超 2011-01-17
    public class CompanyStructure
    {
        #region 客户等级类型
        /// <summary>
        /// 客户等级类型
        /// </summary>
        public enum CustomLevType
        {
            /// <summary>
            /// 门市（系统默认）
            /// </summary>
            门市 = 1,
            /// <summary>
            /// 同行（系统默认）
            /// </summary>
            同行 = 2,
            /// <summary>
            /// 其他（客户添加）
            /// </summary>
            其他 = 3
        }
        #endregion

        #region 公司用户类型
        /// <summary>
        /// 公司用户类型
        /// </summary>
        public enum CompanyUserType
        {
            /// <summary>
            /// 组团用户
            /// </summary>
            组团用户 = 1,
            /// <summary>
            /// 专线用户
            /// </summary>
            专线用户 = 2,
            /// <summary>
            /// 地接用户
            /// </summary>
            地接用户 = 3
        }
        #endregion

        #region 性别
        /// <summary>
        /// 性别
        /// </summary>
        public enum Sex
        {
            /// <summary>
            /// 未知
            /// </summary>
            未知 = 0,
            /// <summary>
            /// 女
            /// </summary>
            女 = 1,
            /// <summary>
            /// 男
            /// </summary>
            男 = 2
        }
        #endregion

        #region 返佣类型
        /// <summary>
        /// 返佣类型
        /// </summary>
        public enum CommissionType
        {
            未确定 = 0,
            /// <summary>
            /// 现返
            /// </summary>
            现返 = 1,
            /// <summary>
            /// 后返
            /// </summary>
            后返 = 2
        }
        #endregion

        #region Satisfaction
        /// <summary>
        /// 客户回访满意度
        /// </summary>
        public enum Satisfaction
        {
            /// <summary>
            /// 满意
            /// </summary>
            满意 = 0,
            /// <summary>
            /// 一般
            /// </summary>
            一般 = 1,
            /// <summary>
            /// 不满意
            /// </summary>
            不满意 = 2
        }
        #endregion

        #region 营销活动状态
        /// <summary>
        /// 营销活动状态
        /// </summary>
        public enum ActivityState
        {
            /// <summary>
            /// 准备阶段
            /// </summary>
            准备阶段 = 0,
            /// <summary>
            /// 进行中
            /// </summary>
            进行中 = 1,
            /// <summary>
            /// 活动结束
            /// </summary>
            活动结束 = 2

        }
        #endregion

        #region 供应商类型
        /// <summary>
        /// 供应商类型
        /// </summary>
        public enum SupplierType
        {
            /// <summary>
            /// 地接
            /// </summary>
            地接 = 1,
            /// <summary>
            /// 票务
            /// </summary>
            票务 = 2,
            /// <summary>
            /// 酒店
            /// </summary>
            酒店 = 3,
            /// <summary>
            /// 餐馆
            /// </summary>
            餐馆 = 4,
            /// <summary>
            /// 车队
            /// </summary>
            车队 = 5,
            /// <summary>
            /// 景点
            /// </summary>
            景点 = 6,
            /// <summary>
            /// 购物
            /// </summary>
            购物 = 7,
            /// <summary>
            /// 保险
            /// </summary>
            保险 = 8,
            /// <summary>
            /// 其他
            /// </summary>
            其他 = 9,
            /// <summary>
            /// 航空公司
            /// </summary>
            航空公司 = 10

        }
        #endregion

        #region 机票票款计算公式
        /// <summary>
        /// 机票票款计算公式
        /// </summary>
        public enum AgencyFeeType
        {
            /// <summary>
            /// (票面价+机建燃油)*人数+代理费
            /// </summary>
            公式一 = 1,
            /// <summary>
            /// (票面价+机建燃油)*人数-代理费
            /// </summary>
            公式二,
            /// <summary>
            /// （票面价*百分比+机建燃油）*人数+其它费用
            /// </summary>
            公式三
        }
        #endregion

        #region 系统配置价格组成类型
        /// <summary>
        /// 系统配置价格组成类型
        /// </summary>
        public enum PriceComponent
        {
            /// <summary>
            /// 分项报价
            /// </summary>
            分项报价 = 1,
            /// <summary>
            /// 统一报价
            /// </summary>
            统一报价 = 2
        }
        #endregion

        #region 行程打印单类型
        /// <summary>
        /// 行程打印单类型
        /// </summary>
        public enum PrintTemplateType
        {
            /// <summary>
            /// 散客确认单
            /// </summary>
            散客确认单 = 3,
            /// <summary>
            /// 团队确认单
            /// </summary>
            团队确认单 = 4,
            /// <summary>
            /// 报价单
            /// </summary>
            报价单 = 5,
            /// <summary>
            /// 送团单
            /// </summary>
            送团单 = 6,
            /// <summary>
            /// 结算通知单
            /// </summary>
            结算通知单 = 7,
            /// <summary>
            /// 结算明细单
            /// </summary>
            结算明细单 = 8,
            /// <summary>
            /// 地接确认单
            /// </summary>
            地接确认单 = 9,
            /// <summary>
            /// 机票确认单
            /// </summary>
            机票确认单 = 10,
            /// <summary>
            /// 订房确认单
            /// </summary>
            订房确认单 = 11,
            /// <summary>
            /// 订票确认单
            /// </summary>
            订票确认单 = 12,
            /// <summary>
            /// 门票确认单
            /// </summary>
            门票确认单 = 13,
            /// <summary>
            /// 保险确认单
            /// </summary>
            保险确认单 = 14,
            /// <summary>
            /// 签证确认单
            /// </summary>
            签证确认单 = 15,
            /// <summary>
            /// 租车确认单
            /// </summary>
            租车确认单 = 16,
            /// <summary>
            /// 内部交款单
            /// </summary>
            内部交款单 = 17,
            /// <summary>
            /// 打印收据
            /// </summary>
            打印收据 = 18,
            /// <summary>
            /// 付款申请单
            /// </summary>
            付款申请单 = 19,
            /// <summary>
            /// 线路快速发布行程单
            /// </summary>
            线路快速发布行程单 = 20,
            /// <summary>
            /// 线路标准发布行程单
            /// </summary>
            线路标准发布行程单 = 21,
            /// <summary>
            /// 散拼计划快速发布行程单
            /// </summary>
            散拼计划快速发布行程单 = 22,
            /// <summary>
            /// 散拼计划标准发布行程单
            /// </summary>
            散拼计划标准发布行程单 = 23,
            /// <summary>
            /// 团队计划快速发布行程单
            /// </summary>
            团队计划快速发布行程单 = 24,
            /// <summary>
            /// 团队计划标准发布行程单
            /// </summary>
            团队计划标准发布行程单 = 25,
            /// <summary>
            /// 散拼计划打印名单
            /// </summary>
            散拼计划打印名单 = 26,
            /// <summary>
            /// 团队计划打印名单
            /// </summary>
            团队计划打印名单 = 27,
            /// <summary>
            /// 订单中心打印名单
            /// </summary>
            订单中心打印名单 = 28,
            /// <summary>
            /// 组团订单打印名单
            /// </summary>
            组团订单打印名单 = 29,
            /// <summary>
            /// 组团线路快速发布行程单
            /// </summary>
            组团线路快速发布行程单 = 30,
            /// <summary>
            /// 组团线路标准发布行程单
            /// </summary>
            组团线路标准发布行程单 = 31,
            /// <summary>
            /// 组团线路打印名单
            /// </summary>
            组团线路打印名单 = 32,
            /// <summary>
            /// 单项服务确认单
            /// </summary>
            单项服务确认单=33,
            /// <summary>
            /// 送团任务单
            /// </summary>
            送团任务单=34,
            /// <summary>
            /// 团队计算明细单
            /// </summary>
            团队结算明细单=35
        }
        #endregion

        #region 客户关怀固定特殊节日发送
        /// <summary>
        /// 客户关怀固定特殊节日发送
        /// </summary>
        public enum CustomerCareForSendSpecialTime
        {
            无 = 0,
            生日 = 1,
            元旦 = 2,
            春节 = 3,
            元宵 = 4,
            五一 = 5,
            国庆 = 6,
            中秋 = 7,
            圣诞 = 8

        }
        #endregion

        #region 回访或投诉
        /// <summary>
        /// 回访或投诉
        /// </summary>
        public enum CallBackType
        {
            回访 = 1,
            投诉 = 2

        }
        #endregion

        #region 统计订单的方式
        /// <summary>
        /// 统计订单的方式
        /// </summary>
        public enum ComputeOrderType
        {
            /// <summary>
            /// 统计确认成交订单
            /// </summary>
            统计确认成交订单 = 1,
            /// <summary>
            /// 统计有效订单（除不受理、留位过期以外所有）
            /// </summary>
            统计有效订单 = 2
        }
        #endregion

        #region 权限小类枚举

        /// <summary>
        /// 权限小类枚举
        /// </summary>
        public enum SysPermissionClass
        {
            /// <summary>
            ///个人中心_事务提醒
            /// </summary>
            个人中心_事务提醒 = 1,


            /// <summary>
            ///个人中心_公告通知
            /// </summary>
            个人中心_公告通知 = 2,


            /// <summary>
            ///个人中心_文档管理
            /// </summary>
            个人中心_文档管理 = 3,


            /// <summary>
            ///个人中心_工作交流
            /// </summary>
            个人中心_工作交流 = 4,


            /// <summary>
            ///线路产品库_线路产品库
            /// </summary>
            线路产品库_线路产品库 = 5,


            /// <summary>
            ///散拼计划_散拼计划
            /// </summary>
            散拼计划_散拼计划 = 6,


            /// <summary>
            ///团队计划_团队计划
            /// </summary>
            团队计划_团队计划 = 7,


            /// <summary>
            ///单项服务_单项服务
            /// </summary>
            单项服务_单项服务 = 8,


            /// <summary>
            ///销售管理_订单中心
            /// </summary>
            销售管理_订单中心 = 9,


            /// <summary>
            ///销售管理_销售收款
            /// </summary>
            销售管理_销售收款 = 10,


            /// <summary>
            ///机票管理_机票管理
            /// </summary>
            机票管理_机票管理 = 11,


            /// <summary>
            ///供应商管理_地接
            /// </summary>
            供应商管理_地接 = 12,


            /// <summary>
            ///供应商管理_酒店
            /// </summary>
            供应商管理_酒店 = 13,


            /// <summary>
            ///供应商管理_餐馆
            /// </summary>
            供应商管理_餐馆 = 14,


            /// <summary>
            ///供应商管理_车队
            /// </summary>
            供应商管理_车队 = 15,


            /// <summary>
            ///供应商管理_票务
            /// </summary>
            供应商管理_票务 = 16,


            /// <summary>
            ///供应商管理_景点
            /// </summary>
            供应商管理_景点 = 17,


            /// <summary>
            ///供应商管理_购物
            /// </summary>
            供应商管理_购物 = 18,


            /// <summary>
            ///供应商管理_保险
            /// </summary>
            供应商管理_保险 = 19,


            /// <summary>
            ///供应商管理_其它
            /// </summary>
            供应商管理_其它 = 20,


            /// <summary>
            ///客户关系管理_客户资料
            /// </summary>
            客户关系管理_客户资料 = 21,


            /// <summary>
            ///客户关系管理_营销活动
            /// </summary>
            客户关系管理_营销活动 = 22,


            /// <summary>
            ///客户关系管理_客户关怀
            /// </summary>
            客户关系管理_客户关怀 = 23,


            /// <summary>
            ///客户关系管理_质量管理
            /// </summary>
            客户关系管理_质量管理 = 24,


            /// <summary>
            ///客户关系管理_销售分析
            /// </summary>
            客户关系管理_销售分析 = 25,


            /// <summary>
            ///财务管理_团队核算
            /// </summary>
            财务管理_团队核算 = 26,


            /// <summary>
            ///财务管理_团款收入
            /// </summary>
            财务管理_团款收入 = 27,


            /// <summary>
            ///财务管理_杂费收入
            /// </summary>
            财务管理_杂费收入 = 28,


            /// <summary>
            ///财务管理_团款支出
            /// </summary>
            财务管理_团款支出 = 29,


            /// <summary>
            ///财务管理_杂费支出
            /// </summary>
            财务管理_杂费支出 = 30,


            /// <summary>
            ///财务管理_借款管理
            /// </summary>
            财务管理_借款管理 = 31,


            /// <summary>
            ///财务管理_报销管理
            /// </summary>
            财务管理_报销管理 = 32,


            /// <summary>
            ///财务管理_出纳登帐
            /// </summary>
            财务管理_出纳登帐 = 33,


            /// <summary>
            ///财务管理_机票审核
            /// </summary>
            财务管理_机票审核 = 34,


            /// <summary>
            ///统计分析_人次统计
            /// </summary>
            统计分析_人次统计 = 35,


            /// <summary>
            ///统计分析_员工业绩
            /// </summary>
            统计分析_员工业绩 = 36,


            /// <summary>
            ///统计分析_利润统计
            /// </summary>
            统计分析_利润统计 = 37,


            /// <summary>
            ///统计分析_收入对账单
            /// </summary>
            统计分析_收入对账单 = 38,


            /// <summary>
            ///统计分析_支出对账单
            /// </summary>
            统计分析_支出对账单 = 39,


            /// <summary>
            ///统计分析_帐龄分析
            /// </summary>
            统计分析_帐龄分析 = 40,


            /// <summary>
            ///统计分析_现金流量
            /// </summary>
            统计分析_现金流量 = 41,


            /// <summary>
            ///行政中心_职务管理
            /// </summary>
            行政中心_职务管理 = 42,


            /// <summary>
            ///行政中心_人事档案
            /// </summary>
            行政中心_人事档案 = 43,


            /// <summary>
            ///行政中心_考勤管理
            /// </summary>
            行政中心_考勤管理 = 44,


            /// <summary>
            ///行政中心_内部通讯录
            /// </summary>
            行政中心_内部通讯录 = 45,


            /// <summary>
            ///行政中心_规章制度
            /// </summary>
            行政中心_规章制度 = 46,


            /// <summary>
            ///行政中心_会议记录
            /// </summary>
            行政中心_会议记录 = 47,


            /// <summary>
            ///行政中心_劳动合同管理
            /// </summary>
            行政中心_劳动合同管理 = 48,


            /// <summary>
            ///行政中心_固定资产管理
            /// </summary>
            行政中心_固定资产管理 = 49,


            /// <summary>
            ///行政中心_培训计划
            /// </summary>
            行政中心_培训计划 = 50,


            /// <summary>
            ///短信中心_短信中心
            /// </summary>
            短信中心_短信中心 = 51,


            /// <summary>
            ///系统设置_基础设置
            /// </summary>
            系统设置_基础设置 = 52,


            /// <summary>
            ///系统设置_组织机构
            /// </summary>
            系统设置_组织机构 = 53,


            /// <summary>
            ///系统设置_角色管理
            /// </summary>
            系统设置_角色管理 = 54,


            /// <summary>
            ///系统设置_公司信息
            /// </summary>
            系统设置_公司信息 = 55,


            /// <summary>
            ///系统设置_信息管理
            /// </summary>
            系统设置_信息管理 = 56,


            /// <summary>
            ///系统设置_系统配置
            /// </summary>
            系统设置_系统配置 = 57,


            /// <summary>
            ///系统设置_系统日志
            /// </summary>
            系统设置_系统日志 = 58,
            /// <summary>
            /// 散拼计划_散客天天发
            /// </summary>
            散拼计划_散客天天发 = 59,
            /// <summary>
            /// 团队计划_组团社询价
            /// </summary>
            团队计划_组团社询价 = 60,
            /// <summary>
            /// 团队计划_上传报价
            /// </summary>
            团队计划_上传报价 = 61,
            /// <summary>
            /// 机票管理_出票统计
            /// </summary>
            机票管理_出票统计 = 62,
            /// <summary>
            /// 个人中心_送团任务表
            /// </summary>
            个人中心_送团任务表 = 63,
            /// <summary>
            /// 个人中心_留言板
            /// </summary>
            个人中心_留言板 = 64,

            /// <summary>
            /// 系统设置_同行平台
            /// </summary>
            系统设置_同行平台 = 65,

            /// <summary>
            /// 供应商管理_航空公司
            /// </summary>
            供应商管理_航空公司 = 66,

            /// <summary>
            ///客户关系管理_销售统计
            /// </summary>
            客户关系管理_销售统计 = 67,
            /// <summary>
            /// 客户关系管理_返佣统计
            /// </summary>
            客户关系管理_返佣统计 = 68,
            /// <summary>
            /// 财务管理_发票管理
            /// </summary>
            财务管理_发票管理 = 73,
            /// <summary>
            /// 统计分析_销售利润统计
            /// </summary>
            统计分析_销售利润统计 = 74,
            /// <summary>
            /// 机票管理_交通管理
            /// </summary>
            机票管理_交通管理=75
        }

        #endregion

        #region 留言回复状态
        /// <summary>
        /// 留言回复状态
        /// </summary>
        public enum ReplyState
        {
            /// <summary>
            /// 未回复
            /// </summary>
            未回复 = 0,
            /// <summary>
            /// 已回复
            /// </summary>
            已回复 = 1
        }
        #endregion

        #region 申请机票游客勾选配置
        /// <summary>
        /// 申请机票游客勾选配置
        /// </summary>
        public enum TicketTravellerCheckedType
        {
            /// <summary>
            /// 不管是否有退票均可申请
            /// </summary>
            None=0,
            /// <summary>
            /// 有退票（有一个或多个航段退票）即可申请
            /// </summary>
            LeastOne,
            /// <summary>
            /// 退完票（所有航段均退票）才可申请
            /// </summary>
            All
        }
        #endregion

        #region 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板类型
        /// <summary>
        /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板类型
        /// </summary>
        public enum NotepadServiceType
        {
            /// <summary>
            /// 包含项目地接
            /// </summary>
            DiJie=1,
            /// <summary>
            /// 包含项目酒店
            /// </summary>
            JiuDian,
            /// <summary>
            /// 包含项目用餐
            /// </summary>
            YongCan,
            /// <summary>
            /// 包含项目小交通
            /// </summary>
            XiaoJiaoTong,
            /// <summary>
            /// 包含项目大交通
            /// </summary>
            DaJiaoTong,
            /// <summary>
            /// 包含项目景点
            /// </summary>
            JingDian,
            /// <summary>
            /// 包含项目购物
            /// </summary>
            GouWu,
            /// <summary>
            /// 包含项目保险
            /// </summary>
            BaoXian,
            /// <summary>
            /// 包含项目其它
            /// </summary>
            QiTa,
            /// <summary>
            /// 包含项目导服
            /// </summary>
            DaoFu,
            /// <summary>
            /// 不含项目
            /// </summary>
            BuHanXiangMu,
            /// <summary>
            /// 购物安排
            /// </summary>
            GouWuAnPai,
            /// <summary>
            /// 儿童安排 
            /// </summary>
            ErTongAnPai,
            /// <summary>
            /// 自费项目
            /// </summary>
            ZiFeiXIangMu,
            /// <summary>
            /// 注意事项
            /// </summary>
            ZhuYiShiXiang,
            /// <summary>
            /// 温馨提醒
            /// </summary>
            WenXinTiXing,
            /// <summary>
            /// 内部信息
            /// </summary>
            NeiBuXingXi,
            /// <summary>
            /// 集合时间
            /// </summary>
            JiHeShiJian,
            /// <summary>
            /// 集合地点
            /// </summary>
            JiHeDiDian,
            /// <summary>
            /// 集合标志
            /// </summary>
            JiHeBiaoZhi
        }
        #endregion

        #region 客户关系结算日类型
        /// <summary>
        /// 客户关系结算日类型
        /// </summary>
        public enum AccountDayType
        {
            /// <summary>
            /// 按每月X日
            /// </summary>
            Month,
            /// <summary>
            /// 按每周星期X
            /// </summary>
            Week
        }
        #endregion

        #region 收款提醒配置
        /// <summary>
        /// 收款提醒配置
        /// </summary>
        public enum ReceiptRemindType
        {
            /// <summary>
            /// 提醒全部用户
            /// </summary>
            AllUser,
            /// <summary>
            /// 仅提醒销售员
            /// </summary>
            OnlySeller
        }
        #endregion

        #region 客户资料列表排列字段
        /// <summary>
        /// 客户资料列表排列字段
        /// </summary>
        public enum CustomerOrderByField
        {
            /// <summary>
            /// 创建时间
            /// </summary>
            创建时间,
            /// <summary>
            /// 客户名称
            /// </summary>
            客户名称,
            /// <summary>
            /// 主要联系人名称
            /// </summary>
            主要联系人名称,
            /// <summary>
            /// 主要联系人电话
            /// </summary>
            主要联系人电话,
            /// <summary>
            /// 主要联系人手机
            /// </summary>
            主要联系人手机,
            /// <summary>
            /// 主要联系人传真
            /// </summary>
            主要联系人传真,
            /// <summary>
            /// 责任销售
            /// </summary>
            责任销售,
            /// <summary>
            /// 交易次数
            /// </summary>
            交易次数,
            /// <summary>
            /// 结算方式
            /// </summary>
            结算方式,
            /// <summary>
            /// 地址
            /// </summary>
            地址,
            /// <summary>
            /// 省份
            /// </summary>
            省份
        }
        #endregion

        #region 排序方式
        /// <summary>
        /// 排序方式
        /// </summary>
        public enum OrderByType
        {
            /// <summary>
            /// 升序
            /// </summary>
            ASC,
            /// <summary>
            /// 降序
            /// </summary>
            DESC
        }
        #endregion

        #region 团队计划人数配置
        /// <summary>
        /// 团队计划人数配置
        /// </summary>
        public enum TeamNumberOfPeople
        {
            /// <summary>
            /// 仅总人数
            /// </summary>
            OnlyTotalNumber=0,
            /// <summary>
            /// 成人、儿童、全陪人数
            /// </summary>
            PartNumber
        }
        #endregion

        #region 机票售票处填写时间
        /// <summary>
        /// 机票售票处填写时间
        /// </summary>
        public enum TicketOfficeFillTime
        {
            /// <summary>
            /// 出票，审核后才可填写
            /// </summary>
            Ticket,
            /// <summary>
            /// 申请，申请时就可填写
            /// </summary>
            Apply
        }
        #endregion

        #region 团队展示方式
        /// <summary>
        /// 团队展示方式
        /// </summary>
        public enum TourDisplayType
        {
            /// <summary>
            /// 明细团
            /// </summary>
            明细团 = 0,
            /// <summary>
            /// 子母团
            /// </summary>
            子母团
        }
        #endregion

        #region 客户单位结算方式
        /// <summary>
        /// 客户单位结算方式
        /// </summary>
        public enum KHJieSuanType
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// 单团结算
            /// </summary>
            单团结算 = 1,
            /// <summary>
            /// 月结
            /// </summary>
            月结 = 2
        }
        #endregion
    }
}
