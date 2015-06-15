using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司系统配置DAL
    /// xuqh 2011-01-23
    /// </summary>
    public class CompanySetting : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanySetting
    {
        #region static constants
        private const string SQL_GetValue = "select FieldValue from tbl_CompanySetting where Id = @Id and FieldName = @FieldName";
        private const string SQL_GetCompanySetting = "select Id,FieldName,FieldValue from tbl_CompanySetting where Id = @Id";
        private const string SQL_GetSetting = "select Id,FieldName,FieldValue from tbl_CompanySetting where Id=@CompanyId";
        private const string SQL_SetSetting = "delete tbl_CompanySetting where Id=@CompanyId and FieldName=@FieldName; insert into tbl_CompanySetting(id,FieldName,FieldValue) values(@CompanyId,@FieldName,@FieldValue);";
        private const string SQL_BcthSetSeting = "if not exists(select 1 from tbl_CompanySetting where id={0} and fieldname='{1}') begin 	insert into tbl_CompanySetting(id,fieldname,fieldvalue) values({0},'{1}','{2}') end else begin update tbl_CompanySetting set fieldvalue='{2}' where id={0} and fieldname='{1}' end ;";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public CompanySetting()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICompanySetting 成员

        /// <summary>
        /// 设置系统配置信息
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns>true：成功 false:失败</returns>
        public bool SetCompanySetting(EyouSoft.Model.CompanyStructure.CompanyFieldSetting model)
        {
            StringBuilder strSql = new StringBuilder();
            if (model.ContractReminderDays>0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "ContractReminderDays", model.ContractReminderDays);
            if (model.BackTourReminderDays > 0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "BackTourReminderDays", model.BackTourReminderDays);            
            if (model.DisplayAfterMonth > 0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "DisplayAfterMonth", model.DisplayAfterMonth);
            if (model.DisplayBeforeMonth > 0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "DisplayBeforeMonth", model.DisplayBeforeMonth);
            if (model.LeaveTourReminderDays > 0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "LeaveTourReminderDays", model.LeaveTourReminderDays);
            if (model.ReservationTime > 0)
                strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "ReservationTime", model.ReservationTime);

            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "PriceComponent", (int)model.PriceComponent);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "CompanyLogo", model.CompanyLogo);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "DepartStamp", model.CompanyPrintFile.DepartStamp);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "PageFootFile", model.CompanyPrintFile.PageFootFile);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "PageHeadFile", model.CompanyPrintFile.PageHeadFile);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "TemplateFile", model.CompanyPrintFile.TemplateFile);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "ReceiptRemindType", (int)model.ReceiptRemindType);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "TanChuangTiXingInterval", model.TanChuangTiXingInterval);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "SongTuanRenId", model.SongTuanRenId);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "SongTuanRenName", model.SongTuanRenName);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "JiHeDiDian", model.JiHeDiDian);
            strSql.AppendFormat(SQL_BcthSetSeting, model.CompanyId, "JiHeBiaoZhi", model.JiHeBiaoZhi);

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取指定公司的配置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="FileKey"></param>
        /// <returns></returns>
        public string GetValue(int CompanyId, string FileKey)
        {
            string fieldValue = string.Empty;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetValue);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, CompanyId);
            this._db.AddInParameter(cmd, "FieldName", DbType.String, FileKey);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    fieldValue = rdr.GetString(rdr.GetOrdinal("FieldValue"));
                }
            }

            return fieldValue;
        }
        /// <summary>
        /// 设置指定公司的配置信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="FieldKey">配置key</param>
        /// <param name="FieldValue">配置value</param>
        /// <returns></returns>
        public bool SetValue(int CompanyId, string FieldKey, string FieldValue)
        {
            DbCommand dc = this._db.GetSqlStringCommand(SQL_SetSetting);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "FieldName", DbType.String, FieldKey);
            this._db.AddInParameter(dc, "FieldValue", DbType.String, FieldValue);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取指定公司的系统配置信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyFieldSetting GetSetting(int CompanyId)
        {
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting model = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting();
            IList<EyouSoft.Model.CompanyStructure.PrintDocument> PrintDocumentList = new List<EyouSoft.Model.CompanyStructure.PrintDocument>();
            EyouSoft.Model.CompanyStructure.CompanyPrintTemplate CompanyPrintFile = new EyouSoft.Model.CompanyStructure.CompanyPrintTemplate();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetSetting);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);
            using (IDataReader dr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {                    
                    if (!dr.IsDBNull(dr.GetOrdinal("FieldName")))
                    { 
                        string FieldName=dr[dr.GetOrdinal("FieldName")].ToString().Trim();
                        if (FieldName.Contains("PrintDocument_"))
                        {
                            EyouSoft.Model.CompanyStructure.PrintDocument PrintDocument = new EyouSoft.Model.CompanyStructure.PrintDocument();
                            PrintDocument.PrintTemplateType = (EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType)int.Parse(FieldName.Split('_')[1]);
                            PrintDocument.PrintTemplate = dr[dr.GetOrdinal("FieldValue")].ToString();
                            PrintDocumentList.Add(PrintDocument);
                            PrintDocument = null;
                        }
                        else
                        {
                            switch (FieldName)
                            {
                                case "ContractReminderDays":
                                    model.ContractReminderDays = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "DisplayAfterMonth":
                                    model.DisplayAfterMonth = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "DisplayBeforeMonth":
                                    model.DisplayBeforeMonth = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "ReservationTime":
                                    model.ReservationTime = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "PriceComponent":
                                    model.PriceComponent = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : (EyouSoft.Model.EnumType.CompanyStructure.PriceComponent)int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "LeaveTourDays":
                                    model.LeaveTourReminderDays = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "BackTourDays":
                                    model.BackTourReminderDays = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? 0 : int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "CompanyLogo":
                                    model.CompanyLogo = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "DepartStamp":
                                    CompanyPrintFile.DepartStamp = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "PageFootFile":
                                    CompanyPrintFile.PageFootFile = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "PageHeadFile":
                                    CompanyPrintFile.PageHeadFile = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "TemplateFile":
                                    CompanyPrintFile.TemplateFile = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "AgencyFee":
                                    model.AgencyFeeInfo = (EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType)int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "ProfitStatTourPagePath":
                                    model.ProfitStatTourPagePath = dr.IsDBNull(dr.GetOrdinal("FieldValue")) ? string.Empty : dr[dr.GetOrdinal("FieldValue")].ToString();
                                    break;
                                case "ComputeOrderType":
                                    model.ComputeOrderType = (EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType)int.Parse(dr[dr.GetOrdinal("FieldValue")].ToString());
                                    break;
                                case "TicketTravellerCheckedType":
                                    model.TicketTravellerCheckedType = (EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType)int.Parse(dr["FieldValue"].ToString());
                                    break;
                                case "ReceiptRemindType":
                                    model.ReceiptRemindType = (EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType)int.Parse(dr["FieldValue"].ToString());
                                    break;
                                case "IsRequiredTraveller":
                                    model.IsRequiredTraveller = this.GetBoolean(dr["FieldValue"].ToString());
                                    break;
                                case "TeamNumberOfPeople":
                                    model.TeamNumberOfPeople = (EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople)EyouSoft.Toolkit.Utils.GetInt(dr["FieldValue"].ToString());
                                    break;
                                case "TicketOfficeFillTime":
                                    model.TicketOfficeFillTime = (EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime)EyouSoft.Toolkit.Utils.GetInt(dr["FieldValue"].ToString());
                                    break;
                                case "IsTicketOutRegisterPayment":
                                    model.IsTicketOutRegisterPayment = this.GetBoolean(dr["FieldValue"].ToString());
                                    break;
                                case "HuiKuanLvSFBHWeiShenHe":
                                    model.HuiKuanLvSFBHWeiShenHe = GetBoolean(dr["FieldValue"].ToString());
                                    break;
                                case "SiteTourDisplayType":
                                    model.SiteTourDisplayType = Utils.GetEnumValue<EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType>(dr["FieldValue"].ToString(), EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团);
                                    break;
                                case "SiteTemplateId":
                                    model.SiteTemplate = Utils.GetEnumValue<EyouSoft.Model.EnumType.SysStructure.SiteTemplate>(dr["FieldValue"].ToString(), EyouSoft.Model.EnumType.SysStructure.SiteTemplate.None);
                                    break;
                                case "TanChuangTiXingInterval":
                                    model.TanChuangTiXingInterval = Utils.GetInt(dr["FieldValue"].ToString());
                                    break;
                                case "SongTuanRenId":
                                    model.SongTuanRenId = dr["FieldValue"].ToString();
                                    break;
                                case "SongTuanRenName":
                                    model.SongTuanRenName = dr["FieldValue"].ToString();
                                    break;
                                case "JiHeDiDian":
                                    model.JiHeDiDian = dr["FieldValue"].ToString();
                                    break;
                                case "JiHeBiaoZhi":
                                    model.JiHeBiaoZhi = dr["FieldValue"].ToString();
                                    break;
                            }
                        }
                    }
                }
            }
            model.PrintDocument = PrintDocumentList;
            model.CompanyPrintFile = CompanyPrintFile;
            return model;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建系统配置的XML
        /// </summary>
        /// <param name="model">系统配置实体</param>
        /// <returns></returns>
        private string CreateSettingXml(EyouSoft.Model.CompanyStructure.CompanyFieldSetting model)
        {
            if (model == null)
                return string.Empty;
            StringBuilder strSettingXml = new StringBuilder();
            strSettingXml.Append("<ROOT>");
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"ContractReminderDays\" FieldValue=\"{0}\" />", model.ContractReminderDays);
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"DisplayAfterMonth\" FieldValue=\"{0}\" />", model.DisplayAfterMonth);
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"DisplayBeforeMonth\" FieldValue=\"{0}\" />", model.DisplayBeforeMonth);
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"ReservationTime\" FieldValue=\"{0}\" />", model.ReservationTime);
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"PriceComponent\" FieldValue=\"{0}\" />", (int)model.PriceComponent);
            strSettingXml.AppendFormat("<SettingInfo FieldName=\"CompanyLogo\" FieldValue=\"{0}\" />", model.CompanyLogo);
            if (model.PrintDocument != null && model.PrintDocument.Count > 0)
            {
                foreach (var item in model.PrintDocument)
                {
                    strSettingXml.AppendFormat("<SettingInfo FieldName=\"PrintDocument_{0}\" FieldValue=\"{1}\" />", (int)item.PrintTemplateType, item.PrintTemplate);
                }
            }
            strSettingXml.Append("</ROOT>");
            return strSettingXml.ToString();
        }
        #endregion
    }
}
