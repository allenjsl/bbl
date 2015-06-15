using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 上团报价新增修改页
    /// 功能：新增，修改上团报价
    /// 创建人：戴银柱
    /// 创建时间： 2011-03-21 
    /// </summary>
    public partial class TourQuoteAdd : BackPage
    {
        //声明上传报价bll对象
        private EyouSoft.BLL.TourStructure.QuoteAttach bll = new EyouSoft.BLL.TourStructure.QuoteAttach();
        protected void Page_Load(object sender, EventArgs e)
        {
            //上传报价权限判断
            if (!CheckGrant(TravelPermission.团队计划_上传报价_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.团队计划_上传报价_栏目, true);
            }

            if (!IsPostBack)
            {
                string type = Utils.GetQueryStringValue("type");
                if (type == "update")
                {
                    int id = Utils.GetInt(Utils.GetQueryStringValue("id"));
                    DataInit(id);
                }
                else
                {
                    this.pnlFile.Visible = false;
                    //发布时间默认当前时间
                    this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //标题
            string txtTitle = Utils.GetFormValue(this.txtTitle.UniqueID);
            //添加日期
            string txtPeriod = Utils.GetFormValue(this.txtPeriod.UniqueID);
            //有效期 开始
            DateTime? validityBegin = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtValidityBegin.UniqueID));
            //有效期 结束
            DateTime? validityEnd = Utils.GetDateTimeNullable(Utils.GetFormValue(this.txtValidityEnd.UniqueID));

            #region 验证上传文件的格式是否正确
            string msg = "";
            if (!EyouSoft.Common.Function.UploadFile.CheckFileType(Request.Files, "fileField", new[] { ".gif", ".jpeg", ".jpg", ".png", ".xls", ".doc", ".docx", ".rar", ".txt" }, null, out msg))
            {
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('" + msg + "');");
                return;
            }
            #endregion

            #region 上传单文件
            //文件路径
            string filePath = "";
            //文件名
            string fileName = "";
            //文件上传
            if (!EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files["fileField"], "TeamPlanFile", out filePath, out fileName))
            {
                //上传失败提示
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('保存失败!');");
                return;
            }
            #endregion

            //声明对象
            EyouSoft.Model.TourStructure.QuoteAttach model = new EyouSoft.Model.TourStructure.QuoteAttach();
            model.CompanyId = SiteUserInfo.CompanyID;
            model.FileName = txtTitle;
            model.OperatorId = Utils.GetInt(this.selectOperator1.OperId);
            model.OperatorName = this.selectOperator1.OperName;
            model.ValidityStart = validityBegin;
            model.ValidityEnd = validityEnd;
            model.AddTime = Utils.GetDateTime(txtPeriod);
            if (filePath.Trim() != "")
            {
                model.FilePath = filePath;
            }
            else
            {
                model.FilePath = Utils.GetFormValue(this.hideFilePath.UniqueID);
            }

            //判断是否为修改
            if (Utils.GetQueryStringValue("type") == "update")
            {
                model.Id = Utils.GetInt(Utils.GetQueryStringValue("id"));
                bll.UpdateQuote(model);
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('修改成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }
            else
            {
                //新增加
                model.AddTime = DateTime.Now;
                bll.AddQuote(model);
                EyouSoft.Common.Function.MessageBox.ResponseScript(this, "javascript:alert('添加成功!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.window.location.reload();");
            }

        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="id">编号ID</param>
        private void DataInit(int id)
        {
            //声明上传报价实体
            EyouSoft.Model.TourStructure.QuoteAttach model = bll.GetQuoteInfo(id);
            if (model != null)
            {
                //标题
                this.txtTitle.Text = model.FileName;
                //添加日期
                this.txtPeriod.Text = model.AddTime==null?"":Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd");
                //开始有效期
                this.txtValidityBegin.Text = model.ValidityStart == null ? "" : Convert.ToDateTime(model.ValidityStart).ToString("yyyy-MM-dd");
                //结束有效期
                this.txtValidityEnd.Text = model.ValidityEnd == null ? "" : Convert.ToDateTime(model.ValidityEnd).ToString("yyyy-MM-dd");
                this.selectOperator1.OperId = model.OperatorId.ToString();
                this.selectOperator1.OperName = model.OperatorName;

                if (model.FilePath.Trim() != "")
                {
                    this.lblFileName.Text = "<a href=\"" + model.FilePath + "\" target=\"_blank\">查看报价单</a>";
                    this.hideFilePath.Value = model.FilePath;
                    this.pnlFile.Visible = true;
                }
                else
                {
                    this.pnlFile.Visible = false;
                }
            }
            else
            {
                this.pnlFile.Visible = false;
            }
        }
    }
}
