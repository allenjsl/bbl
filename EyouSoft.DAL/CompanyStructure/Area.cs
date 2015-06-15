using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Xml.Linq;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司线路区域DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class Area : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.IArea
    {
        #region static constants
        private const string SQL_SELECT_ISEXISTS = "select count(*) from tbl_Area where AreaName = @AreaName and CompanyId = @CompanyId and Id != @Id";
        private const string SQL_INSERT_Area = "insert into tbl_Area (AreaName,CompanyId,OperatorId,IsDelete,SortId) values(@AreaName,@CompanyId,@OperatorId,@IsDelete,@SortId);SELECT @@IDENTITY";
        //private const string SQL_UPDATE_Area = "update tbl_Area set AreaName = @AreaName,SortId=@SortId where Id = @Id";
        private const string SQL_SELECT_Area = "select a.Id,a.AreaName,a.CompanyId,a.OperatorId,a.IssueTime,a.IsDelete,b.UserId,b.AreaId,c.ContactName" 
                                                + " from tbl_Area a inner join tbl_UserArea b"
                                                + " on a.Id = b.AreaId inner join tbl_CompanyUser c"
                                                + " on b.UserId = c.Id where a.Id = @Id";
        private const string SQL_DELETE_Area = "update tbl_Area set IsDelete = '1' ";
        private const string SQL_GetAreaList = "select * from tbl_Area a inner join tbl_UserArea b on a.Id = b.AreaId where b.UserId = @UserId and IsDelete = '0'";
        private const string SQL_GetAreaByCompanyId = "select Id,AreaName from tbl_Area where CompanyId = @CompanyId and IsDelete = '0' ORDER BY [SortId] ASC";
        const string SQL_SELECT_GetAreaSortId = "SELECT MIN(SortId) AS MinSortId,MAX(SortId) AS MaxSortId FROM tbl_Area WHERE CompanyId=@CompanyId AND IsDelete='0'";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public Area()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region IArea 成员

        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="AreaName">线路区域名称</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string AreaName, int CompanyId,int Id)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_ISEXISTS);

            this._db.AddInParameter(cmd, "AreaName", DbType.String, AreaName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <param name="userIds">用户编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Area model,string[] userIds)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_Area);
            StringBuilder SQL_INSERT_AreaUser = new StringBuilder();
            this._db.AddInParameter(cmd, "AreaName", DbType.String, model.AreaName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IsDelete", DbType.String, model.IsDelete == true ? "1" : "0");
            _db.AddInParameter(cmd, "SortId", DbType.Int32, model.SortId);

            object obj = EyouSoft.Toolkit.DAL.DbHelper.GetSingle(cmd, this._db);

            foreach (string str in userIds)
            {
                SQL_INSERT_AreaUser.AppendFormat("insert into tbl_UserArea (UserId,AreaId) values({0},{1});", Convert.ToInt32(str), Convert.ToInt32(obj));
            }
            DbCommand cmdUserArea = this._db.GetSqlStringCommand(SQL_INSERT_AreaUser.ToString());
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(cmdUserArea, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Area model, string[] userIds)
        {

            DbCommand cmd = this._db.GetStoredProcCommand("proc_UserArea_Update");
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "AreaName", DbType.String, model.AreaName);
            this._db.AddInParameter(cmd, "UserAreaXml", DbType.String, this.CreateAreaUserXML(model.AreaUserList));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32,4);
            _db.AddInParameter(cmd, "SortId", DbType.Int32, model.SortId);
            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);
            object obj = this._db.GetParameterValue(cmd, "Result");
            return Convert.ToInt32(obj) > 0 ? true : false;
        }

        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Area GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.Area areaModel = null;
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Area_GetAreaInfo");
            this._db.AddInParameter(cmd,"Id",DbType.Int32,Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if(rdr.Read())
                {
                    #region 线路区域信息
                    areaModel = new EyouSoft.Model.CompanyStructure.Area();
                    areaModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    areaModel.AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"));
                    areaModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    areaModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    areaModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    areaModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    areaModel.SortId = rdr.GetInt32(rdr.GetOrdinal("SortId"));
                    #endregion

                    #region 计调员信息
                    rdr.NextResult();
                    IList<EyouSoft.Model.CompanyStructure.UserArea> lsUserArea = new List<EyouSoft.Model.CompanyStructure.UserArea>();
                    while(rdr.Read())
                    {
                        EyouSoft.Model.CompanyStructure.UserArea userAreaModel = new EyouSoft.Model.CompanyStructure.UserArea();
                        userAreaModel.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                        userAreaModel.UserId = rdr.GetInt32(rdr.GetOrdinal("UserId"));
                        userAreaModel.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName"));
                        lsUserArea.Add(userAreaModel);
                        userAreaModel = null;
                    }
                    #endregion

                    areaModel.AreaUserList = lsUserArea;
                }
            }

            return areaModel;
        }

        /// <summary>
        /// 删除线路区域集合
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            if (Ids == null || Ids.Length <= 0)
                return false;

            DbCommand dc = _db.GetSqlStringCommand(SQL_DELETE_Area + " where Id in (" + ConvertToString(Ids) + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        public bool IsAreaPublish(int areaId, int companyId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Area_IsAreaPublish");
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, areaId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);
            object obj = this._db.GetParameterValue(cmd, "Result");
            return Convert.ToInt32(obj) > 0 ? true : false;
        }

        /// <summary>
        /// 删除线路区域用户关联信息
        /// </summary>
        /// <param name="AreaIds"></param>
        /// <returns></returns>
        public bool DeleteUserArea(params int[] AreaIds)
        {
            if (AreaIds == null || AreaIds.Length <= 0)
                return false;

            string sql = string.Format("delete from tbl_UserArea where AreaId in ({0})", ConvertToString(AreaIds));
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.Area> totals = new List<EyouSoft.Model.CompanyStructure.Area>();

            string tableName = "tbl_Area";
            string primaryKey = "Id";
            string orderByString = "SortId ASC,IssueTime desc";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id, AreaName, CompanyId, OperatorId, IssueTime,IsDelete,");
            fields.Append(" (select UserId,AreaId,ContactName from tbl_UserArea a left join tbl_CompanyUser b on a.UserId = b.Id where a.AreaId = tbl_Area.Id and b.IsDelete = '0' and b.IsEnable = '1' and b.UserType = 2  for xml raw,root('root')) as UserAreaXML");
            fields.Append(" ,SortId ");

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' ");
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.Area areaInfo = new EyouSoft.Model.CompanyStructure.Area();

                    areaInfo.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    areaInfo.AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"));
                    areaInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    areaInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    areaInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    areaInfo.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    areaInfo.AreaUserList = GetUserAreaList(rdr.IsDBNull(rdr.GetOrdinal("UserAreaXML")) ? "" : rdr.GetString(rdr.GetOrdinal("UserAreaXML")));
                    areaInfo.SortId = rdr.GetInt32(rdr.GetOrdinal("SortId"));

                    totals.Add(areaInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.Area> lsArea = new List<EyouSoft.Model.CompanyStructure.Area>();
            EyouSoft.Model.CompanyStructure.Area areaModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetAreaByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    areaModel = new EyouSoft.Model.CompanyStructure.Area();
                    areaModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    areaModel.AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"));
                    lsArea.Add(areaModel);
                }
            }

            return lsArea;
        }

        /// <summary>
        /// 获取线路区域集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreaList(int userId)
        {
            IList<EyouSoft.Model.CompanyStructure.Area> lsArea = new List<EyouSoft.Model.CompanyStructure.Area>();
            EyouSoft.Model.CompanyStructure.Area areaModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetAreaList);
            this._db.AddInParameter(cmd, "UserId", DbType.Int32, userId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    areaModel = new EyouSoft.Model.CompanyStructure.Area();
                    areaModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    areaModel.AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"));
                    areaModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    areaModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    areaModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    areaModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    lsArea.Add(areaModel);
                }
            }

            return lsArea;
        }

        /// <summary>
        /// 根据当前登录用户获取同级及下级部门人员的线路区域信息集合
        /// </summary>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreas(string us)
        {
            IList<EyouSoft.Model.CompanyStructure.Area> ls = new List<EyouSoft.Model.CompanyStructure.Area>();
            EyouSoft.Model.CompanyStructure.Area model = null;
            string sql = string.Format("select * from tbl_Area where Id in (select AreaId from tbl_UserArea where UserId in ({0}))", us);
            DbCommand cmd = _db.GetSqlStringCommand(sql);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.Area();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.AreaName = rdr.GetString(rdr.GetOrdinal("AreaName"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    model.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    ls.Add(model);
                }
            }

            return ls;
        }

        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        public void GetAreaSortId(int companyId, out int min, out int max)
        {
            min = 0; max = 0;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetAreaSortId);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("MinSortId")))
                        min = rdr.GetInt32(rdr.GetOrdinal("MinSortId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("MaxSortId")))
                        max = rdr.GetInt32(rdr.GetOrdinal("MaxSortId"));
                }
            }

        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.UserArea> GetUserAreaList(string userAreaXML)
        {
            if (string.IsNullOrEmpty(userAreaXML))
                return null;
            IList<EyouSoft.Model.CompanyStructure.UserArea> ResultList = new List<EyouSoft.Model.CompanyStructure.UserArea>();
            XElement root = XElement.Parse(userAreaXML);
            var xRow = root.Elements("row");
            foreach (var tmp in xRow)
            {
                EyouSoft.Model.CompanyStructure.UserArea model = new EyouSoft.Model.CompanyStructure.UserArea()
                {
                    UserId = Convert.ToInt32(tmp.Attribute("UserId").Value),
                    AreaId = Convert.ToInt32(tmp.Attribute("AreaId").Value),
                    ContactName = tmp.Attribute("ContactName") == null ? "" : tmp.Attribute("ContactName").Value
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }

        /// <summary>
        /// 创建线路附件信息的XML
        /// </summary>
        /// <param name="Attachs">线路附件列表</param>
        /// <returns></returns>
        private string CreateAreaUserXML(IList<EyouSoft.Model.CompanyStructure.UserArea> UserArea)
        {
            if (UserArea == null || UserArea.Count == 0)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (EyouSoft.Model.CompanyStructure.UserArea model in UserArea)
            {
                strXml.AppendFormat("<UserAreaInfo UserId=\"{0}\" AreaId=\"{1}\" />", Utils.ReplaceXmlSpecialCharacter(model.UserId.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.AreaId.ToString()));
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        private string ConvertToString(params int[] Ids)
        {
            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += "'" + str.ToString().Trim() + "',";
            }
            strIds = strIds.Trim(',');
            return strIds;
        }
        #endregion
    }
}
