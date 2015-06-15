using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 用户信息数据层
    /// </summary>
    /// 鲁功源 2011-01-20
    public class CompanyUser : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyUser
    {
        #region 变量
        private const string Sql_CompanyUser_Remove = "update tbl_CompanyUser set IsDelete='1' where Id in({0}) AND IsAdmin='0' ";
        private const string Sql_CompanyUser_Delete = "delete tbl_CompanyUser where Id in({0})";
        private const string Sql_GetList = "select * from tbl_CompanyUser where CompanyId = @CompanyId and IsDelete = '0' and UserType=2 ";
        private const string Sql_GetUserArea = "select UserId,AreaId from tbl_UserArea where ";
        private const string Sql_GetUserIds = "select Id from tbl_CompanyUser ";
        private const string Sql_SetUserPermission = "update tbl_CompanyUser set PermissionList = @PermissionList,RoleID = @RoleID where Id = @Id and RoleID = @RoleID";
        private const string Sql_GetUserBasicInfo = "select * from tbl_CompanyUser where Id = @Id";
        private const string Sql_SetUserEnable = "update tbl_CompanyUser set IsEnable = @IsEnable where Id = @Id and IsDelete = '0'";
        private const string Sql_GetCompanyUser = "select * from tbl_CompanyUser where CompanyId = @CompanyId and IsDelete = '0' and UserType = 2";
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyUser()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);

        }
        #endregion

        #region ICompanyUser 成员
        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        public bool IsExistsEmail(string email, int userId)
        {
            var obj = dcDal.CompanyUser.FirstOrDefault(item => item.Id != userId && item.ContactEmail == email);
            return obj == null ? false : true;
        }
        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="CompanyId">当前公司编号</param>
        /// <returns></returns>
        public bool IsExists(int id, string userName, int CompanyId)
        {
            var obj = dcDal.CompanyUser.FirstOrDefault(item => item.UserName == userName && item.CompanyId == CompanyId && item.Id != id && item.IsDelete == "0");
            return obj == null ? false : true;
        }
        /// <summary>
        /// 真实删除用户信息
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Delete(params string[] userIdList)
        {
            string strUserId = string.Empty;
            foreach (string str in userIdList)
            {
                strUserId += str + ",";
            }
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(Sql_CompanyUser_Delete, strUserId.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Remove(params string[] userIdList)
        {
            string strUserId = string.Empty;
            foreach (string str in userIdList)
            {
                strUserId += str + ",";
            }
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(Sql_CompanyUser_Remove, strUserId.TrimEnd(',')));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            EyouSoft.Data.CompanyUser obj = new EyouSoft.Data.CompanyUser()
            {
                CompanyId = model.CompanyId,
                DepartName = string.IsNullOrEmpty(model.DepartName) ? "" : model.DepartName,
                DepartId = model.DepartId,
                IsAdmin = model.IsAdmin ? "1" : "0",
                IsEnable = model.IsEnable ? "1" : "0",
                PermissionList = string.IsNullOrEmpty(model.PermissionList) ? "" : model.PermissionList,
                RoleID = model.RoleID,
                UserName = model.UserName,
                SuperviseDepartId = model.SuperviseDepartId,
                SuperviseDepartName = model.SuperviseDepartName,
                IsDelete = model.IsDeleted ? "1" : "0"
            };
            if (model.PassWordInfo != null)
            {
                obj.MD5Password = model.PassWordInfo.MD5Password;
                obj.Password = model.PassWordInfo.NoEncryptPassword;
            }
            //用户联系人信息
            if (model.PersonInfo != null)
            {
                obj.IssueTime = System.DateTime.Now;
                obj.LastLoginTime = System.DateTime.Now;
                obj.ContactEmail = string.IsNullOrEmpty(model.PersonInfo.ContactEmail) ? "" : model.PersonInfo.ContactEmail;
                obj.ContactFax = string.IsNullOrEmpty(model.PersonInfo.ContactFax) ? "" : model.PersonInfo.ContactFax;
                obj.ContactMobile = string.IsNullOrEmpty(model.PersonInfo.ContactMobile) ? "" : model.PersonInfo.ContactMobile;
                obj.ContactName = string.IsNullOrEmpty(model.PersonInfo.ContactName) ? "" : model.PersonInfo.ContactName;
                obj.ContactSex = ((int)model.PersonInfo.ContactSex).ToString();
                obj.ContactTel = string.IsNullOrEmpty(model.PersonInfo.ContactTel) ? "" : model.PersonInfo.ContactTel;
                obj.JobName = string.IsNullOrEmpty(model.PersonInfo.JobName) ? "" : model.PersonInfo.JobName;
                obj.Msn = string.IsNullOrEmpty(model.PersonInfo.MSN) ? "" : model.PersonInfo.MSN;
                obj.PeopProfile = string.IsNullOrEmpty(model.PersonInfo.PeopProfile) ? "" : model.PersonInfo.PeopProfile;
                obj.Qq = string.IsNullOrEmpty(model.PersonInfo.QQ) ? "" : model.PersonInfo.QQ;
                obj.Remark = string.IsNullOrEmpty(model.PersonInfo.Remark) ? "" : model.PersonInfo.Remark;
                obj.UserType = (byte)model.PersonInfo.UserType;
            }
            dcDal.CompanyUser.InsertOnSubmit(obj);
            dcDal.SubmitChanges();
            if (model.UserAreaList != null && model.UserAreaList.Count > 0 && dcDal.ChangeConflicts.Count == 0)
            {
                System.Data.Linq.EntitySet<EyouSoft.Data.UserArea> AreaList = new System.Data.Linq.EntitySet<EyouSoft.Data.UserArea>();
                foreach (EyouSoft.Model.CompanyStructure.UserArea area in model.UserAreaList)
                {
                    EyouSoft.Data.UserArea _area = new EyouSoft.Data.UserArea();
                    _area.AreaId = area.AreaId;
                    _area.UserId = obj.Id;
                    AreaList.Add(_area);
                    _area = null;
                }
                obj.UserUserAreaList = AreaList;
                dcDal.SubmitChanges();
            }
            return dcDal.ChangeConflicts.Count == 0 ? true : false;
        }
        /// <summary>
        /// 修改用户基本信息[不更改密码]
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            EyouSoft.Data.CompanyUser obj = dcDal.CompanyUser.FirstOrDefault(item => item.Id == model.ID && item.CompanyId == model.CompanyId);
            if (obj != null)
            {
                #region 用户基本信息
                obj.DepartName = model.DepartName;
                obj.DepartId = model.DepartId;
                //obj.IsAdmin = model.IsAdmin ? "1" : "0";
                //obj.IsEnable = model.IsEnable ? "1" : "0";
                //obj.PermissionList = model.PermissionList;
                //obj.RoleID = model.RoleID;
                obj.SuperviseDepartId = model.SuperviseDepartId;
                obj.SuperviseDepartName = model.SuperviseDepartName;
                #endregion

                #region 用户联系人信息
                if (model.PersonInfo != null)
                {
                    obj.ContactEmail = model.PersonInfo.ContactEmail;
                    obj.ContactFax = model.PersonInfo.ContactFax;
                    obj.ContactMobile = model.PersonInfo.ContactMobile;
                    obj.ContactName = model.PersonInfo.ContactName;
                    obj.ContactSex = ((int)model.PersonInfo.ContactSex).ToString();
                    obj.ContactTel = model.PersonInfo.ContactTel;
                    obj.JobName = model.PersonInfo.JobName;
                    obj.Msn = model.PersonInfo.MSN;
                    obj.PeopProfile = model.PersonInfo.PeopProfile;
                    obj.Qq = model.PersonInfo.QQ;
                    obj.Remark = model.PersonInfo.Remark;
                    //obj.UserType = (byte)model.PersonInfo.UserType;
                }
                #endregion

                #region 用户线路区域信息
                if (model.UserAreaList != null && model.UserAreaList.Count > 0)
                {
                    System.Data.Linq.EntitySet<EyouSoft.Data.UserArea> AreaList = new System.Data.Linq.EntitySet<EyouSoft.Data.UserArea>();
                    foreach (EyouSoft.Model.CompanyStructure.UserArea area in model.UserAreaList)
                    {
                        EyouSoft.Data.UserArea _area = new EyouSoft.Data.UserArea();
                        _area.AreaId = area.AreaId;
                        _area.UserId = model.ID;
                        AreaList.Add(_area);
                        _area = null;
                    }
                    obj.UserUserAreaList = AreaList;
                }
                #endregion

                dcDal.SubmitChanges();
                return dcDal.ChangeConflicts.Count == 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// 更新组团用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateZuTuan(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            EyouSoft.Data.CompanyUser obj = dcDal.CompanyUser.FirstOrDefault(item => item.Id == model.ID && item.TourCompanyId == model.TourCompanyId);

            if (obj != null)
            {
                obj.UserName = model.UserName;

                if (model.PassWordInfo != null)
                {
                    obj.Password = model.PassWordInfo.NoEncryptPassword;
                    obj.MD5Password = model.PassWordInfo.MD5Password;
                }

                if (model.PersonInfo != null)
                {
                    obj.ContactName = model.PersonInfo.ContactName;
                    obj.ContactSex = ((int)model.PersonInfo.ContactSex).ToString();
                    obj.JobName = model.PersonInfo.JobName;
                    obj.ContactTel = model.PersonInfo.ContactTel;
                    obj.ContactFax = model.PersonInfo.ContactFax;
                    obj.ContactMobile = model.PersonInfo.ContactMobile;
                    obj.Qq = model.PersonInfo.QQ;
                    obj.ContactEmail = model.PersonInfo.ContactEmail;
                    obj.Remark = model.PersonInfo.Remark;
                }
                dcDal.SubmitChanges();
                return dcDal.ChangeConflicts.Count == 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="password">密码实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePassWord(int id, EyouSoft.Model.CompanyStructure.PassWord password)
        {
            EyouSoft.Data.CompanyUser obj = dcDal.CompanyUser.FirstOrDefault(item => item.Id == id);
            if (obj != null)
            {
                obj.MD5Password = password.MD5Password;
                obj.Password = password.NoEncryptPassword;
                dcDal.SubmitChanges();
                return dcDal.ChangeConflicts.Count == 0 ? true : false;
            }
            return false;
        }
        /// <summary>
        /// 根据用户编号获取用户信息实体
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns>用户信息实体</returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(int UserId)
        {
            EyouSoft.Data.CompanyUser obj = dcDal.CompanyUser.Where(item => item.Id == UserId).FirstOrDefault();

            if (obj != null)
            {
                dcDal.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);
                return this.GetUserInfo(obj);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据用户名及密码获取用户信息实体
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <returns>用户信息实体</returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(string UserName, string Pwd)
        {
            EyouSoft.Data.CompanyUser obj = dcDal.CompanyUser.FirstOrDefault(item => item.UserName == UserName && item.MD5Password == Pwd);
            if (obj != null)
            {
                return this.GetUserInfo(obj);
            }
            return null;
        }
        /// <summary>
        /// 获取指定公司的管理员账户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns>管理员用户信息实体</returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetAdminModel(int companyId)
        {
            var obj = dcDal.CompanyUser.FirstOrDefault(item => item.CompanyId == companyId && item.IsAdmin == "1" &&
                item.IsDelete == "0" && item.IsEnable == "1");
            if (obj != null)
            {
                return this.GetUserInfo(obj);
            }
            return null;
        }
        /// <summary>
        /// 获取指定公司的所有用户信息[不含管理员用户]
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount, int userId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> totals = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();

            string tableName = "tbl_CompanyUser";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,CompanyId,UserName,Password,MD5Password,UserType,ContactName,ContactSex,ContactTel,ContactFax,ContactMobile"
                            + ",ContactEmail,QQ,MSN,JobName,LastLoginIP,LastLoginTime,RoleID,PermissionList,PeopProfile,Remark,IsDelete,IsEnable"
                            + ",IsAdmin,IssueTime,DepartName,DepartId,SuperviseDepartId,SuperviseDepartName ");

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' AND UserType=2 and IsEnable = '1' ");//只获取专线端未删除的用户
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", companyId);
            if (userId != 0)
            {
                cmdQuery.AppendFormat(" and Id = {0}", userId);
            }

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();

                    #region 用户基本信息
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    //companyUserModel.DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    if (userId != 0)
                    {
                        //用户线路区域信息
                        companyUserModel.UserAreaList = GetUserAreaByUserIds(userId.ToString());
                    }

                    #region 联系人信息
                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), rdr.GetString(rdr.GetOrdinal("ContactSex")).ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetByte(rdr.GetOrdinal("UserType")).ToString())
                    };
                    #endregion

                    totals.Add(companyUserModel);
                }
            }

            return totals;
        }
        /// <summary>
        /// 设置用户启用状态
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <param name="isEnable">是否启用</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetEnable(int id, bool isEnable)
        {
            //var obj = dcDal.CompanyUser.FirstOrDefault(item => item.Id == id && item.IsDelete == "0");
            //if (obj != null)
            //{
            //    obj.IsEnable = isEnable ? "1" : "0";
            //    dcDal.SubmitChanges();
            //    return dcDal.ChangeConflicts.Count == 0 ? true : false;
            //}
            //return false;

            DbCommand cmd = this._db.GetSqlStringCommand(Sql_SetUserEnable);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);
            this._db.AddInParameter(cmd, "IsEnable", DbType.String, isEnable ? '1' : '0');

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <param name="obj">EyouSoft.Data.CompanyUser 用户对象</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(EyouSoft.Data.CompanyUser obj)
        {
            return new EyouSoft.Model.CompanyStructure.CompanyUser()
            {
                CompanyId = obj.CompanyId,
                DepartId = obj.DepartId,
                DepartName = obj.DepartName,
                ID = obj.Id,
                IsAdmin = obj.IsAdmin == "1" ? true : false,
                IsDeleted = obj.IsDelete == "1" ? true : false,
                IsEnable = obj.IsEnable == "1" ? true : false,
                IssueTime = obj.IssueTime,
                LastLoginIP = obj.LastLoginIP,
                LastLoginTime = obj.LastLoginTime,
                PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord(obj.Password),
                PermissionList = obj.PermissionList,
                PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                {
                    ContactEmail = obj.ContactEmail,
                    ContactFax = obj.ContactFax,
                    ContactMobile = obj.ContactMobile,
                    ContactName = obj.ContactName,
                    ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)int.Parse(obj.ContactSex),
                    ContactTel = obj.ContactTel,
                    JobName = obj.JobName,
                    MSN = obj.Msn,
                    PeopProfile = obj.PeopProfile,
                    QQ = obj.Qq,
                    Remark = obj.Remark,
                    UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)obj.UserType
                },
                RoleID = obj.RoleID,
                SuperviseDepartId = obj.SuperviseDepartId,
                SuperviseDepartName = obj.SuperviseDepartName,
                UserAreaList = (from area in obj.UserUserAreaList
                                select new EyouSoft.Model.CompanyStructure.UserArea()
                                {
                                    AreaId = area.AreaId,
                                    UserId = area.UserId
                                }).ToList(),
                UserName = obj.UserName
            };
        }
        #endregion

        // 新加 xuqh 2011-01-24
        #region ICompanyUser 成员

        /// <summary>
        /// 根据当前用户组织架构信息分页获取用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="CurrUserId">用户编号ID列</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> totals = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();

            string tableName = "View_DeptUser";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();
            //fields.Append(" (select UserId,AreaId from tbl_UserArea a where a.UserId = tbl_CompanyUser.[Id] for xml raw,root('root')) as ContactXML,");
            fields.Append(" Id,CompanyId,UserName,Password,MD5Password,UserType,ContactName,ContactSex,ContactTel,ContactFax,ContactMobile"
                            + ",ContactEmail,QQ,MSN,JobName,LastLoginIP,LastLoginTime,RoleID,PermissionList,PeopProfile,Remark,IsDelete,IsEnable"
                            + ",IsAdmin,IssueTime,DepartName,DepartId,SuperviseDepartId,SuperviseDepartName,DepartManger ");

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' AND UserType=2 ");//只获取专线端未删除的用户
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();

                    #region 用户基本信息
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartManger = rdr.GetInt32(rdr.GetOrdinal("DepartManger"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    //用户线路区域信息
                    companyUserModel.UserAreaList = GetUserAreaByUserIds(companyUserModel.ID.ToString()); ;

                    #region 联系人信息
                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), rdr.GetString(rdr.GetOrdinal("ContactSex")).ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetByte(rdr.GetOrdinal("UserType")).ToString())
                    };
                    #endregion

                    totals.Add(companyUserModel);
                }
            }

            return totals;
        }

        /// <summary>
        /// 根据联系人名称获取人员信息
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="recordCount">记录总数</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="contactName">联系人名称 模糊查询</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetListByContactName(int pageSize, int pageIndex, ref int recordCount, int deptId, string contactName, int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> totals = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();

            string tableName = "tbl_CompanyUser";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,CompanyId,UserName,Password,MD5Password,UserType,ContactName,ContactSex,ContactTel,ContactFax,ContactMobile"
                            + ",ContactEmail,QQ,MSN,JobName,LastLoginIP,LastLoginTime,RoleID,PermissionList,PeopProfile,Remark,IsDelete,IsEnable"
                            + ",IsAdmin,IssueTime,DepartName,DepartId,SuperviseDepartId,SuperviseDepartName ");
            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' AND UserType=2 and IsEnable = '1' ");//只获取专线端未删除的用户
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", companyId);
            if (deptId != -1)
                cmdQuery.AppendFormat(" and DepartId = {0} ", deptId);
            if (!string.IsNullOrEmpty(contactName))
                cmdQuery.AppendFormat(" and ContactName like '%{0}%'", contactName);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();

                    #region 用户基本信息
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    //用户线路区域信息
                    companyUserModel.UserAreaList = GetUserAreaByUserIds(companyUserModel.ID.ToString()); ;

                    #region 联系人信息
                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), rdr.GetString(rdr.GetOrdinal("ContactSex")).ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetByte(rdr.GetOrdinal("UserType")).ToString())
                    };
                    #endregion

                    totals.Add(companyUserModel);
                }
            }

            return totals;
        }

        /// <summary>
        /// 根据当前用户组织架构信息获取所有用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="CurrUserId">当前用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, string CurrUserId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> lsCompanyUser = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();
            EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = null;

            if (CurrUserId == null || CurrUserId.Length <= 0)
                return null;

            DbCommand cmd = _db.GetSqlStringCommand(Sql_GetList + "and Id in (" + CurrUserId + ");");

            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    #region 用户基本信息
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    #endregion

                    //用户密码信息
                    companyUserModel.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord()
                    {
                        NoEncryptPassword = rdr.GetString(rdr.GetOrdinal("Password"))
                    };

                    //用户线路区域信息
                    companyUserModel.UserAreaList = GetUserAreaByUserIds(CurrUserId);

                    #region 联系人信息
                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("ContactSex").ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("UserType").ToString())
                    };
                    #endregion

                    lsCompanyUser.Add(companyUserModel);
                }
            }

            return lsCompanyUser;
        }

        /// <summary>
        /// 获取该公司所有员工信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUser(int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyUser> lsCompanyUser = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();
            EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = null;

            DbCommand cmd = this._db.GetSqlStringCommand(Sql_GetCompanyUser);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    #region 用户基本信息
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();
                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr.GetString(rdr.GetOrdinal("DepartName"));
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));
                    #endregion

                    #region 联系人信息
                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("ContactSex").ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("UserType").ToString())
                    };
                    #endregion

                    lsCompanyUser.Add(companyUserModel);
                }
            }

            return lsCompanyUser;
        }

        /// <summary>
        /// 根据部门ID数组获取所有用户ID数组
        /// </summary>
        /// <param name="departIds">部门ID数组</param>
        /// <returns></returns>
        public int[] GetUserIdsByDepartIds(int[] departIds)
        {
            int[] userIds;
            ArrayList arrayUserId;
            if (departIds == null || departIds.Length <= 0)
                return new int[0];

            string strIds = string.Empty;
            foreach (int str in departIds)
            {
                strIds += "" + str.ToString().Trim() + ",";
            }
            strIds = strIds.Trim(',');

            DbCommand cmd = _db.GetSqlStringCommand(Sql_GetUserIds + " where DepartId in (" + strIds + ") and IsDelete = '0' and IsEnable = '1' and UserType = 2;");
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                arrayUserId = new ArrayList();

                while (rdr.Read())
                {
                    arrayUserId.Add(rdr.GetInt32(rdr.GetOrdinal("Id")));
                }
            }

            userIds = new int[arrayUserId.Count];
            for (int i = 0; i < arrayUserId.Count; i++)
            {
                userIds[i] = (int)arrayUserId[i];
            }

            return userIds;
        }

        /// <summary>
        /// 根据用户编号获取用户基本信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.ContactPersonInfo GetUserBasicInfo(int UserId)
        {
            EyouSoft.Model.CompanyStructure.ContactPersonInfo model = null;

            DbCommand cmd = _db.GetSqlStringCommand(Sql_GetUserBasicInfo);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, UserId);
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")))
                        model.ContactEmail = rdr.GetString(rdr.GetOrdinal("ContactEmail"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactFax")))
                        model.ContactFax = rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")))
                        model.ContactMobile = rdr.GetString(rdr.GetOrdinal("ContactMobile"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactName")))
                        model.ContactName = rdr.GetString(rdr.GetOrdinal("ContactName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactSex")))
                        model.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("ContactSex").ToString());
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ContactTel")))
                        model.ContactTel = rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    model.JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("MSN")))
                        model.MSN = rdr.GetString(rdr.GetOrdinal("MSN"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")))
                        model.PeopProfile = rdr.GetString(rdr.GetOrdinal("PeopProfile"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("QQ")))
                        model.QQ = rdr.GetString(rdr.GetOrdinal("QQ"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Remark")))
                        model.Remark = rdr.GetString(rdr.GetOrdinal("Remark"));
                    model.UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("UserType").ToString());
                }
            }

            return model;
        }
        /// <summary>
        /// 根据组团端用户编号分页获取相应的线路区域计调员
        /// </summary>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="TourUserId">组团用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.ContactPersonInfo> GetAreaJobsByTourUserID(int PageIndex, int PageSize, ref int RecordCount, int TourUserId)
        {
            IList<EyouSoft.Model.CompanyStructure.ContactPersonInfo> list = new List<EyouSoft.Model.CompanyStructure.ContactPersonInfo>();
            string tableName = "tbl_CompanyUser";
            string fields = "id,ContactName,ContactTel,ContactMobile,ContactFax,qq";
            string primaryKey = " id ";
            string strOrderBy = " id asc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' and 	IsEnable='1' and UserType=2 ");
            if (TourUserId > 0)
                strWhere.AppendFormat(" and id in(select distinct userid from tbl_UserArea where AreaId in(select areaid from tbl_UserArea where userid={0})) ", TourUserId);
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, strWhere.ToString(), strOrderBy))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.CompanyStructure.ContactPersonInfo model = new EyouSoft.Model.CompanyStructure.ContactPersonInfo();
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.ContactName = dr[dr.GetOrdinal("ContactName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.ContactTel = dr[dr.GetOrdinal("ContactTel")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactMobile")))
                        model.ContactMobile = dr[dr.GetOrdinal("ContactMobile")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactFax")))
                        model.ContactFax = dr[dr.GetOrdinal("ContactFax")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("QQ")))
                        model.QQ = dr[dr.GetOrdinal("QQ")].ToString();
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 获取用户线路区域信息
        /// </summary>
        /// <param name="CurrUserId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.UserArea> GetUserAreaByUserIds(string CurrUserId)
        {
            IList<EyouSoft.Model.CompanyStructure.UserArea> lsUserArea = new List<EyouSoft.Model.CompanyStructure.UserArea>();
            EyouSoft.Model.CompanyStructure.UserArea userAreaModel = null;

            if (!string.IsNullOrEmpty(CurrUserId))
            {
                //string strIds = string.Empty;
                //foreach (char str in CurrUserId)
                //{
                //    strIds += "'" + str.ToString().Trim() + "',";
                //}
                //strIds = strIds.Trim(',');

                DbCommand cmd1 = this._db.GetSqlStringCommand(Sql_GetUserArea + "UserId in (" + CurrUserId + ");");

                using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd1, this._db))
                {
                    if (rdr.Read())
                    {
                        userAreaModel = new EyouSoft.Model.CompanyStructure.UserArea();
                        userAreaModel.UserId = rdr.GetInt32(rdr.GetOrdinal("UserId"));
                        userAreaModel.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                        lsUserArea.Add(userAreaModel);
                    }
                }
            }

            return lsUserArea;
        }
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleId">角色编号</param>
        /// <param name="PermissionList">权限集合</param>
        /// <returns>是否成功</returns>
        public bool SetPermission(int UserId, int RoleId, string[] PermissionList)
        {
            string permissionStr = string.Empty;
            foreach (string str in PermissionList)
            {
                permissionStr += str + ",";
            }
            permissionStr = permissionStr.Trim(',');

            DbCommand cmd = this._db.GetStoredProcCommand("proc_SetUserPermission");
            this._db.AddInParameter(cmd, "Id", DbType.Int32, UserId);
            this._db.AddInParameter(cmd, "RoleId", DbType.Int32, RoleId);
            this._db.AddInParameter(cmd, "PermissionList", DbType.String, permissionStr);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 根据部门编号获取同级及下级部门所有用户信息集合
        /// </summary>
        /// <param name="departmentId">部门编号</param>
        /// <returns></returns>
        public IList<int> GetUsers(int departmentId)
        {
            IList<int> items = new List<int>();
            DbCommand cmd = this._db.GetStoredProcCommand("proc_CompanyUser_GetUserByDepart");
            this._db.AddInParameter(cmd, "DepartId", DbType.Int32, departmentId);

            using (IDataReader rdr = DbHelper.RunReaderProcedure(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr.GetInt32(rdr.GetOrdinal("Id")));
                }
            }

            return items;
        }

        /// <summary>
        /// 获取公司用户信息
        /// </summary>
        /// <param name="QueryModel">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUsers(Model.CompanyStructure.QueryCompanyUser QueryModel)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.CompanyStructure.CompanyUser> lsCompanyUser = new List<EyouSoft.Model.CompanyStructure.CompanyUser>();
            EyouSoft.Model.CompanyStructure.CompanyUser companyUserModel = null;
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from tbl_CompanyUser where IsDelete = '0' ");
            strSql.Append(this.GetSqlWhere(QueryModel));

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(dc, this._db))
            {
                while (rdr.Read())
                {
                    companyUserModel = new EyouSoft.Model.CompanyStructure.CompanyUser();

                    #region 用户基本信息

                    companyUserModel.ID = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    companyUserModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyUserModel.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    companyUserModel.DepartName = rdr["DepartName"].ToString();
                    companyUserModel.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    companyUserModel.IsDeleted = rdr.GetString(rdr.GetOrdinal("IsDelete")) == "1" ? true : false;
                    companyUserModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    companyUserModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyUserModel.LastLoginIP = rdr.IsDBNull(rdr.GetOrdinal("LastLoginIP")) ? "" : rdr["LastLoginIP"].ToString();
                    companyUserModel.LastLoginTime = rdr.GetDateTime(rdr.GetOrdinal("LastLoginTime"));
                    companyUserModel.PermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    companyUserModel.RoleID = rdr.GetInt32(rdr.GetOrdinal("RoleID"));
                    companyUserModel.SuperviseDepartId = rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId"));
                    companyUserModel.SuperviseDepartName = rdr.GetString(rdr.GetOrdinal("SuperviseDepartName"));

                    #endregion

                    #region 联系人信息

                    companyUserModel.PersonInfo = new EyouSoft.Model.CompanyStructure.ContactPersonInfo()
                    {
                        ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail")),
                        ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax")),
                        ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile")),
                        ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("ContactSex").ToString()),
                        ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel")),
                        JobName = rdr.IsDBNull(rdr.GetOrdinal("JobName")) ? "" : rdr.GetString(rdr.GetOrdinal("JobName")),
                        MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN")),
                        PeopProfile = rdr.IsDBNull(rdr.GetOrdinal("PeopProfile")) ? "" : rdr.GetString(rdr.GetOrdinal("PeopProfile")),
                        QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ")),
                        Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark")),
                        UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType), rdr.GetOrdinal("UserType").ToString())
                    };

                    #endregion

                    lsCompanyUser.Add(companyUserModel);
                }
            }

            return lsCompanyUser;

        }

        #region 私有方法

        /// <summary>
        /// 根据查询实体生成SqlWhere
        /// </summary>
        /// <param name="QueryModel">查询实体</param>
        /// <returns>SqlWhere</returns>
        private string GetSqlWhere(Model.CompanyStructure.QueryCompanyUser QueryModel)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return string.Empty;

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" and CompanyId = {0} ", QueryModel.CompanyId);
            if (QueryModel.ZuTuanCompanyId > 0)
                strWhere.AppendFormat(" and TourCompanyId = {0} ", QueryModel.ZuTuanCompanyId);
            if (QueryModel.UserType.HasValue)
                strWhere.AppendFormat(" and UserType = {0} ", (int)QueryModel.UserType);

            return strWhere.ToString();
        }

        #endregion
    }
}
