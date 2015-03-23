using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace BookStore.Models
{
    public class YandexSearch
    {
        public static List<YaSearchResult> Search(string searchQuery)
        {
            string url = @"http://xmlsearch.yandex.ru/xmlsearch?
             user=koljadar&
             key=03.310576775:d008fbd56ba762a577119ddb1524a8e1";

            // Текст запроса в формате XML
            string command =
              @"<?xml version=""1.0"" encoding=""UTF-8""?>   
          <request>   
           <query>" + searchQuery + @"</query>
           <groupings>
             <groupby attr=""d"" 
                    mode=""deep"" 
                    groups-on-page=""10"" 
                    docs-in-group=""1"" />   
           </groupings>   
          </request>";
            HttpWebResponse resp = GetResponse(url, command);
            XDocument Xmlresponse = GetDocument(resp);
            //Лист структур YaSearchResult, который метод в итоге возвращает.
            List<YaSearchResult> ret = new List<YaSearchResult>();

            //из полученного XML'я выдираем все элементы с именем "group" - это результаты поиска
            var groupQuery = from gr in Xmlresponse.Elements().
                          Elements("response").
                          Elements("results").
                          Elements("grouping").
                          Elements("group")
                             select gr;

            //каждый элемент group преобразовывается в объект SearchResult
            for (int i = 0; i < groupQuery.Count(); i++)
            {
                string urlQuery = GetValue(groupQuery.ElementAt(i), "url");
                string titleQuery = GetValue(groupQuery.ElementAt(i), "title");
                string descriptionQuery = GetValue(groupQuery.ElementAt(i), "headline");
                string indexedTimeQuery = GetValue(groupQuery.ElementAt(i), "modtime");
                string cacheUrlQuery = GetValue(groupQuery.ElementAt(i),
                                "saved-copy-url");
                ret.Add(new YaSearchResult(urlQuery, cacheUrlQuery, titleQuery, descriptionQuery, indexedTimeQuery));
            }

            return ret;
        }

        public static HttpWebResponse GetResponse(string url, string command)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(command);
            // Объект, с помощью которого будем отсылать запрос и получать ответ.
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            // Пишем наш XML-запрос в поток 
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            // Получаем ответ
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response;
        }

        public static XDocument GetDocument(HttpWebResponse response)
        {
            XmlReader xmlReader = XmlReader.Create(response.GetResponseStream());
            XDocument xmlResponse = XDocument.Load(xmlReader);
            return xmlResponse;
        }
        public static string GetValue(XElement group, string name)
        {
            try
            {
                return group.Element("doc").Element(name).Value;
            }
            //это если в результате нету элемента с каким то именем,
            //то будет вместо значащей строчки возвращаться пустая.
            catch
            {
                return string.Empty;
            }
        }
    }


    public struct YaSearchResult
    {
        //url
        public string DisplayUrl,
            //saved-copy-url
        CacheUrl,
            //title
        Title,
            //headline
        Description,
            //modtime
        IndexedTime;

        public YaSearchResult(string url,
                   string cacheUrl,
                   string title,
                   string description,
                   string indexedTime)
        {
            this.DisplayUrl = url;
            this.CacheUrl = cacheUrl;
            this.Title = title;
            this.Description = description;
            this.IndexedTime = indexedTime;
        }
    }
}