﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAppAdmin.MODEL;
using System.Xml;
using System.Data;
using NewsAppAdmin.BLL;
using TweetSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NewsAppAdmin.FeedScrapper
{
    public class TwitterFeedsScrapper
    {
        public static void ScrappingData()
        {
            DataTable rssData = new DataTable();
            BLLFeed objBLLFeed = new BLLFeed();
            rssData = objBLLFeed.GetFeedUrls(2);

            /*>>> API Creadentials <<<<////
                >> Under Informnation is creating from Personal Account Information API
                >> For any change in this credentials Login Account : dikchani@yahoo.com
                >> API Name : News Get Application
                >> Please don't change any word without permission from following API Admin*/
            string _consumerKey = "cvRCCaqLUlM9SyolFwYEQQ2uZ";
            string _consumerSecret = "sVvOgCPwuzXo37v4qjAriGbftwEefMC9xNadPQLTsoOkeqiJ8C";
            string _accessToken = "65042389-YQ3jLfP1RWu9Q7So9VAj3Rc3J9oMB0Suuv0jlioAN";
            string _accessTokenSecret = "yHMVHihkHAaCIOJi2M9WEOq4fpODY1a0hdS92J9OZEJEL";
            // API access Data End

            var service = new TwitterService(_consumerKey, _consumerSecret);
            //var tweets = new ListTweetsOnHomeTimelineOptions();
            service.AuthenticateWith(_accessToken, _accessTokenSecret);



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
                    //double TwitterPageID = Convert.ToDouble(dataRow["TwitterPageId"].ToString());
                    double TwitterPageID = 1;
                    modelFeed.FeedChannelName = dataRow["FeedChannelName"].ToString();

                    modelFeed.FeedURL = "http://www.espncricinfo.com/rss/content/story/feeds/6.xml";
                    //List<ModelFeedsData> feedsDetailList = StartScrapper(modelFeed);
                    GetTwitterTweetsWithDetails(service, TwitterPageID, modelFeed);
                }
            }
        }


        private static void StartSafeScrappedData(ModelTwitterFeeds modelTwitterFeeds, ModelFeed modelFeed)
        {
            try
            {
                CommunicationResponse resp = CommunicationResponse.GetPage(modelFeed.FeedURL);
                if (resp.IsValid)
                {
                    //XmlDocument RSSXml = new XmlDocument();
                    //RSSXml.Load(url);
                    XmlDocument xd = resp.ToXml();
                    //XmlDocument xd = new XmlDocument();
                    xd.Load(modelFeed.FeedURL);
                    if (xd != null)
                    {
                        ModelFeedDetail modelfeedDetail = new ModelFeedDetail();
                        XmlElement xmlDoc = xd.DocumentElement;
                        /*XmlNodeList xmlNodeLists = xd.SelectNodes(feedsSetting.FeedItemsPath);
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
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        double dblLinkId;
        string strFeedURL;
        DateTime PublishDate;
        public static List<ModelTwitterFeedsDetails> GetTwitterTweets(string ScreenName)
        {
            ModelTwitterFeeds modelTwitterFeeds = new ModelTwitterFeeds();
            List<ModelTwitterFeedsDetails> lstModelTwitterFeedsDetail = new List<ModelTwitterFeedsDetails>();
            try
            {
                /*>>> API Creadentials <<<<////
                >> Under Informnation is creating from Personal Account Information API
                >> For any change in this credentials Login Account : dikchani@yahoo.com
                >> API Name : News Get Application
                >> Please don't change any word without permission from following API Admin*/
                string _consumerKey = "cvRCCaqLUlM9SyolFwYEQQ2uZ";
                string _consumerSecret = "sVvOgCPwuzXo37v4qjAriGbftwEefMC9xNadPQLTsoOkeqiJ8C";
                string _accessToken = "65042389-YQ3jLfP1RWu9Q7So9VAj3Rc3J9oMB0Suuv0jlioAN";
                string _accessTokenSecret = "yHMVHihkHAaCIOJi2M9WEOq4fpODY1a0hdS92J9OZEJEL";
                // API access Data End

                var service = new TwitterService(_consumerKey, _consumerSecret);
                //var tweets = new ListTweetsOnHomeTimelineOptions();
                service.AuthenticateWith(_accessToken, _accessTokenSecret);
                IAsyncResult result = service.BeginListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions());
                IEnumerable<TwitterStatus> tweets = service.EndListTweetsOnHomeTimeline(result);


                foreach (var tweet in tweets)
                {
                    if (tweets != null)
                    {
                        modelTwitterFeeds.FeedId = 0;
                        modelTwitterFeeds.UserPageId = 0;
                        modelTwitterFeeds.UserPageTitle = String.Empty;
                        modelTwitterFeeds.UserScreenName = String.Empty;
                        modelTwitterFeeds.UserPageLanguage = String.Empty;
                        modelTwitterFeeds.UserPageFollowers = 0;
                        modelTwitterFeeds.UserPageCoverImageURL = String.Empty;
                        modelTwitterFeeds.UserPageLogoImage = String.Empty;
                    }

                    var st1 = tweet.Text; //string
                    var st2 = tweet.Source; //string
                    var st3 = tweet.TextAsHtml; //string
                    var st4 = tweet.TextDecoded; //string
                    var st5 = tweet.RetweetedStatus; //TwitterStatus
                    var st6 = tweet.RetweetCount; //int
                    var st7 = tweet.RawSource; //string
                    var st8 = tweet.Place; //TwitterPlace
                    var st9 = tweet.Location; //TwitterGeoLocation
                    var st10 = tweet.Language; //string
                    var st11 = tweet.IsTruncated; //bool
                    var st12 = tweet.IsRetweeted; //bool
                    var st13 = tweet.IsPossiblySensitive; //bool is nullable
                    var st14 = tweet.IsFavorited; //bool
                    var st15 = tweet.InReplyToUserId; //long is nullable 
                    var st16 = tweet.InReplyToStatusId; //long is nullable
                    var st17 = tweet.InReplyToScreenName; //string
                    var st18 = tweet.IdStr; //string
                    var st19 = tweet.Id; //long
                    var st20 = tweet.FavoriteCount; //int
                    var st21 = tweet.ExtendedEntities; //TwitterExtendedEntities
                    var st22 = tweet.Entities; //TwitterEntities
                    var st23 = tweet.CreatedDate; //DateTime
                    var st24 = tweet.Author; //ITweeter

                }

                //TwitterAccount user = new TwitterAccount.
                //TwitterUser twitterUser =
                //>>GET OTHER USER TIMELINE//BeginListTweetsOnHomeTimeline

                TwitterService t_service = new TwitterService(_consumerKey, _consumerSecret);
                t_service.AuthenticateWith(_accessToken, _accessTokenSecret);
                var t_options = new ListTweetsOnHomeTimelineOptions();
                t_options.ExcludeReplies = true;
                var t_tweets = t_service.ListTweetsOnHomeTimeline(t_options);
                //ListTweetsOnSpecifiedUserTimeline
                string ScreenNameBBCArabic = "BBCArabic";
                string ScreenNameCNNArabic = "cnnarabic";
                string ScreenName1 = "garbo_speaks";
                var User_OptionInit = new ListTweetsOnUserTimelineOptions { ScreenName = ScreenNameCNNArabic, Count = 200, ExcludeReplies = true };
                var User_Tweets = service.ListTweetsOnUserTimeline(User_OptionInit);

                string TweetText = String.Empty;

                foreach (var tweet in User_Tweets)
                {
                    var userDetail = tweet.User;
                    if (false)
                    {
                        var us1 = tweet.User.ContributorsEnabled; //bool?
                        var us2 = tweet.User.CreatedDate;
                        var us3 = tweet.User.Description;
                        var us4 = tweet.User.FavouritesCount;
                        var us5 = tweet.User.FollowersCount;
                        var us6 = tweet.User.FollowRequestSent;
                        var us7 = tweet.User.FriendsCount;
                        var us8 = tweet.User.Id;
                        var us9 = tweet.User.IsDefaultProfile;
                        var us10 = tweet.User.IsGeoEnabled;
                        var us11 = tweet.User.IsProfileBackgroundTiled;
                        var us12 = tweet.User.IsProtected;
                        var us13 = tweet.User.IsTranslator;
                        var us14 = tweet.User.IsVerified;
                        var us15 = tweet.User.Language;
                        var us16 = tweet.User.ListedCount;
                        var us17 = tweet.User.Location;
                        var us18 = tweet.User.Name;
                        var us19 = tweet.User.ProfileBackgroundColor;
                        var us20 = tweet.User.ProfileBackgroundImageUrl;
                        var us21 = tweet.User.ProfileBackgroundImageUrlHttps;
                        var us22 = tweet.User.ProfileImageUrl;
                        var us23 = tweet.User.ProfileImageUrlHttps;
                        var us24 = tweet.User.ProfileLinkColor;
                        var us25 = tweet.User.ProfileSidebarBorderColor;
                        var us26 = tweet.User.ProfileSidebarFillColor;
                        var us27 = tweet.User.ProfileTextColor;
                        var us28 = tweet.User.RawSource;
                        var us29 = tweet.User.ScreenName;
                        var us30 = tweet.User.Status;
                        var us31 = tweet.User.StatusesCount;
                        var us32 = tweet.User.TimeZone;
                        var us33 = tweet.User.Url;
                        var us34 = tweet.User.UtcOffset;
                    }

                    var st1 = tweet.Text.ToSafeString(); //string
                    TweetText = tweet.Text;
                    var st2 = tweet.Source; //string
                    var st3 = tweet.TextAsHtml; //string
                    var st4 = tweet.TextDecoded; //string
                    var st5 = tweet.RetweetedStatus; //TwitterStatus
                    var st6 = tweet.RetweetCount; //int
                    var st7 = tweet.RawSource; //string
                    var st8 = tweet.Place; //TwitterPlace
                    var st9 = tweet.Location; //TwitterGeoLocation
                    var st10 = tweet.Language; //string
                    var st11 = tweet.IsTruncated; //bool
                    var st12 = tweet.IsRetweeted; //bool
                    var st13 = tweet.IsPossiblySensitive; //bool is nullable
                    var st14 = tweet.IsFavorited; //bool
                    var st15 = tweet.InReplyToUserId; //long is nullable 
                    var st16 = tweet.InReplyToStatusId; //long is nullable
                    var st17 = tweet.InReplyToScreenName; //string
                    var st18 = tweet.IdStr; //string
                    var st19 = tweet.Id; //long
                    var st20 = tweet.FavoriteCount; //int
                    var st21 = tweet.ExtendedEntities; //TwitterExtendedEntities
                    var st22 = tweet.Entities; //TwitterEntities
                    var twitterEntities = new TwitterEntities();
                    twitterEntities = st22;
                    IList<TwitterUrl> twitterUrl = twitterEntities.Urls; //List<TwitterUrl>
                    foreach (var url in twitterUrl)
                    {
                        var url1 = url.DisplayUrl; //string
                        var url2 = url.EndIndex; //int
                        var url3 = url.EntityType; //TwitterEntityType
                        var entityType = url3;
                        //4 Entity Types are Defined
                        //TwitterEntityType.HashTag; //0
                        //TwitterEntityType.Mention; //1
                        //TwitterEntityType.Url; //2
                        //TwitterEntityType.Media; //3

                        var url4 = url.ExpandedValue; //string
                        var url5 = url.Indices; //IList<int>
                        var url6 = url.StartIndex; //int
                        var url7 = url.Value; //string
                        TweetText = TweetText.Trim().Replace(url.Value, String.Empty).Trim();
                    }

                    IList<TwitterMention> twitterMention = twitterEntities.Mentions; //List<TwitterMention>
                    foreach (var mention in twitterMention)
                    {
                        var url1 = mention.EndIndex; //int
                        var url2 = mention.EntityType; //int
                        var entityType = url2;

                        //4 Entity Types are Defined
                        //TwitterEntityType.HashTag; //0
                        //TwitterEntityType.Mention; //1
                        //TwitterEntityType.Url; //2
                        //TwitterEntityType.Media; //3

                        var url3 = mention.Id; //long
                        var url4 = mention.Indices; //IList<int>
                        var url5 = mention.Name; //string
                        var url6 = mention.ScreenName; //string
                        var url7 = mention.StartIndex; //int
                    }
                    IList<TwitterMedia> twitterMedia = twitterEntities.Media; //List<TwitterMedia>
                    foreach (var media in twitterMedia)
                    {
                        var media1 = media.DisplayUrl; //string
                        var media2 = media.EndIndex; //int
                        var media3 = media.EntityType; //TwitterEntity
                        var media4 = media.ExpandedUrl; //string
                        var media5 = media.Id; //long
                        var media6 = media.IdAsString; //string
                        var media7 = media.Indices; //IList<int>
                        var media8 = media.MediaType; //TwitterMediaType
                        var twitterMediaType = media8;

                        /*Three Types of MediaType
                        TwitterMediaType.Photo; //0
                        TwitterMediaType.Video; //1
                        TwitterMediaType.AnimatedGif; //2
                        */

                        var media9 = media.MediaUrl; //string
                        var media10 = media.MediaUrlHttps; //string
                        //var FeedImageURLhttp  = media.MediaUrl; for Http Image
                        //var FeedImageURLhttps  =  media.MediaUrlHttps; for Https Image
                        var media11 = media.Sizes; //TwitterMediaSizes
                        var twitterMediaSizes = media11;
                        //media11.Large
                        //media11.Medium
                        //media11.Small
                        //media11.Thumb
                        var media12 = media.StartIndex; //int
                        var media13 = media.Url; //string

                        TweetText = TweetText.Trim().Replace(media.Url, String.Empty).Trim();
                    }
                    IList<TwitterHashTag> twitterHashTag = twitterEntities.HashTags; //List<TwitterHashTag>
                    foreach (var hashTag in twitterHashTag)
                    {
                        var ht1 = hashTag.EndIndex;
                        var ht2 = hashTag.EntityType;
                        var ht3 = hashTag.Indices;
                        var ht4 = hashTag.StartIndex;
                        var ht5 = hashTag.Text;
                        // hashTagText for refine Tweet Text
                        TweetText = TweetText.Trim().Replace("#" + hashTag.Text, string.Empty).Trim();
                    }
                    TweetText = TweetText.Replace("\n", String.Empty).Trim();
                    var st23 = tweet.CreatedDate; //DateTime
                    var st24 = tweet.Author; //ITweeter

                    var ProfileImageURL = st24.ProfileImageUrl;
                    var ss = st24.RawSource;
                    var screenName = st24.ScreenName;


                }
                return lstModelTwitterFeedsDetail;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static void GetTwitterTweetsWithDetails(TwitterService service, double PageId, ModelFeed modelFeed)
        {
            ModelTwitterFeeds modelTwitterFeeds = new ModelTwitterFeeds();
            ModelTwitterFeedsDetails modelTwitterFeedsDetail = new ModelTwitterFeedsDetails();
            List<ModelTwitterFeedsDetails> lstModelTwitterFeedsDetail = new List<ModelTwitterFeedsDetails>();
            try
            {
                //TwitterAccount user = new TwitterAccount.
                //TwitterUser twitterUser =
                //>>GET OTHER USER TIMELINE//BeginListTweetsOnHomeTimeline

                var User_OptionInit = new ListTweetsOnUserTimelineOptions { ScreenName = modelFeed.FeedChannelName, Count = 200, ExcludeReplies = true };
                var User_Tweets = service.ListTweetsOnUserTimeline(User_OptionInit);

                string TweetText = String.Empty;

                foreach (var tweet in User_Tweets)
                {
                    var userDetail = tweet.User;
                    if (tweet != null)
                    {

                        if (userDetail != null)
                        {
                            modelTwitterFeeds.FeedId = tweet.User.Id;
                            modelTwitterFeeds.UserPageId = tweet.User.Id;
                            modelTwitterFeeds.UserPageTitle = tweet.User.Name;
                            modelTwitterFeeds.UserScreenName = tweet.User.ScreenName;
                            modelTwitterFeeds.UserPageDesc = tweet.User.Description;
                            modelTwitterFeeds.UserPageLanguage = tweet.User.Language;
                            modelTwitterFeeds.UserPageFollowers = tweet.User.FollowersCount;
                            //modelTwitterFeeds.UserPageCoverImageURL = tweet.User.ProfileBackgroundImageUrl;
                            modelTwitterFeeds.UserPageLogoImage = tweet.User.ProfileImageUrl;
                            //JObject o = new JObject(new JProperty("Name", "John Smith"),new JProperty("BirthDate", new DateTime(1983, 3, 20)));
                            JObject obj = (JObject)JsonConvert.DeserializeObject(tweet.RawSource);
                            if (obj != null)
                            {
                                if (obj["user"]["profile_banner_url"] != null)
                                {
                                    modelTwitterFeeds.UserPageCoverImageURL = (string)obj["user"]["profile_banner_url"];
                                }
                            }
                        }

                        var st1 = tweet.Text.ToSafeString(); //string
                        TweetText = tweet.Text;
                        modelTwitterFeedsDetail.TwitterPageId = PageId;
                        modelTwitterFeedsDetail.FeedPostId = tweet.Id;
                        modelTwitterFeedsDetail.FeedText = String.Empty;
                        modelTwitterFeedsDetail.FeedImageURL = String.Empty;
                        modelTwitterFeedsDetail.FeedLanguage = String.Empty;
                        modelTwitterFeedsDetail.FeedPostDate = tweet.CreatedDate;


                        modelTwitterFeedsDetail.FeedTextDetail = "";

                        var st2 = tweet.Source; //string
                        var st3 = tweet.TextAsHtml; //string
                        var st4 = tweet.TextDecoded; //string
                        var st5 = tweet.RetweetedStatus; //TwitterStatus
                        var st6 = tweet.RetweetCount; //int
                        var st7 = tweet.RawSource; //string
                        var st8 = tweet.Place; //TwitterPlace
                        var st9 = tweet.Location; //TwitterGeoLocation
                        var st10 = tweet.Language; //string
                        //MODEL DATA
                        modelTwitterFeedsDetail.FeedLanguage = tweet.Language;
                        var st11 = tweet.IsTruncated; //bool
                        var st12 = tweet.IsRetweeted; //bool
                        var st13 = tweet.IsPossiblySensitive; //bool is nullable
                        var st14 = tweet.IsFavorited; //bool
                        var st15 = tweet.InReplyToUserId; //long is nullable 
                        var st16 = tweet.InReplyToStatusId; //long is nullable
                        var st17 = tweet.InReplyToScreenName; //string
                        var st18 = tweet.IdStr; //string
                        var st19 = tweet.Id; //long
                        var st20 = tweet.FavoriteCount; //int
                        var st21 = tweet.ExtendedEntities; //TwitterExtendedEntities
                        var st22 = tweet.Entities; //TwitterEntities
                        var twitterEntities = new TwitterEntities();
                        twitterEntities = tweet.Entities;
                        IList<TwitterUrl> twitterUrl = twitterEntities.Urls; //List<TwitterUrl>
                        foreach (var url in twitterUrl)
                        {
                            var url1 = url.DisplayUrl; //string
                            var url2 = url.EndIndex; //int
                            var url3 = url.EntityType; //TwitterEntityType
                            var entityType = url3;
                            //4 Entity Types are Defined
                            //TwitterEntityType.HashTag; //0
                            //TwitterEntityType.Mention; //1
                            //TwitterEntityType.Url; //2
                            //TwitterEntityType.Media; //3

                            var url4 = url.ExpandedValue; //string
                            var url5 = url.Indices; //IList<int>
                            var url6 = url.StartIndex; //int
                            //MODEL DATA //Tweet Detail Page URL
                            var url7 = url.Value; //string
                            TweetText = TweetText.Trim().Replace(url.Value, String.Empty).Trim();
                        }

                        IList<TwitterMedia> twitterMedia = twitterEntities.Media; //List<TwitterMedia>
                        foreach (var media in twitterMedia)
                        {
                            var media1 = media.DisplayUrl; //string
                            var media2 = media.EndIndex; //int
                            var media3 = media.EntityType; //TwitterEntity
                            var media4 = media.ExpandedUrl; //string
                            var media5 = media.Id; //long
                            var media6 = media.IdAsString; //string
                            var media7 = media.Indices; //IList<int>
                            var media8 = media.MediaType; //TwitterMediaType
                            var twitterMediaType = media8;

                            /*Three Types of MediaType
                            TwitterMediaType.Photo; //0
                            TwitterMediaType.Video; //1
                            TwitterMediaType.AnimatedGif; //2
                            */

                            var media9 = media.MediaUrl; //string
                            var media10 = media.MediaUrlHttps; //string
                            //var FeedImageURLhttp  = media.MediaUrl; for Http Image
                            //var FeedImageURLhttps  =  media.MediaUrlHttps; for Https Image
                            var media11 = media.Sizes; //TwitterMediaSizes
                            var twitterMediaSizes = media11;
                            //media11.Large
                            //media11.Medium
                            //media11.Small
                            //media11.Thumb
                            var media12 = media.StartIndex; //int
                            var media13 = media.Url; //string
                            //MODEL DATA
                            modelTwitterFeedsDetail.FeedImageURL = !String.IsNullOrEmpty(media.Url.Trim()) ? media.Url.Trim() : String.Empty;
                            TweetText = TweetText.Trim().Replace(media.Url, String.Empty).Trim();
                        }
                        IList<TwitterHashTag> twitterHashTag = twitterEntities.HashTags; //List<TwitterHashTag>
                        foreach (var hashTag in twitterHashTag)
                        {
                            var ht1 = hashTag.EndIndex;
                            var ht2 = hashTag.EntityType;
                            var ht3 = hashTag.Indices;
                            var ht4 = hashTag.StartIndex;
                            var ht5 = hashTag.Text;
                            // hashTagText for refine Tweet Text
                            TweetText = TweetText.Trim().Replace("#" + hashTag.Text, string.Empty).Trim();
                        }
                        TweetText = TweetText.Replace("\n", String.Empty).Trim();
                        var st23 = tweet.CreatedDate; //DateTime
                        var st24 = tweet.Author; //ITweeter

                        var ProfileImageURL = st24.ProfileImageUrl;
                        var ss = st24.RawSource;
                        var screenName = st24.ScreenName;
                        modelTwitterFeedsDetail.FeedText = TweetText;
                    }
                    lstModelTwitterFeedsDetail.Add(modelTwitterFeedsDetail);
                }
                modelTwitterFeeds.TwitterFeedDetails = lstModelTwitterFeedsDetail;
                StartSafeScrappedData(modelTwitterFeeds, modelFeed);
            }
            catch (Exception ex)
            {

                throw ex;
            } 
        }

    }
}
