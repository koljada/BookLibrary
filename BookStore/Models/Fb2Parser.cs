using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace BookStore.Models
{
    public class Fb2Parser
    {
        public StringBuilder Text { get; set; }
        public List<string> Chapters { get; set; }
        private XDocument _doc;
        public int PageCharacters = 15000;
        private XElement _main;

        public Fb2Parser(string path,int section=0)
        {
            Chapters = GetChapters(path, section);
            Text = GetText();
        }

        private List<string> GetChapters(string path, int section = 0)
        {
            _doc = XDocument.Load((path));
            List<string> chapters = new List<string>();
            if (_doc.Root != null)
            {
                var body = _doc.Root.Elements().FirstOrDefault(x => x.Name.LocalName == "body");
                if (body != null)
                {
                    var sections = body.Elements().Where(x => x.Name.LocalName == "section");
                    var xElements = sections as IList<XElement> ?? sections.ToList();
                    foreach (var chapter in xElements)
                    {
                        var name = chapter.Elements().FirstOrDefault(x => x.Name.LocalName == "title");
                        if (name != null)
                        {
                            chapters.Add(name.Element(body.GetDefaultNamespace() + "p").Value);
                        }
                    }
                    _main = xElements.Count() > 2 ? xElements.ElementAt(section) : body;
                }
            }
            return chapters;
        }

        private StringBuilder GetText()
        {
            StringBuilder text = new StringBuilder();
            using (var xReader = _main.CreateReader())
            {
                xReader.MoveToContent();
                text.Append(xReader.ReadInnerXml());
            }
            if (text.Length >= PageCharacters)
            {
                text.Append(' ', PageCharacters - text.Length % PageCharacters);
            }
            else
            {
                PageCharacters = text.Length;
            }
            return text;
        }
    }
}