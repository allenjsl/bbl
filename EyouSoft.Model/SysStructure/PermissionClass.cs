﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SysStructure
{
    /// <summary>
    /// 创建人：鲁功源 2011-01-18
    /// 描述：系统权限子类别实体类
    /// </summary>
    [Serializable]
    public class PermissionClass
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionClass() { }

        #region 成员属性
        /// <summary>
        /// 权限类别编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 权限大类编号
        /// </summary>
        public int CategoryId
        {
            get;
            set;
        }
        /// <summary>
        /// 权限类别名称
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int SortId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用，1：启用；0：禁用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 权限明细集合
        /// </summary>
        public IList<Permission> Permission
        {
            get;
            set;
        }
        #endregion
    }
}
