using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-固定资产管理IDAL
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public interface IFixedAsset
    {
        /// <summary>
        /// 获取固定资产管理实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.FixedAsset GetModel(int CompanyId, int Id);
        /// <summary>
        /// 获取固定资产管理信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="FixedAssetNo">固定资产管理编号</param>
        /// <param name="AssetName">固定资产名称</param>
        /// <param name="BeginStart">会议时间开始</param>
        /// <param name="BeginEnd">会议时间结束</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.FixedAsset> GetList(int PageSize, int PageIndex, ref int RecordCount,int CompanyId,string FixedAssetNo, string AssetName, DateTime? BeginStart, DateTime? BeginEnd);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">固定资产管理实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.AdminCenterStructure.FixedAsset model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">固定资产管理实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.AdminCenterStructure.FixedAsset model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">固定资产管理编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, int Id);
    }
}
