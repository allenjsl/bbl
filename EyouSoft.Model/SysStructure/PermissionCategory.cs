﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SysStructure
{
    /// <summary>
    /// 创建人：鲁功源 2011-01-18
    /// 描述：系统权限大类别实体类
    /// </summary>
    [Serializable]
    public class PermissionCategory
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionCategory() { }

        #region 成员属性
        /// <summary>
        /// 权限大类编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 权限类型编号
        /// </summary>
        public int TypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 权限类别名称
        /// </summary>
        public string CategoryName
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
        /// 权限子类别集合
        /// </summary>
        public IList<PermissionClass> PermissionClass
        {
            get;
            set;
        }
        #endregion
    }
    /// <summary>
    /// 权限系统类型实体
    /// </summary>
    /// 鲁功源 2011-01-18
    [Serializable]
    public class PermissionType
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PermissionType() { }
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 权限类型名称
        /// </summary>
        public string PermissionTypeName
        {
            get;
            set;
        }
        #endregion
    }
}
