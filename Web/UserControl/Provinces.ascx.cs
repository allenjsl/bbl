using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common.Function;
using EyouSoft.Common;
using System.Text;
namespace Web.UserControl
{
    public partial class Provinces : System.Web.UI.UserControl
    {
        protected string _IsIndex = "";
        public string IsIndex
        {
            get { return _IsIndex; }
            set { _IsIndex = value; }
        }

        protected string provinceHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder strBuilder = new StringBuilder();
            EyouSoft.BLL.CompanyStructure.Province pBll = new EyouSoft.BLL.CompanyStructure.Province();
            IList<EyouSoft.Model.CompanyStructure.Province> list = pBll.GetList(CompanyId);
            strBuilder.AppendFormat("<option value=\"0\">请选择</option>");
            if (list != null && list.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.Province p in list)
                {
                    if (p.Id == ProvinceId)
                    {
                        strBuilder.AppendFormat("<option value=\"{0}\" selected=\"selected\">{1}</option>", p.Id, p.ProvinceName);
                        continue;
                    }
                    strBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", p.Id, p.ProvinceName);
                }
            }
            provinceHtml = strBuilder.ToString();
        }

        private int provinceId;
        //获取或设置省份ID
        public int ProvinceId
        {
            get
            {
                if (isUpdate)
                {
                    return provinceId;
                }
                else
                {
                    return Utils.GetInt(Utils.GetFormValue("uc_provinceDynamic" + IsIndex));
                }
            }
            set
            {
                isUpdate = true;
                provinceId = value;
            }
        }

        private bool isUpdate { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyId { get; set; }

        public bool? IsFav { get; set; }

       



    }

}