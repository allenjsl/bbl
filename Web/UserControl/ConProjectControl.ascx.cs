using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.UserControl
{
    public partial class ConProjectControl : System.Web.UI.UserControl
    {
        //设置list
        private IList<EyouSoft.Model.TourStructure.TourServiceInfo> _setList;

        public IList<EyouSoft.Model.TourStructure.TourServiceInfo> SetList
        {
            get { return _setList; }
            set { _setList = value; }
        }
        //获得list
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
                SetDataList();
            }

        }

        #region 为GetList属性 赋值
        /// <summary>
        /// 获得页面数据
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourServiceInfo> GetDataList()
        {
            IList<EyouSoft.Model.TourStructure.TourServiceInfo> list = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
            #region 包含项目
            //项目
            string[] sltProArray = Utils.GetFormValues("selectPro");
            //接待标准
            string[] StandardArray = Utils.GetFormValues("txtStandard");

            if (sltProArray != null && StandardArray != null)
            {
                if (sltProArray.Count() == StandardArray.Count() && sltProArray.Count() > 0)
                {
                    for (int i = 0; i < sltProArray.Count(); i++)
                    {
                         EyouSoft.Model.TourStructure.TourServiceInfo model=new EyouSoft.Model.TourStructure.TourServiceInfo();
                         
                         model.Service = Utils.GetFormValues("txtStandard")[i];
                         model.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Utils.GetInt(Utils.GetFormValues("selectPro")[i]);
                         list.Add(model);
                    }
                }
            }

            return list;
            #endregion
        }
        #endregion

        #region 页面控件赋值
        /// <summary>
        /// 设置rpeate控件数据
        /// </summary>
        public void SetDataList()
        {
            IList<EyouSoft.Model.TourStructure.TourServiceInfo> list = SetList;
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

            if (list != null && list.Count > 0)
            {
                this.hideConProject.Value = list.Count.ToString();
                this.rptList.DataSource = list;
                this.rptList.DataBind();
            }
        }
        #endregion

    }
}