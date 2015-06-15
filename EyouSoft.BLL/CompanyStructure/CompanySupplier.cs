using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 供应商基本信息维护
    /// </summary>
    public class SupplierBaseHandle : BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ISupplierBaseHandle dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ISupplierBaseHandle>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        /// <summary>
        /// 添加供应商基本信息（含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>供应商基本信息Id</returns>
        public int AddSupplierBase(Model.CompanyStructure.SupplierBasic model)
        {
            if (model == null)
                return 0;

            int id = dal.AddSupplierBase(model);
            if (id > 0)
            {
                SetSupplierContact(id, model.SupplierContact);
                SetSupplierAccessory(id, model.SupplierPic);
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = GetModule(model.SupplierType),
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + GetModule(model.SupplierType).ToString() + "新增了数据！编号为：" + id,
                        EventTitle = "新增" + GetModule(model.SupplierType).ToString() + "数据"
                    });
            }

            return id;
        }

        /// <summary>
        ///  修改供应商基本信息（含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>返回1成功；其他失败</returns>
        public int UpdateSupplierBase(Model.CompanyStructure.SupplierBasic model)
        {
            if (model == null)
                return 0;

            int Index = dal.UpdateSupplierBase(model);
            if (Index == 1)
            {
                SetSupplierContact(model.Id, model.SupplierContact);
                SetSupplierAccessory(model.Id, model.SupplierPic);
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = GetModule(model.SupplierType),
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + GetModule(model.SupplierType).ToString() + "修改了数据！编号为：" + model.Id,
                        EventTitle = "修改" + GetModule(model.SupplierType).ToString() + "数据"
                    });
            }
            return Index;
        }

        /// <summary>
        /// 删除供应商基本信息
        /// </summary>
        /// <param name="SupplierBaseIds">供应商Id集合</param>
        /// <returns></returns>
        public bool DeleteSupplierBase(params int[] SupplierBaseIds)
        {
            if (SupplierBaseIds == null || SupplierBaseIds.Length <= 0)
                return false;

            return dal.DeleteSupplierBase(true, SupplierBaseIds);
        }

        /// <summary>
        /// 获取供应商基本信息
        /// </summary>
        /// <param name="SupplierBaseId">供应商基本信息Id</param>
        /// <returns></returns>
        public Model.CompanyStructure.SupplierBasic GetSupplierBase(int SupplierBaseId)
        {
            if (SupplierBaseId <= 0)
                return null;

            return dal.GetSupplierBase(SupplierBaseId);
        }

        /// <summary>
        /// 查询供应商基本信息
        /// </summary>
        /// <param name="CompanyId">专线Id</param>
        /// <param name="SeachModel">供应商查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SupplierBasic> GetSupplierBaseList(int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery SeachModel, int PageSize, int PageIndex, ref int RecordCount)
        {
            return dal.GetSupplierBaseList(CompanyId, SeachModel, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 设置供应商联系人（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商联系人集合</param>
        /// <returns>返回1成功；其他失败</returns>
        private int SetSupplierContact(int SupplierBasicId, IList<EyouSoft.Model.CompanyStructure.SupplierContact> list)
        {
            return dal.SetSupplierContact(SupplierBasicId, list);
        }

        /// <summary>
        /// 设置供应商附件（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商附件集合</param>
        /// <returns>返回1成功；其他失败</returns>
        private int SetSupplierAccessory(int SupplierBasicId, IList<EyouSoft.Model.SupplierStructure.SupplierPic> list)
        {
            return dal.SetSupplierAccessory(SupplierBasicId, list);
        }

        /// <summary>
        /// 将供应商类型转换为模块
        /// </summary>
        /// <param name="SupplierType">供应商类型</param>
        /// <returns></returns>
        private EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass GetModule(EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType)
        {
            EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_其它;
            switch (SupplierType)
            {
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.保险:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_保险;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.餐馆:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_餐馆;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.车队:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_车队;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_地接;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.购物:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_购物;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.景点:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_景点;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.酒店:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_酒店;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_票务;
                    break;
                case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.其他:
                    t = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_其它;
                    break;
            }

            return t;
        }
    }

    /// <summary>
    /// 供应商信息管理BLL
    /// 创建人：xuqh 2011-01-20
    /// </summary>
    public class CompanySupplier
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanySupplier Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanySupplier>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法

        /// <summary>
        /// 删除供应商联系人--暂无用处
        /// </summary>
        /// <param name="supplierId">供应商联系人ID</param>
        /// <returns></returns>
        private bool DeleteSupplierContact(int supplierId)
        {
            return Dal.DeleteSupplierContact(supplierId);
        }

        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSupplierInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            bool result = false;
            result = Dal.AddSupplierInfo(model);
            handleLogsBll.Add(AddLogs("添加", result));

            return result;
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSupplierInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            bool result = false;
            result = Dal.UpdateSupplerInfo(model);
            handleLogsBll.Add(AddLogs("修改", result));

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
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }

        /// <summary>
        /// 获取供应商实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanySupplier GetModel(int Id, int companyId)
        {
            return Dal.GetModel(Id, companyId);
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
        public IList<EyouSoft.Model.CompanyStructure.CompanySupplier> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, int ProvinceId, int CityId, string UnitName, int companyId)
        {
            return Dal.GetList(pageSize, pageIndex, ref RecordCount, supplierType, ProvinceId, CityId, UnitName, companyId);
        }

        /// <summary>
        /// 分页获取供应商-其它列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="UnitName">省份名称</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierOther> GetOtherList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, string UnitName, int companyId)
        {
            return Dal.GetOtherList(pageSize, pageIndex, ref RecordCount, supplierType, UnitName, companyId);
        }

        /// <summary>
        /// 数据批量插入到数据库
        /// </summary>
        /// <param name="ls">数据集(来自文件)</param>
        /// <returns></returns>
        public bool ImportExcelData(List<EyouSoft.Model.CompanyStructure.CompanySupplier> ls)
        {
            return Dal.ImportExcelData(ls);
        }

        /// <summary>
        /// 批量导入供应商-其它信息
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public bool ImportOtherData(List<EyouSoft.Model.SupplierStructure.SupplierOther> ls)
        {
            return Dal.ImportOtherData(ls);
        }

        /// <summary>
        /// 获取供应商列表交易次数合计
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="type">供应商类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public int GetTimesGYSSummary(int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType type, EyouSoft.Model.CompanyStructure.MSupplierSearchInfo searchInfo)
        {
            if (companyId < 1) return 0;

            return Dal.GetTimesGYSSummary(companyId,type,searchInfo);
        }

        /// <summary>
        /// 获取地接供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo GetTimesSummaryDiJie(int companyId, int gysId)
        {
            return Dal.GetTimesSummaryDiJie(companyId, gysId, null);
        }

        /// <summary>
        /// 获取地接供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo GetTimesSummaryDiJie(int companyId, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId < 1 || gysId < 1) return null;

            return Dal.GetTimesSummaryDiJie(companyId, gysId, searchInfo);
        }

        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId, int gysId)
        {
            if (companyId < 1 || gysId < 1) return null;

            return Dal.GetTimesSummaryJiPiao(companyId, gysId);
        }
        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId,int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId < 1 || gysId < 1) return null;

            return Dal.GetTimesSummaryJiPiao(companyId, gysId,searchInfo);
        }
        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_地接;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_地接.ToString() + (flag ? actionName : actionName + "失败") + "了供应商管理地接数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.供应商管理_地接.ToString() + "数据";

            return model;
        }
    }
}
