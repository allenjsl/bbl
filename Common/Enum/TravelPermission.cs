﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Enum
{
    /// <summary>
    /// 专线商权限
    /// xuty 2011/1/29
    /// </summary>
    public enum TravelPermission
    {

        //个人中心
        //事务提醒
        个人中心_事务提醒_收款提醒栏目 = 1,
        个人中心_事务提醒_付款提醒栏目 = 2,
        个人中心_事务提醒_出团提醒栏目 = 3,
        个人中心_事务提醒_回团提醒栏目 = 4,
        个人中心_事务提醒_合同到期提醒栏目 = 5,

        //公告通知
        个人中心_公告通知_查看公告 = 6,

        //文档管理
        个人中心_文档管理_栏目 = 7,
        个人中心_文档管理_新增 = 215,
        个人中心_文档管理_修改 = 216,
        个人中心_文档管理_删除 = 217,
        个人中心_文档管理_下载文档 = 8,

        //工作交流
        个人中心_工作交流_工作汇报栏目 = 9,
        个人中心_工作交流_工作计划栏目 = 10,
        个人中心_工作交流_交流专区栏目 = 11,



        //线路产品库
        //线路产品库
        线路产品库_线路产品库_栏目 = 12,
        线路产品库_线路产品库_新增线路 = 13,
        线路产品库_线路产品库_修改线路 = 14,
        线路产品库_线路产品库_删除线路 = 15,
        线路产品库_线路产品库_发布计划 = 16,
        线路产品库_线路产品库_我要报价 = 17,



        //散拼计划
        //散拼计划
        散拼计划_散拼计划_栏目 = 18,
        散拼计划_散拼计划_新增计划 = 19,
        散拼计划_散拼计划_修改计划 = 20,
        散拼计划_散拼计划_删除计划 = 21,
        散拼计划_散拼计划_处理订单 = 22,
        散拼计划_散拼计划_安排地接 = 23,
        散拼计划_散拼计划_申请机票 = 24,
        散拼计划_散拼计划_团队结算 = 25,


        //团队计划
        //团队计划
        团队计划_团队计划_栏目 = 26,
        团队计划_团队计划_新增计划 = 27,
        团队计划_团队计划_修改计划 = 28,
        团队计划_团队计划_删除计划 = 29,
        团队计划_团队计划_处理订单 = 30,
        团队计划_团队计划_安排地接 = 31,
        团队计划_团队计划_申请机票 = 32,
        团队计划_团队计划_团队结算 = 33,


        //单项服务
        //单项服务
        单项服务_单项服务_栏目 = 34,
        单项服务_单项服务_新增服务 = 35,
        单项服务_单项服务_修改服务 = 36,
        单项服务_单项服务_删除服务 = 37,
        单项服务_单项服务_提交财务 = 38,



        //销售管理
        //订单中心
        销售管理_订单中心_栏目 = 39,
        销售管理_订单中心_订单修改 = 40,
        销售管理_订单中心_退团操作 = 41,
        销售管理_订单中心_退票申请 = 42,

        //销售收款
        销售管理_销售收款_栏目 = 43,
        销售管理_销售收款_收款登记 = 44,
        销售管理_销售收款_退款登记 = 45,



        //机票管理
        //机票管理
        机票管理_机票管理_栏目 = 46,
        机票管理_机票管理_出票操作 = 47,
        机票管理_机票管理_退票操作 = 48,
        机票管理_机票管理_出票统计 = 49,
        机票管理_机票管理_退票统计 = 50,

        //供应商管理
        //地接
        供应商管理_地接_栏目 = 51,
        供应商管理_地接_新增 = 52,
        供应商管理_地接_修改 = 53,
        供应商管理_地接_删除 = 54,
        供应商管理_地接_导入 = 55,
        供应商管理_地接_导出 = 56,

        //酒店
        供应商管理_酒店_栏目 = 57,
        供应商管理_酒店_新增 = 58,
        供应商管理_酒店_修改 = 59,
        供应商管理_酒店_删除 = 60,
        供应商管理_酒店_导入 = 61,
        供应商管理_酒店_导出 = 62,

        //餐馆
        供应商管理_餐馆_栏目 = 63,
        供应商管理_餐馆_新增 = 64,
        供应商管理_餐馆_修改 = 65,
        供应商管理_餐馆_删除 = 66,
        供应商管理_餐馆_导入 = 67,
        供应商管理_餐馆_导出 = 68,

        //车队
        供应商管理_车队_栏目 = 69,
        供应商管理_车队_新增 = 70,
        供应商管理_车队_修改 = 71,
        供应商管理_车队_删除 = 72,
        供应商管理_车队_导入 = 73,
        供应商管理_车队_导出 = 74,

        //票务
        供应商管理_票务_栏目 = 75,
        供应商管理_票务_新增 = 76,
        供应商管理_票务_修改 = 77,
        供应商管理_票务_删除 = 78,
        供应商管理_票务_导入 = 79,
        供应商管理_票务_导出 = 80,

        //景点
        供应商管理_景点_栏目 = 81,
        供应商管理_景点_新增 = 82,
        供应商管理_景点_修改 = 83,
        供应商管理_景点_删除 = 84,
        供应商管理_景点_导入 = 85,
        供应商管理_景点_导出 = 86,

        //购物
        供应商管理_购物_栏目 = 87,
        供应商管理_购物_新增 = 88,
        供应商管理_购物_修改 = 89,
        供应商管理_购物_删除 = 90,
        供应商管理_购物_导入 = 91,
        供应商管理_购物_导出 = 92,

        //保险
        供应商管理_保险_栏目 = 93,
        供应商管理_保险_新增 = 94,
        供应商管理_保险_修改 = 95,
        供应商管理_保险_删除 = 96,
        供应商管理_保险_导入 = 97,
        供应商管理_保险_导出 = 98,

        //其它
        供应商管理_其它_栏目 = 99,
        供应商管理_其它_新增 = 100,
        供应商管理_其它_修改 = 101,
        供应商管理_其它_删除 = 102,
        供应商管理_其它_导入 = 103,
        供应商管理_其它_导出 = 104,

        //客户关系管理
        //客户资料
        客户关系管理_客户资料_栏目 = 106,
        客户关系管理_客户资料_新增客户 = 107,
        客户关系管理_客户资料_修改客户 = 108,
        客户关系管理_客户资料_删除客户 = 218,
        客户关系管理_客户资料_导入客户 = 110,
        客户关系管理_客户资料_导出客户 = 111,
        客户关系管理_客户资料_分配帐号 = 112,
        客户关系管理_客户资料_停用帐号 = 113,

        //营销活动
        客户关系管理_营销活动_栏目 = 114,
        客户关系管理_营销活动_新增活动 = 115,
        客户关系管理_营销活动_修改活动 = 116,
        客户关系管理_营销活动_删除活动 = 117,

        //客户关怀
        客户关系管理_客户关怀_栏目 = 118,

        //质量管理
        客户关系管理_质量管理_回访栏目 = 119,
        客户关系管理_质量管理_投诉栏目 = 120,
        客户关系管理_质量管理_新增回访 = 121,
        客户关系管理_质量管理_修改回访 = 122,
        客户关系管理_质量管理_删除回访 = 123,
        客户关系管理_质量管理_新增投诉 = 124,
        客户关系管理_质量管理_修改投诉 = 125,
        客户关系管理_质量管理_删除投诉 = 126,

        //销售分析
        客户关系管理_销售分析_人次统计栏目 = 127,
        客户关系管理_销售分析_利润统计栏目 = 128,
        客户关系管理_销售分析_帐龄分析栏目 = 129,



        //财务管理
        //团队核算
        财务管理_团队核算_栏目 = 131,
        财务管理_团队核算_费用变更_控制增减费用 = 132,
        财务管理_团队核算_其它收入添加 = 133,
        财务管理_团队核算_其它支出添加 = 134,
        财务管理_团队核算_利润分配添加 = 135,
        财务管理_团队核算_团队复核 = 136,
        财务管理_团队核算_核算结束 = 137,
        财务管理_团队核算_退回计调 = 138,

        //团款收入
        财务管理_团款收入_栏目 = 139,
        财务管理_团款收入_收款登记 = 140,
        财务管理_团款收入_收款审核 = 141,
        财务管理_团款收入_退款登记 = 142,
        财务管理_团款收入_退款审核 = 143,

        //杂费收入
        财务管理_杂费收入_栏目 = 144,
        财务管理_杂费收入_收入登记 = 145,

        //团款支出
        财务管理_团款支出_栏目 = 146,
        财务管理_团款支出_付款登记 = 147,
        财务管理_团款支出_付款审批 = 148,
        财务管理_团款支出_财务支付 = 149,
        财务管理_团款支出_成本确认 = 150,

        //杂费支出
        财务管理_杂费支出_栏目 = 151,
        财务管理_杂费支出_支出登记 = 152,

        //借款管理
        财务管理_借款管理_栏目 = 153,
        财务管理_借款管理_借款审批 = 154,
        财务管理_借款管理_财务支付 = 155,

        //报销管理
        财务管理_报销管理_栏目 = 156,
        财务管理_报销管理_报销审批 = 157,
        财务管理_报销管理_财务支付 = 158,

        //出纳登帐
        财务管理_出纳登帐_栏目 = 159,
        财务管理_出纳登帐_到款登记 = 160,
        财务管理_出纳登帐_销售销账 = 161,

        //机票审核
        财务管理_机票审核_栏目 = 162,
        财务管理_机票审核_财务审核 = 163,



        //统计分析
        //人次统计
        统计分析_人次统计_人次统计栏目 = 164,

        //员工业绩
        统计分析_员工业绩_员工业绩栏目 = 165,

        //利润统计
        统计分析_利润统计_利润统计栏目 = 166,

        //收入对账单
        统计分析_收入对账单_收入对账单栏目 = 167,

        //支出对账单
        统计分析_支出对账单_支出对账单栏目 = 168,

        //帐龄分析
        统计分析_帐龄分析_帐龄分析栏目 = 169,

        //现金流量
        统计分析_现金流量_现金流量栏目 = 170,
        统计分析_现金流量_现金流量_补充现金 = 219,

        //销售统计
        统计分析_销售统计_销售统计栏目 = 248,
        统计分析_区域销售统计_区域销售栏目 = 249,



        //行政中心
        //职务管理
        行政中心_职务管理_栏目 = 171,
        行政中心_职务管理_新增职务 = 172,
        行政中心_职务管理_修改职务 = 173,
        行政中心_职务管理_删除职务 = 174,

        //人事档案
        行政中心_人事档案_栏目 = 175,
        行政中心_人事档案_新增档案 = 176,
        行政中心_人事档案_修改档案 = 177,
        行政中心_人事档案_删除档案 = 178,

        //考勤管理
        行政中心_考勤管理_栏目 = 179,
        行政中心_考勤管理_考勤登记 = 180,


        //内部通讯录
        行政中心_内部通讯录_栏目 = 181,

        //规章制度
        行政中心_规章制度_栏目 = 182,
        行政中心_规章制度_新增制度 = 183,
        行政中心_规章制度_修改制度 = 184,
        行政中心_规章制度_删除制度 = 185,

        //会议记录
        行政中心_会议记录_栏目 = 186,
        行政中心_会议记录_新增记录 = 187,
        行政中心_会议记录_修改记录 = 188,
        行政中心_会议记录_删除记录 = 189,

        //劳动合同管理
        行政中心_劳动合同管理_栏目 = 190,
        行政中心_劳动合同管理_新增合同 = 191,
        行政中心_劳动合同管理_修改合同 = 192,
        行政中心_劳动合同管理_删除合同 = 193,

        //固定资产管理
        行政中心_固定资产管理_栏目 = 194,
        行政中心_固定资产管理_新增资产 = 195,
        行政中心_固定资产管理_修改资产 = 196,
        行政中心_固定资产管理_删除资产 = 197,

        //培训计划
        行政中心_培训计划_栏目 = 198,
        行政中心_培训计划_新增计划 = 199,
        行政中心_培训计划_修改计划 = 200,
        行政中心_培训计划_删除计划 = 201,



        //短信中心
        //短信中心
        短信中心_短信中心_栏目 = 202,



        //系统设置
        //基础设置
        系统设置_基础设置_城市管理栏目 = 203,
        系统设置_基础设置_线路区域栏目 = 204,
        系统设置_基础设置_报价标准栏目 = 205,
        系统设置_基础设置_客户等级栏目 = 206,
        系统设置_基础设置_品牌管理栏目 = 207,

        //组织机构
        系统设置_组织机构_部门设置栏目 = 208,
        系统设置_组织机构_部门人员栏目 = 209,

        //角色管理
        系统设置_角色管理_角色管理栏目 = 210,

        //公司信息
        系统设置_公司信息_公司信息栏目 = 211,

        //信息管理
        系统设置_信息管理_信息管理栏目 = 212,

        //系统配置
        系统设置_系统配置_系统设置栏目 = 213,

        //系统日志
        系统设置_系统日志_系统日志栏目 = 214,

        散拼计划_散客天天发_栏目 = 220,
        散拼计划_散客天天发_新增 = 221,
        散拼计划_散客天天发_修改 = 222,
        散拼计划_散客天天发_删除 = 223,
        散拼计划_散客天天发_生成计划 = 224,
        团队计划_组团社询价_栏目 = 225,
        团队计划_上传报价_栏目 = 226,
        机票管理_出票统计_栏目 = 227,
        个人中心_送团任务表_栏目 = 228,
        个人中心_留言板_栏目 = 229,
        系统设置_同行平台栏目 = 230,
        /// <summary>
        /// 供应商管理_航空公司栏目
        /// </summary>
        供应商管理_航空公司栏目 = 231,
        /// <summary>
        /// 供应商管理_航空公司新增
        /// </summary>
        供应商管理_航空公司新增 = 232,
        /// <summary>
        /// 供应商管理_航空公司修改
        /// </summary>
        供应商管理_航空公司修改 = 233,
        /// <summary>
        /// 供应商管理_航空公司删除
        /// </summary>
        供应商管理_航空公司删除 = 234,
        /// <summary>
        /// 供应商管理_航空公司导入
        /// </summary>
        供应商管理_航空公司导入 = 235,
        /// <summary>
        /// 供应商管理_航空公司导出
        /// </summary>
        供应商管理_航空公司导出 = 236,

        /// <summary>
        /// 客户关系管理_销售统计
        /// </summary>
        客户关系管理_销售统计_栏目 = 237,

        /// <summary>
        /// 客户关系管理_返佣统计
        /// </summary>
        客户关系管理_返佣统计_栏目 = 238,

        /// <summary>
        /// 财务管理_团队核算_退回财务
        /// </summary>
        财务管理_团队核算_退回财务 = 239,


        /// <summary>
        /// 客户关系管理_客户资料_批量指定责任销售
        /// </summary>
        客户关系管理_客户资料_批量指定责任销售 = 240,
        /// <summary>
        /// 行政中心_考勤管理_工资表
        /// </summary>
        行政中心_考勤管理_工资表 = 241,
        /// <summary>
        /// 机票管理_机票管理_取消出票
        /// </summary>
        机票管理_机票管理_取消出票 = 242,
        /// <summary>
        /// 财务管理_杂费收入_收入审核
        /// </summary>
        财务管理_杂费收入_收入审核 = 243,
        /// <summary>
        /// 财务管理_杂费支出_支出审核
        /// </summary>
        财务管理_杂费支出_支出审核 = 244,
        /// <summary>
        /// 财务管理_团款收入_取消收款审核
        /// </summary>
        财务管理_团款收入_取消收款审核 = 245,
        /// <summary>
        /// 财务管理_团款支出_取消审批
        /// </summary>
        财务管理_团款支出_取消审批 = 246,
        /// <summary>
        /// 财务管理_团款支出_取消支付
        /// </summary>
        财务管理_团款支出_取消支付 = 247,

        /// <summary>
        /// 统计分析_回款率分析_回款率分析栏目
        /// </summary>
        统计分析_回款率分析_回款率分析栏目 = 250,
        /// <summary>
        /// 统计分析_支出账龄分析_支出账龄分析栏目
        /// </summary>
        统计分析_支出账龄分析_支出账龄分析栏目 = 251,
        /// <summary>
        /// 机票管理_机票管理_取消退票
        /// </summary>
        机票管理_机票管理_取消退票 = 252,
        /// <summary>
        /// 财务管理_团款收入_取消退款审核
        /// </summary>
        财务管理_团款收入_取消退款审核 = 253,
        /// <summary>
        /// 个人中心_事务提醒_订单提醒栏目
        /// </summary>
        个人中心_事务提醒_订单提醒栏目 = 254,
        /// <summary>
        /// 统计分析_区域销售统计_查看全部统计数据，有统计分析_区域销售统计_区域销售栏目权限只能查看到用户所在部门的统计数据，有查看全部统计数据的可以看到所有部门的统计数据。
        /// </summary>
        统计分析_区域销售统计_查看全部统计数据 = 255,
        /// <summary>
        /// 财务管理_发票管理_栏目
        /// </summary>
        财务管理_发票管理_栏目 = 256,
        /// <summary>
        /// 财务管理_发票管理_新增
        /// </summary>
        财务管理_发票管理_新增 = 257,
        /// <summary>
        /// 财务管理_发票管理_修改
        /// </summary>
        财务管理_发票管理_修改 = 258,
        /// <summary>
        /// 财务管理_发票管理_删除
        /// </summary>
        财务管理_发票管理_删除 = 259,
        /// <summary>
        /// 统计分析_销售利润统计_销售利润统计栏目
        /// </summary>
        统计分析_销售利润统计_销售利润统计栏目 = 260
    }
}
