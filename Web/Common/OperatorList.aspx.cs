using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Collections.Generic;

namespace Web.Common
{
    /// <summary>
    /// 选择操作员页面
    /// 修改记录：
    /// 1、2011-01-20 曹胡生 创建
    /// </summary>
    public partial class OperatorList : BackPage
    {
        protected string strHtml = "";
        protected string strSelectedPeopleHtml = "";

        #region 分页参数
        protected int pageSize = 40;
        protected int pageIndex = 1;
        int recordCount = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                onInit();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        private void onInit()
        {
            string name = Utils.GetQueryStringValue("OperName");
            string departID = Utils.GetQueryStringValue("departID");
            string departName = Utils.GetQueryStringValue("departName");
            string topId = Utils.GetQueryStringValue("topIID");
            string iframeId = Utils.GetQueryStringValue("iframeId");
            string txtname = Utils.GetQueryStringValue("txtname");
            string lblID = Utils.GetQueryStringValue("lblID");
            string hdid = Utils.GetQueryStringValue("hdid");
            string selectedPeopleIdList = GetSelectedIdList();
            this.UCSelectDepartment1.GetDepartmentName = departName;
            this.UCSelectDepartment1.GetDepartId = departID;
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("page"),1);//分页索引
            EyouSoft.BLL.CompanyStructure.CompanyUser bll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);

            //初始化已选择人员列表
            if (selectedPeopleIdList != "")
            {
                System.Text.StringBuilder str1 = new System.Text.StringBuilder();
                IList<EyouSoft.Model.CompanyStructure.CompanyUser> selectedList = 
                    new List<EyouSoft.Model.CompanyStructure.CompanyUser>();
                EyouSoft.Model.CompanyStructure.CompanyUser companyUser = null;
                string[] idArr = selectedPeopleIdList.Split(',');
                int id = 0;
                for (int i = 0; i < idArr.Length; i++)
                {
                    if (Int32.TryParse(idArr[i], out id))
                    {
                        companyUser = bll.GetUserInfo(id);
                        if (companyUser != null)
                        {
                            selectedList.Add(companyUser);
                        }
                    }
                }

                if (selectedList.Count > 0)
                {
                    int k = 1;
                    //循环列表，输出
                    foreach (EyouSoft.Model.CompanyStructure.CompanyUser model in selectedList)
                    {
                        //判断是不是一行的第一个
                        if (k % 5 == 1)//是
                        {
                            //输出TR BEGIN TAG
                            str1.Append("<tr class=\"odd\">");
                        }
                        str1.Append("<td height=\"28\" bgcolor=\"#e3f1fc\" width=\"20%\" class=\"pandl3\">");
                        str1.AppendFormat("<input checked type=\"checkbox\" id=\"{0}\" value=\"{0}\">", model.ID);
                        str1.AppendFormat("<label id=\"for_{0}\" for=\"{0}\">{1}</label>", model.ID, model.PersonInfo.ContactName);
                        str1.Append("</td>");

                        //判断是不是一行的第五个并且不是最后一个
                        if (k % 5 == 0 && k <= selectedList.Count)//是一行的第五个
                        {
                            str1.Append("</tr>");
                        }
                        else if (k == selectedList.Count)
                        {
                            int i = k % 5;
                            //计算当前是第几个TD,然后往后补空。
                            for (++i; i <= 5; i++)
                            {
                                str1.Append("<td height=\"28\" bgcolor=\"#e3f1fc\" width=\"20%\" class=\"pandl3\">");
                                str1.Append("&nbsp;");
                                str1.Append("</td>");
                            }
                            str1.Append("</tr>");
                        }
                        k++;
                    }
                    //str.Append("</tr>");
                    strSelectedPeopleHtml = str1.ToString();
                }
            }

            //初始化人员列表
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            System.Collections.Generic.IList<EyouSoft.Model.CompanyStructure.CompanyUser> Ilist = null;
            if (!string.IsNullOrEmpty(departID) || !string.IsNullOrEmpty(name))
            {
                Ilist = bll.GetListByContactName(pageSize, pageIndex, ref recordCount, Utils.GetInt(departID,-1), name);
            }
            else
            {
                Ilist = bll.GetUserInfo(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount);
            }
            selectedPeopleIdList = ","+selectedPeopleIdList+",";
            bool isSelected = false;
            if (Ilist != null && Ilist.Count > 0)
            {
                int k = 1;
                //循环列表，输出
                foreach (EyouSoft.Model.CompanyStructure.CompanyUser model in Ilist)
                {
                    //判断是不是一行的第一个
                    if (k % 5 == 1)//是
                    {
                        //输出TR BEGIN TAG
                        str.Append("<tr class=\"odd\">");
                    }

                    isSelected = false;
                    if (selectedPeopleIdList.IndexOf(","+model.ID.ToString()+",") != -1)
                    {
                        isSelected = true;
                    }
                    str.Append("<td height=\"28\" bgcolor=\"#e3f1fc\" width=\"20%\" class=\"pandl3\">");
                    str.AppendFormat("<input "+ (isSelected?"checked":"") +" type=\"checkbox\" id=\"{0}\" value=\"{0}\">", model.ID);
                    str.AppendFormat("<label id=\"for_{0}\" for=\"{0}\">{1}</label>", model.ID, model.PersonInfo.ContactName);
                    str.Append("</td>");

                    //判断是不是一行的第五个并且不是最后一个
                    if (k % 5 == 0 && k <=Ilist.Count)//是一行的第五个
                    {
                        str.Append("</tr>");
                    }
                    else if (k == Ilist.Count)
                    {
                        int i = k % 5;
                        //计算当前是第几个TD,然后往后补空。
                        for (++i; i <= 5; i++)
                        {
                            str.Append("<td height=\"28\" bgcolor=\"#e3f1fc\" width=\"20%\" class=\"pandl3\">");
                            str.Append("&nbsp;");
                            str.Append("</td>");
                        }
                        str.Append("</tr>");
                    }
                    k++;
                }
                //str.Append("</tr>");
                strHtml = str.ToString();
            }
            str = null;
            bll = null;
            Ilist = null;

            //分页控件初始化
            BindPage(name, departID, topId, iframeId, txtname, lblID, hdid);
        }

        /// <summary>
        /// 分页控件绑定
        /// </summary>
        protected void BindPage(string name, string depart, string topID, string iframeId, string txtname, string lblID, string hdid)
        {
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
            this.ExporPageInfoSelect1.IsUrlRewrite = false;
            this.ExporPageInfoSelect1.LinkType = 3;
            this.ExporPageInfoSelect1.PageLinkURL = string.Format("OperatorList.aspx?OperName={0}&topIID={1}&iframeId={2}&txtname={3}&hdid={4}&lblID={5}&", name, topID, iframeId, txtname, hdid, lblID);
        }

        private string GetSelectedIdList()
        {
            string key1 = "selectedidlist";
            string key2 = "nowselected";

            string idList = "";

            if (Request.QueryString[key2] != null)
            {
                idList = Utils.GetQueryStringValue(key2);
            }
            else if (Request.QueryString[key1] != null)
            {
                idList = Utils.GetQueryStringValue(key1);
            }

            return idList;
        }
    }
}
