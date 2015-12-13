using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAppAdmin.MODEL;
using System.Xml;
using System.Data;
using NewsAppAdmin.BLL;

namespace NewsAppAdmin.FeedScrapper
{
    public class MainScrapper
    {
        public static void ScrappingData()
        {
            DataTable rssData = new DataTable();
            BLLFeed objBLLFeed = new BLLFeed();
            rssData = objBLLFeed.GetFeedUrls(2);
            if (rssData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in rssData.Rows)
                {
                    ModelFeed modelFeed = new ModelFeed();
                    modelFeed.FeedURL = dataRow["FeedURL"].ToString();
                    modelFeed.FeedTitlePath = dataRow["FeedTitlePath"].ToString();
                    modelFeed.FeedLastUpdateDateTimePath = dataRow["FeedLastUpdateDateTimePath"].ToString();
                    modelFeed.FeedCoverImagePath = dataRow["FeedCoverImagePath"].ToString();
                    modelFeed.FeedItemsPath = dataRow["FeedItemsPath"].ToString();
                    modelFeed.FeedShortDescPath = dataRow["FeedShortDescPath"].ToString();
                    modelFeed.FeedDetailPageURLPath = dataRow["FeedDetailPageURLPath"].ToString();
                    modelFeed.FeedPubDatePath = dataRow["FeedPubDatePath"].ToString();
                    modelFeed.FeedImagePath = dataRow["FeedImagePath"].ToString();
                    modelFeed.FeedDetailPageImagePath = dataRow["FeedDetailPageImagePath"].ToString();
                    modelFeed.FeedPostDetailsPath = dataRow["FeedPostDetailsPath"].ToString();
                    modelFeed.FeedCategoryPath = dataRow["FeedCategoryPath"].ToString();
                    modelFeed.FeedPostDetailsPath = dataRow["FeedSubCategoryPath"].ToString();

                    List<ModelFeedsData> feedsDetailList = StartScrapper(modelFeed);
                }
            }
        }
        private static List<ModelFeedsData> StartScrapper(ModelFeed feedsSetting)
        {

            List<ModelFeedsData> FeedList = new List<ModelFeedsData>();
            ModelFeedsData FeedData = new ModelFeedsData();
            try
            {
                CommunicationResponse resp = CommunicationResponse.GetPage(feedsSetting.FeedURL);
                if (resp.IsValid)
                {
                    //XmlDocument RSSXml = new XmlDocument();
                    //RSSXml.Load(url);
                    //XmlDocument xd = resp.ToXml();
                    XmlDocument xd = new XmlDocument();
                    xd.Load(feedsSetting.FeedURL);
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
                        ModelFeedDetail modelfeedDetail = new ModelFeedDetail();
                        XmlElement xmlDoc = xd.DocumentElement;
                        XmlNodeList xmlNodeLists = xd.SelectNodes(feedsSetting.FeedItemsPath);
                        if (xmlNodeLists.Count > 0 && xmlNodeLists != null)
                        {
                            foreach (XmlNode item in xmlNodeLists)
                            {
                                if (!String.IsNullOrEmpty(feedsSetting.FeedTitlePath.Trim()))
                                {
                                    if (item.SelectSingleNode(feedsSetting.FeedTitlePath) != null)
                                    {
                                        //getting item Title from RSS Feed Page
                                        modelfeedDetail.PostTitle = item.SelectSingleNode(feedsSetting.FeedTitlePath).InnerText;
                                    }
                                }

                                if (!String.IsNullOrEmpty(feedsSetting.FeedShortDescPath.Trim()))
                                {
                                    if (item.SelectSingleNode(feedsSetting.FeedShortDescPath) != null)
                                    {
                                        //getting short description from RSS Feed Page
                                        modelfeedDetail.ShortDescription = item.SelectSingleNode(feedsSetting.FeedShortDescPath).InnerText;
                                    }
                                }

                                if (!String.IsNullOrEmpty(feedsSetting.FeedPubDatePath.Trim()))
                                {
                                    if (item.SelectSingleNode(feedsSetting.FeedPubDatePath) != null)
                                    {
                                        //getting item publish date from RSS Feed Page
                                        modelfeedDetail.PublishDate = item.SelectSingleNode(feedsSetting.FeedPubDatePath).InnerText;
                                    }
                                }

                                if (!String.IsNullOrEmpty(feedsSetting.FeedImagePath.Trim()))
                                {
                                    if (item.SelectSingleNode(feedsSetting.FeedImagePath) != null)
                                    {
                                        //getting item image from RSS Feed Page
                                        modelfeedDetail.RssImage = item.SelectSingleNode(feedsSetting.FeedImagePath).InnerText;
                                    }
                                }

                                if (!String.IsNullOrEmpty(feedsSetting.FeedDetailPageURLPath.Trim()))
                                {
                                    if (item.SelectSingleNode(feedsSetting.FeedDetailPageURLPath) != null)
                                    {
                                        //getting Detail Page URL from Feed
                                        modelfeedDetail.DetailPageURL = item.SelectSingleNode(feedsSetting.FeedDetailPageURLPath).InnerText;
                                    }
                                    //If Detail page URL not empty Then
                                    if (!String.IsNullOrEmpty(modelfeedDetail.DetailPageURL))
                                    {
                                        //web Request on Detial page URL 
                                        //XmlDocument xdDetailPage = new XmlDocument();
                                        //xdDetailPage.Load(modelfeedDetail.DetailPageURL);
                                        CommunicationResponse GetdetailPageResponse = CommunicationResponse.GetPage(modelfeedDetail.DetailPageURL);
                                        XmlDocument xdDetailPage = GetdetailPageResponse.ToXml();
                                        if (xdDetailPage != null)
                                        {
                                            //Detail Page Will screape here
                                        }
                                    }
                                }
                            }
                        }
                        /*
                        XmlElement xmlDoc = xd.DocumentElement;
                        foreach (XmlElement node in xd.DocumentElement)
                        {
                            if (node.Name == "title")
                            {
                                string feedTile = node.InnerText;
                            }
                            if (node.Name == "updated")
                            {
                                string feedcategory = node.InnerText;
                            }

                            XmlNodeList xmlNodeList = node.SelectNodes("/entry");
                            if (xmlNodeList.Count > 0)
                            {
                                
                            }

                            //if (!String.IsNullOrEmpty(node.Attributes[0].Value))
                            //{
                            //    string feedTile = node["title"].Value;    
                            //}
                            //if (!String.IsNullOrEmpty(node["updated"].Value))
                            //{
                            //string feedcategory = node["updated"].Value;    
                            //}
                            
                        }
                        string xmlNode = xmlDoc.ParentNode.InnerText;
                        XmlNodeList xmlNodeLists = xd.SelectNodes("/feed/entry");
                        //XmlNode titleNode = xmlNodeList.Count.ToString();
                        //string title = xmlNodeLists.Count.ToString();
                        if (xmlNodeLists != null)
                        {
                            //FeedData.MainTitle = rssSubNodetitle.InnerText; 
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
                        */
                    }
                }
                return FeedList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
