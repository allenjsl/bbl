using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using EyouSoft.Common.Function;
using EyouSoft.Common;

namespace Web.UserControl
{
    public partial class Citys : System.Web.UI.UserControl
    {
        protected string cityHtml = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("<option value=\"0\">请选择</option>");
            if (ProvinceId != 0)
            {
                EyouSoft.BLL.CompanyStructure.City cBll = new EyouSoft.BLL.CompanyStructure.City();
                IList<EyouSoft.Model.CompanyStructure.City> list = cBll.GetList(CompanyId, ProvinceId, IsFav);

                if (list != null && list.Count > 0)
                {
                    foreach (EyouSoft.Model.CompanyStructure.City c in list)
                    {
                        if (c.Id == CityId)
                        {
                            strBuilder.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", c.Id, c.CityName);
                            continue;
                        }
                        strBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", c.Id, c.CityName);
                    }
                }
            }
            cityHtml = strBuilder.ToString();
        }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }

        //获取或设置城市ID
        private int cityId;
        public int CityId
        {
            get
            {
                if (isUpdate)
                {
                    return cityId;
                }
                else
                {
                    return Utils.GetInt(Utils.GetFormValue("uc_cityDynamic" + IsIndex + ""));
                }
            }
            set
            {
                isUpdate = true;
                cityId = value;
            }

        }

        private int provinceId;
        //获取或设置省份ID
        public int ProvinceId
        {
            get
            {
                if (isUpdateP)
                {
                    return provinceId;
                }
                else
                {
                    return Utils.GetInt(Utils.GetFormValue("uc_province123"));
                }
            }
            set
            {
                isUpdateP = true;
                provinceId = value;
            }
        }

        private bool isUpdate { get; set; }
        private bool isUpdateP { get; set; }
        /// <summary>
        /// 是否为常用城市
        /// </summary>
        public bool? IsFav { get; set; }

        private string _IsIndex = "";
        public string IsIndex
        {
            get { return _IsIndex; }
            set { _IsIndex = value; }
        }


    }
}