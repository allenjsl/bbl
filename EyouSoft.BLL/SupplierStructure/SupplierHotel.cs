/*Author:汪奇志 2011-03-08*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 供应商酒店业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierHotel
    {
        private readonly EyouSoft.IDAL.SupplierStructure.ISupplierHotel dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SupplierStructure.ISupplierHotel>();

        #region private members
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logbll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            logbll.Add(logInfo);
            logbll = null;
        }
        #endregion

        #region public members
        /// <summary>
        /// 写入酒店供应商信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertHotelInfo(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info)
        {
            using (TransactionScope AddTran = new TransactionScope())
            {                
                bool dalResult = false;
                EyouSoft.BLL.CompanyStructure.SupplierBaseHandle basicbll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
                info.Id = basicbll.AddSupplierBase(info);
                basicbll = null;
                
                if (info.Id < 1) return -1;

                dalResult = dal.InsertHotelPertain(info);
                if (!dalResult) return -2;

                dalResult = dal.InsertRoomTypes(info.Id, info.RoomTypes);
                if (!dalResult) return -3;                

                AddTran.Complete();
            }

            return 1;
        }

        /// <summary>
        /// 写入酒店供应商信息集合，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="items">酒店供应商信息集合</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertHotels(IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    this.InsertHotelInfo(item);
                }

                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 获取酒店供应商信息业务实体
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierHotelInfo GetHotelInfo(int supplierId)
        {
            if (supplierId < 1) return null;

            EyouSoft.Model.SupplierStructure.SupplierHotelInfo info = dal.GetHotelPertainInfo(supplierId);            
            EyouSoft.BLL.CompanyStructure.SupplierBaseHandle basicbll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
            EyouSoft.Model.CompanyStructure.SupplierBasic basicinfo = basicbll.GetSupplierBase(supplierId);
            basicbll = null;

            if (basicinfo != null && info != null)
            {
                info.CityId = basicinfo.CityId;
                info.CityName = basicinfo.CityName;
                info.CompanyId = basicinfo.CompanyId;
                info.Id = basicinfo.Id;
                info.IsDelete = basicinfo.IsDelete;
                info.IssueTime = basicinfo.IssueTime;
                info.OperatorId = basicinfo.OperatorId;
                info.ProvinceId = basicinfo.ProvinceId;
                info.ProvinceName = basicinfo.ProvinceName;
                info.Remark = basicinfo.Remark;
                info.SupplierContact = basicinfo.SupplierContact;
                info.SupplierPic = basicinfo.SupplierPic;
                info.SupplierType = basicinfo.SupplierType;
                info.TradeNum = basicinfo.TradeNum;
                info.UnitAddress = basicinfo.UnitAddress;
                info.UnitName = basicinfo.UnitName;

                return info;
            }

            return null ;
        }

        /// <summary>
        /// 更新酒店供应商信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateHotelInfo(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info)
        {
            if (info.Id < 1) return 0;

            using (TransactionScope AddTran = new TransactionScope())
            {
                bool dalResult = false;
                EyouSoft.BLL.CompanyStructure.SupplierBaseHandle basicbll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
                int updateBasicinfoResult = basicbll.UpdateSupplierBase(info);
                basicbll = null;

                if (updateBasicinfoResult != 1) return -1;

                dalResult = dal.UpdateHotelPertain(info);
                if (!dalResult) return -2;

                dalResult = dal.DeleteRoomType(info.Id);
                if (!dalResult) return -4;

                dalResult = dal.InsertRoomTypes(info.Id, info.RoomTypes);
                if (!dalResult) return -3;

                AddTran.Complete();
            }

            return 1;
        }

        /// <summary>
        /// 删除酒店供应商信息
        /// </summary>
        /// <param name="supplierId">供应商编号集合</param>
        /// <returns></returns>
        public bool DeleteHotelInfo(params int[] supplierIds)
        {
            if (supplierIds == null || supplierIds.Length < 1) return false;

            if (new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle().DeleteSupplierBase(supplierIds))
            {
                #region LGWR
                StringBuilder s = new StringBuilder();
                s.Append(supplierIds[0]);
                for (int i = 1; i < supplierIds.Length; i++)
                {
                    s.AppendFormat(",{0}", supplierIds[i]);
                }
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_酒店.ToString() + "删除了供应商信息，供应商编号为：" + s;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除供应商信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_酒店;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取酒店供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info)
        {
            if (companyId < 1) return null;

            return dal.GetHotels(companyId, pageSize, pageIndex, ref recordCount, info);
        }

        /// <summary>
        /// 获取酒店供应商信息集合(供应商基本信息，图片信息，房型信息。同行登录口适用)
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetSiteHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info)
        {
            if (companyId < 1) return null;

            return dal.GetSiteHotels(companyId, pageSize, pageIndex, ref recordCount, info);
        }

        //交易情况点击打开页面的方法待定
        #endregion
    }
}
