/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：selectXianlu.ascx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\UserControl\selectXianlu.ascx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:16:02
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using EyouSoft.SSOComponent.Entity;

namespace Web.UserControl
{
    public partial class selectXianlu : System.Web.UI.UserControl
    {

        public string Url
        {
            set;
            get;
        }
        public int userId
        { set; get; }
        public int curCompany { get; set; }

        public StringBuilder sb=new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);

            //if (!IsPostBack)
            {
                var re = new Regex("xlid" + @"\=(\d*)");
                if (Url == null)
                {
                    Url = Request.Path.ToString();
                }      
                EyouSoft.BLL.CompanyStructure.Area bll = new EyouSoft.BLL.CompanyStructure.Area(userInfo);
                //int count = 0;
                IList<EyouSoft.Model.CompanyStructure.Area> list = bll.GetAreaList(userInfo.ID);
                if(list!=null)

                if (EyouSoft.Common.Utils.GetQueryStringValue("xlid") != "")
                {
                    sb.Append("<li><nobr><img src=\"/images/icon002.gif\"> <a href=\"" + re.Replace(Url, "") + "\">所有线路</a></nobr></li>");
                }
                else
                {
                    sb.Append("<li><nobr><img src=\"/images/icon002.gif\"> <a style=\"color:red\" href=\"" + re.Replace(Url, "") + "\">所有线路</a></nobr></li>");
                }
                for (int i = 0; i < list.Count; i++)
                {
                    string u = "";
                    if (Url != null)
                    {
                        if (Url.Contains("?"))
                        {
                            if (re.IsMatch(Url))
                                u = re.Replace(Url, "xlid=" + list[i].Id.ToString());
                            else
                            {
                                u = Url + "&xlid=" + list[i].Id.ToString();
                            }
                        }
                        else
                        {
                            u = Url + "?xlid=" + list[i].Id.ToString();
                        }
                    }
                    if (EyouSoft.Common.Utils.GetQueryStringValue("xlid") != "")
                    {
                        if (EyouSoft.Common.Utils.GetQueryStringValue("xlid") == list[i].Id.ToString())
                        {
                            sb.Append("<li><nobr><img src=\"/images/icon002.gif\"> <a href=\"" + u + "\" style=\"color:red\">" + list[i].AreaName + "</a></nobr></li>");
                        }
                        else
                        {
                            sb.Append("<li><nobr><img src=\"/images/icon002.gif\"> <a href=\"" + u + "\">" + list[i].AreaName + "</a></nobr></li>");
                        }
                    }
                    else
                    {
                        sb.Append("<li><nobr><img src=\"/images/icon002.gif\"> <a href=\"" + u + "\">" + list[i].AreaName + "</a></nobr></li>");
                    }
                    
                }
            }
        }

        void lbtn_allXL_Click(object sender, EventArgs e)
        {
            string u = Request.Url.ToString();
            var re = new Regex("xlid" + @"\=(\d+)");
            u = re.Replace(u,"");
            Response.Redirect(u);
        }
    }
}