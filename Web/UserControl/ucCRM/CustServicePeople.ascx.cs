using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
using System.ComponentModel;

namespace Web.UserControl.ucCRM
{   
    /// <summary>
    /// 客户服务质量管理中的回访投诉填写人的信息用户控件
    /// xuty 2011/1/19
    /// 修改：2011.6.27
    /// 修改人:田想兵
    /// 修改内容：回访人必填判断
    /// </summary>
    public partial class CustServicePeople : System.Web.UI.UserControl
    {
        protected string ByVisisterCompanyMess = "被访客户不为空";
        protected string VisiterNameMess = "回访人不为空";

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 回访人是否必录 
        /// by 田想兵 2011.6.27
        /// </summary>
        public bool isRequiredVisiterName
        {
            set;
            get;
        }
        /// <summary>
        /// 被访客户(投诉客户)Id
        /// </summary>
        public string ByVisisterCompanyId
        {
            get
            {
                return Utils.InputText(this.txtByVisitCompanyId.Value);
            }
            set
            {
                this.txtByVisitCompanyId.Value = value;
            }
        }
        /// <summary>
        /// 被访客户(投诉客户)名
        /// </summary>
        public string ByVisisterCompany
        {
            get
            {
                return Utils.InputText(this.txtByVisitCompany.Value);
            }
            set
            {
                this.txtByVisitCompany.Value = value;
            }
        }
        //被访人(投诉人)姓名
        public string ByVisisterName
        {
            get
            {
                return Utils.InputText(this.txtByVisiter.Value);
            }
            set
            {
                this.txtByVisiter.Value = value;
            }
        }
        /// <summary>
        /// 回访人(接待人)姓名
        /// </summary>
        public string VisiterName
        {
            get
            {
                return Utils.InputText(this.txtVisiter.Value);
            }
            set
            {
                this.txtVisiter.Value = value;
            }
        }
        
        /// <summary>
        /// 回访时间(投诉时间)
        /// </summary>
        public DateTime VisistDate
        {
            get
            {
                return Utils.GetDateTime(this.txtVisitDate.Value,DateTime.Now);
            }
            set
            {
                this.txtVisitDate.Value = value.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 是否为回访(用来设置显示标题)
        /// </summary>
        public bool IsVisist
        {
            get
            {
              
                return _isVisit;
            }
            set
            {
                _isVisit = value;
                if (!value)
                {
                    ByVisisterCompanyMess = "投诉客户不为空";
                    VisiterNameMess = "接待人不为空";
                }
            }
        }
        private bool _isVisit;


        protected override void OnPreRender(EventArgs e)
        {
            if (!isRequiredVisiterName)
            {
                txtByVisiter.Attributes.Remove("valid");
            }
            base.OnPreRender(e);
        }
    }
}