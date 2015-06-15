using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class Department : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.IDepartment
    {
        #region static constants
        private const string SQL_INSERT_Department = "insert into tbl_CompanyDepartment (DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,PageHeadFile,PageFootFile,TemplateFile,DepartStamp,"
                                                    + "Remark,CompanyId,OperatorId,IsDelete) values(@DepartName,@PrevDepartId,@DepartManger,@ContactTel,@ContactFax,@PageHeadFile,@PageFootFile,@TemplateFile,"
                                                    + "@DepartStamp,@Remark,@CompanyId,@OperatorId,@IsDelete)";
        private const string SQL_UPDATE_Department = "update tbl_CompanyDepartment set DepartName = @DepartName,DepartManger=@DepartManger,ContactTel=@ContactTel,"
                                                    + "ContactFax=@ContactFax,Remark=@Remark,CompanyId=@CompanyId,OperatorId=@OperatorId ";
        private const string SQL_SELECT_Department = "select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,"
                                                    + "PageHeadFile,PageFootFile,TemplateFile,DepartStamp,Remark,"
                                                    + "CompanyId,OperatorId,IssueTime,IsDelete from tbl_CompanyDepartment where Id = @Id";
        private const string SQL_DELETE_Department = "update tbl_CompanyDepartment set IsDelete = '1' where Id=@Id";
        private const string SQL_SELECT_GetList = "select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,"
                                                + "PageHeadFile,PageFootFile,TemplateFile,DepartStamp,Remark,"
                                                + "CompanyId,OperatorId,IssueTime,IsDelete from tbl_CompanyDepartment where CompanyId = @CompanyId and ";
        private const string SQL_SELECT_GetAllDept = "select Id,DepartName,PrevDepartId,DepartManger,ContactTel,ContactFax,"
                                                + "PageHeadFile,PageFootFile,TemplateFile,DepartStamp,Remark,"
                                                + "CompanyId,OperatorId,IssueTime,IsDelete from tbl_CompanyDepartment where CompanyId = @CompanyId and IsDelete = '0'";
        private const string SQL_GetIdByPid = "select Id from tbl_CompanyDepartment where PrevDepartId = @PrevDepartId and CompanyId = @CompanyId";
        private const string SQL_HasNextLev = "select count(*) from tbl_CompanyDepartment where PrevDepartId = @Id and IsDelete = '0'";
        private const string SQL_HasDeptUser = "select count(*) from tbl_CompanyUser where DepartID = @DepartID and CompanyId = @CompanyId and IsDelete = '0' and UserType = 2";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public Department()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region IDepartment 成员

        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Department model)
        {
            //SQL_INSERT_Department
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_Department);

            this._db.AddInParameter(cmd, "DepartName", DbType.String, model.DepartName);
            this._db.AddInParameter(cmd, "PrevDepartId", DbType.Int32, model.PrevDepartId);
            this._db.AddInParameter(cmd, "DepartManger", DbType.Int32, model.DepartManger);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "ContactFax", DbType.String, model.ContactFax);
            this._db.AddInParameter(cmd, "PageHeadFile", DbType.String, model.PageHeadFile);
            this._db.AddInParameter(cmd, "PageFootFile", DbType.String, model.PageFootFile);
            this._db.AddInParameter(cmd, "TemplateFile", DbType.String, model.TemplateFile);
            this._db.AddInParameter(cmd, "DepartStamp", DbType.String, model.DepartStamp);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IsDelete", DbType.String, model.IsDelete == true ? "1" : "0");

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Department model)
        {
            StringBuilder updateStr = new StringBuilder();

            #region 更新条件
            //if (!string.IsNullOrEmpty(model.PageHeadFile))
            //{
            //    updateStr.AppendFormat(",PageHeadFile='{0}'", model.PageHeadFile);
            //}
            //if (!string.IsNullOrEmpty(model.PageFootFile))
            //{
            //    updateStr.AppendFormat(",PageFootFile='{0}'", model.PageFootFile);
            //}
            //if (!string.IsNullOrEmpty(model.TemplateFile))
            //{
            //    updateStr.AppendFormat(",TemplateFile='{0}'", model.TemplateFile);
            //}
            //if (!string.IsNullOrEmpty(model.DepartStamp))
            //{
            //    updateStr.AppendFormat(",DepartStamp='{0}'", model.DepartStamp);
            //}
            #endregion

            updateStr.AppendFormat(",PageHeadFile='{0}'", model.PageHeadFile);
            updateStr.AppendFormat(",PageFootFile='{0}'", model.PageFootFile);
            updateStr.AppendFormat(",TemplateFile='{0}'", model.TemplateFile);
            updateStr.AppendFormat(",DepartStamp='{0}'", model.DepartStamp);
            updateStr.Append(" where CompanyId=@CompanyId and Id= @Id");
            //同步修改用户表的部门名称
            updateStr.Append(";update tbl_CompanyUser set DepartName=@DepartName where DepartId=@Id");
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_Department + updateStr.ToString());
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "DepartName", DbType.String, model.DepartName);
            this._db.AddInParameter(cmd, "DepartManger", DbType.Int32, model.DepartManger);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "ContactFax", DbType.String, model.ContactFax);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Department GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.Department departmentModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_Department);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    departmentModel = new EyouSoft.Model.CompanyStructure.Department();
                    departmentModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    departmentModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    departmentModel.DepartManger = rdr.IsDBNull(rdr.GetOrdinal("DepartManger")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("DepartManger"));
                    departmentModel.PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId"));
                    departmentModel.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    departmentModel.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    departmentModel.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageHeadFile"));
                    departmentModel.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageFootFile"));
                    departmentModel.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr.GetString(rdr.GetOrdinal("TemplateFile"));
                    departmentModel.DepartStamp = rdr.IsDBNull(rdr.GetOrdinal("DepartStamp")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartStamp"));
                    departmentModel.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark"));
                    departmentModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    departmentModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    departmentModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    departmentModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                }
            }

            return departmentModel;
        }

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="Id">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(int Id)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_Department);

            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公司的所有部门信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ParentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Department> GetList(int CompanyId, int ParentDepartId)
        {
            DbCommand cmd = null;
            IList<EyouSoft.Model.CompanyStructure.Department> lsDepartment = new List<EyouSoft.Model.CompanyStructure.Department>();
            string queryStr = string.Empty;

            //0为顶级部门
            if (ParentDepartId == 0)
            {
                DbCommand cmd1 = this._db.GetSqlStringCommand(SQL_GetIdByPid);
                this._db.AddInParameter(cmd1, "PrevDepartId", DbType.Int32, ParentDepartId);
                this._db.AddInParameter(cmd1, "CompanyId", DbType.Int32, CompanyId);
                using (IDataReader rdr1 = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd1, this._db))
                {
                    while (rdr1.Read())
                    {
                        ParentDepartId = rdr1.GetInt32(rdr1.GetOrdinal("Id"));
                    }
                }
                queryStr = string.Format(" IsDelete = '0' and (PrevDepartId = {0} or PrevDepartId = 0)", ParentDepartId);
            }
            else
            {
                queryStr = string.Format(" PrevDepartId = {0} and IsDelete = '0'", ParentDepartId);
            }

            cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetList + queryStr);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.CompanyStructure.Department dpartmentModel = null;

                while (rdr.Read())
                {
                    dpartmentModel = new EyouSoft.Model.CompanyStructure.Department();
                    dpartmentModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    dpartmentModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    dpartmentModel.PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId"));
                    dpartmentModel.DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger"));
                    dpartmentModel.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    dpartmentModel.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    dpartmentModel.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageHeadFile"));
                    dpartmentModel.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageFootFile"));
                    dpartmentModel.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr.GetString(rdr.GetOrdinal("TemplateFile"));
                    dpartmentModel.DepartStamp = rdr.IsDBNull(rdr.GetOrdinal("DepartStamp")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartStamp"));
                    dpartmentModel.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark"));
                    dpartmentModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    dpartmentModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    dpartmentModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    dpartmentModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    dpartmentModel.HasNextLev = HasChildDept(dpartmentModel.Id);
                    lsDepartment.Add(dpartmentModel);
                }
            }

            return lsDepartment;
        }

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Department> GetAllDept(int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.Department> lsDepartment = new List<EyouSoft.Model.CompanyStructure.Department>();

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetAllDept);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                EyouSoft.Model.CompanyStructure.Department dpartmentModel = null;

                while (rdr.Read())
                {
                    dpartmentModel = new EyouSoft.Model.CompanyStructure.Department();
                    dpartmentModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    dpartmentModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    dpartmentModel.PrevDepartId = rdr.GetInt32(rdr.GetOrdinal("PrevDepartId"));
                    dpartmentModel.DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger"));
                    dpartmentModel.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    dpartmentModel.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    dpartmentModel.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageHeadFile"));
                    dpartmentModel.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageFootFile"));
                    dpartmentModel.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr.GetString(rdr.GetOrdinal("TemplateFile"));
                    dpartmentModel.DepartStamp = rdr.IsDBNull(rdr.GetOrdinal("DepartStamp")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartStamp"));
                    dpartmentModel.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark"));
                    dpartmentModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    dpartmentModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    dpartmentModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    dpartmentModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    dpartmentModel.HasNextLev = HasChildDept(dpartmentModel.Id);
                    lsDepartment.Add(dpartmentModel);
                }
            }

            return lsDepartment;
        }

        /// <summary>
        /// 根据用户编号和公司编号获取公司打印文件实体(如果当前用户部门获取到的实体为NULL,则获取总部的)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetDeptPrint(int userId, int companyId)
        {
            //begin
            //if exists(
            //    select 1 from tbl_CompanyDepartment 
            //    where Id = (select DepartId from tbl_CompanyUser where Id = 131) 
            //    and CompanyId = 1)

            //    select PageHeadFile,PageFootFile,TemplateFile,DepartStamp from tbl_CompanyDepartment 
            //    where Id = (select DepartId from tbl_CompanyUser where Id = 131) 
            //    and CompanyId = 1
            //else
            //    select PageHeadFile,PageFootFile,TemplateFile,DepartStamp  from tbl_CompanyDepartment where CompanyId = 1 and PrevDepartId = 0
            //end
            EyouSoft.Model.CompanyStructure.CompanyPrintTemplate model = null;
            StringBuilder sql = new StringBuilder();
            //sql.AppendFormat("begin if exists(select 1 from tbl_CompanyDepartment where Id = (select DepartId from tbl_CompanyUser where Id = {0}) and CompanyId = {1})",
            //                    userId, companyId);
            sql.AppendFormat(" select PageHeadFile,PageFootFile,TemplateFile,DepartStamp from tbl_CompanyDepartment "
                            + " where Id = (select DepartId from tbl_CompanyUser where Id = {0}) and CompanyId = {1}", userId, companyId);
            //sql.AppendFormat(" else select PageHeadFile,PageFootFile,TemplateFile,DepartStamp  from tbl_CompanyDepartment "
            //                + " where CompanyId = {0} and PrevDepartId = 0 end", companyId);
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CompanyPrintTemplate();
                    model.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageHeadFile"));
                    model.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr.GetString(rdr.GetOrdinal("PageFootFile"));
                    model.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr.GetString(rdr.GetOrdinal("TemplateFile"));
                    model.DepartStamp = rdr.IsDBNull(rdr.GetOrdinal("DepartStamp")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartStamp"));
                }
            }

            return model;

        }

        /// <summary>
        /// 判断当前登录用户是否为发布者的上级部门
        /// </summary>
        /// <param name="pubId">登录者ID</param>
        /// <param name="curId">当前用户ID</param>
        /// <returns></returns>
        public int JudgePermission(int pubId, int curId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_JudgePermission");
            this._db.AddInParameter(cmd, "pubId", DbType.Int32, pubId);
            this._db.AddInParameter(cmd, "curId", DbType.Int32, curId);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "result"));
        }
        #endregion

        /// <summary>
        /// 是否有下级部门
        /// </summary>
        /// <param name="id">部门编号</param>
        /// <returns></returns>
        public bool HasChildDept(int id)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_HasNextLev);

            this._db.AddInParameter(cmd, "Id", DbType.String, id);

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
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasDeptUser(int id, int companyId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_HasDeptUser);

            this._db.AddInParameter(cmd, "DepartID", DbType.Int32, id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }
    }
}
