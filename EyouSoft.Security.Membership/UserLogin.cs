using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.SSOComponent;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Cache.Facade;
using EyouSoft.Cache;
using System.Web.Configuration;

namespace EyouSoft.SSOComponent
{
    #region 管理员登陆处理
    /// <summary>
    /// 管理员登陆处理
    /// </summary>
    /// 开发人：蒋胜蓝  开发时间：2010-6-1
    public class MaterLogin : EyouSoft.Toolkit.DAL.DALBase, IMasterLogin
    {
        #region static constants
        //static constants
        readonly string SQL_MASTERUSERLOGIN = "SELECT TOP(1) [Id],[Username],[Password],[MD5Password],[Realname],[Telephone],[Fax],[Mobile],[Permissions],[IsEnable],[IsAdmin],[CreateTime] FROM [tbl_Webmaster] AS A WHERE A.[Username]=@Username AND A.[MD5Password]=@PWD";
        #endregion

        #region IMasterLogin 成员
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">用户密码</param>
        /// <returns>管理员信息</returns>
        public MasterUserInfo MasterLogin(string UserName, string PWD)
        {
            string tmpPermissions = "";
            MasterUserInfo User = null;
            #region 用户查询
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL_MASTERUSERLOGIN);
            this.SystemStore.AddInParameter(dc, "Username", DbType.String, UserName);
            this.SystemStore.AddInParameter(dc, "PWD", DbType.String, PWD);
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.SystemStore))
            {
                if (rdr.Read())
                {
                    User = new MasterUserInfo();
                    User.UserId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    User.Username = rdr.GetString(rdr.GetOrdinal("Username"));
                    User.Fax = rdr["Fax"].ToString();
                    User.Mobile = rdr["Mobile"].ToString();
                    User.Realname = rdr["Realname"].ToString();
                    User.Telephone = rdr["Telephone"].ToString();
                    User.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    User.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    tmpPermissions = rdr.IsDBNull(rdr.GetOrdinal("Permissions")) ? "" : rdr.GetString(rdr.GetOrdinal("Permissions"));                  
                }
            }
            if (User != null)
            {
                if (!String.IsNullOrEmpty(tmpPermissions))
                {
                    string[] Permissions = tmpPermissions.Split(',');
                    User.Permissions = new int[Permissions.Length];
                    for (int i = 0; i < Permissions.Length; i++)
                    {
                        User.Permissions[i] = int.Parse(Permissions[i]);
                    }
                }
                else
                {
                    User.Permissions = new int[1] { -1 };
                }

                UpdateMasterInfo(User);
            }
            #endregion
            return User;
        }
        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <param name="UID">用户名</param>
        /// <returns></returns>
        public void MasterLogout(string UID)
        {
            EyouSoftSSOCache.Remove(EyouSoft.Cache.Tag.System.SystemUser + UID);
            return;
        }
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="UID">用户名</param>
        /// <returns>管理员信息</returns>
        public MasterUserInfo GetMasterInfo(string UID)
        {
            MasterUserInfo User = (MasterUserInfo)EyouSoftSSOCache.GetCache(EyouSoft.Cache.Tag.System.SystemUser + UID);
            return User;
        }
        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="User">用户信息</param>
        public void UpdateMasterInfo(MasterUserInfo User)
        {
            EyouSoftSSOCache.Add(EyouSoft.Cache.Tag.System.SystemUser + User.Username, User, DateTime.Now.AddHours(12));
            return;
        }
        #endregion
    }
    #endregion


    #region 用户登陆处理(处理本地资源方式)
    /// <summary>
    /// 用户登陆处理(处理本地资源方式)
    /// </summary>
    /// 开发人：蒋胜蓝  开发时间：2010-6-1
    public class UserLogin : EyouSoft.Toolkit.DAL.DALBase, IUserLogin
    {
        #region static constants
        //static constants
        readonly string SQL_USERLOGIN = "SELECT TOP 1 Id, CompanyId, UserName, UserType, ContactName, ContactSex, ContactTel, ContactFax, ContactMobile, ContactEmail,"
            + " QQ, MSN, IsEnable, IsAdmin, DepartId, DepartName, SuperviseDepartId, Password, MD5Password, PermissionList, TourCompanyId, "
            + "(SELECT TOP 1 SystemId FROM tbl_CompanyInfo WHERE ID=a.CompanyId) AS SystemId,"
            + "(SELECT TOP 1 CompanyName FROM tbl_CompanyInfo WHERE ID=a.CompanyId) AS CompanyName,"
            /*+ "(SELECT TOP 1 PermissionList FROM tbl_SysRoleManage WHERE ID=a.RoleID) AS PermissionList,"*/
            + "(SELECT AreaId FROM tbl_UserArea WHERE UserId=a.Id for xml path,root('root')) AreaId "
            + " FROM tbl_CompanyUser a WHERE IsDelete='0'";

        const string SQL_DOMAIN_SELECT = "SELECT top 1 A.SysId,A.Domain,(SELECT TOP 1 B.ID FROM tbl_CompanyInfo AS B WHERE B.SystemId=A.SysId) AS CompanyId,A.Url FROM tbl_SysDomain AS A WHERE A.Domain=@domain";
        readonly string SQL_TOURUSERCUSTOMER = "SELECT CustomerLev,name FROM tbl_Customer WHERE ID=@CompanyId";
        readonly string SQL_USERLOGIN_LOG = "UPDATE tbl_CompanyUser SET LastLoginIP=@LastLoginIP,LastLoginTime=GETDATE() WHERE [Id]=@UserId;";
        const string SQL_WRITE_LOG = "INSERT INTO tbl_SysLoginLog([ID],[CompanyId],[Operator],[DepatId],[LoginIp]) VALUES(" +
                                    "@ID,@CompanyId,@Operator,@DepatId,@EventIP)";

        readonly string CustomServicePwd = WebConfigurationManager.AppSettings["CustomerServicePwd"] != null ? WebConfigurationManager.AppSettings["CustomerServicePwd"] : "";
        private const string SQL_SELECT_GetGYSCompanyInfo = "SELECT [UnitName] FROM [tbl_CompanySupplier] WHERE Id=@CompanyId";
        #endregion

        #region IUserLogin 成员
        private UserInfo ReadUserInfo(DbCommand dc)
        {
            #region 用户查询
            string tmpPermissionList = "";
            string AreaXml = "";
            UserInfo User = null;

            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.SystemStore))
            {
                if (rdr.Read())
                {
                    User = new UserInfo();
                    User.UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)rdr.GetByte(rdr.GetOrdinal("UserType"));
                    User.UserName = rdr.GetString(rdr.GetOrdinal("UserName"));
                    User.CompanyName = rdr.IsDBNull(rdr.GetOrdinal("CompanyName")) ? "" : rdr.GetString(rdr.GetOrdinal("CompanyName"));
                    User.CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyID"));
                    //关联公司编号 根据用户类型区分
                    int tmpCompanyId=rdr.GetInt32(rdr.GetOrdinal("TourCompanyId"));
                    if (User.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户)
                    {
                        User.LocalAgencyCompanyInfo.CompanyId = tmpCompanyId;
                    }
                    else if (User.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
                    {
                        User.TourCompany.TourCompanyId = tmpCompanyId;
                    }
                    User.SysId = rdr.GetInt32(rdr.GetOrdinal("SystemId"));
                    User.ContactInfo.UserType = (EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType)rdr.GetByte(rdr.GetOrdinal("UserType"));
                    User.ContactInfo.ContactEmail = rdr.IsDBNull(rdr.GetOrdinal("ContactEmail")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactEmail"));
                    User.ContactInfo.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                    User.ContactInfo.ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile"));
                    User.ContactInfo.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName"));
                    User.ContactInfo.ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)int.Parse(rdr.GetString(rdr.GetOrdinal("ContactSex")));
                    User.ContactInfo.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                    User.ContactInfo.QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ"));                    
                    User.DepartId = rdr.IsDBNull(rdr.GetOrdinal("DepartId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                    User.DepartName = rdr.IsDBNull(rdr.GetOrdinal("DepartName")) ? "" : rdr.GetString(rdr.GetOrdinal("DepartName"));
                    User.JGDepartId = rdr.IsDBNull(rdr.GetOrdinal("SuperviseDepartId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("SuperviseDepartId")); 
                    User.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    User.IsAdmin = rdr.GetString(rdr.GetOrdinal("IsAdmin")) == "1" ? true : false;
                    User.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    User.ContactInfo.MSN = rdr.IsDBNull(rdr.GetOrdinal("MSN")) ? "" : rdr.GetString(rdr.GetOrdinal("MSN"));
                    User.PassWordInfo.SetEncryptPassWord(rdr.IsDBNull(rdr.GetOrdinal("Password")) == true ? "" : rdr.GetString(rdr.GetOrdinal("Password")), rdr.IsDBNull(rdr.GetOrdinal("MD5Password")) == true ? "" : rdr.GetString(rdr.GetOrdinal("MD5Password")));     
                    tmpPermissionList = rdr.IsDBNull(rdr.GetOrdinal("PermissionList")) ? "" : rdr.GetString(rdr.GetOrdinal("PermissionList"));
                    AreaXml = rdr.IsDBNull(rdr.GetOrdinal("AreaId")) ? "" : rdr.GetString(rdr.GetOrdinal("AreaId"));                    
                }
            }

            if (User != null)
            {
                #region 用户权限
                if (!String.IsNullOrEmpty(tmpPermissionList))
                {
                    string[] PermissionList = tmpPermissionList.Split(',');
                    User.PermissionList = new int[PermissionList.Length];
                    for (int i = 0; i < PermissionList.Length; i++)
                    {
                        User.PermissionList[i] = int.Parse(PermissionList[i]);
                    }
                }
                else
                {
                    User.PermissionList = new int[1] { -1 };
                }
                #endregion

                #region 用户线路区域
                if (!String.IsNullOrEmpty(AreaXml))
                {
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.LoadXml(AreaXml);
                    System.Xml.XmlNodeList NodeList = xmlDoc.GetElementsByTagName("AreaId");
                    User.Areas = new int[NodeList.Count];
                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        if (EyouSoft.Common.Function.StringValidate.IsInteger(NodeList[i].FirstChild.Value))
                        {
                            User.Areas[i] = int.Parse(NodeList[i].FirstChild.Value);
                        }
                    }
                }

                else
                {
                    User.Areas = new int[1] { -1 };
                }
                #endregion

                #region 组团用户
                if (User.ContactInfo.UserType==EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户
                    && User.TourCompany.TourCompanyId!=0)
                
                {
                    dc.Parameters.Clear();
                    dc = this.SystemStore.GetSqlStringCommand(SQL_TOURUSERCUSTOMER);
                    this.SystemStore.AddInParameter(dc, "CompanyId", DbType.Int32, User.TourCompany.TourCompanyId);
                    using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.SystemStore))
                    {
                        if (rdr.Read())
                        {
                            User.TourCompany.CustomerLevel = rdr.GetInt32(rdr.GetOrdinal("CustomerLev"));
                            User.TourCompany.CompanyName = rdr.GetString(rdr.GetOrdinal("name")); 
                        }
                    }
                }
                #endregion

                #region 地接用户
                if (User.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户 
                    && User.LocalAgencyCompanyInfo.CompanyId != 0)
                {
                    dc.Parameters.Clear();
                    dc = this.SystemStore.GetSqlStringCommand(SQL_SELECT_GetGYSCompanyInfo);
                    this.SystemStore.AddInParameter(dc, "CompanyId", DbType.Int32, User.LocalAgencyCompanyInfo.CompanyId);

                    using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.SystemStore))
                    {
                        if (rdr.Read())
                        {
                            User.LocalAgencyCompanyInfo.CompanyName = rdr["UnitName"].ToString();
                        }
                    }

                }
                #endregion

                #region 写登录日志

                dc.Parameters.Clear();
                string GetRemoteIP = EyouSoft.Toolkit.Utils.GetRemoteIP();
                string RequestUrl = EyouSoft.Toolkit.Utils.GetRequestUrl();

                dc = this.SystemStore.GetSqlStringCommand(SQL_USERLOGIN_LOG);
                this.SystemStore.AddInParameter(dc, "UserId", DbType.Int32, User.ID);
                this.SystemStore.AddInParameter(dc, "LastLoginIP", DbType.String, EyouSoft.Toolkit.Utils.GetRemoteIP());
                this.SystemStore.AddInParameter(dc, "CompanyId", DbType.Int32, User.CompanyID);
                DbHelper.ExecuteSql(dc, this.SystemStore);

                WriteLog(User.CompanyID, User.ID, User.DepartId, GetRemoteIP);

                #endregion                
            }

            return User;
            #endregion
        }
        /// <summary>
        /// 平台用户登录
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">用户密码</param>
        /// <param name="PwdType">密码类型</param>
        /// <returns>用户信息</returns> 
        private UserInfo UserLoginFunc(int CompanyId, string UserName, string PWD, PasswordType PwdType)
        {           
            #region 用户查询
            string strWhere = string.Empty;
            if (PwdType == PasswordType.MD5 && PWD == CustomServicePwd && !String.IsNullOrEmpty(CustomServicePwd))/*客服密码*/
            {
                strWhere = SQL_USERLOGIN + " AND CompanyId=@CompanyId AND UserName=@UID";
            }
            else if (PwdType == PasswordType.MD5)
            {
                strWhere = SQL_USERLOGIN + " AND CompanyId=@CompanyId AND UserName=@UID AND MD5Password=@PWD";
            }
            DbCommand dc = this.SystemStore.GetSqlStringCommand(strWhere);
            this.SystemStore.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);            
            this.SystemStore.AddInParameter(dc, "UID", DbType.String, UserName);
            this.SystemStore.AddInParameter(dc, "PWD", DbType.String, PWD);            
            #endregion
            return ReadUserInfo(dc);
        }
        /// <summary>
        /// 平台用户登录
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">用户密码</param>
        /// <param name="LoginTicket">登录凭据值</param>
        /// <returns>用户信息</returns>        
        public UserInfo UserLoginAct(int CompanyId, string UserName, string PWD, string LoginTicket)
        {
            UserInfo User = null;
            if (PWD == string.Empty)
            {
                User = GetUserInfo(CompanyId, UserName);
                if (User == null || User.LoginTicket != LoginTicket)
                {
                    User = null;
                }
            }
            else
            {
                User = UserLoginFunc(CompanyId, UserName, PWD, PasswordType.MD5);
                if (User != null)
                {
                    User.LoginTicket = LoginTicket;
                    if(User.IsEnable)
                        UpdateUserInfo(User);
                }
            }
            return User;
        }
        /// <summary>
        /// 平台用户登录
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">用户密码</param>
        /// <param name="LoginTicket">登录凭据值</param>
        /// <param name="PwdType">密码类型</param>
        /// <returns>用户信息</returns> 
        public UserInfo UserLoginAct(int CompanyId, string UserName, string PWD, string LoginTicket, PasswordType PwdType)
        {
            UserInfo User = UserLoginFunc(CompanyId, UserName, PWD, PwdType);
            if (User != null)
            {
                User.LoginTicket = LoginTicket;
                if(User.IsEnable)
                    UpdateUserInfo(User);
            }
            return User;
        }
        /// <summary>
        /// 日志写入
        /// </summary>
        /// <param name="model"></param>
        private void WriteLog(int CompanyId, int OperatorId, int DepatId, string EventIP)
        {
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL_WRITE_LOG);
            this.SystemStore.AddInParameter(dc, "ID", DbType.String, Guid.NewGuid().ToString());
            this.SystemStore.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this.SystemStore.AddInParameter(dc, "Operator", DbType.Int32, OperatorId);
            this.SystemStore.AddInParameter(dc, "DepatId", DbType.Int32, OperatorId);
            this.SystemStore.AddInParameter(dc, "EventIP", DbType.String, EventIP);
            DbHelper.ExecuteSql(dc, this.SystemStore);
        }
        /// <summary>
        /// 用户退出
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UID">用户编号</param>
        /// <returns>是否成功</returns>
        public bool UserLogout(int CompanyId, string UID)
        {
            //RemoveCache();
            string cacheKey = string.Format(EyouSoft.Cache.Tag.Company.CompanyUser, CompanyId.ToString(), UID.ToLower());
            EyouSoftSSOCache.Remove(cacheKey);
            return true;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UID">用户编号</param>
        /// <returns>用户信息</returns>
        public UserInfo GetUserInfo(int CompanyId, string UID)
        {
            //GetCache();
            string cacheKey = string.Format(EyouSoft.Cache.Tag.Company.CompanyUser, CompanyId.ToString(), UID.ToLower());
            UserInfo User = (UserInfo)EyouSoftSSOCache.GetCache(cacheKey);
            return User;
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="User">用户信息</param>
        public void UpdateUserInfo(UserInfo User)
        {
            //SetCache();
            string cacheKey = string.Format(EyouSoft.Cache.Tag.Company.CompanyUser, User.CompanyID.ToString(), User.UserName.ToLower());
            EyouSoftSSOCache.Remove(cacheKey);
            EyouSoftSSOCache.Add(cacheKey, User, DateTime.Now.AddHours(12));
            return;
        }
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.SystemDomain GetDomain(string domain)
        {
           
            EyouSoft.Model.SysStructure.SystemDomain model = (EyouSoft.Model.SysStructure.SystemDomain)
                EyouSoft.Cache.Facade.EyouSoftCache.GetCache(EyouSoft.Cache.Tag.System.SystemDomain + domain);
            if (model == null)
            {
                DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL_DOMAIN_SELECT);
                this.SystemStore.AddInParameter(dc, "domain", DbType.String, domain);
                using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(dc, this.SystemStore))
                {
                    if (rdr.Read())
                    {
                        model = new EyouSoft.Model.SysStructure.SystemDomain()
                        {
                            Domain = rdr["Domain"].ToString(),
                            SysId = rdr.GetInt32(rdr.GetOrdinal("SysId")),
                            CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                            Url=rdr["Url"].ToString()
                        };
                    }
                }
                if(model!=null)
                    EyouSoft.Cache.Facade.EyouSoftCache.Add(EyouSoft.Cache.Tag.System.SystemDomain + domain, model, DateTime.Now.AddHours(2));
            }
            return model;
        }
        #endregion
    }
    #endregion
}

