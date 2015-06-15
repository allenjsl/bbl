using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    public class BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyUser dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyUser>();
        private static object _cacheHelper = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BLLBase() { }

        private int[] _departIds;
        private bool _IsEnable = true;

        /// <summary>
        /// 当前用户公司Id
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门Id数字
        /// </summary>
        public int[] DepartIds
        {
            get { return _departIds; }
            set { _departIds = value; }
        }

        /// <summary>
        /// 根据部门Id生成的用户Id字符串（半角逗号分割）
        /// </summary>
        public string HaveUserIds
        {
            get { return this.GetUserIdsByDepartIds(); }
        }

        /// <summary>
        /// 是否启用组织机构
        /// </summary>
        public bool IsEnable
        {
            get { return _IsEnable; }
            set { _IsEnable = value; }
        }

        /// <summary>
        /// 根据部门Id生成的用户Id字符串
        /// </summary>
        private string GetUserIdsByDepartIds()
        {
            if (!_IsEnable)
                return string.Empty;

            if (this._departIds == null || this._departIds.Length < 1 || CompanyId <= 0)
            {
                throw new System.Exception("错误：new BLL的时候请传入UserModel ！");
            }

            if (_departIds.Length <= 0)
                return string.Empty;

            IList<int> UserIds = new List<int>();

            foreach(int i in _departIds)
            {
                UserIds = UserIds.Union(this.GetUsers(CompanyId, i)).ToList();
            }

            string strIds = string.Empty;
            foreach (int i in UserIds)
            {
                if (i <= 0)
                    continue;

                strIds += i.ToString() + ",";
            }
            strIds = strIds.Trim(',');

            return strIds;
        }

        /// <summary>
        /// 根据部门编号获取同级及下级部门所有用户信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="departmentId">部门编号</param>
        /// <returns></returns>
        public IList<int> GetUsers(int companyId, int departmentId)
        {
            lock (_cacheHelper)
            {
                IList<int> us = null;
                //IDictionary<部门编号,IList<用户编号>>
                IDictionary<int, IList<int>> items = (IDictionary<int, IList<int>>)EyouSoft.Cache.Facade.EyouSoftCache.GetCache(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId));

                if (items == null || items.Count < 1)
                {
                    us = dal.GetUsers(departmentId);

                    items = new Dictionary<int, IList<int>>();
                    items.Add(departmentId, us);

                    EyouSoft.Cache.Facade.EyouSoftCache.Add(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId), items);
                }
                else
                {
                    if (items.ContainsKey(departmentId))
                    {
                        us = items[departmentId];
                    }
                    else
                    {
                        us = dal.GetUsers(departmentId);
                        items.Add(departmentId, us);
                        //items[departmentId] = us;

                        EyouSoft.Cache.Facade.EyouSoftCache.Add(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId), items);
                    }
                }

                return us;
            }
        }
    }
}
