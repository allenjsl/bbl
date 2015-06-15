using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.BLL.CompanyStructure;
using System.Transactions;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 供应商车队信息维护逻辑层
    /// </summary>
    public class SupplierCarTeam
    {
        private readonly EyouSoft.IDAL.SupplierStructure.ISupplierCarTeam dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SupplierStructure.ISupplierCarTeam>();

        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region public memebers
        /// <summary>
        /// 写入车队供应商信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">车队供应商信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertCarTeamInfo(EyouSoft.Model.SupplierStructure.SupplierCarTeam info)
        {
            if (info != null)
            {
                if (info.CompanyId == 0) return -1;
                using (TransactionScope AddTran = new TransactionScope())
                {
                    bool dalResult = false;
                    EyouSoft.BLL.CompanyStructure.SupplierBaseHandle basicbll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
                    info.SupplierType = EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队;
                    info.Id = basicbll.AddSupplierBase(info);
                    basicbll = null;

                    if (info.Id < 1) return -1;

                    dalResult = dal.AddCarTeamAttach(info);
                    if (!dalResult) return -2;

                    dalResult = dal.InsertCars(info.Id, info.CarsInfo);
                    if (!dalResult) return -3;

                    AddTran.Complete();
                }
            }
            return 1;
        }


        /// <summary>
        /// 写入车队供应商信息集合，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="items">车队供应商信息集合</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertCarTeams(IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    this.InsertCarTeamInfo(item);
                }

                return 1;
            }

            return 0;
        }


        /// <summary>
        /// 获取车队供应商信息业务实体
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierCarTeam GetCarTeamInfo(int supplierId)
        {
            if (supplierId < 1) return null;

            EyouSoft.Model.SupplierStructure.SupplierCarTeam info = dal.GetCarTeamAttachInfo(supplierId);
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

            return null;
        }

        /// <summary>
        /// 更新车队供应商信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">车队供应商信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateCarTeam(EyouSoft.Model.SupplierStructure.SupplierCarTeam info)
        {
            if (info.Id < 1) return 0;

            using (TransactionScope AddTran = new TransactionScope())
            {
                bool dalResult = false;
                EyouSoft.BLL.CompanyStructure.SupplierBaseHandle basicbll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
                int updateBasicinfoResult = basicbll.UpdateSupplierBase(info);
                basicbll = null;

                if (updateBasicinfoResult != 1) return -1;

                dalResult = dal.UpdateCarTeamAttach(info);
                if (!dalResult) return -2;

                dalResult = dal.DeleteCars(info.Id);
                if (!dalResult) return -4;

                dalResult = dal.InsertCars(info.Id, info.CarsInfo);
                if (!dalResult) return -3;

                AddTran.Complete();
            }

            return 1;
        }

        /// <summary>
        /// 删除车队供应商信息
        /// </summary>
        /// <param name="supplierId">供应商编号集合</param>
        /// <returns></returns>
        public bool DeleteCarTeamInfo(params int[] supplierIds)
        {
            if (supplierIds == null || supplierIds.Length < 1) return false;

            return new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle().DeleteSupplierBase(supplierIds);
        }

        /// <summary>
        /// 获取车队供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> GetCarTeams(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo info)
        {
            if (companyId < 1) return null;

            return dal.GetCarTeams(companyId, pageSize, pageIndex, ref recordCount, info);
        }

        #endregion
    }
}
