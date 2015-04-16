using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net;
using System.Web;
using NLog;

namespace BookStore.Models
{
    public enum TypeSearch
    {
        BookCover,
        AuthorPic
    }
    public class SearchResult
    {
        private static HtmlDocument doc = new HtmlDocument();
        readonly static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Title { get; set; }
        public string htmlTitle { get; set; }
        public string link { get; set; }
        public static List<string> GetInnerText(List<string> links)
        {
            List<string> result = new List<string>();
            HtmlNode c;
            foreach (string link in links)
            {
                if (link.Contains("www.livelib.ru/author/") && !link.Contains("top") && !link.Contains("quotes") 
                    && !link.Contains("latest") && !link.Contains("reviews") && !link.Contains("selections"))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    doc.LoadHtml(GetRequest(link));

                    foreach (HtmlNode vc in doc.DocumentNode.SelectSingleNode("//div[@id='author-section-1']").ParentNode.ChildNodes)
                        {
                            if (vc.Name == "p" && !vc.InnerText.Contains("Читать дальше"))
                            {
                                sb.Append("<p>");
                                sb.Append(HttpUtility.HtmlDecode(vc.InnerText));
                                sb.Append("</p>");
                                logger.Info("livelib-" + vc.InnerText);
                            }
                        }
                        foreach (var cv in doc.DocumentNode.SelectSingleNode("//div[@id='author-section-1']").ChildNodes)
                        {
                            if (cv.Name == "p")
                            {
                                sb.Append("<p>");
                                sb.Append(HttpUtility.HtmlDecode(cv.InnerText));
                                sb.Append("</p>");
                                logger.Info("livelib-" + cv.InnerText);
                            }
                        }
                        result.Add(sb.ToString());
                }
                else if (link.Contains("www.livelib.ru/book/"))
                {
                    doc.LoadHtml(GetRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//p[@itemprop='about']");
                    if (c != null)
                    {
                        result.Add(HttpUtility.HtmlDecode(c.InnerText));
                        logger.Info("livelib-" + c.InnerText);
                    }
                }
                else if (link.Contains("aldebaran"))
                {
                    doc.LoadHtml(GetRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@class='annotation clearfix']");
                    if (c != null)
                    {
                        result.Add(HttpUtility.HtmlDecode(c.FirstChild.InnerText));
                        logger.Info("aldebaran-" + c.InnerText);
                    }
                }
                //else if (link.Contains("loveread"))
                //{
                //    doc.LoadHtml(GetRequest(link));
                //    c = doc.DocumentNode.SelectSingleNode("//p[@class='span_str']");
                //    if (c != null)
                //    {
                //        result.Add(HttpUtility.HtmlDecode(c.InnerText));
                //        logger.Info("loveread-" + c.InnerText);
                //    }
                //}
                //else if (link.Contains("e-reading.club"))
                //{
                //    doc.LoadHtml(GetRequest(link));
                //    c = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']");
                //    if (c != null)
                //    {
                //        result.Add(HttpUtility.HtmlDecode(c.InnerText));
                //        logger.Info("e-reading-" + c.InnerText);
                //    }
                //}
                else if (link.Contains("litres"))
                {
                    doc.LoadHtml(GetRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@itemprop='description']");
                    if (c != null)
                    {
                        var nodes = c.ChildNodes;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var node in nodes) { sb.Append(node.InnerText); }
                        result.Add(HttpUtility.HtmlDecode(sb.ToString()));
                        logger.Info("litres-"+HttpUtility.HtmlDecode(sb.ToString()));
                    }
                }
                else if (link.Contains("mybook"))
                {
                    doc.LoadHtml(GetRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@class='definition-section']");
                    if (c != null)
                    {
                        result.Add(HttpUtility.HtmlDecode(c.FirstChild.InnerText));
                        logger.Info("mybook-"+c.InnerText);
                    }
                }
                else if (link.Contains("readrate"))
                {
                    doc.LoadHtml(GetRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//p[@itemprop='description']");
                    if (c != null)
                    {
                        result.Add(HttpUtility.HtmlDecode(c.FirstChild.InnerText));
                        logger.Info("mybook-" + c.InnerText);
                    }
                }
            }

            return result;
        }
        public static string GetRequest(string url)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                logger.Error("You exceeded limit", ex);
                return null;
                //throw;
            }

        }
        public static IList<SearchResult> GetSearch(string searchText, string cfg = "")
        {
            string key = "AIzaSyBzcXSZrtK15FFCX8v_Ob-Hcxnc-cVHc-Y";
            string cx = "015577388163479462430:16-o3xadmg4";
            //string key = "AIzaSyAmaV0ew89918tcxHYXbM0VsVM-G6wRKwY";
            //string cx = "003508446447238917805:tgox65vdhtw";
            IList<SearchResult> searchResults = new List<SearchResult>();
            string google = "https://www.googleapis.com/customsearch/v1?key=" + key + "&cx=" + cx + "&q=" + searchText + "&alt=json" + cfg;
            try
            {
                JObject googleSearch = JObject.Parse(GetRequest(google));
                List<JToken> results = googleSearch["items"].Children().ToList();
                foreach (JToken result in results)
                {
                    SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
                    searchResults.Add(searchResult);
                }
                return searchResults;
            }
            catch (Exception)
            {
                return searchResults;
                
                //throw;
            }
        }
    }
}