using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 客户关系管理相关方法
    /// autor :李焕超 date:2011-1-17
    /// </summary>
    public class Customer
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICustomer Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICustomer>();

        //客户资料方法
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsCustomer(int Id)
        {
          return  Dal.ExistsCustomer(Id);
        
        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int AddCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        {
            return Dal.AddCustomer(model);
        }

        /// <summary>
        ///  增加多条数据
        /// </summary>
        public bool AddCustomerMore(IList<EyouSoft.Model.CompanyStructure.CustomerInfo> modelList)
        { return Dal.AddCustomerMore(modelList); }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        {
            return Dal.UpdateCustomer(model);
        }

        /// <summary>
        /// 更新组团端客户资料数据
        /// </summary>
        public bool UpdateSampleCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        { return Dal.UpdateSampleCustomer(model); }

        /// <summary>
        ///  更新组团端客户资料配置管理数据
        /// </summary>
        public  bool UpdateSampleCustomerConfig(EyouSoft.Model.CompanyStructure.CustomerConfig model)
        { return Dal.UpdateSampleCustomerConfig(model); }

        /// <summary>
        /// 更新当前登录游客的电话和手机
        /// </summary>
        /// <param name="Model">只赋值编号，电话和手机</param>
        /// <returns></returns>
        public bool UpDateSampleCompanyUserInfo(EyouSoft.Model.CompanyStructure.CompanyUser Model)
        { return Dal.UpDateSampleCompanyUserInfo(Model); }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomer(int Id)
        {
            if (Id <= 0) return true;

            if (Dal.DeleteCustomer(Id))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_客户资料.ToString() + "删除了客户资料，客户编号为：" + Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除客户资料";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_客户资料;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion

                return true;
            }

            return false;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerModel(int Id)
        {
            return Dal.GetCustomerModel(Id);
        }

        /// <summary>
        /// 得到客户的配置信息对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerConfig GetCustomerConfigModel(int Id)
        { return Dal.GetCustomerConfigModel(Id); }


        /// <summary>
        /// 搜索数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> SearchCustomerList(int PageSize, int PageIndex, int CompanyID,int[] provinceIds,int[] cityIds, string CompanyName,string Contacter,string Saler, ref int RecordCount, ref int PageCount,string telephone)
        {
            EyouSoft.Model.CompanyStructure.MCustomerSeachInfo seachInfo = new EyouSoft.Model.CompanyStructure.MCustomerSeachInfo();

            seachInfo.ProvinceIds = provinceIds;
            seachInfo.CityIdList = cityIds;
            seachInfo.ContactName = Contacter;

            if (!string.IsNullOrEmpty(Saler))
            {
                int sellerId = 0;
                int.TryParse(Saler, out sellerId);
                if (sellerId > 0)
                {
                    seachInfo.SellerIds = new int[] { sellerId };
                }
            }
            
            seachInfo.CustomerName = CompanyName;
            seachInfo.ContactTelephone = telephone;


            return this.GetCustomers(CompanyID, PageSize, PageIndex, ref RecordCount, seachInfo);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomerList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount)
        {
            return this.GetCustomers(CompanyID, PageSize, PageIndex, ref RecordCount, null);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool StopIt(int CustomerId)
        { return Dal.StopIt(CustomerId); }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool StartId(int CustomerId)
        { return Dal.StartId(CustomerId); }

     #endregion  成员方法

        //客户联系人方法
        #region  成员方法
      
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerContactInfo GetCustomerContactModel(int Id)
        {
          return Dal.GetCustomerContactModel(Id); 
        }
        /// <summary>
        /// 获得某个客户的所有联系人
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> GetCustomerContactList(int CustomerID)
        {
         return  Dal.GetCustomerContactList(CustomerID);
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteCustomerContacter(params string[] Ids)
        {
          return  Dal.DeleteCustomerContacter(Ids);
        }

        #endregion  成员方法

        //客户回访/投诉方法
        #region  成员方法

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public bool AddCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model)
        {
           return  Dal.AddCustomerCallBack(model);
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model)
        {
           return Dal.UpdateCustomerCallBack(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomerCallBack(int Id)
        {
           return Dal.DeleteCustomerCallBack(Id);
        }

        /// <summary>
        /// 删除一多条数据
        /// </summary>
        public bool DeleteCustomerCallBackMore(params string[] Ids)
        {
            if (Ids == null || Ids.Length == 0)
                return false;
            return Dal.DeleteCustomerCallBackMore(Ids);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerCallBackInfo GetCustomerCallBackModel(int Id)
        {
           return Dal.GetCustomerCallBackModel(Id);
        }

        /// <summary>
        /// 回访列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> GetCustomerCallBackList(EyouSoft.Model.EnumType.CompanyStructure.CallBackType CallType,int PageSize, int PageIndex, int CompanyId, ref int RecordCount, ref int PageCount)
        {
            string strWhere = string.Empty;
            string filedOrder="*";
            return  Dal.GetCustomerCallBackList(CallType,PageSize, PageIndex, CompanyId, strWhere, filedOrder,ref RecordCount,ref PageCount);
        }
        /// <summary>
        /// 搜索回访
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId"></param>
        /// <param name="CustomerUser">被回访者</param>
        /// <param name="CallBacker">回访者</param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> SearchCustomerCallBackList(EyouSoft.Model.EnumType.CompanyStructure.CallBackType CallType, int PageSize, int PageIndex, int CompanyId, string CustomerUser, string CallBacker, ref int RecordCount, ref int PageCount)
        {
            StringBuilder strWhere = new StringBuilder(" and 1=1 ");
            if(!string.IsNullOrEmpty(CustomerUser))
                strWhere.AppendFormat(" and CustomerUser  like '%{0}%'",CustomerUser);
            if(!string.IsNullOrEmpty(CallBacker))
                strWhere.AppendFormat(" and CallBacker like '%{0}%'", CallBacker);

            string filedOrder = "*";
            return Dal.GetCustomerCallBackList(CallType,PageSize, PageIndex, CompanyId, strWhere.ToString(), filedOrder, ref RecordCount, ref PageCount);
        }


        #endregion  成员方法

        //客户回访/投诉结果方法
        #region  成员方法
        
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerCallbackResult(EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo model)
        {
          return  Dal.UpdateCustomerCallbackResult(model);
        }
        /// <summary>
        /// 获取回访结果
        /// </summary>
        /// <param name="CallBackId">回访编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> GetCustomerCallbackResultList(int CallBackId)
        {
            return Dal.GetCustomerCallbackResultList(CallBackId);
        }



        #endregion  成员方法

        #region 营销活动


        /// <summary>
        ///  增加一条数据
        /// </summary>
        public bool AddCustomerMarket(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model)
        {
           return Dal.AddCustomerMarketing(model);
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerMarket(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model)
        {
            return Dal.UpdateCustomerMarketing(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomerMarket(int Id)
        {
            return Dal.DeleteCustomerMarketing(Id);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool DeleteCustomerMarketingS(int[] Id)
        { return Dal.DeleteCustomerMarketingS(Id); }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerMarketingInfo GetCustomerMarketModel(int Id)
        {
            return Dal.GetCustomerMarketingModel(Id);
        }

        /// <summary>
        /// 回访列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo> GetCustomerMarketList(int PageSize, int PageIndex, int CompanyId, ref int RecordCount, ref int PageCount)
        {
            string strWhere = string.Empty;
            string filedOrder = "*";
            return Dal.GetCustomerMarketingList(PageSize, PageIndex, CompanyId, strWhere, filedOrder, ref RecordCount, ref PageCount);
        }
        /// <summary>
        /// 搜索回访
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId"></param>
        /// <param name="CustomerUser">被回访者</param>
        /// <param name="CallBacker">回访者</param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo> SearchCustomerMarketList(int PageSize, int PageIndex, int CompanyId, string Theme, string DateTime, ref int RecordCount, ref int PageCount)
        {
            StringBuilder strWhere = new StringBuilder(" and 1=1 ");
            if (!string.IsNullOrEmpty(Theme))
                strWhere.AppendFormat(" and Theme  like '%{0}%'", Theme);
            if (!string.IsNullOrEmpty(DateTime))
                strWhere.AppendFormat(" and DateDiff(day,Time,'{0}')=0 ", DateTime);

            string filedOrder = "*";
            return Dal.GetCustomerMarketingList(PageSize, PageIndex, CompanyId, strWhere.ToString(), filedOrder, ref RecordCount, ref PageCount);
        }
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
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomers(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSeachInfo seachInfo)
        {
            if (companyId < 1) return null;

            return Dal.GetCustomers(companyId, pageSize, pageIndex, ref recordCount, seachInfo);
        }

        /// <summary>
        /// 获取客户关系(组团)联系人编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public int GetContactId(int userId)
        {
            return Dal.GetContactId(userId);
        }

        /// <summary>
        /// 批量更新客户资料责任销售
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="customers">客户资料编号集合</param>
        /// <param name="sellerId">责任销售员编号</param>
        /// <param name="sellerName">责任销售员姓名</param>
        /// <returns></returns>
        public bool BatchSpecifiedSeller(int companyId, IList<int> customers, int sellerId, string sellerName)
        {
            if (companyId < 1
                || customers == null
                || customers.Count() <= 0 || sellerId < 1)
                return false;

            if (Dal.BatchSpecifiedSeller(companyId, customers, sellerId, sellerName))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_客户资料.ToString() + "批量指定客户责任销售员，客户编号为：" + EyouSoft.Toolkit.Utils.GetSqlIdStrByList(customers) + "责任销售员编号为:" + sellerId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "批量指定客户责任销售";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_客户资料;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取组团社已欠款金额及最高欠款金额
        /// </summary>
        /// <param name="customerId">客户单位编号</param>
        /// <param name="debtAmount">已欠款金额</param>
        /// <param name="maxDebtAmount">最高欠款金额</param>
        public void GetCustomerDebt(int customerId, out decimal debtAmount, out decimal maxDebtAmount)
        {
            debtAmount = 0;
            maxDebtAmount = 0;

            if (customerId <= 0) return;

            Dal.GetCustomerDebt(customerId, out debtAmount, out maxDebtAmount);
        }
    }
}
