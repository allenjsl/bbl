using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using EyouSoft.SSOComponent.Entity;
using System.IO;

namespace Web.ashx
{
    /// <summary>
    /// 張新兵20110530,用於編輯器的圖片上傳功能
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class fileupload : IHttpHandler
    {
        //定义允许上传的文件扩展名
        private String fileTypes = "gif,jpg,jpeg,png,bmp";
        //最大文件大小
        private int maxSize = 1024 * 1024 * 10;

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;

            //是否登錄
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (_IsLogin == false)
            {
                response.Clear();
                context.Response.Write("<script>alert('登录超时，请重新登录！');window.top.location='/login.aspx';</script>");
                response.End();
            }

            HttpPostedFile imgFile = context.Request.Files["imgFile"];

            if (imgFile == null)
            {
                this.showError(response, "请选择文件。");
            }

            
            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();
            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError(response, "上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                showError(response, "只允许上传后缀名为gif,jpg,jpeg,png,bmp的文件");
            }

            string filepath = string.Empty;
            string oldfilename = string.Empty;
            bool isSucess = EyouSoft.Common.Function.UploadFile.FileUpLoad(imgFile, "SystemSetBaseMange", out filepath, out oldfilename);

            if (!isSucess)
            {
                this.showError(response, "上传图片失败");
            }

            response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            response.Write("{\"url\":\""+filepath+"\",\"error\":0}");
            response.End();
        }

        private void showError(HttpResponse response,string message)
        {
            response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            response.Write("{\"message\":\"" + message + "\",\"error\":1}");
            response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
