using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.UserControl
{
    /// <summary>
    /// 团队计划地接社信息
    /// 功能：地接社信息
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-14
    /// </summary>
    public partial class DiJieControl : System.Web.UI.UserControl
    {
        //设置list
        private IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> _setList;

        public IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> SetList
        {
            get { return _setList; }
            set { _setList = value; }
        }
        //获得list
        private IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> _getList;

        public IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> GetList
        {
            get { return _getList; }
            set { _getList = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                GetDataList();
                base.OnLoad(e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetDataList();
            base.OnPreRender(e);
        }

        #region 为GetList属性 赋值
        /// <summary>
        /// 获得页面数据
        /// </summary>
        protected void GetDataList()
        {
            #region 地接信息
            string[] DjIDArray = Utils.GetFormValues("hideDjId");
            string[] DjNameArray = Utils.GetFormValues("txtDjName");
            string[] licenseArray = Utils.GetFormValues("txtLicense");
            string[] phoneArray = Utils.GetFormValues("txtPhone");
            string[] contactArray = Utils.GetFormValues("txtContact");

            if (DjNameArray != null && licenseArray != null && phoneArray != null && contactArray!=null)
            {
                if (DjNameArray.Count() == licenseArray.Count() && licenseArray.Count() == phoneArray.Count() && phoneArray.Count() > 0 && contactArray.Count() == DjNameArray.Count())
                {
                    IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> list = new List<EyouSoft.Model.TourStructure.TourLocalAgencyInfo>();
                    for (int i = 0; i < DjNameArray.Count(); i++)
                    {
                        if (DjNameArray[i].Trim() != "")
                        {
                            EyouSoft.Model.TourStructure.TourLocalAgencyInfo model = new EyouSoft.Model.TourStructure.TourLocalAgencyInfo();
                            model.AgencyId = Utils.GetInt(DjIDArray[i]);
                            model.Name = DjNameArray[i];
                            model.LicenseNo = licenseArray[i];
                            model.Telephone = phoneArray[i];
                            model.ContacterName = contactArray[i];
                            list.Add(model);
                        }
                    }
                    this.GetList = list;
                }
            }

            #endregion
        }
        #endregion

        #region 页面控件赋值
        /// <summary>
        /// 设置rpeate控件数据
        /// </summary>
        protected void SetDataList()
        {
            if (this.SetList != null && this.SetList.Count > 0)
            {
                this.rptList.DataSource = this.SetList;
                this.rptList.DataBind();
            }
        }
        #endregion

    }
}