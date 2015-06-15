using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.PlanStructure;
using EyouSoft.Model.TourStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.RouteStructure
{
    /// <summary>
    /// 线路数据层
    /// </summary>
    /// 鲁功源 2011-01-24
    public class Route : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.RouteStructure.IRoute
    {
        #region 私有变量
        private const string SQL_Route_DELETE = "UPDATE tbl_Route SET IsDelete='1' where ID in({0})";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Route()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region IRoute 成员
        /// <summary>
        /// 写入线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_Route_InsertRouteInfo");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, info.OperatorName);
            this._db.AddInParameter(dc, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(dc, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(dc, "RouteDays", DbType.Int32, info.RouteDays);
            this._db.AddInParameter(dc, "ReleaseType", DbType.Byte, (int)info.ReleaseType);
            this._db.AddInParameter(dc, "RouteDepict", DbType.String, info.RouteDepict);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "Attachs", DbType.String, this.CreateAttachXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                this._db.AddInParameter(dc, "QuickPrivate", DbType.String, DBNull.Value);                
                if (info.RouteNormalInfo != null)
                {
                    this._db.AddInParameter(dc, "Service", DbType.String, this.CreateRouteServiceXml(info.RouteNormalInfo.Services));
                    this._db.AddInParameter(dc, "Plans", DbType.String, this.CreatePlanXml(info.RouteNormalInfo.Plans));
                }
                else
                {
                    this._db.AddInParameter(dc, "Service", DbType.String, DBNull.Value);
                    this._db.AddInParameter(dc, "Plans", DbType.String, DBNull.Value);
                }
                this._db.AddInParameter(dc, "OtherService", DbType.String, this.CreateOtherServiceXml(info.RouteNormalInfo));
            }
            else
            {
                this._db.AddInParameter(dc, "QuickPrivate", DbType.String, this.CreateQuickPrivateXml(info.RouteQuickInfo));
                this._db.AddInParameter(dc, "Service", DbType.String, DBNull.Value);
                this._db.AddInParameter(dc, "Plans", DbType.String, DBNull.Value);
                this._db.AddInParameter(dc, "OtherService", DbType.String, DBNull.Value);
            }
            DbHelper.RunProcedure(dc, this._db);
            int Result = 0;
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj != null)
            {
                Result = 1;
                info.RouteId = int.Parse(obj.ToString());
            }
            return Result;
        }
        /// <summary>
        /// 更新线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_Route_UpdateRouteInfo");
            this._db.AddInParameter(dc, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, info.OperatorName);
            this._db.AddInParameter(dc, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(dc, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(dc, "RouteDays", DbType.Int32, info.RouteDays);
            this._db.AddInParameter(dc, "ReleaseType", DbType.Byte, (int)info.ReleaseType);
            this._db.AddInParameter(dc, "RouteDepict", DbType.String, info.RouteDepict);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "Attachs", DbType.String, this.CreateAttachXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
            {
                this._db.AddInParameter(dc, "QuickPrivate", DbType.String, DBNull.Value);
                
                if (info.RouteNormalInfo != null)
                {
                    this._db.AddInParameter(dc, "Service", DbType.String, this.CreateRouteServiceXml(info.RouteNormalInfo.Services));
                    this._db.AddInParameter(dc, "Plans", DbType.String, this.CreatePlanXml(info.RouteNormalInfo.Plans));
                }
                else
                {
                    this._db.AddInParameter(dc, "Service", DbType.String, DBNull.Value);
                    this._db.AddInParameter(dc, "Plans", DbType.String, DBNull.Value);
                }
                
                this._db.AddInParameter(dc, "OtherService", DbType.String, this.CreateOtherServiceXml(info.RouteNormalInfo));
            }
            else
            {
                this._db.AddInParameter(dc, "QuickPrivate", DbType.String, this.CreateQuickPrivateXml(info.RouteQuickInfo));                
                this._db.AddInParameter(dc, "Service", DbType.String, DBNull.Value);
                this._db.AddInParameter(dc, "Plans", DbType.String, DBNull.Value);
                this._db.AddInParameter(dc, "OtherService", DbType.String, DBNull.Value);
            }
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            return int.Parse(obj.ToString());
        }
        /// <summary>
        /// 删除线路信息
        /// </summary>
        /// <param name="routeIds">线路编号集合</param>
        /// <returns></returns>
        public bool Delete(string routeIds)
        {
            DbCommand dc = this._db.GetSqlStringCommand(string.Format(SQL_Route_DELETE, routeIds));
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.RouteStructure.RouteInfo GetRouteInfo(int routeId)
        {
            EyouSoft.Model.RouteStructure.RouteInfo model = null;
            DbCommand dc = this._db.GetStoredProcCommand("proc_Route_GetRouteInfo");
            this._db.AddInParameter(dc, "RouteId", DbType.Int32, routeId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    #region 线路基本信息
                    model = new EyouSoft.Model.RouteStructure.RouteInfo();
                    model.AreaId = dr.GetInt32(dr.GetOrdinal("AreaId"));
                    model.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    model.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr[dr.GetOrdinal("RouteIssueTypeId")].ToString());
                    model.RouteDays = dr.GetInt32(dr.GetOrdinal("RouteDays"));
                    model.RouteDepict = dr[dr.GetOrdinal("RouteDepict")].ToString();
                    model.RouteId = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.RouteName = dr[dr.GetOrdinal("RouteName")].ToString();
                    model.TourCount = dr.GetInt32(dr.GetOrdinal("TourCount"));
                    model.VisitorCount = dr.GetInt32(dr.GetOrdinal("VisitorCount"));
                    #endregion

                    #region 标准线路的附件信息
                    dr.NextResult();
                    IList<EyouSoft.Model.TourStructure.TourAttachInfo> AttachList = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
                    while (dr.Read())
                    {
                        EyouSoft.Model.TourStructure.TourAttachInfo attchModel = new EyouSoft.Model.TourStructure.TourAttachInfo();
                        attchModel.FilePath = dr[dr.GetOrdinal("FilePath")].ToString();
                        attchModel.Name = dr[dr.GetOrdinal("FileName")].ToString();
                        AttachList.Add(attchModel);
                        attchModel = null;
                    }
                    model.Attachs = AttachList;
                    #endregion

                    #region 标准线路服务标准信息
                    model.RouteNormalInfo = new TourNormalPrivateInfo();
                    dr.NextResult();
                    IList<EyouSoft.Model.TourStructure.TourServiceInfo> ServiceList = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
                    while (dr.Read())
                    {
                        if (dr[dr.GetOrdinal("ServiceType")].ToString() == "BuHanXiangMu")
                            model.RouteNormalInfo.BuHanXiangMu = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "ErTongAnPai")
                            model.RouteNormalInfo.ErTongAnPai = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "GouWuAnPai")
                            model.RouteNormalInfo.GouWuAnPai = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "NeiBuXingXi")
                            model.RouteNormalInfo.NeiBuXingXi = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "WenXinTiXing")
                            model.RouteNormalInfo.WenXinTiXing = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "ZhuYiShiXiang")
                            model.RouteNormalInfo.ZhuYiShiXiang = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else if (dr[dr.GetOrdinal("ServiceType")].ToString() == "ZiFeiXIangMu")
                            model.RouteNormalInfo.ZiFeiXIangMu = dr[dr.GetOrdinal("ServiceValue")].ToString();
                        else
                        {
                            EyouSoft.Model.TourStructure.TourServiceInfo ServiceModel = null;
                            switch (dr[dr.GetOrdinal("ServiceType")].ToString())
                            {
                                case "DiJie":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.地接;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "JiuDian":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.酒店;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "YongCan":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.用餐;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "JingDian":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.景点;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "BaoXian":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.保险;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "DaJiaoTong":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "DaoFu":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.导服;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "GouWu":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.购物;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "XiaoJiaoTong":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.小交通;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                                case "QiTa":
                                    ServiceModel = new TourServiceInfo();
                                    ServiceModel.ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.其它;
                                    ServiceModel.Service = dr[dr.GetOrdinal("ServiceValue")].ToString();
                                    ServiceList.Add(ServiceModel);
                                    ServiceModel = null;
                                    break;
                            }
                            model.RouteNormalInfo.Services = ServiceList;
                        }
                    }
                    #endregion

                    #region 标准线路行程安排信息
                    dr.NextResult();
                    IList<EyouSoft.Model.TourStructure.TourPlanInfo> PlanList = new List<EyouSoft.Model.TourStructure.TourPlanInfo>();
                    while (dr.Read())
                    {
                        EyouSoft.Model.TourStructure.TourPlanInfo PlanModel = new EyouSoft.Model.TourStructure.TourPlanInfo();
                        PlanModel.Dinner = dr[dr.GetOrdinal("Dinner")].ToString();
                        PlanModel.FilePath = dr[dr.GetOrdinal("ImgPath")].ToString();
                        PlanModel.Hotel = dr[dr.GetOrdinal("House")].ToString();
                        PlanModel.Interval = dr[dr.GetOrdinal("PlanInterval")].ToString();
                        PlanModel.Plan = dr[dr.GetOrdinal("PlanContent")].ToString();
                        PlanModel.Vehicle = dr[dr.GetOrdinal("Vehicle")].ToString();
                        PlanList.Add(PlanModel);
                        PlanModel = null;
                    }
                    model.RouteNormalInfo.Plans = PlanList;
                    #endregion

                    #region 快速线路行程安排信息
                    dr.NextResult();
                    if (dr.Read())
                    {
                        model.RouteQuickInfo = new TourQuickPrivateInfo();
                        model.RouteQuickInfo.QuickPlan = dr[dr.GetOrdinal("RoutePlan")].ToString();
                        model.RouteQuickInfo.Service = dr[dr.GetOrdinal("ServeRule")].ToString();
                        model.RouteQuickInfo.Remark = dr[dr.GetOrdinal("Remark")].ToString();
                    }
                    #endregion
                }
            }
            return model;
        }
        /// <summary>
        /// 获取线路信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="ReleaseType">线路发布类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> GetRoutes(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.RouteStructure.RouteSearchInfo info, string us, EyouSoft.Model.EnumType.TourStructure.ReleaseType? ReleaseType)
        {
            IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> list = new List<EyouSoft.Model.RouteStructure.RouteBaseInfo>();
            
            #region 分页存储过程参数设置
            string tableName = "tbl_Route";
            string fields = "Id,AreaId,RouteName,OperatorId,OperatorName,RouteDays,IssueTime,TourCount,VisitorCount,RouteIssueTypeId,(select AreaName from tbl_Area where id=tbl_Route.AreaId and IsDelete='0') as AreaName";
            string primaryKey = "Id";
            string orderStrBy = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder("");
            strWhere.AppendFormat(" IsDelete='0' AND CompanyId={0} ", companyId);
            if (info != null)
            {
                if (info.AreaId.HasValue && info.AreaId.Value > 0)
                    strWhere.AppendFormat(" AND AreaId={0} ",info.AreaId.Value);
                if (info.ROperatorId.HasValue && info.ROperatorId.Value > 0)
                    strWhere.AppendFormat(" AND OperatorId={0} ", info.ROperatorId.Value);
                if (info.RouteDays.HasValue && info.RouteDays.Value > 0)
                    strWhere.AppendFormat(" AND RouteDays={0} ",info.RouteDays.Value);
                if (!string.IsNullOrEmpty(info.RouteName))
                    strWhere.AppendFormat(" AND RouteName like '%{0}%' ",info.RouteName);
                if (info.RSDate.HasValue)
                    strWhere.AppendFormat(" AND datediff(dd,IssueTime,'{0}')<=0 ",info.RSDate.Value.ToString());
                if (info.REDate.HasValue)
                    strWhere.AppendFormat(" AND datediff(dd,IssueTime,'{0}')>=0 ", info.REDate.Value.ToString());
                if (!string.IsNullOrEmpty(info.OperatorName))
                    strWhere.AppendFormat(" AND OperatorName like '%{0}%' ", info.OperatorName);
            }
            if (!string.IsNullOrEmpty(us))
            {
                strWhere.AppendFormat(" AND OperatorId in({0}) ", us.TrimEnd(','));
            }
            if (ReleaseType.HasValue)
            {
                strWhere.AppendFormat(" AND RouteIssueTypeId={0} ", (int)ReleaseType.Value);
            }
            #endregion

            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, strWhere.ToString(), orderStrBy))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.RouteStructure.RouteBaseInfo model = new EyouSoft.Model.RouteStructure.RouteBaseInfo();
                    if (!dr.IsDBNull(dr.GetOrdinal("AreaId")))
                        model.AreaId = dr.GetInt32(dr.GetOrdinal("AreaId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        model.RouteId = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteName")))
                        model.RouteName = dr[dr.GetOrdinal("RouteName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorName")))
                        model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteDays")))
                        model.RouteDays = dr.GetInt32(dr.GetOrdinal("RouteDays"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TourCount")))
                        model.TourCount = dr.GetInt32(dr.GetOrdinal("TourCount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("VisitorCount")))
                        model.VisitorCount = dr.GetInt32(dr.GetOrdinal("VisitorCount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteIssueTypeId")))
                        model.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr[dr.GetOrdinal("RouteIssueTypeId")].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("AreaName")))
                        model.AreaName = dr[dr.GetOrdinal("AreaName")].ToString();
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 创建线路附件信息的XML
        /// </summary>
        /// <param name="Attachs">线路附件列表</param>
        /// <returns></returns>
        private string CreateAttachXML(IList<EyouSoft.Model.TourStructure.TourAttachInfo> Attachs)
        {
            if (Attachs == null || Attachs.Count == 0)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (EyouSoft.Model.TourStructure.TourAttachInfo model in Attachs)
            {
                strXml.AppendFormat("<AttachInfo FilePath=\"{0}\" Name=\"{1}\" />",model.FilePath,model.Name);
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        /// <summary>
        /// 创建快速版线路专有信息XML
        /// </summary>
        /// <param name="RouteQuickInfo">快速版线路专有信息实体</param>
        /// <returns></returns>
        private string CreateQuickPrivateXml(EyouSoft.Model.TourStructure.TourQuickPrivateInfo RouteQuickInfo)
        {
            if (RouteQuickInfo == null)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            strXml.AppendFormat("<QuickPrivateInfo QuickPlan=\"{0}\" Service=\"{1}\" Remark=\"{2}\" />",
                Utils.ReplaceXmlSpecialCharacter(RouteQuickInfo.QuickPlan), Utils.ReplaceXmlSpecialCharacter(RouteQuickInfo.Service), 
                Utils.ReplaceXmlSpecialCharacter(RouteQuickInfo.Remark));
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        /// <summary>
        /// 创建包含项目信息XML
        /// </summary>
        /// <param name="Services">包含项目信息集合</param>
        /// <returns></returns>
        private string CreateRouteServiceXml(IList<TourServiceInfo> Services)
        {
            if (Services == null || Services.Count == 0)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (TourServiceInfo item in Services)
            {
                strXml.AppendFormat("<ServiceInfo ServiceType=\"{0}\" ServiceValue=\"{1}\"  />", (int)item.ServiceType,Utils.ReplaceXmlSpecialCharacter(item.Service));
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        /// <summary>
        /// 创建标准线路其他项目
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string CreateOtherServiceXml(EyouSoft.Model.TourStructure.TourNormalPrivateInfo model)
        {
            if (model == null)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            if(!string.IsNullOrEmpty(model.BuHanXiangMu))
                strXml.AppendFormat("<OtherService ItemType=\"BuHanXiangMu\" ItemValue=\"{0}\" />",model.BuHanXiangMu);
            if (!string.IsNullOrEmpty(model.ErTongAnPai))
                strXml.AppendFormat("<OtherService ItemType=\"ErTongAnPai\" ItemValue=\"{0}\" />", model.ErTongAnPai);
            if (!string.IsNullOrEmpty(model.GouWuAnPai))
                strXml.AppendFormat("<OtherService ItemType=\"GouWuAnPai\" ItemValue=\"{0}\" />", model.GouWuAnPai);
            if (!string.IsNullOrEmpty(model.NeiBuXingXi))
                strXml.AppendFormat("<OtherService ItemType=\"NeiBuXingXi\" ItemValue=\"{0}\" />", model.NeiBuXingXi);
            if (!string.IsNullOrEmpty(model.WenXinTiXing))
                strXml.AppendFormat("<OtherService ItemType=\"WenXinTiXing\" ItemValue=\"{0}\" />", model.WenXinTiXing);
            if (!string.IsNullOrEmpty(model.ZhuYiShiXiang))
                strXml.AppendFormat("<OtherService ItemType=\"ZhuYiShiXiang\" ItemValue=\"{0}\" />", model.ZhuYiShiXiang);
            if (!string.IsNullOrEmpty(model.ZiFeiXIangMu))
                strXml.AppendFormat("<OtherService ItemType=\"ZiFeiXIangMu\" ItemValue=\"{0}\" />", model.ZiFeiXIangMu);
            strXml.Append("</ROOT>");
            return strXml.ToString();

        }
        /// <summary>
        /// 创建标准版线路行程的XML
        /// </summary>
        /// <param name="model">线路行程的集合</param>
        /// <returns></returns>
        private string CreatePlanXml(IList<EyouSoft.Model.TourStructure.TourPlanInfo> list)
        {
            if (list == null || list.Count == 0)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            int i=1;
            foreach(EyouSoft.Model.TourStructure.TourPlanInfo model in list)
            {
                strXml.Append("<PlanInfo ");
                strXml.AppendFormat(" PlanInterval=\"{0}\" Vehicle=\"{1}\" House=\"{2}\" ", Utils.ReplaceXmlSpecialCharacter(model.Interval), Utils.ReplaceXmlSpecialCharacter(model.Vehicle), Utils.ReplaceXmlSpecialCharacter(model.Hotel));
                strXml.AppendFormat(" Dinner=\"{0}\" PlanContent=\"{1}\" PlanDay=\"{2}\" ImgPath=\"{3}\" />", Utils.ReplaceXmlSpecialCharacter(model.Dinner), Utils.ReplaceXmlSpecialCharacter(model.Plan),
                    i, model.FilePath);
                i++;
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        #endregion
    }
}
