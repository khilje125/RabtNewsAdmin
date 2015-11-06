using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace NewsAppAdmin.FeedScrapper
{
    public class CommunicationResponse
    {

        #region Actual class

        public enum USER_AGENT
        {
            Windows,
            Iphone
        }
        public static Dictionary<string, string> userAgents = new Dictionary<string, string>()
        {
        {USER_AGENT.Windows.ToSafeString(),"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1"},
        {USER_AGENT.Iphone.ToSafeString(),"Mozilla/5.0 (iPhone; U; CPU OS 4_2_1 like Mac OS X) AppleWebKit/532.9 (KHTML, like Gecko) Version/5.0.3 Mobile/8B5097d Safari/6531.22.7"}
        };

        public string Html = string.Empty;
        public CookieCollection Cookies;
        public static CookieContainer COOKIES = null;
        public static string SourceCharsetEncoding = string.Empty;
        public static Stopwatch sw = new Stopwatch();
        public static Stopwatch swDelay = new Stopwatch();
        public Uri Uri = null;
        public object CallbackMethodReturnedValue { get; set; }
        public HttpStatusCode StatusCode;
        public string OriginUrl = string.Empty;
        public delegate CommunicationResponse CallBack(CommunicationResponse commResp);
        public CommunicationResponse()
        {
            RemoveCertificateCheck();
        }

        public CommunicationResponse(string origin)
        {
            OriginUrl = origin;
            RemoveCertificateCheck();
        }

        public void RemoveCertificateCheck()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            /*
            ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            */
        }

        public string Url
        {
            get
            {
                if (Uri == null)
                    return string.Empty;

                return Uri.ToString();
            }
        }

        public bool IsValid
        {
            get
            {
            //    if (Uri == null)
            //    {
            //        Responder.LogDetail(LogType.Invalid_Response, "Null Response URI");
            //        return false;
            //    }

            //    if (string.IsNullOrEmpty(Url))
            //    {
            //        Responder.LogDetail(LogType.Invalid_Response, "Null Response URL");
            //        return false;
            //    }

            //    if (string.IsNullOrEmpty(Html))
            //    {
            //        Responder.LogDetail(LogType.Invalid_Response, "Empty Response HTML");
            //        return false;
            //    }

                return true;
            }
        }

        private void Clear()
        {
            Uri = null;
            Html = string.Empty;
            Cookies = null;
        }

        public XmlDocument ToXml()
        {

            return ConvertToXml(ref Html);
        }

        public static XmlDocument ConvertToXml(ref string html)
        {
            try
            {
                if (!string.IsNullOrEmpty(html))
                {


                    string regEx1 = "<!--(?s:.*?)-->";
                    html = Regex.Replace(html, regEx1, "");
                }
                HtmlDocument hd = new HtmlDocument();

                hd.LoadHtml(html);

                hd.OptionOutputAsXml = true;

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                hd.Save(sw);

                string xmlStr = sb.ToString();

                XmlDocument xd = new XmlDocument();
                string regEx = "<!--(?s:.*?)-->";
                xmlStr = Regex.Replace(xmlStr, regEx, "");
                xmlStr = xmlStr.Replace(" xmlns=\"http://www.w3.org/1999/xhtml\"", "");
                xmlStr = xmlStr.Replace("-=\"\"", "");

                xd.LoadXml(xmlStr);

                return xd;
            }
            catch (Exception e)
            {
                string t = e.Message;
                //- Responder.LogDetail(LogType.Null_XML, e.Message, html.ToHTMLEncoded());
            }

            return null;
        }
        public static XmlDocument ConvertXmlToXmlDocument(ref string xml)
        {
            try
            {


                XmlDocument xd = new XmlDocument();

                xd.LoadXml(xml);

                return xd;

            }
            catch (Exception e)
            {
                string t = e.Message;
                //- Responder.LogDetail(LogType.Null_XML, e.Message, html.ToHTMLEncoded());
            }

            return null;
        }
        #endregion

        #region GET PAGE

        public static CommunicationResponse GetPageWithTries(string url, int tries)
        {
            while (tries > 0)
            {

                CommunicationResponse comms = GetPage(url, string.Empty);

                if (comms.IsValid)
                    return comms;

                tries--;
            }

            return new CommunicationResponse();
        }

        public static CommunicationResponse GetPageWithCallback(string url, CallBack cb)
        {
            return GetPage(url, "", false, USER_AGENT.Windows, cb);
        }

        public static CommunicationResponse GetPage(string url, string referer = "", bool resetCookies = false, USER_AGENT userAgent = USER_AGENT.Windows, CallBack cb = null, int retryCount = 0)
        {
            CheckAndSleep();
            CommunicationResponse communication = new CommunicationResponse(url);

            HttpWebRequest request = null;
            CookieContainer cookies = null;
            HttpWebResponse response = null;
            StreamReader response_stream = null;
            if (resetCookies)
            {

            }
            else
            {
                cookies = null; //-Responder.COOKIES; CRITICAL. in GET request. always clearing the cooking
            }

            if (cookies == null)
            {
                cookies = new CookieContainer();
            }

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgents[userAgent.ToSafeString()];
                request.Method = "GET";
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookies;


                if (string.IsNullOrEmpty(referer) == false)
                    request.Referer = referer;
                
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    if (string.IsNullOrEmpty(SourceCharsetEncoding))
                    {
                        response_stream = new StreamReader(response.GetResponseStream());
                        communication.Html = response_stream.ReadToEnd();
                    }
                    else
                    {
                        response_stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(SourceCharsetEncoding));
                        communication.Html = SournceEncodingToUTF8(response_stream.ReadToEnd());
                    }


                }
                #region Do this whatever the status is returned
                if (response != null)
                {
                    communication.StatusCode = response.StatusCode;
                    if (response.StatusCode != HttpStatusCode.OK) //if OK, already set above
                    {
                        communication.Html = response_stream.ReadToEnd();
                    }
                    communication.Cookies = response.Cookies;
                    communication.Uri = response.ResponseUri;
                    cookies.Add(communication.Cookies);
                    COOKIES = cookies;
                }



                #endregion
            }
            catch (WebException e)
            {
               
                #region Sleep if blocked
                if (retryCount < 3)
                {

                    GetPage(url, referer, resetCookies, userAgent, cb, ++retryCount);
                }
                #endregion
                using (WebResponse Exresponse = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)Exresponse;
                    if (httpResponse != null)
                    {


                        communication.StatusCode = httpResponse.StatusCode;
                        communication.Cookies = httpResponse.Cookies;
                        communication.Uri = httpResponse.ResponseUri;

                        try
                        {
                            if (string.IsNullOrEmpty(SourceCharsetEncoding))
                            {
                                response_stream = new StreamReader(httpResponse.GetResponseStream());
                                communication.Html = response_stream.ReadToEnd();
                            }
                            else
                            {
                                response_stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.GetEncoding(SourceCharsetEncoding));
                                communication.Html = SournceEncodingToUTF8(response_stream.ReadToEnd());
                            }
                        }
                        catch { }

                        cookies.Add(communication.Cookies);
                        COOKIES = cookies;

                    }


                }
            }
            catch (Exception e)
            {
                #region Sleep if blocked
                if (retryCount < 3)
                {
                    GetPage(url, referer, resetCookies, userAgent, cb, ++retryCount);
                }
                else
                {
                    communication.Clear();
                }
                #endregion

            }
            finally
            {
                if (response_stream != null)
                    response_stream.Close();

                if (response != null)
                    response.Close();

                if (cb != null)
                {
                    communication = cb(communication);
                }
               
            }

            return communication;
        }
