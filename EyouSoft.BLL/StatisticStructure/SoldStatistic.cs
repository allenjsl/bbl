using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    public class SoldStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.ISoldStatistic idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.ISoldStatistic>();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public SoldStatistic()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public SoldStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        /// <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomers(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo seachInfo)
        {
            return idal.GetCustomers(companyId, pageSize, pageIndex, ref recordCount, seachInfo);
        }
        /// <summary>
        /// 按指定条件获取 人数合计信息
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        public Dictionary<string, int> GetSoldStatSumInfo(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo seachInfo)
        { return idal.GetSoldStatSumInfo(companyId, pageSize, pageIndex, ref recordCount, seachInfo); }
        // <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaDepartStat> getAreaStatList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.AreaSoldStatSearch searchInfo)
        {
            return idal.getAreaStatList(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
        }
        /// <summary>
        /// 取得有订单信息的部门名称列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime"> 开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="tourType">团队类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaStatDepartList> getDepartList(int companyId, DateTime? startTime, DateTime? entTime
            , Model.EnumType.TourStructure.TourType? tourType)
        {
            return idal.getDepartList(companyId, startTime, entTime, tourType);
        }
        /// <summary>
        /// 取得指定年月某个城市某个部门的人数统计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime">开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="cityId">城市编号</param>
        /// <param name="DeptId">部门编号</param>
        /// <param name="salerId">责任销售编号</param>
        /// <param name="tourType">团队类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaDepartStatInfo> getDepartStatList(int companyId, DateTime? startTime
            , DateTime? entTime, int cityId, int salerId, Model.EnumType.TourStructure.TourType? tourType)
        {
            return idal.getDepartStatList(companyId, startTime, entTime, cityId, salerId, tourType);
        }

        /// <summary>
        /// 获取区域销售统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime">开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="deptId">部门编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="tourType">团队类型</param>
        /// <param name="heJi">合计数</param>
        public void GetQuYuXiaoShouTongJiHeJi(int companyId, DateTime? startTime, DateTime? entTime, int deptId
            , Model.StatisticStructure.AreaSoldStatSearch searchInfo, Model.EnumType.TourStructure.TourType? tourType
            , out int heJi)
        {
            heJi = 0;
            if (companyId < 1 || deptId < 1) return;

            idal.GetQuYuXiaoShouTongJiHeJi(companyId, startTime, entTime, deptId, searchInfo, tourType, out heJi);
        }
    }
}
