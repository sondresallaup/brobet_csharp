using Brobet.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using Stripe;

namespace Brobet
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JobScheduler.Start();

            StripeConfiguration.SetApiKey("sk_test_QAgVMc8HdTAaXaStuHfS3dEW");

            string connectionString = "DefaultConnection";
            string userTableName = "Users";
            string userIdColumn = "userId";
            string usernameColumn = "username";
            WebSecurity.InitializeDatabaseConnection(connectionString, userTableName, userIdColumn, usernameColumn, true);
        }
    }
}
