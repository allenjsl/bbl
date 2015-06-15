using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 供应商-其它BLL
    /// author xuqh 2011-3-9
    /// </summary>
    public class SupplierOther
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanySupplier Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanySupplier>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法

        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSupplierInfo(EyouSoft.Model.SupplierStructure.SupplierOther model)
        {
            bool result = false;
            result = Dal.AddSupplierInfo(ConvertModel(model));
            handleLogsBll.Add(AddLogs("添加", model.SupplierType, result));

            return result;
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSupplierInfo(EyouSoft.Model.SupplierStructure.SupplierOther model)
        {
            bool result = false;
            result = Dal.UpdateSupplerInfo(ConvertModel(model));
            handleLogsBll.Add(AddLogs("修改", model.SupplierType, result));

            return result;
        }

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteSupplierInfo(params int[] Id)
        {
            bool result = false;
            result = Dal.DeleteSupplierInfo(Id);
            handleLogsBll.Add(AddLogs("删除", null, result));

            return result;
        }

        /// <summary>
        /// 获取供应商实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierOther GetModel(int Id, int companyId)
        {
            return ConvertOtherModel(Dal.GetModel(Id, companyId));
        }


        /// <summary>
        /// 分页获取供应商列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="supplierType">供应商类型（0-地接 1票务）</param>
        /// <param name="ProvinceName">省份名称（可为空）</param>
        /// <param name="CityName">城市名称（可为空）</param>
        /// <param name="UnitName">单位名称（可为空）</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierOther> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, int ProvinceId, int CityId, string UnitName, int companyId)
        {
            return Dal.GetOtherList(pageSize, pageIndex, ref RecordCount, supplierType, UnitName, companyId);
        }

        /// <summary>
        /// 数据批量插入到数据库
        /// </summary>
        /// <param name="ls">数据集(来自文件)</param>
        /// <returns></returns>
        public bool ImportExcelData(List<EyouSoft.Model.SupplierStructure.SupplierOther> ls)
        {
            return Dal.ImportOtherData(ls);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName
            , EyouSoft.Model.EnumType.CompanyStructure.SupplierType? SupplierType, bool flag)
        {
            EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass sp = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_其它;
            if (SupplierType.HasValue)
            {
                switch (SupplierType)
                {
                    case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他:
                        sp = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_其它;
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.航空公司:
                        sp = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_航空公司;
                        break;
                }
            }
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = sp;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + sp.ToString() + (flag ? actionName : actionName + "失败") + "了供应商" + (SupplierType.HasValue ? SupplierType.ToString() : string.Empty) + "数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + sp.ToString() + "数据";

            return model;
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanySupplier ConvertModel(EyouSoft.Model.SupplierStructure.SupplierOther model)
        {
            EyouSoft.Model.CompanyStructure.CompanySupplier targetModel = new EyouSoft.Model.CompanyStructure.CompanySupplier();
            targetModel.Id = model.Id;
            targetModel.IsDelete = model.IsDelete;
            targetModel.IssueTime = model.IssueTime;
            targetModel.OperatorId = model.OperatorId;
            targetModel.ProvinceId = model.ProvinceId;
            targetModel.ProvinceName = model.ProvinceName;
            targetModel.Remark = model.Remark;
            targetModel.SupplierContact = model.SupplierContact;
            targetModel.SupplierPic = model.SupplierPic;
            targetModel.SupplierType = model.SupplierType;
            targetModel.TradeNum = model.TradeNum;
            targetModel.UnitAddress = model.UnitAddress;
            targetModel.UnitName = model.UnitName;
            targetModel.CompanyId = model.CompanyId;
            targetModel.CityName = model.CityName;
            targetModel.CityId = model.CityId;
            targetModel.AgreementFile = model.AgreementFile;

            return targetModel;
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private EyouSoft.Model.SupplierStructure.SupplierOther ConvertOtherModel(EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            EyouSoft.Model.SupplierStructure.SupplierOther targetModel = new EyouSoft.Model.SupplierStructure.SupplierOther();
            targetModel.Id = model.Id;
            targetModel.IsDelete = model.IsDelete;
            targetModel.IssueTime = model.IssueTime;
            targetModel.OperatorId = model.OperatorId;
            targetModel.ProvinceId = model.ProvinceId;
            targetModel.ProvinceName = model.ProvinceName;
            targetModel.Remark = model.Remark;
            targetModel.SupplierContact = model.SupplierContact;
            targetModel.SupplierPic = model.SupplierPic;
            targetModel.SupplierType = model.SupplierType;
            targetModel.TradeNum = model.TradeNum;
            targetModel.UnitAddress = model.UnitAddress;
            targetModel.UnitName = model.UnitName;
            targetModel.CompanyId = model.CompanyId;
            targetModel.CityName = model.CityName;
            targetModel.CityId = model.CityId;
            targetModel.AgreementFile = model.AgreementFile;

            return targetModel;
        }
        #endregion
    }
}
