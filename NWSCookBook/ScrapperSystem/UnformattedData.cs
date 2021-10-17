using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HotSLogsScrapper
{
    public class UnformattedData
    {
        public IEnumerable<HtmlNode> Nodes;

        public static UnformattedData Create(string data, string xpath)
        {
            HtmlDocument doc = new HtmlDocument();
            UnformattedData uData = new UnformattedData();

            doc.LoadHtml(data);
            uData.Nodes = doc.DocumentNode.SelectNodes(xpath);

            return (uData);
        }

        public UnformattedData Then(string xpath)
        {
            IEnumerable<HtmlNode> nodes = null;
            
            try
            {
                nodes = Nodes.Select(i => i.SelectNodes(xpath)).SelectMany(i => i).ToList();
            }
            catch (Exception) { }

            return (new UnformattedData
            {
                Nodes = nodes
            });
        }

        public IEnumerable<T> Then<T>(Func<UnformattedData, T> fct)
        {
            List<T> list = new List<T>();

            foreach (HtmlNode node in Nodes)
            {
                list.Add(fct(new UnformattedData
                {
                    Nodes = new[]
                    {
                        node
                    }
                }));
            }

            return (list);
        }

        public string ExtractAttribute(string attribute, string def = "")
        {
            return (Nodes
                .First()
                .GetAttributeValue(attribute, def)
                .Replace("&#39;", "'"));
        }

        public string ExtractContent(char takeWhile)
        {
            string str = new string(Nodes.First().InnerHtml.TakeWhile(c => c != takeWhile).ToArray());

            return (str);
        }
    }
}
