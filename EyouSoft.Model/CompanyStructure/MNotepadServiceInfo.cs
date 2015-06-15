using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-04-28
    public class MNotepadServiceInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MNotepadServiceInfo() { }

        /// <summary>
        /// 模板编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.NotepadServiceType Type { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒等模板查询信息业务实体
    /// </summary>
    public class MNotepadServiceSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MNotepadServiceSearchInfo() { }
    }
}
