using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace WebBrowser
{
    public class DataTransfer
    {
        //the file name
        string filename = "settings.xml";

        public async void SaveSearchTerm(string SearchTerm, string title, string url)
        {
            var doc = await DocumentLoad().AsAsyncOperation(); //loads the Xml file

            var history = doc.GetElementsByTagName("history");

            XmlElement elSearchTerm = doc.CreateElement("searchterm");
            XmlElement elSiteName = doc.CreateElement("sitename");
            XmlElement elUrl = doc.CreateElement("url");

            var historyItem = history[0].AppendChild(doc.CreateElement("historyItem"));

            historyItem.AppendChild(elSearchTerm);
            historyItem.AppendChild(elSiteName);
            historyItem.AppendChild(elUrl);

            elSearchTerm.InnerText = SearchTerm;
            elSiteName.InnerText = title;
            elUrl.InnerText = url;

            SaveDocument(doc);
        }

        private async Task<XmlDocument> DocumentLoad()
        {
            XmlDocument result = null;

            await Task.Run(async () =>
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                XmlDocument doc = await XmlDocument.LoadFromFileAsync(file);
                result = doc;
            });

            return result;
        }

        private async void SaveDocument(XmlDocument doc)
        {
            var file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            await doc.SaveToFileAsync(file);
        }
    }
}