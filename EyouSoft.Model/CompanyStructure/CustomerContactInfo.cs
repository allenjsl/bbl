using System;
namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 实体类CustomerContactInfo ,客户联系人
    /// autor:李焕超
    /// date:2011-1-17
    /// </summary>
    [Serializable]
    public class CustomerContactInfo
    {
        public CustomerContactInfo()
        { }
        #region Model
        private int _id;
        private int _customerid;
        private string _jobid;
        private string _departmentid;
        private int _userid;
        private string _sex;
        private string _name;
        private string _tel;
        private string _mobile;
        private string _qq;
        private DateTime? _birthday;
        private string _email;
        private string _spetialty;
        private string _hobby;
        private string _remark;
        private string _AreaIds;
        private string _AreaNames;
        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerId
        {
            set { _customerid = value; }
            get { return _customerid; }
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string Job
        {
            set { _jobid = value; }
            get { return _jobid; }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string Department
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 账号编号
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 性别(0未知，1代表女，2男)
        /// </summary>
        public string Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile
        {
            set { _mobile = value; }
            get { return _mobile; }
        }
        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string qq
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BirthDay
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 特长
        /// </summary>
        public string Spetialty
        {
            set { _spetialty = value; }
            get { return _spetialty; }
        }
        /// <summary>
        /// 爱好
        /// </summary>
        public string Hobby
        {
            set { _hobby = value; }
            get { return _hobby; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 是否删除
        /// </summary>
        public string IsDelete
        {
            set;
            get;
        }
        /// <summary>
        /// 组团用户账户信息
        /// </summary>
        public EyouSoft.Model.CompanyStructure.UserAccount UserAccount
        {
            get;
            set;
        }

        public string AreaIds
        {
            get { return _AreaIds; }
            set { _AreaIds = value; }
        }

        public string AreaNames
        {
            get { return _AreaNames; }
            set { _AreaNames = value; }
        }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        #endregion Model

    }
}


