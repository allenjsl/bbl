using System;
using System.Data;
using System.Collections.Generic;
namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 数据访问类Customer,客户关系管理
    /// autor:李焕超
    /// date :2011-1-17
    /// </summary>
    public interface ICustomer
    {
        //客户资料方法
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool ExistsCustomer(int Id);
        /// <summary>
        ///  增加一条数据
        /// </summary>
        int AddCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model);

        /// <summary>
        ///  增加多条数据
        /// </summary>
        bool AddCustomerMore(IList<EyouSoft.Model.CompanyStructure.CustomerInfo> modelList);

        /// <summary>
        ///  更新一条数据
        /// </summary>
        int UpdateCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model);

        /// <summary>
        ///  更新组团端客户资料数据
        /// </summary>
        bool UpdateSampleCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model);

        /// <summary>
        ///  更新组团端客户资料配置管理数据
        /// </summary>
        bool UpdateSampleCustomerConfig(EyouSoft.Model.CompanyStructure.CustomerConfig model);

        /// <summary>
        /// 更新当前登录游客的电话和手机
        /// </summary>
        /// <param name="Model">只赋值编号，电话和手机</param>
        /// <returns></returns>
        bool UpDateSampleCompanyUserInfo(EyouSoft.Model.CompanyStructure.CompanyUser Model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool DeleteCustomer(int Id);


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerModel(int Id);

        /// <summary>
        /// 得到客户的配置信息对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerConfig GetCustomerConfigModel(int Id);

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        bool StopIt(int CustomerId);

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        bool StartId(int CustomerId);


        #endregion  成员方法

        //客户联系人方法
        #region  成员方法
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerContactInfo GetCustomerContactModel(int Id);


        /// <summary>
        /// 获得数据列表
        /// </summary>
        IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> GetCustomerContactList(int CustomerId);

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        bool DeleteCustomerContacter(params string[] Ids);


        #endregion  成员方法

        //客户回访方法
        #region  成员方法

        /// <summary>
        ///  增加一条数据
        /// </summary>
        bool AddCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model);

        /// <summary>
        ///  更新一条数据
        /// </summary>
        bool UpdateCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool DeleteCustomerCallBack(int Id);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        bool DeleteCustomerCallBackMore(params string[] Id);


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerCallBackInfo GetCustomerCallBackModel(int Id);


        /// <summary>
        /// 获得数据列表
        /// </summary>
        IList<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> GetCustomerCallBackList(EyouSoft.Model.EnumType.CompanyStructure.CallBackType CallType, int PageSize, int PageIndex, int CompanyID, string strWhere, string filedOrder, ref int RecordCount, ref int PageCount);


        #endregion  成员方法

        #region 客户回访结果方法

      
        /// <summary>
        ///  更新一条数据
        /// </summary>
        bool UpdateCustomerCallbackResult(EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool DeleteCustomerCallbackResult(int Id);


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo GetCustomerCallbackResultModel(int Id);


        /// <summary>
        /// 获得数据列表
        /// </summary>
        IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> GetCustomerCallbackResultList(int CallBackId);



        #endregion  成员方法

        //客户营销活动方法
        #region  成员方法

        /// <summary>
        ///  增加一条数据
        /// </summary>
        bool AddCustomerMarketing(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model);

        /// <summary>
        ///  更新一条数据
        /// </summary>
        bool UpdateCustomerMarketing(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool DeleteCustomerMarketing(int Id);

        /// <summary>
        /// 删除多条数据
        /// </summary>
        bool DeleteCustomerMarketingS(int[] Id);


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        EyouSoft.Model.CompanyStructure.CustomerMarketingInfo GetCustomerMarketingModel(int Id);


        /// <summary>
        /// 获得数据列表
        /// </summary>
        IList<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo> GetCustomerMarketingList(int PageSize, int PageIndex, int CompanyID, string strWhere, string filedOrder, ref int RecordCount, ref int PageCount);


        #endregion  成员方法

        #region 收款提醒方法

        /// <summary>
        /// 分页获取收款提醒-全部用户
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> GetReceiptRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, int accountDay, int[] accountDayWeek, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo);
        /// <summary>
        /// 获取收款提醒合计-全部用户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="daiShouKuanHeJi">待收款合计</param>
        /// <returns></returns>
        void GetReceiptRemind(int companyId, int accountDay, int[] accountDayWeek, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo,out decimal daiShouKuanHeJi);

        /// <summary>
        /// 分页获取收款提醒-按订单销售员
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="sellers">销售员编号集合 多个编号间用“,”间隔</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> GetReceiptRemindByOrderSeller(int pageSize, int pageIndex, ref int recordCount, int CompanyId, int accountDay, int[] accountDayWeek, string sellers, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo);
        /// <summary>
        /// 获取收款提醒合计-按订单销售员
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="sellers">销售员编号集合 多个编号间用“,”间隔</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="daiShouKuanHeji">待收款合计</param>
        /// <returns></returns>
        void GetReceiptRemindByOrderSeller(int companyId, int accountDay, int[] accountDayWeek, string sellers, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo,out decimal daiShouKuanHeji);

        /*/// <summary>
        /// 获取收款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        int GetReceiptRemind(int CompanyId);*/

        #endregion

        /// <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomers(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSeachInfo seachInfo);
        /// <summary>
        /// 获取客户关系(组团)联系人编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        int GetContactId(int userId);

        /// <summary>
        /// 批量更新客户资料责任销售
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="customers">客户资料编号集合</param>
        /// <param name="sellerId">责任销售员编号</param>
        /// <param name="sellerName">责任销售员姓名</param>
        /// <returns></returns>
        bool BatchSpecifiedSeller(int companyId, IList<int> customers, int sellerId, string sellerName);
        /// <summary>
        /// 获取组团社已欠款金额及最高欠款金额
        /// </summary>
        /// <param name="customerId">客户单位编号</param>
        /// <param name="debtAmount">已欠款金额</param>
        /// <param name="maxDebtAmount">最高欠款金额</param>
        void GetCustomerDebt(int customerId, out decimal debtAmount, out decimal maxDebtAmount);
    }
}


