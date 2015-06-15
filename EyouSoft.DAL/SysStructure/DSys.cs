/*Author:汪奇志 2011-04-23*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;

namespace EyouSoft.DAL.SysStructure
{
    /// <summary>
    /// 管理后台系统管理数据访问类
    /// </summary>
    /// Author:汪奇志 2011-04-23
    public class DSys : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SysStructure.ISys
    {
        #region static constants
        //static constants
        /// <summary>
        /// 获取系统信息
        /// </summary>
        private const string SQL_SELECT_GetSysInfo = "SELECT [SysName],[Module],[Part],[Permission] FROM [tbl_Sys] WHERE [SysId]=@SysId";
        /// <summary>
        /// 根据系统编号获取公司编号
        /// </summary>
        private const string SQL_SELECT_GetCompanyIdBySysId = "SELECT [Id] FROM [tbl_CompanyInfo] WHERE [SystemId]=@SysId";
        /// <summary>
        /// 根据公司编号获取公司管理员编号
        /// </summary>
        private const string SQL_SELECT_GetAdminIdByCompanyId = "SELECT TOP(1) [Id] FROM [tbl_CompanyUser] WHERE [CompanyId]=@CompanyId AND [IsAdmin]='1'";
        /// <summary>
        /// 根据公司编号获取公司总部部门编号
        /// </summary>
        private const string SQL_SELECT_GetHeadOfficeIdByCompanyId = "SELECT [Id] FROM [tbl_CompanyDepartment] WHERE [CompanyId]=@CompanyId AND [PrevDepartId]=0";
        #endregion

        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DSys()
        {
            this._db = base.SystemStore;
        }
        #endregion      

        #region private members
        /// <summary>
        /// array to string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string ArrayToString(int[] array)
        {
            if (array == null || array.Length < 1) return string.Empty;

            StringBuilder s = new StringBuilder();
            s.Append(array[0].ToString());

            for (int i = 1; i < array.Length; i++)
            {
                s.AppendFormat(",{0}", array[i].ToString());
            }

            return s.ToString();
        }

        /// <summary>
        /// string to int array
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private int[] StringToIntArray(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;

            string[] tmp = s.Split(',');
            int[] array=new int[tmp.Length];

            for (int i = 0; i < tmp.Length; i++)
            {
                array[i] = Utils.GetInt(tmp[i], 0);
            }

            return array;
        }

        /// <summary>
        /// 创建域名信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateSysDomainsXML(IList<EyouSoft.Model.SysStructure.SystemDomain> items)
        {
            //XML:<ROOT><Info Domain="域名" URL="目标位置url" DomainType="域名类型" /></ROOT>
            if (items == null && items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Domain=\"{0}\" URL=\"{1}\" DomainType=\"{2}\" />", item.Domain
                    , item.Url
                    , (int)item.DomainType);
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建公司配置信息XMLDocument
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private string CreateCompanySettingsXML(EyouSoft.Model.CompanyStructure.CompanyFieldSetting setting)
        {
            //XML:<ROOT><Info Key="配置KEY" Value="配置VALUE" /></ROOT>
            if (setting == null) return string.Empty;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "AgencyFee", (int)setting.AgencyFeeInfo);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "BackTourDays", setting.BackTourReminderDays);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ComputeOrderType", (int)setting.ComputeOrderType);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ContractReminderDays", setting.ContractReminderDays);            
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "DisplayAfterMonth", setting.DisplayAfterMonth);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "DisplayBeforeMonth", setting.DisplayBeforeMonth);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "LeaveTourDays", setting.LeaveTourReminderDays);            
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "PriceComponent", (int)setting.PriceComponent);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ProfitStatTourPagePath", setting.ProfitStatTourPagePath);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ReservationTime", (int)setting.ReservationTime);           
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "TicketTravellerCheckedType", (int)setting.TicketTravellerCheckedType);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "IsRequiredTraveller", setting.IsRequiredTraveller ? "1" : "0");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "TeamNumberOfPeople", (int)setting.TeamNumberOfPeople);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "TicketOfficeFillTime", (int)setting.TicketOfficeFillTime);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "IsTicketOutRegisterPayment", setting.IsTicketOutRegisterPayment ? "1" : "0");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "HuiKuanLvSFBHWeiShenHe", setting.HuiKuanLvSFBHWeiShenHe ? "1" : "0");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "SiteTourDisplayType", (int)setting.SiteTourDisplayType);
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "SiteTemplateId", (int)setting.SiteTemplate);

            if (setting.PrintDocument != null && setting.PrintDocument.Count > 0)
            {
                foreach (var item in setting.PrintDocument)
                {
                    xmlDoc.AppendFormat("<Info Key=\"PrintDocument_{0}\" Value=\"{1}\" />", (int)item.PrintTemplateType, item.PrintTemplate);
                }
            }

            xmlDoc.Append("</ROOT>");
            

            return xmlDoc.ToString();
        }
        #endregion

        #region ISys 成员
        /// <summary>
        /// 创建子系统，返回1成功，其它失败
        /// </summary>
        /// <param name="sysInfo">EyouSoft.Model.SysStructure.MSysInfo</param>
        /// <returns></returns>
        public int CreateSys(EyouSoft.Model.SysStructure.MSysInfo sysInfo)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Sys_CreateSys");
            this._db.AddInParameter(cmd, "SysName", DbType.String, sysInfo.SystemName);
            this._db.AddInParameter(cmd, "SysDomainsXML", DbType.String, this.CreateSysDomainsXML(sysInfo.Domains));
            this._db.AddInParameter(cmd, "FirstPermission", DbType.String, this.ArrayToString(sysInfo.ModuleIds));
            this._db.AddInParameter(cmd, "SecondPermission", DbType.String, this.ArrayToString(sysInfo.PartIds));
            this._db.AddInParameter(cmd, "ThirdPermission", DbType.String, this.ArrayToString(sysInfo.PermissionIds));
            this._db.AddInParameter(cmd, "CompanyName", DbType.String, sysInfo.CompanyInfo.CompanyName);
            this._db.AddInParameter(cmd, "Realname", DbType.String, sysInfo.CompanyInfo.ContactName);
            this._db.AddInParameter(cmd, "Telephone", DbType.String, sysInfo.CompanyInfo.ContactTel);
            this._db.AddInParameter(cmd, "Mobile", DbType.String, sysInfo.CompanyInfo.ContactMobile);
            this._db.AddInParameter(cmd, "Fax", DbType.String, sysInfo.CompanyInfo.ContactFax);
            this._db.AddInParameter(cmd, "DepartmentName", DbType.String, sysInfo.DepartmentInfo.DepartName);
            this._db.AddInParameter(cmd, "Username", DbType.String, sysInfo.AdminInfo.UserName);
            this._db.AddInParameter(cmd, "Password", DbType.String, sysInfo.AdminInfo.PassWordInfo.NoEncryptPassword);
            this._db.AddInParameter(cmd, "MD5Password", DbType.String, sysInfo.AdminInfo.PassWordInfo.MD5Password);
            this._db.AddInParameter(cmd, "SettingsXML", DbType.String, this.CreateCompanySettingsXML(sysInfo.Setting));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "SysId", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "CompanyId", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            sysInfo.SystemId=Convert.ToInt32(this._db.GetParameterValue(cmd, "SysId"));
            sysInfo.CompanyInfo.Id = Convert.ToInt32(this._db.GetParameterValue(cmd, "CompanyId"));

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改系统信息
        /// 1、修改管理员密码
        /// 2、修改权限
        /// 3、修改系统配置
        /// 4、修改域名
        /// </summary>
        /// <param name="sysInfo">系统信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateSys(EyouSoft.Model.SysStructure.MSysInfo sysInfo)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Sys_UpdateSys");
            this._db.AddInParameter(cmd, "SysId", DbType.Int32, sysInfo.SystemId);
            this._db.AddInParameter(cmd, "SysDomainsXML", DbType.String, this.CreateSysDomainsXML(sysInfo.Domains));
            this._db.AddInParameter(cmd, "FirstPermission", DbType.String, this.ArrayToString(sysInfo.ModuleIds));
            this._db.AddInParameter(cmd, "SecondPermission", DbType.String, this.ArrayToString(sysInfo.PartIds));
            this._db.AddInParameter(cmd, "ThirdPermission", DbType.String, this.ArrayToString(sysInfo.PermissionIds));            
            this._db.AddInParameter(cmd, "Username", DbType.String, sysInfo.AdminInfo.UserName);
            this._db.AddInParameter(cmd, "Password", DbType.String, sysInfo.AdminInfo.PassWordInfo.NoEncryptPassword);
            this._db.AddInParameter(cmd, "MD5Password", DbType.String, string.IsNullOrEmpty(sysInfo.AdminInfo.PassWordInfo.NoEncryptPassword) ? "" : sysInfo.AdminInfo.PassWordInfo.MD5Password);
            this._db.AddInParameter(cmd, "SettingsXML", DbType.String, this.CreateCompanySettingsXML(sysInfo.Setting));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取系统信息，仅取WEBMASTER修改子系统时使用的数据
        /// </summary>
        /// <param name="SystemId">系统编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.MSysInfo GetSysInfo(int sysId)
        {
            EyouSoft.Model.SysStructure.MSysInfo sysInfo = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSysInfo);
            this._db.AddInParameter(cmd, "SysId", DbType.Int32, sysId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    sysInfo = new EyouSoft.Model.SysStructure.MSysInfo();
                    sysInfo.SystemId = sysId;
                    sysInfo.SystemName = rdr["SysName"].ToString();
                    sysInfo.ModuleIds = this.StringToIntArray(rdr["Module"].ToString());
                    sysInfo.PartIds = this.StringToIntArray(rdr["Part"].ToString());
                    sysInfo.PermissionIds = this.StringToIntArray(rdr["Permission"].ToString());
                }
            }

            return sysInfo;
        }

        /// <summary>
        /// 获取所有子系统信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.MLBSysInfo> GetSyss(EyouSoft.Model.SysStructure.MSysSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.SysStructure.MLBSysInfo> items = new List<EyouSoft.Model.SysStructure.MLBSysInfo>();
            StringBuilder cmdText = new StringBuilder();

            cmdText.Append(" SELECT ");
            cmdText.Append(" A.[SysId],A.[SysName],A.[CreateTime] ");
            cmdText.Append(" ,B.[Id] AS CompanyId,B.[CompanyName],B.[ContactName],B.[ContactTel],B.[ContactFax] ");
            cmdText.Append(" ,C.[Id] AS UserId,C.[UserName],C.[Password] ");
            cmdText.Append(" ,(SELECT * FROM [tbl_SysDomain] AS D WHERE D.[SysId]=A.[SysId] FOR XML RAW,ROOT('root')) AS Domains ");
            cmdText.Append(" FROM [tbl_Sys] AS A INNER JOIN [tbl_CompanyInfo] AS B ");
            cmdText.Append(" ON A.[SysId]=B.[SystemId] INNER JOIN [tbl_CompanyUser] AS C ");
            cmdText.Append(" ON B.[Id]=C.[CompanyId] AND C.[IsAdmin]='1' AND C.[IsDelete]='0' ");
            cmdText.Append(" ORDER BY A.[SysId] ");

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.SysStructure.MLBSysInfo item = new EyouSoft.Model.SysStructure.MLBSysInfo();

                    item.AdminPassword = rdr["Password"].ToString();
                    item.AdminUserId = rdr.GetInt32(rdr.GetOrdinal("UserId"));
                    item.AdminUsername = rdr["UserName"].ToString();
                    item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    item.CompanyName = rdr["CompanyName"].ToString();
                    item.Domains = new List<EyouSoft.Model.SysStructure.SystemDomain>();
                    item.Fax = rdr["ContactFax"].ToString();
                    item.Realname = rdr["ContactName"].ToString();
                    item.SysCreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                    item.SysId = rdr.GetInt32(rdr.GetOrdinal("SysId"));
                    item.SysName = rdr["SysName"].ToString();
                    item.Telephone = rdr["ContactTel"].ToString();

                    string xml = rdr["Domains"].ToString();

                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xRoot = XElement.Parse(xml);
                        var xRows = Utils.GetXElements(xRoot, "row");

                        foreach (var xRow in xRows)
                        {
                            item.Domains.Add(new EyouSoft.Model.SysStructure.SystemDomain()
                            {
                                CompanyId = item.CompanyId,
                                Domain = Utils.GetXAttributeValue(xRow, "Domain"),
                                SysId = item.SysId,//Utils.GetInt(Utils.GetXAttributeValue(xRow, "SysId")),
                                Url = Utils.GetXAttributeValue(xRow, "Url")
                            });
                        }
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 根据系统编号获取公司编号
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public int GetCompanyIdBySysId(int sysId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetCompanyIdBySysId);
            this._db.AddInParameter(cmd, "SysId", DbType.Int32, sysId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0);
                }
            }

            return 0;
        }
        /// <summary>
        /// 根据公司编号获取公司管理员编号
        /// </summary>
        /// <param name="compayId">公司编号</param>
        /// <returns></returns>
        public int GetAdminIdByCompanyId(int compayId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetAdminIdByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, compayId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 根据公司编号获取公司总部部门编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetHeadOfficeIdByCompanyId(int companyId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetHeadOfficeIdByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 更新管理员账号信息，密码为空时不修改密码
        /// </summary>
        /// <param name="webmasterInfo">EyouSoft.Model.SysStructure.MWebmasterInfo</param>
        /// <returns></returns>
        public bool UpdateWebmasterInfo(EyouSoft.Model.SysStructure.MWebmasterInfo webmasterInfo)
        {
            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" UPDATE [tbl_Webmaster] SET ");
            cmdText.Append(" [Username]=@Username ");
            this._db.AddInParameter(cmd, "Username", DbType.String, webmasterInfo.Username);

            if (webmasterInfo.Password != null && !string.IsNullOrEmpty(webmasterInfo.Password.NoEncryptPassword))
            {
                cmdText.Append(" ,[Password]=@Password ");
                cmdText.Append(" ,[MD5Password]=@MD5Password ");

                this._db.AddInParameter(cmd, "Password", DbType.String, webmasterInfo.Password.NoEncryptPassword);
                this._db.AddInParameter(cmd, "MD5Password", DbType.String, webmasterInfo.Password.MD5Password);
            }

            cmdText.Append(" WHERE [Id]=@UserId ");
            this._db.AddInParameter(cmd, "UserId", DbType.String, webmasterInfo.UserId);

            cmd.CommandText = cmdText.ToString();

            return DbHelper.ExecuteSql(cmd, this._db) == 1 ? true : false;
        }
        #endregion
    }
}
