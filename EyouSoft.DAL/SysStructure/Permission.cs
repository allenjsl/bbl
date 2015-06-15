using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Xml.Linq;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.SysStructure
{
    /// <summary>
    /// 权限操作DAL
    /// xuqh 2011-01-23
    /// </summary>
    public class Permission : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SysStructure.IPermission
    {
        #region static constants
        private const string SQL_GetRolePermission = "select RoleChilds from tbl_SysRoleManage where Id = @Id and CompanyId = @CompanyId";
        private const string SQL_GetCompanyPermission = "select RoleChilds from tbl_SysRoleManage CompanyId = @CompanyId";
        private const string SQL_GetSysPermission = "select Permission from tbl_Sys where SysId = @SysId";
        private const string SQL_SetRolePermission = "update tbl_SysRoleManage set RoleChilds = @RoleChilds where Id = @Id and CompanyId = @CompanyId";

        #region SQL_SELECT_GetAllPermission
        /// <summary>
        /// 按照子系统编号获取栏目、子栏目、权限信息
        /// </summary>
        private const string SQL_SELECT_GetAllPermission = @"
SELECT A.*,(
	SELECT B.*
	,(SELECT C.* FROM [tbl_SysPermissionList] AS C WHERE C.[ClassId]=B.[Id] AND CHARINDEX(','+CAST(C.[Id] AS NVARCHAR(10))+',',(SELECT ','+[Permission]+','  FROM [tbl_Sys] WHERE [SysId]=@SysId))>0 FOR XML RAW,ROOT('root')) AS PermissionXML 
	FROM [tbl_SysPermissionClass] AS B WHERE B.[CategoryId]=A.[Id]  AND CHARINDEX( ','+CAST(B.[Id] AS NVARCHAR(10))+',', (SELECT ','+[Part]+',' FROM [tbl_Sys] WHERE [SysId]=@SysId))>0 FOR XML RAW,ROOT('root')
) AS PartXML 
FROM [tbl_SysPermissionCategory] AS A
WHERE CHARINDEX(','+CAST(A.[Id] AS NVARCHAR(10))+',', (SELECT ','+Module+','  FROM [tbl_Sys] WHERE [SysId]=@SysId))>0
";
        /// <summary>
        /// 获取系统所有栏目、子栏目、权限信息
        /// </summary>
        private const string SQL_SELECT_GetSysPermissions = @"
SELECT A.*,(
	SELECT B.*
	,(SELECT C.* FROM [tbl_SysPermissionList] AS C WHERE C.[ClassId]=B.[Id] FOR XML RAW,ROOT('root')) AS PermissionXML 
	FROM [tbl_SysPermissionClass] AS B WHERE B.[CategoryId]=A.[Id] FOR XML RAW,ROOT('root')
) AS PartXML 
FROM [tbl_SysPermissionCategory] AS A  WHERE A.[TypeId]=1
";
        #endregion

        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public Permission()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region IPermission 成员

        /// <summary>
        /// 获取某个角色的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int[] GetRolePermission(int roleId, int CompanyId)
        {
            string strList = string.Empty;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetRolePermission);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, roleId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    strList = rdr.GetString(rdr.GetOrdinal("RoleChilds"));
                }
            }

            string[] strArray = strList.Split(new char[] { ',' });
            return ConvertArray(strArray);
        }

        /// <summary>
        /// 获取某个公司的所有权限
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int[] GetCompanyPermission(int companyId)
        {
            string strList = string.Empty;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetCompanyPermission);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    strList = rdr.GetString(rdr.GetOrdinal("RoleChilds"));
                }
            }

            string[] strArray = strList.Split(new char[] { ',' });
            return ConvertArray(strArray);
        }

        /// <summary>
        /// 获取某个系统的所有权限
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.PermissionCategory> GetAllPermission(int SysId)
        {
            #region *
            //IList<EyouSoft.Model.SysStructure.PermissionCategory> lsCategory = new List<EyouSoft.Model.SysStructure.PermissionCategory>();
            //IList<EyouSoft.Model.SysStructure.PermissionClass> lsClass = null;
            //IList<EyouSoft.Model.SysStructure.Permission> lsPermission = null;

            ////查询权限大类SQL
            //string SQL_SelectPermissionCategory = "select Id,TypeId,CategoryName,SortId,IsEnable from tbl_SysPermissionCategory where TypeId = 1 and IsEnable = 1";
            ////查询权限子类
            //string SQL_SelectPermissionClass = "select Id,CategoryId,ClassName,SortId,IsEnable from tbl_SysPermissionClass where CategoryId = @ParentCategoryId and IsEnable = 1 AND ID IN(SELECT Part FROM tbl_Sys WHERE SysId=@SysId)";
            ////查询权限集合
            //string SQL_SelectPermissionList = "select Id,ClassId,PermissionName,SortId,IsEnable from tbl_SysPermissionList where ClassId = @ParentClassId and IsEnable = 1";

            //DbCommand cmdCategory = this._db.GetSqlStringCommand(SQL_SelectPermissionCategory);
            //DbCommand cmdClass = null;
            //DbCommand cmdList = null;

            //using (IDataReader rdrCategory = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmdCategory, this._db))
            //{
            //    EyouSoft.Model.SysStructure.PermissionCategory model = null;

            //    while (rdrCategory.Read())
            //    {
            //        model = new EyouSoft.Model.SysStructure.PermissionCategory();
            //        model.Id = rdrCategory.GetInt32(rdrCategory.GetOrdinal("Id"));
            //        model.TypeId = rdrCategory.GetInt32(rdrCategory.GetOrdinal("TypeId"));
            //        model.CategoryName = rdrCategory.GetString(rdrCategory.GetOrdinal("CategoryName"));
            //        model.SortId = rdrCategory.GetInt32(rdrCategory.GetOrdinal("SortId"));
            //        model.IsEnable = Convert.ToBoolean(rdrCategory.GetOrdinal("IsEnable"));
            //        #region 权限子类
            //        cmdClass = this._db.GetSqlStringCommand(SQL_SelectPermissionClass);
            //        this._db.AddInParameter(cmdClass, "ParentCategoryId", DbType.Int32, model.Id);
            //        this._db.AddInParameter(cmdClass, "SysId", DbType.Int32, SysId);
            //        using (IDataReader rdrClass = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmdClass, this._db))
            //        {
            //            EyouSoft.Model.SysStructure.PermissionClass modelClass = null;
            //            lsClass = new List<EyouSoft.Model.SysStructure.PermissionClass>();

            //            while (rdrClass.Read())
            //            {
            //                modelClass = new EyouSoft.Model.SysStructure.PermissionClass();
            //                modelClass.Id = rdrClass.GetInt32(rdrClass.GetOrdinal("Id"));
            //                modelClass.CategoryId = rdrClass.GetInt32(rdrClass.GetOrdinal("CategoryId"));
            //                modelClass.ClassName = rdrClass.GetString(rdrClass.GetOrdinal("ClassName"));
            //                modelClass.SortId = rdrClass.GetInt32(rdrClass.GetOrdinal("SortId"));
            //                modelClass.IsEnable = Convert.ToBoolean(rdrClass.GetOrdinal("IsEnable"));
            //                #region 权限集合
            //                cmdList = this._db.GetSqlStringCommand(SQL_SelectPermissionList);
            //                this._db.AddInParameter(cmdList, "ParentClassId", DbType.Int32, modelClass.Id);
            //                using (IDataReader rdrPermission = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmdList, this._db))
            //                {
            //                    EyouSoft.Model.SysStructure.Permission modelPermission = null;
            //                    lsPermission = new List<EyouSoft.Model.SysStructure.Permission>();

            //                    while (rdrPermission.Read())
            //                    {
            //                        modelPermission = new EyouSoft.Model.SysStructure.Permission();
            //                        modelPermission.Id = rdrPermission.GetInt32(rdrPermission.GetOrdinal("Id"));
            //                        modelPermission.ClassId = rdrPermission.GetInt32(rdrPermission.GetOrdinal("ClassId"));
            //                        modelPermission.PermissionName = rdrPermission.GetString(rdrPermission.GetOrdinal("PermissionName"));
            //                        modelPermission.SortId = rdrPermission.GetInt32(rdrPermission.GetOrdinal("SortId"));
            //                        modelPermission.IsEnable = Convert.ToBoolean(rdrPermission.GetOrdinal("IsEnable"));
            //                        lsPermission.Add(modelPermission);
            //                    }
            //                }
            //                #endregion
            //                modelClass.Permission = lsPermission;

            //                lsClass.Add(modelClass);
            //            }
            //        }
            //        #endregion
            //        model.PermissionClass = lsClass;
            //        lsCategory.Add(model);
            //    }
            //}

            //return lsCategory;
            #endregion

            IList<EyouSoft.Model.SysStructure.PermissionCategory> items = new List<EyouSoft.Model.SysStructure.PermissionCategory>();            

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetAllPermission);
            this._db.AddInParameter(cmd, "SysId", DbType.Int32, SysId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SysStructure.PermissionCategory()
                    {
                        CategoryName = rdr["CategoryName"].ToString(),
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        IsEnable = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsEnable"))),
                        PermissionClass = this.ParsePermissionClassByXml(rdr["PartXML"].ToString()),
                        SortId = rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        TypeId = rdr.GetInt32(rdr.GetOrdinal("TypeId"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 设置权限(此方法为设置单个权限-已过期)
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionId">权限编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public bool SetPermission(int roleId,int permissionId, int companyId, string permissionName)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_SetPermission");
            this._db.AddInParameter(cmd, "RoleId", DbType.Int32, roleId);
            this._db.AddInParameter(cmd, "PermissionId", DbType.Int32, permissionId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "PermissionName", DbType.String, permissionName);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="companyId">公司ID</param>
        /// <param name="PermissionList">权限数组</param>
        /// <returns></returns>
        public bool SetRolePermission(int roleId, int companyId, string[] PermissionList)
        {
            string permissionStr = string.Empty;
            foreach (string str in PermissionList)
            {
                permissionStr += str + ",";
            }
            permissionStr = permissionStr.Trim(',');

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SetRolePermission);
            this._db.AddInParameter(cmd, "RoleChilds", DbType.String, permissionStr);
            this._db.AddInParameter(cmd, "RoleId", DbType.Int32, roleId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.String, companyId);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取系统所有栏目、子栏目、权限信息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.PermissionCategory> GetSysPermissions()
        {
            IList<EyouSoft.Model.SysStructure.PermissionCategory> items = new List<EyouSoft.Model.SysStructure.PermissionCategory>();

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSysPermissions);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.SysStructure.PermissionCategory()
                    {
                        CategoryName = rdr["CategoryName"].ToString(),
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        IsEnable = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsEnable"))),
                        PermissionClass = this.ParsePermissionClassByXml(rdr["PartXML"].ToString()),
                        SortId = rdr.GetInt32(rdr.GetOrdinal("SortId")),
                        TypeId = rdr.GetInt32(rdr.GetOrdinal("TypeId"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 添加系统权限基础数据，返回添加的编号
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="firstId">第一级编号</param>
        /// <param name="secondId">第二级编号</param>
        /// <returns></returns>
        public int InsertSysPermission(string name, int? firstId, int? secondId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Sys_InsertSysPermission");
            this._db.AddInParameter(cmd, "Name", DbType.String, name);
            firstId = firstId ?? 0;
            secondId = secondId ?? 0;
            this._db.AddInParameter(cmd, "Firstid", DbType.Int32, firstId.Value);
            this._db.AddInParameter(cmd, "SecondId", DbType.Int32, secondId.Value);
            this._db.AddOutParameter(cmd, "IdentityId", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "IdentityId"));
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 把字符数组转换成整形数组
        /// </summary>
        /// <param name="strArray">字符数组</param>
        /// <returns></returns>
        private int[] ConvertArray(string[] strArray)
        {
            int[] roleChilds = new int[strArray.Count()];

            for (int i = 0; i < (strArray.Count()); i++)
            {
                roleChilds[i] = Convert.ToInt32(strArray[i]);
            }

            return roleChilds;
        }

        /// <summary>
        /// 获取系统权限数组
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        private string GetSysPermission(int SysId)
        {
            string strList = string.Empty;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetSysPermission);
            this._db.AddInParameter(cmd, "SysId", DbType.Int32, SysId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    strList = rdr.GetString(rdr.GetOrdinal("Permission"));
                }
            }

            return strList;
        }

        /// <summary>
        /// 根据XML解析系统权限子类信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SysStructure.PermissionClass> ParsePermissionClassByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            IList<EyouSoft.Model.SysStructure.PermissionClass> items = new List<EyouSoft.Model.SysStructure.PermissionClass>();
            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");

            foreach (var xRow in xRows)
            {
                items.Add(new EyouSoft.Model.SysStructure.PermissionClass()
                {
                    CategoryId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "CategoryId")),
                    ClassName = Utils.GetXAttributeValue(xRow, "ClassName"),
                    Id = Utils.GetInt(Utils.GetXAttributeValue(xRow, "Id")),
                    IsEnable = this.GetBoolean(Utils.GetXAttributeValue(xRow, "IsEnable")),
                    Permission = this.ParsePermissionByXml(Utils.GetXAttributeValue(xRow, "PermissionXML")),
                    SortId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "SortId"))
                });
            }

            return items;
        }

        /// <summary>
        /// 根据XML解析权限信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.SysStructure.Permission> ParsePermissionByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.SysStructure.Permission> items = new List<EyouSoft.Model.SysStructure.Permission>();
            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                items.Add(new EyouSoft.Model.SysStructure.Permission()
                {
                    ClassId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "ClassId")),
                    Id = Utils.GetInt(Utils.GetXAttributeValue(xRow, "Id")),
                    IsEnable = this.GetBoolean(Utils.GetXAttributeValue(xRow, "IsEnable")),
                    PermissionName = Utils.GetXAttributeValue(xRow, "PermissionName"),
                    SortId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "SortId"))
                });
            }
            
            return items;
        }
        #endregion
    }
}
