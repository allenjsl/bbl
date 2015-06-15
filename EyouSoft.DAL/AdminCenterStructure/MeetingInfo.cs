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
    public class MeetingInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IMeetingInfo
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MeetingInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 获取会议记录管理实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.MeetingInfo GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.AdminCenterStructure.MeetingInfo model = null;
            model = (from item in dcDal.MeetingInfo
                     where item.CompanyId == CompanyId && item.Id == Id
                     select new EyouSoft.Model.AdminCenterStructure.MeetingInfo
                     {
                         Id = item.Id,
                         BeginDate = item.BeginDate,
                         EndDate = item.EndDate,
                         Location = item.Location,
                         MetttingNo = item.MetttingNo,
                         Personal = item.Personal,
                         Remark = item.Remark,
                         Title = item.Title
                     }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取会议记录管理信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="MeetingNo">会议编号</param>
        /// <param name="MeetingTile">会议主题</param>
        /// <param name="BeginStart">会议时间开始</param>
        /// <param name="BeginEnd">会议时间结束</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.MeetingInfo> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, string MeetingNo,string MeetingTile, DateTime? BeginStart, DateTime? BeginEnd)
        {
            IList<EyouSoft.Model.AdminCenterStructure.MeetingInfo> ResultList = null;
            string tableName = "tbl_MeetingInfo";
            string identityColumnName = "Id";
            string fields = " [Id],[MetttingNo],[Title],[Personal],[BeginDate],[EndDate],[Location],[Remark] ";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(MeetingNo))
            {
                query = query + string.Format(" AND MetttingNo LIKE'%{0}%'", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(MeetingNo));
            }            
            if (!string.IsNullOrEmpty(MeetingTile))
            {
                query = query + string.Format(" AND Title LIKE'%{0}%'", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(MeetingTile));
            }
            if (BeginStart.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(mi,'{0}',BeginDate)>=0", BeginStart);
            }
            if (BeginEnd.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(mi,BeginDate,'{0}')>=0", BeginEnd);
            }
            string orderByString = " IssueTime DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.MeetingInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.MeetingInfo model = new EyouSoft.Model.AdminCenterStructure.MeetingInfo()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        BeginDate = dr.GetDateTime(dr.GetOrdinal("BeginDate")),
                        EndDate = dr.GetDateTime(dr.GetOrdinal("EndDate")),
                        Location = dr.IsDBNull(dr.GetOrdinal("Location")) ? "" : dr.GetString(dr.GetOrdinal("Location")),
                        MetttingNo = dr.IsDBNull(dr.GetOrdinal("MetttingNo")) ? "" : dr.GetString(dr.GetOrdinal("MetttingNo")),
                        Personal = dr.IsDBNull(dr.GetOrdinal("Personal")) ? "" : dr.GetString(dr.GetOrdinal("Personal")),
                        Title = dr.IsDBNull(dr.GetOrdinal("Title")) ? "" : dr.GetString(dr.GetOrdinal("Title")),
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
        /// <param name="model">会议记录管理实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.MeetingInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.MeetingInfo DataModel = new EyouSoft.Data.MeetingInfo()
            {
                EndDate = model.EndDate,
                BeginDate = model.BeginDate,
                CompanyId = model.CompanyId,
                Title = model.Title,
                Remark = model.Remark,
                Personal = model.Personal,
                MetttingNo = model.MetttingNo,
                Location = model.Location,
                OperatorId = model.OperatorId,
                IssueTime = System.DateTime.Now
            };
            dcDal.MeetingInfo.InsertOnSubmit(DataModel);
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
        /// <param name="model">会议记录管理实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.MeetingInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.MeetingInfo DataModel = dcDal.MeetingInfo.FirstOrDefault(item =>
               item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                DataModel.Location = model.Location;
                DataModel.MetttingNo = model.MetttingNo;
                DataModel.Personal = model.Personal;
                DataModel.Remark = model.Remark;
                DataModel.Title = model.Title;
                DataModel.BeginDate = model.BeginDate;
                DataModel.EndDate = model.EndDate;
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
        /// <param name="Id">会议记录管理编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            EyouSoft.Data.MeetingInfo DataModel = dcDal.MeetingInfo.FirstOrDefault(item =>
                item.Id == Id && item.CompanyId == CompanyId
            );
            if (DataModel != null)
            {
                dcDal.MeetingInfo.DeleteOnSubmit(DataModel);
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
