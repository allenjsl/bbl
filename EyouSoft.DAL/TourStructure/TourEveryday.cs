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

namespace EyouSoft.DAL.TourStructure
{
    /// <summary>
    /// 散客天天发计划数据访问类
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class TourEveryday : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITourEveryday
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public TourEveryday()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        private const string DEFAULT_XML_DOC = "<ROOT></ROOT>";
        private const string SQL_UPDATE_DeleteTourEverydayInfo = "UPDATE [tbl_TourEveryday] SET [IsDelete]=@V WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayInfo = "SELECT * FROM [tbl_TourEveryday] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayAttachs = "SELECT * FROM [tbl_TourEverydayAttach] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayPriceStandards = "SELECT A.*,B.[LevType],B.[CustomStandName] AS LevName,C.[PriceStandName] As StandardName FROM [tbl_TourEverydayPrice] AS A LEFT OUTER JOIN [tbl_CompanyCustomStand] AS B ON A.[LevelId]=B.[Id] LEFT OUTER JOIN [tbl_CompanyPriceStand] AS C ON A.[Standard]=C.[Id]  WHERE A.[TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayQuickPrivateInfo = "SELECT * FROM [tbl_TourEverydayQuickPlan] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayNormalPlans = "SELECT * FROM [tbl_TourEverydayPlan] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayNormalServices = "SELECT * FROM [tbl_TourEverydayService] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourEverydayApplyInfo = "SELECT * FROM [tbl_TourEverydayApply] WHERE [ApplyId]=@ApplyId;SELECT * FROM [tbl_TourEverydayApplyTraveller] WHERE [ApplyId]=@ApplyId ORDER BY [CreateTime] ASC;";

        #endregion

        #region private members
        #region create xml
        /// <summary>
        /// 创建计划附件信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateAttachsXML(IList<EyouSoft.Model.TourStructure.TourAttachInfo> items)
        {
            //<ROOT><Info FilePath="" Name="" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info FilePath=\"{0}\" Name=\"{1}\" />", item.FilePath, item.Name);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建计划报价信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreatePriceStandardsXML(IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> items)
        {
            //XML:<ROOT><Info AdultPrice="MONEY成人价" ChildrenPrice="MONEY儿童价" StandId="报价标准编号" LevelId="客户等级编号" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                foreach (var tmp in item.CustomerLevels)
                {
                    xmlDoc.AppendFormat("<Info AdultPrice=\"{0}\" ChildrenPrice=\"{1}\" StandardId=\"{2}\" LevelId=\"{3}\" />", tmp.AdultPrice
                        , tmp.ChildrenPrice
                        , item.StandardId
                        , tmp.LevelId);
                }
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建快速发布计划专有信息XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateQuickPrivateXML(EyouSoft.Model.TourStructure.TourQuickPrivateInfo info)
        {
            //XML:<ROOT><Info Plan="团队行程" Service="服务标准" Remark="备注" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Plan=\"{0}\" Service=\"{1}\" Remark=\"{2}\" />", Utils.ReplaceXmlSpecialCharacter(info.QuickPlan)
                , Utils.ReplaceXmlSpecialCharacter(info.Service)
                , Utils.ReplaceXmlSpecialCharacter(info.Remark));
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建标准发布专有信息(不含项目等)XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateNormalPrivateXML(EyouSoft.Model.TourStructure.TourTeamNormalPrivateInfo info)
        {
            //XML:<ROOT><Info Key="键" Value="值" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "BuHanXiangMu", Utils.ReplaceXmlSpecialCharacter(info.BuHanXiangMu));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ErTongAnPai", Utils.ReplaceXmlSpecialCharacter(info.ErTongAnPai));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "GouWuAnPai", Utils.ReplaceXmlSpecialCharacter(info.GouWuAnPai));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "NeiBuXingXi", Utils.ReplaceXmlSpecialCharacter(info.NeiBuXingXi));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "WenXinTiXing", Utils.ReplaceXmlSpecialCharacter(info.WenXinTiXing));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ZhuYiShiXiang", Utils.ReplaceXmlSpecialCharacter(info.ZhuYiShiXiang));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ZiFeiXIangMu", Utils.ReplaceXmlSpecialCharacter(info.ZiFeiXIangMu));
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建标准发布计划包含项目信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateNormalServiceXML(IList<EyouSoft.Model.TourStructure.TourServiceInfo> items)
        {
            //XML:<ROOT><Info Type="INT项目类型" Service="接待标准" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Type=\"{0}\" Service=\"{1}\" />", (int)item.ServiceType
                   , Utils.ReplaceXmlSpecialCharacter(item.Service));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建标准发布计划行程信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateNormalPlansXML(IList<EyouSoft.Model.TourStructure.TourPlanInfo> items)
        {
            //XML:<ROOT><Info Interval="区间" Vehicle="交通工具" Hotel="住宿" Dinner="用餐" Plan="行程内容" FilePath="附件路径" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Interval=\"{0}\" Vehicle=\"{1}\" Hotel=\"{2}\" Dinner=\"{3}\" Plan=\"{4}\" FilePath=\"{5}\" />", Utils.ReplaceXmlSpecialCharacter(item.Interval)
                    , Utils.ReplaceXmlSpecialCharacter(item.Vehicle)
                    , Utils.ReplaceXmlSpecialCharacter(item.Hotel)
                    , Utils.ReplaceXmlSpecialCharacter(item.Dinner)
                    , Utils.ReplaceXmlSpecialCharacter(item.Plan)
                    , item.FilePath);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建散客天天发计划申请游客信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateTourEverydayApplyTravellersXML(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items)
        {
            //XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" /></ROOT>
            if (items == null && items.Count < 1) return DEFAULT_XML_DOC;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.ID)) item.ID = Guid.NewGuid().ToString();

                xmlDoc.AppendFormat("<Info TravellerId=\"{0}\" TravellerName=\"{1}\" CertificateType=\"{2}\" CertificateCode=\"{3}\" Gender=\"{4}\" TravellerType=\"{5}\" Telephone=\"{6}\" />", item.ID
                    , Utils.ReplaceXmlSpecialCharacter(item.VisitorName)
                    , (int)item.CradType
                    , Utils.ReplaceXmlSpecialCharacter(item.CradNumber)
                    , (int)item.Sex
                    , (int)item.VisitorType
                    , Utils.ReplaceXmlSpecialCharacter(item.ContactTel));
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }
        #endregion

        /// <summary>
        /// 获取天天发计划附件信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourAttachInfo> GetTourEverydayAttachs(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourAttachInfo> items = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayAttachs);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourAttachInfo()
                    {
                        FilePath = rdr["FilePath"].ToString(),
                        Name = rdr["Name"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取天天发快速发布计划专有信息业务实体
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourQuickPrivateInfo GetTourEverydayQuickPrivateInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourQuickPrivateInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayQuickPrivateInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo()
                    {
                        QuickPlan = rdr["Plan"].ToString(),
                        Remark = rdr["Remark"].ToString(),
                        Service = rdr["Service"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取天天发标准发布计划专有信息业务实体
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourNormalPrivateInfo GetTourNormalPrivateInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourNormalPrivateInfo info = null;
            IList<EyouSoft.Model.TourStructure.TourServiceInfo> services = null;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayNormalServices);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                info = new EyouSoft.Model.TourStructure.TourNormalPrivateInfo();
                services = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();

                while (rdr.Read())
                {
                    switch (rdr.GetString(rdr.GetOrdinal("Key")))
                    {
                        case "DiJie":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.地接
                            });
                            break;
                        case "JiuDian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.酒店
                            });
                            break;
                        case "YongCan":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.用餐
                            });
                            break;
                        case "XiaoJiaoTong":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.小交通
                            });
                            break;
                        case "DaJiaoTong":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通
                            });
                            break;
                        case "JingDian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.景点
                            });
                            break;
                        case "GouWu":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.购物
                            });
                            break;
                        case "BaoXian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.保险
                            });
                            break;
                        case "QiTa":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.其它
                            });
                            break;
                        case "DaoFu":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.导服
                            });
                            break;
                        case "BuHanXiangMu":
                            info.BuHanXiangMu = rdr["Value"].ToString();
                            break;
                        case "GouWuAnPai":
                            info.GouWuAnPai = rdr["Value"].ToString();
                            break;
                        case "ErTongAnPai":
                            info.ErTongAnPai = rdr["Value"].ToString();
                            break;
                        case "ZiFeiXIangMu":
                            info.ZiFeiXIangMu = rdr["Value"].ToString();
                            break;
                        case "ZhuYiShiXiang":
                            info.ZhuYiShiXiang = rdr["Value"].ToString();
                            break;
                        case "WenXinTiXing":
                            info.WenXinTiXing = rdr["Value"].ToString();
                            break;
                        case "NeiBuXingXi":
                            info.NeiBuXingXi = rdr["Value"].ToString();
                            break;
                    }
                }
            }

            if (info != null)
            {
                info.Services = services;
                info.Plans = this.GetTourEverydayNormalPlans(tourId);
            }

            return info;
        }

        /// <summary>
        /// 获取天天发标准发布计划行程信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourPlanInfo> GetTourEverydayNormalPlans(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourPlanInfo> items = new List<EyouSoft.Model.TourStructure.TourPlanInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayNormalPlans);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourPlanInfo()
                    {
                        Dinner = rdr["Dinner"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        Hotel = rdr["Hotel"].ToString(),
                        Interval = rdr["Interval"].ToString(),
                        Plan = rdr["Plan"].ToString(),
                        Vehicle = rdr["Vehicle"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 解析天天发计划负责人信息
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="info"></param>
        private void ParseTourEverydayOperatorByXml(string xml, EyouSoft.Model.TourStructure.LBTourEverydayInfo info)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XElement xRoot = XElement.Parse(xml);
                var xRow = Utils.GetXElement(xRoot, "row");
                info.ContacterEmail = Utils.GetXAttributeValue(xRow, "ContactEmail");
                info.ContacterFax = Utils.GetXAttributeValue(xRow, "ContactFax");
                info.ContacterMobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                info.ContacterMSN = Utils.GetXAttributeValue(xRow, "MSN");
                info.ContacterName = Utils.GetXAttributeValue(xRow, "ContactName");
                info.ContacterQQ = Utils.GetXAttributeValue(xRow, "QQ");
                info.ContacterTelephone = Utils.GetXAttributeValue(xRow, "ContactTel");
            }
        }

        /// <summary>
        /// 解析天天发计划附件信息
        /// </summary>
        /// <param name="xml"></param>
        private IList<EyouSoft.Model.TourStructure.TourAttachInfo> ParseTourEverydayAttachsByXml(string xml)
        {
            IList<EyouSoft.Model.TourStructure.TourAttachInfo> items = null;
            if (string.IsNullOrEmpty(xml)) return items;
            items = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();

            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");

            foreach (var xRow in xRows)
            {
                items.Add(new EyouSoft.Model.TourStructure.TourAttachInfo()
                {
                    FilePath = Utils.GetXAttributeValue(xRow, "FilePath"),
                    Name = Utils.GetXAttributeValue(xRow, "Name")
                });
            }

            return items;
        }
        #endregion

        #region ITourEveryday 成员
        /// <summary>
        /// 写入散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        public int InsertTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourEveryday_InsertInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CreateTime", DbType.DateTime, info.CreateTime);
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));
            this._db.AddInParameter(cmd, "PriceStandards", DbType.String, this.CreatePriceStandardsXML(info.PriceStandards));
            
            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
            {
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            }

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal && info.TourNormalInfo != null)
            {
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
                this._db.AddInParameter(cmd, "NormalService", DbType.String, this.CreateNormalServiceXML(info.TourNormalInfo.Services));
                this._db.AddInParameter(cmd, "NormalPlans", DbType.String, this.CreateNormalPlansXML(info.TourNormalInfo.Plans));
            }

            this._db.AddInParameter(cmd, "ReleaseType", DbType.Byte, info.ReleaseType);
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
        /// 更新散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        public int UpdateTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourEveryday_UpdateInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "UpdateTime", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));
            this._db.AddInParameter(cmd, "PriceStandards", DbType.String, this.CreatePriceStandardsXML(info.PriceStandards));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
            {
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            }

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal && info.TourNormalInfo != null)
            {
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
                this._db.AddInParameter(cmd, "NormalService", DbType.String, this.CreateNormalServiceXML(info.TourNormalInfo.Services));
                this._db.AddInParameter(cmd, "NormalPlans", DbType.String, this.CreateNormalPlansXML(info.TourNormalInfo.Plans));
            }

            this._db.AddInParameter(cmd, "ReleaseType", DbType.Byte, info.ReleaseType);
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
        /// 删除散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        public int DeleteTourEverydayInfo(string tourId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_DeleteTourEverydayInfo);
            this._db.AddInParameter(cmd, "V", DbType.AnsiStringFixedLength, "1");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            return DbHelper.ExecuteSql(cmd, this._db);
        }

        /// <summary>
        /// 获取天天发计划报价信息集合
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> GetTourEverydayPriceStandards(string tourId)
        {
            List<EyouSoft.Model.TourStructure.TourPriceStandardInfo> items = new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayPriceStandards);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    //从后向前查找报价等级
                    EyouSoft.Model.TourStructure.TourPriceStandardInfo item = items.FindLast((EyouSoft.Model.TourStructure.TourPriceStandardInfo tmp) =>
                    {
                        if (rdr.GetInt32(rdr.GetOrdinal("Standard")) == tmp.StandardId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });

                    item = item ?? new EyouSoft.Model.TourStructure.TourPriceStandardInfo();

                    EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo levelInfo = new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo();
                    levelInfo.AdultPrice = rdr.GetDecimal(rdr.GetOrdinal("AdultPrice"));
                    levelInfo.ChildrenPrice = rdr.GetDecimal(rdr.GetOrdinal("ChildPrice"));
                    levelInfo.LevelId = rdr.GetInt32(rdr.GetOrdinal("LevelId"));
                    if (rdr.IsDBNull(rdr.GetOrdinal("LevType")))
                    {
                        levelInfo.LevelType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                    }
                    else
                    {
                        levelInfo.LevelType = (EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)rdr.GetByte(rdr.GetOrdinal("LevType"));
                    }
                    levelInfo.LevelName = rdr["LevName"].ToString();

                    if (item.StandardId == 0)
                    {
                        item.StandardId = rdr.GetInt32(rdr.GetOrdinal("Standard"));
                        item.CustomerLevels = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();
                        item.CustomerLevels.Add(levelInfo);
                        item.StandardName = rdr["StandardName"].ToString();

                        items.Add(item);
                    }
                    else
                    {
                        item.CustomerLevels.Add(levelInfo);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 获取散客天天发计划信息业务实体
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourEverydayInfo GetTourEverydayInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourEverydayInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourEverydayInfo()
                    {
                        AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId")),
                        Attachs = null,
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        HandleNumber = rdr.GetInt32(rdr.GetOrdinal("HandleNumber")),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        PriceStandards = null,
                        ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)rdr.GetByte(rdr.GetOrdinal("ReleaseType")),
                        RouteId = rdr.GetInt32(rdr.GetOrdinal("RouteId")),
                        RouteName = rdr["RouteName"].ToString(),
                        TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays")),
                        TourId = tourId,
                        TourNormalInfo = null,
                        TourQuickInfo = null,
                        UntreatedNumber = rdr.GetInt32(rdr.GetOrdinal("UntreatedNumber"))
                    };
                }
            }

            if (info != null)
            {
                info.Attachs = this.GetTourEverydayAttachs(info.TourId);
                info.PriceStandards = this.GetTourEverydayPriceStandards(info.TourId);
                info.TourNormalInfo = this.GetTourNormalPrivateInfo(info.TourId);
                info.TourQuickInfo = this.GetTourEverydayQuickPrivateInfo(info.TourId);
            }

            return info;
        }

        /// <summary>
        /// 获取散客天天发计划信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> GetTourEverydays(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourEverydaySearchInfo queryInfo)
        {
            IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> items = new List<EyouSoft.Model.TourStructure.LBTourEverydayInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_TourEveryday";
            string primaryKey = "TourId";
            string orderByString = "CreateTime DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,ReleaseType,RouteName,TourDays,HandleNumber,UntreatedNumber");
            fields.Append(" ,(SELECT ContactName,ContactTel,ContactFax,ContactMobile,ContactEmail,QQ,MSN FROM tbl_CompanyUser AS A WHERE A.Id=tbl_TourEveryday.OperatorId FOR XML RAW,ROOT('root')) AS OperatorXML ");
            fields.Append(" ,(SELECT AreaName FROM tbl_Area AS A WHERE A.Id=tbl_TourEveryday.Areaid) AS AreaName ");
            fields.Append(" ,(SELECT FilePath,Name FROM tbl_TourEverydayAttach AS A WHERE A.TourId=tbl_TourEveryday.TourId  for xml raw,root('root')) AS AttachsXML ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' ", companyId);
            queryInfo = queryInfo ?? new EyouSoft.Model.TourStructure.TourEverydaySearchInfo();

            if (queryInfo.AreaId.HasValue)
            {
                cmdQuery.AppendFormat(" AND AreaId={0} ", queryInfo.AreaId.Value);
            }
            if (queryInfo.Areas != null && queryInfo.Areas.Length > 0)
            {
                cmdQuery.AppendFormat(" AND AreaId IN( ");
                cmdQuery.AppendFormat("{0}", queryInfo.Areas[0]);

                for (int i = 1; i < queryInfo.Areas.Length; i++)
                {
                    cmdQuery.AppendFormat(",{0}", queryInfo.Areas[i]);
                }

                cmdQuery.AppendFormat(" ) ");
            }
            if (!string.IsNullOrEmpty(queryInfo.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", queryInfo.RouteName);
            }
            if (queryInfo.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", queryInfo.TourDays.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.TourStructure.LBTourEverydayInfo();

                    item.AreaName = rdr["AreaName"].ToString();
                    item.HandleNumber = rdr.GetInt32(rdr.GetOrdinal("HandleNumber"));
                    item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)rdr.GetByte(rdr.GetOrdinal("ReleaseType"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.UntreatedNumber = rdr.GetInt32(rdr.GetOrdinal("UntreatedNumber"));
                    //负责人信息
                    this.ParseTourEverydayOperatorByXml(rdr["OperatorXML"].ToString(), item);
                    item.Attachs = this.ParseTourEverydayAttachsByXml(rdr["AttachsXML"].ToString());

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 申请散客天天发计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划申请信息业务实体</param>
        /// <returns></returns>
        public int ApplyTourEveryday(EyouSoft.Model.TourStructure.TourEverydayApplyInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourEveryday_Apply");
            this._db.AddInParameter(cmd, "ApplyId", DbType.AnsiStringFixedLength, info.ApplyId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "ApplyCompanyId", DbType.Int32, info.ApplyCompanyId);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AdultNumber", DbType.Int32, info.AdultNumber);
            this._db.AddInParameter(cmd, "ChildrenNumber", DbType.Int32, info.ChildrenNumber);
            this._db.AddInParameter(cmd, "StandardId", DbType.Int32, info.StandardId);
            this._db.AddInParameter(cmd, "LevelId", DbType.Int32, info.LevelId);
            this._db.AddInParameter(cmd, "SpecialRequirement", DbType.String, info.SpecialRequirement);
            if (info.Traveller != null)
            {
                this._db.AddInParameter(cmd, "TravellerDisplayType", DbType.Byte, info.Traveller.TravellerDisplayType);
                this._db.AddInParameter(cmd, "TravellerFilePath", DbType.String, info.Traveller.TravellerFilePath);
                this._db.AddInParameter(cmd, "Travellers", DbType.String, this.CreateTourEverydayApplyTravellersXML(info.Traveller.Travellers));
            }
            else
            {
                this._db.AddInParameter(cmd, "TravellerDisplayType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.None);
                this._db.AddInParameter(cmd, "TravellerFilePath", DbType.String, DBNull.Value);
                this._db.AddInParameter(cmd, "Travellers", DbType.String, DBNull.Value);
            }

            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "ApplyTime", DbType.DateTime, info.ApplyTime);
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
        /// 获取散客天天发计划申请信息业务实体
        /// </summary>
        /// <param name="applyId">申请编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourEverydayApplyInfo GetTourEverydayApplyInfo(string applyId)
        {
            EyouSoft.Model.TourStructure.TourEverydayApplyInfo info = new EyouSoft.Model.TourStructure.TourEverydayApplyInfo();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourEverydayApplyInfo);
            this._db.AddInParameter(cmd, "ApplyId", DbType.AnsiStringFixedLength, applyId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourEverydayApplyInfo()
                    {
                        AdultNumber = rdr.GetInt32(rdr.GetOrdinal("AdultNumber")),
                        ApplyCompanyId = rdr.GetInt32(rdr.GetOrdinal("ApplyCompanyId")),
                        ApplyId = applyId,
                        ApplyTime = rdr.GetDateTime(rdr.GetOrdinal("ApplyTime")),
                        BuildTourId = rdr.IsDBNull(rdr.GetOrdinal("BuildTourId")) ? null : rdr.GetString(rdr.GetOrdinal("BuildTourId")),
                        ChildrenNumber = rdr.GetInt32(rdr.GetOrdinal("ChildrenNumber")),
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        HandleOperatorId = rdr.IsDBNull(rdr.GetOrdinal("HandleOperatorId")) ? null : (Nullable<int>)rdr.GetInt32(rdr.GetOrdinal("HandleOperatorId")),
                        HandleStatus = (EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus)rdr.GetByte(rdr.GetOrdinal("HandleStatus")),
                        HandleTime = rdr.IsDBNull(rdr.GetOrdinal("HandleTime")) ? null : (Nullable<DateTime>)rdr.GetDateTime(rdr.GetOrdinal("HandleTime")),
                        LDate = rdr.GetDateTime(rdr.GetOrdinal("LDate")),
                        LevelId = rdr.GetInt32(rdr.GetOrdinal("LevelId")),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        SpecialRequirement = rdr["SpecialRequirement"].ToString(),
                        StandardId = rdr.GetInt32(rdr.GetOrdinal("Standard")),
                        TourId = rdr.GetString(rdr.GetOrdinal("TourId")),
                        Traveller = new EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo()
                    };

                    info.Traveller.TravellerDisplayType = (EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType)rdr.GetByte(rdr.GetOrdinal("CustomerDisplayType"));
                    info.Traveller.TravellerFilePath = rdr["CustomerFilePath"].ToString();
                    info.Traveller.Travellers = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
                }

                if (rdr.NextResult() && info != null)
                {
                    while (rdr.Read())
                    {
                        info.Traveller.Travellers.Add(new EyouSoft.Model.TourStructure.TourOrderCustomer()
                        {
                            ID = rdr.GetString(rdr.GetOrdinal("TravellerId")),
                            VisitorName = rdr["TravellerName"].ToString(),
                            CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)rdr.GetByte(rdr.GetOrdinal("CertificateType")),
                            CradNumber = rdr["CertificateCode"].ToString(),
                            Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)rdr.GetByte(rdr.GetOrdinal("Gender")),
                            VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)rdr.GetByte(rdr.GetOrdinal("TravellerType")),
                            ContactTel = rdr["Telephone"].ToString()
                        });
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 获取散客天天发计划处理申请信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="tourId">天天发计划编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="handleStatus">处理状态</param>
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> GetTourEverydayHandleApplys(int companyId, string tourId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus? handleStatus, EyouSoft.Model.TourStructure.TourEverydayHandleApplySearchInfo queryInfo)
        {
            IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> items = new List<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_TourEverydayApply";
            string primaryKey = "TourId";
            string orderByString = "ApplyTime DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("ApplyId,TourId,BuildTourId,AdultNumber,ChildrenNumber,Standard,LevelId,LDate");
            fields.Append(" ,(SELECT TourCode FROM tbl_Tour AS A WHERE A.TourId=tbl_TourEverydayApply.BuildTourId) AS BuildTourCode ");
            fields.Append(" ,(SELECT RouteName,ReleaseType FROM tbl_TourEveryday AS A WHERE A.TourId=tbl_TourEverydayApply.TourId FOR XML RAW,ROOT('root')) AS TourEverydayXML ");
            fields.Append(" ,(SELECT [Name] FROM tbl_Customer AS A WHERE A.Id=tbl_TourEverydayApply.ApplyCompanyId) AS ApplyCompanyName ");
            fields.Append(" ,(SELECT ContactName,ContactTel FROM tbl_CompanyUser AS A WHERE A.Id=tbl_TourEverydayApply.OperatorId FOR XML RAW,ROOT('root')) AS ApplyContacterXML ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);

            if (!string.IsNullOrEmpty(tourId))
            {
                cmdQuery.AppendFormat(" AND TourId='{0}' ", tourId);
            }

            if (handleStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND HandleStatus={0} ", (int)handleStatus.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo()
                    {
                        AdultNumber = rdr.GetInt32(rdr.GetOrdinal("AdultNumber")),
                        AdultPrice = 0,
                        ApplyId = rdr.GetString(rdr.GetOrdinal("ApplyId")),
                        BuildTourCode = rdr["BuildTourCode"].ToString(),
                        BuildTourId = rdr["BuildTourId"].ToString(),
                        ChildrenNumber = rdr.GetInt32(rdr.GetOrdinal("ChildrenNumber")),
                        ChildrenPrice = 0,
                        LevelId = rdr.GetInt32(rdr.GetOrdinal("LevelId")),
                        RouteName = string.Empty,
                        StandardId = rdr.GetInt32(rdr.GetOrdinal("Standard")),
                        TourId = rdr.GetString(rdr.GetOrdinal("TourId")),
                        LDate = rdr.GetDateTime(rdr.GetOrdinal("LDate")),
                        ApplyCompanyName = rdr["ApplyCompanyName"].ToString(),
                        ApplyContacterName = string.Empty,
                        ApplyContacterTelephone = string.Empty,
                        ReleaseType = EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick
                    };

                    string applyContacterXml = rdr["ApplyContacterXML"].ToString();

                    if (!string.IsNullOrEmpty(applyContacterXml))
                    {
                        XElement xRoot = XElement.Parse(applyContacterXml);
                        var xRow = Utils.GetXElement(xRoot, "row");
                        item.ApplyContacterName = Utils.GetXAttributeValue(xRow, "ContactName");
                        item.ApplyContacterTelephone = Utils.GetXAttributeValue(xRow, "ContactTel");
                    }

                    string tourEverydayXML = rdr["TourEverydayXML"].ToString();

                    if (!string.IsNullOrEmpty(tourEverydayXML))
                    {
                        XElement xRoot = XElement.Parse(tourEverydayXML);
                        var xRow = Utils.GetXElement(xRoot, "row");
                        item.RouteName = Utils.GetXAttributeValue(xRow, "RouteName");
                        item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "ReleaseType"));
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 散客天天发计划生成散拼计划后续工作(生成订单、更新申请状态、天天发计划已处理数量维护、未处理数量维护等)，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">散拼计划计划编号</param>
        /// <param name="everydayTourId">天天发计划编号</param>
        /// <param name="everydayTourApplyId">天天发计划申请编号</param>
        /// <param name="buyerCompanyId">客户单位编号</param>
        /// <returns></returns>
        public int BuildTourFollow(string tourId, string everydayTourId, string everydayTourApplyId,out int buyerCompanyId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourEveryday_BuildTourFollow");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            this._db.AddInParameter(cmd, "EverydayTourId", DbType.AnsiStringFixedLength, everydayTourId);
            this._db.AddInParameter(cmd, "EverydayTourApplyId", DbType.AnsiStringFixedLength, everydayTourApplyId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "ApplyCompanyId", DbType.Int32, 4);

            int sqlExceptionCode = 0;
            buyerCompanyId = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            buyerCompanyId = Convert.ToInt32(this._db.GetParameterValue(cmd, "ApplyCompanyId"));   

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));       
        }

        #endregion
    }
}
