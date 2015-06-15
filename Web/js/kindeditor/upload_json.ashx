<%@ webhandler Language="C#" class="Upload" %>

/**
 * KindEditor ASP.NET
 *
 * 本ASP.NET程序是演示程序，建议不要直接在实际项目中使用。
 * 如果您确定直接使用本程序，使用之前请仔细确认相关安全设置。
 *
 */

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using LitJson;
using EyouSoft.Common;
using EyouSoft.Common.DAL;
using System.Data;

public class Upload : IHttpHandler,System.Web.SessionState.IRequiresSessionState 
{
	//文件保存目录路径
    private String savePath = "../../File/Images/";
	//文件保存目录URL
    private String saveUrl = "/File/Images/";
	//定义允许上传的文件扩展名
	private String fileTypes = "gif,jpg,jpeg,png,bmp";
	//最大文件大小
	private int maxSize = 1024*1024*10;

	private HttpContext context;

	public void ProcessRequest(HttpContext context)
	{


        
		this.context = context;

        if (context.Session["u"] != null && context.Session["p"] != null)
        {
            AdminDAL dal = new AdminDAL();

            DataTable dt = dal.Exists(context.Session["u"].ToString(), context.Session["p"].ToString());
            if (dt.Rows.Count > 0)
            {
                context.Session["u"] = dt.Rows[0]["username"];
                context.Session["p"] = dt.Rows[0]["password"];
            }
            else
            {
                context.Response.Write("<script>alert('登录超时，请重新登录！');window.parent.location='/manage/login.aspx';</script>");
                return;
            }
        }
        else
        {
            context.Response.Write("<script>alert('登录超时，请重新登录！');window.parent.location='/manage/login.aspx';</script>");
            return;
        }
		HttpPostedFile imgFile = context.Request.Files["imgFile"];
		if (imgFile == null)
		{
			showError("请选择文件。");
		}

		String dirPath = context.Server.MapPath(savePath);
		if (!Directory.Exists(dirPath))
		{
			showError("上传目录不存在。");
		}

		String fileName = imgFile.FileName;
		String fileExt = Path.GetExtension(fileName).ToLower();
		ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));

		if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
		{
			showError("上传文件大小超过限制。");
		}

		if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
		{
			showError("上传文件扩展名是不允许的扩展名。");
		}

		String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
		String filePath = dirPath + newFileName;

		imgFile.SaveAs(filePath);

		String fileUrl = saveUrl + newFileName;

		Hashtable hash = new Hashtable();
		hash["error"] = 0;
		hash["url"] = fileUrl;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	private void showError(string message)
	{
		Hashtable hash = new Hashtable();
		hash["error"] = 1;
		hash["message"] = message;
		context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
		context.Response.Write(JsonMapper.ToJson(hash));
		context.Response.End();
	}

	public bool IsReusable
	{
		get
		{
			return true;
		}
	}
}
