using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data; 

namespace NewsAppAdmin.FeedScrapper
{
    public class ScrapeHelpper
    {
        
        private static string ConformURL(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            if (input.Trim().StartsWith("https://") || input.Trim().StartsWith("http://"))
            {
                return input;
            }
            string http = "http://";

            if (input.StartsWith("//"))
                input = input.Remove(0, 2);

            if (!input.StartsWith(http))
                return http + input;

            return input;
        }

        //public static string ConvertToSecureURL(string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //        return SourceURLSecure;

        //    url = url.Replace("http://", "https://");

        //    return url;
        //}

        //#region SCRAPED SITE URLS

        ///// <summary>
        ///// Gets the Source BaseURL, ie: TargeSITE
        ///// </summary>
        /////
        //public static string SourceURL
        //{
        //    get
        //    {
        //        return Convert.ToString(ConfigurationManager.AppSettings[string.Format("SourceURL{0}", SourceWebsitePrefix)]).Trim().TrimEnd('/').Replace("https:", "http:");
        //    }
        //}

        //public static string SourceURLSecure
        //{
        //    get
        //    {

        //        string input = ConfigurationManager.AppSettings[string.Format("SourceURLSecure{0}", SourceWebsitePrefix)].ToSafeString().TrimEnd('/').Trim();
        //        if (!string.IsNullOrEmpty(input))
        //        {
        //            return input;
        //        }
        //        else
        //        {
        //            return SourceURL.Replace("http:", "https:");
        //        }
        //    }
        //}

        //#endregion

        ////private static string SourceWebsitePrefix
        ////{
        ////    get
        ////    {
        ////        string sourceWebsitePrefix = string.Empty;
        ////        try
        ////        {
        ////            if (Responder.SourcewebsiteVersion != null && (Responder.SourcewebsiteVersion != SourceWebsiteVersion.DEFAULT))
        ////            {
        ////                sourceWebsitePrefix = Responder.SourcewebsiteVersion.Name;
        ////                if (!string.IsNullOrEmpty(sourceWebsitePrefix))
        ////                {
        ////                    sourceWebsitePrefix = "_" + sourceWebsitePrefix.Trim();
        ////                }
        ////            }
        ////        }
        ////        catch
        ////        {
        ////            //CASE: when session is not started yet
        ////        }
        ////        return sourceWebsitePrefix;
        ////    }
        ////}

        #region CONNECTION STRINGS

        public static string ConnectionString
        {
            get { return Convert.ToString(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString); }
        }


        #endregion

        public static bool IsDBLoggingEnabled
        {
            get { return ConfigurationManager.AppSettings["EnableDbLogging"].ToBoolean(); }
        }
        public static int Delay
        {
            get { return ConfigurationManager.AppSettings["Delay"].ToInt(); }
        }
        public static int ExceptionDelay
        {
            get { return ConfigurationManager.AppSettings["ExceptionDelay"].ToInt(); }
        }
        public static bool EnableThreading
        {
            get { return ConfigurationManager.AppSettings["EnableThreading"].ToBoolean(); }
        }

    }
    public class SourceWebsiteVersion
    {
        public static readonly SourceWebsiteVersion DEFAULT = new SourceWebsiteVersion(0, "DEFAULT");


        public int Value { get; set; }
        public string Name { get; set; }

        public SourceWebsiteVersion(int internalValue, string name)
        {
            this.Value = internalValue;
            this.Name = name;
        }

    }


}
