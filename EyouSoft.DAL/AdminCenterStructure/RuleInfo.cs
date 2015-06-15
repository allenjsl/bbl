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
    public class RuleInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IRuleInfo
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RuleInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion 构造函数

        #region 实现接口公共方法
        /// <summary>
        /// 获取规章制度实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.RuleInfo GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.AdminCenterStructure.RuleInfo model = null;
            model = (from item in dcDal.RuleInfo
                     where item.CompanyId == CompanyId && item.Id == Id
                     select new EyouSoft.Model.AdminCenterStructure.RuleInfo
                     {
                         Id = item.Id,
                         FilePath = item.FilePath,
                         RoleContent = item.RoleContent,
                         RoleNo = item.RoleNo,
                         Title = item.Title
                     }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取规章制度信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="RuleNo">规章制度编号</param>
        /// <param name="Title">规章制度标题</param>      
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.RuleInfo> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, string RuleNo, string Title)
        {
            IList<EyouSoft.Model.AdminCenterStructure.RuleInfo> ResultList = null;
            string tableName = "tbl_RuleInfo";
            string identityColumnName = "Id";
            string fields = " [Id],[Title],RoleNo";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(RuleNo))
            {
                query = query + string.Format(" AND RoleNo LIKE '%{0}%' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(RuleNo));
            }
            if (!string.IsNullOrEmpty(Title))
            {
                query = query + string.Format(" AND Title LIKE '%{0}%' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(Title));
            }
            string orderByString = " IssueTime DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.RuleInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.RuleInfo model = new EyouSoft.Model.AdminCenterStructure.RuleInfo()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Title = dr.IsDBNull(dr.GetOrdinal("Title")) ? "" : dr.GetString(dr.GetOrdinal("Title")),
                        RoleNo = dr.IsDBNull(dr.GetOrdinal("RoleNo")) ? "" : dr.GetString(dr.GetOrdinal("RoleNo"))
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
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.RuleInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.RuleInfo DataModel = new EyouSoft.Data.RuleInfo()
            {
                FilePath = model.FilePath,
                RoleContent = model.RoleContent,
                RoleNo = model.RoleNo,
                Title = model.Title,
                CompanyId = model.CompanyId,
                OperatorId = model.OperatorId,
                IssueTime = System.DateTime.Now
            };
            dcDal.RuleInfo.InsertOnSubmit(DataModel);
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
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.RuleInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.RuleInfo DataModel = dcDal.RuleInfo.FirstOrDefault(item =>
               item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                DataModel.FilePath = model.FilePath;
                DataModel.RoleContent = model.RoleContent;
                DataModel.RoleNo = model.RoleNo;
                DataModel.Title = model.Title;
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
        /// <param name="Id">规章制度编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            EyouSoft.Data.RuleInfo DataModel = dcDal.RuleInfo.FirstOrDefault(item =>
                item.Id == Id && item.CompanyId == CompanyId
            );
            if (DataModel != null)
            {
                dcDal.RuleInfo.DeleteOnSubmit(DataModel);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                DataModel = null;
            }
            return IsTrue;
        }
        #endregion 实现接口公共方法
    }
}
