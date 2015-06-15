//汪奇志 2012-08-26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 团款支出相关业务逻辑类
    /// </summary>
    public class BZhiChu
    {
        readonly EyouSoft.IDAL.FinanceStructure.IZhiChu dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.IZhiChu>();

         #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BZhiChu() { }
        #endregion

        #region public members
        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo> GetJiDiaoZhiChuLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo)
        {
            if (companyId < 1) return null;

            return dal.GetJiDiaoZhiChuLB(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
        }

        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="zhiChuJinE">支出金额</param>
        /// <param name="yiDengJiJinE">已登记金额</param>
        /// <param name="yiZhiFuJinE">已支付金额</param>
        public void GetJiDiaoZhiChuLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo, out decimal zhiChuJinE, out decimal yiDengJiJinE, out decimal yiZhiFuJinE)
        {
            zhiChuJinE = 0; yiDengJiJinE = 0; yiZhiFuJinE = 0;

            if (companyId < 1) return;

            dal.GetJiDiaoZhiChuLBHeJi(companyId, searchInfo, out zhiChuJinE, out yiDengJiJinE, out yiZhiFuJinE);
        }

        /// <summary>
        /// 财务管理-团款支出-按计调类型批量支出登记，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="info">登记信息业务实体</param>
        /// <returns></returns>
        public int PiLiangDengJi(int companyId, int operatorId, EyouSoft.Model.FinanceStructure.MZhiChuPiLiangDengJiInfo info)
        {
            if (companyId < 1 || operatorId < 1 || info == null || info.AnPaiIds == null || info.AnPaiIds.Length < 1) return 0;

            int dalRetCode = dal.PiLiangDengJi(companyId, operatorId, info);

            if (dalRetCode == 1)
            {
                StringBuilder s = new StringBuilder();
                s.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "做批量付款登记(按支出项目)");

                s.Append("，登记的支出安排编号为:");
                foreach (var anPaiId in info.AnPaiIds)
                {
                    s.Append(anPaiId + "、");
                }

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = s.ToString();
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "批量付款登记(按支出项目)";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }

            return dalRetCode;
        }
        #endregion
    }
}