/*
        public static CommunicationResponse GetPageCustom(HttpWebRequest custom, byte[] postDatToLog = null, List<string> paramsToMask = null)
        {
            CheckAndSleep();

            CommunicationResponse communication = new CommunicationResponse(custom.RequestUri.ToString());

            HttpWebRequest request = custom;

            HttpWebResponse response = null;
            StreamReader response_stream = null;
            CookieContainer cookies = custom.CookieContainer;
            if (cookies == null)
            {
                cookies = new CookieContainer();
            }
            try
            {
                if (ApplicationConstants.LogVerbosity == LoggingLevel.Verbose)
                { LogRequest(request, postDatToLog, paramsToMask); }
            }
            catch (Exception e) { Responder.LogException(e.Message, e.ToString()); }
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    if (string.IsNullOrEmpty(Responder.SourceCharsetEncoding))
                    {
                        response_stream = new StreamReader(response.GetResponseStream());
                        communication.Html = response_stream.ReadToEnd();
                    }
                    else
                    {
                        response_stream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(Responder.SourceCharsetEncoding));
                        communication.Html = SournceEncodingToUTF8(response_stream.ReadToEnd());
                    }
                }

                if (response != null)
                {
                    communication.StatusCode = response.StatusCode;
                    if (response.StatusCode != HttpStatusCode.OK) //if OK, already set above
                    {
                        communication.Html = response_stream.ReadToEnd();
                    }
                    communication.Cookies = response.Cookies;
                    communication.Uri = response.ResponseUri;
                    cookies.Add(communication.Cookies);
                    Responder.COOKIES = cookies;
                }
            }
            catch (Exception e)
            {
                Responder.LogException(e.Message, e.ToString());
                communication.Clear();
            }
            finally
            {
                if (response_stream != null)
                    response_stream.Close();

                if (response != null)
                    response.Close();

                try
                {
                    if (ApplicationConstants.LogVerbosity == LoggingLevel.Verbose)
                    {
                        LogResponse(communication, request.Method.Trim() == "POST" ? true : false);
                    }
                }
                catch (Exception e) { Responder.LogException(e.Message, e.ToString()); }
            }

            return communication;
        }
*/
        public static bool IsHTTP200AtPage(string url, USER_AGENT userAgent = USER_AGENT.Windows)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgents[userAgent.ToSafeString()];
                request.Method = "GET";

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;
            }
            catch
            {

            }
            finally
            {
                if (response != null)
                    response.Close();
            }

            return false;
        }

        #endregion



        public static string SournceEncodingToUTF8(string textToConvert)
        {
            Encoding iso8859 = Encoding.GetEncoding(SourceCharsetEncoding);
            Encoding UTF8 = Encoding.UTF8;
            byte[] srcTextBytes = iso8859.GetBytes(textToConvert);
            byte[] destTextBytes = Encoding.Convert(iso8859, UTF8, srcTextBytes);
            char[] destChars = new char[UTF8.GetCharCount(destTextBytes, 0, destTextBytes.Length)];
            UTF8.GetChars(destTextBytes, 0, destTextBytes.Length, destChars, 0);

            StringBuilder result = new StringBuilder(textToConvert.Length + (int)(textToConvert.Length * 0.1));

            foreach (char c in destChars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

       
        private static void CheckAndSleep()
        {
            if (ScrapeHelpper.Delay > 0)
            {
                swDelay.Stop();
                double elapsedMS = swDelay.Elapsed.TotalMilliseconds;
                if (elapsedMS < ScrapeHelpper.Delay)
                {
                    int remainingMS = (ScrapeHelpper.Delay - elapsedMS).ToInt();
                    Console.WriteLine("SLEEPING.... {0} MS", remainingMS);
                    System.Threading.Thread.Sleep(remainingMS);
                    Console.WriteLine("WOKE UP..", remainingMS);
                }
                swDelay.Restart();
            }
        }
        private static void SleepWhenBlocked()
        {
            System.Threading.Thread.Sleep(1000 * 60 * 5);

        }


    }
}
