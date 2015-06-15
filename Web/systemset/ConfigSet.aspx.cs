using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using EyouSoft.Common.Function;
namespace Web.systemset
{
    /// <summary>
    /// 系统配置
    /// xuty 2011/1/17
    /// </summary>
    public partial class ConfigSet : Eyousoft.Common.Page.BackPage
    {
        protected string pageHeader = string.Empty;//页眉
        protected string pageFooter = string.Empty;//页脚
        protected string pageModel = string.Empty;//模板
        protected string departSeal = string.Empty;//部门公章
        protected string companyLog = string.Empty;//公司log
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.系统设置_系统配置_系统设置栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.系统设置_系统配置_系统设置栏目, true);
                return;
            }
            string method = Utils.GetFormValue("hidMethod");
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = null;//配置实体
            if (method == "save")
            {
                //保存
                set = new EyouSoft.Model.CompanyStructure.CompanyFieldSetting();
                string fileName1 = string.Empty;

                string oldName = string.Empty;
                bool result = true;
                HttpPostedFile fLog = Request.Files["fileLog"];
                HttpPostedFile fHeader = Request.Files["fileHeader"];
                HttpPostedFile fFooter = Request.Files["fileFooter"];
                HttpPostedFile fSeal = Request.Files["fileSeal"];
                HttpPostedFile fModel = Request.Files["fileModel"];
                if (fLog != null && !string.IsNullOrEmpty(fLog.FileName))
                {
                    result = UploadFile.FileUpLoad(fLog, "systemset", out fileName1, out oldName);//上传logo
                    set.CompanyLogo = fileName1;
                }
                else
                {
                    set.CompanyLogo = hidLog.Value;
                }
                set.CompanyPrintFile = new EyouSoft.Model.CompanyStructure.CompanyPrintTemplate();

                if (fHeader != null && !string.IsNullOrEmpty(fHeader.FileName))
                {
                    result = UploadFile.FileUpLoad(fHeader, "systemset", out fileName1, out oldName);//上传页眉
                    set.CompanyPrintFile.PageHeadFile = fileName1;
                }
                else
                {
                    set.CompanyPrintFile.PageHeadFile = hidHeader.Value;
                }
                if (result && (fFooter != null && !string.IsNullOrEmpty(fFooter.FileName)))
                {
                    result = UploadFile.FileUpLoad(fFooter, "systemset", out fileName1, out oldName);//上传页脚
                    set.CompanyPrintFile.PageFootFile = fileName1;
                }
                else
                {
                    set.CompanyPrintFile.PageFootFile = hidFooter.Value;
                }
                if (result && (fModel != null && !string.IsNullOrEmpty(fModel.FileName)))
                {
                    result = UploadFile.FileUpLoad(fModel, "systemset", out fileName1, out oldName);//上传模板
                    set.CompanyPrintFile.TemplateFile = fileName1;
                }
                else
                {
                    set.CompanyPrintFile.TemplateFile = hidModel.Value;
                }
                if (result && (fSeal != null && !string.IsNullOrEmpty(fSeal.FileName)))
                {
                    result = UploadFile.FileUpLoad(fSeal, "systemset", out fileName1, out oldName);//上传公章
                    set.CompanyPrintFile.DepartStamp = fileName1;
                }
                else
                {
                    set.CompanyPrintFile.DepartStamp = hidSeat.Value;
                }
                if (result)
                {
                    //留位时间
                    set.ReservationTime = Utils.GetInt(txtRetainHour.Value) * 60;
                    //价格组成
                    set.PriceComponent = rdiPrice.SelectedValue == "1" ? EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价 : EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.统一报价;
                    set.DisplayAfterMonth = Utils.GetInt(txtAfterMonth.Value);//列表显示控制后几月
                    set.DisplayBeforeMonth = Utils.GetInt(txtBeforeMonth.Value);//列表显示控制前几月
                    set.SongTuanRenId = GroupSender.OperId;//送团人编号
                    set.SongTuanRenName = GroupSender.OperName;//送团人姓名
                    set.JiHeDiDian = GroupSet.Value;//集合地点
                    set.JiHeBiaoZhi = SetMark.Value;//集合标志
                    set.CompanyId = CurrentUserCompanyID;
                    set.ReceiptRemindType = (EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType)(int.Parse(rbl_ReceiptRemind.SelectedValue));//收款提醒
                    set.TanChuangTiXingInterval = Utils.GetInt(Utils.GetFormValue(txtTanChuangTiXingInterval.UniqueID), 1) * 60;
                    result = setBll.SetCompanySetting(set);
                }
                if (set != null && set.CompanyPrintFile != null)
                {
                    SetPrintPic(set);//更新模板显示
                }
                MessageBox.ShowAndRedirect(this, result ? "设置成功！" : "设置失败！", "/systemset/ConfigSet.aspx");
                return;
            }
            set = setBll.GetSetting(CurrentUserCompanyID);//获得配置信息
            if (set != null)
            {
                //初始化配置信息
                txtRetainHour.Value = (set.ReservationTime / 60.0).ToString("F1");//最长留位小时
                txtRetainMin.Value = set.ReservationTime.ToString();//最长留位分钟
                rdiPrice.SelectedValue = ((int)set.PriceComponent).ToString();//价格组成
                txtBeforeMonth.Value = set.DisplayBeforeMonth.ToString();//列表显示控制前几月
                txtAfterMonth.Value = set.DisplayAfterMonth.ToString();//列表显示控制后几月
                GroupSender.OperId = set.SongTuanRenId;
                GroupSender.OperName = set.SongTuanRenName;
                GroupSet.Value = set.JiHeDiDian;
                SetMark.Value = set.JiHeBiaoZhi;
                rbl_ReceiptRemind.SelectedValue = ((int)set.ReceiptRemindType).ToString();//收款提醒
                txtTanChuangTiXingInterval.Value = (set.TanChuangTiXingInterval / 60).ToString();
            }
            if (set != null && set.CompanyPrintFile != null)
            {
                SetPrintPic(set);//显示模板
            }
        }
        /// <summary>
        /// 设置打印模板显示
        /// </summary>
        /// <param name="set"></param>
        protected void SetPrintPic(EyouSoft.Model.CompanyStructure.CompanyFieldSetting set)
        {
            if (!string.IsNullOrEmpty(set.CompanyPrintFile.PageHeadFile))
            {
                hidHeader.Value = set.CompanyPrintFile.PageHeadFile;
            }
            if (!string.IsNullOrEmpty(set.CompanyPrintFile.PageFootFile))
            {
                hidFooter.Value = set.CompanyPrintFile.PageFootFile;
            }
            if (!string.IsNullOrEmpty(set.CompanyPrintFile.TemplateFile))
            {
                hidModel.Value = set.CompanyPrintFile.TemplateFile;
            }
            if (!string.IsNullOrEmpty(set.CompanyPrintFile.DepartStamp))
            {
                hidSeat.Value = set.CompanyPrintFile.DepartStamp;
            }
            if (!string.IsNullOrEmpty(set.CompanyLogo))
            {
                hidLog.Value = set.CompanyLogo;
            }
            companyLog = !string.IsNullOrEmpty(set.CompanyLogo) ? string.Format("<a href='{0}' class='" + hidLog.ClientID + "' target='_blank'>查看图片</a>&nbsp;<a href='javascript:;' onclick=\"return del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", set.CompanyLogo, hidLog.ClientID) : "暂无公司Logo";
            pageHeader = !string.IsNullOrEmpty(set.CompanyPrintFile.PageHeadFile) ? string.Format("<a href='{0}' class='" + hidHeader.ClientID + "' target='_blank'>查看页眉</a>&nbsp;<a href='javascript:;' onclick=\"return del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", set.CompanyPrintFile.PageHeadFile, hidHeader.ClientID) : "暂无页眉";
            pageFooter = !string.IsNullOrEmpty(set.CompanyPrintFile.PageFootFile) ? string.Format("<a href='{0}' class='" + hidFooter.ClientID + "' target='_blank'>查看页脚</a>&nbsp;<a href='javascript:;' onclick=\"return del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", set.CompanyPrintFile.PageFootFile, hidFooter.ClientID) : "暂无页脚";
            pageModel = !string.IsNullOrEmpty(set.CompanyPrintFile.TemplateFile) ? string.Format("<a href='{0}' class='" + hidModel.ClientID + "' target='_blank'>查看模板</a>&nbsp;<a href='javascript:;' onclick=\"return del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", set.CompanyPrintFile.TemplateFile, hidModel.ClientID) : "暂无模板";
            departSeal = !string.IsNullOrEmpty(set.CompanyPrintFile.DepartStamp) ? string.Format("<a href='{0}' class='" + hidSeat.ClientID + "' target='_blank'>查看公章</a>&nbsp;<a href='javascript:;' onclick=\"return del('{1}',this);\"><img src='/images/fujian_x.gif'/></a>", set.CompanyPrintFile.DepartStamp, hidSeat.ClientID) : "暂无公章";
        }

    }
}
