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

namespace BookStore.Models
{
    public class SearchResult
    {
        private static HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

        public string Title { get; set; }
        public string htmlTitle { get; set; }
        public string link { get; set; }
        public static List<string> GetInnerText(List<string> links)
        {
            List<string> result = new List<string>();
            HtmlNode c;
            foreach (string link in links)
            {
                if (link.Contains("livelib"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//p[@itemprop='about']");
                    if (c != null) result.Add(HttpUtility.HtmlDecode(c.InnerText));                   
                }
                else if (link.Contains("aldebaran"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@class='annotation clearfix']");
                    if (c != null) result.Add(HttpUtility.HtmlDecode(c.FirstChild.InnerText));   
                }
                else if (link.Contains("loveread"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//p[@class='span_str']");
                    if (c != null) result.Add(HttpUtility.HtmlDecode(c.InnerText));
                }
                else if (link.Contains("e-reading.club"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']");
                    if (c != null) result.Add(HttpUtility.HtmlDecode(c.InnerText));
                }
                else if (link.Contains("litres"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@itemprop='description']");
                    if (c != null)
                    {
                        var nodes = c.ChildNodes;
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var node in nodes) { sb.Append(node.InnerText); }
                        result.Add(HttpUtility.HtmlDecode(sb.ToString()));
                    }
                }
                else if (link.Contains("mybook"))
                {
                    doc.LoadHtml(getRequest(link));
                    c = doc.DocumentNode.SelectSingleNode("//div[@class='definition-section']");
                    if (c != null) result.Add(HttpUtility.HtmlDecode(c.FirstChild.InnerText));
                }
            }

            return result;
        }
        public static string getRequest(string url)
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
        public static IList<SearchResult> getSearch(string searchText, string cfg = "")
        {
            string key = "AIzaSyBzcXSZrtK15FFCX8v_Ob-Hcxnc-cVHc-Y";
            string cx = "015577388163479462430:16-o3xadmg4";
            //string key = "AIzaSyAmaV0ew89918tcxHYXbM0VsVM-G6wRKwY";
            //string cx = "003508446447238917805:tgox65vdhtw";
            string google = "https://www.googleapis.com/customsearch/v1?key=" + key + "&cx=" + cx + "&q=" + searchText + "&alt=json" + cfg;
            JObject googleSearch = JObject.Parse(getRequest(google));
            List<JToken> results = googleSearch["items"].Children().ToList();
            IList<SearchResult> searchResults = new List<SearchResult>();
            foreach (JToken result in results)
            {
                SearchResult searchResult = JsonConvert.DeserializeObject<SearchResult>(result.ToString());
                searchResults.Add(searchResult);
            }
            return searchResults;
        }

    }
}