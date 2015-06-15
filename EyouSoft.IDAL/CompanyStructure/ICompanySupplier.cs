using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 供应商信息操作接口
    /// </summary>
    public interface ICompanySupplier
    {
        /// <summary>
        /// 删除供应商联系人
        /// </summary>
        /// <param name="supplierId">供应商联系人ID</param>
        /// <returns></returns>
        bool DeleteSupplierContact(int supplierId);

        /// <summary>
        /// 新增供应商信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        bool AddSupplierInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model);

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        bool UpdateSupplerInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model);

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteSupplierInfo(params int[] Id);

        /// <summary>
        /// 获取一个供应商实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanySupplier GetModel(int Id, int companyId);

        /// <summary>
        /// 数据批量插入到数据库
        /// </summary>
        /// <param name="ls">数据集(来自文件)</param>
        /// <returns></returns>
        bool ImportExcelData(List<EyouSoft.Model.CompanyStructure.CompanySupplier> ls);

        /// <summary>
        /// 批量导入供应商-其它信息
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        bool ImportOtherData(List<EyouSoft.Model.SupplierStructure.SupplierOther> ls);


        /// <summary>
        /// 分页获取供应商信息列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanySupplier> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, int ProvinceId, int CityId, string UnitName, int companyId);

        /// <summary>
        /// 分页获取供应商-其它信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="supplierType"></param>
        /// <param name="UnitName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierOther> GetOtherList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, string UnitName, int companyId);

        /// <summary>
        /// 分页获取付款提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.PayRemind> GetPayRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo);
        /// <summary>
        /// 获取付款提醒未付款合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="weiFuHeJi">未付款合计</param>
        /// <returns></returns>
        void GetPayRemind(int companyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi);

        /// <summary>
        /// 获取付款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        int GetPayRemind(int CompanyId);

        /// <summary>
        /// 获取供应商列表交易次数合计
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="type">供应商类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        int GetTimesGYSSummary(int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType type, EyouSoft.Model.CompanyStructure.MSupplierSearchInfo searchInfo);
        /// <summary>
        /// 获取地接供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo GetTimesSummaryDiJie(int companyId, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo);
        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId, int gysId);
        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo);
    }

    /// <summary>
    /// 供应商基本信息操作接口
    /// </summary>
    public interface ISupplierBaseHandle
    {
        /// <summary>
        /// 添加供应商基本信息（不含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>供应商基本信息Id</returns>
        int AddSupplierBase(Model.CompanyStructure.SupplierBasic model);

        /// <summary>
        ///  修改供应商基本信息（不含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>返回1成功；其他失败</returns>
        int UpdateSupplierBase(Model.CompanyStructure.SupplierBasic model);

        /// <summary>
        /// 删除供应商基本信息
        /// </summary>
        /// <param name="IsDelete">删除状态</param>
        /// <param name="SupplierBaseIds">供应商Id集合</param>
        /// <returns></returns>
        bool DeleteSupplierBase(bool IsDelete, params int[] SupplierBaseIds);

        /// <summary>
        /// 获取供应商基本信息
        /// </summary>
        /// <param name="SupplierBaseId">供应商基本信息Id</param>
        /// <returns></returns>
        Model.CompanyStructure.SupplierBasic GetSupplierBase(int SupplierBaseId);

        /// <summary>
        /// 查询供应商基本信息
        /// </summary>
        /// <param name="CompanyId">专线Id</param>
        /// <param name="SeachModel">供应商查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.SupplierBasic> GetSupplierBaseList(int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery SeachModel, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 设置供应商联系人（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商联系人集合</param>
        /// <returns>返回1成功；其他失败</returns>
        int SetSupplierContact(int SupplierBasicId, IList<EyouSoft.Model.CompanyStructure.SupplierContact> list);

        /// <summary>
        /// 设置供应商附件（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商附件集合</param>
        /// <returns>返回1成功；其他失败</returns>
        int SetSupplierAccessory(int SupplierBasicId, IList<EyouSoft.Model.SupplierStructure.SupplierPic> list);
    }
}
