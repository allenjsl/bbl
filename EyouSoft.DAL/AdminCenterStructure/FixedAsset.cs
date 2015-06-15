using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.AdminCenterStructure
{
    public class FixedAsset : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IFixedAsset
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FixedAsset()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 获取固定资产管理实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.FixedAsset GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.AdminCenterStructure.FixedAsset model = null;
            model = (from item in dcDal.FixedAsset
                     where item.CompanyId == CompanyId && item.Id == Id
                     select new EyouSoft.Model.AdminCenterStructure.FixedAsset
                     {
                         Id = item.Id,
                         AssetName = item.AssetName,
                         AssetNo = item.AssetNo,
                         BuyDate = item.BuyDate,
                         Cost = item.Cost,
                         Remark = item.Remark
                     }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取固定资产管理信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="FixedAssetNo">固定资产管理编号</param>
        /// <param name="AssetName">固定资产名称</param>
        /// <param name="BeginStart">会议时间开始</param>
        /// <param name="BeginEnd">会议时间结束</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.FixedAsset> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, string FixedAssetNo, string AssetName, DateTime? BeginStart, DateTime? BeginEnd)
        {
            IList<EyouSoft.Model.AdminCenterStructure.FixedAsset> ResultList = null;
            string tableName = "tbl_FixedAsset";
            string identityColumnName = "Id";
            string fields = " [Id],[AssetNo],[AssetName],[BuyDate],[Cost],[Remark] ";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(FixedAssetNo))
            {
                query = query + string.Format(" AND AssetNo='{0}'", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(FixedAssetNo));
            }
            if (!string.IsNullOrEmpty(AssetName))
            {
                query = query + string.Format("AND AssetName LIKE '%{0}%' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(AssetName));
            }
            if (BeginStart.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,'{0}',BuyDate)>=0", BeginStart);
            }
            if (BeginEnd.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,BuyDate,'{0}')>=0", BeginEnd);
            }
            string orderByString = " IssueTime DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.FixedAsset>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.FixedAsset model = new EyouSoft.Model.AdminCenterStructure.FixedAsset()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        AssetName = dr.IsDBNull(dr.GetOrdinal("AssetName")) ? "" : dr.GetString(dr.GetOrdinal("AssetName")),
                        AssetNo = dr.IsDBNull(dr.GetOrdinal("AssetNo")) ? "" : dr.GetString(dr.GetOrdinal("AssetNo")),
                        BuyDate = dr.GetDateTime(dr.GetOrdinal("BuyDate")),
                        Cost = dr.IsDBNull(dr.GetOrdinal("Cost")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Cost")),
                        Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? "" : dr.GetString(dr.GetOrdinal("Remark"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">固定资产管理实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.FixedAsset model)
        {
            bool IsTrue = false;
            EyouSoft.Data.FixedAsset DataModel = new EyouSoft.Data.FixedAsset()
            {
                AssetName = model.AssetName,
                AssetNo = model.AssetNo,
                Remark = model.Remark,
                Cost = model.Cost,
                BuyDate = model.BuyDate,
                CompanyId = model.CompanyId,
                OperatorId = model.OperatorId,
                IssueTime = System.DateTime.Now
            };
            dcDal.FixedAsset.InsertOnSubmit(DataModel);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            DataModel = null;
            return IsTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">固定资产管理实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.FixedAsset model)
        {
            bool IsTrue = false;
            EyouSoft.Data.FixedAsset DataModel = dcDal.FixedAsset.FirstOrDefault(item =>
               item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                DataModel.AssetName = model.AssetName;
                DataModel.AssetNo = model.AssetNo;
                DataModel.Remark = model.Remark;
                DataModel.Cost = model.Cost;
                DataModel.BuyDate = model.BuyDate;
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
            }
            DataModel = null;
            return IsTrue;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">固定资产管理编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            EyouSoft.Data.FixedAsset DataModel = dcDal.FixedAsset.FirstOrDefault(item =>
                item.Id == Id && item.CompanyId == CompanyId
            );
            if (DataModel != null)
            {
                dcDal.FixedAsset.DeleteOnSubmit(DataModel);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                DataModel = null;
            }
            return IsTrue;
        }
        #endregion
    }
}
