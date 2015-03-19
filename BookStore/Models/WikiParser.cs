using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace BookStore.Models
{
    public class WikiParser

    {
                
        public string getRequest(string url){
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.AllowAutoRedirect = false;//Запрещаем автоматический редирект
                    httpWebRequest.Method = "GET"; //Можно не указывать, по умолчанию используется GET.
                    httpWebRequest.Referer = "http://google.com"; // Реферер. Тут можно указать любой URL
                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (var stream = httpWebResponse.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream, Encoding.GetEncoding(httpWebResponse.CharacterSet)))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
        }
            
            
    }
}