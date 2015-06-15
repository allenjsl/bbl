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
    /// 包含项目
    /// 功能：价格组成
    /// 创建人:李晓欢
    /// 创建时间： 2011-01-25
    /// </summary>
    public partial class ProjectControl : System.Web.UI.UserControl
    {
        //设置包含项目list
        private IList<EyouSoft.Model.TourStructure.TourServiceInfo> _setList;

        public IList<EyouSoft.Model.TourStructure.TourServiceInfo> SetList
        {
            get { return _setList; }
            set { _setList = value; }
        }

        //获得包含项目list
        private IList<EyouSoft.Model.TourStructure.TourServiceInfo> _getList;

        public IList<EyouSoft.Model.TourStructure.TourServiceInfo> GetList
        {
            get { return _getList; }
            set { _getList = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //SetDataList();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            // GetDataList();
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            // SetDataList();
            base.OnPreRender(e);
        }

        #region 为GetList属性 赋值
        /// <summary>
        /// 获得页面数据
        /// </summary>
        protected void GetDataList()
        {
            string[] sltProjectArray = Utils.GetFormValues("ddl_Project");
            string[] ProjectArray = Utils.GetFormValues("Txt_XianlProject");

            if (sltProjectArray == null && sltProjectArray.Count() <= 0)
            {
                Response.Write("<script>javascript:window.alert('请选择包含项目!')</script>");
                return;
            }
            if (sltProjectArray != null && ProjectArray != null)
            {
                if (sltProjectArray.Count() == ProjectArray.Count() && sltProjectArray.Count() > 0)
                {
                    IList<EyouSoft.Model.TourStructure.TourServiceInfo> TourServiceInfoList =new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
                    for (int i = 0; i < sltProjectArray.Count(); i++)
                    {
                        EyouSoft.Model.TourStructure.TourServiceInfo ModelTourServiceInfo = new EyouSoft.Model.TourStructure.TourServiceInfo();
                        ModelTourServiceInfo.Service = ProjectArray[i];
                        ModelTourServiceInfo.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), sltProjectArray[i]);
                        TourServiceInfoList.Add(ModelTourServiceInfo);
                    }
                    this.GetList = TourServiceInfoList;
                }
            }
        }
        #endregion

        #region 页面控件赋值
        /// <summary>
        /// 设置rpeate控件数据
        /// </summary>
        protected void SetDataList()
        {
            IList<EyouSoft.Model.TourStructure.TourServiceInfo> list = this.SetList;
            string project = "";
            project += "{value:\"0\",text:\"地接\"}|";
            project += "{value:\"1\",text:\"酒店\"}|";
            project += "{value:\"2\",text:\"用餐\"}|";
            project += "{value:\"3\",text:\"景点\"}|";
            project += "{value:\"4\",text:\"保险\"}|";
            project += "{value:\"5\",text:\"大交通\"}|";
            project += "{value:\"6\",text:\"导服\"}|";
            project += "{value:\"7\",text:\"购物\"}|";
            project += "{value:\"8\",text:\"小交通\"}|";
            project += "{value:\"9\",text:\"其它\"}";
            this.hideProList.Value = project;

            if (this.SetList != null && this.SetList.Count > 0)
            {
                this.hidePrice.Value = this.SetList.Count.ToString();
                this.repProject.DataSource = this.SetList;
                this.repProject.DataBind();
            }
        }
        #endregion
    }
}