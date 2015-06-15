using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.PersonalCenterStructure
{
    /// <summary>
    /// 个人中心-文档管理数据层
    /// </summary>
    /// 鲁功源  2011-01-17
    public class PersonDocument : EyouSoft.Toolkit.DAL.DALBase,EyouSoft.IDAL.PersonalCenterStructure.IPersonDocument
    {
        #region 变量
        private const string Sql_PersonDocument_Delete = "update tbl_Document set IsDelete='1' where DocumentId in({0})";
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonDocument() {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }
        #endregion

        #region PersonDocument 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.PersonDocument model)
        {
            EyouSoft.Data.Document DocumentModel = new EyouSoft.Data.Document()
            {
                CompanyId = model.CompanyId,
                CreateTime = DateTime.Now,
                DocumentId = model.DocumentId,
                DocumentName = model.DocumentName,
                FilePath = model.FilePath,
                IsDelete = model.IsDelete ? "1" : "0",
                OperatorId = model.OperatorId,
                OperatorName = model.OperatorName
            };
            dcDal.Document.InsertOnSubmit(DocumentModel);
            DocumentModel = null;
            dcDal.SubmitChanges();
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">文档管理实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.PersonDocument model)
        {
            EyouSoft.Data.Document DocumentModel = dcDal.Document.FirstOrDefault(item => item.CompanyId == model.CompanyId
                && item.DocumentId == model.DocumentId);
            if (DocumentModel != null)
            { 
                DocumentModel.CompanyId = model.CompanyId;
                DocumentModel.CreateTime = DateTime.Now;
                DocumentModel.DocumentId = model.DocumentId;
                DocumentModel.DocumentName = model.DocumentName;
                DocumentModel.FilePath = model.FilePath;
                DocumentModel.IsDelete = model.IsDelete ? "1" : "0";
                DocumentModel.OperatorId = model.OperatorId;
                DocumentModel.OperatorName = model.OperatorName;
                dcDal.SubmitChanges();
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params string[] Ids)
        {
            string strIds = string.Empty;
            foreach (string str in Ids)
            {
                strIds += str + ",";
            }
            DbCommand dc=this._db.GetSqlStringCommand(string.Format(Sql_PersonDocument_Delete,strIds.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>文档管理实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.PersonDocument GetModel(int Id)
        {
            EyouSoft.Data.Document DocumentModel= dcDal.Document.FirstOrDefault(item => item.DocumentId == Id && item.IsDelete=="0");
            if (DocumentModel != null)
            {
                return new EyouSoft.Model.PersonalCenterStructure.PersonDocument() { 
                CompanyId=DocumentModel.CompanyId,
                CreateTime=DocumentModel.CreateTime,
                DocumentId=DocumentModel.DocumentId,
                DocumentName=DocumentModel.DocumentName,
                FilePath=DocumentModel.FilePath,
                IsDelete=DocumentModel.IsDelete=="1"?true:false,
                OperatorId=DocumentModel.OperatorId,
                OperatorName=DocumentModel.OperatorName
                };
            }
            return null;
        }
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">上传人编号 =0返回所有</param>
        /// <returns>文档管理列表</returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.PersonDocument> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.PersonDocument> list = new List<EyouSoft.Model.PersonalCenterStructure.PersonDocument>();
            string tableName = "tbl_Document";
            string fields = "DocumentId,DocumentName,FilePath,OperatorId,OperatorName,CreateTime";
            string primaryKey = "DocumentId";
            string orderbyStr = " CreateTime Desc ";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            if (CompanyId > 0)
                strWhere.AppendFormat(" and CompanyId={0} ",CompanyId);
            //TODO:根据OperatorId获取相关权限
            if (OperatorId > 0)
                strWhere.AppendFormat("");
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields, strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.PersonDocument model = new EyouSoft.Model.PersonalCenterStructure.PersonDocument();
                    if (!dr.IsDBNull(dr.GetOrdinal("DocumentId")))
                        model.DocumentId = dr.GetInt32(dr.GetOrdinal("DocumentId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DocumentName")))
                    model.DocumentName = dr[dr.GetOrdinal("DocumentName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("FilePath")))
                        model.FilePath = dr[dr.GetOrdinal("FilePath")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorName")))
                        model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        #endregion
    }
}
