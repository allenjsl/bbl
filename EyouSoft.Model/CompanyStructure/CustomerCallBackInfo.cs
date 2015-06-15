using System;
using System.Collections.Generic;
namespace EyouSoft.Model.CompanyStructure
{
	/// <summary>
    /// 实体类CustomerCallBack ,客户回访
    /// autor:李焕超
    /// date:2011-1-17
	/// </summary>
	[Serializable]
	public class CustomerCallBackInfo
	{
		public CustomerCallBackInfo()
		{}
		#region Model
		private int _id;
		private int _customerid;
		private string _customername;
		private string _customeruser;
		private int _callbackerid;
		private string _callbacker;
		private DateTime _time;
		private decimal _result;
		private string _remark;
		private EyouSoft.Model.EnumType.CompanyStructure.CallBackType _iscallback;
		private int _companyid;

		/// <summary>
		/// 编号
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 被访客户Id/投诉客户ID
		/// </summary>
		public int CustomerId
		{
			set{ _customerid=value;}
			get{return _customerid;}
		}
		/// <summary>
		/// 被访客户/投诉客户
		/// </summary>
		public string CustomerName
		{
			set{ _customername=value;}
			get{return _customername;}
		}
		/// <summary>
		/// 被访人/投诉人
		/// </summary>
		public string CustomerUser
		{
			set{ _customeruser=value;}
			get{return _customeruser;}
		}
		/// <summary>
		/// 回访人Id/接待投诉人ID
		/// </summary>
		public int CallBackerId
		{
			set{ _callbackerid=value;}
			get{return _callbackerid;}
		}
		/// <summary>
        /// 回访人/接待投诉人
		/// </summary>
		public string CallBacker
		{
			set{ _callbacker=value;}
			get{return _callbacker;}
		}
		/// <summary>
		/// 回访时间/投诉时间
		/// </summary>
		public DateTime Time
		{
			set{ _time=value;}
			get{return _time;}
		}
		/// <summary>
		/// 回访满意度
		/// </summary>
		public decimal Result
		{
			set{ _result=value;}
			get{return _result;}
		}
		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 判断回访与投诉
		/// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CallBackType IsCallBack
		{
			set{ _iscallback=value;}
			get{return _iscallback;}
		}
		/// <summary>
		/// 公司编号
		/// </summary>
		public int CompanyId
		{
			set{ _companyid=value;}
			get{return _companyid;}
		}
        /// <summary>
        /// 回访结果
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> CustomerCallBackResultInfoList
        {
            set;
            get;
        }
		#endregion Model

	}
}

