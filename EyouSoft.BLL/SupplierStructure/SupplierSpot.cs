using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 供应商-景点业务逻辑层
    /// </summary>
    /// 鲁功源 2011-03-08
    public class SupplierSpot
    {
        private readonly EyouSoft.IDAL.SupplierStructure.ISupplierSpot Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SupplierStructure.ISupplierSpot>();
        private readonly CompanyStructure.SupplierBaseHandle SupplierBll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法
        /// <summary>
        /// 单个添加
        /// </summary>
        /// <param name="model">供应商景点实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.SupplierStructure.SupplierSpot model)
        {
            if (model == null)
                return false;
            IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list = new List<EyouSoft.Model.SupplierStructure.SupplierSpot>() { model };
            return this.Add(list);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">供应商景点实体集合</param>
        /// <returns></returns>
        public bool Add(IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return Dal.Add(list);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">供应商景点实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.SupplierStructure.SupplierSpot model)
        {
            if (model == null)
                return false;
            int Result=SupplierBll.UpdateSupplierBase(model);
            if(Result>0)
                return Dal.Update(model);
            return false;
        }
        /// <summary>
        /// 获取供应商景点实体
        /// </summary>
        /// <param name="Id">景点主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierSpot GetModel(int Id)
        {
            if (Id <= 0)
                return null;
            EyouSoft.Model.SupplierStructure.SupplierSpot model = null;
            EyouSoft.Model.CompanyStructure.SupplierBasic BasicModel = SupplierBll.GetSupplierBase(Id);
            if (BasicModel != null)
            {
                model = new EyouSoft.Model.SupplierStructure.SupplierSpot();
                model.CityId = BasicModel.CityId;
                model.CityName = BasicModel.CityName;
                model.CompanyId = BasicModel.CompanyId;
                model.Id = BasicModel.Id;
                model.IsDelete = BasicModel.IsDelete;
                model.IssueTime = BasicModel.IssueTime;
                model.OperatorId = BasicModel.OperatorId;
                model.ProvinceId = BasicModel.ProvinceId;
                model.ProvinceName = BasicModel.ProvinceName;
                model.Remark = BasicModel.Remark;
                model.SupplierContact = BasicModel.SupplierContact;
                model.SupplierPic = BasicModel.SupplierPic;
                model.SupplierType = BasicModel.SupplierType;
                model.TradeNum = BasicModel.TradeNum;
                model.UnitAddress = BasicModel.UnitAddress;
                model.UnitName = BasicModel.UnitName;
                Dal.GetModel(Id, ref model);
            }
            BasicModel = null;
            return model;
        }
        /// <summary>
        /// 分页获取供应商景点
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页索引</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">供应商查询实体</param>
        /// <returns>供应商景点列表</returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery query)
        {
            if (CompanyId <= 0)
                return null;
            return Dal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId, query);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            if (Ids == null || Ids.Length == 0)
                return false;
            return SupplierBll.DeleteSupplierBase(Ids);
        }

        /// <summary>
        /// 获取供应商景点信息
        /// </summary>
        /// <param name="TopNum">top条数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">供应商查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int TopNum, int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery query)
        {
            if (CompanyId <= 0)
                return null;

            return Dal.GetList(TopNum, CompanyId, query);
        }

        #endregion

    }
}
