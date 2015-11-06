using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAppAdmin.MODEL;
using System.Xml;

namespace NewsAppAdmin.FeedScrapper
{
    public class MainScrapper
    {
        public static void StartScrapper(string url)
        {

            List<ModelFeedsData> FeedList = new List<ModelFeedsData>();
            ModelFeedsData FeedData = new ModelFeedsData();
            try
            {
                CommunicationResponse resp = CommunicationResponse.GetPage(url);
                if (resp.IsValid)
                {
                    //XmlDocument RSSXml = new XmlDocument();
                    //RSSXml.Load(url);
                    //XmlDocument xd = resp.ToXml();
                    XmlDocument xd = new XmlDocument();
                    xd.Load(url);
                    if (xd != null)
                    {
                        //XmlNodeList nodes1 = RSSXml.SelectNodes("rss/channel/item");
                        //XmlNode rssSubNode = nodes1[0].SelectSingleNode("/span/feed/title");
                        //FeedData.MainTitle = rssSubNode != null ? rssSubNode.InnerText : "";
                        //XmlDocument xdInner = new XmlDocument();
                        //xdInner.LoadXml(xd.InnerXml);
                        //XmlNodeList rssSubNodeXml = xdInner.SelectNodes("//channel/item");
                        //XmlNode rssSubNode = xdInner.SelectSingleNode("//channel/title");
                        //if (rssSubNode != null)
                        //{
                        //    FeedData.MainTitle = rssSubNode.InnerText;
                        //}
                        //else
                        //{
                        //    FeedData.MainTitle = String.Empty;
                        //}
                        XmlNode rssSubNodetitle = xd.SelectSingleNode("//feed/id");
                        if (rssSubNodetitle != null)
                        {
                            FeedData.MainTitle = rssSubNodetitle.InnerText; 
                        }
                        XmlNode rssSubNodeCategory = xd.SelectSingleNode("//feed/category[1]");
                        if (rssSubNodeCategory != null)
                        {
                            FeedData.MainCategory = rssSubNodeCategory.InnerText;   
                        }
                        XmlNode rssSubNodeLogo = xd.SelectSingleNode("//feed/logo");
                        if (rssSubNodeLogo != null)
                        {
                            FeedData.CoverImage = rssSubNodeLogo.InnerText;
                        }
                        
                        
                        XmlNodeList nodes = xd.SelectNodes("//entry");
                        if (nodes != null)
                        {
                            ModelFeedDetail modelfeedDetail = new ModelFeedDetail();
                            foreach (XmlNode item in nodes)
                            {
                                modelfeedDetail.FeedDetailId = item.SelectSingleNode("/dc:identifier").InnerText;
                                modelfeedDetail.PostTitle = item.SelectSingleNode("/title").InnerText;
                                modelfeedDetail.PublishDate = item.SelectSingleNode("/published").InnerText;
                                //optional
                                modelfeedDetail.RssImage = item.SelectSingleNode("").InnerText;
                                modelfeedDetail.ShortDescription = item.SelectSingleNode("/summary").InnerText;

                                modelfeedDetail.Category = item.SelectSingleNode("/category ").Attributes["label"].Value;
                                //for send request on Detail page to get Post Details
                                if (item.SelectSingleNode("/link").Attributes != null)
                                {
                                    modelfeedDetail.DetailPageURL = item.SelectSingleNode("/link").Attributes["href"].Value;
                                }
                                else
                                {
                                    modelfeedDetail.DetailPageURL = item.SelectSingleNode("/link").InnerText;
                                }
                                CommunicationResponse respDetailPage = CommunicationResponse.GetPage(modelfeedDetail.DetailPageURL);
                                if (resp.IsValid)
                                {
                                    XmlDocument xdDetailPage = respDetailPage.ToXml();
                                    modelfeedDetail.DetailPageImage = xdDetailPage.SelectSingleNode("//*[@id='page']/div/div[2]/div/div[1]/div[1]/div[2]/figure[1]/span/img").Attributes["src"].Value;
                                    modelfeedDetail.DetailPagePostDetail = xdDetailPage.SelectSingleNode("//*[@id='page']/div/div[2]/div/div[1]/div[1]/div[2]/p").InnerText;
                                }
                                FeedData.FeedDetail.Add(modelfeedDetail); 
                            } 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
