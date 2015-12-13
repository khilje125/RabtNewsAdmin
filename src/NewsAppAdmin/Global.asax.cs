using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using NewsAppAdmin.FeedScrapper;
using System.Threading;

namespace NewsAppAdmin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            string url = "http://www.bbc.com/arabic/index.xml";
            string url3 = "http://arabic.cnn.com/rss";
            //string url2 = "http://feeds.feedburner.com/techulator/articles";

            ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
            {
                while (true)
                {
                   // MainScrapper.ScrappingData();
                   // RSSFeedsScrapper.ScrappingData();
                    TwitterFeedsScrapper.ScrappingData();
                    Thread.Sleep(2000000);
                }

            }), null);


            //MainScrapper.StartScrapper(url);
        }
    }
}