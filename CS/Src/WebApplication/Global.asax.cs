using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

using System.Web.ApplicationServices;

using log4net;

using Common;
using Common.DataAccess;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region ロガーフィールド
        private ILog m_logger;
        #endregion

        protected void Application_Start()
        {
            // ロガーを取得
            m_logger = LogManager.GetLogger(this.GetType());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // 認証カスタマイズ
            AuthenticationService.Authenticating += (s, evt) =>
            {
                using (var dbContext = new BaseDbContext())
                {
                    var dataAccess = new CommonDA(dbContext);
                    evt.Authenticated = dataAccess.Login(evt.UserName, evt.Password);
                }

                evt.AuthenticationIsComplete = true;
            };
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // From認証のリダイレクト不可
            //if (Context.Request.CurrentExecutionFilePathExtension == ".svc")
            //    Context.Response.SuppressFormsAuthenticationRedirect = true;
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Request.CurrentExecutionFilePathExtension == ".svc" && Response.IsRequestBeingRedirected)
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
        }

        
        protected void Application_Error(object sender, EventArgs e)
        {
            m_logger.Error(e);
        }
    }
}
