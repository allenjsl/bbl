using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;

namespace Web.systemset.ToGoTerrace
{
    /// <summary>
    /// 友情链接-添加修改页面
    /// </summary>
    /// 柴逸宁
    /// 2011-4-8
    public partial class FriendshipLinkAdd : Eyousoft.Common.Page.BackPage
    {
        protected int id = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_同行平台栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_同行平台栏目, false);
                return;
            }

            if (!IsPostBack)
            {
                //初始化ssBLL
                EyouSoft.BLL.SiteStructure.SiteFriendLink ssBLL = new EyouSoft.BLL.SiteStructure.SiteFriendLink();
                //初始化Model
                EyouSoft.Model.SiteStructure.SiteFriendLink ssModel = new EyouSoft.Model.SiteStructure.SiteFriendLink();
                //////////////修改时加载原有数据
                if (Utils.GetQueryStringValue("type") == "modify")
                {
                    //获取id
                    id = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                    //获取Model
                    ssModel = ssBLL.GetSiteFriendLink(id, SiteUserInfo.CompanyID);
                    //友情链接文字
                    txt_LinkName.Text = ssModel.LinkName;
                    //友情连接URL
                    txt_linkURL.Text = ssModel.LinkUrl;
                }
                ////////////////////////////////
            }
        }

        protected void linkbtnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            //初始化Model
            EyouSoft.Model.SiteStructure.SiteFriendLink ssModel = new EyouSoft.Model.SiteStructure.SiteFriendLink();
            //初始化BLL
            EyouSoft.BLL.SiteStructure.SiteFriendLink ssBLL = new EyouSoft.BLL.SiteStructure.SiteFriendLink();
            //判断操作
            if (Utils.GetQueryStringValue("type") == "modify")//修改操作
            {
                //获取id
                id = Utils.GetInt(Utils.GetQueryStringValue("tid"));
                //获取Model
                ssModel = ssBLL.GetSiteFriendLink(id, SiteUserInfo.CompanyID);
            }
            else
            {
                //友情链接添加时间
                ssModel.CreateTime = DateTime.Now;
            }
           //公司编号
            ssModel.CompanyId = SiteUserInfo.CompanyID;
            //判断友情链接文字是否为空
            if (txt_LinkName.Text.Trim() == "")
            {
                MessageBox.ResponseScript(this, ";alert('友情链接文字不能为空!');");
            }
            //友情链接文字赋值
            ssModel.LinkName = Utils.EditInputText(txt_LinkName.Text);
            //友情链接URL
            ssModel.LinkUrl = Utils.EditInputText(txt_linkURL.Text);
            //友情链接类型
            ssModel.LinkType = EyouSoft.Model.EnumType.SiteStructure.LinkType.文字;
            //数据 保存是否成功，默认保存失败
            bool res = false;
            //判断添加或修改
            if (id > 0)
            {
                res = ssBLL.UpdateFriendLink(ssModel);

            }
            else
            {
                res = ssBLL.AddFriendLink(ssModel);
            }
            //判断添加或修改成功否
            if (res)
            {
                /////////////////////////保存成功

                MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", "保存成功!", Utils.GetQueryStringValue("iframeId"), id > 0 ? "window.parent.location.reload();" : "window.parent.location.href='/systemset/ToGoTerrace/FriendshipLink.aspx';"));

                ////////////////////////////////////////////
            }
            else
            {
                MessageBox.ResponseScript(this, ";alert('保存失败!');");
            }
        }
    }
}
