using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    public class News : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.INews
    {
        #region static constants
        private const string SQL_SELECT_News = "select * from tbl_News a inner join tbl_NewsAccept b on a.ID = b.[NewId] where a.ID = @ID";
        private const string SQL_DELETE_News = "update tbl_News set IsDelete = '1' ";
        private const string SQL_SETCLICKS = "update tbl_News set Views = Views+1 where ID = @ID";
        private const string SQL_SELECT_DeptId = "select DepartId from tbl_CompanyUser where Id = @Id and CompanyId = @CompanyId";
        private const string SQL_SELECR_ZuTuanNews = "select * from View_AceptNews where AcceptType = 2 and IsDelete = '0' and CompanyId={0} order by IisuerTime desc";
        private readonly Database _db = null;
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        #endregion

        #region 构造函数
        public News()
        {
            this._db = base.SystemStore;
            this.dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion


        #region INews 成员

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.News model)
        {
            EyouSoft.Data.News dataNews = new EyouSoft.Data.News();
            dataNews.Id = model.ID;
            dataNews.CompanyId = model.CompanyId;
            dataNews.Title = model.Title;
            dataNews.Iisuer = string.IsNullOrEmpty(model.OperatorName) ? "" : model.OperatorName;
            dataNews.IisuerTime = model.IssueTime;
            dataNews.Content = model.Content;
            dataNews.Files = model.UploadFiles;
            dataNews.Views = model.Clicks;
            dataNews.IsDelete = model.IsDelete ? "1" : "0";

            if (model.AcceptList != null && model.AcceptList.Count > 0)
            {
                ((List<EyouSoft.Model.CompanyStructure.NewsAccept>)model.AcceptList).ForEach(item =>
                {
                    EyouSoft.Data.NewsAccept dataNewsAccept = new EyouSoft.Data.NewsAccept();
                    //dataNewsAccept.NewId = model.ID;
                    dataNewsAccept.AcceptId = item.AcceptId;
                    dataNewsAccept.AcceptType = (byte)item.AcceptType;
                    dataNews.NewNewsAcceptList.Add(dataNewsAccept);
                    dataNewsAccept = null;
                });
            }

            dcDal.News.InsertOnSubmit(dataNews);
            dcDal.SubmitChanges();
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公告信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.News model)
        {
            EyouSoft.Data.News dataNews = dcDal.News.FirstOrDefault(item => item.Id == model.ID);

            if (dataNews != null)
            {
                dataNews.Id = model.ID;
                dataNews.CompanyId = model.CompanyId;
                dataNews.Title = model.Title;
                dataNews.Iisuer = string.IsNullOrEmpty(model.OperatorName) ? "" : model.OperatorName;
                dataNews.IisuerTime = model.IssueTime;
                dataNews.Content = model.Content;
                if (!string.IsNullOrEmpty(model.UploadFiles))
                {
                    dataNews.Files = model.UploadFiles;
                }
                dataNews.Views = model.Clicks;
                dataNews.IsDelete = model.IsDelete ? "1" : "0";

                if (dataNews.NewNewsAcceptList != null && dataNews.NewNewsAcceptList.Count > 0)
                {
                    if (this.AcceptNewsDelete(dataNews.Id))
                    {
                        ((List<EyouSoft.Model.CompanyStructure.NewsAccept>)model.AcceptList).ForEach(item =>
                            {
                                EyouSoft.Data.NewsAccept dataNewsAccept = new EyouSoft.Data.NewsAccept();
                                dataNewsAccept.NewId = model.ID;
                                dataNewsAccept.AcceptId = item.AcceptId;
                                dataNewsAccept.AcceptType = (byte)item.AcceptType;
                                dataNews.NewNewsAcceptList.Add(dataNewsAccept);
                                dataNewsAccept = null;
                            }
                        );
                    }
                }

                dcDal.SubmitChanges();
            }

            return dcDal.ChangeConflicts.Count == 0 ? true : false;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            if (Ids == null || Ids.Length <= 0)
                return false;

            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += "'" + str.ToString().Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand cmd = _db.GetSqlStringCommand(SQL_DELETE_News + " where ID in (" + strIds + ");");

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公告信息实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.News GetModel(int Id)
        {
            EyouSoft.Data.News newsModel = dcDal.News.FirstOrDefault(item => item.Id == Id);
            IList<EyouSoft.Model.CompanyStructure.NewsAccept> ls = new List<EyouSoft.Model.CompanyStructure.NewsAccept>();
            EyouSoft.Model.CompanyStructure.NewsAccept contact = null;

            if (newsModel != null)
            {
                foreach (var item in newsModel.NewNewsAcceptList)
                {
                    contact = new EyouSoft.Model.CompanyStructure.NewsAccept();
                    contact.NewId = item.NewId;
                    contact.AcceptId = item.AcceptId;
                    contact.AcceptType = (EyouSoft.Model.EnumType.PersonalCenterStructure.AcceptType)(byte)item.AcceptType;
                    ls.Add(contact);
                }
            }

            return new EyouSoft.Model.CompanyStructure.News()
            {
                #region 实体赋值
                ID = newsModel.Id,
                CompanyId = newsModel.CompanyId,
                Title = newsModel.Title,
                OperatorName = newsModel.Iisuer,
                IssueTime = newsModel.IisuerTime,
                Content = newsModel.Content,
                UploadFiles = newsModel.Files,
                Clicks = (int)newsModel.Views,
                IsDelete = newsModel.IsDelete == "1" ? true : false,
                AcceptList = ls
                #endregion
            };
        }

        /// <summary>
        /// 设置点击次数
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetClicks(int id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SETCLICKS);

            this._db.AddInParameter(cmd, "ID", DbType.Int32, id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 分页获取公告信息列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.News> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.News> totals = new List<EyouSoft.Model.CompanyStructure.News>();

            string tableName = "tbl_News";
            string primaryKey = "ID";
            string orderByString = "IisuerTime DESC";
            string fields = " Id, CompanyId, Title, Iisuer,IisuerTime,[Content],Views,Files,IsDelete";

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' ");
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.News newInfo = new EyouSoft.Model.CompanyStructure.News();

                    newInfo.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    newInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    newInfo.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? " " : rdr.GetString(rdr.GetOrdinal("Title"));
                    newInfo.OperatorName = rdr.GetString(rdr.GetOrdinal("Iisuer"));
                    newInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IisuerTime"));
                    newInfo.Content = rdr.IsDBNull(rdr.GetOrdinal("Content")) ? " " : rdr.GetString(rdr.GetOrdinal("Content"));
                    newInfo.Clicks = rdr.IsDBNull(rdr.GetOrdinal("Views")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Views"));
                    newInfo.UploadFiles = rdr.IsDBNull(rdr.GetOrdinal("Files")) ? " " : rdr.GetString(rdr.GetOrdinal("Files"));
                    newInfo.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));

                    totals.Add(newInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取某个用户接收到的消息列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="companydId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int userId, int companydId)
        {
            int deptId = 0;
            IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> lsNews = new List<EyouSoft.Model.PersonalCenterStructure.NoticeNews>();

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_DeptId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, userId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companydId);
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    deptId = rdr.IsDBNull(rdr.GetOrdinal("DepartId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                }
            }

            string tableName = "View_AceptNews";
            string primaryKey = "ID";
            string orderByString = "IisuerTime DESC";
            string fields = " ID, Title,Views,Iisuer,IisuerTime";

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0'");
            cmdQuery.AppendFormat(" and CompanyId = {1} and (AcceptId = {0} or AcceptType = 0)", deptId, companydId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.NoticeNews newInfo = new EyouSoft.Model.PersonalCenterStructure.NoticeNews();
                    newInfo.Id = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    newInfo.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title"));
                    newInfo.ClickNum = rdr.IsDBNull(rdr.GetOrdinal("Views")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Views"));
                    newInfo.OperateName = rdr.IsDBNull(rdr.GetOrdinal("Iisuer")) ? "" : rdr.GetString(rdr.GetOrdinal("Iisuer"));
                    newInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IisuerTime"));

                    lsNews.Add(newInfo);
                }
            }

            return lsNews;
        }

        /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int CompanyId)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> lsNews = new List<EyouSoft.Model.PersonalCenterStructure.NoticeNews>();
            EyouSoft.Model.PersonalCenterStructure.NoticeNews model = null;

            DbCommand cmd = this._db.GetSqlStringCommand(String.Format(SQL_SELECR_ZuTuanNews, CompanyId));

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.PersonalCenterStructure.NoticeNews();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    model.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title"));
                    model.ClickNum = rdr.IsDBNull(rdr.GetOrdinal("Views")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Views"));
                    model.OperateName = rdr.IsDBNull(rdr.GetOrdinal("Iisuer")) ? "" : rdr.GetString(rdr.GetOrdinal("Iisuer"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IisuerTime"));
                    lsNews.Add(model);
                }
            }

            return lsNews;
        }

        /// <summary>
        /// 获取组团端消息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> GetZuTuanAcceptNews(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.PersonalCenterStructure.NoticeNews SearchModel)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> lsNews = new List<EyouSoft.Model.PersonalCenterStructure.NoticeNews>();
            EyouSoft.Model.PersonalCenterStructure.NoticeNews model = null;
            string tableName = "View_AceptNews";
            string primaryKey = "ID";
            string orderByString = "IisuerTime DESC";
            string fields = " ID, Title,Views,Iisuer,IisuerTime";
            StringBuilder cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat(" AcceptType = 2 and IsDelete = '0' and CompanyId={0}", CompanyId);
            if (SearchModel != null)
            {
                if (!String.IsNullOrEmpty(SearchModel.Title))
                {
                    cmdQuery.AppendFormat(" AND Title like '%{0}%' ", SearchModel.Title);
                }
                if (!String.IsNullOrEmpty(SearchModel.OperateName))
                {
                    cmdQuery.AppendFormat(" AND Iisuer='{0}' ", SearchModel.OperateName.Trim());
                }
                if (SearchModel.IssueTime.HasValue && SearchModel.IssueTime!=DateTime.MinValue)
                {
                    cmdQuery.AppendFormat(" AND  DATEDIFF(DAY,'{0}',IisuerTime)=0", SearchModel.IssueTime);
                }
            }
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.PersonalCenterStructure.NoticeNews();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    model.Title = rdr.IsDBNull(rdr.GetOrdinal("Title")) ? "" : rdr.GetString(rdr.GetOrdinal("Title"));
                    model.OperateName = rdr.IsDBNull(rdr.GetOrdinal("Iisuer")) ? "" : rdr.GetString(rdr.GetOrdinal("Iisuer"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IisuerTime"));
                    lsNews.Add(model);
                }
            }

            return lsNews;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 删除接收对象信息
        /// </summary>
        /// <param name="acceptId">接收对象ID</param>
        /// <returns></returns>
        private bool AcceptNewsDelete(int acceptId)
        {
            IEnumerable<EyouSoft.Data.NewsAccept> Lists = from item in dcDal.NewsAccept
                                                          where item.NewId == acceptId
                                                          select item;
            dcDal.NewsAccept.DeleteAllOnSubmit<EyouSoft.Data.NewsAccept>(Lists);
            dcDal.SubmitChanges();
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        #endregion
    }
}
