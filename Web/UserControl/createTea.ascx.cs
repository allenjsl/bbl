/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：建团规则
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\UserControl\createTea.ascx.cs
/// 功    能： 
/// 作    者：田想兵
/// 创建时间：2011-1-15 09:16:02
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
using EyouSoft.Common;

namespace Web.UserControl
{
    public partial class createTea : System.Web.UI.UserControl
    {
        public string guize="";
        public int companyid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public string getGuiZe()
        {
            return Utils.GetFormValue(hd_guize.UniqueID);
        }
        public int getDays()
        {
            return EyouSoft.Common.Utils.GetInt(hd_days.Value);
        }
        public void setGuiZe(string gz)
        {
            guize = gz;
        }
    }
}