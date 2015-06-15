using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 基础设置 数据类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteBasicConfig : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SiteStructure.ISiteBasicConfig
    {
        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public SiteBasicConfig()
        {
            DB = this.SystemStore;
        }
        /// <summary>
        /// 修改基础设置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public bool UpdateSiteInfo(EyouSoft.Model.SiteStructure.SiteBasicConfig SiteBasicConfig)
        {
            StringBuilder SQL = new StringBuilder();
            SQL.Append("IF(EXISTS(SELECT 1 FROM [tbl_Site] WHERE [CompanyId]=@CompanyId))");
            SQL.Append(" UPDATE [tbl_Site] SET [Title]=@Title,[Meta]=@Meta,[IsSetHome]=@IsSetHome,[IsSetFavorite]=@IsSetFavorite,[LogoPath]=@LogoPath,[Intr]=@Intr,[Copyright]=@Copyright,FriendLink=@FriendLink,[MainRoute]=@MainRoute,[CorporateCulture]=@CorporateCulture,[LianXiFangShi]=@LianXiFangShi WHERE [CompanyId]=@CompanyId");
            SQL.Append(" ELSE");
            SQL.Append(" INSERT INTO [tbl_Site]([CompanyId],[Title],[Meta],[IsSetHome],[IsSetFavorite],[LogoPath],[Intr],[Copyright],[FriendLink],[MainRoute],[CorporateCulture],[LianXiFangShi]) VALUES(@CompanyId,@Title,@Meta,@IsSetHome,@IsSetFavorite,@LogoPath,@Intr,@Copyright,@FriendLink,@MainRoute,@CorporateCulture,@LianXiFangShi)");
            DbCommand dc = this.DB.GetSqlStringCommand(SQL.ToString());
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, SiteBasicConfig.CompanyId);
            this.DB.AddInParameter(dc, "Title", DbType.String, SiteBasicConfig.SiteTitle);
            this.DB.AddInParameter(dc, "Meta", DbType.String, SiteBasicConfig.SiteMeta);
            this.DB.AddInParameter(dc, "IsSetHome", DbType.AnsiStringFixedLength, SiteBasicConfig.IsSetHome ? "1" : "0");
            this.DB.AddInParameter(dc, "IsSetFavorite", DbType.AnsiStringFixedLength, SiteBasicConfig.IsSetFavorite ? "1" : "0");
            this.DB.AddInParameter(dc, "LogoPath", DbType.String, SiteBasicConfig.LogoPath);
            this.DB.AddInParameter(dc, "Intr", DbType.String, SiteBasicConfig.Introduction);
            this.DB.AddInParameter(dc, "Copyright", DbType.String, SiteBasicConfig.Copyright);
            this.DB.AddInParameter(dc, "FriendLink", DbType.String, SiteBasicConfig.SiteIntro);
            this.DB.AddInParameter(dc, "MainRoute", DbType.String, SiteBasicConfig.MainRoute);
            this.DB.AddInParameter(dc, "CorporateCulture", DbType.String, SiteBasicConfig.CorporateCulture);
            DB.AddInParameter(dc, "LianXiFangShi", DbType.String, SiteBasicConfig.LianXiFangShi);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 获得公司基础设置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteBasicConfig GetSiteBasicConfig(int CompanyId)
        {
            EyouSoft.Model.SiteStructure.SiteBasicConfig SiteBasicConfig = null;
            DbCommand dc = this.DB.GetSqlStringCommand(String.Format("SELECT * FROM [tbl_Site] WHERE [CompanyId]={0}", CompanyId));
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this.DB))
            {
                while (rdr.Read())
                {
                    SiteBasicConfig = new EyouSoft.Model.SiteStructure.SiteBasicConfig()
                    {
                        CompanyId = rdr.IsDBNull(rdr.GetOrdinal("CompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        SiteTitle = rdr["Title"].ToString(),
                        SiteMeta = rdr["Meta"].ToString(),
                        IsSetFavorite = rdr["IsSetFavorite"].ToString() == "1" ? true : false,
                        IsSetHome = rdr["IsSetHome"].ToString() == "1" ? true : false,
                        Introduction = rdr["Intr"].ToString(),
                        LogoPath = rdr["LogoPath"].ToString(),
                        Copyright = rdr["Copyright"].ToString(),
                        SiteIntro = rdr["FriendLink"].ToString(),
                        MainRoute = rdr["MainRoute"].ToString(),
                        CorporateCulture = rdr["CorporateCulture"].ToString(),
                        LianXiFangShi = rdr["LianXiFangShi"].ToString()
                    };
                }
            }
            return SiteBasicConfig;
        }
    }
}
