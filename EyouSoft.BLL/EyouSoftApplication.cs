using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.SessionState;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using EyouSoft.Services.BackgroundServices;

namespace EyouSoft.WEB
{
    public class EyouSoftApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            this.registerControllerFactory();
            this.launchBackgroundServices();
        }

        /// <summary>
        /// register controller factory
        /// </summary>
        private void registerControllerFactory()
        {
            IUnityContainer container = new UnityContainer();
            UnityConfigurationSection section = null;
            System.Configuration.Configuration config = null;
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();

            map.ExeConfigFilename = EyouSoft.Toolkit.Utils.GetMapPath("/Config/IDAL.Configuration.xml");
            config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            section = (UnityConfigurationSection)config.GetSection("unity");
            section.Containers.Default.Configure(container);

            container
                .RegisterType<EyouSoft.IDAL.BackgroundServices.IPluginService, EyouSoft.DAL.BackgroundServices.PluginService>()
                .RegisterType<EyouSoft.IDAL.BackgroundServices.ISmsTimerServices, EyouSoft.DAL.BackgroundServices.SmsTimerServices>()
                .RegisterType<EyouSoft.IDAL.BackgroundServices.ICaringSmsTimerServices, EyouSoft.DAL.BackgroundServices.CaringSmsTimerServices>();

            Application.Add("container", container);
        }

        /// <summary>
        /// launch background services
        /// </summary>
        private void launchBackgroundServices()
        {
            if (!System.IO.File.Exists(EyouSoft.Toolkit.Utils.GetMapPath("/Config/BackgroundServices.txt")))
                return;

            IUnityContainer container = (IUnityContainer)Application["container"];

            BackgroundServicesExecutor backgroundServicesExecutor = (BackgroundServicesExecutor)Application["backgroundServicesExecutor"];

            if (backgroundServicesExecutor == null)
            {
                backgroundServicesExecutor = new BackgroundServicesExecutor(container);

                Application.Add("backgroundServicesExecutor", backgroundServicesExecutor);

                backgroundServicesExecutor.Start();
                EyouSoft.Services.BackgroundServices.SmsUtils.WLog("后台服务开启","/Config/BackgroundServicesLog.txt");
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            EyouSoft.Exception.Facade.ApplicationException.ProcessException(Server.GetLastError().InnerException, "MyPolicy");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            BackgroundServicesExecutor backgroundServicesExecutor = (BackgroundServicesExecutor)Application["backgroundServicesExecutor"];

            if (backgroundServicesExecutor != null)
                backgroundServicesExecutor.Stop();
        }
    }
}