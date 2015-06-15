using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 专线商角色管理DAL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class SysRoleManage : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ISysRoleManage
    {
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysRoleManage()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion 构造函数

        #region 实现接口公共方法
        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SysRoleManage> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.SysRoleManage> ResultList = null;
            string tableName = "tbl_SysRoleManage";
            string identityColumnName = "Id";
            string fields = "[Id],[RoleChilds],[RoleName]";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            string orderByString = " [id] ASC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.CompanyStructure.SysRoleManage>();
                while (dr.Read())
                {
                    EyouSoft.Model.CompanyStructure.SysRoleManage model = new EyouSoft.Model.CompanyStructure.SysRoleManage();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.RoleChilds = dr.GetString(dr.GetOrdinal("RoleChilds"));
                    model.RoleName = dr.GetString(dr.GetOrdinal("RoleName"));
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">角色编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.SysRoleManage GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.CompanyStructure.SysRoleManage model = null;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
            EyouSoft.Data.SysRoleManage DataModel = dcDal.SysRoleManage.FirstOrDefault(item =>
                item.CompanyId == CompanyId && item.Id == Id
            );
            if (DataModel != null)
            {
                model = new EyouSoft.Model.CompanyStructure.SysRoleManage()
                {
                    Id = DataModel.Id,
                    RoleName = DataModel.RoleName,
                    RoleChilds = DataModel.RoleChilds
                };
            }
            DataModel = null;
            return model;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysRoleManage model)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
            bool IsTrue = false;
            EyouSoft.Data.SysRoleManage DataModel = new EyouSoft.Data.SysRoleManage()
            {
                CompanyId = model.CompanyId,
                RoleChilds = model.RoleChilds.Trim(),
                RoleName = model.RoleName,
                IsDelete = "0"
            };
            dcDal.SysRoleManage.InsertOnSubmit(DataModel);
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
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.SysRoleManage model)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_SysRoleManage_Update");
            this._db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "RoleName", DbType.String, model.RoleName);
            this._db.AddInParameter(dc, "RoleChilds", DbType.AnsiString, model.RoleChilds);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }            

            return IsTrue;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, params int[] RoleId)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);

            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.SysRoleManage> SysRoleLists = (from item in dcDal.SysRoleManage
                                                                     where item.CompanyId == CompanyId && RoleId.Contains(item.Id)
                                                                     select item);
            dcDal.SysRoleManage.DeleteAllOnSubmit<EyouSoft.Data.SysRoleManage>(SysRoleLists);
            dcDal.SubmitChanges();
            IsTrue = true;
            SysRoleLists = null;
            
            return IsTrue;
        }
        #endregion 实现接口公共方法
    }
}
